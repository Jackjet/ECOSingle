using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class SysSettings : UserControl
	{
		private IContainer components;
		private Panel panel1;
		private FlowLayoutPanel flowLayoutPanelSysManPage;
		private Button butSysParaSet;
		private Button butSysSNMP;
		private Button butSysSMTP;
		private Button butSysNTP;
		private Panel panel2;
		private SysPara sysPara1;
		private SysManSNMP sysManSNMP1;
		private SysManSMTP sysManSMTP1;
		private SysManNTP sysManNTP1;
		private Button butSysOther;
		private SysOthers sysOthers1;
		private Button butSysSession;
		private SysSession sysSession1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysSettings));
			this.panel1 = new Panel();
			this.flowLayoutPanelSysManPage = new FlowLayoutPanel();
			this.butSysParaSet = new Button();
			this.butSysSNMP = new Button();
			this.butSysSMTP = new Button();
			this.butSysOther = new Button();
			this.butSysSession = new Button();
			this.butSysNTP = new Button();
			this.panel2 = new Panel();
			this.sysOthers1 = new SysOthers();
			this.sysManSMTP1 = new SysManSMTP();
			this.sysManSNMP1 = new SysManSNMP();
			this.sysPara1 = new SysPara();
			this.sysManNTP1 = new SysManNTP();
			this.sysSession1 = new SysSession();
			this.panel1.SuspendLayout();
			this.flowLayoutPanelSysManPage.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.panel1.BackColor = Color.Gainsboro;
			this.panel1.Controls.Add(this.flowLayoutPanelSysManPage);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.flowLayoutPanelSysManPage.BackColor = Color.Gainsboro;
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysParaSet);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysSNMP);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysSMTP);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysOther);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysSession);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysNTP);
			componentResourceManager.ApplyResources(this.flowLayoutPanelSysManPage, "flowLayoutPanelSysManPage");
			this.flowLayoutPanelSysManPage.Name = "flowLayoutPanelSysManPage";
			componentResourceManager.ApplyResources(this.butSysParaSet, "butSysParaSet");
			this.butSysParaSet.MinimumSize = new Size(150, 27);
			this.butSysParaSet.Name = "butSysParaSet";
			this.butSysParaSet.Tag = "Parameters";
			this.butSysParaSet.UseVisualStyleBackColor = true;
			this.butSysParaSet.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysSNMP, "butSysSNMP");
			this.butSysSNMP.MinimumSize = new Size(150, 27);
			this.butSysSNMP.Name = "butSysSNMP";
			this.butSysSNMP.Tag = "SNMP";
			this.butSysSNMP.UseVisualStyleBackColor = true;
			this.butSysSNMP.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysSMTP, "butSysSMTP");
			this.butSysSMTP.MinimumSize = new Size(150, 27);
			this.butSysSMTP.Name = "butSysSMTP";
			this.butSysSMTP.Tag = "SMTP";
			this.butSysSMTP.UseVisualStyleBackColor = true;
			this.butSysSMTP.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysOther, "butSysOther");
			this.butSysOther.MinimumSize = new Size(150, 27);
			this.butSysOther.Name = "butSysOther";
			this.butSysOther.Tag = "Other";
			this.butSysOther.UseVisualStyleBackColor = true;
			this.butSysOther.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysSession, "butSysSession");
			this.butSysSession.MinimumSize = new Size(150, 27);
			this.butSysSession.Name = "butSysSession";
			this.butSysSession.Tag = "Session";
			this.butSysSession.UseVisualStyleBackColor = true;
			this.butSysSession.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysNTP, "butSysNTP");
			this.butSysNTP.MinimumSize = new Size(150, 27);
			this.butSysNTP.Name = "butSysNTP";
			this.butSysNTP.Tag = "NTP";
			this.butSysNTP.UseVisualStyleBackColor = true;
			this.butSysNTP.Click += new System.EventHandler(this.comm_butClick);
			this.panel2.Controls.Add(this.sysOthers1);
			this.panel2.Controls.Add(this.sysManSMTP1);
			this.panel2.Controls.Add(this.sysManSNMP1);
			this.panel2.Controls.Add(this.sysPara1);
			this.panel2.Controls.Add(this.sysManNTP1);
			this.panel2.Controls.Add(this.sysSession1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.sysOthers1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysOthers1, "sysOthers1");
			this.sysOthers1.Name = "sysOthers1";
			this.sysManSMTP1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManSMTP1, "sysManSMTP1");
			this.sysManSMTP1.Name = "sysManSMTP1";
			this.sysManSNMP1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManSNMP1, "sysManSNMP1");
			this.sysManSNMP1.Name = "sysManSNMP1";
			this.sysPara1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysPara1, "sysPara1");
			this.sysPara1.Name = "sysPara1";
			this.sysManNTP1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManNTP1, "sysManNTP1");
			this.sysManNTP1.Name = "sysManNTP1";
			this.sysSession1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysSession1, "sysSession1");
			this.sysSession1.Name = "sysSession1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysSettings";
			this.panel1.ResumeLayout(false);
			this.flowLayoutPanelSysManPage.ResumeLayout(false);
			this.flowLayoutPanelSysManPage.PerformLayout();
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public SysSettings()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			if (!EcoGlobalVar.gl_supportBP)
			{
				this.butSysOther.Visible = false;
			}
			else
			{
				this.butSysOther.Visible = true;
			}
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butSysParaSet, null);
				return;
			case 1:
				this.comm_butClick(this.butSysSNMP, null);
				return;
			case 2:
				this.comm_butClick(this.butSysSMTP, null);
				return;
			case 3:
				this.comm_butClick(this.butSysOther, null);
				return;
			case 4:
				this.comm_butClick(this.butSysSession, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.sysPara1.Visible = false;
			this.sysManSNMP1.Visible = false;
			this.sysManSMTP1.Visible = false;
			this.sysOthers1.Visible = false;
			this.sysManNTP1.Visible = false;
			EcoGlobalVar.gl_SysDBCapCtrl.endDBcapTimer();
			foreach (Control control in this.flowLayoutPanelSysManPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butSysParaSet.Name))
					{
						this.sysPara1.Visible = true;
						this.sysPara1.pageInit();
					}
					else
					{
						if (name.Equals(this.butSysSNMP.Name))
						{
							this.sysManSNMP1.Visible = true;
							this.sysManSNMP1.pageInit();
						}
						else
						{
							if (name.Equals(this.butSysSMTP.Name))
							{
								this.sysManSMTP1.Visible = true;
								this.sysManSMTP1.pageInit();
							}
							else
							{
								if (name.Equals(this.butSysOther.Name))
								{
									this.sysOthers1.Visible = true;
									this.sysOthers1.pageInit();
								}
								else
								{
									if (name.Equals(this.butSysNTP.Name))
									{
										this.sysManNTP1.Visible = true;
									}
									else
									{
										if (name.Equals(this.butSysSession.Name))
										{
											this.sysSession1.Visible = true;
											this.sysSession1.pageInit();
										}
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
