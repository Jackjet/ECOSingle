using EcoSensors.SysManDB;
using EcoSensors.SysManMaint;
using EcoSensors.SysManPage.Billing;
using EcoSensors.SysManPage.settings;
using EcoSensors.SysManPage.Tasks;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage
{
	public class SysManPage : UserControl
	{
		private IContainer components;
		private FlowLayoutPanel flowLayoutPanelSysManPage;
		private Button butSysDB;
		private Button butSysMaint;
		private Button butSysSettings;
        private SysManMaint.SysManMaint sysManMaint1;
        private SysManDB.SysManDB sysManDB1;
		private Panel panel1;
		private Panel panel2;
		private Button butTaskSchedule;
		private SysSettings sysSettings1;
		private tasks tasks1;
		private Button butBilling;
        private Billing.Billing billing1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManPage));
			this.flowLayoutPanelSysManPage = new FlowLayoutPanel();
			this.butSysSettings = new Button();
			this.butSysMaint = new Button();
			this.butSysDB = new Button();
			this.butTaskSchedule = new Button();
			this.butBilling = new Button();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
            this.billing1 = new Billing.Billing();
			this.tasks1 = new tasks();
            this.sysManDB1 = new SysManDB.SysManDB();
            this.sysManMaint1 = new SysManMaint.SysManMaint();
			this.sysSettings1 = new SysSettings();
			this.flowLayoutPanelSysManPage.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.flowLayoutPanelSysManPage.BackColor = Color.FromArgb(18, 160, 143);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysSettings);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysMaint);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysDB);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butTaskSchedule);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butBilling);
			componentResourceManager.ApplyResources(this.flowLayoutPanelSysManPage, "flowLayoutPanelSysManPage");
			this.flowLayoutPanelSysManPage.Name = "flowLayoutPanelSysManPage";
			componentResourceManager.ApplyResources(this.butSysSettings, "butSysSettings");
			this.butSysSettings.MinimumSize = new Size(160, 27);
			this.butSysSettings.Name = "butSysSettings";
			this.butSysSettings.Tag = "Settings";
			this.butSysSettings.UseVisualStyleBackColor = true;
			this.butSysSettings.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysMaint, "butSysMaint");
			this.butSysMaint.MinimumSize = new Size(160, 27);
			this.butSysMaint.Name = "butSysMaint";
			this.butSysMaint.Tag = "Maintenance";
			this.butSysMaint.UseVisualStyleBackColor = true;
			this.butSysMaint.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysDB, "butSysDB");
			this.butSysDB.MinimumSize = new Size(160, 27);
			this.butSysDB.Name = "butSysDB";
			this.butSysDB.Tag = "Database";
			this.butSysDB.UseVisualStyleBackColor = true;
			this.butSysDB.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butTaskSchedule, "butTaskSchedule");
			this.butTaskSchedule.MinimumSize = new Size(160, 27);
			this.butTaskSchedule.Name = "butTaskSchedule";
			this.butTaskSchedule.Tag = "Tag_task";
			this.butTaskSchedule.UseVisualStyleBackColor = true;
			this.butTaskSchedule.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butBilling, "butBilling");
			this.butBilling.MinimumSize = new Size(160, 27);
			this.butBilling.Name = "butBilling";
			this.butBilling.Tag = "Tag_Billing";
			this.butBilling.UseVisualStyleBackColor = true;
			this.butBilling.Click += new System.EventHandler(this.comm_butClick);
			this.panel1.BackColor = Color.FromArgb(18, 160, 143);
			this.panel1.Controls.Add(this.flowLayoutPanelSysManPage);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.panel2.Controls.Add(this.billing1);
			this.panel2.Controls.Add(this.tasks1);
			this.panel2.Controls.Add(this.sysManDB1);
			this.panel2.Controls.Add(this.sysManMaint1);
			this.panel2.Controls.Add(this.sysSettings1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.billing1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.billing1, "billing1");
			this.billing1.Name = "billing1";
			this.tasks1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.tasks1, "tasks1");
			this.tasks1.Name = "tasks1";
			this.sysManDB1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManDB1, "sysManDB1");
			this.sysManDB1.Name = "sysManDB1";
			this.sysManMaint1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManMaint1, "sysManMaint1");
			this.sysManMaint1.ForeColor = SystemColors.ControlText;
			this.sysManMaint1.Name = "sysManMaint1";
			this.sysSettings1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysSettings1, "sysSettings1");
			this.sysSettings1.Name = "sysSettings1";
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.Name = "SysManPage";
			this.flowLayoutPanelSysManPage.ResumeLayout(false);
			this.flowLayoutPanelSysManPage.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public SysManPage()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butSysSettings, null);
				return;
			case 1:
				this.comm_butClick(this.butSysMaint, null);
				return;
			case 2:
				this.comm_butClick(this.butSysDB, null);
				return;
			case 3:
				this.comm_butClick(this.butTaskSchedule, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.sysSettings1.Visible = false;
			this.sysManMaint1.Visible = false;
			this.sysManDB1.Visible = false;
			this.tasks1.Visible = false;
			this.billing1.Visible = false;
			EcoGlobalVar.gl_SysDBCapCtrl.endDBcapTimer();
			foreach (Control control in this.flowLayoutPanelSysManPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butSysSettings.Name))
					{
						this.sysSettings1.Visible = true;
						this.sysSettings1.pageInit(0);
					}
					else
					{
						if (name.Equals(this.butSysMaint.Name))
						{
							this.sysManMaint1.pageInit();
							this.sysManMaint1.Visible = true;
						}
						else
						{
							if (name.Equals(this.butSysDB.Name))
							{
								this.sysManDB1.Visible = true;
								this.sysManDB1.pageInit(0);
							}
							else
							{
								if (name.Equals(this.butTaskSchedule.Name))
								{
									this.tasks1.Visible = true;
									this.tasks1.pageInit(0);
								}
								else
								{
									if (name.Equals(this.butBilling.Name))
									{
										this.billing1.Visible = true;
										this.billing1.pageInit(0);
									}
								}
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
	}
}
