using DBAccessAPI.user;
using EcoSensors.EnegManPage.Analysis;
using EcoSensors.EnegManPage.AnalysisThermal;
using EcoSensors.EnegManPage.DashBoard;
using EcoSensors.EnegManPage.DashBoardUser;
using EcoSensors.EnegManPage.DataGPOP;
using EcoSensors.EnegManPage.PowerOp;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage
{
	public class EnegManPage : UserControl
	{
		private IContainer components;
		private FlowLayoutPanel flowLayoutPanelEnegManPage;
		private Button butAnalysis;
		private Button butDashboard;
		private Button butPWCtrl;
		private EnegAnalysis enegAnalysis1;
		private EPowerOp EPowerOp1;
        private DashBoard.DashBoard dashBoard1;
		private DashBoardUser.DashBoardUser dashBoardUser;
		private Panel panel2;
		private Button butDataGpCtrl;
		private DataGpOPAll dataGpOPAll1;
		private Button butThermAnalysis;
		private ThermAnalysis thermAnalysis1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EnegManPage));
			this.flowLayoutPanelEnegManPage = new FlowLayoutPanel();
			this.butDashboard = new Button();
			this.butPWCtrl = new Button();
			this.butDataGpCtrl = new Button();
			this.butAnalysis = new Button();
			this.butThermAnalysis = new Button();
			this.panel2 = new Panel();
            this.dashBoard1 = new DashBoard.DashBoard();
            this.dashBoardUser = new DashBoardUser.DashBoardUser();
			this.thermAnalysis1 = new ThermAnalysis();
			this.dataGpOPAll1 = new DataGpOPAll();
			this.EPowerOp1 = new EPowerOp();
			this.enegAnalysis1 = new EnegAnalysis();
			this.flowLayoutPanelEnegManPage.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.flowLayoutPanelEnegManPage.BackColor = Color.FromArgb(18, 160, 143);
			this.flowLayoutPanelEnegManPage.Controls.Add(this.butDashboard);
			this.flowLayoutPanelEnegManPage.Controls.Add(this.butPWCtrl);
			this.flowLayoutPanelEnegManPage.Controls.Add(this.butDataGpCtrl);
			this.flowLayoutPanelEnegManPage.Controls.Add(this.butAnalysis);
			this.flowLayoutPanelEnegManPage.Controls.Add(this.butThermAnalysis);
			componentResourceManager.ApplyResources(this.flowLayoutPanelEnegManPage, "flowLayoutPanelEnegManPage");
			this.flowLayoutPanelEnegManPage.Name = "flowLayoutPanelEnegManPage";
			componentResourceManager.ApplyResources(this.butDashboard, "butDashboard");
			this.butDashboard.MinimumSize = new Size(160, 27);
			this.butDashboard.Name = "butDashboard";
			this.butDashboard.Tag = "Dash Board";
			this.butDashboard.UseVisualStyleBackColor = true;
			this.butDashboard.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butPWCtrl, "butPWCtrl");
			this.butPWCtrl.MinimumSize = new Size(160, 27);
			this.butPWCtrl.Name = "butPWCtrl";
			this.butPWCtrl.Tag = "Power Control";
			this.butPWCtrl.UseVisualStyleBackColor = true;
			this.butPWCtrl.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butDataGpCtrl, "butDataGpCtrl");
			this.butDataGpCtrl.MinimumSize = new Size(160, 27);
			this.butDataGpCtrl.Name = "butDataGpCtrl";
			this.butDataGpCtrl.Tag = "DataGP Control";
			this.butDataGpCtrl.UseVisualStyleBackColor = true;
			this.butDataGpCtrl.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butAnalysis, "butAnalysis");
			this.butAnalysis.MinimumSize = new Size(160, 27);
			this.butAnalysis.Name = "butAnalysis";
			this.butAnalysis.Tag = "Power Analysis";
			this.butAnalysis.UseVisualStyleBackColor = true;
			this.butAnalysis.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butThermAnalysis, "butThermAnalysis");
			this.butThermAnalysis.MinimumSize = new Size(160, 27);
			this.butThermAnalysis.Name = "butThermAnalysis";
			this.butThermAnalysis.Tag = "Thermal Analysis";
			this.butThermAnalysis.UseVisualStyleBackColor = true;
			this.butThermAnalysis.Click += new System.EventHandler(this.comm_butClick);
			this.panel2.Controls.Add(this.dashBoard1);
			this.panel2.Controls.Add(this.dashBoardUser);
			this.panel2.Controls.Add(this.thermAnalysis1);
			this.panel2.Controls.Add(this.dataGpOPAll1);
			this.panel2.Controls.Add(this.EPowerOp1);
			this.panel2.Controls.Add(this.enegAnalysis1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.dashBoard1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.dashBoard1, "dashBoard1");
			this.dashBoard1.FreshFlg_DashBoard = 1;
			this.dashBoard1.Name = "dashBoard1";
			this.dashBoardUser.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.dashBoardUser, "dashBoardUser");
			this.dashBoardUser.Name = "dashBoardUser";
			this.thermAnalysis1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.thermAnalysis1, "thermAnalysis1");
			this.thermAnalysis1.Name = "thermAnalysis1";
			this.dataGpOPAll1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.dataGpOPAll1, "dataGpOPAll1");
			this.dataGpOPAll1.GroupTreeOpFlg = 64;
			this.dataGpOPAll1.Name = "dataGpOPAll1";
			this.EPowerOp1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.EPowerOp1, "EPowerOp1");
			this.EPowerOp1.Name = "EPowerOp1";
			this.enegAnalysis1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.enegAnalysis1, "enegAnalysis1");
			this.enegAnalysis1.Name = "enegAnalysis1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.flowLayoutPanelEnegManPage);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "EnegManPage";
			this.flowLayoutPanelEnegManPage.ResumeLayout(false);
			this.flowLayoutPanelEnegManPage.PerformLayout();
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public EnegManPage()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_EnegManPage = this;
			if (EcoGlobalVar.ECOAppRunMode == 2)
			{
				this.butAnalysis.Visible = false;
				this.butThermAnalysis.Visible = false;
			}
		}
		public void pageInit_1()
		{
			UserInfo gl_LoginUser = EcoGlobalVar.gl_LoginUser;
			if (gl_LoginUser.UserType != 1)
			{
				this.dashBoard1.pageInit_1();
				this.dashBoard1.Visible = true;
				this.dashBoardUser.Visible = false;
			}
			else
			{
				this.dashBoard1.Visible = false;
				this.dashBoardUser.pageInit();
				this.dashBoardUser.Visible = true;
			}
			this.enegAnalysis1.Visible = false;
			this.EPowerOp1.Visible = false;
			this.dataGpOPAll1.Visible = false;
			this.thermAnalysis1.Visible = false;
			if (gl_LoginUser.UserType != 0 && (gl_LoginUser.UserDevice == null || gl_LoginUser.UserDevice.Length == 0))
			{
				this.butPWCtrl.Visible = false;
			}
			else
			{
				this.butPWCtrl.Visible = true;
			}
			if (gl_LoginUser.UserType != 0 && (gl_LoginUser.UserGroup == null || gl_LoginUser.UserGroup.Length == 0))
			{
				this.butDataGpCtrl.Visible = false;
				return;
			}
			this.butDataGpCtrl.Visible = true;
		}
		public void pageInit(int selIndex)
		{
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butDashboard, null);
				return;
			case 1:
				this.comm_butClick(this.butPWCtrl, null);
				return;
			case 2:
				this.comm_butClick(this.butDataGpCtrl, null);
				return;
			case 3:
				this.comm_butClick(this.butAnalysis, null);
				return;
			case 4:
				this.comm_butClick(this.butThermAnalysis, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.dashBoard1.Visible = false;
			this.dashBoardUser.Visible = false;
			this.enegAnalysis1.Visible = false;
			this.EPowerOp1.Visible = false;
			this.dataGpOPAll1.Visible = false;
			this.thermAnalysis1.Visible = false;
			foreach (Control control in this.flowLayoutPanelEnegManPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butDashboard.Name))
					{
						EcoGlobalVar.gl_PowerOPCtrl.endTimer();
						EcoGlobalVar.gl_DataGpOPAll.endTimer();
						EcoGlobalVar.gl_DashBoardCtrl.endBoardTimer();
						EcoGlobalVar.gl_DashBoardUserCtrl.endBoardTimer();
						UserInfo gl_LoginUser = EcoGlobalVar.gl_LoginUser;
						if (gl_LoginUser.UserType != 1)
						{
							this.dashBoard1.Visible = true;
							this.dashBoard1.pageInit();
						}
						else
						{
							this.dashBoardUser.Visible = true;
							this.dashBoardUser.pageInit();
						}
					}
					else
					{
						if (name.Equals(this.butPWCtrl.Name))
						{
							EcoGlobalVar.gl_PowerOPCtrl.endTimer();
							EcoGlobalVar.gl_DataGpOPAll.endTimer();
							EcoGlobalVar.gl_DashBoardCtrl.endBoardTimer();
							EcoGlobalVar.gl_DashBoardUserCtrl.endBoardTimer();
							this.EPowerOp1.Visible = true;
							this.EPowerOp1.pageInit();
						}
						else
						{
							if (name.Equals(this.butDataGpCtrl.Name))
							{
								EcoGlobalVar.gl_PowerOPCtrl.endTimer();
								EcoGlobalVar.gl_DataGpOPAll.endTimer();
								EcoGlobalVar.gl_DashBoardCtrl.endBoardTimer();
								EcoGlobalVar.gl_DashBoardUserCtrl.endBoardTimer();
								this.dataGpOPAll1.Visible = true;
								this.dataGpOPAll1.pageInit();
							}
							else
							{
								if (name.Equals(this.butAnalysis.Name))
								{
									EcoGlobalVar.gl_PowerOPCtrl.endTimer();
									EcoGlobalVar.gl_DataGpOPAll.endTimer();
									EcoGlobalVar.gl_DashBoardCtrl.endBoardTimer();
									EcoGlobalVar.gl_DashBoardUserCtrl.endBoardTimer();
									this.enegAnalysis1.Visible = true;
									this.enegAnalysis1.pageInit(0);
								}
								else
								{
									if (name.Equals(this.butThermAnalysis.Name))
									{
										EcoGlobalVar.gl_PowerOPCtrl.endTimer();
										EcoGlobalVar.gl_DataGpOPAll.endTimer();
										EcoGlobalVar.gl_DashBoardCtrl.endBoardTimer();
										EcoGlobalVar.gl_DashBoardUserCtrl.endBoardTimer();
										this.thermAnalysis1.Visible = true;
										this.thermAnalysis1.pageInit(0);
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
		public void hiddenPowerAnalysisButton()
		{
			this.butAnalysis.Hide();
			this.butThermAnalysis.Hide();
		}
		public void showPowerControlButton(bool sh)
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(new Action(()=>
                    {
                        this.showPowerControlButton(sh);
                    }));				
				return;
			}
			if (sh)
			{
				this.butPWCtrl.Show();
				return;
			}
			if (this.EPowerOp1.Visible)
			{
				this.comm_butClick(this.butDashboard, null);
			}
			this.butPWCtrl.Hide();
		}
		public void showGpControlButton(bool sh)
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(new Action(()=>
				{
					this.showGpControlButton(sh);
				}));
				return;
			}
			if (sh)
			{
				this.butDataGpCtrl.Show();
				return;
			}
			if (this.dataGpOPAll1.Visible)
			{
				this.comm_butClick(this.butDashboard, null);
			}
			this.butDataGpCtrl.Hide();
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (!this.dashBoard1.Visible && !this.dashBoardUser.Visible)
			{
				return base.ProcessCmdKey(ref msg, keyData);
			}
			return (keyData != Keys.Tab && keyData != Keys.Left && keyData != Keys.Right && keyData != Keys.Up && keyData != Keys.Down && keyData != Keys.Space && keyData != Keys.Return) || base.ProcessCmdKey(ref msg, keyData);
		}
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 63001)
			{
				this.pageInit_1();
				this.comm_butClick(this.butDashboard, null);
				return;
			}
			base.DefWndProc(ref m);
		}
	}
}
