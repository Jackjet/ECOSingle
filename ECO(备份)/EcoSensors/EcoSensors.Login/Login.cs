using CommonAPI.Global;
using CommonAPI.InterProcess;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
namespace EcoSensors.Login
{
	public class Login : Form
	{
		private IContainer components;
		private Label label3;
		private Label label5;
		private TableLayoutPanel tableLayoutPanel1;
		private Panel panelLocal;
		private Panel panel1;
		private Button butcancel;
		private Button butlogin;
		private Label label1;
		private ComboBox cbolanguage;
		private Label label6;
		private TextBox tbpassword;
		private Label label7;
		private TextBox tbuserId;
		private Label label8;
		private Panel panel2;
		private Panel panelRemote;
		private TextBox rmttbSrvPort;
		private Label rmtlbSrvPort;
		private TextBox rmttbSrvIP;
		private Label rmtlbSrvIP;
		private ComboBox rmtcbLang;
		private Label rmtlbLang;
		private TextBox rmttbpsw;
		private Label rmtlbpsw;
		private TextBox rmttbusrnm;
		private Label rmtlbusrnm;
		private Button rmtbutcancel;
		private Button rmtbutLogin;
		private Label lbSetting;
		private string m_userName;
		private string m_psw;
		private int m_forceClose;
		public string UserName
		{
			get
			{
				return this.m_userName;
			}
		}
		public string UserPsw
		{
			get
			{
				return this.m_psw;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Login));
			this.label5 = new Label();
			this.label3 = new Label();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.panel1 = new Panel();
			this.label1 = new Label();
			this.panel2 = new Panel();
			this.panelRemote = new Panel();
			this.rmttbSrvPort = new TextBox();
			this.rmtlbSrvPort = new Label();
			this.rmttbSrvIP = new TextBox();
			this.rmtlbSrvIP = new Label();
			this.rmtcbLang = new ComboBox();
			this.rmtlbLang = new Label();
			this.rmttbpsw = new TextBox();
			this.rmtlbpsw = new Label();
			this.rmttbusrnm = new TextBox();
			this.rmtlbusrnm = new Label();
			this.rmtbutcancel = new Button();
			this.rmtbutLogin = new Button();
			this.panelLocal = new Panel();
			this.cbolanguage = new ComboBox();
			this.label6 = new Label();
			this.tbpassword = new TextBox();
			this.label7 = new Label();
			this.tbuserId = new TextBox();
			this.label8 = new Label();
			this.butcancel = new Button();
			this.butlogin = new Button();
			this.lbSetting = new Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panelRemote.SuspendLayout();
			this.panelLocal.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label5, "label5");
            this.label5.Image = Resources.down;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label3, "label3");

            //Õû¸ölogo
            //this.label3.Image = Resources.login_banner;

            //this.Icon = null;



			this.label3.Name = "label3";
			this.tableLayoutPanel1.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.panel1.BackColor = Color.FromArgb(0, 79, 153);
			this.panel1.Controls.Add(this.label1);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.White;
			this.label1.Name = "label1";
			this.panel2.BackColor = Color.White;
			this.panel2.Controls.Add(this.panelRemote);
			this.panel2.Controls.Add(this.panelLocal);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.panelRemote.BackColor = Color.White;
			this.panelRemote.Controls.Add(this.rmttbSrvPort);
			this.panelRemote.Controls.Add(this.rmtlbSrvPort);
			this.panelRemote.Controls.Add(this.rmttbSrvIP);
			this.panelRemote.Controls.Add(this.rmtlbSrvIP);
			this.panelRemote.Controls.Add(this.rmtcbLang);
			this.panelRemote.Controls.Add(this.rmtlbLang);
			this.panelRemote.Controls.Add(this.rmttbpsw);
			this.panelRemote.Controls.Add(this.rmtlbpsw);
			this.panelRemote.Controls.Add(this.rmttbusrnm);
			this.panelRemote.Controls.Add(this.rmtlbusrnm);
			this.panelRemote.Controls.Add(this.rmtbutcancel);
			this.panelRemote.Controls.Add(this.rmtbutLogin);
			componentResourceManager.ApplyResources(this.panelRemote, "panelRemote");
			this.panelRemote.Name = "panelRemote";
			componentResourceManager.ApplyResources(this.rmttbSrvPort, "rmttbSrvPort");
			this.rmttbSrvPort.Name = "rmttbSrvPort";
			componentResourceManager.ApplyResources(this.rmtlbSrvPort, "rmtlbSrvPort");
			this.rmtlbSrvPort.Name = "rmtlbSrvPort";
			componentResourceManager.ApplyResources(this.rmttbSrvIP, "rmttbSrvIP");
			this.rmttbSrvIP.Name = "rmttbSrvIP";
			componentResourceManager.ApplyResources(this.rmtlbSrvIP, "rmtlbSrvIP");
			this.rmtlbSrvIP.Name = "rmtlbSrvIP";
			this.rmtcbLang.DropDownHeight = 200;
			this.rmtcbLang.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.rmtcbLang, "rmtcbLang");
			this.rmtcbLang.FormattingEnabled = true;
			this.rmtcbLang.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("rmtcbLang.Items"),
				componentResourceManager.GetString("rmtcbLang.Items1"),
				componentResourceManager.GetString("rmtcbLang.Items2"),
				componentResourceManager.GetString("rmtcbLang.Items3"),
				componentResourceManager.GetString("rmtcbLang.Items4"),
				componentResourceManager.GetString("rmtcbLang.Items5"),
				componentResourceManager.GetString("rmtcbLang.Items6"),
				componentResourceManager.GetString("rmtcbLang.Items7"),
				componentResourceManager.GetString("rmtcbLang.Items8"),
				componentResourceManager.GetString("rmtcbLang.Items9"),
				componentResourceManager.GetString("rmtcbLang.Items10")
			});
			this.rmtcbLang.Name = "rmtcbLang";
			this.rmtcbLang.SelectedIndexChanged += new System.EventHandler(this.rmtcbLang_SelectedIndexChanged);
			this.rmtcbLang.KeyPress += new KeyPressEventHandler(this.comm_KeyPress);
			componentResourceManager.ApplyResources(this.rmtlbLang, "rmtlbLang");
			this.rmtlbLang.Name = "rmtlbLang";
			componentResourceManager.ApplyResources(this.rmttbpsw, "rmttbpsw");
			this.rmttbpsw.Name = "rmttbpsw";
			this.rmttbpsw.KeyPress += new KeyPressEventHandler(this.comm_KeyPress);
			componentResourceManager.ApplyResources(this.rmtlbpsw, "rmtlbpsw");
			this.rmtlbpsw.Name = "rmtlbpsw";
			componentResourceManager.ApplyResources(this.rmttbusrnm, "rmttbusrnm");
			this.rmttbusrnm.Name = "rmttbusrnm";
			this.rmttbusrnm.KeyPress += new KeyPressEventHandler(this.userNm_KeyPress);
			componentResourceManager.ApplyResources(this.rmtlbusrnm, "rmtlbusrnm");
			this.rmtlbusrnm.Name = "rmtlbusrnm";
			this.rmtbutcancel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.rmtbutcancel, "rmtbutcancel");
			this.rmtbutcancel.Name = "rmtbutcancel";
			this.rmtbutcancel.UseVisualStyleBackColor = false;
			this.rmtbutcancel.Click += new System.EventHandler(this.butcancel_Click);
			this.rmtbutLogin.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.rmtbutLogin, "rmtbutLogin");
			this.rmtbutLogin.Name = "rmtbutLogin";
			this.rmtbutLogin.UseVisualStyleBackColor = false;
			this.rmtbutLogin.Click += new System.EventHandler(this.rmtbutLogin_Click);
			this.panelLocal.BackColor = Color.White;
			this.panelLocal.Controls.Add(this.cbolanguage);
			this.panelLocal.Controls.Add(this.label6);
			this.panelLocal.Controls.Add(this.tbpassword);
			this.panelLocal.Controls.Add(this.label7);
			this.panelLocal.Controls.Add(this.tbuserId);
			this.panelLocal.Controls.Add(this.label8);
			this.panelLocal.Controls.Add(this.butcancel);
			this.panelLocal.Controls.Add(this.butlogin);
			componentResourceManager.ApplyResources(this.panelLocal, "panelLocal");
			this.panelLocal.Name = "panelLocal";
			this.cbolanguage.DropDownHeight = 200;
			this.cbolanguage.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbolanguage, "cbolanguage");
			this.cbolanguage.FormattingEnabled = true;
			this.cbolanguage.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbolanguage.Items"),
				componentResourceManager.GetString("cbolanguage.Items1"),
				componentResourceManager.GetString("cbolanguage.Items2"),
				componentResourceManager.GetString("cbolanguage.Items3"),
				componentResourceManager.GetString("cbolanguage.Items4"),
				componentResourceManager.GetString("cbolanguage.Items5"),
				componentResourceManager.GetString("cbolanguage.Items6"),
				componentResourceManager.GetString("cbolanguage.Items7"),
				componentResourceManager.GetString("cbolanguage.Items8"),
				componentResourceManager.GetString("cbolanguage.Items9"),
				componentResourceManager.GetString("cbolanguage.Items10")
			});
			this.cbolanguage.Name = "cbolanguage";
			this.cbolanguage.SelectedIndexChanged += new System.EventHandler(this.cbolanguage_SelectedIndexChanged);
			this.cbolanguage.KeyPress += new KeyPressEventHandler(this.comm_KeyPress);
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.tbpassword, "tbpassword");
			this.tbpassword.Name = "tbpassword";
			this.tbpassword.KeyPress += new KeyPressEventHandler(this.comm_KeyPress);
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.tbuserId, "tbuserId");
			this.tbuserId.Name = "tbuserId";
			this.tbuserId.KeyPress += new KeyPressEventHandler(this.userNm_KeyPress);
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.butcancel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butcancel, "butcancel");
			this.butcancel.Name = "butcancel";
			this.butcancel.UseVisualStyleBackColor = false;
			this.butcancel.Click += new System.EventHandler(this.butcancel_Click);
			this.butlogin.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butlogin, "butlogin");
			this.butlogin.Name = "butlogin";
			this.butlogin.UseVisualStyleBackColor = false;
			this.butlogin.Click += new System.EventHandler(this.butlogin_Click);
			this.lbSetting.Cursor = Cursors.Hand;
            this.lbSetting.Image = Resources.Settingicon;
			componentResourceManager.ApplyResources(this.lbSetting, "lbSetting");
			this.lbSetting.Name = "lbSetting";
			this.lbSetting.Click += new System.EventHandler(this.lbSetting_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
            this.BackgroundImage = Resources.login_background;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.lbSetting);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.label3);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Login";
			base.SizeGripStyle = SizeGripStyle.Hide;
			base.FormClosing += new FormClosingEventHandler(this.Login_FormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panelRemote.ResumeLayout(false);
			this.panelRemote.PerformLayout();
			this.panelLocal.ResumeLayout(false);
			this.panelLocal.PerformLayout();
			base.ResumeLayout(false);
		}
		public Login()
		{
			this.InitializeComponent();
			this.tbuserId.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbpassword.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.cbolanguage.Items.Clear();
			this.rmtcbLang.Items.Clear();
			for (int i = 0; i < EcoLanguage.strLang.Length; i++)
			{
				this.cbolanguage.Items.Add(EcoLanguage.strLang[i]);
				this.rmtcbLang.Items.Add(EcoLanguage.strLang[i]);
			}
			this.cbolanguage.SelectedIndex = 0;
			this.rmtcbLang.SelectedIndex = 0;
			string text = DevAccessCfg.GetInstance().getVersion();
			if (text.Length > 0)
			{
				text = " (" + text + ")";
			}
			else
			{
				System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				text = string.Concat(new string[]
				{
					" (V",
					version.Major.ToString(),
					".",
					version.Minor.ToString(),
					".",
					version.Build.ToString("000"),
					".",
					version.Revision.ToString("000"),
					")"
				});
			}
			this.Text += text;
			if (EcoGlobalVar.ECOAppRunMode == 1)
			{
				this.panelRemote.Visible = false;
				ValuePairs.setValuePair("MasterIP", "127.0.0.1");
				return;
			}
			this.panelLocal.Visible = false;
			this.lbSetting.Visible = false;
			this.rmttbSrvIP.Text = ValuePairs.getValuePair("MasterIP");
			this.rmttbSrvPort.Text = ValuePairs.getValuePair("ServicePort");
		}
		private void userNm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				if (EcoGlobalVar.ECOAppRunMode == 1)
				{
					this.butlogin.Focus();
					this.butlogin_Click(this.butlogin, null);
				}
				else
				{
					this.rmtbutLogin.Focus();
					this.rmtbutLogin_Click(this.butlogin, null);
				}
			}
			if (e.KeyChar != '\b' && (e.KeyChar > '9' || e.KeyChar < '0') && ('A' > e.KeyChar || (e.KeyChar > 'Z' && e.KeyChar < '_') || e.KeyChar > 'z'))
			{
				e.Handled = true;
			}
		}
		private void butlogin_Click(object sender, System.EventArgs e)
		{
			this.m_forceClose = 0;
			try
			{
				if (this.tbuserId.Text == "")
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_needname, new string[0]));
					this.tbuserId.Focus();
				}
				else
				{
					if (this.tbpassword.Text == "")
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_needpsw, new string[0]));
						this.tbpassword.Focus();
					}
					else
					{
						string text = "Standalone";
						string interProcessKeyValue = InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.getInterProcessKeyValue("Listen_Port");
						if (!string.IsNullOrEmpty(interProcessKeyValue) && System.Convert.ToInt32(interProcessKeyValue) != 0)
						{
							ValuePairs.setValuePair("ServicePort", interProcessKeyValue);
						}
						System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
						string serialNo = ValuePairs.getSerialNo(false);
						string param = string.Concat(new object[]
						{
							this.tbuserId.Text,
							"\n",
							this.tbpassword.Text,
							"\n",
							version,
							"\n",
							text,
							"\nLocal\n",
							serialNo
						});
						progressPopup progressPopup = new progressPopup("User Login", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_Login, new string[0]), Resources.login_background, new progressPopup.ProcessInThread(this.checkAuth), param, 0);
						progressPopup.StartPosition = FormStartPosition.CenterScreen;
						progressPopup.ShowDialog();
						object return_V = progressPopup.Return_V;
						int? num = return_V as int?;
						if (num == -1)
						{
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvConnectFail, new string[0]));
							this.m_forceClose = -1;
							base.Close();
						}
						else
						{
							if (num == -2)
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_loginfail, new string[0]));
							}
							else
							{
								if (num == -3)
								{
									EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_VerMismatch, new string[0]));
								}
								else
								{
									if (num == -4)
									{
										EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_waitready, new string[0]));
									}
									else
									{
										this.m_userName = this.tbuserId.Text;
										this.m_psw = this.tbpassword.Text;
										EcoLanguage.ChangeLang(this.cbolanguage.SelectedIndex);
										base.Close();
									}
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
				this.m_forceClose = -1;
				base.Close();
			}
		}
		private void butcancel_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}
		private void cbolanguage_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int selectedIndex = this.cbolanguage.SelectedIndex;
			EcoLanguage.ChangeLang(selectedIndex);
			System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(base.GetType());
			this.Text = resourceManager.GetString("$this.Text");
			this.label8.Text = resourceManager.GetString("label8.Text");
			this.label7.Text = resourceManager.GetString("label7.Text");
			this.label6.Text = resourceManager.GetString("label6.Text");
			this.butlogin.Text = resourceManager.GetString("butlogin.Text");
			this.butcancel.Text = resourceManager.GetString("butcancel.Text");
		}
		private void Login_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.m_userName != null)
			{
				return;
			}
			if (this.m_forceClose != 0)
			{
				this.m_userName = null;
				return;
			}
			if (EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Login_quit, new string[0]), MessageBoxButtons.YesNo) == DialogResult.No)
			{
				e.Cancel = true;
			}
		}
		private void comm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				if (EcoGlobalVar.ECOAppRunMode == 1)
				{
					this.butlogin.Focus();
					this.butlogin_Click(this.butlogin, null);
					return;
				}
				this.rmtbutLogin.Focus();
				this.rmtbutLogin_Click(this.rmtbutLogin, null);
			}
		}
		private object checkAuth(object param)
		{
			string user_pass_ver = param as string;
			int num = ClientAPI.WaitAuthentication(false, user_pass_ver, false, 30000u);
			return num;
		}
		private void rmtcbLang_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int selectedIndex = this.rmtcbLang.SelectedIndex;
			EcoLanguage.ChangeLang(selectedIndex);
			System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(base.GetType());
			this.Text = resourceManager.GetString("$this.Text");
			this.rmtlbSrvIP.Text = resourceManager.GetString("rmtlbSrvIP.Text");
			this.rmtlbSrvPort.Text = resourceManager.GetString("rmtlbSrvPort.Text");
			this.rmtlbusrnm.Text = resourceManager.GetString("label8.Text");
			this.rmtlbpsw.Text = resourceManager.GetString("label7.Text");
			this.rmtlbLang.Text = resourceManager.GetString("label6.Text");
			this.rmtbutLogin.Text = resourceManager.GetString("butlogin.Text");
			this.rmtbutcancel.Text = resourceManager.GetString("butcancel.Text");
		}
		private void rmtbutLogin_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			this.m_forceClose = 0;
			try
			{
				Ecovalidate.checkTextIsNull(this.rmttbSrvIP, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.rmtlbSrvIP.Text
					}));
				}
				else
				{
					try
					{
						string text = IPAddress.Parse(this.rmttbSrvIP.Text).ToString();
						this.rmttbSrvIP.Text = text;
					}
					catch (System.Exception)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
						this.rmttbSrvIP.Focus();
						return;
					}
					Ecovalidate.checkTextIsNull(this.rmttbSrvPort, ref flag);
					if (flag)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
						{
							this.rmtlbSrvPort.Text
						}));
					}
					else
					{
						if (!Ecovalidate.Rangeint(this.rmttbSrvPort, 1, 65535))
						{
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
							{
								this.rmtlbSrvPort.Text,
								"1",
								"65535"
							}));
						}
						else
						{
							if (this.rmttbusrnm.Text == "")
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_needname, new string[0]));
								this.rmttbusrnm.Focus();
							}
							else
							{
								if (this.rmttbpsw.Text == "")
								{
									EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_needpsw, new string[0]));
									this.rmttbpsw.Focus();
								}
								else
								{
									ValuePairs.setValuePair("MasterIP", this.rmttbSrvIP.Text);
									string text2 = this.rmttbSrvPort.Text;
									ValuePairs.setValuePair("ServicePort", text2);
									ValuePairs.setValuePair("Username", this.rmttbusrnm.Text);
									System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
									string serialNo = ValuePairs.getSerialNo(true);
									string param = string.Concat(new object[]
									{
										this.rmttbusrnm.Text,
										"\n",
										this.rmttbpsw.Text,
										"\n",
										version,
										"\nMaster\nRemote\n",
										serialNo
									});
									progressPopup progressPopup = new progressPopup("Login", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_Login, new string[0]), Resources.login_background, new progressPopup.ProcessInThread(this.checkAuth), param, 0);
									progressPopup.StartPosition = FormStartPosition.CenterScreen;
									progressPopup.ShowDialog();
									object return_V = progressPopup.Return_V;
									int? num = return_V as int?;
									if (num == -1)
									{
										EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvConnectFail, new string[0]));
										this.m_forceClose = -1;
										base.Close();
									}
									else
									{
										if (num == -2)
										{
											EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_loginfail, new string[0]));
										}
										else
										{
											if (num == -3)
											{
												EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Login_VerMismatch, new string[0]));
											}
											else
											{
												if (num == -4)
												{
													EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_waitready, new string[0]));
												}
												else
												{
													ValuePairs.setValuePair("MasterIP", this.rmttbSrvIP.Text);
													ValuePairs.setValuePair("ServicePort", this.rmttbSrvPort.Text);
													ValuePairs.SaveValueKeyToRegistry(false);
													this.m_userName = this.rmttbusrnm.Text;
													this.m_psw = this.rmttbpsw.Text;
													EcoLanguage.ChangeLang(this.rmtcbLang.SelectedIndex);
													base.Close();
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
				this.m_forceClose = -1;
				base.Close();
			}
		}
		private void lbSetting_Click(object sender, System.EventArgs e)
		{
		}
	}
}
