using CommonAPI.Global;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice._Dev
{
	public class PropLine : UserControl
	{
		private IContainer components;
		private ToolTip toolTip1;
		private GroupBox gbLineConfig;
		private GroupBox gbLine;
		private GroupBox gbThreshold;
		private Label labMaxPowerDisBound;
		private Label labMaxPowerBound;
		private Label labMaxVoltageBound;
		private Label labMaxCurrentBound;
		private Label lbcurrent;
		private Label lbvoltage;
		private Label lbpower;
		private Label lbPD;
		private Label lbmin;
		private Label lbmax;
		private TextBox tbLMinCurrent;
		private TextBox tbLMaxCurrent;
		private Label lb_A1;
		private Label lb_A2;
		private Label lb_kWh2;
		private TextBox tbLMinVoltage;
		private Label lb_kWh1;
		private TextBox tbLMaxVoltage;
		private TextBox tbLMaxPowerDiss;
		private Label lb_V1;
		private TextBox tbLMinPowerDiss;
		private Label lb_V2;
		private Label lb_W2;
		private TextBox tbLMinPower;
		private Label lb_W1;
		private TextBox tbLMaxPower;
		private GroupBox gbDevInfo;
		private Label lbDevNM;
		private Label lbDevIP;
		private Label lbDevRack;
		private Label lbDevModel;
		private Label labDevNm;
		private Label labDevIp;
		private Label labDevModel;
		private Label labDevRackNm;
		private Label labLineNo;
		private Button butLine4;
		private Button butLine3;
		private Button butLine2;
		private Button butLine1;
		private Button butLineAssign;
		private Button butLineSave;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropLine));
			this.toolTip1 = new ToolTip(this.components);
			this.gbLineConfig = new GroupBox();
			this.gbLine = new GroupBox();
			this.gbThreshold = new GroupBox();
			this.labMaxPowerDisBound = new Label();
			this.labMaxPowerBound = new Label();
			this.labMaxVoltageBound = new Label();
			this.labMaxCurrentBound = new Label();
			this.lbcurrent = new Label();
			this.lbvoltage = new Label();
			this.lbpower = new Label();
			this.lbPD = new Label();
			this.lbmin = new Label();
			this.lbmax = new Label();
			this.tbLMinCurrent = new TextBox();
			this.tbLMaxCurrent = new TextBox();
			this.lb_A1 = new Label();
			this.lb_A2 = new Label();
			this.lb_kWh2 = new Label();
			this.tbLMinVoltage = new TextBox();
			this.lb_kWh1 = new Label();
			this.tbLMaxVoltage = new TextBox();
			this.tbLMaxPowerDiss = new TextBox();
			this.lb_V1 = new Label();
			this.tbLMinPowerDiss = new TextBox();
			this.lb_V2 = new Label();
			this.lb_W2 = new Label();
			this.tbLMinPower = new TextBox();
			this.lb_W1 = new Label();
			this.tbLMaxPower = new TextBox();
			this.gbDevInfo = new GroupBox();
			this.lbDevNM = new Label();
			this.lbDevIP = new Label();
			this.lbDevRack = new Label();
			this.lbDevModel = new Label();
			this.labDevNm = new Label();
			this.labDevIp = new Label();
			this.labDevModel = new Label();
			this.labDevRackNm = new Label();
			this.labLineNo = new Label();
			this.butLine4 = new Button();
			this.butLine3 = new Button();
			this.butLine2 = new Button();
			this.butLine1 = new Button();
			this.butLineAssign = new Button();
			this.butLineSave = new Button();
			this.gbLineConfig.SuspendLayout();
			this.gbLine.SuspendLayout();
			this.gbThreshold.SuspendLayout();
			this.gbDevInfo.SuspendLayout();
			base.SuspendLayout();
			this.gbLineConfig.Controls.Add(this.gbLine);
			this.gbLineConfig.Controls.Add(this.gbDevInfo);
			this.gbLineConfig.Controls.Add(this.labLineNo);
			this.gbLineConfig.Controls.Add(this.butLine4);
			this.gbLineConfig.Controls.Add(this.butLine3);
			this.gbLineConfig.Controls.Add(this.butLine2);
			this.gbLineConfig.Controls.Add(this.butLine1);
			componentResourceManager.ApplyResources(this.gbLineConfig, "gbLineConfig");
			this.gbLineConfig.ForeColor = SystemColors.ControlText;
			this.gbLineConfig.Name = "gbLineConfig";
			this.gbLineConfig.TabStop = false;
			this.gbLine.Controls.Add(this.gbThreshold);
			componentResourceManager.ApplyResources(this.gbLine, "gbLine");
			this.gbLine.Name = "gbLine";
			this.gbLine.TabStop = false;
			this.gbThreshold.BackColor = Color.WhiteSmoke;
			this.gbThreshold.Controls.Add(this.labMaxPowerDisBound);
			this.gbThreshold.Controls.Add(this.labMaxPowerBound);
			this.gbThreshold.Controls.Add(this.labMaxVoltageBound);
			this.gbThreshold.Controls.Add(this.labMaxCurrentBound);
			this.gbThreshold.Controls.Add(this.lbcurrent);
			this.gbThreshold.Controls.Add(this.lbvoltage);
			this.gbThreshold.Controls.Add(this.lbpower);
			this.gbThreshold.Controls.Add(this.lbPD);
			this.gbThreshold.Controls.Add(this.lbmin);
			this.gbThreshold.Controls.Add(this.lbmax);
			this.gbThreshold.Controls.Add(this.tbLMinCurrent);
			this.gbThreshold.Controls.Add(this.tbLMaxCurrent);
			this.gbThreshold.Controls.Add(this.lb_A1);
			this.gbThreshold.Controls.Add(this.lb_A2);
			this.gbThreshold.Controls.Add(this.lb_kWh2);
			this.gbThreshold.Controls.Add(this.tbLMinVoltage);
			this.gbThreshold.Controls.Add(this.lb_kWh1);
			this.gbThreshold.Controls.Add(this.tbLMaxVoltage);
			this.gbThreshold.Controls.Add(this.tbLMaxPowerDiss);
			this.gbThreshold.Controls.Add(this.lb_V1);
			this.gbThreshold.Controls.Add(this.tbLMinPowerDiss);
			this.gbThreshold.Controls.Add(this.lb_V2);
			this.gbThreshold.Controls.Add(this.lb_W2);
			this.gbThreshold.Controls.Add(this.tbLMinPower);
			this.gbThreshold.Controls.Add(this.lb_W1);
			this.gbThreshold.Controls.Add(this.tbLMaxPower);
			componentResourceManager.ApplyResources(this.gbThreshold, "gbThreshold");
			this.gbThreshold.Name = "gbThreshold";
			this.gbThreshold.TabStop = false;
			componentResourceManager.ApplyResources(this.labMaxPowerDisBound, "labMaxPowerDisBound");
			this.labMaxPowerDisBound.ForeColor = Color.Red;
			this.labMaxPowerDisBound.Name = "labMaxPowerDisBound";
			componentResourceManager.ApplyResources(this.labMaxPowerBound, "labMaxPowerBound");
			this.labMaxPowerBound.ForeColor = Color.Red;
			this.labMaxPowerBound.Name = "labMaxPowerBound";
			componentResourceManager.ApplyResources(this.labMaxVoltageBound, "labMaxVoltageBound");
			this.labMaxVoltageBound.ForeColor = Color.Red;
			this.labMaxVoltageBound.Name = "labMaxVoltageBound";
			componentResourceManager.ApplyResources(this.labMaxCurrentBound, "labMaxCurrentBound");
			this.labMaxCurrentBound.ForeColor = Color.Red;
			this.labMaxCurrentBound.Name = "labMaxCurrentBound";
			componentResourceManager.ApplyResources(this.lbcurrent, "lbcurrent");
			this.lbcurrent.ForeColor = Color.Black;
			this.lbcurrent.Name = "lbcurrent";
			componentResourceManager.ApplyResources(this.lbvoltage, "lbvoltage");
			this.lbvoltage.ForeColor = Color.Black;
			this.lbvoltage.Name = "lbvoltage";
			componentResourceManager.ApplyResources(this.lbpower, "lbpower");
			this.lbpower.ForeColor = Color.Black;
			this.lbpower.Name = "lbpower";
			componentResourceManager.ApplyResources(this.lbPD, "lbPD");
			this.lbPD.ForeColor = Color.Black;
			this.lbPD.Name = "lbPD";
			componentResourceManager.ApplyResources(this.lbmin, "lbmin");
			this.lbmin.ForeColor = Color.Black;
			this.lbmin.Name = "lbmin";
			componentResourceManager.ApplyResources(this.lbmax, "lbmax");
			this.lbmax.ForeColor = Color.Black;
			this.lbmax.Name = "lbmax";
			this.tbLMinCurrent.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbLMinCurrent, "tbLMinCurrent");
			this.tbLMinCurrent.ForeColor = Color.Black;
			this.tbLMinCurrent.Name = "tbLMinCurrent";
			this.tbLMinCurrent.ReadOnly = true;
			this.tbLMinCurrent.Tag = "lcurrent";
			this.tbLMinCurrent.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.tbLMaxCurrent, "tbLMaxCurrent");
			this.tbLMaxCurrent.ForeColor = Color.Black;
			this.tbLMaxCurrent.Name = "tbLMaxCurrent";
			this.tbLMaxCurrent.Tag = "lcurrent";
			this.tbLMaxCurrent.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.lb_A1, "lb_A1");
			this.lb_A1.ForeColor = Color.Black;
			this.lb_A1.Name = "lb_A1";
			componentResourceManager.ApplyResources(this.lb_A2, "lb_A2");
			this.lb_A2.ForeColor = Color.Black;
			this.lb_A2.Name = "lb_A2";
			componentResourceManager.ApplyResources(this.lb_kWh2, "lb_kWh2");
			this.lb_kWh2.ForeColor = Color.Black;
			this.lb_kWh2.Name = "lb_kWh2";
			componentResourceManager.ApplyResources(this.tbLMinVoltage, "tbLMinVoltage");
			this.tbLMinVoltage.ForeColor = Color.Black;
			this.tbLMinVoltage.Name = "tbLMinVoltage";
			this.tbLMinVoltage.Tag = "lvoltage";
			this.tbLMinVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.lb_kWh1, "lb_kWh1");
			this.lb_kWh1.ForeColor = Color.Black;
			this.lb_kWh1.Name = "lb_kWh1";
			componentResourceManager.ApplyResources(this.tbLMaxVoltage, "tbLMaxVoltage");
			this.tbLMaxVoltage.ForeColor = Color.Black;
			this.tbLMaxVoltage.Name = "tbLMaxVoltage";
			this.tbLMaxVoltage.Tag = "lvoltage";
			this.tbLMaxVoltage.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.tbLMaxPowerDiss, "tbLMaxPowerDiss");
			this.tbLMaxPowerDiss.ForeColor = Color.Black;
			this.tbLMaxPowerDiss.Name = "tbLMaxPowerDiss";
			this.tbLMaxPowerDiss.Tag = "lpowerDiss";
			this.tbLMaxPowerDiss.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.lb_V1, "lb_V1");
			this.lb_V1.ForeColor = Color.Black;
			this.lb_V1.Name = "lb_V1";
			this.tbLMinPowerDiss.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbLMinPowerDiss, "tbLMinPowerDiss");
			this.tbLMinPowerDiss.ForeColor = Color.Black;
			this.tbLMinPowerDiss.Name = "tbLMinPowerDiss";
			this.tbLMinPowerDiss.ReadOnly = true;
			this.tbLMinPowerDiss.Tag = "lpowerDiss";
			this.tbLMinPowerDiss.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.lb_V2, "lb_V2");
			this.lb_V2.ForeColor = Color.Black;
			this.lb_V2.Name = "lb_V2";
			componentResourceManager.ApplyResources(this.lb_W2, "lb_W2");
			this.lb_W2.ForeColor = Color.Black;
			this.lb_W2.Name = "lb_W2";
			this.tbLMinPower.BackColor = Color.Silver;
			componentResourceManager.ApplyResources(this.tbLMinPower, "tbLMinPower");
			this.tbLMinPower.ForeColor = Color.Black;
			this.tbLMinPower.Name = "tbLMinPower";
			this.tbLMinPower.ReadOnly = true;
			this.tbLMinPower.Tag = "lpower";
			this.tbLMinPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.lb_W1, "lb_W1");
			this.lb_W1.ForeColor = Color.Black;
			this.lb_W1.Name = "lb_W1";
			componentResourceManager.ApplyResources(this.tbLMaxPower, "tbLMaxPower");
			this.tbLMaxPower.ForeColor = Color.Black;
			this.tbLMaxPower.Name = "tbLMaxPower";
			this.tbLMaxPower.Tag = "lpower";
			this.tbLMaxPower.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.gbDevInfo.Controls.Add(this.lbDevNM);
			this.gbDevInfo.Controls.Add(this.lbDevIP);
			this.gbDevInfo.Controls.Add(this.lbDevRack);
			this.gbDevInfo.Controls.Add(this.lbDevModel);
			this.gbDevInfo.Controls.Add(this.labDevNm);
			this.gbDevInfo.Controls.Add(this.labDevIp);
			this.gbDevInfo.Controls.Add(this.labDevModel);
			this.gbDevInfo.Controls.Add(this.labDevRackNm);
			componentResourceManager.ApplyResources(this.gbDevInfo, "gbDevInfo");
			this.gbDevInfo.ForeColor = SystemColors.ControlText;
			this.gbDevInfo.Name = "gbDevInfo";
			this.gbDevInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.lbDevNM, "lbDevNM");
			this.lbDevNM.ForeColor = Color.Black;
			this.lbDevNM.Name = "lbDevNM";
			componentResourceManager.ApplyResources(this.lbDevIP, "lbDevIP");
			this.lbDevIP.ForeColor = Color.Black;
			this.lbDevIP.Name = "lbDevIP";
			componentResourceManager.ApplyResources(this.lbDevRack, "lbDevRack");
			this.lbDevRack.ForeColor = Color.Black;
			this.lbDevRack.Name = "lbDevRack";
			componentResourceManager.ApplyResources(this.lbDevModel, "lbDevModel");
			this.lbDevModel.ForeColor = Color.Black;
			this.lbDevModel.Name = "lbDevModel";
			this.labDevNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevNm, "labDevNm");
			this.labDevNm.ForeColor = Color.Black;
			this.labDevNm.Name = "labDevNm";
			this.labDevIp.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevIp, "labDevIp");
			this.labDevIp.ForeColor = Color.Black;
			this.labDevIp.Name = "labDevIp";
			this.labDevModel.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevModel, "labDevModel");
			this.labDevModel.ForeColor = Color.Black;
			this.labDevModel.Name = "labDevModel";
			this.labDevRackNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevRackNm, "labDevRackNm");
			this.labDevRackNm.ForeColor = Color.Black;
			this.labDevRackNm.Name = "labDevRackNm";
			componentResourceManager.ApplyResources(this.labLineNo, "labLineNo");
			this.labLineNo.ForeColor = Color.Black;
			this.labLineNo.Name = "labLineNo";
			this.butLine4.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butLine4, "butLine4");
			this.butLine4.ForeColor = Color.Black;
			this.butLine4.Name = "butLine4";
			this.butLine4.Tag = "4";
			this.butLine4.UseVisualStyleBackColor = false;
			this.butLine4.Click += new System.EventHandler(this.butLine_Click);
			this.butLine3.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butLine3, "butLine3");
			this.butLine3.ForeColor = Color.Black;
			this.butLine3.Name = "butLine3";
			this.butLine3.Tag = "3";
			this.butLine3.UseVisualStyleBackColor = false;
			this.butLine3.Click += new System.EventHandler(this.butLine_Click);
			this.butLine2.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butLine2, "butLine2");
			this.butLine2.ForeColor = Color.Black;
			this.butLine2.Name = "butLine2";
			this.butLine2.Tag = "2";
			this.butLine2.UseVisualStyleBackColor = false;
			this.butLine2.Click += new System.EventHandler(this.butLine_Click);
			this.butLine1.BackColor = Color.PaleTurquoise;
			componentResourceManager.ApplyResources(this.butLine1, "butLine1");
			this.butLine1.ForeColor = Color.Black;
			this.butLine1.Name = "butLine1";
			this.butLine1.Tag = "1";
			this.butLine1.UseVisualStyleBackColor = false;
			this.butLine1.Click += new System.EventHandler(this.butLine_Click);
			this.butLineAssign.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butLineAssign, "butLineAssign");
			this.butLineAssign.Name = "butLineAssign";
			this.butLineAssign.UseVisualStyleBackColor = false;
			this.butLineAssign.Click += new System.EventHandler(this.butLineAssign_Click);
			this.butLineSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butLineSave, "butLineSave");
			this.butLineSave.Name = "butLineSave";
			this.butLineSave.UseVisualStyleBackColor = false;
			this.butLineSave.Click += new System.EventHandler(this.butLineSave_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butLineAssign);
			base.Controls.Add(this.butLineSave);
			base.Controls.Add(this.gbLineConfig);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropLine";
			this.gbLineConfig.ResumeLayout(false);
			this.gbLine.ResumeLayout(false);
			this.gbThreshold.ResumeLayout(false);
			this.gbThreshold.PerformLayout();
			this.gbDevInfo.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public PropLine()
		{
			this.InitializeComponent();
			this.tbLMinCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbLMaxCurrent.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbLMinVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbLMaxVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbLMinPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbLMaxPower.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbLMinPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbLMaxPowerDiss.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(int devID, int lineID, bool onlinest)
		{
			this.butLineAssign.Enabled = onlinest;
			this.butLineSave.Enabled = onlinest;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID);
			this.labDevIp.Text = deviceByID.DeviceIP;
			this.labDevIp.Tag = deviceByID.FWVersion;
			this.labDevModel.Text = deviceByID.ModelNm;
			this.labDevModel.Tag = devID.ToString();
			this.labDevNm.Text = deviceByID.DeviceName;
			string text = deviceByID.ModelNm;
			if (DevAccessCfg.GetInstance().isAutodectDev(deviceByID.ModelNm, deviceByID.FWVersion))
			{
				text = text + " (F/W: " + deviceByID.FWVersion + ")";
			}
			this.toolTip1.SetToolTip(this.labDevModel, text);
			RackInfo rackByID = RackInfo.getRackByID(deviceByID.RackID);
			this.labDevRackNm.Text = rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
			this.butLine1.Visible = false;
			this.butLine2.Visible = false;
			this.butLine3.Visible = false;
			this.butLine4.Visible = false;
			System.Collections.Generic.List<LineInfo> lineInfo = deviceByID.GetLineInfo();
			foreach (LineInfo current in lineInfo)
			{
				switch (current.LineNumber)
				{
				case 1:
					this.butLine1.Visible = true;
					this.butLine1.Tag = current.ID.ToString();
					break;
				case 2:
					this.butLine2.Visible = true;
					this.butLine2.Tag = current.ID.ToString();
					break;
				case 3:
					this.butLine3.Visible = true;
					this.butLine3.Tag = current.ID.ToString();
					break;
				case 4:
					this.butLine4.Visible = true;
					this.butLine4.Tag = current.ID.ToString();
					break;
				}
			}
			this.lineConfigPageControlInit(deviceModelConfig);
			if (lineID == 0)
			{
				this.setLineConfigData(deviceByID, System.Convert.ToInt32(this.butLine1.Tag));
				return;
			}
			this.setLineConfigData(deviceByID, lineID);
		}
		private void lineConfigPageControlInit(DevModelConfig devcfg)
		{
			if (devcfg.perlineReading == Constant.YES)
			{
				this.gbThreshold.Show();
				this.butLineAssign.Visible = true;
				return;
			}
			this.gbThreshold.Hide();
			this.butLineAssign.Visible = false;
		}
		private void setLineConfigData(DeviceInfo pCurDev, int lineID)
		{
			string text = this.labDevModel.Text;
			string value = lineID.ToString();
			LineInfo lineInfoByID = DeviceOperation.GetLineInfoByID(lineID);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, pCurDev.FWVersion);
			int selNo = System.Convert.ToInt32(lineInfoByID.LineNumber);
			this.labLineNo.Text = selNo.ToString();
			System.Collections.Generic.List<LineInfo> lineInfo = pCurDev.GetLineInfo();
			for (int i = 1; i <= lineInfo.Count; i++)
			{
				Control[] array = this.gbLineConfig.Controls.Find("butLine" + i, false);
				if (array.Length > 0)
				{
					string text2 = ((Button)array[0]).Tag.ToString();
					if (!text2.Equals(value))
					{
						((Button)array[0]).BackColor = Color.PaleTurquoise;
					}
					else
					{
						((Button)array[0]).BackColor = Color.DarkCyan;
					}
				}
			}
			int num = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "line");
			this.labMaxCurrentBound.Text = ((!this.tbLMinCurrent.ReadOnly || !this.tbLMaxCurrent.ReadOnly) ? devcfgUtil.RangeCurrent(deviceModelConfig, "line", selNo, "F1") : "");
			ThresholdUtil.SetUIEdit(this.tbLMinCurrent, (num & 1) == 0, lineInfoByID.Min_current, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbLMaxCurrent, (num & 2) == 0, lineInfoByID.Max_current, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbLMinVoltage, (num & 4) == 0, lineInfoByID.Min_voltage, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbLMaxVoltage, (num & 8) == 0, lineInfoByID.Max_voltage, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbLMinPower, (num & 16) == 0, lineInfoByID.Min_power, 0, "F1");
			ThresholdUtil.SetUIEdit(this.tbLMaxPower, (num & 32) == 0, lineInfoByID.Max_power, 0, "F1");
			this.labMaxVoltageBound.Text = ((!this.tbLMinVoltage.ReadOnly || !this.tbLMaxVoltage.ReadOnly) ? devcfgUtil.RangeVoltage(deviceModelConfig, "line", selNo) : "");
			this.labMaxPowerBound.Text = ((!this.tbLMinPower.ReadOnly || !this.tbLMaxPower.ReadOnly) ? devcfgUtil.RangePower(deviceModelConfig, "line", selNo, 1.0) : "");
			this.labMaxPowerDisBound.Text = ((!this.tbLMinPowerDiss.ReadOnly || !this.tbLMaxPowerDiss.ReadOnly) ? devcfgUtil.RangePowerDiss(deviceModelConfig, "line", selNo) : "");
		}
		public void TimerProc(bool onlinest, int haveThresholdChange)
		{
			if (haveThresholdChange == 1)
			{
				string value = this.labDevModel.Tag.ToString();
				int num = System.Convert.ToInt32(value);
				string text = this.labLineNo.Text;
				int i_linenum = System.Convert.ToInt32(text);
				LineInfo lineInfo = new LineInfo(num, i_linenum);
				this.pageInit(num, lineInfo.ID, onlinest);
				return;
			}
			this.butLineAssign.Enabled = onlinest;
			this.butLineSave.Enabled = onlinest;
		}
		private void butLine_Click(object sender, System.EventArgs e)
		{
			string text = this.labDevModel.Text;
			string value = this.labDevModel.Tag.ToString();
			int l_id = System.Convert.ToInt32(value);
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, deviceByID.FWVersion);
			this.lineConfigPageControlInit(deviceModelConfig);
			string value2 = ((Button)sender).Tag.ToString();
			((Button)sender).BackColor = Color.DarkCyan;
			this.setLineConfigData(deviceByID, System.Convert.ToInt32(value2));
		}
		private void butLineSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				string value = this.labDevModel.Tag.ToString();
				int num = System.Convert.ToInt32(value);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.lineCheck(deviceModelConfig))
				{
					string text = this.labLineNo.Text;
					int num2 = System.Convert.ToInt32(text);
					LineInfo lineInfo = new LineInfo(num, num2);
					DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
					LineThreshold lineThreshold = new LineThreshold(num2);
					if (this.gbThreshold.Visible)
					{
						lineInfo.Min_current = ThresholdUtil.UI2DB(this.tbLMinCurrent, lineInfo.Min_current, 0);
						lineInfo.Max_current = ThresholdUtil.UI2DB(this.tbLMaxCurrent, lineInfo.Max_current, 0);
						lineInfo.Min_voltage = ThresholdUtil.UI2DB(this.tbLMinVoltage, lineInfo.Min_voltage, 0);
						lineInfo.Max_voltage = ThresholdUtil.UI2DB(this.tbLMaxVoltage, lineInfo.Max_voltage, 0);
						lineInfo.Min_power = ThresholdUtil.UI2DB(this.tbLMinPower, lineInfo.Min_power, 0);
						lineInfo.Max_power = ThresholdUtil.UI2DB(this.tbLMaxPower, lineInfo.Max_power, 0);
						int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "line");
						lineThreshold.MinCurrentMt = lineInfo.Min_current;
						lineThreshold.MaxCurrentMT = lineInfo.Max_current;
						lineThreshold.MinVoltageMT = lineInfo.Min_voltage;
						lineThreshold.MaxVoltageMT = lineInfo.Max_voltage;
						lineThreshold.MinPowerMT = lineInfo.Min_power;
						lineThreshold.MaxPowerMT = lineInfo.Max_power;
						ThresholdUtil.UI2Dev(thflg, lineThreshold);
					}
					bool flag = true;
					if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold)
					{
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						flag = devAccessAPI.SetLineThreshold(lineThreshold, deviceByID.Mac);
					}
					if (flag)
					{
						if (lineInfo != null)
						{
							lineInfo.Update();
							EcoGlobalVar.setDashBoardFlg(128uL, string.Concat(new object[]
							{
								"#UPDATE#D",
								lineInfo.DeviceID,
								":L",
								lineInfo.ID,
								";"
							}), 2);
							string valuePair = ValuePairs.getValuePair("Username");
							if (!string.IsNullOrEmpty(valuePair))
							{
								LogAPI.writeEventLog("0630015", new string[]
								{
									lineInfo.LineNumber.ToString(),
									deviceByID.DeviceIP,
									deviceByID.DeviceName,
									valuePair
								});
							}
							else
							{
								LogAPI.writeEventLog("0630015", new string[]
								{
									lineInfo.LineNumber.ToString(),
									deviceByID.DeviceIP,
									deviceByID.DeviceName
								});
							}
						}
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
					}
					else
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("PropLine Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private bool lineCheck(DevModelConfig devcfg)
		{
			string text = this.labLineNo.Text;
			System.Convert.ToInt32(text);
			bool flag = false;
			if (this.gbThreshold.Visible)
			{
				flag = true;
				int num = devcfgUtil.UIThresholdEditFlg(devcfg, "line");
				Ecovalidate.checkThresholdValue(this.tbLMinCurrent, this.labMaxCurrentBound, (num & 256) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbLMaxCurrent, this.labMaxCurrentBound, (num & 512) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbLMinVoltage, this.labMaxVoltageBound, (num & 1024) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbLMaxVoltage, this.labMaxVoltageBound, (num & 2048) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbLMinPower, this.labMaxPowerBound, (num & 4096) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbLMaxPower, this.labMaxPowerBound, (num & 8192) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbLMinPowerDiss, this.labMaxPowerDisBound, (num & 16384) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbLMaxPowerDiss, this.labMaxPowerDisBound, (num & 32768) == 0, ref flag);
				if (!flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_Thresholdinvalid, new string[0]));
					return false;
				}
				Ecovalidate.checkThresholdMaxMixValue(this.tbLMaxCurrent, this.tbLMinCurrent, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbLMaxVoltage, this.tbLMinVoltage, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbLMaxPower, this.tbLMinPower, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbLMaxPowerDiss, this.tbLMinPowerDiss, ref flag);
				if (!flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdMinMax, new string[0]));
					return false;
				}
			}
			return true;
		}
		private void threshold_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			string arg_0D_0 = textBox.Text;
			bool flag = Ecovalidate.inputCheck_float(textBox, e.KeyChar, 1);
			if (flag)
			{
				return;
			}
			e.Handled = true;
		}
		private void butLineAssign_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			DBConn dBConn = null;
			string text = this.labDevModel.Tag.ToString();
			try
			{
				int num = System.Convert.ToInt32(text);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.lineCheck(deviceModelConfig))
				{
					DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Dev_ApplyLine, new string[0]), MessageBoxButtons.OKCancel);
					if (dialogResult != DialogResult.Cancel)
					{
						string text2 = this.labLineNo.Text;
						int i_linenum = System.Convert.ToInt32(text2);
						LineInfo lineInfo = new LineInfo(num, i_linenum);
						DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
						LineThreshold lineThreshold = new LineThreshold(1);
						if (this.gbThreshold.Visible)
						{
							lineInfo.Min_current = ThresholdUtil.UI2DB(this.tbLMinCurrent, lineInfo.Min_current, 0);
							lineInfo.Max_current = ThresholdUtil.UI2DB(this.tbLMaxCurrent, lineInfo.Max_current, 0);
							lineInfo.Min_voltage = ThresholdUtil.UI2DB(this.tbLMinVoltage, lineInfo.Min_voltage, 0);
							lineInfo.Max_voltage = ThresholdUtil.UI2DB(this.tbLMaxVoltage, lineInfo.Max_voltage, 0);
							lineInfo.Min_power = ThresholdUtil.UI2DB(this.tbLMinPower, lineInfo.Min_power, 0);
							lineInfo.Max_power = ThresholdUtil.UI2DB(this.tbLMaxPower, lineInfo.Max_power, 0);
							int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "line");
							lineThreshold.MinCurrentMt = lineInfo.Min_current;
							lineThreshold.MaxCurrentMT = lineInfo.Max_current;
							lineThreshold.MinVoltageMT = lineInfo.Min_voltage;
							lineThreshold.MaxVoltageMT = lineInfo.Max_voltage;
							lineThreshold.MinPowerMT = lineInfo.Min_power;
							lineThreshold.MaxPowerMT = lineInfo.Max_power;
							ThresholdUtil.UI2Dev(thflg, lineThreshold);
						}
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						string myMac = deviceByID.Mac;
						System.Collections.Generic.List<LineInfo> lineInfo2 = deviceByID.GetLineInfo();
						dBConn = DBConnPool.getConnection();
						foreach (LineInfo current in lineInfo2)
						{
							LineThreshold lineThreshold2 = new LineThreshold(current.LineNumber);
							if (this.gbThreshold.Visible)
							{
								lineThreshold2.MaxCurrentMT = lineThreshold.MaxCurrentMT;
								lineThreshold2.MinCurrentMt = lineThreshold.MinCurrentMt;
								lineThreshold2.MaxPowerMT = lineThreshold.MaxPowerMT;
								lineThreshold2.MinPowerMT = lineThreshold.MinPowerMT;
								lineThreshold2.MaxVoltageMT = lineThreshold.MaxVoltageMT;
								lineThreshold2.MinVoltageMT = lineThreshold.MinVoltageMT;
							}
							bool flag2 = true;
							if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold)
							{
								flag2 = devAccessAPI.SetLineThreshold(lineThreshold2, myMac);
							}
							if (!flag2)
							{
								EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
								return;
							}
							if (this.gbThreshold.Visible)
							{
								current.CopyThreshold(lineInfo);
							}
							current.UpdateLineThreshold(dBConn);
							flag = true;
							myMac = "";
						}
						if (dBConn != null)
						{
							dBConn.close();
						}
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							LogAPI.writeEventLog("0630016", new string[]
							{
								deviceByID.ModelNm,
								deviceByID.DeviceIP,
								deviceByID.DeviceName,
								valuePair
							});
						}
						else
						{
							LogAPI.writeEventLog("0630016", new string[]
							{
								deviceByID.ModelNm,
								deviceByID.DeviceIP,
								deviceByID.DeviceName
							});
						}
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("butLineAssign_Click Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
			finally
			{
				if (dBConn != null)
				{
					dBConn.close();
				}
				DeviceOperation.RefreshDBCache(false);
				if (flag)
				{
					EcoGlobalVar.setDashBoardFlg(128uL, "#UPDATE#D" + text + ":L*;", 2);
				}
			}
		}
	}
}
