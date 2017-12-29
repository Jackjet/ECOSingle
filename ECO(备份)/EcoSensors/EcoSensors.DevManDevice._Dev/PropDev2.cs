using CommonAPI.CultureTransfer;
using CommonAPI.Global;
using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.DevAccess;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice._Dev
{
	public class PropDev2 : UserControl
	{
		private IContainer components;
		private GroupBox groupBox2;
		private Label label3;
		private Label label2;
		private Label label1;
		private Label label19;
		private Label labMaxPowerBound;
		private Label labMaxVoltageBound;
		private Label label101;
		private Label labMaxCurrentBound;
		private TextBox tbMinCurrent;
		private Label labMaxPowerDisBound;
		private TextBox tbMaxCurrent;
		private Label label103;
		private TextBox tbMinVoltage;
		private TextBox tbMaxVoltage;
		private Label label106;
		private TextBox tbMinPower;
		private Label label128;
		private TextBox tbMaxPower;
		private Label label127;
		private Label lbunitPower;
		private Label label126;
		private TextBox tbMinPowerDiss;
		private Label label112;
		private TextBox tbMaxPowerDiss;
		private Button butSave;
		private Button butAssign;
		private GroupBox groupBox1;
		private Label label4;
		private Label label11;
		private Label label14;
		private Label labDevRackNm;
		private Label labDevNm;
		private Label label16;
		private Label labDevModel;
		private Label labDevIp;
		private GroupBox gbDoorSS;
		private RadioButton rbReed_3;
		private RadioButton rbInductive_2;
		private RadioButton rbPhoto_1;
		private RadioButton rbNoInstall_0;
		private ToolTip toolTip1;
		private TextBox tbRefVoltage;
		private Label lbRefVoltage;
		private Label lbRefVoltageUnit;
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropDev2));
			this.groupBox2 = new GroupBox();
			this.label3 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.label19 = new Label();
			this.labMaxPowerBound = new Label();
			this.labMaxVoltageBound = new Label();
			this.label101 = new Label();
			this.labMaxCurrentBound = new Label();
			this.tbMinCurrent = new TextBox();
			this.labMaxPowerDisBound = new Label();
			this.tbMaxCurrent = new TextBox();
			this.label103 = new Label();
			this.tbMinVoltage = new TextBox();
			this.tbMaxVoltage = new TextBox();
			this.label106 = new Label();
			this.tbMinPower = new TextBox();
			this.label128 = new Label();
			this.tbMaxPower = new TextBox();
			this.label127 = new Label();
			this.lbunitPower = new Label();
			this.label126 = new Label();
			this.tbMinPowerDiss = new TextBox();
			this.label112 = new Label();
			this.tbMaxPowerDiss = new TextBox();
			this.butSave = new Button();
			this.butAssign = new Button();
			this.groupBox1 = new GroupBox();
			this.lbRefVoltageUnit = new Label();
			this.tbRefVoltage = new TextBox();
			this.lbRefVoltage = new Label();
			this.label4 = new Label();
			this.label11 = new Label();
			this.label14 = new Label();
			this.labDevRackNm = new Label();
			this.labDevNm = new Label();
			this.label16 = new Label();
			this.labDevModel = new Label();
			this.labDevIp = new Label();
			this.gbDoorSS = new GroupBox();
			this.rbReed_3 = new RadioButton();
			this.rbInductive_2 = new RadioButton();
			this.rbPhoto_1 = new RadioButton();
			this.rbNoInstall_0 = new RadioButton();
			this.toolTip1 = new ToolTip(this.components);
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.gbDoorSS.SuspendLayout();
			base.SuspendLayout();
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label19);
			this.groupBox2.Controls.Add(this.labMaxPowerBound);
			this.groupBox2.Controls.Add(this.labMaxVoltageBound);
			this.groupBox2.Controls.Add(this.label101);
			this.groupBox2.Controls.Add(this.labMaxCurrentBound);
			this.groupBox2.Controls.Add(this.tbMinCurrent);
			this.groupBox2.Controls.Add(this.labMaxPowerDisBound);
			this.groupBox2.Controls.Add(this.tbMaxCurrent);
			this.groupBox2.Controls.Add(this.label103);
			this.groupBox2.Controls.Add(this.tbMinVoltage);
			this.groupBox2.Controls.Add(this.tbMaxVoltage);
			this.groupBox2.Controls.Add(this.label106);
			this.groupBox2.Controls.Add(this.tbMinPower);
			this.groupBox2.Controls.Add(this.label128);
			this.groupBox2.Controls.Add(this.tbMaxPower);
			this.groupBox2.Controls.Add(this.label127);
			this.groupBox2.Controls.Add(this.lbunitPower);
			this.groupBox2.Controls.Add(this.label126);
			this.groupBox2.Controls.Add(this.tbMinPowerDiss);
			this.groupBox2.Controls.Add(this.label112);
			this.groupBox2.Controls.Add(this.tbMaxPowerDiss);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = Color.Black;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = Color.Black;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Black;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label19, "label19");
			this.label19.ForeColor = Color.Black;
			this.label19.Name = "label19";
			componentResourceManager.ApplyResources(this.labMaxPowerBound, "labMaxPowerBound");
			this.labMaxPowerBound.ForeColor = Color.Red;
			this.labMaxPowerBound.Name = "labMaxPowerBound";
			componentResourceManager.ApplyResources(this.labMaxVoltageBound, "labMaxVoltageBound");
			this.labMaxVoltageBound.ForeColor = Color.Red;
			this.labMaxVoltageBound.Name = "labMaxVoltageBound";
			this.label101.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.label101, "label101");
			this.label101.ForeColor = Color.Black;
			this.label101.Name = "label101";
			componentResourceManager.ApplyResources(this.labMaxCurrentBound, "labMaxCurrentBound");
			this.labMaxCurrentBound.ForeColor = Color.Red;
			this.labMaxCurrentBound.Name = "labMaxCurrentBound";
			this.tbMinCurrent.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbMinCurrent, "tbMinCurrent");
			this.tbMinCurrent.ForeColor = Color.Black;
			this.tbMinCurrent.Name = "tbMinCurrent";
			this.tbMinCurrent.Tag = "current";
			this.tbMinCurrent.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.labMaxPowerDisBound, "labMaxPowerDisBound");
			this.labMaxPowerDisBound.ForeColor = Color.Red;
			this.labMaxPowerDisBound.Name = "labMaxPowerDisBound";
			this.tbMaxCurrent.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxCurrent, "tbMaxCurrent");
			this.tbMaxCurrent.ForeColor = Color.Black;
			this.tbMaxCurrent.Name = "tbMaxCurrent";
			this.tbMaxCurrent.Tag = "current";
			this.tbMaxCurrent.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label103, "label103");
			this.label103.ForeColor = Color.Black;
			this.label103.Name = "label103";
			this.tbMinVoltage.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinVoltage, "tbMinVoltage");
			this.tbMinVoltage.ForeColor = Color.Black;
			this.tbMinVoltage.Name = "tbMinVoltage";
			this.tbMinVoltage.Tag = "voltage";
			this.tbMinVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.tbMaxVoltage.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxVoltage, "tbMaxVoltage");
			this.tbMaxVoltage.ForeColor = Color.Black;
			this.tbMaxVoltage.Name = "tbMaxVoltage";
			this.tbMaxVoltage.Tag = "voltage";
			this.tbMaxVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label106, "label106");
			this.label106.ForeColor = Color.Black;
			this.label106.Name = "label106";
			this.tbMinPower.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbMinPower, "tbMinPower");
			this.tbMinPower.ForeColor = Color.Black;
			this.tbMinPower.Name = "tbMinPower";
			this.tbMinPower.ReadOnly = true;
			this.tbMinPower.Tag = "power";
			this.tbMinPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label128, "label128");
			this.label128.ForeColor = Color.Black;
			this.label128.Name = "label128";
			this.tbMaxPower.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxPower, "tbMaxPower");
			this.tbMaxPower.ForeColor = Color.Black;
			this.tbMaxPower.Name = "tbMaxPower";
			this.tbMaxPower.Tag = "power";
			this.tbMaxPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label127, "label127");
			this.label127.ForeColor = Color.Black;
			this.label127.Name = "label127";
			componentResourceManager.ApplyResources(this.lbunitPower, "lbunitPower");
			this.lbunitPower.ForeColor = Color.Black;
			this.lbunitPower.Name = "lbunitPower";
			componentResourceManager.ApplyResources(this.label126, "label126");
			this.label126.ForeColor = Color.Black;
			this.label126.Name = "label126";
			this.tbMinPowerDiss.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbMinPowerDiss, "tbMinPowerDiss");
			this.tbMinPowerDiss.ForeColor = Color.Black;
			this.tbMinPowerDiss.Name = "tbMinPowerDiss";
			this.tbMinPowerDiss.ReadOnly = true;
			this.tbMinPowerDiss.Tag = "powerDiss";
			this.tbMinPowerDiss.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label112, "label112");
			this.label112.ForeColor = Color.Black;
			this.label112.Name = "label112";
			this.tbMaxPowerDiss.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxPowerDiss, "tbMaxPowerDiss");
			this.tbMaxPowerDiss.ForeColor = Color.Black;
			this.tbMaxPowerDiss.Name = "tbMaxPowerDiss";
			this.tbMaxPowerDiss.Tag = "powerDiss";
			this.tbMaxPowerDiss.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.butSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSave, "butSave");
			this.butSave.Name = "butSave";
			this.butSave.UseVisualStyleBackColor = false;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			this.butAssign.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butAssign, "butAssign");
			this.butAssign.Name = "butAssign";
			this.butAssign.UseVisualStyleBackColor = false;
			this.butAssign.Click += new System.EventHandler(this.butAssign_Click);
			this.groupBox1.Controls.Add(this.lbRefVoltageUnit);
			this.groupBox1.Controls.Add(this.tbRefVoltage);
			this.groupBox1.Controls.Add(this.lbRefVoltage);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.labDevRackNm);
			this.groupBox1.Controls.Add(this.labDevNm);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.labDevModel);
			this.groupBox1.Controls.Add(this.labDevIp);
			this.groupBox1.ForeColor = SystemColors.ControlText;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.lbRefVoltageUnit, "lbRefVoltageUnit");
			this.lbRefVoltageUnit.ForeColor = Color.Black;
			this.lbRefVoltageUnit.Name = "lbRefVoltageUnit";
			this.tbRefVoltage.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbRefVoltage, "tbRefVoltage");
			this.tbRefVoltage.ForeColor = Color.Black;
			this.tbRefVoltage.Name = "tbRefVoltage";
			this.tbRefVoltage.Tag = "current";
			this.tbRefVoltage.KeyPress += new KeyPressEventHandler(this.tbRefVoltage_KeyPress);
			componentResourceManager.ApplyResources(this.lbRefVoltage, "lbRefVoltage");
			this.lbRefVoltage.ForeColor = Color.Black;
			this.lbRefVoltage.Name = "lbRefVoltage";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = Color.Black;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.ForeColor = Color.Black;
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.ForeColor = Color.Black;
			this.label14.Name = "label14";
			this.labDevRackNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevRackNm, "labDevRackNm");
			this.labDevRackNm.ForeColor = Color.Black;
			this.labDevRackNm.Name = "labDevRackNm";
			this.labDevNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevNm, "labDevNm");
			this.labDevNm.ForeColor = Color.Black;
			this.labDevNm.Name = "labDevNm";
			componentResourceManager.ApplyResources(this.label16, "label16");
			this.label16.ForeColor = Color.Black;
			this.label16.Name = "label16";
			this.labDevModel.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevModel, "labDevModel");
			this.labDevModel.ForeColor = Color.Black;
			this.labDevModel.Name = "labDevModel";
			this.labDevIp.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevIp, "labDevIp");
			this.labDevIp.ForeColor = Color.Black;
			this.labDevIp.Name = "labDevIp";
			this.gbDoorSS.Controls.Add(this.rbReed_3);
			this.gbDoorSS.Controls.Add(this.rbInductive_2);
			this.gbDoorSS.Controls.Add(this.rbPhoto_1);
			this.gbDoorSS.Controls.Add(this.rbNoInstall_0);
			componentResourceManager.ApplyResources(this.gbDoorSS, "gbDoorSS");
			this.gbDoorSS.Name = "gbDoorSS";
			this.gbDoorSS.TabStop = false;
			componentResourceManager.ApplyResources(this.rbReed_3, "rbReed_3");
			this.rbReed_3.Name = "rbReed_3";
			this.rbReed_3.TabStop = true;
			this.rbReed_3.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.rbInductive_2, "rbInductive_2");
			this.rbInductive_2.Name = "rbInductive_2";
			this.rbInductive_2.TabStop = true;
			this.rbInductive_2.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.rbPhoto_1, "rbPhoto_1");
			this.rbPhoto_1.Name = "rbPhoto_1";
			this.rbPhoto_1.TabStop = true;
			this.rbPhoto_1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.rbNoInstall_0, "rbNoInstall_0");
			this.rbNoInstall_0.Name = "rbNoInstall_0";
			this.rbNoInstall_0.TabStop = true;
			this.rbNoInstall_0.UseVisualStyleBackColor = true;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.gbDoorSS);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.butAssign);
			base.Controls.Add(this.butSave);
			base.Controls.Add(this.groupBox2);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropDev2";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.gbDoorSS.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public PropDev2()
		{
			this.InitializeComponent();
			this.tbMinCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(int devID, bool onlinest)
		{
			this.butSave.Enabled = onlinest;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID);
			this.labDevNm.Text = deviceByID.DeviceName;
			this.labDevModel.Text = deviceByID.ModelNm;
			this.labDevModel.Tag = devID.ToString();
			string text = deviceByID.ModelNm;
			if (DevAccessCfg.GetInstance().isAutodectDev(deviceByID.ModelNm, deviceByID.FWVersion))
			{
				text = text + " (F/W: " + deviceByID.FWVersion + ")";
			}
			this.toolTip1.SetToolTip(this.labDevModel, text);
			this.labDevIp.Text = deviceByID.DeviceIP;
			this.labDevIp.Tag = deviceByID.FWVersion;
			this.labDevNm.Text = deviceByID.DeviceName;
			RackInfo rackByID = RackInfo.getRackByID(deviceByID.RackID);
			this.labDevRackNm.Text = rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
			if (deviceByID.ModelNm.Equals("EC1000"))
			{
				this.lbRefVoltage.Visible = true;
				this.tbRefVoltage.Visible = true;
				this.lbRefVoltageUnit.Visible = true;
				if (DevAccessCfg.GetInstance().isAutodectDev(deviceByID.ModelNm, deviceByID.FWVersion))
				{
					this.tbRefVoltage.Enabled = true;
					if (deviceByID.ReferenceVoltage == -500f || deviceByID.ReferenceVoltage == -300f)
					{
						this.tbRefVoltage.Text = "";
					}
					else
					{
						this.tbRefVoltage.Text = System.Convert.ToString(deviceByID.ReferenceVoltage);
					}
				}
				else
				{
					this.tbRefVoltage.Enabled = false;
					this.tbRefVoltage.Text = System.Convert.ToString(Sys_Para.GetEnergyValue());
				}
			}
			else
			{
				this.lbRefVoltage.Visible = false;
				this.tbRefVoltage.Visible = false;
				this.lbRefVoltageUnit.Visible = false;
			}
			int num = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "dev");
			ThresholdUtil.SetUIEdit(this.tbMinVoltage, (num & 4) == 0, deviceByID.Min_voltage, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbMaxVoltage, (num & 8) == 0, deviceByID.Max_voltage, 0, "F1");
			this.labMaxVoltageBound.Text = ((!this.tbMinVoltage.ReadOnly || !this.tbMaxVoltage.ReadOnly) ? devcfgUtil.RangeVoltage(deviceModelConfig, "dev", 0) : "");
			if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
			{
				ThresholdUtil.SetUIEdit(this.tbMinCurrent, (num & 1) == 0, deviceByID.Min_current, 0, "F0");
				ThresholdUtil.SetUIEdit(this.tbMaxCurrent, (num & 2) == 0, deviceByID.Max_current, 0, "F0");
				this.labMaxCurrentBound.Text = ((!this.tbMinCurrent.ReadOnly || !this.tbMaxCurrent.ReadOnly) ? devcfgUtil.RangeCurrent(deviceModelConfig, "dev", 0, "F0") : "");
				this.lbunitPower.Text = "kW";
				ThresholdUtil.SetUIEdit(this.tbMinPower, (num & 16) == 0, deviceByID.Min_power / 1000f, 0, "F1");
				ThresholdUtil.SetUIEdit(this.tbMaxPower, (num & 32) == 0, deviceByID.Max_power / 1000f, 0, "F1");
				this.labMaxPowerBound.Text = ((!this.tbMinPower.ReadOnly || !this.tbMaxPower.ReadOnly) ? devcfgUtil.RangePower(deviceModelConfig, "dev", 0, 1000.0) : "");
			}
			else
			{
				ThresholdUtil.SetUIEdit(this.tbMinCurrent, (num & 1) == 0, deviceByID.Min_current, 0, "F1");
				ThresholdUtil.SetUIEdit(this.tbMaxCurrent, (num & 2) == 0, deviceByID.Max_current, 0, "F1");
				this.labMaxCurrentBound.Text = ((!this.tbMinCurrent.ReadOnly || !this.tbMaxCurrent.ReadOnly) ? devcfgUtil.RangeCurrent(deviceModelConfig, "dev", 0, "F1") : "");
				this.lbunitPower.Text = "W";
				ThresholdUtil.SetUIEdit(this.tbMinPower, (num & 16) == 0, deviceByID.Min_power, 0, "F1");
				ThresholdUtil.SetUIEdit(this.tbMaxPower, (num & 32) == 0, deviceByID.Max_power, 0, "F1");
				this.labMaxPowerBound.Text = ((!this.tbMinPower.ReadOnly || !this.tbMaxPower.ReadOnly) ? devcfgUtil.RangePower(deviceModelConfig, "dev", 0, 1.0) : "");
			}
			ThresholdUtil.SetUIEdit(this.tbMinPowerDiss, (num & 64) == 0, deviceByID.Min_power_diss, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbMaxPowerDiss, (num & 128) == 0, deviceByID.Max_power_diss, 0, "F1");
			this.labMaxPowerDisBound.Text = ((!this.tbMinPowerDiss.ReadOnly || !this.tbMaxPowerDiss.ReadOnly) ? devcfgUtil.RangePowerDiss(deviceModelConfig, "dev", 0) : "");
			if (deviceModelConfig.doorReading != 2)
			{
				this.gbDoorSS.Hide();
				return;
			}
			this.gbDoorSS.Show();
			switch (deviceByID.DoorSensor)
			{
			case 0:
				this.rbNoInstall_0.Checked = true;
				return;
			case 1:
				this.rbPhoto_1.Checked = true;
				return;
			case 2:
				this.rbInductive_2.Checked = true;
				return;
			case 3:
				this.rbReed_3.Checked = true;
				return;
			default:
				return;
			}
		}
		public void TimerProc(bool onlinest, int haveThresholdChange)
		{
			if (haveThresholdChange == 1)
			{
				string value = this.labDevModel.Tag.ToString();
				int devID = System.Convert.ToInt32(value);
				this.pageInit(devID, onlinest);
				return;
			}
			this.butSave.Enabled = onlinest;
		}
		private void threshold_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(textBox, e.KeyChar, 1);
			if (flag)
			{
				char keyChar = e.KeyChar;
				if ((keyChar == '.' || keyChar == ',') && (textBox.Name.Equals(this.tbMinCurrent.Name) || textBox.Name.Equals(this.tbMaxCurrent.Name)))
				{
					string text = this.labDevModel.Text;
					string fmwareVer = this.labDevIp.Tag.ToString();
					if (DevAccessCfg.GetInstance().getDeviceModelConfig(text, fmwareVer).commonThresholdFlag == Constant.APC_PDU)
					{
						e.Handled = true;
					}
				}
				return;
			}
			e.Handled = true;
		}
		private bool devConfigCheck()
		{
			string value = this.labDevModel.Tag.ToString();
			string fmwareVer = this.labDevIp.Tag.ToString();
			System.Convert.ToInt32(value);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(this.labDevModel.Text, fmwareVer);
			bool flag = false;
			if (this.tbRefVoltage.Visible)
			{
				Ecovalidate.checkTextIsNull(this.tbRefVoltage, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbRefVoltage.Text
					}));
					return false;
				}
				if (!Ecovalidate.RangeDouble(this.tbRefVoltage, 90.0, 260.0))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.lbRefVoltage.Text,
						"90",
						"260"
					}));
					return false;
				}
			}
			flag = true;
			int num = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "dev");
			Ecovalidate.checkThresholdValue(this.tbMinCurrent, this.labMaxCurrentBound, (num & 256) == 0, ref flag);
			Ecovalidate.checkThresholdValue(this.tbMaxCurrent, this.labMaxCurrentBound, (num & 512) == 0, ref flag);
			Ecovalidate.checkThresholdValue(this.tbMinVoltage, this.labMaxVoltageBound, (num & 1024) == 0, ref flag);
			Ecovalidate.checkThresholdValue(this.tbMaxVoltage, this.labMaxVoltageBound, (num & 2048) == 0, ref flag);
			Ecovalidate.checkThresholdValue(this.tbMinPower, this.labMaxPowerBound, (num & 4096) == 0, ref flag);
			Ecovalidate.checkThresholdValue(this.tbMaxPower, this.labMaxPowerBound, (num & 8192) == 0, ref flag);
			Ecovalidate.checkThresholdValue(this.tbMinPowerDiss, this.labMaxPowerDisBound, (num & 16384) == 0, ref flag);
			Ecovalidate.checkThresholdValue(this.tbMaxPowerDiss, this.labMaxPowerDisBound, (num & 32768) == 0, ref flag);
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_Thresholdinvalid, new string[0]));
				return false;
			}
			Ecovalidate.checkThresholdMaxMixValue(this.tbMaxCurrent, this.tbMinCurrent, ref flag);
			Ecovalidate.checkThresholdMaxMixValue(this.tbMaxVoltage, this.tbMinVoltage, ref flag);
			Ecovalidate.checkThresholdMaxMixValue(this.tbMaxPower, this.tbMinPower, ref flag);
			Ecovalidate.checkThresholdMaxMixValue(this.tbMaxPowerDiss, this.tbMinPowerDiss, ref flag);
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdMinMax, new string[0]));
				return false;
			}
			return true;
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.devConfigCheck())
				{
					string text = this.labDevModel.Text;
					string value = this.labDevModel.Tag.ToString();
					int l_id = System.Convert.ToInt32(value);
					DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
					DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, deviceByID.FWVersion);
					if (this.tbRefVoltage.Visible)
					{
						deviceByID.ReferenceVoltage = System.Convert.ToSingle(this.tbRefVoltage.Text);
					}
					deviceByID.Min_current = ThresholdUtil.UI2DB(this.tbMinCurrent, deviceByID.Min_current, 0);
					deviceByID.Max_current = ThresholdUtil.UI2DB(this.tbMaxCurrent, deviceByID.Max_current, 0);
					deviceByID.Min_voltage = ThresholdUtil.UI2DB(this.tbMinVoltage, deviceByID.Min_voltage, 0);
					deviceByID.Max_voltage = ThresholdUtil.UI2DB(this.tbMaxVoltage, deviceByID.Max_voltage, 0);
					deviceByID.Min_power = ThresholdUtil.UI2DB(this.tbMinPower, deviceByID.Min_power, 0);
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU && deviceByID.Min_power != -300f)
					{
						deviceByID.Min_power *= 1000f;
					}
					deviceByID.Max_power = ThresholdUtil.UI2DB(this.tbMaxPower, deviceByID.Max_power, 0);
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU && deviceByID.Max_power != -300f)
					{
						deviceByID.Max_power *= 1000f;
					}
					deviceByID.Min_power_diss = ThresholdUtil.UI2DB(this.tbMinPowerDiss, deviceByID.Min_power_diss, 0);
					deviceByID.Max_power_diss = ThresholdUtil.UI2DB(this.tbMaxPowerDiss, deviceByID.Max_power_diss, 0);
					if (deviceModelConfig.doorReading == 2)
					{
						deviceByID.DoorSensor = 0;
						if (this.rbPhoto_1.Checked)
						{
							deviceByID.DoorSensor = 1;
						}
						else
						{
							if (this.rbInductive_2.Checked)
							{
								deviceByID.DoorSensor = 2;
							}
							else
							{
								if (this.rbReed_3.Checked)
								{
									deviceByID.DoorSensor = 3;
								}
							}
						}
					}
					string mac = deviceByID.Mac;
					DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
					DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
					DeviceThreshold deviceThreshold = new DeviceThreshold();
					int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "dev");
					deviceThreshold.MinCurrentMT = deviceByID.Min_current;
					deviceThreshold.MaxCurrentMT = deviceByID.Max_current;
					deviceThreshold.MinVoltageMT = deviceByID.Min_voltage;
					deviceThreshold.MaxVoltageMT = deviceByID.Max_voltage;
					deviceThreshold.MinPowerMT = deviceByID.Min_power;
					deviceThreshold.MaxPowerMT = deviceByID.Max_power;
					deviceThreshold.MaxPowerDissMT = deviceByID.Max_power_diss;
					ThresholdUtil.UI2Dev(thflg, deviceThreshold);
					deviceThreshold.DevReferenceVoltage = deviceByID.ReferenceVoltage;
					deviceThreshold.PopEnableMode = -500;
					deviceThreshold.PopThreshold = -500f;
					deviceThreshold.PopModeOutlet = -500;
					deviceThreshold.PopModeLIFO = -500;
					deviceThreshold.PopModePriority = -500;
					deviceThreshold.DoorSensorType = deviceByID.DoorSensor;
					if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold && !devAccessAPI.SetDeviceThreshold(deviceThreshold, mac))
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
					}
					else
					{
						deviceByID.Update();
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							LogAPI.writeEventLog("0630000", new string[]
							{
								deviceByID.DeviceName,
								deviceByID.Mac,
								deviceByID.DeviceIP,
								valuePair
							});
						}
						else
						{
							LogAPI.writeEventLog("0630000", new string[]
							{
								deviceByID.DeviceName,
								deviceByID.Mac,
								deviceByID.DeviceIP
							});
						}
						EcoGlobalVar.setDashBoardFlg(128uL, "#UPDATE#D" + deviceByID.DeviceID + ":;", 2);
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("PropDev Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private void butAssign_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.devConfigCheck())
				{
					DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Dev_ApplyAll, new string[0]), MessageBoxButtons.OKCancel);
					if (dialogResult != DialogResult.Cancel)
					{
						string text = this.labDevModel.Text;
						string value = this.labDevModel.Tag.ToString();
						int l_id = System.Convert.ToInt32(value);
						DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
						DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, deviceByID.FWVersion);
						System.Collections.Generic.List<DeviceInfo> allDeviceByModel = DeviceOperation.GetAllDeviceByModel(text);
						string fWVersion = deviceByID.FWVersion;
						System.Collections.Generic.List<DeviceInfo> list = new System.Collections.Generic.List<DeviceInfo>();
						foreach (DeviceInfo current in allDeviceByModel)
						{
							if (current.DeviceID != deviceByID.DeviceID)
							{
								if (!DevAccessCfg.GetInstance().isAutodectDev(current.ModelNm, current.FWVersion))
								{
									list.Add(current);
								}
								else
								{
									if (current.FWVersion.Equals(fWVersion))
									{
										list.Add(current);
									}
								}
							}
						}
						if (this.tbRefVoltage.Visible)
						{
							deviceByID.ReferenceVoltage = System.Convert.ToSingle(this.tbRefVoltage.Text);
						}
						deviceByID.Min_current = ThresholdUtil.UI2DB(this.tbMinCurrent, deviceByID.Min_current, 0);
						deviceByID.Max_current = ThresholdUtil.UI2DB(this.tbMaxCurrent, deviceByID.Max_current, 0);
						deviceByID.Min_voltage = ThresholdUtil.UI2DB(this.tbMinVoltage, deviceByID.Min_voltage, 0);
						deviceByID.Max_voltage = ThresholdUtil.UI2DB(this.tbMaxVoltage, deviceByID.Max_voltage, 0);
						deviceByID.Min_power = ThresholdUtil.UI2DB(this.tbMinPower, deviceByID.Min_power, 0);
						if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU && deviceByID.Min_power != -300f)
						{
							deviceByID.Min_power *= 1000f;
						}
						deviceByID.Max_power = ThresholdUtil.UI2DB(this.tbMaxPower, deviceByID.Max_power, 0);
						if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU && deviceByID.Max_power != -300f)
						{
							deviceByID.Max_power *= 1000f;
						}
						deviceByID.Min_power_diss = ThresholdUtil.UI2DB(this.tbMinPowerDiss, deviceByID.Min_power_diss, 0);
						deviceByID.Max_power_diss = ThresholdUtil.UI2DB(this.tbMaxPowerDiss, deviceByID.Max_power_diss, 0);
						if (deviceModelConfig.doorReading == 2)
						{
							deviceByID.DoorSensor = 0;
							if (this.rbPhoto_1.Checked)
							{
								deviceByID.DoorSensor = 1;
							}
							else
							{
								if (this.rbInductive_2.Checked)
								{
									deviceByID.DoorSensor = 2;
								}
								else
								{
									if (this.rbReed_3.Checked)
									{
										deviceByID.DoorSensor = 3;
									}
								}
							}
						}
						DeviceInfo item = new DeviceInfo(-1, "", "", "", "", "", "", "", 0, 0, "", 161, 1, "", deviceByID.Max_voltage, deviceByID.Min_voltage, deviceByID.Max_power_diss, deviceByID.Min_power_diss, deviceByID.Max_power, deviceByID.Min_power, deviceByID.Max_current, deviceByID.Min_current, -1L, "", -500, -500f, deviceByID.DoorSensor, 0f, -500, -500, -500, "", -500f);
						int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "dev");
						DeviceThreshold deviceThreshold = new DeviceThreshold();
						deviceThreshold.MinCurrentMT = deviceByID.Min_current;
						deviceThreshold.MaxCurrentMT = deviceByID.Max_current;
						deviceThreshold.MinVoltageMT = deviceByID.Min_voltage;
						deviceThreshold.MaxVoltageMT = deviceByID.Max_voltage;
						deviceThreshold.MinPowerMT = deviceByID.Min_power;
						deviceThreshold.MaxPowerMT = deviceByID.Max_power;
						deviceThreshold.MaxPowerDissMT = deviceByID.Max_power_diss;
						ThresholdUtil.UI2Dev(thflg, deviceThreshold);
						deviceThreshold.PopEnableMode = -500;
						deviceThreshold.PopThreshold = -500f;
						deviceThreshold.PopModeOutlet = -500;
						deviceThreshold.PopModeLIFO = -500;
						deviceThreshold.PopModePriority = -500;
						deviceThreshold.DoorSensorType = -500;
						System.Collections.Generic.List<DevSnmpConfig> list2 = new System.Collections.Generic.List<DevSnmpConfig>();
						foreach (DeviceInfo current2 in list)
						{
							if (ClientAPI.IsDeviceOnline(current2.DeviceID))
							{
								DevSnmpConfig sNMPpara = commUtil.getSNMPpara(current2);
								list2.Add(sNMPpara);
							}
						}
						bool flag = false;
						string text2 = "";
						if (list.Count > 0)
						{
							System.Collections.Generic.List<object> list3 = new System.Collections.Generic.List<object>();
							list3.Add(deviceThreshold);
							list3.Add(item);
							list3.Add(list2);
							list3.Add(list);
							Program.IdleTimer_Pause(1);
							System.Collections.Generic.List<object> list4;
							if (list2.Count > 50)
							{
								progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_setDevThreshold, new string[0]), null, new progressPopup.ProcessInThread(this.SetDeviceThresholdProc), list3, 0);
								progressPopup.ShowDialog();
								list4 = (progressPopup.Return_V as System.Collections.Generic.List<object>);
							}
							else
							{
								list4 = (this.SetDeviceThresholdProc(list3) as System.Collections.Generic.List<object>);
							}
							flag = (bool)list4[0];
							text2 = (string)list4[1];
						}
						if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold)
						{
							DevSnmpConfig sNMPpara2 = commUtil.getSNMPpara(deviceByID);
							DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara2);
							deviceThreshold.DevReferenceVoltage = deviceByID.ReferenceVoltage;
							deviceThreshold.DoorSensorType = deviceByID.DoorSensor;
							if (devAccessAPI.SetDeviceThreshold(deviceThreshold, deviceByID.Mac))
							{
								flag = true;
							}
							else
							{
								text2 = "HAVE EXCEPTION!";
							}
						}
						Program.IdleTimer_Run(1);
						if (flag)
						{
							EcoGlobalVar.setDashBoardFlg(2uL, "", 2);
							string valuePair = ValuePairs.getValuePair("Username");
							if (!string.IsNullOrEmpty(valuePair))
							{
								LogAPI.writeEventLog("0630001", new string[]
								{
									text,
									valuePair
								});
							}
							else
							{
								LogAPI.writeEventLog("0630001", new string[]
								{
									text
								});
							}
						}
						if (text2.Length > 0)
						{
							if (text2.Equals("HAVE EXCEPTION!"))
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
							}
							else
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail_1, new string[]
								{
									text2
								}));
							}
						}
						else
						{
							EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("Devive Porperties -- butAssign_Click Error:" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private object SetDeviceThresholdProc(object param)
		{
			DBConn dBConn = null;
			bool flag = false;
			string text = "";
			try
			{
				System.Collections.Generic.List<object> list = (System.Collections.Generic.List<object>)param;
				DeviceThreshold deviceThreshold = (DeviceThreshold)list[0];
				DeviceInfo tmp_di = (DeviceInfo)list[1];
				System.Collections.Generic.List<DevSnmpConfig> configs = (System.Collections.Generic.List<DevSnmpConfig>)list[2];
				System.Collections.Generic.List<DeviceInfo> list2 = (System.Collections.Generic.List<DeviceInfo>)list[3];
				DeviceInfo deviceInfo = list2[0];
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceInfo.ModelNm, deviceInfo.FWVersion);
				System.Collections.Generic.Dictionary<string, bool> dictionary = null;
				if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold)
				{
					AppDevAccess appDevAccess = new AppDevAccess();
					dictionary = appDevAccess.SetDeviceThresholds(configs, deviceThreshold);
				}
				dBConn = DBConnPool.getConnection();
				foreach (DeviceInfo current in list2)
				{
					string key = CultureTransfer.ToString(current.DeviceID);
					bool flag2;
					if (dictionary == null)
					{
						flag2 = true;
					}
					else
					{
						if (!dictionary.ContainsKey(key))
						{
							continue;
						}
						flag2 = dictionary[key];
					}
					if (flag2)
					{
						current.CopyThreshold(tmp_di);
						current.UpdateDeviceThreshold(dBConn);
						flag = true;
					}
					else
					{
						text = text + current.DeviceIP + ",";
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("Devive Porperties -- butAssign_Click Error:" + ex.Message);
				text = "HAVE EXCEPTION!";
			}
			if (dBConn != null)
			{
				dBConn.close();
			}
			DeviceOperation.RefreshDBCache(false);
			return new System.Collections.Generic.List<object>
			{
				flag,
				text
			};
		}
		private void tbRefVoltage_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(tb, e.KeyChar, 1);
			if (flag)
			{
				return;
			}
			e.Handled = true;
		}
	}
}
