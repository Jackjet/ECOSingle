using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.DevManDCFloorGrid;
using EventLogAPI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class SysPara : UserControl
	{
		private string m_ElecUnit;
		private string m_CO2Unit;
		private IContainer components;
		private GroupBox groupBox1;
		private Label label51;
		private TextBox tbECVoltage;
		private Label lbReadDelay;
		private Label lbCO2_unit;
		private TextBox txtprice_co2;
		private Label label3;
		private Label lbElec_unit;
		private TextBox txtprice_elec;
		private Label label31;
		private Label label40;
		private TextBox txtco2_elec;
		private Label label30;
		private Button butSysparaSave;
		private RadioButton rbEC3;
		private RadioButton rbEC2;
		private RadioButton rbEC1;
		private Button btbrowsedev;
		private Label lbECDevNm;
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private GroupBox groupBox4;
		private RadioButton TempUnitF;
		private RadioButton TempUnitC;
		private ComboBox cbDClayout;
		private Label label1;
		private ComboBox cbCurrency;
		private Label label4;
		private CheckBox cbERackFNm;
		private ComboBox cbReadDelay;
		private CheckBox cbERPower;
		public SysPara()
		{
			this.InitializeComponent();
			this.tbECVoltage.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtco2_elec.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtprice_elec.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.txtprice_co2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.m_ElecUnit = this.lbElec_unit.Text;
			this.m_CO2Unit = this.lbCO2_unit.Text;
		}
		public void pageInit()
		{
			int serviceDelay = Sys_Para.GetServiceDelay();
			if (serviceDelay == 30)
			{
				this.cbReadDelay.SelectedIndex = 0;
			}
			else
			{
				if (serviceDelay == 60)
				{
					this.cbReadDelay.SelectedIndex = 1;
				}
				else
				{
					if (serviceDelay == 180)
					{
						this.cbReadDelay.SelectedIndex = 2;
					}
					else
					{
						if (serviceDelay == 300)
						{
							this.cbReadDelay.SelectedIndex = 3;
						}
						else
						{
							if (serviceDelay == 600)
							{
								this.cbReadDelay.SelectedIndex = 4;
							}
							else
							{
								if (serviceDelay == 900)
								{
									this.cbReadDelay.SelectedIndex = 5;
								}
								else
								{
									this.cbReadDelay.SelectedIndex = 1;
								}
							}
						}
					}
				}
			}
			this.cbDClayout.SelectedIndex = Sys_Para.GetResolution();
			EcoGlobalVar.TempUnit = Sys_Para.GetTemperatureUnit();
			EcoGlobalVar.CurCurrency = Sys_Para.GetCurrency();
			EcoGlobalVar.co2kg = Sys_Para.GetCO2KG();
			switch (Sys_Para.GetEnergyType())
			{
			case 0:
				this.rbEC1.Checked = true;
				this.tbECVoltage.Text = Sys_Para.GetEnergyValue().ToString("F0");
				this.lbECDevNm.Text = "N/A";
				this.lbECDevNm.Tag = -1;
				break;
			case 1:
			{
				this.rbEC2.Checked = true;
				int referenceDevice = Sys_Para.GetReferenceDevice();
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(referenceDevice);
				if (deviceByID == null)
				{
					this.lbECDevNm.Text = "N/A";
					this.lbECDevNm.Tag = -1;
				}
				else
				{
					this.lbECDevNm.Text = deviceByID.DeviceName + " (IP:" + deviceByID.DeviceIP + ")";
					this.lbECDevNm.Tag = deviceByID.DeviceID;
				}
				break;
			}
			case 2:
				this.rbEC3.Checked = true;
				this.lbECDevNm.Text = "N/A";
				this.lbECDevNm.Tag = -1;
				break;
			}
			float cO2KG = Sys_Para.GetCO2KG();
			if (cO2KG < 0f)
			{
				this.txtco2_elec.Text = "";
			}
			else
			{
				this.txtco2_elec.Text = cO2KG.ToString("F2");
			}
			float eLECTRICITYCOST = Sys_Para.GetELECTRICITYCOST();
			if (eLECTRICITYCOST < 0f)
			{
				this.txtprice_elec.Text = "";
			}
			else
			{
				this.txtprice_elec.Text = eLECTRICITYCOST.ToString("F2");
			}
			float cO2COST = Sys_Para.GetCO2COST();
			if (eLECTRICITYCOST < 0f)
			{
				this.txtprice_co2.Text = "";
			}
			else
			{
				this.txtprice_co2.Text = cO2COST.ToString("F2");
			}
			if (EcoGlobalVar.TempUnit == 0)
			{
				this.TempUnitC.Checked = true;
			}
			else
			{
				this.TempUnitF.Checked = true;
			}
			this.cbCurrency.SelectedItem = EcoGlobalVar.CurCurrency;
			this.lbElec_unit.Text = EcoGlobalVar.CurCurrency + this.m_ElecUnit;
			this.lbCO2_unit.Text = EcoGlobalVar.CurCurrency + this.m_CO2Unit;
			if (Sys_Para.GetRackFullNameflag() == 1)
			{
				this.cbERackFNm.Checked = true;
			}
			else
			{
				this.cbERackFNm.Checked = false;
			}
			this.cbERPower.Checked = Sys_Para.GetEnablePowerControlFlag();
		}
		private bool sysparaCheck()
		{
			bool flag = false;
			int selectedIndex = this.cbDClayout.SelectedIndex;
			if (selectedIndex < EcoGlobalVar.DCLayoutType)
			{
				int num = DevManFloorGrids.get_DCRows(selectedIndex);
				int num2 = DevManFloorGrids.get_DCColumns(selectedIndex);
				System.Collections.ArrayList allRack = RackInfo.getAllRack();
				foreach (RackInfo rackInfo in allRack)
				{
					if (rackInfo.StartPoint_X + 1 > num || rackInfo.EndPoint_X + 1 > num)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_OutRange, new string[]
						{
							rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag)
						}));
						bool result = false;
						return result;
					}
					if (rackInfo.StartPoint_Y + 1 > num2 || rackInfo.EndPoint_Y + 1 > num2)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_OutRange, new string[]
						{
							rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag)
						}));
						bool result = false;
						return result;
					}
				}
				System.Collections.ArrayList allZone = ZoneInfo.getAllZone();
				foreach (ZoneInfo zoneInfo in allZone)
				{
					if (zoneInfo.StartPointX + 1 > num || zoneInfo.EndPointX + 1 > num)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Zone_OutRange, new string[]
						{
							zoneInfo.ZoneName
						}));
						bool result = false;
						return result;
					}
					if (zoneInfo.StartPointY + 1 > num2 || zoneInfo.EndPointY + 1 > num2)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Zone_OutRange, new string[]
						{
							zoneInfo.ZoneName
						}));
						bool result = false;
						return result;
					}
				}
			}
			if (this.rbEC1.Checked)
			{
				Ecovalidate.checkTextIsNull(this.tbECVoltage, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.rbEC1.Text
					}));
					return false;
				}
				if (!Ecovalidate.Rangeint(this.tbECVoltage, 90, 260))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.rbEC1.Text,
						"90",
						"260"
					}));
					return false;
				}
			}
			if (this.rbEC2.Checked)
			{
				try
				{
					int l_id = System.Convert.ToInt32(this.lbECDevNm.Tag.ToString());
					if (DeviceOperation.getDeviceByID(l_id) == null)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
						{
							this.rbEC2.Text
						}));
						bool result = false;
						return result;
					}
				}
				catch (System.Exception)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.rbEC2.Text
					}));
					bool result = false;
					return result;
				}
			}
			if (this.txtco2_elec.Text.Length > 0 && !Ecovalidate.NumberFormat_double(this.txtco2_elec))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
				return false;
			}
			if (this.txtprice_elec.Text.Length > 0 && !Ecovalidate.NumberFormat_double(this.txtprice_elec))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
				return false;
			}
			if (this.txtprice_co2.Text.Length > 0 && !Ecovalidate.NumberFormat_double(this.txtprice_co2))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
				return false;
			}
			return true;
		}
		private void butSysparaSave_Click(object sender, System.EventArgs e)
		{
			if (!this.sysparaCheck())
			{
				return;
			}
			int selectedIndex = this.cbReadDelay.SelectedIndex;
			int serviceDelay = 60;
			if (selectedIndex == 0)
			{
				serviceDelay = 30;
			}
			else
			{
				if (selectedIndex == 1)
				{
					serviceDelay = 60;
				}
				else
				{
					if (selectedIndex == 2)
					{
						serviceDelay = 180;
					}
					else
					{
						if (selectedIndex == 3)
						{
							serviceDelay = 300;
						}
						else
						{
							if (selectedIndex == 4)
							{
								serviceDelay = 600;
							}
							else
							{
								if (selectedIndex == 5)
								{
									serviceDelay = 900;
								}
							}
						}
					}
				}
			}
			float cO2KG = -5f;
			float cO2KG2 = Sys_Para.GetCO2KG();
			int temperatureUnit = Sys_Para.GetTemperatureUnit();
			string currency = Sys_Para.GetCurrency();
			int rackFullNameflag = Sys_Para.GetRackFullNameflag();
			bool enablePowerControlFlag = Sys_Para.GetEnablePowerControlFlag();
			if (this.txtco2_elec.Text.Length > 0)
			{
				cO2KG = System.Convert.ToSingle(this.txtco2_elec.Text);
			}
			float eLECTRICITYCOST = -5f;
			if (this.txtprice_elec.Text.Length > 0)
			{
				eLECTRICITYCOST = System.Convert.ToSingle(this.txtprice_elec.Text);
			}
			float cO2COST = -5f;
			if (this.txtprice_co2.Text.Length > 0)
			{
				cO2COST = System.Convert.ToSingle(this.txtprice_co2.Text);
			}
			if (Sys_Para.SetServiceDelay(serviceDelay) < 0 || Sys_Para.SetCO2KG(cO2KG) < 0 || Sys_Para.SetELECTRICITYCOST(eLECTRICITYCOST) < 0 || Sys_Para.SetCO2COST(cO2COST) < 0)
			{
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				EcoGlobalVar.co2kg = Sys_Para.GetCO2KG();
				return;
			}
			bool flag = false;
			ulong num = 0uL;
			int num2 = 0;
			EcoGlobalVar.co2kg = Sys_Para.GetCO2KG();
			if (cO2KG2 != EcoGlobalVar.co2kg)
			{
				flag = true;
			}
			if (this.rbEC1.Checked)
			{
				int energyType = 0;
				float energyValue = System.Convert.ToSingle(this.tbECVoltage.Text);
				if (Sys_Para.SetEnergyType(energyType) < 0 || Sys_Para.SetEnergyValue(energyValue) < 0)
				{
					EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
					return;
				}
			}
			else
			{
				if (this.rbEC2.Checked)
				{
					int energyType = 1;
					int referenceDevice = System.Convert.ToInt32(this.lbECDevNm.Tag.ToString());
					if (Sys_Para.SetEnergyType(energyType) < 0 || Sys_Para.SetReferenceDevice(referenceDevice) < 0)
					{
						EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
						return;
					}
				}
				else
				{
					if (this.rbEC3.Checked)
					{
						int energyType = 2;
						if (Sys_Para.SetEnergyType(energyType) < 0)
						{
							EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
							return;
						}
					}
				}
			}
			if (this.TempUnitC.Checked)
			{
				EcoGlobalVar.TempUnit = 0;
			}
			else
			{
				EcoGlobalVar.TempUnit = 1;
			}
			Sys_Para.SetTemperatureUnit(EcoGlobalVar.TempUnit);
			if (temperatureUnit != EcoGlobalVar.TempUnit)
			{
				flag = true;
			}
			EcoGlobalVar.CurCurrency = this.cbCurrency.SelectedItem.ToString();
			Sys_Para.SetCurrency(EcoGlobalVar.CurCurrency);
			if (!currency.Equals(EcoGlobalVar.CurCurrency))
			{
				flag = true;
			}
			int selectedIndex2 = this.cbDClayout.SelectedIndex;
			if (selectedIndex2 != EcoGlobalVar.DCLayoutType)
			{
				Sys_Para.SetResolution(selectedIndex2);
				EcoGlobalVar.DCLayoutType = selectedIndex2;
				num |= 4uL;
				num2 |= 1;
			}
			if (this.cbERackFNm.Checked)
			{
				Sys_Para.SetRackFullNameflag(1);
				if (rackFullNameflag != 1)
				{
					flag = true;
				}
			}
			else
			{
				Sys_Para.SetRackFullNameflag(0);
				if (rackFullNameflag != 0)
				{
					flag = true;
				}
			}
			Sys_Para.SetEnablePowerControlFlag(this.cbERPower.Checked);
			if (enablePowerControlFlag != this.cbERPower.Checked)
			{
				flag = true;
			}
			if (flag)
			{
				num |= 32uL;
				num2 |= 32;
			}
			EcoGlobalVar.setDashBoardFlg(num, "", num2);
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0130023", new string[]
				{
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0130023", new string[0]);
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private void int_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar >= '0' && keyChar <= '9')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void double_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(tb, e.KeyChar, 2);
			if (flag)
			{
				return;
			}
			e.Handled = true;
		}
		private void rbEC_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rbEC1.Checked)
			{
				this.tbECVoltage.Enabled = true;
				this.btbrowsedev.Enabled = false;
				return;
			}
			if (this.rbEC2.Checked)
			{
				this.tbECVoltage.Enabled = false;
				this.btbrowsedev.Enabled = true;
				return;
			}
			this.tbECVoltage.Enabled = false;
			this.btbrowsedev.Enabled = false;
		}
		private void btbrowsedev_Click(object sender, System.EventArgs e)
		{
			int devID = System.Convert.ToInt32(this.lbECDevNm.Tag);
			FindDevDlg findDevDlg = new FindDevDlg(devID);
			DialogResult dialogResult = findDevDlg.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				int devID2 = findDevDlg.DevID;
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID2);
				this.lbECDevNm.Text = deviceByID.DeviceName + " (IP:" + deviceByID.DeviceIP + ")";
				this.lbECDevNm.Tag = devID2;
			}
		}
		private void cbCurrency_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string str = this.cbCurrency.SelectedItem.ToString();
			this.lbElec_unit.Text = str + this.m_ElecUnit;
			this.lbCO2_unit.Text = str + this.m_CO2Unit;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysPara));
			this.groupBox1 = new GroupBox();
			this.cbReadDelay = new ComboBox();
			this.cbDClayout = new ComboBox();
			this.label1 = new Label();
			this.groupBox3 = new GroupBox();
			this.cbERackFNm = new CheckBox();
			this.cbCurrency = new ComboBox();
			this.label4 = new Label();
			this.groupBox4 = new GroupBox();
			this.TempUnitF = new RadioButton();
			this.TempUnitC = new RadioButton();
			this.label30 = new Label();
			this.txtco2_elec = new TextBox();
			this.lbCO2_unit = new Label();
			this.label40 = new Label();
			this.txtprice_co2 = new TextBox();
			this.label31 = new Label();
			this.label3 = new Label();
			this.txtprice_elec = new TextBox();
			this.lbElec_unit = new Label();
			this.groupBox2 = new GroupBox();
			this.rbEC2 = new RadioButton();
			this.btbrowsedev = new Button();
			this.tbECVoltage = new TextBox();
			this.lbECDevNm = new Label();
			this.label51 = new Label();
			this.rbEC3 = new RadioButton();
			this.rbEC1 = new RadioButton();
			this.lbReadDelay = new Label();
			this.butSysparaSave = new Button();
			this.cbERPower = new CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.cbReadDelay);
			this.groupBox1.Controls.Add(this.cbDClayout);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.lbReadDelay);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.cbReadDelay.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbReadDelay, "cbReadDelay");
			this.cbReadDelay.FormattingEnabled = true;
			this.cbReadDelay.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbReadDelay.Items"),
				componentResourceManager.GetString("cbReadDelay.Items1"),
				componentResourceManager.GetString("cbReadDelay.Items2"),
				componentResourceManager.GetString("cbReadDelay.Items3"),
				componentResourceManager.GetString("cbReadDelay.Items4"),
				componentResourceManager.GetString("cbReadDelay.Items5")
			});
			this.cbReadDelay.Name = "cbReadDelay";
			this.cbDClayout.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbDClayout, "cbDClayout");
			this.cbDClayout.FormattingEnabled = true;
			this.cbDClayout.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbDClayout.Items"),
				componentResourceManager.GetString("cbDClayout.Items1"),
				componentResourceManager.GetString("cbDClayout.Items2")
			});
			this.cbDClayout.Name = "cbDClayout";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Black;
			this.label1.Name = "label1";
			this.groupBox3.Controls.Add(this.cbERPower);
			this.groupBox3.Controls.Add(this.cbERackFNm);
			this.groupBox3.Controls.Add(this.cbCurrency);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.groupBox4);
			this.groupBox3.Controls.Add(this.label30);
			this.groupBox3.Controls.Add(this.txtco2_elec);
			this.groupBox3.Controls.Add(this.lbCO2_unit);
			this.groupBox3.Controls.Add(this.label40);
			this.groupBox3.Controls.Add(this.txtprice_co2);
			this.groupBox3.Controls.Add(this.label31);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.txtprice_elec);
			this.groupBox3.Controls.Add(this.lbElec_unit);
			this.groupBox3.ForeColor = Color.FromArgb(20, 73, 160);
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			componentResourceManager.ApplyResources(this.cbERackFNm, "cbERackFNm");
			this.cbERackFNm.ForeColor = Color.Black;
			this.cbERackFNm.Name = "cbERackFNm";
			this.cbERackFNm.UseVisualStyleBackColor = true;
			this.cbCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cbCurrency, "cbCurrency");
			this.cbCurrency.FormattingEnabled = true;
			this.cbCurrency.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cbCurrency.Items"),
				componentResourceManager.GetString("cbCurrency.Items1"),
				componentResourceManager.GetString("cbCurrency.Items2"),
				componentResourceManager.GetString("cbCurrency.Items3"),
				componentResourceManager.GetString("cbCurrency.Items4")
			});
			this.cbCurrency.Name = "cbCurrency";
			this.cbCurrency.SelectedIndexChanged += new System.EventHandler(this.cbCurrency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = Color.Black;
			this.label4.Name = "label4";
			this.groupBox4.Controls.Add(this.TempUnitF);
			this.groupBox4.Controls.Add(this.TempUnitC);
			this.groupBox4.ForeColor = Color.FromArgb(20, 73, 160);
			componentResourceManager.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			componentResourceManager.ApplyResources(this.TempUnitF, "TempUnitF");
			this.TempUnitF.ForeColor = Color.Black;
			this.TempUnitF.Name = "TempUnitF";
			this.TempUnitF.TabStop = true;
			this.TempUnitF.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.TempUnitC, "TempUnitC");
			this.TempUnitC.ForeColor = Color.Black;
			this.TempUnitC.Name = "TempUnitC";
			this.TempUnitC.TabStop = true;
			this.TempUnitC.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label30, "label30");
			this.label30.ForeColor = Color.Black;
			this.label30.Name = "label30";
			componentResourceManager.ApplyResources(this.txtco2_elec, "txtco2_elec");
			this.txtco2_elec.ForeColor = Color.Black;
			this.txtco2_elec.Name = "txtco2_elec";
			this.txtco2_elec.KeyPress += new KeyPressEventHandler(this.double_KeyPress);
			componentResourceManager.ApplyResources(this.lbCO2_unit, "lbCO2_unit");
			this.lbCO2_unit.ForeColor = Color.Black;
			this.lbCO2_unit.Name = "lbCO2_unit";
			componentResourceManager.ApplyResources(this.label40, "label40");
			this.label40.ForeColor = Color.Black;
			this.label40.Name = "label40";
			componentResourceManager.ApplyResources(this.txtprice_co2, "txtprice_co2");
			this.txtprice_co2.ForeColor = Color.Black;
			this.txtprice_co2.Name = "txtprice_co2";
			this.txtprice_co2.KeyPress += new KeyPressEventHandler(this.double_KeyPress);
			componentResourceManager.ApplyResources(this.label31, "label31");
			this.label31.ForeColor = Color.Black;
			this.label31.Name = "label31";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = Color.Black;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.txtprice_elec, "txtprice_elec");
			this.txtprice_elec.ForeColor = Color.Black;
			this.txtprice_elec.Name = "txtprice_elec";
			this.txtprice_elec.KeyPress += new KeyPressEventHandler(this.double_KeyPress);
			componentResourceManager.ApplyResources(this.lbElec_unit, "lbElec_unit");
			this.lbElec_unit.ForeColor = Color.Black;
			this.lbElec_unit.Name = "lbElec_unit";
			this.groupBox2.Controls.Add(this.rbEC2);
			this.groupBox2.Controls.Add(this.btbrowsedev);
			this.groupBox2.Controls.Add(this.tbECVoltage);
			this.groupBox2.Controls.Add(this.lbECDevNm);
			this.groupBox2.Controls.Add(this.label51);
			this.groupBox2.Controls.Add(this.rbEC3);
			this.groupBox2.Controls.Add(this.rbEC1);
			this.groupBox2.ForeColor = Color.FromArgb(20, 73, 160);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.rbEC2, "rbEC2");
			this.rbEC2.ForeColor = Color.Black;
			this.rbEC2.Name = "rbEC2";
			this.rbEC2.TabStop = true;
			this.rbEC2.UseVisualStyleBackColor = true;
			this.rbEC2.CheckedChanged += new System.EventHandler(this.rbEC_CheckedChanged);
			this.btbrowsedev.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btbrowsedev, "btbrowsedev");
			this.btbrowsedev.ForeColor = SystemColors.ControlText;
			this.btbrowsedev.Name = "btbrowsedev";
			this.btbrowsedev.UseVisualStyleBackColor = false;
			this.btbrowsedev.Click += new System.EventHandler(this.btbrowsedev_Click);
			componentResourceManager.ApplyResources(this.tbECVoltage, "tbECVoltage");
			this.tbECVoltage.ForeColor = Color.Black;
			this.tbECVoltage.Name = "tbECVoltage";
			this.tbECVoltage.KeyPress += new KeyPressEventHandler(this.int_KeyPress);
			this.lbECDevNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.lbECDevNm, "lbECDevNm");
			this.lbECDevNm.ForeColor = Color.Black;
			this.lbECDevNm.Name = "lbECDevNm";
			componentResourceManager.ApplyResources(this.label51, "label51");
			this.label51.ForeColor = Color.Black;
			this.label51.Name = "label51";
			componentResourceManager.ApplyResources(this.rbEC3, "rbEC3");
			this.rbEC3.ForeColor = Color.Black;
			this.rbEC3.Name = "rbEC3";
			this.rbEC3.TabStop = true;
			this.rbEC3.UseVisualStyleBackColor = true;
			this.rbEC3.CheckedChanged += new System.EventHandler(this.rbEC_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbEC1, "rbEC1");
			this.rbEC1.ForeColor = Color.Black;
			this.rbEC1.Name = "rbEC1";
			this.rbEC1.TabStop = true;
			this.rbEC1.UseVisualStyleBackColor = true;
			this.rbEC1.CheckedChanged += new System.EventHandler(this.rbEC_CheckedChanged);
			componentResourceManager.ApplyResources(this.lbReadDelay, "lbReadDelay");
			this.lbReadDelay.ForeColor = Color.Black;
			this.lbReadDelay.Name = "lbReadDelay";
			this.butSysparaSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSysparaSave, "butSysparaSave");
			this.butSysparaSave.ForeColor = SystemColors.ControlText;
			this.butSysparaSave.Name = "butSysparaSave";
			this.butSysparaSave.UseVisualStyleBackColor = false;
			this.butSysparaSave.Click += new System.EventHandler(this.butSysparaSave_Click);
			componentResourceManager.ApplyResources(this.cbERPower, "cbERPower");
			this.cbERPower.ForeColor = Color.Black;
			this.cbERPower.Name = "cbERPower";
			this.cbERPower.UseVisualStyleBackColor = true;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.butSysparaSave);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysPara";
			this.groupBox1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
