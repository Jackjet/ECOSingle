using CommonAPI.Global;
using DBAccessAPI.user;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.UserManPage
{
	public class UserManAddUser : Form
	{
		private IContainer components;
		private Button butCancel;
		private GroupBox groupBox19;
		private TextBox tbUserCPwEdit;
		private TextBox tbUserPwEdit;
		private TextBox tbUserNmEdit;
		private Label lbUserCPwEdit;
		private Label lbUserPwEdit;
		private Label lbUserNmEdit;
		private Button butUserSave;
		private GroupBox groupBox21;
		private CheckBox cbSelectAll;
		private CheckBox cbSManagement;
		private CheckBox cbLog;
		private CheckBox cbDManagement;
		private CheckBox cbUManagement;
		private GroupBox groupBox20;
		private RadioButton rbRole1;
		private RadioButton rbRole2;
		private string m_username = "";
		public string UserNM
		{
			get
			{
				return this.m_username;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserManAddUser));
			this.butCancel = new Button();
			this.groupBox19 = new GroupBox();
			this.tbUserCPwEdit = new TextBox();
			this.tbUserPwEdit = new TextBox();
			this.tbUserNmEdit = new TextBox();
			this.lbUserCPwEdit = new Label();
			this.lbUserPwEdit = new Label();
			this.lbUserNmEdit = new Label();
			this.butUserSave = new Button();
			this.groupBox21 = new GroupBox();
			this.cbSelectAll = new CheckBox();
			this.cbSManagement = new CheckBox();
			this.cbLog = new CheckBox();
			this.cbDManagement = new CheckBox();
			this.cbUManagement = new CheckBox();
			this.groupBox20 = new GroupBox();
			this.rbRole1 = new RadioButton();
			this.rbRole2 = new RadioButton();
			this.groupBox19.SuspendLayout();
			this.groupBox21.SuspendLayout();
			this.groupBox20.SuspendLayout();
			base.SuspendLayout();
			this.butCancel.BackColor = SystemColors.Control;
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = false;
			this.groupBox19.Controls.Add(this.tbUserCPwEdit);
			this.groupBox19.Controls.Add(this.tbUserPwEdit);
			this.groupBox19.Controls.Add(this.tbUserNmEdit);
			this.groupBox19.Controls.Add(this.lbUserCPwEdit);
			this.groupBox19.Controls.Add(this.lbUserPwEdit);
			this.groupBox19.Controls.Add(this.lbUserNmEdit);
			componentResourceManager.ApplyResources(this.groupBox19, "groupBox19");
			this.groupBox19.ForeColor = SystemColors.ControlText;
			this.groupBox19.Name = "groupBox19";
			this.groupBox19.TabStop = false;
			componentResourceManager.ApplyResources(this.tbUserCPwEdit, "tbUserCPwEdit");
			this.tbUserCPwEdit.ForeColor = Color.Black;
			this.tbUserCPwEdit.Name = "tbUserCPwEdit";
			this.tbUserCPwEdit.UseSystemPasswordChar = true;
			this.tbUserCPwEdit.KeyPress += new KeyPressEventHandler(this.password_KeyPress);
			componentResourceManager.ApplyResources(this.tbUserPwEdit, "tbUserPwEdit");
			this.tbUserPwEdit.ForeColor = Color.Black;
			this.tbUserPwEdit.Name = "tbUserPwEdit";
			this.tbUserPwEdit.UseSystemPasswordChar = true;
			this.tbUserPwEdit.KeyPress += new KeyPressEventHandler(this.password_KeyPress);
			componentResourceManager.ApplyResources(this.tbUserNmEdit, "tbUserNmEdit");
			this.tbUserNmEdit.ForeColor = Color.Black;
			this.tbUserNmEdit.Name = "tbUserNmEdit";
			this.tbUserNmEdit.KeyPress += new KeyPressEventHandler(this.UserNm_KeyPress);
			componentResourceManager.ApplyResources(this.lbUserCPwEdit, "lbUserCPwEdit");
			this.lbUserCPwEdit.ForeColor = Color.Black;
			this.lbUserCPwEdit.Name = "lbUserCPwEdit";
			componentResourceManager.ApplyResources(this.lbUserPwEdit, "lbUserPwEdit");
			this.lbUserPwEdit.ForeColor = Color.Black;
			this.lbUserPwEdit.Name = "lbUserPwEdit";
			componentResourceManager.ApplyResources(this.lbUserNmEdit, "lbUserNmEdit");
			this.lbUserNmEdit.ForeColor = Color.Black;
			this.lbUserNmEdit.Name = "lbUserNmEdit";
			this.butUserSave.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.butUserSave, "butUserSave");
			this.butUserSave.Name = "butUserSave";
			this.butUserSave.UseVisualStyleBackColor = false;
			this.butUserSave.Click += new System.EventHandler(this.butUserSave_Click);
			this.groupBox21.Controls.Add(this.cbSelectAll);
			this.groupBox21.Controls.Add(this.cbSManagement);
			this.groupBox21.Controls.Add(this.cbLog);
			this.groupBox21.Controls.Add(this.cbDManagement);
			this.groupBox21.Controls.Add(this.cbUManagement);
			componentResourceManager.ApplyResources(this.groupBox21, "groupBox21");
			this.groupBox21.ForeColor = SystemColors.ControlText;
			this.groupBox21.Name = "groupBox21";
			this.groupBox21.TabStop = false;
			componentResourceManager.ApplyResources(this.cbSelectAll, "cbSelectAll");
			this.cbSelectAll.ForeColor = Color.Black;
			this.cbSelectAll.Name = "cbSelectAll";
			this.cbSelectAll.UseVisualStyleBackColor = true;
			this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
			componentResourceManager.ApplyResources(this.cbSManagement, "cbSManagement");
			this.cbSManagement.ForeColor = Color.Black;
			this.cbSManagement.Name = "cbSManagement";
			this.cbSManagement.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.cbLog, "cbLog");
			this.cbLog.ForeColor = Color.Black;
			this.cbLog.Name = "cbLog";
			this.cbLog.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.cbDManagement, "cbDManagement");
			this.cbDManagement.ForeColor = Color.Black;
			this.cbDManagement.Name = "cbDManagement";
			this.cbDManagement.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.cbUManagement, "cbUManagement");
			this.cbUManagement.ForeColor = Color.Black;
			this.cbUManagement.Name = "cbUManagement";
			this.cbUManagement.UseVisualStyleBackColor = true;
			this.groupBox20.Controls.Add(this.rbRole1);
			this.groupBox20.Controls.Add(this.rbRole2);
			componentResourceManager.ApplyResources(this.groupBox20, "groupBox20");
			this.groupBox20.ForeColor = SystemColors.ControlText;
			this.groupBox20.Name = "groupBox20";
			this.groupBox20.TabStop = false;
			componentResourceManager.ApplyResources(this.rbRole1, "rbRole1");
			this.rbRole1.Checked = true;
			this.rbRole1.ForeColor = Color.Black;
			this.rbRole1.Name = "rbRole1";
			this.rbRole1.TabStop = true;
			this.rbRole1.UseVisualStyleBackColor = true;
			this.rbRole1.CheckedChanged += new System.EventHandler(this.rbRole1_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbRole2, "rbRole2");
			this.rbRole2.ForeColor = Color.Black;
			this.rbRole2.Name = "rbRole2";
			this.rbRole2.UseVisualStyleBackColor = true;
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.groupBox19);
			base.Controls.Add(this.butUserSave);
			base.Controls.Add(this.groupBox21);
			base.Controls.Add(this.groupBox20);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "UserManAddUser";
			base.ShowInTaskbar = false;
			this.groupBox19.ResumeLayout(false);
			this.groupBox19.PerformLayout();
			this.groupBox21.ResumeLayout(false);
			this.groupBox21.PerformLayout();
			this.groupBox20.ResumeLayout(false);
			this.groupBox20.PerformLayout();
			base.ResumeLayout(false);
		}
		public UserManAddUser()
		{
			this.InitializeComponent();
			this.tbUserNmEdit.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbUserPwEdit.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbUserCPwEdit.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		private void rbRole1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (((RadioButton)sender).Checked)
			{
				this.cbUManagement.Checked = false;
				this.cbDManagement.Checked = false;
				this.cbLog.Checked = false;
				this.cbSManagement.Checked = false;
				this.cbSelectAll.Checked = false;
				this.cbUManagement.Enabled = false;
				this.cbDManagement.Enabled = false;
				this.cbSManagement.Enabled = false;
				this.cbSelectAll.Enabled = false;
				return;
			}
			this.cbUManagement.Enabled = true;
			this.cbDManagement.Enabled = true;
			this.cbSManagement.Enabled = true;
			this.cbSelectAll.Enabled = true;
			this.cbUManagement.Checked = true;
			this.cbDManagement.Checked = true;
			this.cbLog.Checked = true;
			this.cbSManagement.Checked = true;
			this.cbSelectAll.Checked = true;
		}
		private bool userCheck()
		{
			string text = this.tbUserNmEdit.Text;
			if (string.IsNullOrEmpty(text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbUserNmEdit.Text
				}));
				this.tbUserNmEdit.Focus();
				return false;
			}
			if (string.IsNullOrEmpty(this.tbUserPwEdit.Text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbUserPwEdit.Text
				}));
				this.tbUserPwEdit.Focus();
				return false;
			}
			if (!this.tbUserPwEdit.Text.Equals(this.tbUserCPwEdit.Text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.match, new string[]
				{
					this.lbUserPwEdit.Text,
					this.lbUserCPwEdit.Text
				}));
				this.tbUserCPwEdit.Focus();
				return false;
			}
			return true;
		}
		private void UserNm_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void password_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == ' ' || keyChar == '_')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void butUserSave_Click(object sender, System.EventArgs e)
		{
			if (!this.userCheck())
			{
				return;
			}
			string text = this.tbUserNmEdit.Text;
			string text2 = this.tbUserPwEdit.Text;
			int num = 0;
			int i_type;
			if (this.rbRole1.Checked)
			{
				i_type = 1;
				if (this.cbLog.Checked)
				{
					num |= 8;
				}
			}
			else
			{
				i_type = 0;
				if (this.cbUManagement.Checked)
				{
					num |= 1;
				}
				if (this.cbDManagement.Checked)
				{
					num |= 2;
				}
				if (this.cbSManagement.Checked)
				{
					num |= 4;
				}
				if (this.cbLog.Checked)
				{
					num |= 8;
				}
			}
			UserInfo userInfo = new UserInfo(0L, text, text2, 1, i_type, num, string.Empty, string.Empty, string.Empty);
			switch (userInfo.InsertUser())
			{
			case -2:
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.UsrInfo_dup, new string[]
				{
					text
				}));
				this.tbUserNmEdit.Focus();
				return;
			case -1:
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.UsrInfo_NewFail, new string[0]));
				base.DialogResult = DialogResult.Cancel;
				break;
			default:
			{
				string valuePair = ValuePairs.getValuePair("Username");
				if (!string.IsNullOrEmpty(valuePair))
				{
					LogAPI.writeEventLog("0330000", new string[]
					{
						text,
						valuePair
					});
				}
				else
				{
					LogAPI.writeEventLog("0330000", new string[]
					{
						text
					});
				}
				this.m_username = text;
				base.DialogResult = DialogResult.OK;
				break;
			}
			}
			base.Close();
			base.Dispose();
		}
		private void cbSelectAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (((CheckBox)sender).Checked)
			{
				this.cbUManagement.Checked = true;
				this.cbDManagement.Checked = true;
				this.cbLog.Checked = true;
				this.cbSManagement.Checked = true;
				this.rbRole2.Checked = true;
			}
		}
	}
}
