using CommonAPI.Global;
using CommonAPI.network;
using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class SysManSNMP : UserControl
	{
		private Sys_Para m_pSyspara;
		private Combobox_item m_snmpV1Comb = new Combobox_item(1.ToString(), "v1");
		private Combobox_item m_snmpV2Comb = new Combobox_item(2.ToString(), "v2c");
		private Combobox_item m_snmpV3Comb = new Combobox_item(3.ToString(), "v3");
		private Combobox_item m_trapsnmpV12Comb = new Combobox_item(1.ToString(), "v1/v2c");
		private Combobox_item m_trapsnmpV123Comb = new Combobox_item(2.ToString(), "v1/v2c/v3");
		private IContainer components;
		private GroupBox groupBox2;
		private Label label2;
		private Label label3;
		private Label lbSysTrapAuthenPw;
		private Label lbSysTrapPrivacyPw;
		private Label label1;
		private ComboBox cbTrapSnmpV;
		private ComboBox cbSysTrapAuthen;
		private ComboBox cbSysTrapPrivacy;
		private TextBox tbSysTrapUserNm;
		private TextBox tbSysTrapPort;
		private Label lbSysTrapPort;
		private Label lbTrapUsername;
		private TextBox tbSysTrapAuthenPw;
		private TextBox tbSysTrapPrivacyPw;
		private GroupBox groupBox25;
		private Label label6;
		private Label label8;
		private ComboBox cbSnmpV;
		private TextBox tbSysRetry;
		private Label lbSysRetry;
		private ComboBox cbSysAuthen;
		private ComboBox cbSysPrivacy;
		private TextBox tbSysPort;
		private Label lbSysPort;
		private TextBox tbSysUserNm;
		private Label lbUsername;
		private Label labAuthen;
		private Label labPrivacy;
		private TextBox tbSysAuthenPw;
		private Label lbSysAuthenPw;
		private TextBox tbSysPrivacyPw;
		private Label lbSysPrivacyPw;
		private TextBox tbSysTimeOut;
		private Label lbtimeout;
		private Button butSysparaSave;
		public SysManSNMP()
		{
			this.InitializeComponent();
			this.tbSysUserNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysTimeOut.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysRetry.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysTrapUserNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbSysTrapPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
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
			this.cbTrapSnmpV.Items.Clear();
			this.cbTrapSnmpV.Items.Add(this.m_trapsnmpV12Comb);
			this.cbTrapSnmpV.Items.Add(this.m_trapsnmpV123Comb);
		}
		public void pageInit()
		{
			this.m_pSyspara = new Sys_Para();
			this.tbSysUserNm.Text = this.m_pSyspara.username;
			this.tbSysPort.Text = System.Convert.ToString(this.m_pSyspara.port);
			this.tbSysTimeOut.Text = System.Convert.ToString(this.m_pSyspara.timeout);
			this.tbSysRetry.Text = System.Convert.ToString(this.m_pSyspara.retry);
			switch (this.m_pSyspara.SnmpVersion)
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
				this.cbSysPrivacy.SelectedItem = this.m_pSyspara.privacy;
				this.tbSysPrivacyPw.Text = this.m_pSyspara.privacypwd;
				this.cbSysAuthen.SelectedItem = this.m_pSyspara.authen;
				this.tbSysAuthenPw.Text = this.m_pSyspara.authenpwd;
				if (this.m_pSyspara.authen.Equals("None"))
				{
					this.tbSysAuthenPw.Enabled = false;
					this.cbSysPrivacy.SelectedItem = "None";
					this.cbSysPrivacy.Enabled = false;
					this.tbSysPrivacyPw.Enabled = false;
				}
				else
				{
					this.tbSysAuthenPw.Enabled = true;
					if (this.m_pSyspara.privacy.Equals("None"))
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
			this.tbSysTrapUserNm.Text = this.m_pSyspara.TrapUserName;
			this.tbSysTrapPort.Text = System.Convert.ToString(this.m_pSyspara.TrapPort);
			switch (this.m_pSyspara.TrapSnmpVersion)
			{
			case 1:
				this.cbTrapSnmpV.SelectedItem = this.m_trapsnmpV12Comb;
				this.showTrapV3setting(false);
				this.cbSysTrapAuthen.SelectedItem = "None";
				this.tbSysTrapPrivacyPw.Text = string.Empty;
				this.cbSysTrapAuthen.SelectedItem = "None";
				this.tbSysTrapAuthenPw.Text = string.Empty;
				return;
			case 2:
				this.cbTrapSnmpV.SelectedItem = this.m_trapsnmpV123Comb;
				this.showTrapV3setting(true);
				this.cbSysTrapPrivacy.SelectedItem = this.m_pSyspara.TrapPrivacy;
				this.tbSysTrapPrivacyPw.Text = this.m_pSyspara.TrapPrivacyPwd;
				this.cbSysTrapAuthen.SelectedItem = this.m_pSyspara.TrapAuthen;
				this.tbSysTrapAuthenPw.Text = this.m_pSyspara.TrapAuthenPwd;
				if (this.m_pSyspara.TrapAuthen.Equals("None"))
				{
					this.tbSysTrapAuthenPw.Enabled = false;
					this.cbSysTrapPrivacy.SelectedItem = "None";
					this.cbSysTrapPrivacy.Enabled = false;
					this.tbSysTrapPrivacyPw.Enabled = false;
					return;
				}
				this.tbSysTrapAuthenPw.Enabled = true;
				if (this.m_pSyspara.TrapPrivacy.Equals("None"))
				{
					this.tbSysTrapPrivacyPw.Enabled = false;
					return;
				}
				this.tbSysTrapPrivacyPw.Enabled = true;
				return;
			default:
				return;
			}
		}
		private void showV3setting(bool disp)
		{
			this.tbSysPrivacyPw.Enabled = disp;
			this.tbSysAuthenPw.Enabled = disp;
		}
		private void showTrapV3setting(bool disp)
		{
			this.tbSysTrapPrivacyPw.Enabled = disp;
			this.tbSysTrapAuthenPw.Enabled = disp;
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
		private void cbTrapSnmpV_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.cbTrapSnmpV.SelectedIndex == 0)
			{
				this.showTrapV3setting(false);
				this.cbSysTrapAuthen.SelectedItem = "None";
				this.cbSysTrapPrivacy.SelectedItem = "None";
				return;
			}
			this.showTrapV3setting(true);
			this.cbSysTrapAuthen.SelectedItem = "MD5";
			this.cbSysTrapPrivacy.SelectedItem = "AES";
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
			if (System.Convert.ToInt16(((Combobox_item)this.cbSnmpV.SelectedItem).getKey()) == 3 && !this.cbSysAuthen.SelectedItem.ToString().Equals("None"))
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
			Ecovalidate.checkTextIsNull(this.tbSysTrapUserNm, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbTrapUsername.Text
				}));
				return false;
			}
			Ecovalidate.checkTextIsNull(this.tbSysTrapPort, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbSysTrapPort.Text
				}));
				return false;
			}
			if (!Ecovalidate.Rangeint(this.tbSysTrapPort, 1, 65535))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbSysTrapPort.Text,
					"1",
					"65535"
				}));
				return false;
			}
			if (System.Convert.ToInt16(((Combobox_item)this.cbTrapSnmpV.SelectedItem).getKey()) == 2 && !this.cbSysTrapAuthen.SelectedItem.ToString().Equals("None"))
			{
				Ecovalidate.checkTextIsNull(this.tbSysTrapAuthenPw, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbSysTrapAuthenPw.Text
					}));
					return false;
				}
				if (!Ecovalidate.minlength(this.tbSysTrapAuthenPw, 8))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.minlength, new string[]
					{
						this.lbSysTrapAuthenPw.Text,
						"8"
					}));
					return false;
				}
				if (!this.cbSysTrapPrivacy.SelectedItem.ToString().Equals("None"))
				{
					Ecovalidate.checkTextIsNull(this.tbSysTrapPrivacyPw, ref flag);
					if (flag)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
						{
							this.lbSysTrapPrivacyPw.Text
						}));
						return false;
					}
					if (!Ecovalidate.minlength(this.tbSysTrapPrivacyPw, 8))
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.minlength, new string[]
						{
							this.lbSysTrapPrivacyPw.Text,
							"8"
						}));
						return false;
					}
				}
			}
			return true;
		}
		private void butSysparaSave_Click(object sender, System.EventArgs e)
		{
			if (!this.sysparaCheck())
			{
				return;
			}
			int num = System.Convert.ToInt32(this.tbSysTrapPort.Text);
			if (num != this.m_pSyspara.TrapPort)
			{
				bool flag = NetworkShareAccesser.UDPPortInUse(num);
				if (flag)
				{
					this.tbSysTrapPort.Focus();
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Portconflict, new string[]
					{
						this.tbSysTrapPort.Text
					}));
					return;
				}
			}
			this.m_pSyspara.username = this.tbSysUserNm.Text;
			this.m_pSyspara.timeout = System.Convert.ToInt32(this.tbSysTimeOut.Text);
			this.m_pSyspara.port = System.Convert.ToInt32(this.tbSysPort.Text);
			this.m_pSyspara.retry = System.Convert.ToInt32(this.tbSysRetry.Text);
			int num2 = System.Convert.ToInt32(((Combobox_item)this.cbSnmpV.SelectedItem).getKey());
			this.m_pSyspara.SnmpVersion = num2;
			this.m_pSyspara.authen = "None";
			this.m_pSyspara.authenpwd = string.Empty;
			this.m_pSyspara.privacy = "None";
			this.m_pSyspara.privacypwd = string.Empty;
			if (num2 == 3)
			{
				this.m_pSyspara.authen = this.cbSysAuthen.SelectedItem.ToString();
				if (!this.m_pSyspara.authen.Equals("None"))
				{
					this.m_pSyspara.authenpwd = this.tbSysAuthenPw.Text;
					this.m_pSyspara.privacy = this.cbSysPrivacy.SelectedItem.ToString();
					this.m_pSyspara.privacypwd = this.tbSysPrivacyPw.Text;
				}
			}
			Sys_Para sys_Para = new Sys_Para();
			bool flag2 = false;
			sys_Para.TrapUserName = this.tbSysTrapUserNm.Text;
			sys_Para.TrapPort = System.Convert.ToInt32(this.tbSysTrapPort.Text);
			num2 = System.Convert.ToInt32(((Combobox_item)this.cbTrapSnmpV.SelectedItem).getKey());
			sys_Para.TrapSnmpVersion = num2;
			sys_Para.TrapAuthen = "None";
			sys_Para.TrapAuthenPwd = string.Empty;
			sys_Para.TrapPrivacy = "None";
			sys_Para.TrapPrivacyPwd = string.Empty;
			if (num2 == 2)
			{
				sys_Para.TrapAuthen = this.cbSysTrapAuthen.SelectedItem.ToString();
				if (!sys_Para.TrapAuthen.Equals("None"))
				{
					sys_Para.TrapAuthenPwd = this.tbSysTrapAuthenPw.Text;
					sys_Para.TrapPrivacy = this.cbSysTrapPrivacy.SelectedItem.ToString();
					sys_Para.TrapPrivacyPwd = this.tbSysTrapPrivacyPw.Text;
				}
			}
			if (!this.m_pSyspara.TrapUserName.Equals(sys_Para.TrapUserName) || this.m_pSyspara.TrapPort != sys_Para.TrapPort || this.m_pSyspara.TrapSnmpVersion != sys_Para.TrapSnmpVersion || !this.m_pSyspara.TrapAuthen.Equals(sys_Para.TrapAuthen) || !this.m_pSyspara.TrapAuthenPwd.Equals(sys_Para.TrapAuthenPwd) || !this.m_pSyspara.TrapPrivacy.Equals(sys_Para.TrapPrivacy) || !this.m_pSyspara.TrapPrivacyPwd.Equals(sys_Para.TrapPrivacyPwd))
			{
				this.m_pSyspara.TrapUserName = sys_Para.TrapUserName;
				this.m_pSyspara.TrapPort = sys_Para.TrapPort;
				this.m_pSyspara.TrapSnmpVersion = sys_Para.TrapSnmpVersion;
				this.m_pSyspara.TrapAuthen = sys_Para.TrapAuthen;
				this.m_pSyspara.TrapAuthenPwd = sys_Para.TrapAuthenPwd;
				this.m_pSyspara.TrapPrivacy = sys_Para.TrapPrivacy;
				this.m_pSyspara.TrapPrivacyPwd = sys_Para.TrapPrivacyPwd;
				flag2 = true;
			}
			this.m_pSyspara.update();
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0130022", new string[]
				{
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0130022", new string[0]);
			}
			if (flag2)
			{
				System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
				System.Collections.Generic.List<DevSnmpConfig> list = new System.Collections.Generic.List<DevSnmpConfig>();
				foreach (DeviceInfo current in allDevice)
				{
					if (ClientAPI.IsDeviceOnline(current.DeviceID))
					{
						DevSnmpConfig sNMPpara = commUtil.getSNMPpara(current);
						list.Add(sNMPpara);
					}
				}
				try
				{
					DevMonitorAPI devMonitorAPI = new DevMonitorAPI();
					devMonitorAPI.SetTrapReceiver(list, this.m_pSyspara);
				}
				catch (System.Exception)
				{
				}
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
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
		private bool CheckUDPport(int port)
		{
			Socket socket = null;
			try
			{
				EndPoint localEP = new IPEndPoint(IPAddress.Any, port);
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.Bind(localEP);
			}
			catch (System.Exception)
			{
				return true;
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
				}
			}
			return false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManSNMP));
			this.groupBox2 = new GroupBox();
			this.label2 = new Label();
			this.label3 = new Label();
			this.lbSysTrapAuthenPw = new Label();
			this.lbSysTrapPrivacyPw = new Label();
			this.label1 = new Label();
			this.cbTrapSnmpV = new ComboBox();
			this.cbSysTrapAuthen = new ComboBox();
			this.cbSysTrapPrivacy = new ComboBox();
			this.tbSysTrapUserNm = new TextBox();
			this.tbSysTrapPort = new TextBox();
			this.lbSysTrapPort = new Label();
			this.lbTrapUsername = new Label();
			this.tbSysTrapAuthenPw = new TextBox();
			this.tbSysTrapPrivacyPw = new TextBox();
			this.groupBox25 = new GroupBox();
			this.label6 = new Label();
			this.label8 = new Label();
			this.cbSnmpV = new ComboBox();
			this.tbSysRetry = new TextBox();
			this.lbSysRetry = new Label();
			this.cbSysAuthen = new ComboBox();
			this.cbSysPrivacy = new ComboBox();
			this.tbSysPort = new TextBox();
			this.lbSysPort = new Label();
			this.tbSysUserNm = new TextBox();
			this.lbUsername = new Label();
			this.labAuthen = new Label();
			this.labPrivacy = new Label();
			this.tbSysAuthenPw = new TextBox();
			this.lbSysAuthenPw = new Label();
			this.tbSysPrivacyPw = new TextBox();
			this.lbSysPrivacyPw = new Label();
			this.tbSysTimeOut = new TextBox();
			this.lbtimeout = new Label();
			this.butSysparaSave = new Button();
			this.groupBox2.SuspendLayout();
			this.groupBox25.SuspendLayout();
			base.SuspendLayout();
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.lbSysTrapAuthenPw);
			this.groupBox2.Controls.Add(this.lbSysTrapPrivacyPw);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.cbTrapSnmpV);
			this.groupBox2.Controls.Add(this.cbSysTrapAuthen);
			this.groupBox2.Controls.Add(this.cbSysTrapPrivacy);
			this.groupBox2.Controls.Add(this.tbSysTrapUserNm);
			this.groupBox2.Controls.Add(this.tbSysTrapPort);
			this.groupBox2.Controls.Add(this.lbSysTrapPort);
			this.groupBox2.Controls.Add(this.lbTrapUsername);
			this.groupBox2.Controls.Add(this.tbSysTrapAuthenPw);
			this.groupBox2.Controls.Add(this.tbSysTrapPrivacyPw);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = Color.Black;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = Color.Black;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.lbSysTrapAuthenPw, "lbSysTrapAuthenPw");
			this.lbSysTrapAuthenPw.ForeColor = Color.Black;
			this.lbSysTrapAuthenPw.Name = "lbSysTrapAuthenPw";
			componentResourceManager.ApplyResources(this.lbSysTrapPrivacyPw, "lbSysTrapPrivacyPw");
			this.lbSysTrapPrivacyPw.ForeColor = Color.Black;
			this.lbSysTrapPrivacyPw.Name = "lbSysTrapPrivacyPw";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Black;
			this.label1.Name = "label1";
			this.cbTrapSnmpV.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbTrapSnmpV, "cbTrapSnmpV");
			this.cbTrapSnmpV.ForeColor = Color.Black;
			this.cbTrapSnmpV.FormattingEnabled = true;
			this.cbTrapSnmpV.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbTrapSnmpV.Items"),
				componentResourceManager.GetString("cbTrapSnmpV.Items1")
			});
			this.cbTrapSnmpV.Name = "cbTrapSnmpV";
			this.cbTrapSnmpV.SelectedIndexChanged += new System.EventHandler(this.cbTrapSnmpV_SelectedIndexChanged);
			this.cbSysTrapAuthen.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSysTrapAuthen, "cbSysTrapAuthen");
			this.cbSysTrapAuthen.ForeColor = Color.Black;
			this.cbSysTrapAuthen.FormattingEnabled = true;
			this.cbSysTrapAuthen.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSysTrapAuthen.Items"),
				componentResourceManager.GetString("cbSysTrapAuthen.Items1"),
				componentResourceManager.GetString("cbSysTrapAuthen.Items2")
			});
			this.cbSysTrapAuthen.Name = "cbSysTrapAuthen";
			this.cbSysTrapPrivacy.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbSysTrapPrivacy, "cbSysTrapPrivacy");
			this.cbSysTrapPrivacy.ForeColor = Color.Black;
			this.cbSysTrapPrivacy.FormattingEnabled = true;
			this.cbSysTrapPrivacy.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbSysTrapPrivacy.Items"),
				componentResourceManager.GetString("cbSysTrapPrivacy.Items1"),
				componentResourceManager.GetString("cbSysTrapPrivacy.Items2")
			});
			this.cbSysTrapPrivacy.Name = "cbSysTrapPrivacy";
			componentResourceManager.ApplyResources(this.tbSysTrapUserNm, "tbSysTrapUserNm");
			this.tbSysTrapUserNm.ForeColor = Color.Black;
			this.tbSysTrapUserNm.Name = "tbSysTrapUserNm";
			componentResourceManager.ApplyResources(this.tbSysTrapPort, "tbSysTrapPort");
			this.tbSysTrapPort.ForeColor = Color.Black;
			this.tbSysTrapPort.Name = "tbSysTrapPort";
			this.tbSysTrapPort.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.lbSysTrapPort, "lbSysTrapPort");
			this.lbSysTrapPort.ForeColor = Color.Black;
			this.lbSysTrapPort.Name = "lbSysTrapPort";
			componentResourceManager.ApplyResources(this.lbTrapUsername, "lbTrapUsername");
			this.lbTrapUsername.ForeColor = Color.Black;
			this.lbTrapUsername.Name = "lbTrapUsername";
			componentResourceManager.ApplyResources(this.tbSysTrapAuthenPw, "tbSysTrapAuthenPw");
			this.tbSysTrapAuthenPw.ForeColor = Color.Black;
			this.tbSysTrapAuthenPw.Name = "tbSysTrapAuthenPw";
			this.tbSysTrapAuthenPw.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.tbSysTrapPrivacyPw, "tbSysTrapPrivacyPw");
			this.tbSysTrapPrivacyPw.ForeColor = Color.Black;
			this.tbSysTrapPrivacyPw.Name = "tbSysTrapPrivacyPw";
			this.tbSysTrapPrivacyPw.UseSystemPasswordChar = true;
			this.groupBox25.Controls.Add(this.label6);
			this.groupBox25.Controls.Add(this.label8);
			this.groupBox25.Controls.Add(this.cbSnmpV);
			this.groupBox25.Controls.Add(this.tbSysRetry);
			this.groupBox25.Controls.Add(this.lbSysRetry);
			this.groupBox25.Controls.Add(this.cbSysAuthen);
			this.groupBox25.Controls.Add(this.cbSysPrivacy);
			this.groupBox25.Controls.Add(this.tbSysPort);
			this.groupBox25.Controls.Add(this.lbSysPort);
			this.groupBox25.Controls.Add(this.tbSysUserNm);
			this.groupBox25.Controls.Add(this.lbUsername);
			this.groupBox25.Controls.Add(this.labAuthen);
			this.groupBox25.Controls.Add(this.labPrivacy);
			this.groupBox25.Controls.Add(this.tbSysAuthenPw);
			this.groupBox25.Controls.Add(this.lbSysAuthenPw);
			this.groupBox25.Controls.Add(this.tbSysPrivacyPw);
			this.groupBox25.Controls.Add(this.lbSysPrivacyPw);
			this.groupBox25.Controls.Add(this.tbSysTimeOut);
			this.groupBox25.Controls.Add(this.lbtimeout);
			componentResourceManager.ApplyResources(this.groupBox25, "groupBox25");
			this.groupBox25.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox25.Name = "groupBox25";
			this.groupBox25.TabStop = false;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = Color.Black;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.ForeColor = Color.Black;
			this.label8.Name = "label8";
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
			componentResourceManager.ApplyResources(this.tbSysRetry, "tbSysRetry");
			this.tbSysRetry.ForeColor = Color.Black;
			this.tbSysRetry.Name = "tbSysRetry";
			this.tbSysRetry.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.lbSysRetry, "lbSysRetry");
			this.lbSysRetry.ForeColor = Color.Black;
			this.lbSysRetry.Name = "lbSysRetry";
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
			componentResourceManager.ApplyResources(this.tbSysPort, "tbSysPort");
			this.tbSysPort.ForeColor = Color.Black;
			this.tbSysPort.Name = "tbSysPort";
			this.tbSysPort.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.lbSysPort, "lbSysPort");
			this.lbSysPort.ForeColor = Color.Black;
			this.lbSysPort.Name = "lbSysPort";
			componentResourceManager.ApplyResources(this.tbSysUserNm, "tbSysUserNm");
			this.tbSysUserNm.ForeColor = Color.Black;
			this.tbSysUserNm.Name = "tbSysUserNm";
			this.tbSysUserNm.KeyPress += new KeyPressEventHandler(this.tbSysUserNm_KeyPress);
			componentResourceManager.ApplyResources(this.lbUsername, "lbUsername");
			this.lbUsername.ForeColor = Color.Black;
			this.lbUsername.Name = "lbUsername";
			componentResourceManager.ApplyResources(this.labAuthen, "labAuthen");
			this.labAuthen.ForeColor = Color.Black;
			this.labAuthen.Name = "labAuthen";
			componentResourceManager.ApplyResources(this.labPrivacy, "labPrivacy");
			this.labPrivacy.ForeColor = Color.Black;
			this.labPrivacy.Name = "labPrivacy";
			componentResourceManager.ApplyResources(this.tbSysAuthenPw, "tbSysAuthenPw");
			this.tbSysAuthenPw.ForeColor = Color.Black;
			this.tbSysAuthenPw.Name = "tbSysAuthenPw";
			this.tbSysAuthenPw.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.lbSysAuthenPw, "lbSysAuthenPw");
			this.lbSysAuthenPw.ForeColor = Color.Black;
			this.lbSysAuthenPw.Name = "lbSysAuthenPw";
			componentResourceManager.ApplyResources(this.tbSysPrivacyPw, "tbSysPrivacyPw");
			this.tbSysPrivacyPw.ForeColor = Color.Black;
			this.tbSysPrivacyPw.Name = "tbSysPrivacyPw";
			this.tbSysPrivacyPw.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.lbSysPrivacyPw, "lbSysPrivacyPw");
			this.lbSysPrivacyPw.ForeColor = Color.Black;
			this.lbSysPrivacyPw.Name = "lbSysPrivacyPw";
			componentResourceManager.ApplyResources(this.tbSysTimeOut, "tbSysTimeOut");
			this.tbSysTimeOut.ForeColor = Color.Black;
			this.tbSysTimeOut.Name = "tbSysTimeOut";
			this.tbSysTimeOut.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.lbtimeout, "lbtimeout");
			this.lbtimeout.ForeColor = Color.Black;
			this.lbtimeout.Name = "lbtimeout";
			this.butSysparaSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSysparaSave, "butSysparaSave");
			this.butSysparaSave.Name = "butSysparaSave";
			this.butSysparaSave.UseVisualStyleBackColor = false;
			this.butSysparaSave.Click += new System.EventHandler(this.butSysparaSave_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox25);
			base.Controls.Add(this.butSysparaSave);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysManSNMP";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox25.ResumeLayout(false);
			this.groupBox25.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
