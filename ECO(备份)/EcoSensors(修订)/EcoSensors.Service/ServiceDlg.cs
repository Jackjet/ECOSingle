using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.Thread;
using EcoSensors.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
namespace EcoSensors.Service
{
	public class ServiceDlg : Form
	{
		private IContainer components;
		private Label lbMsg;
		private PictureBox pictureBoxLoading;
		private int enableclose;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ServiceDlg));
			this.lbMsg = new Label();
			this.pictureBoxLoading = new PictureBox();
			((ISupportInitialize)this.pictureBoxLoading).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.lbMsg, "lbMsg");
			this.lbMsg.BackColor = Color.Transparent;
			this.lbMsg.Name = "lbMsg";
			this.pictureBoxLoading.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pictureBoxLoading, "pictureBoxLoading");
			this.pictureBoxLoading.Image = Resources.loader;
			this.pictureBoxLoading.Name = "pictureBoxLoading";
			this.pictureBoxLoading.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			this.BackgroundImage = Resources.login_background;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.pictureBoxLoading);
			base.Controls.Add(this.lbMsg);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ServiceDlg";
			base.SizeGripStyle = SizeGripStyle.Hide;
			base.FormClosing += new FormClosingEventHandler(this.ServiceDlg_FormClosing);
			((ISupportInitialize)this.pictureBoxLoading).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ServiceDlg()
		{
			this.InitializeComponent();
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.initPage));
		}
		private void initPage(object obj)
		{
			try
			{
				System.Threading.Thread.Sleep(500);
				System.TimeSpan timeout = System.TimeSpan.FromMilliseconds(30000.0);
				try
				{
					ServiceController serviceController = new ServiceController(EcoGlobalVar.gl_ServiceName);
					serviceController.Start();
					serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
				}
				catch (System.Exception)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.srvFail, new string[0]));
				}
				ControlAccess.ConfigControl config = delegate(Control control, object param)
				{
					this.enableclose = 1;
					base.Close();
				};
				ControlAccess controlAccess = new ControlAccess(this, config);
				controlAccess.Access(this, null);
			}
			catch (System.Exception)
			{
			}
		}
		private void ServiceDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.enableclose == 0)
			{
				e.Cancel = true;
			}
		}
	}
}
