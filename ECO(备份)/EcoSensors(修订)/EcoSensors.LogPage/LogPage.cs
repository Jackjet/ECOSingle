using EcoSensors.LogOptions;
using EcoSensors.LogSysLog;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.LogPage
{
	public class LogPage : UserControl
	{
		private IContainer components;
		private FlowLayoutPanel flowLayoutPanelLogPage;
		private Button butLogOption;
		private Button butSyslog;
        private LogOptions.LogOptions logOptions1;
        private LogSysLog.LogSysLog logSysLog1;
		private Panel panel1;
		private Panel panel2;
		private Button butEvent;
		private LogEvents logEvents1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LogPage));
			this.flowLayoutPanelLogPage = new FlowLayoutPanel();
			this.butSyslog = new Button();
			this.butLogOption = new Button();
			this.butEvent = new Button();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
			this.logEvents1 = new LogEvents();
            this.logOptions1 = new LogOptions.LogOptions();
            this.logSysLog1 = new LogSysLog.LogSysLog();
			this.flowLayoutPanelLogPage.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.flowLayoutPanelLogPage.BackColor = Color.FromArgb(18, 160, 143);
			this.flowLayoutPanelLogPage.Controls.Add(this.butSyslog);
			this.flowLayoutPanelLogPage.Controls.Add(this.butLogOption);
			this.flowLayoutPanelLogPage.Controls.Add(this.butEvent);
			componentResourceManager.ApplyResources(this.flowLayoutPanelLogPage, "flowLayoutPanelLogPage");
			this.flowLayoutPanelLogPage.Name = "flowLayoutPanelLogPage";
			componentResourceManager.ApplyResources(this.butSyslog, "butSyslog");
			this.butSyslog.MinimumSize = new Size(160, 27);
			this.butSyslog.Name = "butSyslog";
			this.butSyslog.Tag = "System Log";
			this.butSyslog.UseVisualStyleBackColor = true;
			this.butSyslog.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butLogOption, "butLogOption");
			this.butLogOption.MinimumSize = new Size(160, 27);
			this.butLogOption.Name = "butLogOption";
			this.butLogOption.Tag = "Log Option";
			this.butLogOption.UseVisualStyleBackColor = true;
			this.butLogOption.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butEvent, "butEvent");
			this.butEvent.MinimumSize = new Size(160, 27);
			this.butEvent.Name = "butEvent";
			this.butEvent.Tag = "Event";
			this.butEvent.UseVisualStyleBackColor = true;
			this.butEvent.Click += new System.EventHandler(this.comm_butClick);
			this.panel1.BackColor = Color.FromArgb(18, 160, 143);
			this.panel1.Controls.Add(this.flowLayoutPanelLogPage);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.panel2.Controls.Add(this.logEvents1);
			this.panel2.Controls.Add(this.logOptions1);
			this.panel2.Controls.Add(this.logSysLog1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.logEvents1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.logEvents1, "logEvents1");
			this.logEvents1.Name = "logEvents1";
			this.logOptions1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.logOptions1, "logOptions1");
			this.logOptions1.Name = "logOptions1";
			this.logSysLog1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.logSysLog1, "logSysLog1");
			this.logSysLog1.Name = "logSysLog1";
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.Name = "LogPage";
			this.flowLayoutPanelLogPage.ResumeLayout(false);
			this.flowLayoutPanelLogPage.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public LogPage()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butSyslog, null);
				return;
			case 1:
				this.comm_butClick(this.butLogOption, null);
				return;
			case 2:
				this.comm_butClick(this.butEvent, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.logSysLog1.Visible = false;
			this.logOptions1.Visible = false;
			this.logEvents1.Visible = false;
			foreach (Control control in this.flowLayoutPanelLogPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butSyslog.Name))
					{
						this.logSysLog1.Visible = true;
						this.logSysLog1.pageInit();
					}
					else
					{
						if (name.Equals(this.butLogOption.Name))
						{
							this.logOptions1.Visible = true;
							this.logOptions1.pageInit();
						}
						else
						{
							if (name.Equals(this.butEvent.Name))
							{
								this.logEvents1.Visible = true;
								this.logEvents1.pageInit();
							}
						}
					}
				}
				else
				{
					((Button)control).Font = new Font(font.FontFamily, font.Size, FontStyle.Regular);
				}
			}
		}
		public void hidden_LogOptionEvent()
		{
			this.butLogOption.Hide();
			this.butEvent.Hide();
		}
	}
}
