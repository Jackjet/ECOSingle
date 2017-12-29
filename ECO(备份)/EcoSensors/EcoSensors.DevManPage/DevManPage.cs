using EcoSensors.DevManDevice;
using EcoSensors.DevManPage.DataGroup;
using EcoSensors.DevManPage.OtherDevices;
using EcoSensors.DevManRack;
using EcoSensors.DevManZone;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManPage
{
	public class DevManPage : UserControl
	{
		public const int RackBoard_state_INIT = 0;
		public const int RackBoard_state_UPDATE = 1;
		public const int RackBoard_state_OK = 2;
		public const int zoneBoard_state_INIT = 0;
		public const int zoneBoard_state_UPDATE = 1;
		public const int zoneBoard_state_OK = 2;
		private int rackBoardFlushFlg;
		private int zoneBoardFlushFlg;
		private IContainer components;
		private FlowLayoutPanel flowLayoutPanelDevManPage;
		private Button butDevSetup;
        private DevManDevice.DevManDevice devManDevice1;
		private Panel panel1;
		private Panel panel2;
		private Button butDataGroup;
        private DataGroup.DataGroup dataGroup1;
		private ManRack manRack1;
		private Button butRackNew;
		private Button butZoneNew;
		private ManZone manZone1;
		private Button butOtherDevice;
        private OtherDevices.OtherDevices otherDevices1;
		public int FlushFlg_RackBoard
		{
			get
			{
				return this.rackBoardFlushFlg;
			}
			set
			{
				if (this.rackBoardFlushFlg == 0 && value != 2)
				{
					return;
				}
				this.rackBoardFlushFlg = value;
			}
		}
		public int FlushFlg_ZoneBoard
		{
			get
			{
				return this.zoneBoardFlushFlg;
			}
			set
			{
				if (this.zoneBoardFlushFlg == 0 && value != 2)
				{
					return;
				}
				this.zoneBoardFlushFlg = value;
			}
		}
		public DevManPage()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			if (!EcoGlobalVar.gl_supportISG)
			{
				this.butOtherDevice.Visible = false;
			}
			else
			{
				this.butOtherDevice.Visible = true;
			}
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butZoneNew, null);
				return;
			case 1:
				this.comm_butClick(this.butRackNew, null);
				return;
			case 2:
				this.comm_butClick(this.butDevSetup, null);
				return;
			case 3:
				this.comm_butClick(this.butDataGroup, null);
				return;
			case 4:
				this.comm_butClick(this.butOtherDevice, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (Control control in this.flowLayoutPanelDevManPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butZoneNew.Name))
					{
						num = 0;
					}
					else
					{
						if (name.Equals(this.butRackNew.Name))
						{
							num = 1;
						}
						else
						{
							if (name.Equals(this.butDevSetup.Name))
							{
								num = 2;
							}
							else
							{
								if (name.Equals(this.butDataGroup.Name))
								{
									num = 3;
								}
								else
								{
									if (name.Equals(this.butOtherDevice.Name))
									{
										num = 4;
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
			this.manZone1.Visible = false;
			this.manRack1.Visible = false;
			this.devManDevice1.Visible = false;
			this.dataGroup1.Visible = false;
			this.otherDevices1.Visible = false;
			switch (num)
			{
			case 0:
				this.endZoneBicker();
				EcoGlobalVar.gl_DevManCtrl.endTimer();
				EcoGlobalVar.gl_otherDevCtrl.endTimer();
				this.manZone1.init();
				this.manZone1.Visible = true;
				this.manZone1.startBicker();
				return;
			case 1:
				this.endZoneBicker();
				EcoGlobalVar.gl_DevManCtrl.endTimer();
				EcoGlobalVar.gl_otherDevCtrl.endTimer();
				this.manRack1.init();
				this.manRack1.Visible = true;
				return;
			case 2:
				this.endZoneBicker();
				EcoGlobalVar.gl_DevManCtrl.endTimer();
				EcoGlobalVar.gl_otherDevCtrl.endTimer();
				this.devManDevice1.pageInit();
				this.devManDevice1.Visible = true;
				return;
			case 3:
				this.endZoneBicker();
				EcoGlobalVar.gl_DevManCtrl.endTimer();
				EcoGlobalVar.gl_otherDevCtrl.endTimer();
				this.dataGroup1.pageInit();
				this.dataGroup1.Visible = true;
				return;
			case 4:
				this.endZoneBicker();
				EcoGlobalVar.gl_DevManCtrl.endTimer();
				EcoGlobalVar.gl_otherDevCtrl.endTimer();
				this.otherDevices1.pageInit();
				this.otherDevices1.Visible = true;
				return;
			default:
				return;
			}
		}
		public void endZoneBicker()
		{
			this.manZone1.endBicker();
		}
		private void DevManPage_VisibleChanged(object sender, System.EventArgs e)
		{
			if (!base.Visible)
			{
				this.manRack1.closetips();
				this.manRack1.Visible = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DevManPage));
			this.flowLayoutPanelDevManPage = new FlowLayoutPanel();
			this.butZoneNew = new Button();
			this.butRackNew = new Button();
			this.butDevSetup = new Button();
			this.butDataGroup = new Button();
			this.butOtherDevice = new Button();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
            this.otherDevices1 = new OtherDevices.OtherDevices();
			this.manZone1 = new ManZone();
			this.manRack1 = new ManRack();
            this.devManDevice1 = new DevManDevice.DevManDevice();
            this.dataGroup1 = new DataGroup.DataGroup();
			this.flowLayoutPanelDevManPage.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.flowLayoutPanelDevManPage.BackColor = Color.FromArgb(18, 160, 143);
			this.flowLayoutPanelDevManPage.Controls.Add(this.butZoneNew);
			this.flowLayoutPanelDevManPage.Controls.Add(this.butRackNew);
			this.flowLayoutPanelDevManPage.Controls.Add(this.butDevSetup);
			this.flowLayoutPanelDevManPage.Controls.Add(this.butDataGroup);
			this.flowLayoutPanelDevManPage.Controls.Add(this.butOtherDevice);
			componentResourceManager.ApplyResources(this.flowLayoutPanelDevManPage, "flowLayoutPanelDevManPage");
			this.flowLayoutPanelDevManPage.MinimumSize = new Size(889, 27);
			this.flowLayoutPanelDevManPage.Name = "flowLayoutPanelDevManPage";
			componentResourceManager.ApplyResources(this.butZoneNew, "butZoneNew");
			this.butZoneNew.BackColor = Color.FromArgb(18, 160, 143);
			this.butZoneNew.MinimumSize = new Size(160, 27);
			this.butZoneNew.Name = "butZoneNew";
			this.butZoneNew.Tag = "Tag_ZoneDefine";
			this.butZoneNew.UseVisualStyleBackColor = true;
			this.butZoneNew.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butRackNew, "butRackNew");
			this.butRackNew.BackColor = Color.FromArgb(18, 160, 143);
			this.butRackNew.MinimumSize = new Size(160, 27);
			this.butRackNew.Name = "butRackNew";
			this.butRackNew.Tag = "Tag_TestRack";
			this.butRackNew.UseVisualStyleBackColor = true;
			this.butRackNew.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butDevSetup, "butDevSetup");
			this.butDevSetup.MinimumSize = new Size(160, 27);
			this.butDevSetup.Name = "butDevSetup";
			this.butDevSetup.Tag = "PDU Setup";
			this.butDevSetup.UseVisualStyleBackColor = true;
			this.butDevSetup.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butDataGroup, "butDataGroup");
			this.butDataGroup.MinimumSize = new Size(160, 27);
			this.butDataGroup.Name = "butDataGroup";
			this.butDataGroup.Tag = "Tag_datagroup";
			this.butDataGroup.UseVisualStyleBackColor = true;
			this.butDataGroup.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butOtherDevice, "butOtherDevice");
			this.butOtherDevice.MinimumSize = new Size(160, 27);
			this.butOtherDevice.Name = "butOtherDevice";
			this.butOtherDevice.Tag = "Tag_OtherDevice";
			this.butOtherDevice.UseVisualStyleBackColor = true;
			this.butOtherDevice.Click += new System.EventHandler(this.comm_butClick);
			this.panel1.BackColor = Color.FromArgb(18, 160, 143);
			this.panel1.Controls.Add(this.flowLayoutPanelDevManPage);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.panel2.Controls.Add(this.otherDevices1);
			this.panel2.Controls.Add(this.manZone1);
			this.panel2.Controls.Add(this.manRack1);
			this.panel2.Controls.Add(this.devManDevice1);
			this.panel2.Controls.Add(this.dataGroup1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.otherDevices1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.otherDevices1, "otherDevices1");
			this.otherDevices1.Name = "otherDevices1";
			this.manZone1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.manZone1, "manZone1");
			this.manZone1.Name = "manZone1";
			this.manRack1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.manRack1, "manRack1");
			this.manRack1.Name = "manRack1";
			this.devManDevice1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.devManDevice1, "devManDevice1");
			this.devManDevice1.Name = "devManDevice1";
			this.dataGroup1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.dataGroup1, "dataGroup1");
			this.dataGroup1.Name = "dataGroup1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DevManPage";
			base.VisibleChanged += new System.EventHandler(this.DevManPage_VisibleChanged);
			this.flowLayoutPanelDevManPage.ResumeLayout(false);
			this.flowLayoutPanelDevManPage.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
