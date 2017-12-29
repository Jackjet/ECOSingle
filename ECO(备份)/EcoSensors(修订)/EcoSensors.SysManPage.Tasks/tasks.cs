using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Tasks
{
	public class tasks : UserControl
	{
		private IContainer components;
		private FlowLayoutPanel flowLayoutPanelTaskPage;
		private Button butGpPower;
		private Button butCfgBackup;
		private GpPowerTask gpPowerTask1;
		private Panel panel1;
		private Panel panel2;
		private CfgBackupTask cfgBackupTask1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(tasks));
			this.flowLayoutPanelTaskPage = new FlowLayoutPanel();
			this.butGpPower = new Button();
			this.butCfgBackup = new Button();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
			this.cfgBackupTask1 = new CfgBackupTask();
			this.gpPowerTask1 = new GpPowerTask();
			this.flowLayoutPanelTaskPage.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.flowLayoutPanelTaskPage.BackColor = Color.Gainsboro;
			this.flowLayoutPanelTaskPage.Controls.Add(this.butGpPower);
			this.flowLayoutPanelTaskPage.Controls.Add(this.butCfgBackup);
			componentResourceManager.ApplyResources(this.flowLayoutPanelTaskPage, "flowLayoutPanelTaskPage");
			this.flowLayoutPanelTaskPage.Name = "flowLayoutPanelTaskPage";
			componentResourceManager.ApplyResources(this.butGpPower, "butGpPower");
			this.butGpPower.MinimumSize = new Size(150, 27);
			this.butGpPower.Name = "butGpPower";
			this.butGpPower.Tag = "GpPower";
			this.butGpPower.UseVisualStyleBackColor = true;
			this.butGpPower.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butCfgBackup, "butCfgBackup");
			this.butCfgBackup.MinimumSize = new Size(150, 27);
			this.butCfgBackup.Name = "butCfgBackup";
			this.butCfgBackup.Tag = "CfgBackup";
			this.butCfgBackup.UseVisualStyleBackColor = true;
			this.butCfgBackup.Click += new System.EventHandler(this.comm_butClick);
			this.panel1.BackColor = Color.Gainsboro;
			this.panel1.Controls.Add(this.flowLayoutPanelTaskPage);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.panel2.Controls.Add(this.cfgBackupTask1);
			this.panel2.Controls.Add(this.gpPowerTask1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.cfgBackupTask1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.cfgBackupTask1, "cfgBackupTask1");
			this.cfgBackupTask1.Name = "cfgBackupTask1";
			this.gpPowerTask1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.gpPowerTask1, "gpPowerTask1");
			this.gpPowerTask1.Name = "gpPowerTask1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "tasks";
			this.flowLayoutPanelTaskPage.ResumeLayout(false);
			this.flowLayoutPanelTaskPage.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public tasks()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butGpPower, null);
				return;
			case 1:
				this.comm_butClick(this.butCfgBackup, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.gpPowerTask1.Visible = false;
			this.cfgBackupTask1.Visible = false;
			EcoGlobalVar.gl_SysDBCapCtrl.endDBcapTimer();
			foreach (Control control in this.flowLayoutPanelTaskPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butGpPower.Name))
					{
						this.gpPowerTask1.Visible = true;
						this.gpPowerTask1.pageInit();
					}
					else
					{
						if (name.Equals(this.butCfgBackup.Name))
						{
							this.cfgBackupTask1.Visible = true;
							this.cfgBackupTask1.pageInit();
						}
					}
				}
				else
				{
					((Button)control).Font = new Font(font.FontFamily, font.Size, FontStyle.Regular);
				}
			}
		}
	}
}
