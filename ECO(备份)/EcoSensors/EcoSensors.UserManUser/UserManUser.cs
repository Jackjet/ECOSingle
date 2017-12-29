using CommonAPI.Global;
using DBAccessAPI.user;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.UserManPage;
using EventLogAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.UserManUser
{
	public class UserManUser : UserControl
	{
		private string m_deviceIds = string.Empty;
		private string m_groupIds = string.Empty;
		private UserInfo m_CurUser;
        private UserManPage.UserManPage m_pParent;
		private bool in_init = true;
		private IContainer components;
		private CheckBox cbSelectAll;
		private CheckBox cbSManagement;
		private Button butSetDevice;
		private CheckBox cbLog;
		private CheckBox cbDManagement;
		private CheckBox cbUManagement;
		private RadioButton rbRole1;
		private GroupBox groupBox19;
		private TextBox tbUserCPwEdit;
		private TextBox tbUserPwEdit;
		private TextBox tbUserNmEdit;
		private Label lbUserCPwEdit;
		private Label lbUserPwEdit;
		private Label lbUserNmEdit;
		private RadioButton rbRole2;
		private Button butUserSave;
		private GroupBox groupBox21;
		private GroupBox groupBox20;
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public UserManUser()
		{
			this.InitializeComponent();
			this.tbUserNmEdit.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbUserPwEdit.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbUserCPwEdit.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
        public void pageInit(UserManPage.UserManPage pParent, string userName)
		{
			this.m_pParent = pParent;
			this.in_init = true;
			this.pageInit_1();
			this.m_CurUser = UserMaintain.getUserByName(userName);
			if (this.m_CurUser == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.UsrInfo_nofind, new string[]
				{
					userName
				}));
				this.in_init = false;
				return;
			}
			this.tbUserNmEdit.Text = this.m_CurUser.UserName;
			this.tbUserPwEdit.Text = this.m_CurUser.UserPwd;
			this.tbUserCPwEdit.Text = this.m_CurUser.UserPwd;
			int userType = this.m_CurUser.UserType;
			if (userType == 0)
			{
				this.rbRole2.Checked = true;
				if ((this.m_CurUser.UserRight & 1) != 0)
				{
					this.cbUManagement.Checked = true;
				}
				else
				{
					this.cbUManagement.Checked = false;
				}
				if ((this.m_CurUser.UserRight & 2) != 0)
				{
					this.cbDManagement.Checked = true;
				}
				else
				{
					this.cbDManagement.Checked = false;
				}
				if ((this.m_CurUser.UserRight & 4) != 0)
				{
					this.cbSManagement.Checked = true;
				}
				else
				{
					this.cbSManagement.Checked = false;
				}
				if ((this.m_CurUser.UserRight & 8) != 0)
				{
					this.cbLog.Checked = true;
				}
				else
				{
					this.cbLog.Checked = false;
				}
				this.cbUManagement.Enabled = true;
				this.cbDManagement.Enabled = true;
				this.cbSManagement.Enabled = true;
				this.cbSelectAll.Enabled = true;
				this.cbSelectAll.Checked = false;
				this.butSetDevice.Hide();
			}
			else
			{
				this.rbRole1.Checked = true;
				if ((this.m_CurUser.UserRight & 8) != 0)
				{
					this.cbLog.Checked = true;
				}
				else
				{
					this.cbLog.Checked = false;
				}
				this.cbUManagement.Enabled = false;
				this.cbDManagement.Enabled = false;
				this.cbSManagement.Enabled = false;
				this.cbSelectAll.Enabled = false;
				this.butSetDevice.Show();
			}
			this.m_deviceIds = this.m_CurUser.UserDevice;
			this.m_groupIds = this.m_CurUser.UserGroup;
			this.tbUserNmEdit.Enabled = true;
			this.tbUserPwEdit.Enabled = true;
			this.tbUserCPwEdit.Enabled = true;
			if (userName.Equals("administrator"))
			{
				this.tbUserNmEdit.Enabled = false;
				this.rbRole2.Enabled = false;
				this.rbRole1.Enabled = false;
				this.cbUManagement.Enabled = false;
				this.cbDManagement.Enabled = false;
				this.cbLog.Enabled = false;
				this.cbSManagement.Enabled = false;
				this.cbSelectAll.Enabled = false;
			}
			this.in_init = false;
		}
		private void pageInit_1()
		{
			this.tbUserNmEdit.Text = string.Empty;
			this.tbUserNmEdit.Enabled = true;
			this.tbUserPwEdit.Text = string.Empty;
			this.tbUserCPwEdit.Text = string.Empty;
			this.rbRole1.Checked = true;
			this.cbUManagement.Checked = false;
			this.cbDManagement.Checked = false;
			this.cbLog.Checked = false;
			this.cbSManagement.Checked = false;
			this.cbSelectAll.Checked = false;
			this.rbRole2.Enabled = true;
			this.rbRole1.Enabled = true;
			this.cbUManagement.Enabled = true;
			this.cbDManagement.Enabled = true;
			this.cbLog.Enabled = true;
			this.cbSManagement.Enabled = true;
			this.cbSelectAll.Enabled = true;
			this.butUserSave.Enabled = true;
		}
		private void rbRole1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.in_init)
			{
				return;
			}
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
				this.butSetDevice.Show();
				return;
			}
			this.m_deviceIds = string.Empty;
			this.m_groupIds = string.Empty;
			this.cbUManagement.Enabled = true;
			this.cbDManagement.Enabled = true;
			this.cbSManagement.Enabled = true;
			this.cbSelectAll.Enabled = true;
			this.butSetDevice.Hide();
			this.cbUManagement.Checked = true;
			this.cbDManagement.Checked = true;
			this.cbLog.Checked = true;
			this.cbSManagement.Checked = true;
			this.cbSelectAll.Checked = true;
		}
		private void butSetDevice_Click(object sender, System.EventArgs e)
		{
			UserManUserSetDevDlg userManUserSetDevDlg = new UserManUserSetDevDlg(this.m_deviceIds, this.m_groupIds);
			DialogResult dialogResult = userManUserSetDevDlg.ShowDialog(this);
			if (dialogResult == DialogResult.OK)
			{
				this.m_deviceIds = userManUserSetDevDlg.accDevIDs;
				this.m_groupIds = userManUserSetDevDlg.accGroupIDs;
			}
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
			if (this.tbUserPwEdit.Text != this.tbUserCPwEdit.Text)
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
			string userName = this.m_CurUser.UserName;
			string text = this.tbUserNmEdit.Text;
			string text2 = this.tbUserPwEdit.Text;
			int num = 0;
			int num2;
			if (this.rbRole1.Checked)
			{
				num2 = 1;
				if (this.cbLog.Checked)
				{
					num |= 8;
				}
			}
			else
			{
				num2 = 0;
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
			bool flag = false;
			bool flag2 = false;
			if (!this.m_CurUser.UserName.Equals(text) || !this.m_CurUser.UserPwd.Equals(text2) || this.m_CurUser.UserType != num2 || this.m_CurUser.UserRight != num)
			{
				flag = true;
			}
			if (!this.m_CurUser.UserDevice.Equals(this.m_deviceIds) || !this.m_CurUser.UserGroup.Equals(this.m_groupIds))
			{
				flag2 = true;
			}
			if (!flag && !flag2)
			{
				return;
			}
			this.m_CurUser.UserName = text;
			this.m_CurUser.UserPwd = text2;
			this.m_CurUser.UserType = num2;
			this.m_CurUser.UserRight = num;
			this.m_CurUser.UserDevice = this.m_deviceIds;
			this.m_CurUser.UserGroup = this.m_groupIds;
			switch (this.m_CurUser.UpdateUser())
			{
			case -2:
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.UsrInfo_dup, new string[]
				{
					text
				}));
				return;
			case -1:
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			case 0:
				break;
			case 1:
			{
				if (flag)
				{
					EcoGlobalVar.setDashBoardFlg(8192uL, "USER:" + userName, 128);
				}
				else
				{
					if (flag2)
					{
						EcoGlobalVar.setDashBoardFlg(8uL, "", 0);
					}
				}
				string valuePair = ValuePairs.getValuePair("Username");
				if (!string.IsNullOrEmpty(valuePair))
				{
					LogAPI.writeEventLog("0330002", new string[]
					{
						text,
						valuePair
					});
				}
				else
				{
					LogAPI.writeEventLog("0330002", new string[]
					{
						text
					});
				}
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
				if (!userName.Equals(text))
				{
					this.changeTreeSelect(text);
				}
				break;
			}
			default:
				return;
			}
		}
		private void changeTreeSelect(string userName)
		{
			UserManUser.PostMessage(this.m_pParent.Handle, 63000u, 0, userName);
		}
		private void cbcomm_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!((CheckBox)sender).Checked)
			{
				this.cbSelectAll.Checked = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserManUser));
			this.cbSelectAll = new CheckBox();
			this.cbSManagement = new CheckBox();
			this.butSetDevice = new Button();
			this.cbLog = new CheckBox();
			this.cbDManagement = new CheckBox();
			this.cbUManagement = new CheckBox();
			this.rbRole1 = new RadioButton();
			this.groupBox19 = new GroupBox();
			this.tbUserCPwEdit = new TextBox();
			this.tbUserPwEdit = new TextBox();
			this.tbUserNmEdit = new TextBox();
			this.lbUserCPwEdit = new Label();
			this.lbUserPwEdit = new Label();
			this.lbUserNmEdit = new Label();
			this.rbRole2 = new RadioButton();
			this.butUserSave = new Button();
			this.groupBox21 = new GroupBox();
			this.groupBox20 = new GroupBox();
			this.groupBox19.SuspendLayout();
			this.groupBox21.SuspendLayout();
			this.groupBox20.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cbSelectAll, "cbSelectAll");
			this.cbSelectAll.ForeColor = Color.Black;
			this.cbSelectAll.Name = "cbSelectAll";
			this.cbSelectAll.UseVisualStyleBackColor = true;
			this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
			componentResourceManager.ApplyResources(this.cbSManagement, "cbSManagement");
			this.cbSManagement.ForeColor = Color.Black;
			this.cbSManagement.Name = "cbSManagement";
			this.cbSManagement.UseVisualStyleBackColor = true;
			this.cbSManagement.CheckedChanged += new System.EventHandler(this.cbcomm_CheckedChanged);
			this.butSetDevice.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSetDevice, "butSetDevice");
			this.butSetDevice.Name = "butSetDevice";
			this.butSetDevice.UseVisualStyleBackColor = false;
			this.butSetDevice.Click += new System.EventHandler(this.butSetDevice_Click);
			componentResourceManager.ApplyResources(this.cbLog, "cbLog");
			this.cbLog.ForeColor = Color.Black;
			this.cbLog.Name = "cbLog";
			this.cbLog.UseVisualStyleBackColor = true;
			this.cbLog.CheckedChanged += new System.EventHandler(this.cbcomm_CheckedChanged);
			componentResourceManager.ApplyResources(this.cbDManagement, "cbDManagement");
			this.cbDManagement.ForeColor = Color.Black;
			this.cbDManagement.Name = "cbDManagement";
			this.cbDManagement.UseVisualStyleBackColor = true;
			this.cbDManagement.CheckedChanged += new System.EventHandler(this.cbcomm_CheckedChanged);
			componentResourceManager.ApplyResources(this.cbUManagement, "cbUManagement");
			this.cbUManagement.ForeColor = Color.Black;
			this.cbUManagement.Name = "cbUManagement";
			this.cbUManagement.UseVisualStyleBackColor = true;
			this.cbUManagement.CheckedChanged += new System.EventHandler(this.cbcomm_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbRole1, "rbRole1");
			this.rbRole1.Checked = true;
			this.rbRole1.ForeColor = Color.Black;
			this.rbRole1.Name = "rbRole1";
			this.rbRole1.TabStop = true;
			this.rbRole1.UseVisualStyleBackColor = true;
			this.rbRole1.CheckedChanged += new System.EventHandler(this.rbRole1_CheckedChanged);
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
			componentResourceManager.ApplyResources(this.rbRole2, "rbRole2");
			this.rbRole2.ForeColor = Color.Black;
			this.rbRole2.Name = "rbRole2";
			this.rbRole2.UseVisualStyleBackColor = true;
			this.butUserSave.BackColor = Color.Gainsboro;
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
			this.groupBox20.Controls.Add(this.rbRole1);
			this.groupBox20.Controls.Add(this.rbRole2);
			componentResourceManager.ApplyResources(this.groupBox20, "groupBox20");
			this.groupBox20.ForeColor = SystemColors.ControlText;
			this.groupBox20.Name = "groupBox20";
			this.groupBox20.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butSetDevice);
			base.Controls.Add(this.groupBox19);
			base.Controls.Add(this.butUserSave);
			base.Controls.Add(this.groupBox21);
			base.Controls.Add(this.groupBox20);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "UserManUser";
			this.groupBox19.ResumeLayout(false);
			this.groupBox19.PerformLayout();
			this.groupBox21.ResumeLayout(false);
			this.groupBox21.PerformLayout();
			this.groupBox20.ResumeLayout(false);
			this.groupBox20.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
