using CommonAPI;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Billing
{
	public class BillingRptPara : UserControl
	{
		public const int RPTTYPE_TOTAL = 1;
		public const int RPTTYPE_RACK = 2;
		private Billing m_parent;
		private Combobox_item m_RptType_Total = new Combobox_item(1.ToString(), "Total");
		private Combobox_item m_RptType_Rack = new Combobox_item(2.ToString(), "Rack");
		private IContainer components;
		private Button btnGen;
		private GroupBox groupBox2;
		private Button btndel;
		private Button btnAdd;
		private ListView grouplist;
		private ColumnHeader gptype;
		private ColumnHeader gpname;
		private GroupBox gbxInfo;
		private Label lbtitle;
		private Label label1;
		private DateTimePicker dtpbegin;
		private TextBox txttitle;
		private Label lbStart;
		private Label lbwriter;
		private Label lbDuration;
		private DateTimePicker dtptime;
		private ComboBox cbRptType;
		private TextBox txtwrite;
		private Label lbTime;
		private TextBox tbduration;
		private PictureBox pbLoading;
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
		public int RptType
		{
			get
			{
				return System.Convert.ToInt32(((Combobox_item)this.cbRptType.SelectedItem).getKey());
			}
		}
		public string Dtptime
		{
			get
			{
				return this.dtptime.Text;
			}
		}
		public string BeginText
		{
			get
			{
				return this.dtpbegin.Text;
			}
		}
		public int Cboduration
		{
			get
			{
				return 1;
			}
		}
		public ListView.ListViewItemCollection Grouplist
		{
			get
			{
				return this.grouplist.Items;
			}
		}
		public BillingRptPara()
		{
			this.InitializeComponent();
			this.txttitle.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtwrite.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.cbRptType.Items.Clear();
			this.cbRptType.Items.Add(this.m_RptType_Total);
			this.cbRptType.Items.Add(this.m_RptType_Rack);
		}
		public void pageInit(Billing parent)
		{
			this.m_parent = parent;
			this.pbLoading.Visible = false;
			this.btndel.Enabled = true;
			this.btnAdd.Enabled = true;
			this.dtptime.Value = System.DateTime.Now;
			System.DateTime value = this.dtpbegin.Value;
			int month = value.Month;
			System.DateTime value2 = new System.DateTime(value.Year, month, 1, value.Hour, value.Minute, value.Second, value.Millisecond);
			this.dtpbegin.Value = value2;
			this.cbRptType.SelectedIndex = 0;
			this.FillgroupList();
			if (!DBTools.DatabaseIsReady())
			{
				EcoMessageBox.ShowInfo(this, EcoLanguage.getMsg(LangRes.DB_waitready, new string[0]));
				this.btnGen.Enabled = false;
				return;
			}
			this.btnGen.Enabled = true;
		}
		public void pageInit_2(int showtype)
		{
			if (showtype == 0)
			{
				this.pbLoading.Visible = false;
				return;
			}
			this.pbLoading.Visible = true;
			this.pbLoading.BringToFront();
		}
		public void resettime()
		{
			this.dtptime.Value = System.DateTime.Now;
		}
		private void FillgroupList()
		{
			this.grouplist.Items.Clear();
			System.Collections.Generic.List<GroupInfo> groupByBillFlag = GroupInfo.GetGroupByBillFlag(1);
			foreach (GroupInfo current in groupByBillFlag)
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
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			BillingRptParaGpAddDlg billingRptParaGpAddDlg = new BillingRptParaGpAddDlg();
			billingRptParaGpAddDlg.ShowDialog();
			this.FillgroupList();
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
			GroupInfo.UpdateGroupBillFlag(0, text);
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
			DebugCenter.GetInstance().appendToFile("BillingAnalysis Start.");
			this.m_parent.showRpt(2, 0);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BillingRptPara));
			this.btnGen = new Button();
			this.groupBox2 = new GroupBox();
			this.btndel = new Button();
			this.btnAdd = new Button();
			this.grouplist = new ListView();
			this.gptype = new ColumnHeader();
			this.gpname = new ColumnHeader();
			this.gbxInfo = new GroupBox();
			this.lbtitle = new Label();
			this.label1 = new Label();
			this.dtpbegin = new DateTimePicker();
			this.dtptime = new DateTimePicker();
			this.lbTime = new Label();
			this.txttitle = new TextBox();
			this.lbStart = new Label();
			this.lbwriter = new Label();
			this.lbDuration = new Label();
			this.cbRptType = new ComboBox();
			this.txtwrite = new TextBox();
			this.tbduration = new TextBox();
			this.pbLoading = new PictureBox();
			this.groupBox2.SuspendLayout();
			this.gbxInfo.SuspendLayout();
			((ISupportInitialize)this.pbLoading).BeginInit();
			base.SuspendLayout();
			this.btnGen.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnGen, "btnGen");
			this.btnGen.Name = "btnGen";
			this.btnGen.UseVisualStyleBackColor = false;
			this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
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
			this.gbxInfo.Controls.Add(this.lbtitle);
			this.gbxInfo.Controls.Add(this.label1);
			this.gbxInfo.Controls.Add(this.dtpbegin);
			this.gbxInfo.Controls.Add(this.dtptime);
			this.gbxInfo.Controls.Add(this.lbTime);
			this.gbxInfo.Controls.Add(this.txttitle);
			this.gbxInfo.Controls.Add(this.lbStart);
			this.gbxInfo.Controls.Add(this.lbwriter);
			this.gbxInfo.Controls.Add(this.lbDuration);
			this.gbxInfo.Controls.Add(this.cbRptType);
			this.gbxInfo.Controls.Add(this.txtwrite);
			this.gbxInfo.Controls.Add(this.tbduration);
			componentResourceManager.ApplyResources(this.gbxInfo, "gbxInfo");
			this.gbxInfo.ForeColor = Color.FromArgb(20, 73, 160);
			this.gbxInfo.Name = "gbxInfo";
			this.gbxInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.lbtitle, "lbtitle");
			this.lbtitle.ForeColor = Color.Black;
			this.lbtitle.Name = "lbtitle";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Black;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.dtpbegin, "dtpbegin");
			this.dtpbegin.Format = DateTimePickerFormat.Custom;
			this.dtpbegin.Name = "dtpbegin";
			this.dtpbegin.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtptime, "dtptime");
			this.dtptime.Format = DateTimePickerFormat.Custom;
			this.dtptime.Name = "dtptime";
			this.dtptime.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.lbTime, "lbTime");
			this.lbTime.ForeColor = Color.Black;
			this.lbTime.Name = "lbTime";
			componentResourceManager.ApplyResources(this.txttitle, "txttitle");
			this.txttitle.Name = "txttitle";
			this.txttitle.KeyPress += new KeyPressEventHandler(this.txttitle_KeyPress);
			componentResourceManager.ApplyResources(this.lbStart, "lbStart");
			this.lbStart.ForeColor = Color.Black;
			this.lbStart.Name = "lbStart";
			componentResourceManager.ApplyResources(this.lbwriter, "lbwriter");
			this.lbwriter.ForeColor = Color.Black;
			this.lbwriter.Name = "lbwriter";
			componentResourceManager.ApplyResources(this.lbDuration, "lbDuration");
			this.lbDuration.ForeColor = Color.Black;
			this.lbDuration.Name = "lbDuration";
			this.cbRptType.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbRptType, "cbRptType");
			this.cbRptType.FormattingEnabled = true;
			this.cbRptType.Name = "cbRptType";
			componentResourceManager.ApplyResources(this.txtwrite, "txtwrite");
			this.txtwrite.Name = "txtwrite";
			this.txtwrite.KeyPress += new KeyPressEventHandler(this.txtwrite_KeyPress);
			componentResourceManager.ApplyResources(this.tbduration, "tbduration");
			this.tbduration.Name = "tbduration";
			this.tbduration.KeyPress += new KeyPressEventHandler(this.int_KeyPress);
			this.pbLoading.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pbLoading, "pbLoading");
			this.pbLoading.Image = Resources.processing;
			this.pbLoading.Name = "pbLoading";
			this.pbLoading.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.btnGen);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.gbxInfo);
			base.Controls.Add(this.pbLoading);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "BillingRptPara";
			this.groupBox2.ResumeLayout(false);
			this.gbxInfo.ResumeLayout(false);
			this.gbxInfo.PerformLayout();
			((ISupportInitialize)this.pbLoading).EndInit();
			base.ResumeLayout(false);
		}
	}
}
