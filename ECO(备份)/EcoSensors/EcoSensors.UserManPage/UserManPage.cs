using DBAccessAPI.user;
using EcoSensors._Lang;
using EcoSensors.UserManAllUsers;
using EcoSensors.UserManUser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.UserManPage
{
	public class UserManPage : UserControl
	{
		public const int MSG_USRTREE_GENSEL = 63000;
		private const string TV_UserRoot = "UserRoot";
		private const string TV_User = "User";
		private IContainer components;
		private FlowLayoutPanel flowLayoutPanelUserManPage;
		private Button butAccount;
		private Panel panel1;
		private SplitContainer splitContainer1;
		private TreeView tvUser;
        private UserManUser.UserManUser userManUser1;
        private UserManAllUsers.UserManAllUsers userManAllUsers1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserManPage));
			this.splitContainer1 = new SplitContainer();
			this.tvUser = new TreeView();
            this.userManAllUsers1 = new UserManAllUsers.UserManAllUsers();
            this.userManUser1 = new UserManUser.UserManUser();
			this.flowLayoutPanelUserManPage = new FlowLayoutPanel();
			this.butAccount = new Button();
			this.panel1 = new Panel();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.flowLayoutPanelUserManPage.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.tvUser);
			this.splitContainer1.Panel2.Controls.Add(this.userManAllUsers1);
			this.splitContainer1.Panel2.Controls.Add(this.userManUser1);
			componentResourceManager.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.tvUser.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.tvUser, "tvUser");
			this.tvUser.HideSelection = false;
			this.tvUser.Name = "tvUser";
			this.tvUser.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("tvUser.Nodes")
			});
			this.tvUser.AfterSelect += new TreeViewEventHandler(this.tvUser_AfterSelect);
			this.userManAllUsers1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.userManAllUsers1, "userManAllUsers1");
			this.userManAllUsers1.Name = "userManAllUsers1";
			this.userManUser1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.userManUser1, "userManUser1");
			this.userManUser1.Name = "userManUser1";
			this.flowLayoutPanelUserManPage.BackColor = Color.FromArgb(18, 160, 143);
			this.flowLayoutPanelUserManPage.Controls.Add(this.butAccount);
			componentResourceManager.ApplyResources(this.flowLayoutPanelUserManPage, "flowLayoutPanelUserManPage");
			this.flowLayoutPanelUserManPage.MinimumSize = new Size(1008, 27);
			this.flowLayoutPanelUserManPage.Name = "flowLayoutPanelUserManPage";
			componentResourceManager.ApplyResources(this.butAccount, "butAccount");
			this.butAccount.MinimumSize = new Size(160, 27);
			this.butAccount.Name = "butAccount";
			this.butAccount.Tag = "Accounts";
			this.butAccount.UseVisualStyleBackColor = true;
			this.butAccount.Click += new System.EventHandler(this.comm_butClick);
			this.panel1.Controls.Add(this.splitContainer1);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.flowLayoutPanelUserManPage);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "UserManPage";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.flowLayoutPanelUserManPage.ResumeLayout(false);
			this.flowLayoutPanelUserManPage.PerformLayout();
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public UserManPage()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			if (selIndex != 0)
			{
				return;
			}
			this.comm_butClick(this.butAccount, null);
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			foreach (Control control in this.flowLayoutPanelUserManPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butAccount.Name))
					{
						this.UsertreeInit();
						this.tvUser.SelectedNode = this.tvUser.Nodes[0];
					}
				}
				else
				{
					((Button)control).Font = new Font(font.FontFamily, font.Size, FontStyle.Regular);
				}
			}
		}
		private void UsertreeInit(string userName)
		{
			this.UsertreeInit();
			foreach (TreeNode treeNode in this.tvUser.Nodes[0].Nodes)
			{
				if (treeNode.Text.Equals(userName))
				{
					this.tvUser.SelectedNode = treeNode;
					return;
				}
			}
			this.tvUser.SelectedNode = this.tvUser.Nodes[0];
		}
		private void UsertreeInit()
		{
			this.tvUser.Nodes.Clear();
			TreeNode treeNode = new TreeNode();
			treeNode.Text = EcoLanguage.getMsg(LangRes.UserRoot, new string[0]);
			treeNode.Name = "UserRoot";
			treeNode.Tag = "UserRoot";
			this.tvUser.Nodes.Add(treeNode);
			System.Collections.Generic.List<UserInfo> allUsers = UserMaintain.getAllUsers();
			for (int i = 0; i < allUsers.Count; i++)
			{
				UserInfo userInfo = allUsers[i];
				string userName = userInfo.UserName;
				TreeNode treeNode2 = new TreeNode();
				treeNode2.Text = userName;
				int userType = userInfo.UserType;
				int userStatus = userInfo.UserStatus;
				treeNode2.Name = userType + "|" + userStatus;
				treeNode2.Tag = "User";
				treeNode.Nodes.Add(treeNode2);
			}
		}
		private void tvUser_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string text = e.Node.Tag.ToString();
			string text2 = e.Node.Text;
			string a;
			if ((a = text) != null)
			{
				if (a == "UserRoot")
				{
					this.userManUser1.Visible = false;
					this.userManAllUsers1.pageInit(this, e.Node);
					this.userManAllUsers1.Visible = true;
					return;
				}
				if (!(a == "User"))
				{
					return;
				}
				this.userManAllUsers1.Visible = false;
				string arg_7F_0 = e.Node.Text;
				this.userManUser1.pageInit(this, text2);
				this.userManUser1.Visible = true;
			}
		}
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 63000)
			{
				string userName = System.Runtime.InteropServices.Marshal.PtrToStringUni(m.LParam);
				this.UsertreeInit(userName);
				return;
			}
			base.DefWndProc(ref m);
		}
	}
}
