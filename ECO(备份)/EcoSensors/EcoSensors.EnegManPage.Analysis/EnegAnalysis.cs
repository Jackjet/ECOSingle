using CommonAPI;
using DBAccessAPI;
using Dispatcher;
using EcoSensors.Common;
using EcoSensors.EnegManPage.DashBoard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.Analysis
{
	public class EnegAnalysis : UserControl
	{
		private delegate void AnalysisRefresh();
		private const int Interval_Timer = 1000;
		private int m_inAnalysisFlg = 1;
		private int m_oldAnalysisFlg;
		private System.Collections.Generic.List<object> m_retDataSet;
		private EGenRptParaClass m_pParaClass;
		private System.Timers.Timer dTimer;
		private IContainer components;
		private FlowLayoutPanel fLyPlEnegAnalysis;
		private Button butGenReport;
		private Button butReportMng;
		private EGenRptPara genRptPara1;
		private EGenRptShow EgenRptShow1;
		private ERptManage rptManage1;
		private Panel panel2;
		public EnegAnalysis()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_EnegAnalysisCtrl = this;
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
				this.comm_butClick(this.butGenReport, null);
				return;
			case 1:
				this.comm_butClick(this.butReportMng, null);
				return;
			default:
				return;
			}
		}
		private void theTimeout(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new EnegAnalysis.AnalysisRefresh(this.AnalysisTimerRefresh));
				return;
			}
			this.AnalysisTimerRefresh();
		}
		private void AnalysisTimerRefresh()
		{
			if (this.m_oldAnalysisFlg == 2 && this.m_inAnalysisFlg == 3)
			{
				this.dTimer.Enabled = false;
				this.m_oldAnalysisFlg = 3;
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
			this.genRptPara1.Visible = false;
			this.EgenRptShow1.Visible = false;
			this.rptManage1.Visible = false;
			foreach (Control control in this.fLyPlEnegAnalysis.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butGenReport.Name))
					{
						this.showRpt(this.m_inAnalysisFlg, 1);
					}
					else
					{
						if (name.Equals(this.butReportMng.Name))
						{
							this.rptManage1.pageInit();
							this.rptManage1.Visible = true;
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
					this.genRptPara1.pageInit_1(this);
				}
				else
				{
					this.genRptPara1.pageInit_2(0);
				}
				this.genRptPara1.Visible = true;
				this.EgenRptShow1.Visible = false;
				this.genRptPara1.resettime();
				this.m_inAnalysisFlg = 1;
				return;
			case 2:
				Program.IdleTimer_Pause(2);
				this.genRptPara1.pageInit_2(1);
				this.genRptPara1.Visible = true;
				this.EgenRptShow1.Visible = false;
				if (this.m_inAnalysisFlg == 2)
				{
					return;
				}
				this.m_pParaClass = new EGenRptParaClass();
				this.m_pParaClass.pageInit(this.genRptPara1);
				this.m_inAnalysisFlg = 2;
				this.m_oldAnalysisFlg = 2;
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.prepare_dataProc), this.m_pParaClass);
				this.dTimer.Enabled = true;
				return;
			case 3:
				this.genRptPara1.Visible = false;
				this.EgenRptShow1.Visible = true;
				this.EgenRptShow1.pageInit(this, this.m_pParaClass, this.m_retDataSet);
				DebugCenter.GetInstance().appendToFile("PowerAnalysis Finish.");
				Program.IdleTimer_Run(2);
				this.m_inAnalysisFlg = 4;
				return;
			case 4:
				this.genRptPara1.Visible = false;
				this.EgenRptShow1.Visible = true;
				this.m_inAnalysisFlg = 4;
				return;
			default:
				return;
			}
		}
		private void prepare_dataProc(object param)
		{
			System.DateTime now = System.DateTime.Now;
			EGenRptParaClass eGenRptParaClass = param as EGenRptParaClass;
			this.m_retDataSet = new System.Collections.Generic.List<object>();
			DataTable[] array = new DataTable[]
			{
				null,
				null,
				null,
				null
			};
			DataTable[] array2 = new DataTable[]
			{
				null,
				null,
				null,
				null
			};
			DataTable[] array3 = new DataTable[]
			{
				null,
				null,
				null,
				null
			};
			DataTable[] array4 = new DataTable[]
			{
				null,
				null,
				null,
				null
			};
			System.Collections.Generic.Dictionary<long, RackStatusOne> item = null;
			double[] array5 = new double[4];
			double[] array6 = new double[4];
			double[] array7 = new double[4];
			double[] array8 = new double[4];
			array5[0] = -1.0;
			array5[1] = -1.0;
			array5[2] = -1.0;
			array5[3] = -1.0;
			array6[0] = -1.0;
			array6[1] = -1.0;
			array6[2] = -1.0;
			array6[3] = -1.0;
			array7[0] = -1.0;
			array7[1] = -1.0;
			array7[2] = -1.0;
			array7[3] = -1.0;
			array8[0] = -1.0;
			array8[1] = -1.0;
			array8[2] = -1.0;
			array8[3] = -1.0;
			System.Collections.ArrayList gppara_list = eGenRptParaClass.gppara_list;
			string[] array9 = new string[4];
			string[] array10 = new string[4];
			string[] array11 = new string[4];
			string[] array12 = new string[4];
			for (int i = 0; i < 4; i++)
			{
				array9[i] = null;
				array11[i] = null;
				array12[i] = null;
			}
			for (int j = 0; j < gppara_list.Count; j++)
			{
				string[] array13 = gppara_list[j].ToString().Split(new char[]
				{
					'|'
				});
				array9[j] = array13[EGenRptShow.AnalysisIndex_devIDs];
				array10[j] = array13[EGenRptShow.AnalysisIndex_invaliddevIDs];
				array12[j] = array13[EGenRptShow.AnalysisIndex_gpTP];
				array11[j] = array13[EGenRptShow.AnalysisIndex_portIDs];
			}
			if (eGenRptParaClass.chkchart1_Checked() || eGenRptParaClass.chkchart2_Checked() || eGenRptParaClass.chkchart3_Checked() || eGenRptParaClass.chkchart4_Checked() || eGenRptParaClass.chkchart5_Checked() || eGenRptParaClass.chkchart7_Checked() || eGenRptParaClass.chkchart9_Checked())
			{
				for (int k = 0; k < gppara_list.Count; k++)
				{
					string[] array14 = gppara_list[k].ToString().Split(new char[]
					{
						'|'
					});
					if (array[k] == null)
					{
						System.DateTime now2 = System.DateTime.Now;
						DebugCenter.GetInstance().appendToFile("PowerAnalysis - PD: GP:" + array14[EGenRptShow.AnalysisIndex_gpNM] + " SSS.");
						array[k] = DBTools.GetChart1Data(eGenRptParaClass.strBegin, eGenRptParaClass.strEnd, array9[k], array11[k], eGenRptParaClass.groupby, array12[k], eGenRptParaClass.dblibnameDev, eGenRptParaClass.dblibnamePort);
						System.DateTime now3 = System.DateTime.Now;
						System.TimeSpan timeSpan = now3 - now2;
						DebugCenter.GetInstance().appendToFile("PowerAnalysis - PD: GP:" + array14[EGenRptShow.AnalysisIndex_gpNM] + " EEE. sp(s) = " + timeSpan.TotalSeconds.ToString("F3"));
						DBProcessUtil.TransferRatio(array[k], "power_consumption", 10000.0);
						array5[k] = timeSpan.TotalSeconds;
					}
				}
			}
			if (eGenRptParaClass.chkchart2_Checked() || eGenRptParaClass.chkchart7_Checked())
			{
				for (int l = 0; l < gppara_list.Count; l++)
				{
					string[] array15 = gppara_list[l].ToString().Split(new char[]
					{
						'|'
					});
					if (array2[l] == null)
					{
						System.DateTime now4 = System.DateTime.Now;
						DebugCenter.GetInstance().appendToFile("PowerAnalysis - POWER: GP:" + array15[EGenRptShow.AnalysisIndex_gpNM] + " SSS.");
						array2[l] = DBTools.GetChart2Data(eGenRptParaClass.strBegin, eGenRptParaClass.strEnd, array9[l], array11[l], eGenRptParaClass.groupby, array12[l]);
						System.DateTime now5 = System.DateTime.Now;
						System.TimeSpan timeSpan2 = now5 - now4;
						DebugCenter.GetInstance().appendToFile("PowerAnalysis - POWER: GP:" + array15[EGenRptShow.AnalysisIndex_gpNM] + " EEE. sp(s) = " + timeSpan2.TotalSeconds.ToString("F3"));
						DBProcessUtil.TransferRatio(array2[l], "power", 10000.0);
						array6[l] = timeSpan2.TotalSeconds;
					}
				}
			}
			if (eGenRptParaClass.chkchart6_Checked())
			{
				System.DateTime now6 = System.DateTime.Now;
				DebugCenter.GetInstance().appendToFile("PowerAnalysis - Energy Saving: SSS.");
				System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
				string text = "";
				foreach (RackInfo rackInfo in allRack_NoEmpty)
				{
					text = text + rackInfo.DeviceInfo + ",";
				}
				if (text.Length > 1)
				{
					text = text.Substring(0, text.Length - 1);
				}
				else
				{
					text = "0";
				}
				DataSet dataSet = ClientAPI.getDataSet(0);
				item = boardDataUtil.RackCal_infor(allRack_NoEmpty, dataSet);
				System.DateTime now7 = System.DateTime.Now;
				System.TimeSpan timeSpan3 = now7 - now6;
				DebugCenter.GetInstance().appendToFile("PowerAnalysis - Energy Saving: EEE. sp(s) = " + timeSpan3.TotalSeconds.ToString("F3"));
			}
			if (eGenRptParaClass.chkchart8_Checked())
			{
				for (int m = 0; m < gppara_list.Count; m++)
				{
					string[] array16 = gppara_list[m].ToString().Split(new char[]
					{
						'|'
					});
					System.DateTime now8 = System.DateTime.Now;
					DebugCenter.GetInstance().appendToFile("PowerAnalysis - InventoryList PD: GP:" + array16[EGenRptShow.AnalysisIndex_gpNM] + " SSS.");
					array4[m] = DBTools.GetOutLetPDAndName(array12[m], eGenRptParaClass.dblibnamePort, eGenRptParaClass.dblibnameDev, eGenRptParaClass.strBegin, eGenRptParaClass.strEnd, array11[m], array10[m]);
					DBProcessUtil.TransferRatio(array4[m], "power_consumption", 10000.0);
					System.DateTime now9 = System.DateTime.Now;
					System.TimeSpan timeSpan4 = now9 - now8;
					DebugCenter.GetInstance().appendToFile("PowerAnalysis - InventoryList PD: GP:" + array16[EGenRptShow.AnalysisIndex_gpNM] + " EEE. sp(s) = " + timeSpan4.TotalSeconds.ToString("F3"));
					System.DateTime now10 = System.DateTime.Now;
					DebugCenter.GetInstance().appendToFile("PowerAnalysis - InventoryList POWER: GP:" + array16[EGenRptShow.AnalysisIndex_gpNM] + " SSS.");
					array3[m] = DBTools.GetOutLetPowerAndName(array12[m], eGenRptParaClass.strBegin, eGenRptParaClass.strEnd, array11[m], array10[m]);
					DBProcessUtil.TransferRatio(array3[m], "power", 10000.0);
					System.DateTime now11 = System.DateTime.Now;
					System.TimeSpan timeSpan5 = now11 - now10;
					DebugCenter.GetInstance().appendToFile("PowerAnalysis - InventoryList POWER: GP:" + array16[EGenRptShow.AnalysisIndex_gpNM] + " EEE. sp(s) = " + timeSpan5.TotalSeconds.ToString("F3"));
					array7[m] = timeSpan4.TotalSeconds;
					array8[m] = timeSpan5.TotalSeconds;
				}
			}
			this.m_retDataSet.Add(array[0]);
			this.m_retDataSet.Add(array[1]);
			this.m_retDataSet.Add(array[2]);
			this.m_retDataSet.Add(array[3]);
			this.m_retDataSet.Add(array2[0]);
			this.m_retDataSet.Add(array2[1]);
			this.m_retDataSet.Add(array2[2]);
			this.m_retDataSet.Add(array2[3]);
			this.m_retDataSet.Add(array3[0]);
			this.m_retDataSet.Add(array3[1]);
			this.m_retDataSet.Add(array3[2]);
			this.m_retDataSet.Add(array3[3]);
			this.m_retDataSet.Add(array4[0]);
			this.m_retDataSet.Add(array4[1]);
			this.m_retDataSet.Add(array4[2]);
			this.m_retDataSet.Add(array4[3]);
			this.m_retDataSet.Add(item);
			this.m_inAnalysisFlg = 3;
			System.DateTime now12 = System.DateTime.Now;
			System.TimeSpan timeSpan6 = now12 - now;
			double num = 0.0;
			for (int n = 0; n < gppara_list.Count; n++)
			{
				string[] array17 = gppara_list[n].ToString().Split(new char[]
				{
					'|'
				});
				string str = "PowerAnalysis prepare_dataProc";
				if (array5[n] >= 0.0)
				{
					str = str + " | PD: " + array5[n].ToString("F3");
					num += array5[n];
				}
				if (array6[n] >= 0.0)
				{
					str = str + " | POWER: " + array6[n].ToString("F3");
					num += array6[n];
				}
				if (array7[n] >= 0.0)
				{
					str = str + " | InventoryPD: " + array7[n].ToString("F3");
					num += array7[n];
				}
				if (array8[n] >= 0.0)
				{
					str = str + " | InventoryPOWER: " + array8[n].ToString("F3");
					num += array8[n];
				}
				DebugCenter.GetInstance().appendToFile(str + " | " + array17[EGenRptShow.AnalysisIndex_gpNM]);
			}
			DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
			{
				"PowerAnalysis - Threads: ",
				MultiThreadQuery.getQueryThreads(),
				" prepare_dataProc(s) | Total: ",
				timeSpan6.TotalSeconds.ToString("F3"),
				" | DATABASE: ",
				num.ToString("F3"),
				" | Other: ",
				(timeSpan6.TotalSeconds - num).ToString("F3")
			}));
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EnegAnalysis));
			this.fLyPlEnegAnalysis = new FlowLayoutPanel();
			this.butGenReport = new Button();
			this.butReportMng = new Button();
			this.panel2 = new Panel();
			this.rptManage1 = new ERptManage();
			this.genRptPara1 = new EGenRptPara();
			this.EgenRptShow1 = new EGenRptShow();
			this.fLyPlEnegAnalysis.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.fLyPlEnegAnalysis.BackColor = Color.Gainsboro;
			this.fLyPlEnegAnalysis.Controls.Add(this.butGenReport);
			this.fLyPlEnegAnalysis.Controls.Add(this.butReportMng);
			componentResourceManager.ApplyResources(this.fLyPlEnegAnalysis, "fLyPlEnegAnalysis");
			this.fLyPlEnegAnalysis.Name = "fLyPlEnegAnalysis";
			componentResourceManager.ApplyResources(this.butGenReport, "butGenReport");
			this.butGenReport.MinimumSize = new Size(150, 27);
			this.butGenReport.Name = "butGenReport";
			this.butGenReport.Tag = "Tag_genreport";
			this.butGenReport.UseVisualStyleBackColor = true;
			this.butGenReport.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butReportMng, "butReportMng");
			this.butReportMng.MinimumSize = new Size(150, 27);
			this.butReportMng.Name = "butReportMng";
			this.butReportMng.Tag = "Tag_reportmng";
			this.butReportMng.UseVisualStyleBackColor = true;
			this.butReportMng.Click += new System.EventHandler(this.comm_butClick);
			this.panel2.Controls.Add(this.rptManage1);
			this.panel2.Controls.Add(this.genRptPara1);
			this.panel2.Controls.Add(this.EgenRptShow1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.rptManage1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.rptManage1, "rptManage1");
			this.rptManage1.Name = "rptManage1";
			this.genRptPara1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.genRptPara1, "genRptPara1");
			this.genRptPara1.Name = "genRptPara1";
			this.EgenRptShow1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.EgenRptShow1, "EgenRptShow1");
			this.EgenRptShow1.Name = "EgenRptShow1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.fLyPlEnegAnalysis);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "EnegAnalysis";
			this.fLyPlEnegAnalysis.ResumeLayout(false);
			this.fLyPlEnegAnalysis.PerformLayout();
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
