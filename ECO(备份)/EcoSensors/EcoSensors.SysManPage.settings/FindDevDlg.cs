using DBAccessAPI;
using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class FindDevDlg : Form
	{
		private IContainer components;
		private Button butSel;
		private Button butCancel;
		private DataGridView dgvAllDevices;
		private Button butrefresh;
		private RadioButton rbonlinedev;
		private RadioButton rballev;
		private DataGridViewTextBoxColumn dgvtbDeviceNm;
		private DataGridViewTextBoxColumn dgvtbcMac;
		private DataGridViewTextBoxColumn dgvtbcIp;
		private DataGridViewTextBoxColumn dgvtbcport;
		private DataGridViewTextBoxColumn dgvtbcModel;
		private DataGridViewTextBoxColumn Voltage;
		private DataGridViewTextBoxColumn dgvtbcRackNm;
		private DataGridViewTextBoxColumn deviceId;
		private int m_devID = -1;
		private string stronlineDevText = "";
		private string strallDevText = "";
		public int DevID
		{
			get
			{
				return this.m_devID;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FindDevDlg));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			this.butSel = new Button();
			this.butCancel = new Button();
			this.dgvAllDevices = new DataGridView();
			this.dgvtbDeviceNm = new DataGridViewTextBoxColumn();
			this.dgvtbcMac = new DataGridViewTextBoxColumn();
			this.dgvtbcIp = new DataGridViewTextBoxColumn();
			this.dgvtbcport = new DataGridViewTextBoxColumn();
			this.dgvtbcModel = new DataGridViewTextBoxColumn();
			this.Voltage = new DataGridViewTextBoxColumn();
			this.dgvtbcRackNm = new DataGridViewTextBoxColumn();
			this.deviceId = new DataGridViewTextBoxColumn();
			this.butrefresh = new Button();
			this.rbonlinedev = new RadioButton();
			this.rballev = new RadioButton();
			((ISupportInitialize)this.dgvAllDevices).BeginInit();
			base.SuspendLayout();
			this.butSel.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.butSel, "butSel");
			this.butSel.Name = "butSel";
			this.butSel.UseVisualStyleBackColor = false;
			this.butSel.Click += new System.EventHandler(this.butSel_Click);
			this.butCancel.BackColor = SystemColors.Control;
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = false;
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
				this.dgvtbDeviceNm,
				this.dgvtbcMac,
				this.dgvtbcIp,
				this.dgvtbcport,
				this.dgvtbcModel,
				this.Voltage,
				this.dgvtbcRackNm,
				this.deviceId
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
			this.dgvAllDevices.MultiSelect = false;
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
			this.dgvAllDevices.RowTemplate.ReadOnly = true;
			this.dgvAllDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAllDevices.StandardTab = true;
			this.dgvAllDevices.TabStop = false;
			this.dgvAllDevices.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvAllDevices_CellDoubleClick);
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
			componentResourceManager.ApplyResources(this.Voltage, "Voltage");
			this.Voltage.Name = "Voltage";
			this.dgvtbcRackNm.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dgvtbcRackNm, "dgvtbcRackNm");
			this.dgvtbcRackNm.Name = "dgvtbcRackNm";
			this.dgvtbcRackNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.deviceId, "deviceId");
			this.deviceId.Name = "deviceId";
			this.deviceId.ReadOnly = true;
			this.butrefresh.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.butrefresh, "butrefresh");
			this.butrefresh.Name = "butrefresh";
			this.butrefresh.UseVisualStyleBackColor = false;
			this.butrefresh.Click += new System.EventHandler(this.butrefresh_Click);
			componentResourceManager.ApplyResources(this.rbonlinedev, "rbonlinedev");
			this.rbonlinedev.Name = "rbonlinedev";
			this.rbonlinedev.TabStop = true;
			this.rbonlinedev.UseVisualStyleBackColor = true;
			this.rbonlinedev.CheckedChanged += new System.EventHandler(this.rbonlinedev_CheckedChanged);
			componentResourceManager.ApplyResources(this.rballev, "rballev");
			this.rballev.Name = "rballev";
			this.rballev.TabStop = true;
			this.rballev.UseVisualStyleBackColor = true;
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.rballev);
			base.Controls.Add(this.rbonlinedev);
			base.Controls.Add(this.butrefresh);
			base.Controls.Add(this.dgvAllDevices);
			base.Controls.Add(this.butSel);
			base.Controls.Add(this.butCancel);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FindDevDlg";
			base.ShowInTaskbar = false;
			((ISupportInitialize)this.dgvAllDevices).EndInit();
			base.ResumeLayout(false);
		}
		public FindDevDlg()
		{
			this.InitializeComponent();
			this.stronlineDevText = this.rbonlinedev.Text;
			this.strallDevText = this.rballev.Text;
		}
		public FindDevDlg(int devID)
		{
			this.InitializeComponent();
			this.stronlineDevText = this.rbonlinedev.Text;
			this.strallDevText = this.rballev.Text;
			this.m_devID = devID;
			this.rbonlinedev.Checked = true;
		}
		private void pageInit()
		{
			this.dgvAllDevices.Rows.Clear();
			System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			int num = -1;
			DataSet dataSet = ClientAPI.getDataSet(0);
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < allDevice.Count; i++)
			{
				DeviceInfo deviceInfo = allDevice[i];
				string deviceName = deviceInfo.DeviceName;
				if (!deviceInfo.ModelNm.Equals("EC1000") && !deviceInfo.ModelNm.Equals("EC2004"))
				{
					num4++;
					DataRow[] array;
					if (dataSet == null)
					{
						array = null;
					}
					else
					{
						array = dataSet.Tables[0].Select("device_id = " + System.Convert.ToString(deviceInfo.DeviceID));
					}
					string text = "";
					if (ClientAPI.IsDeviceOnline(deviceInfo.DeviceID))
					{
						num3++;
						try
						{
							text = System.Convert.ToSingle(array[0]["voltage_value"]).ToString("F2");
							goto IL_FE;
						}
						catch
						{
							goto IL_FE;
						}
					}
					if (this.rbonlinedev.Checked)
					{
						goto IL_19E;
					}
					IL_FE:
					RackInfo rackByID = RackInfo.getRackByID(deviceInfo.RackID);
					string[] values = new string[]
					{
						deviceName,
						deviceInfo.Mac,
						deviceInfo.DeviceIP,
						deviceInfo.Port.ToString(),
						deviceInfo.ModelNm,
						text,
						rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag),
						deviceInfo.DeviceID.ToString()
					};
					this.dgvAllDevices.Rows.Add(values);
					if (deviceInfo.DeviceID == this.m_devID)
					{
						num = num2;
					}
					num2++;
				}
				IL_19E:;
			}
			this.rbonlinedev.Text = this.stronlineDevText + " (" + num3.ToString() + ")";
			this.rballev.Text = this.strallDevText + " (" + num4.ToString() + ")";
			if (num >= 0)
			{
				this.dgvAllDevices.CurrentCell = this.dgvAllDevices.Rows[num].Cells[0];
			}
		}
		private void butSel_Click(object sender, System.EventArgs e)
		{
			DataGridViewSelectedRowCollection selectedRows = this.dgvAllDevices.SelectedRows;
			if (selectedRows.Count <= 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_selectneed, new string[0]));
				return;
			}
			DataGridViewRow dataGridViewRow = selectedRows[0];
			string value = dataGridViewRow.Cells["deviceId"].Value.ToString();
			this.m_devID = System.Convert.ToInt32(value);
			if (!ClientAPI.IsDeviceOnline(this.m_devID))
			{
				DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Comm_devofflineCrm, new string[0]), MessageBoxButtons.OKCancel);
				if (dialogResult == DialogResult.Cancel)
				{
					return;
				}
			}
			base.DialogResult = DialogResult.OK;
		}
		private void dgvAllDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			string value = this.dgvAllDevices.Rows[rowIndex].Cells["deviceId"].Value.ToString();
			if (!ClientAPI.IsDeviceOnline(System.Convert.ToInt32(value)))
			{
				DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Comm_devofflineCrm, new string[0]), MessageBoxButtons.OKCancel);
				if (dialogResult == DialogResult.Cancel)
				{
					return;
				}
			}
			this.m_devID = System.Convert.ToInt32(value);
			base.DialogResult = DialogResult.OK;
		}
		private void butrefresh_Click(object sender, System.EventArgs e)
		{
			this.pageInit();
		}
		private void rbonlinedev_CheckedChanged(object sender, System.EventArgs e)
		{
			this.pageInit();
		}
	}
}
