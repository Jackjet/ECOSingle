using CommonAPI.Global;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice._Dev
{
	public class PropDev1 : UserControl
	{
		private DevManDevice m_pParent;
		private bool m_onlinest;
		private Combobox_item m_snmpV1Comb = new Combobox_item(1.ToString(), "v1");
		private Combobox_item m_snmpV2Comb = new Combobox_item(2.ToString(), "v2c");
		private Combobox_item m_snmpV3Comb = new Combobox_item(3.ToString(), "v3");
		private IContainer components;
		private GroupBox groupBox4;
		private GroupBox gBoxSensors;
		private ComboBox cbSS4;
		private Label lbSS4;
		private ComboBox cbSS3;
		private Label lbSS3;
		private ComboBox cbSS2;
		private Label lbSS2;
		private ComboBox cbSS1;
		private Label lbSS1;
		private GroupBox groupBox3;
		private TextBox tbDevNm;
		private Label lbDevNm;
		private Label label36;
		private Label labDevModel;
		private ComboBox cbDevRack;
		private TextBox tbIP;
		private Label label27;
		private Label label28;
		private GroupBox groupBox1;
		private ComboBox cbSnmpV;
		private TextBox tbSysUserNm;
		private Label lbUsername;
		private Label label6;
		private Label lbSysPort;
		private Label label8;
		private TextBox tbSysPort;
		private Label lbtimeout;
		private TextBox tbSysRetry;
		private TextBox tbSysTimeOut;
		private Label lbSysRetry;
		private Label lbSysPrivacyPw;
		private ComboBox cbSysAuthen;
		private TextBox tbSysPrivacyPw;
		private ComboBox cbSysPrivacy;
		private Label lbSysAuthenPw;
		private Label labAuthen;
		private TextBox tbSysAuthenPw;
		private Label labPrivacy;
		private Button butSave;
		private ToolTip toolTip1;
		private Button butDevicesDel;
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public PropDev1()
		{
			this.InitializeComponent();
			this.tbDevNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbIP.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysUserNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysTimeOut.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysRetry.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.cbSnmpV.Items.Clear();
			this.cbSnmpV.Items.Add(this.m_snmpV1Comb);
			this.cbSnmpV.Items.Add(this.m_snmpV2Comb);
			this.cbSnmpV.Items.Add(this.m_snmpV3Comb);
			this.cbSysAuthen.Items.Clear();
			this.cbSysAuthen.Items.Add("MD5");
			this.cbSysAuthen.Items.Add("SHA");
			this.cbSysAuthen.Items.Add("None");
			this.cbSysPrivacy.Items.Clear();
			this.cbSysPrivacy.Items.Add("AES");
			this.cbSysPrivacy.Items.Add("DES");
			this.cbSysPrivacy.Items.Add("None");
		}
		public void pageInit(DevManDevice pParent, int devID, bool onlinest)
		{
			this.m_pParent = pParent;
			this.m_onlinest = onlinest;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID);
			this.tbDevNm.Text = deviceByID.DeviceName;
			if (!onlinest)
			{
				this.tbDevNm.BackColor = Color.WhiteSmoke;
				this.tbDevNm.ReadOnly = true;
			}
			else
			{
				this.tbDevNm.BackColor = Color.White;
				this.tbDevNm.ReadOnly = false;
			}
			this.labDevModel.Text = deviceByID.ModelNm;
			this.labDevModel.Tag = devID.ToString();
			string text = deviceByID.ModelNm;
			if (DevAccessCfg.GetInstance().isAutodectDev(deviceByID.ModelNm, deviceByID.FWVersion))
			{
				text = text + " (F/W: " + deviceByID.FWVersion + ")";
			}
			this.toolTip1.SetToolTip(this.labDevModel, text);
			RackInfo rackInfo = RackInfo.getRackByID(deviceByID.RackID);
			string displayRackName = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			System.Collections.ArrayList allRack = RackInfo.getAllRack();
			this.cbDevRack.Items.Clear();
			foreach (object current in allRack)
			{
				rackInfo = (RackInfo)current;
				Combobox_item combobox_item = new Combobox_item(rackInfo.RackID.ToString(), rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag));
				this.cbDevRack.Items.Add(combobox_item);
				if (displayRackName.Equals(rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag)))
				{
					this.cbDevRack.SelectedItem = combobox_item;
				}
			}
			this.cbDevRack.BackColor = Color.White;
			this.tbSysUserNm.Text = deviceByID.Username;
			this.tbSysUserNm.BackColor = Color.White;
			this.tbIP.Text = deviceByID.DeviceIP;
			this.tbIP.BackColor = Color.White;
			this.tbSysPort.Text = deviceByID.Port.ToString();
			this.tbSysPort.BackColor = Color.White;
			this.tbSysTimeOut.Text = deviceByID.Timeout.ToString();
			this.tbSysTimeOut.BackColor = Color.White;
			this.tbSysRetry.Text = deviceByID.Retry.ToString();
			this.tbSysRetry.BackColor = Color.White;
			this.cbSnmpV.BackColor = Color.White;
			switch (deviceByID.SnmpVersion)
			{
			case 1:
				this.cbSnmpV.SelectedItem = this.m_snmpV1Comb;
				this.showV3setting(false);
				this.cbSysPrivacy.SelectedItem = "None";
				this.tbSysPrivacyPw.Text = string.Empty;
				this.cbSysAuthen.SelectedItem = "None";
				this.tbSysAuthenPw.Text = string.Empty;
				break;
			case 2:
				this.cbSnmpV.SelectedItem = this.m_snmpV2Comb;
				this.showV3setting(false);
				this.cbSysPrivacy.SelectedItem = "None";
				this.tbSysPrivacyPw.Text = string.Empty;
				this.cbSysAuthen.SelectedItem = "None";
				this.tbSysAuthenPw.Text = string.Empty;
				break;
			case 3:
				this.cbSnmpV.SelectedItem = this.m_snmpV3Comb;
				this.showV3setting(true);
				this.cbSysPrivacy.SelectedItem = deviceByID.Privacy;
				this.tbSysPrivacyPw.Text = deviceByID.PrivacyPassword;
				this.cbSysAuthen.SelectedItem = deviceByID.Authentication;
				this.tbSysAuthenPw.Text = deviceByID.AuthenPassword;
				if (deviceByID.Authentication.Equals("None"))
				{
					this.tbSysAuthenPw.Enabled = false;
					this.cbSysPrivacy.SelectedItem = "None";
					this.cbSysPrivacy.Enabled = false;
					this.tbSysPrivacyPw.Enabled = false;
				}
				else
				{
					this.tbSysAuthenPw.Enabled = true;
					if (deviceByID.Privacy.Equals("None"))
					{
						this.tbSysPrivacyPw.Enabled = false;
					}
					else
					{
						this.tbSysPrivacyPw.Enabled = true;
					}
				}
				break;
			}
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
			if (deviceModelConfig.sensorNum == 0)
			{
				this.gBoxSensors.Visible = false;
				return;
			}
			this.gBoxSensors.Visible = true;
			System.Collections.Generic.List<SensorInfo> sensorInfo = deviceByID.GetSensorInfo();
			this.cbSS1.BackColor = Color.White;
			this.cbSS2.BackColor = Color.White;
			this.cbSS3.BackColor = Color.White;
			this.cbSS4.BackColor = Color.White;
			if (deviceModelConfig.sensorNum == 1)
			{
				this.lbSS2.Visible = false;
				this.cbSS2.Visible = false;
				this.lbSS3.Visible = false;
				this.cbSS3.Visible = false;
				this.lbSS4.Visible = false;
				this.cbSS4.Visible = false;
				this.cbSS1.Items.Clear();
				this.cbSS1.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]));
				this.cbSS1.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0]));
			}
			else
			{
				if (deviceModelConfig.sensorNum == 2)
				{
					this.lbSS2.Visible = true;
					this.cbSS2.Visible = true;
					this.lbSS3.Visible = false;
					this.cbSS3.Visible = false;
					this.lbSS4.Visible = false;
					this.cbSS4.Visible = false;
					this.cbSS1.Items.Clear();
					this.cbSS1.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]));
					this.cbSS2.Items.Clear();
					this.cbSS2.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0]));
					this.cbSS2.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Floor, new string[0]));
				}
				else
				{
					this.lbSS2.Visible = true;
					this.cbSS2.Visible = true;
					this.lbSS3.Visible = true;
					this.cbSS3.Visible = true;
					this.lbSS4.Visible = true;
					this.cbSS4.Visible = true;
					this.cbSS1.Items.Clear();
					this.cbSS1.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]));
					this.cbSS2.Items.Clear();
					this.cbSS2.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]));
					this.cbSS3.Items.Clear();
					this.cbSS3.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0]));
					this.cbSS3.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Floor, new string[0]));
					this.cbSS4.Items.Clear();
					this.cbSS4.Items.Add(EcoLanguage.getMsg(LangRes.Sensor_Loct_Floor, new string[0]));
				}
			}
			foreach (SensorInfo current2 in sensorInfo)
			{
				switch (current2.Type)
				{
				case 1:
					if (current2.Location == 0)
					{
						this.cbSS1.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]);
					}
					else
					{
						if (current2.Location == 1)
						{
							this.cbSS1.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0]);
						}
						else
						{
							this.cbSS1.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Floor, new string[0]);
						}
					}
					break;
				case 2:
					if (current2.Location == 0)
					{
						this.cbSS2.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]);
					}
					else
					{
						if (current2.Location == 1)
						{
							this.cbSS2.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0]);
						}
						else
						{
							this.cbSS2.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Floor, new string[0]);
						}
					}
					break;
				case 3:
					if (current2.Location == 0)
					{
						this.cbSS3.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]);
					}
					else
					{
						if (current2.Location == 1)
						{
							this.cbSS3.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0]);
						}
						else
						{
							this.cbSS3.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Floor, new string[0]);
						}
					}
					break;
				case 4:
					if (current2.Location == 0)
					{
						this.cbSS4.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0]);
					}
					else
					{
						if (current2.Location == 1)
						{
							this.cbSS4.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0]);
						}
						else
						{
							this.cbSS4.SelectedItem = EcoLanguage.getMsg(LangRes.Sensor_Loct_Floor, new string[0]);
						}
					}
					break;
				}
			}
		}
		public void TimerProc(bool onlinest, int haveThresholdChange)
		{
			if (!onlinest)
			{
				this.tbDevNm.BackColor = Color.WhiteSmoke;
				this.tbDevNm.ReadOnly = true;
			}
			else
			{
				this.tbDevNm.BackColor = Color.White;
				this.tbDevNm.ReadOnly = false;
			}
			if (haveThresholdChange == 1)
			{
				string value = this.labDevModel.Tag.ToString();
				int l_id = System.Convert.ToInt32(value);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
				string text = this.tbDevNm.Text;
				if (!text.Equals(deviceByID.DeviceName))
				{
					this.tbDevNm.Text = deviceByID.DeviceName;
				}
			}
		}
		private void showV3setting(bool disp)
		{
			this.tbSysPrivacyPw.Enabled = disp;
			this.tbSysAuthenPw.Enabled = disp;
		}
		private void cbSnmpV_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.cbSnmpV.SelectedIndex == 0 || this.cbSnmpV.SelectedIndex == 1)
			{
				this.showV3setting(false);
				this.cbSysAuthen.SelectedItem = "None";
				this.cbSysPrivacy.SelectedItem = "None";
				return;
			}
			this.showV3setting(true);
			this.cbSysAuthen.SelectedItem = "MD5";
			this.cbSysPrivacy.SelectedItem = "AES";
		}
		private void digit_KeyPress(object sender, KeyPressEventArgs e)
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
		private void tbSysUserNm_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == ' ' || keyChar == '_' || keyChar == '\'')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void tbDevNm_KeyPress(object sender, KeyPressEventArgs e)
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
		private void tbIP_KeyPress(object sender, KeyPressEventArgs e)
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
			if (char.IsPunctuation(keyChar))
			{
				return;
			}
			e.Handled = true;
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			try
			{
				if (this.devConfigCheck())
				{
					DeviceInfo deviceInfo = this.saveLocalInfo(ref num, ref num2);
					bool flag = true;
					if (DevAccessCfg.GetInstance().getDeviceModelConfig(deviceInfo.ModelNm, deviceInfo.FWVersion).commonThresholdFlag != Constant.EatonPDUThreshold)
					{
						string myMac = deviceInfo.Mac;
						DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceInfo);
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						if (num == 1)
						{
							if (!devAccessAPI.SetDeviceName(deviceInfo.DeviceName, myMac))
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
								this.changeTreeSelect(deviceInfo.DeviceName);
								return;
							}
							myMac = "";
						}
						flag = true;
						if (num2 == 1)
						{
							RackInfo rackByID = RackInfo.getRackByID(deviceInfo.RackID);
							string originalName = rackByID.OriginalName;
							flag = devAccessAPI.SetRackName(originalName, myMac);
						}
					}
					if (!flag)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
					}
					else
					{
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
					}
					if (num == 1 || num2 == 1)
					{
						this.changeTreeSelect(deviceInfo.DeviceName);
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("PropDev Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private bool devConfigCheck()
		{
			string value = this.labDevModel.Tag.ToString();
			int item = System.Convert.ToInt32(value);
			bool flag = false;
			if (!this.tbDevNm.ReadOnly)
			{
				Ecovalidate.checkTextIsNull(this.tbDevNm, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbDevNm.Text
					}));
					this.tbDevNm.BackColor = Color.Red;
					return false;
				}
				string text = this.tbDevNm.Text;
				DeviceInfo deviceByName = DeviceOperation.getDeviceByName(text);
				if (deviceByName != null && deviceByName.DeviceID != System.Convert.ToInt32(value))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_nmdup, new string[]
					{
						this.tbDevNm.Text
					}));
					this.tbDevNm.Focus();
					this.tbDevNm.BackColor = Color.Red;
					return false;
				}
				this.tbDevNm.BackColor = Color.White;
			}
			Combobox_item combobox_item = (Combobox_item)this.cbDevRack.SelectedItem;
			string text2 = this.cbDevRack.SelectedItem.ToString();
			string key = combobox_item.getKey();
			long r_id = System.Convert.ToInt64(key);
			System.Collections.Generic.List<int> deviceIDByID = RackInfo.GetDeviceIDByID(r_id);
			if (!deviceIDByID.Contains(item) && deviceIDByID.Count >= 10)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_RackFull, new string[]
				{
					text2
				}));
				this.cbDevRack.Focus();
				this.cbDevRack.BackColor = Color.Red;
				return false;
			}
			this.cbDevRack.BackColor = Color.White;
			if (this.tbIP.Text.Equals(string.Empty))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
				this.tbIP.Focus();
				this.tbIP.BackColor = Color.Red;
				return false;
			}
			try
			{
				string text3 = IPAddress.Parse(this.tbIP.Text).ToString();
				this.tbIP.Text = text3;
			}
			catch (System.Exception)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
				this.tbIP.Focus();
				this.tbIP.BackColor = Color.Red;
				bool result = false;
				return result;
			}
			this.tbIP.BackColor = Color.White;
			if (!this.sysparaCheck())
			{
				return false;
			}
			object arg_261_0 = this.cbSS1.SelectedItem;
			if (this.cbSS1.Visible && this.cbSS1.SelectedItem == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Sensor_Loct_needSel, new string[0]));
				this.cbSS1.Focus();
				this.cbSS1.BackColor = Color.Red;
				return false;
			}
			this.cbSS1.BackColor = Color.White;
			if (this.cbSS2.Visible && this.cbSS2.SelectedItem == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Sensor_Loct_needSel, new string[0]));
				this.cbSS2.Focus();
				this.cbSS2.BackColor = Color.Red;
				return false;
			}
			this.cbSS2.BackColor = Color.White;
			if (this.cbSS3.Visible && this.cbSS3.SelectedItem == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Sensor_Loct_needSel, new string[0]));
				this.cbSS3.Focus();
				this.cbSS3.BackColor = Color.Red;
				return false;
			}
			this.cbSS3.BackColor = Color.White;
			if (this.cbSS4.Visible && this.cbSS4.SelectedItem == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Sensor_Loct_needSel, new string[0]));
				this.cbSS4.Focus();
				this.cbSS4.BackColor = Color.Red;
				return false;
			}
			this.cbSS4.BackColor = Color.White;
			return true;
		}
		private bool sysparaCheck()
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbSysUserNm, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbUsername.Text
				}));
				return false;
			}
			Ecovalidate.checkTextIsNull(this.tbSysTimeOut, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbtimeout.Text
				}));
				return false;
			}
			if (!Ecovalidate.Rangeint(this.tbSysTimeOut, 100, 5000))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbtimeout.Text,
					"100",
					"5000"
				}));
				return false;
			}
			Ecovalidate.checkTextIsNull(this.tbSysPort, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbSysPort.Text
				}));
				return false;
			}
			if (!Ecovalidate.Rangeint(this.tbSysTimeOut, 1, 65535))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbSysPort.Text,
					"1",
					"65535"
				}));
				return false;
			}
			Ecovalidate.checkTextIsNull(this.tbSysRetry, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbSysRetry.Text
				}));
				return false;
			}
			if (!Ecovalidate.Rangeint(this.tbSysRetry, 1, 5))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbSysRetry.Text,
					"1",
					"5"
				}));
				return false;
			}
			if (System.Convert.ToInt32(((Combobox_item)this.cbSnmpV.SelectedItem).getKey()) == 3 && !this.cbSysAuthen.SelectedItem.ToString().Equals("None"))
			{
				Ecovalidate.checkTextIsNull(this.tbSysAuthenPw, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbSysAuthenPw.Text
					}));
					return false;
				}
				if (!Ecovalidate.minlength(this.tbSysAuthenPw, 8))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.minlength, new string[]
					{
						this.lbSysAuthenPw.Text,
						"8"
					}));
					return false;
				}
				if (!this.cbSysPrivacy.SelectedItem.ToString().Equals("None"))
				{
					Ecovalidate.checkTextIsNull(this.tbSysPrivacyPw, ref flag);
					if (flag)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
						{
							this.lbSysPrivacyPw.Text
						}));
						return false;
					}
					if (!Ecovalidate.minlength(this.tbSysPrivacyPw, 8))
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.minlength, new string[]
						{
							this.lbSysPrivacyPw.Text,
							"8"
						}));
						return false;
					}
				}
			}
			return true;
		}
		private void changeTreeSelect(string devName)
		{
			PropDev1.PostMessage(this.m_pParent.Handle, 63000u, 0, devName);
		}
		private DeviceInfo saveLocalInfo(ref int nameChanged, ref int rackchanged)
		{
			string value = this.labDevModel.Tag.ToString();
			int l_id = System.Convert.ToInt32(value);
			Combobox_item combobox_item = (Combobox_item)this.cbDevRack.SelectedItem;
			string key = combobox_item.getKey();
			long num = System.Convert.ToInt64(key);
			string deviceInfo = RackInfo.getRackByID(num).DeviceInfo;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
			long rackID = deviceByID.RackID;
			if (!this.tbDevNm.ReadOnly && !deviceByID.DeviceName.Equals(this.tbDevNm.Text))
			{
				string text = this.tbDevNm.Text;
				deviceByID.DeviceName = text;
				nameChanged = 1;
			}
			if (deviceByID.RackID != num)
			{
				deviceByID.RackID = num;
				rackchanged = 1;
			}
			deviceByID.DeviceIP = this.tbIP.Text;
			deviceByID.Username = this.tbSysUserNm.Text;
			deviceByID.Port = System.Convert.ToInt32(this.tbSysPort.Text);
			deviceByID.Timeout = System.Convert.ToInt32(this.tbSysTimeOut.Text);
			deviceByID.Retry = System.Convert.ToInt32(this.tbSysRetry.Text);
			int num2 = System.Convert.ToInt32(((Combobox_item)this.cbSnmpV.SelectedItem).getKey());
			deviceByID.SnmpVersion = num2;
			deviceByID.Authentication = (deviceByID.Authentication = "None");
			deviceByID.AuthenPassword = string.Empty;
			deviceByID.Privacy = "None";
			deviceByID.PrivacyPassword = string.Empty;
			if (num2 == 3)
			{
				deviceByID.Authentication = this.cbSysAuthen.SelectedItem.ToString();
				if (!deviceByID.Authentication.Equals("None"))
				{
					deviceByID.AuthenPassword = this.tbSysAuthenPw.Text;
					deviceByID.Privacy = this.cbSysPrivacy.SelectedItem.ToString();
					deviceByID.PrivacyPassword = this.tbSysPrivacyPw.Text;
				}
			}
			deviceByID.Update();
			int num3 = 0;
			System.Collections.Generic.List<SensorInfo> sensorInfo = DeviceOperation.GetSensorInfo(deviceByID.DeviceID);
			string text2 = "";
			foreach (SensorInfo current in sensorInfo)
			{
				switch (current.Type)
				{
				case 1:
					if (!this.cbSS1.Visible)
					{
						continue;
					}
					text2 = this.cbSS1.SelectedItem.ToString();
					break;
				case 2:
					if (!this.cbSS2.Visible)
					{
						continue;
					}
					text2 = this.cbSS2.SelectedItem.ToString();
					break;
				case 3:
					if (!this.cbSS3.Visible)
					{
						continue;
					}
					text2 = this.cbSS3.SelectedItem.ToString();
					break;
				case 4:
					if (!this.cbSS4.Visible)
					{
						continue;
					}
					text2 = this.cbSS4.SelectedItem.ToString();
					break;
				}
				int num4;
				if (text2.Equals(EcoLanguage.getMsg(LangRes.Sensor_Loct_Intake, new string[0])))
				{
					num4 = 0;
				}
				else
				{
					if (text2.Equals(EcoLanguage.getMsg(LangRes.Sensor_Loct_Exhaust, new string[0])))
					{
						num4 = 1;
					}
					else
					{
						num4 = 2;
					}
				}
				if (current.Location != num4)
				{
					num3 = 1;
					current.Location = num4;
					current.Update();
				}
			}
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0630000", new string[]
				{
					deviceByID.DeviceName,
					deviceByID.Mac,
					deviceByID.DeviceIP,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0630000", new string[]
				{
					deviceByID.DeviceName,
					deviceByID.Mac,
					deviceByID.DeviceIP
				});
			}
			if (rackchanged == 1)
			{
				RackInfo rackByID = RackInfo.getRackByID(rackID);
				int num5 = 0;
				if (rackByID.DeviceInfo.Length == 0 || deviceInfo.Length == 0)
				{
					num5 |= 1;
				}
				EcoGlobalVar.gl_DevManPage.FlushFlg_RackBoard = 1;
				EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
				if (nameChanged == 1 || num3 == 1)
				{
					EcoGlobalVar.setDashBoardFlg(140uL, "#UPDATE#D" + deviceByID.DeviceID + ":;", num5 | 64);
				}
				else
				{
					EcoGlobalVar.setDashBoardFlg(12uL, "", num5 | 64);
				}
			}
			else
			{
				if (nameChanged == 1 || num3 == 1)
				{
					EcoGlobalVar.setDashBoardFlg(128uL, "#UPDATE#D" + deviceByID.DeviceID + ":;", 2);
				}
			}
			return deviceByID;
		}
		private void butDevicesDel_Click(object sender, System.EventArgs e)
		{
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Dev_delCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
			string value = this.labDevModel.Tag.ToString();
			int l_id = System.Convert.ToInt32(value);
			DBConn connection = DBConnPool.getConnection();
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
			int num = DeviceOperation.DeleteDeviceByID(connection, l_id);
			if (num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0430001", new string[]
				{
					deviceByID.ModelNm,
					deviceByID.Mac,
					deviceByID.DeviceIP,
					deviceByID.DeviceName,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0430001", new string[]
				{
					deviceByID.ModelNm,
					deviceByID.Mac,
					deviceByID.DeviceIP,
					deviceByID.DeviceName
				});
			}
			connection.Close();
			DeviceOperation.RefreshDBCache(false);
			DeviceOperation.StartDBCleanupThread();
			System.Collections.ArrayList allRack_NoEmpty2 = RackInfo.GetAllRack_NoEmpty();
			EcoGlobalVar.gl_DevManPage.FlushFlg_RackBoard = 1;
			EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
			if (allRack_NoEmpty.Count == allRack_NoEmpty2.Count)
			{
				EcoGlobalVar.setDashBoardFlg(526uL, "", 66);
			}
			else
			{
				EcoGlobalVar.setDashBoardFlg(526uL, "", 65);
			}
			this.changeTreeSelect("+1");
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropDev1));
			this.groupBox4 = new GroupBox();
			this.gBoxSensors = new GroupBox();
			this.cbSS4 = new ComboBox();
			this.lbSS4 = new Label();
			this.cbSS3 = new ComboBox();
			this.lbSS3 = new Label();
			this.cbSS2 = new ComboBox();
			this.lbSS2 = new Label();
			this.cbSS1 = new ComboBox();
			this.lbSS1 = new Label();
			this.groupBox3 = new GroupBox();
			this.tbDevNm = new TextBox();
			this.lbDevNm = new Label();
			this.label36 = new Label();
			this.labDevModel = new Label();
			this.cbDevRack = new ComboBox();
			this.tbIP = new TextBox();
			this.label27 = new Label();
			this.label28 = new Label();
			this.groupBox1 = new GroupBox();
			this.cbSnmpV = new ComboBox();
			this.tbSysUserNm = new TextBox();
			this.lbUsername = new Label();
			this.label6 = new Label();
			this.lbSysPort = new Label();
			this.label8 = new Label();
			this.tbSysPort = new TextBox();
			this.lbtimeout = new Label();
			this.tbSysRetry = new TextBox();
			this.tbSysTimeOut = new TextBox();
			this.lbSysRetry = new Label();
			this.lbSysPrivacyPw = new Label();
			this.cbSysAuthen = new ComboBox();
			this.tbSysPrivacyPw = new TextBox();
			this.cbSysPrivacy = new ComboBox();
			this.lbSysAuthenPw = new Label();
			this.labAuthen = new Label();
			this.tbSysAuthenPw = new TextBox();
			this.labPrivacy = new Label();
			this.butSave = new Button();
			this.toolTip1 = new ToolTip(this.components);
			this.butDevicesDel = new Button();
			this.groupBox4.SuspendLayout();
			this.gBoxSensors.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox4.BackColor = Color.WhiteSmoke;
			this.groupBox4.Controls.Add(this.gBoxSensors);
			this.groupBox4.Controls.Add(this.groupBox3);
			this.groupBox4.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.ForeColor = SystemColors.ControlText;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			this.gBoxSensors.Controls.Add(this.cbSS4);
			this.gBoxSensors.Controls.Add(this.lbSS4);
			this.gBoxSensors.Controls.Add(this.cbSS3);
			this.gBoxSensors.Controls.Add(this.lbSS3);
			this.gBoxSensors.Controls.Add(this.cbSS2);
			this.gBoxSensors.Controls.Add(this.lbSS2);
			this.gBoxSensors.Controls.Add(this.cbSS1);
			this.gBoxSensors.Controls.Add(this.lbSS1);
			componentResourceManager.ApplyResources(this.gBoxSensors, "gBoxSensors");
			this.gBoxSensors.Name = "gBoxSensors";
			this.gBoxSensors.TabStop = false;
			this.cbSS4.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSS4, "cbSS4");
			this.cbSS4.ForeColor = Color.Black;
			this.cbSS4.FormattingEnabled = true;
			this.cbSS4.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSS4.Items"),
				componentResourceManager.GetString("cbSS4.Items1"),
				componentResourceManager.GetString("cbSS4.Items2")
			});
			this.cbSS4.Name = "cbSS4";
			componentResourceManager.ApplyResources(this.lbSS4, "lbSS4");
			this.lbSS4.ForeColor = Color.Black;
			this.lbSS4.Name = "lbSS4";
			this.cbSS3.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSS3, "cbSS3");
			this.cbSS3.ForeColor = Color.Black;
			this.cbSS3.FormattingEnabled = true;
			this.cbSS3.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSS3.Items"),
				componentResourceManager.GetString("cbSS3.Items1"),
				componentResourceManager.GetString("cbSS3.Items2")
			});
			this.cbSS3.Name = "cbSS3";
			componentResourceManager.ApplyResources(this.lbSS3, "lbSS3");
			this.lbSS3.ForeColor = Color.Black;
			this.lbSS3.Name = "lbSS3";
			this.cbSS2.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSS2, "cbSS2");
			this.cbSS2.ForeColor = Color.Black;
			this.cbSS2.FormattingEnabled = true;
			this.cbSS2.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSS2.Items"),
				componentResourceManager.GetString("cbSS2.Items1"),
				componentResourceManager.GetString("cbSS2.Items2")
			});
			this.cbSS2.Name = "cbSS2";
			componentResourceManager.ApplyResources(this.lbSS2, "lbSS2");
			this.lbSS2.ForeColor = Color.Black;
			this.lbSS2.Name = "lbSS2";
			this.cbSS1.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSS1, "cbSS1");
			this.cbSS1.ForeColor = Color.Black;
			this.cbSS1.FormattingEnabled = true;
			this.cbSS1.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSS1.Items"),
				componentResourceManager.GetString("cbSS1.Items1"),
				componentResourceManager.GetString("cbSS1.Items2")
			});
			this.cbSS1.Name = "cbSS1";
			componentResourceManager.ApplyResources(this.lbSS1, "lbSS1");
			this.lbSS1.ForeColor = Color.Black;
			this.lbSS1.Name = "lbSS1";
			this.groupBox3.Controls.Add(this.tbDevNm);
			this.groupBox3.Controls.Add(this.lbDevNm);
			this.groupBox3.Controls.Add(this.label36);
			this.groupBox3.Controls.Add(this.labDevModel);
			this.groupBox3.Controls.Add(this.cbDevRack);
			this.groupBox3.Controls.Add(this.tbIP);
			this.groupBox3.Controls.Add(this.label27);
			this.groupBox3.Controls.Add(this.label28);
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.tbDevNm, "tbDevNm");
			this.tbDevNm.ForeColor = Color.Black;
			this.tbDevNm.Name = "tbDevNm";
			this.tbDevNm.KeyPress += new KeyPressEventHandler(this.tbDevNm_KeyPress);
			componentResourceManager.ApplyResources(this.lbDevNm, "lbDevNm");
			this.lbDevNm.ForeColor = Color.Black;
			this.lbDevNm.Name = "lbDevNm";
			componentResourceManager.ApplyResources(this.label36, "label36");
			this.label36.ForeColor = Color.Black;
			this.label36.Name = "label36";
			this.labDevModel.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevModel, "labDevModel");
			this.labDevModel.ForeColor = Color.Black;
			this.labDevModel.Name = "labDevModel";
			this.toolTip1.SetToolTip(this.labDevModel, componentResourceManager.GetString("labDevModel.ToolTip"));
			this.cbDevRack.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbDevRack, "cbDevRack");
			this.cbDevRack.Name = "cbDevRack";
			componentResourceManager.ApplyResources(this.tbIP, "tbIP");
			this.tbIP.ForeColor = Color.Black;
			this.tbIP.Name = "tbIP";
			this.tbIP.KeyPress += new KeyPressEventHandler(this.tbIP_KeyPress);
			componentResourceManager.ApplyResources(this.label27, "label27");
			this.label27.ForeColor = Color.Black;
			this.label27.Name = "label27";
			componentResourceManager.ApplyResources(this.label28, "label28");
			this.label28.ForeColor = Color.Black;
			this.label28.Name = "label28";
			this.groupBox1.Controls.Add(this.cbSnmpV);
			this.groupBox1.Controls.Add(this.tbSysUserNm);
			this.groupBox1.Controls.Add(this.lbUsername);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.lbSysPort);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.tbSysPort);
			this.groupBox1.Controls.Add(this.lbtimeout);
			this.groupBox1.Controls.Add(this.tbSysRetry);
			this.groupBox1.Controls.Add(this.tbSysTimeOut);
			this.groupBox1.Controls.Add(this.lbSysRetry);
			this.groupBox1.Controls.Add(this.lbSysPrivacyPw);
			this.groupBox1.Controls.Add(this.cbSysAuthen);
			this.groupBox1.Controls.Add(this.tbSysPrivacyPw);
			this.groupBox1.Controls.Add(this.cbSysPrivacy);
			this.groupBox1.Controls.Add(this.lbSysAuthenPw);
			this.groupBox1.Controls.Add(this.labAuthen);
			this.groupBox1.Controls.Add(this.tbSysAuthenPw);
			this.groupBox1.Controls.Add(this.labPrivacy);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.cbSnmpV.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSnmpV, "cbSnmpV");
			this.cbSnmpV.ForeColor = Color.Black;
			this.cbSnmpV.FormattingEnabled = true;
			this.cbSnmpV.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSnmpV.Items"),
				componentResourceManager.GetString("cbSnmpV.Items1"),
				componentResourceManager.GetString("cbSnmpV.Items2")
			});
			this.cbSnmpV.Name = "cbSnmpV";
			this.cbSnmpV.SelectedIndexChanged += new System.EventHandler(this.cbSnmpV_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.tbSysUserNm, "tbSysUserNm");
			this.tbSysUserNm.ForeColor = Color.Black;
			this.tbSysUserNm.Name = "tbSysUserNm";
			this.tbSysUserNm.KeyPress += new KeyPressEventHandler(this.tbSysUserNm_KeyPress);
			componentResourceManager.ApplyResources(this.lbUsername, "lbUsername");
			this.lbUsername.ForeColor = Color.Black;
			this.lbUsername.Name = "lbUsername";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = Color.Black;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.lbSysPort, "lbSysPort");
			this.lbSysPort.ForeColor = Color.Black;
			this.lbSysPort.Name = "lbSysPort";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.ForeColor = Color.Black;
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.tbSysPort, "tbSysPort");
			this.tbSysPort.ForeColor = Color.Black;
			this.tbSysPort.Name = "tbSysPort";
			this.tbSysPort.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.lbtimeout, "lbtimeout");
			this.lbtimeout.ForeColor = Color.Black;
			this.lbtimeout.Name = "lbtimeout";
			componentResourceManager.ApplyResources(this.tbSysRetry, "tbSysRetry");
			this.tbSysRetry.ForeColor = Color.Black;
			this.tbSysRetry.Name = "tbSysRetry";
			this.tbSysRetry.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.tbSysTimeOut, "tbSysTimeOut");
			this.tbSysTimeOut.ForeColor = Color.Black;
			this.tbSysTimeOut.Name = "tbSysTimeOut";
			this.tbSysTimeOut.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.lbSysRetry, "lbSysRetry");
			this.lbSysRetry.ForeColor = Color.Black;
			this.lbSysRetry.Name = "lbSysRetry";
			componentResourceManager.ApplyResources(this.lbSysPrivacyPw, "lbSysPrivacyPw");
			this.lbSysPrivacyPw.ForeColor = Color.Black;
			this.lbSysPrivacyPw.Name = "lbSysPrivacyPw";
			this.cbSysAuthen.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSysAuthen, "cbSysAuthen");
			this.cbSysAuthen.ForeColor = Color.Black;
			this.cbSysAuthen.FormattingEnabled = true;
			this.cbSysAuthen.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSysAuthen.Items"),
				componentResourceManager.GetString("cbSysAuthen.Items1"),
				componentResourceManager.GetString("cbSysAuthen.Items2")
			});
			this.cbSysAuthen.Name = "cbSysAuthen";
			componentResourceManager.ApplyResources(this.tbSysPrivacyPw, "tbSysPrivacyPw");
			this.tbSysPrivacyPw.ForeColor = Color.Black;
			this.tbSysPrivacyPw.Name = "tbSysPrivacyPw";
			this.tbSysPrivacyPw.UseSystemPasswordChar = true;
			this.cbSysPrivacy.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSysPrivacy, "cbSysPrivacy");
			this.cbSysPrivacy.ForeColor = Color.Black;
			this.cbSysPrivacy.FormattingEnabled = true;
			this.cbSysPrivacy.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSysPrivacy.Items"),
				componentResourceManager.GetString("cbSysPrivacy.Items1"),
				componentResourceManager.GetString("cbSysPrivacy.Items2")
			});
			this.cbSysPrivacy.Name = "cbSysPrivacy";
			componentResourceManager.ApplyResources(this.lbSysAuthenPw, "lbSysAuthenPw");
			this.lbSysAuthenPw.ForeColor = Color.Black;
			this.lbSysAuthenPw.Name = "lbSysAuthenPw";
			componentResourceManager.ApplyResources(this.labAuthen, "labAuthen");
			this.labAuthen.ForeColor = Color.Black;
			this.labAuthen.Name = "labAuthen";
			componentResourceManager.ApplyResources(this.tbSysAuthenPw, "tbSysAuthenPw");
			this.tbSysAuthenPw.ForeColor = Color.Black;
			this.tbSysAuthenPw.Name = "tbSysAuthenPw";
			this.tbSysAuthenPw.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.labPrivacy, "labPrivacy");
			this.labPrivacy.ForeColor = Color.Black;
			this.labPrivacy.Name = "labPrivacy";
			this.butSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSave, "butSave");
			this.butSave.Name = "butSave";
			this.butSave.UseVisualStyleBackColor = false;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			this.butDevicesDel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butDevicesDel, "butDevicesDel");
			this.butDevicesDel.Name = "butDevicesDel";
			this.butDevicesDel.UseVisualStyleBackColor = false;
			this.butDevicesDel.Click += new System.EventHandler(this.butDevicesDel_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butDevicesDel);
			base.Controls.Add(this.butSave);
			base.Controls.Add(this.groupBox4);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropDev1";
			this.groupBox4.ResumeLayout(false);
			this.gBoxSensors.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
