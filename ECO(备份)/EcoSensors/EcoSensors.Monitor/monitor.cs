using CommonAPI.CultureTransfer;
using DBAccessAPI;
using Dispatcher;
using EcoSensors.DevManDCFloorGrid;
using EcoSensors.EnegManPage.DashBoard;
using EcoSensors.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using DG = EcoSensors.DevManDCFloorGrid;
namespace EcoSensors.Monitor
{
	public class monitor : Form
	{
		private delegate void boardDate(int from);
		private const int Interval_AfterChange = 10000;
		private const int Interval_NoChange = 1000;
		private IContainer components;
        private DG.DevManDCFloorGrid devManDCFloorGrid1;
		private TextBox tbCarbonfootprint;
		private Label lbCarbonfootprint;
		private TextBox tbTotalPower;
		private Label lbTotalPower;
		private Label lbconsumptionTitle;
		private MonMeter monMeterPUE;
		private MonMeter monMeterRTI;
		private MonMeter monMeterRCIHi;
		private MonMeter monMeterRCILo;
		private Label lbperformanceTitle;
		private TableLayoutPanel tableLayoutPanel1;
		private TableLayoutPanel tableLayoutPanel2;
		private TableLayoutPanel tblNormal;
		private TableLayoutPanel tableLayoutPanel4;
		private TableLayoutPanel tableLayoutPanel5;
		private TableLayoutPanel tableLayoutPanel6;
		private TableLayoutPanel tblISG;
		private TableLayoutPanel tableLayoutPanel10;
		private TextBox tbCarbonfootprintISG;
		private Label label4;
		private Label label1;
		private TableLayoutPanel tableLayoutPanel8;
		private TextBox tbTotalPowerISG;
		private Label label2;
		private TableLayoutPanel tableLayoutPanel9;
		private TextBox tbITotalPowerISG;
		private Label label3;
		public bool isfirsttimeshow = true;
		private TitleInfo title = new TitleInfo(true);
		private int boardFlg = 1;
		private int DB_FlgISG;
		private int DB_flgAtenPDU;
		private System.Timers.Timer boardTimer;
		private TableLayoutPanel tlpRackInfo;
		private DataSet m_TitleData_Info = new DataSet();
		private System.Collections.ArrayList m_allRacks;
		private int m_getdataflg;
		private double lasttime_PDUPower;
		public int FreshFlg_DashBoard
		{
			get
			{
				return this.boardFlg;
			}
			set
			{
				this.boardFlg = value;
			}
		}
		public int FreshFlg_ISGPower
		{
			set
			{
				this.DB_FlgISG = value;
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(monitor));
			this.lbconsumptionTitle = new Label();
			this.tbCarbonfootprint = new TextBox();
			this.lbCarbonfootprint = new Label();
			this.tbTotalPower = new TextBox();
			this.lbTotalPower = new Label();
			this.lbperformanceTitle = new Label();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.tableLayoutPanel2 = new TableLayoutPanel();
			this.monMeterRTI = new MonMeter();
			this.monMeterPUE = new MonMeter();
			this.monMeterRCILo = new MonMeter();
			this.monMeterRCIHi = new MonMeter();
			this.tblNormal = new TableLayoutPanel();
			this.tableLayoutPanel4 = new TableLayoutPanel();
			this.tableLayoutPanel5 = new TableLayoutPanel();
			this.tableLayoutPanel6 = new TableLayoutPanel();
            this.devManDCFloorGrid1 = new DG.DevManDCFloorGrid();
			this.tblISG = new TableLayoutPanel();
			this.tableLayoutPanel10 = new TableLayoutPanel();
			this.tbCarbonfootprintISG = new TextBox();
			this.label4 = new Label();
			this.label1 = new Label();
			this.tableLayoutPanel8 = new TableLayoutPanel();
			this.tbTotalPowerISG = new TextBox();
			this.label2 = new Label();
			this.tableLayoutPanel9 = new TableLayoutPanel();
			this.tbITotalPowerISG = new TextBox();
			this.label3 = new Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tblNormal.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.tblISG.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			base.SuspendLayout();
			this.lbconsumptionTitle.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.lbconsumptionTitle, "lbconsumptionTitle");
			this.lbconsumptionTitle.ForeColor = Color.FromArgb(20, 73, 160);
			this.lbconsumptionTitle.Name = "lbconsumptionTitle";
			componentResourceManager.ApplyResources(this.tbCarbonfootprint, "tbCarbonfootprint");
			this.tbCarbonfootprint.Name = "tbCarbonfootprint";
			this.tbCarbonfootprint.ReadOnly = true;
			componentResourceManager.ApplyResources(this.lbCarbonfootprint, "lbCarbonfootprint");
			this.lbCarbonfootprint.ForeColor = Color.Black;
			this.lbCarbonfootprint.Name = "lbCarbonfootprint";
			componentResourceManager.ApplyResources(this.tbTotalPower, "tbTotalPower");
			this.tbTotalPower.Name = "tbTotalPower";
			this.tbTotalPower.ReadOnly = true;
			componentResourceManager.ApplyResources(this.lbTotalPower, "lbTotalPower");
			this.lbTotalPower.ForeColor = Color.Black;
			this.lbTotalPower.Name = "lbTotalPower";
			this.lbperformanceTitle.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.lbperformanceTitle, "lbperformanceTitle");
			this.lbperformanceTitle.ForeColor = Color.FromArgb(20, 73, 160);
			this.lbperformanceTitle.Name = "lbperformanceTitle";
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.BackColor = Color.Gainsboro;
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbperformanceTitle, 0, 0);
			this.tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
			this.tableLayoutPanel1.MaximumSize = new Size(534, 182);
			this.tableLayoutPanel1.MinimumSize = new Size(534, 182);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.monMeterRTI, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.monMeterPUE, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.monMeterRCILo, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.monMeterRCIHi, 2, 0);
			this.tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
			this.tableLayoutPanel2.MaximumSize = new Size(530, 144);
			this.tableLayoutPanel2.MinimumSize = new Size(530, 144);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.monMeterRTI.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.monMeterRTI, "monMeterRTI");
			this.monMeterRTI.Name = "monMeterRTI";
			this.monMeterPUE.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.monMeterPUE, "monMeterPUE");
			this.monMeterPUE.Name = "monMeterPUE";
			this.monMeterRCILo.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.monMeterRCILo, "monMeterRCILo");
			this.monMeterRCILo.Name = "monMeterRCILo";
			this.monMeterRCIHi.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.monMeterRCIHi, "monMeterRCIHi");
			this.monMeterRCIHi.Name = "monMeterRCIHi";
			this.tblNormal.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.tblNormal, "tblNormal");
			this.tblNormal.Controls.Add(this.lbconsumptionTitle, 0, 0);
			this.tblNormal.Controls.Add(this.tableLayoutPanel4, 0, 1);
			this.tblNormal.Controls.Add(this.tableLayoutPanel5, 0, 2);
			this.tblNormal.Name = "tblNormal";
			componentResourceManager.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.tbTotalPower, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.lbTotalPower, 0, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			componentResourceManager.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.tbCarbonfootprint, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.lbCarbonfootprint, 0, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel6.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
			this.tableLayoutPanel6.Controls.Add(this.devManDCFloorGrid1, 0, 0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.devManDCFloorGrid1.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.devManDCFloorGrid1, "devManDCFloorGrid1");
			this.devManDCFloorGrid1.Name = "devManDCFloorGrid1";
			this.tblISG.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.tblISG, "tblISG");
			this.tblISG.Controls.Add(this.tableLayoutPanel10, 0, 3);
			this.tblISG.Controls.Add(this.label1, 0, 0);
			this.tblISG.Controls.Add(this.tableLayoutPanel8, 0, 1);
			this.tblISG.Controls.Add(this.tableLayoutPanel9, 0, 2);
			this.tblISG.Name = "tblISG";
			componentResourceManager.ApplyResources(this.tableLayoutPanel10, "tableLayoutPanel10");
			this.tableLayoutPanel10.Controls.Add(this.tbCarbonfootprintISG, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			componentResourceManager.ApplyResources(this.tbCarbonfootprintISG, "tbCarbonfootprintISG");
			this.tbCarbonfootprintISG.Name = "tbCarbonfootprintISG";
			this.tbCarbonfootprintISG.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = Color.Black;
			this.label4.Name = "label4";
			this.label1.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.FromArgb(20, 73, 160);
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
			this.tableLayoutPanel8.Controls.Add(this.tbTotalPowerISG, 1, 0);
			this.tableLayoutPanel8.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			componentResourceManager.ApplyResources(this.tbTotalPowerISG, "tbTotalPowerISG");
			this.tbTotalPowerISG.Name = "tbTotalPowerISG";
			this.tbTotalPowerISG.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = Color.Black;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
			this.tableLayoutPanel9.Controls.Add(this.tbITotalPowerISG, 1, 0);
			this.tableLayoutPanel9.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			componentResourceManager.ApplyResources(this.tbITotalPowerISG, "tbITotalPowerISG");
			this.tbITotalPowerISG.Name = "tbITotalPowerISG";
			this.tbITotalPowerISG.ReadOnly = true;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = Color.Black;
			this.label3.Name = "label3";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.White;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tableLayoutPanel6);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.tblNormal);
			base.Controls.Add(this.tblISG);
			base.Name = "monitor";
			base.FormClosing += new FormClosingEventHandler(this.monitor_FormClosing);
			base.SizeChanged += new System.EventHandler(this.monitor_SizeChanged);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tblNormal.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tblISG.ResumeLayout(false);
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			base.ResumeLayout(false);
		}
		public monitor()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_monitorCtrl = this;
			this.boardTimer = new System.Timers.Timer();
			this.boardTimer.Elapsed += new ElapsedEventHandler(this.boardTimerEvent);
			this.boardTimer.Interval = 10000.0;
			this.boardTimer.AutoReset = true;
			this.boardTimer.Enabled = false;
			this.FreshFlg_DashBoard = 1;
			this.tbCarbonfootprint.Text = "";
			this.tbCarbonfootprintISG.Text = "";
		}
		public void pageInit(int initdata)
		{
			this.monMeterPUE.init_lable(1, "PUE", "", "", "", Resources.meter_PUE);
			this.monMeterRTI.init_lable(2, "RTI", "Bypass", "Recirc.", "", Resources.meter_RTI);
			this.monMeterRCIHi.init_lable(3, "RCI Hi", "", "", "", Resources.meter_RCI);
			this.monMeterRCILo.init_lable(3, "RCI Lo", "", "", "", Resources.meter_RCI);
			this.monMeterRTI.init_valueErr();
			this.monMeterRCIHi.init_valueErr();
			this.monMeterRCILo.init_valueErr();
			this.monMeterPUE.init_valueErr();
			this.boardDataInit();
			this.boardTimer.Interval = 5000.0;
			this.m_getdataflg = 0;
		}
		public void starBoardTimer()
		{
			this.boardTimer.Enabled = true;
		}
		public void endBoardTimer()
		{
			this.boardTimer.Enabled = false;
		}
		public void resetTimer()
		{
			if (this.boardTimer.Interval == 10000.0)
			{
				this.boardTimer.Interval = 1000.0;
			}
		}
		private int boardDataInit()
		{
			int result = 2;
			bool flag = false;
			switch (this.FreshFlg_DashBoard)
			{
			case 1:
				if (!EcoGlobalVar.gl_isProcessThreadRuning())
				{
					EcoGlobalVar.gl_StartProcessfThread(false);
					flag = true;
				}
				this.devManDCFloorGrid1.Hide();
				this.tlpRackInfo = this.devManDCFloorGrid1.getRackCtrl();
				this.tlpRackInfo.SuspendLayout();
				this.m_allRacks = ClientAPI.getRackInfo();
				foreach (RackInfo rackInfo in this.m_allRacks)
				{
					int startPoint_X = rackInfo.StartPoint_X;
					int startPoint_Y = rackInfo.StartPoint_Y;
					int endPoint_X = rackInfo.EndPoint_X;
					int arg_9F_0 = rackInfo.EndPoint_Y;
					int num;
					if (startPoint_X == endPoint_X)
					{
						num = 0;
					}
					else
					{
						num = 1;
					}
					Label label = new Label();
					label.Tag = rackInfo.RackID;
					label.ForeColor = Color.Silver;
					label.BackColor = Color.Silver;
					label.BorderStyle = BorderStyle.FixedSingle;
					label.Dock = DockStyle.Fill;
					label.Margin = new Padding(0);
					label.MouseHover += new System.EventHandler(this.Rack_MouseHover);
					label.MouseLeave += new System.EventHandler(this.Rack_MouseLeave);
					this.tlpRackInfo.Controls.Add(label, startPoint_Y, startPoint_X);
					if (num == 0)
					{
						this.tlpRackInfo.SetColumnSpan(label, 2);
						this.tlpRackInfo.SetRowSpan(label, 1);
					}
					else
					{
						if (num == 1)
						{
							this.tlpRackInfo.SetColumnSpan(label, 1);
							this.tlpRackInfo.SetRowSpan(label, 2);
						}
					}
				}
				this.tlpRackInfo.ResumeLayout();
				this.m_TitleData_Info = ClientAPI.getDataSet(0);
				this.FreshFlg_DashBoard = -1;
				this.devManDCFloorGrid1.Show();
				if (flag)
				{
					EcoGlobalVar.gl_StopProcessfThread();
				}
				return 0;
			case 2:
				this.m_allRacks = ClientAPI.getRackInfo();
				this.m_TitleData_Info = ClientAPI.getDataSet(0);
				this.FreshFlg_DashBoard = -1;
				return 0;
			default:
				return result;
			}
		}
		private void Rack_MouseHover(object sender, System.EventArgs e)
		{
			Screen[] allScreens = Screen.AllScreens;
			int width = allScreens[0].Bounds.Width;
			int height = allScreens[0].Bounds.Height;
			int x = Control.MousePosition.X - 5;
			int y = Control.MousePosition.Y - 5;
			this.Cursor = Cursors.WaitCursor;
			if (width - Control.MousePosition.X < this.title.Size.Width)
			{
				x = Control.MousePosition.X - this.title.Size.Width;
			}
			if (height - Control.MousePosition.Y < this.title.Size.Height)
			{
				y = Control.MousePosition.Y - this.title.Size.Height;
			}
			this.title.Location = new Point(x, y);
			long rackID = (long)((Label)sender).Tag;
			string devidsinRack = this.getDevidsinRack(this.m_allRacks, rackID);
			this.title.Set(rackID, devidsinRack, this.m_TitleData_Info, "0:0");
			this.title.Show();
			this.Cursor = Cursors.Arrow;
		}
		private void Rack_MouseLeave(object sender, System.EventArgs e)
		{
			if (Control.MousePosition.X >= this.title.Location.X && Control.MousePosition.X <= this.title.Location.X + this.title.Size.Width && Control.MousePosition.Y >= this.title.Location.Y && Control.MousePosition.Y <= this.title.Location.Y + this.title.Size.Height)
			{
				return;
			}
			this.title.Hide();
			this.Cursor = Cursors.Arrow;
		}
		private void boardTimerEvent(object source, ElapsedEventArgs e)
		{
			this.boardTimer.Interval = 30000.0;
			if (base.InvokeRequired)
			{
				base.Invoke(new monitor.boardDate(this.boardTimeDate), new object[]
				{
					this.m_getdataflg
				});
			}
			else
			{
				this.boardTimeDate(this.m_getdataflg);
			}
			this.m_getdataflg = 2;
		}
		private void boardTimeDate(int from)
		{
			int num = 0;
			System.DateTime arg_07_0 = System.DateTime.Now;
			if (this.boardDataInit() == 0)
			{
				from = 0;
			}
			else
			{
				num = ClientAPI.updateDataSet(ref this.m_TitleData_Info, from);
			}
			double interval = 1000.0;
			if (num != boardDataUtil.PDUData_FreshFLG_NO)
			{
				System.Collections.Generic.Dictionary<long, Color> dictionary = new System.Collections.Generic.Dictionary<long, Color>();
				dictionary = boardDataUtil.sensorsThresholdStatus(this.m_allRacks, this.m_TitleData_Info);
				foreach (Control control in this.tlpRackInfo.Controls)
				{
					long key = (long)control.Tag;
					if (dictionary.ContainsKey(key))
					{
						control.BackColor = dictionary[key];
					}
				}
				interval = 10000.0;
			}
			this.refresh_meters(num);
			if (this.FreshFlg_DashBoard == 1)
			{
				this.boardTimer.Interval = 1000.0;
				return;
			}
			this.boardTimer.Interval = interval;
		}
		private void monitor_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			base.Hide();
			this.endBoardTimer();
		}
		private void monitor_SizeChanged(object sender, System.EventArgs e)
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				base.Hide();
				this.endBoardTimer();
			}
		}
		private void refresh_meters(int pduPowerflg)
		{
			this.DB_FlgISG = AppData.getDB_FlgISG();
			this.DB_flgAtenPDU = AppData.getDB_flgAtenPDU();
			if (this.DB_FlgISG == 1)
			{
				this.tbTotalPowerISG.Text = "";
				this.tbITotalPowerISG.Text = "";
				this.tbCarbonfootprintISG.Text = "";
				this.tblISG.Visible = true;
				this.tblNormal.Visible = false;
			}
			else
			{
				this.tbTotalPower.Text = "";
				this.tbCarbonfootprint.Text = "";
				this.tblISG.Visible = false;
				this.tblNormal.Visible = true;
			}
			if (pduPowerflg != boardDataUtil.PDUData_FreshFLG_NO)
			{
				string info = this.getInfo(this.m_allRacks, this.m_TitleData_Info, ref this.lasttime_PDUPower);
				if (info == null)
				{
					this.monMeterRTI.init_valueErr();
				}
				else
				{
					double v = System.Convert.ToDouble(info);
					this.monMeterRTI.init_value(v);
				}
				DataRow[] array = this.m_TitleData_Info.Tables[1].Select(string.Concat(new object[]
				{
					"sensor_location=",
					0,
					" and temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"'"
				}), "temperature asc");
				double num = 27.0;
				double num2 = 32.0;
				double num3 = 18.0;
				double num4 = 15.0;
				double num5 = 0.0;
				double num6 = 0.0;
				int num7 = 0;
				double v2;
				double v3;
				if (array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						double num8 = ecoConvert.f2d(array[i]["temperature"]);
						num7++;
						if (num8 > num)
						{
							num5 += num8 - num;
						}
						if (num3 > num8)
						{
							num6 += num3 - num8;
						}
					}
					v2 = (1.0 - num5 / ((num2 - num) * (double)num7)) * 100.0;
					v3 = (1.0 - num6 / ((num3 - num4) * (double)num7)) * 100.0;
				}
				else
				{
					v2 = 0.0;
					v3 = 0.0;
				}
				if (num7 > 0)
				{
					this.monMeterRCIHi.init_value(v2);
					this.monMeterRCILo.init_value(v3);
				}
				else
				{
					this.monMeterRCIHi.init_valueErr();
					this.monMeterRCILo.init_valueErr();
				}
			}
			double num9 = this.lasttime_PDUPower;
			double num10 = this.lasttime_PDUPower;
			switch (this.DB_FlgISG)
			{
			case 0:
				num9 = this.lasttime_PDUPower;
				this.tbTotalPower.Text = (num9 / 1000.0).ToString("F3");
				break;
			case 1:
				num10 = AppData.getPUE(0);
				num9 = AppData.getPUE(1);
				if (this.DB_flgAtenPDU == 1)
				{
					num10 += this.lasttime_PDUPower;
					num9 += this.lasttime_PDUPower;
				}
				this.tbTotalPowerISG.Text = (num9 / 1000.0).ToString("F3");
				this.tbITotalPowerISG.Text = (num10 / 1000.0).ToString("F3");
				break;
			}
			float co2kg = EcoGlobalVar.co2kg;
			if (co2kg < 0f)
			{
				this.tbCarbonfootprint.Text = "";
				this.tbCarbonfootprintISG.Text = "";
			}
			else
			{
				this.tbCarbonfootprint.Text = (num9 / 1000.0 * (double)co2kg).ToString("F3");
				this.tbCarbonfootprintISG.Text = this.tbCarbonfootprint.Text;
			}
			switch (this.DB_FlgISG)
			{
			case 0:
				this.monMeterPUE.init_valueErr();
				return;
			case 1:
			{
				double num11 = -1.0;
				try
				{
					num11 = num9 / num10;
				}
				catch (System.Exception)
				{
				}
				if (num11 < 0.0 || double.IsInfinity(num11) || double.IsNaN(num11))
				{
					this.monMeterPUE.init_valueErr();
					return;
				}
				this.monMeterPUE.init_value(num11);
				return;
			}
			default:
				return;
			}
		}
		private string getInfo(System.Collections.ArrayList allRacks, DataSet ds, ref double rettotal_power)
		{
			System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary = boardDataUtil.RackCal_infor(allRacks, ds);
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current in dictionary)
			{
				RackStatusOne value = current.Value;
				num3 += value.Power;
			}
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current2 in dictionary)
			{
				RackStatusOne value = current2.Value;
				if (value.IntakeSSnum == 0 || value.ExhaustSSnum == 0 || value.FloorSSnum == 0 || value.TEquipk_avg <= 0.0 || value.TFloor_avg <= 0.0 || value.TFloor_avg - value.TIntake_diff <= 0.0 || value.Power == 0.0)
				{
					value.Power = 0.0;
				}
				else
				{
					num += (value.TFloor_avg - value.TIntake_diff) * (value.Power / num3);
					num2 += value.TEquipk * (value.Power / num3);
					num4 += value.VEquipk;
				}
			}
			rettotal_power = num3;
			if (num != 0.0 && num2 != 0.0)
			{
				double num5 = num / num2;
				return (num5 * 100.0).ToString("F2");
			}
			return null;
		}
		private string getDevidsinRack(System.Collections.ArrayList allRacks, long rackID)
		{
			foreach (RackInfo rackInfo in allRacks)
			{
				if (rackInfo.RackID == rackID)
				{
					return rackInfo.DeviceInfo;
				}
			}
			return "";
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			return true;
		}
	}
}
