using DBAccessAPI.user;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using deviceP = EcoSensors.DevManPage;
using mainp= EcoSensors.EnegManPage;
using logP= EcoSensors.LogPage;
using EcoSensors.Monitor;
using EcoSensors.Properties;
using sysmainp =EcoSensors.SysManPage;
using userP = EcoSensors.UserManPage;
using RawInput;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.MainForm
{
	public class MainForm : Form
	{
		private const uint SW_RESTORE = 9u;
		private const uint SW_MINIMIZE = 6u;
		private IContainer components;
		private Panel panel1;
		private TableLayoutPanel tlpMainMenu;
		private Button butSysManagement;
		private Button butOutletAccess;
		private Button butLog;
		private Button butUser;
		private Button butDevice;
		private PictureBox pictureBox1;
		private PictureBox pblogout;
        private sysmainp.SysManPage sysManPage1;
        private mainp.EnegManPage enegManPage1;
        private logP.LogPage logPage1;
        private userP.UserManPage userManPage1;
        private deviceP.DevManPage devManPage1;
		private Panel panel3;
		private Panel panel2;
		private PictureBox pbmonitor;
		private PictureBox pbIdleTm;
		private static RawInput.RawInput _rawinput;
		private int CurSelMainTab;
		private ToolTip toolTip1 = new ToolTip();
		private monitor m_pWinmonitor = new monitor();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
			this.panel2 = new Panel();
			this.pbIdleTm = new PictureBox();
			this.pbmonitor = new PictureBox();
			this.pblogout = new PictureBox();
			this.panel1 = new Panel();
			this.panel3 = new Panel();
			this.pictureBox1 = new PictureBox();
			this.tlpMainMenu = new TableLayoutPanel();
			this.butSysManagement = new Button();
			this.butOutletAccess = new Button();
			this.butLog = new Button();
			this.butUser = new Button();
			this.butDevice = new Button();
            this.devManPage1 = new deviceP.DevManPage();
            this.userManPage1 = new userP.UserManPage();
            this.logPage1 = new logP.LogPage();
            this.sysManPage1 = new sysmainp.SysManPage();
            this.enegManPage1 = new mainp.EnegManPage();
			this.panel2.SuspendLayout();
			((ISupportInitialize)this.pbIdleTm).BeginInit();
			((ISupportInitialize)this.pbmonitor).BeginInit();
			((ISupportInitialize)this.pblogout).BeginInit();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			this.tlpMainMenu.SuspendLayout();
			base.SuspendLayout();
			this.panel2.Controls.Add(this.pbIdleTm);
			this.panel2.Controls.Add(this.pbmonitor);
			this.panel2.Controls.Add(this.pblogout);
			this.panel2.Controls.Add(this.devManPage1);
			this.panel2.Controls.Add(this.userManPage1);
			this.panel2.Controls.Add(this.logPage1);
			this.panel2.Controls.Add(this.sysManPage1);
			this.panel2.Controls.Add(this.enegManPage1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			componentResourceManager.ApplyResources(this.pbIdleTm, "pbIdleTm");
			this.pbIdleTm.BackColor = Color.FromArgb(18, 160, 143);
			this.pbIdleTm.Cursor = Cursors.Hand;
			this.pbIdleTm.Image = Resources.top_idletm;
			this.pbIdleTm.Name = "pbIdleTm";
			this.pbIdleTm.TabStop = false;
			this.pbIdleTm.Click += new System.EventHandler(this.pbIdleTm_Click);
			this.pbIdleTm.MouseHover += new System.EventHandler(this.pbIdleTm_MouseHover);
			componentResourceManager.ApplyResources(this.pbmonitor, "pbmonitor");
			this.pbmonitor.BackColor = Color.FromArgb(18, 160, 143);
			this.pbmonitor.Cursor = Cursors.Hand;
			this.pbmonitor.Image = Resources.top_monitor;
			this.pbmonitor.Name = "pbmonitor";
			this.pbmonitor.TabStop = false;
			this.pbmonitor.Click += new System.EventHandler(this.pbmonitor_Click);
			this.pbmonitor.MouseHover += new System.EventHandler(this.pbmonitor_MouseHover);
			componentResourceManager.ApplyResources(this.pblogout, "pblogout");
			this.pblogout.BackColor = Color.FromArgb(18, 160, 143);
			this.pblogout.Cursor = Cursors.Hand;
			this.pblogout.Image = Resources.top_logout;
			this.pblogout.Name = "pblogout";
			this.pblogout.TabStop = false;
			this.pblogout.Click += new System.EventHandler(this.logout_LinkClicked);
			this.pblogout.MouseHover += new System.EventHandler(this.lnklogout_MouseHover);
			this.panel1.BackColor = Color.Transparent;
			this.panel1.BackgroundImage = Resources.background_1280;
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.tlpMainMenu);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.panel3.BackColor = Color.Transparent;
			this.panel3.Controls.Add(this.pictureBox1);
			componentResourceManager.ApplyResources(this.panel3, "panel3");
			this.panel3.Name = "panel3";
			this.pictureBox1.BackColor = Color.Transparent;
			this.pictureBox1.Cursor = Cursors.Hand;
			this.pictureBox1.Image = Resources.logo;
			componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			this.tlpMainMenu.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.tlpMainMenu, "tlpMainMenu");
			this.tlpMainMenu.Controls.Add(this.butSysManagement, 3, 0);
			this.tlpMainMenu.Controls.Add(this.butOutletAccess, 0, 0);
			this.tlpMainMenu.Controls.Add(this.butLog, 4, 0);
			this.tlpMainMenu.Controls.Add(this.butUser, 1, 0);
			this.tlpMainMenu.Controls.Add(this.butDevice, 2, 0);
			this.tlpMainMenu.Name = "tlpMainMenu";
			this.butSysManagement.AccessibleRole = AccessibleRole.None;
			this.butSysManagement.BackColor = Color.Transparent;
			this.butSysManagement.BackgroundImage = Resources.back;
			componentResourceManager.ApplyResources(this.butSysManagement, "butSysManagement");
			this.butSysManagement.CausesValidation = false;
			this.butSysManagement.Cursor = Cursors.Default;
			this.butSysManagement.FlatAppearance.BorderColor = Color.FromArgb(89, 89, 89);
			this.butSysManagement.FlatAppearance.BorderSize = 0;
			this.butSysManagement.FlatAppearance.MouseDownBackColor = Color.Transparent;
			this.butSysManagement.FlatAppearance.MouseOverBackColor = Color.Transparent;
			this.butSysManagement.ForeColor = Color.FromArgb(0, 168, 150);
			this.butSysManagement.Image = Resources.t_maintenance;
			this.butSysManagement.Name = "butSysManagement";
			this.butSysManagement.TabStop = false;
			this.butSysManagement.Tag = "Tag_SysMng";
			this.butSysManagement.UseVisualStyleBackColor = false;
			this.butSysManagement.Click += new System.EventHandler(this.butSysManagement_Click);
			this.butSysManagement.MouseLeave += new System.EventHandler(this.comm_butMouseLeave);
			this.butSysManagement.MouseHover += new System.EventHandler(this.comm_butMouseHover);
			this.butSysManagement.MouseMove += new MouseEventHandler(this.comm_butMouseMove);
			this.butOutletAccess.AccessibleRole = AccessibleRole.None;
			this.butOutletAccess.BackColor = Color.Transparent;
			this.butOutletAccess.BackgroundImage = Resources.back;
			componentResourceManager.ApplyResources(this.butOutletAccess, "butOutletAccess");
			this.butOutletAccess.CausesValidation = false;
			this.butOutletAccess.Cursor = Cursors.Default;
			this.butOutletAccess.FlatAppearance.BorderColor = Color.FromArgb(89, 89, 89);
			this.butOutletAccess.FlatAppearance.BorderSize = 0;
			this.butOutletAccess.FlatAppearance.MouseDownBackColor = Color.Transparent;
			this.butOutletAccess.FlatAppearance.MouseOverBackColor = Color.Transparent;
			this.butOutletAccess.ForeColor = Color.FromArgb(0, 168, 150);
			this.butOutletAccess.Image = Resources.t_outletaccess;
			this.butOutletAccess.Name = "butOutletAccess";
			this.butOutletAccess.TabStop = false;
			this.butOutletAccess.Tag = "Tag_EngMng";
			this.butOutletAccess.UseVisualStyleBackColor = false;
			this.butOutletAccess.Click += new System.EventHandler(this.butOutletAccess_Click);
			this.butOutletAccess.MouseLeave += new System.EventHandler(this.comm_butMouseLeave);
			this.butOutletAccess.MouseHover += new System.EventHandler(this.comm_butMouseHover);
			this.butOutletAccess.MouseMove += new MouseEventHandler(this.comm_butMouseMove);
			this.butLog.AccessibleRole = AccessibleRole.None;
			this.butLog.BackColor = Color.Transparent;
			this.butLog.BackgroundImage = Resources.back;
			componentResourceManager.ApplyResources(this.butLog, "butLog");
			this.butLog.CausesValidation = false;
			this.butLog.Cursor = Cursors.Default;
			this.butLog.FlatAppearance.BorderColor = Color.FromArgb(89, 89, 89);
			this.butLog.FlatAppearance.BorderSize = 0;
			this.butLog.FlatAppearance.MouseDownBackColor = Color.Transparent;
			this.butLog.FlatAppearance.MouseOverBackColor = Color.Transparent;
			this.butLog.ForeColor = Color.FromArgb(0, 168, 150);
			this.butLog.Image = Resources.t_log;
			this.butLog.Name = "butLog";
			this.butLog.TabStop = false;
			this.butLog.Tag = "Tag_Log";
			this.butLog.UseVisualStyleBackColor = false;
			this.butLog.Click += new System.EventHandler(this.butSystemLog_Click);
			this.butLog.MouseLeave += new System.EventHandler(this.comm_butMouseLeave);
			this.butLog.MouseHover += new System.EventHandler(this.comm_butMouseHover);
			this.butLog.MouseMove += new MouseEventHandler(this.comm_butMouseMove);
			this.butUser.AccessibleRole = AccessibleRole.None;
			this.butUser.BackColor = Color.Transparent;
			this.butUser.BackgroundImage = Resources.back;
			componentResourceManager.ApplyResources(this.butUser, "butUser");
			this.butUser.CausesValidation = false;
			this.butUser.Cursor = Cursors.Default;
			this.butUser.FlatAppearance.BorderColor = Color.FromArgb(89, 89, 89);
			this.butUser.FlatAppearance.BorderSize = 0;
			this.butUser.FlatAppearance.MouseDownBackColor = Color.Transparent;
			this.butUser.FlatAppearance.MouseOverBackColor = Color.Transparent;
			this.butUser.ForeColor = Color.FromArgb(0, 168, 150);
			this.butUser.Image = Resources.t_usermanage;
			this.butUser.Name = "butUser";
			this.butUser.TabStop = false;
			this.butUser.Tag = "Tag_UserMng";
			this.butUser.UseVisualStyleBackColor = false;
			this.butUser.Click += new System.EventHandler(this.butUser_Click);
			this.butUser.MouseLeave += new System.EventHandler(this.comm_butMouseLeave);
			this.butUser.MouseHover += new System.EventHandler(this.comm_butMouseHover);
			this.butUser.MouseMove += new MouseEventHandler(this.comm_butMouseMove);
			this.butDevice.AccessibleRole = AccessibleRole.None;
			this.butDevice.BackColor = Color.Transparent;
			this.butDevice.BackgroundImage = Resources.back;
			componentResourceManager.ApplyResources(this.butDevice, "butDevice");
			this.butDevice.CausesValidation = false;
			this.butDevice.Cursor = Cursors.Default;
			this.butDevice.FlatAppearance.BorderColor = Color.FromArgb(89, 89, 89);
			this.butDevice.FlatAppearance.BorderSize = 0;
			this.butDevice.FlatAppearance.MouseDownBackColor = Color.Transparent;
			this.butDevice.FlatAppearance.MouseOverBackColor = Color.Transparent;
			this.butDevice.ForeColor = Color.FromArgb(0, 168, 150);
			this.butDevice.Image = Resources.t_devicemanage;
			this.butDevice.Name = "butDevice";
			this.butDevice.TabStop = false;
			this.butDevice.Tag = "Tag_DevMng";
			this.butDevice.UseVisualStyleBackColor = false;
			this.butDevice.Click += new System.EventHandler(this.butDevice_Click);
			this.butDevice.MouseLeave += new System.EventHandler(this.comm_butMouseLeave);
			this.butDevice.MouseHover += new System.EventHandler(this.comm_butMouseHover);
			this.butDevice.MouseMove += new MouseEventHandler(this.comm_butMouseMove);
			this.devManPage1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.devManPage1, "devManPage1");
			this.devManPage1.FlushFlg_RackBoard = 0;
			this.devManPage1.FlushFlg_ZoneBoard = 0;
			this.devManPage1.Name = "devManPage1";
			this.userManPage1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.userManPage1, "userManPage1");
			this.userManPage1.Name = "userManPage1";
			componentResourceManager.ApplyResources(this.logPage1, "logPage1");
			this.logPage1.BackColor = Color.WhiteSmoke;
			this.logPage1.Name = "logPage1";
			componentResourceManager.ApplyResources(this.sysManPage1, "sysManPage1");
			this.sysManPage1.BackColor = Color.WhiteSmoke;
			this.sysManPage1.Name = "sysManPage1";
			this.enegManPage1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.enegManPage1, "enegManPage1");
			this.enegManPage1.Name = "enegManPage1";
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.Name = "MainForm";
			base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((ISupportInitialize)this.pbIdleTm).EndInit();
			((ISupportInitialize)this.pbmonitor).EndInit();
			((ISupportInitialize)this.pblogout).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			((ISupportInitialize)this.pictureBox1).EndInit();
			this.tlpMainMenu.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		private static void OnRawInput(object sender, InputEventArg e)
		{
			if (e.RawInputEvent.DeviceType.Equals("MOUSE"))
			{
				if (e.RawInputEvent.mouseMessage == 1uL || e.RawInputEvent.mouseMessage == 2uL || e.RawInputEvent.mouseMessage == 16uL || e.RawInputEvent.mouseMessage == 32uL || e.RawInputEvent.mouseMessage == 4uL || e.RawInputEvent.mouseMessage == 8uL || e.RawInputEvent.mouseMessage == 1uL || e.RawInputEvent.mouseMessage == 2uL || e.RawInputEvent.mouseMessage == 4uL || e.RawInputEvent.mouseMessage == 8uL || e.RawInputEvent.mouseMessage == 16uL || e.RawInputEvent.mouseMessage == 32uL || e.RawInputEvent.mouseMessage == 64uL || e.RawInputEvent.mouseMessage == 128uL || e.RawInputEvent.mouseMessage == 256uL || e.RawInputEvent.mouseMessage == 512uL || e.RawInputEvent.mouseMessage == 1024uL)
				{
					Program.m_IdleCounter = 0;
					return;
				}
			}
			else
			{
				if (e.RawInputEvent.DeviceType.Equals("KEYBOARD"))
				{
					Program.m_IdleCounter = 0;
				}
			}
		}
		public static void RawInput_Install(System.IntPtr handle)
		{
			MainForm._rawinput = new RawInput.RawInput(handle);
			MainForm._rawinput.CaptureOnlyIfTopMostWindow = true;
			MainForm._rawinput.AddMessageFilter();
			MainForm._rawinput.InputEvent += new RawInputDriver.DeviceEventHandler(MainForm.OnRawInput);
		}
		public static void RawInput_Uninstall()
		{
			MainForm._rawinput.InputEvent -= new RawInputDriver.DeviceEventHandler(MainForm.OnRawInput);
		}
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int ShowWindow(System.IntPtr hWnd, uint Msg);
		public MainForm()
		{
			this.InitializeComponent();
			MainForm.RawInput_Install(base.Handle);
			EcoGlobalVar.gl_DevManPage = this.devManPage1;
			this.pageInit();
		}
		public MainForm(UserInfo pUser)
		{
			this.InitializeComponent();
			MainForm.RawInput_Install(base.Handle);
			string text = DevAccessCfg.GetInstance().getVersion();
			if (text.Length > 0)
			{
				text = " (" + text + ")";
			}
			else
			{
				System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				text = string.Concat(new string[]
				{
					" (V",
					version.Major.ToString(),
					".",
					version.Minor.ToString(),
					".",
					version.Build.ToString("000"),
					")"
				});
			}
			this.Text += text;
			EcoGlobalVar.gl_DevManPage = this.devManPage1;
			int num = 1;
			this.butUser.Hide();
			this.butDevice.Hide();
			this.butSysManagement.Hide();
			this.butLog.Hide();
			if (EcoGlobalVar.ECOAppRunMode == 1)
			{
				int userRight = pUser.UserRight;
				if ((userRight & 1) != 0)
				{
					this.tlpMainMenu.SetCellPosition(this.butUser, new TableLayoutPanelCellPosition(num, 0));
					this.butUser.Show();
					num++;
				}
				if ((userRight & 2) != 0)
				{
					this.tlpMainMenu.SetCellPosition(this.butDevice, new TableLayoutPanelCellPosition(num, 0));
					this.butDevice.Show();
					num++;
				}
				if ((userRight & 4) != 0)
				{
					this.tlpMainMenu.SetCellPosition(this.butSysManagement, new TableLayoutPanelCellPosition(num, 0));
					this.butSysManagement.Show();
					num++;
				}
				if ((userRight & 8) != 0)
				{
					this.tlpMainMenu.SetCellPosition(this.butLog, new TableLayoutPanelCellPosition(num, 0));
					this.butLog.Show();
					num++;
				}
			}
			if (pUser.UserType != 0)
			{
				this.enegManPage1.hiddenPowerAnalysisButton();
				this.logPage1.hidden_LogOptionEvent();
			}
			this.pageInit();
		}
		private void pageInit()
		{
			this.toolTip1.AutoPopDelay = 5000;
			this.toolTip1.InitialDelay = 1000;
			this.toolTip1.ReshowDelay = 500;
			this.toolTip1.ToolTipTitle = "";
			this.toolTip1.ShowAlways = true;
			this.comm_butClick(this.butOutletAccess, null);
			this.enegManPage1.pageInit_1();
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			string name = ((Button)sender).Name;
			this.enegManPage1.Visible = false;
			this.userManPage1.Visible = false;
			this.devManPage1.Visible = false;
			this.sysManPage1.Visible = false;
			this.logPage1.Visible = false;
			if (name.Equals(this.butOutletAccess.Name))
			{
				EcoGlobalVar.stopalltimer(false);
				this.enegManPage1.Visible = true;
				this.CurSelMainTab = 0;
				this.butOutletAccess.Image = Resources.t_outletaccess_over;
				this.butUser.Image = Resources.t_usermanage;
				this.butDevice.Image = Resources.t_devicemanage;
				this.butSysManagement.Image = Resources.t_maintenance;
				this.butLog.Image = Resources.t_log;
			}
			else
			{
				if (name.Equals(this.butUser.Name))
				{
					EcoGlobalVar.stopalltimer(false);
					this.userManPage1.Visible = true;
					this.CurSelMainTab = 1;
					this.butOutletAccess.Image = Resources.t_outletaccess;
					this.butUser.Image = Resources.t_usermanage_over;
					this.butDevice.Image = Resources.t_devicemanage;
					this.butSysManagement.Image = Resources.t_maintenance;
					this.butLog.Image = Resources.t_log;
				}
				else
				{
					if (name.Equals(this.butDevice.Name))
					{
						EcoGlobalVar.stopalltimer(false);
						this.devManPage1.Visible = true;
						this.CurSelMainTab = 2;
						this.butOutletAccess.Image = Resources.t_outletaccess;
						this.butUser.Image = Resources.t_usermanage;
						this.butDevice.Image = Resources.t_devicemanage_over;
						this.butSysManagement.Image = Resources.t_maintenance;
						this.butLog.Image = Resources.t_log;
					}
					else
					{
						if (name.Equals(this.butSysManagement.Name))
						{
							EcoGlobalVar.stopalltimer(false);
							this.sysManPage1.Visible = true;
							this.CurSelMainTab = 3;
							this.butOutletAccess.Image = Resources.t_outletaccess;
							this.butUser.Image = Resources.t_usermanage;
							this.butDevice.Image = Resources.t_devicemanage;
							this.butSysManagement.Image = Resources.t_maintenance_over;
							this.butLog.Image = Resources.t_log;
						}
						else
						{
							if (name.Equals(this.butLog.Name))
							{
								EcoGlobalVar.stopalltimer(false);
								this.logPage1.Visible = true;
								this.CurSelMainTab = 4;
								this.butOutletAccess.Image = Resources.t_outletaccess;
								this.butUser.Image = Resources.t_usermanage;
								this.butDevice.Image = Resources.t_devicemanage;
								this.butSysManagement.Image = Resources.t_maintenance;
								this.butLog.Image = Resources.t_log_over;
							}
						}
					}
				}
			}
			this.pblogout.BringToFront();
			if (EcoGlobalVar.gl_LoginUser.UserType != 1)
			{
				this.pbmonitor.BringToFront();
			}
			else
			{
				this.pbmonitor.Visible = false;
				this.pbIdleTm.Location = this.pbmonitor.Location;
			}
			this.pbIdleTm.BringToFront();
			this.butOutletAccess.ForeColor = Color.FromArgb(0, 168, 150);
			this.butUser.ForeColor = Color.FromArgb(0, 168, 150);
			this.butDevice.ForeColor = Color.FromArgb(0, 168, 150);
			this.butSysManagement.ForeColor = Color.FromArgb(0, 168, 150);
			this.butLog.ForeColor = Color.FromArgb(0, 168, 150);
			((Button)sender).ForeColor = Color.FromArgb(211, 211, 212);
		}
		private void comm_butMouseHover(object sender, System.EventArgs e)
		{
			this.toolTip1.SetToolTip((Button)sender, ((Button)sender).Text);
		}
		private void comm_butMouseLeave(object sender, System.EventArgs e)
		{
			string name = ((Button)sender).Name;
			if (name.Equals(this.butOutletAccess.Name))
			{
				if (this.CurSelMainTab != 0)
				{
					this.butOutletAccess.Image = Resources.t_outletaccess;
					return;
				}
			}
			else
			{
				if (name.Equals(this.butUser.Name))
				{
					if (this.CurSelMainTab != 1)
					{
						this.butUser.Image = Resources.t_usermanage;
						return;
					}
				}
				else
				{
					if (name.Equals(this.butDevice.Name))
					{
						if (this.CurSelMainTab != 2)
						{
							this.butDevice.Image = Resources.t_devicemanage;
							return;
						}
					}
					else
					{
						if (name.Equals(this.butSysManagement.Name))
						{
							if (this.CurSelMainTab != 3)
							{
								this.butSysManagement.Image = Resources.t_maintenance;
								return;
							}
						}
						else
						{
							if (name.Equals(this.butLog.Name) && this.CurSelMainTab != 4)
							{
								this.butLog.Image = Resources.t_log;
							}
						}
					}
				}
			}
		}
		private void comm_butMouseMove(object sender, MouseEventArgs e)
		{
			string name = ((Button)sender).Name;
			if (name.Equals(this.butOutletAccess.Name))
			{
				this.butOutletAccess.Image = Resources.t_outletaccess_over;
				return;
			}
			if (name.Equals(this.butUser.Name))
			{
				this.butUser.Image = Resources.t_usermanage_over;
				return;
			}
			if (name.Equals(this.butDevice.Name))
			{
				this.butDevice.Image = Resources.t_devicemanage_over;
				return;
			}
			if (name.Equals(this.butSysManagement.Name))
			{
				this.butSysManagement.Image = Resources.t_maintenance_over;
				return;
			}
			if (name.Equals(this.butLog.Name))
			{
				this.butLog.Image = Resources.t_log_over;
			}
		}
		private void butOutletAccess_Click(object sender, System.EventArgs e)
		{
			this.comm_butClick(sender, e);
			this.enegManPage1.pageInit(0);
		}
		private void butUser_Click(object sender, System.EventArgs e)
		{
			this.comm_butClick(sender, e);
			this.userManPage1.pageInit(0);
		}
		private void butDevice_Click(object sender, System.EventArgs e)
		{
			this.comm_butClick(sender, e);
			this.devManPage1.pageInit(0);
		}
		private void butSysManagement_Click(object sender, System.EventArgs e)
		{
			this.comm_butClick(sender, e);
			this.sysManPage1.pageInit(0);
		}
		private void butSystemLog_Click(object sender, System.EventArgs e)
		{
			this.comm_butClick(sender, e);
			this.logPage1.pageInit(0);
		}
		private void logout_LinkClicked(object sender, System.EventArgs e)
		{
			if (EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Login_quit, new string[0]), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				this.Logout();
				ClientAPI.Logout();
				ClientAPI.StopBroadcastChannel();
				EcoGlobalVar.stopalltimer(true);
				Program.ExitApp();
			}
		}
		private void Logout()
		{
			string para = "0230002\n" + EcoGlobalVar.gl_LoginUser.UserName;
			ClientAPI.RemoteCall(100, 1, para, 10000);
		}
		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			Process.Start("IEXPLORE.EXE", "http://www.aten.com");
		}
		private void lnklogout_MouseHover(object sender, System.EventArgs e)
		{
			this.toolTip1.SetToolTip(this.pblogout, EcoLanguage.getMsg(LangRes.Logout_txt, new string[0]));
		}
		private void pbmonitor_MouseHover(object sender, System.EventArgs e)
		{
			this.toolTip1.SetToolTip(this.pbmonitor, EcoLanguage.getMsg(LangRes.Monitor_txt, new string[0]));
		}
		private void pbIdleTm_MouseHover(object sender, System.EventArgs e)
		{
			this.toolTip1.SetToolTip(this.pbIdleTm, EcoLanguage.getMsg(LangRes.IdleTime_txt, new string[0]));
		}
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Login_quit, new string[0]), MessageBoxButtons.YesNo) == DialogResult.No)
			{
				e.Cancel = true;
				return;
			}
			this.Logout();
			ClientAPI.StopBroadcastChannel();
			EcoGlobalVar.stopalltimer(true);
			Program.ExitApp();
		}
		private void pbmonitor_Click(object sender, System.EventArgs e)
		{
			if (this.m_pWinmonitor == null || this.m_pWinmonitor.IsDisposed)
			{
				System.Console.WriteLine("ERRRRR----pbmonitor_Click");
			}
			if (this.m_pWinmonitor.Visible)
			{
				this.m_pWinmonitor.WindowState = FormWindowState.Minimized;
				return;
			}
			this.m_pWinmonitor.pageInit(0);
			try
			{
				this.m_pWinmonitor.Show();
			}
			catch (System.Exception)
			{
			}
			if (this.m_pWinmonitor.WindowState == FormWindowState.Minimized)
			{
				MainForm.ShowWindow(this.m_pWinmonitor.Handle, 9u);
			}
			this.m_pWinmonitor.starBoardTimer();
		}
		private void pbIdleTm_Click(object sender, System.EventArgs e)
		{
			IdleTmDlg idleTmDlg = new IdleTmDlg();
			idleTmDlg.ShowDialog();
		}
	}
}
