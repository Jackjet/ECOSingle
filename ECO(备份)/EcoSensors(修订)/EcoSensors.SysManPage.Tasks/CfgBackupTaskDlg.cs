using CommonAPI.Global;
using CommonAPI.network;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Tasks
{
	public class CfgBackupTaskDlg : Form
	{
		private IContainer components;
		private Button butSave;
		private GroupBox gbTaskInfo;
		private TextBox tbTaskName;
		private ComboBox cbTaskType;
		private Label label2;
		private Label lbTaskname;
		private GroupBox groupBox1;
		private Panel panelDayWeek;
		private TableLayoutPanel tableLayoutPanelDaily;
		private DateTimePicker dtPickerOnD00;
		private Label label21;
		private TableLayoutPanel tableLayoutPanelWeekly;
		private CheckBox checkBoxOnW05;
		private CheckBox checkBoxOnW04;
		private DateTimePicker dtPickerOnW04;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label label10;
		private Label label11;
		private CheckBox checkBoxOnAll;
		private CheckBox checkBoxOnW01;
		private CheckBox checkBoxOnW02;
		private CheckBox checkBoxOnW03;
		private CheckBox checkBoxOnW06;
		private CheckBox checkBoxOnW07;
		private DateTimePicker dtPickerOnW01;
		private DateTimePicker dtPickerOnW02;
		private DateTimePicker dtPickerOnW03;
		private DateTimePicker dtPickerOnW05;
		private DateTimePicker dtPickerOnW06;
		private DateTimePicker dtPickerOnW07;
		private Button butCancel;
		private TextBox tbSMBDir;
		private Label lbSMBDir;
		private TextBox tbSMBPsw;
		private Label lbSMBPsw;
		private TextBox tbSMBUsername;
		private Label lbSMBUsername;
		private Button btnSMBBrowse;
		private Panel panelFTP;
		private CheckBox cbFTPAuth;
		private TextBox tbFTPPort;
		private Label lbFTPport;
		private TextBox tbFTPDir;
		private Label lbFTPDir;
		private TextBox tbFTPUsername;
		private Label lbFTPUsername;
		private TextBox tbFTPServer;
		private Label lbFTPPsw;
		private Label lbFTPServer;
		private TextBox tbFTPPsw;
		private Panel panelSMB;
		private RadioButton rbStore_FTP;
		private RadioButton rbStore_SMB;
		private Combobox_item m_TPDaily = new Combobox_item(0.ToString(), EcoLanguage.getMsg(LangRes.Task_TPDaily, new string[0]));
		private Combobox_item m_TPWeekly = new Combobox_item(1.ToString(), EcoLanguage.getMsg(LangRes.Task_TPWeekly, new string[0]));
		private bool modifAll_only;
		private long m_taskID = -1L;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CfgBackupTaskDlg));
			this.butSave = new Button();
			this.gbTaskInfo = new GroupBox();
			this.rbStore_FTP = new RadioButton();
			this.rbStore_SMB = new RadioButton();
			this.tbTaskName = new TextBox();
			this.cbTaskType = new ComboBox();
			this.label2 = new Label();
			this.lbTaskname = new Label();
			this.panelFTP = new Panel();
			this.cbFTPAuth = new CheckBox();
			this.tbFTPPort = new TextBox();
			this.lbFTPport = new Label();
			this.tbFTPDir = new TextBox();
			this.lbFTPDir = new Label();
			this.tbFTPUsername = new TextBox();
			this.lbFTPUsername = new Label();
			this.tbFTPServer = new TextBox();
			this.lbFTPPsw = new Label();
			this.lbFTPServer = new Label();
			this.tbFTPPsw = new TextBox();
			this.panelSMB = new Panel();
			this.tbSMBUsername = new TextBox();
			this.btnSMBBrowse = new Button();
			this.lbSMBUsername = new Label();
			this.tbSMBDir = new TextBox();
			this.lbSMBPsw = new Label();
			this.lbSMBDir = new Label();
			this.tbSMBPsw = new TextBox();
			this.groupBox1 = new GroupBox();
			this.panelDayWeek = new Panel();
			this.tableLayoutPanelWeekly = new TableLayoutPanel();
			this.checkBoxOnW05 = new CheckBox();
			this.checkBoxOnW04 = new CheckBox();
			this.dtPickerOnW04 = new DateTimePicker();
			this.label4 = new Label();
			this.label5 = new Label();
			this.label6 = new Label();
			this.label7 = new Label();
			this.label8 = new Label();
			this.label9 = new Label();
			this.label10 = new Label();
			this.label11 = new Label();
			this.checkBoxOnAll = new CheckBox();
			this.checkBoxOnW01 = new CheckBox();
			this.checkBoxOnW02 = new CheckBox();
			this.checkBoxOnW03 = new CheckBox();
			this.checkBoxOnW06 = new CheckBox();
			this.checkBoxOnW07 = new CheckBox();
			this.dtPickerOnW01 = new DateTimePicker();
			this.dtPickerOnW02 = new DateTimePicker();
			this.dtPickerOnW03 = new DateTimePicker();
			this.dtPickerOnW05 = new DateTimePicker();
			this.dtPickerOnW06 = new DateTimePicker();
			this.dtPickerOnW07 = new DateTimePicker();
			this.tableLayoutPanelDaily = new TableLayoutPanel();
			this.dtPickerOnD00 = new DateTimePicker();
			this.label21 = new Label();
			this.butCancel = new Button();
			this.gbTaskInfo.SuspendLayout();
			this.panelFTP.SuspendLayout();
			this.panelSMB.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panelDayWeek.SuspendLayout();
			this.tableLayoutPanelWeekly.SuspendLayout();
			this.tableLayoutPanelDaily.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.butSave, "butSave");
			this.butSave.Name = "butSave";
			this.butSave.UseVisualStyleBackColor = true;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			this.gbTaskInfo.Controls.Add(this.rbStore_FTP);
			this.gbTaskInfo.Controls.Add(this.rbStore_SMB);
			this.gbTaskInfo.Controls.Add(this.tbTaskName);
			this.gbTaskInfo.Controls.Add(this.cbTaskType);
			this.gbTaskInfo.Controls.Add(this.label2);
			this.gbTaskInfo.Controls.Add(this.lbTaskname);
			this.gbTaskInfo.Controls.Add(this.panelSMB);
			this.gbTaskInfo.Controls.Add(this.panelFTP);
			componentResourceManager.ApplyResources(this.gbTaskInfo, "gbTaskInfo");
			this.gbTaskInfo.ForeColor = Color.FromArgb(20, 73, 160);
			this.gbTaskInfo.Name = "gbTaskInfo";
			this.gbTaskInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.rbStore_FTP, "rbStore_FTP");
			this.rbStore_FTP.ForeColor = Color.Black;
			this.rbStore_FTP.Name = "rbStore_FTP";
			this.rbStore_FTP.TabStop = true;
			this.rbStore_FTP.UseVisualStyleBackColor = true;
			this.rbStore_FTP.CheckedChanged += new System.EventHandler(this.rbStore_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbStore_SMB, "rbStore_SMB");
			this.rbStore_SMB.ForeColor = Color.Black;
			this.rbStore_SMB.Name = "rbStore_SMB";
			this.rbStore_SMB.TabStop = true;
			this.rbStore_SMB.UseVisualStyleBackColor = true;
			this.rbStore_SMB.CheckedChanged += new System.EventHandler(this.rbStore_CheckedChanged);
			componentResourceManager.ApplyResources(this.tbTaskName, "tbTaskName");
			this.tbTaskName.Name = "tbTaskName";
			this.tbTaskName.KeyPress += new KeyPressEventHandler(this.tbTaskName_KeyPress);
			this.cbTaskType.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbTaskType, "cbTaskType");
			this.cbTaskType.FormattingEnabled = true;
			this.cbTaskType.Name = "cbTaskType";
			this.cbTaskType.SelectedIndexChanged += new System.EventHandler(this.cbTaskType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = SystemColors.ControlText;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.lbTaskname, "lbTaskname");
			this.lbTaskname.ForeColor = SystemColors.ControlText;
			this.lbTaskname.Name = "lbTaskname";
			this.panelFTP.Controls.Add(this.cbFTPAuth);
			this.panelFTP.Controls.Add(this.tbFTPPort);
			this.panelFTP.Controls.Add(this.lbFTPport);
			this.panelFTP.Controls.Add(this.tbFTPDir);
			this.panelFTP.Controls.Add(this.lbFTPDir);
			this.panelFTP.Controls.Add(this.tbFTPUsername);
			this.panelFTP.Controls.Add(this.lbFTPUsername);
			this.panelFTP.Controls.Add(this.tbFTPServer);
			this.panelFTP.Controls.Add(this.lbFTPPsw);
			this.panelFTP.Controls.Add(this.lbFTPServer);
			this.panelFTP.Controls.Add(this.tbFTPPsw);
			componentResourceManager.ApplyResources(this.panelFTP, "panelFTP");
			this.panelFTP.Name = "panelFTP";
			componentResourceManager.ApplyResources(this.cbFTPAuth, "cbFTPAuth");
			this.cbFTPAuth.ForeColor = Color.Black;
			this.cbFTPAuth.Name = "cbFTPAuth";
			this.cbFTPAuth.UseVisualStyleBackColor = true;
			this.cbFTPAuth.CheckedChanged += new System.EventHandler(this.cbFTPAuth_CheckedChanged);
			componentResourceManager.ApplyResources(this.tbFTPPort, "tbFTPPort");
			this.tbFTPPort.Name = "tbFTPPort";
			this.tbFTPPort.KeyPress += new KeyPressEventHandler(this.tbFTPPort_KeyPress);
			componentResourceManager.ApplyResources(this.lbFTPport, "lbFTPport");
			this.lbFTPport.ForeColor = SystemColors.ControlText;
			this.lbFTPport.Name = "lbFTPport";
			componentResourceManager.ApplyResources(this.tbFTPDir, "tbFTPDir");
			this.tbFTPDir.Name = "tbFTPDir";
			componentResourceManager.ApplyResources(this.lbFTPDir, "lbFTPDir");
			this.lbFTPDir.ForeColor = SystemColors.ControlText;
			this.lbFTPDir.Name = "lbFTPDir";
			componentResourceManager.ApplyResources(this.tbFTPUsername, "tbFTPUsername");
			this.tbFTPUsername.Name = "tbFTPUsername";
			componentResourceManager.ApplyResources(this.lbFTPUsername, "lbFTPUsername");
			this.lbFTPUsername.ForeColor = SystemColors.ControlText;
			this.lbFTPUsername.Name = "lbFTPUsername";
			componentResourceManager.ApplyResources(this.tbFTPServer, "tbFTPServer");
			this.tbFTPServer.Name = "tbFTPServer";
			componentResourceManager.ApplyResources(this.lbFTPPsw, "lbFTPPsw");
			this.lbFTPPsw.ForeColor = SystemColors.ControlText;
			this.lbFTPPsw.Name = "lbFTPPsw";
			componentResourceManager.ApplyResources(this.lbFTPServer, "lbFTPServer");
			this.lbFTPServer.ForeColor = SystemColors.ControlText;
			this.lbFTPServer.Name = "lbFTPServer";
			componentResourceManager.ApplyResources(this.tbFTPPsw, "tbFTPPsw");
			this.tbFTPPsw.Name = "tbFTPPsw";
			this.panelSMB.Controls.Add(this.tbSMBUsername);
			this.panelSMB.Controls.Add(this.btnSMBBrowse);
			this.panelSMB.Controls.Add(this.lbSMBUsername);
			this.panelSMB.Controls.Add(this.tbSMBDir);
			this.panelSMB.Controls.Add(this.lbSMBPsw);
			this.panelSMB.Controls.Add(this.lbSMBDir);
			this.panelSMB.Controls.Add(this.tbSMBPsw);
			componentResourceManager.ApplyResources(this.panelSMB, "panelSMB");
			this.panelSMB.Name = "panelSMB";
			componentResourceManager.ApplyResources(this.tbSMBUsername, "tbSMBUsername");
			this.tbSMBUsername.Name = "tbSMBUsername";
			this.btnSMBBrowse.BackColor = Color.Gainsboro;
			this.btnSMBBrowse.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnSMBBrowse, "btnSMBBrowse");
			this.btnSMBBrowse.ForeColor = Color.Black;
			this.btnSMBBrowse.Name = "btnSMBBrowse";
			this.btnSMBBrowse.UseVisualStyleBackColor = false;
			this.btnSMBBrowse.Click += new System.EventHandler(this.btnSMBBrowse_Click);
			componentResourceManager.ApplyResources(this.lbSMBUsername, "lbSMBUsername");
			this.lbSMBUsername.ForeColor = SystemColors.ControlText;
			this.lbSMBUsername.Name = "lbSMBUsername";
			componentResourceManager.ApplyResources(this.tbSMBDir, "tbSMBDir");
			this.tbSMBDir.Name = "tbSMBDir";
			this.tbSMBDir.TextChanged += new System.EventHandler(this.tbSMBDir_TextChanged);
			componentResourceManager.ApplyResources(this.lbSMBPsw, "lbSMBPsw");
			this.lbSMBPsw.ForeColor = SystemColors.ControlText;
			this.lbSMBPsw.Name = "lbSMBPsw";
			componentResourceManager.ApplyResources(this.lbSMBDir, "lbSMBDir");
			this.lbSMBDir.ForeColor = SystemColors.ControlText;
			this.lbSMBDir.Name = "lbSMBDir";
			componentResourceManager.ApplyResources(this.tbSMBPsw, "tbSMBPsw");
			this.tbSMBPsw.Name = "tbSMBPsw";
			this.groupBox1.Controls.Add(this.panelDayWeek);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.panelDayWeek.Controls.Add(this.tableLayoutPanelWeekly);
			this.panelDayWeek.Controls.Add(this.tableLayoutPanelDaily);
			componentResourceManager.ApplyResources(this.panelDayWeek, "panelDayWeek");
			this.panelDayWeek.ForeColor = SystemColors.ControlText;
			this.panelDayWeek.Name = "panelDayWeek";
			this.tableLayoutPanelWeekly.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.tableLayoutPanelWeekly, "tableLayoutPanelWeekly");
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnW05, 1, 5);
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnW04, 1, 4);
			this.tableLayoutPanelWeekly.Controls.Add(this.dtPickerOnW04, 2, 4);
			this.tableLayoutPanelWeekly.Controls.Add(this.label4, 0, 1);
			this.tableLayoutPanelWeekly.Controls.Add(this.label5, 0, 2);
			this.tableLayoutPanelWeekly.Controls.Add(this.label6, 0, 3);
			this.tableLayoutPanelWeekly.Controls.Add(this.label7, 0, 4);
			this.tableLayoutPanelWeekly.Controls.Add(this.label8, 0, 5);
			this.tableLayoutPanelWeekly.Controls.Add(this.label9, 0, 6);
			this.tableLayoutPanelWeekly.Controls.Add(this.label10, 0, 7);
			this.tableLayoutPanelWeekly.Controls.Add(this.label11, 2, 0);
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnAll, 1, 0);
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnW01, 1, 1);
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnW02, 1, 2);
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnW03, 1, 3);
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnW06, 1, 6);
			this.tableLayoutPanelWeekly.Controls.Add(this.checkBoxOnW07, 1, 7);
			this.tableLayoutPanelWeekly.Controls.Add(this.dtPickerOnW01, 2, 1);
			this.tableLayoutPanelWeekly.Controls.Add(this.dtPickerOnW02, 2, 2);
			this.tableLayoutPanelWeekly.Controls.Add(this.dtPickerOnW03, 2, 3);
			this.tableLayoutPanelWeekly.Controls.Add(this.dtPickerOnW05, 2, 5);
			this.tableLayoutPanelWeekly.Controls.Add(this.dtPickerOnW06, 2, 6);
			this.tableLayoutPanelWeekly.Controls.Add(this.dtPickerOnW07, 2, 7);
			this.tableLayoutPanelWeekly.Name = "tableLayoutPanelWeekly";
			componentResourceManager.ApplyResources(this.checkBoxOnW05, "checkBoxOnW05");
			this.checkBoxOnW05.Name = "checkBoxOnW05";
			this.checkBoxOnW05.UseVisualStyleBackColor = true;
			this.checkBoxOnW05.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW05.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW04, "checkBoxOnW04");
			this.checkBoxOnW04.Name = "checkBoxOnW04";
			this.checkBoxOnW04.UseVisualStyleBackColor = true;
			this.checkBoxOnW04.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW04.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.dtPickerOnW04, "dtPickerOnW04");
			this.dtPickerOnW04.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW04.Name = "dtPickerOnW04";
			this.dtPickerOnW04.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = SystemColors.ControlText;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = SystemColors.ControlText;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = SystemColors.ControlText;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = SystemColors.ControlText;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.ForeColor = SystemColors.ControlText;
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.ForeColor = SystemColors.ControlText;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.ForeColor = SystemColors.ControlText;
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.checkBoxOnAll, "checkBoxOnAll");
			this.checkBoxOnAll.Name = "checkBoxOnAll";
			this.checkBoxOnAll.UseVisualStyleBackColor = true;
			this.checkBoxOnAll.CheckedChanged += new System.EventHandler(this.checkBoxOnAll_CheckedChanged);
			this.checkBoxOnAll.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW01, "checkBoxOnW01");
			this.checkBoxOnW01.Name = "checkBoxOnW01";
			this.checkBoxOnW01.UseVisualStyleBackColor = true;
			this.checkBoxOnW01.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW01.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW02, "checkBoxOnW02");
			this.checkBoxOnW02.Name = "checkBoxOnW02";
			this.checkBoxOnW02.UseVisualStyleBackColor = true;
			this.checkBoxOnW02.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW02.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW03, "checkBoxOnW03");
			this.checkBoxOnW03.Name = "checkBoxOnW03";
			this.checkBoxOnW03.UseVisualStyleBackColor = true;
			this.checkBoxOnW03.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW03.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW06, "checkBoxOnW06");
			this.checkBoxOnW06.Name = "checkBoxOnW06";
			this.checkBoxOnW06.UseVisualStyleBackColor = true;
			this.checkBoxOnW06.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW06.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW07, "checkBoxOnW07");
			this.checkBoxOnW07.Name = "checkBoxOnW07";
			this.checkBoxOnW07.UseVisualStyleBackColor = true;
			this.checkBoxOnW07.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW07.Paint += new PaintEventHandler(this.checkBoxOn_Paint);
			componentResourceManager.ApplyResources(this.dtPickerOnW01, "dtPickerOnW01");
			this.dtPickerOnW01.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW01.Name = "dtPickerOnW01";
			this.dtPickerOnW01.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW02, "dtPickerOnW02");
			this.dtPickerOnW02.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW02.Name = "dtPickerOnW02";
			this.dtPickerOnW02.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW03, "dtPickerOnW03");
			this.dtPickerOnW03.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW03.Name = "dtPickerOnW03";
			this.dtPickerOnW03.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW05, "dtPickerOnW05");
			this.dtPickerOnW05.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW05.Name = "dtPickerOnW05";
			this.dtPickerOnW05.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW06, "dtPickerOnW06");
			this.dtPickerOnW06.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW06.Name = "dtPickerOnW06";
			this.dtPickerOnW06.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW07, "dtPickerOnW07");
			this.dtPickerOnW07.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW07.Name = "dtPickerOnW07";
			this.dtPickerOnW07.ShowUpDown = true;
			this.tableLayoutPanelDaily.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.tableLayoutPanelDaily, "tableLayoutPanelDaily");
			this.tableLayoutPanelDaily.Controls.Add(this.dtPickerOnD00, 0, 1);
			this.tableLayoutPanelDaily.Controls.Add(this.label21, 0, 0);
			this.tableLayoutPanelDaily.Name = "tableLayoutPanelDaily";
			componentResourceManager.ApplyResources(this.dtPickerOnD00, "dtPickerOnD00");
			this.dtPickerOnD00.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnD00.Name = "dtPickerOnD00";
			this.dtPickerOnD00.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.label21, "label21");
			this.label21.Name = "label21";
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = true;
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.butSave);
			base.Controls.Add(this.gbTaskInfo);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.butCancel);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CfgBackupTaskDlg";
			base.ShowInTaskbar = false;
			this.gbTaskInfo.ResumeLayout(false);
			this.gbTaskInfo.PerformLayout();
			this.panelFTP.ResumeLayout(false);
			this.panelFTP.PerformLayout();
			this.panelSMB.ResumeLayout(false);
			this.panelSMB.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.panelDayWeek.ResumeLayout(false);
			this.tableLayoutPanelWeekly.ResumeLayout(false);
			this.tableLayoutPanelDaily.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public CfgBackupTaskDlg()
		{
			this.InitializeComponent();
			this.tbTaskName.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbFTPPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public CfgBackupTaskDlg(long taskID)
		{
			this.InitializeComponent();
			this.m_taskID = taskID;
			this.cbTaskType.Items.Clear();
			this.cbTaskType.Items.Add(this.m_TPDaily);
			this.cbTaskType.Items.Add(this.m_TPWeekly);
			this.cbTaskType.SelectedItem = this.m_TPDaily;
			this.tbFTPPort.Text = "21";
			if (taskID <= 0L)
			{
				this.tbSMBUsername.Enabled = false;
				this.tbSMBPsw.Enabled = false;
				this.rbStore_SMB.Checked = true;
				return;
			}
			Backuptask taskByID = Backuptask.GetTaskByID(taskID);
			this.tbTaskName.Text = taskByID.TaskName;
			if (taskByID.STOREType == 0 || taskByID.STOREType == 2)
			{
				this.rbStore_SMB.Checked = true;
				this.tbSMBUsername.Text = taskByID.UserName;
				this.tbSMBPsw.Text = taskByID.Password;
				this.tbSMBDir.Text = taskByID.Filepath;
			}
			else
			{
				this.rbStore_FTP.Checked = true;
				this.tbFTPServer.Text = taskByID.Host;
				this.tbFTPPort.Text = System.Convert.ToString(taskByID.Port);
				this.tbFTPDir.Text = taskByID.Filepath;
				if (taskByID.UserName.Length > 0)
				{
					this.cbFTPAuth.Checked = false;
					this.tbFTPUsername.Text = taskByID.UserName;
					this.tbFTPPsw.Text = taskByID.Password;
				}
				else
				{
					this.cbFTPAuth.Checked = true;
				}
			}
			if (taskByID.TaskType == 0)
			{
				this.cbTaskType.SelectedItem = this.m_TPDaily;
				this.dtPickerOnD00.Text = taskByID.TaskSchedule[0];
				return;
			}
			this.cbTaskType.SelectedItem = this.m_TPWeekly;
			if (taskByID.TaskSchedule[0].Length == 0)
			{
				this.checkBoxOnW01.Checked = false;
			}
			else
			{
				this.checkBoxOnW01.Checked = true;
				this.dtPickerOnW01.Text = taskByID.TaskSchedule[0];
			}
			if (taskByID.TaskSchedule[1].Length == 0)
			{
				this.checkBoxOnW02.Checked = false;
			}
			else
			{
				this.checkBoxOnW02.Checked = true;
				this.dtPickerOnW02.Text = taskByID.TaskSchedule[1];
			}
			if (taskByID.TaskSchedule[2].Length == 0)
			{
				this.checkBoxOnW03.Checked = false;
			}
			else
			{
				this.checkBoxOnW03.Checked = true;
				this.dtPickerOnW03.Text = taskByID.TaskSchedule[2];
			}
			if (taskByID.TaskSchedule[3].Length == 0)
			{
				this.checkBoxOnW04.Checked = false;
			}
			else
			{
				this.checkBoxOnW04.Checked = true;
				this.dtPickerOnW04.Text = taskByID.TaskSchedule[3];
			}
			if (taskByID.TaskSchedule[4].Length == 0)
			{
				this.checkBoxOnW05.Checked = false;
			}
			else
			{
				this.checkBoxOnW05.Checked = true;
				this.dtPickerOnW05.Text = taskByID.TaskSchedule[4];
			}
			if (taskByID.TaskSchedule[5].Length == 0)
			{
				this.checkBoxOnW06.Checked = false;
			}
			else
			{
				this.checkBoxOnW06.Checked = true;
				this.dtPickerOnW06.Text = taskByID.TaskSchedule[5];
			}
			if (taskByID.TaskSchedule[6].Length == 0)
			{
				this.checkBoxOnW07.Checked = false;
				return;
			}
			this.checkBoxOnW07.Checked = true;
			this.dtPickerOnW07.Text = taskByID.TaskSchedule[6];
		}
		private void cbTaskType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ((Combobox_item)this.cbTaskType.SelectedItem == this.m_TPDaily)
			{
				this.tableLayoutPanelWeekly.Visible = false;
				this.tableLayoutPanelDaily.Visible = true;
				this.panelDayWeek.Visible = true;
				return;
			}
			this.tableLayoutPanelDaily.Visible = false;
			this.tableLayoutPanelWeekly.Visible = true;
			this.panelDayWeek.Visible = true;
		}
		private void tbTaskName_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == ' ')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void checkBoxOnAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.modifAll_only)
			{
				this.checkBoxOnW01.Checked = this.checkBoxOnAll.Checked;
				this.checkBoxOnW02.Checked = this.checkBoxOnAll.Checked;
				this.checkBoxOnW03.Checked = this.checkBoxOnAll.Checked;
				this.checkBoxOnW04.Checked = this.checkBoxOnAll.Checked;
				this.checkBoxOnW05.Checked = this.checkBoxOnAll.Checked;
				this.checkBoxOnW06.Checked = this.checkBoxOnAll.Checked;
				this.checkBoxOnW07.Checked = this.checkBoxOnAll.Checked;
			}
			this.modifAll_only = false;
		}
		private void checkBoxOnWxx_CheckedChanged(object sender, System.EventArgs e)
		{
			bool @checked = this.checkBoxOnAll.Checked;
			if (sender.Equals(this.checkBoxOnW01))
			{
				this.dtPickerOnW01.Enabled = this.checkBoxOnW01.Checked;
				@checked = this.checkBoxOnW01.Checked;
			}
			else
			{
				if (sender.Equals(this.checkBoxOnW02))
				{
					this.dtPickerOnW02.Enabled = this.checkBoxOnW02.Checked;
					@checked = this.checkBoxOnW02.Checked;
				}
				else
				{
					if (sender.Equals(this.checkBoxOnW03))
					{
						this.dtPickerOnW03.Enabled = this.checkBoxOnW03.Checked;
						@checked = this.checkBoxOnW03.Checked;
					}
					else
					{
						if (sender.Equals(this.checkBoxOnW04))
						{
							this.dtPickerOnW04.Enabled = this.checkBoxOnW04.Checked;
							@checked = this.checkBoxOnW04.Checked;
						}
						else
						{
							if (sender.Equals(this.checkBoxOnW05))
							{
								this.dtPickerOnW05.Enabled = this.checkBoxOnW05.Checked;
								@checked = this.checkBoxOnW05.Checked;
							}
							else
							{
								if (sender.Equals(this.checkBoxOnW06))
								{
									this.dtPickerOnW06.Enabled = this.checkBoxOnW06.Checked;
									@checked = this.checkBoxOnW06.Checked;
								}
								else
								{
									if (sender.Equals(this.checkBoxOnW07))
									{
										this.dtPickerOnW07.Enabled = this.checkBoxOnW07.Checked;
										@checked = this.checkBoxOnW07.Checked;
									}
								}
							}
						}
					}
				}
			}
			if (this.checkBoxOnAll.Checked)
			{
				if (!@checked)
				{
					this.modifAll_only = true;
					this.checkBoxOnAll.Checked = false;
					return;
				}
			}
			else
			{
				if (this.checkBoxOnW01.Checked && this.checkBoxOnW02.Checked && this.checkBoxOnW03.Checked && this.checkBoxOnW04.Checked && this.checkBoxOnW05.Checked && this.checkBoxOnW06.Checked && this.checkBoxOnW07.Checked)
				{
					this.modifAll_only = true;
					this.checkBoxOnAll.Checked = true;
				}
			}
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			if (!this.checkValue())
			{
				return;
			}
			string text = this.tbTaskName.Text;
			int num;
			string text2;
			string text3;
			string text4;
			string text5;
			int num2;
			if (this.rbStore_SMB.Checked)
			{
				num = 2;
				text2 = this.tbSMBUsername.Text;
				text3 = this.tbSMBPsw.Text;
				text4 = this.tbSMBDir.Text;
				text5 = "";
				num2 = 0;
			}
			else
			{
				num = 1;
				text5 = this.tbFTPServer.Text;
				num2 = System.Convert.ToInt32(this.tbFTPPort.Text);
				text4 = this.tbFTPDir.Text;
				if (this.cbFTPAuth.Checked)
				{
					text2 = "";
					text3 = this.tbFTPPsw.Text;
				}
				else
				{
					text2 = this.tbFTPUsername.Text;
					text3 = this.tbFTPPsw.Text;
				}
			}
			int num3;
			string[] array;
			if (this.cbTaskType.SelectedItem.Equals(this.m_TPDaily))
			{
				num3 = 0;
				array = new string[1];
				string text6 = this.dtPickerOnD00.Text;
				array[0] = text6 + ":00";
			}
			else
			{
				num3 = 1;
				array = new string[7];
				if (this.checkBoxOnW01.Checked)
				{
					array[0] = this.dtPickerOnW01.Text + ":00";
				}
				else
				{
					array[0] = "";
				}
				if (this.checkBoxOnW02.Checked)
				{
					array[1] = this.dtPickerOnW02.Text + ":00";
				}
				else
				{
					array[1] = "";
				}
				if (this.checkBoxOnW03.Checked)
				{
					array[2] = this.dtPickerOnW03.Text + ":00";
				}
				else
				{
					array[2] = "";
				}
				if (this.checkBoxOnW04.Checked)
				{
					array[3] = this.dtPickerOnW04.Text + ":00";
				}
				else
				{
					array[3] = "";
				}
				if (this.checkBoxOnW05.Checked)
				{
					array[4] = this.dtPickerOnW05.Text + ":00";
				}
				else
				{
					array[4] = "";
				}
				if (this.checkBoxOnW06.Checked)
				{
					array[5] = this.dtPickerOnW06.Text + ":00";
				}
				else
				{
					array[5] = "";
				}
				if (this.checkBoxOnW07.Checked)
				{
					array[6] = this.dtPickerOnW07.Text + ":00";
				}
				else
				{
					array[6] = "";
				}
			}
			int num4;
			if (this.m_taskID < 0L)
			{
				num4 = Backuptask.CreateTask(text, num3, num, text2, text3, text5, num2, text4, array);
				if (num4 > 0)
				{
					string valuePair = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair))
					{
						LogAPI.writeEventLog("0530000", new string[]
						{
							text,
							valuePair
						});
					}
					else
					{
						LogAPI.writeEventLog("0530000", new string[]
						{
							text
						});
					}
				}
			}
			else
			{
				Backuptask taskByID = Backuptask.GetTaskByID(this.m_taskID);
				taskByID.TaskName = text;
				taskByID.TaskType = num3;
				taskByID.STOREType = num;
				taskByID.Host = text5;
				taskByID.UserName = text2;
				taskByID.Password = text3;
				taskByID.Port = num2;
				taskByID.Filepath = text4;
				taskByID.TaskSchedule = array;
				num4 = taskByID.UpdateTask();
				if (num4 > 0)
				{
					string valuePair2 = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair2))
					{
						LogAPI.writeEventLog("0530002", new string[]
						{
							text,
							valuePair2
						});
					}
					else
					{
						LogAPI.writeEventLog("0530002", new string[]
						{
							text
						});
					}
				}
			}
			if (num4 < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			base.DialogResult = DialogResult.OK;
			base.Close();
		}
		private bool checkValue()
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbTaskName, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbTaskname.Text
				}));
				return false;
			}
			string text = this.tbTaskName.Text;
			if (!Backuptask.CheckName(this.m_taskID, text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_nmdup, new string[]
				{
					text
				}));
				this.tbTaskName.Focus();
				return false;
			}
			if (this.rbStore_SMB.Checked)
			{
				Ecovalidate.checkTextIsNull(this.tbSMBDir, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbSMBDir.Text
					}));
					return false;
				}
				string text2 = this.tbSMBDir.Text;
				if (text2.Length > 255)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.maxlength, new string[]
					{
						this.lbSMBDir.Text,
						255.ToString()
					}));
					return false;
				}
			}
			else
			{
				Ecovalidate.checkTextIsNull(this.tbFTPServer, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbFTPServer.Text
					}));
					return false;
				}
				Ecovalidate.checkTextIsNull(this.tbFTPPort, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbFTPport.Text
					}));
					return false;
				}
				if (!Ecovalidate.Rangeint(this.tbFTPPort, 1, 65535))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.lbFTPport.Text,
						"1",
						"65535"
					}));
					return false;
				}
				string text3 = this.tbFTPDir.Text;
				if (text3.Length > 255)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.maxlength, new string[]
					{
						this.lbFTPDir.Text,
						255.ToString()
					}));
					return false;
				}
				if (!this.cbFTPAuth.Checked)
				{
					Ecovalidate.checkTextIsNull(this.tbFTPUsername, ref flag);
					if (flag)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
						{
							this.lbFTPUsername.Text
						}));
						return false;
					}
				}
			}
			if (this.cbTaskType.SelectedItem.Equals(this.m_TPDaily))
			{
				string arg_2A6_0 = this.dtPickerOnD00.Text;
			}
			else
			{
				if (!this.checkBoxOnW01.Checked && !this.checkBoxOnW02.Checked && !this.checkBoxOnW03.Checked && !this.checkBoxOnW04.Checked && !this.checkBoxOnW05.Checked && !this.checkBoxOnW06.Checked && !this.checkBoxOnW07.Checked)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timeStartneed, new string[0]));
					return false;
				}
			}
			return true;
		}
		private void btnSMBBrowse_Click(object sender, System.EventArgs e)
		{
			string text = this.tbSMBUsername.Text;
			string text2 = this.tbSMBPsw.Text;
			if (text.Length > 0)
			{
				string arg_31_0 = this.tbSMBDir.Text;
				bool flag = false;
				Ecovalidate.checkTextIsNull(this.tbSMBDir, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbSMBDir.Text
					}));
					return;
				}
				string text3 = this.tbSMBDir.Text;
				string text4 = text3;
				while (text4.StartsWith("\\"))
				{
					text4 = text4.Substring(1);
				}
				int num = text4.IndexOf('\\');
				if (num > 0)
				{
					text4 = text4.Substring(0, num);
				}
				NetworkShareAccesser networkShareAccesser = NetworkShareAccesser.Access(text4, text, text2);
				if (networkShareAccesser.Result.Length > 0)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_Connectfailed, new string[0]));
					return;
				}
			}
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = this.tbSMBDir.Text;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.tbSMBDir.Text = folderBrowserDialog.SelectedPath;
			}
		}
		private void checkBoxOn_Paint(object sender, PaintEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox.Focused)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
			}
		}
		private void cbFTPAuth_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbFTPAuth.Checked)
			{
				this.tbFTPUsername.Enabled = false;
				this.tbFTPPsw.Enabled = false;
				return;
			}
			this.tbFTPUsername.Enabled = true;
			this.tbFTPPsw.Enabled = true;
		}
		private void rbStore_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rbStore_SMB.Checked)
			{
				this.panelSMB.Visible = true;
				this.panelFTP.Visible = false;
				return;
			}
			this.panelFTP.Visible = true;
			this.panelSMB.Visible = false;
		}
		private void tbFTPPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\b' && (e.KeyChar > '9' || e.KeyChar < '0'))
			{
				e.Handled = true;
			}
		}
		private void tbSMBDir_TextChanged(object sender, System.EventArgs e)
		{
			string text = this.tbSMBDir.Text;
			if (text.StartsWith("\\\\"))
			{
				this.tbSMBUsername.Enabled = true;
				this.tbSMBPsw.Enabled = true;
				return;
			}
			this.tbSMBUsername.Text = "";
			this.tbSMBUsername.Enabled = false;
			this.tbSMBPsw.Text = "";
			this.tbSMBPsw.Enabled = false;
		}
	}
}
