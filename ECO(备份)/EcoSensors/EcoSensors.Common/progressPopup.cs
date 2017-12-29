using EcoSensors._Lang;
using EcoSensors.Common.Thread;
using EcoSensors.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.Common
{
	public class progressPopup : Form
	{
		public delegate object ProcessInThread(object param);
		public delegate object ProgramBarThread(ref int ibarper, ref string sbartype);
		private delegate void programbarRefresh(int i);
		private delegate void programbarTypeRefresh(string str);
		public const int PIC_LoadingBAR = 0;
		public const int PIC_CIRCLE = 1;
		public const int PIC_ProgressBar = 2;
		public const int opt_CloseDlg = 0;
		public const int opt_KeepDlg = 1;
		private IContainer components;
		private Label labLoading;
		private Label lbMsg;
		private PictureBox pictureBoxLoading;
		private ProgressBar progressBar1;
		private string m_title;
		private int m_usepic;
		private progressPopup.ProcessInThread theProcess;
		private progressPopup.ProgramBarThread m_ProgramBarThread;
		private object m_param;
		private int m_ProgramBar_Percent = -100;
		private int m_ProgramBar_Type = -100;
		private int m_opt;
		private System.Timers.Timer setbarTimer;
		private object m_retv;
		private int i_bar_per = 1;
		private int last_percent = 1;
		private string sbarType = "";
		private string lastbarType;
		public object Return_V
		{
			get
			{
				return this.m_retv;
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
			this.labLoading = new Label();
			this.lbMsg = new Label();
			this.pictureBoxLoading = new PictureBox();
			this.progressBar1 = new ProgressBar();
			((ISupportInitialize)this.pictureBoxLoading).BeginInit();
			base.SuspendLayout();
			this.labLoading.BackColor = Color.White;
			this.labLoading.Image = Resources.loading;
			this.labLoading.ImeMode = ImeMode.NoControl;
			this.labLoading.Location = new Point(82, 69);
			this.labLoading.Name = "labLoading";
			this.labLoading.Size = new Size(201, 49);
			this.labLoading.TabIndex = 36;
			this.labLoading.TextAlign = ContentAlignment.BottomLeft;
			this.labLoading.Visible = false;
			this.lbMsg.AutoSize = true;
			this.lbMsg.BackColor = Color.Transparent;
			this.lbMsg.ImeMode = ImeMode.NoControl;
			this.lbMsg.Location = new Point(21, 18);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new Size(102, 15);
			this.lbMsg.TabIndex = 37;
			this.lbMsg.Text = "Starting service ...";
			this.pictureBoxLoading.BackColor = Color.Transparent;
			this.pictureBoxLoading.BackgroundImageLayout = ImageLayout.None;
			this.pictureBoxLoading.Image = Resources.loader;
			this.pictureBoxLoading.ImeMode = ImeMode.NoControl;
			this.pictureBoxLoading.Location = new Point(132, 46);
			this.pictureBoxLoading.Name = "pictureBoxLoading";
			this.pictureBoxLoading.Size = new Size(101, 100);
			this.pictureBoxLoading.TabIndex = 127;
			this.pictureBoxLoading.TabStop = false;
			this.pictureBoxLoading.Visible = false;
			this.progressBar1.ImeMode = ImeMode.NoControl;
			this.progressBar1.Location = new Point(44, 78);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new Size(276, 22);
			this.progressBar1.TabIndex = 128;
			this.progressBar1.Visible = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = SystemColors.Control;
			base.ClientSize = new Size(364, 179);
			base.ControlBox = false;
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.pictureBoxLoading);
			base.Controls.Add(this.lbMsg);
			base.Controls.Add(this.labLoading);
			this.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "progressPopup";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "progressPopup";
			base.TopMost = true;
			((ISupportInitialize)this.pictureBoxLoading).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public progressPopup()
		{
			this.InitializeComponent();
		}
		public progressPopup(string title, int usepic, string strlbMsg, Bitmap backgroundImg, progressPopup.ProcessInThread process, object param, int ifcloseDlg)
		{
			this.InitializeComponent();
			this.m_title = title;
			this.m_usepic = usepic;
			this.m_opt = ifcloseDlg;
			this.Text = this.m_title;
			this.lbMsg.Text = strlbMsg;
			this.theProcess = process;
			this.m_param = param;
			if (backgroundImg != null)
			{
				this.BackgroundImage = backgroundImg;
			}
			switch (this.m_usepic)
			{
			case 0:
				this.labLoading.Show();
				break;
			case 1:
				this.pictureBoxLoading.Show();
				break;
			case 2:
				this.progressBar1.Minimum = 1;
				this.progressBar1.Maximum = 100;
				this.progressBar1.Value = 1;
				this.progressBar1.Step = 1;
				this.progressBar1.Show();
				break;
			}
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.startThread));
		}
		public progressPopup(string title, int usepic, string strlbMsg, Bitmap backgroundImg, progressPopup.ProcessInThread process, object param, progressPopup.ProgramBarThread pProgramBarThread, int ifcloseDlg)
		{
			this.InitializeComponent();
			this.m_title = title;
			this.m_usepic = usepic;
			this.m_ProgramBarThread = pProgramBarThread;
			this.m_opt = ifcloseDlg;
			this.Text = this.m_title;
			this.lbMsg.Text = strlbMsg;
			this.theProcess = process;
			this.m_param = param;
			if (backgroundImg != null)
			{
				this.BackgroundImage = backgroundImg;
			}
			switch (this.m_usepic)
			{
			case 0:
				this.labLoading.Show();
				break;
			case 1:
				this.pictureBoxLoading.Show();
				break;
			case 2:
				this.progressBar1.Minimum = 1;
				this.progressBar1.Maximum = 100;
				this.progressBar1.Value = 1;
				this.progressBar1.Step = 1;
				this.progressBar1.Show();
				break;
			}
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.startThread));
			this.setbarTimer = new System.Timers.Timer();
			this.setbarTimer.Elapsed += new ElapsedEventHandler(this.setbarTimer_Elapsed);
			this.setbarTimer.Interval = 1000.0;
			this.setbarTimer.Enabled = false;
			if (pProgramBarThread != null)
			{
				this.setbarTimer.Enabled = true;
			}
		}
		private void setbarTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.m_ProgramBarThread(ref this.i_bar_per, ref this.sbarType);
			if (this.i_bar_per > this.last_percent)
			{
				this.last_percent = this.i_bar_per;
				if (base.InvokeRequired)
				{
					base.Invoke(new progressPopup.programbarRefresh(this.setbar), new object[]
					{
						this.i_bar_per
					});
					return;
				}
				this.setbar(this.i_bar_per);
			}
		}
		private void setbarString(string str)
		{
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				this.lbMsg.Text = str;
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this, null);
		}
		private void setbar(int v)
		{
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				this.progressBar1.Value = v;
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this, null);
		}
		private void startThread(object obj)
		{
			try
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
				System.Threading.Thread.Sleep(50);
				if (this.theProcess != null)
				{
					this.m_retv = this.theProcess(this.m_param);
				}
				if (this.setbarTimer != null)
				{
					try
					{
						this.setbarTimer.Stop();
						this.setbarTimer.Dispose();
					}
					catch
					{
					}
				}
				this.setbar(100);
				if (this.m_opt == 0)
				{
					ControlAccess.ConfigControl config = delegate(Control control, object param)
					{
						progressPopup progressPopup = control as progressPopup;
						progressPopup.Close();
					};
					ControlAccess controlAccess = new ControlAccess(this, config);
					controlAccess.Access(this, null);
				}
			}
			catch (System.Exception)
			{
				this.m_retv = null;
				ControlAccess.ConfigControl config2 = delegate(Control control, object param)
				{
					progressPopup progressPopup = control as progressPopup;
					progressPopup.Close();
				};
				ControlAccess controlAccess2 = new ControlAccess(this, config2);
				controlAccess2.Access(this, null);
			}
		}
	}
}
