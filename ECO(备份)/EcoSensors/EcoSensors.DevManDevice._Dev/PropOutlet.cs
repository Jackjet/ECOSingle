using CommonAPI.Global;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Properties;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice._Dev
{
	public class PropOutlet : UserControl
	{
		private const int MAX_OUTLETNUM = 42;
		private IContainer components;
		private Button butOutletSetAssign;
		private GroupBox gbOutletConfig;
		private Button butOutlet15;
		private GroupBox gbShutdownMethod;
		private Label labShutdown;
		private TextBox tbOffDelayTime;
		private ComboBox cbShutdownMethod;
		private Label lbOffDelayTime;
		private TextBox tbOnDelayTime;
		private Label lbOnDelayTime;
		private TextBox tbOutletNm;
		private Label lbOutletNm;
		private Button butOutlet16;
		private GroupBox gbThreshold;
		private Label labMaxPowerDisBound;
		private Label labMaxPowerBound;
		private Label labMaxVoltageBound;
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
		private Button butOutlet14;
		private Button butOutlet13;
		private Button butOutlet12;
		private Button butOutlet7;
		private Button butOutlet11;
		private Button butOutlet8;
		private Button butOutlet10;
		private Button butOutlet6;
		private Button butOutlet9;
		private Button butOutlet5;
		private Button butOutlet4;
		private Button butOutlet3;
		private Button butOutlet2;
		private Button butOutlet1;
		private Button butPortSave;
		private Label labDevNm;
		private Label labDevRackNm;
		private Label labDevModel;
		private Label labDevIp;
		private Label labOutletNo;
		private GroupBox groupBox2;
		private Label label2;
		private Label label11;
		private Label label14;
		private Label label16;
		private GroupBox groupBox1;
		private Button butOutlet24;
		private Button butOutlet23;
		private Button butOutlet22;
		private Button butOutlet17;
		private Button butOutlet21;
		private Button butOutlet18;
		private Button butOutlet20;
		private Button butOutlet19;
		private GroupBox groupBox3;
		private TextBox tbMAC;
		private Label label1;
		private Button butOutlet36;
		private Button butOutlet35;
		private Button butOutlet34;
		private Button butOutlet29;
		private Button butOutlet33;
		private Button butOutlet30;
		private Button butOutlet32;
		private Button butOutlet31;
		private Button butOutlet27;
		private Button butOutlet28;
		private Button butOutlet26;
		private Button butOutlet25;
		private Button butOutlet39;
		private Button butOutlet40;
		private Button butOutlet42;
		private Button butOutlet41;
		private Button butOutlet37;
		private Button butOutlet38;
		private ToolTip toolTip1;
		public PropOutlet()
		{
			this.InitializeComponent();
			this.tbOutletNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMAC.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOnDelayTime.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOffDelayTime.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMinPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbOMaxPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(int devID, int PortID, bool onlinest)
		{
			this.butOutletSetAssign.Enabled = onlinest;
			this.butPortSave.Enabled = onlinest;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID);
			this.labDevIp.Text = deviceByID.DeviceIP;
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
			for (int i = 1; i <= 42; i++)
			{
				Control[] array = this.gbOutletConfig.Controls.Find("butOutlet" + i, false);
				if (array.Length > 0)
				{
					((Button)array[0]).Visible = false;
				}
			}
			System.Collections.Generic.List<PortInfo> portInfo = deviceByID.GetPortInfo();
			foreach (PortInfo current in portInfo)
			{
				Control[] array2 = this.gbOutletConfig.Controls.Find("butOutlet" + current.PortNum, false);
				if (array2.Length > 0)
				{
					((Button)array2[0]).Visible = true;
					((Button)array2[0]).Tag = current.ID.ToString();
				}
			}
			this.outletConfigPageControlInit();
			if (PortID == 0)
			{
				this.setOutletConfigData(deviceByID, System.Convert.ToInt32(this.butOutlet1.Tag));
				return;
			}
			this.setOutletConfigData(deviceByID, PortID);
		}
		private void outletConfigPageControlInit()
		{
			this.tbOutletNm.BackColor = Color.White;
			this.tbOnDelayTime.BackColor = Color.White;
			this.tbOffDelayTime.BackColor = Color.White;
		}
		private void setOutletConfigData(DeviceInfo CurDev, int portID)
		{
			string text = this.labDevModel.Text;
			string value = portID.ToString();
			PortInfo portInfoByID = DeviceOperation.GetPortInfoByID(portID);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, CurDev.FWVersion);
			int portNum = portInfoByID.PortNum;
			bool flag;
			if (deviceModelConfig.perportreading == 2)
			{
				this.gbThreshold.Show();
				flag = true;
			}
			else
			{
				this.gbThreshold.Hide();
				flag = false;
			}
			bool flag2;
			if (deviceModelConfig.isOutletSwitchable(portNum - 1))
			{
				this.gbShutdownMethod.Show();
				flag2 = true;
				this.cbShutdownMethod.SelectedIndex = portInfoByID.OutletShutdownMethod - 1;
			}
			else
			{
				flag2 = false;
				this.gbShutdownMethod.Visible = false;
			}
			System.Collections.Generic.List<PortInfo> portInfo = CurDev.GetPortInfo();
			for (int i = 1; i <= portInfo.Count; i++)
			{
				Control[] array = this.gbOutletConfig.Controls.Find("butOutlet" + i, false);
				if (array.Length > 0)
				{
					string text2 = ((Button)array[0]).Tag.ToString();
					if (!text2.Equals(value))
					{
						((Button)array[0]).BackColor = Color.PaleTurquoise;
						((Button)array[0]).Image = Resources.tree_outletoff;
					}
					else
					{
						((Button)array[0]).BackColor = Color.DarkCyan;
						((Button)array[0]).Image = Resources.tree_outleton;
					}
				}
			}
			this.tbMAC.Text = portInfoByID.OutletMAC;
			this.tbOnDelayTime.Text = portInfoByID.OutletOnDelayTime.ToString();
			this.tbOffDelayTime.Text = portInfoByID.OutletOffDelayTime.ToString();
			this.tbOutletNm.Text = portInfoByID.PortName;
			this.labOutletNo.Text = portNum.ToString();
			int num = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "port");
			ThresholdUtil.SetUIEdit(this.tbOMinCurrent, (num & 1) == 0, portInfoByID.Min_current, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMaxCurrent, (num & 2) == 0, portInfoByID.Max_current, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMinVoltage, (num & 4) == 0, portInfoByID.Min_voltage, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMaxVoltage, (num & 8) == 0, portInfoByID.Max_voltage, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMinPower, (num & 16) == 0, portInfoByID.Min_power, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMaxPower, (num & 32) == 0, portInfoByID.Max_power, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMinPowerDiss, (num & 64) == 0, portInfoByID.Min_power_diss, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbOMaxPowerDiss, (num & 128) == 0, portInfoByID.Max_power_diss, 0, "F1");
			this.labMaxPortCurrentBound.Text = ((!this.tbOMinCurrent.ReadOnly || !this.tbOMaxCurrent.ReadOnly) ? devcfgUtil.RangeCurrent(deviceModelConfig, "port", portNum, "F1") : "");
			this.labMaxVoltageBound.Text = ((!this.tbOMinVoltage.ReadOnly || !this.tbOMaxVoltage.ReadOnly) ? devcfgUtil.RangeVoltage(deviceModelConfig, "port", portNum) : "");
			this.labMaxPowerBound.Text = ((!this.tbOMinPower.ReadOnly || !this.tbOMaxPower.ReadOnly) ? devcfgUtil.RangePower(deviceModelConfig, "port", portNum, 1.0) : "");
			this.labMaxPowerDisBound.Text = ((!this.tbOMinPowerDiss.ReadOnly || !this.tbOMaxPowerDiss.ReadOnly) ? devcfgUtil.RangePowerDiss(deviceModelConfig, "port", portNum) : "");
			if (!flag && !flag2)
			{
				this.butOutletSetAssign.Visible = false;
				return;
			}
			this.butOutletSetAssign.Visible = true;
		}
		public void TimerProc(bool onlinest, int haveThresholdChange)
		{
			if (haveThresholdChange == 1)
			{
				string value = this.labDevModel.Tag.ToString();
				int num = System.Convert.ToInt32(value);
				string text = this.labOutletNo.Text;
				int i_portnum = System.Convert.ToInt32(text);
				PortInfo portInfo = new PortInfo(num, i_portnum);
				this.pageInit(num, portInfo.ID, onlinest);
				return;
			}
			this.butOutletSetAssign.Enabled = onlinest;
			this.butPortSave.Enabled = onlinest;
		}
		private void butOutlet_Click(object sender, System.EventArgs e)
		{
			string text = this.labDevModel.Text;
			string value = this.labDevModel.Tag.ToString();
			int l_id = System.Convert.ToInt32(value);
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
			DevAccessCfg.GetInstance().getDeviceModelConfig(text, deviceByID.FWVersion);
			string value2 = ((Button)sender).Tag.ToString();
			((Button)sender).BackColor = Color.DarkCyan;
			this.outletConfigPageControlInit();
			this.setOutletConfigData(deviceByID, System.Convert.ToInt32(value2));
		}
		private void tbOutletNm_KeyPress(object sender, KeyPressEventArgs e)
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
		private void butPortSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				string value = this.labDevModel.Tag.ToString();
				int num = System.Convert.ToInt32(value);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.portCheck(deviceModelConfig))
				{
					string text = this.tbOutletNm.Text;
					string text2 = this.labOutletNo.Text;
					int num2 = System.Convert.ToInt32(text2);
					PortInfo portInfo = new PortInfo(num, num2);
					OutletThreshold outletThreshold = new OutletThreshold(num2);
					portInfo.PortName = text;
					outletThreshold.OutletName = text;
					if (this.gbShutdownMethod.Visible)
					{
						int outletShutdownMethod = this.cbShutdownMethod.SelectedIndex + 1;
						portInfo.OutletShutdownMethod = outletShutdownMethod;
						portInfo.OutletOnDelayTime = (this.tbOnDelayTime.Text.Equals(string.Empty) ? 0 : System.Convert.ToInt32(this.tbOnDelayTime.Text));
						portInfo.OutletOffDelayTime = (this.tbOffDelayTime.Text.Equals(string.Empty) ? 0 : System.Convert.ToInt32(this.tbOffDelayTime.Text));
						outletThreshold.OnDelayTime = portInfo.OutletOnDelayTime;
						outletThreshold.OffDelayTime = portInfo.OutletOffDelayTime;
						switch (outletShutdownMethod)
						{
						case 1:
							outletThreshold.ShutdownMethod = ShutDownMethod.KillThePower;
							portInfo.OutletMAC = "000000000000";
							break;
						case 2:
							outletThreshold.ShutdownMethod = ShutDownMethod.WakeOnLan;
							portInfo.OutletMAC = this.tbMAC.Text;
							break;
						case 3:
							outletThreshold.ShutdownMethod = ShutDownMethod.AfterAcBack;
							portInfo.OutletMAC = this.tbMAC.Text;
							break;
						}
						outletThreshold.MacAddress = portInfo.OutletMAC;
					}
					if (this.gbThreshold.Visible)
					{
						portInfo.Min_current = ThresholdUtil.UI2DB(this.tbOMinCurrent, portInfo.Min_current, 0);
						portInfo.Max_current = ThresholdUtil.UI2DB(this.tbOMaxCurrent, portInfo.Max_current, 0);
						portInfo.Min_voltage = ThresholdUtil.UI2DB(this.tbOMinVoltage, portInfo.Min_voltage, 0);
						portInfo.Max_voltage = ThresholdUtil.UI2DB(this.tbOMaxVoltage, portInfo.Max_voltage, 0);
						portInfo.Min_power = ThresholdUtil.UI2DB(this.tbOMinPower, portInfo.Min_power, 0);
						portInfo.Max_power = ThresholdUtil.UI2DB(this.tbOMaxPower, portInfo.Max_power, 0);
						portInfo.Min_power_diss = ThresholdUtil.UI2DB(this.tbOMinPowerDiss, portInfo.Min_power_diss, 0);
						portInfo.Max_power_diss = ThresholdUtil.UI2DB(this.tbOMaxPowerDiss, portInfo.Max_power_diss, 0);
						int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "port");
						outletThreshold.MinCurrentMt = portInfo.Min_current;
						outletThreshold.MaxCurrentMT = portInfo.Max_current;
						outletThreshold.MinVoltageMT = portInfo.Min_voltage;
						outletThreshold.MaxVoltageMT = portInfo.Max_voltage;
						outletThreshold.MinPowerMT = portInfo.Min_power;
						outletThreshold.MaxPowerMT = portInfo.Max_power;
						outletThreshold.MaxPowerDissMT = portInfo.Max_power_diss;
						ThresholdUtil.UI2Dev(thflg, outletThreshold);
					}
					DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
					DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
					if (devAccessAPI.SetOutletThreshold(outletThreshold, deviceByID.Mac))
					{
						if (portInfo != null)
						{
							portInfo.Update();
							EcoGlobalVar.setDashBoardFlg(128uL, string.Concat(new object[]
							{
								"#UPDATE#D",
								portInfo.DeviceID,
								":P",
								portInfo.ID,
								";"
							}), 2);
							string valuePair = ValuePairs.getValuePair("Username");
							if (!string.IsNullOrEmpty(valuePair))
							{
								LogAPI.writeEventLog("0630020", new string[]
								{
									portInfo.PortName,
									portInfo.PortNum.ToString(),
									deviceByID.DeviceIP,
									deviceByID.DeviceName,
									valuePair
								});
							}
							else
							{
								LogAPI.writeEventLog("0630020", new string[]
								{
									portInfo.PortName,
									portInfo.PortNum.ToString(),
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
				System.Console.WriteLine("PropPort Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private bool portCheck(DevModelConfig devcfg)
		{
			string text = this.tbOutletNm.Text.Trim();
			this.tbOutletNm.Text = text;
			string text2 = this.labOutletNo.Text;
			System.Convert.ToInt32(text2);
			bool flag = false;
			if (this.gbShutdownMethod.Visible)
			{
				flag = true;
				Ecovalidate.checkTextIsNull(this.tbOnDelayTime, ref flag);
				Ecovalidate.checkTextIsNull(this.tbOffDelayTime, ref flag);
				if (!flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbOnDelayTime.Text + "&" + this.lbOffDelayTime.Text
					}));
					return false;
				}
				int num = this.cbShutdownMethod.SelectedIndex + 1;
				if (num != 1 && this.tbMAC.Text.Length != 12)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidMAC, new string[0]));
					this.tbMAC.Focus();
					return false;
				}
			}
			if (this.gbThreshold.Visible)
			{
				flag = true;
				int num2 = devcfgUtil.UIThresholdEditFlg(devcfg, "port");
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
		private void tbOnDelayTime_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar >= '0' && keyChar <= '9')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void threshold_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(tb, e.KeyChar, 1);
			if (flag)
			{
				return;
			}
			e.Handled = true;
		}
		private void butOutletSetAssign_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			DBConn dBConn = null;
			string text = this.labDevModel.Tag.ToString();
			try
			{
				Program.IdleTimer_Pause(1);
				int num = System.Convert.ToInt32(text);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.portCheck(deviceModelConfig))
				{
					DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Dev_ApplyOutlet, new string[0]), MessageBoxButtons.OKCancel);
					if (dialogResult != DialogResult.Cancel)
					{
						string text2 = this.tbOutletNm.Text;
						string text3 = this.labOutletNo.Text;
						int num2 = System.Convert.ToInt32(text3);
						DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
						PortInfo portInfo = new PortInfo(num, num2);
						OutletThreshold outletThreshold = new OutletThreshold(1);
						if (this.gbShutdownMethod.Visible)
						{
							int outletShutdownMethod = this.cbShutdownMethod.SelectedIndex + 1;
							portInfo.OutletShutdownMethod = outletShutdownMethod;
							portInfo.OutletOnDelayTime = (this.tbOnDelayTime.Text.Equals(string.Empty) ? 0 : System.Convert.ToInt32(this.tbOnDelayTime.Text));
							portInfo.OutletOffDelayTime = (this.tbOffDelayTime.Text.Equals(string.Empty) ? 0 : System.Convert.ToInt32(this.tbOffDelayTime.Text));
							outletThreshold.OnDelayTime = portInfo.OutletOnDelayTime;
							outletThreshold.OffDelayTime = portInfo.OutletOffDelayTime;
							switch (outletShutdownMethod)
							{
							case 1:
								outletThreshold.ShutdownMethod = ShutDownMethod.KillThePower;
								outletThreshold.MacAddress = "000000000000";
								break;
							case 2:
								outletThreshold.ShutdownMethod = ShutDownMethod.WakeOnLan;
								outletThreshold.MacAddress = this.tbMAC.Text;
								break;
							case 3:
								outletThreshold.ShutdownMethod = ShutDownMethod.AfterAcBack;
								outletThreshold.MacAddress = this.tbMAC.Text;
								break;
							}
							portInfo.OutletMAC = outletThreshold.MacAddress;
						}
						if (this.gbThreshold.Visible)
						{
							portInfo.Min_current = ThresholdUtil.UI2DB(this.tbOMinCurrent, portInfo.Min_current, 0);
							portInfo.Max_current = ThresholdUtil.UI2DB(this.tbOMaxCurrent, portInfo.Max_current, 0);
							portInfo.Min_voltage = ThresholdUtil.UI2DB(this.tbOMinVoltage, portInfo.Min_voltage, 0);
							portInfo.Max_voltage = ThresholdUtil.UI2DB(this.tbOMaxVoltage, portInfo.Max_voltage, 0);
							portInfo.Min_power = ThresholdUtil.UI2DB(this.tbOMinPower, portInfo.Min_power, 0);
							portInfo.Max_power = ThresholdUtil.UI2DB(this.tbOMaxPower, portInfo.Max_power, 0);
							portInfo.Min_power_diss = ThresholdUtil.UI2DB(this.tbOMinPowerDiss, portInfo.Min_power_diss, 0);
							portInfo.Max_power_diss = ThresholdUtil.UI2DB(this.tbOMaxPowerDiss, portInfo.Max_power_diss, 0);
							int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "port");
							outletThreshold.MinCurrentMt = portInfo.Min_current;
							outletThreshold.MaxCurrentMT = portInfo.Max_current;
							outletThreshold.MinVoltageMT = portInfo.Min_voltage;
							outletThreshold.MaxVoltageMT = portInfo.Max_voltage;
							outletThreshold.MinPowerMT = portInfo.Min_power;
							outletThreshold.MaxPowerMT = portInfo.Max_power;
							outletThreshold.MaxPowerDissMT = portInfo.Max_power_diss;
							ThresholdUtil.UI2Dev(thflg, outletThreshold);
						}
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						string myMac = deviceByID.Mac;
						System.Collections.Generic.List<PortInfo> portInfo2 = deviceByID.GetPortInfo();
						dBConn = DBConnPool.getConnection();
						foreach (PortInfo current in portInfo2)
						{
							OutletThreshold outletThreshold2 = new OutletThreshold(current.PortNum);
							if (current.PortNum == num2)
							{
								current.PortName = text2;
							}
							outletThreshold2.OutletName = current.PortName;
							if (deviceModelConfig.isOutletSwitchable(current.PortNum - 1))
							{
								if (this.gbShutdownMethod.Visible)
								{
									outletThreshold2.ShutdownMethod = outletThreshold.ShutdownMethod;
									outletThreshold2.MacAddress = outletThreshold.MacAddress;
									outletThreshold2.OnDelayTime = outletThreshold.OnDelayTime;
									outletThreshold2.OffDelayTime = outletThreshold.OffDelayTime;
								}
								else
								{
									switch (current.OutletShutdownMethod)
									{
									case 1:
										outletThreshold2.ShutdownMethod = ShutDownMethod.KillThePower;
										break;
									case 2:
										outletThreshold2.ShutdownMethod = ShutDownMethod.WakeOnLan;
										break;
									case 3:
										outletThreshold2.ShutdownMethod = ShutDownMethod.AfterAcBack;
										break;
									}
									outletThreshold2.MacAddress = current.OutletMAC;
									outletThreshold2.OnDelayTime = current.OutletOnDelayTime;
									outletThreshold2.OffDelayTime = current.OutletOffDelayTime;
								}
							}
							if (this.gbThreshold.Visible)
							{
								float num3 = devcfgUtil.fmaxCurrent(deviceModelConfig, "port", current.PortNum);
								if (outletThreshold.MaxCurrentMT > num3)
								{
									outletThreshold2.MaxCurrentMT = num3;
								}
								else
								{
									outletThreshold2.MaxCurrentMT = outletThreshold.MaxCurrentMT;
								}
								outletThreshold2.MinCurrentMt = outletThreshold.MinCurrentMt;
								outletThreshold2.MaxPowerMT = outletThreshold.MaxPowerMT;
								outletThreshold2.MinPowerMT = outletThreshold.MinPowerMT;
								outletThreshold2.MaxVoltageMT = outletThreshold.MaxVoltageMT;
								outletThreshold2.MinVoltageMT = outletThreshold.MinVoltageMT;
								outletThreshold2.MaxPowerDissMT = outletThreshold.MaxPowerDissMT;
							}
							if (!devAccessAPI.SetOutletThreshold(outletThreshold2, myMac))
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
								return;
							}
							if (this.gbShutdownMethod.Visible && deviceModelConfig.isOutletSwitchable(current.PortNum - 1))
							{
								current.OutletShutdownMethod = portInfo.OutletShutdownMethod;
								current.OutletMAC = portInfo.OutletMAC;
								current.OutletOnDelayTime = portInfo.OutletOnDelayTime;
								current.OutletOffDelayTime = portInfo.OutletOffDelayTime;
							}
							if (this.gbThreshold.Visible)
							{
								float num4 = devcfgUtil.fmaxCurrent(deviceModelConfig, "port", current.PortNum);
								if (outletThreshold.MaxCurrentMT > num4)
								{
									current.Max_current = num4;
								}
								else
								{
									current.Max_current = portInfo.Max_current;
								}
								current.Min_current = portInfo.Min_current;
								current.Max_power = portInfo.Max_power;
								current.Min_power = portInfo.Min_power;
								current.Max_voltage = portInfo.Max_voltage;
								current.Min_voltage = portInfo.Min_voltage;
								current.Max_power_diss = portInfo.Max_power_diss;
								current.Min_power_diss = portInfo.Min_power_diss;
							}
							current.UpdatePortThreshold(dBConn);
							flag = true;
							myMac = "";
						}
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							LogAPI.writeEventLog("0630021", new string[]
							{
								deviceByID.ModelNm,
								deviceByID.DeviceIP,
								deviceByID.DeviceName,
								valuePair
							});
						}
						else
						{
							LogAPI.writeEventLog("0630021", new string[]
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
				System.Console.WriteLine("butOutletSetAssign_Click Exception" + ex.Message);
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
					EcoGlobalVar.setDashBoardFlg(128uL, "#UPDATE#D" + text + ":P*;", 2);
				}
				Program.IdleTimer_Run(1);
			}
		}
		private void cbShutdownMethod_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int num = this.cbShutdownMethod.SelectedIndex + 1;
			if (num == 2 || num == 3)
			{
				this.tbMAC.Enabled = true;
				return;
			}
			this.tbMAC.Enabled = false;
		}
		private void tbMAC_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar >= '0' && keyChar <= '9')
			{
				return;
			}
			if (keyChar == '.' || keyChar == '\b')
			{
				return;
			}
			if (keyChar == 'a' || keyChar == 'A' || keyChar == 'b' || keyChar == 'B' || keyChar == 'c' || keyChar == 'C' || keyChar == 'd' || keyChar == 'D' || keyChar == 'e' || keyChar == 'E' || keyChar == 'f' || keyChar == 'F')
			{
				e.KeyChar = char.ToUpper(keyChar);
				return;
			}
			e.Handled = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropOutlet));
			this.butOutletSetAssign = new Button();
			this.gbOutletConfig = new GroupBox();
			this.butOutlet39 = new Button();
			this.butOutlet36 = new Button();
			this.butOutlet35 = new Button();
			this.butOutlet40 = new Button();
			this.butOutlet34 = new Button();
			this.butOutlet29 = new Button();
			this.butOutlet42 = new Button();
			this.butOutlet33 = new Button();
			this.butOutlet41 = new Button();
			this.butOutlet30 = new Button();
			this.butOutlet37 = new Button();
			this.butOutlet32 = new Button();
			this.butOutlet31 = new Button();
			this.butOutlet38 = new Button();
			this.butOutlet27 = new Button();
			this.butOutlet28 = new Button();
			this.butOutlet26 = new Button();
			this.butOutlet25 = new Button();
			this.groupBox1 = new GroupBox();
			this.lbOutletNm = new Label();
			this.gbThreshold = new GroupBox();
			this.labMaxPowerDisBound = new Label();
			this.labMaxPowerBound = new Label();
			this.labMaxVoltageBound = new Label();
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
			this.gbShutdownMethod = new GroupBox();
			this.tbMAC = new TextBox();
			this.label1 = new Label();
			this.groupBox3 = new GroupBox();
			this.lbOffDelayTime = new Label();
			this.lbOnDelayTime = new Label();
			this.tbOffDelayTime = new TextBox();
			this.tbOnDelayTime = new TextBox();
			this.labShutdown = new Label();
			this.cbShutdownMethod = new ComboBox();
			this.tbOutletNm = new TextBox();
			this.butOutlet24 = new Button();
			this.butOutlet23 = new Button();
			this.butOutlet22 = new Button();
			this.butOutlet17 = new Button();
			this.butOutlet21 = new Button();
			this.butOutlet18 = new Button();
			this.butOutlet20 = new Button();
			this.butOutlet19 = new Button();
			this.groupBox2 = new GroupBox();
			this.label2 = new Label();
			this.label11 = new Label();
			this.label14 = new Label();
			this.labDevRackNm = new Label();
			this.labDevNm = new Label();
			this.label16 = new Label();
			this.labDevModel = new Label();
			this.labDevIp = new Label();
			this.labOutletNo = new Label();
			this.butOutlet15 = new Button();
			this.butOutlet16 = new Button();
			this.butOutlet14 = new Button();
			this.butOutlet13 = new Button();
			this.butOutlet12 = new Button();
			this.butOutlet7 = new Button();
			this.butOutlet11 = new Button();
			this.butOutlet8 = new Button();
			this.butOutlet10 = new Button();
			this.butOutlet6 = new Button();
			this.butOutlet9 = new Button();
			this.butOutlet5 = new Button();
			this.butOutlet4 = new Button();
			this.butOutlet3 = new Button();
			this.butOutlet2 = new Button();
			this.butOutlet1 = new Button();
			this.butPortSave = new Button();
			this.toolTip1 = new ToolTip(this.components);
			this.gbOutletConfig.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.gbThreshold.SuspendLayout();
			this.gbShutdownMethod.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.butOutletSetAssign.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butOutletSetAssign, "butOutletSetAssign");
			this.butOutletSetAssign.Name = "butOutletSetAssign";
			this.butOutletSetAssign.UseVisualStyleBackColor = false;
			this.butOutletSetAssign.Click += new System.EventHandler(this.butOutletSetAssign_Click);
			this.gbOutletConfig.Controls.Add(this.butOutlet39);
			this.gbOutletConfig.Controls.Add(this.butOutlet36);
			this.gbOutletConfig.Controls.Add(this.butOutlet35);
			this.gbOutletConfig.Controls.Add(this.butOutlet40);
			this.gbOutletConfig.Controls.Add(this.butOutlet34);
			this.gbOutletConfig.Controls.Add(this.butOutlet29);
			this.gbOutletConfig.Controls.Add(this.butOutlet42);
			this.gbOutletConfig.Controls.Add(this.butOutlet33);
			this.gbOutletConfig.Controls.Add(this.butOutlet41);
			this.gbOutletConfig.Controls.Add(this.butOutlet30);
			this.gbOutletConfig.Controls.Add(this.butOutlet37);
			this.gbOutletConfig.Controls.Add(this.butOutlet32);
			this.gbOutletConfig.Controls.Add(this.butOutlet31);
			this.gbOutletConfig.Controls.Add(this.butOutlet38);
			this.gbOutletConfig.Controls.Add(this.butOutlet27);
			this.gbOutletConfig.Controls.Add(this.butOutlet28);
			this.gbOutletConfig.Controls.Add(this.butOutlet26);
			this.gbOutletConfig.Controls.Add(this.butOutlet25);
			this.gbOutletConfig.Controls.Add(this.groupBox1);
			this.gbOutletConfig.Controls.Add(this.butOutlet24);
			this.gbOutletConfig.Controls.Add(this.butOutlet23);
			this.gbOutletConfig.Controls.Add(this.butOutlet22);
			this.gbOutletConfig.Controls.Add(this.butOutlet17);
			this.gbOutletConfig.Controls.Add(this.butOutlet21);
			this.gbOutletConfig.Controls.Add(this.butOutlet18);
			this.gbOutletConfig.Controls.Add(this.butOutlet20);
			this.gbOutletConfig.Controls.Add(this.butOutlet19);
			this.gbOutletConfig.Controls.Add(this.groupBox2);
			this.gbOutletConfig.Controls.Add(this.labOutletNo);
			this.gbOutletConfig.Controls.Add(this.butOutlet15);
			this.gbOutletConfig.Controls.Add(this.butOutlet16);
			this.gbOutletConfig.Controls.Add(this.butOutlet14);
			this.gbOutletConfig.Controls.Add(this.butOutlet13);
			this.gbOutletConfig.Controls.Add(this.butOutlet12);
			this.gbOutletConfig.Controls.Add(this.butOutlet7);
			this.gbOutletConfig.Controls.Add(this.butOutlet11);
			this.gbOutletConfig.Controls.Add(this.butOutlet8);
			this.gbOutletConfig.Controls.Add(this.butOutlet10);
			this.gbOutletConfig.Controls.Add(this.butOutlet6);
			this.gbOutletConfig.Controls.Add(this.butOutlet9);
			this.gbOutletConfig.Controls.Add(this.butOutlet5);
			this.gbOutletConfig.Controls.Add(this.butOutlet4);
			this.gbOutletConfig.Controls.Add(this.butOutlet3);
			this.gbOutletConfig.Controls.Add(this.butOutlet2);
			this.gbOutletConfig.Controls.Add(this.butOutlet1);
			componentResourceManager.ApplyResources(this.gbOutletConfig, "gbOutletConfig");
			this.gbOutletConfig.ForeColor = SystemColors.ControlText;
			this.gbOutletConfig.Name = "gbOutletConfig";
			this.gbOutletConfig.TabStop = false;
			this.butOutlet39.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet39, "butOutlet39");
			this.butOutlet39.ForeColor = Color.Black;
			this.butOutlet39.Name = "butOutlet39";
			this.butOutlet39.Tag = "39";
			this.butOutlet39.UseVisualStyleBackColor = false;
			this.butOutlet39.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet36.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet36, "butOutlet36");
			this.butOutlet36.ForeColor = Color.Black;
			this.butOutlet36.Name = "butOutlet36";
			this.butOutlet36.Tag = "36";
			this.butOutlet36.UseVisualStyleBackColor = false;
			this.butOutlet36.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet35.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet35, "butOutlet35");
			this.butOutlet35.ForeColor = Color.Black;
			this.butOutlet35.Name = "butOutlet35";
			this.butOutlet35.Tag = "35";
			this.butOutlet35.UseVisualStyleBackColor = false;
			this.butOutlet35.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet40.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet40, "butOutlet40");
			this.butOutlet40.ForeColor = Color.Black;
			this.butOutlet40.Name = "butOutlet40";
			this.butOutlet40.Tag = "40";
			this.butOutlet40.UseVisualStyleBackColor = false;
			this.butOutlet40.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet34.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet34, "butOutlet34");
			this.butOutlet34.ForeColor = Color.Black;
			this.butOutlet34.Name = "butOutlet34";
			this.butOutlet34.Tag = "34";
			this.butOutlet34.UseVisualStyleBackColor = false;
			this.butOutlet34.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet29.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet29, "butOutlet29");
			this.butOutlet29.ForeColor = Color.Black;
			this.butOutlet29.Name = "butOutlet29";
			this.butOutlet29.Tag = "29";
			this.butOutlet29.UseVisualStyleBackColor = false;
			this.butOutlet29.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet42.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet42, "butOutlet42");
			this.butOutlet42.ForeColor = Color.Black;
			this.butOutlet42.Name = "butOutlet42";
			this.butOutlet42.Tag = "42";
			this.butOutlet42.UseVisualStyleBackColor = false;
			this.butOutlet42.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet33.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet33, "butOutlet33");
			this.butOutlet33.ForeColor = Color.Black;
			this.butOutlet33.Name = "butOutlet33";
			this.butOutlet33.Tag = "33";
			this.butOutlet33.UseVisualStyleBackColor = false;
			this.butOutlet33.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet41.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet41, "butOutlet41");
			this.butOutlet41.ForeColor = Color.Black;
			this.butOutlet41.Name = "butOutlet41";
			this.butOutlet41.Tag = "41";
			this.butOutlet41.UseVisualStyleBackColor = false;
			this.butOutlet41.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet30.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet30, "butOutlet30");
			this.butOutlet30.ForeColor = Color.Black;
			this.butOutlet30.Name = "butOutlet30";
			this.butOutlet30.Tag = "30";
			this.butOutlet30.UseVisualStyleBackColor = false;
			this.butOutlet30.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet37.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet37, "butOutlet37");
			this.butOutlet37.ForeColor = Color.Black;
			this.butOutlet37.Name = "butOutlet37";
			this.butOutlet37.Tag = "37";
			this.butOutlet37.UseVisualStyleBackColor = false;
			this.butOutlet37.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet32.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet32, "butOutlet32");
			this.butOutlet32.ForeColor = Color.Black;
			this.butOutlet32.Name = "butOutlet32";
			this.butOutlet32.Tag = "32";
			this.butOutlet32.UseVisualStyleBackColor = false;
			this.butOutlet32.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet31.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet31, "butOutlet31");
			this.butOutlet31.ForeColor = Color.Black;
			this.butOutlet31.Name = "butOutlet31";
			this.butOutlet31.Tag = "31";
			this.butOutlet31.UseVisualStyleBackColor = false;
			this.butOutlet31.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet38.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet38, "butOutlet38");
			this.butOutlet38.ForeColor = Color.Black;
			this.butOutlet38.Name = "butOutlet38";
			this.butOutlet38.Tag = "38";
			this.butOutlet38.UseVisualStyleBackColor = false;
			this.butOutlet38.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet27.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet27, "butOutlet27");
			this.butOutlet27.ForeColor = Color.Black;
			this.butOutlet27.Name = "butOutlet27";
			this.butOutlet27.Tag = "27";
			this.butOutlet27.UseVisualStyleBackColor = false;
			this.butOutlet27.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet28.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet28, "butOutlet28");
			this.butOutlet28.ForeColor = Color.Black;
			this.butOutlet28.Name = "butOutlet28";
			this.butOutlet28.Tag = "28";
			this.butOutlet28.UseVisualStyleBackColor = false;
			this.butOutlet28.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet26.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet26, "butOutlet26");
			this.butOutlet26.ForeColor = Color.Black;
			this.butOutlet26.Name = "butOutlet26";
			this.butOutlet26.Tag = "26";
			this.butOutlet26.UseVisualStyleBackColor = false;
			this.butOutlet26.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet25.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet25, "butOutlet25");
			this.butOutlet25.ForeColor = Color.Black;
			this.butOutlet25.Name = "butOutlet25";
			this.butOutlet25.Tag = "25";
			this.butOutlet25.UseVisualStyleBackColor = false;
			this.butOutlet25.Click += new System.EventHandler(this.butOutlet_Click);
			this.groupBox1.Controls.Add(this.lbOutletNm);
			this.groupBox1.Controls.Add(this.gbThreshold);
			this.groupBox1.Controls.Add(this.gbShutdownMethod);
			this.groupBox1.Controls.Add(this.tbOutletNm);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.lbOutletNm, "lbOutletNm");
			this.lbOutletNm.ForeColor = Color.Black;
			this.lbOutletNm.Name = "lbOutletNm";
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
			componentResourceManager.ApplyResources(this.labMaxPowerDisBound, "labMaxPowerDisBound");
			this.labMaxPowerDisBound.ForeColor = Color.Red;
			this.labMaxPowerDisBound.Name = "labMaxPowerDisBound";
			componentResourceManager.ApplyResources(this.labMaxPowerBound, "labMaxPowerBound");
			this.labMaxPowerBound.ForeColor = Color.Red;
			this.labMaxPowerBound.Name = "labMaxPowerBound";
			componentResourceManager.ApplyResources(this.labMaxVoltageBound, "labMaxVoltageBound");
			this.labMaxVoltageBound.ForeColor = Color.Red;
			this.labMaxVoltageBound.Name = "labMaxVoltageBound";
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
			this.tbOMinCurrent.Tag = "ocurrent";
			this.tbOMinCurrent.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.tbOMaxCurrent, "tbOMaxCurrent");
			this.tbOMaxCurrent.ForeColor = Color.Black;
			this.tbOMaxCurrent.Name = "tbOMaxCurrent";
			this.tbOMaxCurrent.Tag = "ocurrent";
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
			this.tbOMinVoltage.Tag = "ovoltage";
			this.tbOMinVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label175, "label175");
			this.label175.ForeColor = Color.Black;
			this.label175.Name = "label175";
			componentResourceManager.ApplyResources(this.tbOMaxVoltage, "tbOMaxVoltage");
			this.tbOMaxVoltage.ForeColor = Color.Black;
			this.tbOMaxVoltage.Name = "tbOMaxVoltage";
			this.tbOMaxVoltage.Tag = "ovoltage";
			this.tbOMaxVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.tbOMaxPowerDiss, "tbOMaxPowerDiss");
			this.tbOMaxPowerDiss.ForeColor = Color.Black;
			this.tbOMaxPowerDiss.Name = "tbOMaxPowerDiss";
			this.tbOMaxPowerDiss.Tag = "opowerDiss";
			this.tbOMaxPowerDiss.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label169, "label169");
			this.label169.ForeColor = Color.Black;
			this.label169.Name = "label169";
			this.tbOMinPowerDiss.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbOMinPowerDiss, "tbOMinPowerDiss");
			this.tbOMinPowerDiss.ForeColor = Color.Black;
			this.tbOMinPowerDiss.Name = "tbOMinPowerDiss";
			this.tbOMinPowerDiss.ReadOnly = true;
			this.tbOMinPowerDiss.Tag = "opowerDiss";
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
			this.tbOMinPower.Tag = "opower";
			this.tbOMinPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label172, "label172");
			this.label172.ForeColor = Color.Black;
			this.label172.Name = "label172";
			componentResourceManager.ApplyResources(this.tbOMaxPower, "tbOMaxPower");
			this.tbOMaxPower.ForeColor = Color.Black;
			this.tbOMaxPower.Name = "tbOMaxPower";
			this.tbOMaxPower.Tag = "opower";
			this.tbOMaxPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.gbShutdownMethod.Controls.Add(this.tbMAC);
			this.gbShutdownMethod.Controls.Add(this.label1);
			this.gbShutdownMethod.Controls.Add(this.groupBox3);
			this.gbShutdownMethod.Controls.Add(this.labShutdown);
			this.gbShutdownMethod.Controls.Add(this.cbShutdownMethod);
			componentResourceManager.ApplyResources(this.gbShutdownMethod, "gbShutdownMethod");
			this.gbShutdownMethod.Name = "gbShutdownMethod";
			this.gbShutdownMethod.TabStop = false;
			componentResourceManager.ApplyResources(this.tbMAC, "tbMAC");
			this.tbMAC.ForeColor = Color.Black;
			this.tbMAC.Name = "tbMAC";
			this.tbMAC.Tag = "";
			this.tbMAC.KeyPress += new KeyPressEventHandler(this.tbMAC_KeyPress);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Black;
			this.label1.Name = "label1";
			this.groupBox3.Controls.Add(this.lbOffDelayTime);
			this.groupBox3.Controls.Add(this.lbOnDelayTime);
			this.groupBox3.Controls.Add(this.tbOffDelayTime);
			this.groupBox3.Controls.Add(this.tbOnDelayTime);
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.lbOffDelayTime, "lbOffDelayTime");
			this.lbOffDelayTime.ForeColor = Color.Black;
			this.lbOffDelayTime.Name = "lbOffDelayTime";
			componentResourceManager.ApplyResources(this.lbOnDelayTime, "lbOnDelayTime");
			this.lbOnDelayTime.ForeColor = Color.Black;
			this.lbOnDelayTime.Name = "lbOnDelayTime";
			componentResourceManager.ApplyResources(this.tbOffDelayTime, "tbOffDelayTime");
			this.tbOffDelayTime.ForeColor = Color.Black;
			this.tbOffDelayTime.Name = "tbOffDelayTime";
			this.tbOffDelayTime.KeyPress += new KeyPressEventHandler(this.tbOnDelayTime_KeyPress);
			componentResourceManager.ApplyResources(this.tbOnDelayTime, "tbOnDelayTime");
			this.tbOnDelayTime.ForeColor = Color.Black;
			this.tbOnDelayTime.Name = "tbOnDelayTime";
			this.tbOnDelayTime.KeyPress += new KeyPressEventHandler(this.tbOnDelayTime_KeyPress);
			componentResourceManager.ApplyResources(this.labShutdown, "labShutdown");
			this.labShutdown.ForeColor = Color.Black;
			this.labShutdown.Name = "labShutdown";
			this.cbShutdownMethod.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbShutdownMethod, "cbShutdownMethod");
			this.cbShutdownMethod.ForeColor = Color.Black;
			this.cbShutdownMethod.FormattingEnabled = true;
			this.cbShutdownMethod.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbShutdownMethod.Items"),
				componentResourceManager.GetString("cbShutdownMethod.Items1"),
				componentResourceManager.GetString("cbShutdownMethod.Items2")
			});
			this.cbShutdownMethod.Name = "cbShutdownMethod";
			this.cbShutdownMethod.SelectedIndexChanged += new System.EventHandler(this.cbShutdownMethod_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.tbOutletNm, "tbOutletNm");
			this.tbOutletNm.ForeColor = Color.Black;
			this.tbOutletNm.Name = "tbOutletNm";
			this.tbOutletNm.KeyPress += new KeyPressEventHandler(this.tbOutletNm_KeyPress);
			this.butOutlet24.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet24, "butOutlet24");
			this.butOutlet24.ForeColor = Color.Black;
			this.butOutlet24.Name = "butOutlet24";
			this.butOutlet24.Tag = "24";
			this.butOutlet24.UseVisualStyleBackColor = false;
			this.butOutlet24.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet23.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet23, "butOutlet23");
			this.butOutlet23.ForeColor = Color.Black;
			this.butOutlet23.Name = "butOutlet23";
			this.butOutlet23.Tag = "23";
			this.butOutlet23.UseVisualStyleBackColor = false;
			this.butOutlet23.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet22.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet22, "butOutlet22");
			this.butOutlet22.ForeColor = Color.Black;
			this.butOutlet22.Name = "butOutlet22";
			this.butOutlet22.Tag = "22";
			this.butOutlet22.UseVisualStyleBackColor = false;
			this.butOutlet22.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet17.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet17, "butOutlet17");
			this.butOutlet17.ForeColor = Color.Black;
			this.butOutlet17.Name = "butOutlet17";
			this.butOutlet17.Tag = "17";
			this.butOutlet17.UseVisualStyleBackColor = false;
			this.butOutlet17.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet21.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet21, "butOutlet21");
			this.butOutlet21.ForeColor = Color.Black;
			this.butOutlet21.Name = "butOutlet21";
			this.butOutlet21.Tag = "21";
			this.butOutlet21.UseVisualStyleBackColor = false;
			this.butOutlet21.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet18.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet18, "butOutlet18");
			this.butOutlet18.ForeColor = Color.Black;
			this.butOutlet18.Name = "butOutlet18";
			this.butOutlet18.Tag = "18";
			this.butOutlet18.UseVisualStyleBackColor = false;
			this.butOutlet18.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet20.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet20, "butOutlet20");
			this.butOutlet20.ForeColor = Color.Black;
			this.butOutlet20.Name = "butOutlet20";
			this.butOutlet20.Tag = "20";
			this.butOutlet20.UseVisualStyleBackColor = false;
			this.butOutlet20.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet19.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet19, "butOutlet19");
			this.butOutlet19.ForeColor = Color.Black;
			this.butOutlet19.Name = "butOutlet19";
			this.butOutlet19.Tag = "19";
			this.butOutlet19.UseVisualStyleBackColor = false;
			this.butOutlet19.Click += new System.EventHandler(this.butOutlet_Click);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.labDevRackNm);
			this.groupBox2.Controls.Add(this.labDevNm);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.labDevModel);
			this.groupBox2.Controls.Add(this.labDevIp);
			this.groupBox2.ForeColor = SystemColors.ControlText;
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = Color.Black;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.ForeColor = Color.Black;
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.ForeColor = Color.Black;
			this.label14.Name = "label14";
			this.labDevRackNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevRackNm, "labDevRackNm");
			this.labDevRackNm.ForeColor = Color.Black;
			this.labDevRackNm.Name = "labDevRackNm";
			this.labDevNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevNm, "labDevNm");
			this.labDevNm.ForeColor = Color.Black;
			this.labDevNm.Name = "labDevNm";
			componentResourceManager.ApplyResources(this.label16, "label16");
			this.label16.ForeColor = Color.Black;
			this.label16.Name = "label16";
			this.labDevModel.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevModel, "labDevModel");
			this.labDevModel.ForeColor = Color.Black;
			this.labDevModel.Name = "labDevModel";
			this.labDevIp.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevIp, "labDevIp");
			this.labDevIp.ForeColor = Color.Black;
			this.labDevIp.Name = "labDevIp";
			componentResourceManager.ApplyResources(this.labOutletNo, "labOutletNo");
			this.labOutletNo.ForeColor = Color.Black;
			this.labOutletNo.Name = "labOutletNo";
			this.butOutlet15.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet15, "butOutlet15");
			this.butOutlet15.ForeColor = Color.Black;
			this.butOutlet15.Name = "butOutlet15";
			this.butOutlet15.Tag = "15";
			this.butOutlet15.UseVisualStyleBackColor = false;
			this.butOutlet15.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet16.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet16, "butOutlet16");
			this.butOutlet16.ForeColor = Color.Black;
			this.butOutlet16.Name = "butOutlet16";
			this.butOutlet16.Tag = "16";
			this.butOutlet16.UseVisualStyleBackColor = false;
			this.butOutlet16.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet14.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet14, "butOutlet14");
			this.butOutlet14.ForeColor = Color.Black;
			this.butOutlet14.Name = "butOutlet14";
			this.butOutlet14.Tag = "14";
			this.butOutlet14.UseVisualStyleBackColor = false;
			this.butOutlet14.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet13.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet13, "butOutlet13");
			this.butOutlet13.ForeColor = Color.Black;
			this.butOutlet13.Name = "butOutlet13";
			this.butOutlet13.Tag = "13";
			this.butOutlet13.UseVisualStyleBackColor = false;
			this.butOutlet13.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet12.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet12, "butOutlet12");
			this.butOutlet12.ForeColor = Color.Black;
			this.butOutlet12.Name = "butOutlet12";
			this.butOutlet12.Tag = "12";
			this.butOutlet12.UseVisualStyleBackColor = false;
			this.butOutlet12.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet7.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet7, "butOutlet7");
			this.butOutlet7.ForeColor = Color.Black;
			this.butOutlet7.Name = "butOutlet7";
			this.butOutlet7.Tag = "7";
			this.butOutlet7.UseVisualStyleBackColor = false;
			this.butOutlet7.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet11.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet11, "butOutlet11");
			this.butOutlet11.ForeColor = Color.Black;
			this.butOutlet11.Name = "butOutlet11";
			this.butOutlet11.Tag = "11";
			this.butOutlet11.UseVisualStyleBackColor = false;
			this.butOutlet11.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet8.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet8, "butOutlet8");
			this.butOutlet8.ForeColor = Color.Black;
			this.butOutlet8.Name = "butOutlet8";
			this.butOutlet8.Tag = "8";
			this.butOutlet8.UseVisualStyleBackColor = false;
			this.butOutlet8.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet10.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet10, "butOutlet10");
			this.butOutlet10.ForeColor = Color.Black;
			this.butOutlet10.Name = "butOutlet10";
			this.butOutlet10.Tag = "10";
			this.butOutlet10.UseVisualStyleBackColor = false;
			this.butOutlet10.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet6.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet6, "butOutlet6");
			this.butOutlet6.ForeColor = Color.Black;
			this.butOutlet6.Name = "butOutlet6";
			this.butOutlet6.Tag = "6";
			this.butOutlet6.UseVisualStyleBackColor = false;
			this.butOutlet6.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet9.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet9, "butOutlet9");
			this.butOutlet9.ForeColor = Color.Black;
			this.butOutlet9.Name = "butOutlet9";
			this.butOutlet9.Tag = "9";
			this.butOutlet9.UseVisualStyleBackColor = false;
			this.butOutlet9.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet5.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet5, "butOutlet5");
			this.butOutlet5.ForeColor = Color.Black;
			this.butOutlet5.Name = "butOutlet5";
			this.butOutlet5.Tag = "5";
			this.butOutlet5.UseVisualStyleBackColor = false;
			this.butOutlet5.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet4.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet4, "butOutlet4");
			this.butOutlet4.ForeColor = Color.Black;
			this.butOutlet4.Name = "butOutlet4";
			this.butOutlet4.Tag = "4";
			this.butOutlet4.UseVisualStyleBackColor = false;
			this.butOutlet4.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet3.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet3, "butOutlet3");
			this.butOutlet3.ForeColor = Color.Black;
			this.butOutlet3.Name = "butOutlet3";
			this.butOutlet3.Tag = "3";
			this.butOutlet3.UseVisualStyleBackColor = false;
			this.butOutlet3.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet2.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet2, "butOutlet2");
			this.butOutlet2.ForeColor = Color.Black;
			this.butOutlet2.Name = "butOutlet2";
			this.butOutlet2.Tag = "2";
			this.butOutlet2.UseVisualStyleBackColor = false;
			this.butOutlet2.Click += new System.EventHandler(this.butOutlet_Click);
			this.butOutlet1.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butOutlet1, "butOutlet1");
			this.butOutlet1.ForeColor = Color.Black;
			this.butOutlet1.Image = Resources.tree_outletoff;
			this.butOutlet1.Name = "butOutlet1";
			this.butOutlet1.Tag = "1";
			this.butOutlet1.UseVisualStyleBackColor = false;
			this.butOutlet1.Click += new System.EventHandler(this.butOutlet_Click);
			this.butPortSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butPortSave, "butPortSave");
			this.butPortSave.Name = "butPortSave";
			this.butPortSave.UseVisualStyleBackColor = false;
			this.butPortSave.Click += new System.EventHandler(this.butPortSave_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butOutletSetAssign);
			base.Controls.Add(this.gbOutletConfig);
			base.Controls.Add(this.butPortSave);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropOutlet";
			this.gbOutletConfig.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.gbThreshold.ResumeLayout(false);
			this.gbThreshold.PerformLayout();
			this.gbShutdownMethod.ResumeLayout(false);
			this.gbShutdownMethod.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
