using CommonAPI.Global;
using DBAccessAPI;
using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Properties;
using EventLogAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice
{
	public class DevManAllDev : UserControl
	{
		private const string DTbCol0_Img = "imgstatus";
		private const string DTbCol1_devName = "dgvtbDeviceNm";
		private const string DTbCol2_MAC = "dgvtbcMac";
		private const string DTbCol6_ID = "dgvtbcdeviceId";
		private IContainer components;
		private DataGridView dgvAllDevices;
		private Button butDevicesDel;
		private Button butAddDevices;
		private Button butDeviceSetup;
		private Button butSyncThreshold;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private DataGridViewImageColumn imgstatus;
		private DataGridViewTextBoxColumn dgvtbDeviceNm;
		private DataGridViewTextBoxColumn dgvtbcMac;
		private DataGridViewTextBoxColumn dgvtbcIp;
		private DataGridViewTextBoxColumn dgvtbcport;
		private DataGridViewTextBoxColumn dgvtbcModel;
		private DataGridViewTextBoxColumn dgvtbcRackNm;
		private DataGridViewTextBoxColumn dgvtbcdeviceId;
		private TextBox tbFilterKey;
		private Button btnFilter;
		private DevManDevice m_pParent;
		private TreeNode m_rootNode;
		private bool m_ininit;
		private Image imgon;
		private Image imgoff;
		private Image imgConflict;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DevManAllDev));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			this.butDeviceSetup = new Button();
			this.dgvAllDevices = new DataGridView();
			this.imgstatus = new DataGridViewImageColumn();
			this.dgvtbDeviceNm = new DataGridViewTextBoxColumn();
			this.dgvtbcMac = new DataGridViewTextBoxColumn();
			this.dgvtbcIp = new DataGridViewTextBoxColumn();
			this.dgvtbcport = new DataGridViewTextBoxColumn();
			this.dgvtbcModel = new DataGridViewTextBoxColumn();
			this.dgvtbcRackNm = new DataGridViewTextBoxColumn();
			this.dgvtbcdeviceId = new DataGridViewTextBoxColumn();
			this.butDevicesDel = new Button();
			this.butAddDevices = new Button();
			this.butSyncThreshold = new Button();
			this.tbFilterKey = new TextBox();
			this.btnFilter = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
			((ISupportInitialize)this.dgvAllDevices).BeginInit();
			base.SuspendLayout();
			this.butDeviceSetup.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butDeviceSetup, "butDeviceSetup");
			this.butDeviceSetup.Name = "butDeviceSetup";
			this.butDeviceSetup.UseVisualStyleBackColor = false;
			this.butDeviceSetup.Click += new System.EventHandler(this.butDeviceSetup_Click);
			this.dgvAllDevices.AllowUserToAddRows = false;
			this.dgvAllDevices.AllowUserToDeleteRows = false;
			this.dgvAllDevices.AllowUserToResizeColumns = false;
			this.dgvAllDevices.AllowUserToResizeRows = false;
			this.dgvAllDevices.BackgroundColor = Color.WhiteSmoke;
			this.dgvAllDevices.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.SelectionForeColor = Color.Black;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvAllDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvAllDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAllDevices.Columns.AddRange(new DataGridViewColumn[]
			{
				this.imgstatus,
				this.dgvtbDeviceNm,
				this.dgvtbcMac,
				this.dgvtbcIp,
				this.dgvtbcport,
				this.dgvtbcModel,
				this.dgvtbcRackNm,
				this.dgvtbcdeviceId
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.White;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = Color.White;
			dataGridViewCellStyle2.SelectionForeColor = Color.Black;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvAllDevices.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvAllDevices.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvAllDevices, "dgvAllDevices");
			this.dgvAllDevices.Name = "dgvAllDevices";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.White;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = Color.White;
			dataGridViewCellStyle3.SelectionForeColor = Color.Black;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvAllDevices.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvAllDevices.RowHeadersVisible = false;
			this.dgvAllDevices.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle4.BackColor = Color.White;
			dataGridViewCellStyle4.ForeColor = Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = Color.Blue;
			dataGridViewCellStyle4.SelectionForeColor = Color.White;
			this.dgvAllDevices.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvAllDevices.RowTemplate.Height = 23;
			this.dgvAllDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAllDevices.StandardTab = true;
			this.dgvAllDevices.TabStop = false;
			this.dgvAllDevices.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvAllDevices_CellDoubleClick);
			this.dgvAllDevices.SelectionChanged += new System.EventHandler(this.dgvAllDevices_SelectionChanged);
			this.dgvAllDevices.SortCompare += new DataGridViewSortCompareEventHandler(this.dgvAllDevices_SortCompare);
			componentResourceManager.ApplyResources(this.imgstatus, "imgstatus");
			this.imgstatus.Name = "imgstatus";
			this.imgstatus.SortMode = DataGridViewColumnSortMode.Automatic;
			dataGridViewCellStyle5.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dgvtbDeviceNm.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dgvtbDeviceNm, "dgvtbDeviceNm");
			this.dgvtbDeviceNm.Name = "dgvtbDeviceNm";
			this.dgvtbDeviceNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcMac, "dgvtbcMac");
			this.dgvtbcMac.Name = "dgvtbcMac";
			this.dgvtbcMac.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcIp, "dgvtbcIp");
			this.dgvtbcIp.Name = "dgvtbcIp";
			this.dgvtbcIp.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcport, "dgvtbcport");
			this.dgvtbcport.Name = "dgvtbcport";
			this.dgvtbcport.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcModel, "dgvtbcModel");
			this.dgvtbcModel.Name = "dgvtbcModel";
			this.dgvtbcModel.ReadOnly = true;
			this.dgvtbcRackNm.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dgvtbcRackNm, "dgvtbcRackNm");
			this.dgvtbcRackNm.Name = "dgvtbcRackNm";
			this.dgvtbcRackNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcdeviceId, "dgvtbcdeviceId");
			this.dgvtbcdeviceId.Name = "dgvtbcdeviceId";
			this.butDevicesDel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butDevicesDel, "butDevicesDel");
			this.butDevicesDel.Name = "butDevicesDel";
			this.butDevicesDel.UseVisualStyleBackColor = false;
			this.butDevicesDel.Click += new System.EventHandler(this.butDevicesDel_Click);
			this.butAddDevices.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butAddDevices, "butAddDevices");
			this.butAddDevices.Name = "butAddDevices";
			this.butAddDevices.UseVisualStyleBackColor = false;
			this.butAddDevices.Click += new System.EventHandler(this.butAddDevices_Click);
			this.butSyncThreshold.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSyncThreshold, "butSyncThreshold");
			this.butSyncThreshold.Name = "butSyncThreshold";
			this.butSyncThreshold.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.tbFilterKey, "tbFilterKey");
			this.tbFilterKey.Name = "tbFilterKey";
			componentResourceManager.ApplyResources(this.btnFilter, "btnFilter");
			this.btnFilter.ForeColor = SystemColors.ControlText;
			this.btnFilter.Image = Resources.filter;
			this.btnFilter.Name = "btnFilter";
			this.btnFilter.UseVisualStyleBackColor = true;
			this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
			dataGridViewCellStyle6.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.btnFilter);
			base.Controls.Add(this.tbFilterKey);
			base.Controls.Add(this.butSyncThreshold);
			base.Controls.Add(this.dgvAllDevices);
			base.Controls.Add(this.butDevicesDel);
			base.Controls.Add(this.butAddDevices);
			base.Controls.Add(this.butDeviceSetup);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DevManAllDev";
			((ISupportInitialize)this.dgvAllDevices).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public DevManAllDev()
		{
			this.InitializeComponent();
			this.imgon = Resources.DeviceOnline;
			this.imgoff = Resources.DeviceOffline;
			this.imgConflict = Resources.DeviceUnmatched;
		}
		public void pageInit(DevManDevice pParent, TreeNode rootNode)
		{
			this.m_pParent = pParent;
			this.m_rootNode = rootNode;
			this.show_devTb();
		}
		public void clearFilter()
		{
			this.tbFilterKey.Text = "";
		}
		private void show_devTb()
		{
			this.m_ininit = true;
			DataGridViewColumn sortedColumn = this.dgvAllDevices.SortedColumn;
			this.dgvAllDevices.Rows.Clear();
			System.Collections.Generic.Dictionary<long, CommParaClass> deviceRackMapping = DeviceOperation.GetDeviceRackMapping();
			System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			foreach (TreeNode treeNode in this.m_rootNode.Nodes)
			{
				string text = treeNode.Text;
				long num = System.Convert.ToInt64(treeNode.Name);
				bool flag = false;
				DeviceInfo deviceInfo = null;
				for (int i = 0; i < allDevice.Count; i++)
				{
					deviceInfo = allDevice[i];
					if ((long)deviceInfo.DeviceID == num)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					string text2 = "";
					IPAddress iPAddress = IPAddress.Parse(deviceInfo.DeviceIP);
					if (deviceRackMapping.ContainsKey(num))
					{
						text2 = deviceRackMapping[num].String_First;
					}
					string text3 = this.tbFilterKey.Text;
					if (text3.Length <= 0 || text.Contains(text3) || deviceInfo.Mac.Contains(text3) || iPAddress.ToString().Contains(text3) || deviceInfo.Port.ToString().Contains(text3) || deviceInfo.ModelNm.Contains(text3) || text2.Contains(text3))
					{
						object[] values;
						if (ClientAPI.IsDeviceOnline(deviceInfo.DeviceID))
						{
							values = new object[]
							{
								this.imgon,
								text,
								deviceInfo.Mac,
								iPAddress,
								deviceInfo.Port,
								deviceInfo.ModelNm,
								text2,
								num.ToString()
							};
						}
						else
						{
							if (ClientAPI.IsMACConflict(deviceInfo.Mac))
							{
								values = new object[]
								{
									this.imgConflict,
									text,
									deviceInfo.Mac,
									iPAddress,
									deviceInfo.Port,
									deviceInfo.ModelNm,
									text2,
									num.ToString()
								};
							}
							else
							{
								values = new object[]
								{
									this.imgoff,
									text,
									deviceInfo.Mac,
									iPAddress,
									deviceInfo.Port,
									deviceInfo.ModelNm,
									text2,
									num.ToString()
								};
							}
						}
						this.dgvAllDevices.Rows.Add(values);
					}
				}
			}
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvAllDevices.Rows)
			{
				DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[0];
				if (dataGridViewCell.Value.Equals(this.imgConflict))
				{
					dataGridViewCell.ToolTipText = EcoLanguage.getMsg(LangRes.tips_MACMismatch, new string[0]);
				}
			}
			if (sortedColumn != null)
			{
				ListSortDirection direction = (this.dgvAllDevices.SortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
				this.dgvAllDevices.Sort(sortedColumn, direction);
			}
			if (this.dgvAllDevices.Rows.Count == 0)
			{
				this.butDevicesDel.Enabled = false;
				this.butDeviceSetup.Enabled = false;
				this.butSyncThreshold.Enabled = false;
			}
			else
			{
				this.butDevicesDel.Enabled = true;
				this.butDeviceSetup.Enabled = true;
				this.butSyncThreshold.Enabled = true;
			}
			this.m_ininit = false;
		}
		public void TimerProc(int haveThresholdChange)
		{
			if (this.m_ininit)
			{
				return;
			}
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvAllDevices.Rows)
			{
				DataGridViewCellCollection cells = dataGridViewRow.Cells;
				string value = cells["dgvtbcdeviceId"].Value.ToString();
				if (ClientAPI.IsDeviceOnline(System.Convert.ToInt32(value)))
				{
					cells["imgstatus"].Value = this.imgon;
					cells["imgstatus"].ToolTipText = null;
				}
				else
				{
					if (ClientAPI.IsMACConflict(System.Convert.ToString(cells["dgvtbcMac"].Value)))
					{
						cells["imgstatus"].Value = this.imgConflict;
						cells["imgstatus"].ToolTipText = EcoLanguage.getMsg(LangRes.tips_MACMismatch, new string[0]);
					}
					else
					{
						cells["imgstatus"].Value = this.imgoff;
						cells["imgstatus"].ToolTipText = null;
					}
				}
				if (haveThresholdChange == 1)
				{
					DeviceInfo deviceByID = DeviceOperation.getDeviceByID(System.Convert.ToInt32(value));
					string text = cells["dgvtbDeviceNm"].Value.ToString();
					if (!text.Equals(deviceByID.DeviceName))
					{
						cells["dgvtbDeviceNm"].Value = deviceByID.DeviceName;
					}
				}
			}
		}
		private void butAddDevices_Click(object sender, System.EventArgs e)
		{
			if (this.dgvAllDevices.Rows.Count >= EcoGlobalVar.gl_maxDevNum)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_MaxNum, new string[]
				{
					EcoGlobalVar.gl_maxDevNum.ToString()
				}));
				return;
			}
			ScanDevDlg scanDevDlg = new ScanDevDlg();
			scanDevDlg.pageInit();
			scanDevDlg.ShowDialog(this);
			string iPs = scanDevDlg.getIPs();
			if (iPs.Length == 0)
			{
				return;
			}
			AddDevDlg addDevDlg = new AddDevDlg();
			addDevDlg.pageInit(scanDevDlg.getScanCompany(), iPs, scanDevDlg.getSNMPpara());
			DialogResult dialogResult = DialogResult.Cancel;
			try
			{
				dialogResult = addDevDlg.ShowDialog(this);
			}
			catch (System.Exception)
			{
			}
			if (dialogResult == DialogResult.OK)
			{
				this.changeTreeSelect("DevRoot");
			}
		}
		private void changeTreeSelect(string devName)
		{
			DevManAllDev.PostMessage(this.m_pParent.Handle, 63000u, 0, devName);
		}
		private void butDevicesDel_Click(object sender, System.EventArgs e)
		{
			if (this.dgvAllDevices.SelectedRows.Count == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_needselect, new string[0]));
				return;
			}
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Dev_delCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
			for (int i = 0; i < this.dgvAllDevices.SelectedRows.Count; i++)
			{
				DataGridViewCellCollection cells = this.dgvAllDevices.SelectedRows[i].Cells;
				string value = cells["dgvtbcdeviceId"].Value.ToString();
				arrayList.Insert(0, value);
			}
			System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
			Program.IdleTimer_Pause(1);
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_delDev, new string[0]), null, new progressPopup.ProcessInThread(this.delDevicePro), arrayList, 0);
			progressPopup.ShowDialog();
			object return_V = progressPopup.Return_V;
			Program.IdleTimer_Run(1);
			int? num = return_V as int?;
			if (!num.HasValue || num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
			System.Collections.ArrayList allRack_NoEmpty2 = RackInfo.GetAllRack_NoEmpty();
			EcoGlobalVar.gl_DevManPage.FlushFlg_RackBoard = 1;
			if (allRack_NoEmpty.Count == allRack_NoEmpty2.Count)
			{
				EcoGlobalVar.setDashBoardFlg(526uL, "", 66);
			}
			else
			{
				EcoGlobalVar.setDashBoardFlg(526uL, "", 65);
			}
			EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
			this.changeTreeSelect("DevRoot");
		}
		private object delDevicePro(object param)
		{
			int num = 1;
			System.Collections.ArrayList arrayList = param as System.Collections.ArrayList;
			DBConn connection = DBConnPool.getConnection();
			for (int i = 0; i < arrayList.Count; i++)
			{
				string value = (string)arrayList[i];
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(System.Convert.ToInt32(value));
				int num2 = DeviceOperation.DeleteDeviceByID(connection, System.Convert.ToInt32(value));
				if (num2 < 0)
				{
					num = num2;
				}
				else
				{
					string valuePair = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair))
					{
						LogAPI.writeEventLog("0430001", new string[]
						{
							deviceByID.ModelNm,
							deviceByID.Mac,
							deviceByID.DeviceIP,
							deviceByID.DeviceName,
							valuePair
						});
					}
					else
					{
						LogAPI.writeEventLog("0430001", new string[]
						{
							deviceByID.ModelNm,
							deviceByID.Mac,
							deviceByID.DeviceIP,
							deviceByID.DeviceName
						});
					}
				}
			}
			connection.Close();
			DeviceOperation.RefreshDBCache(false);
			DeviceOperation.StartDBCleanupThread();
			return num;
		}
		private void butDeviceSetup_Click(object sender, System.EventArgs e)
		{
			string devName = this.dgvAllDevices.CurrentRow.Cells["dgvtbDeviceNm"].Value.ToString();
			this.changeTreeSelect(devName);
		}
		private void dgvAllDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			string devName = this.dgvAllDevices.Rows[rowIndex].Cells["dgvtbDeviceNm"].Value.ToString();
			this.changeTreeSelect(devName);
		}
		private void dgvAllDevices_SelectionChanged(object sender, System.EventArgs e)
		{
			int count = this.dgvAllDevices.SelectedRows.Count;
			if (count == 1)
			{
				this.butDeviceSetup.Enabled = true;
				return;
			}
			this.butDeviceSetup.Enabled = false;
		}
		private void dgvAllDevices_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (e.Column == this.imgstatus)
			{
				int num = this.CompareImg(e.CellValue1, e.CellValue2);
				if (num == 0)
				{
					num = string.Compare(this.dgvAllDevices.Rows[e.RowIndex1].Cells["dgvtbDeviceNm"].Value.ToString(), this.dgvAllDevices.Rows[e.RowIndex2].Cells["dgvtbDeviceNm"].Value.ToString());
				}
				e.SortResult = num;
				e.Handled = true;
			}
		}
		private int CompareImg(object o1, object o2)
		{
			Image image = o1 as Image;
			Image image2 = o2 as Image;
			int num;
			if (image.Equals(this.imgon))
			{
				num = 0;
			}
			else
			{
				if (image.Equals(this.imgoff))
				{
					num = 1;
				}
				else
				{
					num = 2;
				}
			}
			int value;
			if (image2.Equals(this.imgon))
			{
				value = 0;
			}
			else
			{
				if (image2.Equals(this.imgoff))
				{
					value = 1;
				}
				else
				{
					value = 2;
				}
			}
			return num.CompareTo(value);
		}
		private void btnFilter_Click(object sender, System.EventArgs e)
		{
			this.show_devTb();
		}
	}
}
