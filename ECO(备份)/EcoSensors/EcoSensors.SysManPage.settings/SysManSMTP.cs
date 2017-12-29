using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class SysManSMTP : UserControl
	{
		private IContainer components;
		private GroupBox groupBox1;
		private ComboBox cboevent;
		private Label label7;
		private TextBox txtto;
		private Label lbto;
		private TextBox txtpwd;
		private Label lbpwd;
		private TextBox txtaccount;
		private Label lbaccount;
		private CheckBox chkauth;
		private TextBox txtfrom;
		private Label lbfrom;
		private TextBox txtport;
		private Label lbport;
		private TextBox txtserver;
		private Label lbserver;
		private CheckBox chkenablesmtp;
		private Button btnsave;
		private SMTPSetting m_pSMTPpara;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManSMTP));
			this.groupBox1 = new GroupBox();
			this.cboevent = new ComboBox();
			this.label7 = new Label();
			this.txtto = new TextBox();
			this.lbto = new Label();
			this.txtpwd = new TextBox();
			this.lbpwd = new Label();
			this.txtaccount = new TextBox();
			this.lbaccount = new Label();
			this.chkauth = new CheckBox();
			this.txtfrom = new TextBox();
			this.lbfrom = new Label();
			this.txtport = new TextBox();
			this.lbport = new Label();
			this.txtserver = new TextBox();
			this.lbserver = new Label();
			this.chkenablesmtp = new CheckBox();
			this.btnsave = new Button();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.cboevent);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.txtto);
			this.groupBox1.Controls.Add(this.lbto);
			this.groupBox1.Controls.Add(this.txtpwd);
			this.groupBox1.Controls.Add(this.lbpwd);
			this.groupBox1.Controls.Add(this.txtaccount);
			this.groupBox1.Controls.Add(this.lbaccount);
			this.groupBox1.Controls.Add(this.chkauth);
			this.groupBox1.Controls.Add(this.txtfrom);
			this.groupBox1.Controls.Add(this.lbfrom);
			this.groupBox1.Controls.Add(this.txtport);
			this.groupBox1.Controls.Add(this.lbport);
			this.groupBox1.Controls.Add(this.txtserver);
			this.groupBox1.Controls.Add(this.lbserver);
			this.groupBox1.Controls.Add(this.chkenablesmtp);
			this.groupBox1.Controls.Add(this.btnsave);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.cboevent.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cboevent, "cboevent");
			this.cboevent.ForeColor = SystemColors.ControlText;
			this.cboevent.FormattingEnabled = true;
			this.cboevent.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboevent.Items"),
				componentResourceManager.GetString("cboevent.Items1"),
				componentResourceManager.GetString("cboevent.Items2")
			});
			this.cboevent.Name = "cboevent";
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = SystemColors.ControlText;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.txtto, "txtto");
			this.txtto.ForeColor = SystemColors.ControlText;
			this.txtto.Name = "txtto";
			componentResourceManager.ApplyResources(this.lbto, "lbto");
			this.lbto.ForeColor = SystemColors.ControlText;
			this.lbto.Name = "lbto";
			componentResourceManager.ApplyResources(this.txtpwd, "txtpwd");
			this.txtpwd.ForeColor = SystemColors.ControlText;
			this.txtpwd.Name = "txtpwd";
			this.txtpwd.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.lbpwd, "lbpwd");
			this.lbpwd.ForeColor = SystemColors.ControlText;
			this.lbpwd.Name = "lbpwd";
			componentResourceManager.ApplyResources(this.txtaccount, "txtaccount");
			this.txtaccount.ForeColor = SystemColors.ControlText;
			this.txtaccount.Name = "txtaccount";
			this.lbaccount.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.lbaccount, "lbaccount");
			this.lbaccount.ForeColor = SystemColors.ControlText;
			this.lbaccount.Name = "lbaccount";
			componentResourceManager.ApplyResources(this.chkauth, "chkauth");
			this.chkauth.ForeColor = SystemColors.ControlText;
			this.chkauth.Name = "chkauth";
			this.chkauth.UseVisualStyleBackColor = true;
			this.chkauth.CheckedChanged += new System.EventHandler(this.chkauth_CheckedChanged);
			componentResourceManager.ApplyResources(this.txtfrom, "txtfrom");
			this.txtfrom.ForeColor = SystemColors.ControlText;
			this.txtfrom.Name = "txtfrom";
			componentResourceManager.ApplyResources(this.lbfrom, "lbfrom");
			this.lbfrom.ForeColor = SystemColors.ControlText;
			this.lbfrom.Name = "lbfrom";
			componentResourceManager.ApplyResources(this.txtport, "txtport");
			this.txtport.ForeColor = SystemColors.ControlText;
			this.txtport.Name = "txtport";
			this.txtport.KeyPress += new KeyPressEventHandler(this.txtport_KeyPress);
			componentResourceManager.ApplyResources(this.lbport, "lbport");
			this.lbport.ForeColor = SystemColors.ControlText;
			this.lbport.Name = "lbport";
			componentResourceManager.ApplyResources(this.txtserver, "txtserver");
			this.txtserver.ForeColor = SystemColors.ControlText;
			this.txtserver.Name = "txtserver";
			this.txtserver.KeyPress += new KeyPressEventHandler(this.txtserver_KeyPress);
			componentResourceManager.ApplyResources(this.lbserver, "lbserver");
			this.lbserver.ForeColor = SystemColors.ControlText;
			this.lbserver.Name = "lbserver";
			componentResourceManager.ApplyResources(this.chkenablesmtp, "chkenablesmtp");
			this.chkenablesmtp.ForeColor = SystemColors.ControlText;
			this.chkenablesmtp.Name = "chkenablesmtp";
			this.chkenablesmtp.UseVisualStyleBackColor = true;
			this.chkenablesmtp.CheckedChanged += new System.EventHandler(this.chkenablesmtp_CheckedChanged);
			this.btnsave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsave, "btnsave");
			this.btnsave.ForeColor = SystemColors.ControlText;
			this.btnsave.Name = "btnsave";
			this.btnsave.UseVisualStyleBackColor = false;
			this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysManSMTP";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
		public SysManSMTP()
		{
			this.InitializeComponent();
			this.txtserver.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtport.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtfrom.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtto.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtaccount.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtpwd.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit()
		{
			this.m_pSMTPpara = new SMTPSetting();
			this.txtserver.Text = this.m_pSMTPpara.ServerIP;
			this.txtport.Text = System.Convert.ToString(this.m_pSMTPpara.Port);
			this.txtfrom.Text = this.m_pSMTPpara.Sender;
			this.txtto.Text = this.m_pSMTPpara.Receiver;
			this.txtaccount.Text = this.m_pSMTPpara.AccountName;
			this.txtpwd.Text = this.m_pSMTPpara.AccountPwd;
			string eVENT = this.m_pSMTPpara.EVENT;
			if (eVENT == "")
			{
				this.cboevent.SelectedIndex = 0;
			}
			else
			{
				if (eVENT == "All")
				{
					this.cboevent.SelectedIndex = 0;
				}
				else
				{
					if (eVENT == "Event")
					{
						this.cboevent.SelectedIndex = 1;
					}
					else
					{
						if (eVENT == "Severity")
						{
							this.cboevent.SelectedIndex = 2;
						}
					}
				}
			}
			if (this.m_pSMTPpara.Status != 1)
			{
				this.chkenablesmtp.Checked = false;
				this.txtserver.Enabled = false;
				this.txtport.Enabled = false;
				this.txtfrom.Enabled = false;
				this.txtto.Enabled = false;
				this.cboevent.Enabled = false;
				this.chkauth.Enabled = false;
				this.chkauth.Checked = false;
				this.txtaccount.Enabled = false;
				this.txtpwd.Enabled = false;
				return;
			}
			this.chkenablesmtp.Checked = true;
			this.txtserver.Enabled = true;
			this.txtport.Enabled = true;
			this.txtfrom.Enabled = true;
			this.txtto.Enabled = true;
			this.cboevent.Enabled = true;
			this.chkauth.Enabled = true;
			if (this.m_pSMTPpara.AuthenticationFlag == 1)
			{
				this.chkauth.Checked = true;
				this.txtaccount.Enabled = true;
				this.txtpwd.Enabled = true;
				return;
			}
			this.chkauth.Checked = false;
			this.txtaccount.Enabled = false;
			this.txtpwd.Enabled = false;
		}
		private void txtport_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\b' && (e.KeyChar > '9' || e.KeyChar < '0'))
			{
				e.Handled = true;
			}
		}
		private void chkenablesmtp_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.chkenablesmtp.Checked)
			{
				this.txtserver.Enabled = false;
				this.txtport.Enabled = false;
				this.txtfrom.Enabled = false;
				this.txtto.Enabled = false;
				this.cboevent.Enabled = false;
				this.chkauth.Enabled = false;
				this.txtaccount.Enabled = false;
				this.txtpwd.Enabled = false;
				return;
			}
			this.txtserver.Enabled = true;
			this.txtport.Enabled = true;
			this.txtfrom.Enabled = true;
			this.txtto.Enabled = true;
			this.cboevent.Enabled = true;
			this.chkauth.Enabled = true;
			if (this.chkauth.Checked)
			{
				this.txtaccount.Enabled = true;
				this.txtpwd.Enabled = true;
				return;
			}
			this.txtaccount.Enabled = false;
			this.txtpwd.Enabled = false;
		}
		private void chkauth_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.chkauth.Checked)
			{
				this.txtaccount.Enabled = true;
				this.txtpwd.Enabled = true;
				return;
			}
			this.txtaccount.Enabled = false;
			this.txtpwd.Enabled = false;
		}
		private void btnsave_Click(object sender, System.EventArgs e)
		{
			int status = 0;
			string eVENT = "";
			int authenticationFlag = 0;
			string text = this.txtto.Text.Replace("'", "''");
			if (this.cboevent.SelectedIndex == 0)
			{
				eVENT = "All";
			}
			else
			{
				if (this.cboevent.SelectedIndex == 1)
				{
					eVENT = "Event";
				}
				else
				{
					if (this.cboevent.SelectedIndex == 2)
					{
						eVENT = "Severity";
					}
				}
			}
			bool flag = false;
			if (this.chkenablesmtp.Checked)
			{
				status = 1;
				Ecovalidate.checkTextIsNull(this.txtserver, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbserver.Text
					}));
					return;
				}
				Ecovalidate.checkTextIsNull(this.txtport, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbport.Text
					}));
					return;
				}
				if (!Ecovalidate.Rangeint(this.txtport, 1, 65535))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.lbport.Text,
						"1",
						"65535"
					}));
					return;
				}
				Ecovalidate.checkTextIsNull(this.txtfrom, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbfrom.Text
					}));
					return;
				}
				Ecovalidate.checkTextIsNull(this.txtto, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbto.Text
					}));
					return;
				}
				if (!Ecovalidate.validEmail(this.txtfrom.Text))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.validEmail, new string[0]));
					this.txtfrom.Focus();
					return;
				}
				if (text.Length < 5)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.validEmail, new string[0]));
					this.txtto.Focus();
					return;
				}
				while (text.Substring(text.Length - 2) == "\r\n")
				{
					text = text.Substring(0, text.Length - 2);
				}
				string[] array = text.Split(new string[]
				{
					"\r\n",
					";",
					",",
					" "
				}, System.StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (text2 != "" && !Ecovalidate.validEmail(text2))
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.validEmail, new string[0]));
						this.txtto.Focus();
						return;
					}
				}
			}
			if (this.chkauth.Checked)
			{
				authenticationFlag = 1;
				Ecovalidate.checkTextIsNull(this.txtaccount, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbaccount.Text
					}));
					return;
				}
				Ecovalidate.checkTextIsNull(this.txtpwd, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbpwd.Text
					}));
					return;
				}
			}
			this.m_pSMTPpara.Status = status;
			this.m_pSMTPpara.ServerIP = this.txtserver.Text;
			this.m_pSMTPpara.Port = System.Convert.ToInt32(this.txtport.Text);
			this.m_pSMTPpara.Sender = this.txtfrom.Text;
			this.m_pSMTPpara.Receiver = this.txtto.Text;
			this.m_pSMTPpara.EVENT = eVENT;
			this.m_pSMTPpara.AuthenticationFlag = authenticationFlag;
			this.m_pSMTPpara.AccountName = this.txtaccount.Text;
			this.m_pSMTPpara.AccountPwd = this.txtpwd.Text;
			int num = this.m_pSMTPpara.UpdateSetting();
			if (num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0130021", new string[]
				{
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0130021", new string[0]);
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private void txtserver_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == '.' || keyChar == '-')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
	}
}
