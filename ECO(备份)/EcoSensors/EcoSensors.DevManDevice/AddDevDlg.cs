using CommonAPI.Global;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.Thread;
using EcoSensors.Properties;
using EventLogAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice
{
	public class AddDevDlg : Form
	{
		private const string DTbCol1_datasel = "datasel";
		private const string DTbCol2_devname = "dgvtbPduNm";
		private const string DTbCol3_MAC = "dgvtbMac";
		private const string DTbCol4_IP = "dgvtbIp";
		private const string DTbCol5_Model = "dgvtbModel";
		private const string DTbCol6_RackNm = "dgvtbRackNm";
		private const string DTbCol7_fwVer = "fwVersion";
		private bool cbsel_changeonly;
		private int m_scanCompany;
		private string m_strIPs;
		private DevSnmpConfig m_SNMPPara;
		private DiscoverThread m_discoverT;
		private System.Collections.Generic.Dictionary<string, string> m_RackMapOrNM2ID = new System.Collections.Generic.Dictionary<string, string>();
		private System.Collections.Generic.Dictionary<string, string> m_RackMapID2OrNm = new System.Collections.Generic.Dictionary<string, string>();
		private IContainer components;
		private CheckBox cbsel;
		private Button butAssignNm;
		private Button butAdd;
		private Button butCancel;
		private DataGridView dgvAutoDevice;
		private PictureBox pictureBoxLoading;
		private DataGridViewCheckBoxColumn datasel;
		private DataGridViewTextBoxColumn dgvtbPduNm;
		private DataGridViewTextBoxColumn dgvtbMac;
		private DataGridViewTextBoxColumn dgvtbIp;
		private DataGridViewTextBoxColumn dgvtbModel;
		private DataGridViewComboBoxColumn dgvtbRackNm;
		private DataGridViewTextBoxColumn fwVersion;
		public AddDevDlg()
		{
			this.InitializeComponent();
		}
		public void pageInit(int scanCompany, string IPs, DevSnmpConfig SNMPPara)
		{
			this.m_scanCompany = scanCompany;
			this.m_strIPs = IPs;
			this.m_SNMPPara = SNMPPara;
			this.cbsel_changeonly = false;
			System.Collections.ArrayList allRack = RackInfo.getAllRack();
			this.dgvtbRackNm.Items.Clear();
			this.dgvtbRackNm.Items.Add("");
			for (int i = 0; i < allRack.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRack[i];
				this.dgvtbRackNm.Items.Add(rackInfo.OriginalName);
				this.m_RackMapOrNM2ID.Add(rackInfo.OriginalName, rackInfo.RackID.ToString());
				this.m_RackMapID2OrNm.Add(rackInfo.RackID.ToString(), rackInfo.OriginalName);
			}
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.setScanResult));
			System.Threading.Thread.Sleep(200);
			this.butCancel.Enabled = true;
		}
		private void setScanResult(object obj)
		{
			try
			{
				Program.IdleTimer_Pause(1);
				ControlAccess.ConfigControl config = delegate(Control control, object param)
				{
					PictureBox pictureBox = control as PictureBox;
					pictureBox.Show();
				};
				ControlAccess controlAccess = new ControlAccess(this, config);
				controlAccess.Access(this.pictureBoxLoading, null);
				this.m_discoverT = new DiscoverThread(this.m_scanCompany, this.m_strIPs, this.m_SNMPPara);
				System.Threading.Thread.Sleep(200);
				System.Collections.Generic.List<string[]> list = this.m_discoverT.excute();
				if (list.Count<string[]>() == 0)
				{
					config = delegate(Control control, object param)
					{
						AddDevDlg addDevDlg = control as AddDevDlg;
						EcoMessageBox.ShowError(addDevDlg, EcoLanguage.getMsg(LangRes.noNewDev, new string[0]));
						addDevDlg.Close();
					};
					controlAccess = new ControlAccess(this, config);
					controlAccess.Access(this, null);
					return;
				}
				config = delegate(Control control, object param)
				{
					AddDevDlg addDevDlg = control as AddDevDlg;
					System.Collections.Generic.List<string[]> list2 = param as System.Collections.Generic.List<string[]>;
					addDevDlg.dgvAutoDevice.Rows.Clear();
					foreach (string[] current in list2)
					{
						string text = current[4];
						if (!this.m_RackMapOrNM2ID.ContainsKey(text))
						{
							text = "";
						}
						addDevDlg.dgvAutoDevice.Rows.Add(new object[]
						{
							true,
							current[0],
							current[1],
							current[2],
							current[3],
							text,
							current[5]
						});
					}
					if (addDevDlg.dgvAutoDevice.Rows.Count > 0)
					{
						addDevDlg.butAdd.Enabled = true;
					}
					else
					{
						addDevDlg.butAdd.Enabled = false;
					}
					addDevDlg.pictureBoxLoading.Hide();
					addDevDlg.butAssignNm.Enabled = true;
					addDevDlg.butCancel.Enabled = true;
					addDevDlg.butAdd.Enabled = true;
				};
				controlAccess = new ControlAccess(this, config);
				controlAccess.Access(this, list);
			}
			catch (System.Exception)
			{
			}
			Program.IdleTimer_Run(1);
		}
		private void AddDevDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.butCancel.Enabled)
			{
				e.Cancel = true;
				return;
			}
			if (this.m_discoverT != null)
			{
				this.m_discoverT.threadAbort();
			}
		}
		private void butAssignNm_Click(object sender, System.EventArgs e)
		{
			if (this.dgvAutoDevice.Rows.Count == 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < this.dgvAutoDevice.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvAutoDevice.Rows[i];
				if (System.Convert.ToBoolean(dataGridViewRow.Cells["datasel"].Value))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_needselect, new string[0]));
				return;
			}
			AssignNameParaDlg assignNameParaDlg = new AssignNameParaDlg();
			assignNameParaDlg.ShowDialog(this);
			int assignPara = assignNameParaDlg.AssignPara;
			if (assignPara == -1)
			{
				return;
			}
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				DataGridView dataGridView = control as DataGridView;
				string value = param as string;
				int num = System.Convert.ToInt32(value);
				DataGridViewRowCollection rows = dataGridView.Rows;
				for (int j = 0; j < rows.Count; j++)
				{
					DataGridViewRow dataGridViewRow2 = rows[j];
					if (System.Convert.ToBoolean(dataGridViewRow2.Cells["datasel"].Value))
					{
						string text = dataGridViewRow2.Cells["dgvtbModel"].Value.ToString();
						switch (num)
						{
						case 1:
						{
							int num2 = 1;
							string text2 = "";
							try
							{
								text2 = dataGridViewRow2.Cells["dgvtbRackNm"].Value.ToString();
							}
							catch
							{
							}
							string text3 = string.Concat(new string[]
							{
								text,
								"_",
								text2,
								"_",
								num2++.ToString()
							});
							while (!this.pduNmCheckAtControl(text3, j, rows) || !this.pduNmCheckAtDb(text3))
							{
								text3 = string.Concat(new string[]
								{
									text,
									"_",
									text2,
									"_",
									num2++.ToString()
								});
							}
							dataGridViewRow2.Cells["dgvtbPduNm"].Value = text3;
							break;
						}
						case 2:
						{
							int num3 = 1;
							string text3 = text + "_" + num3++.ToString();
							while (!this.pduNmCheckAtControl(text3, j, rows) || !this.pduNmCheckAtDb(text3))
							{
								text3 = text + "_" + num3++.ToString();
							}
							dataGridViewRow2.Cells["dgvtbPduNm"].Value = text3;
							break;
						}
						}
					}
				}
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this.dgvAutoDevice, assignPara.ToString());
		}
		private bool pduNmCheckAtControl(string name, int rowCount, DataGridViewRowCollection dgvrc)
		{
			for (int i = 0; i < rowCount; i++)
			{
				DataGridViewRow dataGridViewRow = dgvrc[i];
				if (System.Convert.ToBoolean(dataGridViewRow.Cells["datasel"].Value))
				{
					string value = dataGridViewRow.Cells["dgvtbPduNm"].Value.ToString();
					if (name.Equals(value))
					{
						return false;
					}
				}
			}
			return true;
		}
		private bool pduNmCheckAtDb(string devname)
		{
			DeviceInfo deviceByName = DeviceOperation.getDeviceByName(devname);
			return deviceByName == null;
		}
		private void cbsel_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbsel_changeonly)
			{
				this.cbsel_changeonly = false;
				return;
			}
			if (this.cbsel.Checked)
			{
				for (int i = 0; i < this.dgvAutoDevice.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dgvAutoDevice.Rows.SharedRow(i).Cells;
					cells[0].Value = true;
				}
				return;
			}
			for (int j = 0; j < this.dgvAutoDevice.Rows.Count; j++)
			{
				DataGridViewCellCollection cells2 = this.dgvAutoDevice.Rows.SharedRow(j).Cells;
				cells2[0].Value = false;
			}
		}
		private void butAdd_Click(object sender, System.EventArgs e)
		{
			if (this.dgvAutoDevice.Rows.Count == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_needselect, new string[0]));
				return;
			}
			bool flag = true;
			bool flag2 = true;
			int num = 0;
			for (int i = 0; i < this.dgvAutoDevice.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvAutoDevice.Rows[i];
				DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
				if (System.Convert.ToBoolean(dataGridViewRow.Cells["datasel"].Value))
				{
					if (System.Convert.ToString(dataGridViewRow.Cells["dgvtbPduNm"].Value).Equals(string.Empty))
					{
						dataGridViewCellStyle.BackColor = Color.Red;
						dataGridViewRow.Cells["dgvtbPduNm"].Style = dataGridViewCellStyle;
						this.dgvAutoDevice.CurrentCell = dataGridViewRow.Cells["dgvtbPduNm"];
						this.dgvAutoDevice.BeginEdit(true);
						flag = false;
					}
					else
					{
						string text = dataGridViewRow.Cells["dgvtbPduNm"].Value.ToString();
						text = text.Trim();
						dataGridViewRow.Cells["dgvtbPduNm"].Value = text;
						if (!Ecovalidate.ValidDevName(text) || !this.pduNmCheckAtDb(text) || !this.pduNmCheckAtControl(text, i, this.dgvAutoDevice.Rows))
						{
							dataGridViewCellStyle.BackColor = Color.Red;
							dataGridViewRow.Cells["dgvtbPduNm"].Style = dataGridViewCellStyle;
							this.dgvAutoDevice.CurrentCell = dataGridViewRow.Cells["dgvtbPduNm"];
							this.dgvAutoDevice.BeginEdit(true);
							flag = false;
						}
					}
					if (dataGridViewRow.Cells["dgvtbRackNm"].Value == null || dataGridViewRow.Cells["dgvtbRackNm"].Value.ToString().Equals(string.Empty))
					{
						dataGridViewCellStyle.BackColor = Color.Red;
						dataGridViewRow.Cells["dgvtbRackNm"].Style = dataGridViewCellStyle;
						flag2 = false;
					}
					num++;
				}
			}
			if (num == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_needselect, new string[0]));
				return;
			}
			System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			if (num + allDevice.Count > EcoGlobalVar.gl_maxDevNum)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_MaxNum, new string[]
				{
					EcoGlobalVar.gl_maxDevNum.ToString()
				}));
				return;
			}
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_nameErr, new string[0]));
				return;
			}
			if (!flag2)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_needRack, new string[0]));
				return;
			}
			for (int j = 0; j < this.dgvAutoDevice.Rows.Count; j++)
			{
				DataGridViewRow dataGridViewRow2 = this.dgvAutoDevice.Rows[j];
				DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
				if (System.Convert.ToBoolean(dataGridViewRow2.Cells["datasel"].Value))
				{
					string text2 = dataGridViewRow2.Cells["dgvtbRackNm"].Value.ToString();
					System.Collections.Generic.List<int> deviceIDByOriginalName = RackInfo.GetDeviceIDByOriginalName(text2);
					if (deviceIDByOriginalName.Count >= 10)
					{
						dataGridViewCellStyle2.BackColor = Color.Red;
						dataGridViewRow2.Cells["dgvtbRackNm"].Style = dataGridViewCellStyle2;
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_RackFull, new string[]
						{
							text2
						}));
						return;
					}
					int num2 = deviceIDByOriginalName.Count;
					for (int k = 0; k < this.dgvAutoDevice.Rows.Count; k++)
					{
						DataGridViewRow dataGridViewRow3 = this.dgvAutoDevice.Rows[k];
						if (System.Convert.ToBoolean(dataGridViewRow3.Cells["datasel"].Value))
						{
							string value = dataGridViewRow3.Cells["dgvtbRackNm"].Value.ToString();
							if (text2.Equals(value))
							{
								num2++;
								if (num2 > 10)
								{
									EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_RackFull, new string[]
									{
										text2
									}));
									return;
								}
							}
						}
					}
				}
			}
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			for (int l = 0; l < this.dgvAutoDevice.Rows.Count; l++)
			{
				DataGridViewRow dataGridViewRow4 = this.dgvAutoDevice.Rows[l];
				if (System.Convert.ToBoolean(dataGridViewRow4.Cells["datasel"].Value))
				{
					string value2 = dataGridViewRow4.Cells["dgvtbIp"].Value.ToString();
					string value3 = dataGridViewRow4.Cells["dgvtbPduNm"].Value.ToString();
					string value4 = dataGridViewRow4.Cells["dgvtbMac"].Value.ToString();
					string value5 = dataGridViewRow4.Cells["dgvtbModel"].Value.ToString();
					string value6 = dataGridViewRow4.Cells["dgvtbRackNm"].Value.ToString();
					string value7 = dataGridViewRow4.Cells["fwVersion"].Value.ToString();
					System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
					hashtable["ip"] = value2;
					hashtable["pduNm"] = value3;
					hashtable["mac"] = value4;
					hashtable["modelNm"] = value5;
					hashtable["rackorNM"] = value6;
					hashtable["fwVersion"] = value7;
					list.Add(hashtable);
				}
			}
			this.butAdd.Enabled = false;
			this.butAssignNm.Enabled = false;
			this.butCancel.Enabled = false;
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.addDeviceProc), list);
		}
		private void addDeviceProc(object obj)
		{
			Program.IdleTimer_Pause(1);
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				PictureBox pictureBox = control as PictureBox;
				pictureBox.Show();
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this.pictureBoxLoading, null);
			System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
			System.Collections.Generic.List<System.Collections.Hashtable> list = obj as System.Collections.Generic.List<System.Collections.Hashtable>;
			System.Collections.Generic.List<DevSnmpConfig> list2 = this.boardDataSave(list);
			if (list2 != null && list2.Count > 0)
			{
				this.updateTrapReceiver(list2);
				System.Collections.ArrayList allRack_NoEmpty2 = RackInfo.GetAllRack_NoEmpty();
				EcoGlobalVar.gl_DevManPage.FlushFlg_RackBoard = 1;
				EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
				if (allRack_NoEmpty.Count == allRack_NoEmpty2.Count)
				{
					EcoGlobalVar.setDashBoardFlg(526uL, "", 66);
				}
				else
				{
					EcoGlobalVar.setDashBoardFlg(526uL, "", 65);
				}
				if (list2.Count == list.Count)
				{
					config = delegate(Control control, object param)
					{
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
					};
					controlAccess = new ControlAccess(this, config);
					controlAccess.Access(this, null);
				}
				else
				{
					config = delegate(Control control, object param)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
					};
					controlAccess = new ControlAccess(this, config);
					controlAccess.Access(this, null);
				}
			}
			config = delegate(Control control, object param)
			{
				AddDevDlg addDevDlg = control as AddDevDlg;
				addDevDlg.butCancel.Enabled = true;
				addDevDlg.DialogResult = DialogResult.OK;
				addDevDlg.Close();
			};
			controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this, null);
			Program.IdleTimer_Run(1);
		}
		private void updateTrapReceiver(System.Collections.Generic.List<DevSnmpConfig> devsnmpConfigs)
		{
			try
			{
				Sys_Para pSys = new Sys_Para();
				DevMonitorAPI devMonitorAPI = new DevMonitorAPI();
				devMonitorAPI.SetTrapReceiver(devsnmpConfigs, pSys);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private System.Collections.Generic.List<DevSnmpConfig> boardDataSave(System.Collections.Generic.List<System.Collections.Hashtable> list)
		{
			System.Collections.Generic.List<DevSnmpConfig> list2 = new System.Collections.Generic.List<DevSnmpConfig>();
			System.Collections.Generic.List<DevSnmpConfig> list3 = new System.Collections.Generic.List<DevSnmpConfig>();
			System.Collections.Generic.List<DevSnmpConfig> list4 = new System.Collections.Generic.List<DevSnmpConfig>();
			System.Collections.Generic.List<DevSnmpConfig> list5 = new System.Collections.Generic.List<DevSnmpConfig>();
			System.Collections.Generic.List<DevSnmpConfig> list6 = new System.Collections.Generic.List<DevSnmpConfig>();
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			foreach (System.Collections.Hashtable current in list)
			{
				string devIP = (string)current["ip"];
				string arg_60_0 = (string)current["pduNm"];
				string devMac = (string)current["mac"];
				string text = (string)current["modelNm"];
				string fmwareVer = (string)current["fwVersion"];
				DevSnmpConfig devSnmpConfig = new DevSnmpConfig();
				devSnmpConfig.copyConfig(this.m_SNMPPara);
				devSnmpConfig.devIP = devIP;
				devSnmpConfig.devMac = devMac;
				devSnmpConfig.modelName = text;
				devSnmpConfig.fmwareVer = fmwareVer;
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(text, fmwareVer);
				if (deviceModelConfig.commonThresholdFlag == Constant.EatonPDUThreshold)
				{
					list4.Add(devSnmpConfig);
				}
				else
				{
					if (deviceModelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
					{
						list5.Add(devSnmpConfig);
					}
					else
					{
						if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
						{
							list6.Add(devSnmpConfig);
						}
						else
						{
							list3.Add(devSnmpConfig);
						}
					}
				}
			}
			System.Collections.Generic.List<ThresholdMessage> list7 = new System.Collections.Generic.List<ThresholdMessage>();
			DevMonitorAPI devMonitorAPI = new DevMonitorAPI();
			if (list3.Count > 0)
			{
				list7 = devMonitorAPI.GetMonitorThresholds(list3);
				if (list7 == null || list7.Count == 0)
				{
					ControlAccess.ConfigControl config = delegate(Control control, object param)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ConFail_all, new string[0]));
					};
					ControlAccess controlAccess = new ControlAccess(this, config);
					controlAccess.Access(this, null);
					return null;
				}
				if (list3.Count != list7.Count)
				{
					string text2 = "";
					using (System.Collections.Generic.List<DevSnmpConfig>.Enumerator enumerator2 = list3.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							DevSnmpConfig current2 = enumerator2.Current;
							string devMac2 = current2.devMac;
							bool flag = false;
							foreach (ThresholdMessage current3 in list7)
							{
								string deviceMac = current3.DeviceMac;
								if (devMac2.Equals(deviceMac))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								if (text2.Length == 0)
								{
									text2 = devMac2;
								}
								else
								{
									text2 = text2 + "\r\n" + devMac2;
								}
							}
							else
							{
								list2.Add(current2);
							}
						}
						goto IL_2AF;
					}
				}
				foreach (DevSnmpConfig current4 in list3)
				{
					list2.Add(current4);
				}
			}
			IL_2AF:
			foreach (DevSnmpConfig current5 in list4)
			{
				ThresholdMessage thresholdMessage = new ThresholdMessage();
				thresholdMessage.DeviceReplyMac = current5.devMac;
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current5.modelName, current5.fmwareVer);
				thresholdMessage.ModelName = current5.modelName;
				thresholdMessage.IpAddress = current5.devIP;
				thresholdMessage.CreateTime = System.DateTime.Now;
				thresholdMessage.PortNums = deviceModelConfig.portNum;
				thresholdMessage.SensorNums = deviceModelConfig.sensorNum;
				thresholdMessage.BankNums = devMonitorAPI.GetEatonPDUBankNumber(current5);
				thresholdMessage.PerPortReading = deviceModelConfig.perportreading;
				thresholdMessage.Switchable = deviceModelConfig.switchable;
				thresholdMessage.PerBankReading = deviceModelConfig.perbankReading;
				thresholdMessage.PerDoorReading = deviceModelConfig.doorReading;
				thresholdMessage.DeviceMac = current5.devMac;
				thresholdMessage.DeviceID = current5.devID;
				if (thresholdMessage.BankNums > 0)
				{
					System.Collections.Generic.Dictionary<int, BankThreshold> dictionary = new System.Collections.Generic.Dictionary<int, BankThreshold>();
					for (int i = 1; i < thresholdMessage.BankNums + 1; i++)
					{
						BankThreshold bankThreshold = new BankThreshold(i);
						bankThreshold.BankName = i.ToString();
						dictionary.Add(i, bankThreshold);
					}
					thresholdMessage.BankThreshold = dictionary;
				}
				list7.Add(thresholdMessage);
				list2.Add(current5);
			}
			if (list5.Count > 0)
			{
				list7 = devMonitorAPI.GetThresholdsEatonPDU_M2(list5);
				if (list7 == null || list7.Count == 0)
				{
					ControlAccess.ConfigControl config2 = delegate(Control control, object param)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ConFail_all, new string[0]));
					};
					ControlAccess controlAccess2 = new ControlAccess(this, config2);
					controlAccess2.Access(this, null);
					return null;
				}
				if (list5.Count != list7.Count)
				{
					string text3 = "";
					using (System.Collections.Generic.List<DevSnmpConfig>.Enumerator enumerator6 = list5.GetEnumerator())
					{
						while (enumerator6.MoveNext())
						{
							DevSnmpConfig current6 = enumerator6.Current;
							string devMac3 = current6.devMac;
							bool flag2 = false;
							foreach (ThresholdMessage current7 in list7)
							{
								string deviceMac2 = current7.DeviceMac;
								if (devMac3.Equals(deviceMac2))
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								if (text3.Length == 0)
								{
									text3 = devMac3;
								}
								else
								{
									text3 = text3 + "\r\n" + devMac3;
								}
							}
							else
							{
								list2.Add(current6);
							}
						}
						goto IL_56F;
					}
				}
				foreach (DevSnmpConfig current8 in list5)
				{
					list2.Add(current8);
				}
			}
			IL_56F:
			if (list6.Count > 0)
			{
				list7 = devMonitorAPI.GetThresholdsApcPDU(list6);
				if (list7 == null || list7.Count == 0)
				{
					ControlAccess.ConfigControl config3 = delegate(Control control, object param)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ConFail_all, new string[0]));
					};
					ControlAccess controlAccess3 = new ControlAccess(this, config3);
					controlAccess3.Access(this, null);
					return null;
				}
				if (list6.Count != list7.Count)
				{
					string text4 = "";
					using (System.Collections.Generic.List<DevSnmpConfig>.Enumerator enumerator2 = list6.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							DevSnmpConfig current9 = enumerator2.Current;
							string devMac4 = current9.devMac;
							bool flag3 = false;
							foreach (ThresholdMessage current10 in list7)
							{
								string deviceMac3 = current10.DeviceMac;
								if (devMac4.Equals(deviceMac3))
								{
									flag3 = true;
									break;
								}
							}
							if (!flag3)
							{
								if (text4.Length == 0)
								{
									text4 = devMac4;
								}
								else
								{
									text4 = text4 + "\r\n" + devMac4;
								}
							}
							else
							{
								list2.Add(current9);
							}
						}
						goto IL_6C9;
					}
				}
				foreach (DevSnmpConfig current11 in list6)
				{
					list2.Add(current11);
				}
			}
			IL_6C9:
			System.Collections.Generic.List<DevSnmpConfig> list8 = this.addBaseDevice(list, list2, list7);
			foreach (DevSnmpConfig current12 in list8)
			{
				string devName = "";
				string text5 = "";
				foreach (System.Collections.Hashtable current13 in list)
				{
					if (current13["mac"].ToString().Equals(current12.devMac))
					{
						devName = current13["pduNm"].ToString();
						text5 = current13["rackorNM"].ToString();
						string arg_75F_0 = this.m_RackMapOrNM2ID[text5];
						break;
					}
				}
				if (instance.getDeviceModelConfig(current12.modelName, current12.fmwareVer).commonThresholdFlag != Constant.EatonPDUThreshold)
				{
					DevAccessAPI devAccessAPI = new DevAccessAPI(current12);
					devAccessAPI.SetDeviceName(devName, "");
					devAccessAPI.SetRackName(text5, "");
				}
			}
			return list8;
		}
		private System.Collections.Generic.List<DevSnmpConfig> addBaseDevice(System.Collections.Generic.List<System.Collections.Hashtable> list, System.Collections.Generic.List<DevSnmpConfig> allconfigs, System.Collections.Generic.List<ThresholdMessage> thresholdMessageList)
		{
			System.Collections.Generic.List<DevSnmpConfig> list2 = new System.Collections.Generic.List<DevSnmpConfig>();
			DBConn connection = DBConnPool.getConnection();
			int i_voltage = (int)Sys_Para.GetEnergyValue();
			foreach (ThresholdMessage current in thresholdMessageList)
			{
				if (current == null)
				{
					System.Console.WriteLine(System.DateTime.Now.ToString() + " addBaseDevice: tm=null");
				}
				else
				{
					DevSnmpConfig devSnmpConfig = null;
					foreach (DevSnmpConfig current2 in allconfigs)
					{
						if (current2.devMac.Equals(current.DeviceMac))
						{
							devSnmpConfig = new DevSnmpConfig();
							devSnmpConfig.copyConfig(current2);
							break;
						}
					}
					string str_name = "";
					string str_name2 = "";
					string str_version = "";
					DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(devSnmpConfig.modelName, devSnmpConfig.fmwareVer);
					foreach (System.Collections.Hashtable current3 in list)
					{
						if (current3["mac"].ToString().Equals(current.DeviceMac))
						{
							str_name = current3["pduNm"].ToString();
							str_name2 = current3["rackorNM"].ToString();
							str_version = current3["fwVersion"].ToString();
							break;
						}
					}
					RackInfo rackByOriginalName = RackInfo.getRackByOriginalName(str_name2);
					int i_snmpver = 1;
					switch (devSnmpConfig.snmpVer)
					{
					case 0:
						i_snmpver = 1;
						break;
					case 1:
						i_snmpver = 2;
						break;
					case 3:
						i_snmpver = 3;
						break;
					}
					int num = devcfgUtil.ThresholdFlg(deviceModelConfig, "dev");
					int num2 = devcfgUtil.ThresholdFlg(deviceModelConfig, "bank");
					int num3 = devcfgUtil.ThresholdFlg(deviceModelConfig, "line");
					int num4 = devcfgUtil.ThresholdFlg(deviceModelConfig, "port");
					DeviceThreshold deviceThreshold = current.DeviceThreshold;
					if (deviceThreshold == null)
					{
						deviceThreshold = new DeviceThreshold();
					}
					if ((num & 1) != 0)
					{
						deviceThreshold.MinCurrentMT = -300f;
					}
					if ((num & 2) != 0)
					{
						deviceThreshold.MaxCurrentMT = devcfgUtil.fmaxCurrent(deviceModelConfig, "dev", 0);
					}
					if ((num & 4) != 0)
					{
						deviceThreshold.MinVoltageMT = devcfgUtil.fMinVoltage(deviceModelConfig, "dev", 0);
						if (devSnmpConfig.modelName.Equals(EcoGlobalVar.EC2004))
						{
							deviceThreshold.MinVoltageMT = -500f;
						}
					}
					if ((num & 8) != 0)
					{
						deviceThreshold.MaxVoltageMT = devcfgUtil.fMaxVoltage(deviceModelConfig, "dev", 0);
						if (devSnmpConfig.modelName.Equals(EcoGlobalVar.EC2004))
						{
							deviceThreshold.MaxVoltageMT = -500f;
						}
					}
					if ((num & 16) != 0)
					{
						deviceThreshold.MinPowerMT = -300f;
					}
					if ((num & 32) != 0)
					{
						deviceThreshold.MaxPowerMT = devcfgUtil.fMaxPower(deviceModelConfig, "dev", 0);
					}
					if ((num & 128) != 0)
					{
						deviceThreshold.MaxPowerDissMT = devcfgUtil.fMaxPowerD(deviceModelConfig, "dev", 0);
					}
					int i_door = 0;
					if (deviceThreshold != null)
					{
						switch (deviceThreshold.DoorSensorType)
						{
						case 0:
							i_door = 0;
							break;
						case 1:
							i_door = 1;
							break;
						case 2:
							i_door = 2;
							break;
						case 3:
							i_door = 3;
							break;
						}
					}
					DeviceInfo deviceInfo = new DeviceInfo(-1, devSnmpConfig.devIP, str_name, current.DeviceMac, devSnmpConfig.privPSW, devSnmpConfig.authPSW, devSnmpConfig.privType, devSnmpConfig.authType, devSnmpConfig.timeout, devSnmpConfig.retry, devSnmpConfig.userName, devSnmpConfig.devPort, i_snmpver, devSnmpConfig.modelName, deviceThreshold.MaxVoltageMT, deviceThreshold.MinVoltageMT, deviceThreshold.MaxPowerDissMT, -300f, deviceThreshold.MaxPowerMT, deviceThreshold.MinPowerMT, deviceThreshold.MaxCurrentMT, deviceThreshold.MinCurrentMT, rackByOriginalName.RackID, str_version, deviceThreshold.PopEnableMode, deviceThreshold.PopThreshold, i_door, 0f, deviceThreshold.PopModeOutlet, deviceThreshold.PopModeLIFO, deviceThreshold.PopModePriority, deviceThreshold.PopPriorityList, deviceThreshold.DevReferenceVoltage);
					System.Collections.Generic.Dictionary<int, OutletThreshold> outletThreshold = current.OutletThreshold;
					System.Collections.Generic.List<PortInfo> list3 = null;
					if (outletThreshold != null)
					{
						list3 = new System.Collections.Generic.List<PortInfo>();
						foreach (int current4 in outletThreshold.Keys)
						{
							OutletThreshold outletThreshold2 = outletThreshold[current4];
							if ((num4 & 1) != 0)
							{
								outletThreshold2.MinCurrentMt = -300f;
							}
							if ((num4 & 2) != 0)
							{
								outletThreshold2.MaxCurrentMT = -300f;
							}
							if ((num4 & 4) != 0)
							{
								outletThreshold2.MinVoltageMT = devcfgUtil.fMinVoltage(deviceModelConfig, "port", current4);
							}
							if ((num4 & 8) != 0)
							{
								outletThreshold2.MaxVoltageMT = devcfgUtil.fMaxVoltage(deviceModelConfig, "port", current4);
							}
							if ((num4 & 16) != 0)
							{
								outletThreshold2.MinPowerMT = -300f;
							}
							if ((num4 & 32) != 0)
							{
								outletThreshold2.MaxPowerMT = devcfgUtil.fMaxPower(deviceModelConfig, "port", current4);
							}
							if ((num4 & 128) != 0)
							{
								outletThreshold2.MaxPowerDissMT = devcfgUtil.fMaxPowerD(deviceModelConfig, "port", current4);
							}
							string str_name3 = (outletThreshold2.OutletName.Equals(System.Convert.ToString(-1000)) || outletThreshold2.OutletName.Equals(System.Convert.ToString(-500))) ? "" : outletThreshold2.OutletName;
							PortInfo item = new PortInfo(-1, -1, outletThreshold2.OutletNumber, str_name3, outletThreshold2.MaxVoltageMT, outletThreshold2.MinVoltageMT, outletThreshold2.MaxPowerDissMT, -300f, outletThreshold2.MaxPowerMT, outletThreshold2.MinPowerMT, outletThreshold2.MaxCurrentMT, outletThreshold2.MinCurrentMt, (int)outletThreshold2.Confirmation, outletThreshold2.OnDelayTime, outletThreshold2.OffDelayTime, (int)outletThreshold2.ShutdownMethod, outletThreshold2.MacAddress);
							list3.Add(item);
						}
					}
					System.Collections.Generic.Dictionary<int, SensorThreshold> sensorThreshold = current.SensorThreshold;
					System.Collections.Generic.List<SensorInfo> list4 = null;
					if (deviceModelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						SensorInfo[] array = new SensorInfo[]
						{
							null,
							null,
							null,
							null
						};
						list4 = new System.Collections.Generic.List<SensorInfo>();
						SensorInfo sensorInfo;
						if (sensorThreshold != null)
						{
							foreach (int current5 in sensorThreshold.Keys)
							{
								SensorThreshold sensorThreshold2 = sensorThreshold[current5];
								int i_location;
								if (sensorThreshold2.SensorNumber <= 2)
								{
									i_location = 0;
								}
								else
								{
									if (sensorThreshold2.SensorNumber == 3)
									{
										i_location = 1;
									}
									else
									{
										i_location = 2;
									}
								}
								sensorInfo = new SensorInfo(-1, -1, "", i_location, sensorThreshold2.SensorNumber, sensorThreshold2.MaxHumidityMT, sensorThreshold2.MinHumidityMT, sensorThreshold2.MaxTemperatureMT, sensorThreshold2.MinTemperatureMT, sensorThreshold2.MaxPressMT, sensorThreshold2.MinPressMT);
								array[sensorThreshold2.SensorNumber - 1] = sensorInfo;
							}
						}
						if (array[0] == null)
						{
							sensorInfo = new SensorInfo(-1, -1, "", 0, 1, -600f, -600f, -600f, -600f, -600f, -600f);
						}
						else
						{
							sensorInfo = array[0];
						}
						list4.Add(sensorInfo);
						if (array[1] == null)
						{
							sensorInfo = new SensorInfo(-1, -1, "", 0, 2, -600f, -600f, -600f, -600f, -600f, -600f);
						}
						else
						{
							sensorInfo = array[1];
						}
						list4.Add(sensorInfo);
						if (array[2] == null)
						{
							sensorInfo = new SensorInfo(-1, -1, "", 1, 3, -600f, -600f, -600f, -600f, -600f, -600f);
						}
						else
						{
							sensorInfo = array[2];
						}
						list4.Add(sensorInfo);
						if (array[3] == null)
						{
							sensorInfo = new SensorInfo(-1, -1, "", 2, 4, -600f, -600f, -600f, -600f, -600f, -600f);
						}
						else
						{
							sensorInfo = array[3];
						}
						list4.Add(sensorInfo);
					}
					else
					{
						if (sensorThreshold != null)
						{
							list4 = new System.Collections.Generic.List<SensorInfo>();
							foreach (int current6 in sensorThreshold.Keys)
							{
								SensorThreshold sensorThreshold3 = sensorThreshold[current6];
								int i_location;
								if (deviceModelConfig.sensorNum == 2)
								{
									if (sensorThreshold3.SensorNumber == 1)
									{
										i_location = 0;
									}
									else
									{
										i_location = 1;
									}
								}
								else
								{
									if (sensorThreshold3.SensorNumber <= 2)
									{
										i_location = 0;
									}
									else
									{
										if (sensorThreshold3.SensorNumber == 3)
										{
											i_location = 1;
										}
										else
										{
											i_location = 2;
										}
									}
								}
								SensorInfo sensorInfo = new SensorInfo(-1, -1, "", i_location, sensorThreshold3.SensorNumber, sensorThreshold3.MaxHumidityMT, sensorThreshold3.MinHumidityMT, sensorThreshold3.MaxTemperatureMT, sensorThreshold3.MinTemperatureMT, sensorThreshold3.MaxPressMT, sensorThreshold3.MinPressMT);
								list4.Add(sensorInfo);
							}
						}
					}
					System.Collections.Generic.Dictionary<int, BankThreshold> bankThreshold = current.BankThreshold;
					System.Collections.Generic.List<BankInfo> list5 = null;
					if (bankThreshold != null)
					{
						list5 = new System.Collections.Generic.List<BankInfo>();
						foreach (int current7 in bankThreshold.Keys)
						{
							BankThreshold bankThreshold2 = bankThreshold[current7];
							if ((num2 & 1) != 0)
							{
								bankThreshold2.MinCurrentMt = -300f;
							}
							if ((num2 & 2) != 0)
							{
								bankThreshold2.MaxCurrentMT = devcfgUtil.fmaxCurrent(deviceModelConfig, "bank", current7);
							}
							if ((num2 & 4) != 0)
							{
								bankThreshold2.MinVoltageMT = devcfgUtil.fMinVoltage(deviceModelConfig, "bank", current7);
							}
							if ((num2 & 8) != 0)
							{
								bankThreshold2.MaxVoltageMT = devcfgUtil.fMaxVoltage(deviceModelConfig, "bank", current7);
							}
							if ((num2 & 16) != 0)
							{
								bankThreshold2.MinPowerMT = -300f;
							}
							if ((num2 & 32) != 0)
							{
								bankThreshold2.MaxPowerMT = devcfgUtil.fMaxPower(deviceModelConfig, "bank", current7);
							}
							if ((num2 & 128) != 0)
							{
								bankThreshold2.MaxPowerDissMT = devcfgUtil.fMaxPowerD(deviceModelConfig, "bank", current7);
							}
							string str_name4 = (bankThreshold2.BankName.Equals(System.Convert.ToString(-1000)) || bankThreshold2.BankName.Equals(System.Convert.ToString(-500))) ? "" : bankThreshold2.BankName;
							BankInfo item2 = new BankInfo(-1, -1, i_voltage, bankThreshold2.BankNumber.ToString(), str_name4, bankThreshold2.MaxVoltageMT, bankThreshold2.MinVoltageMT, bankThreshold2.MaxPowerDissMT, -300f, bankThreshold2.MaxPowerMT, bankThreshold2.MinPowerMT, bankThreshold2.MaxCurrentMT, bankThreshold2.MinCurrentMt);
							list5.Add(item2);
						}
					}
					System.Collections.Generic.Dictionary<int, LineThreshold> lineThreshold = current.LineThreshold;
					System.Collections.Generic.List<LineInfo> list6 = null;
					if (lineThreshold != null)
					{
						list6 = new System.Collections.Generic.List<LineInfo>();
						foreach (int current8 in lineThreshold.Keys)
						{
							LineThreshold lineThreshold2 = lineThreshold[current8];
							if ((num3 & 1) != 0)
							{
								lineThreshold2.MinCurrentMt = -300f;
							}
							if ((num3 & 2) != 0)
							{
								lineThreshold2.MaxCurrentMT = devcfgUtil.fmaxCurrent(deviceModelConfig, "line", current8);
							}
							if ((num3 & 4) != 0)
							{
								lineThreshold2.MinVoltageMT = devcfgUtil.fMinVoltage(deviceModelConfig, "line", current8);
							}
							if ((num3 & 8) != 0)
							{
								lineThreshold2.MaxVoltageMT = devcfgUtil.fMaxVoltage(deviceModelConfig, "line", current8);
							}
							if ((num3 & 16) != 0)
							{
								lineThreshold2.MinPowerMT = -300f;
							}
							if ((num3 & 32) != 0)
							{
								lineThreshold2.MaxPowerMT = devcfgUtil.fMaxPower(deviceModelConfig, "line", current8);
							}
							string str_name5 = "";
							LineInfo item3 = new LineInfo(-1, -1, str_name5, lineThreshold2.LineNumber, lineThreshold2.MaxVoltageMT, lineThreshold2.MinVoltageMT, lineThreshold2.MaxPowerMT, lineThreshold2.MinPowerMT, lineThreshold2.MaxCurrentMT, lineThreshold2.MinCurrentMt);
							list6.Add(item3);
						}
					}
					int num5 = DeviceOperation.AddNewDevice(connection, deviceInfo, list3, list4, list5, list6);
					if (num5 > 0)
					{
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							LogAPI.writeEventLog("0430000", new string[]
							{
								deviceInfo.ModelNm,
								deviceInfo.Mac,
								deviceInfo.DeviceIP,
								deviceInfo.DeviceName,
								valuePair
							});
						}
						else
						{
							LogAPI.writeEventLog("0430000", new string[]
							{
								deviceInfo.ModelNm,
								deviceInfo.Mac,
								deviceInfo.DeviceIP,
								deviceInfo.DeviceName
							});
						}
						devSnmpConfig.devID = num5;
						list2.Add(devSnmpConfig);
					}
				}
			}
			connection.Close();
			DeviceOperation.RefreshDBCache(false);
			return list2;
		}
		private void dgvAutoDevice_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == ' ')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void dgvAutoDevice_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			e.Control.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			e.Control.KeyPress += new KeyPressEventHandler(this.dgvAutoDevice_KeyPress);
		}
		private void dgvAutoDevice_SelectionChanged(object sender, System.EventArgs e)
		{
			int arg_10_0 = this.dgvAutoDevice.SelectedRows.Count;
		}
		private void dgvAutoDevice_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				return;
			}
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}
			DataGridViewCell currentCell = this.dgvAutoDevice.Rows[e.RowIndex].Cells[e.ColumnIndex];
			if (e.ColumnIndex == 5)
			{
				this.dgvAutoDevice.CurrentCell = currentCell;
				this.dgvAutoDevice.BeginEdit(false);
				ComboBox comboBox = (ComboBox)this.dgvAutoDevice.EditingControl;
				comboBox.DroppedDown = true;
			}
		}
		private void dgvAutoDevice_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dgvAutoDevice.IsCurrentCellDirty)
			{
				this.dgvAutoDevice.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void dgvAutoDevice_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
			{
				return;
			}
			new Point(e.ColumnIndex, e.RowIndex);
			if (e.ColumnIndex == 0)
			{
				try
				{
					DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)this.dgvAutoDevice.Rows[e.RowIndex].Cells[0];
					if ((bool)dataGridViewCheckBoxCell.Value)
					{
						for (int i = 0; i < this.dgvAutoDevice.Rows.Count; i++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dgvAutoDevice.Rows[i].Cells[0];
							if (!(bool)dataGridViewCheckBoxCell2.Value)
							{
								return;
							}
						}
						if (!this.cbsel.Checked)
						{
							this.cbsel_changeonly = true;
							this.cbsel.Checked = true;
						}
					}
					else
					{
						if (this.cbsel.Checked)
						{
							this.cbsel_changeonly = true;
							this.cbsel.Checked = false;
						}
					}
				}
				catch (System.Exception)
				{
				}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AddDevDlg));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			this.cbsel = new CheckBox();
			this.butAssignNm = new Button();
			this.butAdd = new Button();
			this.butCancel = new Button();
			this.dgvAutoDevice = new DataGridView();
			this.datasel = new DataGridViewCheckBoxColumn();
			this.dgvtbPduNm = new DataGridViewTextBoxColumn();
			this.dgvtbMac = new DataGridViewTextBoxColumn();
			this.dgvtbIp = new DataGridViewTextBoxColumn();
			this.dgvtbModel = new DataGridViewTextBoxColumn();
			this.dgvtbRackNm = new DataGridViewComboBoxColumn();
			this.fwVersion = new DataGridViewTextBoxColumn();
			this.pictureBoxLoading = new PictureBox();
			((ISupportInitialize)this.dgvAutoDevice).BeginInit();
			((ISupportInitialize)this.pictureBoxLoading).BeginInit();
			base.SuspendLayout();
			this.cbsel.BackColor = Color.Transparent;
			this.cbsel.Checked = true;
			this.cbsel.CheckState = CheckState.Checked;
			componentResourceManager.ApplyResources(this.cbsel, "cbsel");
			this.cbsel.Name = "cbsel";
			this.cbsel.UseVisualStyleBackColor = false;
			this.cbsel.CheckedChanged += new System.EventHandler(this.cbsel_CheckedChanged);
			this.butAssignNm.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.butAssignNm, "butAssignNm");
			this.butAssignNm.Name = "butAssignNm";
			this.butAssignNm.UseVisualStyleBackColor = false;
			this.butAssignNm.Click += new System.EventHandler(this.butAssignNm_Click);
			this.butAdd.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.butAdd, "butAdd");
			this.butAdd.Name = "butAdd";
			this.butAdd.UseVisualStyleBackColor = false;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			this.butCancel.BackColor = SystemColors.Control;
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = false;
			this.dgvAutoDevice.AllowUserToAddRows = false;
			this.dgvAutoDevice.AllowUserToDeleteRows = false;
			this.dgvAutoDevice.AllowUserToResizeColumns = false;
			this.dgvAutoDevice.AllowUserToResizeRows = false;
			this.dgvAutoDevice.BackgroundColor = Color.White;
			this.dgvAutoDevice.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvAutoDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvAutoDevice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvAutoDevice.Columns.AddRange(new DataGridViewColumn[]
			{
				this.datasel,
				this.dgvtbPduNm,
				this.dgvtbMac,
				this.dgvtbIp,
				this.dgvtbModel,
				this.dgvtbRackNm,
				this.fwVersion
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvAutoDevice.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvAutoDevice.EditMode = DataGridViewEditMode.EditOnEnter;
			this.dgvAutoDevice.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvAutoDevice, "dgvAutoDevice");
			this.dgvAutoDevice.MultiSelect = false;
			this.dgvAutoDevice.Name = "dgvAutoDevice";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvAutoDevice.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvAutoDevice.RowHeadersVisible = false;
			this.dgvAutoDevice.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvAutoDevice.RowTemplate.Height = 23;
			this.dgvAutoDevice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAutoDevice.StandardTab = true;
			this.dgvAutoDevice.TabStop = false;
			this.dgvAutoDevice.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvAutoDevice_CellMouseDown);
			this.dgvAutoDevice.CellValueChanged += new DataGridViewCellEventHandler(this.dgvAutoDevice_CellValueChanged);
			this.dgvAutoDevice.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAutoDevice_CurrentCellDirtyStateChanged);
			this.dgvAutoDevice.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgvAutoDevice_EditingControlShowing);
			this.dgvAutoDevice.SelectionChanged += new System.EventHandler(this.dgvAutoDevice_SelectionChanged);
			this.dgvAutoDevice.KeyPress += new KeyPressEventHandler(this.dgvAutoDevice_KeyPress);
			componentResourceManager.ApplyResources(this.datasel, "datasel");
			this.datasel.Name = "datasel";
			dataGridViewCellStyle4.Font = new Font("SimSun", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dgvtbPduNm.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.dgvtbPduNm, "dgvtbPduNm");
			this.dgvtbPduNm.MaxInputLength = 39;
			this.dgvtbPduNm.Name = "dgvtbPduNm";
			componentResourceManager.ApplyResources(this.dgvtbMac, "dgvtbMac");
			this.dgvtbMac.Name = "dgvtbMac";
			this.dgvtbMac.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbIp, "dgvtbIp");
			this.dgvtbIp.Name = "dgvtbIp";
			this.dgvtbIp.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbModel, "dgvtbModel");
			this.dgvtbModel.Name = "dgvtbModel";
			this.dgvtbModel.ReadOnly = true;
			this.dgvtbRackNm.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dgvtbRackNm, "dgvtbRackNm");
			this.dgvtbRackNm.Name = "dgvtbRackNm";
			this.dgvtbRackNm.Resizable = DataGridViewTriState.True;
			this.dgvtbRackNm.SortMode = DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.fwVersion, "fwVersion");
			this.fwVersion.Name = "fwVersion";
			this.fwVersion.ReadOnly = true;
			this.pictureBoxLoading.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pictureBoxLoading, "pictureBoxLoading");
			this.pictureBoxLoading.Image = Resources.loader;
			this.pictureBoxLoading.Name = "pictureBoxLoading";
			this.pictureBoxLoading.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.pictureBoxLoading);
			base.Controls.Add(this.cbsel);
			base.Controls.Add(this.butAssignNm);
			base.Controls.Add(this.butAdd);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.dgvAutoDevice);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AddDevDlg";
			base.ShowInTaskbar = false;
			base.FormClosing += new FormClosingEventHandler(this.AddDevDlg_FormClosing);
			((ISupportInitialize)this.dgvAutoDevice).EndInit();
			((ISupportInitialize)this.pictureBoxLoading).EndInit();
			base.ResumeLayout(false);
		}
	}
}
