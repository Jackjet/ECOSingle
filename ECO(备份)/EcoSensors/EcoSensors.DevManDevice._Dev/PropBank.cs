using CommonAPI.Global;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice._Dev
{
	public class PropBank : UserControl
	{
		private IContainer components;
		private Label labMaxPowerDisBound;
		private Label labMaxPowerBound;
		private Label labMaxVoltageBound;
		private GroupBox gbThreshold;
		private Label labMaxPortCurrentBound;
		private Label label156;
		private Label label157;
		private Label label158;
		private Label label159;
		private Label label161;
		private Label label162;
		private TextBox tbOMinCurrent;
		private TextBox tbOMaxCurrent;
		private Label label164;
		private Label label165;
		private Label label173;
		private TextBox tbOMinVoltage;
		private Label label175;
		private TextBox tbOMaxVoltage;
		private TextBox tbOMaxPowerDiss;
		private Label label169;
		private TextBox tbOMinPowerDiss;
		private Label label168;
		private Label label171;
		private TextBox tbOMinPower;
		private Label label172;
		private TextBox tbOMaxPower;
		private Button butBankAssign;
		private Button butBank4;
		private Button butBank3;
		private Button butBank2;
		private Button butBankSave;
		private Button butBank1;
		private TextBox tbBankNm;
		private Label lbBankNm;
		private Label labDevNm;
		private Label labDevRackNm;
		private Label labDevModel;
		private Label labDevIp;
		private GroupBox gbBankConfig;
		private Label labBankNo;
		private GroupBox groupBox2;
		private Label label12;
		private Label label13;
		private Label label16;
		private Label label18;
		private GroupBox groupBox1;
		private Button butBank8;
		private Button butBank7;
		private Button butBank6;
		private Button butBank5;
		private ToolTip toolTip1;
		public PropBank()
		{
			this.InitializeComponent();
			this.tbBankNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(int devID, int bankID, bool onlinest)
		{
			this.butBankAssign.Enabled = onlinest;
			this.butBankSave.Enabled = onlinest;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID);
			this.labDevIp.Text = deviceByID.DeviceIP;
			this.labDevIp.Tag = deviceByID.FWVersion;
			this.labDevModel.Text = deviceByID.ModelNm;
			this.labDevModel.Tag = devID.ToString();
			this.labDevNm.Text = deviceByID.DeviceName;
			string text = deviceByID.ModelNm;
			if (DevAccessCfg.GetInstance().isAutodectDev(deviceByID.ModelNm, deviceByID.FWVersion))
			{
				text = text + " (F/W: " + deviceByID.FWVersion + ")";
			}
			this.toolTip1.SetToolTip(this.labDevModel, text);
			RackInfo rackByID = RackInfo.getRackByID(deviceByID.RackID);
			this.labDevRackNm.Text = rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
			this.butBank1.Visible = false;
			this.butBank2.Visible = false;
			this.butBank3.Visible = false;
			this.butBank4.Visible = false;
			this.butBank5.Visible = false;
			this.butBank6.Visible = false;
			this.butBank7.Visible = false;
			this.butBank8.Visible = false;
			System.Collections.Generic.List<BankInfo> bankInfo = deviceByID.GetBankInfo();
			foreach (BankInfo current in bankInfo)
			{
				string portLists = current.PortLists;
				switch (System.Convert.ToInt32(portLists))
				{
				case 1:
					this.butBank1.Visible = true;
					this.butBank1.Tag = current.ID.ToString();
					break;
				case 2:
					this.butBank2.Visible = true;
					this.butBank2.Tag = current.ID.ToString();
					break;
				case 3:
					this.butBank3.Visible = true;
					this.butBank3.Tag = current.ID.ToString();
					break;
				case 4:
					this.butBank4.Visible = true;
					this.butBank4.Tag = current.ID.ToString();
					break;
				case 5:
					this.butBank5.Visible = true;
					this.butBank5.Tag = current.ID.ToString();
					break;
				case 6:
					this.butBank6.Visible = true;
					this.butBank6.Tag = current.ID.ToString();
					break;
				case 7:
					this.butBank7.Visible = true;
					this.butBank7.Tag = current.ID.ToString();
					break;
				case 8:
					this.butBank8.Visible = true;
					this.butBank8.Tag = current.ID.ToString();
					break;
				}
			}
			this.bankConfigPageControlInit(deviceModelConfig);
			if (bankID == 0)
			{
				this.setBankConfigData(deviceByID, System.Convert.ToInt32(this.butBank1.Tag));
				return;
			}
			this.setBankConfigData(deviceByID, bankID);
		}
		private void bankConfigPageControlInit(DevModelConfig devcfg)
		{
			this.tbBankNm.BackColor = Color.White;
			if (devcfg.perbankReading == 2)
			{
				this.gbThreshold.Show();
				this.butBankAssign.Visible = true;
				return;
			}
			this.gbThreshold.Hide();
			this.butBankAssign.Visible = false;
		}
		private void setBankConfigData(DeviceInfo pCurDev, int bankID)
		{
			string text = this.labDevModel.Text;
			string value = bankID.ToString();
			BankInfo bankInfoByID = DeviceOperation.GetBankInfoByID(bankID);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, pCurDev.FWVersion);
			int selNo = System.Convert.ToInt32(bankInfoByID.PortLists);
			this.labBankNo.Text = selNo.ToString();
			System.Collections.Generic.List<BankInfo> bankInfo = pCurDev.GetBankInfo();
			for (int i = 1; i <= bankInfo.Count; i++)
			{
				Control[] array = this.gbBankConfig.Controls.Find("butBank" + i, false);
				if (array.Length > 0)
				{
					string text2 = ((Button)array[0]).Tag.ToString();
					if (!text2.Equals(value))
					{
						((Button)array[0]).BackColor = Color.PaleTurquoise;
					}
					else
					{
						((Button)array[0]).BackColor = Color.DarkCyan;
					}
				}
			}
			this.tbBankNm.Text = bankInfoByID.BankName;
			int num = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "bank");
			if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
			{
				this.labMaxPortCurrentBound.Text = ((!this.tbOMinCurrent.ReadOnly || !this.tbOMaxCurrent.ReadOnly) ? devcfgUtil.RangeCurrent(deviceModelConfig, "bank", selNo, "F0") : "");
				ThresholdUtil.SetUIEdit(this.tbOMinCurrent, (num & 1) == 0, bankInfoByID.Min_current, 0, "F0");
				ThresholdUtil.SetUIEdit(this.tbOMaxCurrent, (num & 2) == 0, bankInfoByID.Max_current, 0, "F0");
			}
			else
			{
				this.labMaxPortCurrentBound.Text = ((!this.tbOMinCurrent.ReadOnly || !this.tbOMaxCurrent.ReadOnly) ? devcfgUtil.RangeCurrent(deviceModelConfig, "bank", selNo, "F1") : "");
				ThresholdUtil.SetUIEdit(this.tbOMinCurrent, (num & 1) == 0, bankInfoByID.Min_current, 0, "F1");
				ThresholdUtil.SetUIEdit(this.tbOMaxCurrent, (num & 2) == 0, bankInfoByID.Max_current, 0, "F1");
			}
			ThresholdUtil.SetUIEdit(this.tbOMinVoltage, (num & 4) == 0, bankInfoByID.Min_voltage, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMaxVoltage, (num & 8) == 0, bankInfoByID.Max_voltage, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMinPower, (num & 16) == 0, bankInfoByID.Min_power, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMaxPower, (num & 32) == 0, bankInfoByID.Max_power, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMinPowerDiss, (num & 64) == 0, bankInfoByID.Min_power_diss, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMaxPowerDiss, (num & 128) == 0, bankInfoByID.Max_power_diss, 0, "F1");
			this.labMaxVoltageBound.Text = ((!this.tbOMinVoltage.ReadOnly || !this.tbOMaxVoltage.ReadOnly) ? devcfgUtil.RangeVoltage(deviceModelConfig, "bank", selNo) : "");
			this.labMaxPowerBound.Text = ((!this.tbOMinPower.ReadOnly || !this.tbOMaxPower.ReadOnly) ? devcfgUtil.RangePower(deviceModelConfig, "bank", selNo, 1.0) : "");
			this.labMaxPowerDisBound.Text = ((!this.tbOMinPowerDiss.ReadOnly || !this.tbOMaxPowerDiss.ReadOnly) ? devcfgUtil.RangePowerDiss(deviceModelConfig, "bank", selNo) : "");
			this.tbBankNm.BackColor = Color.White;
		}
		public void TimerProc(bool onlinest, int haveThresholdChange)
		{
			if (haveThresholdChange == 1)
			{
				string value = this.labDevModel.Tag.ToString();
				int num = System.Convert.ToInt32(value);
				string text = this.labBankNo.Text;
				int i_banknum = System.Convert.ToInt32(text);
				BankInfo bankInfo = new BankInfo(num, i_banknum);
				this.pageInit(num, bankInfo.ID, onlinest);
				return;
			}
			this.butBankAssign.Enabled = onlinest;
			this.butBankSave.Enabled = onlinest;
		}
		private void butBank_Click(object sender, System.EventArgs e)
		{
			string text = this.labDevModel.Text;
			string value = this.labDevModel.Tag.ToString();
			int l_id = System.Convert.ToInt32(value);
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, deviceByID.FWVersion);
			this.bankConfigPageControlInit(deviceModelConfig);
			string value2 = ((Button)sender).Tag.ToString();
			((Button)sender).BackColor = Color.DarkCyan;
			this.setBankConfigData(deviceByID, System.Convert.ToInt32(value2));
		}
		private void tbBankNm_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == ' ')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void butBankSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				string value = this.labDevModel.Tag.ToString();
				int num = System.Convert.ToInt32(value);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.bankCheck(deviceModelConfig))
				{
					string text = this.tbBankNm.Text;
					string text2 = this.labBankNo.Text;
					int num2 = System.Convert.ToInt32(text2);
					BankInfo bankInfo = new BankInfo(num, num2);
					DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
					BankThreshold bankThreshold = new BankThreshold(num2);
					bankThreshold.BankName = text;
					if (this.gbThreshold.Visible)
					{
						bankInfo.Min_current = ThresholdUtil.UI2DB(this.tbOMinCurrent, bankInfo.Min_current, 0);
						bankInfo.Max_current = ThresholdUtil.UI2DB(this.tbOMaxCurrent, bankInfo.Max_current, 0);
						bankInfo.Min_voltage = ThresholdUtil.UI2DB(this.tbOMinVoltage, bankInfo.Min_voltage, 0);
						bankInfo.Max_voltage = ThresholdUtil.UI2DB(this.tbOMaxVoltage, bankInfo.Max_voltage, 0);
						bankInfo.Min_power = ThresholdUtil.UI2DB(this.tbOMinPower, bankInfo.Min_power, 0);
						bankInfo.Max_power = ThresholdUtil.UI2DB(this.tbOMaxPower, bankInfo.Max_power, 0);
						bankInfo.Min_power_diss = ThresholdUtil.UI2DB(this.tbOMinPowerDiss, bankInfo.Min_power_diss, 0);
						bankInfo.Max_power_diss = ThresholdUtil.UI2DB(this.tbOMaxPowerDiss, bankInfo.Max_power_diss, 0);
						int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "bank");
						bankThreshold.MinCurrentMt = bankInfo.Min_current;
						bankThreshold.MaxCurrentMT = bankInfo.Max_current;
						bankThreshold.MinVoltageMT = bankInfo.Min_voltage;
						bankThreshold.MaxVoltageMT = bankInfo.Max_voltage;
						bankThreshold.MinPowerMT = bankInfo.Min_power;
						bankThreshold.MaxPowerMT = bankInfo.Max_power;
						bankThreshold.MaxPowerDissMT = bankInfo.Max_power_diss;
						ThresholdUtil.UI2Dev(thflg, bankThreshold);
					}
					bool flag = true;
					if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold)
					{
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						flag = devAccessAPI.SetBankThreshold(bankThreshold, deviceByID.Mac);
					}
					if (flag)
					{
						if (bankInfo != null)
						{
							bankInfo.BankName = text;
							bankInfo.Update();
							EcoGlobalVar.setDashBoardFlg(128uL, string.Concat(new object[]
							{
								"#UPDATE#D",
								bankInfo.DeviceID,
								":B",
								bankInfo.ID,
								";"
							}), 2);
							string valuePair = ValuePairs.getValuePair("Username");
							if (!string.IsNullOrEmpty(valuePair))
							{
								LogAPI.writeEventLog("0630010", new string[]
								{
									bankInfo.BankName,
									bankInfo.PortLists,
									deviceByID.DeviceIP,
									deviceByID.DeviceName,
									valuePair
								});
							}
							else
							{
								LogAPI.writeEventLog("0630010", new string[]
								{
									bankInfo.BankName,
									bankInfo.PortLists,
									deviceByID.DeviceIP,
									deviceByID.DeviceName
								});
							}
						}
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
					}
					else
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("PropBank Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private bool bankCheck(DevModelConfig devcfg)
		{
			string text = this.tbBankNm.Text.Trim();
			this.tbBankNm.Text = text;
			string text2 = this.labBankNo.Text;
			int num = System.Convert.ToInt32(text2);
			bool flag = false;
			if (((ulong)devcfg.bankOpt_nameempty & (ulong)(1L << (num - 1 & 31))) != 0uL)
			{
				Ecovalidate.checkTextIsNull(this.tbBankNm, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbBankNm.Text
					}));
					this.tbBankNm.BackColor = Color.Red;
					return false;
				}
				this.tbBankNm.BackColor = Color.White;
			}
			if (this.gbThreshold.Visible)
			{
				flag = true;
				int num2 = devcfgUtil.UIThresholdEditFlg(devcfg, "bank");
				Ecovalidate.checkThresholdValue(this.tbOMinCurrent, this.labMaxPortCurrentBound, (num2 & 256) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbOMaxCurrent, this.labMaxPortCurrentBound, (num2 & 512) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbOMinVoltage, this.labMaxVoltageBound, (num2 & 1024) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbOMaxVoltage, this.labMaxVoltageBound, (num2 & 2048) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbOMinPower, this.labMaxPowerBound, (num2 & 4096) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbOMaxPower, this.labMaxPowerBound, (num2 & 8192) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbOMinPowerDiss, this.labMaxPowerDisBound, (num2 & 16384) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbOMaxPowerDiss, this.labMaxPowerDisBound, (num2 & 32768) == 0, ref flag);
				if (!flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_Thresholdinvalid, new string[0]));
					return false;
				}
				Ecovalidate.checkThresholdMaxMixValue(this.tbOMaxCurrent, this.tbOMinCurrent, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbOMaxVoltage, this.tbOMinVoltage, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbOMaxPower, this.tbOMinPower, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbOMaxPowerDiss, this.tbOMinPowerDiss, ref flag);
				if (!flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdMinMax, new string[0]));
					return false;
				}
			}
			return true;
		}
		private void threshold_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(textBox, e.KeyChar, 1);
			if (flag)
			{
				char keyChar = e.KeyChar;
				if ((keyChar == '.' || keyChar == ',') && (textBox.Name.Equals(this.tbOMinCurrent.Name) || textBox.Name.Equals(this.tbOMaxCurrent.Name)))
				{
					string text = this.labDevModel.Text;
					string fmwareVer = this.labDevIp.Tag.ToString();
					if (DevAccessCfg.GetInstance().getDeviceModelConfig(text, fmwareVer).commonThresholdFlag == Constant.APC_PDU)
					{
						e.Handled = true;
					}
				}
				return;
			}
			e.Handled = true;
		}
		private void butBankAssign_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			DBConn dBConn = null;
			string text = this.labDevModel.Tag.ToString();
			try
			{
				int num = System.Convert.ToInt32(text);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.bankCheck(deviceModelConfig))
				{
					DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Dev_ApplyBank, new string[0]), MessageBoxButtons.OKCancel);
					if (dialogResult != DialogResult.Cancel)
					{
						string text2 = this.tbBankNm.Text;
						string text3 = this.labBankNo.Text;
						int num2 = System.Convert.ToInt32(text3);
						BankInfo bankInfo = new BankInfo(num, num2);
						DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
						BankThreshold bankThreshold = new BankThreshold(1);
						if (this.gbThreshold.Visible)
						{
							bankInfo.Min_current = ThresholdUtil.UI2DB(this.tbOMinCurrent, bankInfo.Min_current, 0);
							bankInfo.Max_current = ThresholdUtil.UI2DB(this.tbOMaxCurrent, bankInfo.Max_current, 0);
							bankInfo.Min_voltage = ThresholdUtil.UI2DB(this.tbOMinVoltage, bankInfo.Min_voltage, 0);
							bankInfo.Max_voltage = ThresholdUtil.UI2DB(this.tbOMaxVoltage, bankInfo.Max_voltage, 0);
							bankInfo.Min_power = ThresholdUtil.UI2DB(this.tbOMinPower, bankInfo.Min_power, 0);
							bankInfo.Max_power = ThresholdUtil.UI2DB(this.tbOMaxPower, bankInfo.Max_power, 0);
							bankInfo.Min_power_diss = ThresholdUtil.UI2DB(this.tbOMinPowerDiss, bankInfo.Min_power_diss, 0);
							bankInfo.Max_power_diss = ThresholdUtil.UI2DB(this.tbOMaxPowerDiss, bankInfo.Max_power_diss, 0);
							int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "bank");
							bankThreshold.MinCurrentMt = bankInfo.Min_current;
							bankThreshold.MaxCurrentMT = bankInfo.Max_current;
							bankThreshold.MinVoltageMT = bankInfo.Min_voltage;
							bankThreshold.MaxVoltageMT = bankInfo.Max_voltage;
							bankThreshold.MinPowerMT = bankInfo.Min_power;
							bankThreshold.MaxPowerMT = bankInfo.Max_power;
							bankThreshold.MaxPowerDissMT = bankInfo.Max_power_diss;
							ThresholdUtil.UI2Dev(thflg, bankThreshold);
						}
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						string myMac = deviceByID.Mac;
						System.Collections.Generic.List<BankInfo> bankInfo2 = deviceByID.GetBankInfo();
						dBConn = DBConnPool.getConnection();
						foreach (BankInfo current in bankInfo2)
						{
							BankThreshold bankThreshold2 = new BankThreshold(System.Convert.ToInt32(current.PortLists));
							if (num2 == System.Convert.ToInt32(current.PortLists))
							{
								current.BankName = text2;
							}
							bankThreshold2.BankName = current.BankName;
							if (this.gbThreshold.Visible)
							{
								bankThreshold2.MaxCurrentMT = bankThreshold.MaxCurrentMT;
								bankThreshold2.MinCurrentMt = bankThreshold.MinCurrentMt;
								bankThreshold2.MaxPowerMT = bankThreshold.MaxPowerMT;
								bankThreshold2.MinPowerMT = bankThreshold.MinPowerMT;
								bankThreshold2.MaxVoltageMT = bankThreshold.MaxVoltageMT;
								bankThreshold2.MinVoltageMT = bankThreshold.MinVoltageMT;
								bankThreshold2.MaxPowerDissMT = bankThreshold.MaxPowerDissMT;
							}
							bool flag2 = true;
							if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold)
							{
								flag2 = devAccessAPI.SetBankThreshold(bankThreshold2, myMac);
							}
							if (!flag2)
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
								return;
							}
							if (this.gbThreshold.Visible)
							{
								current.CopyThreshold(bankInfo);
							}
							current.UpdateBankThreshold(dBConn);
							flag = true;
							myMac = "";
						}
						if (dBConn != null)
						{
							dBConn.close();
						}
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							LogAPI.writeEventLog("0630011", new string[]
							{
								deviceByID.ModelNm,
								deviceByID.DeviceIP,
								deviceByID.DeviceName,
								valuePair
							});
						}
						else
						{
							LogAPI.writeEventLog("0630011", new string[]
							{
								deviceByID.ModelNm,
								deviceByID.DeviceIP,
								deviceByID.DeviceName
							});
						}
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("butBankAssign_Click Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
			finally
			{
				if (dBConn != null)
				{
					dBConn.close();
				}
				DeviceOperation.RefreshDBCache(false);
				if (flag)
				{
					EcoGlobalVar.setDashBoardFlg(128uL, "#UPDATE#D" + text + ":B*;", 2);
				}
			}
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropBank));
			this.labMaxPowerDisBound = new Label();
			this.labMaxPowerBound = new Label();
			this.labMaxVoltageBound = new Label();
			this.gbThreshold = new GroupBox();
			this.labMaxPortCurrentBound = new Label();
			this.label156 = new Label();
			this.label157 = new Label();
			this.label158 = new Label();
			this.label159 = new Label();
			this.label161 = new Label();
			this.label162 = new Label();
			this.tbOMinCurrent = new TextBox();
			this.tbOMaxCurrent = new TextBox();
			this.label164 = new Label();
			this.label165 = new Label();
			this.label173 = new Label();
			this.tbOMinVoltage = new TextBox();
			this.label175 = new Label();
			this.tbOMaxVoltage = new TextBox();
			this.tbOMaxPowerDiss = new TextBox();
			this.label169 = new Label();
			this.tbOMinPowerDiss = new TextBox();
			this.label168 = new Label();
			this.label171 = new Label();
			this.tbOMinPower = new TextBox();
			this.label172 = new Label();
			this.tbOMaxPower = new TextBox();
			this.butBankAssign = new Button();
			this.butBank4 = new Button();
			this.butBank3 = new Button();
			this.butBank2 = new Button();
			this.butBankSave = new Button();
			this.butBank1 = new Button();
			this.tbBankNm = new TextBox();
			this.lbBankNm = new Label();
			this.labDevNm = new Label();
			this.labDevRackNm = new Label();
			this.labDevModel = new Label();
			this.labDevIp = new Label();
			this.gbBankConfig = new GroupBox();
			this.butBank8 = new Button();
			this.groupBox1 = new GroupBox();
			this.groupBox2 = new GroupBox();
			this.label12 = new Label();
			this.label13 = new Label();
			this.label16 = new Label();
			this.label18 = new Label();
			this.butBank7 = new Button();
			this.labBankNo = new Label();
			this.butBank6 = new Button();
			this.butBank5 = new Button();
			this.toolTip1 = new ToolTip(this.components);
			this.gbThreshold.SuspendLayout();
			this.gbBankConfig.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labMaxPowerDisBound, "labMaxPowerDisBound");
			this.labMaxPowerDisBound.ForeColor = Color.Red;
			this.labMaxPowerDisBound.Name = "labMaxPowerDisBound";
			componentResourceManager.ApplyResources(this.labMaxPowerBound, "labMaxPowerBound");
			this.labMaxPowerBound.ForeColor = Color.Red;
			this.labMaxPowerBound.Name = "labMaxPowerBound";
			componentResourceManager.ApplyResources(this.labMaxVoltageBound, "labMaxVoltageBound");
			this.labMaxVoltageBound.ForeColor = Color.Red;
			this.labMaxVoltageBound.Name = "labMaxVoltageBound";
			this.gbThreshold.BackColor = Color.WhiteSmoke;
			this.gbThreshold.Controls.Add(this.labMaxPowerDisBound);
			this.gbThreshold.Controls.Add(this.labMaxPowerBound);
			this.gbThreshold.Controls.Add(this.labMaxVoltageBound);
			this.gbThreshold.Controls.Add(this.labMaxPortCurrentBound);
			this.gbThreshold.Controls.Add(this.label156);
			this.gbThreshold.Controls.Add(this.label157);
			this.gbThreshold.Controls.Add(this.label158);
			this.gbThreshold.Controls.Add(this.label159);
			this.gbThreshold.Controls.Add(this.label161);
			this.gbThreshold.Controls.Add(this.label162);
			this.gbThreshold.Controls.Add(this.tbOMinCurrent);
			this.gbThreshold.Controls.Add(this.tbOMaxCurrent);
			this.gbThreshold.Controls.Add(this.label164);
			this.gbThreshold.Controls.Add(this.label165);
			this.gbThreshold.Controls.Add(this.label173);
			this.gbThreshold.Controls.Add(this.tbOMinVoltage);
			this.gbThreshold.Controls.Add(this.label175);
			this.gbThreshold.Controls.Add(this.tbOMaxVoltage);
			this.gbThreshold.Controls.Add(this.tbOMaxPowerDiss);
			this.gbThreshold.Controls.Add(this.label169);
			this.gbThreshold.Controls.Add(this.tbOMinPowerDiss);
			this.gbThreshold.Controls.Add(this.label168);
			this.gbThreshold.Controls.Add(this.label171);
			this.gbThreshold.Controls.Add(this.tbOMinPower);
			this.gbThreshold.Controls.Add(this.label172);
			this.gbThreshold.Controls.Add(this.tbOMaxPower);
			componentResourceManager.ApplyResources(this.gbThreshold, "gbThreshold");
			this.gbThreshold.Name = "gbThreshold";
			this.gbThreshold.TabStop = false;
			componentResourceManager.ApplyResources(this.labMaxPortCurrentBound, "labMaxPortCurrentBound");
			this.labMaxPortCurrentBound.ForeColor = Color.Red;
			this.labMaxPortCurrentBound.Name = "labMaxPortCurrentBound";
			componentResourceManager.ApplyResources(this.label156, "label156");
			this.label156.ForeColor = Color.Black;
			this.label156.Name = "label156";
			componentResourceManager.ApplyResources(this.label157, "label157");
			this.label157.ForeColor = Color.Black;
			this.label157.Name = "label157";
			componentResourceManager.ApplyResources(this.label158, "label158");
			this.label158.ForeColor = Color.Black;
			this.label158.Name = "label158";
			componentResourceManager.ApplyResources(this.label159, "label159");
			this.label159.ForeColor = Color.Black;
			this.label159.Name = "label159";
			componentResourceManager.ApplyResources(this.label161, "label161");
			this.label161.ForeColor = Color.Black;
			this.label161.Name = "label161";
			componentResourceManager.ApplyResources(this.label162, "label162");
			this.label162.ForeColor = Color.Black;
			this.label162.Name = "label162";
			this.tbOMinCurrent.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbOMinCurrent, "tbOMinCurrent");
			this.tbOMinCurrent.ForeColor = Color.Black;
			this.tbOMinCurrent.Name = "tbOMinCurrent";
			this.tbOMinCurrent.ReadOnly = true;
			this.tbOMinCurrent.Tag = "bcurrent";
			this.tbOMinCurrent.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.tbOMaxCurrent, "tbOMaxCurrent");
			this.tbOMaxCurrent.ForeColor = Color.Black;
			this.tbOMaxCurrent.Name = "tbOMaxCurrent";
			this.tbOMaxCurrent.Tag = "bcurrent";
			this.tbOMaxCurrent.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label164, "label164");
			this.label164.ForeColor = Color.Black;
			this.label164.Name = "label164";
			componentResourceManager.ApplyResources(this.label165, "label165");
			this.label165.ForeColor = Color.Black;
			this.label165.Name = "label165";
			componentResourceManager.ApplyResources(this.label173, "label173");
			this.label173.ForeColor = Color.Black;
			this.label173.Name = "label173";
			componentResourceManager.ApplyResources(this.tbOMinVoltage, "tbOMinVoltage");
			this.tbOMinVoltage.ForeColor = Color.Black;
			this.tbOMinVoltage.Name = "tbOMinVoltage";
			this.tbOMinVoltage.Tag = "bvoltage";
			this.tbOMinVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label175, "label175");
			this.label175.ForeColor = Color.Black;
			this.label175.Name = "label175";
			componentResourceManager.ApplyResources(this.tbOMaxVoltage, "tbOMaxVoltage");
			this.tbOMaxVoltage.ForeColor = Color.Black;
			this.tbOMaxVoltage.Name = "tbOMaxVoltage";
			this.tbOMaxVoltage.Tag = "bvoltage";
			this.tbOMaxVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.tbOMaxPowerDiss, "tbOMaxPowerDiss");
			this.tbOMaxPowerDiss.ForeColor = Color.Black;
			this.tbOMaxPowerDiss.Name = "tbOMaxPowerDiss";
			this.tbOMaxPowerDiss.Tag = "bpowerDiss";
			this.tbOMaxPowerDiss.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label169, "label169");
			this.label169.ForeColor = Color.Black;
			this.label169.Name = "label169";
			this.tbOMinPowerDiss.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbOMinPowerDiss, "tbOMinPowerDiss");
			this.tbOMinPowerDiss.ForeColor = Color.Black;
			this.tbOMinPowerDiss.Name = "tbOMinPowerDiss";
			this.tbOMinPowerDiss.ReadOnly = true;
			this.tbOMinPowerDiss.Tag = "bpowerDiss";
			this.tbOMinPowerDiss.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label168, "label168");
			this.label168.ForeColor = Color.Black;
			this.label168.Name = "label168";
			componentResourceManager.ApplyResources(this.label171, "label171");
			this.label171.ForeColor = Color.Black;
			this.label171.Name = "label171";
			this.tbOMinPower.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbOMinPower, "tbOMinPower");
			this.tbOMinPower.ForeColor = Color.Black;
			this.tbOMinPower.Name = "tbOMinPower";
			this.tbOMinPower.ReadOnly = true;
			this.tbOMinPower.Tag = "bpower";
			this.tbOMinPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label172, "label172");
			this.label172.ForeColor = Color.Black;
			this.label172.Name = "label172";
			componentResourceManager.ApplyResources(this.tbOMaxPower, "tbOMaxPower");
			this.tbOMaxPower.ForeColor = Color.Black;
			this.tbOMaxPower.Name = "tbOMaxPower";
			this.tbOMaxPower.Tag = "bpower";
			this.tbOMaxPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.butBankAssign.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butBankAssign, "butBankAssign");
			this.butBankAssign.Name = "butBankAssign";
			this.butBankAssign.UseVisualStyleBackColor = false;
			this.butBankAssign.Click += new System.EventHandler(this.butBankAssign_Click);
			this.butBank4.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank4, "butBank4");
			this.butBank4.ForeColor = Color.Black;
			this.butBank4.Name = "butBank4";
			this.butBank4.Tag = "4";
			this.butBank4.UseVisualStyleBackColor = false;
			this.butBank4.Click += new System.EventHandler(this.butBank_Click);
			this.butBank3.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank3, "butBank3");
			this.butBank3.ForeColor = Color.Black;
			this.butBank3.Name = "butBank3";
			this.butBank3.Tag = "3";
			this.butBank3.UseVisualStyleBackColor = false;
			this.butBank3.Click += new System.EventHandler(this.butBank_Click);
			this.butBank2.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank2, "butBank2");
			this.butBank2.ForeColor = Color.Black;
			this.butBank2.Name = "butBank2";
			this.butBank2.Tag = "2";
			this.butBank2.UseVisualStyleBackColor = false;
			this.butBank2.Click += new System.EventHandler(this.butBank_Click);
			this.butBankSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butBankSave, "butBankSave");
			this.butBankSave.Name = "butBankSave";
			this.butBankSave.UseVisualStyleBackColor = false;
			this.butBankSave.Click += new System.EventHandler(this.butBankSave_Click);
			this.butBank1.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank1, "butBank1");
			this.butBank1.ForeColor = Color.Black;
			this.butBank1.Name = "butBank1";
			this.butBank1.Tag = "1";
			this.butBank1.UseVisualStyleBackColor = false;
			this.butBank1.Click += new System.EventHandler(this.butBank_Click);
			componentResourceManager.ApplyResources(this.tbBankNm, "tbBankNm");
			this.tbBankNm.ForeColor = Color.Black;
			this.tbBankNm.Name = "tbBankNm";
			this.tbBankNm.KeyPress += new KeyPressEventHandler(this.tbBankNm_KeyPress);
			componentResourceManager.ApplyResources(this.lbBankNm, "lbBankNm");
			this.lbBankNm.ForeColor = Color.Black;
			this.lbBankNm.Name = "lbBankNm";
			this.labDevNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevNm, "labDevNm");
			this.labDevNm.ForeColor = Color.Black;
			this.labDevNm.Name = "labDevNm";
			this.labDevRackNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevRackNm, "labDevRackNm");
			this.labDevRackNm.ForeColor = Color.Black;
			this.labDevRackNm.Name = "labDevRackNm";
			this.labDevModel.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevModel, "labDevModel");
			this.labDevModel.ForeColor = Color.Black;
			this.labDevModel.Name = "labDevModel";
			this.labDevIp.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevIp, "labDevIp");
			this.labDevIp.ForeColor = Color.Black;
			this.labDevIp.Name = "labDevIp";
			this.gbBankConfig.Controls.Add(this.butBank8);
			this.gbBankConfig.Controls.Add(this.groupBox1);
			this.gbBankConfig.Controls.Add(this.groupBox2);
			this.gbBankConfig.Controls.Add(this.butBank7);
			this.gbBankConfig.Controls.Add(this.labBankNo);
			this.gbBankConfig.Controls.Add(this.butBank6);
			this.gbBankConfig.Controls.Add(this.butBank4);
			this.gbBankConfig.Controls.Add(this.butBank3);
			this.gbBankConfig.Controls.Add(this.butBank5);
			this.gbBankConfig.Controls.Add(this.butBank2);
			this.gbBankConfig.Controls.Add(this.butBank1);
			componentResourceManager.ApplyResources(this.gbBankConfig, "gbBankConfig");
			this.gbBankConfig.ForeColor = SystemColors.ControlText;
			this.gbBankConfig.Name = "gbBankConfig";
			this.gbBankConfig.TabStop = false;
			this.butBank8.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank8, "butBank8");
			this.butBank8.ForeColor = Color.Black;
			this.butBank8.Name = "butBank8";
			this.butBank8.Tag = "8";
			this.butBank8.UseVisualStyleBackColor = false;
			this.butBank8.Click += new System.EventHandler(this.butBank_Click);
			this.groupBox1.Controls.Add(this.lbBankNm);
			this.groupBox1.Controls.Add(this.tbBankNm);
			this.groupBox1.Controls.Add(this.gbThreshold);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.labDevNm);
			this.groupBox2.Controls.Add(this.labDevIp);
			this.groupBox2.Controls.Add(this.labDevModel);
			this.groupBox2.Controls.Add(this.labDevRackNm);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.ForeColor = SystemColors.ControlText;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.ForeColor = Color.Black;
			this.label12.Name = "label12";
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.ForeColor = Color.Black;
			this.label13.Name = "label13";
			componentResourceManager.ApplyResources(this.label16, "label16");
			this.label16.ForeColor = Color.Black;
			this.label16.Name = "label16";
			componentResourceManager.ApplyResources(this.label18, "label18");
			this.label18.ForeColor = Color.Black;
			this.label18.Name = "label18";
			this.butBank7.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank7, "butBank7");
			this.butBank7.ForeColor = Color.Black;
			this.butBank7.Name = "butBank7";
			this.butBank7.Tag = "7";
			this.butBank7.UseVisualStyleBackColor = false;
			this.butBank7.Click += new System.EventHandler(this.butBank_Click);
			componentResourceManager.ApplyResources(this.labBankNo, "labBankNo");
			this.labBankNo.ForeColor = Color.Black;
			this.labBankNo.Name = "labBankNo";
			this.butBank6.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank6, "butBank6");
			this.butBank6.ForeColor = Color.Black;
			this.butBank6.Name = "butBank6";
			this.butBank6.Tag = "6";
			this.butBank6.UseVisualStyleBackColor = false;
			this.butBank6.Click += new System.EventHandler(this.butBank_Click);
			this.butBank5.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butBank5, "butBank5");
			this.butBank5.ForeColor = Color.Black;
			this.butBank5.Name = "butBank5";
			this.butBank5.Tag = "5";
			this.butBank5.UseVisualStyleBackColor = false;
			this.butBank5.Click += new System.EventHandler(this.butBank_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butBankAssign);
			base.Controls.Add(this.butBankSave);
			base.Controls.Add(this.gbBankConfig);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropBank";
			this.gbThreshold.ResumeLayout(false);
			this.gbThreshold.PerformLayout();
			this.gbBankConfig.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
