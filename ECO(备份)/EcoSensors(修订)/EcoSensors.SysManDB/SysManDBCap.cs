using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace EcoSensors.SysManDB
{
	public class SysManDBCap : UserControl
	{
		private delegate void boardDate();
		private IContainer components;
		private Chart chtpie;
		private Label labFreeSpace;
		private Label label4;
		private Label labcapacity;
		private Label labdb;
		private ComboBox cbdbtype;
		private System.Timers.Timer boardTimer_show;
		private System.Timers.Timer boardTimer_getdata;
		private long[] m_dbsize;
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
			ChartArea chartArea = new ChartArea();
			Legend legend = new Legend();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManDBCap));
			Series series = new Series();
			this.chtpie = new Chart();
			this.labFreeSpace = new Label();
			this.label4 = new Label();
			this.labcapacity = new Label();
			this.labdb = new Label();
			this.cbdbtype = new ComboBox();
			((ISupportInitialize)this.chtpie).BeginInit();
			base.SuspendLayout();
			this.chtpie.BackColor = Color.WhiteSmoke;
			chartArea.BackColor = Color.WhiteSmoke;
			chartArea.Name = "ChartArea1";
			this.chtpie.ChartAreas.Add(chartArea);
			legend.BackColor = Color.WhiteSmoke;
			legend.Name = "Legend1";
			this.chtpie.Legends.Add(legend);
			componentResourceManager.ApplyResources(this.chtpie, "chtpie");
			this.chtpie.Name = "chtpie";
			series.ChartArea = "ChartArea1";
			series.ChartType = SeriesChartType.Pie;
			series.Legend = "Legend1";
			series.Name = "Series1";
			this.chtpie.Series.Add(series);
			componentResourceManager.ApplyResources(this.labFreeSpace, "labFreeSpace");
			this.labFreeSpace.Name = "labFreeSpace";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.labcapacity, "labcapacity");
			this.labcapacity.Name = "labcapacity";
			componentResourceManager.ApplyResources(this.labdb, "labdb");
			this.labdb.Name = "labdb";
			this.cbdbtype.FormattingEnabled = true;
			this.cbdbtype.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbdbtype.Items"),
				componentResourceManager.GetString("cbdbtype.Items1")
			});
			componentResourceManager.ApplyResources(this.cbdbtype, "cbdbtype");
			this.cbdbtype.Name = "cbdbtype";
			this.cbdbtype.SelectedIndexChanged += new System.EventHandler(this.cbdbtype_SelectedIndexChanged);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.cbdbtype);
			base.Controls.Add(this.chtpie);
			base.Controls.Add(this.labFreeSpace);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.labcapacity);
			base.Controls.Add(this.labdb);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysManDBCap";
			((ISupportInitialize)this.chtpie).EndInit();
			base.ResumeLayout(false);
		}
		public SysManDBCap()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_SysDBCapCtrl = this;
			this.boardTimer_show = new System.Timers.Timer();
			this.boardTimer_show.Elapsed += new ElapsedEventHandler(this.DbCapacityEvent_show);
			this.boardTimer_show.Interval = 60000.0;
			this.boardTimer_show.AutoReset = true;
			this.boardTimer_show.Enabled = false;
			this.boardTimer_getdata = new System.Timers.Timer();
			this.boardTimer_getdata.Elapsed += new ElapsedEventHandler(this.DbCapacityEvent_getData);
			this.boardTimer_getdata.Interval = 60000.0;
			this.boardTimer_getdata.AutoReset = true;
			this.boardTimer_getdata.Enabled = false;
		}
		public void pageInit()
		{
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_loading, new string[0]), null, new progressPopup.ProcessInThread(this.startThread_FirstTimegetData), null, 0);
			progressPopup.ShowDialog();
			if (this.m_dbsize.Length == 2)
			{
				this.cbdbtype.Visible = false;
			}
			else
			{
				this.cbdbtype.Visible = true;
				this.cbdbtype.SelectedIndex = 0;
			}
			this.dbCapacityData_show();
			this.startDBcapTimer();
		}
		private void DbCapacityEvent_getData(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new SysManDBCap.boardDate(this.dbCapacityData_getdata));
				return;
			}
			this.dbCapacityData_getdata();
		}
		private void dbCapacityData_getdata()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.startThread_getData));
		}
		private void startThread_getData(object obj)
		{
			commUtil.ShowInfo_DEBUG(" GetDBSpaceSize Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
			this.m_dbsize = DBTools.GetDBSpaceSize();
			commUtil.ShowInfo_DEBUG(" GetDBSpaceSize end == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
		}
		private object startThread_FirstTimegetData(object aaa)
		{
			this.m_dbsize = DBTools.GetDBSpaceSize();
			return 0;
		}
		private void DbCapacityEvent_show(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new SysManDBCap.boardDate(this.dbCapacityData_show));
				return;
			}
			this.dbCapacityData_show();
		}
		private void dbCapacityData_show()
		{
			if (this.m_dbsize == null)
			{
				this.m_dbsize = DBTools.GetDBSpaceSize();
			}
			double num;
			double num2;
			if (!this.cbdbtype.Visible)
			{
				num = (double)this.m_dbsize[1];
				num2 = (double)this.m_dbsize[0];
			}
			else
			{
				if (this.cbdbtype.SelectedIndex == 0)
				{
					num = (double)this.m_dbsize[1];
					num2 = (double)this.m_dbsize[0];
				}
				else
				{
					num = (double)this.m_dbsize[3];
					num2 = (double)this.m_dbsize[2];
				}
			}
			bool flag = true;
			if (num2 < 0.0)
			{
				num2 = 0.0;
				this.labcapacity.Text = "N/A";
				flag = false;
			}
			else
			{
				this.labcapacity.Text = (num2 / 1048576.0).ToString("F2") + "MB";
			}
			if (num < 0.0)
			{
				num = 0.0;
				this.labFreeSpace.Text = "N/A";
				flag = false;
			}
			else
			{
				this.labFreeSpace.Text = (num / 1048576.0).ToString("F2") + "MB";
			}
			if (!flag)
			{
				this.chtpie.Visible = false;
				return;
			}
			this.chtpie.Visible = true;
			this.chtpie.Legends[0].Docking = Docking.Top;
			this.chtpie.Legends[0].Alignment = StringAlignment.Center;
			this.chtpie.Legends[0].LegendStyle = LegendStyle.Table;
			this.chtpie.Series[0].Points.Clear();
			double[] array = new double[]
			{
				num2 / 1048576.0,
				num / 1048576.0
			};
			string[] xValue = new string[]
			{
				EcoLanguage.getMsg(LangRes.DBcap, new string[0]),
				EcoLanguage.getMsg(LangRes.Freespace, new string[0])
			};
			this.chtpie.Series[0].Points.DataBindXY(xValue, new System.Collections.IEnumerable[]
			{
				array
			});
			this.chtpie.Series[0].ChartType = SeriesChartType.Pie;
			this.chtpie.Series[0]["PieLabelStyle"] = "Outside";
			this.chtpie.Series[0].Points[1]["Exploded"] = "true";
			this.chtpie.ChartAreas[0].Area3DStyle.Enable3D = true;
			this.chtpie.Series[0]["PieDrawingStyle"] = "SoftEdge";
			this.chtpie.Series[0].Points[0].Color = Color.RoyalBlue;
			this.chtpie.Series[0].Points[1].Color = Color.FromArgb(162, 215, 48);
			if (num2 / 1048576.0 >= 200.0)
			{
				this.chtpie.Series[0].Points[0].Color = Color.Orange;
			}
			else
			{
				if (num2 / 1048576.0 >= 300.0)
				{
					this.chtpie.Series[0].Points[0].Color = Color.Red;
				}
			}
			this.chtpie.Series[0].MarkerStyle = MarkerStyle.Circle;
			this.chtpie.Series[0].MarkerSize = 4;
			this.chtpie.Series[0].IsValueShownAsLabel = true;
			this.chtpie.Series[0].LabelFormat = "F2";
		}
		private void startDBcapTimer()
		{
			this.boardTimer_show.Enabled = true;
			this.boardTimer_getdata.Enabled = true;
		}
		public void endDBcapTimer()
		{
			this.boardTimer_show.Enabled = false;
			this.boardTimer_getdata.Enabled = false;
		}
		private string getDBPath()
		{
			return DBUrl.DB_LOCATION;
		}
		private string getDBPath2()
		{
			return DBUrl.DB_LOCATION2;
		}
		private void cbdbtype_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.dbCapacityData_show();
		}
	}
}
