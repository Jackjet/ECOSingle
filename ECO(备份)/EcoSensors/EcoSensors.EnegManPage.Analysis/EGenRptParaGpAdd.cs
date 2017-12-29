using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.Analysis
{
	public class EGenRptParaGpAdd : Form
	{
		private int m_existgpnum;
		private IContainer components;
		private Button btnadd;
		private Button btncancel;
		private ListView grouplist;
		private ColumnHeader gptype;
		private ColumnHeader gpname;
		public EGenRptParaGpAdd(int existgpnum)
		{
			this.InitializeComponent();
			this.m_existgpnum = existgpnum;
			this.grouplist.Items.Clear();
			System.Collections.Generic.List<GroupInfo> partGroup = GroupInfo.GetPartGroup(0);
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
				listViewItem.Tag = current.ID.ToString();
				this.grouplist.Items.Add(listViewItem);
			}
		}
		private void btnadd_Click(object sender, System.EventArgs e)
		{
			string text = "";
			int count = this.grouplist.SelectedItems.Count;
			if (this.m_existgpnum + count > 4)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Group_selmaxnum, new string[0]));
				this.grouplist.Focus();
				return;
			}
			foreach (ListViewItem listViewItem in this.grouplist.SelectedItems)
			{
				text = text + listViewItem.Tag + ",";
			}
			if (text.Length == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_selectneed, new string[0]));
				return;
			}
			text = text.Substring(0, text.Length - 1);
			GroupInfo.UpdateGroupStatus(1, text);
			base.Close();
			base.Dispose();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EGenRptParaGpAdd));
			this.btnadd = new Button();
			this.btncancel = new Button();
			this.grouplist = new ListView();
			this.gptype = new ColumnHeader();
			this.gpname = new ColumnHeader();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnadd, "btnadd");
			this.btnadd.Name = "btnadd";
			this.btnadd.UseVisualStyleBackColor = true;
			this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
			this.btncancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btncancel, "btncancel");
			this.btncancel.Name = "btncancel";
			this.btncancel.UseVisualStyleBackColor = true;
			this.grouplist.BackColor = Color.White;
			this.grouplist.Columns.AddRange(new ColumnHeader[]
			{
				this.gptype,
				this.gpname
			});
			componentResourceManager.ApplyResources(this.grouplist, "grouplist");
			this.grouplist.FullRowSelect = true;
			this.grouplist.Name = "grouplist";
			this.grouplist.UseCompatibleStateImageBehavior = false;
			this.grouplist.View = View.Details;
			componentResourceManager.ApplyResources(this.gptype, "gptype");
			componentResourceManager.ApplyResources(this.gpname, "gpname");
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.btncancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnadd);
			base.Controls.Add(this.btncancel);
			base.Controls.Add(this.grouplist);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "EGenRptParaGpAdd";
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
		}
	}
}
