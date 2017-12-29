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
	public class PropSensor : UserControl
	{
		private IContainer components;
		private Label labDevNm;
		private Label labDevRackNm;
		private Label lbSS1P;
		private Label lbSS1T;
		private Label lbSS1H;
		private Label labDevModel;
		private Label label28;
		private Label label27;
		private Label label33;
		private Label label34;
		private Label label24;
		private TextBox tbMaxPress1;
		private TextBox tbMinPress1;
		private Label labDevIp;
		private Label label36;
		private Label label130;
		private Label label129;
		private Label label115;
		private TextBox tbMaxTemperature1;
		private Label label116;
		private TextBox tbMinTemperature1;
		private Label label118;
		private TextBox tbMinHumidity1;
		private Button butSSAssign;
		private TextBox tbMaxHumidity1;
		private Label label119;
		private Label label94;
		private Button butSSSave;
		private GroupBox gbSensorConfig;
		private GroupBox gBSensor1;
		private GroupBox gBSensor2;
		private Label lbSS2P;
		private Label lbSS2T;
		private Label lbSS2H;
		private Label label67;
		private Label label68;
		private Label label69;
		private TextBox tbMaxPress2;
		private TextBox tbMinPress2;
		private Label label70;
		private Label label71;
		private Label label72;
		private TextBox tbMaxTemperature2;
		private Label label73;
		private TextBox tbMinTemperature2;
		private Label label74;
		private TextBox tbMaxHumidity2;
		private Label label75;
		private TextBox tbMinHumidity2;
		private GroupBox gBSensor4;
		private Label lbSS4P;
		private Label lbSS4T;
		private Label lbSS4H;
		private Label label26;
		private Label label29;
		private Label label30;
		private TextBox tbMaxPress4;
		private TextBox tbMinPress4;
		private Label label31;
		private Label label32;
		private Label label35;
		private TextBox tbMaxTemperature4;
		private Label label37;
		private TextBox tbMinTemperature4;
		private Label label38;
		private TextBox tbMaxHumidity4;
		private Label label39;
		private TextBox tbMinHumidity4;
		private GroupBox gBSensor3;
		private Label lbSS3P;
		private Label lbSS3T;
		private Label lbSS3H;
		private Label label11;
		private Label label12;
		private Label label13;
		private TextBox tbMaxPress3;
		private TextBox tbMinPress3;
		private Label label14;
		private Label label15;
		private Label label17;
		private TextBox tbMaxTemperature3;
		private Label label19;
		private TextBox tbMinTemperature3;
		private Label label20;
		private TextBox tbMaxHumidity3;
		private Label label21;
		private TextBox tbMinHumidity3;
		private GroupBox groupBox1;
		private ToolTip toolTip1;
		private bool haveEditBox;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropSensor));
			this.labDevNm = new Label();
			this.labDevRackNm = new Label();
			this.lbSS1P = new Label();
			this.lbSS1T = new Label();
			this.lbSS1H = new Label();
			this.labDevModel = new Label();
			this.label28 = new Label();
			this.label27 = new Label();
			this.label33 = new Label();
			this.label34 = new Label();
			this.label24 = new Label();
			this.tbMaxPress1 = new TextBox();
			this.tbMinPress1 = new TextBox();
			this.labDevIp = new Label();
			this.label36 = new Label();
			this.label130 = new Label();
			this.label129 = new Label();
			this.label115 = new Label();
			this.tbMaxTemperature1 = new TextBox();
			this.label116 = new Label();
			this.tbMinTemperature1 = new TextBox();
			this.label118 = new Label();
			this.tbMinHumidity1 = new TextBox();
			this.butSSAssign = new Button();
			this.tbMaxHumidity1 = new TextBox();
			this.label119 = new Label();
			this.label94 = new Label();
			this.butSSSave = new Button();
			this.gbSensorConfig = new GroupBox();
			this.groupBox1 = new GroupBox();
			this.gBSensor4 = new GroupBox();
			this.lbSS4P = new Label();
			this.lbSS4T = new Label();
			this.lbSS4H = new Label();
			this.label26 = new Label();
			this.label29 = new Label();
			this.label30 = new Label();
			this.tbMaxPress4 = new TextBox();
			this.tbMinPress4 = new TextBox();
			this.label31 = new Label();
			this.label32 = new Label();
			this.label35 = new Label();
			this.tbMaxTemperature4 = new TextBox();
			this.label37 = new Label();
			this.tbMinTemperature4 = new TextBox();
			this.label38 = new Label();
			this.tbMaxHumidity4 = new TextBox();
			this.label39 = new Label();
			this.tbMinHumidity4 = new TextBox();
			this.gBSensor3 = new GroupBox();
			this.lbSS3P = new Label();
			this.lbSS3T = new Label();
			this.lbSS3H = new Label();
			this.label11 = new Label();
			this.label12 = new Label();
			this.label13 = new Label();
			this.tbMaxPress3 = new TextBox();
			this.tbMinPress3 = new TextBox();
			this.label14 = new Label();
			this.label15 = new Label();
			this.label17 = new Label();
			this.tbMaxTemperature3 = new TextBox();
			this.label19 = new Label();
			this.tbMinTemperature3 = new TextBox();
			this.label20 = new Label();
			this.tbMaxHumidity3 = new TextBox();
			this.label21 = new Label();
			this.tbMinHumidity3 = new TextBox();
			this.gBSensor1 = new GroupBox();
			this.gBSensor2 = new GroupBox();
			this.lbSS2P = new Label();
			this.lbSS2T = new Label();
			this.lbSS2H = new Label();
			this.label67 = new Label();
			this.label68 = new Label();
			this.label69 = new Label();
			this.tbMaxPress2 = new TextBox();
			this.tbMinPress2 = new TextBox();
			this.label70 = new Label();
			this.label71 = new Label();
			this.label72 = new Label();
			this.tbMaxTemperature2 = new TextBox();
			this.label73 = new Label();
			this.tbMinTemperature2 = new TextBox();
			this.label74 = new Label();
			this.tbMaxHumidity2 = new TextBox();
			this.label75 = new Label();
			this.tbMinHumidity2 = new TextBox();
			this.toolTip1 = new ToolTip(this.components);
			this.gbSensorConfig.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.gBSensor4.SuspendLayout();
			this.gBSensor3.SuspendLayout();
			this.gBSensor1.SuspendLayout();
			this.gBSensor2.SuspendLayout();
			base.SuspendLayout();
			this.labDevNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevNm, "labDevNm");
			this.labDevNm.ForeColor = Color.Black;
			this.labDevNm.Name = "labDevNm";
			this.labDevRackNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevRackNm, "labDevRackNm");
			this.labDevRackNm.ForeColor = Color.Black;
			this.labDevRackNm.Name = "labDevRackNm";
			componentResourceManager.ApplyResources(this.lbSS1P, "lbSS1P");
			this.lbSS1P.ForeColor = Color.Red;
			this.lbSS1P.Name = "lbSS1P";
			componentResourceManager.ApplyResources(this.lbSS1T, "lbSS1T");
			this.lbSS1T.ForeColor = Color.Red;
			this.lbSS1T.Name = "lbSS1T";
			componentResourceManager.ApplyResources(this.lbSS1H, "lbSS1H");
			this.lbSS1H.ForeColor = Color.Red;
			this.lbSS1H.Name = "lbSS1H";
			this.labDevModel.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevModel, "labDevModel");
			this.labDevModel.ForeColor = Color.Black;
			this.labDevModel.Name = "labDevModel";
			componentResourceManager.ApplyResources(this.label28, "label28");
			this.label28.ForeColor = Color.Black;
			this.label28.Name = "label28";
			componentResourceManager.ApplyResources(this.label27, "label27");
			this.label27.ForeColor = Color.Black;
			this.label27.Name = "label27";
			componentResourceManager.ApplyResources(this.label33, "label33");
			this.label33.ForeColor = Color.Black;
			this.label33.Name = "label33";
			componentResourceManager.ApplyResources(this.label34, "label34");
			this.label34.ForeColor = Color.Black;
			this.label34.Name = "label34";
			componentResourceManager.ApplyResources(this.label24, "label24");
			this.label24.ForeColor = Color.Black;
			this.label24.Name = "label24";
			this.tbMaxPress1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxPress1, "tbMaxPress1");
			this.tbMaxPress1.ForeColor = Color.Black;
			this.tbMaxPress1.Name = "tbMaxPress1";
			this.tbMaxPress1.Tag = "press";
			this.tbMaxPress1.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.tbMinPress1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinPress1, "tbMinPress1");
			this.tbMinPress1.ForeColor = Color.Black;
			this.tbMinPress1.Name = "tbMinPress1";
			this.tbMinPress1.Tag = "press";
			this.tbMinPress1.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.labDevIp.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevIp, "labDevIp");
			this.labDevIp.ForeColor = Color.Black;
			this.labDevIp.Name = "labDevIp";
			componentResourceManager.ApplyResources(this.label36, "label36");
			this.label36.ForeColor = Color.Black;
			this.label36.Name = "label36";
			componentResourceManager.ApplyResources(this.label130, "label130");
			this.label130.ForeColor = Color.Black;
			this.label130.Name = "label130";
			componentResourceManager.ApplyResources(this.label129, "label129");
			this.label129.ForeColor = Color.Black;
			this.label129.Name = "label129";
			componentResourceManager.ApplyResources(this.label115, "label115");
			this.label115.ForeColor = Color.Black;
			this.label115.Name = "label115";
			this.tbMaxTemperature1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxTemperature1, "tbMaxTemperature1");
			this.tbMaxTemperature1.ForeColor = Color.Black;
			this.tbMaxTemperature1.Name = "tbMaxTemperature1";
			this.tbMaxTemperature1.Tag = "temperature";
			this.tbMaxTemperature1.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label116, "label116");
			this.label116.ForeColor = Color.Black;
			this.label116.Name = "label116";
			this.tbMinTemperature1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinTemperature1, "tbMinTemperature1");
			this.tbMinTemperature1.ForeColor = Color.Black;
			this.tbMinTemperature1.Name = "tbMinTemperature1";
			this.tbMinTemperature1.Tag = "temperature";
			this.tbMinTemperature1.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label118, "label118");
			this.label118.ForeColor = Color.Black;
			this.label118.Name = "label118";
			this.tbMinHumidity1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinHumidity1, "tbMinHumidity1");
			this.tbMinHumidity1.ForeColor = Color.Black;
			this.tbMinHumidity1.Name = "tbMinHumidity1";
			this.tbMinHumidity1.Tag = "humidity";
			this.tbMinHumidity1.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.butSSAssign.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSSAssign, "butSSAssign");
			this.butSSAssign.Name = "butSSAssign";
			this.butSSAssign.UseVisualStyleBackColor = false;
			this.butSSAssign.Click += new System.EventHandler(this.butSSAssign_Click);
			this.tbMaxHumidity1.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxHumidity1, "tbMaxHumidity1");
			this.tbMaxHumidity1.ForeColor = Color.Black;
			this.tbMaxHumidity1.Name = "tbMaxHumidity1";
			this.tbMaxHumidity1.Tag = "humidity";
			this.tbMaxHumidity1.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label119, "label119");
			this.label119.ForeColor = Color.Black;
			this.label119.Name = "label119";
			componentResourceManager.ApplyResources(this.label94, "label94");
			this.label94.ForeColor = Color.Black;
			this.label94.Name = "label94";
			this.butSSSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSSSave, "butSSSave");
			this.butSSSave.Name = "butSSSave";
			this.butSSSave.UseVisualStyleBackColor = false;
			this.butSSSave.Click += new System.EventHandler(this.butSSSave_Click);
			this.gbSensorConfig.BackColor = Color.WhiteSmoke;
			this.gbSensorConfig.Controls.Add(this.groupBox1);
			this.gbSensorConfig.Controls.Add(this.gBSensor4);
			this.gbSensorConfig.Controls.Add(this.gBSensor3);
			this.gbSensorConfig.Controls.Add(this.gBSensor1);
			this.gbSensorConfig.Controls.Add(this.gBSensor2);
			componentResourceManager.ApplyResources(this.gbSensorConfig, "gbSensorConfig");
			this.gbSensorConfig.ForeColor = SystemColors.ControlText;
			this.gbSensorConfig.Name = "gbSensorConfig";
			this.gbSensorConfig.TabStop = false;
			this.groupBox1.Controls.Add(this.labDevModel);
			this.groupBox1.Controls.Add(this.label94);
			this.groupBox1.Controls.Add(this.label36);
			this.groupBox1.Controls.Add(this.labDevIp);
			this.groupBox1.Controls.Add(this.labDevNm);
			this.groupBox1.Controls.Add(this.label27);
			this.groupBox1.Controls.Add(this.labDevRackNm);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.ForeColor = SystemColors.ControlText;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.gBSensor4.Controls.Add(this.lbSS4P);
			this.gBSensor4.Controls.Add(this.lbSS4T);
			this.gBSensor4.Controls.Add(this.lbSS4H);
			this.gBSensor4.Controls.Add(this.label26);
			this.gBSensor4.Controls.Add(this.label29);
			this.gBSensor4.Controls.Add(this.label30);
			this.gBSensor4.Controls.Add(this.tbMaxPress4);
			this.gBSensor4.Controls.Add(this.tbMinPress4);
			this.gBSensor4.Controls.Add(this.label31);
			this.gBSensor4.Controls.Add(this.label32);
			this.gBSensor4.Controls.Add(this.label35);
			this.gBSensor4.Controls.Add(this.tbMaxTemperature4);
			this.gBSensor4.Controls.Add(this.label37);
			this.gBSensor4.Controls.Add(this.tbMinTemperature4);
			this.gBSensor4.Controls.Add(this.label38);
			this.gBSensor4.Controls.Add(this.tbMaxHumidity4);
			this.gBSensor4.Controls.Add(this.label39);
			this.gBSensor4.Controls.Add(this.tbMinHumidity4);
			this.gBSensor4.ForeColor = SystemColors.ControlText;
			componentResourceManager.ApplyResources(this.gBSensor4, "gBSensor4");
			this.gBSensor4.Name = "gBSensor4";
			this.gBSensor4.TabStop = false;
			componentResourceManager.ApplyResources(this.lbSS4P, "lbSS4P");
			this.lbSS4P.ForeColor = Color.Red;
			this.lbSS4P.Name = "lbSS4P";
			componentResourceManager.ApplyResources(this.lbSS4T, "lbSS4T");
			this.lbSS4T.ForeColor = Color.Red;
			this.lbSS4T.Name = "lbSS4T";
			componentResourceManager.ApplyResources(this.lbSS4H, "lbSS4H");
			this.lbSS4H.ForeColor = Color.Red;
			this.lbSS4H.Name = "lbSS4H";
			componentResourceManager.ApplyResources(this.label26, "label26");
			this.label26.ForeColor = Color.Black;
			this.label26.Name = "label26";
			componentResourceManager.ApplyResources(this.label29, "label29");
			this.label29.ForeColor = Color.Black;
			this.label29.Name = "label29";
			componentResourceManager.ApplyResources(this.label30, "label30");
			this.label30.ForeColor = Color.Black;
			this.label30.Name = "label30";
			this.tbMaxPress4.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxPress4, "tbMaxPress4");
			this.tbMaxPress4.ForeColor = Color.Black;
			this.tbMaxPress4.Name = "tbMaxPress4";
			this.tbMaxPress4.Tag = "press";
			this.tbMaxPress4.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.tbMinPress4.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinPress4, "tbMinPress4");
			this.tbMinPress4.ForeColor = Color.Black;
			this.tbMinPress4.Name = "tbMinPress4";
			this.tbMinPress4.Tag = "press";
			this.tbMinPress4.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label31, "label31");
			this.label31.ForeColor = Color.Black;
			this.label31.Name = "label31";
			componentResourceManager.ApplyResources(this.label32, "label32");
			this.label32.ForeColor = Color.Black;
			this.label32.Name = "label32";
			componentResourceManager.ApplyResources(this.label35, "label35");
			this.label35.ForeColor = Color.Black;
			this.label35.Name = "label35";
			this.tbMaxTemperature4.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxTemperature4, "tbMaxTemperature4");
			this.tbMaxTemperature4.ForeColor = Color.Black;
			this.tbMaxTemperature4.Name = "tbMaxTemperature4";
			this.tbMaxTemperature4.Tag = "temperature";
			this.tbMaxTemperature4.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label37, "label37");
			this.label37.ForeColor = Color.Black;
			this.label37.Name = "label37";
			this.tbMinTemperature4.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinTemperature4, "tbMinTemperature4");
			this.tbMinTemperature4.ForeColor = Color.Black;
			this.tbMinTemperature4.Name = "tbMinTemperature4";
			this.tbMinTemperature4.Tag = "temperature";
			this.tbMinTemperature4.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label38, "label38");
			this.label38.ForeColor = Color.Black;
			this.label38.Name = "label38";
			this.tbMaxHumidity4.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxHumidity4, "tbMaxHumidity4");
			this.tbMaxHumidity4.ForeColor = Color.Black;
			this.tbMaxHumidity4.Name = "tbMaxHumidity4";
			this.tbMaxHumidity4.Tag = "humidity";
			this.tbMaxHumidity4.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label39, "label39");
			this.label39.ForeColor = Color.Black;
			this.label39.Name = "label39";
			this.tbMinHumidity4.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinHumidity4, "tbMinHumidity4");
			this.tbMinHumidity4.ForeColor = Color.Black;
			this.tbMinHumidity4.Name = "tbMinHumidity4";
			this.tbMinHumidity4.Tag = "humidity";
			this.tbMinHumidity4.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.gBSensor3.Controls.Add(this.lbSS3P);
			this.gBSensor3.Controls.Add(this.lbSS3T);
			this.gBSensor3.Controls.Add(this.lbSS3H);
			this.gBSensor3.Controls.Add(this.label11);
			this.gBSensor3.Controls.Add(this.label12);
			this.gBSensor3.Controls.Add(this.label13);
			this.gBSensor3.Controls.Add(this.tbMaxPress3);
			this.gBSensor3.Controls.Add(this.tbMinPress3);
			this.gBSensor3.Controls.Add(this.label14);
			this.gBSensor3.Controls.Add(this.label15);
			this.gBSensor3.Controls.Add(this.label17);
			this.gBSensor3.Controls.Add(this.tbMaxTemperature3);
			this.gBSensor3.Controls.Add(this.label19);
			this.gBSensor3.Controls.Add(this.tbMinTemperature3);
			this.gBSensor3.Controls.Add(this.label20);
			this.gBSensor3.Controls.Add(this.tbMaxHumidity3);
			this.gBSensor3.Controls.Add(this.label21);
			this.gBSensor3.Controls.Add(this.tbMinHumidity3);
			this.gBSensor3.ForeColor = SystemColors.ControlText;
			componentResourceManager.ApplyResources(this.gBSensor3, "gBSensor3");
			this.gBSensor3.Name = "gBSensor3";
			this.gBSensor3.TabStop = false;
			componentResourceManager.ApplyResources(this.lbSS3P, "lbSS3P");
			this.lbSS3P.ForeColor = Color.Red;
			this.lbSS3P.Name = "lbSS3P";
			componentResourceManager.ApplyResources(this.lbSS3T, "lbSS3T");
			this.lbSS3T.ForeColor = Color.Red;
			this.lbSS3T.Name = "lbSS3T";
			componentResourceManager.ApplyResources(this.lbSS3H, "lbSS3H");
			this.lbSS3H.ForeColor = Color.Red;
			this.lbSS3H.Name = "lbSS3H";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.ForeColor = Color.Black;
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.ForeColor = Color.Black;
			this.label12.Name = "label12";
			componentResourceManager.ApplyResources(this.label13, "label13");
			this.label13.ForeColor = Color.Black;
			this.label13.Name = "label13";
			this.tbMaxPress3.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxPress3, "tbMaxPress3");
			this.tbMaxPress3.ForeColor = Color.Black;
			this.tbMaxPress3.Name = "tbMaxPress3";
			this.tbMaxPress3.Tag = "press";
			this.tbMaxPress3.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.tbMinPress3.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinPress3, "tbMinPress3");
			this.tbMinPress3.ForeColor = Color.Black;
			this.tbMinPress3.Name = "tbMinPress3";
			this.tbMinPress3.Tag = "press";
			this.tbMinPress3.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label14, "label14");
			this.label14.ForeColor = Color.Black;
			this.label14.Name = "label14";
			componentResourceManager.ApplyResources(this.label15, "label15");
			this.label15.ForeColor = Color.Black;
			this.label15.Name = "label15";
			componentResourceManager.ApplyResources(this.label17, "label17");
			this.label17.ForeColor = Color.Black;
			this.label17.Name = "label17";
			this.tbMaxTemperature3.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxTemperature3, "tbMaxTemperature3");
			this.tbMaxTemperature3.ForeColor = Color.Black;
			this.tbMaxTemperature3.Name = "tbMaxTemperature3";
			this.tbMaxTemperature3.Tag = "temperature";
			this.tbMaxTemperature3.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label19, "label19");
			this.label19.ForeColor = Color.Black;
			this.label19.Name = "label19";
			this.tbMinTemperature3.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinTemperature3, "tbMinTemperature3");
			this.tbMinTemperature3.ForeColor = Color.Black;
			this.tbMinTemperature3.Name = "tbMinTemperature3";
			this.tbMinTemperature3.Tag = "temperature";
			this.tbMinTemperature3.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label20, "label20");
			this.label20.ForeColor = Color.Black;
			this.label20.Name = "label20";
			this.tbMaxHumidity3.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxHumidity3, "tbMaxHumidity3");
			this.tbMaxHumidity3.ForeColor = Color.Black;
			this.tbMaxHumidity3.Name = "tbMaxHumidity3";
			this.tbMaxHumidity3.Tag = "humidity";
			this.tbMaxHumidity3.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label21, "label21");
			this.label21.ForeColor = Color.Black;
			this.label21.Name = "label21";
			this.tbMinHumidity3.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinHumidity3, "tbMinHumidity3");
			this.tbMinHumidity3.ForeColor = Color.Black;
			this.tbMinHumidity3.Name = "tbMinHumidity3";
			this.tbMinHumidity3.Tag = "humidity";
			this.tbMinHumidity3.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.gBSensor1.Controls.Add(this.lbSS1P);
			this.gBSensor1.Controls.Add(this.lbSS1T);
			this.gBSensor1.Controls.Add(this.lbSS1H);
			this.gBSensor1.Controls.Add(this.label33);
			this.gBSensor1.Controls.Add(this.label34);
			this.gBSensor1.Controls.Add(this.label24);
			this.gBSensor1.Controls.Add(this.tbMaxPress1);
			this.gBSensor1.Controls.Add(this.tbMinPress1);
			this.gBSensor1.Controls.Add(this.label130);
			this.gBSensor1.Controls.Add(this.label129);
			this.gBSensor1.Controls.Add(this.label115);
			this.gBSensor1.Controls.Add(this.tbMaxTemperature1);
			this.gBSensor1.Controls.Add(this.label116);
			this.gBSensor1.Controls.Add(this.tbMinTemperature1);
			this.gBSensor1.Controls.Add(this.label118);
			this.gBSensor1.Controls.Add(this.tbMaxHumidity1);
			this.gBSensor1.Controls.Add(this.label119);
			this.gBSensor1.Controls.Add(this.tbMinHumidity1);
			this.gBSensor1.ForeColor = SystemColors.ControlText;
			componentResourceManager.ApplyResources(this.gBSensor1, "gBSensor1");
			this.gBSensor1.Name = "gBSensor1";
			this.gBSensor1.TabStop = false;
			this.gBSensor2.Controls.Add(this.lbSS2P);
			this.gBSensor2.Controls.Add(this.lbSS2T);
			this.gBSensor2.Controls.Add(this.lbSS2H);
			this.gBSensor2.Controls.Add(this.label67);
			this.gBSensor2.Controls.Add(this.label68);
			this.gBSensor2.Controls.Add(this.label69);
			this.gBSensor2.Controls.Add(this.tbMaxPress2);
			this.gBSensor2.Controls.Add(this.tbMinPress2);
			this.gBSensor2.Controls.Add(this.label70);
			this.gBSensor2.Controls.Add(this.label71);
			this.gBSensor2.Controls.Add(this.label72);
			this.gBSensor2.Controls.Add(this.tbMaxTemperature2);
			this.gBSensor2.Controls.Add(this.label73);
			this.gBSensor2.Controls.Add(this.tbMinTemperature2);
			this.gBSensor2.Controls.Add(this.label74);
			this.gBSensor2.Controls.Add(this.tbMaxHumidity2);
			this.gBSensor2.Controls.Add(this.label75);
			this.gBSensor2.Controls.Add(this.tbMinHumidity2);
			this.gBSensor2.ForeColor = SystemColors.ControlText;
			componentResourceManager.ApplyResources(this.gBSensor2, "gBSensor2");
			this.gBSensor2.Name = "gBSensor2";
			this.gBSensor2.TabStop = false;
			componentResourceManager.ApplyResources(this.lbSS2P, "lbSS2P");
			this.lbSS2P.ForeColor = Color.Red;
			this.lbSS2P.Name = "lbSS2P";
			componentResourceManager.ApplyResources(this.lbSS2T, "lbSS2T");
			this.lbSS2T.ForeColor = Color.Red;
			this.lbSS2T.Name = "lbSS2T";
			componentResourceManager.ApplyResources(this.lbSS2H, "lbSS2H");
			this.lbSS2H.ForeColor = Color.Red;
			this.lbSS2H.Name = "lbSS2H";
			componentResourceManager.ApplyResources(this.label67, "label67");
			this.label67.ForeColor = Color.Black;
			this.label67.Name = "label67";
			componentResourceManager.ApplyResources(this.label68, "label68");
			this.label68.ForeColor = Color.Black;
			this.label68.Name = "label68";
			componentResourceManager.ApplyResources(this.label69, "label69");
			this.label69.ForeColor = Color.Black;
			this.label69.Name = "label69";
			this.tbMaxPress2.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxPress2, "tbMaxPress2");
			this.tbMaxPress2.ForeColor = Color.Black;
			this.tbMaxPress2.Name = "tbMaxPress2";
			this.tbMaxPress2.Tag = "press";
			this.tbMaxPress2.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			this.tbMinPress2.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinPress2, "tbMinPress2");
			this.tbMinPress2.ForeColor = Color.Black;
			this.tbMinPress2.Name = "tbMinPress2";
			this.tbMinPress2.Tag = "press";
			this.tbMinPress2.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label70, "label70");
			this.label70.ForeColor = Color.Black;
			this.label70.Name = "label70";
			componentResourceManager.ApplyResources(this.label71, "label71");
			this.label71.ForeColor = Color.Black;
			this.label71.Name = "label71";
			componentResourceManager.ApplyResources(this.label72, "label72");
			this.label72.ForeColor = Color.Black;
			this.label72.Name = "label72";
			this.tbMaxTemperature2.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxTemperature2, "tbMaxTemperature2");
			this.tbMaxTemperature2.ForeColor = Color.Black;
			this.tbMaxTemperature2.Name = "tbMaxTemperature2";
			this.tbMaxTemperature2.Tag = "temperature";
			this.tbMaxTemperature2.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label73, "label73");
			this.label73.ForeColor = Color.Black;
			this.label73.Name = "label73";
			this.tbMinTemperature2.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinTemperature2, "tbMinTemperature2");
			this.tbMinTemperature2.ForeColor = Color.Black;
			this.tbMinTemperature2.Name = "tbMinTemperature2";
			this.tbMinTemperature2.Tag = "temperature";
			this.tbMinTemperature2.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label74, "label74");
			this.label74.ForeColor = Color.Black;
			this.label74.Name = "label74";
			this.tbMaxHumidity2.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMaxHumidity2, "tbMaxHumidity2");
			this.tbMaxHumidity2.ForeColor = Color.Black;
			this.tbMaxHumidity2.Name = "tbMaxHumidity2";
			this.tbMaxHumidity2.Tag = "humidity";
			this.tbMaxHumidity2.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			componentResourceManager.ApplyResources(this.label75, "label75");
			this.label75.ForeColor = Color.Black;
			this.label75.Name = "label75";
			this.tbMinHumidity2.BackColor = Color.White;
			componentResourceManager.ApplyResources(this.tbMinHumidity2, "tbMinHumidity2");
			this.tbMinHumidity2.ForeColor = Color.Black;
			this.tbMinHumidity2.Name = "tbMinHumidity2";
			this.tbMinHumidity2.Tag = "humidity";
			this.tbMinHumidity2.KeyPress += new KeyPressEventHandler(this.threshold_KeyPress);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butSSAssign);
			base.Controls.Add(this.butSSSave);
			base.Controls.Add(this.gbSensorConfig);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropSensor";
			this.gbSensorConfig.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.gBSensor4.ResumeLayout(false);
			this.gBSensor4.PerformLayout();
			this.gBSensor3.ResumeLayout(false);
			this.gBSensor3.PerformLayout();
			this.gBSensor1.ResumeLayout(false);
			this.gBSensor1.PerformLayout();
			this.gBSensor2.ResumeLayout(false);
			this.gBSensor2.PerformLayout();
			base.ResumeLayout(false);
		}
		public PropSensor()
		{
			this.InitializeComponent();
			this.tbMinTemperature1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxTemperature1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinHumidity1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxHumidity1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinPress1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxPress1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinTemperature2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxTemperature2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinHumidity2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxHumidity2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinPress2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxPress2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinTemperature3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxTemperature3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinHumidity3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxHumidity3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinPress3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxPress3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinTemperature4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxTemperature4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinHumidity4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxHumidity4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMinPress4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMaxPress4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(int devID, bool onlinest)
		{
			this.butSSSave.Enabled = false;
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
			this.gBSensor1.Visible = false;
			this.gBSensor2.Visible = false;
			this.gBSensor3.Visible = false;
			this.gBSensor4.Visible = false;
			this.ConfigPageControlInit();
			if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
			{
				this.butSSAssign.Visible = false;
			}
			else
			{
				this.butSSAssign.Visible = true;
			}
			System.Collections.Generic.List<SensorInfo> sensorInfo = DeviceOperation.GetSensorInfo(devID);
			int num = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "ss");
			int num2 = 0;
			this.haveEditBox = false;
			foreach (SensorInfo current in sensorInfo)
			{
				if (num2 >= deviceModelConfig.sensorNum)
				{
					break;
				}
				num2++;
				switch (current.Type)
				{
				case 1:
					this.gBSensor1.Visible = true;
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature1, (num & 1) == 0, current.Min_temperature, 1, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature1, (num & 2) == 0, current.Max_temperature, 1, "F0");
						this.lbSS1T.Text = ((!this.tbMinTemperature1.ReadOnly || !this.tbMaxTemperature1.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 1, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature1, (num & 1) == 0, current.Min_temperature, 1, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature1, (num & 2) == 0, current.Max_temperature, 1, "F1");
						this.lbSS1T.Text = ((!this.tbMinTemperature1.ReadOnly || !this.tbMaxTemperature1.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 1, "F1") : "");
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.label115.Text = "°C";
					}
					else
					{
						this.label115.Text = "°F";
					}
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity1, (num & 4) == 0, current.Min_humidity, 0, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity1, (num & 8) == 0, current.Max_humidity, 0, "F0");
						this.lbSS1H.Text = ((!this.tbMinHumidity1.ReadOnly || !this.tbMaxHumidity1.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 1, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity1, (num & 4) == 0, current.Min_humidity, 0, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity1, (num & 8) == 0, current.Max_humidity, 0, "F1");
						this.lbSS1H.Text = ((!this.tbMinHumidity1.ReadOnly || !this.tbMaxHumidity1.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 1, "F1") : "");
					}
					ThresholdUtil.SetUIEdit(this.tbMinPress1, (num & 16) == 0, current.Min_press, 0, "F1");
					ThresholdUtil.SetUIEdit(this.tbMaxPress1, (num & 32) == 0, current.Max_press, 0, "F1");
					this.lbSS1P.Text = ((!this.tbMinPress1.ReadOnly || !this.tbMaxPress1.ReadOnly) ? devcfgUtil.RangePress(deviceModelConfig, "ss", 1) : "");
					if (!this.tbMinTemperature1.ReadOnly || !this.tbMaxTemperature1.ReadOnly || !this.tbMinHumidity1.ReadOnly || !this.tbMaxHumidity1.ReadOnly || !this.tbMinPress1.ReadOnly || !this.tbMaxPress1.ReadOnly)
					{
						this.haveEditBox = true;
					}
					break;
				case 2:
					this.gBSensor2.Visible = true;
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature2, (num & 1) == 0, current.Min_temperature, 1, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature2, (num & 2) == 0, current.Max_temperature, 1, "F0");
						this.lbSS2T.Text = ((!this.tbMinTemperature2.ReadOnly || !this.tbMaxTemperature2.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 2, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature2, (num & 1) == 0, current.Min_temperature, 1, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature2, (num & 2) == 0, current.Max_temperature, 1, "F1");
						this.lbSS2T.Text = ((!this.tbMinTemperature2.ReadOnly || !this.tbMaxTemperature2.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 2, "F1") : "");
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.label72.Text = "°C";
					}
					else
					{
						this.label72.Text = "°F";
					}
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity1, (num & 4) == 0, current.Min_humidity, 0, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity1, (num & 8) == 0, current.Max_humidity, 0, "F0");
						this.lbSS1H.Text = ((!this.tbMinHumidity1.ReadOnly || !this.tbMaxHumidity1.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 1, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity2, (num & 4) == 0, current.Min_humidity, 0, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity2, (num & 8) == 0, current.Max_humidity, 0, "F1");
						this.lbSS2H.Text = ((!this.tbMinHumidity2.ReadOnly || !this.tbMaxHumidity2.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 2, "F1") : "");
					}
					ThresholdUtil.SetUIEdit(this.tbMinPress2, (num & 16) == 0, current.Min_press, 0, "F1");
					ThresholdUtil.SetUIEdit(this.tbMaxPress2, (num & 32) == 0, current.Max_press, 0, "F1");
					this.lbSS2P.Text = ((!this.tbMinPress2.ReadOnly || !this.tbMaxPress2.ReadOnly) ? devcfgUtil.RangePress(deviceModelConfig, "ss", 2) : "");
					if (!this.tbMinTemperature2.ReadOnly || !this.tbMaxTemperature2.ReadOnly || !this.tbMinHumidity2.ReadOnly || !this.tbMaxHumidity2.ReadOnly || !this.tbMinPress2.ReadOnly || !this.tbMaxPress2.ReadOnly)
					{
						this.haveEditBox = true;
					}
					break;
				case 3:
					this.gBSensor3.Visible = true;
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature3, (num & 1) == 0, current.Min_temperature, 1, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature3, (num & 2) == 0, current.Max_temperature, 1, "F0");
						this.lbSS3T.Text = ((!this.tbMinTemperature3.ReadOnly || !this.tbMaxTemperature3.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 3, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature3, (num & 1) == 0, current.Min_temperature, 1, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature3, (num & 2) == 0, current.Max_temperature, 1, "F1");
						this.lbSS3T.Text = ((!this.tbMinTemperature3.ReadOnly || !this.tbMaxTemperature3.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 3, "F1") : "");
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.label17.Text = "°C";
					}
					else
					{
						this.label17.Text = "°F";
					}
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity1, (num & 4) == 0, current.Min_humidity, 0, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity1, (num & 8) == 0, current.Max_humidity, 0, "F0");
						this.lbSS1H.Text = ((!this.tbMinHumidity1.ReadOnly || !this.tbMaxHumidity1.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 1, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity3, (num & 4) == 0, current.Min_humidity, 0, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity3, (num & 8) == 0, current.Max_humidity, 0, "F1");
						this.lbSS3H.Text = ((!this.tbMinHumidity3.ReadOnly || !this.tbMaxHumidity3.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 3, "F1") : "");
					}
					ThresholdUtil.SetUIEdit(this.tbMinPress3, (num & 16) == 0, current.Min_press, 0, "F1");
					ThresholdUtil.SetUIEdit(this.tbMaxPress3, (num & 32) == 0, current.Max_press, 0, "F1");
					this.lbSS3P.Text = ((!this.tbMinPress3.ReadOnly || !this.tbMaxPress3.ReadOnly) ? devcfgUtil.RangePress(deviceModelConfig, "ss", 3) : "");
					if (!this.tbMinTemperature3.ReadOnly || !this.tbMaxTemperature3.ReadOnly || !this.tbMinHumidity3.ReadOnly || !this.tbMaxHumidity3.ReadOnly || !this.tbMinPress3.ReadOnly || !this.tbMaxPress3.ReadOnly)
					{
						this.haveEditBox = true;
					}
					break;
				case 4:
					this.gBSensor4.Visible = true;
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature4, (num & 1) == 0, current.Min_temperature, 1, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature4, (num & 2) == 0, current.Max_temperature, 1, "F0");
						this.lbSS4T.Text = ((!this.tbMinTemperature4.ReadOnly || !this.tbMaxTemperature4.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 4, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinTemperature4, (num & 1) == 0, current.Min_temperature, 1, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxTemperature4, (num & 2) == 0, current.Max_temperature, 1, "F1");
						this.lbSS4T.Text = ((!this.tbMinTemperature4.ReadOnly || !this.tbMaxTemperature4.ReadOnly) ? devcfgUtil.RangeTemp(deviceModelConfig, "ss", 4, "F1") : "");
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.label35.Text = "°C";
					}
					else
					{
						this.label35.Text = "°F";
					}
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity1, (num & 4) == 0, current.Min_humidity, 0, "F0");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity1, (num & 8) == 0, current.Max_humidity, 0, "F0");
						this.lbSS1H.Text = ((!this.tbMinHumidity1.ReadOnly || !this.tbMaxHumidity1.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 1, "F0") : "");
					}
					else
					{
						ThresholdUtil.SetUIEdit(this.tbMinHumidity4, (num & 4) == 0, current.Min_humidity, 0, "F1");
						ThresholdUtil.SetUIEdit(this.tbMaxHumidity4, (num & 8) == 0, current.Max_humidity, 0, "F1");
						this.lbSS4H.Text = ((!this.tbMinHumidity4.ReadOnly || !this.tbMaxHumidity4.ReadOnly) ? devcfgUtil.RangeHumi(deviceModelConfig, "ss", 4, "F1") : "");
					}
					ThresholdUtil.SetUIEdit(this.tbMinPress4, (num & 16) == 0, current.Min_press, 0, "F1");
					ThresholdUtil.SetUIEdit(this.tbMaxPress4, (num & 32) == 0, current.Max_press, 0, "F1");
					this.lbSS4P.Text = ((!this.tbMinPress4.ReadOnly || !this.tbMaxPress4.ReadOnly) ? devcfgUtil.RangePress(deviceModelConfig, "ss", 4) : "");
					if (!this.tbMinTemperature4.ReadOnly || !this.tbMaxTemperature4.ReadOnly || !this.tbMinHumidity4.ReadOnly || !this.tbMaxHumidity4.ReadOnly || !this.tbMinPress4.ReadOnly || !this.tbMaxPress4.ReadOnly)
					{
						this.haveEditBox = true;
					}
					break;
				}
			}
			if (!onlinest)
			{
				this.butSSSave.Enabled = onlinest;
				return;
			}
			if (!this.haveEditBox)
			{
				this.butSSSave.Enabled = false;
				return;
			}
			this.butSSSave.Enabled = true;
		}
		private void ConfigPageControlInit()
		{
			this.tbMinHumidity1.BackColor = Color.White;
			this.tbMaxHumidity1.BackColor = Color.White;
			this.tbMinTemperature1.BackColor = Color.White;
			this.tbMaxTemperature1.BackColor = Color.White;
			this.tbMinPress1.BackColor = Color.White;
			this.tbMaxPress1.BackColor = Color.White;
			this.tbMinHumidity2.BackColor = Color.White;
			this.tbMaxHumidity2.BackColor = Color.White;
			this.tbMinTemperature2.BackColor = Color.White;
			this.tbMaxTemperature2.BackColor = Color.White;
			this.tbMinPress2.BackColor = Color.White;
			this.tbMaxPress2.BackColor = Color.White;
			this.tbMinHumidity3.BackColor = Color.White;
			this.tbMaxHumidity3.BackColor = Color.White;
			this.tbMinTemperature3.BackColor = Color.White;
			this.tbMaxTemperature3.BackColor = Color.White;
			this.tbMinPress3.BackColor = Color.White;
			this.tbMaxPress3.BackColor = Color.White;
			this.tbMinHumidity4.BackColor = Color.White;
			this.tbMaxHumidity4.BackColor = Color.White;
			this.tbMinTemperature4.BackColor = Color.White;
			this.tbMaxTemperature4.BackColor = Color.White;
			this.tbMinPress4.BackColor = Color.White;
			this.tbMaxPress4.BackColor = Color.White;
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
			if (!onlinest)
			{
				this.butSSSave.Enabled = onlinest;
				return;
			}
			if (!this.haveEditBox)
			{
				this.butSSSave.Enabled = false;
				return;
			}
			this.butSSSave.Enabled = true;
		}
		private void threshold_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(textBox, e.KeyChar, 1);
			if (flag)
			{
				char keyChar = e.KeyChar;
				if ((keyChar == '.' || keyChar == ',') && (textBox.Name.Equals(this.tbMinTemperature1.Name) || textBox.Name.Equals(this.tbMaxTemperature1.Name) || textBox.Name.Equals(this.tbMinTemperature2.Name) || textBox.Name.Equals(this.tbMaxTemperature2.Name) || textBox.Name.Equals(this.tbMinTemperature3.Name) || textBox.Name.Equals(this.tbMaxTemperature3.Name) || textBox.Name.Equals(this.tbMinTemperature4.Name) || textBox.Name.Equals(this.tbMaxTemperature4.Name) || textBox.Name.Equals(this.tbMinHumidity1.Name) || textBox.Name.Equals(this.tbMaxHumidity1.Name) || textBox.Name.Equals(this.tbMinHumidity2.Name) || textBox.Name.Equals(this.tbMaxHumidity2.Name) || textBox.Name.Equals(this.tbMinHumidity3.Name) || textBox.Name.Equals(this.tbMaxHumidity3.Name) || textBox.Name.Equals(this.tbMinHumidity4.Name) || textBox.Name.Equals(this.tbMaxHumidity4.Name)))
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
		private void butSSSave_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			string text = this.labDevModel.Tag.ToString();
			try
			{
				int num = System.Convert.ToInt32(text);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.SSConfigCheck(deviceModelConfig))
				{
					DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
					DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
					string myMac = deviceByID.Mac;
					int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "ss");
					if (this.gBSensor1.Visible)
					{
						SensorInfo sensorInfo = new SensorInfo(num, 1);
						sensorInfo.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature1, sensorInfo.Min_temperature, 1);
						sensorInfo.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature1, sensorInfo.Max_temperature, 1);
						sensorInfo.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity1, sensorInfo.Min_humidity, 0);
						sensorInfo.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity1, sensorInfo.Max_humidity, 0);
						sensorInfo.Min_press = ThresholdUtil.UI2DB(this.tbMinPress1, sensorInfo.Min_press, 0);
						sensorInfo.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress1, sensorInfo.Max_press, 0);
						SensorThreshold sensorThreshold = new SensorThreshold(1);
						sensorThreshold.MinTemperatureMT = sensorInfo.Min_temperature;
						sensorThreshold.MaxTemperatureMT = sensorInfo.Max_temperature;
						sensorThreshold.MinHumidityMT = sensorInfo.Min_humidity;
						sensorThreshold.MaxHumidityMT = sensorInfo.Max_humidity;
						sensorThreshold.MinPressMT = sensorInfo.Min_press;
						sensorThreshold.MaxPressMT = sensorInfo.Max_press;
						ThresholdUtil.UI2Dev(thflg, sensorThreshold, sensorInfo);
						if (!devAccessAPI.SetSensorThreshold(sensorThreshold, myMac))
						{
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
							return;
						}
						sensorInfo.Update();
						flag = true;
						myMac = "";
					}
					if (this.gBSensor2.Visible)
					{
						SensorInfo sensorInfo2 = new SensorInfo(num, 2);
						sensorInfo2.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature2, sensorInfo2.Min_temperature, 1);
						sensorInfo2.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature2, sensorInfo2.Max_temperature, 1);
						sensorInfo2.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity2, sensorInfo2.Min_humidity, 0);
						sensorInfo2.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity2, sensorInfo2.Max_humidity, 0);
						sensorInfo2.Min_press = ThresholdUtil.UI2DB(this.tbMinPress2, sensorInfo2.Min_press, 0);
						sensorInfo2.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress2, sensorInfo2.Max_press, 0);
						SensorThreshold sensorThreshold2 = new SensorThreshold(2);
						sensorThreshold2.MinTemperatureMT = sensorInfo2.Min_temperature;
						sensorThreshold2.MaxTemperatureMT = sensorInfo2.Max_temperature;
						sensorThreshold2.MinHumidityMT = sensorInfo2.Min_humidity;
						sensorThreshold2.MaxHumidityMT = sensorInfo2.Max_humidity;
						sensorThreshold2.MinPressMT = sensorInfo2.Min_press;
						sensorThreshold2.MaxPressMT = sensorInfo2.Max_press;
						ThresholdUtil.UI2Dev(thflg, sensorThreshold2, sensorInfo2);
						if (!devAccessAPI.SetSensorThreshold(sensorThreshold2, myMac))
						{
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
							return;
						}
						sensorInfo2.Update();
						flag = true;
						myMac = "";
					}
					if (this.gBSensor3.Visible)
					{
						SensorInfo sensorInfo3 = new SensorInfo(num, 3);
						sensorInfo3.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature3, sensorInfo3.Min_temperature, 1);
						sensorInfo3.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature3, sensorInfo3.Max_temperature, 1);
						sensorInfo3.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity3, sensorInfo3.Min_humidity, 0);
						sensorInfo3.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity3, sensorInfo3.Max_humidity, 0);
						sensorInfo3.Min_press = ThresholdUtil.UI2DB(this.tbMinPress3, sensorInfo3.Min_press, 0);
						sensorInfo3.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress3, sensorInfo3.Max_press, 0);
						SensorThreshold sensorThreshold3 = new SensorThreshold(3);
						sensorThreshold3.MinTemperatureMT = sensorInfo3.Min_temperature;
						sensorThreshold3.MaxTemperatureMT = sensorInfo3.Max_temperature;
						sensorThreshold3.MinHumidityMT = sensorInfo3.Min_humidity;
						sensorThreshold3.MaxHumidityMT = sensorInfo3.Max_humidity;
						sensorThreshold3.MinPressMT = sensorInfo3.Min_press;
						sensorThreshold3.MaxPressMT = sensorInfo3.Max_press;
						ThresholdUtil.UI2Dev(thflg, sensorThreshold3, sensorInfo3);
						if (!devAccessAPI.SetSensorThreshold(sensorThreshold3, myMac))
						{
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
							return;
						}
						sensorInfo3.Update();
						flag = true;
						myMac = "";
					}
					if (this.gBSensor4.Visible)
					{
						SensorInfo sensorInfo4 = new SensorInfo(num, 4);
						sensorInfo4.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature4, sensorInfo4.Min_temperature, 1);
						sensorInfo4.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature4, sensorInfo4.Max_temperature, 1);
						sensorInfo4.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity4, sensorInfo4.Min_humidity, 0);
						sensorInfo4.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity4, sensorInfo4.Max_humidity, 0);
						sensorInfo4.Min_press = ThresholdUtil.UI2DB(this.tbMinPress4, sensorInfo4.Min_press, 0);
						sensorInfo4.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress4, sensorInfo4.Max_press, 0);
						SensorThreshold sensorThreshold4 = new SensorThreshold(4);
						sensorThreshold4.MinTemperatureMT = sensorInfo4.Min_temperature;
						sensorThreshold4.MaxTemperatureMT = sensorInfo4.Max_temperature;
						sensorThreshold4.MinHumidityMT = sensorInfo4.Min_humidity;
						sensorThreshold4.MaxHumidityMT = sensorInfo4.Max_humidity;
						sensorThreshold4.MinPressMT = sensorInfo4.Min_press;
						sensorThreshold4.MaxPressMT = sensorInfo4.Max_press;
						ThresholdUtil.UI2Dev(thflg, sensorThreshold4, sensorInfo4);
						if (!devAccessAPI.SetSensorThreshold(sensorThreshold4, myMac))
						{
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
							return;
						}
						sensorInfo4.Update();
						flag = true;
					}
					string valuePair = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair))
					{
						LogAPI.writeEventLog("0630030", new string[]
						{
							deviceByID.DeviceName,
							deviceByID.Mac,
							deviceByID.DeviceIP,
							valuePair
						});
					}
					else
					{
						LogAPI.writeEventLog("0630030", new string[]
						{
							deviceByID.DeviceName,
							deviceByID.Mac,
							deviceByID.DeviceIP
						});
					}
					EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("PropSensor Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
			finally
			{
				if (flag)
				{
					EcoGlobalVar.setDashBoardFlg(128uL, "#UPDATE#D" + text + ":S*;", 2);
				}
			}
		}
		private bool SSConfigCheck(DevModelConfig devcfg)
		{
			string arg_0B_0 = this.labDevIp.Text;
			this.labDevModel.Tag.ToString();
			bool flag = true;
			int num = devcfgUtil.UIThresholdEditFlg(devcfg, "ss");
			if (this.gBSensor1.Visible)
			{
				Ecovalidate.checkThresholdValue(this.tbMinTemperature1, this.lbSS1T, (num & 256) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxTemperature1, this.lbSS1T, (num & 512) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinHumidity1, this.lbSS1H, (num & 1024) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxHumidity1, this.lbSS1H, (num & 2048) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinPress1, this.lbSS1P, (num & 4096) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxPress1, this.lbSS1P, (num & 8192) == 0, ref flag);
			}
			if (this.gBSensor2.Visible)
			{
				Ecovalidate.checkThresholdValue(this.tbMinTemperature2, this.lbSS2T, (num & 256) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxTemperature2, this.lbSS2T, (num & 512) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinHumidity2, this.lbSS2H, (num & 1024) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxHumidity2, this.lbSS2H, (num & 2048) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinPress2, this.lbSS2P, (num & 4096) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxPress2, this.lbSS2P, (num & 8192) == 0, ref flag);
			}
			if (this.gBSensor3.Visible)
			{
				Ecovalidate.checkThresholdValue(this.tbMinTemperature3, this.lbSS3T, (num & 256) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxTemperature3, this.lbSS3T, (num & 512) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinHumidity3, this.lbSS3H, (num & 1024) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxHumidity3, this.lbSS3H, (num & 2048) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinPress3, this.lbSS3P, (num & 4096) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxPress3, this.lbSS3P, (num & 8192) == 0, ref flag);
			}
			if (this.gBSensor4.Visible)
			{
				Ecovalidate.checkThresholdValue(this.tbMinTemperature4, this.lbSS4T, (num & 256) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxTemperature4, this.lbSS4T, (num & 512) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinHumidity4, this.lbSS4H, (num & 1024) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxHumidity4, this.lbSS4H, (num & 2048) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMinPress4, this.lbSS4P, (num & 4096) == 0, ref flag);
				Ecovalidate.checkThresholdValue(this.tbMaxPress4, this.lbSS4P, (num & 8192) == 0, ref flag);
			}
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_Thresholdinvalid, new string[0]));
				return false;
			}
			if (this.gBSensor1.Visible)
			{
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxHumidity1, this.tbMinHumidity1, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxTemperature1, this.tbMinTemperature1, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxPress1, this.tbMinPress1, ref flag);
			}
			if (this.gBSensor2.Visible)
			{
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxHumidity2, this.tbMinHumidity2, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxTemperature2, this.tbMinTemperature2, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxPress2, this.tbMinPress2, ref flag);
			}
			if (this.gBSensor3.Visible)
			{
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxHumidity3, this.tbMinHumidity3, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxTemperature3, this.tbMinTemperature3, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxPress3, this.tbMinPress3, ref flag);
			}
			if (this.gBSensor4.Visible)
			{
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxHumidity4, this.tbMinHumidity4, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxTemperature4, this.tbMinTemperature4, ref flag);
				Ecovalidate.checkThresholdMaxMixValue(this.tbMaxPress4, this.tbMinPress4, ref flag);
			}
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdMinMax, new string[0]));
				return false;
			}
			return true;
		}
		private void butSSAssign_Click(object sender, System.EventArgs e)
		{
			try
			{
				string value = this.labDevModel.Tag.ToString();
				int num = System.Convert.ToInt32(value);
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
				if (this.SSConfigCheck(deviceModelConfig))
				{
					DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Dev_ApplyAll, new string[0]), MessageBoxButtons.OKCancel);
					if (dialogResult != DialogResult.Cancel)
					{
						string text = this.labDevModel.Text;
						this.labDevModel.Tag.ToString();
						System.Collections.Generic.List<DeviceInfo> allDeviceByModel = DeviceOperation.GetAllDeviceByModel(text);
						string fWVersion = deviceByID.FWVersion;
						System.Collections.Generic.List<DeviceInfo> list = new System.Collections.Generic.List<DeviceInfo>();
						foreach (DeviceInfo current in allDeviceByModel)
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
						SensorThreshold sensorThreshold = null;
						SensorThreshold sensorThreshold2 = null;
						SensorThreshold sensorThreshold3 = null;
						SensorThreshold sensorThreshold4 = null;
						SensorInfo sensorInfo = null;
						SensorInfo sensorInfo2 = null;
						SensorInfo sensorInfo3 = null;
						SensorInfo sensorInfo4 = null;
						int thflg = devcfgUtil.ThresholdFlg(deviceModelConfig, "ss");
						if (this.gBSensor1.Visible)
						{
							sensorInfo = new SensorInfo(num, 1);
							sensorInfo.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature1, sensorInfo.Min_temperature, 1);
							sensorInfo.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature1, sensorInfo.Max_temperature, 1);
							sensorInfo.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity1, sensorInfo.Min_humidity, 0);
							sensorInfo.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity1, sensorInfo.Max_humidity, 0);
							sensorInfo.Min_press = ThresholdUtil.UI2DB(this.tbMinPress1, sensorInfo.Min_press, 0);
							sensorInfo.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress1, sensorInfo.Max_press, 0);
							sensorThreshold = new SensorThreshold(1);
							sensorThreshold.MinTemperatureMT = sensorInfo.Min_temperature;
							sensorThreshold.MaxTemperatureMT = sensorInfo.Max_temperature;
							sensorThreshold.MinHumidityMT = sensorInfo.Min_humidity;
							sensorThreshold.MaxHumidityMT = sensorInfo.Max_humidity;
							sensorThreshold.MinPressMT = sensorInfo.Min_press;
							sensorThreshold.MaxPressMT = sensorInfo.Max_press;
							ThresholdUtil.UI2Dev(thflg, sensorThreshold, sensorInfo);
						}
						if (this.gBSensor2.Visible)
						{
							sensorInfo2 = new SensorInfo(num, 2);
							sensorInfo2.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature2, sensorInfo2.Min_temperature, 1);
							sensorInfo2.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature2, sensorInfo2.Max_temperature, 1);
							sensorInfo2.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity2, sensorInfo2.Min_humidity, 0);
							sensorInfo2.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity2, sensorInfo2.Max_humidity, 0);
							sensorInfo2.Min_press = ThresholdUtil.UI2DB(this.tbMinPress2, sensorInfo2.Min_press, 0);
							sensorInfo2.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress2, sensorInfo2.Max_press, 0);
							sensorThreshold2 = new SensorThreshold(2);
							sensorThreshold2.MinTemperatureMT = sensorInfo2.Min_temperature;
							sensorThreshold2.MaxTemperatureMT = sensorInfo2.Max_temperature;
							sensorThreshold2.MinHumidityMT = sensorInfo2.Min_humidity;
							sensorThreshold2.MaxHumidityMT = sensorInfo2.Max_humidity;
							sensorThreshold2.MinPressMT = sensorInfo2.Min_press;
							sensorThreshold2.MaxPressMT = sensorInfo2.Max_press;
							ThresholdUtil.UI2Dev(thflg, sensorThreshold2, sensorInfo2);
						}
						if (this.gBSensor3.Visible)
						{
							sensorInfo3 = new SensorInfo(num, 3);
							sensorInfo3.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature3, sensorInfo3.Min_temperature, 1);
							sensorInfo3.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature3, sensorInfo3.Max_temperature, 1);
							sensorInfo3.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity3, sensorInfo3.Min_humidity, 0);
							sensorInfo3.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity3, sensorInfo3.Max_humidity, 0);
							sensorInfo3.Min_press = ThresholdUtil.UI2DB(this.tbMinPress3, sensorInfo3.Min_press, 0);
							sensorInfo3.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress3, sensorInfo3.Max_press, 0);
							sensorThreshold3 = new SensorThreshold(3);
							sensorThreshold3.MinTemperatureMT = sensorInfo3.Min_temperature;
							sensorThreshold3.MaxTemperatureMT = sensorInfo3.Max_temperature;
							sensorThreshold3.MinHumidityMT = sensorInfo3.Min_humidity;
							sensorThreshold3.MaxHumidityMT = sensorInfo3.Max_humidity;
							sensorThreshold3.MinPressMT = sensorInfo3.Min_press;
							sensorThreshold3.MaxPressMT = sensorInfo3.Max_press;
							ThresholdUtil.UI2Dev(thflg, sensorThreshold3, sensorInfo3);
						}
						if (this.gBSensor4.Visible)
						{
							sensorInfo4 = new SensorInfo(num, 4);
							sensorInfo4.Min_temperature = ThresholdUtil.UI2DB(this.tbMinTemperature4, sensorInfo4.Min_temperature, 1);
							sensorInfo4.Max_temperature = ThresholdUtil.UI2DB(this.tbMaxTemperature4, sensorInfo4.Max_temperature, 1);
							sensorInfo4.Min_humidity = ThresholdUtil.UI2DB(this.tbMinHumidity4, sensorInfo4.Min_humidity, 0);
							sensorInfo4.Max_humidity = ThresholdUtil.UI2DB(this.tbMaxHumidity4, sensorInfo4.Max_humidity, 0);
							sensorInfo4.Min_press = ThresholdUtil.UI2DB(this.tbMinPress4, sensorInfo4.Min_press, 0);
							sensorInfo4.Max_press = ThresholdUtil.UI2DB(this.tbMaxPress4, sensorInfo4.Max_press, 0);
							sensorThreshold4 = new SensorThreshold(4);
							sensorThreshold4.MinTemperatureMT = sensorInfo4.Min_temperature;
							sensorThreshold4.MaxTemperatureMT = sensorInfo4.Max_temperature;
							sensorThreshold4.MinHumidityMT = sensorInfo4.Min_humidity;
							sensorThreshold4.MaxHumidityMT = sensorInfo4.Max_humidity;
							sensorThreshold4.MinPressMT = sensorInfo4.Min_press;
							sensorThreshold4.MaxPressMT = sensorInfo4.Max_press;
							ThresholdUtil.UI2Dev(thflg, sensorThreshold4, sensorInfo4);
						}
						System.Collections.Generic.List<DevSnmpConfig> list2 = new System.Collections.Generic.List<DevSnmpConfig>();
						foreach (DeviceInfo current2 in list)
						{
							if (ClientAPI.IsDeviceOnline(current2.DeviceID))
							{
								DevSnmpConfig sNMPpara = commUtil.getSNMPpara(current2);
								list2.Add(sNMPpara);
							}
						}
						System.Collections.Generic.List<object> list3 = new System.Collections.Generic.List<object>();
						list3.Add(sensorInfo);
						list3.Add(sensorInfo2);
						list3.Add(sensorInfo3);
						list3.Add(sensorInfo4);
						list3.Add(sensorThreshold);
						list3.Add(sensorThreshold2);
						list3.Add(sensorThreshold3);
						list3.Add(sensorThreshold4);
						list3.Add(list2);
						list3.Add(list);
						Program.IdleTimer_Pause(1);
						System.Collections.Generic.List<object> list4;
						if (list2.Count > 50)
						{
							progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_setSSThreshold, new string[0]), null, new progressPopup.ProcessInThread(this.SetSensorThresholdProc), list3, 0);
							progressPopup.ShowDialog();
							list4 = (progressPopup.Return_V as System.Collections.Generic.List<object>);
						}
						else
						{
							list4 = (this.SetSensorThresholdProc(list3) as System.Collections.Generic.List<object>);
						}
						Program.IdleTimer_Run(1);
						bool flag = (bool)list4[0];
						string text2 = (string)list4[1];
						if (flag)
						{
							string valuePair = ValuePairs.getValuePair("Username");
							if (!string.IsNullOrEmpty(valuePair))
							{
								LogAPI.writeEventLog("0630031", new string[]
								{
									text,
									valuePair
								});
							}
							else
							{
								LogAPI.writeEventLog("0630031", new string[]
								{
									text
								});
							}
							EcoGlobalVar.setDashBoardFlg(2uL, "", 2);
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
				System.Console.WriteLine("Sensor -- butSSAssign_Click Error:" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private object SetSensorThresholdProc(object param)
		{
			DBConn dBConn = null;
			string text = "";
			bool flag = false;
			try
			{
				System.Collections.Generic.List<object> list = (System.Collections.Generic.List<object>)param;
				SensorInfo sensorInfo = (SensorInfo)list[0];
				SensorInfo sensorInfo2 = (SensorInfo)list[1];
				SensorInfo sensorInfo3 = (SensorInfo)list[2];
				SensorInfo sensorInfo4 = (SensorInfo)list[3];
				SensorThreshold sSThreshold = (SensorThreshold)list[4];
				SensorThreshold sSThreshold2 = (SensorThreshold)list[5];
				SensorThreshold sSThreshold3 = (SensorThreshold)list[6];
				SensorThreshold sSThreshold4 = (SensorThreshold)list[7];
				System.Collections.Generic.List<DevSnmpConfig> configs = (System.Collections.Generic.List<DevSnmpConfig>)list[8];
				System.Collections.Generic.List<DeviceInfo> list2 = (System.Collections.Generic.List<DeviceInfo>)list[9];
				AppDevAccess appDevAccess = new AppDevAccess();
				System.Collections.Generic.Dictionary<string, bool> dictionary = null;
				System.Collections.Generic.Dictionary<string, bool> dictionary2 = null;
				System.Collections.Generic.Dictionary<string, bool> dictionary3 = null;
				System.Collections.Generic.Dictionary<string, bool> dictionary4 = null;
				if (sensorInfo != null)
				{
					dictionary = appDevAccess.SetSensorThreshold(configs, sSThreshold);
				}
				if (sensorInfo2 != null)
				{
					dictionary2 = appDevAccess.SetSensorThreshold(configs, sSThreshold2);
				}
				if (sensorInfo3 != null)
				{
					dictionary3 = appDevAccess.SetSensorThreshold(configs, sSThreshold3);
				}
				if (sensorInfo4 != null)
				{
					dictionary4 = appDevAccess.SetSensorThreshold(configs, sSThreshold4);
				}
				dBConn = DBConnPool.getConnection();
				foreach (DeviceInfo current in list2)
				{
					string key = CultureTransfer.ToString(current.DeviceID);
					bool flag2 = true;
					if (dictionary != null && dictionary.ContainsKey(key))
					{
						bool flag3 = dictionary[key];
						if (flag3)
						{
							SensorInfo sensorInfo5 = new SensorInfo(current.DeviceID, 1);
							sensorInfo5.CopyThreshold(sensorInfo);
							sensorInfo5.UpdateSensorThreshold(dBConn);
							flag = true;
						}
						else
						{
							flag2 = false;
						}
					}
					if (dictionary2 != null && dictionary2.ContainsKey(key))
					{
						bool flag3 = dictionary2[key];
						if (flag3)
						{
							SensorInfo sensorInfo6 = new SensorInfo(current.DeviceID, 2);
							sensorInfo6.CopyThreshold(sensorInfo2);
							sensorInfo6.UpdateSensorThreshold(dBConn);
							flag = true;
						}
						else
						{
							flag2 = false;
						}
					}
					if (dictionary3 != null && dictionary3.ContainsKey(key))
					{
						bool flag3 = dictionary3[key];
						if (flag3)
						{
							SensorInfo sensorInfo7 = new SensorInfo(current.DeviceID, 3);
							sensorInfo7.CopyThreshold(sensorInfo3);
							sensorInfo7.UpdateSensorThreshold(dBConn);
							flag = true;
						}
						else
						{
							flag2 = false;
						}
					}
					if (dictionary4 != null && dictionary4.ContainsKey(key))
					{
						bool flag3 = dictionary4[key];
						if (flag3)
						{
							SensorInfo sensorInfo8 = new SensorInfo(current.DeviceID, 4);
							sensorInfo8.CopyThreshold(sensorInfo4);
							sensorInfo8.UpdateSensorThreshold(dBConn);
							flag = true;
						}
						else
						{
							flag2 = false;
						}
					}
					if (!flag2)
					{
						text = text + current.DeviceIP + ",";
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("Sensor -- butSSAssign_Click Error:" + ex.Message);
				text = "HAVE EXCEPTION!";
			}
			finally
			{
				if (dBConn != null)
				{
					dBConn.close();
				}
				DeviceOperation.RefreshDBCache(false);
			}
			return new System.Collections.Generic.List<object>
			{
				flag,
				text
			};
		}
	}
}
