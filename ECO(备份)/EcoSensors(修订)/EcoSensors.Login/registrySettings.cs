using CommonAPI;
using CommonAPI.Global;
using CommonAPI.network;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using InSnergyAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
namespace EcoSensors.Login
{
	public class registrySettings : Form
	{
		private IContainer components;
		private Label lbDBMsg;
		private TabControl tcLoginSettings;
		private TabPage tpdatabase;
		private TabPage tpPortsetting;
		private Button btncancel1;
		private Button btnsaveDB;
		private GroupBox groupBox1;
		private TextBox tbDBIP;
		private Label lbDBIP;
		private TextBox tbDBPsw;
		private TextBox tbDBUsrnm;
		private TextBox tbDBPort;
		private Label lbDBPsw;
		private Label lbDBUsrnm;
		private Label lbDBPort;
		private TextBox tbManagerPort;
		private Label lbManagerPort;
		private CheckBox checkBoxUseMySQL;
		private TextBox tbTrapPort;
		private Label lbTrapPort;
		private Button btnsavePort;
		private TextBox tbGatewayPort;
		private Label lbGatewayPort;
		private TextBox tbBillingPort;
		private Label lbBillingPort;
		private Label lbPortMsg;
		private Button btnCancel2;
		private Button btnrestore;
		private int m_oldSrvStCode;
		private Sys_Para m_pSyspara;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(registrySettings));
			this.lbDBMsg = new Label();
			this.tcLoginSettings = new TabControl();
			this.tpdatabase = new TabPage();
			this.btnrestore = new Button();
			this.btncancel1 = new Button();
			this.btnsaveDB = new Button();
			this.groupBox1 = new GroupBox();
			this.checkBoxUseMySQL = new CheckBox();
			this.tbDBIP = new TextBox();
			this.lbDBIP = new Label();
			this.tbDBPsw = new TextBox();
			this.tbDBUsrnm = new TextBox();
			this.tbDBPort = new TextBox();
			this.lbDBPsw = new Label();
			this.lbDBUsrnm = new Label();
			this.lbDBPort = new Label();
			this.tpPortsetting = new TabPage();
			this.btnCancel2 = new Button();
			this.lbPortMsg = new Label();
			this.tbTrapPort = new TextBox();
			this.lbTrapPort = new Label();
			this.btnsavePort = new Button();
			this.tbGatewayPort = new TextBox();
			this.lbGatewayPort = new Label();
			this.tbBillingPort = new TextBox();
			this.lbBillingPort = new Label();
			this.tbManagerPort = new TextBox();
			this.lbManagerPort = new Label();
			this.tcLoginSettings.SuspendLayout();
			this.tpdatabase.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tpPortsetting.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.lbDBMsg, "lbDBMsg");
			this.lbDBMsg.Name = "lbDBMsg";
			this.tcLoginSettings.Controls.Add(this.tpdatabase);
			this.tcLoginSettings.Controls.Add(this.tpPortsetting);
			componentResourceManager.ApplyResources(this.tcLoginSettings, "tcLoginSettings");
			this.tcLoginSettings.Name = "tcLoginSettings";
			this.tcLoginSettings.SelectedIndex = 0;
			this.tpdatabase.BackColor = Color.WhiteSmoke;
			this.tpdatabase.Controls.Add(this.btnrestore);
			this.tpdatabase.Controls.Add(this.btncancel1);
			this.tpdatabase.Controls.Add(this.btnsaveDB);
			this.tpdatabase.Controls.Add(this.groupBox1);
			this.tpdatabase.Controls.Add(this.lbDBMsg);
			componentResourceManager.ApplyResources(this.tpdatabase, "tpdatabase");
			this.tpdatabase.Name = "tpdatabase";
			this.btnrestore.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnrestore, "btnrestore");
			this.btnrestore.ForeColor = SystemColors.ControlText;
			this.btnrestore.Name = "btnrestore";
			this.btnrestore.UseVisualStyleBackColor = false;
			this.btnrestore.Click += new System.EventHandler(this.btnrestore_Click);
			this.btncancel1.BackColor = Color.Gainsboro;
			this.btncancel1.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btncancel1, "btncancel1");
			this.btncancel1.ForeColor = SystemColors.ControlText;
			this.btncancel1.Name = "btncancel1";
			this.btncancel1.UseVisualStyleBackColor = false;
			this.btnsaveDB.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsaveDB, "btnsaveDB");
			this.btnsaveDB.ForeColor = SystemColors.ControlText;
			this.btnsaveDB.Name = "btnsaveDB";
			this.btnsaveDB.UseVisualStyleBackColor = false;
			this.btnsaveDB.Click += new System.EventHandler(this.btnsaveDB_Click);
			this.groupBox1.Controls.Add(this.checkBoxUseMySQL);
			this.groupBox1.Controls.Add(this.tbDBIP);
			this.groupBox1.Controls.Add(this.lbDBIP);
			this.groupBox1.Controls.Add(this.tbDBPsw);
			this.groupBox1.Controls.Add(this.tbDBUsrnm);
			this.groupBox1.Controls.Add(this.tbDBPort);
			this.groupBox1.Controls.Add(this.lbDBPsw);
			this.groupBox1.Controls.Add(this.lbDBUsrnm);
			this.groupBox1.Controls.Add(this.lbDBPort);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxUseMySQL, "checkBoxUseMySQL");
			this.checkBoxUseMySQL.Name = "checkBoxUseMySQL";
			this.checkBoxUseMySQL.UseVisualStyleBackColor = true;
			this.checkBoxUseMySQL.CheckedChanged += new System.EventHandler(this.checkBoxUseMySQL_CheckedChanged);
			componentResourceManager.ApplyResources(this.tbDBIP, "tbDBIP");
			this.tbDBIP.Name = "tbDBIP";
			this.tbDBIP.KeyPress += new KeyPressEventHandler(this.IPStr_KeyPress);
			componentResourceManager.ApplyResources(this.lbDBIP, "lbDBIP");
			this.lbDBIP.Name = "lbDBIP";
			componentResourceManager.ApplyResources(this.tbDBPsw, "tbDBPsw");
			this.tbDBPsw.Name = "tbDBPsw";
			componentResourceManager.ApplyResources(this.tbDBUsrnm, "tbDBUsrnm");
			this.tbDBUsrnm.Name = "tbDBUsrnm";
			componentResourceManager.ApplyResources(this.tbDBPort, "tbDBPort");
			this.tbDBPort.Name = "tbDBPort";
			this.tbDBPort.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.lbDBPsw, "lbDBPsw");
			this.lbDBPsw.Name = "lbDBPsw";
			componentResourceManager.ApplyResources(this.lbDBUsrnm, "lbDBUsrnm");
			this.lbDBUsrnm.Name = "lbDBUsrnm";
			componentResourceManager.ApplyResources(this.lbDBPort, "lbDBPort");
			this.lbDBPort.Name = "lbDBPort";
			this.tpPortsetting.BackColor = Color.WhiteSmoke;
			this.tpPortsetting.Controls.Add(this.btnCancel2);
			this.tpPortsetting.Controls.Add(this.lbPortMsg);
			this.tpPortsetting.Controls.Add(this.tbTrapPort);
			this.tpPortsetting.Controls.Add(this.lbTrapPort);
			this.tpPortsetting.Controls.Add(this.btnsavePort);
			this.tpPortsetting.Controls.Add(this.tbGatewayPort);
			this.tpPortsetting.Controls.Add(this.lbGatewayPort);
			this.tpPortsetting.Controls.Add(this.tbBillingPort);
			this.tpPortsetting.Controls.Add(this.lbBillingPort);
			this.tpPortsetting.Controls.Add(this.tbManagerPort);
			this.tpPortsetting.Controls.Add(this.lbManagerPort);
			componentResourceManager.ApplyResources(this.tpPortsetting, "tpPortsetting");
			this.tpPortsetting.Name = "tpPortsetting";
			this.btnCancel2.BackColor = Color.Gainsboro;
			this.btnCancel2.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btnCancel2, "btnCancel2");
			this.btnCancel2.ForeColor = SystemColors.ControlText;
			this.btnCancel2.Name = "btnCancel2";
			this.btnCancel2.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.lbPortMsg, "lbPortMsg");
			this.lbPortMsg.Name = "lbPortMsg";
			componentResourceManager.ApplyResources(this.tbTrapPort, "tbTrapPort");
			this.tbTrapPort.Name = "tbTrapPort";
			this.tbTrapPort.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.lbTrapPort, "lbTrapPort");
			this.lbTrapPort.Name = "lbTrapPort";
			this.btnsavePort.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsavePort, "btnsavePort");
			this.btnsavePort.ForeColor = SystemColors.ControlText;
			this.btnsavePort.Name = "btnsavePort";
			this.btnsavePort.UseVisualStyleBackColor = false;
			this.btnsavePort.Click += new System.EventHandler(this.btnsavePort_Click);
			componentResourceManager.ApplyResources(this.tbGatewayPort, "tbGatewayPort");
			this.tbGatewayPort.Name = "tbGatewayPort";
			this.tbGatewayPort.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.lbGatewayPort, "lbGatewayPort");
			this.lbGatewayPort.Name = "lbGatewayPort";
			componentResourceManager.ApplyResources(this.tbBillingPort, "tbBillingPort");
			this.tbBillingPort.Name = "tbBillingPort";
			this.tbBillingPort.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.lbBillingPort, "lbBillingPort");
			this.lbBillingPort.Name = "lbBillingPort";
			componentResourceManager.ApplyResources(this.tbManagerPort, "tbManagerPort");
			this.tbManagerPort.Name = "tbManagerPort";
			this.tbManagerPort.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.lbManagerPort, "lbManagerPort");
			this.lbManagerPort.Name = "lbManagerPort";
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tcLoginSettings);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "registrySettings";
			base.FormClosing += new FormClosingEventHandler(this.registrySettings_FormClosing);
			this.tcLoginSettings.ResumeLayout(false);
			this.tpdatabase.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tpPortsetting.ResumeLayout(false);
			this.tpPortsetting.PerformLayout();
			base.ResumeLayout(false);
		}
		public registrySettings(int SrvStCode)
		{
			this.InitializeComponent();
			this.m_oldSrvStCode = SrvStCode;
			this.tbDBIP.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbDBPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbDBUsrnm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbDBPsw.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbManagerPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbTrapPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbBillingPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbGatewayPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tcLoginSettings.Controls.Clear();
			if ((SrvStCode & DebugCenter.ST_fatalMask) != 0)
			{
				string text = "";
				if (SrvStCode == DebugCenter.ST_SevsPortNA)
				{
					this.tcLoginSettings.Controls.Add(this.tpPortsetting);
					this.lbManagerPort.Visible = true;
					this.tbManagerPort.Visible = true;
					this.lbTrapPort.Visible = false;
					this.tbTrapPort.Visible = false;
					this.lbBillingPort.Visible = false;
					this.tbBillingPort.Visible = false;
					this.lbGatewayPort.Visible = false;
					this.tbGatewayPort.Visible = false;
					this.tbManagerPort.Text = ValuePairs.getValuePair("ServicePort");
				}
				DBUrl.initconfig();
				if (SrvStCode == DebugCenter.ST_MYSQLCONNECT_ERROR)
				{
					text = "MySQL database connection failure (error code: " + SrvStCode.ToString("X4") + "). Please check your settings and try again.";
				}
				if (SrvStCode == DebugCenter.ST_MYSQLAUTH_ERROR)
				{
					text = "MySQL database authentication failure (error code: " + SrvStCode.ToString("X4") + "). Please check your settings and try again.";
				}
				if (SrvStCode == DebugCenter.ST_MYSQLNotExist || SrvStCode == DebugCenter.ST_MYSQLSIDNotMatch || SrvStCode == DebugCenter.ST_MYSQLREPAIR_ERROR)
				{
					text = "MySQL database fatal error (error code: " + SrvStCode.ToString("X4") + "). Please try either restore database, or use Access (by uncheck \"Use MySQL database\") instead.";
				}
				if (text.Length > 0)
				{
					this.tcLoginSettings.Controls.Add(this.tpdatabase);
					this.lbDBMsg.Text = text;
					this.checkBoxUseMySQL.Checked = true;
					this.tbDBIP.Text = "127.0.0.1";
					this.tbDBPort.Text = DBUrl.CURRENT_PORT.ToString();
					this.tbDBUsrnm.Text = DBUrl.CURRENT_USER_NAME;
					this.tbDBPsw.Text = DBUrl.CURRENT_PWD;
				}
				return;
			}
			this.tcLoginSettings.Controls.Add(this.tpPortsetting);
			this.lbManagerPort.Visible = false;
			this.tbManagerPort.Visible = false;
			this.m_pSyspara = new Sys_Para();
			if ((SrvStCode & DebugCenter.ST_TrapPortNA) != 0)
			{
				this.lbTrapPort.Visible = true;
				this.tbTrapPort.Visible = true;
				this.tbTrapPort.Text = System.Convert.ToString(this.m_pSyspara.TrapPort);
			}
			else
			{
				this.lbTrapPort.Visible = false;
				this.tbTrapPort.Visible = false;
			}
			if ((SrvStCode & DebugCenter.ST_BillingPortNA) != 0)
			{
				this.lbBillingPort.Visible = true;
				this.tbBillingPort.Visible = true;
				this.tbBillingPort.Text = System.Convert.ToString(Sys_Para.GetBPPort());
			}
			else
			{
				this.lbBillingPort.Visible = false;
				this.tbBillingPort.Visible = false;
			}
			if ((SrvStCode & DebugCenter.ST_GateWayPortNA) != 0)
			{
				this.lbGatewayPort.Visible = true;
				this.tbGatewayPort.Visible = true;
				this.tbGatewayPort.Text = System.Convert.ToString(Sys_Para.GetISGPort());
				return;
			}
			this.lbGatewayPort.Visible = false;
			this.tbGatewayPort.Visible = false;
		}
		private void num_KeyPress(object sender, KeyPressEventArgs e)
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
		private bool DBparaCheck()
		{
			if (!this.checkBoxUseMySQL.Checked)
			{
				return true;
			}
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbDBIP, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbDBIP.Text
				}));
				return false;
			}
			try
			{
				string text = IPAddress.Parse(this.tbDBIP.Text).ToString();
				this.tbDBIP.Text = text;
			}
			catch (System.Exception)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
				this.tbDBIP.Focus();
				bool result = false;
				return result;
			}
			Ecovalidate.checkTextIsNull(this.tbDBPort, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbDBPort.Text
				}));
				return false;
			}
			if (!Ecovalidate.Rangeint(this.tbDBPort, 1, 65535))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbDBPort.Text,
					"1",
					"65535"
				}));
				return false;
			}
			Ecovalidate.checkTextIsNull(this.tbDBUsrnm, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbDBUsrnm.Text
				}));
				return false;
			}
			return true;
		}
		private bool portparacheck(Label lb, TextBox tb, int Portindex)
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(tb, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					lb.Text
				}));
				return false;
			}
			if (!Ecovalidate.Rangeint(tb, 1, 65535))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					lb.Text,
					"1",
					"65535"
				}));
				return false;
			}
			int port = System.Convert.ToInt32(tb.Text);
			bool flag2 = false;
			switch (Portindex)
			{
			case 0:
			case 2:
			case 3:
				flag2 = NetworkShareAccesser.TcpPortInUse(port);
				break;
			case 1:
				flag2 = NetworkShareAccesser.UDPPortInUse(port);
				break;
			}
			if (flag2)
			{
				tb.Focus();
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Portconflict, new string[]
				{
					tb.Text
				}));
				return false;
			}
			return true;
		}
		private void btnsavePort_Click(object sender, System.EventArgs e)
		{
			if (!this.tbManagerPort.Visible)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				if (this.tbTrapPort.Visible)
				{
					if (!this.portparacheck(this.lbTrapPort, this.tbTrapPort, 1))
					{
						return;
					}
					num = System.Convert.ToInt32(this.tbTrapPort.Text);
				}
				if (this.tbBillingPort.Visible)
				{
					if (!this.portparacheck(this.lbBillingPort, this.tbBillingPort, 3))
					{
						return;
					}
					num2 = System.Convert.ToInt32(this.tbBillingPort.Text);
					if (num2 == num)
					{
						this.tbBillingPort.Focus();
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Portconflict, new string[]
						{
							this.tbBillingPort.Text
						}));
						return;
					}
				}
				if (this.tbGatewayPort.Visible)
				{
					if (!this.portparacheck(this.lbGatewayPort, this.tbGatewayPort, 2))
					{
						return;
					}
					num3 = System.Convert.ToInt32(this.tbGatewayPort.Text);
					if (num3 == num || num3 == num2)
					{
						this.tbGatewayPort.Focus();
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Portconflict, new string[]
						{
							this.tbGatewayPort.Text
						}));
						return;
					}
				}
				if (this.tbTrapPort.Visible)
				{
					new Sys_Para
					{
						TrapPort = num
					}.update();
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
				}
				if (this.tbBillingPort.Visible)
				{
					Sys_Para.SetBPPort(num2);
					InSnergyService.RestartBillingProtocol(Sys_Para.GetBPFlag() == 1, num2, Sys_Para.GetBPSecurity());
				}
				if (this.tbGatewayPort.Visible)
				{
					Sys_Para.SetISGPort(num3);
					if (Sys_Para.GetISGFlag() == 0)
					{
						InSnergyService.Restart(false, num3);
					}
					else
					{
						InSnergyService.Restart(true, num3);
					}
				}
				base.DialogResult = DialogResult.OK;
				return;
			}
			if (!this.portparacheck(this.lbManagerPort, this.tbManagerPort, 0))
			{
				return;
			}
			int value = System.Convert.ToInt32(this.tbManagerPort.Text);
			ValuePairs.setValuePair("ServicePort", System.Convert.ToString(value));
			ValuePairs.SaveValueKeyToRegistry(true);
			base.DialogResult = DialogResult.OK;
		}
		private void btnsaveDB_Click(object sender, System.EventArgs e)
		{
			if (!this.DBparaCheck())
			{
				return;
			}
			if (!this.checkBoxUseMySQL.Checked)
			{
				DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.DB_ChangeCrm, new string[0]), MessageBoxButtons.OKCancel);
				if (dialogResult == DialogResult.Cancel)
				{
					return;
				}
				AccessDBUpdate.InitAccessDataDB();
				DBUtil.ChangeDBSetting2Access();
				base.DialogResult = DialogResult.OK;
				return;
			}
			else
			{
				string text = this.tbDBIP.Text;
				int num = System.Convert.ToInt32(this.tbDBPort.Text);
				string text2 = this.tbDBUsrnm.Text;
				string text3 = this.tbDBPsw.Text;
				string[] param = new string[]
				{
					text,
					this.tbDBPort.Text,
					text2,
					text3
				};
				progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_Checkdbconnect, new string[0]), null, new progressPopup.ProcessInThread(this.dbCheckParameter), param, 0);
				progressPopup.ShowDialog();
				object return_V = progressPopup.Return_V;
				int? num2 = return_V as int?;
				if (!num2.HasValue)
				{
					num2 = new int?(DebugCenter.ST_Unknown);
				}
				if (num2 == DebugCenter.ST_Success)
				{
					DBUtil.ChangeDBSetting2MySQL(text, num, text2, text3, false);
					base.DialogResult = DialogResult.OK;
					return;
				}
				if (num2 == DebugCenter.ST_MYSQLCONNECT_ERROR || num2 == DebugCenter.ST_MYSQLAUTH_ERROR || num2 == DebugCenter.ST_Unknown)
				{
					EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.DB_Connectfail, new string[0]));
					return;
				}
				DialogResult dialogResult2 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.DBinitcrm_Master, new string[0]), MessageBoxButtons.OKCancel);
				if (dialogResult2 == DialogResult.Cancel)
				{
					return;
				}
				string dbname = "";
				int num3 = DBMaintain.InitMySQLDatabase4Master(text, num, text2, text3, ref dbname);
				if (num3 > 0)
				{
					num3 = DBUtil.ChangeDBSetting2MySQL(dbname, text, num, text2, text3);
					if (num3 == 1)
					{
						num3 = DebugCenter.ST_Success;
					}
				}
				if (num3 == DebugCenter.ST_Success)
				{
					base.DialogResult = DialogResult.OK;
					return;
				}
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
		}
		private object dbCheckParameter(object para)
		{
			string[] array = (string[])para;
			string str_host = array[0];
			int i_port = System.Convert.ToInt32(array[1]);
			string str_usr = array[2];
			string str_pwd = array[3];
			int num = DBUtil.CheckMySQLParameter(str_host, i_port, str_usr, str_pwd, false);
			return num;
		}
		private void registrySettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			if ((this.m_oldSrvStCode & DebugCenter.ST_fatalMask) == 0)
			{
				return;
			}
			if (base.DialogResult != DialogResult.OK && EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Login_quit, new string[0]), MessageBoxButtons.YesNo) == DialogResult.No)
			{
				e.Cancel = true;
			}
		}
		private void btnrestore_Click(object sender, System.EventArgs e)
		{
			restoredb restoredb = new restoredb(2);
			DialogResult dialogResult = restoredb.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				base.DialogResult = DialogResult.OK;
			}
		}
		private void checkBoxUseMySQL_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.checkBoxUseMySQL.Checked)
			{
				this.tbDBPort.Enabled = true;
				this.tbDBPort.Text = "3306";
				this.tbDBUsrnm.Enabled = true;
				this.tbDBPsw.Enabled = true;
				return;
			}
			this.tbDBIP.Enabled = false;
			this.tbDBPort.Enabled = false;
			this.tbDBUsrnm.Enabled = false;
			this.tbDBPsw.Enabled = false;
		}
	}
}
