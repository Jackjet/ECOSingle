using CommonAPI.InterProcess;
using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Login;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace EcoSensors.SysManDB
{
	public class SysManDBMaint : UserControl
	{
		public const string DBback_path = "\\DBBackup\\";
		public const string DBconfig_path = "\\DBConfig\\";
		private IContainer components;
		private TabControl tpDbMaintenance;
		private TabPage tpDbExport;
		private Button btnDbExport;
		private Label label1;
		private TextBox tbExportPath;
		private Button btnExportPath;
		private Label lbExportPath;
		private TabPage tpDbImport;
		private Label label5;
		private TextBox tbImportFile;
		private Button btnImportFile;
		private Label lbImportFile;
		private Button btnDbImport;
		private TabPage tpCfgRestore;
		private Label label2;
		private TextBox tbRestoreFile;
		private Button btRestore;
		private Label lbRestoreFile;
		private Button btCfgRestore;
		private TabPage tpCfgBackup;
		private Button btCfgBackup;
		private Label label3;
		private TextBox tbBackupPath;
		private Button btnBackupPath;
		private Label lbBackupPath;
		public SysManDBMaint()
		{
			this.InitializeComponent();
			this.tbImportFile.Text = System.IO.Directory.GetCurrentDirectory() + "\\DBBackup\\";
			this.tbExportPath.Text = System.IO.Directory.GetCurrentDirectory() + "\\DBBackup\\";
			this.tbRestoreFile.Text = System.IO.Directory.GetCurrentDirectory() + "\\DBConfig\\";
			this.tbBackupPath.Text = System.IO.Directory.GetCurrentDirectory() + "\\DBConfig\\";
		}
		public void pageInit()
		{
		}
		private void btnExportPath_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = this.tbExportPath.Text;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.tbExportPath.Text = folderBrowserDialog.SelectedPath;
			}
		}
		private void btnImportFile_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = this.tbImportFile.Text;
			openFileDialog.Filter = "bak files (*.bak)|*.bak|All Files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.tbImportFile.Text = openFileDialog.FileName;
			}
		}
		private void btnDbExport_Click(object sender, System.EventArgs e)
		{
			if (!DBMaintain.ConvertOldDataFinish)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_inSplitMySQLTable, new string[0]));
				return;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") && !DBMaintain.IsLocalIP(DBUrl.CURRENT_HOST_PATH))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBExportFail1, new string[0]));
				return;
			}
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbExportPath, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbExportPath.Text
				}));
				return;
			}
			if (!DBTools.CheckFreeSpaceSize4ExportDB(this.tbExportPath.Text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBExportFail2, new string[0]));
				return;
			}
			Program.IdleTimer_Pause(1);
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_ExportDB, new string[0]), null, new progressPopup.ProcessInThread(this.dbExportPro), null, 0);
			progressPopup.ShowDialog();
			object return_V = progressPopup.Return_V;
			Program.IdleTimer_Run(1);
			int? num = return_V as int?;
			if (!num.HasValue || num < 0)
			{
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.quitEcoFailed, new string[0]));
				Program.ExitApp();
				return;
			}
			EcoMessageBox.ShowInfo(this, EcoLanguage.getMsg(LangRes.quitEcoSucc, new string[0]));
			Program.ExitApp();
		}
		private object dbExportPro(object obj)
		{
			DbExportHanler dbExportHanler = new DbExportHanler(this.tbExportPath.Text);
			EcoGlobalVar.ECOAppRunStatus = 2;
			Program.StopService(EcoGlobalVar.gl_ServiceName, 30000);
			this.syncThreshold();
			System.Threading.Thread.Sleep(3000);
			bool flag = dbExportHanler.HandleEvennt();
			InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStDBMaintain", "DBMaintainFinish");
			Program.StartService(null);
			if (flag)
			{
				return 1;
			}
			return -1;
		}
		private void btnDbImport_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbImportFile, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbImportFile.Text
				}));
				return;
			}
			if (!System.IO.File.Exists(this.tbImportFile.Text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.File_unexist, new string[0]));
				this.tbImportFile.Focus();
				return;
			}
			DBTools.ProgramBar_Percent = 1;
			Program.IdleTimer_Pause(1);
			progressPopup progressPopup = new progressPopup("Information", 2, EcoLanguage.getMsg(LangRes.PopProgressMsg_Checkfile, new string[0]), null, new progressPopup.ProcessInThread(this.dbCheckImportFile), this.tbImportFile.Text, new progressPopup.ProgramBarThread(this.dbCheckImportFileBar), 0);
			progressPopup.ShowDialog();
			object return_V = progressPopup.Return_V;
			Program.IdleTimer_Run(1);
			string text = return_V as string;
			if (text == null || text.Length == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.File_illegal, new string[0]));
				this.tbImportFile.Focus();
				return;
			}
			if (text.StartsWith("DISKSIZELOW"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBImportFail2, new string[0]));
				this.tbImportFile.Focus();
				return;
			}
			if (text.StartsWith("UNZIP_ERROR"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBunziperror, new string[0]));
				return;
			}
			if (text.StartsWith("DBVERSION_ERROR"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBImportLowVerError, new string[0]));
				return;
			}
			string text2 = "MYSQLVERSIONERROR;";
			if (text.StartsWith(text2))
			{
				string text3 = text.Substring(text2.Length);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.needHighVerMySQL, new string[]
				{
					text3
				}));
				return;
			}
			if (text.StartsWith("MYSQL_CONNECT_ERROR"))
			{
				mysqlsetting mysqlsetting = new mysqlsetting(text);
				DialogResult dialogResult = mysqlsetting.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					return;
				}
				string text4 = DBUtil.CheckMySQLVersion("127.0.0.1", mysqlsetting.DBPort, mysqlsetting.DBusrnm, mysqlsetting.DBPsw, mysqlsetting.mySQLVer);
				if (text4.Length > 0)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.needHighVerMySQL, new string[]
					{
						text4
					}));
					return;
				}
				string[] array = text.Split(new string[]
				{
					","
				}, System.StringSplitOptions.RemoveEmptyEntries);
				string text5 = array[5];
				text = string.Concat(new object[]
				{
					"127.0.0.1,",
					mysqlsetting.DBPort,
					",",
					mysqlsetting.DBusrnm,
					",",
					mysqlsetting.DBPsw,
					",",
					text5,
					",RESET"
				});
			}
			DialogResult dialogResult2 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.DB_ChangeCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult2 == DialogResult.Cancel)
			{
				try
				{
					string aimPath = System.AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
					DBTools.DeleteDir(aimPath);
				}
				catch
				{
				}
				return;
			}
			Program.IdleTimer_Pause(1);
			DBTools.ProgramBar_Percent = 1;
			progressPopup = new progressPopup("Information", 2, EcoLanguage.getMsg(LangRes.PopProgressMsg_ImportDB, new string[0]), null, new progressPopup.ProcessInThread(this.dbImportPro), text, new progressPopup.ProgramBarThread(this.dbImportProBar), 0);
			progressPopup.ShowDialog();
			return_V = progressPopup.Return_V;
			Program.IdleTimer_Run(1);
			int? num = return_V as int?;
			if (!num.HasValue || num < 0)
			{
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.quitEcoFailed, new string[0]));
				Program.ExitApp();
				return;
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.quitEcoSucc, new string[0]));
			Program.ExitApp();
		}
		private object dbCheckImportFileBar(ref int ibarper, ref string sbartype)
		{
			ibarper = DBTools.ProgramBar_Percent;
			int arg_0C_0 = DBTools.ProgramBar_Type;
			string text = "";
			sbartype = text;
			return "";
		}
		private object dbCheckImportFile(object filename)
		{
			return DBTools.CheckDatabase((string)filename);
		}
		private object dbImportProBar(ref int ibarper, ref string sbartype)
		{
			ibarper = DBTools.ProgramBar_Percent;
			int arg_0C_0 = DBTools.ProgramBar_Type;
			string text = "";
			sbartype = text;
			return "";
		}
		private object dbImportPro(object param)
		{
			string path = (string)param;
			DbImportHandler dbImportHandler = new DbImportHandler(path);
			EcoGlobalVar.ECOAppRunStatus = 2;
			Program.StopService(EcoGlobalVar.gl_ServiceName, 30000);
			System.Threading.Thread.Sleep(3000);
			bool flag = dbImportHandler.HandleEvennt();
			InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStDBMaintain", "DBMaintainFinish");
			Program.StartService(null);
			if (!flag)
			{
				return -1;
			}
			return 1;
		}
		private void btRestore_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = this.tbRestoreFile.Text;
			openFileDialog.Filter = "dat files (*.dat)|*.dat|All Files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.tbRestoreFile.Text = openFileDialog.FileName;
			}
		}
		private void btCfgRestore_Click(object sender, System.EventArgs e)
		{
			if (!DBMaintain.ConvertOldDataFinish)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_inSplitMySQLTable, new string[0]));
				return;
			}
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbRestoreFile, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbRestoreFile.Text
				}));
				return;
			}
			if (!System.IO.File.Exists(this.tbRestoreFile.Text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.File_unexist, new string[0]));
				this.tbRestoreFile.Focus();
				return;
			}
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.DB_ConfigRestoreCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			Program.IdleTimer_Pause(1);
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_Restorecfg, new string[0]), null, new progressPopup.ProcessInThread(this.RestorecfgPro), this.tbRestoreFile.Text, 0);
			progressPopup.ShowDialog();
			object return_V = progressPopup.Return_V;
			Program.IdleTimer_Run(1);
			int? num = return_V as int?;
			if (!num.HasValue || num < 0)
			{
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.quitEcoFailed, new string[0]));
				Program.ExitApp();
				return;
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.quitEcoSucc, new string[0]));
			Program.ExitApp();
		}
		private object RestorecfgPro(object param)
		{
			string str_file = (string)param;
			EcoGlobalVar.ECOAppRunStatus = 2;
			Program.StopService(EcoGlobalVar.gl_ServiceName, 30000);
			System.Threading.Thread.Sleep(3000);
			int num = Backuptask.RestoreConfig(str_file);
			InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStDBMaintain", "DBMaintainFinish");
			Program.StartService(null);
			return num;
		}
		private void btnBackupPath_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = this.tbBackupPath.Text;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.tbBackupPath.Text = folderBrowserDialog.SelectedPath;
			}
		}
		private void btCfgBackup_Click(object sender, System.EventArgs e)
		{
			if (!DBMaintain.ConvertOldDataFinish)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_inSplitMySQLTable, new string[0]));
				return;
			}
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbBackupPath, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbBackupPath.Text
				}));
				return;
			}
			Program.IdleTimer_Pause(1);
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_saving, new string[0]), null, new progressPopup.ProcessInThread(this.backupcfgPro), this.tbBackupPath.Text, 0);
			progressPopup.ShowDialog();
			object return_V = progressPopup.Return_V;
			Program.IdleTimer_Run(1);
			int? num = return_V as int?;
			if (!num.HasValue || num < 0)
			{
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private object backupcfgPro(object param)
		{
			string str_folder = (string)param;
			this.syncThreshold();
			DeviceOperation.RefreshDBCache(false);
			EcoGlobalVar.setDashBoardFlg(2uL, "", 0);
			int num = Backuptask.BackupConfig4UI(str_folder);
			return num;
		}
		private int syncThreshold()
		{
			int result;
			try
			{
				System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
				System.Collections.Generic.List<DevSnmpConfig> list = new System.Collections.Generic.List<DevSnmpConfig>();
				new System.Collections.Generic.List<System.Collections.Hashtable>();
				foreach (DeviceInfo current in allDevice)
				{
					if (ClientAPI.IsDeviceOnline(current.DeviceID))
					{
						DevSnmpConfig sNMPpara = commUtil.getSNMPpara(current);
						list.Add(sNMPpara);
					}
				}
				DevMonitorAPI devMonitorAPI = new DevMonitorAPI();
				System.Collections.Generic.List<ThresholdMessage> monitorThresholds = devMonitorAPI.GetMonitorThresholds(list);
				this.UpdateAllThresholds(monitorThresholds);
				result = 1;
			}
			catch (System.Exception)
			{
				result = -1;
			}
			return result;
		}
		private bool UpdateAllThresholds(System.Collections.Generic.List<ThresholdMessage> thresholdMessageList)
		{
			bool result = false;
			DBConn dBConn = null;
			System.Collections.Hashtable deviceCache = DBCache.GetDeviceCache();
			System.Collections.Hashtable portCache = DBCache.GetPortCache();
			System.Collections.Hashtable bankCache = DBCache.GetBankCache();
			System.Collections.Hashtable sensorCache = DBCache.GetSensorCache();
			System.Collections.Hashtable deviceBankMap = DBCache.GetDeviceBankMap();
			System.Collections.Hashtable devicePortMap = DBCache.GetDevicePortMap();
			System.Collections.Hashtable deviceSensorMap = DBCache.GetDeviceSensorMap();
			if (dBConn == null)
			{
				dBConn = DBConnPool.getConnection();
			}
			foreach (ThresholdMessage current in thresholdMessageList)
			{
				if (current != null && current.DeviceReplyMac.Equals(current.DeviceMac))
				{
					if (!string.IsNullOrEmpty(current.AutoBasicInfo) && !string.IsNullOrEmpty(current.AutoRatingInfo))
					{
						try
						{
							DevAccessCfg.GetInstance().updateAutoModelList2Database(dBConn, current.ModelName, current.DeviceFWVer, current.AutoBasicInfo, current.AutoRatingInfo);
						}
						catch (System.Exception)
						{
						}
					}
					if (deviceCache != null && deviceCache.Count >= 1 && deviceCache.ContainsKey(current.DeviceID))
					{
						DeviceInfo deviceInfo = (DeviceInfo)deviceCache[current.DeviceID];
						if (deviceInfo != null)
						{
							deviceInfo.FWVersion = current.DeviceFWVer;
							deviceInfo.DeviceName = current.DeviceName;
							if (current.DeviceThreshold.MaxCurrentMT != -500f)
							{
								deviceInfo.Max_current = current.DeviceThreshold.MaxCurrentMT;
							}
							if (current.DeviceThreshold.MaxPowerMT != -500f)
							{
								deviceInfo.Max_power = current.DeviceThreshold.MaxPowerMT;
							}
							if (current.DeviceThreshold.MaxPowerDissMT != -500f)
							{
								deviceInfo.Max_power_diss = current.DeviceThreshold.MaxPowerDissMT;
							}
							if (current.DeviceThreshold.MaxVoltageMT != -500f)
							{
								deviceInfo.Max_voltage = current.DeviceThreshold.MaxVoltageMT;
							}
							if (current.DeviceThreshold.MinCurrentMT != -500f)
							{
								deviceInfo.Min_current = current.DeviceThreshold.MinCurrentMT;
							}
							if (current.DeviceThreshold.MinPowerMT != -500f)
							{
								deviceInfo.Min_power = current.DeviceThreshold.MinPowerMT;
							}
							if (current.DeviceThreshold.MinVoltageMT != -500f)
							{
								deviceInfo.Min_voltage = current.DeviceThreshold.MinVoltageMT;
							}
							deviceInfo.POPThreshold = current.DeviceThreshold.PopThreshold;
							deviceInfo.POPEnableMode = current.DeviceThreshold.PopEnableMode;
							deviceInfo.OutletPOPMode = current.DeviceThreshold.PopModeOutlet;
							deviceInfo.BankPOPLIFOMode = current.DeviceThreshold.PopModeLIFO;
							deviceInfo.BankPOPPriorityMode = current.DeviceThreshold.PopModePriority;
							if (current.DeviceThreshold.DoorSensorType != -500)
							{
								deviceInfo.DoorSensor = current.DeviceThreshold.DoorSensorType;
							}
							deviceInfo.UpdateDeviceThreshold(dBConn);
							if (current.SensorThreshold != null && current.SensorThreshold.Count > 0)
							{
								if (sensorCache == null || sensorCache.Count < 1 || deviceSensorMap == null || deviceSensorMap.Count < 1 || !deviceSensorMap.ContainsKey(current.DeviceID))
								{
									continue;
								}
								System.Collections.Generic.IEnumerator<int> enumerator2 = current.SensorThreshold.Keys.GetEnumerator();
								while (enumerator2.MoveNext())
								{
									SensorThreshold sensorThreshold = current.SensorThreshold[enumerator2.Current];
									SensorInfo sensor = this.getSensor(sensorCache, (System.Collections.Generic.List<int>)deviceSensorMap[current.DeviceID], enumerator2.Current);
									if (sensor != null)
									{
										if (sensorThreshold.MaxHumidityMT != -500f)
										{
											sensor.Max_humidity = sensorThreshold.MaxHumidityMT;
										}
										if (sensorThreshold.MaxPressMT != -500f)
										{
											sensor.Max_press = sensorThreshold.MaxPressMT;
										}
										if (sensorThreshold.MaxTemperatureMT != -500f)
										{
											sensor.Max_temperature = sensorThreshold.MaxTemperatureMT;
										}
										if (sensorThreshold.MinHumidityMT != -500f)
										{
											sensor.Min_humidity = sensorThreshold.MinHumidityMT;
										}
										if (sensorThreshold.MinPressMT != -500f)
										{
											sensor.Min_press = sensorThreshold.MinPressMT;
										}
										if (sensorThreshold.MinTemperatureMT != -500f)
										{
											sensor.Min_temperature = sensorThreshold.MinTemperatureMT;
										}
										sensor.UpdateSensorThreshold(dBConn);
									}
								}
							}
							if (current.OutletThreshold != null && current.OutletThreshold.Count > 0)
							{
								if (portCache == null || portCache.Count < 1 || devicePortMap == null || devicePortMap.Count < 1 || !devicePortMap.ContainsKey(current.DeviceID))
								{
									continue;
								}
								System.Collections.Generic.IEnumerator<int> enumerator3 = current.OutletThreshold.Keys.GetEnumerator();
								while (enumerator3.MoveNext())
								{
									OutletThreshold outletThreshold = current.OutletThreshold[enumerator3.Current];
									PortInfo port = this.getPort(portCache, (System.Collections.Generic.List<int>)devicePortMap[current.DeviceID], enumerator3.Current);
									if (port != null)
									{
										port.PortName = outletThreshold.OutletName;
										if (outletThreshold.MaxCurrentMT != -500f)
										{
											port.Max_current = outletThreshold.MaxCurrentMT;
										}
										if (outletThreshold.MaxPowerMT != -500f)
										{
											port.Max_power = outletThreshold.MaxPowerMT;
										}
										if (outletThreshold.MaxPowerDissMT != -500f)
										{
											port.Max_power_diss = outletThreshold.MaxPowerDissMT;
										}
										if (outletThreshold.MaxVoltageMT != -500f)
										{
											port.Max_voltage = outletThreshold.MaxVoltageMT;
										}
										if (outletThreshold.MinCurrentMt != -500f)
										{
											port.Min_current = outletThreshold.MinCurrentMt;
										}
										if (outletThreshold.MinPowerMT != -500f)
										{
											port.Min_power = outletThreshold.MinPowerMT;
										}
										if (outletThreshold.MinVoltageMT != -500f)
										{
											port.Min_voltage = outletThreshold.MinVoltageMT;
										}
										port.OutletConfirmation = (int)outletThreshold.Confirmation;
										port.OutletOffDelayTime = outletThreshold.OffDelayTime;
										port.OutletOnDelayTime = outletThreshold.OnDelayTime;
										port.OutletShutdownMethod = (int)outletThreshold.ShutdownMethod;
										port.OutletMAC = outletThreshold.MacAddress;
										port.UpdatePortThreshold(dBConn);
									}
								}
							}
							if (current.BankThreshold != null && current.BankThreshold.Count > 0 && bankCache != null && bankCache.Count >= 1 && deviceBankMap != null && deviceBankMap.Count >= 1 && deviceBankMap.ContainsKey(current.DeviceID))
							{
								System.Collections.Generic.IEnumerator<int> enumerator4 = current.BankThreshold.Keys.GetEnumerator();
								while (enumerator4.MoveNext())
								{
									BankThreshold bankThreshold = current.BankThreshold[enumerator4.Current];
									BankInfo bank = this.getBank(bankCache, (System.Collections.Generic.List<int>)deviceBankMap[current.DeviceID], enumerator4.Current);
									if (bank != null)
									{
										bank.BankName = bankThreshold.BankName;
										if (bankThreshold.MaxCurrentMT != -500f)
										{
											bank.Max_current = bankThreshold.MaxCurrentMT;
										}
										if (bankThreshold.MinCurrentMt != -500f)
										{
											bank.Min_current = bankThreshold.MinCurrentMt;
										}
										if (bankThreshold.MaxVoltageMT != -500f)
										{
											bank.Max_voltage = bankThreshold.MaxVoltageMT;
										}
										if (bankThreshold.MinVoltageMT != -500f)
										{
											bank.Min_voltage = bankThreshold.MinVoltageMT;
										}
										if (bankThreshold.MaxPowerMT != -500f)
										{
											bank.Max_power = bankThreshold.MaxPowerMT;
										}
										if (bankThreshold.MinPowerMT != -500f)
										{
											bank.Min_power = bankThreshold.MinPowerMT;
										}
										if (bankThreshold.MaxPowerDissMT != -500f)
										{
											bank.Max_power_diss = bankThreshold.MaxPowerDissMT;
										}
										bank.UpdateBankThreshold(dBConn);
									}
								}
							}
						}
					}
				}
			}
			if (dBConn != null)
			{
				dBConn.close();
			}
			return result;
		}
		private SensorInfo getSensor(System.Collections.Hashtable ht_scache, System.Collections.Generic.List<int> lt_sids, int i_num)
		{
			foreach (int current in lt_sids)
			{
				SensorInfo sensorInfo = (SensorInfo)ht_scache[current];
				if (sensorInfo != null && sensorInfo.Type == i_num)
				{
					return sensorInfo;
				}
			}
			return null;
		}
		private BankInfo getBank(System.Collections.Hashtable ht_bcache, System.Collections.Generic.List<int> lt_bids, int i_num)
		{
			foreach (int current in lt_bids)
			{
				BankInfo bankInfo = (BankInfo)ht_bcache[current];
				if (bankInfo != null && System.Convert.ToInt32(bankInfo.PortLists) == i_num)
				{
					return bankInfo;
				}
			}
			return null;
		}
		private PortInfo getPort(System.Collections.Hashtable ht_pcache, System.Collections.Generic.List<int> lt_pids, int i_num)
		{
			foreach (int current in lt_pids)
			{
				PortInfo portInfo = (PortInfo)ht_pcache[current];
				if (portInfo != null && portInfo.PortNum == i_num)
				{
					return portInfo;
				}
			}
			return null;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManDBMaint));
			this.tpDbMaintenance = new TabControl();
			this.tpDbImport = new TabPage();
			this.label5 = new Label();
			this.tbImportFile = new TextBox();
			this.btnImportFile = new Button();
			this.lbImportFile = new Label();
			this.btnDbImport = new Button();
			this.tpDbExport = new TabPage();
			this.btnDbExport = new Button();
			this.label1 = new Label();
			this.tbExportPath = new TextBox();
			this.btnExportPath = new Button();
			this.lbExportPath = new Label();
			this.tpCfgRestore = new TabPage();
			this.label2 = new Label();
			this.tbRestoreFile = new TextBox();
			this.btRestore = new Button();
			this.lbRestoreFile = new Label();
			this.btCfgRestore = new Button();
			this.tpCfgBackup = new TabPage();
			this.btCfgBackup = new Button();
			this.label3 = new Label();
			this.tbBackupPath = new TextBox();
			this.btnBackupPath = new Button();
			this.lbBackupPath = new Label();
			this.tpDbMaintenance.SuspendLayout();
			this.tpDbImport.SuspendLayout();
			this.tpDbExport.SuspendLayout();
			this.tpCfgRestore.SuspendLayout();
			this.tpCfgBackup.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tpDbMaintenance, "tpDbMaintenance");
			this.tpDbMaintenance.Controls.Add(this.tpDbImport);
			this.tpDbMaintenance.Controls.Add(this.tpDbExport);
			this.tpDbMaintenance.Controls.Add(this.tpCfgRestore);
			this.tpDbMaintenance.Controls.Add(this.tpCfgBackup);
			this.tpDbMaintenance.Name = "tpDbMaintenance";
			this.tpDbMaintenance.SelectedIndex = 0;
			this.tpDbImport.BackColor = Color.WhiteSmoke;
			this.tpDbImport.BorderStyle = BorderStyle.FixedSingle;
			this.tpDbImport.Controls.Add(this.label5);
			this.tpDbImport.Controls.Add(this.tbImportFile);
			this.tpDbImport.Controls.Add(this.btnImportFile);
			this.tpDbImport.Controls.Add(this.lbImportFile);
			this.tpDbImport.Controls.Add(this.btnDbImport);
			componentResourceManager.ApplyResources(this.tpDbImport, "tpDbImport");
			this.tpDbImport.Name = "tpDbImport";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = SystemColors.ControlText;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.tbImportFile, "tbImportFile");
			this.tbImportFile.Name = "tbImportFile";
			this.btnImportFile.BackColor = Color.Gainsboro;
			this.btnImportFile.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnImportFile, "btnImportFile");
			this.btnImportFile.Name = "btnImportFile";
			this.btnImportFile.UseVisualStyleBackColor = false;
			this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
			componentResourceManager.ApplyResources(this.lbImportFile, "lbImportFile");
			this.lbImportFile.Name = "lbImportFile";
			this.btnDbImport.BackColor = Color.Gainsboro;
			this.btnDbImport.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnDbImport, "btnDbImport");
			this.btnDbImport.Name = "btnDbImport";
			this.btnDbImport.UseVisualStyleBackColor = false;
			this.btnDbImport.Click += new System.EventHandler(this.btnDbImport_Click);
			this.tpDbExport.BackColor = Color.WhiteSmoke;
			this.tpDbExport.BorderStyle = BorderStyle.FixedSingle;
			this.tpDbExport.Controls.Add(this.btnDbExport);
			this.tpDbExport.Controls.Add(this.label1);
			this.tpDbExport.Controls.Add(this.tbExportPath);
			this.tpDbExport.Controls.Add(this.btnExportPath);
			this.tpDbExport.Controls.Add(this.lbExportPath);
			componentResourceManager.ApplyResources(this.tpDbExport, "tpDbExport");
			this.tpDbExport.Name = "tpDbExport";
			this.btnDbExport.BackColor = Color.Gainsboro;
			this.btnDbExport.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnDbExport, "btnDbExport");
			this.btnDbExport.Name = "btnDbExport";
			this.btnDbExport.UseVisualStyleBackColor = false;
			this.btnDbExport.Click += new System.EventHandler(this.btnDbExport_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = SystemColors.ControlText;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.tbExportPath, "tbExportPath");
			this.tbExportPath.Name = "tbExportPath";
			this.btnExportPath.BackColor = Color.Gainsboro;
			this.btnExportPath.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnExportPath, "btnExportPath");
			this.btnExportPath.Name = "btnExportPath";
			this.btnExportPath.UseVisualStyleBackColor = false;
			this.btnExportPath.Click += new System.EventHandler(this.btnExportPath_Click);
			componentResourceManager.ApplyResources(this.lbExportPath, "lbExportPath");
			this.lbExportPath.Name = "lbExportPath";
			this.tpCfgRestore.BackColor = Color.WhiteSmoke;
			this.tpCfgRestore.BorderStyle = BorderStyle.FixedSingle;
			this.tpCfgRestore.Controls.Add(this.label2);
			this.tpCfgRestore.Controls.Add(this.tbRestoreFile);
			this.tpCfgRestore.Controls.Add(this.btRestore);
			this.tpCfgRestore.Controls.Add(this.lbRestoreFile);
			this.tpCfgRestore.Controls.Add(this.btCfgRestore);
			componentResourceManager.ApplyResources(this.tpCfgRestore, "tpCfgRestore");
			this.tpCfgRestore.Name = "tpCfgRestore";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = SystemColors.ControlText;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.tbRestoreFile, "tbRestoreFile");
			this.tbRestoreFile.Name = "tbRestoreFile";
			this.btRestore.BackColor = Color.Gainsboro;
			this.btRestore.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btRestore, "btRestore");
			this.btRestore.Name = "btRestore";
			this.btRestore.UseVisualStyleBackColor = false;
			this.btRestore.Click += new System.EventHandler(this.btRestore_Click);
			componentResourceManager.ApplyResources(this.lbRestoreFile, "lbRestoreFile");
			this.lbRestoreFile.Name = "lbRestoreFile";
			this.btCfgRestore.BackColor = Color.Gainsboro;
			this.btCfgRestore.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btCfgRestore, "btCfgRestore");
			this.btCfgRestore.Name = "btCfgRestore";
			this.btCfgRestore.UseVisualStyleBackColor = false;
			this.btCfgRestore.Click += new System.EventHandler(this.btCfgRestore_Click);
			this.tpCfgBackup.BackColor = Color.WhiteSmoke;
			this.tpCfgBackup.BorderStyle = BorderStyle.FixedSingle;
			this.tpCfgBackup.Controls.Add(this.btCfgBackup);
			this.tpCfgBackup.Controls.Add(this.label3);
			this.tpCfgBackup.Controls.Add(this.tbBackupPath);
			this.tpCfgBackup.Controls.Add(this.btnBackupPath);
			this.tpCfgBackup.Controls.Add(this.lbBackupPath);
			componentResourceManager.ApplyResources(this.tpCfgBackup, "tpCfgBackup");
			this.tpCfgBackup.Name = "tpCfgBackup";
			this.btCfgBackup.BackColor = Color.Gainsboro;
			this.btCfgBackup.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btCfgBackup, "btCfgBackup");
			this.btCfgBackup.Name = "btCfgBackup";
			this.btCfgBackup.UseVisualStyleBackColor = false;
			this.btCfgBackup.Click += new System.EventHandler(this.btCfgBackup_Click);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = SystemColors.ControlText;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.tbBackupPath, "tbBackupPath");
			this.tbBackupPath.Name = "tbBackupPath";
			this.btnBackupPath.BackColor = Color.Gainsboro;
			this.btnBackupPath.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnBackupPath, "btnBackupPath");
			this.btnBackupPath.Name = "btnBackupPath";
			this.btnBackupPath.UseVisualStyleBackColor = false;
			this.btnBackupPath.Click += new System.EventHandler(this.btnBackupPath_Click);
			componentResourceManager.ApplyResources(this.lbBackupPath, "lbBackupPath");
			this.lbBackupPath.Name = "lbBackupPath";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.tpDbMaintenance);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysManDBMaint";
			this.tpDbMaintenance.ResumeLayout(false);
			this.tpDbImport.ResumeLayout(false);
			this.tpDbImport.PerformLayout();
			this.tpDbExport.ResumeLayout(false);
			this.tpDbExport.PerformLayout();
			this.tpCfgRestore.ResumeLayout(false);
			this.tpCfgRestore.PerformLayout();
			this.tpCfgBackup.ResumeLayout(false);
			this.tpCfgBackup.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
