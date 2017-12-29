using CommonAPI.Global;
using DBAccessAPI.user;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.UserManPage;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.UserManAllUsers
{
	public class UserManAllUsers : UserControl
	{
		private UserControl m_pParent;
		private IContainer components;
		private DataGridView dgvUserAll;
		private Button butUserDel;
		private Button butUserEdit;
		private Button butUserAdd;
		private DataGridViewTextBoxColumn dgvUserNm;
		private DataGridViewTextBoxColumn dgvRole;
		private DataGridViewTextBoxColumn dgvState;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public UserManAllUsers()
		{
			this.InitializeComponent();
		}
		public void pageInit(UserControl pParent, TreeNode rootNode)
		{
			this.m_pParent = pParent;
			this.dgvUserAll.Rows.Clear();
			foreach (TreeNode treeNode in rootNode.Nodes)
			{
				string text = treeNode.Text;
				string[] array = treeNode.Name.Split(new char[]
				{
					'|'
				});
				System.Convert.ToInt16(array[1]);
				short num = System.Convert.ToInt16(array[0]);
				string msg;
				if (num == 0)
				{
					msg = EcoLanguage.getMsg(LangRes.UsrInfo_TPadmin, new string[0]);
				}
				else
				{
					msg = EcoLanguage.getMsg(LangRes.UsrInfo_TPuser, new string[0]);
				}
				short num2 = System.Convert.ToInt16(array[1]);
				string msg2;
				if (num2 == 1)
				{
					msg2 = EcoLanguage.getMsg(LangRes.UsrInfo_STactive, new string[0]);
				}
				else
				{
					msg2 = EcoLanguage.getMsg(LangRes.UsrInfo_STinactive, new string[0]);
				}
				string[] values = new string[]
				{
					text,
					msg,
					msg2
				};
				this.dgvUserAll.Rows.Add(values);
			}
		}
		private void changeTreeSelect(string userName)
		{
			UserManAllUsers.PostMessage(this.m_pParent.Handle, 63000u, 0, userName);
		}
		private void butUserAdd_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<UserInfo> allUsers = UserMaintain.getAllUsers();
			int num = 8;
			if (allUsers.Count >= num)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.UsrInfo_limit, new string[]
				{
					num.ToString()
				}));
				return;
			}
			UserManAddUser userManAddUser = new UserManAddUser();
			DialogResult dialogResult = userManAddUser.ShowDialog();
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			string userNM = userManAddUser.UserNM;
			this.changeTreeSelect(userNM);
		}
		private void dgvUserAll_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			string userName = this.dgvUserAll.Rows[rowIndex].Cells[0].Value.ToString();
			this.changeTreeSelect(userName);
		}
		private void butUserEdit_Click(object sender, System.EventArgs e)
		{
			string userName = this.dgvUserAll.CurrentRow.Cells[0].Value.ToString();
			this.changeTreeSelect(userName);
		}
		private void butUserDel_Click(object sender, System.EventArgs e)
		{
			string text = this.dgvUserAll.CurrentRow.Cells[0].Value.ToString();
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.UsrInfo_delCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			int num = UserMaintain.DeleteByName(text);
			if (num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.UsrInfo_delfail, new string[]
				{
					text
				}));
				return;
			}
			EcoGlobalVar.setDashBoardFlg(8192uL, "USER:" + text, 128);
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0330001", new string[]
				{
					text,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0330001", new string[]
				{
					text
				});
			}
			this.changeTreeSelect("");
		}
		private void dgvUserAll_SelectionChanged(object sender, System.EventArgs e)
		{
			string text = this.dgvUserAll.CurrentRow.Cells[0].Value.ToString();
			UserInfo gl_LoginUser = EcoGlobalVar.gl_LoginUser;
			if (text.Equals("administrator") || text.Equals(gl_LoginUser.UserName))
			{
				this.butUserDel.Enabled = false;
				return;
			}
			this.butUserDel.Enabled = true;
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserManAllUsers));
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.dgvUserAll = new DataGridView();
			this.butUserDel = new Button();
			this.butUserEdit = new Button();
			this.butUserAdd = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dgvUserNm = new DataGridViewTextBoxColumn();
			this.dgvRole = new DataGridViewTextBoxColumn();
			this.dgvState = new DataGridViewTextBoxColumn();
			((ISupportInitialize)this.dgvUserAll).BeginInit();
			base.SuspendLayout();
			this.dgvUserAll.AllowUserToAddRows = false;
			this.dgvUserAll.AllowUserToDeleteRows = false;
			this.dgvUserAll.AllowUserToResizeColumns = false;
			this.dgvUserAll.AllowUserToResizeRows = false;
			this.dgvUserAll.BackgroundColor = Color.WhiteSmoke;
			this.dgvUserAll.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.Gainsboro;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvUserAll.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvUserAll, "dgvUserAll");
			this.dgvUserAll.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvUserAll.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dgvUserNm,
				this.dgvRole,
				this.dgvState
			});
			this.dgvUserAll.GridColor = Color.White;
			this.dgvUserAll.MultiSelect = false;
			this.dgvUserAll.Name = "dgvUserAll";
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			this.dgvUserAll.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvUserAll.RowHeadersVisible = false;
			this.dgvUserAll.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvUserAll.RowTemplate.Height = 23;
			this.dgvUserAll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvUserAll.StandardTab = true;
			this.dgvUserAll.TabStop = false;
			this.dgvUserAll.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvUserAll_CellDoubleClick);
			this.dgvUserAll.SelectionChanged += new System.EventHandler(this.dgvUserAll_SelectionChanged);
			this.butUserDel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butUserDel, "butUserDel");
			this.butUserDel.Name = "butUserDel";
			this.butUserDel.UseVisualStyleBackColor = false;
			this.butUserDel.Click += new System.EventHandler(this.butUserDel_Click);
			this.butUserEdit.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butUserEdit, "butUserEdit");
			this.butUserEdit.Name = "butUserEdit";
			this.butUserEdit.UseVisualStyleBackColor = false;
			this.butUserEdit.Click += new System.EventHandler(this.butUserEdit_Click);
			this.butUserAdd.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butUserAdd, "butUserAdd");
			this.butUserAdd.Name = "butUserAdd";
			this.butUserAdd.UseVisualStyleBackColor = false;
			this.butUserAdd.Click += new System.EventHandler(this.butUserAdd_Click);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvUserNm, "dgvUserNm");
			this.dgvUserNm.Name = "dgvUserNm";
			this.dgvUserNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvRole, "dgvRole");
			this.dgvRole.Name = "dgvRole";
			this.dgvRole.ReadOnly = true;
			this.dgvState.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dgvState, "dgvState");
			this.dgvState.Name = "dgvState";
			this.dgvState.ReadOnly = true;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.dgvUserAll);
			base.Controls.Add(this.butUserDel);
			base.Controls.Add(this.butUserEdit);
			base.Controls.Add(this.butUserAdd);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "UserManAllUsers";
			((ISupportInitialize)this.dgvUserAll).EndInit();
			base.ResumeLayout(false);
		}
	}
}
