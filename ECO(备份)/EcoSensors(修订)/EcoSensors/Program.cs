using CommonAPI;
using CommonAPI.CultureTransfer;
using CommonAPI.Global;
using CommonAPI.InterProcess;
using DBAccessAPI;
using DBAccessAPI.user;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.Thread;
using EcoSensors.Login;
using EcoSensors.MainForm;
using EcoSensors.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors
{
	internal static class Program
	{
		public const int IdleFlg_Normal = 1;
		public const int IdleFlg_PowerReport = 2;
		public const int IdleFlg_ThermalReport = 4;
		public const int IdleFlg_BillingReport = 8;
		public static int program_uid = 0;
		public static string program_serverid = "";
		private static System.Timers.Timer m_IdleTimer;
		public static int m_IdleCounter = 0;
		public static int m_IdleTimeSet = 0;
		public static int IDLETIMER_RUNING = 0;
		public static int IDLETIMER_PAUSE = 1;
		private static int m_IdleTimerStopFlg = Program.IDLETIMER_RUNING;
		public static int m_IdleTimeFact = 6;
		private static int m_Idle_Flag = 0;
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		private static extern bool SetForegroundWindow(System.IntPtr hWnd);
		[System.STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			System.Security.Principal.WindowsPrincipal windowsPrincipal = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent());
			if (!windowsPrincipal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
			{
				TopMostMessageBox.Show(EcoLanguage.getMsg(LangRes.NeedPrivilege, new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				System.Environment.Exit(0);
				return;
			}
			EcoGlobalVar.nullcontextMenuStrip = new ContextMenuStrip();
			EcoGlobalVar.ECOAppRunMode = 1;
			AppInterProcess.OpenInterProcessShared();
			int processID = AppInterProcess.getProcessID(Program.program_uid, Program.program_serverid, EcoGlobalVar.ECOAppRunMode);
			if (processID != 0)
			{
				AppInterProcess.CloseShared();
				string processOwner = Program.GetProcessOwner(processID);
				string processOwner2 = Program.GetProcessOwner(Process.GetCurrentProcess().Id);
				if (processOwner.Equals(processOwner2))
				{
					Program.setTopMost(processID);
				}
				else
				{
					TopMostMessageBox.Show(EcoLanguage.getMsg(LangRes.APPRunbyuser, new string[]
					{
						processOwner
					}), "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				System.Environment.Exit(0);
				return;
			}
			AppInterProcess.setMyProcessID(Program.program_uid, Program.program_serverid, EcoGlobalVar.ECOAppRunMode);
			Program.setTopMost(Process.GetCurrentProcess().Id);
			int mySQLUseMajorVersionOnly = DevAccessCfg.GetInstance().getMySQLUseMajorVersionOnly();
			DBMaintain.SetMySQLVersionRole(mySQLUseMajorVersionOnly);
			while (Program.LocalConsole_cfg() == -2)
			{
			}
			DBUrl.RUNMODE = 1;
			ClientAPI.SetBroadcastCallback( new  CommonAPI.CommonAPI.DelegateOnBroadcast(EcoGlobalVar.receiveDashBoardFlgProc) );
            ClientAPI.SetClosedCallback(new CommonAPI.CommonAPI.DelegateOnClosed(EcoGlobalVar.ServerClosedProc));
            Login.Login login = new Login.Login();

            login.Icon = null;
			login.ShowDialog();
			if (login.UserName == null)
			{
				Program.ExitApp();
				return;
			}
			EcoGlobalVar.gl_StartProcessfThread(true);
			Application.CurrentCulture = System.Globalization.CultureInfo.CurrentUICulture;
			if (EcoGlobalVar.ECOAppRunMode == 1)
			{
				DBCacheStatus.DBSyncEventInit(false);
				DBCacheEventProcess.StartRefreshThread(false);
				DBCache.DBCacheInit(false);
			}
			if (ClientAPI.WaitDatasetReady(40000u) < 0)
			{
				TopMostMessageBox.Show(EcoLanguage.getMsg(LangRes.DB_waitready, new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Program.ExitApp();
				return;
			}
			EcoGlobalVar.DCLayoutType = ClientAPI.getRackLayout();
			EcoGlobalVar.gl_maxZoneNum = CultureTransfer.ToInt32(ClientAPI.getKeyValue("MaxZoneNum"));
			EcoGlobalVar.gl_maxRackNum = CultureTransfer.ToInt32(ClientAPI.getKeyValue("MaxRackNum"));
			EcoGlobalVar.gl_maxDevNum = CultureTransfer.ToInt32(ClientAPI.getKeyValue("MaxDevNum"));
			EcoGlobalVar.gl_supportISG = (CultureTransfer.ToInt32(ClientAPI.getKeyValue("SupportISG")) > 0);
			EcoGlobalVar.gl_supportBP = (CultureTransfer.ToInt32(ClientAPI.getKeyValue("SupportBP")) > 0);
			EcoGlobalVar.TempUnit = CultureTransfer.ToInt32(ClientAPI.getKeyValue("TempUnit"));
			EcoGlobalVar.CurCurrency = ClientAPI.getKeyValue("CurCurrency");
			EcoGlobalVar.co2kg = CultureTransfer.ToSingle(ClientAPI.getKeyValue("co2kg"));
			EcoGlobalVar.flgEnablePower = AppData.getDB_flgEnablePower();
			EcoGlobalVar.RackFullNameFlag = CultureTransfer.ToInt32(ClientAPI.getKeyValue("RackFullNameFlag"));
			EcoGlobalVar.gl_PeakPowerMethod = DevAccessCfg.GetInstance().getPowerPeakMethod();
			string valuePair = ValuePairs.getValuePair("UserID");
			long l_id = System.Convert.ToInt64(valuePair);
			string valuePair2 = ValuePairs.getValuePair("UserName");
			valuePair = ValuePairs.getValuePair("UserType");
			int i_type = System.Convert.ToInt32(valuePair);
			valuePair = ValuePairs.getValuePair("UserRight");
			int i_right = System.Convert.ToInt32(valuePair);
			string valuePair3 = ValuePairs.getValuePair("UserPortNM");
			string valuePair4 = ValuePairs.getValuePair("UserDevice");
			string valuePair5 = ValuePairs.getValuePair("UserGroup");
			valuePair = ValuePairs.getValuePair("UserStatus");
			ValuePairs.getValuePair("trial");
			ValuePairs.getValuePair("remaining_days");
			int i_status = System.Convert.ToInt32(valuePair);
			UserInfo userInfo = new UserInfo(l_id, valuePair2, "", i_status, i_type, i_right, valuePair3, valuePair4, valuePair5);
			EcoGlobalVar.gl_LoginUser = userInfo;
			EcoGlobalVar.gl_LoginUserUACDev2Port = (System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>>)ClientAPI.RemoteCall(8, 1, "", 10000);
			if (EcoGlobalVar.gl_LoginUserUACDev2Port == null)
			{
				TopMostMessageBox.Show(EcoLanguage.getMsg(LangRes.DB_waitready, new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Program.ExitApp();
				return;
			}
			string para = "0230000\n" + userInfo.UserName;
			ClientAPI.RemoteCall(100, 1, para, 10000);
            MainForm.MainForm mainForm = new MainForm.MainForm(userInfo);
			EcoGlobalVar.gl_mainForm = mainForm;
			if (!EcoGlobalVar.gl_supportISG)
			{
				EcoGlobalVar.gl_monitorCtrl.FreshFlg_ISGPower = 0;
				EcoGlobalVar.gl_DashBoardCtrl.FreshFlg_ISGPower = 0;
			}
			EcoGlobalVar.gl_StopProcessfThread();
			Program.IdleTimer_init();
			Application.ApplicationExit += new System.EventHandler(Program.Application_ApplicationExit);
			System.AppDomain.CurrentDomain.ProcessExit += new System.EventHandler(Program.CurrentDomain_ProcessExit);
			Application.Run(mainForm);
		}
		public static object StartService(object param)
		{
			System.Threading.Thread.Sleep(500);
			System.TimeSpan timeout = System.TimeSpan.FromMilliseconds(30000.0);
			try
			{
				ServiceController serviceController = new ServiceController(EcoGlobalVar.gl_ServiceName);
				if (serviceController.Status.Equals(ServiceControllerStatus.Running))
				{
					object result = 0;
					return result;
				}
				InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStatus", "waiting");
				serviceController.Start();
				serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
			}
			catch (System.Exception)
			{
				object result = -1;
				return result;
			}
			return Program.waitService_initfinish();
		}
		public static int waitService_initfinish()
		{
			new ServiceController(EcoGlobalVar.gl_ServiceName);
			int num = 0;
			string interProcessKeyValue = InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.getInterProcessKeyValue("ServiceStatus");
			while (num < 60 && (interProcessKeyValue == null || (!interProcessKeyValue.Equals("ready") && !interProcessKeyValue.Equals("failed"))))
			{
				System.Threading.Thread.Sleep(1000);
				interProcessKeyValue = InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.getInterProcessKeyValue("ServiceStatus");
				num++;
			}
			if (num >= 60)
			{
				return -2;
			}
			if (interProcessKeyValue.Equals("failed"))
			{
				return -3;
			}
			return 0;
		}
		public static bool isService_initfinish()
		{
			string interProcessKeyValue = InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.getInterProcessKeyValue("ServiceStatus");
			return interProcessKeyValue != null && (interProcessKeyValue.Equals("ready") || interProcessKeyValue.Equals("failed"));
		}
		public static void StopService(string serviceName, int timeoutMilliseconds)
		{
			InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStDBMaintain", "DBMaintain");
			ServiceController serviceController = new ServiceController(serviceName);
			try
			{
				System.TimeSpan timeout = System.TimeSpan.FromMilliseconds((double)timeoutMilliseconds);
				ClientAPI.StopBroadcastChannel();
				serviceController.Stop();
				serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
			}
			catch
			{
			}
		}
		private static void setTopMost(int processid)
		{
			Process processById = Process.GetProcessById(processid);
			Program.ShowWindowAsync(processById.MainWindowHandle, 1);
			Program.SetForegroundWindow(processById.MainWindowHandle);
		}
		private static string GetProcessOwner(int processId)
		{
			string queryString = "Select * From Win32_Process Where ProcessID = " + processId;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
			ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					string[] array = new string[]
					{
						string.Empty,
						string.Empty
					};
					if (System.Convert.ToInt32(managementObject.InvokeMethod("GetOwner", array)) == 0)
					{
						return array[1] + "\\" + array[0];
					}
				}
			}
			return "NO OWNER";
		}
		private static bool RunElevated(string fileName, string args)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.Verb = "runas";
			processStartInfo.FileName = fileName;
			processStartInfo.Arguments = args;
			try
			{
				Process.Start(processStartInfo);
				return true;
			}
			catch (System.Exception)
			{
			}
			return false;
		}
		public static void ExitApp()
		{
			AppInterProcess.removeMyProcessID(Program.program_uid, Program.program_serverid, EcoGlobalVar.ECOAppRunMode);
			AppInterProcess.CloseShared();
			System.Environment.Exit(0);
		}
		private static int LocalConsole_cfg()
		{
			DebugCenter instance = DebugCenter.GetInstance();
			int lastStatusCode = instance.getLastStatusCode();
			if (lastStatusCode == DebugCenter.ST_Unknown)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvFailUnknown, new string[]
				{
					lastStatusCode.ToString("X4")
				}));
				Program.ExitApp();
				return -1;
			}
			if (lastStatusCode == DebugCenter.ST_DiskFull)
			{
				if (EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.srvFailDiskFull, new string[0]), MessageBoxButtons.YesNo) == DialogResult.No)
				{
					Program.ExitApp();
					return -1;
				}
				progressPopup progressPopup = new progressPopup("Service Checker", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_startsrv, new string[0]), Resources.login_background, new progressPopup.ProcessInThread(Program.StartService), null, 0);
				progressPopup.StartPosition = FormStartPosition.CenterScreen;
				progressPopup.ShowDialog();
				object return_V = progressPopup.Return_V;
				int? num = return_V as int?;
				if (num == -1 || num == -2)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvFail, new string[0]));
					Program.ExitApp();
					return -1;
				}
				return -2;
			}
			else
			{
				if ((lastStatusCode & DebugCenter.ST_fatalMask) == 0)
				{
					ServiceController serviceController = new ServiceController(EcoGlobalVar.gl_ServiceName);
					if (serviceController.Status.Equals(ServiceControllerStatus.Stopped))
					{
						progressPopup progressPopup2 = new progressPopup("Service Checker", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_startsrv, new string[0]), Resources.login_background, new progressPopup.ProcessInThread(Program.StartService), null, 0);
						progressPopup2.StartPosition = FormStartPosition.CenterScreen;
						progressPopup2.ShowDialog();
						object return_V2 = progressPopup2.Return_V;
						int? num2 = return_V2 as int?;
						if (num2 == -1 || num2 == -2)
						{
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvFail, new string[0]));
							Program.ExitApp();
							return -1;
						}
						return -2;
					}
					else
					{
						if (!Program.isService_initfinish())
						{
							int num3 = Program.waitService_initfinish();
							if (num3 == -2)
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvFail, new string[0]));
								Program.ExitApp();
								return -1;
							}
							return -2;
						}
						else
						{
							if (lastStatusCode == DebugCenter.ST_Success)
							{
								return 0;
							}
							DBUrl.RUNMODE = 1;
							DBCacheStatus.DBSyncEventInit(false);
							DBCacheEventProcess.StartRefreshThread(false);
							DBCache.DBCacheInit(false);
							if ((lastStatusCode & DebugCenter.ST_MYSQLCONNECT_LOST) != 0)
							{
								string text = "Lost DB connection. Please check your MySQL database service.";
								EcoMessageBox.ShowWarning(text, MessageBoxButtons.OK);
							}
							if (lastStatusCode == DebugCenter.ST_MYSQLCONNECT_LOST)
							{
								return 0;
							}
							registrySettings registrySettings = new registrySettings(lastStatusCode);
							registrySettings.ShowDialog();
							return 0;
						}
					}
				}
				else
				{
					if (lastStatusCode == DebugCenter.ST_DbUpgrade || lastStatusCode == DebugCenter.ST_SysdbNotExist || lastStatusCode == DebugCenter.ST_LogdbNotExist || lastStatusCode == DebugCenter.ST_DatadbNotExist || lastStatusCode == DebugCenter.ST_SysdbNotMatch || lastStatusCode == DebugCenter.ST_ImportDatabase_ERROR)
					{
						restoredb restoredb = new restoredb(1);
						DialogResult dialogResult = restoredb.ShowDialog();
						if (dialogResult != DialogResult.OK)
						{
							commUtil.ShowInfo_DEBUG("restoredbdlg.ShowDialog() result is not DialogResult.OK!!!!!");
						}
					}
					else
					{
						DBUrl.RUNMODE = 1;
						registrySettings registrySettings = new registrySettings(lastStatusCode);
						DialogResult dialogResult2 = registrySettings.ShowDialog();
						if (dialogResult2 != DialogResult.OK)
						{
							Program.ExitApp();
							return -1;
						}
					}
					progressPopup progressPopup3 = new progressPopup("Service Checker", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_startsrv, new string[0]), Resources.login_background, new progressPopup.ProcessInThread(Program.StartService), null, 0);
					progressPopup3.StartPosition = FormStartPosition.CenterScreen;
					progressPopup3.ShowDialog();
					object return_V3 = progressPopup3.Return_V;
					int? num4 = return_V3 as int?;
					if (num4 == -1 || num4 == -2)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvFail, new string[0]));
						Program.ExitApp();
						return -1;
					}
					return -2;
				}
			}
		}
		public static void IdleTimer_Run(int flg)
		{
			Program.m_Idle_Flag &= ~flg;
			if (Program.m_Idle_Flag == 0)
			{
				Program.m_IdleTimerStopFlg = Program.IDLETIMER_RUNING;
			}
		}
		public static void IdleTimer_Pause(int flg)
		{
			Program.m_Idle_Flag |= flg;
			Program.m_IdleTimerStopFlg = Program.IDLETIMER_PAUSE;
		}
		private static void IdleTimer_init()
		{
			if (Program.m_IdleTimer == null)
			{
				Program.m_IdleTimer = new System.Timers.Timer();
				Program.m_IdleTimer.Elapsed += new ElapsedEventHandler(Program.theIdleTimeout);
				Program.m_IdleTimer.Interval = 10000.0;
				Program.m_IdleTimer.AutoReset = true;
				Program.m_IdleCounter = 0;
				Program.m_IdleTimeSet = ValuePairs.getIdleTimeout(true);
				Program.m_IdleTimeSet *= Program.m_IdleTimeFact;
				Program.m_IdleTimer.Start();
			}
		}
		private static void theIdleTimeout(object source, ElapsedEventArgs e)
		{
			if (Program.m_IdleTimerStopFlg == Program.IDLETIMER_PAUSE)
			{
				return;
			}
			if (Program.m_IdleTimeSet <= 0)
			{
				return;
			}
			Program.m_IdleCounter++;
			if (Program.m_IdleCounter >= Program.m_IdleTimeSet)
			{
				Program.m_IdleTimer.Stop();
				string para = "0230003\nAAA";
				ClientAPI.RemoteCall(100, 1, para, 10000);
				ClientAPI.Logout();
				ClientAPI.StopBroadcastChannel();
				EcoGlobalVar.stopalltimer(true);
				ControlAccess.ConfigControl config = delegate(Control control, object obj)
				{
					TopMostMessageBox.Show(EcoLanguage.getMsginThread("ThreadPopMsg_IdleTimeout", new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				};
				ControlAccess controlAccess = new ControlAccess(EcoGlobalVar.gl_mainForm, config);
				controlAccess.Access(EcoGlobalVar.gl_mainForm, null);
				Program.ExitApp();
			}
		}
		public static void Application_ApplicationExit(object sender, System.EventArgs e)
		{
            
			MainForm.MainForm.RawInput_Uninstall();
		}
		public static void CurrentDomain_ProcessExit(object sender, System.EventArgs e)
		{
            MainForm.MainForm.RawInput_Uninstall();
		}
	}
}
