using CommonAPI.CultureTransfer;
using CommonAPI.Global;
using DBAccessAPI.user;
using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.Thread;
using EcoSensors.DevManDevice;
using EcoSensors.DevManPage;
using EcoSensors.DevManPage.OtherDevices;
using EcoSensors.EnegManPage;
using EcoSensors.EnegManPage.Analysis;
using EcoSensors.EnegManPage.DashBoard;
using EcoSensors.EnegManPage.DashBoardUser;
using EcoSensors.EnegManPage.DataGPOP;
using EcoSensors.EnegManPage.PowerOp;
using EcoSensors.MainForm;
using EcoSensors.Monitor;
using EcoSensors.Properties;
using EcoSensors.SysManDB;
using EcoSensors.SysManPage.Billing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
namespace EcoSensors
{
	internal class EcoGlobalVar
	{
		public const int FLGAppAct_no = 0;
		public const int FLGAppAct_DrawRack = 1;
		public const int FLGAppAct_modifydata = 2;
		public const int FLGAppAct_SYSPara = 32;
		public const int FLGAppAct_GpTreeChange = 64;
		public const int FLGAppAct_UserLogout = 128;
		public const int FLG_RefreshDashBoard_no = -1;
		public const int UNIT_C = 0;
		public const int UNIT_F = 1;
		public const int RUNMODE_LOCAL = 1;
		public const int RUNMODE_DOWNLOAD = 2;
		public const int RUNStatus_normal = 1;
		public const int RUNStatus_inDBOP = 2;
		public static readonly string EC2004 = "EC2004";
		public static MainForm.MainForm gl_mainForm = null;
        public static EnegManPage.EnegManPage gl_EnegManPage = null;
        public static DevManPage.DevManPage gl_DevManPage = null;
		public static EPowerOpDev gl_PowerOPCtrl = null;
		public static DataGpOPAll gl_DataGpOPAll = null;
		public static DashBoard gl_DashBoardCtrl = null;
		public static DashBoardUser gl_DashBoardUserCtrl = null;
		public static monitor gl_monitorCtrl = null;
		public static EnegAnalysis gl_EnegAnalysisCtrl = null;
		public static Billing gl_BillingRptCtrl = null;
        public static DevManDevice.DevManDevice gl_DevManCtrl = null;
		public static OtherDevices gl_otherDevCtrl = null;
		public static SysManDBCap gl_SysDBCapCtrl = null;
		public static UserInfo gl_LoginUser = null;
		public static System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>> gl_LoginUserUACDev2Port = new System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>>();
		private static bool isinExit = false;


        //eco Sensors Monitor服务（依赖性服务）

