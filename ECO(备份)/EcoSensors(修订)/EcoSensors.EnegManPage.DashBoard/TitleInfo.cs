using CommonAPI.CultureTransfer;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.DashBoard
{
	public class TitleInfo : Form
	{
		private delegate void HideEvent();
		private const int IMG_folderclosed = 0;
		private const int IMG_folderopen = 1;
		private const int IMG_current_gray = 2;
		private const int IMG_current_green = 3;
		private const int IMG_current_orange = 4;
		private const int IMG_current_red = 5;
		private const int IMG_deviceoff = 6;
		private const int IMG_deviceon = 7;
		private const int IMG_humidity_gray = 8;
		private const int IMG_humidity_green = 9;
		private const int IMG_humidity_orange = 10;
		private const int IMG_humidity_red = 11;
		private const int IMG_power_gray = 12;
		private const int IMG_power_green = 13;
		private const int IMG_power_orange = 14;
		private const int IMG_power_red = 15;
		private const int IMG_pressure_gray = 16;
		private const int IMG_pressure_green = 17;
		private const int IMG_pressure_orange = 18;
		private const int IMG_pressure_red = 19;
		private const int IMG_temperature_green = 20;
		private const int IMG_temperature_gray = 21;
		private const int IMG_temperature_orange = 22;
		private const int IMG_temperature_red = 23;
		private const int IMG_voltage_gray = 24;
		private const int IMG_voltage_green = 25;
		private const int IMG_voltage_orange = 26;
		private const int IMG_voltage_red = 27;
		private const int IMG_sensor = 28;
		private const int IMG_tree_outleton = 29;
		private const int IMG_tree_bank = 30;
		private const int IMG_tree_AMPCapacity = 31;
		private const int IMG_tree_AMPinUse = 32;
		private const int IMG_tree_AMPAvailable = 33;
		private const int IMG_tree_line = 34;
		private const int IMG_tree_lineLeak = 35;
		private IContainer components;
		private TreeView treeView1;
		private ImageList imageList1;
		private System.Timers.Timer HideTimer;
		private long m_rackID;
		private string m_devids = "";
		private DataSet allData = new DataSet();
		private string m_curBorad_Tag;
		private bool m_frmMonitor;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TitleInfo));
			this.treeView1 = new TreeView();
			this.imageList1 = new ImageList(this.components);
			base.SuspendLayout();
			this.treeView1.BackColor = Color.White;
			this.treeView1.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.treeView1, "treeView1");
			this.treeView1.FullRowSelect = true;
			this.treeView1.ImageList = this.imageList1;
			this.treeView1.ItemHeight = 24;
			this.treeView1.Name = "treeView1";
			this.treeView1.ShowNodeToolTips = true;
			this.treeView1.ShowRootLines = false;
			this.treeView1.AfterCollapse += new TreeViewEventHandler(this.treeView1_AfterCollapse);
			this.treeView1.AfterExpand += new TreeViewEventHandler(this.treeView1_AfterExpand);
			this.treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			this.treeView1.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
			this.imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "folderclosed.gif");
			this.imageList1.Images.SetKeyName(1, "folderopen.gif");
			this.imageList1.Images.SetKeyName(2, "current_gray.gif");
			this.imageList1.Images.SetKeyName(3, "current_green.gif");
			this.imageList1.Images.SetKeyName(4, "current_orange.gif");
			this.imageList1.Images.SetKeyName(5, "current_red.gif");
			this.imageList1.Images.SetKeyName(6, "deviceoff.gif");
			this.imageList1.Images.SetKeyName(7, "deviceon.gif");
			this.imageList1.Images.SetKeyName(8, "humidity_gray.gif");
			this.imageList1.Images.SetKeyName(9, "humidity_green.gif");
			this.imageList1.Images.SetKeyName(10, "humidity_orangegif.gif");
			this.imageList1.Images.SetKeyName(11, "humidity_red.gif");
			this.imageList1.Images.SetKeyName(12, "power_gray.gif");
			this.imageList1.Images.SetKeyName(13, "power_green.gif");
			this.imageList1.Images.SetKeyName(14, "power_orange.gif");
			this.imageList1.Images.SetKeyName(15, "power_red.gif");
			this.imageList1.Images.SetKeyName(16, "pressure_gray.gif");
			this.imageList1.Images.SetKeyName(17, "pressure_green.gif");
			this.imageList1.Images.SetKeyName(18, "pressure_orange.gif");
			this.imageList1.Images.SetKeyName(19, "pressure_red.gif");
			this.imageList1.Images.SetKeyName(20, "temperature_geeen.gif");
			this.imageList1.Images.SetKeyName(21, "temperature_gray.gif");
			this.imageList1.Images.SetKeyName(22, "temperature_orange.gif");
			this.imageList1.Images.SetKeyName(23, "temperature_red.gif");
			this.imageList1.Images.SetKeyName(24, "voltage_gray.gif");
			this.imageList1.Images.SetKeyName(25, "voltage_green.gif");
			this.imageList1.Images.SetKeyName(26, "voltage_orange.gif");
			this.imageList1.Images.SetKeyName(27, "voltage_red.gif");
			this.imageList1.Images.SetKeyName(28, "sensor.gif");
			this.imageList1.Images.SetKeyName(29, "tree_outleton.gif");
			this.imageList1.Images.SetKeyName(30, "Bank.png");
			this.imageList1.Images.SetKeyName(31, "AmpCapacity.png");
			this.imageList1.Images.SetKeyName(32, "AmpinUse.png");
			this.imageList1.Images.SetKeyName(33, "AmpAvailable.png");
			this.imageList1.Images.SetKeyName(34, "line.png");
			this.imageList1.Images.SetKeyName(35, "leak.png");
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.LightBlue;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.treeView1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "TitleInfo";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.TopMost = true;
			base.ResumeLayout(false);
		}
		public TitleInfo()
		{
			this.InitializeComponent();
		}
		public TitleInfo(bool frmMonitor)
		{
			this.InitializeComponent();
			this.m_frmMonitor = frmMonitor;
		}
		public void Set(long rackID, string devids, DataSet ds, string curBorad_Tag)
		{
			Program.m_IdleCounter = 0;
			this.m_rackID = rackID;
			this.m_devids = devids;
			this.m_curBorad_Tag = curBorad_Tag;
			this.allData = ds;
			this.FillData();
			this.HideTimer = new System.Timers.Timer();
			this.HideTimer.Elapsed += new ElapsedEventHandler(this.HideTimer_Elapsed);
			this.HideTimer.Interval = 1000.0;
			this.HideTimer.AutoReset = true;
			this.HideTimer.Enabled = true;
		}
		private void HideTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new TitleInfo.HideEvent(this.HideForm));
				return;
			}
			this.HideForm();
		}
		private void HideForm()
		{
			if (Control.MousePosition.X >= base.Location.X && Control.MousePosition.X <= base.Location.X + base.Size.Width && Control.MousePosition.Y >= base.Location.Y && Control.MousePosition.Y <= base.Location.Y + base.Size.Height)
			{
				return;
			}
			this.HideTimer.Enabled = false;
			base.Hide();
		}
		private void FillData()
		{
			string[] array = this.m_devids.Split(new char[]
			{
				','
			});
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			this.treeView1.Nodes.Clear();
			string text = "";
			TreeNode treeNode = new TreeNode();
			treeNode.Name = "Root";
			treeNode.ImageIndex = 1;
			this.treeView1.Nodes.Add(treeNode);
			this.treeView1.Indent = 15;
			DataSet dataSet = new DataSet();
			try
			{
				dataSet = this.allData;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					int key = System.Convert.ToInt32(text2);
					if (devicRackZoneRelation.ContainsKey(key))
					{
						ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[key];
						DataTable dataTable = dataSet.Tables[0].Clone();
						DataRow[] array3 = dataSet.Tables[0].Select("device_id=" + text2);
						for (int j = 0; j < array3.Length; j++)
						{
							dataTable.ImportRow(array3[j]);
						}
						if (dataTable.Rows.Count > 0)
						{
							TreeNode treeNode2 = new TreeNode();
							treeNode2.Name = "Device";
							if (System.Convert.ToString(dataTable.Rows[0]["device_nm"]) != System.Convert.ToString(-1000))
							{
								if (System.Convert.ToString(dataTable.Rows[0]["device_nm"]) != "")
								{
									treeNode2.Text = System.Convert.ToString(dataTable.Rows[0]["device_nm"]);
								}
								else
								{
									treeNode2.Text = System.Convert.ToString(dataTable.Rows[0]["device_ip"]);
								}
							}
							else
							{
								treeNode2.Text = "";
							}
							text = deviceWithZoneRackInfo.rack_nm;
							if (System.Convert.ToString(dataTable.Rows[0]["device_state"]) != "1")
							{
								treeNode2.ImageIndex = 6;
								treeNode2.SelectedImageIndex = 6;
								treeNode.Nodes.Add(treeNode2);
							}
							else
							{
								treeNode2.ImageIndex = 7;
								treeNode2.SelectedImageIndex = 7;
								if (!this.m_frmMonitor && this.m_curBorad_Tag.Equals("0:1"))
								{
									string device_model = deviceWithZoneRackInfo.device_model;
									string fw_version = deviceWithZoneRackInfo.fw_version;
									DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model, fw_version);
									double num = devcfgUtil.ampMax(deviceModelConfig, "dev");
									double num2 = ecoConvert.f2d(dataTable.Rows[0]["current_value"]);
									TreeNode node;
									TreeNode node2;
									if (num == 0.0)
									{
										node = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AllAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Capacity, new string[0]) + ": N/A", 31);
										node2 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AvAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Available, new string[0]) + ": N/A", 33);
									}
									else
									{
										node = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AllAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Capacity, new string[0]) + ": ", num, "F2", "A", 31);
										node2 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AvAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Available, new string[0]) + ": ", num - num2, "F2", "A", 33);
									}
									treeNode2.Nodes.Add(node);
									TreeNode node3 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AmpCapInuse, new string[0]), " " + EcoLanguage.getMsg(LangRes.Title_Inuse, new string[0]) + ": ", num2, "F2", "A", 32);
									treeNode2.Nodes.Add(node3);
									treeNode2.Nodes.Add(node2);
									this.FillData_Bank(treeNode2, dataSet, text2, deviceModelConfig);
									this.FillData_Line(treeNode2, dataSet, text2, deviceModelConfig);
								}
								else
								{
									bool flag = true;
									double maxV = ecoConvert.f2d(dataTable.Rows[0]["max_power"]);
									double minV = ecoConvert.f2d(dataTable.Rows[0]["min_power"]);
									double maxV2 = ecoConvert.f2d(dataTable.Rows[0]["max_current"]);
									double minV2 = ecoConvert.f2d(dataTable.Rows[0]["min_current"]);
									double maxV3 = ecoConvert.f2d(dataTable.Rows[0]["max_voltage"]);
									double minV3 = ecoConvert.f2d(dataTable.Rows[0]["min_voltage"]);
									double maxV4 = ecoConvert.f2d(dataTable.Rows[0]["max_power_diss"]);
									double minV4 = ecoConvert.f2d(dataTable.Rows[0]["min_power_diss"]);
									string device_model = deviceWithZoneRackInfo.device_model;
									string fw_version = deviceWithZoneRackInfo.fw_version;
									DevModelConfig deviceModelConfig2 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model, fw_version);
									int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(deviceModelConfig2, "dev");
									TreeNode treeNode3 = this.genTreeNode(uIthEdidflg, 4, EcoLanguage.getMsg(LangRes.Title_PowerDiss, new string[0]), ecoConvert.f2d(dataTable.Rows[0]["power_consumption"]), minV4, maxV4, "F4", "kWh", 13, 15, 14, 12, ref flag);
									if (treeNode3 != null)
									{
										treeNode2.Nodes.Add(treeNode3);
									}
									TreeNode treeNode4 = this.genTreeNode(uIthEdidflg, 3, EcoLanguage.getMsg(LangRes.Title_Power, new string[0]), ecoConvert.f2d(dataTable.Rows[0]["power_value"]), minV, maxV, "F4", "W", 13, 15, 14, 12, ref flag);
									if (treeNode4 != null)
									{
										treeNode2.Nodes.Add(treeNode4);
									}
									TreeNode treeNode5 = this.genTreeNode(uIthEdidflg, 2, EcoLanguage.getMsg(LangRes.Title_Voltage, new string[0]), ecoConvert.f2d(dataTable.Rows[0]["voltage_value"]), minV3, maxV3, "F2", "V", 25, 27, 26, 24, ref flag);
									if (treeNode5 != null)
									{
										treeNode2.Nodes.Add(treeNode5);
									}
									TreeNode treeNode6 = this.genTreeNode(uIthEdidflg, 1, EcoLanguage.getMsg(LangRes.Title_Current, new string[0]), ecoConvert.f2d(dataTable.Rows[0]["current_value"]), minV2, maxV2, "F2", "A", 3, 5, 4, 2, ref flag);
									if (treeNode6 != null)
									{
										treeNode2.Nodes.Add(treeNode6);
									}
									if (deviceModelConfig2.leakCurrent == Constant.YES)
									{
										int num3 = (int)dataTable.Rows[0]["leakcurrent_status"];
										if (num3 == Constant.YES)
										{
											TreeNode treeNode7 = new TreeNode();
											treeNode7.Name = "Neutral leakage current";
											treeNode7.ToolTipText = treeNode7.Name;
											treeNode7.ImageIndex = 35;
											treeNode7.SelectedImageIndex = 35;
											treeNode7.Text = "Neutral leakage current";
											treeNode2.Nodes.Add(treeNode7);
											flag = false;
										}
									}
									if (!flag)
									{
										treeNode2.Expand();
									}
									this.FillData_Sensor(treeNode2, dataSet, text2, deviceModelConfig2);
									this.FillData_Outlet(treeNode2, dataSet, text2, deviceModelConfig2);
									this.FillData_Bank(treeNode2, dataSet, text2, deviceModelConfig2);
									this.FillData_Line(treeNode2, dataSet, text2, deviceModelConfig2);
								}
								treeNode.Nodes.Add(treeNode2);
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			if (this.m_frmMonitor)
			{
				treeNode.Text = text;
			}
			else
			{
				string curBorad_Tag;
				switch (curBorad_Tag = this.m_curBorad_Tag)
				{
				case "0:1":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  " + EcoLanguage.getMsg(LangRes.Title_Available, new string[0]) + ":";
					if (text3.Equals("Error"))
					{
						TreeNode expr_981 = treeNode;
						expr_981.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_9A8 = treeNode;
					expr_9A8.Text = expr_9A8.Text + text3 + "%";
					goto IL_11DA;
				}
				case "1:0":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  U:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_9F9 = treeNode;
						expr_9F9.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_A20 = treeNode;
					expr_A20.Text = expr_A20.Text + text3 + "kWh";
					goto IL_11DA;
				}
				case "1:1":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  LW:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_A71 = treeNode;
						expr_A71.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_A98 = treeNode;
					expr_A98.Text = expr_A98.Text + text3 + "W";
					goto IL_11DA;
				}
				case "2:0":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  t:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_AE9 = treeNode;
						expr_AE9.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						TreeNode expr_B17 = treeNode;
						expr_B17.Text = expr_B17.Text + text3 + "°C";
						goto IL_11DA;
					}
					double degrees = System.Convert.ToDouble(text3);
					TreeNode expr_B3E = treeNode;
					expr_B3E.Text = expr_B3E.Text + RackStatusAll.CtoFdegrees(degrees).ToString("F2") + "°F";
					goto IL_11DA;
				}
				case "2:1":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  △t:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_BA2 = treeNode;
						expr_BA2.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						TreeNode expr_BD0 = treeNode;
						expr_BD0.Text = expr_BD0.Text + text3 + "°C";
						goto IL_11DA;
					}
					double degrees = System.Convert.ToDouble(text3);
					TreeNode expr_BF7 = treeNode;
					expr_BF7.Text = expr_BF7.Text + RackStatusAll.CtoFdegrees(degrees).ToString("F2") + "°F";
					goto IL_11DA;
				}
				case "2:2":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  △T:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_C5B = treeNode;
						expr_C5B.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						TreeNode expr_C89 = treeNode;
						expr_C89.Text = expr_C89.Text + text3 + "°C";
						goto IL_11DA;
					}
					double degrees = System.Convert.ToDouble(text3);
					TreeNode expr_CB0 = treeNode;
					expr_CB0.Text = expr_CB0.Text + RackStatusAll.CtoFdegrees(degrees).ToString("F2") + "°F";
					goto IL_11DA;
				}
				case "2:3":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  T:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_D14 = treeNode;
						expr_D14.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						TreeNode expr_D42 = treeNode;
						expr_D42.Text = expr_D42.Text + text3 + "°C";
						goto IL_11DA;
					}
					double degrees = System.Convert.ToDouble(text3);
					TreeNode expr_D69 = treeNode;
					expr_D69.Text = expr_D69.Text + RackStatusAll.CtoFdegrees(degrees).ToString("F2") + "°F";
					goto IL_11DA;
				}
				case "2:4":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  △T_equip:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_DCD = treeNode;
						expr_DCD.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						TreeNode expr_DFB = treeNode;
						expr_DFB.Text = expr_DFB.Text + text3 + "°C";
						goto IL_11DA;
					}
					double degrees = System.Convert.ToDouble(text3);
					TreeNode expr_E22 = treeNode;
					expr_E22.Text = expr_E22.Text + RackStatusAll.CtoFdegrees(degrees).ToString("F2") + "°F";
					goto IL_11DA;
				}
				case "3:0":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  △P:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_E86 = treeNode;
						expr_E86.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_EAD = treeNode;
					expr_EAD.Text = expr_EAD.Text + text3 + "Pa";
					goto IL_11DA;
				}
				case "3:1":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  V_equip:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_EFE = treeNode;
						expr_EFE.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_F25 = treeNode;
					expr_F25.Text = expr_F25.Text + text3 + "cfm";
					goto IL_11DA;
				}
				case "3:2":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  Q_floor:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_F76 = treeNode;
						expr_F76.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_F9D = treeNode;
					expr_F9D.Text = expr_F9D.Text + text3 + "cfm";
					goto IL_11DA;
				}
				case "3:4":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  △A_circk:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_FEE = treeNode;
						expr_FEE.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_1015 = treeNode;
					expr_1015.Text = expr_1015.Text + text3 + "%";
					goto IL_11DA;
				}
				case "3:5":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  △A_bypas:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_1066 = treeNode;
						expr_1066.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_108D = treeNode;
					expr_108D.Text = expr_108D.Text + text3 + "%";
					goto IL_11DA;
				}
				case "4:0":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  h_relkx:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_10DE = treeNode;
						expr_10DE.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					TreeNode expr_1105 = treeNode;
					expr_1105.Text = expr_1105.Text + text3 + "%";
					goto IL_11DA;
				}
				case "4:1":
				{
					string text3 = RackStatusAll.SingleValue_List[this.m_rackID];
					treeNode.Text = text + "  t_dew:";
					if (text3.Equals("Error"))
					{
						TreeNode expr_1156 = treeNode;
						expr_1156.Text += EcoLanguage.getMsg(LangRes.Title_Error, new string[0]);
						goto IL_11DA;
					}
					if (EcoGlobalVar.TempUnit == 0)
					{
						TreeNode expr_1181 = treeNode;
						expr_1181.Text = expr_1181.Text + text3 + "°C";
						goto IL_11DA;
					}
					double degrees = System.Convert.ToDouble(text3);
					TreeNode expr_11A5 = treeNode;
					expr_11A5.Text = expr_11A5.Text + RackStatusAll.CtoFdegrees(degrees).ToString("F2") + "°F";
					goto IL_11DA;
				}
				}
				treeNode.Text = text;
			}
			IL_11DA:
			treeNode.Expand();
		}
		private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode[] array = this.treeView1.Nodes.Find("Root", false);
			if (array.Length > 0)
			{
				array[0].SelectedImageIndex = 1;
				this.treeView1.Refresh();
			}
		}
		private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Name == "Root")
			{
				node.SelectedImageIndex = 0;
				this.treeView1.Refresh();
				return;
			}
			TreeNode[] array = this.treeView1.Nodes.Find("Root", false);
			if (array.Length > 0)
			{
				array[0].SelectedImageIndex = 1;
				this.treeView1.Refresh();
			}
		}
		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Name != "Root")
			{
				TreeNode[] array = this.treeView1.Nodes.Find("Root", false);
				array[0].ImageIndex = 1;
				this.treeView1.Refresh();
			}
		}
		private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Name != "Root")
			{
				TreeNode[] array = this.treeView1.Nodes.Find("Root", false);
				array[0].ImageIndex = 1;
				this.treeView1.Refresh();
			}
		}
		private TreeNode genTreeNode(string strName, string showtext, double curV, string strfmt, string unit, int IMG_green)
		{
			return new TreeNode
			{
				Name = strName,
				ToolTipText = strName,
				ImageIndex = IMG_green,
				SelectedImageIndex = IMG_green,
				Text = showtext + curV.ToString(strfmt) + unit
			};
		}
		private TreeNode genTreeNode(string strName, string showtext, int IMG_green)
		{
			return new TreeNode
			{
				Name = strName,
				ToolTipText = strName,
				ImageIndex = IMG_green,
				SelectedImageIndex = IMG_green,
				Text = showtext
			};
		}
		private TreeNode genTreeNode(int UIthEdidflg, int measure_tp, string strName, double curV, double minV, double maxV, string strfmt, string unit, int IMG_green, int IMG_red, int IMG_orange, int IMG_gray, ref bool de_status)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			switch (measure_tp)
			{
			case 1:
				flag = devcfgUtil.haveMeasureCurrent(UIthEdidflg);
				num = 1;
				num2 = 2;
				break;
			case 2:
				flag = devcfgUtil.haveMeasureVoltage(UIthEdidflg);
				num = 4;
				num2 = 8;
				break;
			case 3:
				flag = devcfgUtil.haveMeasurePower(UIthEdidflg);
				num = 16;
				num2 = 32;
				break;
			case 4:
				flag = devcfgUtil.haveMeasurePowerD(UIthEdidflg);
				num = 64;
				num2 = 128;
				break;
			case 5:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 1, 2, minV, maxV);
				num = 1;
				num2 = 2;
				break;
			case 6:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 4, 8, minV, maxV);
				num = 4;
				num2 = 8;
				break;
			case 7:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 16, 32, minV, maxV);
				num = 16;
				num2 = 32;
				break;
			}
			if (!flag)
			{
				if (curV == -500.0)
				{
					return null;
				}
				TreeNode treeNode = new TreeNode();
				treeNode.Name = strName;
				treeNode.ToolTipText = strName;
				treeNode.ImageIndex = IMG_green;
				treeNode.SelectedImageIndex = IMG_green;
				treeNode.Text = curV.ToString(strfmt) + unit;
				if (curV == -1000.0)
				{
					treeNode.Text = "N/A";
				}
				return treeNode;
			}
			else
			{
				TreeNode treeNode = new TreeNode();
				treeNode.Name = strName;
				treeNode.ToolTipText = strName;
				if (minV == -500.0 && maxV == -500.0)
				{
					return null;
				}
				if (curV == -500.0)
				{
					treeNode.ImageIndex = IMG_red;
					treeNode.SelectedImageIndex = IMG_red;
					int num3 = (int)curV;
					treeNode.Text = EcoLanguage.getMsg(LangRes.Title_ConFail, new string[]
					{
						System.Convert.ToString(num3 + 501)
					});
					de_status = false;
					return treeNode;
				}
				treeNode.ImageIndex = IMG_green;
				treeNode.SelectedImageIndex = IMG_green;
				treeNode.Text = curV.ToString(strfmt) + unit;
				if (curV == -1000.0)
				{
					treeNode.Text = "N/A";
					return treeNode;
				}
				if (measure_tp == 5 && unit.Equals("°F"))
				{
					treeNode.Text = RackStatusAll.CtoFdegrees(curV).ToString(strfmt) + unit;
				}
				int num4 = 0;
				if ((UIthEdidflg & num) == 0 && minV != -300.0)
				{
					num4 |= 1;
				}
				if ((UIthEdidflg & num2) == 0 && maxV != -300.0)
				{
					num4 |= 2;
				}
				if (num4 == 3)
				{
					if (curV > maxV || curV < minV)
					{
						treeNode.ImageIndex = IMG_red;
						treeNode.SelectedImageIndex = IMG_red;
						de_status = false;
						return treeNode;
					}
					if (measure_tp == 1 || measure_tp == 3)
					{
						if (RackStatusAll.MaxWarningthreshold(maxV, minV) < curV && curV <= maxV)
						{
							treeNode.ImageIndex = IMG_orange;
							treeNode.SelectedImageIndex = IMG_orange;
							de_status = false;
						}
						return treeNode;
					}
					if ((curV >= minV && curV < RackStatusAll.MinWarningthreshold(maxV, minV)) || (RackStatusAll.MaxWarningthreshold(maxV, minV) < curV && curV <= maxV))
					{
						treeNode.ImageIndex = IMG_orange;
						treeNode.SelectedImageIndex = IMG_orange;
						de_status = false;
					}
					return treeNode;
				}
				else
				{
					if (num4 == 1)
					{
						if (curV < minV)
						{
							treeNode.ImageIndex = IMG_red;
							treeNode.SelectedImageIndex = IMG_red;
							de_status = false;
							return treeNode;
						}
						if (measure_tp == 1 || measure_tp == 3)
						{
							return treeNode;
						}
						if (curV >= minV && curV < 1.15 * minV)
						{
							treeNode.ImageIndex = IMG_orange;
							treeNode.SelectedImageIndex = IMG_orange;
							de_status = false;
						}
						return treeNode;
					}
					else
					{
						if (num4 == 2)
						{
							if (curV > maxV)
							{
								treeNode.ImageIndex = IMG_red;
								treeNode.SelectedImageIndex = IMG_red;
								de_status = false;
							}
							else
							{
								if (0.85 * maxV < curV && curV <= maxV)
								{
									treeNode.ImageIndex = IMG_orange;
									treeNode.SelectedImageIndex = IMG_orange;
									de_status = false;
								}
							}
							return treeNode;
						}
						return treeNode;
					}
				}
			}
		}
		private void FillData_Sensor(TreeNode denode, DataSet ds, string devid, DevModelConfig devcfg)
		{
			DataTable dataTable = ds.Tables[1].Clone();
			DataRow[] array = ds.Tables[1].Select("device_id = " + devid);
			for (int i = 0; i < array.Length; i++)
			{
				dataTable.ImportRow(array[i]);
			}
			int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(devcfg, "ss");
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				bool flag = true;
				double maxV = ecoConvert.f2d(dataTable.Rows[j]["max_humidity"]);
				double minV = ecoConvert.f2d(dataTable.Rows[j]["min_humidity"]);
				double maxV2 = ecoConvert.f2d(dataTable.Rows[j]["max_temperature"]);
				double minV2 = ecoConvert.f2d(dataTable.Rows[j]["min_temperature"]);
				double maxV3 = ecoConvert.f2d(dataTable.Rows[j]["max_press"]);
				double minV3 = ecoConvert.f2d(dataTable.Rows[j]["min_press"]);
				TreeNode treeNode = new TreeNode();
				treeNode.ImageIndex = 28;
				treeNode.SelectedImageIndex = 28;
				treeNode.Name = EcoLanguage.getMsg(LangRes.Title_Sensor, new string[0]);
				treeNode.ToolTipText = EcoLanguage.getMsg(LangRes.Title_Sensor, new string[0]) + " " + System.Convert.ToString(dataTable.Rows[j]["sensor_type"]);
				treeNode.Text = EcoLanguage.getMsg(LangRes.Title_Sensor, new string[0]) + " " + System.Convert.ToString(dataTable.Rows[j]["sensor_type"]);
				denode.Nodes.Add(treeNode);
				TreeNode treeNode2;
				if (EcoGlobalVar.TempUnit == 0)
				{
					treeNode2 = this.genTreeNode(uIthEdidflg, 5, EcoLanguage.getMsg(LangRes.Title_Temperature, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["temperature"]), minV2, maxV2, "F2", "°C", 20, 23, 22, 21, ref flag);
				}
				else
				{
					treeNode2 = this.genTreeNode(uIthEdidflg, 5, EcoLanguage.getMsg(LangRes.Title_Temperature, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["temperature"]), minV2, maxV2, "F2", "°F", 20, 23, 22, 21, ref flag);
				}
				if (treeNode2 != null)
				{
					treeNode.Nodes.Add(treeNode2);
				}
				TreeNode treeNode3 = this.genTreeNode(uIthEdidflg, 6, EcoLanguage.getMsg(LangRes.Title_Humidity, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["humidity"]), minV, maxV, "F2", "%", 9, 11, 10, 8, ref flag);
				if (treeNode3 != null)
				{
					treeNode.Nodes.Add(treeNode3);
				}
				TreeNode treeNode4 = this.genTreeNode(uIthEdidflg, 7, EcoLanguage.getMsg(LangRes.Title_Pressure, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["press_value"]), minV3, maxV3, "F2", "Pa", 17, 19, 18, 16, ref flag);
				if (treeNode4 != null)
				{
					treeNode.Nodes.Add(treeNode4);
				}
				if (!flag)
				{
					denode.Expand();
					treeNode.Expand();
				}
			}
		}
		private void FillData_Outlet(TreeNode denode, DataSet ds, string id, DevModelConfig devcfg)
		{
			if (devcfg.perportreading == 1)
			{
				return;
			}
			DataTable dataTable = ds.Tables[2].Clone();
			DataRow[] array = ds.Tables[2].Select("device_id= " + id);
			for (int i = 0; i < array.Length; i++)
			{
				dataTable.ImportRow(array[i]);
			}
			int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(devcfg, "port");
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				bool flag = true;
				double maxV = ecoConvert.f2d(dataTable.Rows[j]["max_power"]);
				double minV = ecoConvert.f2d(dataTable.Rows[j]["min_power"]);
				double maxV2 = ecoConvert.f2d(dataTable.Rows[j]["max_current"]);
				double minV2 = ecoConvert.f2d(dataTable.Rows[j]["min_current"]);
				double maxV3 = ecoConvert.f2d(dataTable.Rows[j]["max_voltage"]);
				double minV3 = ecoConvert.f2d(dataTable.Rows[j]["min_voltage"]);
				double maxV4 = ecoConvert.f2d(dataTable.Rows[j]["max_power_diss"]);
				double minV4 = ecoConvert.f2d(dataTable.Rows[j]["min_power_diss"]);
				TreeNode treeNode = new TreeNode();
				treeNode.Name = System.Convert.ToString(dataTable.Rows[j]["port_number"]);
				if (System.Convert.ToString(dataTable.Rows[j]["port_nm"]) != System.Convert.ToString(-1000))
				{
					treeNode.Text = System.Convert.ToString(dataTable.Rows[j]["port_nm"]);
				}
				else
				{
					treeNode.Text = "";
				}
				treeNode.Text = treeNode.Name + ":" + treeNode.Text;
				treeNode.ToolTipText = treeNode.Text;
				treeNode.ImageIndex = 29;
				treeNode.SelectedImageIndex = 29;
				string a = System.Convert.ToString(dataTable.Rows[j]["port_state"]);
				if (!(a != OutletStatus.ON.ToString()) || !(a != OutletStatus.NA.ToString()))
				{
					TreeNode treeNode2 = this.genTreeNode(uIthEdidflg, 4, EcoLanguage.getMsg(LangRes.Title_PowerDiss, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["power_consumption"]), minV4, maxV4, "F4", "kWh", 13, 15, 14, 12, ref flag);
					TreeNode treeNode3 = this.genTreeNode(uIthEdidflg, 3, EcoLanguage.getMsg(LangRes.Title_Power, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["power_value"]), minV, maxV, "F4", "W", 13, 15, 14, 12, ref flag);
					TreeNode treeNode4 = this.genTreeNode(uIthEdidflg, 2, EcoLanguage.getMsg(LangRes.Title_Voltage, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["voltage_value"]), minV3, maxV3, "F2", "V", 25, 27, 26, 24, ref flag);
					TreeNode treeNode5 = this.genTreeNode(uIthEdidflg, 1, EcoLanguage.getMsg(LangRes.Title_Current, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["current_value"]), minV2, maxV2, "F2", "A", 3, 5, 4, 2, ref flag);
					if (!flag)
					{
						if (treeNode2 != null)
						{
							treeNode.Nodes.Add(treeNode2);
						}
						if (treeNode3 != null)
						{
							treeNode.Nodes.Add(treeNode3);
						}
						if (treeNode4 != null)
						{
							treeNode.Nodes.Add(treeNode4);
						}
						if (treeNode5 != null)
						{
							treeNode.Nodes.Add(treeNode5);
						}
						denode.Nodes.Add(treeNode);
						denode.Expand();
						treeNode.Expand();
					}
				}
			}
		}
		private void FillData_Bank(TreeNode denode, DataSet ds, string id, DevModelConfig devcfg)
		{
			if (devcfg.perbankReading == 1)
			{
				return;
			}
			DataTable dataTable = ds.Tables[3].Clone();
			DataRow[] array = ds.Tables[3].Select(string.Concat(new string[]
			{
				"device_id= ",
				id,
				" and bank_state='",
				BankStatus.ON.ToString(),
				"'"
			}));
			for (int i = 0; i < array.Length; i++)
			{
				dataTable.ImportRow(array[i]);
			}
			int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(devcfg, "bank");
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				if (!this.m_frmMonitor && this.m_curBorad_Tag.Equals("0:1"))
				{
					int selNo = System.Convert.ToInt32(dataTable.Rows[j]["bank_number"]);
					double num = devcfgUtil.ampMax(devcfg, "bank", selNo);
					double num2 = ecoConvert.f2d(dataTable.Rows[j]["current_value"]);
					TreeNode treeNode = new TreeNode();
					treeNode.Name = System.Convert.ToString(dataTable.Rows[j]["bank_number"]);
					if (System.Convert.ToString(dataTable.Rows[j]["bank_nm"]) != System.Convert.ToString(-1000))
					{
						treeNode.Text = System.Convert.ToString(dataTable.Rows[j]["bank_nm"]);
					}
					else
					{
						treeNode.Text = "";
					}
					treeNode.Text = treeNode.Name + ":" + treeNode.Text;
					treeNode.ToolTipText = treeNode.Text;
					treeNode.ImageIndex = 30;
					treeNode.SelectedImageIndex = 30;
					TreeNode node;
					TreeNode node2;
					if (num == 0.0)
					{
						node = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AllAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Capacity, new string[0]) + ": N/A", 31);
						node2 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AvAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Available, new string[0]) + ": N/A", 33);
					}
					else
					{
						node = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AllAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Capacity, new string[0]) + ": ", num, "F2", "A", 31);
						node2 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AvAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Available, new string[0]) + ": ", num - num2, "F2", "A", 33);
					}
					treeNode.Nodes.Add(node);
					TreeNode node3 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AmpCapInuse, new string[0]), " " + EcoLanguage.getMsg(LangRes.Title_Inuse, new string[0]) + ": ", num2, "F2", "A", 32);
					treeNode.Nodes.Add(node3);
					treeNode.Nodes.Add(node2);
					denode.Nodes.Add(treeNode);
				}
				else
				{
					bool flag = true;
					double maxV = ecoConvert.f2d(dataTable.Rows[j]["max_power"]);
					double minV = ecoConvert.f2d(dataTable.Rows[j]["min_power"]);
					double maxV2 = ecoConvert.f2d(dataTable.Rows[j]["max_current"]);
					double minV2 = ecoConvert.f2d(dataTable.Rows[j]["min_current"]);
					double maxV3 = ecoConvert.f2d(dataTable.Rows[j]["max_voltage"]);
					double minV3 = ecoConvert.f2d(dataTable.Rows[j]["min_voltage"]);
					double maxV4 = ecoConvert.f2d(dataTable.Rows[j]["max_power_diss"]);
					double minV4 = ecoConvert.f2d(dataTable.Rows[j]["min_power_diss"]);
					TreeNode treeNode2 = new TreeNode();
					treeNode2.Name = System.Convert.ToString(dataTable.Rows[j]["bank_number"]);
					treeNode2.ToolTipText = System.Convert.ToString(dataTable.Rows[j]["bank_nm"]);
					if (System.Convert.ToString(dataTable.Rows[j]["bank_nm"]) != System.Convert.ToString(-1000))
					{
						treeNode2.Text = System.Convert.ToString(dataTable.Rows[j]["bank_nm"]);
					}
					else
					{
						treeNode2.Text = "";
					}
					treeNode2.Text = treeNode2.Name + ":" + treeNode2.Text;
					treeNode2.ToolTipText = treeNode2.Text;
					treeNode2.ImageIndex = 30;
					treeNode2.SelectedImageIndex = 30;
					TreeNode treeNode3 = this.genTreeNode(uIthEdidflg, 4, EcoLanguage.getMsg(LangRes.Title_PowerDiss, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["power_consumption"]), minV4, maxV4, "F4", "kWh", 13, 15, 14, 12, ref flag);
					TreeNode treeNode4 = this.genTreeNode(uIthEdidflg, 3, EcoLanguage.getMsg(LangRes.Title_Power, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["power_value"]), minV, maxV, "F4", "W", 13, 15, 14, 12, ref flag);
					TreeNode treeNode5 = this.genTreeNode(uIthEdidflg, 2, EcoLanguage.getMsg(LangRes.Title_Voltage, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["voltage_value"]), minV3, maxV3, "F2", "V", 25, 27, 26, 24, ref flag);
					TreeNode treeNode6 = this.genTreeNode(uIthEdidflg, 1, EcoLanguage.getMsg(LangRes.Title_Current, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["current_value"]), minV2, maxV2, "F2", "A", 3, 5, 4, 2, ref flag);
					if (!flag)
					{
						if (treeNode3 != null)
						{
							treeNode2.Nodes.Add(treeNode3);
						}
						if (treeNode4 != null)
						{
							treeNode2.Nodes.Add(treeNode4);
						}
						if (treeNode5 != null)
						{
							treeNode2.Nodes.Add(treeNode5);
						}
						if (treeNode6 != null)
						{
							treeNode2.Nodes.Add(treeNode6);
						}
						denode.Nodes.Add(treeNode2);
						denode.Expand();
						treeNode2.Expand();
					}
				}
			}
		}
		private void FillData_Line(TreeNode denode, DataSet ds, string id, DevModelConfig devcfg)
		{
			if (devcfg.perlineReading == Constant.NO)
			{
				return;
			}
			DataTable dataTable = ds.Tables[4].Clone();
			DataRow[] array = ds.Tables[4].Select("device_id= " + id);
			for (int i = 0; i < array.Length; i++)
			{
				dataTable.ImportRow(array[i]);
			}
			int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(devcfg, "line");
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				if (!this.m_frmMonitor && this.m_curBorad_Tag.Equals("0:1"))
				{
					int num = System.Convert.ToInt32(dataTable.Rows[j]["line_number"]);
					double num2 = devcfgUtil.ampMax(devcfg, "line", num);
					double num3 = ecoConvert.f2d(dataTable.Rows[j]["current_value"]);
					TreeNode treeNode = new TreeNode();
					treeNode.Name = "Line" + System.Convert.ToString(num);
					treeNode.Text = treeNode.Name;
					treeNode.ToolTipText = treeNode.Text;
					treeNode.ImageIndex = 34;
					treeNode.SelectedImageIndex = 34;
					TreeNode node;
					TreeNode node2;
					if (num2 == 0.0)
					{
						node = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AllAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Capacity, new string[0]) + ": N/A", 31);
						node2 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AvAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Available, new string[0]) + ": N/A", 33);
					}
					else
					{
						node = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AllAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Capacity, new string[0]) + ": ", num2, "F2", "A", 31);
						node2 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AvAmpCap, new string[0]), EcoLanguage.getMsg(LangRes.Title_Available, new string[0]) + ": ", num2 - num3, "F2", "A", 33);
					}
					treeNode.Nodes.Add(node);
					TreeNode node3 = this.genTreeNode(EcoLanguage.getMsg(LangRes.Title_AmpCapInuse, new string[0]), " " + EcoLanguage.getMsg(LangRes.Title_Inuse, new string[0]) + ": ", num3, "F2", "A", 32);
					treeNode.Nodes.Add(node3);
					treeNode.Nodes.Add(node2);
					denode.Nodes.Add(treeNode);
				}
				else
				{
					bool flag = true;
					double maxV = ecoConvert.f2d(dataTable.Rows[j]["max_power"]);
					double minV = ecoConvert.f2d(dataTable.Rows[j]["min_power"]);
					double maxV2 = ecoConvert.f2d(dataTable.Rows[j]["max_current"]);
					double minV2 = ecoConvert.f2d(dataTable.Rows[j]["min_current"]);
					double maxV3 = ecoConvert.f2d(dataTable.Rows[j]["max_voltage"]);
					double minV3 = ecoConvert.f2d(dataTable.Rows[j]["min_voltage"]);
					TreeNode treeNode2 = new TreeNode();
					treeNode2.Name = "Line" + System.Convert.ToString(dataTable.Rows[j]["line_number"]);
					treeNode2.ToolTipText = treeNode2.Name;
					treeNode2.Text = treeNode2.Name;
					treeNode2.ToolTipText = treeNode2.Text;
					treeNode2.ImageIndex = 34;
					treeNode2.SelectedImageIndex = 34;
					TreeNode treeNode3 = this.genTreeNode(uIthEdidflg, 3, EcoLanguage.getMsg(LangRes.Title_Power, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["power_value"]), minV, maxV, "F4", "W", 13, 15, 14, 12, ref flag);
					TreeNode treeNode4 = this.genTreeNode(uIthEdidflg, 2, EcoLanguage.getMsg(LangRes.Title_Voltage, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["voltage_value"]), minV3, maxV3, "F2", "V", 25, 27, 26, 24, ref flag);
					TreeNode treeNode5 = this.genTreeNode(uIthEdidflg, 1, EcoLanguage.getMsg(LangRes.Title_Current, new string[0]), ecoConvert.f2d(dataTable.Rows[j]["current_value"]), minV2, maxV2, "F2", "A", 3, 5, 4, 2, ref flag);
					if (!flag)
					{
						if (treeNode3 != null)
						{
							treeNode2.Nodes.Add(treeNode3);
						}
						if (treeNode4 != null)
						{
							treeNode2.Nodes.Add(treeNode4);
						}
						if (treeNode5 != null)
						{
							treeNode2.Nodes.Add(treeNode5);
						}
						denode.Nodes.Add(treeNode2);
						denode.Expand();
						treeNode2.Expand();
					}
				}
			}
		}
	}
}
