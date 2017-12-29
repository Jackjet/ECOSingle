using CommonAPI.Global;
using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.DataGPOP
{
	public class DataGpOPDev : UserControl
	{
		private DataGpOPAll m_parent;
		private int m_devID;
		private System.Collections.Generic.Dictionary<string, int> m_PortIDs_Map = new System.Collections.Generic.Dictionary<string, int>();
		private string m_ModelNm;
		private string m_FWVer;
		private string m_devName;
		private int m_orgOffsetOutetlab;
		private int m_orgOffsetOutletTB;
		private int m_outletTableWidth;
		private int m_orgOffsetBanklab;
		private int m_orgOffsetBankTB;
		private string mStr_LeakStatus;
		private int m_orgOffsetLinelab_X;
		private IContainer components;
		private DataGridView dgvBankInfo;
		private DataGridViewTextBoxColumn bank_id;
		private DataGridViewTextBoxColumn bank_number;
		private DataGridViewTextBoxColumn bank_nm;
		private DataGridViewDisableButtonColumn bank_acton;
		private DataGridViewDisableButtonColumn bank_actoff;
		private DataGridViewDisableCheckBoxColumn bank_reboot;
		private DataGridViewTextBoxColumn bankvoltage;
		private DataGridViewTextBoxColumn bankcurrent;
		private DataGridViewTextBoxColumn bankpower;
		private DataGridViewTextBoxColumn bankpowerD;
		private Label labIp;
		private Label labBankStatus;
		private DataGridViewTextBoxColumn portPowerDiss;
		private DataGridViewDisableCheckBoxColumn reboot;
		private DataGridViewTextBoxColumn portPower;
		private DataGridViewTextBoxColumn portVoltage;
		private DataGridViewTextBoxColumn portCurrent;
		private DataGridViewDisableButtonColumn act;
		private DataGridViewTextBoxColumn portStatus;
		private CheckBox cbDReboot;
		private Button butOff;
		private Button butOn;
		private Label labDevStatus;
		private Label labSensorStatus;
		private Label labOutletStatus;
		private DataGridView dgvDeviceSensorInfo;
		private DataGridViewTextBoxColumn port_id;
		private DataGridView dgvDeviceInfo;
		private DataGridViewTextBoxColumn deviceNm;
		private DataGridViewTextBoxColumn dVoltage;
		private DataGridViewTextBoxColumn dCurrent;
		private DataGridViewTextBoxColumn dPower;
		private DataGridViewTextBoxColumn dPowerDiss;
		private DataGridViewTextBoxColumn port_number;
		private DataGridView dgvOutletInfo;
		private DataGridViewTextBoxColumn port_nm;
		private DataGridViewTextBoxColumn sensor_type;
		private DataGridViewTextBoxColumn humiditys;
		private DataGridViewTextBoxColumn temperatures;
		private DataGridViewTextBoxColumn press_values;
		private Label lbLeakStatus;
		private Label lbLineStatus;
		private DataGridView dgvLineInfo;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
		private DataGridViewDisableCheckBoxColumn dataGridViewDisableCheckBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
		private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn2;
		private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn3;
		private DataGridViewDisableCheckBoxColumn dataGridViewDisableCheckBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public DataGpOPDev()
		{
			this.InitializeComponent();
			this.mStr_LeakStatus = this.lbLeakStatus.Text;
			this.m_orgOffsetLinelab_X = this.lbLineStatus.Location.X - this.labSensorStatus.Location.X;
			this.m_orgOffsetOutetlab = this.labOutletStatus.Location.Y - this.labSensorStatus.Location.Y;
			this.m_orgOffsetOutletTB = this.dgvOutletInfo.Location.Y - this.labOutletStatus.Location.Y;
			this.m_orgOffsetBanklab = this.labBankStatus.Location.Y - this.labOutletStatus.Location.Y;
			this.m_orgOffsetBankTB = this.dgvBankInfo.Location.Y - this.labBankStatus.Location.Y;
		}
		public void pageInit(string IDs_DevPorts, string devName, DataGpOPAll parent)
		{
			this.m_parent = parent;
			int num = IDs_DevPorts.IndexOf('@');
			string text = IDs_DevPorts.Substring(0, num);
			this.m_devID = System.Convert.ToInt32(text);
			text = IDs_DevPorts.Substring(num + 1);
			string[] array = text.Split(new string[]
			{
				","
			}, System.StringSplitOptions.RemoveEmptyEntries);
			this.m_PortIDs_Map = new System.Collections.Generic.Dictionary<string, int>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string key = array2[i];
				this.m_PortIDs_Map.Add(key, 0);
			}
			this.m_devName = devName;
			this.m_ModelNm = "";
			this.m_FWVer = "";
			if (this.m_devID < 0)
			{
				this.labIp.Text = "";
				if (this.dgvDeviceInfo.DataSource != null)
				{
					DataTable dataTable = (DataTable)this.dgvDeviceInfo.DataSource;
					dataTable.Clear();
				}
				else
				{
					this.dgvDeviceInfo.Rows.Clear();
				}
				this.butOn.Hide();
				this.butOff.Hide();
				this.cbDReboot.Hide();
				if (this.dgvDeviceSensorInfo.DataSource != null)
				{
					DataTable dataTable2 = (DataTable)this.dgvDeviceSensorInfo.DataSource;
					dataTable2.Clear();
				}
				else
				{
					this.dgvDeviceSensorInfo.Rows.Clear();
				}
				this.lbLineStatus.Hide();
				this.lbLeakStatus.Hide();
				this.dgvLineInfo.Hide();
				this.labOutletStatus.Hide();
				this.dgvOutletInfo.Hide();
				this.labBankStatus.Hide();
				this.dgvBankInfo.Hide();
				return;
			}
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			if (!devicRackZoneRelation.ContainsKey(this.m_devID))
			{
				this.except_devnotExist(this.m_devName);
				return;
			}
			ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[this.m_devID];
			DataRow[] dataRows = ClientAPI.getDataRows(0, "device_id=" + this.m_devID, "");
			this.labIp.Text = (string)dataRows[0]["device_ip"];
			this.m_ModelNm = deviceWithZoneRackInfo.device_model;
			this.m_FWVer = deviceWithZoneRackInfo.fw_version;
			this.pageControlInit();
			DataRow[] dataRows2 = ClientAPI.getDataRows(0, "device_id=" + this.m_devID + " and device_state=1", "");
			int deviceData = this.getDeviceData(this.m_devID.ToString(), dataRows2);
			this.getOutletData(this.m_devID.ToString(), true, deviceData);
			this.getDeviceSensorData(this.m_devID.ToString(), deviceData);
			this.getBankData(this.m_devID.ToString(), true, deviceData);
			this.getLineData(this.m_devID.ToString(), true, deviceData);
		}
		private void except_devnotExist(string devName)
		{
			this.m_parent.endTimer();
			if (devName != null && devName.Length > 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DevInfo_nofind, new string[]
				{
					devName
				}));
			}
			DataGpOPDev.PostMessage(this.m_parent.Handle, 63000u, 0, "");
		}
		private int getDeviceData(string deviceId, DataRow[] drArr)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("device_nm", typeof(string));
			dataTable.Columns.Add("voltage_values", typeof(string));
			dataTable.Columns.Add("current_values", typeof(string));
			dataTable.Columns.Add("power_values", typeof(string));
			dataTable.Columns.Add("power_consumptions", typeof(string));
			for (int i = 0; i < drArr.Length; i++)
			{
				DataRow dataRow = drArr[i];
				DataRow dataRow2 = dataTable.NewRow();
				dataRow2["device_nm"] = dataRow["device_nm"];
				if (EcoGlobalVar.gl_LoginUser.UserType == 1 && this.m_PortIDs_Map.Count > 0)
				{
					this.dataTransPdu(dataRow, dataRow2, true);
				}
				else
				{
					this.dataTransPdu(dataRow, dataRow2, false);
				}
				dataTable.Rows.Add(dataRow2);
			}
			this.dgvDeviceInfo.DataSource = dataTable;
			if (dataTable.Rows.Count > 0)
			{
				if (DevAccessCfg.GetInstance().getDeviceModelConfig(this.m_ModelNm, this.m_FWVer).switchableOutlets == 0uL)
				{
					this.butOn.Hide();
					this.butOff.Hide();
					this.cbDReboot.Hide();
				}
				else
				{
					this.butOn.Show();
					this.butOff.Show();
					this.cbDReboot.Show();
					this.butOn.Enabled = EcoGlobalVar.flgEnablePower;
					this.butOff.Enabled = EcoGlobalVar.flgEnablePower;
					this.cbDReboot.Enabled = EcoGlobalVar.flgEnablePower;
				}
				int num = -1;
				if (drArr.Length > 0)
				{
					num = (int)drArr[0]["leakcurrent_status"];
				}
				if (this.lbLeakStatus.Visible)
				{
					string text = this.mStr_LeakStatus;
					if (num == 1)
					{
						text += " NO";
					}
					else
					{
						if (num == 2)
						{
							text += " Yes";
						}
						else
						{
							text += " N/A";
						}
					}
					this.lbLeakStatus.Text = text;
				}
				return 1;
			}
			return 0;
		}
		private void dataTransPdu(DataRow srcdr, DataRow tardr, bool hide4Remote = false)
		{
			if (srcdr["voltage_value"].ToString().Equals(System.Convert.ToString(-500)))
			{
				tardr["voltage_values"] = "";
			}
			else
			{
				if (srcdr["voltage_value"].ToString().Equals(System.Convert.ToString(-1000)))
				{
					tardr["voltage_values"] = "N/A";
				}
				else
				{
					if (hide4Remote)
					{
						tardr["voltage_values"] = "N/A";
					}
					else
					{
						tardr["voltage_values"] = srcdr["voltage_value"];
					}
				}
			}
			if (srcdr["current_value"].ToString().Equals(System.Convert.ToString(-500)))
			{
				tardr["current_values"] = "";
			}
			else
			{
				if (srcdr["current_value"].ToString().Equals(System.Convert.ToString(-1000)))
				{
					tardr["current_values"] = "N/A";
				}
				else
				{
					if (hide4Remote)
					{
						tardr["current_values"] = "N/A";
					}
					else
					{
						tardr["current_values"] = srcdr["current_value"];
					}
				}
			}
			if (srcdr["power_value"].ToString().Equals(System.Convert.ToString(-500)))
			{
				tardr["power_values"] = "";
			}
			else
			{
				if (srcdr["power_value"].ToString().Equals(System.Convert.ToString(-1000)))
				{
					tardr["power_values"] = "N/A";
				}
				else
				{
					if (hide4Remote)
					{
						tardr["power_values"] = "N/A";
					}
					else
					{
						tardr["power_values"] = srcdr["power_value"];
					}
				}
			}
			if (tardr.Table.Columns.Contains("power_consumptions"))
			{
				if (srcdr["power_consumption"].ToString().Equals(System.Convert.ToString(-500)))
				{
					tardr["power_consumptions"] = "";
					return;
				}
				if (srcdr["power_consumption"].ToString().Equals(System.Convert.ToString(-1000)))
				{
					tardr["power_consumptions"] = "N/A";
					return;
				}
				if (hide4Remote)
				{
					tardr["power_consumptions"] = "N/A";
					return;
				}
				tardr["power_consumptions"] = srcdr["power_consumption"];
			}
		}
		private void getOutletData(string deviceId, bool initFlg, int devonline)
		{
			if (!this.dgvOutletInfo.Visible)
			{
				return;
			}
			DataRow[] dataRows = ClientAPI.getDataRows(2, "device_id=" + this.m_devID, "port_number ASC");
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("port_id", typeof(int));
			dataTable.Columns.Add("port_number", typeof(int));
			dataTable.Columns.Add("port_nm", typeof(string));
			dataTable.Columns.Add("port_state", typeof(string));
			dataTable.Columns.Add("act", typeof(string));
			dataTable.Columns.Add("reboot", typeof(string));
			dataTable.Columns.Add("voltage_values", typeof(string));
			dataTable.Columns.Add("current_values", typeof(string));
			dataTable.Columns.Add("power_values", typeof(string));
			dataTable.Columns.Add("power_consumptions", typeof(string));
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(this.m_ModelNm, this.m_FWVer);
			int num = -1;
			if (devonline == 1)
			{
				DataRow[] array = dataRows;
				for (int i = 0; i < array.Length; i++)
				{
					DataRow dataRow = array[i];
					string key = dataRow["port_id"].ToString();
					if (EcoGlobalVar.gl_LoginUser.UserType != 1 || this.m_PortIDs_Map.Count <= 0 || this.m_PortIDs_Map.ContainsKey(key))
					{
						DataRow dataRow2 = dataTable.NewRow();
						dataRow2["port_id"] = dataRow["port_id"];
						dataRow2["port_number"] = dataRow["port_number"];
						dataRow2["port_nm"] = dataRow["port_nm"];
						dataRow2["port_state"] = dataRow["port_state"];
						this.dataTransPdu(dataRow, dataRow2, false);
						if (dataRow["port_nm"].ToString().Equals(System.Convert.ToString(-500)) || dataRow["port_nm"].ToString().Equals(System.Convert.ToString(-1000)))
						{
							dataRow2["port_nm"] = "";
						}
						int num2 = System.Convert.ToInt32(dataRow["port_number"].ToString());
						key = dataRow["port_id"].ToString();
						if (this.m_PortIDs_Map.ContainsKey(key))
						{
							this.m_PortIDs_Map[key] = num2;
						}
						if (deviceModelConfig.isOutletSwitchable(num2 - 1))
						{
							string text = dataRow["port_state"].ToString();
							if (text.Equals(OutletStatus.OFF.ToString()))
							{
								dataRow2["port_state"] = EcoLanguage.getMsg(LangRes.OPST_OFF, new string[0]);
								dataRow2["act"] = EcoLanguage.getMsg(LangRes.OPACT_ON, new string[0]);
							}
							else
							{
								if (text.Equals(OutletStatus.ON.ToString()))
								{
									dataRow2["port_state"] = EcoLanguage.getMsg(LangRes.OPST_ON, new string[0]);
									dataRow2["act"] = EcoLanguage.getMsg(LangRes.OPACT_OFF, new string[0]);
								}
								else
								{
									if (text.Equals(OutletStatus.Pending.ToString()))
									{
										dataRow2["port_state"] = EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0]);
									}
									else
									{
										if (text.Equals(OutletStatus.Fault.ToString()))
										{
											dataRow2["port_state"] = EcoLanguage.getMsg(LangRes.OPST_FAULT, new string[0]);
										}
									}
								}
							}
						}
						else
						{
							dataRow2["port_state"] = EcoLanguage.getMsg(LangRes.OPST_NA, new string[0]);
							dataRow2["act"] = EcoLanguage.getMsg(LangRes.OPACT_NA, new string[0]);
						}
						if (this.m_PortIDs_Map.Count != 0 && !this.m_PortIDs_Map.ContainsKey(key))
						{
							dataRow2["act"] = EcoLanguage.getMsg(LangRes.OPACT_NA, new string[0]);
						}
						string text2 = dataRow2["port_nm"].ToString();
						if (text2.Equals("\0"))
						{
							dataRow2["port_nm"] = string.Empty;
						}
						dataRow2["reboot"] = "0";
						dataTable.Rows.Add(dataRow2);
					}
				}
				if (!initFlg)
				{
					for (int j = 0; j < this.dgvOutletInfo.Rows.Count; j++)
					{
						DataGridViewRow dataGridViewRow = this.dgvOutletInfo.Rows[j];
						if (!dataGridViewRow.Cells["reboot"].Visible)
						{
							break;
						}
						dataTable.Rows[j]["reboot"] = dataGridViewRow.Cells["reboot"].Value.ToString();
					}
					DataGridViewCell currentCell = this.dgvOutletInfo.CurrentCell;
					if (currentCell != null)
					{
						num = currentCell.RowIndex;
					}
				}
				if (dataTable.Rows.Count > 8)
				{
					this.dgvOutletInfo.Width = this.m_outletTableWidth + SystemInformation.VerticalScrollBarWidth;
				}
				else
				{
					this.dgvOutletInfo.Width = this.m_outletTableWidth;
				}
			}
			this.dgvOutletInfo.DataSource = dataTable;
			System.Collections.Generic.List<PortInfo> list = null;
			foreach (DataGridViewRow dataGridViewRow2 in (System.Collections.IEnumerable)this.dgvOutletInfo.Rows)
			{
				if (!dataGridViewRow2.Cells["act"].Visible)
				{
					break;
				}
				DataGridViewDisableButtonCell dataGridViewDisableButtonCell = (DataGridViewDisableButtonCell)dataGridViewRow2.Cells["act"];
				if (dataGridViewRow2.Cells["portStatus"].Value.ToString().Equals(EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0])) || dataGridViewRow2.Cells["portStatus"].Value.ToString().Equals(EcoLanguage.getMsg(LangRes.OPST_NA, new string[0])) || dataGridViewRow2.Cells["portStatus"].Value.ToString().Equals(EcoLanguage.getMsg(LangRes.OPST_FAULT, new string[0])) || dataGridViewRow2.Cells["act"].Value.ToString().Equals(EcoLanguage.getMsg(LangRes.OPACT_NA, new string[0])) || !EcoGlobalVar.flgEnablePower)
				{
					dataGridViewDisableButtonCell.Enabled = false;
					((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["reboot"]).Enabled = false;
					((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["reboot"]).ReadOnly = true;
				}
				else
				{
					dataGridViewDisableButtonCell.Enabled = true;
					if (dataGridViewRow2.Cells["portStatus"].Value.ToString().Equals(EcoLanguage.getMsg(LangRes.OPST_OFF, new string[0])))
					{
						((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["reboot"]).Enabled = false;
						((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["reboot"]).ReadOnly = true;
					}
					else
					{
						string value = dataGridViewRow2.Cells["port_number"].Value.ToString();
						int num3 = System.Convert.ToInt32(value);
						if (deviceModelConfig.isOutletRebootable(num3 - 1))
						{
							if (list == null)
							{
								list = (System.Collections.Generic.List<PortInfo>)ClientAPI.RemoteCall(4, 1, System.Convert.ToString(this.m_devID), 10000);
								if (list == null)
								{
									continue;
								}
							}
							PortInfo portInfo = list[num3 - 1];
							if (portInfo.OutletShutdownMethod == 1)
							{
								((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["reboot"]).Enabled = false;
								((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["reboot"]).ReadOnly = true;
							}
						}
						else
						{
							((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["reboot"]).ReadOnly = false;
						}
					}
				}
			}
			if (num >= 0)
			{
				this.dgvOutletInfo.CurrentCell = this.dgvOutletInfo.Rows[num].Cells["port_nm"];
			}
			this.dgvOutletInfo.Update();
		}
		private void getDeviceSensorData(string deviceId, int devonline)
		{
			if (!this.dgvDeviceSensorInfo.Visible)
			{
				return;
			}
			DataRow[] dataRows = ClientAPI.getDataRows(1, "device_id=" + deviceId, "sensor_type ASC");
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("sensor_type", typeof(short));
			dataTable.Columns.Add("humiditys", typeof(string));
			dataTable.Columns.Add("temperatures", typeof(string));
			dataTable.Columns.Add("press_values", typeof(string));
			if (devonline == 1)
			{
				DataRow[] array = dataRows;
				for (int i = 0; i < array.Length; i++)
				{
					DataRow dataRow = array[i];
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2["sensor_type"] = dataRow["sensor_type"];
					if (dataRow["humidity"].ToString().Equals(System.Convert.ToString(-500)))
					{
						dataRow2["humiditys"] = "";
					}
					else
					{
						if (dataRow["humidity"].ToString().Equals(System.Convert.ToString(-1000)))
						{
							dataRow2["humiditys"] = "N/A";
						}
						else
						{
							dataRow2["humiditys"] = dataRow["humidity"];
						}
					}
					if (dataRow["temperature"].ToString().Equals(System.Convert.ToString(-500)))
					{
						dataRow2["temperatures"] = "";
					}
					else
					{
						if (dataRow["temperature"].ToString().Equals(System.Convert.ToString(-1000)))
						{
							dataRow2["temperatures"] = "N/A";
						}
						else
						{
							dataRow2["temperatures"] = dataRow["temperature"];
						}
					}
					if (dataRow["press_value"].ToString().Equals(System.Convert.ToString(-500)))
					{
						dataRow2["press_values"] = "";
					}
					else
					{
						if (dataRow["press_value"].ToString().Equals(System.Convert.ToString(-1000)))
						{
							dataRow2["press_values"] = "N/A";
						}
						else
						{
							dataRow2["press_values"] = dataRow["press_value"];
						}
					}
					dataTable.Rows.Add(dataRow2);
				}
			}
			this.dgvDeviceSensorInfo.DataSource = dataTable;
		}
		private void getBankData(string deviceId, bool initFlg, int devonline)
		{
			if (!this.dgvBankInfo.Visible)
			{
				return;
			}
			DataRow[] dataRows = ClientAPI.getDataRows(3, "device_id=" + deviceId, "bank_number ASC");
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("bank_id", typeof(int));
			dataTable.Columns.Add("bank_number", typeof(int));
			dataTable.Columns.Add("bank_nm", typeof(string));
			dataTable.Columns.Add("bank_acton", typeof(string));
			dataTable.Columns.Add("bank_actoff", typeof(string));
			dataTable.Columns.Add("bank_reboot", typeof(string));
			dataTable.Columns.Add("voltage_values", typeof(string));
			dataTable.Columns.Add("current_values", typeof(string));
			dataTable.Columns.Add("power_values", typeof(string));
			dataTable.Columns.Add("power_consumptions", typeof(string));
			if (devonline == 1)
			{
				DataRow[] array = dataRows;
				for (int i = 0; i < array.Length; i++)
				{
					DataRow dataRow = array[i];
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2["bank_id"] = dataRow["bank_id"];
					dataRow2["bank_number"] = dataRow["bank_number"];
					if (dataRow["bank_nm"].ToString().Equals(System.Convert.ToString(-500)) || dataRow["bank_nm"].ToString().Equals(System.Convert.ToString(-1000)))
					{
						dataRow2["bank_nm"] = "";
					}
					else
					{
						dataRow2["bank_nm"] = dataRow["bank_nm"];
					}
					string text = dataRow2["bank_nm"].ToString();
					if (text.Equals("\0"))
					{
						dataRow2["bank_nm"] = string.Empty;
					}
					dataRow2["bank_acton"] = EcoLanguage.getMsg(LangRes.OPACT_ON, new string[0]);
					dataRow2["bank_actoff"] = EcoLanguage.getMsg(LangRes.OPACT_OFF, new string[0]);
					dataRow2["bank_reboot"] = "0";
					this.dataTransPdu(dataRow, dataRow2, false);
					dataTable.Rows.Add(dataRow2);
				}
				if (!initFlg)
				{
					for (int j = 0; j < this.dgvBankInfo.Rows.Count; j++)
					{
						DataGridViewRow dataGridViewRow = this.dgvBankInfo.Rows[j];
						if (dataGridViewRow.Cells["bank_reboot"].Value == null || dataGridViewRow.Cells["bank_reboot"].Value.Equals(string.Empty))
						{
							break;
						}
						dataTable.Rows[j]["bank_reboot"] = dataGridViewRow.Cells["bank_reboot"].Value.ToString();
					}
				}
			}
			this.dgvBankInfo.DataSource = dataTable;
			if (this.dgvBankInfo.Columns["bank_acton"].Visible)
			{
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(this.m_ModelNm, this.m_FWVer);
				uint bankCtrlflg = deviceModelConfig.bankCtrlflg;
				for (int k = 0; k < this.dgvBankInfo.Rows.Count; k++)
				{
					bool flag = true;
					if (this.m_PortIDs_Map.Count > 0)
					{
						flag = false;
						System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
						for (int l = deviceModelConfig.bankOutlets[k].fromPort; l <= deviceModelConfig.bankOutlets[k].toPort; l++)
						{
							if (deviceModelConfig.isOutletSwitchable(l - 1))
							{
								foreach (System.Collections.Generic.KeyValuePair<string, int> current in this.m_PortIDs_Map)
								{
									if (l == current.Value)
									{
										list.Add(current.Value);
									}
								}
							}
						}
						if (list.Count > 0)
						{
							flag = true;
						}
					}
					DataGridViewRow dataGridViewRow2 = this.dgvBankInfo.Rows[k];
					if (((ulong)bankCtrlflg & (ulong)(1L << (k & 31))) != 0uL && flag && EcoGlobalVar.flgEnablePower)
					{
						((DataGridViewDisableButtonCell)dataGridViewRow2.Cells["bank_acton"]).Enabled = true;
						((DataGridViewDisableButtonCell)dataGridViewRow2.Cells["bank_actoff"]).Enabled = true;
						((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["bank_reboot"]).Enabled = true;
						((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["bank_reboot"]).ReadOnly = false;
					}
					else
					{
						((DataGridViewDisableButtonCell)dataGridViewRow2.Cells["bank_acton"]).Enabled = false;
						((DataGridViewDisableButtonCell)dataGridViewRow2.Cells["bank_actoff"]).Enabled = false;
						((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["bank_reboot"]).Enabled = false;
						((DataGridViewDisableCheckBoxCell)dataGridViewRow2.Cells["bank_reboot"]).ReadOnly = true;
					}
				}
			}
			this.dgvBankInfo.Update();
		}
		private void getLineData(string deviceId, bool initFlg, int devonline)
		{
			if (!this.dgvLineInfo.Visible)
			{
				return;
			}
			DataRow[] dataRows = ClientAPI.getDataRows(4, "device_id=" + deviceId, "line_number ASC");
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("line_number", typeof(int));
			dataTable.Columns.Add("voltage_values", typeof(string));
			dataTable.Columns.Add("current_values", typeof(string));
			dataTable.Columns.Add("power_values", typeof(string));
			if (devonline == 1)
			{
				DataRow[] array = dataRows;
				for (int i = 0; i < array.Length; i++)
				{
					DataRow dataRow = array[i];
					DataRow dataRow2 = dataTable.NewRow();
					dataRow2["line_number"] = dataRow["line_number"];
					this.dataTransPdu(dataRow, dataRow2, false);
					dataTable.Rows.Add(dataRow2);
				}
			}
			this.dgvLineInfo.DataSource = dataTable;
			this.dgvLineInfo.Update();
		}
		private void pageControlInit()
		{
			this.dgvOutletInfo.Width = this.dgvOutletInfo.Columns["port_number"].Width + this.dgvOutletInfo.Columns["port_nm"].Width;
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(this.m_ModelNm, this.m_FWVer);
			this.butOn.Hide();
			this.butOff.Hide();
			this.cbDReboot.Hide();
			if (deviceModelConfig.switchableOutlets != 0uL)
			{
				this.dgvOutletInfo.Columns["portStatus"].Visible = true;
				this.dgvOutletInfo.Columns["act"].Visible = true;
				this.dgvOutletInfo.Columns["reboot"].Visible = true;
				this.dgvOutletInfo.Width = this.dgvOutletInfo.Width + this.dgvOutletInfo.Columns["portStatus"].Width + this.dgvOutletInfo.Columns["act"].Width + this.dgvOutletInfo.Columns["reboot"].Width;
			}
			else
			{
				this.dgvOutletInfo.Columns["portStatus"].Visible = false;
				this.dgvOutletInfo.Columns["act"].Visible = false;
				this.dgvOutletInfo.Columns["reboot"].Visible = false;
			}
			if (deviceModelConfig.perportreading == 2)
			{
				this.dgvOutletInfo.Columns["portVoltage"].Visible = true;
				this.dgvOutletInfo.Columns["portCurrent"].Visible = true;
				this.dgvOutletInfo.Columns["portPower"].Visible = true;
				this.dgvOutletInfo.Columns["portPowerDiss"].Visible = true;
				this.dgvOutletInfo.Width = this.dgvOutletInfo.Width + this.dgvOutletInfo.Columns["portVoltage"].Width + this.dgvOutletInfo.Columns["portCurrent"].Width + this.dgvOutletInfo.Columns["portPower"].Width + this.dgvOutletInfo.Columns["portPowerDiss"].Width;
			}
			else
			{
				this.dgvOutletInfo.Columns["portVoltage"].Visible = false;
				this.dgvOutletInfo.Columns["portCurrent"].Visible = false;
				this.dgvOutletInfo.Columns["portPower"].Visible = false;
				this.dgvOutletInfo.Columns["portPowerDiss"].Visible = false;
			}
			Point location = new Point(this.labSensorStatus.Location.X, this.labSensorStatus.Location.Y + this.m_orgOffsetOutetlab);
			Point location2 = new Point(this.dgvDeviceSensorInfo.Location.X, location.Y + this.m_orgOffsetOutletTB);
			bool flag = true;
			if (EcoGlobalVar.gl_LoginUser.UserType == 1 || deviceModelConfig.sensorNum == 0)
			{
				flag = false;
			}
			if (!flag)
			{
				this.labSensorStatus.Hide();
				this.dgvDeviceSensorInfo.Hide();
			}
			else
			{
				this.labSensorStatus.Show();
				this.dgvDeviceSensorInfo.Show();
			}
			if (deviceModelConfig.leakCurrent == Constant.NO)
			{
				this.lbLeakStatus.Hide();
			}
			else
			{
				this.lbLeakStatus.Show();
				this.lbLeakStatus.Text = this.mStr_LeakStatus + " N/A";
			}
			bool flag2 = true;
			if ((EcoGlobalVar.gl_LoginUser.UserType == 1 && this.m_PortIDs_Map.Count != 0) || deviceModelConfig.perlineReading == Constant.NO)
			{
				flag2 = false;
			}
			if (!flag2)
			{
				this.lbLineStatus.Hide();
				this.dgvLineInfo.Hide();
			}
			else
			{
				if (!flag)
				{
					this.lbLineStatus.Location = this.labSensorStatus.Location;
					this.dgvLineInfo.Location = new Point(this.dgvDeviceSensorInfo.Location.X, this.dgvLineInfo.Location.Y);
				}
				else
				{
					this.lbLineStatus.Location = new Point(this.labSensorStatus.Location.X + this.m_orgOffsetLinelab_X, this.lbLineStatus.Location.Y);
					this.dgvLineInfo.Location = new Point(this.lbLineStatus.Location.X, this.dgvLineInfo.Location.Y);
				}
				this.lbLineStatus.Show();
				this.dgvLineInfo.Show();
			}
			if (!flag && !flag2)
			{
				location = this.labSensorStatus.Location;
				location2 = new Point(this.dgvDeviceSensorInfo.Location.X, location.Y + this.m_orgOffsetOutletTB);
			}
			Point location3 = new Point(location.X, location.Y + this.m_orgOffsetBanklab);
			Point location4 = new Point(location2.X, location3.Y + this.m_orgOffsetBankTB);
			if (deviceModelConfig.switchableOutlets == 0uL && deviceModelConfig.perportreading == 1)
			{
				this.labOutletStatus.Hide();
				this.dgvOutletInfo.Hide();
				location3 = new Point(location.X, location.Y);
				location4 = new Point(location2.X, location3.Y + this.m_orgOffsetBankTB);
			}
			else
			{
				this.dgvOutletInfo.Show();
				this.m_outletTableWidth = this.dgvOutletInfo.Width;
				this.labOutletStatus.Location = location;
				this.dgvOutletInfo.Location = location2;
				this.labOutletStatus.Show();
			}
			bool flag3 = true;
			if ((EcoGlobalVar.gl_LoginUser.UserType == 1 && this.m_PortIDs_Map.Count != 0) || deviceModelConfig.perbankReading == 1)
			{
				flag3 = false;
			}
			if (!flag3)
			{
				this.labBankStatus.Hide();
				this.dgvBankInfo.Hide();
				return;
			}
			this.labBankStatus.Location = location3;
			this.dgvBankInfo.Location = location4;
			this.labBankStatus.Show();
			this.dgvBankInfo.Show();
			if (deviceModelConfig.bankCtrlflg == 0u)
			{
				this.dgvBankInfo.Columns["bank_acton"].Visible = false;
				this.dgvBankInfo.Columns["bank_actoff"].Visible = false;
				this.dgvBankInfo.Columns["bank_reboot"].Visible = false;
				this.dgvBankInfo.Width = this.dgvBankInfo.Columns["bank_number"].Width + this.dgvBankInfo.Columns["bank_nm"].Width + this.dgvBankInfo.Columns["bankvoltage"].Width + this.dgvBankInfo.Columns["bankcurrent"].Width + this.dgvBankInfo.Columns["bankpower"].Width + this.dgvBankInfo.Columns["bankpowerD"].Width;
				return;
			}
			this.dgvBankInfo.Columns["bank_acton"].Visible = true;
			this.dgvBankInfo.Columns["bank_actoff"].Visible = true;
			this.dgvBankInfo.Columns["bank_reboot"].Visible = true;
			this.dgvBankInfo.Width = this.dgvBankInfo.Columns["bank_number"].Width + this.dgvBankInfo.Columns["bank_nm"].Width + this.dgvBankInfo.Columns["bank_acton"].Width + this.dgvBankInfo.Columns["bank_actoff"].Width + this.dgvBankInfo.Columns["bank_reboot"].Width + this.dgvBankInfo.Columns["bankvoltage"].Width + this.dgvBankInfo.Columns["bankcurrent"].Width + this.dgvBankInfo.Columns["bankpower"].Width + this.dgvBankInfo.Columns["bankpowerD"].Width;
		}
		private void butOn_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.m_devID < 0)
				{
					return;
				}
				DeviceInfo deviceInfo = (DeviceInfo)ClientAPI.RemoteCall(3, 1, System.Convert.ToString(this.m_devID), 10000);
				if (deviceInfo == null)
				{
					this.except_devnotExist(this.m_devName);
					return;
				}
				this.m_parent.endTimer();
				DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_on, new string[0]), MessageBoxButtons.OKCancel);
				if (dialogResult == DialogResult.Cancel)
				{
					this.m_parent.starTimer();
					return;
				}
				DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceInfo);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceInfo.ModelNm, deviceInfo.FWVersion);
				bool flag;
				if (this.m_PortIDs_Map.Count == 0 || this.m_PortIDs_Map.Count == deviceModelConfig.portNum)
				{
					DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
					flag = devAccessAPI.TurnOnAllOutlets();
				}
				else
				{
					System.Collections.Generic.List<DevSnmpConfig> list = new System.Collections.Generic.List<DevSnmpConfig>();
					System.Collections.Generic.List<int> list2 = new System.Collections.Generic.List<int>();
					foreach (System.Collections.Generic.KeyValuePair<string, int> current in this.m_PortIDs_Map)
					{
						list2.Add(current.Value);
					}
					sNMPpara.groupOutlets = list2;
					list.Add(sNMPpara);
					DevPortGroupAPI devPortGroupAPI = new DevPortGroupAPI(list);
					flag = devPortGroupAPI.TurnOnGroupOutlets();
				}
				if (flag)
				{
					int devonline = 0;
					if (ClientAPI.IsDeviceOnline(this.m_devID))
					{
						devonline = 1;
					}
					this.getOutletData(this.m_devID.ToString(), false, devonline);
					string text = string.Concat(new string[]
					{
						"0630050\n",
						deviceInfo.ModelNm,
						"\n",
						deviceInfo.DeviceName,
						"\n",
						deviceInfo.DeviceIP
					});
					string valuePair = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair))
					{
						text = text + "\n" + valuePair;
					}
					ClientAPI.RemoteCall(100, 1, text, 10000);
					EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
				}
				else
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("butOn_Click fail ! " + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
			this.m_parent.starTimer();
		}
		private void butOff_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.m_devID < 0)
				{
					return;
				}
				DeviceInfo deviceInfo = (DeviceInfo)ClientAPI.RemoteCall(3, 1, System.Convert.ToString(this.m_devID), 10000);
				if (deviceInfo == null)
				{
					this.except_devnotExist(this.m_devName);
					return;
				}
				DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceInfo);
				DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceInfo.ModelNm, deviceInfo.FWVersion);
				this.m_parent.endTimer();
				bool flag;
				if (this.cbDReboot.Checked)
				{
					DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_reboot, new string[0]), MessageBoxButtons.OKCancel);
					if (dialogResult == DialogResult.Cancel)
					{
						this.m_parent.starTimer();
						return;
					}
					if (this.m_PortIDs_Map.Count == 0 || this.m_PortIDs_Map.Count == deviceModelConfig.portNum)
					{
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						flag = devAccessAPI.RebootAllOutlets();
					}
					else
					{
						System.Collections.Generic.List<DevSnmpConfig> list = new System.Collections.Generic.List<DevSnmpConfig>();
						System.Collections.Generic.List<int> list2 = new System.Collections.Generic.List<int>();
						foreach (System.Collections.Generic.KeyValuePair<string, int> current in this.m_PortIDs_Map)
						{
							list2.Add(current.Value);
						}
						sNMPpara.groupOutlets = list2;
						list.Add(sNMPpara);
						DevPortGroupAPI devPortGroupAPI = new DevPortGroupAPI(list);
						flag = devPortGroupAPI.RebootGroupOutlets();
					}
					if (flag)
					{
						string text = string.Concat(new string[]
						{
							"0630052\n",
							deviceInfo.ModelNm,
							"\n",
							deviceInfo.DeviceName,
							"\n",
							deviceInfo.DeviceIP
						});
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							text = text + "\n" + valuePair;
						}
						ClientAPI.RemoteCall(100, 1, text, 10000);
					}
				}
				else
				{
					DialogResult dialogResult2 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_off, new string[0]), MessageBoxButtons.OKCancel);
					if (dialogResult2 == DialogResult.Cancel)
					{
						this.m_parent.starTimer();
						return;
					}
					if (this.m_PortIDs_Map.Count == 0 || this.m_PortIDs_Map.Count == deviceModelConfig.portNum)
					{
						DevAccessAPI devAccessAPI2 = new DevAccessAPI(sNMPpara);
						flag = devAccessAPI2.TurnOffAllOutlets();
					}
					else
					{
						System.Collections.Generic.List<DevSnmpConfig> list3 = new System.Collections.Generic.List<DevSnmpConfig>();
						System.Collections.Generic.List<int> list4 = new System.Collections.Generic.List<int>();
						foreach (System.Collections.Generic.KeyValuePair<string, int> current2 in this.m_PortIDs_Map)
						{
							list4.Add(current2.Value);
						}
						sNMPpara.groupOutlets = list4;
						list3.Add(sNMPpara);
						DevPortGroupAPI devPortGroupAPI2 = new DevPortGroupAPI(list3);
						flag = devPortGroupAPI2.TurnOffGroupOutlets();
					}
					if (flag)
					{
						string text2 = string.Concat(new string[]
						{
							"0630051\n",
							deviceInfo.ModelNm,
							"\n",
							deviceInfo.DeviceName,
							"\n",
							deviceInfo.DeviceIP
						});
						string valuePair2 = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair2))
						{
							text2 = text2 + "\n" + valuePair2;
						}
						ClientAPI.RemoteCall(100, 1, text2, 10000);
					}
				}
				if (flag)
				{
					int devonline = 0;
					if (ClientAPI.IsDeviceOnline(this.m_devID))
					{
						devonline = 1;
					}
					this.getOutletData(this.m_devID.ToString(), false, devonline);
					EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
				}
				else
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("butOff_Click Error! " + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
			this.m_parent.starTimer();
		}
		private void dgvOutletInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (this.m_devID >= 0)
				{
					if (e.RowIndex >= 0)
					{
						if (this.dgvOutletInfo.Columns[e.ColumnIndex].Name.Equals("reboot"))
						{
							DataGridViewDisableCheckBoxCell dataGridViewDisableCheckBoxCell = (DataGridViewDisableCheckBoxCell)this.dgvOutletInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
							if (dataGridViewDisableCheckBoxCell.Enabled)
							{
								if (!this.dgvOutletInfo.Rows[e.RowIndex].Cells["portStatus"].Value.ToString().Equals(EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0])))
								{
									if (this.dgvOutletInfo.Rows[e.RowIndex].Cells["reboot"].Value.Equals("0"))
									{
										this.dgvOutletInfo.Rows[e.RowIndex].Cells["reboot"].Value = "1";
									}
									else
									{
										this.dgvOutletInfo.Rows[e.RowIndex].Cells["reboot"].Value = "0";
									}
								}
							}
						}
						else
						{
							if (this.dgvOutletInfo.Columns[e.ColumnIndex] is DataGridViewDisableButtonColumn)
							{
								DataGridViewDisableButtonCell dataGridViewDisableButtonCell = (DataGridViewDisableButtonCell)this.dgvOutletInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
								if (dataGridViewDisableButtonCell.Enabled)
								{
									string text = this.dgvOutletInfo.Rows[e.RowIndex].Cells["portStatus"].Value.ToString();
									if (!text.Equals(EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0])))
									{
										DeviceInfo deviceInfo = (DeviceInfo)ClientAPI.RemoteCall(3, 1, System.Convert.ToString(this.m_devID), 10000);
										if (deviceInfo == null)
										{
											this.except_devnotExist(this.m_devName);
										}
										else
										{
											this.m_parent.endTimer();
											DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceInfo);
											DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
											string text2 = this.dgvOutletInfo.Rows[e.RowIndex].Cells["port_number"].Value.ToString();
											string text3 = this.dgvOutletInfo.Rows[e.RowIndex].Cells["port_nm"].Value.ToString();
											if (this.dgvOutletInfo.Rows[e.RowIndex].Cells["act"].Value.Equals(EcoLanguage.getMsg(LangRes.OPACT_OFF, new string[0])))
											{
												if (this.dgvOutletInfo.Rows[e.RowIndex].Cells["reboot"].Value.Equals("0"))
												{
													DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_off, new string[0]), MessageBoxButtons.OKCancel);
													if (dialogResult == DialogResult.Cancel)
													{
														this.m_parent.starTimer();
														return;
													}
													if (!devAccessAPI.TurnOffOutlet(System.Convert.ToInt32(text2)))
													{
														EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
														this.m_parent.starTimer();
														return;
													}
													string text4 = string.Concat(new string[]
													{
														"0630054\n",
														text3,
														"\n",
														text2,
														"\n",
														deviceInfo.DeviceIP,
														"\n",
														deviceInfo.DeviceName
													});
													string valuePair = ValuePairs.getValuePair("Username");
													if (!string.IsNullOrEmpty(valuePair))
													{
														text4 = text4 + "\n" + valuePair;
													}
													ClientAPI.RemoteCall(100, 1, text4, 10000);
													this.dgvOutletInfo.Rows[e.RowIndex].Cells["portStatus"].Value = EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0]);
													((DataGridViewDisableButtonCell)this.dgvOutletInfo.Rows[e.RowIndex].Cells["act"]).Enabled = false;
												}
												else
												{
													DialogResult dialogResult2 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_reboot, new string[0]), MessageBoxButtons.OKCancel);
													if (dialogResult2 == DialogResult.Cancel)
													{
														this.m_parent.starTimer();
														return;
													}
													if (!devAccessAPI.RebootOutlet(System.Convert.ToInt32(text2)))
													{
														EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
														this.m_parent.starTimer();
														return;
													}
													string text5 = string.Concat(new string[]
													{
														"0630055\n",
														text3,
														"\n",
														text2,
														"\n",
														deviceInfo.DeviceIP,
														"\n",
														deviceInfo.DeviceName
													});
													string valuePair2 = ValuePairs.getValuePair("Username");
													if (!string.IsNullOrEmpty(valuePair2))
													{
														text5 = text5 + "\n" + valuePair2;
													}
													ClientAPI.RemoteCall(100, 1, text5, 10000);
													this.dgvOutletInfo.Rows[e.RowIndex].Cells["portStatus"].Value = EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0]);
													((DataGridViewDisableButtonCell)this.dgvOutletInfo.Rows[e.RowIndex].Cells["act"]).Enabled = false;
												}
											}
											else
											{
												DialogResult dialogResult3 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_on, new string[0]), MessageBoxButtons.OKCancel);
												if (dialogResult3 == DialogResult.Cancel)
												{
													this.m_parent.starTimer();
													return;
												}
												if (!devAccessAPI.TurnOnOutlet(System.Convert.ToInt32(text2)))
												{
													EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
													this.m_parent.starTimer();
													return;
												}
												string text6 = string.Concat(new string[]
												{
													"0630053\n",
													text3,
													"\n",
													text2,
													"\n",
													deviceInfo.DeviceIP,
													"\n",
													deviceInfo.DeviceName
												});
												string valuePair3 = ValuePairs.getValuePair("Username");
												if (!string.IsNullOrEmpty(valuePair3))
												{
													text6 = text6 + "\n" + valuePair3;
												}
												ClientAPI.RemoteCall(100, 1, text6, 10000);
												this.dgvOutletInfo.Rows[e.RowIndex].Cells["portStatus"].Value = EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0]);
												((DataGridViewDisableButtonCell)this.dgvOutletInfo.Rows[e.RowIndex].Cells["act"]).Enabled = false;
											}
											this.m_parent.starTimer();
										}
									}
								}
							}
						}
					}
				}
			}
			catch (SnmpException ex)
			{
				System.Console.WriteLine("dgvOutletInfo_CellContentClick error!" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
		}
		private void dgvBankInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (this.m_devID >= 0)
				{
					if (e.RowIndex >= 0)
					{
						if (this.dgvBankInfo.Columns[e.ColumnIndex].Name.Equals("bank_reboot"))
						{
							DataGridViewDisableCheckBoxCell dataGridViewDisableCheckBoxCell = (DataGridViewDisableCheckBoxCell)this.dgvOutletInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
							if (dataGridViewDisableCheckBoxCell.Enabled)
							{
								if (this.dgvBankInfo.Rows[e.RowIndex].Cells["bank_reboot"].Value.Equals("0"))
								{
									this.dgvBankInfo.Rows[e.RowIndex].Cells["bank_reboot"].Value = "1";
								}
								else
								{
									this.dgvBankInfo.Rows[e.RowIndex].Cells["bank_reboot"].Value = "0";
								}
							}
						}
						else
						{
							if (this.dgvBankInfo.Columns[e.ColumnIndex].Name.Equals("bank_acton"))
							{
								DataGridViewDisableButtonCell dataGridViewDisableButtonCell = (DataGridViewDisableButtonCell)this.dgvBankInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
								if (!dataGridViewDisableButtonCell.Enabled)
								{
									return;
								}
								DeviceInfo deviceInfo = (DeviceInfo)ClientAPI.RemoteCall(3, 1, System.Convert.ToString(this.m_devID), 10000);
								if (deviceInfo == null)
								{
									this.except_devnotExist(this.m_devName);
									return;
								}
								DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceInfo);
								DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceInfo.ModelNm, deviceInfo.FWVersion);
								this.m_parent.endTimer();
								string value = this.dgvBankInfo.Rows[e.RowIndex].Cells["bank_number"].Value.ToString();
								int num = System.Convert.ToInt32(value);
								System.Collections.Generic.List<DevSnmpConfig> list = new System.Collections.Generic.List<DevSnmpConfig>();
								System.Collections.Generic.List<int> list2 = new System.Collections.Generic.List<int>();
								for (int i = deviceModelConfig.bankOutlets[num - 1].fromPort; i <= deviceModelConfig.bankOutlets[num - 1].toPort; i++)
								{
									if (deviceModelConfig.isOutletSwitchable(i - 1))
									{
										if (this.m_PortIDs_Map.Count == 0)
										{
											list2.Add(i);
										}
										else
										{
											foreach (System.Collections.Generic.KeyValuePair<string, int> current in this.m_PortIDs_Map)
											{
												if (i == current.Value)
												{
													list2.Add(current.Value);
												}
											}
										}
									}
								}
								DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_on, new string[0]), MessageBoxButtons.OKCancel);
								if (dialogResult == DialogResult.Cancel)
								{
									this.m_parent.starTimer();
									return;
								}
								sNMPpara.groupOutlets = list2;
								list.Add(sNMPpara);
								DevPortGroupAPI devPortGroupAPI = new DevPortGroupAPI(list);
								if (!devPortGroupAPI.TurnOnGroupOutlets())
								{
									EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
									this.m_parent.starTimer();
									return;
								}
								this.m_parent.starTimer();
							}
							if (this.dgvBankInfo.Columns[e.ColumnIndex].Name.Equals("bank_actoff"))
							{
								DataGridViewDisableButtonCell dataGridViewDisableButtonCell2 = (DataGridViewDisableButtonCell)this.dgvBankInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
								if (dataGridViewDisableButtonCell2.Enabled)
								{
									DeviceInfo deviceInfo2 = (DeviceInfo)ClientAPI.RemoteCall(3, 1, System.Convert.ToString(this.m_devID), 10000);
									if (deviceInfo2 == null)
									{
										this.except_devnotExist(this.m_devName);
									}
									else
									{
										DevSnmpConfig sNMPpara2 = commUtil.getSNMPpara(deviceInfo2);
										DevModelConfig deviceModelConfig2 = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceInfo2.ModelNm, deviceInfo2.FWVersion);
										this.m_parent.endTimer();
										string value2 = this.dgvBankInfo.Rows[e.RowIndex].Cells["bank_number"].Value.ToString();
										int num2 = System.Convert.ToInt32(value2);
										System.Collections.Generic.List<DevSnmpConfig> list3 = new System.Collections.Generic.List<DevSnmpConfig>();
										System.Collections.Generic.List<int> list4 = new System.Collections.Generic.List<int>();
										for (int j = deviceModelConfig2.bankOutlets[num2 - 1].fromPort; j <= deviceModelConfig2.bankOutlets[num2 - 1].toPort; j++)
										{
											if (deviceModelConfig2.isOutletSwitchable(j - 1))
											{
												if (this.m_PortIDs_Map.Count == 0)
												{
													list4.Add(j);
												}
												else
												{
													foreach (System.Collections.Generic.KeyValuePair<string, int> current2 in this.m_PortIDs_Map)
													{
														if (j == current2.Value)
														{
															list4.Add(current2.Value);
														}
													}
												}
											}
										}
										if (this.dgvBankInfo.Rows[e.RowIndex].Cells["bank_reboot"].Value.Equals("0"))
										{
											DialogResult dialogResult2 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_off, new string[0]), MessageBoxButtons.OKCancel);
											if (dialogResult2 == DialogResult.Cancel)
											{
												this.m_parent.starTimer();
												return;
											}
											sNMPpara2.groupOutlets = list4;
											list3.Add(sNMPpara2);
											DevPortGroupAPI devPortGroupAPI2 = new DevPortGroupAPI(list3);
											if (!devPortGroupAPI2.TurnOffGroupOutlets())
											{
												EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
												this.m_parent.starTimer();
												return;
											}
										}
										else
										{
											DialogResult dialogResult3 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_reboot, new string[0]), MessageBoxButtons.OKCancel);
											if (dialogResult3 == DialogResult.Cancel)
											{
												this.m_parent.starTimer();
												return;
											}
											sNMPpara2.groupOutlets = list4;
											list3.Add(sNMPpara2);
											DevPortGroupAPI devPortGroupAPI3 = new DevPortGroupAPI(list3);
											if (!devPortGroupAPI3.RebootGroupOutlets())
											{
												EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
												this.m_parent.starTimer();
												return;
											}
										}
										this.m_parent.starTimer();
									}
								}
							}
						}
					}
				}
			}
			catch (SnmpException ex)
			{
				System.Console.WriteLine("dgvBankInfo_CellContentClick error!" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataGpOPDev));
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
			this.dgvBankInfo = new DataGridView();
			this.bank_id = new DataGridViewTextBoxColumn();
			this.bank_number = new DataGridViewTextBoxColumn();
			this.bank_nm = new DataGridViewTextBoxColumn();
			this.bank_acton = new DataGridViewDisableButtonColumn();
			this.bank_actoff = new DataGridViewDisableButtonColumn();
			this.bank_reboot = new DataGridViewDisableCheckBoxColumn();
			this.bankvoltage = new DataGridViewTextBoxColumn();
			this.bankcurrent = new DataGridViewTextBoxColumn();
			this.bankpower = new DataGridViewTextBoxColumn();
			this.bankpowerD = new DataGridViewTextBoxColumn();
			this.labIp = new Label();
			this.labBankStatus = new Label();
			this.cbDReboot = new CheckBox();
			this.butOff = new Button();
			this.butOn = new Button();
			this.labDevStatus = new Label();
			this.labSensorStatus = new Label();
			this.labOutletStatus = new Label();
			this.dgvDeviceSensorInfo = new DataGridView();
			this.sensor_type = new DataGridViewTextBoxColumn();
			this.humiditys = new DataGridViewTextBoxColumn();
			this.temperatures = new DataGridViewTextBoxColumn();
			this.press_values = new DataGridViewTextBoxColumn();
			this.dgvDeviceInfo = new DataGridView();
			this.deviceNm = new DataGridViewTextBoxColumn();
			this.dVoltage = new DataGridViewTextBoxColumn();
			this.dCurrent = new DataGridViewTextBoxColumn();
			this.dPower = new DataGridViewTextBoxColumn();
			this.dPowerDiss = new DataGridViewTextBoxColumn();
			this.dgvOutletInfo = new DataGridView();
			this.port_id = new DataGridViewTextBoxColumn();
			this.port_number = new DataGridViewTextBoxColumn();
			this.port_nm = new DataGridViewTextBoxColumn();
			this.portStatus = new DataGridViewTextBoxColumn();
			this.act = new DataGridViewDisableButtonColumn();
			this.reboot = new DataGridViewDisableCheckBoxColumn();
			this.portVoltage = new DataGridViewTextBoxColumn();
			this.portCurrent = new DataGridViewTextBoxColumn();
			this.portPower = new DataGridViewTextBoxColumn();
			this.portPowerDiss = new DataGridViewTextBoxColumn();
			this.lbLeakStatus = new Label();
			this.lbLineStatus = new Label();
			this.dgvLineInfo = new DataGridView();
			this.dataGridViewTextBoxColumn25 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn26 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn27 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn28 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewDisableButtonColumn1 = new DataGridViewDisableButtonColumn();
			this.dataGridViewDisableCheckBoxColumn1 = new DataGridViewDisableCheckBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn14 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn15 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn16 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn17 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn18 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn19 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn20 = new DataGridViewTextBoxColumn();
			this.dataGridViewDisableButtonColumn2 = new DataGridViewDisableButtonColumn();
			this.dataGridViewDisableButtonColumn3 = new DataGridViewDisableButtonColumn();
			this.dataGridViewDisableCheckBoxColumn2 = new DataGridViewDisableCheckBoxColumn();
			this.dataGridViewTextBoxColumn21 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn22 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn23 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn24 = new DataGridViewTextBoxColumn();
			((ISupportInitialize)this.dgvBankInfo).BeginInit();
			((ISupportInitialize)this.dgvDeviceSensorInfo).BeginInit();
			((ISupportInitialize)this.dgvDeviceInfo).BeginInit();
			((ISupportInitialize)this.dgvOutletInfo).BeginInit();
			((ISupportInitialize)this.dgvLineInfo).BeginInit();
			base.SuspendLayout();
			this.dgvBankInfo.AllowUserToAddRows = false;
			this.dgvBankInfo.AllowUserToDeleteRows = false;
			this.dgvBankInfo.AllowUserToResizeColumns = false;
			this.dgvBankInfo.AllowUserToResizeRows = false;
			this.dgvBankInfo.BackgroundColor = Color.White;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvBankInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvBankInfo, "dgvBankInfo");
			this.dgvBankInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvBankInfo.Columns.AddRange(new DataGridViewColumn[]
			{
				this.bank_id,
				this.bank_number,
				this.bank_nm,
				this.bank_acton,
				this.bank_actoff,
				this.bank_reboot,
				this.bankvoltage,
				this.bankcurrent,
				this.bankpower,
				this.bankpowerD
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.White;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvBankInfo.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvBankInfo.MultiSelect = false;
			this.dgvBankInfo.Name = "dgvBankInfo";
			this.dgvBankInfo.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvBankInfo.RowHeadersVisible = false;
			this.dgvBankInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvBankInfo.RowTemplate.Height = 23;
			this.dgvBankInfo.RowTemplate.ReadOnly = true;
			this.dgvBankInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvBankInfo.StandardTab = true;
			this.dgvBankInfo.TabStop = false;
			this.dgvBankInfo.CellContentClick += new DataGridViewCellEventHandler(this.dgvBankInfo_CellContentClick);
			this.bank_id.DataPropertyName = "bank_id";
			componentResourceManager.ApplyResources(this.bank_id, "bank_id");
			this.bank_id.Name = "bank_id";
			this.bank_number.DataPropertyName = "bank_number";
			componentResourceManager.ApplyResources(this.bank_number, "bank_number");
			this.bank_number.Name = "bank_number";
			this.bank_number.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.bank_nm.DataPropertyName = "bank_nm";
			componentResourceManager.ApplyResources(this.bank_nm, "bank_nm");
			this.bank_nm.Name = "bank_nm";
			this.bank_nm.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.bank_acton.DataPropertyName = "bank_acton";
			componentResourceManager.ApplyResources(this.bank_acton, "bank_acton");
			this.bank_acton.Name = "bank_acton";
			this.bank_acton.Resizable = DataGridViewTriState.False;
			this.bank_actoff.DataPropertyName = "bank_actoff";
			componentResourceManager.ApplyResources(this.bank_actoff, "bank_actoff");
			this.bank_actoff.Name = "bank_actoff";
			this.bank_actoff.Resizable = DataGridViewTriState.False;
			this.bank_reboot.DataPropertyName = "bank_reboot";
			this.bank_reboot.FalseValue = "0";
			componentResourceManager.ApplyResources(this.bank_reboot, "bank_reboot");
			this.bank_reboot.IndeterminateValue = "";
			this.bank_reboot.Name = "bank_reboot";
			this.bank_reboot.Resizable = DataGridViewTriState.False;
			this.bank_reboot.TrueValue = "1";
			this.bankvoltage.DataPropertyName = "voltage_values";
			componentResourceManager.ApplyResources(this.bankvoltage, "bankvoltage");
			this.bankvoltage.Name = "bankvoltage";
			this.bankvoltage.Resizable = DataGridViewTriState.False;
			this.bankvoltage.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.bankcurrent.DataPropertyName = "current_values";
			componentResourceManager.ApplyResources(this.bankcurrent, "bankcurrent");
			this.bankcurrent.Name = "bankcurrent";
			this.bankcurrent.Resizable = DataGridViewTriState.False;
			this.bankcurrent.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.bankpower.DataPropertyName = "power_values";
			componentResourceManager.ApplyResources(this.bankpower, "bankpower");
			this.bankpower.Name = "bankpower";
			this.bankpower.Resizable = DataGridViewTriState.False;
			this.bankpower.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.bankpowerD.DataPropertyName = "power_consumptions";
			componentResourceManager.ApplyResources(this.bankpowerD, "bankpowerD");
			this.bankpowerD.Name = "bankpowerD";
			this.bankpowerD.Resizable = DataGridViewTriState.False;
			this.bankpowerD.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.labIp, "labIp");
			this.labIp.ForeColor = Color.Blue;
			this.labIp.Name = "labIp";
			componentResourceManager.ApplyResources(this.labBankStatus, "labBankStatus");
			this.labBankStatus.ForeColor = Color.Blue;
			this.labBankStatus.Name = "labBankStatus";
			componentResourceManager.ApplyResources(this.cbDReboot, "cbDReboot");
			this.cbDReboot.Name = "cbDReboot";
			this.cbDReboot.UseVisualStyleBackColor = true;
			this.butOff.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butOff, "butOff");
			this.butOff.Name = "butOff";
			this.butOff.UseVisualStyleBackColor = false;
			this.butOff.Click += new System.EventHandler(this.butOff_Click);
			this.butOn.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butOn, "butOn");
			this.butOn.Name = "butOn";
			this.butOn.UseVisualStyleBackColor = false;
			this.butOn.Click += new System.EventHandler(this.butOn_Click);
			componentResourceManager.ApplyResources(this.labDevStatus, "labDevStatus");
			this.labDevStatus.ForeColor = Color.Blue;
			this.labDevStatus.Name = "labDevStatus";
			componentResourceManager.ApplyResources(this.labSensorStatus, "labSensorStatus");
			this.labSensorStatus.ForeColor = Color.Blue;
			this.labSensorStatus.Name = "labSensorStatus";
			componentResourceManager.ApplyResources(this.labOutletStatus, "labOutletStatus");
			this.labOutletStatus.ForeColor = Color.Blue;
			this.labOutletStatus.Name = "labOutletStatus";
			this.dgvDeviceSensorInfo.AllowUserToAddRows = false;
			this.dgvDeviceSensorInfo.AllowUserToDeleteRows = false;
			this.dgvDeviceSensorInfo.AllowUserToResizeColumns = false;
			this.dgvDeviceSensorInfo.AllowUserToResizeRows = false;
			this.dgvDeviceSensorInfo.BackgroundColor = Color.White;
			this.dgvDeviceSensorInfo.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvDeviceSensorInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.dgvDeviceSensorInfo, "dgvDeviceSensorInfo");
			this.dgvDeviceSensorInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDeviceSensorInfo.Columns.AddRange(new DataGridViewColumn[]
			{
				this.sensor_type,
				this.humiditys,
				this.temperatures,
				this.press_values
			});
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = Color.White;
			dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle4.ForeColor = Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
			this.dgvDeviceSensorInfo.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvDeviceSensorInfo.GridColor = Color.White;
			this.dgvDeviceSensorInfo.MultiSelect = false;
			this.dgvDeviceSensorInfo.Name = "dgvDeviceSensorInfo";
			this.dgvDeviceSensorInfo.RowHeadersVisible = false;
			this.dgvDeviceSensorInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvDeviceSensorInfo.RowTemplate.Height = 23;
			this.dgvDeviceSensorInfo.RowTemplate.ReadOnly = true;
			this.dgvDeviceSensorInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvDeviceSensorInfo.StandardTab = true;
			this.dgvDeviceSensorInfo.TabStop = false;
			this.sensor_type.DataPropertyName = "sensor_type";
			componentResourceManager.ApplyResources(this.sensor_type, "sensor_type");
			this.sensor_type.Name = "sensor_type";
			this.sensor_type.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.humiditys.DataPropertyName = "humiditys";
			componentResourceManager.ApplyResources(this.humiditys, "humiditys");
			this.humiditys.Name = "humiditys";
			this.humiditys.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.temperatures.DataPropertyName = "temperatures";
			componentResourceManager.ApplyResources(this.temperatures, "temperatures");
			this.temperatures.Name = "temperatures";
			this.temperatures.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.press_values.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.press_values.DataPropertyName = "press_values";
			componentResourceManager.ApplyResources(this.press_values, "press_values");
			this.press_values.Name = "press_values";
			this.press_values.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dgvDeviceInfo.AllowUserToAddRows = false;
			this.dgvDeviceInfo.AllowUserToDeleteRows = false;
			this.dgvDeviceInfo.AllowUserToResizeColumns = false;
			this.dgvDeviceInfo.AllowUserToResizeRows = false;
			this.dgvDeviceInfo.BackgroundColor = Color.White;
			this.dgvDeviceInfo.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.BackColor = SystemColors.Control;
			dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
			this.dgvDeviceInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dgvDeviceInfo, "dgvDeviceInfo");
			this.dgvDeviceInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvDeviceInfo.Columns.AddRange(new DataGridViewColumn[]
			{
				this.deviceNm,
				this.dVoltage,
				this.dCurrent,
				this.dPower,
				this.dPowerDiss
			});
			dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = Color.White;
			dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle6.ForeColor = Color.Black;
			dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
			this.dgvDeviceInfo.DefaultCellStyle = dataGridViewCellStyle6;
			this.dgvDeviceInfo.GridColor = Color.White;
			this.dgvDeviceInfo.MultiSelect = false;
			this.dgvDeviceInfo.Name = "dgvDeviceInfo";
			this.dgvDeviceInfo.RowHeadersVisible = false;
			this.dgvDeviceInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvDeviceInfo.RowTemplate.Height = 23;
			this.dgvDeviceInfo.RowTemplate.ReadOnly = true;
			this.dgvDeviceInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvDeviceInfo.StandardTab = true;
			this.dgvDeviceInfo.TabStop = false;
			this.deviceNm.DataPropertyName = "device_nm";
			componentResourceManager.ApplyResources(this.deviceNm, "deviceNm");
			this.deviceNm.Name = "deviceNm";
			this.deviceNm.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dVoltage.DataPropertyName = "voltage_values";
			componentResourceManager.ApplyResources(this.dVoltage, "dVoltage");
			this.dVoltage.Name = "dVoltage";
			this.dVoltage.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dCurrent.DataPropertyName = "current_values";
			componentResourceManager.ApplyResources(this.dCurrent, "dCurrent");
			this.dCurrent.Name = "dCurrent";
			this.dCurrent.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dPower.DataPropertyName = "power_values";
			componentResourceManager.ApplyResources(this.dPower, "dPower");
			this.dPower.Name = "dPower";
			this.dPower.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dPowerDiss.DataPropertyName = "power_consumptions";
			componentResourceManager.ApplyResources(this.dPowerDiss, "dPowerDiss");
			this.dPowerDiss.Name = "dPowerDiss";
			this.dPowerDiss.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dgvOutletInfo.AllowUserToAddRows = false;
			this.dgvOutletInfo.AllowUserToDeleteRows = false;
			this.dgvOutletInfo.AllowUserToResizeColumns = false;
			this.dgvOutletInfo.AllowUserToResizeRows = false;
			this.dgvOutletInfo.BackgroundColor = Color.White;
			dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle7.BackColor = SystemColors.Control;
			dataGridViewCellStyle7.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
			this.dgvOutletInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			componentResourceManager.ApplyResources(this.dgvOutletInfo, "dgvOutletInfo");
			this.dgvOutletInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvOutletInfo.Columns.AddRange(new DataGridViewColumn[]
			{
				this.port_id,
				this.port_number,
				this.port_nm,
				this.portStatus,
				this.act,
				this.reboot,
				this.portVoltage,
				this.portCurrent,
				this.portPower,
				this.portPowerDiss
			});
			dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = Color.White;
			dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle8.ForeColor = Color.Black;
			dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
			this.dgvOutletInfo.DefaultCellStyle = dataGridViewCellStyle8;
			this.dgvOutletInfo.MultiSelect = false;
			this.dgvOutletInfo.Name = "dgvOutletInfo";
			this.dgvOutletInfo.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvOutletInfo.RowHeadersVisible = false;
			this.dgvOutletInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvOutletInfo.RowTemplate.Height = 23;
			this.dgvOutletInfo.RowTemplate.ReadOnly = true;
			this.dgvOutletInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvOutletInfo.StandardTab = true;
			this.dgvOutletInfo.TabStop = false;
			this.dgvOutletInfo.CellContentClick += new DataGridViewCellEventHandler(this.dgvOutletInfo_CellContentClick);
			this.port_id.DataPropertyName = "port_id";
			componentResourceManager.ApplyResources(this.port_id, "port_id");
			this.port_id.Name = "port_id";
			this.port_number.DataPropertyName = "port_number";
			componentResourceManager.ApplyResources(this.port_number, "port_number");
			this.port_number.Name = "port_number";
			this.port_number.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.port_nm.DataPropertyName = "port_nm";
			componentResourceManager.ApplyResources(this.port_nm, "port_nm");
			this.port_nm.Name = "port_nm";
			this.port_nm.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.portStatus.DataPropertyName = "port_state";
			componentResourceManager.ApplyResources(this.portStatus, "portStatus");
			this.portStatus.Name = "portStatus";
			this.portStatus.Resizable = DataGridViewTriState.False;
			this.portStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.act.DataPropertyName = "act";
			componentResourceManager.ApplyResources(this.act, "act");
			this.act.Name = "act";
			this.act.Resizable = DataGridViewTriState.False;
			this.reboot.DataPropertyName = "reboot";
			this.reboot.FalseValue = "0";
			componentResourceManager.ApplyResources(this.reboot, "reboot");
			this.reboot.IndeterminateValue = "0";
			this.reboot.Name = "reboot";
			this.reboot.Resizable = DataGridViewTriState.False;
			this.reboot.TrueValue = "1";
			this.portVoltage.DataPropertyName = "voltage_values";
			componentResourceManager.ApplyResources(this.portVoltage, "portVoltage");
			this.portVoltage.Name = "portVoltage";
			this.portVoltage.Resizable = DataGridViewTriState.False;
			this.portVoltage.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.portCurrent.DataPropertyName = "current_values";
			componentResourceManager.ApplyResources(this.portCurrent, "portCurrent");
			this.portCurrent.Name = "portCurrent";
			this.portCurrent.Resizable = DataGridViewTriState.False;
			this.portCurrent.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.portPower.DataPropertyName = "power_values";
			componentResourceManager.ApplyResources(this.portPower, "portPower");
			this.portPower.Name = "portPower";
			this.portPower.Resizable = DataGridViewTriState.False;
			this.portPower.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.portPowerDiss.DataPropertyName = "power_consumptions";
			componentResourceManager.ApplyResources(this.portPowerDiss, "portPowerDiss");
			this.portPowerDiss.Name = "portPowerDiss";
			this.portPowerDiss.Resizable = DataGridViewTriState.False;
			this.portPowerDiss.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.lbLeakStatus, "lbLeakStatus");
			this.lbLeakStatus.ForeColor = Color.Black;
			this.lbLeakStatus.Name = "lbLeakStatus";
			componentResourceManager.ApplyResources(this.lbLineStatus, "lbLineStatus");
			this.lbLineStatus.ForeColor = Color.Blue;
			this.lbLineStatus.Name = "lbLineStatus";
			this.dgvLineInfo.AllowUserToAddRows = false;
			this.dgvLineInfo.AllowUserToDeleteRows = false;
			this.dgvLineInfo.AllowUserToResizeColumns = false;
			this.dgvLineInfo.AllowUserToResizeRows = false;
			this.dgvLineInfo.BackgroundColor = Color.White;
			this.dgvLineInfo.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle9.BackColor = SystemColors.Control;
			dataGridViewCellStyle9.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
			this.dgvLineInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
			componentResourceManager.ApplyResources(this.dgvLineInfo, "dgvLineInfo");
			this.dgvLineInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvLineInfo.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dataGridViewTextBoxColumn25,
				this.dataGridViewTextBoxColumn26,
				this.dataGridViewTextBoxColumn27,
				this.dataGridViewTextBoxColumn28
			});
			dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = Color.White;
			dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle10.ForeColor = Color.Black;
			dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
			this.dgvLineInfo.DefaultCellStyle = dataGridViewCellStyle10;
			this.dgvLineInfo.GridColor = Color.White;
			this.dgvLineInfo.MultiSelect = false;
			this.dgvLineInfo.Name = "dgvLineInfo";
			this.dgvLineInfo.RowHeadersVisible = false;
			this.dgvLineInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvLineInfo.RowTemplate.Height = 23;
			this.dgvLineInfo.RowTemplate.ReadOnly = true;
			this.dgvLineInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvLineInfo.StandardTab = true;
			this.dgvLineInfo.TabStop = false;
			this.dataGridViewTextBoxColumn25.DataPropertyName = "line_number";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn25, "dataGridViewTextBoxColumn25");
			this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
			this.dataGridViewTextBoxColumn25.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn26.DataPropertyName = "voltage_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn26, "dataGridViewTextBoxColumn26");
			this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
			this.dataGridViewTextBoxColumn26.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn27.DataPropertyName = "current_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn27, "dataGridViewTextBoxColumn27");
			this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
			this.dataGridViewTextBoxColumn27.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn28.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn28.DataPropertyName = "power_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn28, "dataGridViewTextBoxColumn28");
			this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
			this.dataGridViewTextBoxColumn28.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn1.DataPropertyName = "port_id";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn2.DataPropertyName = "port_number";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn3.DataPropertyName = "port_nm";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn4.DataPropertyName = "port_state";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewDisableButtonColumn1.DataPropertyName = "act";
			componentResourceManager.ApplyResources(this.dataGridViewDisableButtonColumn1, "dataGridViewDisableButtonColumn1");
			this.dataGridViewDisableButtonColumn1.Name = "dataGridViewDisableButtonColumn1";
			this.dataGridViewDisableButtonColumn1.Resizable = DataGridViewTriState.False;
			this.dataGridViewDisableCheckBoxColumn1.DataPropertyName = "reboot";
			this.dataGridViewDisableCheckBoxColumn1.FalseValue = "0";
			componentResourceManager.ApplyResources(this.dataGridViewDisableCheckBoxColumn1, "dataGridViewDisableCheckBoxColumn1");
			this.dataGridViewDisableCheckBoxColumn1.IndeterminateValue = "0";
			this.dataGridViewDisableCheckBoxColumn1.Name = "dataGridViewDisableCheckBoxColumn1";
			this.dataGridViewDisableCheckBoxColumn1.Resizable = DataGridViewTriState.False;
			this.dataGridViewDisableCheckBoxColumn1.TrueValue = "1";
			this.dataGridViewTextBoxColumn5.DataPropertyName = "voltage_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn6.DataPropertyName = "current_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn7.DataPropertyName = "power_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn7.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn8.DataPropertyName = "power_consumptions";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn8.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn9.DataPropertyName = "device_nm";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn10.DataPropertyName = "voltage_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn11.DataPropertyName = "current_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn12.DataPropertyName = "power_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			this.dataGridViewTextBoxColumn12.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn13.DataPropertyName = "power_consumptions";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn14.DataPropertyName = "sensor_type";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			this.dataGridViewTextBoxColumn14.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn15.DataPropertyName = "humiditys";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
			this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			this.dataGridViewTextBoxColumn15.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn16.DataPropertyName = "temperatures";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			this.dataGridViewTextBoxColumn16.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn17.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn17.DataPropertyName = "press_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn18.DataPropertyName = "bank_id";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn18, "dataGridViewTextBoxColumn18");
			this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
			this.dataGridViewTextBoxColumn19.DataPropertyName = "bank_number";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn19, "dataGridViewTextBoxColumn19");
			this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
			this.dataGridViewTextBoxColumn19.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn20.DataPropertyName = "bank_nm";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn20, "dataGridViewTextBoxColumn20");
			this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
			this.dataGridViewTextBoxColumn20.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewDisableButtonColumn2.DataPropertyName = "bank_acton";
			componentResourceManager.ApplyResources(this.dataGridViewDisableButtonColumn2, "dataGridViewDisableButtonColumn2");
			this.dataGridViewDisableButtonColumn2.Name = "dataGridViewDisableButtonColumn2";
			this.dataGridViewDisableButtonColumn2.Resizable = DataGridViewTriState.False;
			this.dataGridViewDisableButtonColumn3.DataPropertyName = "bank_actoff";
			componentResourceManager.ApplyResources(this.dataGridViewDisableButtonColumn3, "dataGridViewDisableButtonColumn3");
			this.dataGridViewDisableButtonColumn3.Name = "dataGridViewDisableButtonColumn3";
			this.dataGridViewDisableButtonColumn3.Resizable = DataGridViewTriState.False;
			this.dataGridViewDisableCheckBoxColumn2.DataPropertyName = "bank_reboot";
			this.dataGridViewDisableCheckBoxColumn2.FalseValue = "0";
			componentResourceManager.ApplyResources(this.dataGridViewDisableCheckBoxColumn2, "dataGridViewDisableCheckBoxColumn2");
			this.dataGridViewDisableCheckBoxColumn2.IndeterminateValue = "";
			this.dataGridViewDisableCheckBoxColumn2.Name = "dataGridViewDisableCheckBoxColumn2";
			this.dataGridViewDisableCheckBoxColumn2.Resizable = DataGridViewTriState.False;
			this.dataGridViewDisableCheckBoxColumn2.TrueValue = "1";
			this.dataGridViewTextBoxColumn21.DataPropertyName = "voltage_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn21, "dataGridViewTextBoxColumn21");
			this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
			this.dataGridViewTextBoxColumn21.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn21.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn22.DataPropertyName = "current_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn22, "dataGridViewTextBoxColumn22");
			this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
			this.dataGridViewTextBoxColumn22.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn22.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn23.DataPropertyName = "power_values";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn23, "dataGridViewTextBoxColumn23");
			this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
			this.dataGridViewTextBoxColumn23.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn23.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn24.DataPropertyName = "power_consumptions";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn24, "dataGridViewTextBoxColumn24");
			this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
			this.dataGridViewTextBoxColumn24.Resizable = DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn24.SortMode = DataGridViewColumnSortMode.NotSortable;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.lbLeakStatus);
			base.Controls.Add(this.lbLineStatus);
			base.Controls.Add(this.dgvLineInfo);
			base.Controls.Add(this.dgvBankInfo);
			base.Controls.Add(this.labIp);
			base.Controls.Add(this.labBankStatus);
			base.Controls.Add(this.cbDReboot);
			base.Controls.Add(this.butOff);
			base.Controls.Add(this.butOn);
			base.Controls.Add(this.labDevStatus);
			base.Controls.Add(this.labSensorStatus);
			base.Controls.Add(this.labOutletStatus);
			base.Controls.Add(this.dgvDeviceSensorInfo);
			base.Controls.Add(this.dgvDeviceInfo);
			base.Controls.Add(this.dgvOutletInfo);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DataGpOPDev";
			((ISupportInitialize)this.dgvBankInfo).EndInit();
			((ISupportInitialize)this.dgvDeviceSensorInfo).EndInit();
			((ISupportInitialize)this.dgvDeviceInfo).EndInit();
			((ISupportInitialize)this.dgvOutletInfo).EndInit();
			((ISupportInitialize)this.dgvLineInfo).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
