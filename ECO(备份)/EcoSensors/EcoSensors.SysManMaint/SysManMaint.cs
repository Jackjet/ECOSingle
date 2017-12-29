using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.component;
using EcoSensors.Common.Thread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.SysManMaint
{
	public class SysManMaint : UserControl
	{
		private delegate void Timerdelegate();
		private const string TBcol_devIP = "dgvtbIp";
		private const string TBcol_status = "dgvtStatus";
		private const string TBcol_devID = "dgvtdevID";
		private const string TBcol_httpPort = "dgvtdevhttpPort";
		private const int TBcolIndex_DevNM = 1;
		private const int TBcolIndex_IP = 2;
		private const int TBcolIndex_Model = 3;
		private const int TBcolIndex_FWVer = 4;
		private const int TBcolIndex_Status = 5;
		private const int TBcolIndex_devID = 6;
		private const int TBcolIndex_httpPort = 7;
		private const int UPGRADEFLG_init = 0;
		private const int UPGRADEFLG_Upgrading = 1;
		private const int UPGRADEFLG_Finish = 2;
		private int m_isUpgradeFlg;
		private bool cbsel_changeonly;
		private System.Timers.Timer dTimer;
		private System.Collections.Generic.List<string[]> m_allRows = new System.Collections.Generic.List<string[]>();
		private IContainer components;
		private Label label1;
		private Button butUpdate;
		private Button butBrowse;
		private TextBox tbFileNm;
		private Label lbFileNm;
		private CheckBox cbMainFw;
		private GroupBox groupBox27;
		private CheckBox cbsel;
		private DataGridView dgvFwDevice;
		private Panel panel1;
		private DataGridViewDisableCheckBoxColumn datasel;
		private DataGridViewTextBoxColumn dgvtbPduNm;
		private DataGridViewTextBoxColumn dgvtbIp;
		private DataGridViewTextBoxColumn dgvtbModel;
		private DataGridViewTextBoxColumn dgvtbFwVersion;
		private DataGridViewTextBoxColumn dgvtStatus;
		private DataGridViewTextBoxColumn dgvtdevID;
		private DataGridViewTextBoxColumn dgvtdevhttpPort;
		private Button butSearch;
		private DataGridViewDisableCheckBoxColumn dataGridViewDisableCheckBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		public SysManMaint()
		{
			this.InitializeComponent();
		}
		public void pageInit()
		{
			this.cbsel_changeonly = false;
			this.InitTimer();
			if (this.m_isUpgradeFlg == 0)
			{
				this.tbFileNm.Text = string.Empty;
				this.cbMainFw.Checked = true;
				if (this.m_allRows.Count == 0)
				{
					this.dTimer.Enabled = true;
					return;
				}
				this.filteronlinedev();
			}
		}
		private void InitTimer()
		{
			if (this.dTimer == null)
			{
				this.dTimer = new System.Timers.Timer();
				this.dTimer.Elapsed += new ElapsedEventHandler(this.TimerProc);
				this.dTimer.Interval = 500.0;
				this.dTimer.AutoReset = false;
				this.dTimer.Enabled = false;
			}
		}
		private void TimerProc(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new SysManMaint.Timerdelegate(this.init_devTB));
				return;
			}
			this.init_devTB();
		}
		private void init_devTB()
		{
			this.dgvFwDevice.Rows.Clear();
			this.m_allRows = null;
			this.m_allRows = new System.Collections.Generic.List<string[]>();
			this.cbsel.Checked = true;
			this.cbMainFw.Enabled = false;
			this.butBrowse.Enabled = false;
			this.butUpdate.Enabled = false;
			this.cbsel.Enabled = false;
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_FWupfinddev, new string[0]), null, new progressPopup.ProcessInThread(this.initFWDevProc), null, 0);
			progressPopup.ShowDialog();
		}
		private object initFWDevProc(object aaa)
		{
			System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			DevAccessCfg.GetInstance();
			foreach (DeviceInfo current in allDevice)
			{
				if (ClientAPI.IsDeviceOnline(current.DeviceID))
				{
					string text = current.DeviceID.ToString();
					string modelNm = current.ModelNm;
					DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(modelNm, current.FWVersion);
					if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDU_M2 && deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold && deviceModelConfig.commonThresholdFlag != Constant.APC_PDU)
					{
						DevSnmpConfig sNMPpara = commUtil.getSNMPpara(current);
						string mac = current.Mac;
						DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
						DevServiceInfo devServiceInfo = devAccessAPI.GetDevServiceInfo(mac);
						int httpPort = devServiceInfo.httpPort;
						if (httpPort != 0)
						{
							string[] obj = new string[]
							{
								"1",
								current.DeviceName,
								current.DeviceIP,
								current.ModelNm,
								devServiceInfo.fwVersion,
								"",
								text,
								httpPort.ToString()
							};
							ControlAccess.ConfigControl config = delegate(Control control, object param)
							{
								DataGridView dataGridView = control as DataGridView;
								string[] array = param as string[];
								dataGridView.Rows.Add(new object[]
								{
									true,
									array[1],
									array[2],
									array[3],
									array[4],
									array[5],
									array[6],
									array[7]
								});
								this.m_allRows.Add(array);
							};
							ControlAccess controlAccess = new ControlAccess(this, config);
							controlAccess.Access(this.dgvFwDevice, obj);
						}
					}
				}
			}
			ControlAccess.ConfigControl config2 = delegate(Control control, object param)
			{
				this.cbMainFw.Enabled = true;
				this.butBrowse.Enabled = true;
				this.butUpdate.Enabled = true;
				this.cbsel.Enabled = true;
			};
			ControlAccess controlAccess2 = new ControlAccess(this, config2);
			controlAccess2.Access(this.dgvFwDevice, null);
			return 0;
		}
		private void filteronlinedev()
		{
			this.dgvFwDevice.Rows.Clear();
			this.cbsel.Checked = true;
			string text = null;
			string strB = null;
			string text2 = null;
			string text3 = "";
			string text4 = "";
			if (this.tbFileNm.Text.Length > 0)
			{
				System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.tbFileNm.Text);
				text = fileInfo.Name.ToUpper();
				text2 = fileInfo.Extension;
				if (text2 != null)
				{
					text2 = text2.ToUpper();
				}
				strB = this.getDevFWflg(this.tbFileNm.Text, ref text3, ref text4);
			}
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			if (text4.Length == 0)
			{
				using (System.Collections.Generic.List<string[]>.Enumerator enumerator = this.m_allRows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string[] current = enumerator.Current;
						string text5 = current[6];
						string text6 = current[7];
						string text7 = current[4];
						if (text != null)
						{
							DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current[3], text7);
							if (deviceModelConfig.FWvalidate != 0)
							{
								if ((deviceModelConfig.FWnms.Length > 0 && !text.StartsWith(deviceModelConfig.FWnms)) || (deviceModelConfig.FWext.Length > 0 && (text2 == null || !text2.Equals(deviceModelConfig.FWext))))
								{
									continue;
								}
								switch (deviceModelConfig.FWvalidate)
								{
								case 1:
									if (!text3.Equals("ATEN") || (this.cbMainFw.Checked && text7.CompareTo(strB) >= 0))
									{
										continue;
									}
									break;
								}
							}
						}
						string[] obj = new string[]
						{
							"1",
							current[1],
							current[2],
							current[3],
							text7,
							current[5],
							text5,
							text6
						};
						ControlAccess.ConfigControl config = delegate(Control control, object param)
						{
							DataGridView dataGridView = control as DataGridView;
							string[] array2 = param as string[];
							dataGridView.Rows.Add(new object[]
							{
								true,
								array2[1],
								array2[2],
								array2[3],
								array2[4],
								array2[5],
								array2[6],
								array2[7]
							});
						};
						ControlAccess controlAccess = new ControlAccess(this, config);
						controlAccess.Access(this.dgvFwDevice, obj);
					}
					goto IL_37E;
				}
			}
			string[] array = text4.Split(new char[]
			{
				'/'
			});
			foreach (string[] current2 in this.m_allRows)
			{
				string text5 = current2[6];
				string text6 = current2[7];
				string text7 = current2[4];
				if (text != null)
				{
					bool flag = false;
					for (int i = 0; i < array.Length; i++)
					{
						string text8 = array[i];
						int num = text8.IndexOf("*");
						if (num >= 0)
						{
							text8 = text8.Substring(0, num);
							if (current2[3].IndexOf(text8) >= 0)
							{
								flag = true;
								break;
							}
						}
						else
						{
							if (current2[3].Equals(array[i]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag || (this.cbMainFw.Checked && text7.CompareTo(strB) >= 0))
					{
						continue;
					}
				}
				string[] obj2 = new string[]
				{
					"1",
					current2[1],
					current2[2],
					current2[3],
					text7,
					current2[5],
					text5,
					text6
				};
				ControlAccess.ConfigControl config2 = delegate(Control control, object param)
				{
					DataGridView dataGridView = control as DataGridView;
					string[] array2 = param as string[];
					dataGridView.Rows.Add(new object[]
					{
						true,
						array2[1],
						array2[2],
						array2[3],
						array2[4],
						array2[5],
						array2[6],
						array2[7]
					});
				};
				ControlAccess controlAccess2 = new ControlAccess(this, config2);
				controlAccess2.Access(this.dgvFwDevice, obj2);
			}
			IL_37E:
			ControlAccess.ConfigControl config3 = delegate(Control control, object param)
			{
				this.cbMainFw.Enabled = true;
				this.butBrowse.Enabled = true;
				this.butUpdate.Enabled = true;
				this.cbsel.Enabled = true;
			};
			ControlAccess controlAccess3 = new ControlAccess(this, config3);
			controlAccess3.Access(this.dgvFwDevice, null);
		}
		private void butBrowse_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.bin,*.dat)|*.bin;*.dat|All Files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.tbFileNm.Text = openFileDialog.FileName;
				this.filteronlinedev();
			}
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
				for (int i = 0; i < this.dgvFwDevice.Rows.Count; i++)
				{
					DataGridViewRow dataGridViewRow = this.dgvFwDevice.Rows[i];
					dataGridViewRow.Cells[0].Value = true;
				}
				return;
			}
			for (int j = 0; j < this.dgvFwDevice.Rows.Count; j++)
			{
				DataGridViewRow dataGridViewRow2 = this.dgvFwDevice.Rows[j];
				dataGridViewRow2.Cells[0].Value = false;
			}
		}
		private void changeUpgradeStatus(DataGridView grid, int devID, string status)
		{
			this.changeUpgradeStatus(grid, devID, status, "");
		}
		private void changeUpgradeStatus(DataGridView grid, int devID, string status, string fwVersion)
		{
			int i = 0;
			while (i < grid.Rows.Count)
			{
				DataGridViewCellCollection cells = grid.Rows.SharedRow(i).Cells;
				if (devID == System.Convert.ToInt32(cells["dgvtdevID"].Value.ToString()))
				{
					cells["dgvtStatus"].Value = status;
					if (!fwVersion.Equals(string.Empty))
					{
						cells["dgvtbFwVersion"].Value = fwVersion;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		private void Upgrade(UpgradeStatus us, int devID, string parameter)
		{
			string status = "";
			if (us == UpgradeStatus.UpgradeFailed)
			{
				ControlAccess.ConfigControl config = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_upgradefail, new string[0]);
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess = new ControlAccess(this, config);
				controlAccess.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.UpgradeSucceed)
			{
				ControlAccess.ConfigControl config2 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int num = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_upgradesucc, new string[0]);
					string parameter2 = parameter;
					this.changeUpgradeStatus(grid, num, status, parameter2);
					DeviceInfo deviceByID = DeviceOperation.getDeviceByID(num);
					deviceByID.FWVersion = parameter2;
					deviceByID.Update();
				};
				ControlAccess controlAccess2 = new ControlAccess(this, config2);
				controlAccess2.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.ServerUnconnected)
			{
				ControlAccess.ConfigControl config3 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_serverunconnected, new string[0]);
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess3 = new ControlAccess(this, config3);
				controlAccess3.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.Uploading)
			{
				ControlAccess.ConfigControl config4 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_uploading, new string[0]) + " " + parameter;
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess4 = new ControlAccess(this, config4);
				controlAccess4.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.Upgrading)
			{
				ControlAccess.ConfigControl config5 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_upgrading, new string[0]) + " " + parameter;
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess5 = new ControlAccess(this, config5);
				controlAccess5.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.WrongFile)
			{
				ControlAccess.ConfigControl config6 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_wrongfile, new string[0]);
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess6 = new ControlAccess(this, config6);
				controlAccess6.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.NoNeedToUpgrade)
			{
				ControlAccess.ConfigControl config7 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_noneedupgrade, new string[0]);
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess7 = new ControlAccess(this, config7);
				controlAccess7.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.ServerBusy)
			{
				ControlAccess.ConfigControl config8 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_serverbusy, new string[0]);
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess8 = new ControlAccess(this, config8);
				controlAccess8.Access(this.dgvFwDevice, devID.ToString());
				return;
			}
			if (us == UpgradeStatus.Starting)
			{
				ControlAccess.ConfigControl config9 = delegate(Control control, object param)
				{
					DataGridView grid = control as DataGridView;
					string value = param as string;
					int devID2 = System.Convert.ToInt32(value);
					status = EcoLanguage.getMsg(LangRes.FrwUpgradeST_starting, new string[0]);
					this.changeUpgradeStatus(grid, devID2, status);
				};
				ControlAccess controlAccess9 = new ControlAccess(this, config9);
				controlAccess9.Access(this.dgvFwDevice, devID.ToString());
			}
		}
		private void butUpdate_Click(object sender, System.EventArgs e)
		{
			if (!this.check())
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_selectneed, new string[0]));
				return;
			}
			if (this.tbFileNm.Text.Equals(string.Empty))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbFileNm.Text
				}));
				return;
			}
			this.cbMainFw.Enabled = false;
			this.butBrowse.Enabled = false;
			this.butSearch.Enabled = false;
			this.butUpdate.Enabled = false;
			this.cbsel.Enabled = false;
			System.Collections.Generic.List<FirmwareUpgrade> list = new System.Collections.Generic.List<FirmwareUpgrade>();
			for (int i = 0; i < this.dgvFwDevice.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvFwDevice.Rows[i];
				DataGridViewDisableCheckBoxCell dataGridViewDisableCheckBoxCell = dataGridViewRow.Cells[0] as DataGridViewDisableCheckBoxCell;
				if (dataGridViewRow.Cells[0].Value != null && (bool)dataGridViewRow.Cells[0].Value)
				{
					list.Add(new FirmwareUpgrade
					{
						FileName = this.tbFileNm.Text,
						CheckVersion = this.cbMainFw.Checked ? 1 : 0,
						DevID = System.Convert.ToInt32(dataGridViewRow.Cells["dgvtdevID"].Value.ToString()),
						DevIP = dataGridViewRow.Cells["dgvtbIp"].Value.ToString(),
						httpPort = dataGridViewRow.Cells["dgvtdevhttpPort"].Value.ToString()
					});
					dataGridViewRow.Cells["dgvtStatus"].Value = EcoLanguage.getMsg(LangRes.FrwUpgradeST_waiting, new string[0]);
				}
				dataGridViewDisableCheckBoxCell.Enabled = false;
				dataGridViewDisableCheckBoxCell.ReadOnly = true;
			}
			this.dgvFwDevice.Update();
			this.dgvFwDevice.Focus();
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.ThreadFwUpgrade), list);
		}
		private void ThreadFwUpgrade(object obj)
		{
			this.m_isUpgradeFlg = 1;
			Program.IdleTimer_Pause(1);
			System.Collections.Generic.List<FirmwareUpgrade> list = obj as System.Collections.Generic.List<FirmwareUpgrade>;
			for (int i = 0; i < list.Count; i++)
			{
				FirmwareUpgrade firmwareUpgrade = list[i];
				firmwareUpgrade.Upgrade(new FirmwareUpgrade.Upgrading(this.Upgrade));
			}
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				this.cbMainFw.Enabled = true;
				this.butBrowse.Enabled = true;
				this.butSearch.Enabled = true;
				this.butUpdate.Enabled = true;
				this.cbsel.Enabled = true;
				for (int j = 0; j < this.dgvFwDevice.Rows.Count; j++)
				{
					DataGridViewRow dataGridViewRow = this.dgvFwDevice.Rows[j];
					int num = System.Convert.ToInt32(dataGridViewRow.Cells["dgvtdevID"].Value.ToString());
					string text = dataGridViewRow.Cells[4].Value.ToString();
					foreach (string[] current in this.m_allRows)
					{
						int num2 = System.Convert.ToInt32(current[6]);
						if (num == num2)
						{
							current[4] = text;
							current[5] = dataGridViewRow.Cells[5].Value.ToString();
							break;
						}
					}
					DataGridViewDisableCheckBoxCell dataGridViewDisableCheckBoxCell = dataGridViewRow.Cells[0] as DataGridViewDisableCheckBoxCell;
					dataGridViewDisableCheckBoxCell.Enabled = true;
					dataGridViewDisableCheckBoxCell.ReadOnly = false;
				}
				this.dgvFwDevice.Update();
				this.dgvFwDevice.Focus();
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this.dgvFwDevice, null);
			Program.IdleTimer_Run(1);
			this.m_isUpgradeFlg = 2;
		}
		private bool check()
		{
			bool result = false;
			for (int i = 0; i < this.dgvFwDevice.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvFwDevice.Rows[i];
				if (dataGridViewRow.Cells[0].Value != null && (bool)dataGridViewRow.Cells[0].Value)
				{
					result = true;
					break;
				}
			}
			return result;
		}
		private string getDevFWflg(string filepath, ref string atenstr, ref string SummaryModelNm)
		{
			byte[] array = new byte[8];
			byte[] array2 = new byte[4];
			System.IO.FileStream fileStream = null;
			string text = null;
			string text2 = this.findSUMMARY(filepath);
			if (text2.Length == 0)
			{
				try
				{
					fileStream = System.IO.File.OpenRead(filepath);
					fileStream.Seek(-16L, System.IO.SeekOrigin.End);
					fileStream.Read(array, 0, 8);
					fileStream.Read(array2, 0, 4);
					atenstr = "";
					atenstr += (char)array2[0];
					atenstr += (char)array2[1];
					atenstr += (char)array2[2];
					atenstr += (char)array2[3];
					text = "";
					int num = 0;
					while (num < array.Length && array[num] != 255)
					{
						text += (char)array[num];
						num++;
					}
					fileStream.Close();
					return text;
				}
				catch (System.Exception)
				{
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				return text;
			}
			string text3 = "";
			int num2 = text2.IndexOf("MODELNAME=");
			if (num2 >= 0)
			{
				text3 = text2.Substring(num2 + 10);
				num2 = text3.IndexOf(";");
				text3 = text3.Substring(0, num2);
			}
			SummaryModelNm = text3;
			string text4 = "";
			num2 = text2.IndexOf("VERSION=");
			if (num2 >= 0)
			{
				text4 = text2.Substring(num2 + 8);
				num2 = text4.IndexOf(";");
				text4 = text4.Substring(0, num2);
				text4 = text4.Replace("V", "");
				text4 = text4.Replace("v", "");
			}
			return text4;
		}
		private string findSUMMARY(string filepath)
		{
			byte[] array = System.IO.File.ReadAllBytes(filepath);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes("[SUMMARY-BEGIN:");
			byte[] bytes2 = System.Text.Encoding.UTF8.GetBytes("SUMMARY-END]");
			try
			{
				for (int i = 0; i <= array.Length - bytes.Length; i++)
				{
					if (array[i] == bytes[0])
					{
						int num = 1;
						while (num < bytes.Length && array[i + num] == bytes[num])
						{
							num++;
						}
						if (num == bytes.Length)
						{
							System.Console.WriteLine("String was found at offset {0}", i);
							int num2 = i;
							for (int j = num2; j <= array.Length - bytes2.Length; j++)
							{
								if (array[j] == bytes2[0])
								{
									num = 1;
									while (num < bytes2.Length && array[j + num] == bytes2[num])
									{
										num++;
									}
									if (num == bytes2.Length)
									{
										System.Console.WriteLine("String was found at offset {0}", j);
										return System.Text.Encoding.UTF8.GetString(array, num2, j - num2 + num);
									}
								}
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
			}
			return "";
		}
		private void dgvFwDevice_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dgvFwDevice.IsCurrentCellDirty)
			{
				this.dgvFwDevice.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void dgvFwDevice_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
			{
				return;
			}
			if (e.ColumnIndex == 0)
			{
				try
				{
					DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)this.dgvFwDevice.Rows[e.RowIndex].Cells[0];
					if ((bool)dataGridViewCheckBoxCell.Value)
					{
						for (int i = 0; i < this.dgvFwDevice.Rows.Count; i++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dgvFwDevice.Rows[i].Cells[0];
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
		private void cbMainFw_CheckedChanged(object sender, System.EventArgs e)
		{
			this.filteronlinedev();
		}
		private void butSearch_Click(object sender, System.EventArgs e)
		{
			this.m_allRows.Clear();
			this.tbFileNm.Text = string.Empty;
			this.cbMainFw.Checked = true;
			this.dTimer.Enabled = true;
			this.m_isUpgradeFlg = 0;
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
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManMaint));
			this.groupBox27 = new GroupBox();
			this.panel1 = new Panel();
			this.cbsel = new CheckBox();
			this.dgvFwDevice = new DataGridView();
			this.label1 = new Label();
			this.butUpdate = new Button();
			this.butBrowse = new Button();
			this.tbFileNm = new TextBox();
			this.lbFileNm = new Label();
			this.cbMainFw = new CheckBox();
			this.dataGridViewDisableCheckBoxColumn1 = new DataGridViewDisableCheckBoxColumn();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
			this.datasel = new DataGridViewDisableCheckBoxColumn();
			this.dgvtbPduNm = new DataGridViewTextBoxColumn();
			this.dgvtbIp = new DataGridViewTextBoxColumn();
			this.dgvtbModel = new DataGridViewTextBoxColumn();
			this.dgvtbFwVersion = new DataGridViewTextBoxColumn();
			this.dgvtStatus = new DataGridViewTextBoxColumn();
			this.dgvtdevID = new DataGridViewTextBoxColumn();
			this.dgvtdevhttpPort = new DataGridViewTextBoxColumn();
			this.butSearch = new Button();
			this.groupBox27.SuspendLayout();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.dgvFwDevice).BeginInit();
			base.SuspendLayout();
			this.groupBox27.BackColor = Color.WhiteSmoke;
			this.groupBox27.Controls.Add(this.butSearch);
			this.groupBox27.Controls.Add(this.panel1);
			this.groupBox27.Controls.Add(this.label1);
			this.groupBox27.Controls.Add(this.butUpdate);
			this.groupBox27.Controls.Add(this.butBrowse);
			this.groupBox27.Controls.Add(this.tbFileNm);
			this.groupBox27.Controls.Add(this.lbFileNm);
			this.groupBox27.Controls.Add(this.cbMainFw);
			componentResourceManager.ApplyResources(this.groupBox27, "groupBox27");
			this.groupBox27.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox27.Name = "groupBox27";
			this.groupBox27.TabStop = false;
			this.panel1.Controls.Add(this.cbsel);
			this.panel1.Controls.Add(this.dgvFwDevice);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.ForeColor = Color.Black;
			this.panel1.Name = "panel1";
			this.cbsel.BackColor = Color.Transparent;
			this.cbsel.Checked = true;
			this.cbsel.CheckState = CheckState.Checked;
			componentResourceManager.ApplyResources(this.cbsel, "cbsel");
			this.cbsel.ForeColor = Color.Black;
			this.cbsel.Name = "cbsel";
			this.cbsel.UseVisualStyleBackColor = false;
			this.cbsel.CheckedChanged += new System.EventHandler(this.cbsel_CheckedChanged);
			this.dgvFwDevice.AllowUserToAddRows = false;
			this.dgvFwDevice.AllowUserToDeleteRows = false;
			this.dgvFwDevice.AllowUserToResizeRows = false;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dgvFwDevice.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.dgvFwDevice.BackgroundColor = Color.White;
			this.dgvFwDevice.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = SystemColors.Control;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			this.dgvFwDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.dgvFwDevice, "dgvFwDevice");
			this.dgvFwDevice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvFwDevice.Columns.AddRange(new DataGridViewColumn[]
			{
				this.datasel,
				this.dgvtbPduNm,
				this.dgvtbIp,
				this.dgvtbModel,
				this.dgvtbFwVersion,
				this.dgvtStatus,
				this.dgvtdevID,
				this.dgvtdevhttpPort
			});
			this.dgvFwDevice.GridColor = Color.White;
			this.dgvFwDevice.MultiSelect = false;
			this.dgvFwDevice.Name = "dgvFwDevice";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvFwDevice.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvFwDevice.RowHeadersVisible = false;
			this.dgvFwDevice.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvFwDevice.RowTemplate.Height = 23;
			this.dgvFwDevice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvFwDevice.StandardTab = true;
			this.dgvFwDevice.TabStop = false;
			this.dgvFwDevice.CellValueChanged += new DataGridViewCellEventHandler(this.dgvFwDevice_CellValueChanged);
			this.dgvFwDevice.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvFwDevice_CurrentCellDirtyStateChanged);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Red;
			this.label1.Name = "label1";
			this.butUpdate.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butUpdate, "butUpdate");
			this.butUpdate.ForeColor = Color.Black;
			this.butUpdate.Name = "butUpdate";
			this.butUpdate.UseVisualStyleBackColor = false;
			this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
			this.butBrowse.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butBrowse, "butBrowse");
			this.butBrowse.ForeColor = SystemColors.ControlText;
			this.butBrowse.Name = "butBrowse";
			this.butBrowse.UseVisualStyleBackColor = false;
			this.butBrowse.Click += new System.EventHandler(this.butBrowse_Click);
			componentResourceManager.ApplyResources(this.tbFileNm, "tbFileNm");
			this.tbFileNm.Name = "tbFileNm";
			this.tbFileNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.lbFileNm, "lbFileNm");
			this.lbFileNm.ForeColor = SystemColors.ControlText;
			this.lbFileNm.Name = "lbFileNm";
			componentResourceManager.ApplyResources(this.cbMainFw, "cbMainFw");
			this.cbMainFw.Checked = true;
			this.cbMainFw.CheckState = CheckState.Checked;
			this.cbMainFw.ForeColor = SystemColors.ControlText;
			this.cbMainFw.Name = "cbMainFw";
			this.cbMainFw.UseVisualStyleBackColor = true;
			this.cbMainFw.CheckedChanged += new System.EventHandler(this.cbMainFw_CheckedChanged);
			componentResourceManager.ApplyResources(this.dataGridViewDisableCheckBoxColumn1, "dataGridViewDisableCheckBoxColumn1");
			this.dataGridViewDisableCheckBoxColumn1.Name = "dataGridViewDisableCheckBoxColumn1";
			this.dataGridViewTextBoxColumn1.DataPropertyName = "device_nm";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.MaxInputLength = 15;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.DataPropertyName = "device_ip";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.DataPropertyName = "model_nm";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.DataPropertyName = "fw_version";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			componentResourceManager.ApplyResources(this.datasel, "datasel");
			this.datasel.Name = "datasel";
			this.dgvtbPduNm.DataPropertyName = "device_nm";
			componentResourceManager.ApplyResources(this.dgvtbPduNm, "dgvtbPduNm");
			this.dgvtbPduNm.MaxInputLength = 15;
			this.dgvtbPduNm.Name = "dgvtbPduNm";
			this.dgvtbPduNm.ReadOnly = true;
			this.dgvtbIp.DataPropertyName = "device_ip";
			componentResourceManager.ApplyResources(this.dgvtbIp, "dgvtbIp");
			this.dgvtbIp.Name = "dgvtbIp";
			this.dgvtbIp.ReadOnly = true;
			this.dgvtbModel.DataPropertyName = "model_nm";
			componentResourceManager.ApplyResources(this.dgvtbModel, "dgvtbModel");
			this.dgvtbModel.Name = "dgvtbModel";
			this.dgvtbModel.ReadOnly = true;
			this.dgvtbFwVersion.DataPropertyName = "fw_version";
			componentResourceManager.ApplyResources(this.dgvtbFwVersion, "dgvtbFwVersion");
			this.dgvtbFwVersion.Name = "dgvtbFwVersion";
			this.dgvtbFwVersion.ReadOnly = true;
			this.dgvtStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dgvtStatus, "dgvtStatus");
			this.dgvtStatus.Name = "dgvtStatus";
			this.dgvtStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtdevID, "dgvtdevID");
			this.dgvtdevID.Name = "dgvtdevID";
			componentResourceManager.ApplyResources(this.dgvtdevhttpPort, "dgvtdevhttpPort");
			this.dgvtdevhttpPort.Name = "dgvtdevhttpPort";
			this.butSearch.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSearch, "butSearch");
			this.butSearch.ForeColor = Color.Black;
			this.butSearch.Name = "butSearch";
			this.butSearch.UseVisualStyleBackColor = false;
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBox27);
			componentResourceManager.ApplyResources(this, "$this");
			this.ForeColor = SystemColors.ControlText;
			base.Name = "SysManMaint";
			this.groupBox27.ResumeLayout(false);
			this.groupBox27.PerformLayout();
			this.panel1.ResumeLayout(false);
			((ISupportInitialize)this.dgvFwDevice).EndInit();
			base.ResumeLayout(false);
		}
	}
}
