using CommonAPI.CultureTransfer;
using DBAccessAPI;
using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.DevManDCFloorGrid;
using EcoSensors.EnegManPage.DashBoard.meter;
using EcoSensors.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace EcoSensors.EnegManPage.DashBoard
{
	public class DashBoard : UserControl
	{
		private delegate void boardDate(int from);
		private delegate void TabFlash();
		public const string BDNm_board = "board";
		public const string BDNm_boardgroup = "boardgroup";
		public const string BDNm_boardline = "boardline";
		public const string BDNm_effectiveness = "Effectiveness";
		public const string BDTag_THThreshold = "0:0";
		public const string BDTag_THAvailPower = "0:1";
		public const string BDTag_THRCI = "0:2";
		public const string BDTag_THRHI = "0:3";
		public const string BDTag_THRPI = "0:4";
		public const string BDTag_THRAI = "0:5";
		public const string BDTag_THRTI = "0:6";
		public const string BDTag_THOverIndices = "0:7";
		public const string BDTag_THEnergySavingEst = "0:8";
		public const string BDTag_THdoorst = "0:9";
		public const string BDTag_PWHeatLoadDiss = "1:0";
		public const string BDTag_PWHeatLoadDensity = "1:1";
		public const string BDTag_TPColdIntakeTemp = "2:0";
		public const string BDTag_TPIntakeDiff = "2:1";
		public const string BDTag_TPExhaustDiff = "2:2";
		public const string BDTag_TPHotExhaustTemp = "2:3";
		public const string BDTag_TPColdHotAcrossTemp = "2:4";
		public const string BDTag_PrIntakeDiff = "3:0";
		public const string BDTag_PrHeatLoadAirflowAcross = "3:1";
		public const string BDTag_PrFloorPlenum = "3:2";
		public const string BDTag_PrHeatLoadAirflowSupply = "3:3";
		public const string BDTag_PrHotRecirculation = "3:4";
		public const string BDTag_PrColdBypass = "3:5";
		public const string BDTag_HuColdIntakeRelative = "4:0";
		public const string BDTag_HuColdIntakeDewPoint = "4:1";
		private const int Interval_AfterChange = 10000;
		private const int Interval_NoChange = 1000;
		private IContainer components;
		private TableLayoutPanel tableLayoutPanel1;
		private Button btnair;
		private Button btnthreshold;
		private Button btnhumidity;
		private Button btnpower;
		private Button btnthermal;
		private Panel panel3;
		private ComboBox cbopdperiod;
		private Label label9;
		private ComboBox cbodash;
		private Panel paloverall;
		private Panel palIndices;
		private Label label117;
		private Label label114;
		private Label label111;
		private Label label108;
		private Label label2;
		private Label label39;
		private Label label49;
		private Label label90;
		private Label label95;
		private Label label105;
		private Label lbldiagram;
		private ComboBox cbo_high_low;
		private Panel palleg;
		private Label label21;
		private Label label20;
		private Label label19;
		private Label label15;
		private Label label14;
		private Label label12;
		private Chart overallchart;
		private Label lblIndex;
		private Panel palboard;
		private Panel paltime;
		private Label lblstart;
		private Label lblcurrenttime;
		private Label label104;
		private Label label100;
		private Panel panelAverage;
		private Label lblvar;
		private Label lblavg;
        private DevManDCFloorGrid.DevManDCFloorGrid devManDCFloorGrid1;
		private Panel panel1;
		private Label lbldetection;
		private Label lblstatus2;
		private Label lblstatus3;
		private Label lblstatus4;
		private Label lblstatus1;
		private Label labrang5;
		private Label labsign;
		private Label labrang4;
		private Label labrang3;
		private Label labrang2;
		private Label labrang1;
		private PictureBox pic;
		private Panel panelImgStatus;
        private meter.meter meter2;
        private meter.meter meter1;
		private ComboBox cbPUE;
		private System.Collections.Generic.Dictionary<string, Combobox_item> m_comboitems = new System.Collections.Generic.Dictionary<string, Combobox_item>();
		private int boardFlg = 1;
		private int DB_FlgISG;
		private int DB_flgAtenPDU;
		private System.Timers.Timer boardTimer;
		private int FlashTimes = 1;
		private System.Timers.Timer FlashTimer;
		private Color[] FlashColor = new Color[]
		{
			Color.FromArgb(162, 215, 48),
			Color.FromArgb(162, 215, 48),
			Color.FromArgb(162, 215, 48),
			Color.FromArgb(162, 215, 48)
		};
		private bool poweralarm;
		private bool thermalalarm;
		private bool airalarm;
		private bool humidityalarm;
		private System.DateTime m_LocalTime4getSrvTime = System.DateTime.Now;
		private TableLayoutPanel devRackInfo;
		private string boardName = "";
		private string m_SelectedTag;
		private string BoardTag = "PDU Sensors Thershold Status";
		private DataSet m_TitleData_Info = new DataSet();
		private System.Collections.ArrayList m_allRacks;
		private string origstr_lblstatus1;
		private string origstr_lblstatus2;
		private string origstr_lblstatus3;
		private TitleInfo title = new TitleInfo();
		public int FreshFlg_DashBoard
		{
			get
			{
				return this.boardFlg;
			}
			set
			{
				this.boardFlg = value;
			}
		}
		public int FreshFlg_ISGPower
		{
			set
			{
				this.DB_FlgISG = value;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DashBoard));
			ChartArea chartArea = new ChartArea();
			ChartArea chartArea2 = new ChartArea();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.btnthreshold = new Button();
			this.btnpower = new Button();
			this.btnthermal = new Button();
			this.btnhumidity = new Button();
			this.btnair = new Button();
			this.panel3 = new Panel();
			this.panelImgStatus = new Panel();
			this.lblstatus1 = new Label();
			this.lblstatus2 = new Label();
			this.lblstatus3 = new Label();
			this.lblstatus4 = new Label();
			this.labrang5 = new Label();
			this.labsign = new Label();
			this.labrang4 = new Label();
			this.labrang3 = new Label();
			this.labrang2 = new Label();
			this.labrang1 = new Label();
			this.pic = new PictureBox();
			this.lbldetection = new Label();
			this.label9 = new Label();
			this.cbodash = new ComboBox();
			this.cbPUE = new ComboBox();
			this.cbopdperiod = new ComboBox();
			this.paloverall = new Panel();
			this.palIndices = new Panel();
			this.label117 = new Label();
			this.label114 = new Label();
			this.label111 = new Label();
			this.label108 = new Label();
			this.label2 = new Label();
			this.label39 = new Label();
			this.label49 = new Label();
			this.label90 = new Label();
			this.label95 = new Label();
			this.label105 = new Label();
			this.lbldiagram = new Label();
			this.cbo_high_low = new ComboBox();
			this.palleg = new Panel();
			this.label21 = new Label();
			this.label20 = new Label();
			this.label19 = new Label();
			this.label15 = new Label();
			this.label14 = new Label();
			this.label12 = new Label();
			this.overallchart = new Chart();
			this.lblIndex = new Label();
			this.palboard = new Panel();
			this.paltime = new Panel();
			this.lblstart = new Label();
			this.lblcurrenttime = new Label();
			this.label104 = new Label();
			this.label100 = new Label();
			this.panelAverage = new Panel();
			this.lblvar = new Label();
			this.lblavg = new Label();
			this.panel1 = new Panel();
            this.devManDCFloorGrid1 = new DevManDCFloorGrid.DevManDCFloorGrid();
            this.meter2 = new meter.meter();
            this.meter1 = new meter.meter();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panelImgStatus.SuspendLayout();
			((ISupportInitialize)this.pic).BeginInit();
			this.paloverall.SuspendLayout();
			this.palIndices.SuspendLayout();
			this.palleg.SuspendLayout();
			((ISupportInitialize)this.overallchart).BeginInit();
			this.palboard.SuspendLayout();
			this.paltime.SuspendLayout();
			this.panelAverage.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.tableLayoutPanel1.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.btnthreshold, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnpower, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnthermal, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnhumidity, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnair, 3, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.btnthreshold.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.btnthreshold, "btnthreshold");
			this.btnthreshold.Name = "btnthreshold";
			this.btnthreshold.UseVisualStyleBackColor = false;
			this.btnthreshold.Click += new System.EventHandler(this.btnthreshold_Click);
			this.btnpower.BackColor = Color.FromArgb(162, 215, 48);
			componentResourceManager.ApplyResources(this.btnpower, "btnpower");
			this.btnpower.Name = "btnpower";
			this.btnpower.UseVisualStyleBackColor = false;
			this.btnpower.Click += new System.EventHandler(this.btnpower_Click);
			this.btnthermal.BackColor = Color.FromArgb(162, 215, 48);
			componentResourceManager.ApplyResources(this.btnthermal, "btnthermal");
			this.btnthermal.Name = "btnthermal";
			this.btnthermal.UseVisualStyleBackColor = false;
			this.btnthermal.Click += new System.EventHandler(this.btnthermal_Click);
			this.btnhumidity.BackColor = Color.FromArgb(162, 215, 48);
			componentResourceManager.ApplyResources(this.btnhumidity, "btnhumidity");
			this.btnhumidity.Name = "btnhumidity";
			this.btnhumidity.UseVisualStyleBackColor = false;
			this.btnhumidity.Click += new System.EventHandler(this.btnhumidity_Click);
			this.btnair.BackColor = Color.FromArgb(162, 215, 48);
			componentResourceManager.ApplyResources(this.btnair, "btnair");
			this.btnair.Name = "btnair";
			this.btnair.UseVisualStyleBackColor = false;
			this.btnair.Click += new System.EventHandler(this.btnair_Click);
			this.panel3.BackColor = Color.White;
			this.panel3.Controls.Add(this.panelImgStatus);
			this.panel3.Controls.Add(this.lbldetection);
			this.panel3.Controls.Add(this.label9);
			this.panel3.Controls.Add(this.cbodash);
			this.panel3.Controls.Add(this.cbPUE);
			this.panel3.Controls.Add(this.cbopdperiod);
			componentResourceManager.ApplyResources(this.panel3, "panel3");
			this.panel3.Name = "panel3";
			this.panelImgStatus.Controls.Add(this.lblstatus1);
			this.panelImgStatus.Controls.Add(this.lblstatus2);
			this.panelImgStatus.Controls.Add(this.lblstatus3);
			this.panelImgStatus.Controls.Add(this.lblstatus4);
			this.panelImgStatus.Controls.Add(this.labrang5);
			this.panelImgStatus.Controls.Add(this.labsign);
			this.panelImgStatus.Controls.Add(this.labrang4);
			this.panelImgStatus.Controls.Add(this.labrang3);
			this.panelImgStatus.Controls.Add(this.labrang2);
			this.panelImgStatus.Controls.Add(this.labrang1);
			this.panelImgStatus.Controls.Add(this.pic);
			componentResourceManager.ApplyResources(this.panelImgStatus, "panelImgStatus");
			this.panelImgStatus.Name = "panelImgStatus";
			componentResourceManager.ApplyResources(this.lblstatus1, "lblstatus1");
			this.lblstatus1.Name = "lblstatus1";
			componentResourceManager.ApplyResources(this.lblstatus2, "lblstatus2");
			this.lblstatus2.Name = "lblstatus2";
			componentResourceManager.ApplyResources(this.lblstatus3, "lblstatus3");
			this.lblstatus3.Name = "lblstatus3";
			componentResourceManager.ApplyResources(this.lblstatus4, "lblstatus4");
			this.lblstatus4.Name = "lblstatus4";
			componentResourceManager.ApplyResources(this.labrang5, "labrang5");
			this.labrang5.Name = "labrang5";
			componentResourceManager.ApplyResources(this.labsign, "labsign");
			this.labsign.Name = "labsign";
			componentResourceManager.ApplyResources(this.labrang4, "labrang4");
			this.labrang4.Name = "labrang4";
			componentResourceManager.ApplyResources(this.labrang3, "labrang3");
			this.labrang3.Name = "labrang3";
			componentResourceManager.ApplyResources(this.labrang2, "labrang2");
			this.labrang2.Name = "labrang2";
			componentResourceManager.ApplyResources(this.labrang1, "labrang1");
			this.labrang1.Name = "labrang1";
			this.pic.Image = Resources.Img650x75;
			componentResourceManager.ApplyResources(this.pic, "pic");
			this.pic.Name = "pic";
			this.pic.TabStop = false;
			componentResourceManager.ApplyResources(this.lbldetection, "lbldetection");
			this.lbldetection.Name = "lbldetection";
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			this.cbodash.BackColor = Color.White;
			this.cbodash.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbodash, "cbodash");
			this.cbodash.FormattingEnabled = true;
			this.cbodash.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbodash.Items"),
				componentResourceManager.GetString("cbodash.Items1"),
				componentResourceManager.GetString("cbodash.Items2"),
				componentResourceManager.GetString("cbodash.Items3"),
				componentResourceManager.GetString("cbodash.Items4"),
				componentResourceManager.GetString("cbodash.Items5"),
				componentResourceManager.GetString("cbodash.Items6"),
				componentResourceManager.GetString("cbodash.Items7"),
				componentResourceManager.GetString("cbodash.Items8"),
				componentResourceManager.GetString("cbodash.Items9"),
				componentResourceManager.GetString("cbodash.Items10"),
				componentResourceManager.GetString("cbodash.Items11"),
				componentResourceManager.GetString("cbodash.Items12"),
				componentResourceManager.GetString("cbodash.Items13"),
				componentResourceManager.GetString("cbodash.Items14"),
				componentResourceManager.GetString("cbodash.Items15"),
				componentResourceManager.GetString("cbodash.Items16"),
				componentResourceManager.GetString("cbodash.Items17"),
				componentResourceManager.GetString("cbodash.Items18"),
				componentResourceManager.GetString("cbodash.Items19"),
				componentResourceManager.GetString("cbodash.Items20"),
				componentResourceManager.GetString("cbodash.Items21"),
				componentResourceManager.GetString("cbodash.Items22")
			});
			this.cbodash.Name = "cbodash";
			this.cbodash.SelectedIndexChanged += new System.EventHandler(this.cbodash_SelectedIndexChanged);
			this.cbPUE.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbPUE, "cbPUE");
			this.cbPUE.FormattingEnabled = true;
			this.cbPUE.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbPUE.Items"),
				componentResourceManager.GetString("cbPUE.Items1"),
				componentResourceManager.GetString("cbPUE.Items2"),
				componentResourceManager.GetString("cbPUE.Items3")
			});
			this.cbPUE.Name = "cbPUE";
			this.cbPUE.SelectedIndexChanged += new System.EventHandler(this.cbPUE_SelectedIndexChanged);
			this.cbopdperiod.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbopdperiod, "cbopdperiod");
			this.cbopdperiod.FormattingEnabled = true;
			this.cbopdperiod.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbopdperiod.Items"),
				componentResourceManager.GetString("cbopdperiod.Items1"),
				componentResourceManager.GetString("cbopdperiod.Items2"),
				componentResourceManager.GetString("cbopdperiod.Items3"),
				componentResourceManager.GetString("cbopdperiod.Items4"),
				componentResourceManager.GetString("cbopdperiod.Items5")
			});
			this.cbopdperiod.Name = "cbopdperiod";
			this.cbopdperiod.SelectedIndexChanged += new System.EventHandler(this.cbopdperiod_SelectedIndexChanged);
			this.paloverall.BackColor = Color.White;
			this.paloverall.Controls.Add(this.meter2);
			this.paloverall.Controls.Add(this.meter1);
			this.paloverall.Controls.Add(this.palIndices);
			this.paloverall.Controls.Add(this.lbldiagram);
			this.paloverall.Controls.Add(this.cbo_high_low);
			this.paloverall.Controls.Add(this.palleg);
			this.paloverall.Controls.Add(this.overallchart);
			this.paloverall.Controls.Add(this.lblIndex);
			componentResourceManager.ApplyResources(this.paloverall, "paloverall");
			this.paloverall.Name = "paloverall";
			this.palIndices.Controls.Add(this.label117);
			this.palIndices.Controls.Add(this.label114);
			this.palIndices.Controls.Add(this.label111);
			this.palIndices.Controls.Add(this.label108);
			this.palIndices.Controls.Add(this.label2);
			this.palIndices.Controls.Add(this.label39);
			this.palIndices.Controls.Add(this.label49);
			this.palIndices.Controls.Add(this.label90);
			this.palIndices.Controls.Add(this.label95);
			this.palIndices.Controls.Add(this.label105);
			componentResourceManager.ApplyResources(this.palIndices, "palIndices");
			this.palIndices.Name = "palIndices";
			componentResourceManager.ApplyResources(this.label117, "label117");
			this.label117.BackColor = Color.White;
			this.label117.Name = "label117";
			componentResourceManager.ApplyResources(this.label114, "label114");
			this.label114.BackColor = Color.White;
			this.label114.Name = "label114";
			this.label111.BackColor = Color.Orange;
			componentResourceManager.ApplyResources(this.label111, "label111");
			this.label111.Name = "label111";
			this.label108.BackColor = Color.FromArgb(162, 215, 48);
			componentResourceManager.ApplyResources(this.label108, "label108");
			this.label108.Name = "label108";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.BackColor = Color.White;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label39, "label39");
			this.label39.BackColor = Color.White;
			this.label39.Name = "label39";
			componentResourceManager.ApplyResources(this.label49, "label49");
			this.label49.BackColor = Color.White;
			this.label49.Name = "label49";
			this.label90.BackColor = Color.Red;
			componentResourceManager.ApplyResources(this.label90, "label90");
			this.label90.Name = "label90";
			this.label95.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.label95, "label95");
			this.label95.Name = "label95";
			this.label105.BackColor = Color.Blue;
			componentResourceManager.ApplyResources(this.label105, "label105");
			this.label105.Name = "label105";
			componentResourceManager.ApplyResources(this.lbldiagram, "lbldiagram");
			this.lbldiagram.Name = "lbldiagram";
			this.cbo_high_low.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbo_high_low.FormattingEnabled = true;
			this.cbo_high_low.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbo_high_low.Items"),
				componentResourceManager.GetString("cbo_high_low.Items1")
			});
			componentResourceManager.ApplyResources(this.cbo_high_low, "cbo_high_low");
			this.cbo_high_low.Name = "cbo_high_low";
			this.cbo_high_low.SelectedIndexChanged += new System.EventHandler(this.cbo_high_low_SelectedIndexChanged);
			this.palleg.Controls.Add(this.label21);
			this.palleg.Controls.Add(this.label20);
			this.palleg.Controls.Add(this.label19);
			this.palleg.Controls.Add(this.label15);
			this.palleg.Controls.Add(this.label14);
			this.palleg.Controls.Add(this.label12);
			componentResourceManager.ApplyResources(this.palleg, "palleg");
			this.palleg.Name = "palleg";
			componentResourceManager.ApplyResources(this.label21, "label21");
			this.label21.BackColor = Color.White;
			this.label21.Name = "label21";
			componentResourceManager.ApplyResources(this.label20, "label20");
			this.label20.BackColor = Color.White;
			this.label20.Name = "label20";
			componentResourceManager.ApplyResources(this.label19, "label19");
			this.label19.BackColor = Color.White;
			this.label19.Name = "label19";
			this.label15.BackColor = Color.FromArgb(162, 215, 48);
			componentResourceManager.ApplyResources(this.label15, "label15");
			this.label15.Name = "label15";
			this.label14.BackColor = Color.Red;
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			this.label12.BackColor = Color.FromArgb(255, 128, 0);
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			chartArea.Name = "ChartArea1";
			chartArea.Position.Auto = false;
			chartArea.Position.Height = 98f;
			chartArea.Position.Width = 49f;
			chartArea.Position.X = 1f;
			chartArea.Position.Y = 1f;
			chartArea2.Name = "ChartArea2";
			chartArea2.Position.Auto = false;
			chartArea2.Position.Height = 98f;
			chartArea2.Position.Width = 49f;
			chartArea2.Position.X = 50f;
			chartArea2.Position.Y = 1f;
			this.overallchart.ChartAreas.Add(chartArea);
			this.overallchart.ChartAreas.Add(chartArea2);
			componentResourceManager.ApplyResources(this.overallchart, "overallchart");
			this.overallchart.Name = "overallchart";
			this.overallchart.TabStop = false;
			this.lblIndex.BorderStyle = BorderStyle.FixedSingle;
			componentResourceManager.ApplyResources(this.lblIndex, "lblIndex");
			this.lblIndex.Name = "lblIndex";
			this.palboard.BackColor = Color.White;
			this.palboard.Controls.Add(this.devManDCFloorGrid1);
			this.palboard.Controls.Add(this.paltime);
			this.palboard.Controls.Add(this.panelAverage);
			componentResourceManager.ApplyResources(this.palboard, "palboard");
			this.palboard.Name = "palboard";
			this.paltime.BorderStyle = BorderStyle.FixedSingle;
			this.paltime.Controls.Add(this.lblstart);
			this.paltime.Controls.Add(this.lblcurrenttime);
			this.paltime.Controls.Add(this.label104);
			this.paltime.Controls.Add(this.label100);
			componentResourceManager.ApplyResources(this.paltime, "paltime");
			this.paltime.Name = "paltime";
			componentResourceManager.ApplyResources(this.lblstart, "lblstart");
			this.lblstart.Name = "lblstart";
			componentResourceManager.ApplyResources(this.lblcurrenttime, "lblcurrenttime");
			this.lblcurrenttime.Name = "lblcurrenttime";
			componentResourceManager.ApplyResources(this.label104, "label104");
			this.label104.Name = "label104";
			componentResourceManager.ApplyResources(this.label100, "label100");
			this.label100.Name = "label100";
			this.panelAverage.BorderStyle = BorderStyle.FixedSingle;
			this.panelAverage.Controls.Add(this.lblvar);
			this.panelAverage.Controls.Add(this.lblavg);
			componentResourceManager.ApplyResources(this.panelAverage, "panelAverage");
			this.panelAverage.Name = "panelAverage";
			componentResourceManager.ApplyResources(this.lblvar, "lblvar");
			this.lblvar.Name = "lblvar";
			componentResourceManager.ApplyResources(this.lblavg, "lblavg");
			this.lblavg.Name = "lblavg";
			this.panel1.Controls.Add(this.palboard);
			this.panel1.Controls.Add(this.paloverall);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.devManDCFloorGrid1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.devManDCFloorGrid1, "devManDCFloorGrid1");
			this.devManDCFloorGrid1.Name = "devManDCFloorGrid1";
			this.devManDCFloorGrid1.TabStop = false;
			this.meter2.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.meter2, "meter2");
			this.meter2.Name = "meter2";
			this.meter1.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.meter1, "meter1");
			this.meter1.Name = "meter1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.panel3);
			base.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DashBoard";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panelImgStatus.ResumeLayout(false);
			this.panelImgStatus.PerformLayout();
			((ISupportInitialize)this.pic).EndInit();
			this.paloverall.ResumeLayout(false);
			this.paloverall.PerformLayout();
			this.palIndices.ResumeLayout(false);
			this.palIndices.PerformLayout();
			this.palleg.ResumeLayout(false);
			this.palleg.PerformLayout();
			((ISupportInitialize)this.overallchart).EndInit();
			this.palboard.ResumeLayout(false);
			this.paltime.ResumeLayout(false);
			this.paltime.PerformLayout();
			this.panelAverage.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public DashBoard()
		{
			this.InitializeComponent();
			this.origstr_lblstatus1 = this.lblstatus1.Text;
			this.origstr_lblstatus2 = this.lblstatus2.Text;
			this.origstr_lblstatus3 = this.lblstatus3.Text;
			this.FreshFlg_DashBoard = 1;
			EcoGlobalVar.gl_DashBoardCtrl = this;
			this.m_comboitems.Add("0:0", new Combobox_item("0:0", EcoLanguage.getMsg(LangRes.BDTag_THThreshold, new string[0])));
			this.m_comboitems.Add("0:1", new Combobox_item("0:1", EcoLanguage.getMsg(LangRes.BDTag_THAvailPower, new string[0])));
			this.m_comboitems.Add("0:2", new Combobox_item("0:2", EcoLanguage.getMsg(LangRes.BDTag_THRCI, new string[0])));
			this.m_comboitems.Add("0:3", new Combobox_item("0:3", EcoLanguage.getMsg(LangRes.BDTag_THRHI, new string[0])));
			this.m_comboitems.Add("0:4", new Combobox_item("0:4", EcoLanguage.getMsg(LangRes.BDTag_THRPI, new string[0])));
			this.m_comboitems.Add("0:5", new Combobox_item("0:5", EcoLanguage.getMsg(LangRes.BDTag_THRAI, new string[0])));
			this.m_comboitems.Add("0:6", new Combobox_item("0:6", EcoLanguage.getMsg(LangRes.BDTag_THRTI, new string[0])));
			this.m_comboitems.Add("0:7", new Combobox_item("0:7", EcoLanguage.getMsg(LangRes.BDTag_THOverIndices, new string[0])));
			this.m_comboitems.Add("0:8", new Combobox_item("0:8", EcoLanguage.getMsg(LangRes.BDTag_THEnergySavingEst, new string[0])));
			this.m_comboitems.Add("0:9", new Combobox_item("0:9", EcoLanguage.getMsg(LangRes.BDTag_THdoorst, new string[0])));
			this.m_comboitems.Add("1:0", new Combobox_item("1:0", EcoLanguage.getMsg(LangRes.BDTag_PWHeatLoadDiss, new string[0])));
			this.m_comboitems.Add("1:1", new Combobox_item("1:1", EcoLanguage.getMsg(LangRes.BDTag_PWHeatLoadDensity, new string[0])));
			this.m_comboitems.Add("2:0", new Combobox_item("2:0", EcoLanguage.getMsg(LangRes.BDTag_TPColdIntakeTemp, new string[0])));
			this.m_comboitems.Add("2:1", new Combobox_item("2:1", EcoLanguage.getMsg(LangRes.BDTag_TPIntakeDiff, new string[0])));
			this.m_comboitems.Add("2:2", new Combobox_item("2:2", EcoLanguage.getMsg(LangRes.BDTag_TPExhaustDiff, new string[0])));
			this.m_comboitems.Add("2:3", new Combobox_item("2:3", EcoLanguage.getMsg(LangRes.BDTag_TPHotExhaustTemp, new string[0])));
			this.m_comboitems.Add("2:4", new Combobox_item("2:4", EcoLanguage.getMsg(LangRes.BDTag_TPColdHotAcrossTemp, new string[0])));
			this.m_comboitems.Add("3:0", new Combobox_item("3:0", EcoLanguage.getMsg(LangRes.BDTag_PrIntakeDiff, new string[0])));
			this.m_comboitems.Add("3:1", new Combobox_item("3:1", EcoLanguage.getMsg(LangRes.BDTag_PrHeatLoadAirflowAcross, new string[0])));
			this.m_comboitems.Add("3:2", new Combobox_item("3:2", EcoLanguage.getMsg(LangRes.BDTag_PrFloorPlenum, new string[0])));
			this.m_comboitems.Add("3:3", new Combobox_item("3:3", EcoLanguage.getMsg(LangRes.BDTag_PrHeatLoadAirflowSupply, new string[0])));
			this.m_comboitems.Add("3:4", new Combobox_item("3:4", EcoLanguage.getMsg(LangRes.BDTag_PrHotRecirculation, new string[0])));
			this.m_comboitems.Add("3:5", new Combobox_item("3:5", EcoLanguage.getMsg(LangRes.BDTag_PrColdBypass, new string[0])));
			this.m_comboitems.Add("4:0", new Combobox_item("4:0", EcoLanguage.getMsg(LangRes.BDTag_HuColdIntakeRelative, new string[0])));
			this.m_comboitems.Add("4:1", new Combobox_item("4:1", EcoLanguage.getMsg(LangRes.BDTag_HuColdIntakeDewPoint, new string[0])));
			this.boardTimer = new System.Timers.Timer();
			this.boardTimer.Elapsed += new ElapsedEventHandler(this.boardTimerEvent);
			this.boardTimer.Interval = 10000.0;
			this.boardTimer.AutoReset = true;
			this.boardTimer.Enabled = false;
			this.FlashTimer = new System.Timers.Timer();
			this.FlashTimer.Elapsed += new ElapsedEventHandler(this.FlashEvent);
			this.FlashTimer.Interval = 500.0;
			this.FlashTimer.AutoReset = true;
			this.FlashTimer.Enabled = false;
		}
		public void pageInit_1()
		{
			this.boardDataInit();
			this.btnthreshold_Click(this.btnthreshold, null);
			this.cbopdperiod.SelectedIndex = 0;
			this.cbPUE.SelectedIndex = 0;
			this.paltime.Visible = false;
			RackStatusAll.PUE_period = 0;
			RackStatusAll.Power_dissipation_period = 0;
		}
		public void pageInit()
		{
			string boardTag = this.BoardTag;
			commUtil.ShowInfo_DEBUG("DashBoard pageInit -1  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff") + "BoardTag==" + this.BoardTag);
			this.btnthreshold_Click(this.btnthreshold, null);
			this.cbopdperiod.SelectedIndex = 0;
			this.cbPUE.SelectedIndex = 0;
			this.paltime.Visible = false;
			RackStatusAll.PUE_period = 0;
			RackStatusAll.Power_dissipation_period = 0;
			if (boardTag.Equals("0:0"))
			{
				this.boardTimeDate(0);
			}
			this.startBoardTimer();
			commUtil.ShowInfo_DEBUG("DashBoard pageInit -2  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
		}
		private int boardDataInit()
		{
			bool flag = false;
			switch (this.FreshFlg_DashBoard)
			{
			case 1:
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_DrawRack Begin " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				if (!EcoGlobalVar.gl_isProcessThreadRuning())
				{
					EcoGlobalVar.gl_StartProcessfThread(false);
					flag = true;
				}
				this.devManDCFloorGrid1.Hide();
				this.devRackInfo = this.devManDCFloorGrid1.getRackCtrl();
				this.devRackInfo.SuspendLayout();
				this.m_allRacks = ClientAPI.getRackInfo();
				foreach (RackInfo rackInfo in this.m_allRacks)
				{
					int startPoint_X = rackInfo.StartPoint_X;
					int startPoint_Y = rackInfo.StartPoint_Y;
					int endPoint_X = rackInfo.EndPoint_X;
					int arg_BE_0 = rackInfo.EndPoint_Y;
					int num;
					if (startPoint_X == endPoint_X)
					{
						num = 0;
					}
					else
					{
						num = 1;
					}
					Label label = new Label();
					label.Tag = rackInfo.RackID;
					label.ForeColor = Color.Silver;
					label.BackColor = Color.Silver;
					label.BorderStyle = BorderStyle.FixedSingle;
					label.Dock = DockStyle.Fill;
					label.Margin = new Padding(0);
					label.MouseHover += new System.EventHandler(this.Rack_MouseHover);
					label.MouseLeave += new System.EventHandler(this.Rack_MouseLeave);
					this.devRackInfo.Controls.Add(label, startPoint_Y, startPoint_X);
					if (num == 0)
					{
						this.devRackInfo.SetColumnSpan(label, 2);
						this.devRackInfo.SetRowSpan(label, 1);
					}
					else
					{
						if (num == 1)
						{
							this.devRackInfo.SetColumnSpan(label, 1);
							this.devRackInfo.SetRowSpan(label, 2);
						}
					}
				}
				this.devRackInfo.ResumeLayout();
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_DrawRack End  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				this.m_TitleData_Info = ClientAPI.getDataSet(0);
				this.FreshFlg_DashBoard = -1;
				this.devManDCFloorGrid1.Show();
				if (flag)
				{
					EcoGlobalVar.gl_StopProcessfThread();
				}
				return 0;
			case 2:
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_modifydata Begin  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				this.m_allRacks = ClientAPI.getRackInfo();
				this.m_TitleData_Info = ClientAPI.getDataSet(0);
				this.FreshFlg_DashBoard = -1;
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_modifydata End  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				return 0;
			default:
				return 1;
			}
		}
		private void Rack_MouseHover(object sender, System.EventArgs e)
		{
			Screen[] allScreens = Screen.AllScreens;
			int width = allScreens[0].Bounds.Width;
			int height = allScreens[0].Bounds.Height;
			int x = Control.MousePosition.X - 5;
			int y = Control.MousePosition.Y - 5;
			if (width - Control.MousePosition.X < this.title.Size.Width)
			{
				x = Control.MousePosition.X - this.title.Size.Width;
			}
			if (height - Control.MousePosition.Y < this.title.Size.Height)
			{
				y = Control.MousePosition.Y - this.title.Size.Height;
			}
			this.Cursor = Cursors.WaitCursor;
			this.title.Location = new Point(x, y);
			long rackID = (long)((Label)sender).Tag;
			string devidsinRack = this.getDevidsinRack(this.m_allRacks, rackID);
			this.title.Set(rackID, devidsinRack, this.m_TitleData_Info, RackStatusAll.Board_Tag);
			this.title.Show();
			this.Cursor = Cursors.Arrow;
		}
		private void Rack_MouseLeave(object sender, System.EventArgs e)
		{
			if (Control.MousePosition.X >= this.title.Location.X && Control.MousePosition.X <= this.title.Location.X + this.title.Size.Width && Control.MousePosition.Y >= this.title.Location.Y && Control.MousePosition.Y <= this.title.Location.Y + this.title.Size.Height)
			{
				return;
			}
			this.title.Hide();
			this.Cursor = Cursors.Arrow;
		}
		private void btnthreshold_Click(object sender, System.EventArgs e)
		{
			this.showPicItems(true);
			this.palboard.Visible = true;
			this.paloverall.Visible = false;
			this.btnhumidity.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthermal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnair.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnpower.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
			this.cbodash.Items.Clear();
			this.cbodash.Items.Add(this.m_comboitems["0:0"]);
			this.cbodash.Items.Add(this.m_comboitems["0:1"]);
			this.cbodash.Items.Add(this.m_comboitems["0:2"]);
			this.cbodash.Items.Add(this.m_comboitems["0:3"]);
			this.cbodash.Items.Add(this.m_comboitems["0:4"]);
			this.cbodash.Items.Add(this.m_comboitems["0:5"]);
			this.cbodash.Items.Add(this.m_comboitems["0:6"]);
			this.cbodash.Items.Add(this.m_comboitems["0:7"]);
			this.cbodash.Items.Add(this.m_comboitems["0:8"]);
			this.cbodash.Items.Add(this.m_comboitems["0:9"]);
			this.cbodash.SelectedIndex = 0;
		}
		private void btnpower_Click(object sender, System.EventArgs e)
		{
			this.showPicItems(true);
			this.palboard.Visible = true;
			this.paloverall.Visible = false;
			this.btnhumidity.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthermal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnair.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnpower.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
			this.btnthreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.cbodash.Items.Clear();
			this.cbodash.Items.Add(this.m_comboitems["1:0"]);
			this.cbodash.Items.Add(this.m_comboitems["1:1"]);
			this.cbodash.SelectedIndex = 0;
		}
		private void btnthermal_Click(object sender, System.EventArgs e)
		{
			this.showPicItems(true);
			this.palboard.Visible = true;
			this.paloverall.Visible = false;
			this.btnhumidity.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthermal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
			this.btnair.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnpower.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.cbodash.Items.Clear();
			this.cbodash.Items.Add(this.m_comboitems["2:0"]);
			this.cbodash.Items.Add(this.m_comboitems["2:1"]);
			this.cbodash.Items.Add(this.m_comboitems["2:3"]);
			this.cbodash.Items.Add(this.m_comboitems["2:2"]);
			this.cbodash.Items.Add(this.m_comboitems["2:4"]);
			this.cbodash.SelectedIndex = 0;
		}
		private void btnair_Click(object sender, System.EventArgs e)
		{
			this.showPicItems(true);
			this.palboard.Visible = true;
			this.paloverall.Visible = false;
			this.btnhumidity.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthermal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnair.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
			this.btnpower.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.cbodash.Items.Clear();
			this.cbodash.Items.Add(this.m_comboitems["3:0"]);
			this.cbodash.Items.Add(this.m_comboitems["3:1"]);
			this.cbodash.Items.Add(this.m_comboitems["3:2"]);
			this.cbodash.Items.Add(this.m_comboitems["3:4"]);
			this.cbodash.Items.Add(this.m_comboitems["3:5"]);
			this.cbodash.SelectedIndex = 0;
		}
		private void btnhumidity_Click(object sender, System.EventArgs e)
		{
			this.showPicItems(true);
			this.palboard.Visible = true;
			this.paloverall.Visible = false;
			this.btnhumidity.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
			this.btnthermal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnair.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnpower.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.btnthreshold.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
			this.cbodash.Items.Clear();
			this.cbodash.Items.Add(this.m_comboitems["4:0"]);
			this.cbodash.Items.Add(this.m_comboitems["4:1"]);
			this.cbodash.SelectedIndex = 0;
		}
		private void init_typeBoard(string SelboardTag)
		{
			bool flag = false;
			bool visible = false;
			bool visible2 = false;
			bool visible3 = false;
			bool visible4 = false;
			Bitmap image = null;
			switch (SelboardTag)
			{
			case "0:0":
				flag = false;
				image = Resources.Img650x75;
				visible = false;
				visible2 = false;
				visible3 = false;
				visible4 = false;
				break;
			case "0:9":
				flag = false;
				image = Resources.Img650x75doorSS;
				visible = false;
				visible2 = false;
				visible3 = false;
				visible4 = false;
				break;
			case "0:1":
			case "1:1":
			case "2:0":
			case "2:1":
			case "2:2":
			case "2:3":
			case "2:4":
			case "3:0":
			case "3:1":
			case "3:2":
			case "3:4":
			case "3:5":
			case "4:0":
			case "4:1":
				flag = true;
				image = Resources.Img400x50;
				visible = false;
				visible2 = false;
				visible4 = false;
				break;
			case "1:0":
				flag = true;
				image = Resources.Img400x50;
				visible = true;
				visible2 = false;
				visible4 = true;
				break;
			}
			this.labsign.Visible = (this.labrang1.Visible = (this.labrang2.Visible = (this.labrang3.Visible = (this.labrang4.Visible = (this.labrang5.Visible = flag)))));
			this.lblstatus1.Visible = (this.lblstatus2.Visible = (this.lblstatus3.Visible = (this.lblstatus4.Visible = !flag)));
			this.pic.Image = image;
			this.cbopdperiod.Visible = visible;
			this.cbPUE.Visible = visible2;
			this.panelAverage.Visible = visible3;
			this.paltime.Visible = visible4;
			System.DateTime dateTime = System.DateTime.Now;
			object obj = ClientAPI.RemoteCall(104, 1, "", 10000);
			if (obj != null)
			{
				dateTime = (System.DateTime)obj;
			}
			this.m_LocalTime4getSrvTime = System.DateTime.Now;
			switch (SelboardTag)
			{
			case "0:0":
				this.labsign.Text = "     ";
				this.labrang1.Text = "";
				this.labrang2.Text = "";
				this.labrang3.Text = "";
				this.labrang4.Text = "";
				this.labrang5.Text = "";
				this.lblstatus1.Text = this.origstr_lblstatus1;
				this.lblstatus2.Text = this.origstr_lblstatus2;
				this.lblstatus3.Text = this.origstr_lblstatus3;
				this.DB_FlgISG = AppData.getDB_FlgISG();
				this.DB_flgAtenPDU = AppData.getDB_flgAtenPDU();
				if (this.DB_FlgISG == 0)
				{
					return;
				}
				this.cbPUE.Visible = true;
				this.panelAverage.Visible = true;
				switch (this.cbPUE.SelectedIndex)
				{
				case 0:
					break;
				case 1:
					this.paltime.Visible = true;
					this.lblstart.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				case 2:
					this.paltime.Visible = true;
					this.lblstart.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				case 3:
				{
					this.paltime.Visible = true;
					System.DateTime dateTime2 = dateTime.AddDays((double)(1 - System.Convert.ToInt32(dateTime.DayOfWeek.ToString("d"))));
					if (dateTime.DayOfWeek == System.DayOfWeek.Sunday)
					{
						dateTime2 = dateTime2.AddDays(-7.0);
					}
					System.DateTime dateTime3 = dateTime2.AddDays(6.0);
					this.lblstart.Text = dateTime2.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime3.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				}
				default:
					return;
				}
				break;
			case "0:9":
				this.lblstatus4.Visible = false;
				this.lblstatus1.Text = EcoLanguage.getMsg(LangRes.DashBd_doorSS_Close, new string[0]);
				this.lblstatus2.Text = EcoLanguage.getMsg(LangRes.DashBd_doorSS_NA, new string[0]);
				this.lblstatus3.Text = EcoLanguage.getMsg(LangRes.DashBd_doorSS_Open, new string[0]);
				return;
			case "0:1":
				this.labsign.Text = "%";
				this.labrang1.Text = "100";
				this.labrang2.Text = "75";
				this.labrang3.Text = "50";
				this.labrang4.Text = "25";
				this.labrang5.Text = "0";
				return;
			case "1:0":
				this.labsign.Text = "U kWh";
				switch (this.cbopdperiod.SelectedIndex)
				{
				case 0:
					RackStatusAll.MinValue = 0.5;
					RackStatusAll.MaxValue = 4.5;
					this.labrang1.Text = System.Convert.ToString(0.5);
					this.labrang2.Text = System.Convert.ToString(1.5);
					this.labrang3.Text = System.Convert.ToString(2.5);
					this.labrang4.Text = System.Convert.ToString(3.3);
					this.labrang5.Text = System.Convert.ToString(4.5);
					this.lblstart.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				case 1:
					RackStatusAll.MinValue = 10.0;
					RackStatusAll.MaxValue = 100.0;
					this.labrang1.Text = "10";
					this.labrang2.Text = "30";
					this.labrang3.Text = "55";
					this.labrang4.Text = "75";
					this.labrang5.Text = "100";
					this.lblstart.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				case 2:
				{
					RackStatusAll.MinValue = 70.0;
					RackStatusAll.MaxValue = 700.0;
					this.labrang1.Text = "70";
					this.labrang2.Text = "230";
					this.labrang3.Text = "390";
					this.labrang4.Text = "550";
					this.labrang5.Text = "700";
					System.DateTime dateTime2 = dateTime.AddDays((double)(1 - System.Convert.ToInt32(dateTime.DayOfWeek.ToString("d"))));
					if (dateTime.DayOfWeek == System.DayOfWeek.Sunday)
					{
						dateTime2 = dateTime2.AddDays(-7.0);
					}
					System.DateTime dateTime3 = dateTime2.AddDays(6.0);
					this.lblstart.Text = dateTime2.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime3.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				}
				case 3:
				{
					RackStatusAll.MinValue = 300.0;
					RackStatusAll.MaxValue = 3000.0;
					this.labrang1.Text = "300";
					this.labrang2.Text = "950";
					this.labrang3.Text = "1600";
					this.labrang4.Text = "2250";
					this.labrang5.Text = "3000";
					System.DateTime dateTime2 = dateTime.AddDays((double)(1 - dateTime.Day));
					System.DateTime dateTime3 = dateTime2.AddMonths(1).AddDays(-1.0);
					this.lblstart.Text = dateTime2.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime3.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				}
				case 4:
				{
					RackStatusAll.MinValue = 1000.0;
					RackStatusAll.MaxValue = 10000.0;
					this.labrang1.Text = "1000";
					this.labrang2.Text = "3000";
					this.labrang3.Text = "5500";
					this.labrang4.Text = "7500";
					this.labrang5.Text = "10000";
					System.DateTime dateTime2 = dateTime.AddMonths(-((dateTime.Month - 1) % 3)).AddDays((double)(1 - dateTime.Day));
					System.DateTime dateTime3 = dateTime2.AddMonths(3).AddDays(-1.0);
					this.lblstart.Text = dateTime2.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime3.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				}
				case 5:
				{
					RackStatusAll.MinValue = 4000.0;
					RackStatusAll.MaxValue = 40000.0;
					this.labrang1.Text = "4000";
					this.labrang2.Text = "13000";
					this.labrang3.Text = "22000";
					this.labrang4.Text = "31000";
					this.labrang5.Text = "40000";
					System.DateTime dateTime2 = dateTime.AddDays((double)(1 - System.Convert.ToInt32(dateTime.DayOfYear.ToString("d"))));
					System.DateTime dateTime3 = dateTime2.AddYears(1).AddDays(-1.0);
					this.lblstart.Text = dateTime2.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime3.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					return;
				}
				default:
					return;
				}
				break;
			case "1:1":
				this.labsign.Text = "L W";
				this.labrang1.Text = "500";
				this.labrang2.Text = "1500";
				this.labrang3.Text = "2500";
				this.labrang4.Text = "3500";
				this.labrang5.Text = "4500";
				return;
			case "2:0":
				if (EcoGlobalVar.TempUnit == 0)
				{
					this.labsign.Text = "t (°C)";
					this.labrang1.Text = "10";
					this.labrang2.Text = "18";
					this.labrang3.Text = "26";
					this.labrang4.Text = "34";
					this.labrang5.Text = "45";
				}
				else
				{
					this.labsign.Text = "t (°F)";
					this.labrang1.Text = RackStatusAll.CtoFdegrees(10.0).ToString("F0");
					this.labrang2.Text = RackStatusAll.CtoFdegrees(18.0).ToString("F0");
					this.labrang3.Text = RackStatusAll.CtoFdegrees(26.0).ToString("F0");
					this.labrang4.Text = RackStatusAll.CtoFdegrees(34.0).ToString("F0");
					this.labrang5.Text = RackStatusAll.CtoFdegrees(45.0).ToString("F0");
				}
				EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
				return;
			case "2:1":
				if (EcoGlobalVar.TempUnit == 0)
				{
					this.labsign.Text = "△t (°C)";
					this.labrang1.Text = "2";
					this.labrang2.Text = "6";
					this.labrang3.Text = "10";
					this.labrang4.Text = "14";
					this.labrang5.Text = "20";
				}
				else
				{
					this.labsign.Text = "△t (°F)";
					this.labrang1.Text = RackStatusAll.CtoFdegrees(2.0).ToString("F0");
					this.labrang2.Text = RackStatusAll.CtoFdegrees(6.0).ToString("F0");
					this.labrang3.Text = RackStatusAll.CtoFdegrees(10.0).ToString("F0");
					this.labrang4.Text = RackStatusAll.CtoFdegrees(14.0).ToString("F0");
					this.labrang5.Text = RackStatusAll.CtoFdegrees(20.0).ToString("F0");
				}
				EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
				return;
			case "2:2":
				if (EcoGlobalVar.TempUnit == 0)
				{
					this.labsign.Text = "△T (°C)";
					this.labrang1.Text = "10";
					this.labrang2.Text = "18";
					this.labrang3.Text = "26";
					this.labrang4.Text = "34";
					this.labrang5.Text = "45";
				}
				else
				{
					this.labsign.Text = "△T (°F)";
					this.labrang1.Text = RackStatusAll.CtoFdegrees(10.0).ToString("F0");
					this.labrang2.Text = RackStatusAll.CtoFdegrees(18.0).ToString("F0");
					this.labrang3.Text = RackStatusAll.CtoFdegrees(26.0).ToString("F0");
					this.labrang4.Text = RackStatusAll.CtoFdegrees(34.0).ToString("F0");
					this.labrang5.Text = RackStatusAll.CtoFdegrees(45.0).ToString("F0");
				}
				EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
				return;
			case "2:3":
				if (EcoGlobalVar.TempUnit == 0)
				{
					this.labsign.Text = "T (°C)";
					this.labrang1.Text = "0";
					this.labrang2.Text = "15";
					this.labrang3.Text = "30";
					this.labrang4.Text = "45";
					this.labrang5.Text = "60";
				}
				else
				{
					this.labsign.Text = "T (°F)";
					this.labrang1.Text = RackStatusAll.CtoFdegrees(0.0).ToString("F0");
					this.labrang2.Text = RackStatusAll.CtoFdegrees(15.0).ToString("F0");
					this.labrang3.Text = RackStatusAll.CtoFdegrees(30.0).ToString("F0");
					this.labrang4.Text = RackStatusAll.CtoFdegrees(45.0).ToString("F0");
					this.labrang5.Text = RackStatusAll.CtoFdegrees(60.0).ToString("F0");
				}
				EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
				return;
			case "2:4":
				if (EcoGlobalVar.TempUnit == 0)
				{
					this.labsign.Text = "△T_equip (°C)";
					this.labrang1.Text = "5";
					this.labrang2.Text = "18";
					this.labrang3.Text = "30";
					this.labrang4.Text = "42";
					this.labrang5.Text = "55";
				}
				else
				{
					this.labsign.Text = "△T_equip (°F)";
					this.labrang1.Text = RackStatusAll.CtoFdegrees(5.0).ToString("F0");
					this.labrang2.Text = RackStatusAll.CtoFdegrees(18.0).ToString("F0");
					this.labrang3.Text = RackStatusAll.CtoFdegrees(30.0).ToString("F0");
					this.labrang4.Text = RackStatusAll.CtoFdegrees(42.0).ToString("F0");
					this.labrang5.Text = RackStatusAll.CtoFdegrees(55.0).ToString("F0");
				}
				EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
				return;
			case "3:0":
				this.labsign.Text = "△P (Pa)";
				this.labrang1.Text = "10";
				this.labrang2.Text = "50";
				this.labrang3.Text = "100";
				this.labrang4.Text = "150";
				this.labrang5.Text = "200";
				return;
			case "3:1":
				this.labsign.Text = "V_equip (cfm)";
				this.labrang1.Text = "500";
				this.labrang2.Text = "1000";
				this.labrang3.Text = "1500";
				this.labrang4.Text = "2000";
				this.labrang5.Text = "2500";
				return;
			case "3:2":
				this.labsign.Text = "Q_floor (cfm)";
				this.labrang1.Text = "500";
				this.labrang2.Text = "1000";
				this.labrang3.Text = "1500";
				this.labrang4.Text = "2000";
				this.labrang5.Text = "2500";
				return;
			case "3:4":
				this.labsign.Text = "△A_circk (%)";
				this.labrang1.Text = "10";
				this.labrang2.Text = "20";
				this.labrang3.Text = "30";
				this.labrang4.Text = "40";
				this.labrang5.Text = "50";
				return;
			case "3:5":
				this.labsign.Text = "△A_bypas (%)";
				this.labrang1.Text = "10";
				this.labrang2.Text = "20";
				this.labrang3.Text = "30";
				this.labrang4.Text = "40";
				this.labrang5.Text = "50";
				return;
			case "4:0":
				this.labsign.Text = "h_rel (%)";
				this.labrang1.Text = "10";
				this.labrang2.Text = "30";
				this.labrang3.Text = "50";
				this.labrang4.Text = "70";
				this.labrang5.Text = "90";
				return;
			case "4:1":
				if (EcoGlobalVar.TempUnit == 0)
				{
					this.labsign.Text = "t_dew (°C)";
					this.labrang1.Text = "0";
					this.labrang2.Text = "4";
					this.labrang3.Text = "8";
					this.labrang4.Text = "12";
					this.labrang5.Text = "15";
				}
				else
				{
					this.labsign.Text = "t_dew (°F)";
					this.labrang1.Text = RackStatusAll.CtoFdegrees(0.0).ToString("F0");
					this.labrang2.Text = RackStatusAll.CtoFdegrees(4.0).ToString("F0");
					this.labrang3.Text = RackStatusAll.CtoFdegrees(8.0).ToString("F0");
					this.labrang4.Text = RackStatusAll.CtoFdegrees(12.0).ToString("F0");
					this.labrang5.Text = RackStatusAll.CtoFdegrees(15.0).ToString("F0");
				}
				EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
				break;

				return;
			}
		}
		private void tvDevice_AfterSelect(string Name, string Tag)
		{
			this.endBoardTimer();
			this.m_SelectedTag = Tag;
			if (Name == "board")
			{
				this.showPicItems(true);
				this.palboard.Visible = true;
				this.paloverall.Visible = false;
				this.palIndices.Visible = false;
				this.boardName = "board";
				if (this.BoardTag != Tag)
				{
					this.init_typeBoard(Tag);
					this.BoardTag = Tag;
					this.boardTimeDate(0);
				}
			}
			else
			{
				if (Name == "boardline")
				{
					this.boardName = "boardline";
					this.overallchart.ChartAreas.Clear();
					this.overallchart.Series.Clear();
					this.overallchart.ChartAreas.Add("ChartArea1");
					if (Tag == "0:8")
					{
						this.overallchart.ChartAreas.Add("ChartArea2");
						this.overallchart.ChartAreas[0].Position.X = 1f;
						this.overallchart.ChartAreas[0].Position.Y = 1f;
						this.overallchart.ChartAreas[0].Position.Width = 47f;
						this.overallchart.ChartAreas[0].Position.Height = 98f;
						this.overallchart.ChartAreas[1].Position.X = 48f;
						this.overallchart.ChartAreas[1].Position.Y = 1f;
						this.overallchart.ChartAreas[1].Position.Width = 53f;
						this.overallchart.ChartAreas[1].Position.Height = 98f;
						this.overallchart.Location = new Point(80, 40);
						this.overallchart.Width = 750;
						this.overallchart.Height = 450;
						this.palIndices.Visible = false;
					}
					else
					{
						this.overallchart.ChartAreas[0].Position.X = 2f;
						this.overallchart.ChartAreas[0].Position.Y = 2f;
						this.overallchart.ChartAreas[0].Position.Width = 98f;
						this.overallchart.ChartAreas[0].Position.Height = 98f;
						this.overallchart.Location = new Point(20, 40);
						this.overallchart.Width = 800;
						this.overallchart.Height = 450;
						this.palIndices.Visible = true;
						this.palIndices.Location = new Point(818, 46);
					}
					this.showPicItems(false);
					this.cbPUE.Visible = false;
					this.palboard.Visible = false;
					this.paloverall.Visible = true;
					this.lblIndex.Visible = false;
					this.meter1.Visible = false;
					this.meter2.Visible = false;
					this.overallchart.Visible = false;
					this.BoardTag = Tag;
					this.boardTimeDate(0);
				}
				else
				{
					if (Name == "Effectiveness")
					{
						this.boardName = "Effectiveness";
						this.palIndices.Visible = false;
						this.overallchart.ChartAreas.Clear();
						this.overallchart.Series.Clear();
						this.overallchart.ChartAreas.Add("ChartArea1");
						this.overallchart.ChartAreas[0].Position.X = 2f;
						this.overallchart.ChartAreas[0].Position.Y = 2f;
						this.overallchart.ChartAreas[0].Position.Width = 98f;
						this.overallchart.ChartAreas[0].Position.Height = 98f;
						if (Tag == "0:6")
						{
							this.overallchart.Location = new Point(40, 40);
							this.overallchart.Width = 800;
							this.overallchart.Height = 450;
							this.lbldiagram.Location = new Point(228, 23);
						}
						else
						{
							this.overallchart.Width = 750;
							this.overallchart.Height = 430;
							this.overallchart.Location = new Point(58, 80);
							this.lbldiagram.Location = new Point(228, 83);
						}
						this.showPicItems(false);
						this.cbPUE.Visible = false;
						this.palboard.Visible = false;
						this.paloverall.Visible = true;
						this.lblIndex.Visible = false;
						this.meter1.Visible = false;
						this.meter2.Visible = false;
						this.overallchart.Visible = false;
						this.BoardTag = Tag;
						this.boardTimeDate(0);
					}
				}
			}
			this.startBoardTimer();
		}
		private void cbodash_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:0"])
			{
				this.tvDevice_AfterSelect("board", "0:0");
			}
			else
			{
				if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:1"])
				{
					this.tvDevice_AfterSelect("board", "0:1");
				}
				else
				{
					if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:2"])
					{
						this.tvDevice_AfterSelect("Effectiveness", "0:2");
					}
					else
					{
						if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:3"])
						{
							this.tvDevice_AfterSelect("Effectiveness", "0:3");
						}
						else
						{
							if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:4"])
							{
								this.tvDevice_AfterSelect("Effectiveness", "0:4");
							}
							else
							{
								if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:5"])
								{
									this.tvDevice_AfterSelect("Effectiveness", "0:5");
								}
								else
								{
									if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:6"])
									{
										this.tvDevice_AfterSelect("Effectiveness", "0:6");
									}
									else
									{
										if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:7"])
										{
											this.cbo_high_low.SelectedIndex = 0;
											this.tvDevice_AfterSelect("boardline", "0:7");
										}
										else
										{
											if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:8"])
											{
												this.tvDevice_AfterSelect("boardline", "0:8");
											}
											else
											{
												if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:9"])
												{
													this.tvDevice_AfterSelect("board", "0:9");
												}
												else
												{
													if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["1:0"])
													{
														this.tvDevice_AfterSelect("board", "1:0");
													}
													else
													{
														if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["1:1"])
														{
															this.tvDevice_AfterSelect("board", "1:1");
														}
														else
														{
															if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["2:0"])
															{
																this.tvDevice_AfterSelect("board", "2:0");
															}
															else
															{
																if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["2:1"])
																{
																	this.tvDevice_AfterSelect("board", "2:1");
																}
																else
																{
																	if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["2:2"])
																	{
																		this.tvDevice_AfterSelect("board", "2:2");
																	}
																	else
																	{
																		if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["2:3"])
																		{
																			this.tvDevice_AfterSelect("board", "2:3");
																		}
																		else
																		{
																			if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["2:4"])
																			{
																				this.tvDevice_AfterSelect("board", "2:4");
																			}
																			else
																			{
																				if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["3:0"])
																				{
																					this.tvDevice_AfterSelect("board", "3:0");
																				}
																				else
																				{
																					if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["3:1"])
																					{
																						this.tvDevice_AfterSelect("board", "3:1");
																					}
																					else
																					{
																						if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["3:2"])
																						{
																							this.tvDevice_AfterSelect("board", "3:2");
																						}
																						else
																						{
																							if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["3:4"])
																							{
																								this.tvDevice_AfterSelect("board", "3:4");
																							}
																							else
																							{
																								if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["3:5"])
																								{
																									this.tvDevice_AfterSelect("board", "3:5");
																								}
																								else
																								{
																									if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["4:0"])
																									{
																										this.tvDevice_AfterSelect("board", "4:0");
																									}
																									else
																									{
																										if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["4:1"])
																										{
																											this.tvDevice_AfterSelect("board", "4:1");
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.label9.Focus();
		}
		private void cbo_high_low_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.boardTimeDate(0);
		}
		private void startBoardTimer()
		{
			this.boardTimer.Enabled = true;
		}
		public void endBoardTimer()
		{
			this.boardTimer.Enabled = false;
			this.FlashTimer.Enabled = false;
		}
		public void resetTimer()
		{
			if (this.boardTimer.Interval == 10000.0)
			{
				this.boardTimer.Interval = 1000.0;
			}
		}
		private void boardTimerEvent(object source, ElapsedEventArgs e)
		{
			this.boardTimer.Interval = 30000.0;
			if (base.InvokeRequired)
			{
				base.Invoke(new DashBoard.boardDate(this.boardTimeDate), new object[]
				{
					1
				});
				return;
			}
			this.boardTimeDate(1);
		}
		private void boardTimeDate(int from)
		{
			System.DateTime arg_05_0 = System.DateTime.Now;
			int num = boardDataUtil.PDUData_FreshFLG_Yes;
			if (this.boardDataInit() != 0)
			{
				num = ClientAPI.updateDataSet(ref this.m_TitleData_Info, from);
			}
			if (num == boardDataUtil.PDUData_FreshFLG_Yes)
			{
				this.FlashColor = boardDataUtil.StatusAlarmColor(this.m_TitleData_Info);
			}
			if (this.btnpower.BackColor != this.FlashColor[0] || this.btnthermal.BackColor != this.FlashColor[1] || this.btnair.BackColor != this.FlashColor[2] || this.btnhumidity.BackColor != this.FlashColor[3])
			{
				if (this.btnpower.BackColor != this.FlashColor[0])
				{
					this.poweralarm = true;
				}
				if (this.btnthermal.BackColor != this.FlashColor[1])
				{
					this.thermalalarm = true;
				}
				if (this.btnair.BackColor != this.FlashColor[2])
				{
					this.airalarm = true;
				}
				if (this.btnhumidity.BackColor != this.FlashColor[3])
				{
					this.humidityalarm = true;
				}
				this.FlashTimer.Enabled = true;
				this.FlashTimes = 1;
			}
			this.showRealtimeData(num);
			if (num == boardDataUtil.PDUData_FreshFLG_NO)
			{
				this.boardTimer.Interval = 1000.0;
				return;
			}
			if (this.boardName == "board")
			{
				string selectedTag = this.m_SelectedTag;
				string text = "";
				System.Collections.Generic.Dictionary<long, Color> dictionary = new System.Collections.Generic.Dictionary<long, Color>();
				dictionary = boardDataUtil.getAllRacksColor(selectedTag, this.m_allRacks, this.m_TitleData_Info, ref text);
				foreach (Control control in this.devRackInfo.Controls)
				{
					long key = (long)control.Tag;
					if (dictionary.ContainsKey(key))
					{
						control.BackColor = dictionary[key];
					}
				}
				if (RackStatusAll.fetal_error)
				{
					this.lbldetection.Text = EcoLanguage.getMsg(LangRes.DashBd_fatalerror, new string[0]);
				}
				else
				{
					this.lbldetection.Text = "";
				}
				if (num == boardDataUtil.PDUData_FreshFLG_NO)
				{
					this.boardTimer.Interval = 1000.0;
					return;
				}
				string text2;
				switch (text2 = RackStatusAll.Board_Tag)
				{
				case "0:0":
					goto IL_5E8D;
				case "0:1":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2") + "%";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2") + "%";
					goto IL_5E8D;
				case "1:0":
				{
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F4") + "kWh";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F4") + "kWh";
					string[] array = text.Split(new string[]
					{
						"#"
					}, System.StringSplitOptions.RemoveEmptyEntries);
					this.lblstart.Text = array[0];
					this.lblcurrenttime.Text = array[1];
					this.m_LocalTime4getSrvTime = System.DateTime.Now;
					goto IL_5E8D;
				}
				case "1:1":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F4") + "W";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F4") + "W";
					goto IL_5E8D;
				case "2:0":
				case "2:1":
				case "2:2":
				case "2:3":
				case "2:4":
					if (EcoGlobalVar.TempUnit_Disp != EcoGlobalVar.TempUnit)
					{
						this.init_typeBoard(RackStatusAll.Board_Tag);
						EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
					}
					this.panelAverage.Visible = true;
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2") + "°C";
						this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2") + "°C";
						goto IL_5E8D;
					}
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.CtoFdegrees(RackStatusAll.AvgValue()).ToString("F2") + "°F";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.CtoFdegrees(RackStatusAll.VarValue()).ToString("F2") + "°F";
					goto IL_5E8D;
				case "3:0":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2") + "Pa";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2") + "Pa";
					goto IL_5E8D;
				case "3:1":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2");
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2");
					goto IL_5E8D;
				case "3:2":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2");
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2");
					goto IL_5E8D;
				case "3:4":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2") + "%";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2") + "%";
					goto IL_5E8D;
				case "3:5":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2") + "%";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2") + "%";
					goto IL_5E8D;
				case "4:0":
					this.panelAverage.Visible = true;
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2") + "%";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2") + "%";
					goto IL_5E8D;
				case "4:1":
					if (EcoGlobalVar.TempUnit_Disp != EcoGlobalVar.TempUnit)
					{
						this.init_typeBoard(RackStatusAll.Board_Tag);
						EcoGlobalVar.TempUnit_Disp = EcoGlobalVar.TempUnit;
					}
					this.panelAverage.Visible = true;
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.AvgValue().ToString("F2") + "°C";
						this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.VarValue().ToString("F2") + "°C";
						goto IL_5E8D;
					}
					this.lblavg.Text = EcoLanguage.getMsg(LangRes.DashBd_average, new string[0]) + " " + RackStatusAll.CtoFdegrees(RackStatusAll.AvgValue()).ToString("F2") + "°F";
					this.lblvar.Text = EcoLanguage.getMsg(LangRes.DashBd_var, new string[0]) + " " + RackStatusAll.CtoFdegrees(RackStatusAll.VarValue()).ToString("F2") + "°F";
					goto IL_5E8D;
				}
				this.panelAverage.Visible = false;
			}
			else
			{
				if (this.boardName == "boardline")
				{
					string selectedTag2 = this.m_SelectedTag;
					this.lbldiagram.Text = "";
					string text2;
					if ((text2 = selectedTag2) != null)
					{
						if (!(text2 == "0:7"))
						{
							if (text2 == "0:8")
							{
								this.palleg.Visible = false;
								this.cbo_high_low.Visible = false;
								double num3 = 0.0;
								double num4 = 0.0;
								double num5 = 0.0;
								double num6 = 0.0;
								double num7 = 0.0;
								double num8 = 27.0;
								double num9 = 32.0;
								double num10 = 0.0;
								double num11 = 0.0;
								double num12 = 0.0;
								double num13 = 0.0;
								System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary2 = boardDataUtil.RackCal_infor(this.m_allRacks, this.m_TitleData_Info);
								foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current in dictionary2)
								{
									RackStatusOne value = current.Value;
									num3 += value.Power;
								}
								foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current2 in dictionary2)
								{
									RackStatusOne value = current2.Value;
									if (value.IntakeSSnum == 0 || value.ExhaustSSnum == 0 || value.FloorSSnum == 0 || value.TEquipk_avg <= 0.0 || value.TFloor_avg <= 0.0 || value.TFloor_avg - value.TIntake_diff <= 0.0 || value.Power == 0.0)
									{
										value.Power = 0.0;
									}
									else
									{
										num6 += (value.TFloor_avg - value.TIntake_diff) * (value.Power / num3);
										num7 += value.TEquipk * (value.Power / num3);
										num4 += value.VEquipk;
									}
								}
								if (num7 != 0.0 && num6 != 0.0)
								{
									num10 = num6 / num7;
									double num14 = num4 / num10;
									foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current3 in dictionary2)
									{
										RackStatusOne value = current3.Value;
										if (value.Power > 0.0)
										{
											double num15 = num14 * value.Power / num3;
											num5 += value.TFloor_avg * num15 / num14;
										}
									}
									num10 *= 100.0;
									num11 = System.Math.Pow(num10 / 100.0, 2.9) * 100.0;
									if (num11 >= 100.0)
									{
										num11 = 100.0;
									}
									num12 = ((num5 < 0.0) ? 0.0 : (100.0 - (num8 - num5) * 2.0));
									num13 = ((num5 < 0.0) ? 0.0 : (100.0 - (num9 - num5) * 2.0));
								}
								this.lblIndex.Visible = false;
								this.meter1.Visible = false;
								this.meter2.Visible = false;
								this.lblIndex.BackColor = Color.White;
								this.lblIndex.BorderStyle = BorderStyle.None;
								this.lblIndex.Location = new Point(330, 5);
								this.lblIndex.AutoSize = true;
								this.overallchart.Visible = true;
								this.overallchart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
								this.overallchart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
								this.overallchart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisFansaving, new string[0]);
								this.overallchart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.overallchart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.overallchart.ChartAreas[0].AxisY.LabelStyle = new LabelStyle
								{
									Format = "{N2}"
								};
								this.overallchart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
								this.overallchart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[0].AxisY.Interval = 10.0;
								this.overallchart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
								this.overallchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
								this.overallchart.ChartAreas[0].AxisX.LabelStyle.Format = "";
								this.overallchart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
								this.overallchart.ChartAreas[0].AxisY.IsStartedFromZero = true;
								this.overallchart.ChartAreas[0].AxisX.IsStartedFromZero = false;
								this.overallchart.ChartAreas[1].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
								this.overallchart.ChartAreas[1].AlignmentStyle = AreaAlignmentStyles.Cursor;
								this.overallchart.ChartAreas[1].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisChillersaving, new string[0]);
								this.overallchart.ChartAreas[1].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.overallchart.ChartAreas[1].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.overallchart.ChartAreas[1].AxisY.LabelStyle = new LabelStyle
								{
									Format = "{N2}"
								};
								this.overallchart.ChartAreas[1].AxisX.TitleAlignment = StringAlignment.Center;
								this.overallchart.ChartAreas[1].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[1].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[1].AxisY.Interval = 10.0;
								this.overallchart.ChartAreas[1].AxisY.MajorGrid.Enabled = true;
								this.overallchart.ChartAreas[1].AxisX.MajorGrid.Enabled = false;
								this.overallchart.ChartAreas[1].AxisX.LabelStyle.Format = "";
								this.overallchart.ChartAreas[1].AxisY.MajorTickMark.Enabled = false;
								this.overallchart.ChartAreas[1].AxisY.IsStartedFromZero = true;
								this.overallchart.ChartAreas[1].AxisX.IsStartedFromZero = false;
								this.overallchart.Series.Clear();
								this.overallchart.Legends.Clear();
								Series series = new Series();
								series.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shMeasured, new string[0]), new object[]
								{
									100
								});
								series.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shPotential, new string[0]), new object[]
								{
									num11
								});
								series.XValueType = ChartValueType.String;
								series.ChartArea = "ChartArea1";
								series.ChartType = SeriesChartType.Column;
								series.Color = Color.FromArgb(162, 215, 48);
								series.BorderWidth = 1;
								this.overallchart.Series.Add(series);
								Series series2 = new Series();
								series2.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shMeasured, new string[0]), new object[]
								{
									100
								});
								series2.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shPotential, new string[0]), new object[]
								{
									num12
								});
								series2.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shAggressive, new string[0]), new object[]
								{
									num13
								});
								series2.XValueType = ChartValueType.String;
								series2.ChartArea = "ChartArea2";
								series2.ChartType = SeriesChartType.Column;
								series2.Color = Color.Blue;
								series2.BorderWidth = 1;
								this.overallchart.Series.Add(series2);
							}
						}
						else
						{
							this.palleg.Visible = false;
							this.cbo_high_low.Visible = true;
							string sql = "select RCI_High,RHI_High,RPI_High,RAI_High,RTI,insert_time from rack_effect";
							if (this.cbo_high_low.SelectedIndex != 0)
							{
								sql = "select RCI_Low,RHI_Low,RPI_Low,RAI_Low,RTI,insert_time from rack_effect";
							}
							DataTable dataTable = boardDataUtil.DispatchAPI_CreateDataTable("BDTag_THOverIndices", sql);
							this.lblIndex.Visible = false;
							this.meter1.Visible = false;
							this.meter2.Visible = false;
							this.lblIndex.BackColor = Color.White;
							this.lblIndex.BorderStyle = BorderStyle.None;
							if (dataTable == null || dataTable.Rows.Count == 0)
							{
								this.palIndices.Visible = false;
								this.cbo_high_low.Visible = false;
								this.lbldiagram.Text = EcoLanguage.getMsg(LangRes.DashBd_IndexnoData, new string[0]);
							}
							else
							{
								this.lblIndex.Location = new Point(340, 5);
								this.lblIndex.AutoSize = true;
								this.overallchart.Visible = true;
								this.overallchart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
								this.overallchart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
								this.overallchart.ChartAreas[0].AxisX.Title = "";
								this.overallchart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.overallchart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.overallchart.ChartAreas[0].AxisY.Title = "(%)";
								this.overallchart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
								this.overallchart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisTime, new string[0]);
								this.overallchart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
								this.overallchart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
								this.overallchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
								this.overallchart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
								this.overallchart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
								this.overallchart.ChartAreas[0].AxisY.IsStartedFromZero = false;
								this.overallchart.ChartAreas[0].AxisX.IsStartedFromZero = false;
								this.overallchart.Series.Clear();
								this.overallchart.Legends.Clear();
								StripLine stripLine = new StripLine();
								stripLine.BorderDashStyle = ChartDashStyle.NotSet;
								stripLine.Interval = 0.0;
								stripLine.StripWidth = 0.001;
								stripLine.IntervalOffset = 100.0;
								stripLine.BackColor = Color.FromArgb(255, Color.Black);
								stripLine.Text = "";
								this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine);
								Legend legend = new Legend();
								legend.Alignment = StringAlignment.Far;
								legend.Docking = Docking.Top;
								legend.LegendStyle = LegendStyle.Column;
								legend.BorderWidth = 1;
								legend.Name = "leg";
								legend.DockedToChartArea = "ChartArea1";
								legend.IsDockedInsideChartArea = true;
								legend.Enabled = false;
								this.overallchart.Legends.Add(legend);
								if (dataTable.Rows.Count == 0)
								{
									this.palIndices.Visible = false;
								}
								else
								{
									this.palIndices.Visible = true;
								}
								Series series3 = new Series();
								DataView dataView = new DataView(dataTable);
								dataView.Sort = "insert_time asc";
								if (this.cbo_high_low.SelectedIndex == 0)
								{
									series3.Points.DataBindXY(dataView, "insert_time", dataView, "RCI_High");
								}
								else
								{
									series3.Points.DataBindXY(dataView, "insert_time", dataView, "RCI_Low");
								}
								series3.XValueType = ChartValueType.DateTime;
								series3.ChartArea = "ChartArea1";
								series3.ChartType = SeriesChartType.Spline;
								series3.Color = Color.Red;
								series3.LegendText = "RCI";
								series3.IsVisibleInLegend = true;
								series3.Legend = "leg";
								series3.BorderWidth = 1;
								this.overallchart.Series.Add(series3);
								Series series4 = new Series();
								if (this.cbo_high_low.SelectedIndex == 0)
								{
									series4.Points.DataBindXY(dataView, "insert_time", dataView, "RHI_High");
								}
								else
								{
									series4.Points.DataBindXY(dataView, "insert_time", dataView, "RHI_Low");
								}
								series4.XValueType = ChartValueType.DateTime;
								series4.ChartArea = "ChartArea1";
								series4.ChartType = SeriesChartType.Spline;
								series4.Color = Color.Blue;
								series4.LegendText = "RHI";
								series4.IsVisibleInLegend = true;
								series4.Legend = "leg";
								series4.BorderWidth = 1;
								this.overallchart.Series.Add(series4);
								Series series5 = new Series();
								if (this.cbo_high_low.SelectedIndex == 0)
								{
									series5.Points.DataBindXY(dataView, "insert_time", dataView, "RPI_High");
								}
								else
								{
									series5.Points.DataBindXY(dataView, "insert_time", dataView, "RPI_Low");
								}
								series5.XValueType = ChartValueType.DateTime;
								series5.ChartArea = "ChartArea1";
								series5.ChartType = SeriesChartType.Spline;
								series5.Color = Color.Silver;
								series5.LegendText = "RPI";
								series5.IsVisibleInLegend = true;
								series5.Legend = "leg";
								series5.BorderWidth = 1;
								this.overallchart.Series.Add(series5);
								Series series6 = new Series();
								if (this.cbo_high_low.SelectedIndex == 0)
								{
									series6.Points.DataBindXY(dataView, "insert_time", dataView, "RAI_High");
								}
								else
								{
									series6.Points.DataBindXY(dataView, "insert_time", dataView, "RAI_Low");
								}
								series6.XValueType = ChartValueType.DateTime;
								series6.ChartArea = "ChartArea1";
								series6.ChartType = SeriesChartType.Spline;
								series6.Color = Color.FromArgb(162, 215, 48);
								series6.LegendText = "RAI";
								series6.IsVisibleInLegend = true;
								series6.Legend = "leg";
								series6.BorderWidth = 1;
								this.overallchart.Series.Add(series6);
								Series series7 = new Series();
								series7.Points.DataBindXY(dataView, "insert_time", dataView, "RTI");
								series7.XValueType = ChartValueType.DateTime;
								series7.ChartArea = "ChartArea1";
								series7.ChartType = SeriesChartType.Spline;
								series7.Color = Color.Orange;
								series7.LegendText = "RTI";
								series7.IsVisibleInLegend = true;
								series7.Legend = "leg";
								series7.BorderWidth = 1;
								this.overallchart.Series.Add(series7);
							}
						}
					}
					this.overallchart.ResetAutoValues();
				}
				else
				{
					if (this.boardName == "Effectiveness")
					{
						string selectedTag3 = this.m_SelectedTag;
						this.cbo_high_low.Visible = false;
						string text2;
						if ((text2 = selectedTag3) != null)
						{
							if (!(text2 == "0:2"))
							{
								if (!(text2 == "0:3"))
								{
									if (!(text2 == "0:4"))
									{
										if (!(text2 == "0:5"))
										{
											if (text2 == "0:6")
											{
												commUtil.ShowInfo_DEBUG("Start RTI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
												this.palleg.Visible = false;
												string sql2 = "select RTI,insert_time from rack_effect";
												DataTable dataTable2 = boardDataUtil.DispatchAPI_CreateDataTable("BDTag_THRTI", sql2);
												if (dataTable2 != null && dataTable2.Rows.Count >= 1)
												{
													this.lbldiagram.Text = "";
													this.lblIndex.Visible = false;
													this.meter1.Visible = false;
													this.meter2.Visible = false;
													this.lblIndex.BackColor = Color.White;
													this.lblIndex.BorderStyle = BorderStyle.None;
													this.lblIndex.Location = new Point(300, 5);
													this.lblIndex.AutoSize = true;
													this.overallchart.Visible = true;
													this.overallchart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
													this.overallchart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
													this.overallchart.ChartAreas[0].AxisX.Title = "";
													this.overallchart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
													this.overallchart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
													this.overallchart.ChartAreas[0].AxisY.Title = "RTI(%)";
													this.overallchart.ChartAreas[0].AxisY.LabelStyle = new LabelStyle
													{
														Format = "{N2}"
													};
													this.overallchart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
													this.overallchart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisTime, new string[0]);
													this.overallchart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
													this.overallchart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
													this.overallchart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
													this.overallchart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
													this.overallchart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
													this.overallchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
													this.overallchart.ChartAreas[0].AxisY.IsStartedFromZero = false;
													this.overallchart.ChartAreas[0].AxisX.IsStartedFromZero = false;
													this.overallchart.Series.Clear();
													this.overallchart.Legends.Clear();
													Series series8 = new Series();
													DataView dataView2 = new DataView(dataTable2);
													dataView2.Sort = "insert_time asc";
													series8.Points.DataBindXY(dataView2, "insert_time", dataView2, "RTI");
													series8.XValueType = ChartValueType.DateTime;
													series8.Name = "RTI";
													series8.ChartArea = "ChartArea1";
													series8.ChartType = SeriesChartType.Spline;
													series8.BorderWidth = 1;
													this.overallchart.Series.Add(series8);
													commUtil.ShowInfo_DEBUG("END RTI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
												}
												else
												{
													this.lbldiagram.Text = EcoLanguage.getMsg(LangRes.DashBd_noenoughSS, new string[0]);
												}
											}
										}
										else
										{
											commUtil.ShowInfo_DEBUG("Start RAI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
											this.palleg.Visible = true;
											double num16 = 0.0;
											double num17 = 0.0;
											double num18 = 0.0;
											double num19 = 0.0;
											DataTable dataTable3 = new DataTable();
											DataColumn dataColumn = new DataColumn();
											dataColumn.DataType = System.Type.GetType("System.Int32");
											dataColumn.ColumnName = "no";
											dataTable3.Columns.Add(dataColumn);
											dataColumn = new DataColumn();
											dataColumn.DataType = System.Type.GetType("System.Double");
											dataColumn.ColumnName = "v_cfm";
											dataTable3.Columns.Add(dataColumn);
											int num20 = 0;
											System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
											double num21 = 0.0;
											double num22 = 0.0;
											double num23 = 0.0;
											double num24 = 0.0;
											int num25 = 0;
											double num26 = 0.0;
											double num27 = 0.0;
											double num28 = 0.0;
											double num29 = 0.0;
											System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary3 = boardDataUtil.RackCal_infor(this.m_allRacks, this.m_TitleData_Info);
											foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current4 in dictionary3)
											{
												RackStatusOne value2 = current4.Value;
												num28 += value2.Power;
											}
											foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current5 in dictionary3)
											{
												RackStatusOne value2 = current5.Value;
												if (value2.IntakeSSnum == 0 || value2.ExhaustSSnum == 0 || value2.FloorSSnum == 0 || value2.TEquipk_avg <= 0.0 || value2.TFloor_avg <= 0.0 || value2.TFloor_avg - value2.TIntake_diff <= 0.0 || value2.Power == 0.0)
												{
													value2.Power = 0.0;
												}
												else
												{
													num25++;
													num26 += (value2.TFloor_avg - value2.TIntake_diff) * (value2.Power / num28);
													num27 += value2.TEquipk * (value2.Power / num28);
													num29 += value2.VEquipk;
												}
											}
											num21 = num29 * 1.3;
											num22 = num29 * 1.15;
											num23 = num29 * 0.75;
											num24 = num29 * 0.85;
											if (num26 != 0.0 && num27 != 0.0)
											{
												double num30 = num26 / num27;
												double num31 = num29 / num30;
												foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current6 in dictionary3)
												{
													RackStatusOne value2 = current6.Value;
													if (value2.Power > 0.0)
													{
														double num32 = num31 * value2.Power / num28;
														double vEquipk = value2.VEquipk;
														double num33 = vEquipk * 1.15;
														double num34 = vEquipk * 0.85;
														if (num32 > num33)
														{
															num16 += num32 - num33;
														}
														if (num34 > num32)
														{
															num17 += num34 - num32;
														}
														arrayList.Add(num32);
													}
												}
											}
											double num35 = (num25 == 0) ? 0.0 : (num21 / (double)num25);
											double num36 = (num25 == 0) ? 0.0 : (num22 / (double)num25);
											double num37 = (num25 == 0) ? 0.0 : (num23 / (double)num25);
											double num38 = (num25 == 0) ? 0.0 : (num24 / (double)num25);
											arrayList.Sort();
											foreach (double num39 in arrayList)
											{
												DataRow dataRow = dataTable3.NewRow();
												num20++;
												dataRow["no"] = num20;
												dataRow["v_cfm"] = num39;
												dataTable3.Rows.Add(dataRow);
											}
											if (arrayList.Count >= 1)
											{
												this.lbldiagram.Text = "";
											}
											else
											{
												this.lbldiagram.Text = EcoLanguage.getMsg(LangRes.DashBd_noenoughSS, new string[0]);
											}
											if (arrayList.Count == 0)
											{
												num18 = 0.0;
												num19 = 0.0;
											}
											else
											{
												if (num35 - num36 != 0.0)
												{
													num18 = (1.0 - num16 / ((num35 - num36) * (double)arrayList.Count)) * 100.0;
												}
												if (num38 - num37 != 0.0)
												{
													num19 = (1.0 - num17 / ((num38 - num37) * (double)arrayList.Count)) * 100.0;
												}
											}
											commUtil.ShowInfo_DEBUG("Start RAI --- 1 : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
											this.lblIndex.Text = string.Concat(new string[]
											{
												"RAI_Hi=",
												num18.ToString("F2"),
												"(%)\r\n\r\nRAI_Lo=",
												num19.ToString("F2"),
												"(%)"
											});
											this.lblIndex.Visible = true;
											this.meter1.Visible = false;
											this.meter2.Visible = false;
											this.lblIndex.BorderStyle = BorderStyle.FixedSingle;
											this.lblIndex.Location = new Point(323, 5);
											this.lblIndex.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
											this.lblIndex.Height = 73;
											this.lblIndex.Width = 167;
											this.lblIndex.AutoSize = false;
											if (dataTable3.Rows.Count > 0)
											{
												if (num18 < 90.0 || num19 < 90.0)
												{
													this.lblIndex.BackColor = Color.Red;
												}
												else
												{
													if (num18 >= 95.0 && num19 >= 95.0)
													{
														this.lblIndex.BackColor = Color.FromArgb(162, 215, 48);
													}
													else
													{
														this.lblIndex.BackColor = Color.Orange;
													}
												}
											}
											else
											{
												this.lblIndex.BackColor = Color.Silver;
												this.lblIndex.Text = "N/A";
											}
											this.lblIndex.Visible = true;
											this.meter1.Visible = false;
											this.meter2.Visible = false;
											this.overallchart.Visible = true;
											this.overallchart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
											this.overallchart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
											this.overallchart.ChartAreas[0].AxisX.Title = "";
											this.overallchart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
											this.overallchart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
											this.overallchart.ChartAreas[0].AxisY.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisRackAirflow, new string[0]);
											this.overallchart.ChartAreas[0].AxisY.LabelStyle = new LabelStyle
											{
												Format = "{N2}"
											};
											this.overallchart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
											this.overallchart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisRackNum, new string[0]);
											this.overallchart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
											this.overallchart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
											this.overallchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
											this.overallchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
											this.overallchart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
											if (dataTable3.Rows.Count > 0)
											{
												this.overallchart.ChartAreas[0].AxisY.Maximum = System.Convert.ToDouble(dataTable3.Rows[dataTable3.Rows.Count - 1]["v_cfm"]) + 0.1;
												this.overallchart.ChartAreas[0].AxisY.Minimum = System.Convert.ToDouble(dataTable3.Rows[0]["v_cfm"]) - 0.1;
												this.overallchart.ChartAreas[0].AxisX.Maximum = (double)dataTable3.Rows.Count;
												this.overallchart.ChartAreas[0].AxisX.Minimum = 1.0;
											}
											this.overallchart.ChartAreas[0].AxisY.IsStartedFromZero = false;
											this.overallchart.ChartAreas[0].AxisX.IsStartedFromZero = false;
											this.overallchart.Series.Clear();
											this.overallchart.Legends.Clear();
											Series series9 = new Series();
											DataView dataView3 = new DataView(dataTable3);
											dataView3.Sort = "no asc";
											series9.Points.DataBindXY(dataView3, "no", dataView3, "v_cfm");
											series9.XValueType = ChartValueType.Int64;
											series9.Name = "RAI";
											series9.ChartArea = "ChartArea1";
											series9.ChartType = SeriesChartType.Spline;
											series9.MarkerStyle = MarkerStyle.Circle;
											series9.MarkerSize = 4;
											series9.IsValueShownAsLabel = true;
											series9.LabelFormat = "F2";
											if (num18 < 90.0 || num19 < 90.0)
											{
												series9.Color = Color.Red;
											}
											else
											{
												if (num18 >= 95.0 && num19 >= 95.0)
												{
													series9.Color = Color.FromArgb(162, 215, 48);
												}
												else
												{
													series9.Color = Color.Orange;
												}
											}
											series9.BorderWidth = 2;
											this.overallchart.Series.Add(series9);
											if (dataTable3.Rows.Count >= 5)
											{
												series9.Enabled = false;
												Series series10 = new Series();
												series10.Name = "New";
												series10.ChartType = SeriesChartType.Spline;
												series10.BorderWidth = 2;
												if (num18 < 90.0 || num19 < 90.0)
												{
													series10.Color = Color.Red;
												}
												else
												{
													if (num18 >= 95.0 && num19 >= 95.0)
													{
														series10.Color = Color.FromArgb(162, 215, 48);
													}
													else
													{
														series10.Color = Color.Orange;
													}
												}
												this.overallchart.Series.Add(series10);
												string text3 = "5";
												int num40 = 10;
												string text4 = "false";
												string text5 = "false";
												string parameters = string.Concat(new object[]
												{
													text3,
													',',
													num40,
													',',
													text4,
													',',
													text5
												});
												this.overallchart.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters, "RAI", "New");
											}
											else
											{
												series9.Enabled = true;
											}
											this.overallchart.ChartAreas[0].AxisY.StripLines.Clear();
											StripLine stripLine2 = new StripLine();
											stripLine2.BorderDashStyle = ChartDashStyle.NotSet;
											stripLine2.Interval = 0.0;
											stripLine2.StripWidth = num38 - num37;
											stripLine2.IntervalOffset = num37;
											stripLine2.BackColor = Color.FromArgb(120, Color.Gold);
											stripLine2.Text = "";
											stripLine2.TextAlignment = StringAlignment.Far;
											stripLine2.TextLineAlignment = StringAlignment.Near;
											this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine2);
											StripLine stripLine3 = new StripLine();
											stripLine3.BorderDashStyle = ChartDashStyle.NotSet;
											stripLine3.Interval = 0.0;
											stripLine3.StripWidth = 0.0001;
											stripLine3.IntervalOffset = num38;
											stripLine3.BackColor = Color.FromArgb(120, Color.Black);
											stripLine3.Text = EcoLanguage.getMsg(LangRes.DashBd_minRecommend, new string[0]) + ":" + num38.ToString("F2");
											stripLine3.TextAlignment = StringAlignment.Far;
											stripLine3.TextLineAlignment = StringAlignment.Near;
											this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine3);
											StripLine stripLine4 = new StripLine();
											stripLine4.BorderDashStyle = ChartDashStyle.NotSet;
											stripLine4.Interval = 0.0;
											stripLine4.StripWidth = 0.0001;
											stripLine4.IntervalOffset = num37;
											stripLine4.BackColor = Color.FromArgb(120, Color.Black);
											stripLine4.Text = EcoLanguage.getMsg(LangRes.DashBd_minAllow, new string[0]) + ":" + num37.ToString("F2");
											stripLine4.TextAlignment = StringAlignment.Far;
											stripLine4.TextLineAlignment = StringAlignment.Near;
											this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine4);
											StripLine stripLine5 = new StripLine();
											stripLine5.BorderDashStyle = ChartDashStyle.NotSet;
											stripLine5.Interval = 0.0;
											stripLine5.StripWidth = num35 - num36;
											stripLine5.IntervalOffset = num36;
											stripLine5.BackColor = Color.FromArgb(120, Color.Gold);
											stripLine5.Text = "";
											stripLine5.TextAlignment = StringAlignment.Far;
											stripLine5.TextLineAlignment = StringAlignment.Near;
											this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine5);
											StripLine stripLine6 = new StripLine();
											stripLine6.BorderDashStyle = ChartDashStyle.NotSet;
											stripLine6.Interval = 0.0;
											stripLine6.StripWidth = 0.0001;
											stripLine6.IntervalOffset = num35;
											stripLine6.BackColor = Color.FromArgb(120, Color.Black);
											stripLine6.Text = EcoLanguage.getMsg(LangRes.DashBd_maxAllow, new string[0]) + ":" + num35.ToString("F2");
											stripLine6.TextAlignment = StringAlignment.Far;
											stripLine6.TextLineAlignment = StringAlignment.Near;
											this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine6);
											StripLine stripLine7 = new StripLine();
											stripLine7.BorderDashStyle = ChartDashStyle.NotSet;
											stripLine7.Interval = 0.0;
											stripLine7.StripWidth = 0.0001;
											stripLine7.IntervalOffset = num36;
											stripLine7.BackColor = Color.FromArgb(120, Color.Black);
											stripLine7.Text = EcoLanguage.getMsg(LangRes.DashBd_maxRecommend, new string[0]) + ":" + num36.ToString("F2");
											stripLine7.TextAlignment = StringAlignment.Far;
											stripLine7.TextLineAlignment = StringAlignment.Near;
											this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine7);
											commUtil.ShowInfo_DEBUG("End RAI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
										}
									}
									else
									{
										commUtil.ShowInfo_DEBUG("Start RPI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
										this.palleg.Visible = true;
										double num41 = 0.07;
										double num42 = 0.03;
										double num43 = 0.0;
										double num44 = 0.0;
										DataTable dataTable4 = new DataTable();
										DataColumn dataColumn2 = new DataColumn();
										dataColumn2.DataType = System.Type.GetType("System.Int32");
										dataColumn2.ColumnName = "no";
										dataTable4.Columns.Add(dataColumn2);
										dataColumn2 = new DataColumn();
										dataColumn2.DataType = System.Type.GetType("System.Double");
										dataColumn2.ColumnName = "press_value";
										dataTable4.Columns.Add(dataColumn2);
										commUtil.ShowInfo_DEBUG("Start RPI ---1: " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
										DataRow[] array2 = this.m_TitleData_Info.Tables[1].Select(string.Concat(new object[]
										{
											"sensor_location=",
											2,
											" and press_value <>'",
											-500,
											"' and press_value <>'",
											-1000,
											"'"
										}), "press_value asc");
										commUtil.ShowInfo_DEBUG("Start RPI ---2: " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
										if (array2.Length >= 1)
										{
											this.lbldiagram.Text = "";
										}
										else
										{
											this.lbldiagram.Text = EcoLanguage.getMsg(LangRes.DashBd_noenoughSS, new string[0]);
										}
										double num46;
										double num47;
										if (array2.Length > 0)
										{
											for (int i = 0; i < array2.Length; i++)
											{
												DataRow dataRow2 = dataTable4.NewRow();
												dataRow2["no"] = i + 1;
												dataRow2["press_value"] = ecoConvert.f2d(array2[i]["press_value"]) / 249.1;
												dataTable4.Rows.Add(dataRow2);
												double num45 = ecoConvert.f2d(array2[i]["press_value"]) / 249.1;
												if (num45 > num41)
												{
													num43 += num45 - num41;
												}
												if (num42 > num45)
												{
													num44 += num42 - num45;
												}
											}
											num46 = (1.0 - num43 / ((num41 - num42) * (double)dataTable4.Rows.Count / 2.0)) * 100.0;
											num47 = (1.0 - num44 / ((num41 - num42) * (double)dataTable4.Rows.Count / 2.0)) * 100.0;
										}
										else
										{
											num46 = 0.0;
											num47 = 0.0;
										}
										commUtil.ShowInfo_DEBUG("Start RPI --- 3 : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
										this.lblIndex.Text = string.Concat(new string[]
										{
											"RPI_Hi=",
											num46.ToString("F2"),
											"(%)\r\n\r\nRPI_Lo=",
											num47.ToString("F2"),
											"(%)"
										});
										this.lblIndex.BorderStyle = BorderStyle.FixedSingle;
										this.lblIndex.Location = new Point(323, 5);
										this.lblIndex.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
										this.lblIndex.Height = 73;
										this.lblIndex.Width = 167;
										this.lblIndex.AutoSize = false;
										if (dataTable4.Rows.Count > 0)
										{
											if (num46 < 90.0 || num47 < 90.0)
											{
												this.lblIndex.BackColor = Color.Red;
											}
											else
											{
												if (num46 >= 95.0 && num47 >= 95.0)
												{
													this.lblIndex.BackColor = Color.FromArgb(162, 215, 48);
												}
												else
												{
													this.lblIndex.BackColor = Color.Orange;
												}
											}
										}
										else
										{
											this.lblIndex.BackColor = Color.Silver;
											this.lblIndex.Text = "N/A";
										}
										this.lblIndex.Visible = true;
										this.meter1.Visible = false;
										this.meter2.Visible = false;
										this.overallchart.Visible = true;
										this.overallchart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
										this.overallchart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
										this.overallchart.ChartAreas[0].AxisX.Title = "";
										this.overallchart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
										this.overallchart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
										this.overallchart.ChartAreas[0].AxisY.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisRackDiffPress, new string[0]);
										this.overallchart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
										this.overallchart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisPresSSnum, new string[0]);
										this.overallchart.ChartAreas[0].AxisY.LabelStyle = new LabelStyle
										{
											Format = "{N2}"
										};
										this.overallchart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
										this.overallchart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
										this.overallchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
										this.overallchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
										this.overallchart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
										if (dataTable4.Rows.Count > 0)
										{
											if (System.Convert.ToDouble(dataTable4.Rows[dataTable4.Rows.Count - 1]["press_value"]) < 0.07)
											{
												this.overallchart.ChartAreas[0].AxisY.Maximum = 0.075;
											}
											else
											{
												this.overallchart.ChartAreas[0].AxisY.Maximum = System.Convert.ToDouble(dataTable4.Rows[dataTable4.Rows.Count - 1]["press_value"]) + 0.005;
											}
											if (System.Convert.ToDouble(dataTable4.Rows[0]["press_value"]) > 0.03)
											{
												this.overallchart.ChartAreas[0].AxisY.Minimum = 0.025;
											}
											else
											{
												this.overallchart.ChartAreas[0].AxisY.Minimum = System.Convert.ToDouble(dataTable4.Rows[0]["press_value"]) - 0.005;
											}
											this.overallchart.ChartAreas[0].AxisX.Maximum = (double)dataTable4.Rows.Count;
											this.overallchart.ChartAreas[0].AxisX.Minimum = 1.0;
										}
										this.overallchart.ChartAreas[0].AxisY.IsStartedFromZero = false;
										this.overallchart.ChartAreas[0].AxisX.IsStartedFromZero = false;
										this.overallchart.Series.Clear();
										this.overallchart.Legends.Clear();
										Series series11 = new Series();
										DataView dataView4 = new DataView(dataTable4);
										series11.Points.DataBindXY(dataView4, "no", dataView4, "press_value");
										series11.XValueType = ChartValueType.Int64;
										series11.Name = "RPI";
										series11.ChartArea = "ChartArea1";
										series11.ChartType = SeriesChartType.Spline;
										series11.MarkerStyle = MarkerStyle.Circle;
										series11.MarkerSize = 4;
										series11.IsValueShownAsLabel = true;
										series11.LabelFormat = "F2";
										if (num46 < 90.0 || num47 < 90.0)
										{
											series11.Color = Color.Red;
										}
										else
										{
											if (num46 >= 95.0 && num47 >= 95.0)
											{
												series11.Color = Color.FromArgb(162, 215, 48);
											}
											else
											{
												series11.Color = Color.Orange;
											}
										}
										series11.BorderWidth = 2;
										this.overallchart.Series.Add(series11);
										if (dataTable4.Rows.Count >= 5)
										{
											series11.Enabled = false;
											Series series12 = new Series();
											series12.Name = "New";
											series12.ChartType = SeriesChartType.Spline;
											series12.BorderWidth = 2;
											if (num46 < 90.0 || num47 < 90.0)
											{
												series12.Color = Color.Red;
											}
											else
											{
												if (num46 >= 95.0 && num47 >= 95.0)
												{
													series12.Color = Color.FromArgb(162, 215, 48);
												}
												else
												{
													series12.Color = Color.Orange;
												}
											}
											this.overallchart.Series.Add(series12);
											string text6 = "5";
											int num48 = 10;
											string text7 = "false";
											string text8 = "false";
											string parameters2 = string.Concat(new object[]
											{
												text6,
												',',
												num48,
												',',
												text7,
												',',
												text8
											});
											this.overallchart.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters2, "RPI", "New");
										}
										else
										{
											series11.Enabled = true;
										}
										this.overallchart.ChartAreas[0].AxisY.StripLines.Clear();
										StripLine stripLine8 = new StripLine();
										stripLine8.BorderDashStyle = ChartDashStyle.NotSet;
										stripLine8.Interval = 0.0;
										stripLine8.StripWidth = 0.04;
										stripLine8.IntervalOffset = 0.03;
										stripLine8.BackColor = Color.FromArgb(120, Color.Gold);
										stripLine8.Text = "";
										stripLine8.TextAlignment = StringAlignment.Far;
										stripLine8.TextLineAlignment = StringAlignment.Near;
										this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine8);
										StripLine stripLine9 = new StripLine();
										stripLine9.BorderDashStyle = ChartDashStyle.NotSet;
										stripLine9.Interval = 0.0;
										stripLine9.StripWidth = 0.0001;
										stripLine9.IntervalOffset = 0.03;
										stripLine9.BackColor = Color.FromArgb(120, Color.Black);
										stripLine9.Text = EcoLanguage.getMsg(LangRes.DashBd_minRecommend, new string[0]) + ":" + System.Convert.ToString(0.03);
										stripLine9.TextAlignment = StringAlignment.Far;
										stripLine9.TextLineAlignment = StringAlignment.Near;
										this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine9);
										StripLine stripLine10 = new StripLine();
										stripLine10.BorderDashStyle = ChartDashStyle.NotSet;
										stripLine10.Interval = 0.0;
										stripLine10.StripWidth = 0.0001;
										stripLine10.IntervalOffset = 0.07;
										stripLine10.BackColor = Color.FromArgb(120, Color.Black);
										stripLine10.Text = EcoLanguage.getMsg(LangRes.DashBd_maxRecommend, new string[0]) + ":" + System.Convert.ToString(0.07);
										stripLine10.TextAlignment = StringAlignment.Far;
										stripLine10.TextLineAlignment = StringAlignment.Near;
										this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine10);
										commUtil.ShowInfo_DEBUG("END RPI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
									}
								}
								else
								{
									commUtil.ShowInfo_DEBUG("Start RHI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
									this.palleg.Visible = true;
									double num49 = 15.0;
									double num50 = 5.5;
									double num51 = 0.6;
									double num52 = 0.0;
									double num53 = 0.0;
									double num54 = 0.0;
									DataTable dataTable5 = new DataTable();
									DataColumn dataColumn3 = new DataColumn();
									dataColumn3.DataType = System.Type.GetType("System.Int32");
									dataColumn3.ColumnName = "no";
									dataTable5.Columns.Add(dataColumn3);
									dataColumn3 = new DataColumn();
									dataColumn3.DataType = System.Type.GetType("System.Double");
									dataColumn3.ColumnName = "dew_point";
									dataTable5.Columns.Add(dataColumn3);
									dataColumn3 = new DataColumn();
									dataColumn3.DataType = System.Type.GetType("System.Double");
									dataColumn3.ColumnName = "C2F";
									dataTable5.Columns.Add(dataColumn3);
									DataTable dataTable6 = this.m_TitleData_Info.Tables[1].Clone();
									commUtil.ShowInfo_DEBUG("Start RHI --- 1 : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
									DataRow[] array3 = this.m_TitleData_Info.Tables[1].Select(string.Concat(new object[]
									{
										"sensor_location=",
										0,
										" and temperature <>'",
										-500,
										"' and temperature <>'",
										-1000,
										"' and humidity <>'",
										-500,
										"' and humidity <>'",
										-1000,
										"'"
									}));
									commUtil.ShowInfo_DEBUG("Start RHI --- 2 : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
									for (int j = 0; j < array3.Length; j++)
									{
										dataTable6.ImportRow(array3[j]);
									}
									dataColumn3 = new DataColumn();
									dataColumn3.DataType = System.Type.GetType("System.Double");
									dataColumn3.ColumnName = "dew_point";
									dataTable6.Columns.Add(dataColumn3);
									for (int k = 0; k < dataTable6.Rows.Count; k++)
									{
										double num55 = ecoConvert.f2d(dataTable6.Rows[k]["temperature"]);
										double num56 = ecoConvert.f2d(dataTable6.Rows[k]["humidity"]);
										double num57 = 0.0;
										if (num56 > 0.0)
										{
											double num58 = 17.27 * num55 / (237.7 + num55) + System.Math.Log(num56 / 100.0, 2.7182818284590451);
											num57 = 237.7 * num58 / (17.27 - num58);
										}
										dataTable6.Rows[k]["dew_point"] = num57;
									}
									array3 = dataTable6.Select(" ", "dew_point asc");
									if (array3.Length >= 1)
									{
										this.lbldiagram.Text = "";
									}
									else
									{
										this.lbldiagram.Text = EcoLanguage.getMsg(LangRes.DashBd_noenoughSS, new string[0]);
									}
									double num61;
									double num62;
									if (array3.Length > 0)
									{
										for (int l = 0; l < array3.Length; l++)
										{
											double num59 = ecoConvert.f2d(array3[l]["humidity"]);
											double num60;
											if (array3[l]["dew_point"] != System.DBNull.Value)
											{
												num60 = ecoConvert.f2d(array3[l]["dew_point"]);
											}
											else
											{
												num60 = 0.0;
											}
											DataRow dataRow3 = dataTable5.NewRow();
											dataRow3["no"] = l + 1;
											dataRow3["dew_point"] = num60;
											dataRow3["C2F"] = RackStatusAll.CtoFdegrees(num60);
											dataTable5.Rows.Add(dataRow3);
											if (num60 > num49)
											{
												num52 += num60 - num49;
											}
											if (num50 > num60)
											{
												num53 += num50 - num60;
											}
											if (num59 / 100.0 > num51)
											{
												num54 += num59 / 100.0 - num51;
											}
										}
										num61 = (1.0 - (num52 / ((num49 - num50) * (double)dataTable5.Rows.Count / 2.0) + num54 / (num51 * (double)dataTable5.Rows.Count / 2.0))) * 100.0;
										num62 = (1.0 - num53 / ((num49 - num50) * (double)dataTable5.Rows.Count / 2.0)) * 100.0;
									}
									else
									{
										num61 = 0.0;
										num62 = 0.0;
									}
									commUtil.ShowInfo_DEBUG("Start RHI --- 3 : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
									this.lblIndex.Text = string.Concat(new string[]
									{
										"RHI_Hi=",
										num61.ToString("F2"),
										"(%)\r\n\r\nRHI_Lo=",
										num62.ToString("F2"),
										"(%)"
									});
									this.lblIndex.Visible = true;
									this.meter1.Visible = false;
									this.meter2.Visible = false;
									this.lblIndex.BorderStyle = BorderStyle.FixedSingle;
									this.lblIndex.Location = new Point(323, 5);
									this.lblIndex.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
									this.lblIndex.Height = 73;
									this.lblIndex.Width = 167;
									this.lblIndex.AutoSize = false;
									if (dataTable5.Rows.Count > 0)
									{
										if (num61 < 90.0 || num62 < 90.0)
										{
											this.lblIndex.BackColor = Color.Red;
										}
										else
										{
											if (num61 >= 95.0 && num62 >= 95.0)
											{
												this.lblIndex.BackColor = Color.FromArgb(162, 215, 48);
											}
											else
											{
												this.lblIndex.BackColor = Color.Orange;
											}
										}
									}
									else
									{
										this.lblIndex.BackColor = Color.Silver;
										this.lblIndex.Text = "N/A";
									}
									this.lblIndex.Visible = true;
									this.meter1.Visible = false;
									this.meter2.Visible = false;
									this.overallchart.Visible = true;
									this.overallchart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
									this.overallchart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
									this.overallchart.ChartAreas[0].AxisX.Title = "";
									this.overallchart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
									this.overallchart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
									string yFields;
									string str;
									double num63;
									double num64;
									if (EcoGlobalVar.TempUnit == 0)
									{
										yFields = "dew_point";
										str = "°C";
										num63 = num49;
										num64 = num50;
										this.overallchart.ChartAreas[0].AxisY.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisRackintakeDewPt, new string[0]) + "(°C)";
									}
									else
									{
										yFields = "C2F";
										str = "°F";
										num63 = RackStatusAll.CtoFdegrees(num49);
										num64 = RackStatusAll.CtoFdegrees(num50);
										this.overallchart.ChartAreas[0].AxisY.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisRackintakeDewPt, new string[0]) + "(°F)";
									}
									this.overallchart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
									this.overallchart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisHumiSSnum, new string[0]);
									this.overallchart.ChartAreas[0].AxisY.LabelStyle = new LabelStyle
									{
										Format = "{N2}"
									};
									this.overallchart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
									this.overallchart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
									this.overallchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
									this.overallchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
									this.overallchart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
									if (dataTable5.Rows.Count > 0)
									{
										this.overallchart.ChartAreas[0].AxisX.Maximum = (double)System.Convert.ToInt16(dataTable5.Rows[dataTable5.Rows.Count - 1]["no"]);
										this.overallchart.ChartAreas[0].AxisX.Minimum = 1.0;
									}
									this.overallchart.ChartAreas[0].AxisY.IsStartedFromZero = false;
									this.overallchart.ChartAreas[0].AxisX.IsStartedFromZero = false;
									this.overallchart.Series.Clear();
									this.overallchart.Legends.Clear();
									Series series13 = new Series();
									DataView dataView5 = new DataView(dataTable5);
									series13.Points.DataBindXY(dataView5, "no", dataView5, yFields);
									series13.XValueType = ChartValueType.Int64;
									series13.Name = "RHI";
									series13.MarkerStyle = MarkerStyle.Circle;
									series13.MarkerSize = 4;
									series13.IsValueShownAsLabel = true;
									series13.LabelFormat = "F2";
									series13.ChartArea = "ChartArea1";
									series13.ChartType = SeriesChartType.Spline;
									if (num61 < 90.0 || num62 < 90.0)
									{
										series13.Color = Color.Red;
									}
									else
									{
										if (num61 >= 95.0 && num62 >= 95.0)
										{
											series13.Color = Color.FromArgb(162, 215, 48);
										}
										else
										{
											series13.Color = Color.Orange;
										}
									}
									series13.BorderWidth = 2;
									this.overallchart.Series.Add(series13);
									if (dataTable5.Rows.Count >= 5)
									{
										series13.Enabled = false;
										Series series14 = new Series();
										series14.Name = "New";
										series14.ChartType = SeriesChartType.Spline;
										series14.BorderWidth = 2;
										if (num61 < 90.0 || num62 < 90.0)
										{
											series14.Color = Color.Red;
										}
										else
										{
											if (num61 >= 95.0 && num62 >= 95.0)
											{
												series14.Color = Color.FromArgb(162, 215, 48);
											}
											else
											{
												series14.Color = Color.Orange;
											}
										}
										this.overallchart.Series.Add(series14);
										string text9 = "5";
										int num65 = 10;
										string text10 = "false";
										string text11 = "false";
										string parameters3 = string.Concat(new object[]
										{
											text9,
											',',
											num65,
											',',
											text10,
											',',
											text11
										});
										this.overallchart.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters3, "RHI", "New");
									}
									else
									{
										series13.Enabled = true;
									}
									this.overallchart.ChartAreas[0].AxisY.StripLines.Clear();
									StripLine stripLine11 = new StripLine();
									stripLine11.BorderDashStyle = ChartDashStyle.NotSet;
									stripLine11.Interval = 0.0;
									stripLine11.StripWidth = num63 - num64;
									stripLine11.IntervalOffset = num64;
									stripLine11.BackColor = Color.FromArgb(120, Color.Gold);
									stripLine11.Text = "";
									stripLine11.TextAlignment = StringAlignment.Far;
									stripLine11.TextLineAlignment = StringAlignment.Near;
									this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine11);
									StripLine stripLine12 = new StripLine();
									stripLine12.BorderDashStyle = ChartDashStyle.NotSet;
									stripLine12.Interval = 0.0;
									stripLine12.StripWidth = 0.0001;
									stripLine12.IntervalOffset = num64;
									stripLine12.BackColor = Color.FromArgb(120, Color.Black);
									stripLine12.Text = EcoLanguage.getMsg(LangRes.DashBd_minRecommend, new string[0]) + ":" + num64.ToString("F1") + str;
									stripLine12.TextAlignment = StringAlignment.Far;
									stripLine12.TextLineAlignment = StringAlignment.Near;
									this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine12);
									StripLine stripLine13 = new StripLine();
									stripLine13.BorderDashStyle = ChartDashStyle.NotSet;
									stripLine13.Interval = 0.0;
									stripLine13.StripWidth = 0.0001;
									stripLine13.IntervalOffset = num63;
									stripLine13.BackColor = Color.FromArgb(120, Color.Black);
									stripLine13.Text = EcoLanguage.getMsg(LangRes.DashBd_maxRecommend, new string[0]) + ":" + num63.ToString("F1") + str;
									stripLine13.TextAlignment = StringAlignment.Far;
									stripLine13.TextLineAlignment = StringAlignment.Near;
									this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine13);
									commUtil.ShowInfo_DEBUG("END RHI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
								}
							}
							else
							{
								commUtil.ShowInfo_DEBUG("Start RCI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
								this.palleg.Visible = true;
								double num66 = 27.0;
								double num67 = 32.0;
								double num68 = 18.0;
								double num69 = 15.0;
								double num70 = 0.0;
								double num71 = 0.0;
								string str2 = "°C";
								DataTable dataTable7 = new DataTable();
								DataColumn dataColumn4 = new DataColumn();
								dataColumn4.DataType = System.Type.GetType("System.Int32");
								dataColumn4.ColumnName = "no";
								dataTable7.Columns.Add(dataColumn4);
								dataColumn4 = new DataColumn();
								dataColumn4.DataType = System.Type.GetType("System.Double");
								dataColumn4.ColumnName = "temperature";
								dataTable7.Columns.Add(dataColumn4);
								dataColumn4 = new DataColumn();
								dataColumn4.DataType = System.Type.GetType("System.Double");
								dataColumn4.ColumnName = "C2F";
								dataTable7.Columns.Add(dataColumn4);
								commUtil.ShowInfo_DEBUG("Start RCI ---1: " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
								DataRow[] array4 = this.m_TitleData_Info.Tables[1].Select(string.Concat(new object[]
								{
									"sensor_location=",
									0,
									" and temperature <>'",
									-500,
									"' and temperature <>'",
									-1000,
									"'"
								}), "temperature asc");
								commUtil.ShowInfo_DEBUG("Start RCI ---2: " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
								if (array4.Length >= 1)
								{
									this.lbldiagram.Text = "";
								}
								else
								{
									this.lbldiagram.Text = EcoLanguage.getMsg(LangRes.DashBd_noenoughSS, new string[0]);
								}
								double num74;
								double num75;
								if (array4.Length > 0)
								{
									for (int m = 0; m < array4.Length; m++)
									{
										double num72 = ecoConvert.f2d(array4[m]["temperature"]);
										double num73 = num72 * 1.8 + 32.0;
										DataRow dataRow4 = dataTable7.NewRow();
										dataRow4["no"] = m + 1;
										dataRow4["temperature"] = num72;
										dataRow4["C2F"] = num73;
										dataTable7.Rows.Add(dataRow4);
										if (num72 > num66)
										{
											num70 += num72 - num66;
										}
										if (num68 > num72)
										{
											num71 += num68 - num72;
										}
									}
									num74 = (1.0 - num70 / ((num67 - num66) * (double)dataTable7.Rows.Count)) * 100.0;
									num75 = (1.0 - num71 / ((num68 - num69) * (double)dataTable7.Rows.Count)) * 100.0;
								}
								else
								{
									num74 = 0.0;
									num75 = 0.0;
								}
								commUtil.ShowInfo_DEBUG("Start RCI --- 3 : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
								this.lblIndex.Text = string.Concat(new string[]
								{
									"RCI_Hi=",
									num74.ToString("F2"),
									"(%)\r\n\r\nRCI_Lo=",
									num75.ToString("F2"),
									"(%)"
								});
								this.lblIndex.BorderStyle = BorderStyle.FixedSingle;
								this.lblIndex.Location = new Point(323, 5);
								this.lblIndex.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.lblIndex.Height = 73;
								this.lblIndex.Width = 167;
								this.lblIndex.AutoSize = false;
								if (dataTable7.Rows.Count > 0)
								{
									if (num74 < 90.0 || num75 < 90.0)
									{
										this.lblIndex.BackColor = Color.Red;
									}
									else
									{
										if (num74 >= 95.0 && num75 >= 95.0)
										{
											this.lblIndex.BackColor = Color.FromArgb(162, 215, 48);
										}
										else
										{
											this.lblIndex.BackColor = Color.Orange;
										}
									}
									this.meter1.init("RCI Hi", num74);
									this.meter2.init("RCI Lo", num75);
									this.meter1.Visible = true;
									this.meter2.Visible = true;
									this.lblIndex.Visible = true;
								}
								else
								{
									this.lblIndex.Visible = true;
									this.lblIndex.BackColor = Color.Silver;
									this.lblIndex.Text = "N/A";
									this.meter1.Visible = false;
									this.meter2.Visible = false;
								}
								this.overallchart.Visible = true;
								this.overallchart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
								this.overallchart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
								this.overallchart.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								this.overallchart.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
								string text12;
								double num76;
								double num77;
								double num78;
								double num79;
								if (EcoGlobalVar.TempUnit == 0)
								{
									text12 = "temperature";
									num76 = num66;
									num77 = num67;
									num78 = num68;
									num79 = num69;
									this.overallchart.ChartAreas[0].AxisY.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisRackintaketemp, new string[0]) + "(°C)";
								}
								else
								{
									text12 = "C2F";
									str2 = "°F";
									num76 = RackStatusAll.CtoFdegrees(num66);
									num77 = RackStatusAll.CtoFdegrees(num67);
									num78 = RackStatusAll.CtoFdegrees(num68);
									num79 = RackStatusAll.CtoFdegrees(num69);
									this.overallchart.ChartAreas[0].AxisY.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisRackintaketemp, new string[0]) + "(°F)";
								}
								this.overallchart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
								this.overallchart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.DashBd_AxisTempSSnum, new string[0]);
								this.overallchart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
								this.overallchart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
								this.overallchart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
								this.overallchart.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;
								this.overallchart.ChartAreas[0].AxisX.IsMarginVisible = false;
								this.overallchart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
								this.overallchart.ChartAreas[0].AxisY.IsStartedFromZero = false;
								this.overallchart.ChartAreas[0].AxisX.IsStartedFromZero = false;
								if (dataTable7.Rows.Count > 0)
								{
									this.overallchart.ChartAreas[0].AxisX.Maximum = (double)System.Convert.ToInt16(dataTable7.Rows[dataTable7.Rows.Count - 1]["no"]);
									this.overallchart.ChartAreas[0].AxisX.Minimum = 1.0;
									System.Convert.ToDouble(dataTable7.Rows[0][text12]);
									double num80 = System.Convert.ToDouble(dataTable7.Rows[dataTable7.Rows.Count - 1][text12]);
									if (num80 < num77 + 2.0)
									{
										this.overallchart.ChartAreas[0].AxisY.Maximum = num77 + 2.0;
									}
									else
									{
										this.overallchart.ChartAreas[0].AxisY.Maximum = num80 + 2.0;
									}
								}
								this.overallchart.Series.Clear();
								this.overallchart.Legends.Clear();
								Series series15 = new Series();
								DataView dataView6 = new DataView(dataTable7);
								series15.Points.DataBindXY(dataView6, "no", dataView6, text12);
								series15.XValueType = ChartValueType.Int64;
								series15.Name = "RCI";
								series15.ChartArea = "ChartArea1";
								series15.ChartType = SeriesChartType.Spline;
								series15.MarkerStyle = MarkerStyle.Circle;
								series15.MarkerSize = 4;
								series15.IsValueShownAsLabel = true;
								series15.LabelFormat = "F2";
								if (num74 < 90.0 || num75 < 90.0)
								{
									series15.Color = Color.Red;
								}
								else
								{
									if (num74 >= 95.0 && num75 >= 95.0)
									{
										series15.Color = Color.FromArgb(162, 215, 48);
									}
									else
									{
										series15.Color = Color.Orange;
									}
								}
								series15.BorderWidth = 2;
								this.overallchart.Series.Add(series15);
								if (dataTable7.Rows.Count >= 5)
								{
									series15.Enabled = false;
									Series series16 = new Series();
									series16.Name = "New";
									series16.ChartType = SeriesChartType.Spline;
									series16.BorderWidth = 2;
									if (num74 < 90.0 || num75 < 90.0)
									{
										series16.Color = Color.Red;
									}
									else
									{
										if (num74 >= 95.0 && num75 >= 95.0)
										{
											series16.Color = Color.FromArgb(162, 215, 48);
										}
										else
										{
											series16.Color = Color.Orange;
										}
									}
									this.overallchart.Series.Add(series16);
									string text13 = "5";
									int num81 = 10;
									string text14 = "false";
									string text15 = "false";
									string parameters4 = string.Concat(new object[]
									{
										text13,
										',',
										num81,
										',',
										text14,
										',',
										text15
									});
									this.overallchart.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters4, "RCI", "New");
								}
								else
								{
									series15.Enabled = true;
								}
								this.overallchart.ChartAreas[0].AxisY.StripLines.Clear();
								StripLine stripLine14 = new StripLine();
								stripLine14.BorderDashStyle = ChartDashStyle.NotSet;
								stripLine14.Interval = 0.0;
								stripLine14.StripWidth = num78 - num79;
								stripLine14.IntervalOffset = num79;
								stripLine14.BackColor = Color.FromArgb(120, Color.Gold);
								stripLine14.Text = "";
								stripLine14.TextAlignment = StringAlignment.Far;
								stripLine14.TextLineAlignment = StringAlignment.Near;
								this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine14);
								StripLine stripLine15 = new StripLine();
								stripLine15.BorderDashStyle = ChartDashStyle.NotSet;
								stripLine15.Interval = 0.0;
								stripLine15.StripWidth = 0.0001;
								stripLine15.BackColor = Color.FromArgb(120, Color.Black);
								stripLine15.IntervalOffset = num78;
								stripLine15.Text = EcoLanguage.getMsg(LangRes.DashBd_minRecommend, new string[0]) + ":" + num78.ToString("F1") + str2;
								stripLine15.TextAlignment = StringAlignment.Far;
								stripLine15.TextLineAlignment = StringAlignment.Near;
								this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine15);
								StripLine stripLine16 = new StripLine();
								stripLine16.BorderDashStyle = ChartDashStyle.NotSet;
								stripLine16.Interval = 0.0;
								stripLine16.StripWidth = 0.0001;
								stripLine16.BackColor = Color.FromArgb(120, Color.Black);
								stripLine16.IntervalOffset = num79;
								stripLine16.Text = EcoLanguage.getMsg(LangRes.DashBd_minAllow, new string[0]) + ":" + num79.ToString("F1") + str2;
								stripLine16.TextAlignment = StringAlignment.Far;
								stripLine16.TextLineAlignment = StringAlignment.Near;
								this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine16);
								StripLine stripLine17 = new StripLine();
								stripLine17.BorderDashStyle = ChartDashStyle.NotSet;
								stripLine17.Interval = 0.0;
								stripLine17.StripWidth = num77 - num76;
								stripLine17.IntervalOffset = num76;
								stripLine17.BackColor = Color.FromArgb(120, Color.Gold);
								stripLine17.Text = "";
								stripLine17.TextAlignment = StringAlignment.Far;
								stripLine17.TextLineAlignment = StringAlignment.Near;
								this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine17);
								StripLine stripLine18 = new StripLine();
								stripLine18.BorderDashStyle = ChartDashStyle.NotSet;
								stripLine18.Interval = 0.0;
								stripLine18.StripWidth = 0.0001;
								stripLine18.BackColor = Color.FromArgb(120, Color.Black);
								stripLine18.IntervalOffset = num77;
								stripLine18.Text = EcoLanguage.getMsg(LangRes.DashBd_maxAllow, new string[0]) + ":" + num77.ToString("F1") + str2;
								stripLine18.TextAlignment = StringAlignment.Far;
								stripLine18.TextLineAlignment = StringAlignment.Near;
								this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine18);
								StripLine stripLine19 = new StripLine();
								stripLine19.BorderDashStyle = ChartDashStyle.NotSet;
								stripLine19.Interval = 0.0;
								stripLine19.StripWidth = 0.0001;
								stripLine19.BackColor = Color.FromArgb(120, Color.Black);
								stripLine19.IntervalOffset = num76;
								stripLine19.Text = EcoLanguage.getMsg(LangRes.DashBd_maxRecommend, new string[0]) + ":" + num76.ToString("F1") + str2;
								stripLine19.TextAlignment = StringAlignment.Far;
								stripLine19.TextLineAlignment = StringAlignment.Near;
								this.overallchart.ChartAreas[0].AxisY.StripLines.Add(stripLine19);
								commUtil.ShowInfo_DEBUG("END RCI : " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
							}
						}
						this.overallchart.ResetAutoValues();
					}
				}
			}
			IL_5E8D:
			if (this.FreshFlg_DashBoard == 1)
			{
				this.boardTimer.Interval = 1000.0;
				return;
			}
			this.boardTimer.Interval = 10000.0;
		}
		public void FlashEvent(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new DashBoard.TabFlash(this.FlashTab));
				return;
			}
			this.FlashTab();
		}
		private void FlashTab()
		{
			Color backColor = this.FlashColor[0];
			Color backColor2 = this.FlashColor[1];
			Color backColor3 = this.FlashColor[2];
			Color backColor4 = this.FlashColor[3];
			if (this.FlashTimes <= 11)
			{
				if (this.poweralarm)
				{
					if (this.FlashTimes % 2 == 1)
					{
						this.btnpower.BackColor = backColor;
					}
					else
					{
						this.btnpower.BackColor = Color.Silver;
					}
				}
				if (this.thermalalarm)
				{
					if (this.FlashTimes % 2 == 1)
					{
						this.btnthermal.BackColor = backColor2;
					}
					else
					{
						this.btnthermal.BackColor = Color.Silver;
					}
				}
				if (this.airalarm)
				{
					if (this.FlashTimes % 2 == 1)
					{
						this.btnair.BackColor = backColor3;
					}
					else
					{
						this.btnair.BackColor = Color.Silver;
					}
				}
				if (this.humidityalarm)
				{
					if (this.FlashTimes % 2 == 1)
					{
						this.btnhumidity.BackColor = backColor4;
					}
					else
					{
						this.btnhumidity.BackColor = Color.Silver;
					}
				}
				this.FlashTimes++;
				return;
			}
			this.FlashTimer.Enabled = false;
			this.thermalalarm = false;
			this.poweralarm = false;
			this.humidityalarm = false;
			this.airalarm = false;
		}
		private void showPicItems(bool show)
		{
			this.panelImgStatus.Visible = show;
		}
		private void cbopdperiod_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			RackStatusAll.Power_dissipation_period = this.cbopdperiod.SelectedIndex;
			if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["1:0"])
			{
				this.endBoardTimer();
				switch (this.cbopdperiod.SelectedIndex)
				{
				case 0:
					RackStatusAll.MinValue = 0.5;
					RackStatusAll.MaxValue = 4.5;
					this.labrang1.Text = System.Convert.ToString(0.5);
					this.labrang2.Text = System.Convert.ToString(1.5);
					this.labrang3.Text = System.Convert.ToString(2.5);
					this.labrang4.Text = System.Convert.ToString(3.3);
					this.labrang5.Text = System.Convert.ToString(4.5);
					break;
				case 1:
					RackStatusAll.MinValue = 10.0;
					RackStatusAll.MaxValue = 100.0;
					this.labrang1.Text = "10";
					this.labrang2.Text = "30";
					this.labrang3.Text = "55";
					this.labrang4.Text = "75";
					this.labrang5.Text = "100";
					break;
				case 2:
					RackStatusAll.MinValue = 70.0;
					RackStatusAll.MaxValue = 700.0;
					this.labrang1.Text = "70";
					this.labrang2.Text = "230";
					this.labrang3.Text = "390";
					this.labrang4.Text = "550";
					this.labrang5.Text = "700";
					break;
				case 3:
					RackStatusAll.MinValue = 300.0;
					RackStatusAll.MaxValue = 3000.0;
					this.labrang1.Text = "300";
					this.labrang2.Text = "950";
					this.labrang3.Text = "1600";
					this.labrang4.Text = "2250";
					this.labrang5.Text = "3000";
					break;
				case 4:
					RackStatusAll.MinValue = 1000.0;
					RackStatusAll.MaxValue = 10000.0;
					this.labrang1.Text = "1000";
					this.labrang2.Text = "3000";
					this.labrang3.Text = "5500";
					this.labrang4.Text = "7500";
					this.labrang5.Text = "10000";
					break;
				case 5:
					RackStatusAll.MinValue = 4000.0;
					RackStatusAll.MaxValue = 40000.0;
					this.labrang1.Text = "4000";
					this.labrang2.Text = "13000";
					this.labrang3.Text = "22000";
					this.labrang4.Text = "31000";
					this.labrang5.Text = "40000";
					break;
				}
				this.boardTimeDate(0);
				this.label9.Focus();
				this.startBoardTimer();
			}
		}
		private void cbPUE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			RackStatusAll.PUE_period = this.cbPUE.SelectedIndex;
			System.DateTime dateTime = System.DateTime.Now;
			object obj = ClientAPI.RemoteCall(104, 1, "", 10000);
			if (obj != null)
			{
				dateTime = (System.DateTime)obj;
			}
			this.m_LocalTime4getSrvTime = System.DateTime.Now;
			if ((Combobox_item)this.cbodash.SelectedItem == this.m_comboitems["0:0"])
			{
				this.endBoardTimer();
				this.paltime.Visible = true;
				switch (this.cbPUE.SelectedIndex)
				{
				case 0:
					this.paltime.Visible = false;
					break;
				case 1:
					this.lblstart.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.paltime.Update();
					break;
				case 2:
					this.lblstart.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.paltime.Update();
					break;
				case 3:
				{
					System.DateTime dateTime2 = dateTime.AddDays((double)(1 - System.Convert.ToInt32(dateTime.DayOfWeek.ToString("d"))));
					if (dateTime.DayOfWeek == System.DayOfWeek.Sunday)
					{
						dateTime2 = dateTime2.AddDays(-7.0);
					}
					System.DateTime dateTime3 = dateTime2.AddDays(6.0);
					this.lblstart.Text = dateTime2.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime3.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
					this.paltime.Update();
					break;
				}
				}
				this.boardTimeDate(0);
				this.label9.Focus();
				this.startBoardTimer();
			}
		}
		private string getDevidsinRack(System.Collections.ArrayList allRacks, long rackID)
		{
			foreach (RackInfo rackInfo in allRacks)
			{
				if (rackInfo.RackID == rackID)
				{
					return rackInfo.DeviceInfo;
				}
			}
			return "";
		}
		private int showRealtimeData(int pduPowerfreshflg)
		{
			string selectedTag = this.m_SelectedTag;
			if (this.boardName == "board")
			{
				if (selectedTag.Equals("0:0"))
				{
					this.DB_FlgISG = AppData.getDB_FlgISG();
					this.DB_flgAtenPDU = AppData.getDB_flgAtenPDU();
					if (this.DB_FlgISG == 0)
					{
						this.cbPUE.Visible = false;
						this.panelAverage.Visible = false;
						this.paltime.Visible = false;
					}
					else
					{
						if (this.cbPUE.Visible && pduPowerfreshflg == boardDataUtil.PDUData_FreshFLG_NO)
						{
							System.DateTime now = System.DateTime.Now;
							if ((now - this.m_LocalTime4getSrvTime).Seconds < 10)
							{
								return 0;
							}
						}
						object obj = ClientAPI.RemoteCall(104, 1, "", 10000);
						if (obj == null)
						{
							return 0;
						}
						System.DateTime dateTime = (System.DateTime)obj;
						this.m_LocalTime4getSrvTime = System.DateTime.Now;
						switch (RackStatusAll.PUE_period)
						{
						case 1:
							this.lblstart.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						case 2:
							this.lblstart.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						case 3:
						{
							System.DateTime dateTime2 = dateTime.AddDays((double)(1 - System.Convert.ToInt32(dateTime.DayOfWeek.ToString("d"))));
							if (dateTime.DayOfWeek == System.DayOfWeek.Sunday)
							{
								dateTime2 = dateTime2.AddDays(-7.0);
							}
							System.DateTime dateTime3 = dateTime2.AddDays(6.0);
							this.lblstart.Text = dateTime2.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime3.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						}
						}
						this.cbPUE.Visible = true;
						this.panelAverage.Visible = true;
						if (RackStatusAll.PUE_period == 0)
						{
							this.paltime.Visible = false;
						}
						else
						{
							this.paltime.Visible = true;
						}
						double total_Value = RackStatusAll.Total_Value;
						double num = total_Value;
						double num2 = total_Value;
						string str = "N/A";
						switch (RackStatusAll.PUE_period)
						{
						case 0:
							num2 = AppData.getPUE(0);
							num = AppData.getPUE(1);
							if (this.DB_flgAtenPDU == 1)
							{
								num2 += total_Value;
								num += total_Value;
							}
							break;
						case 1:
							num2 = AppData.getPUE(2);
							num = AppData.getPUE(3);
							break;
						case 2:
							num2 = AppData.getPUE(4);
							num = AppData.getPUE(5);
							break;
						case 3:
							num2 = AppData.getPUE(6);
							num = AppData.getPUE(7);
							break;
						}
						double num3 = -1.0;
						try
						{
							num3 = num / num2;
						}
						catch (System.Exception)
						{
						}
						if (num3 >= 0.0 && !double.IsInfinity(num3) && !double.IsNaN(num3))
						{
							str = num3.ToString("F2");
						}
						this.lblavg.Text = EcoLanguage.getMsg(LangRes.PUE_value, new string[0]) + " " + str;
						this.lblvar.Text = "";
					}
				}
				else
				{
					if (RackStatusAll.Board_Tag.Equals("1:0"))
					{
						if (pduPowerfreshflg == boardDataUtil.PDUData_FreshFLG_Yes)
						{
							return 0;
						}
						System.DateTime now2 = System.DateTime.Now;
						if ((now2 - this.m_LocalTime4getSrvTime).Seconds < 10)
						{
							return 0;
						}
						System.DateTime dateTime4 = System.DateTime.Now;
						object obj2 = ClientAPI.RemoteCall(104, 1, "", 10000);
						if (obj2 != null)
						{
							dateTime4 = (System.DateTime)obj2;
						}
						this.m_LocalTime4getSrvTime = System.DateTime.Now;
						switch (RackStatusAll.Power_dissipation_period)
						{
						case 0:
							this.lblstart.Text = dateTime4.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime4.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						case 1:
							this.lblstart.Text = dateTime4.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime4.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						case 2:
						{
							System.DateTime dateTime5 = dateTime4.AddDays((double)(1 - System.Convert.ToInt32(dateTime4.DayOfWeek.ToString("d"))));
							if (dateTime4.DayOfWeek == System.DayOfWeek.Sunday)
							{
								dateTime5 = dateTime5.AddDays(-7.0);
							}
							System.DateTime dateTime6 = dateTime5.AddDays(6.0);
							this.lblstart.Text = dateTime5.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime6.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime4.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						}
						case 3:
						{
							System.DateTime dateTime5 = dateTime4.AddDays((double)(1 - dateTime4.Day));
							System.DateTime dateTime6 = dateTime5.AddMonths(1).AddDays(-1.0);
							this.lblstart.Text = dateTime5.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime6.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime4.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						}
						case 4:
						{
							System.DateTime dateTime5 = dateTime4.AddMonths(-((System.DateTime.Now.Month - 1) % 3)).AddDays((double)(1 - dateTime4.Day));
							System.DateTime dateTime6 = dateTime5.AddMonths(3).AddDays(-1.0);
							this.lblstart.Text = dateTime5.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime6.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime4.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						}
						case 5:
						{
							System.DateTime dateTime5 = dateTime4.AddDays((double)(1 - System.Convert.ToInt32(dateTime4.DayOfYear.ToString("d"))));
							System.DateTime dateTime6 = dateTime5.AddYears(1).AddDays(-1.0);
							this.lblstart.Text = dateTime5.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime6.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							this.lblcurrenttime.Text = dateTime4.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
							break;
						}
						}
					}
				}
			}
			return 0;
		}
	}
}
