using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.LogOptions
{
	public class LogOptions : UserControl
	{
		private const int P_DAYS_MIN = 7;
		private const int P_DAYS_MAX = 366;
		private const int R_RECOEDS_MIN = 100;
		private const int R_RECOEDS_MAX = 99999;
		private const int PAGE_SIZE_MIN = 10;
		private const int PAGE_SIZE_MAX = 100;
		private int i_days;
		private int i_records;
		private int i_pagesize;
		private LogSetting myLogSetting;
		private IContainer components;
		private GroupBox groupBox1;
		private Panel panel1;
		private RadioButton rbbyrecord;
		private RadioButton rbbyperiod;
		private Button btnsave;
		private TextBox txtrecords;
		private TextBox txtperiod;
		private TextBox txtpagesize;
		private Label label3;
		private Label label1;
		private Label lab_1;
		public LogOptions()
		{
			this.InitializeComponent();
			this.txtperiod.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtrecords.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtpagesize.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit()
		{
			this.myLogSetting = new LogSetting();
			if (this.myLogSetting.Maintenance == 0)
			{
				this.rbbyperiod.Checked = true;
				this.rbbyrecord.Checked = false;
				this.txtperiod.Enabled = true;
				this.txtrecords.Enabled = false;
				this.i_days = this.myLogSetting.Days;
				this.txtperiod.Text = this.i_days.ToString();
				this.txtrecords.Text = "";
			}
			else
			{
				this.rbbyperiod.Checked = false;
				this.rbbyrecord.Checked = true;
				this.txtperiod.Enabled = false;
				this.txtrecords.Enabled = true;
				this.i_records = this.myLogSetting.RecordNum;
				this.txtperiod.Text = "";
				this.txtrecords.Text = this.i_records.ToString();
			}
			this.i_pagesize = this.myLogSetting.PageSize;
			this.txtpagesize.Text = this.i_pagesize.ToString();
			this.btnsave.Enabled = false;
			this.groupBox1.Update();
		}
		private void rbbyperiod_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtperiod.Enabled = true;
			this.txtrecords.Enabled = false;
			this.i_days = this.myLogSetting.Days;
			this.txtperiod.Text = this.i_days.ToString();
			this.txtrecords.Text = "";
			if (this.i_days > 0 && this.i_pagesize > 0)
			{
				this.btnsave.Enabled = true;
			}
		}
		private void txtperiod_TextChanged(object sender, System.EventArgs e)
		{
			if (this.txtperiod.TextLength == 0)
			{
				this.i_days = 0;
			}
			else
			{
				int num;
				if (int.TryParse(this.txtperiod.Text, out num))
				{
					this.i_days = num;
				}
			}
			if (this.i_days != 0)
			{
				this.txtperiod.Text = this.i_days.ToString();
				this.txtperiod.Select(this.txtperiod.Text.Length, 1);
			}
			else
			{
				this.txtperiod.Text = "";
			}
			if (this.i_days > 0 && this.i_pagesize > 0)
			{
				this.btnsave.Enabled = true;
				return;
			}
			this.btnsave.Enabled = false;
		}
		private void rbbyrecord_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtperiod.Enabled = false;
			this.txtrecords.Enabled = true;
			this.i_records = this.myLogSetting.RecordNum;
			this.txtperiod.Text = "";
			this.txtrecords.Text = this.i_records.ToString();
			if (this.i_records > 0 && this.i_pagesize > 0)
			{
				this.btnsave.Enabled = true;
			}
		}
		private void txtrecords_TextChanged(object sender, System.EventArgs e)
		{
			if (this.txtrecords.TextLength == 0)
			{
				this.i_records = 0;
			}
			else
			{
				int num;
				if (int.TryParse(this.txtrecords.Text, out num))
				{
					this.i_records = num;
				}
			}
			if (this.i_records != 0)
			{
				this.txtrecords.Text = this.i_records.ToString();
				this.txtrecords.Select(this.txtrecords.Text.Length, 1);
			}
			else
			{
				this.txtrecords.Text = "";
			}
			if (this.i_records > 0 && this.i_pagesize > 0)
			{
				this.btnsave.Enabled = true;
				return;
			}
			this.btnsave.Enabled = false;
		}
		private void txtpagesize_TextChanged(object sender, System.EventArgs e)
		{
			if (this.txtpagesize.TextLength == 0)
			{
				this.i_pagesize = 0;
			}
			else
			{
				int num;
				if (int.TryParse(this.txtpagesize.Text, out num))
				{
					this.i_pagesize = num;
				}
			}
			if (this.i_pagesize != 0)
			{
				this.txtpagesize.Text = this.i_pagesize.ToString();
				this.txtpagesize.Select(this.txtpagesize.Text.Length, 1);
			}
			else
			{
				this.txtpagesize.Text = "";
			}
			if (this.i_pagesize > 0 && ((this.rbbyperiod.Checked && this.i_days > 0) || (!this.rbbyperiod.Checked && this.i_records > 0)))
			{
				this.btnsave.Enabled = true;
				return;
			}
			this.btnsave.Enabled = false;
		}
		private void btnsave_Click(object sender, System.EventArgs e)
		{
			if (this.rbbyperiod.Checked)
			{
				if (this.i_days < 7)
				{
					this.i_days = 7;
				}
				else
				{
					if (this.i_days > 366)
					{
						this.i_days = 366;
					}
				}
				this.txtperiod.Text = this.i_days.ToString();
				this.myLogSetting.Days = this.i_days;
				this.myLogSetting.Maintenance = 0;
			}
			else
			{
				if (this.i_records < 100)
				{
					this.i_records = 100;
				}
				else
				{
					if (this.i_records > 99999)
					{
						this.i_records = 99999;
					}
				}
				this.txtrecords.Text = this.i_records.ToString();
				this.myLogSetting.RecordNum = this.i_records;
				this.myLogSetting.Maintenance = 1;
			}
			if (this.i_pagesize < 10)
			{
				this.i_pagesize = 10;
			}
			else
			{
				if (this.i_pagesize > 100)
				{
					this.i_pagesize = 100;
				}
			}
			this.txtpagesize.Text = this.i_pagesize.ToString();
			this.myLogSetting.PageSize = this.i_pagesize;
			if (this.myLogSetting.update() > 0)
			{
				string valuePair = ValuePairs.getValuePair("Username");
				if (!string.IsNullOrEmpty(valuePair))
				{
					LogAPI.writeEventLog("0130030", new string[]
					{
						valuePair
					});
				}
				else
				{
					LogAPI.writeEventLog("0130030", new string[0]);
				}
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
				return;
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LogOptions));
			this.groupBox1 = new GroupBox();
			this.panel1 = new Panel();
			this.rbbyrecord = new RadioButton();
			this.rbbyperiod = new RadioButton();
			this.txtrecords = new TextBox();
			this.txtperiod = new TextBox();
			this.btnsave = new Button();
			this.txtpagesize = new TextBox();
			this.label3 = new Label();
			this.label1 = new Label();
			this.lab_1 = new Label();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.btnsave);
			this.groupBox1.Controls.Add(this.txtpagesize);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.lab_1);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.panel1.Controls.Add(this.rbbyrecord);
			this.panel1.Controls.Add(this.rbbyperiod);
			this.panel1.Controls.Add(this.txtrecords);
			this.panel1.Controls.Add(this.txtperiod);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.ForeColor = SystemColors.ControlText;
			this.panel1.Name = "panel1";
			componentResourceManager.ApplyResources(this.rbbyrecord, "rbbyrecord");
			this.rbbyrecord.Name = "rbbyrecord";
			this.rbbyrecord.TabStop = true;
			this.rbbyrecord.UseVisualStyleBackColor = true;
			this.rbbyrecord.CheckedChanged += new System.EventHandler(this.rbbyrecord_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbbyperiod, "rbbyperiod");
			this.rbbyperiod.Name = "rbbyperiod";
			this.rbbyperiod.TabStop = true;
			this.rbbyperiod.UseVisualStyleBackColor = true;
			this.rbbyperiod.CheckedChanged += new System.EventHandler(this.rbbyperiod_CheckedChanged);
			componentResourceManager.ApplyResources(this.txtrecords, "txtrecords");
			this.txtrecords.Name = "txtrecords";
			this.txtrecords.TextChanged += new System.EventHandler(this.txtrecords_TextChanged);
			componentResourceManager.ApplyResources(this.txtperiod, "txtperiod");
			this.txtperiod.Name = "txtperiod";
			this.txtperiod.TextChanged += new System.EventHandler(this.txtperiod_TextChanged);
			this.btnsave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsave, "btnsave");
			this.btnsave.ForeColor = SystemColors.ControlText;
			this.btnsave.Name = "btnsave";
			this.btnsave.UseVisualStyleBackColor = false;
			this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
			componentResourceManager.ApplyResources(this.txtpagesize, "txtpagesize");
			this.txtpagesize.Name = "txtpagesize";
			this.txtpagesize.TextChanged += new System.EventHandler(this.txtpagesize_TextChanged);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = SystemColors.ControlText;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = SystemColors.ControlText;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.lab_1, "lab_1");
			this.lab_1.ForeColor = SystemColors.ControlText;
			this.lab_1.Name = "lab_1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "LogOptions";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
