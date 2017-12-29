using CommonAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.AnalysisThermal
{
	public class ThermAnalysis : UserControl
	{
		private IContainer components;
		private FlowLayoutPanel fLyPlThermAnalysis;
		private Button butGenThermReport;
		private Button butThermReportMng;
		private Panel panel1;
		private ThermGenRptShow thermRptShow1;
		private ThermGenRptPara thermGenRptPara1;
		private ThermRptManage thermRptManage1;
		private int m_inAnalysisFlg = 1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ThermAnalysis));
			this.fLyPlThermAnalysis = new FlowLayoutPanel();
			this.butGenThermReport = new Button();
			this.butThermReportMng = new Button();
			this.panel1 = new Panel();
			this.thermRptManage1 = new ThermRptManage();
			this.thermRptShow1 = new ThermGenRptShow();
			this.thermGenRptPara1 = new ThermGenRptPara();
			this.fLyPlThermAnalysis.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.fLyPlThermAnalysis.BackColor = Color.Gainsboro;
			this.fLyPlThermAnalysis.Controls.Add(this.butGenThermReport);
			this.fLyPlThermAnalysis.Controls.Add(this.butThermReportMng);
			componentResourceManager.ApplyResources(this.fLyPlThermAnalysis, "fLyPlThermAnalysis");
			this.fLyPlThermAnalysis.Name = "fLyPlThermAnalysis";
			componentResourceManager.ApplyResources(this.butGenThermReport, "butGenThermReport");
			this.butGenThermReport.MinimumSize = new Size(150, 27);
			this.butGenThermReport.Name = "butGenThermReport";
			this.butGenThermReport.Tag = "Tag_genThermreport";
			this.butGenThermReport.UseVisualStyleBackColor = true;
			this.butGenThermReport.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butThermReportMng, "butThermReportMng");
			this.butThermReportMng.MinimumSize = new Size(150, 27);
			this.butThermReportMng.Name = "butThermReportMng";
			this.butThermReportMng.Tag = "Tag_Thermreportmng";
			this.butThermReportMng.UseVisualStyleBackColor = true;
			this.butThermReportMng.Click += new System.EventHandler(this.comm_butClick);
			this.panel1.Controls.Add(this.thermRptManage1);
			this.panel1.Controls.Add(this.thermRptShow1);
			this.panel1.Controls.Add(this.thermGenRptPara1);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.thermRptManage1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.thermRptManage1, "thermRptManage1");
			this.thermRptManage1.Name = "thermRptManage1";
			this.thermRptShow1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.thermRptShow1, "thermRptShow1");
			this.thermRptShow1.Name = "thermRptShow1";
			this.thermGenRptPara1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.thermGenRptPara1, "thermGenRptPara1");
			this.thermGenRptPara1.Name = "thermGenRptPara1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.fLyPlThermAnalysis);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "ThermAnalysis";
			this.fLyPlThermAnalysis.ResumeLayout(false);
			this.fLyPlThermAnalysis.PerformLayout();
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public ThermAnalysis()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butGenThermReport, null);
				return;
			case 1:
				this.comm_butClick(this.butThermReportMng, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.thermGenRptPara1.Visible = false;
			this.thermRptShow1.Visible = false;
			this.thermRptManage1.Visible = false;
			foreach (Control control in this.fLyPlThermAnalysis.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butGenThermReport.Name))
					{
						this.showRpt(this.m_inAnalysisFlg, 1);
					}
					else
					{
						if (name.Equals(this.butThermReportMng.Name))
						{
							this.thermRptManage1.pageInit();
							this.thermRptManage1.Visible = true;
						}
					}
				}
				else
				{
					((Button)control).Font = new Font(font.FontFamily, font.Size, FontStyle.Regular);
				}
			}
		}
		public void showRpt(int selpage, int needinit)
		{
			if (selpage == 1)
			{
				if (needinit == 1)
				{
					this.thermGenRptPara1.pageInit(this);
				}
				this.thermGenRptPara1.Visible = true;
				this.thermRptShow1.Visible = false;
				this.thermGenRptPara1.resettime();
				this.m_inAnalysisFlg = 1;
				return;
			}
			if (selpage == 2)
			{
				Program.IdleTimer_Pause(4);
				this.thermGenRptPara1.Visible = false;
				this.thermRptShow1.Visible = true;
				this.thermRptShow1.pageInit(this, this.thermGenRptPara1);
				DebugCenter.GetInstance().appendToFile("ThermalAnalysis Finish.");
				this.m_inAnalysisFlg = 4;
				return;
			}
			if (selpage == 4)
			{
				this.thermGenRptPara1.Visible = false;
				this.thermRptShow1.Visible = true;
				Program.IdleTimer_Run(4);
				this.m_inAnalysisFlg = 4;
			}
		}
	}
}