        public static string gl_ServiceName = "eco Sensors Device Monitor";
		public static int gl_maxZoneNum = 128;
		public static int gl_maxRackNum = 1250;
		public static int gl_maxDevNum = 2500;
		public static bool gl_supportISG = false;
		public static bool gl_supportBP = false;
		public static int gl_PeakPowerMethod = 0;
		private static int gl_TmpUnit = 0;
		private static int gl_TmpUnit_Disp = 0;
		private static float gl_co2kg;
		private static bool gl_flgEnablePower = true;
		private static string gl_Currency = "$";
		private static int gl_RackFullNameFlag = 0;
		private static int gl_DCLayoutType = 2;
		public static ContextMenuStrip nullcontextMenuStrip = null;
		public static int ECOAppRunMode = 2;
		public static int ECOAppRunStatus = 1;
		private static object _lockDataSet = new object();
		private static readonly int PROST_STOP = 0;
		private static readonly int PROST_RUNING = 1;
		private static readonly int PROST_WAITSTOP = 2;
		private static int ProcessfThread_ST = EcoGlobalVar.PROST_STOP;
		public static int TempUnit
		{
			get
			{
				return EcoGlobalVar.gl_TmpUnit;
			}
			set
			{
				EcoGlobalVar.gl_TmpUnit = value;
			}
		}
		public static int TempUnit_Disp
		{
			get
			{
				return EcoGlobalVar.gl_TmpUnit_Disp;
			}
			set
			{
				EcoGlobalVar.gl_TmpUnit_Disp = value;
			}
		}
		public static string CurCurrency
		{
			get
			{
				return EcoGlobalVar.gl_Currency;
			}
			set
			{
				EcoGlobalVar.gl_Currency = value;
			}
		}
		public static int RackFullNameFlag
		{
			get
			{
				return EcoGlobalVar.gl_RackFullNameFlag;
			}
			set
			{
				EcoGlobalVar.gl_RackFullNameFlag = value;
			}
		}
		public static float co2kg
		{
			get
			{
				return EcoGlobalVar.gl_co2kg;
			}
			set
			{
				EcoGlobalVar.gl_co2kg = value;
			}
		}
		public static bool flgEnablePower
		{
			get
			{
				return EcoGlobalVar.gl_flgEnablePower;
			}
			set
			{
				EcoGlobalVar.gl_flgEnablePower = value;
			}
		}
		public static int DCLayoutType
		{
			get
			{
				return EcoGlobalVar.gl_DCLayoutType;
			}
			set
			{
				EcoGlobalVar.gl_DCLayoutType = value;
			}
		}
		public static void gl_StartProcessfThread(bool showinTaskBar)
		{
			lock (EcoGlobalVar._lockDataSet)
			{
				if (EcoGlobalVar.ProcessfThread_ST == EcoGlobalVar.PROST_STOP)
				{
					try
					{
						EcoGlobalVar.ProcessfThread_ST = EcoGlobalVar.PROST_RUNING;
                        System.Threading.Thread thread = new Thread(new  ThreadStart(() =>
                            {
                                EcoGlobalVar.showpro(showinTaskBar);
                            }));

                      

						thread.Start();
						System.Threading.Thread.Sleep(500);
					}
					catch (System.Exception ex)
					{
						commUtil.ShowInfo_DEBUG(ex.Message);
					}
				}
			}
		}
		public static void gl_StopProcessfThread()
		{
			lock (EcoGlobalVar._lockDataSet)
			{
				try
				{
					EcoGlobalVar.ProcessfThread_ST = EcoGlobalVar.PROST_WAITSTOP;
				}
				catch (System.Threading.ThreadAbortException ex)
				{
					commUtil.ShowInfo_DEBUG(ex.Message);
				}
				catch (System.Exception ex2)
				{
					commUtil.ShowInfo_DEBUG(ex2.Message);
				}
			}
		}
		public static bool gl_isProcessThreadRuning()
		{
			bool result;
			lock (EcoGlobalVar._lockDataSet)
			{
				if (EcoGlobalVar.ProcessfThread_ST == EcoGlobalVar.PROST_STOP)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
		private static void showpro(bool showinTaskBar)
		{
			switch (EcoLanguage.getLang())
			{
			case 0:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				break;
			case 1:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("de");
				break;
			case 2:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es");
				break;
			case 3:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr");
				break;
			case 4:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("it");
				break;
			case 5:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ja");
				break;
			case 6:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ko");
				break;
			case 7:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("pt");
				break;
			case 8:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru");
				break;
			case 9:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHS");
				break;
			case 10:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHT");
				break;
			default:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				break;
			}
			progressPopup dlg = new progressPopup("eco Sensors", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_loading, new string[0]), Resources.login_background, new progressPopup.ProcessInThread(EcoGlobalVar.waitstopshowPro), null, 0);
			dlg.StartPosition = FormStartPosition.CenterScreen;
			if (showinTaskBar)
			{
				Form form = new Form();
				form.Size = new Size(1, 1);
				form.StartPosition = FormStartPosition.Manual;
				Rectangle virtualScreen = SystemInformation.VirtualScreen;
				form.Location = new Point(virtualScreen.Bottom + 10, virtualScreen.Right + 10);
                form.Icon = Resources.altusen_32x32;

                
				form.Show();
				form.Focus();
				form.BringToFront();
				form.TopMost = true;
				dlg.Owner = form;
			}
			ControlAccess.ConfigControl config = delegate(Control control, object obj)
			{
				dlg.ShowDialog();
			};
			ControlAccess controlAccess = new ControlAccess(dlg, config);
			controlAccess.Access(dlg, null);
		}
		private static object waitstopshowPro(object param)
		{
			while (EcoGlobalVar.ProcessfThread_ST == EcoGlobalVar.PROST_RUNING)
			{
				System.Threading.Thread.Sleep(500);
			}
			EcoGlobalVar.ProcessfThread_ST = EcoGlobalVar.PROST_STOP;
			return 0;
		}
		public static void setDashBoardFlg(ulong dataset2Srv, string para4Srv, int AppAction)
		{
			ClientAPI.NotifyDCReloadDS(dataset2Srv, string.Concat(AppAction), para4Srv);
		}
		public static void receiveDashBoardFlgProc(int infoType, object info, object carried)
		{
			string text = (string)carried;
			int num = 0;
			try
			{
				if ((infoType & 8) != 0 && EcoGlobalVar.gl_LoginUser != null && (EcoGlobalVar.gl_LoginUser.UserType == 1 || EcoGlobalVar.gl_LoginUser.UserType == 2))
				{
					if (EcoGlobalVar.gl_LoginUser.UserType == 1)
					{
						EcoGlobalVar.gl_LoginUserUACDev2Port = (System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>>)ClientAPI.RemoteCall(8, 1, "", 10000);
						EcoGlobalVar.gl_LoginUser.UserDevice = ValuePairs.getValuePair("UserDevice");
						EcoGlobalVar.gl_LoginUser.UserGroup = ValuePairs.getValuePair("UserGroup");
					}
					if (EcoGlobalVar.gl_EnegManPage != null)
					{
						if (EcoGlobalVar.gl_LoginUser.UserType != 0 && (EcoGlobalVar.gl_LoginUser.UserDevice == null || EcoGlobalVar.gl_LoginUser.UserDevice.Length == 0))
						{
							EcoGlobalVar.gl_EnegManPage.showPowerControlButton(false);
						}
						else
						{
							EcoGlobalVar.gl_EnegManPage.showPowerControlButton(true);
						}
						if (EcoGlobalVar.gl_LoginUser.UserType != 0 && (EcoGlobalVar.gl_LoginUser.UserGroup == null || EcoGlobalVar.gl_LoginUser.UserGroup.Length == 0))
						{
							EcoGlobalVar.gl_EnegManPage.showGpControlButton(false);
						}
						else
						{
							EcoGlobalVar.gl_EnegManPage.showGpControlButton(true);
						}
					}
					if (EcoGlobalVar.gl_DashBoardUserCtrl != null)
					{
						EcoGlobalVar.gl_DashBoardUserCtrl.FreshFlg_DashBoard = 1;
						EcoGlobalVar.gl_DashBoardUserCtrl.resetTimer();
					}
					if (EcoGlobalVar.gl_DataGpOPAll != null && EcoGlobalVar.gl_DataGpOPAll.GroupTreeOpFlg != 64)
					{
						EcoGlobalVar.gl_DataGpOPAll.GroupTreeOpFlg = 64;
					}
				}
				if ((infoType & 8192) != 0)
				{
					try
					{
						string text2 = (string)info;
						string text3 = (string)carried;
						if (text2.Equals("RestartListener", System.StringComparison.InvariantCultureIgnoreCase))
						{
							string[] array = text3.Split(new char[]
							{
								','
							});
							if (array.Length >= 2)
							{
								array[0].Trim();
								array[1].Trim();
								ClientAPI.StopBroadcastChannel();
								EcoGlobalVar.stopalltimer(true);
								if (!EcoGlobalVar.isinExit)
								{
									EcoGlobalVar.isinExit = true;
									ControlAccess.ConfigControl config = delegate(Control control, object obj)
									{
										TopMostMessageBox.Show(EcoLanguage.getMsginThread("ThreadPopMsgneedRelogin", new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
									};
									ControlAccess controlAccess = new ControlAccess(EcoGlobalVar.gl_mainForm, config);
									controlAccess.Access(EcoGlobalVar.gl_mainForm, null);
									Program.ExitApp();
								}
							}
						}
					}
					catch (System.Exception)
					{
					}
				}
				if (text.Length != 0)
				{
					num = System.Convert.ToInt32(text);
				}
			}
			catch (System.Exception)
			{
			}
			if ((infoType & 4) != 0)
			{
				EcoGlobalVar.DCLayoutType = ClientAPI.getRackLayout();
			}
			if ((num & 32) != 0 || (infoType & 32) != 0)
			{
				System.Collections.Generic.Dictionary<string, string> sysValuePairs = ClientAPI.getSysValuePairs();
				if (sysValuePairs.ContainsKey("TempUnit"))
				{
					EcoGlobalVar.TempUnit = CultureTransfer.ToInt32(sysValuePairs["TempUnit"]);
				}
				if (sysValuePairs.ContainsKey("CurCurrency"))
				{
					EcoGlobalVar.CurCurrency = sysValuePairs["CurCurrency"];
				}
				if (sysValuePairs.ContainsKey("co2kg"))
				{
					EcoGlobalVar.co2kg = CultureTransfer.ToSingle(sysValuePairs["co2kg"]);
				}
				if (sysValuePairs.ContainsKey("ENABLE_POWER_OP"))
				{
					EcoGlobalVar.flgEnablePower = AppData.getDB_flgEnablePower();
				}
				if (sysValuePairs.ContainsKey("RackFullNameFlag"))
				{
					EcoGlobalVar.RackFullNameFlag = CultureTransfer.ToInt32(sysValuePairs["RackFullNameFlag"]);
				}
				EcoGlobalVar.gl_maxZoneNum = CultureTransfer.ToInt32(ClientAPI.getKeyValue("MaxZoneNum"));
				EcoGlobalVar.gl_maxRackNum = CultureTransfer.ToInt32(ClientAPI.getKeyValue("MaxRackNum"));
				EcoGlobalVar.gl_maxDevNum = CultureTransfer.ToInt32(ClientAPI.getKeyValue("MaxDevNum"));
				EcoGlobalVar.gl_supportISG = (CultureTransfer.ToInt32(ClientAPI.getKeyValue("SupportISG")) > 0);
				EcoGlobalVar.gl_supportBP = (CultureTransfer.ToInt32(ClientAPI.getKeyValue("SupportBP")) > 0);
			}
			if ((num & 64) != 0 && EcoGlobalVar.gl_LoginUser != null && EcoGlobalVar.gl_LoginUser.UserType != 1 && EcoGlobalVar.gl_DataGpOPAll != null && EcoGlobalVar.gl_DataGpOPAll.GroupTreeOpFlg != 64)
			{
				EcoGlobalVar.gl_DataGpOPAll.GroupTreeOpFlg = 64;
			}
			if ((num & 128) != 0 && EcoGlobalVar.ECOAppRunMode == 2)
			{
				ClientAPI.StopBroadcastChannel();
				EcoGlobalVar.stopalltimer(true);
				if (!EcoGlobalVar.isinExit)
				{
					EcoGlobalVar.isinExit = true;
					ControlAccess.ConfigControl config2 = delegate(Control control, object obj)
					{
						TopMostMessageBox.Show(EcoLanguage.getMsginThread("ThreadPopMsgneedRelogin", new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					};
					ControlAccess controlAccess2 = new ControlAccess(EcoGlobalVar.gl_mainForm, config2);
					controlAccess2.Access(EcoGlobalVar.gl_mainForm, null);
					Program.ExitApp();
				}
			}
			if ((num & 1) != 0)
			{
				if (EcoGlobalVar.gl_DashBoardCtrl != null)
				{
					EcoGlobalVar.gl_DashBoardCtrl.FreshFlg_DashBoard = 1;
					EcoGlobalVar.gl_DashBoardCtrl.resetTimer();
				}
				if (EcoGlobalVar.gl_DashBoardUserCtrl != null)
				{
					EcoGlobalVar.gl_DashBoardUserCtrl.FreshFlg_DashBoard = 1;
					EcoGlobalVar.gl_DashBoardUserCtrl.resetTimer();
				}
				if (EcoGlobalVar.gl_monitorCtrl != null)
				{
					EcoGlobalVar.gl_monitorCtrl.FreshFlg_DashBoard = 1;
					EcoGlobalVar.gl_monitorCtrl.resetTimer();
				}
			}
			if ((num & 2) != 0 || (infoType & 2) != 0)
			{
				if (EcoGlobalVar.gl_DashBoardCtrl != null && EcoGlobalVar.gl_DashBoardCtrl.FreshFlg_DashBoard != 1)
				{
					EcoGlobalVar.gl_DashBoardCtrl.FreshFlg_DashBoard = 2;
				}
				if (EcoGlobalVar.gl_DashBoardUserCtrl != null && EcoGlobalVar.gl_DashBoardUserCtrl.FreshFlg_DashBoard != 1)
				{
					EcoGlobalVar.gl_DashBoardUserCtrl.FreshFlg_DashBoard = 2;
				}
				if (EcoGlobalVar.gl_monitorCtrl != null && EcoGlobalVar.gl_monitorCtrl.FreshFlg_DashBoard != 1)
				{
					EcoGlobalVar.gl_monitorCtrl.FreshFlg_DashBoard = 2;
				}
			}
		}
		public static void ServerClosedProc(int reason)
		{
			if (EcoGlobalVar.ECOAppRunStatus == 2)
			{
				return;
			}
			if (reason != -1)
			{
				if (reason != -2)
				{
					return;
				}
			}
			try
			{
				ClientAPI.StopBroadcastChannel();
			}
			catch (System.Exception)
			{
			}
			try
			{
				EcoGlobalVar.stopalltimer(true);
			}
			catch (System.Exception)
			{
			}
			if (!EcoGlobalVar.isinExit)
			{
				EcoGlobalVar.isinExit = true;
				ControlAccess.ConfigControl config = delegate(Control control, object param)
				{
					int? num = param as int?;
					if (!num.HasValue || num == -1)
					{
						TopMostMessageBox.Show(EcoLanguage.getMsginThread("srvConnectFail", new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					TopMostMessageBox.Show(EcoLanguage.getMsginThread("srvEndSession", new string[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				};
				ControlAccess controlAccess = new ControlAccess(EcoGlobalVar.gl_mainForm, config);
				controlAccess.Access(EcoGlobalVar.gl_mainForm, reason);
				Program.ExitApp();
			}
		}
		public static void stopalltimer(bool exitApp)
		{
			if (EcoGlobalVar.gl_DevManPage != null)
			{
				EcoGlobalVar.gl_DevManPage.endZoneBicker();
			}
			if (EcoGlobalVar.gl_PowerOPCtrl != null)
			{
				EcoGlobalVar.gl_PowerOPCtrl.endTimer();
			}
			if (EcoGlobalVar.gl_DataGpOPAll != null)
			{
				EcoGlobalVar.gl_DataGpOPAll.endTimer();
			}
			if (EcoGlobalVar.gl_DashBoardCtrl != null)
			{
				EcoGlobalVar.gl_DashBoardCtrl.endBoardTimer();
			}
			if (EcoGlobalVar.gl_DashBoardUserCtrl != null)
			{
				EcoGlobalVar.gl_DashBoardUserCtrl.endBoardTimer();
			}
			if (EcoGlobalVar.gl_DevManCtrl != null)
			{
				EcoGlobalVar.gl_DevManCtrl.endTimer();
			}
			if (EcoGlobalVar.gl_otherDevCtrl != null)
			{
				EcoGlobalVar.gl_otherDevCtrl.endTimer();
			}
			if (EcoGlobalVar.gl_SysDBCapCtrl != null)
			{
				EcoGlobalVar.gl_SysDBCapCtrl.endDBcapTimer();
			}
			if (exitApp)
			{
				if (EcoGlobalVar.gl_monitorCtrl != null)
				{
					EcoGlobalVar.gl_monitorCtrl.endBoardTimer();
				}
				if (EcoGlobalVar.gl_EnegAnalysisCtrl != null)
				{
					EcoGlobalVar.gl_EnegAnalysisCtrl.endTimer();
				}
				if (EcoGlobalVar.gl_BillingRptCtrl != null)
				{
					EcoGlobalVar.gl_BillingRptCtrl.endTimer();
				}
			}
		}
	}
}
