using DBAccessAPI;
using Dispatcher;
using EcoSensors.Common;
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
namespace EcoSensors.EnegManPage.DashBoardUser
{
	public class DashBoardUser : UserControl
	{
		private delegate void boardDate(int from);
		private const int Interval_AfterChange = 10000;
		private const int Interval_NoChange = 1000;
		private int boardFlg = 1;
		private System.Timers.Timer boardTimer;
		private TableLayoutPanel devRackInfo;
		private DataSet m_TitleData_Info = new DataSet();
		private System.Collections.ArrayList m_allRacks;
		private TitleInfoUser title = new TitleInfoUser();
		private IContainer components;
		private TableLayoutPanel tableLayoutPanel1;
		private Button btnthreshold;
		private Panel panel3;
		private Panel panelImgStatus;
		private Label lblstatus1;
		private Label lblstatus2;
		private Label lblstatus3;
		private Label lblstatus4;
		private PictureBox pic;
		private Label label9;
		private Panel panel1;
		private Panel palboard;
		private EcoSensors.DevManDCFloorGrid.DevManDCFloorGrid  devManDCFloorGrid1;
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
		public DashBoardUser()
		{
			this.InitializeComponent();
			this.FreshFlg_DashBoard = 1;
			EcoGlobalVar.gl_DashBoardUserCtrl = this;
			this.boardTimer = new System.Timers.Timer();
			this.boardTimer.Elapsed += new ElapsedEventHandler(this.boardTimerEvent);
			this.boardTimer.Interval = 10000.0;
			this.boardTimer.AutoReset = true;
			this.boardTimer.Enabled = false;
		}
		public void pageInit_1()
		{
			this.boardDataInit();
		}
		public void pageInit()
		{
			commUtil.ShowInfo_DEBUG("DashBoard pageInit -1  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
			this.boardTimeDate(0);
			this.startBoardTimer();
			commUtil.ShowInfo_DEBUG("DashBoard pageInit -2  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
		}
		private void startBoardTimer()
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
		private void boardTimerEvent(object source, ElapsedEventArgs e)
		{
			this.boardTimer.Interval = 30000.0;
			if (base.InvokeRequired)
			{
				base.Invoke(new DashBoardUser.boardDate(this.boardTimeDate), new object[]
				{
					1
				});
				return;
			}
			this.boardTimeDate(1);
		}
		private int boardDataInit()
		{
			bool flag = false;
			switch (this.FreshFlg_DashBoard)
			{
			case 1:
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_DrawRack Begin " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				if (!EcoGlobalVar.gl_isProcessThreadRuning())
				{
					EcoGlobalVar.gl_StartProcessfThread(false);
					flag = true;
				}
				this.devManDCFloorGrid1.Hide();
				this.devRackInfo = this.devManDCFloorGrid1.getRackCtrl();
				this.devRackInfo.SuspendLayout();
				this.m_allRacks = ClientAPI.getRackInfo();
				foreach (RackInfo rackInfo in this.m_allRacks)
				{
					int startPoint_X = rackInfo.StartPoint_X;
					int startPoint_Y = rackInfo.StartPoint_Y;
					int endPoint_X = rackInfo.EndPoint_X;
					int arg_BE_0 = rackInfo.EndPoint_Y;
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
					this.devRackInfo.Controls.Add(label, startPoint_Y, startPoint_X);
					if (num == 0)
					{
						this.devRackInfo.SetColumnSpan(label, 2);
						this.devRackInfo.SetRowSpan(label, 1);
					}
					else
					{
						if (num == 1)
						{
							this.devRackInfo.SetColumnSpan(label, 1);
							this.devRackInfo.SetRowSpan(label, 2);
						}
					}
				}
				this.devRackInfo.ResumeLayout();
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_DrawRack End  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				this.m_TitleData_Info = ClientAPI.getDataSet(0);
				this.FreshFlg_DashBoard = -1;
				this.devManDCFloorGrid1.Show();
				if (flag)
				{
					EcoGlobalVar.gl_StopProcessfThread();
				}
				return 0;
			case 2:
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_modifydata Begin  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				this.m_allRacks = ClientAPI.getRackInfo();
				this.m_TitleData_Info = ClientAPI.getDataSet(0);
				this.FreshFlg_DashBoard = -1;
				commUtil.ShowInfo_DEBUG("boardDataInit ---FLGAppAct_modifydata End  " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
				return 0;
			default:
				return 1;
			}
		}
		private void Rack_MouseHover(object sender, System.EventArgs e)
		{
			Screen[] allScreens = Screen.AllScreens;
			int width = allScreens[0].Bounds.Width;
			int height = allScreens[0].Bounds.Height;
			int x = Control.MousePosition.X - 5;
			int y = Control.MousePosition.Y - 5;
			if (width - Control.MousePosition.X < this.title.Size.Width)
			{
				x = Control.MousePosition.X - this.title.Size.Width;
			}
			if (height - Control.MousePosition.Y < this.title.Size.Height)
			{
				y = Control.MousePosition.Y - this.title.Size.Height;
			}
			this.Cursor = Cursors.WaitCursor;
			this.title.Location = new Point(x, y);
			long rackID = (long)((Label)sender).Tag;
			string devidsinRack = this.getDevidsinRack(this.m_allRacks, rackID);
			this.title.Set(rackID, devidsinRack, EcoGlobalVar.gl_LoginUserUACDev2Port, this.m_TitleData_Info);
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
		private void boardTimeDate(int from)
		{
			int num = boardDataUtil.PDUData_FreshFLG_Yes;
			if (this.boardDataInit() != 0)
			{
				num = ClientAPI.updateDataSet(ref this.m_TitleData_Info, from);
			}
			if (num == boardDataUtil.PDUData_FreshFLG_NO)
			{
				this.boardTimer.Interval = 1000.0;
				return;
			}
			System.Collections.Generic.Dictionary<long, Color> dictionary = new System.Collections.Generic.Dictionary<long, Color>();
			dictionary = boardDataUtil.UACsensorsThresholdStatus(EcoGlobalVar.gl_LoginUserUACDev2Port, this.m_allRacks, this.m_TitleData_Info);
			foreach (Control control in this.devRackInfo.Controls)
			{
				long key = (long)control.Tag;
				if (dictionary.ContainsKey(key))
				{
					control.BackColor = dictionary[key];
				}
			}
			if (num == boardDataUtil.PDUData_FreshFLG_NO)
			{
				this.boardTimer.Interval = 1000.0;
				return;
			}
			if (this.FreshFlg_DashBoard == 1)
			{
				this.boardTimer.Interval = 1000.0;
				return;
			}
			this.boardTimer.Interval = 10000.0;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DashBoardUser));
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.btnthreshold = new Button();
			this.panel3 = new Panel();
			this.panelImgStatus = new Panel();
			this.lblstatus1 = new Label();
			this.lblstatus2 = new Label();
			this.lblstatus3 = new Label();
			this.lblstatus4 = new Label();
			this.pic = new PictureBox();
			this.label9 = new Label();
			this.panel1 = new Panel();
			this.palboard = new Panel();
            this.devManDCFloorGrid1 = new EcoSensors.DevManDCFloorGrid.DevManDCFloorGrid();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panelImgStatus.SuspendLayout();
			((ISupportInitialize)this.pic).BeginInit();
			this.panel1.SuspendLayout();
			this.palboard.SuspendLayout();
			base.SuspendLayout();
			this.tableLayoutPanel1.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.btnthreshold, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.btnthreshold.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.btnthreshold, "btnthreshold");
			this.btnthreshold.Name = "btnthreshold";
			this.btnthreshold.UseVisualStyleBackColor = false;
			this.panel3.BackColor = Color.White;
			this.panel3.Controls.Add(this.panelImgStatus);
			this.panel3.Controls.Add(this.label9);
			componentResourceManager.ApplyResources(this.panel3, "panel3");
			this.panel3.Name = "panel3";
			this.panelImgStatus.Controls.Add(this.lblstatus1);
			this.panelImgStatus.Controls.Add(this.lblstatus2);
			this.panelImgStatus.Controls.Add(this.lblstatus3);
			this.panelImgStatus.Controls.Add(this.lblstatus4);
			this.panelImgStatus.Controls.Add(this.pic);
			componentResourceManager.ApplyResources(this.panelImgStatus, "panelImgStatus");
			this.panelImgStatus.Name = "panelImgStatus";
			componentResourceManager.ApplyResources(this.lblstatus1, "lblstatus1");
			this.lblstatus1.Name = "lblstatus1";
			componentResourceManager.ApplyResources(this.lblstatus2, "lblstatus2");
			this.lblstatus2.Name = "lblstatus2";
			componentResourceManager.ApplyResources(this.lblstatus3, "lblstatus3");
			this.lblstatus3.Name = "lblstatus3";
			componentResourceManager.ApplyResources(this.lblstatus4, "lblstatus4");
			this.lblstatus4.Name = "lblstatus4";
			this.pic.Image = Resources.Img650x75;
			componentResourceManager.ApplyResources(this.pic, "pic");
			this.pic.Name = "pic";
			this.pic.TabStop = false;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			this.panel1.Controls.Add(this.palboard);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.palboard.BackColor = Color.White;
			this.palboard.Controls.Add(this.devManDCFloorGrid1);
			componentResourceManager.ApplyResources(this.palboard, "palboard");
			this.palboard.Name = "palboard";
			this.devManDCFloorGrid1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.devManDCFloorGrid1, "devManDCFloorGrid1");
			this.devManDCFloorGrid1.Name = "devManDCFloorGrid1";
			this.devManDCFloorGrid1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.panel3);
			base.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DashBoardUser";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panelImgStatus.ResumeLayout(false);
			((ISupportInitialize)this.pic).EndInit();
			this.panel1.ResumeLayout(false);
			this.palboard.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
