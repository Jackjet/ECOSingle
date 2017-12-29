using CommonAPI;
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
	public class ThermGenRptPara : UserControl
	{
		private IContainer components;
		private Button btnGen;
		private Label label14;
		private TextBox txttitle;
		private Label lbwriter;
		private Label label15;
		private DateTimePicker dtpbegin;
		private Label label1;
		private DateTimePicker dtptime;
		private ComboBox cboperiod;
		private TextBox txtwrite;
		private CheckBox chkchart1;
		private CheckBox chkchart3;
		private Label lbtitle;
		private CheckBox chkchart4;
		private CheckBox chkchart2;
		private GroupBox groupBox2;
		private Button btndel;
		private Button btnAdd;
		private ListView grouplist;
		private ColumnHeader gptype;
		private ColumnHeader gpname;
		private ComboBox cboduration;
		private CheckBox chkchart5;
		private GroupBox grpbchart;
		private GroupBox gbxInfo;
		private Label label29;
		private TextBox tbduration;
		private ThermAnalysis m_parent;
		private System.DateTime m_dtbeginlast;
		private int m_oldcboperiodindex;
		public string Txttitle
		{
			get
			{
				return this.txttitle.Text;
			}
		}
		public string Txtwriter
		{
			get
			{
				return this.txtwrite.Text;
			}
		}
		public string Dtptime
		{
			get
			{
				return this.dtptime.Text;
			}
		}
		public int Cboperiod_SelectedIndex
		{
			get
			{
				return this.cboperiod.SelectedIndex;
			}
		}
		public string BeginText
		{
			get
			{
				if (this.cboperiod.SelectedIndex == 0 || this.cboperiod.SelectedIndex == 1 || this.cboperiod.SelectedIndex == 2)
				{
					return this.dtpbegin.Text;
				}
				System.DateTime value = this.dtpbegin.Value;
				int month = value.Month;
				int month2 = (month - 1) * 3 + 1;
				System.DateTime dateTime = new System.DateTime(value.Year, month2, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
				return dateTime.ToString("yyyy-MM");
			}
		}
		public int Cboduration
		{
			get
			{
				if (this.cboperiod.SelectedIndex == 1)
				{
					return (int)System.Convert.ToInt16(this.tbduration.Text);
				}
				return (int)System.Convert.ToInt16(this.cboduration.SelectedItem.ToString());
			}
		}
		public ListView.ListViewItemCollection Grouplist
		{
			get
			{
				return this.grouplist.Items;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ThermGenRptPara));
			this.btnGen = new Button();
			this.label14 = new Label();
			this.txttitle = new TextBox();
			this.lbwriter = new Label();
			this.label15 = new Label();
			this.dtpbegin = new DateTimePicker();
			this.label1 = new Label();
			this.dtptime = new DateTimePicker();
			this.cboperiod = new ComboBox();
			this.txtwrite = new TextBox();
			this.chkchart1 = new CheckBox();
			this.chkchart3 = new CheckBox();
			this.lbtitle = new Label();
			this.chkchart4 = new CheckBox();
			this.chkchart2 = new CheckBox();
			this.groupBox2 = new GroupBox();
			this.btndel = new Button();
			this.btnAdd = new Button();
			this.grouplist = new ListView();
			this.gptype = new ColumnHeader();
			this.gpname = new ColumnHeader();
			this.cboduration = new ComboBox();
			this.chkchart5 = new CheckBox();
			this.grpbchart = new GroupBox();
			this.gbxInfo = new GroupBox();
			this.tbduration = new TextBox();
			this.label29 = new Label();
			this.groupBox2.SuspendLayout();
			this.grpbchart.SuspendLayout();
			this.gbxInfo.SuspendLayout();
			base.SuspendLayout();
			this.btnGen.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnGen, "btnGen");
			this.btnGen.Name = "btnGen";
			this.btnGen.UseVisualStyleBackColor = false;
			this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.ForeColor = Color.Black;
			this.label14.Name = "label14";
			componentResourceManager.ApplyResources(this.txttitle, "txttitle");
			this.txttitle.Name = "txttitle";
			this.txttitle.KeyPress += new KeyPressEventHandler(this.txttitle_KeyPress);
			componentResourceManager.ApplyResources(this.lbwriter, "lbwriter");
			this.lbwriter.ForeColor = Color.Black;
			this.lbwriter.Name = "lbwriter";
			componentResourceManager.ApplyResources(this.label15, "label15");
			this.label15.ForeColor = Color.Black;
			this.label15.Name = "label15";
			componentResourceManager.ApplyResources(this.dtpbegin, "dtpbegin");
			this.dtpbegin.Format = DateTimePickerFormat.Custom;
			this.dtpbegin.Name = "dtpbegin";
			this.dtpbegin.ShowUpDown = true;
			this.dtpbegin.ValueChanged += new System.EventHandler(this.dtpbegin_ValueChanged);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Black;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.dtptime, "dtptime");
			this.dtptime.Format = DateTimePickerFormat.Custom;
			this.dtptime.Name = "dtptime";
			this.dtptime.ShowUpDown = true;
			this.cboperiod.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cboperiod, "cboperiod");
			this.cboperiod.FormattingEnabled = true;
			this.cboperiod.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboperiod.Items"),
				componentResourceManager.GetString("cboperiod.Items1"),
				componentResourceManager.GetString("cboperiod.Items2"),
				componentResourceManager.GetString("cboperiod.Items3")
			});
			this.cboperiod.Name = "cboperiod";
			this.cboperiod.SelectedIndexChanged += new System.EventHandler(this.cboperiod_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.txtwrite, "txtwrite");
			this.txtwrite.Name = "txtwrite";
			this.txtwrite.KeyPress += new KeyPressEventHandler(this.txtwrite_KeyPress);
			componentResourceManager.ApplyResources(this.chkchart1, "chkchart1");
			this.chkchart1.ForeColor = Color.Black;
			this.chkchart1.Name = "chkchart1";
			this.chkchart1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkchart3, "chkchart3");
			this.chkchart3.ForeColor = Color.Black;
			this.chkchart3.Name = "chkchart3";
			this.chkchart3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.lbtitle, "lbtitle");
			this.lbtitle.ForeColor = Color.Black;
			this.lbtitle.Name = "lbtitle";
			componentResourceManager.ApplyResources(this.chkchart4, "chkchart4");
			this.chkchart4.ForeColor = Color.Black;
			this.chkchart4.Name = "chkchart4";
			this.chkchart4.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkchart2, "chkchart2");
			this.chkchart2.ForeColor = Color.Black;
			this.chkchart2.Name = "chkchart2";
			this.chkchart2.UseVisualStyleBackColor = true;
			this.groupBox2.Controls.Add(this.btndel);
			this.groupBox2.Controls.Add(this.btnAdd);
			this.groupBox2.Controls.Add(this.grouplist);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.btndel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btndel, "btndel");
			this.btndel.ForeColor = SystemColors.ControlText;
			this.btndel.Name = "btndel";
			this.btndel.UseVisualStyleBackColor = false;
			this.btndel.Click += new System.EventHandler(this.btndel_Click);
			this.btnAdd.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.ForeColor = SystemColors.ControlText;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			this.grouplist.Columns.AddRange(new ColumnHeader[]
			{
				this.gptype,
				this.gpname
			});
			componentResourceManager.ApplyResources(this.grouplist, "grouplist");
			this.grouplist.FullRowSelect = true;
			this.grouplist.HideSelection = false;
			this.grouplist.Name = "grouplist";
			this.grouplist.UseCompatibleStateImageBehavior = false;
			this.grouplist.View = View.Details;
			this.grouplist.SelectedIndexChanged += new System.EventHandler(this.grouplist_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.gptype, "gptype");
			componentResourceManager.ApplyResources(this.gpname, "gpname");
			this.cboduration.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cboduration, "cboduration");
			this.cboduration.FormattingEnabled = true;
			this.cboduration.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboduration.Items"),
				componentResourceManager.GetString("cboduration.Items1"),
				componentResourceManager.GetString("cboduration.Items2"),
				componentResourceManager.GetString("cboduration.Items3"),
				componentResourceManager.GetString("cboduration.Items4"),
				componentResourceManager.GetString("cboduration.Items5"),
				componentResourceManager.GetString("cboduration.Items6")
			});
			this.cboduration.Name = "cboduration";
			componentResourceManager.ApplyResources(this.chkchart5, "chkchart5");
			this.chkchart5.ForeColor = Color.Black;
			this.chkchart5.Name = "chkchart5";
			this.chkchart5.UseVisualStyleBackColor = true;
			this.grpbchart.Controls.Add(this.chkchart1);
			this.grpbchart.Controls.Add(this.chkchart3);
			this.grpbchart.Controls.Add(this.chkchart4);
			this.grpbchart.Controls.Add(this.chkchart2);
			this.grpbchart.Controls.Add(this.chkchart5);
			componentResourceManager.ApplyResources(this.grpbchart, "grpbchart");
			this.grpbchart.ForeColor = Color.FromArgb(20, 73, 160);
			this.grpbchart.Name = "grpbchart";
			this.grpbchart.TabStop = false;
			this.gbxInfo.Controls.Add(this.tbduration);
			this.gbxInfo.Controls.Add(this.cboduration);
			this.gbxInfo.Controls.Add(this.lbtitle);
			this.gbxInfo.Controls.Add(this.label1);
			this.gbxInfo.Controls.Add(this.dtpbegin);
			this.gbxInfo.Controls.Add(this.txttitle);
			this.gbxInfo.Controls.Add(this.label14);
			this.gbxInfo.Controls.Add(this.lbwriter);
			this.gbxInfo.Controls.Add(this.label15);
			this.gbxInfo.Controls.Add(this.dtptime);
			this.gbxInfo.Controls.Add(this.cboperiod);
			this.gbxInfo.Controls.Add(this.txtwrite);
			this.gbxInfo.Controls.Add(this.label29);
			componentResourceManager.ApplyResources(this.gbxInfo, "gbxInfo");
			this.gbxInfo.ForeColor = Color.FromArgb(20, 73, 160);
			this.gbxInfo.Name = "gbxInfo";
			this.gbxInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.tbduration, "tbduration");
			this.tbduration.Name = "tbduration";
			this.tbduration.KeyPress += new KeyPressEventHandler(this.int_KeyPress);
			componentResourceManager.ApplyResources(this.label29, "label29");
			this.label29.ForeColor = Color.Black;
			this.label29.Name = "label29";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.btnGen);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.grpbchart);
			base.Controls.Add(this.gbxInfo);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "ThermGenRptPara";
			this.groupBox2.ResumeLayout(false);
			this.grpbchart.ResumeLayout(false);
			this.grpbchart.PerformLayout();
			this.gbxInfo.ResumeLayout(false);
			this.gbxInfo.PerformLayout();
			base.ResumeLayout(false);
		}
		public bool chkchart1_Checked()
		{
			return this.chkchart1.Checked;
		}
		public bool chkchart2_Checked()
		{
			return this.chkchart2.Checked;
		}
		public bool chkchart3_Checked()
		{
			return this.chkchart3.Checked;
		}
		public bool chkchart4_Checked()
		{
			return this.chkchart4.Checked;
		}
		public bool chkchart5_Checked()
		{
			return this.chkchart5.Checked;
		}
		public ThermGenRptPara()
		{
			this.InitializeComponent();
			this.txttitle.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtwrite.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbduration.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(ThermAnalysis parent)
		{
			this.m_parent = parent;
			this.dtptime.Value = System.DateTime.Now;
			this.m_dtbeginlast = this.dtptime.Value;
			this.cboperiod.SelectedIndex = 0;
			this.m_oldcboperiodindex = this.cboperiod.SelectedIndex;
			this.FillgroupList();
		}
		public void resettime()
		{
			this.dtptime.Value = System.DateTime.Now;
			this.m_dtbeginlast = this.dtptime.Value;
		}
		private void FillgroupList()
		{
			this.grouplist.Items.Clear();
			System.Collections.Generic.List<GroupInfo> groupByThermalFlag = GroupInfo.GetGroupByThermalFlag(1);
			foreach (GroupInfo current in groupByThermalFlag)
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
				listViewItem.Tag = string.Concat(new string[]
				{
					current.ID.ToString(),
					"|",
					current.Members,
					"|",
					current.GroupType
				});
				this.grouplist.Items.Add(listViewItem);
			}
			if (this.grouplist.SelectedItems.Count <= 0)
			{
				this.btndel.Enabled = false;
				return;
			}
			this.btndel.Enabled = true;
		}
		private void cboperiod_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.cboduration.Visible = false;
			this.tbduration.Visible = false;
			if (this.m_oldcboperiodindex == 3 && (this.cboperiod.SelectedIndex == 0 || this.cboperiod.SelectedIndex == 1 || this.cboperiod.SelectedIndex == 2))
			{
				System.DateTime value = this.dtpbegin.Value;
				int month = value.Month;
				int month2 = (month - 1) * 3 + 1;
				System.DateTime value2 = new System.DateTime(value.Year, month2, 1, value.Hour, value.Minute, value.Second, value.Millisecond);
				this.dtpbegin.Value = value2;
			}
			if (this.cboperiod.SelectedIndex == 0)
			{
				this.dtpbegin.CustomFormat = "yyyy-MM-dd HH";
				this.cboduration.Visible = true;
				this.cboduration.Items.Clear();
				for (int i = 1; i <= 24; i++)
				{
					this.cboduration.Items.Add(i.ToString());
				}
				this.m_oldcboperiodindex = 0;
			}
			else
			{
				if (this.cboperiod.SelectedIndex == 1)
				{
					this.dtpbegin.CustomFormat = "yyyy-MM-dd";
					this.cboduration.Items.Clear();
					for (int j = 1; j <= 31; j++)
					{
						this.cboduration.Items.Add(j.ToString());
					}
					this.tbduration.Visible = true;
					this.tbduration.Text = "31";
					this.m_oldcboperiodindex = 1;
				}
				else
				{
					if (this.cboperiod.SelectedIndex == 2)
					{
						this.dtpbegin.CustomFormat = "yyyy-MM";
						System.DateTime value3 = this.dtpbegin.Value;
						int month3 = value3.Month;
						System.DateTime value4 = new System.DateTime(value3.Year, month3, 1, value3.Hour, value3.Minute, value3.Second, value3.Millisecond);
						this.dtpbegin.Value = value4;
						this.cboduration.Visible = true;
						this.cboduration.Items.Clear();
						for (int k = 1; k <= 12; k++)
						{
							this.cboduration.Items.Add(k.ToString());
						}
						this.m_oldcboperiodindex = 2;
					}
					else
					{
						if (this.cboperiod.SelectedIndex == 3)
						{
							this.dtpbegin.CustomFormat = "yyyy-QMM";
							System.DateTime value5 = this.dtpbegin.Value;
							int month4 = value5.Month;
							int month5 = (month4 - 1) / 3 + 1;
							System.DateTime value6 = new System.DateTime(value5.Year, month5, 1, value5.Hour, value5.Minute, value5.Second, value5.Millisecond);
							this.dtpbegin.Value = value6;
							this.cboduration.Visible = true;
							this.cboduration.Items.Clear();
							for (int l = 1; l <= 4; l++)
							{
								this.cboduration.Items.Add(l.ToString());
							}
							this.m_oldcboperiodindex = 3;
						}
					}
				}
			}
			this.cboduration.SelectedIndex = 0;
		}
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			if (this.grouplist.Items.Count >= 4)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Group_selmaxnum, new string[0]));
				return;
			}
			ThermGenRptParaGpAddDlg thermGenRptParaGpAddDlg = new ThermGenRptParaGpAddDlg(this.grouplist.Items.Count);
			thermGenRptParaGpAddDlg.ShowDialog();
			this.FillgroupList();
		}
		private void btnGen_Click(object sender, System.EventArgs e)
		{
			if (!DBMaintain.ConvertOldDataFinish)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_inSplitMySQLTable, new string[0]));
				return;
			}
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.txttitle, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbtitle.Text
				}));
				this.txttitle.Focus();
				return;
			}
			Ecovalidate.checkTextIsNull(this.txtwrite, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbwriter.Text
				}));
				this.txtwrite.Focus();
				return;
			}
			if (this.grouplist.Items.Count == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Group_needadd, new string[0]));
				return;
			}
			if (this.cboperiod.SelectedIndex == 1)
			{
				Ecovalidate.checkTextIsNull(this.tbduration, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.label15.Text
					}));
					this.txtwrite.Focus();
					return;
				}
				if (!Ecovalidate.Rangeint(this.tbduration, 1, 31))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.label15.Text,
						"1",
						"31"
					}));
					return;
				}
			}
			if (!this.chkchart1.Checked && !this.chkchart2.Checked && !this.chkchart3.Checked && !this.chkchart4.Checked && !this.chkchart5.Checked)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rpt_selchart, new string[0]));
				return;
			}
			DebugCenter.GetInstance().appendToFile("ThermalAnalysis Start.");
			this.m_parent.showRpt(2, 0);
		}
		private void grouplist_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.grouplist.SelectedItems.Count <= 0)
			{
				this.btndel.Enabled = false;
				return;
			}
			this.btndel.Enabled = true;
		}
		private void btndel_Click(object sender, System.EventArgs e)
		{
			string text = "";
			foreach (ListViewItem listViewItem in this.grouplist.SelectedItems)
			{
				string str = System.Convert.ToString(listViewItem.Tag).Split(new char[]
				{
					'|'
				})[0];
				text = text + str + ",";
				this.grouplist.Items.Remove(listViewItem);
			}
			text = text.Substring(0, text.Length - 1);
			GroupInfo.UpdateGroupThermalFlag(0, text);
		}
		private void txttitle_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void txtwrite_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void int_KeyPress(object sender, KeyPressEventArgs e)
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
		private void dtpbegin_ValueChanged(object sender, System.EventArgs e)
		{
			if (this.cboperiod.SelectedIndex == 3)
			{
				System.DateTime value = this.dtpbegin.Value;
				int month = value.Month;
				if (month == 12 && this.m_dtbeginlast.Month == 1)
				{
					System.DateTime value2 = new System.DateTime(value.Year, 4, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
					this.dtpbegin.Value = value2;
				}
				else
				{
					if (month > 4)
					{
						System.DateTime value3 = new System.DateTime(value.Year, 1, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
						this.dtpbegin.Value = value3;
					}
				}
			}
			this.m_dtbeginlast = this.dtpbegin.Value;
		}
	}
}
