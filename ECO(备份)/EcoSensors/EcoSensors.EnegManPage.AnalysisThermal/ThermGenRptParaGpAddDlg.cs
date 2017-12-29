using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.AnalysisThermal
{
	public class ThermGenRptParaGpAddDlg : Form
	{
		private int m_existgpnum;
		private IContainer components;
		private Button btnadd;
		private ColumnHeader gptype;
		private Button btncancel;
		private ColumnHeader gpname;
		private ListView grouplist;
		public ThermGenRptParaGpAddDlg(int existgpnum)
		{
			this.InitializeComponent();
			this.m_existgpnum = existgpnum;
			this.grouplist.Items.Clear();
			System.Collections.Generic.List<GroupInfo> groupByThermalFlag = GroupInfo.GetGroupByThermalFlag(0);
			foreach (GroupInfo current in groupByThermalFlag)
			{
				ListViewItem listViewItem = new ListViewItem();
				string text = "";
				string groupType;
				if ((groupType = current.GroupType) != null)
				{
					if (!(groupType == "zone"))
					{
						if (groupType == "rack" || groupType == "allrack")
						{
							text = LangRes.Group_TPRack;
						}
					}
					else
					{
						text = LangRes.Group_TPZone;
					}
				}
				if (text.Length != 0)
				{
					listViewItem.Text = EcoLanguage.getMsg(text, new string[0]);
					listViewItem.SubItems.Add(current.GroupName);
					listViewItem.Tag = current.ID.ToString();
					this.grouplist.Items.Add(listViewItem);
				}
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
			GroupInfo.UpdateGroupThermalFlag(1, text);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ThermGenRptParaGpAddDlg));
			this.btnadd = new Button();
			this.gptype = new ColumnHeader();
			this.btncancel = new Button();
			this.gpname = new ColumnHeader();
			this.grouplist = new ListView();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.btnadd, "btnadd");
			this.btnadd.Name = "btnadd";
			this.btnadd.UseVisualStyleBackColor = true;
			this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
			componentResourceManager.ApplyResources(this.gptype, "gptype");
			this.btncancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btncancel, "btncancel");
			this.btncancel.Name = "btncancel";
			this.btncancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.gpname, "gpname");
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
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.btnadd);
			base.Controls.Add(this.btncancel);
			base.Controls.Add(this.grouplist);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ThermGenRptParaGpAddDlg";
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
		}
	}
}
