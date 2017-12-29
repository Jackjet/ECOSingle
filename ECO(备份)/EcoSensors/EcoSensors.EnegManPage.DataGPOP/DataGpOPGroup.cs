using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.DataGPOP
{
	public class DataGpOPGroup : UserControl
	{
		private IContainer components;
		private GroupBox groupBoxPowerControl;
		private CheckBox cbDReboot;
		private Button butOn;
		private Button butOff;
		private GroupBox groupBoxOutletsInfo;
		private Label nonOPoutletNum;
		private Label canOPoutletNum;
		private Label lbNum_noOp;
		private Label lbNum_canOp;
		private Label lbNoOp;
		private Label labInfo;
		private DataGridView dgvInfo;
		private DataGpOPAll m_parent;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataGpOPGroup));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.groupBoxPowerControl = new GroupBox();
			this.cbDReboot = new CheckBox();
			this.butOn = new Button();
			this.butOff = new Button();
			this.groupBoxOutletsInfo = new GroupBox();
			this.nonOPoutletNum = new Label();
			this.canOPoutletNum = new Label();
			this.lbNum_noOp = new Label();
			this.lbNum_canOp = new Label();
			this.lbNoOp = new Label();
			this.labInfo = new Label();
			this.dgvInfo = new DataGridView();
			this.groupBoxPowerControl.SuspendLayout();
			this.groupBoxOutletsInfo.SuspendLayout();
			((ISupportInitialize)this.dgvInfo).BeginInit();
			base.SuspendLayout();
			this.groupBoxPowerControl.Controls.Add(this.cbDReboot);
			this.groupBoxPowerControl.Controls.Add(this.butOn);
			this.groupBoxPowerControl.Controls.Add(this.butOff);
			componentResourceManager.ApplyResources(this.groupBoxPowerControl, "groupBoxPowerControl");
			this.groupBoxPowerControl.Name = "groupBoxPowerControl";
			this.groupBoxPowerControl.TabStop = false;
			componentResourceManager.ApplyResources(this.cbDReboot, "cbDReboot");
			this.cbDReboot.Name = "cbDReboot";
			this.cbDReboot.UseVisualStyleBackColor = true;
			this.butOn.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butOn, "butOn");
			this.butOn.Name = "butOn";
			this.butOn.UseVisualStyleBackColor = false;
			this.butOn.Click += new System.EventHandler(this.butOn_Click);
			this.butOff.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butOff, "butOff");
			this.butOff.Name = "butOff";
			this.butOff.UseVisualStyleBackColor = false;
			this.butOff.Click += new System.EventHandler(this.butOff_Click);
			this.groupBoxOutletsInfo.Controls.Add(this.nonOPoutletNum);
			this.groupBoxOutletsInfo.Controls.Add(this.canOPoutletNum);
			this.groupBoxOutletsInfo.Controls.Add(this.lbNum_noOp);
			this.groupBoxOutletsInfo.Controls.Add(this.lbNum_canOp);
			componentResourceManager.ApplyResources(this.groupBoxOutletsInfo, "groupBoxOutletsInfo");
			this.groupBoxOutletsInfo.Name = "groupBoxOutletsInfo";
			this.groupBoxOutletsInfo.TabStop = false;
			this.nonOPoutletNum.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.nonOPoutletNum, "nonOPoutletNum");
			this.nonOPoutletNum.ForeColor = Color.Black;
			this.nonOPoutletNum.Name = "nonOPoutletNum";
			this.canOPoutletNum.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.canOPoutletNum, "canOPoutletNum");
			this.canOPoutletNum.ForeColor = Color.Black;
			this.canOPoutletNum.Name = "canOPoutletNum";
			componentResourceManager.ApplyResources(this.lbNum_noOp, "lbNum_noOp");
			this.lbNum_noOp.ForeColor = Color.Black;
			this.lbNum_noOp.Name = "lbNum_noOp";
			componentResourceManager.ApplyResources(this.lbNum_canOp, "lbNum_canOp");
			this.lbNum_canOp.ForeColor = Color.Black;
			this.lbNum_canOp.Name = "lbNum_canOp";
			componentResourceManager.ApplyResources(this.lbNoOp, "lbNoOp");
			this.lbNoOp.ForeColor = Color.Black;
			this.lbNoOp.Name = "lbNoOp";
			componentResourceManager.ApplyResources(this.labInfo, "labInfo");
			this.labInfo.ForeColor = Color.Blue;
			this.labInfo.Name = "labInfo";
			this.dgvInfo.AllowUserToAddRows = false;
			this.dgvInfo.AllowUserToDeleteRows = false;
			this.dgvInfo.AllowUserToResizeColumns = false;
			this.dgvInfo.AllowUserToResizeRows = false;
			this.dgvInfo.BackgroundColor = Color.White;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvInfo, "dgvInfo");
			this.dgvInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvInfo.MultiSelect = false;
			this.dgvInfo.Name = "dgvInfo";
			this.dgvInfo.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvInfo.RowHeadersVisible = false;
			this.dgvInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvInfo.RowTemplate.Height = 23;
			this.dgvInfo.RowTemplate.ReadOnly = true;
			this.dgvInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvInfo.StandardTab = true;
			this.dgvInfo.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBoxPowerControl);
			base.Controls.Add(this.groupBoxOutletsInfo);
			base.Controls.Add(this.lbNoOp);
			base.Controls.Add(this.labInfo);
			base.Controls.Add(this.dgvInfo);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DataGpOPGroup";
			this.groupBoxPowerControl.ResumeLayout(false);
			this.groupBoxPowerControl.PerformLayout();
			this.groupBoxOutletsInfo.ResumeLayout(false);
			((ISupportInitialize)this.dgvInfo).EndInit();
			base.ResumeLayout(false);
		}
		public DataGpOPGroup()
		{
			this.InitializeComponent();
		}
		public void pageInit(string strGPID, int from, DataGpOPAll parent)
		{
			this.m_parent = parent;
			if (strGPID.Length == 0)
			{
				this.groupBoxOutletsInfo.Visible = false;
				this.nonOPoutletNum.Text = "0";
				this.canOPoutletNum.Text = "0";
				this.groupBoxPowerControl.Visible = false;
				this.labInfo.Visible = false;
				this.dgvInfo.DataSource = null;
				this.dgvInfo.Visible = false;
				this.lbNoOp.Visible = true;
				return;
			}
			this.groupBoxOutletsInfo.Visible = true;
			this.labInfo.Visible = true;
			this.dgvInfo.Visible = true;
			this.lbNoOp.Visible = false;
			long gpID = (long)System.Convert.ToInt32(strGPID);
			this.showOutletInfo(gpID);
		}
		private void showOutletInfo(long gpID)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("devID", typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.OPDataGp_InfoC1No, new string[0]), typeof(int));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Comm_name, new string[0]), typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_TPOutlet, new string[0]), typeof(int));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.OPDataGp_Status, new string[0]), typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
			DataSet dataSet = ClientAPI.getDataSet(0);
			if (dataSet == null)
			{
				return;
			}
			DataTable onlineOutlets = dataSet.Tables[2];
			GroupInfo groupInfo = null;
			System.Collections.Generic.List<GroupInfo> groupCopy = ClientAPI.getGroupCopy();
			foreach (GroupInfo current in groupCopy)
			{
				if (current.ID == gpID)
				{
					groupInfo = current;
					break;
				}
			}
			if (groupInfo == null)
			{
				return;
			}
			string groupType = groupInfo.GroupType;
			string text = groupInfo.Members;
			if (text == null || text.Length == 0)
			{
				text = "-1";
			}
			System.DateTime now = System.DateTime.Now;
			commUtil.ShowInfo_DEBUG("Start GetDevicRackZoneRelation ----------------" + now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			System.DateTime now2 = System.DateTime.Now;
			System.TimeSpan timeSpan = now2 - now;
			commUtil.ShowInfo_DEBUG(string.Concat(new object[]
			{
				"End   GetDevicRackZoneRelation ----------------",
				now2.ToString("yyyy/MM/dd HH:mm:ss:fff"),
				" spend=",
				timeSpan.TotalMilliseconds
			}));
			string ssss = "device_id";
			string key;
			switch (key = groupType)
			{
			case "zone":
			{
				string text2 = ClientAPI.getRacklistByZonelist(text);
				text2 = commUtil.uniqueIDs(text2);
				text = "";
				if (text2.Length > 0)
				{
					text2 = text2.Substring(0, text2.Length - 1);
					DataRow[] dataRows = ClientAPI.getDataRows(0, "rack_id in (" + text2 + ")", "");
					DataRow[] array = dataRows;
					for (int i = 0; i < array.Length; i++)
					{
						DataRow dataRow = array[i];
						text = text + dataRow["device_id"] + ",";
					}
					text = commUtil.uniqueIDs(text);
				}
				if (text.Length > 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
				ssss = "device_id";
				break;
			}
			case "rack":
			case "allrack":
			{
				DataRow[] dataRows = ClientAPI.getDataRows(0, "rack_id in (" + text + ")", "");
				text = "";
				DataRow[] array2 = dataRows;
				for (int j = 0; j < array2.Length; j++)
				{
					DataRow dataRow2 = array2[j];
					text = text + dataRow2["device_id"] + ",";
				}
				text = commUtil.uniqueIDs(text);
				if (text.Length > 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
				ssss = "device_id";
				break;
			}
			case "dev":
			case "alldev":
				ssss = "device_id";
				break;
			case "outlet":
			case "alloutlet":
				ssss = "port_id";
				break;
			}
			DataTable oPOutlets = this.getOPOutlets(onlineOutlets, ssss, text);
			string text3 = "";
			string text4 = "";
			DataGridViewCell currentCell = this.dgvInfo.CurrentCell;
			if (currentCell != null)
			{
				int rowIndex = currentCell.RowIndex;
				text3 = this.dgvInfo.Rows[rowIndex].Cells[0].Value.ToString();
				text4 = this.dgvInfo.Rows[rowIndex].Cells[3].Value.ToString();
			}
			foreach (DataRow dataRow3 in oPOutlets.Rows)
			{
				string text5 = dataRow3["device_id"].ToString();
				if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(text5)))
				{
					ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(text5)];
					string device_model = deviceWithZoneRackInfo.device_model;
					string fw_version = deviceWithZoneRackInfo.fw_version;
					DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model, fw_version);
					if (deviceModelConfig.switchableOutlets != 0uL)
					{
						int num2 = System.Convert.ToInt32(dataRow3["port_number"].ToString());
						if (deviceModelConfig.isOutletSwitchable(num2 - 1))
						{
							if (dataRow3["port_nm"].ToString().Equals(System.Convert.ToString(-500)) || dataRow3["port_nm"].ToString().Equals(System.Convert.ToString(-1000)))
							{
								dataRow3["port_nm"] = "";
							}
							string text6 = dataRow3["port_state"].ToString();
							if (text6.Equals(OutletStatus.OFF.ToString()))
							{
								dataRow3["port_state"] = EcoLanguage.getMsg(LangRes.OPST_OFF, new string[0]);
							}
							else
							{
								if (text6.Equals(OutletStatus.ON.ToString()))
								{
									dataRow3["port_state"] = EcoLanguage.getMsg(LangRes.OPST_ON, new string[0]);
								}
								else
								{
									if (text6.Equals(OutletStatus.Pending.ToString()))
									{
										dataRow3["port_state"] = EcoLanguage.getMsg(LangRes.OPST_PENDING, new string[0]);
									}
									else
									{
										if (text6.Equals(OutletStatus.Fault.ToString()))
										{
											dataRow3["port_state"] = EcoLanguage.getMsg(LangRes.OPST_FAULT, new string[0]);
										}
									}
								}
							}
							string text7 = dataRow3["port_nm"].ToString();
							if (text7.Equals("\0"))
							{
								dataRow3["port_nm"] = string.Empty;
							}
							dataTable.Rows.Add(new object[]
							{
								text5,
								0,
								dataRow3["port_nm"],
								dataRow3["port_number"],
								dataRow3["port_state"],
								deviceWithZoneRackInfo.device_nm,
								deviceWithZoneRackInfo.rack_nm,
								deviceWithZoneRackInfo.zone_list
							});
						}
					}
				}
			}
			dataTable = new DataView(dataTable)
			{
				Sort = EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]) + " ASC, " + EcoLanguage.getMsg(LangRes.Group_TPOutlet, new string[0]) + " ASC"
			}.ToTable();
			int num3 = -1;
			int num4 = 0;
			foreach (DataRow dataRow4 in dataTable.Rows)
			{
				num4++;
				dataRow4[1] = num4;
				string text5 = (string)dataRow4[0];
				if (text3.Equals(text5) && text4.Equals(dataRow4[3].ToString()))
				{
					num3 = num4 - 1;
				}
			}
			this.dgvInfo.DataSource = null;
			this.dgvInfo.DataSource = dataTable;
			this.dgvInfo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.dgvInfo.Columns[0].Visible = false;
			this.dgvInfo.Columns[1].Width = 60;
			this.dgvInfo.Columns[2].Width = 150;
			this.dgvInfo.Columns[3].Width = 40;
			this.dgvInfo.Columns[4].Width = 45;
			this.dgvInfo.Columns[5].Width = 150;
			this.dgvInfo.Columns[6].Width = 130;
			this.dgvInfo.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			int count = oPOutlets.Rows.Count;
			this.nonOPoutletNum.Text = System.Convert.ToString(count - num4);
			this.canOPoutletNum.Text = System.Convert.ToString(num4);
			if (num4 > 0)
			{
				this.groupBoxPowerControl.Visible = true;
				this.butOn.Enabled = EcoGlobalVar.flgEnablePower;
				this.butOff.Enabled = EcoGlobalVar.flgEnablePower;
				this.cbDReboot.Enabled = EcoGlobalVar.flgEnablePower;
			}
			else
			{
				this.groupBoxPowerControl.Visible = false;
			}
			if (num3 >= 0)
			{
				this.dgvInfo.CurrentCell = this.dgvInfo.Rows[num3].Cells[2];
			}
		}
		private DataTable getOPOutlets(DataTable onlineOutlets, string ssss, string objIDs_ingp)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			if (objIDs_ingp.Length == 0)
			{
				objIDs_ingp = "-1";
			}
			string[] array = objIDs_ingp.Split(new string[]
			{
				","
			}, System.StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				dictionary.Add(text, text);
			}
			DataTable dataTable = onlineOutlets.Clone();
			foreach (DataRow dataRow in onlineOutlets.Rows)
			{
				string text2 = System.Convert.ToString(dataRow["device_id"]);
				if (ClientAPI.IsDeviceOnline(System.Convert.ToInt32(text2)))
				{
					text2 = System.Convert.ToString(dataRow[ssss]);
					if (dictionary.ContainsKey(text2))
					{
						DataRow dataRow2 = dataTable.NewRow();
						dataRow2.ItemArray = dataRow.ItemArray;
						dataTable.Rows.Add(dataRow2);
					}
				}
			}
			return dataTable;
		}
		private void butOn_Click(object sender, System.EventArgs e)
		{
			DataTable dataTable = (DataTable)this.dgvInfo.DataSource;
			System.Collections.Generic.List<DevSnmpConfig> list = new System.Collections.Generic.List<DevSnmpConfig>();
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> dictionary = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>>();
			this.m_parent.endTimer();
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_on, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				this.m_parent.starTimer();
				return;
			}
			foreach (DataRow dataRow in dataTable.Rows)
			{
				string text = dataRow[0].ToString();
				string value = dataRow[3].ToString();
				if (dictionary.ContainsKey(text))
				{
					System.Collections.Generic.List<int> list2 = dictionary[text];
					list2.Add(System.Convert.ToInt32(value));
				}
				else
				{
					dictionary.Add(text, new System.Collections.Generic.List<int>
					{
						System.Convert.ToInt32(value)
					});
				}
			}
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>>.Enumerator enumerator2 = dictionary.GetEnumerator();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			while (enumerator2.MoveNext())
			{
				System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> current = enumerator2.Current;
				string text = current.Key;
				stringBuilder.Append(text + ",");
			}
			string text2 = stringBuilder.ToString();
			if (text2.Length > 1)
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			System.Collections.Generic.List<DeviceInfo> list3 = (System.Collections.Generic.List<DeviceInfo>)ClientAPI.RemoteCall(7, 1, text2, 10000);
			if (list3 == null)
			{
				list3 = new System.Collections.Generic.List<DeviceInfo>();
			}
			foreach (DeviceInfo current2 in list3)
			{
				string text = current2.DeviceID.ToString();
				DevSnmpConfig sNMPpara = commUtil.getSNMPpara(current2);
				sNMPpara.groupOutlets = dictionary[text];
				list.Add(sNMPpara);
			}
			DevPortGroupAPI devPortGroupAPI = new DevPortGroupAPI(list);
			bool flag = devPortGroupAPI.TurnOnGroupOutlets();
			if (flag)
			{
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			}
			else
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
			this.m_parent.starTimer();
		}
		private void butOff_Click(object sender, System.EventArgs e)
		{
			DataTable dataTable = (DataTable)this.dgvInfo.DataSource;
			System.Collections.Generic.List<DevSnmpConfig> list = new System.Collections.Generic.List<DevSnmpConfig>();
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> dictionary = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>>();
			this.m_parent.endTimer();
			DialogResult dialogResult;
			if (this.cbDReboot.Checked)
			{
				dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_reboot, new string[0]), MessageBoxButtons.OKCancel);
			}
			else
			{
				dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.OPCrm_off, new string[0]), MessageBoxButtons.OKCancel);
			}
			if (dialogResult == DialogResult.Cancel)
			{
				this.m_parent.starTimer();
				return;
			}
			foreach (DataRow dataRow in dataTable.Rows)
			{
				string text = dataRow[0].ToString();
				string value = dataRow[3].ToString();
				if (dictionary.ContainsKey(text))
				{
					System.Collections.Generic.List<int> list2 = dictionary[text];
					list2.Add(System.Convert.ToInt32(value));
				}
				else
				{
					dictionary.Add(text, new System.Collections.Generic.List<int>
					{
						System.Convert.ToInt32(value)
					});
				}
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> current in dictionary)
			{
				string text = current.Key;
				stringBuilder.Append(text + ",");
			}
			string text2 = stringBuilder.ToString();
			if (text2.Length > 1)
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			System.Collections.Generic.List<DeviceInfo> list3 = (System.Collections.Generic.List<DeviceInfo>)ClientAPI.RemoteCall(7, 1, text2, 10000);
			if (list3 == null)
			{
				list3 = new System.Collections.Generic.List<DeviceInfo>();
			}
			foreach (DeviceInfo current2 in list3)
			{
				string text = current2.DeviceID.ToString();
				DevSnmpConfig sNMPpara = commUtil.getSNMPpara(current2);
				sNMPpara.groupOutlets = dictionary[text];
				list.Add(sNMPpara);
			}
			DevPortGroupAPI devPortGroupAPI = new DevPortGroupAPI(list);
			bool flag;
			if (this.cbDReboot.Checked)
			{
				flag = devPortGroupAPI.RebootGroupOutlets();
			}
			else
			{
				flag = devPortGroupAPI.TurnOffGroupOutlets();
			}
			if (flag)
			{
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			}
			else
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
			this.m_parent.starTimer();
		}
	}
}
