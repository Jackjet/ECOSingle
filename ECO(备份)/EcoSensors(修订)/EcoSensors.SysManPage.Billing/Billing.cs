using CommonAPI;
using EcoSensors._Lang;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Billing
{
	public class Billing : UserControl
	{
		private delegate void AnalysisRefresh();
		private const int Interval_Timer = 1000;
		private int m_inAnalysisFlg = 1;
		private int m_oldAnalysisFlg;
		private BillingRptParaClass m_pParaClass;
		private System.Timers.Timer dTimer;
		private IContainer components;
		private Panel panel1;
		private FlowLayoutPanel flowLayoutPanelBillingPage;
		private Button butBillSetting;
		private Button butBillGenerate;
		private Panel panel2;
		private Button butBillRptMng;
		private BillingSettings billingSettings1;
		private BillingRptMng billingRptMng1;
		private BillingRptShow billingRptShow1;
		private BillingRptPara billingRptPara1;
		public Billing()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_BillingRptCtrl = this;
			if (this.dTimer == null)
			{
				this.dTimer = new System.Timers.Timer();
				this.dTimer.Elapsed += new ElapsedEventHandler(this.theTimeout);
				this.dTimer.Interval = 100.0;
				this.dTimer.AutoReset = true;
				this.dTimer.Enabled = false;
			}
		}
		public void pageInit(int selIndex)
		{
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butBillSetting, null);
				return;
			case 1:
				this.comm_butClick(this.butBillGenerate, null);
				return;
			case 2:
				this.comm_butClick(this.butBillRptMng, null);
				return;
			default:
				return;
			}
		}
		private void theTimeout(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new Billing.AnalysisRefresh(this.AnalysisTimerRefresh));
				return;
			}
			this.AnalysisTimerRefresh();
		}
		private void AnalysisTimerRefresh()
		{
			if (this.m_oldAnalysisFlg == 2 && this.m_inAnalysisFlg == 3)
			{
				this.dTimer.Enabled = false;
				this.showRpt(3, 0);
			}
		}
		public void endTimer()
		{
			if (this.dTimer != null)
			{
				this.dTimer.Enabled = false;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.billingSettings1.Visible = false;
			this.billingRptPara1.Visible = false;
			this.billingRptShow1.Visible = false;
			this.billingRptMng1.Visible = false;
			foreach (Control control in this.flowLayoutPanelBillingPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butBillSetting.Name))
					{
						this.billingSettings1.pageInit();
						this.billingSettings1.Visible = true;
					}
					else
					{
						if (name.Equals(this.butBillGenerate.Name))
						{
							this.showRpt(this.m_inAnalysisFlg, 1);
						}
						else
						{
							if (name.Equals(this.butBillRptMng.Name))
							{
								this.billingRptMng1.pageInit();
								this.billingRptMng1.Visible = true;
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
		public void showRpt(int selpage, int needinit)
		{
			switch (selpage)
			{
			case 1:
				if (needinit == 1)
				{
					this.billingRptPara1.pageInit(this);
				}
				else
				{
					this.billingRptPara1.pageInit_2(0);
				}
				this.billingRptPara1.Visible = true;
				this.billingRptShow1.Visible = false;
				this.billingRptPara1.resettime();
				this.m_inAnalysisFlg = 1;
				return;
			case 2:
				Program.IdleTimer_Pause(8);
				this.billingRptPara1.pageInit_2(1);
				this.billingRptPara1.Visible = true;
				this.billingRptShow1.Visible = false;
				if (this.m_inAnalysisFlg == 2)
				{
					return;
				}
				this.m_inAnalysisFlg = 2;
				this.m_oldAnalysisFlg = 2;
				this.m_pParaClass = new BillingRptParaClass(this.billingRptPara1);
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.prepare_dataProc), null);
				this.dTimer.Enabled = true;
				return;
			case 3:
				this.billingRptPara1.Visible = false;
				this.billingRptShow1.Visible = true;
				this.billingRptShow1.pageInit(this, this.m_pParaClass);
				DebugCenter.GetInstance().appendToFile("BillingAnalysis Finish.");
				Program.IdleTimer_Run(8);
				this.m_inAnalysisFlg = 4;
				return;
			case 4:
				this.billingRptPara1.Visible = false;
				this.billingRptShow1.Visible = true;
				this.m_inAnalysisFlg = 4;
				return;
			default:
				return;
			}
		}
		private void prepare_dataProc(object param)
		{
			switch (EcoLanguage.getLang())
			{
			case 0:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				break;
			case 1:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("de");
				break;
			case 2:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es");
				break;
			case 3:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr");
				break;
			case 4:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("it");
				break;
			case 5:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ja");
				break;
			case 6:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ko");
				break;
			case 7:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("pt");
				break;
			case 8:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru");
				break;
			case 9:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHS");
				break;
			case 10:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHT");
				break;
			default:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				break;
			}
			this.m_pParaClass.pageInit();
			this.m_inAnalysisFlg = 3;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Billing));
			this.panel1 = new Panel();
			this.flowLayoutPanelBillingPage = new FlowLayoutPanel();
			this.butBillSetting = new Button();
			this.butBillGenerate = new Button();
			this.butBillRptMng = new Button();
			this.panel2 = new Panel();
			this.billingRptPara1 = new BillingRptPara();
			this.billingSettings1 = new BillingSettings();
			this.billingRptMng1 = new BillingRptMng();
			this.billingRptShow1 = new BillingRptShow();
			this.panel1.SuspendLayout();
			this.flowLayoutPanelBillingPage.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.panel1.BackColor = Color.Gainsboro;
			this.panel1.Controls.Add(this.flowLayoutPanelBillingPage);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.flowLayoutPanelBillingPage.BackColor = Color.Gainsboro;
			this.flowLayoutPanelBillingPage.Controls.Add(this.butBillSetting);
			this.flowLayoutPanelBillingPage.Controls.Add(this.butBillGenerate);
			this.flowLayoutPanelBillingPage.Controls.Add(this.butBillRptMng);
			componentResourceManager.ApplyResources(this.flowLayoutPanelBillingPage, "flowLayoutPanelBillingPage");
			this.flowLayoutPanelBillingPage.Name = "flowLayoutPanelBillingPage";
			componentResourceManager.ApplyResources(this.butBillSetting, "butBillSetting");
			this.butBillSetting.MinimumSize = new Size(150, 27);
			this.butBillSetting.Name = "butBillSetting";
			this.butBillSetting.Tag = "BillSetting";
			this.butBillSetting.UseVisualStyleBackColor = true;
			this.butBillSetting.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butBillGenerate, "butBillGenerate");
			this.butBillGenerate.MinimumSize = new Size(150, 27);
			this.butBillGenerate.Name = "butBillGenerate";
			this.butBillGenerate.Tag = "BillRpt";
			this.butBillGenerate.UseVisualStyleBackColor = true;
			this.butBillGenerate.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butBillRptMng, "butBillRptMng");
			this.butBillRptMng.MinimumSize = new Size(150, 27);
			this.butBillRptMng.Name = "butBillRptMng";
			this.butBillRptMng.Tag = "BillMng";
			this.butBillRptMng.UseVisualStyleBackColor = true;
			this.butBillRptMng.Click += new System.EventHandler(this.comm_butClick);
			this.panel2.Controls.Add(this.billingRptPara1);
			this.panel2.Controls.Add(this.billingSettings1);
			this.panel2.Controls.Add(this.billingRptMng1);
			this.panel2.Controls.Add(this.billingRptShow1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.billingRptPara1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.billingRptPara1, "billingRptPara1");
			this.billingRptPara1.Name = "billingRptPara1";
			this.billingSettings1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.billingSettings1, "billingSettings1");
			this.billingSettings1.Name = "billingSettings1";
			this.billingRptMng1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.billingRptMng1, "billingRptMng1");
			this.billingRptMng1.Name = "billingRptMng1";
			this.billingRptShow1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.billingRptShow1, "billingRptShow1");
			this.billingRptShow1.Name = "billingRptShow1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "Billing";
			this.panel1.ResumeLayout(false);
			this.flowLayoutPanelBillingPage.ResumeLayout(false);
			this.flowLayoutPanelBillingPage.PerformLayout();
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
