using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice
{
	public class ScanDevDlg : Form
	{
		public const int SCANCompany_ATEN = 1;
		public const int SCANCompany_EATON = 2;
		public const int SCANCompany_APC = 4;
		private int m_scanCompany = 1;
		private string m_IPs = "";
		private Sys_Para m_pSyspara;
		private Combobox_item m_snmpV1Comb = new Combobox_item(1.ToString(), "v1");
		private Combobox_item m_snmpV2Comb = new Combobox_item(2.ToString(), "v2c");
		private Combobox_item m_snmpV3Comb = new Combobox_item(3.ToString(), "v3");
		private Combobox_item m_ScanComanyCombo_aten = new Combobox_item(1.ToString(), "ATEN PDU");
		private Combobox_item m_ScanComanyCombo_eaton = new Combobox_item(2.ToString(), "Eaton PDU");
		private Combobox_item m_ScanComanyCombo_APC = new Combobox_item(4.ToString(), "APC PDU");
		private IContainer components;
		private Button butScan;
		private Button butCancel;
		private TextBox tbIp;
		private RadioButton rbIp;
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
		private GroupBox groupBox1;
		private TextBox tbIp3;
		private TextBox tbIp2;
		private TextBox tbIp1;
		private TextBox textBox1;
		private RadioButton rbSubnet;
		private Label lbScanCompany;
		private ComboBox cbScanCompany;
		public ScanDevDlg()
		{
			this.InitializeComponent();
			this.tbIp1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbIp2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbIp3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbIp.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
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
			this.cbScanCompany.Items.Clear();
			this.cbScanCompany.Items.Add(this.m_ScanComanyCombo_aten);
			int num = DevAccessCfg.GetInstance().supportOEMdev();
			if (num == 0)
			{
				this.lbScanCompany.Visible = false;
				this.cbScanCompany.Visible = false;
			}
			if ((num & 1) != 0)
			{
				this.cbScanCompany.Items.Add(this.m_ScanComanyCombo_eaton);
				Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\ATEN\\ecoSensors");
				if (registryKey == null)
				{
					registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\ATEN\\ecoSensors");
				}
				if (registryKey != null)
				{
					try
					{
						string text = registryKey.GetValue("SerialNo").ToString();
						if (text.Contains("HONDA-"))
						{
							this.cbScanCompany.Items.Add(this.m_ScanComanyCombo_APC);
						}
					}
					catch (System.Exception)
					{
					}
				}
			}
			this.cbScanCompany.SelectedItem = this.m_ScanComanyCombo_aten;
		}
		public int getScanCompany()
		{
			return this.m_scanCompany;
		}
		public string getIPs()
		{
			return this.m_IPs;
		}
		public DevSnmpConfig getSNMPpara()
		{
			DevSnmpConfig devSnmpConfig = new DevSnmpConfig();
			switch (this.m_pSyspara.SnmpVersion)
			{
			case 1:
				devSnmpConfig.snmpVer = 0;
				break;
			case 2:
				devSnmpConfig.snmpVer = 1;
				break;
			case 3:
				devSnmpConfig.snmpVer = 3;
				break;
			}
			devSnmpConfig.devPort = this.m_pSyspara.port;
			devSnmpConfig.userName = this.m_pSyspara.username;
			devSnmpConfig.retry = this.m_pSyspara.retry;
			devSnmpConfig.timeout = this.m_pSyspara.timeout;
			devSnmpConfig.authPSW = this.m_pSyspara.authenpwd;
			devSnmpConfig.authType = this.m_pSyspara.authen;
			devSnmpConfig.privPSW = this.m_pSyspara.privacypwd;
			devSnmpConfig.privType = this.m_pSyspara.privacy;
			return devSnmpConfig;
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
				return;
			case 2:
				this.cbSnmpV.SelectedItem = this.m_snmpV2Comb;
				this.showV3setting(false);
				this.cbSysPrivacy.SelectedItem = "None";
				this.tbSysPrivacyPw.Text = string.Empty;
				this.cbSysAuthen.SelectedItem = "None";
				this.tbSysAuthenPw.Text = string.Empty;
				return;
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
					return;
				}
				this.tbSysAuthenPw.Enabled = true;
				if (this.m_pSyspara.privacy.Equals("None"))
				{
					this.tbSysPrivacyPw.Enabled = false;
					return;
				}
				this.tbSysPrivacyPw.Enabled = true;
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
		private void rbSubnet_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rbSubnet.Checked)
			{
				this.tbIp1.Enabled = true;
				this.tbIp2.Enabled = true;
				this.tbIp3.Enabled = true;
				this.tbIp.Enabled = false;
			}
			else
			{
				this.tbIp1.Enabled = false;
				this.tbIp2.Enabled = false;
				this.tbIp3.Enabled = false;
				this.tbIp.Enabled = true;
			}
			this.tbIp1.Text = "";
			this.tbIp2.Text = "";
			this.tbIp3.Text = "";
			this.tbIp.Text = "";
		}
		private void IP_KeyPress(object sender, KeyPressEventArgs e)
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
			if (keyChar == '.')
			{
				if (((TextBox)sender).Name.Equals("tbIp1"))
				{
					this.tbIp2.Focus();
				}
				else
				{
					if (((TextBox)sender).Name.Equals("tbIp2"))
					{
						this.tbIp3.Focus();
					}
				}
			}
			e.Handled = true;
		}
		private void IPStr_KeyPress(object sender, KeyPressEventArgs e)
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
			if (char.IsPunctuation(keyChar))
			{
				return;
			}
			e.Handled = true;
		}
		private void butScan_Click(object sender, System.EventArgs e)
		{
			if (!this.checkIp() || !this.sysparaCheck())
			{
				return;
			}
			base.Close();
			this.m_scanCompany = 1;
			if (this.cbScanCompany.Visible)
			{
				this.m_scanCompany = System.Convert.ToInt32(((Combobox_item)this.cbScanCompany.SelectedItem).getKey());
			}
			string iPs;
			if (this.rbSubnet.Checked)
			{
				iPs = string.Concat(new string[]
				{
					this.tbIp1.Text,
					".",
					this.tbIp2.Text,
					".",
					this.tbIp3.Text
				});
			}
			else
			{
				iPs = this.tbIp.Text;
			}
			this.m_IPs = iPs;
			this.m_pSyspara.username = this.tbSysUserNm.Text;
			this.m_pSyspara.timeout = System.Convert.ToInt32(this.tbSysTimeOut.Text);
			this.m_pSyspara.port = System.Convert.ToInt32(this.tbSysPort.Text);
			this.m_pSyspara.retry = System.Convert.ToInt32(this.tbSysRetry.Text);
			int num = System.Convert.ToInt32(((Combobox_item)this.cbSnmpV.SelectedItem).getKey());
			this.m_pSyspara.SnmpVersion = num;
			this.m_pSyspara.authen = "None";
			this.m_pSyspara.authenpwd = string.Empty;
			this.m_pSyspara.privacy = "None";
			this.m_pSyspara.privacypwd = string.Empty;
			if (num == 3)
			{
				this.m_pSyspara.authen = this.cbSysAuthen.SelectedItem.ToString();
				if (!this.m_pSyspara.authen.Equals("None"))
				{
					this.m_pSyspara.authenpwd = this.tbSysAuthenPw.Text;
					this.m_pSyspara.privacy = this.cbSysPrivacy.SelectedItem.ToString();
					this.m_pSyspara.privacypwd = this.tbSysPrivacyPw.Text;
				}
			}
		}
		private bool checkIp()
		{
			if (this.rbSubnet.Checked)
			{
				if (this.tbIp1.Text.Equals(string.Empty))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp1.Focus();
					return false;
				}
				if (!this.checkInt(this.tbIp1.Text))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp1.Focus();
					return false;
				}
				int num = System.Convert.ToInt32(this.tbIp1.Text);
				if (num > 254 || num <= 0)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp1.Focus();
					return false;
				}
				if (this.tbIp2.Text.Equals(string.Empty))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp2.Focus();
					return false;
				}
				if (!this.checkInt(this.tbIp2.Text))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp2.Focus();
					return false;
				}
				int num2 = (int)System.Convert.ToInt16(this.tbIp2.Text);
				if (num2 > 254 || num2 < 0)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp2.Focus();
					return false;
				}
				if (this.tbIp3.Text.Equals(string.Empty))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp3.Focus();
					return false;
				}
				if (!this.checkInt(this.tbIp3.Text))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp3.Focus();
					return false;
				}
				int num3 = (int)System.Convert.ToInt16(this.tbIp3.Text);
				if (num3 > 254 || num3 < 0)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp3.Focus();
					return false;
				}
				return true;
			}
			else
			{
				if (this.tbIp.Text.Equals(string.Empty))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp.Focus();
					return false;
				}
				try
				{
					string text = IPAddress.Parse(this.tbIp.Text).ToString();
					this.tbIp.Text = text;
				}
				catch (System.Exception)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIp.Focus();
					return false;
				}
				return true;
			}
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
			return true;
		}
		private bool checkInt(string checkStr)
		{
			Regex regex = new Regex("^[0-9]+$");
			return regex.IsMatch(checkStr);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ScanDevDlg));
			this.butScan = new Button();
			this.butCancel = new Button();
			this.tbIp = new TextBox();
			this.rbIp = new RadioButton();
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
			this.groupBox1 = new GroupBox();
			this.tbIp3 = new TextBox();
			this.tbIp2 = new TextBox();
			this.tbIp1 = new TextBox();
			this.textBox1 = new TextBox();
			this.rbSubnet = new RadioButton();
			this.lbScanCompany = new Label();
			this.cbScanCompany = new ComboBox();
			this.groupBox25.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.butScan, "butScan");
			this.butScan.Name = "butScan";
			this.butScan.UseVisualStyleBackColor = true;
			this.butScan.Click += new System.EventHandler(this.butScan_Click);
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.DialogResult = DialogResult.Cancel;
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tbIp, "tbIp");
			this.tbIp.Name = "tbIp";
			this.tbIp.KeyPress += new KeyPressEventHandler(this.IPStr_KeyPress);
			componentResourceManager.ApplyResources(this.rbIp, "rbIp");
			this.rbIp.Name = "rbIp";
			this.rbIp.UseVisualStyleBackColor = true;
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
			this.groupBox25.ForeColor = SystemColors.ControlText;
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
			this.groupBox1.Controls.Add(this.tbIp3);
			this.groupBox1.Controls.Add(this.tbIp2);
			this.groupBox1.Controls.Add(this.tbIp1);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.tbIp);
			this.groupBox1.Controls.Add(this.rbIp);
			this.groupBox1.Controls.Add(this.rbSubnet);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.tbIp3, "tbIp3");
			this.tbIp3.Name = "tbIp3";
			this.tbIp3.KeyPress += new KeyPressEventHandler(this.IP_KeyPress);
			componentResourceManager.ApplyResources(this.tbIp2, "tbIp2");
			this.tbIp2.Name = "tbIp2";
			this.tbIp2.KeyPress += new KeyPressEventHandler(this.IP_KeyPress);
			componentResourceManager.ApplyResources(this.tbIp1, "tbIp1");
			this.tbIp1.Name = "tbIp1";
			this.tbIp1.KeyPress += new KeyPressEventHandler(this.IP_KeyPress);
			componentResourceManager.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.rbSubnet, "rbSubnet");
			this.rbSubnet.Checked = true;
			this.rbSubnet.Name = "rbSubnet";
			this.rbSubnet.TabStop = true;
			this.rbSubnet.CheckedChanged += new System.EventHandler(this.rbSubnet_CheckedChanged);
			componentResourceManager.ApplyResources(this.lbScanCompany, "lbScanCompany");
			this.lbScanCompany.ForeColor = Color.Black;
			this.lbScanCompany.Name = "lbScanCompany";
			this.cbScanCompany.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbScanCompany, "cbScanCompany");
			this.cbScanCompany.ForeColor = Color.Black;
			this.cbScanCompany.FormattingEnabled = true;
			this.cbScanCompany.Name = "cbScanCompany";
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.cbScanCompany);
			base.Controls.Add(this.lbScanCompany);
			base.Controls.Add(this.groupBox25);
			base.Controls.Add(this.butScan);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ScanDevDlg";
			base.ShowInTaskbar = false;
			this.groupBox25.ResumeLayout(false);
			this.groupBox25.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
