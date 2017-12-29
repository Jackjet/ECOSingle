using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManPage.DataGroup
{
	public class DataGroup : UserControl
	{
		private IContainer components;
		private GroupBox groupBox2;
		private ListView itemlist;
		private ColumnHeader gptype;
		private Button btnAdd;
		private GroupBox groupBox1;
		private Button btnDel;
		private ListView grouplist;
		private ColumnHeader gpname;
		private DataGridView dgvGpmember;
		private Button btnModify;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataGroup));
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.groupBox2 = new GroupBox();
			this.dgvGpmember = new DataGridView();
			this.itemlist = new ListView();
			this.gptype = new ColumnHeader();
			this.btnAdd = new Button();
			this.groupBox1 = new GroupBox();
			this.btnDel = new Button();
			this.grouplist = new ListView();
			this.gpname = new ColumnHeader();
			this.btnModify = new Button();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.dgvGpmember).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox2.Controls.Add(this.dgvGpmember);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.dgvGpmember.AllowUserToAddRows = false;
			this.dgvGpmember.AllowUserToDeleteRows = false;
			this.dgvGpmember.AllowUserToResizeColumns = false;
			this.dgvGpmember.AllowUserToResizeRows = false;
			this.dgvGpmember.BackgroundColor = Color.White;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvGpmember.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvGpmember, "dgvGpmember");
			this.dgvGpmember.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvGpmember.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvGpmember.MultiSelect = false;
			this.dgvGpmember.Name = "dgvGpmember";
			this.dgvGpmember.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvGpmember.RowHeadersVisible = false;
			this.dgvGpmember.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvGpmember.RowTemplate.ReadOnly = true;
			this.dgvGpmember.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvGpmember.StandardTab = true;
			this.dgvGpmember.TabStop = false;
			this.itemlist.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.itemlist, "itemlist");
			this.itemlist.FullRowSelect = true;
			this.itemlist.MultiSelect = false;
			this.itemlist.Name = "itemlist";
			this.itemlist.UseCompatibleStateImageBehavior = false;
			this.itemlist.View = View.Details;
			componentResourceManager.ApplyResources(this.gptype, "gptype");
			this.btnAdd.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.ForeColor = SystemColors.ControlText;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			this.groupBox1.Controls.Add(this.btnModify);
			this.groupBox1.Controls.Add(this.btnAdd);
			this.groupBox1.Controls.Add(this.btnDel);
			this.groupBox1.Controls.Add(this.grouplist);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.btnDel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnDel, "btnDel");
			this.btnDel.ForeColor = SystemColors.ControlText;
			this.btnDel.Name = "btnDel";
			this.btnDel.UseVisualStyleBackColor = false;
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			this.grouplist.BackColor = Color.White;
			this.grouplist.Columns.AddRange(new ColumnHeader[]
			{
				this.gptype,
				this.gpname
			});
			componentResourceManager.ApplyResources(this.grouplist, "grouplist");
			this.grouplist.FullRowSelect = true;
			this.grouplist.HideSelection = false;
			this.grouplist.MultiSelect = false;
			this.grouplist.Name = "grouplist";
			this.grouplist.UseCompatibleStateImageBehavior = false;
			this.grouplist.View = View.Details;
			this.grouplist.SelectedIndexChanged += new System.EventHandler(this.grouplist_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.gpname, "gpname");
			this.btnModify.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnModify, "btnModify");
			this.btnModify.ForeColor = SystemColors.ControlText;
			this.btnModify.Name = "btnModify";
			this.btnModify.UseVisualStyleBackColor = false;
			this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.itemlist);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DataGroup";
			this.groupBox2.ResumeLayout(false);
			((ISupportInitialize)this.dgvGpmember).EndInit();
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public DataGroup()
		{
			this.InitializeComponent();
		}
		public void pageInit()
		{
			this.FillList();
		}
		private void FillList()
		{
			this.grouplist.Items.Clear();
			System.Collections.Generic.List<GroupInfo> partGroup = GroupInfo.GetPartGroup(2);
			foreach (GroupInfo current in partGroup)
			{
				ListViewItem listViewItem = new ListViewItem();
				string skey = "";
				string groupType;
				switch (groupType = current.GroupType)
				{
				case "zone":
					skey = LangRes.Group_TPZone;
					break;
				case "rack":
				case "allrack":
					skey = LangRes.Group_TPRack;
					break;
				case "dev":
				case "alldev":
					skey = LangRes.Group_TPDev;
					break;
				case "outlet":
				case "alloutlet":
					skey = LangRes.Group_TPOutlet;
					break;
				}
				listViewItem.Text = EcoLanguage.getMsg(skey, new string[0]);
				listViewItem.SubItems.Add(current.GroupName);
				listViewItem.Tag = current.ID.ToString() + "|" + current.GroupType;
				this.grouplist.Items.Add(listViewItem);
			}
			if (this.grouplist.Items.Count > 0)
			{
				this.grouplist.Items[0].Selected = true;
				this.grouplist.Select();
				return;
			}
			this.btnDel.Enabled = false;
			this.btnModify.Enabled = false;
		}
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			DataGroupModifyDlg dataGroupModifyDlg = new DataGroupModifyDlg(this, -1L);
			if (dataGroupModifyDlg.ShowDialog() != DialogResult.Cancel)
			{
				this.FillList();
			}
		}
		private void grouplist_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.grouplist.SelectedItems.Count <= 0)
			{
				this.btnDel.Enabled = false;
				this.btnModify.Enabled = false;
				return;
			}
			ListViewItem listViewItem = this.grouplist.SelectedItems[0];
			string value = System.Convert.ToString(listViewItem.Tag).Split(new char[]
			{
				'|'
			})[0];
			string text = System.Convert.ToString(listViewItem.Tag).Split(new char[]
			{
				'|'
			})[1];
			if (text.Length == 0)
			{
				return;
			}
			DataTable memberNameInfo = GroupInfo.GetMemberNameInfo((long)System.Convert.ToInt32(value));
			DataTable dataTable = new DataTable();
			string key;
			switch (key = text)
			{
			case "zone":
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
				break;
			case "rack":
			case "allrack":
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
				break;
			case "dev":
			case "alldev":
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
				break;
			case "outlet":
			case "alloutlet":
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMOutlet, new string[0]), typeof(string));
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
				break;
			}
			string a;
			if ((a = text) != null && a == "zone")
			{
				System.Collections.IEnumerator enumerator = memberNameInfo.Rows.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DataRow dataRow = (DataRow)enumerator.Current;
						dataTable.Rows.Add(new object[]
						{
							dataRow["name"]
						});
					}
					goto IL_35D;
				}
				finally
				{
					System.IDisposable disposable = enumerator as System.IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			foreach (DataRow dataRow2 in memberNameInfo.Rows)
			{
				dataTable.Rows.Add(new object[]
				{
					dataRow2["name"],
					dataRow2["parentname"]
				});
			}
			IL_35D:
			this.dgvGpmember.DataSource = null;
			this.dgvGpmember.DataSource = dataTable;
			this.dgvGpmember.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			string a2;
			if ((a2 = text) != null && a2 == "zone")
			{
				this.dgvGpmember.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			else
			{
				this.dgvGpmember.Columns[0].Width = 140;
				this.dgvGpmember.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			if (text.Equals("allrack") || text.Equals("alldev") || text.Equals("alloutlet"))
			{
				this.btnDel.Enabled = false;
				this.btnModify.Enabled = false;
				return;
			}
			this.btnDel.Enabled = true;
			this.btnModify.Enabled = true;
		}
		private void btnDel_Click(object sender, System.EventArgs e)
		{
			if (this.grouplist.SelectedItems.Count <= 0)
			{
				return;
			}
			if (EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Group_delCfm, new string[0]), MessageBoxButtons.OKCancel).Equals(DialogResult.Cancel))
			{
				return;
			}
			string value = this.grouplist.SelectedItems[0].Tag.ToString().Split(new char[]
			{
				'|'
			})[0];
			string text = this.grouplist.SelectedItems[0].SubItems[0].Text;
			string text2 = this.grouplist.SelectedItems[0].SubItems[1].Text;
			int num = GroupInfo.DeleteGroupByID((long)System.Convert.ToInt32(value));
			if (num > 0)
			{
				LogAPI.writeEventLog("0430031", new string[]
				{
					text2,
					text,
					EcoGlobalVar.gl_LoginUser.UserName
				});
				EcoGlobalVar.setDashBoardFlg(520uL, "", 64);
				this.FillList();
				if (this.grouplist.Items.Count == 0)
				{
					this.btnDel.Enabled = false;
					this.btnModify.Enabled = false;
					this.itemlist.Columns.Clear();
					this.itemlist.Items.Clear();
					return;
				}
			}
			else
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
		}
		private void btnModify_Click(object sender, System.EventArgs e)
		{
			if (this.grouplist.SelectedItems.Count <= 0)
			{
				this.btnDel.Enabled = false;
				this.btnModify.Enabled = false;
				return;
			}
			ListViewItem listViewItem = this.grouplist.SelectedItems[0];
			string value = System.Convert.ToString(listViewItem.Tag).Split(new char[]
			{
				'|'
			})[0];
			DataGroupModifyDlg dataGroupModifyDlg = new DataGroupModifyDlg(this, (long)System.Convert.ToInt32(value));
			if (dataGroupModifyDlg.ShowDialog() != DialogResult.Cancel)
			{
				this.FillList();
			}
		}
	}
}
