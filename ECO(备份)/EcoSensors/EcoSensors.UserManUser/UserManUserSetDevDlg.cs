using DBAccessAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace EcoSensors.UserManUser
{
	public class UserManUserSetDevDlg : Form
	{
		private string m_deviceIds = "";
		private string m_groupIds = "";
		private bool cbselAllDev_changeonly;
		private bool cbselAllGp_changeonly;
		private IContainer components;
		private Button butSaveDev;
		private DataGridView dgvAllDevices;
		private Button butCancelDev;
		private CheckBox cbSelAllDev;
		private TabControl accTC;
		private TabPage tpDevice;
		private TabPage tpGroup;
		private CheckBox cbSelAllGp;
		private Button btnCancelGp;
		private Button btnSaveGp;
		private DataGridView dgvAllGroups;
		private DataGridViewCheckBoxColumn dgvcbSelectDev;
		private DataGridViewTextBoxColumn dgvtbDeviceNm;
		private DataGridViewTextBoxColumn dgvtbcMac;
		private DataGridViewTextBoxColumn dgvtbcIp;
		private DataGridViewTextBoxColumn dgvtbcModel;
		private DataGridViewTextBoxColumn dgvtbcRackNm;
		private DataGridViewTextBoxColumn deviceId;
		private DataGridViewCheckBoxColumn dgvcbSelectGp;
		private DataGridViewTextBoxColumn groupNm;
		private DataGridViewTextBoxColumn groupType;
		private DataGridViewTextBoxColumn groupId;
		public string accDevIDs
		{
			get
			{
				return this.m_deviceIds;
			}
		}
		public string accGroupIDs
		{
			get
			{
				return this.m_groupIds;
			}
		}
		public UserManUserSetDevDlg()
		{
			this.InitializeComponent();
		}
		public UserManUserSetDevDlg(string deviceIds, string groupIds)
		{
			this.InitializeComponent();
			this.cbselAllDev_changeonly = false;
			this.cbselAllGp_changeonly = false;
			this.m_deviceIds = deviceIds;
			this.m_groupIds = groupIds;
			this.tvUserDeviceInit();
			this.tvUserGroupInit();
		}
		private void tvUserDeviceInit()
		{
			this.dgvAllDevices.Rows.Clear();
			string[] source = this.m_deviceIds.Split(new char[]
			{
				','
			});
			System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			for (int i = 0; i < allDevice.Count; i++)
			{
				DeviceInfo deviceInfo = allDevice[i];
				string deviceIP = deviceInfo.DeviceIP;
				string mac = deviceInfo.Mac;
				string deviceName = deviceInfo.DeviceName;
				string modelNm = deviceInfo.ModelNm;
				RackInfo rackByID = RackInfo.getRackByID(deviceInfo.RackID);
				string displayRackName = rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
				string text = deviceInfo.DeviceID.ToString();
				if (source.Contains(text))
				{
					this.dgvAllDevices.Rows.Add(new object[]
					{
						true,
						deviceName,
						mac,
						deviceIP,
						modelNm,
						displayRackName,
						text
					});
				}
				else
				{
					this.dgvAllDevices.Rows.Add(new object[]
					{
						false,
						deviceName,
						mac,
						deviceIP,
						modelNm,
						displayRackName,
						text
					});
				}
			}
		}
		private void tvUserGroupInit()
		{
			this.dgvAllGroups.Rows.Clear();
			string[] source = this.m_groupIds.Split(new char[]
			{
				','
			});
			System.Collections.Generic.List<GroupInfo> partGroup = GroupInfo.GetPartGroup(2);
			for (int i = 0; i < partGroup.Count; i++)
			{
				GroupInfo groupInfo = partGroup[i];
				string groupName = groupInfo.GroupName;
				string text = groupInfo.GroupType;
				string text2 = groupInfo.ID.ToString();
				if (source.Contains(text2))
				{
					this.dgvAllGroups.Rows.Add(new object[]
					{
						true,
						groupName,
						text,
						text2
					});
				}
				else
				{
					this.dgvAllGroups.Rows.Add(new object[]
					{
						false,
						groupName,
						text,
						text2
					});
				}
			}
		}
		private void cbSelAllDev_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbselAllDev_changeonly)
			{
				this.cbselAllDev_changeonly = false;
				return;
			}
			if (this.cbSelAllDev.Checked)
			{
				for (int i = 0; i < this.dgvAllDevices.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dgvAllDevices.Rows.SharedRow(i).Cells;
					cells[0].Value = true;
				}
				return;
			}
			for (int j = 0; j < this.dgvAllDevices.Rows.Count; j++)
			{
				DataGridViewCellCollection cells2 = this.dgvAllDevices.Rows.SharedRow(j).Cells;
				cells2[0].Value = false;
			}
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			string text = string.Empty;
			for (int i = 0; i < this.dgvAllDevices.Rows.Count; i++)
			{
				DataGridViewCellCollection cells = this.dgvAllDevices.Rows.SharedRow(i).Cells;
				if (System.Convert.ToBoolean(cells[0].Value))
				{
					text = text + cells["deviceId"].Value.ToString() + ",";
				}
			}
			if (!text.Equals(string.Empty))
			{
				text = text.Substring(0, text.Length - 1);
			}
			this.m_deviceIds = text;
			string text2 = string.Empty;
			for (int j = 0; j < this.dgvAllGroups.Rows.Count; j++)
			{
				DataGridViewCellCollection cells2 = this.dgvAllGroups.Rows.SharedRow(j).Cells;
				if (System.Convert.ToBoolean(cells2[0].Value))
				{
					text2 = text2 + cells2["groupId"].Value.ToString() + ",";
				}
			}
			if (!text2.Equals(string.Empty))
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			this.m_groupIds = text2;
		}
		private void dgvAllDevices_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dgvAllDevices.IsCurrentCellDirty)
			{
				this.dgvAllDevices.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void dgvAllDevices_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
			{
				return;
			}
			if (e.ColumnIndex == 0)
			{
				try
				{
					DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)this.dgvAllDevices.Rows[e.RowIndex].Cells[0];
					if ((bool)dataGridViewCheckBoxCell.Value)
					{
						for (int i = 0; i < this.dgvAllDevices.Rows.Count; i++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dgvAllDevices.Rows[i].Cells[0];
							if (!(bool)dataGridViewCheckBoxCell2.Value)
							{
								return;
							}
						}
						if (!this.cbSelAllDev.Checked)
						{
							this.cbselAllDev_changeonly = true;
							this.cbSelAllDev.Checked = true;
						}
					}
					else
					{
						if (this.cbSelAllDev.Checked)
						{
							this.cbselAllDev_changeonly = true;
							this.cbSelAllDev.Checked = false;
						}
					}
				}
				catch (System.Exception)
				{
				}
			}
		}
		private void cbSelAllDev_Paint(object sender, PaintEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox.Focused)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
			}
		}
		private void cbSelAllGp_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbselAllGp_changeonly)
			{
				this.cbselAllGp_changeonly = false;
				return;
			}
			if (this.cbSelAllGp.Checked)
			{
				for (int i = 0; i < this.dgvAllGroups.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dgvAllGroups.Rows.SharedRow(i).Cells;
					cells[0].Value = true;
				}
				return;
			}
			for (int j = 0; j < this.dgvAllGroups.Rows.Count; j++)
			{
				DataGridViewCellCollection cells2 = this.dgvAllGroups.Rows.SharedRow(j).Cells;
				cells2[0].Value = false;
			}
		}
		private void dgvAllGroups_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dgvAllGroups.IsCurrentCellDirty)
			{
				this.dgvAllGroups.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void dgvAllGroups_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
			{
				return;
			}
			if (e.ColumnIndex == 0)
			{
				try
				{
					DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)this.dgvAllGroups.Rows[e.RowIndex].Cells[0];
					if ((bool)dataGridViewCheckBoxCell.Value)
					{
						for (int i = 0; i < this.dgvAllGroups.Rows.Count; i++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dgvAllGroups.Rows[i].Cells[0];
							if (!(bool)dataGridViewCheckBoxCell2.Value)
							{
								return;
							}
						}
						if (!this.cbSelAllGp.Checked)
						{
							this.cbselAllGp_changeonly = true;
							this.cbSelAllGp.Checked = true;
						}
					}
					else
					{
						if (this.cbSelAllGp.Checked)
						{
							this.cbselAllGp_changeonly = true;
							this.cbSelAllGp.Checked = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserManUserSetDevDlg));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
			this.butSaveDev = new Button();
			this.dgvAllDevices = new DataGridView();
			this.dgvcbSelectDev = new DataGridViewCheckBoxColumn();
			this.dgvtbDeviceNm = new DataGridViewTextBoxColumn();
			this.dgvtbcMac = new DataGridViewTextBoxColumn();
			this.dgvtbcIp = new DataGridViewTextBoxColumn();
			this.dgvtbcModel = new DataGridViewTextBoxColumn();
			this.dgvtbcRackNm = new DataGridViewTextBoxColumn();
			this.deviceId = new DataGridViewTextBoxColumn();
			this.butCancelDev = new Button();
			this.cbSelAllDev = new CheckBox();
			this.accTC = new TabControl();
			this.tpDevice = new TabPage();
			this.tpGroup = new TabPage();
			this.cbSelAllGp = new CheckBox();
			this.btnCancelGp = new Button();
			this.btnSaveGp = new Button();
			this.dgvAllGroups = new DataGridView();
			this.dgvcbSelectGp = new DataGridViewCheckBoxColumn();
			this.groupNm = new DataGridViewTextBoxColumn();
			this.groupType = new DataGridViewTextBoxColumn();
			this.groupId = new DataGridViewTextBoxColumn();
			((ISupportInitialize)this.dgvAllDevices).BeginInit();
			this.accTC.SuspendLayout();
			this.tpDevice.SuspendLayout();
			this.tpGroup.SuspendLayout();
			((ISupportInitialize)this.dgvAllGroups).BeginInit();
			base.SuspendLayout();
			this.butSaveDev.DialogResult = DialogResult.OK;
			componentResourceManager.ApplyResources(this.butSaveDev, "butSaveDev");
			this.butSaveDev.Name = "butSaveDev";
			this.butSaveDev.UseVisualStyleBackColor = true;
			this.butSaveDev.Click += new System.EventHandler(this.butSave_Click);
			this.dgvAllDevices.AllowUserToAddRows = false;
			this.dgvAllDevices.AllowUserToDeleteRows = false;
			this.dgvAllDevices.AllowUserToResizeColumns = false;
			this.dgvAllDevices.AllowUserToResizeRows = false;
			this.dgvAllDevices.BackgroundColor = Color.WhiteSmoke;
			this.dgvAllDevices.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvAllDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvAllDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvAllDevices.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dgvcbSelectDev,
				this.dgvtbDeviceNm,
				this.dgvtbcMac,
				this.dgvtbcIp,
				this.dgvtbcModel,
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
			this.dgvAllDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAllDevices.CellValueChanged += new DataGridViewCellEventHandler(this.dgvAllDevices_CellValueChanged);
			this.dgvAllDevices.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAllDevices_CurrentCellDirtyStateChanged);
			componentResourceManager.ApplyResources(this.dgvcbSelectDev, "dgvcbSelectDev");
			this.dgvcbSelectDev.Name = "dgvcbSelectDev";
			componentResourceManager.ApplyResources(this.dgvtbDeviceNm, "dgvtbDeviceNm");
			this.dgvtbDeviceNm.Name = "dgvtbDeviceNm";
			this.dgvtbDeviceNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcMac, "dgvtbcMac");
			this.dgvtbcMac.Name = "dgvtbcMac";
			this.dgvtbcMac.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcIp, "dgvtbcIp");
			this.dgvtbcIp.Name = "dgvtbcIp";
			this.dgvtbcIp.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcModel, "dgvtbcModel");
			this.dgvtbcModel.Name = "dgvtbcModel";
			this.dgvtbcModel.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcRackNm, "dgvtbcRackNm");
			this.dgvtbcRackNm.Name = "dgvtbcRackNm";
			this.dgvtbcRackNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.deviceId, "deviceId");
			this.deviceId.Name = "deviceId";
			this.deviceId.ReadOnly = true;
			this.butCancelDev.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancelDev, "butCancelDev");
			this.butCancelDev.Name = "butCancelDev";
			this.butCancelDev.UseVisualStyleBackColor = true;
			this.cbSelAllDev.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.cbSelAllDev, "cbSelAllDev");
			this.cbSelAllDev.ForeColor = Color.Black;
			this.cbSelAllDev.Name = "cbSelAllDev";
			this.cbSelAllDev.UseVisualStyleBackColor = false;
			this.cbSelAllDev.CheckedChanged += new System.EventHandler(this.cbSelAllDev_CheckedChanged);
			this.accTC.Controls.Add(this.tpDevice);
			this.accTC.Controls.Add(this.tpGroup);
			componentResourceManager.ApplyResources(this.accTC, "accTC");
			this.accTC.Name = "accTC";
			this.accTC.SelectedIndex = 0;
			this.tpDevice.BackColor = SystemColors.Control;
			this.tpDevice.Controls.Add(this.cbSelAllDev);
			this.tpDevice.Controls.Add(this.butCancelDev);
			this.tpDevice.Controls.Add(this.butSaveDev);
			this.tpDevice.Controls.Add(this.dgvAllDevices);
			componentResourceManager.ApplyResources(this.tpDevice, "tpDevice");
			this.tpDevice.Name = "tpDevice";
			this.tpGroup.BackColor = SystemColors.Control;
			this.tpGroup.Controls.Add(this.cbSelAllGp);
			this.tpGroup.Controls.Add(this.btnCancelGp);
			this.tpGroup.Controls.Add(this.btnSaveGp);
			this.tpGroup.Controls.Add(this.dgvAllGroups);
			componentResourceManager.ApplyResources(this.tpGroup, "tpGroup");
			this.tpGroup.Name = "tpGroup";
			this.cbSelAllGp.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.cbSelAllGp, "cbSelAllGp");
			this.cbSelAllGp.ForeColor = Color.Black;
			this.cbSelAllGp.Name = "cbSelAllGp";
			this.cbSelAllGp.UseVisualStyleBackColor = false;
			this.cbSelAllGp.CheckedChanged += new System.EventHandler(this.cbSelAllGp_CheckedChanged);
			this.btnCancelGp.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btnCancelGp, "btnCancelGp");
			this.btnCancelGp.Name = "btnCancelGp";
			this.btnCancelGp.UseVisualStyleBackColor = true;
			this.btnSaveGp.DialogResult = DialogResult.OK;
			componentResourceManager.ApplyResources(this.btnSaveGp, "btnSaveGp");
			this.btnSaveGp.Name = "btnSaveGp";
			this.btnSaveGp.UseVisualStyleBackColor = true;
			this.btnSaveGp.Click += new System.EventHandler(this.butSave_Click);
			this.dgvAllGroups.AllowUserToAddRows = false;
			this.dgvAllGroups.AllowUserToDeleteRows = false;
			this.dgvAllGroups.AllowUserToResizeColumns = false;
			this.dgvAllGroups.AllowUserToResizeRows = false;
			this.dgvAllGroups.BackgroundColor = Color.WhiteSmoke;
			this.dgvAllGroups.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.BackColor = SystemColors.Control;
			dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle5.ForeColor = Color.Black;
			dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
			this.dgvAllGroups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.dgvAllGroups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvAllGroups.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dgvcbSelectGp,
				this.groupNm,
				this.groupType,
				this.groupId
			});
			dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = Color.White;
			dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle6.ForeColor = Color.Black;
			dataGridViewCellStyle6.SelectionBackColor = Color.White;
			dataGridViewCellStyle6.SelectionForeColor = Color.Black;
			dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
			this.dgvAllGroups.DefaultCellStyle = dataGridViewCellStyle6;
			this.dgvAllGroups.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvAllGroups, "dgvAllGroups");
			this.dgvAllGroups.MultiSelect = false;
			this.dgvAllGroups.Name = "dgvAllGroups";
			dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = Color.White;
			dataGridViewCellStyle7.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle7.ForeColor = Color.Black;
			dataGridViewCellStyle7.SelectionBackColor = Color.White;
			dataGridViewCellStyle7.SelectionForeColor = Color.Black;
			dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
			this.dgvAllGroups.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgvAllGroups.RowHeadersVisible = false;
			this.dgvAllGroups.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle8.BackColor = Color.White;
			dataGridViewCellStyle8.ForeColor = Color.Black;
			dataGridViewCellStyle8.SelectionBackColor = Color.Blue;
			dataGridViewCellStyle8.SelectionForeColor = Color.White;
			this.dgvAllGroups.RowsDefaultCellStyle = dataGridViewCellStyle8;
			this.dgvAllGroups.RowTemplate.Height = 23;
			this.dgvAllGroups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAllGroups.CellValueChanged += new DataGridViewCellEventHandler(this.dgvAllGroups_CellValueChanged);
			this.dgvAllGroups.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAllGroups_CurrentCellDirtyStateChanged);
			componentResourceManager.ApplyResources(this.dgvcbSelectGp, "dgvcbSelectGp");
			this.dgvcbSelectGp.Name = "dgvcbSelectGp";
			componentResourceManager.ApplyResources(this.groupNm, "groupNm");
			this.groupNm.Name = "groupNm";
			this.groupNm.ReadOnly = true;
			this.groupType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.groupType, "groupType");
			this.groupType.Name = "groupType";
			this.groupType.ReadOnly = true;
			componentResourceManager.ApplyResources(this.groupId, "groupId");
			this.groupId.Name = "groupId";
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancelDev;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.accTC);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "UserManUserSetDevDlg";
			base.ShowInTaskbar = false;
			((ISupportInitialize)this.dgvAllDevices).EndInit();
			this.accTC.ResumeLayout(false);
			this.tpDevice.ResumeLayout(false);
			this.tpGroup.ResumeLayout(false);
			((ISupportInitialize)this.dgvAllGroups).EndInit();
			base.ResumeLayout(false);
		}
	}
}
