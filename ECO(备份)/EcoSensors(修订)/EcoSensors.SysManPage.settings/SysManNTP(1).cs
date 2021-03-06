using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class SysManNTP : UserControl
	{
		private IContainer components;
		private TextBox txtcalibratetime;
		private Button btnCalibrate;
		private ComboBox cbopreserver;
		private ComboBox cboaltserver;
		private TextBox txtpreip;
		private CheckBox chkpreip;
		private CheckBox chkaltip;
		private TextBox txtaltip;
		private CheckBox chkaltserver;
		private TextBox txtadjtime;
		private CheckBox chkadjust;
		private CheckBox chkdaylight;
		private Label label6;
		private Label label5;
		private Label label4;
		private Button btnsave;
		private Label label2;
		private Timer timer1;
		private TextBox txttime;
		private TextBox txtdate;
		private TextBox txtzone;
		private Label label3;
		private GroupBox groupBox1;
		private Label label1;
		public SysManNTP()
		{
			this.InitializeComponent();
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManNTP));
			this.txtcalibratetime = new TextBox();
			this.btnCalibrate = new Button();
			this.cbopreserver = new ComboBox();
			this.cboaltserver = new ComboBox();
			this.txtpreip = new TextBox();
			this.chkpreip = new CheckBox();
			this.chkaltip = new CheckBox();
			this.txtaltip = new TextBox();
			this.chkaltserver = new CheckBox();
			this.txtadjtime = new TextBox();
			this.chkadjust = new CheckBox();
			this.chkdaylight = new CheckBox();
			this.label6 = new Label();
			this.label5 = new Label();
			this.label4 = new Label();
			this.btnsave = new Button();
			this.label2 = new Label();
			this.timer1 = new Timer(this.components);
			this.txttime = new TextBox();
			this.txtdate = new TextBox();
			this.txtzone = new TextBox();
			this.label3 = new Label();
			this.groupBox1 = new GroupBox();
			this.label1 = new Label();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.txtcalibratetime, "txtcalibratetime");
			this.txtcalibratetime.ForeColor = SystemColors.ControlText;
			this.txtcalibratetime.Name = "txtcalibratetime";
			componentResourceManager.ApplyResources(this.btnCalibrate, "btnCalibrate");
			this.btnCalibrate.ForeColor = SystemColors.ControlText;
			this.btnCalibrate.Name = "btnCalibrate";
			this.btnCalibrate.UseVisualStyleBackColor = true;
			this.cbopreserver.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbopreserver, "cbopreserver");
			this.cbopreserver.ForeColor = SystemColors.ControlText;
			this.cbopreserver.FormattingEnabled = true;
			this.cbopreserver.Name = "cbopreserver";
			this.cboaltserver.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cboaltserver, "cboaltserver");
			this.cboaltserver.ForeColor = SystemColors.ControlText;
			this.cboaltserver.FormattingEnabled = true;
			this.cboaltserver.Name = "cboaltserver";
			componentResourceManager.ApplyResources(this.txtpreip, "txtpreip");
			this.txtpreip.ForeColor = SystemColors.ControlText;
			this.txtpreip.Name = "txtpreip";
			componentResourceManager.ApplyResources(this.chkpreip, "chkpreip");
			this.chkpreip.ForeColor = SystemColors.ControlText;
			this.chkpreip.Name = "chkpreip";
			this.chkpreip.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkaltip, "chkaltip");
			this.chkaltip.ForeColor = SystemColors.ControlText;
			this.chkaltip.Name = "chkaltip";
			this.chkaltip.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtaltip, "txtaltip");
			this.txtaltip.ForeColor = SystemColors.ControlText;
			this.txtaltip.Name = "txtaltip";
			componentResourceManager.ApplyResources(this.chkaltserver, "chkaltserver");
			this.chkaltserver.ForeColor = SystemColors.ControlText;
			this.chkaltserver.Name = "chkaltserver";
			this.chkaltserver.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtadjtime, "txtadjtime");
			this.txtadjtime.ForeColor = SystemColors.ControlText;
			this.txtadjtime.Name = "txtadjtime";
			componentResourceManager.ApplyResources(this.chkadjust, "chkadjust");
			this.chkadjust.ForeColor = SystemColors.ControlText;
			this.chkadjust.Name = "chkadjust";
			this.chkadjust.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.chkdaylight, "chkdaylight");
			this.chkdaylight.ForeColor = SystemColors.ControlText;
			this.chkdaylight.Name = "chkdaylight";
			this.chkdaylight.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = SystemColors.ControlText;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = SystemColors.ControlText;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = SystemColors.ControlText;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.btnsave, "btnsave");
			this.btnsave.ForeColor = SystemColors.ControlText;
			this.btnsave.Name = "btnsave";
			this.btnsave.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = SystemColors.ControlText;
			this.label2.Name = "label2";
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			componentResourceManager.ApplyResources(this.txttime, "txttime");
			this.txttime.ForeColor = SystemColors.ControlText;
			this.txttime.Name = "txttime";
			this.txttime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.txtdate, "txtdate");
			this.txtdate.ForeColor = SystemColors.ControlText;
			this.txtdate.Name = "txtdate";
			this.txtdate.ReadOnly = true;
			componentResourceManager.ApplyResources(this.txtzone, "txtzone");
			this.txtzone.ForeColor = SystemColors.ControlText;
			this.txtzone.Name = "txtzone";
			this.txtzone.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = SystemColors.ControlText;
			this.label3.Name = "label3";
			this.groupBox1.Controls.Add(this.txtcalibratetime);
			this.groupBox1.Controls.Add(this.btnCalibrate);
			this.groupBox1.Controls.Add(this.cbopreserver);
			this.groupBox1.Controls.Add(this.cboaltserver);
			this.groupBox1.Controls.Add(this.txtpreip);
			this.groupBox1.Controls.Add(this.chkpreip);
			this.groupBox1.Controls.Add(this.chkaltip);
			this.groupBox1.Controls.Add(this.txtaltip);
			this.groupBox1.Controls.Add(this.chkaltserver);
			this.groupBox1.Controls.Add(this.txtadjtime);
			this.groupBox1.Controls.Add(this.chkadjust);
			this.groupBox1.Controls.Add(this.chkdaylight);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.btnsave);
			this.groupBox1.Controls.Add(this.txttime);
			this.groupBox1.Controls.Add(this.txtdate);
			this.groupBox1.Controls.Add(this.txtzone);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = SystemColors.ControlText;
			this.label1.Name = "label1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysManNTP";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
