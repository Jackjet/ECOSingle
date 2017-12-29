using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.DevManDevice._Dev;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice
{
	public class DevMan_DevTB : UserControl
	{
		private bool m_cleartab;
		private bool m_ininit;
		private TabPage m_selectedTab;
		private string m_curdevID;
		private bool m_onlinest;
		private IContainer components;
		private TabControl tcDev;
		private TabPage tbDevice;
		private TabPage tbOutlet;
		private TabPage tbSensor;
		private TabPage tbBank;
		private PropDev propDev1;
		private PropOutlet propOutlet1;
		private PropSensor propSensor1;
		private PropBank propBank1;
		private TabPage tbPop;
		private PropPOP propPOP1;
		private TabPage tbLine;
		private PropLine propLine1;
		public DevMan_DevTB()
		{
			this.InitializeComponent();
		}
		public void pageInit(DevManDevice pParent, int devID, string devName, bool onlinest)
		{
			this.m_ininit = true;
			this.m_cleartab = true;
			this.tcDev.Controls.Clear();
			this.m_cleartab = false;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID);
			if (deviceByID == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DevInfo_nofind, new string[]
				{
					devName
				}));
				return;
			}
			this.m_onlinest = onlinest;
			this.m_curdevID = devID.ToString();
			this.tcDev.Controls.Add(this.tbDevice);
			this.propDev1.pageInit(pParent, devID, onlinest);
			if (deviceByID.GetPortInfo().Count > 0)
			{
				this.tcDev.Controls.Add(this.tbOutlet);
				this.propOutlet1.pageInit(devID, 0, onlinest);
			}
			if (deviceByID.GetBankInfo().Count > 0)
			{
				this.tcDev.Controls.Add(this.tbBank);
				this.propBank1.pageInit(devID, 0, onlinest);
			}
			if (deviceByID.GetLineInfo().Count > 0)
			{
				this.tcDev.Controls.Add(this.tbLine);
				this.propLine1.pageInit(devID, 0, onlinest);
			}
			if (deviceByID.GetSensorInfo().Count > 0)
			{
				this.tcDev.Controls.Add(this.tbSensor);
				this.propSensor1.pageInit(devID, onlinest);
			}
			if (DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion).popReading == 2)
			{
				this.tcDev.Controls.Add(this.tbPop);
				this.propPOP1.pageInit(devID, onlinest);
			}
			if (this.m_selectedTab == null || !this.tcDev.Contains(this.m_selectedTab))
			{
				this.m_selectedTab = this.tbDevice;
			}
			this.tcDev.SelectTab(this.m_selectedTab);
			this.m_ininit = false;
		}
		public void TimerProc(int haveThresholdChange)
		{
			if (this.m_ininit)
			{
				return;
			}
			bool onlinest = ClientAPI.IsDeviceOnline(System.Convert.ToInt32(this.m_curdevID));
			this.m_onlinest = onlinest;
			if (this.m_selectedTab == this.tbDevice)
			{
				this.propDev1.TimerProc(onlinest, haveThresholdChange);
				return;
			}
			if (this.m_selectedTab == this.tbBank)
			{
				this.propBank1.TimerProc(onlinest, haveThresholdChange);
				return;
			}
			if (this.m_selectedTab == this.tbLine)
			{
				this.propLine1.TimerProc(onlinest, haveThresholdChange);
				return;
			}
			if (this.m_selectedTab == this.tbOutlet)
			{
				this.propOutlet1.TimerProc(onlinest, haveThresholdChange);
				return;
			}
			if (this.m_selectedTab == this.tbSensor)
			{
				this.propSensor1.TimerProc(onlinest, haveThresholdChange);
				return;
			}
			if (this.m_selectedTab == this.tbPop)
			{
				this.propPOP1.TimerProc(onlinest, haveThresholdChange);
			}
		}
		private void tcDev_Selected(object sender, TabControlEventArgs e)
		{
			if (this.m_cleartab)
			{
				return;
			}
			TabControl tabControl = (TabControl)sender;
			if (tabControl.SelectedTab == null)
			{
				return;
			}
			this.m_selectedTab = tabControl.SelectedTab;
			if (this.m_selectedTab == this.tbDevice)
			{
				this.propDev1.TimerProc(this.m_onlinest, 1);
				return;
			}
			if (this.m_selectedTab == this.tbBank)
			{
				this.propBank1.TimerProc(this.m_onlinest, 1);
				return;
			}
			if (this.m_selectedTab == this.tbLine)
			{
				this.propLine1.TimerProc(this.m_onlinest, 1);
				return;
			}
			if (this.m_selectedTab == this.tbOutlet)
			{
				this.propOutlet1.TimerProc(this.m_onlinest, 1);
				return;
			}
			if (this.m_selectedTab == this.tbSensor)
			{
				this.propSensor1.TimerProc(this.m_onlinest, 1);
				return;
			}
			if (this.m_selectedTab == this.tbPop)
			{
				this.propPOP1.TimerProc(this.m_onlinest, 1);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DevMan_DevTB));
			this.tcDev = new TabControl();
			this.tbDevice = new TabPage();
			this.propDev1 = new PropDev();
			this.tbOutlet = new TabPage();
			this.propOutlet1 = new PropOutlet();
			this.tbSensor = new TabPage();
			this.propSensor1 = new PropSensor();
			this.tbBank = new TabPage();
			this.propBank1 = new PropBank();
			this.tbPop = new TabPage();
			this.propPOP1 = new PropPOP();
			this.tbLine = new TabPage();
			this.propLine1 = new PropLine();
			this.tcDev.SuspendLayout();
			this.tbDevice.SuspendLayout();
			this.tbOutlet.SuspendLayout();
			this.tbSensor.SuspendLayout();
			this.tbBank.SuspendLayout();
			this.tbPop.SuspendLayout();
			this.tbLine.SuspendLayout();
			base.SuspendLayout();
			this.tcDev.Controls.Add(this.tbDevice);
			this.tcDev.Controls.Add(this.tbOutlet);
			this.tcDev.Controls.Add(this.tbSensor);
			this.tcDev.Controls.Add(this.tbBank);
			this.tcDev.Controls.Add(this.tbLine);
			this.tcDev.Controls.Add(this.tbPop);
			componentResourceManager.ApplyResources(this.tcDev, "tcDev");
			this.tcDev.Name = "tcDev";
			this.tcDev.SelectedIndex = 0;
			this.tcDev.Selected += new TabControlEventHandler(this.tcDev_Selected);
			this.tbDevice.Controls.Add(this.propDev1);
			componentResourceManager.ApplyResources(this.tbDevice, "tbDevice");
			this.tbDevice.Name = "tbDevice";
			this.tbDevice.UseVisualStyleBackColor = true;
			this.propDev1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propDev1, "propDev1");
			this.propDev1.Name = "propDev1";
			this.tbOutlet.Controls.Add(this.propOutlet1);
			componentResourceManager.ApplyResources(this.tbOutlet, "tbOutlet");
			this.tbOutlet.Name = "tbOutlet";
			this.tbOutlet.UseVisualStyleBackColor = true;
			this.propOutlet1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propOutlet1, "propOutlet1");
			this.propOutlet1.Name = "propOutlet1";
			this.tbSensor.Controls.Add(this.propSensor1);
			componentResourceManager.ApplyResources(this.tbSensor, "tbSensor");
			this.tbSensor.Name = "tbSensor";
			this.tbSensor.UseVisualStyleBackColor = true;
			this.propSensor1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propSensor1, "propSensor1");
			this.propSensor1.Name = "propSensor1";
			this.tbBank.Controls.Add(this.propBank1);
			componentResourceManager.ApplyResources(this.tbBank, "tbBank");
			this.tbBank.Name = "tbBank";
			this.tbBank.UseVisualStyleBackColor = true;
			this.propBank1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propBank1, "propBank1");
			this.propBank1.Name = "propBank1";
			this.tbPop.Controls.Add(this.propPOP1);
			componentResourceManager.ApplyResources(this.tbPop, "tbPop");
			this.tbPop.Name = "tbPop";
			this.tbPop.UseVisualStyleBackColor = true;
			this.propPOP1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propPOP1, "propPOP1");
			this.propPOP1.Name = "propPOP1";
			this.tbLine.Controls.Add(this.propLine1);
			componentResourceManager.ApplyResources(this.tbLine, "tbLine");
			this.tbLine.Name = "tbLine";
			this.tbLine.UseVisualStyleBackColor = true;
			this.propLine1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propLine1, "propLine1");
			this.propLine1.Name = "propLine1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.tcDev);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DevMan_DevTB";
			this.tcDev.ResumeLayout(false);
			this.tbDevice.ResumeLayout(false);
			this.tbOutlet.ResumeLayout(false);
			this.tbSensor.ResumeLayout(false);
			this.tbBank.ResumeLayout(false);
			this.tbPop.ResumeLayout(false);
			this.tbLine.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
