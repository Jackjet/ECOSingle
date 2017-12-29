using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.Thread;
using EcoSensors.Properties;
using EventLogAPI;
using InSnergyAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
namespace EcoSensors.DevManPage.OtherDevices
{
	public class OtherDeviceAddDevDlg : Form
	{
		private const string DTbCol1_datasel = "datasel";
		private const string DTbCol2_devname = "dgvtbOtherDevName";
		private const string DTbCol3_Type = "dgvtbOtherDevType";
		private const string DTbCol4_IP = "dgvtbOtherDevIP";
		private const string DTbCol5_ID = "dgvtbOtherDevID";
		private IContainer components;
		private PictureBox pictureBoxLoading;
		private CheckBox cbsel;
		private Button butAdd;
		private Button butCancel;
		private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private ComboBox cbISGMeterType;
		private DataGridViewTextBoxColumn dgvtbOtherDevID;
		private DataGridViewTextBoxColumn dgvtbOtherDevIP;
		private DataGridViewTextBoxColumn dgvtbOtherDevType;
		private DataGridViewTextBoxColumn dgvtbOtherDevName;
		private DataGridViewCheckBoxColumn datasel;
		private DataGridView dgvAutoDevice;
		private bool cbsel_changeonly;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OtherDeviceAddDevDlg));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			this.pictureBoxLoading = new PictureBox();
			this.cbsel = new CheckBox();
			this.butAdd = new Button();
			this.butCancel = new Button();
			this.dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.cbISGMeterType = new ComboBox();
			this.dgvtbOtherDevID = new DataGridViewTextBoxColumn();
			this.dgvtbOtherDevIP = new DataGridViewTextBoxColumn();
			this.dgvtbOtherDevType = new DataGridViewTextBoxColumn();
			this.dgvtbOtherDevName = new DataGridViewTextBoxColumn();
			this.datasel = new DataGridViewCheckBoxColumn();
			this.dgvAutoDevice = new DataGridView();
			((ISupportInitialize)this.pictureBoxLoading).BeginInit();
			((ISupportInitialize)this.dgvAutoDevice).BeginInit();
			base.SuspendLayout();
			this.pictureBoxLoading.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pictureBoxLoading, "pictureBoxLoading");
			this.pictureBoxLoading.Image = Resources.loader;
			this.pictureBoxLoading.Name = "pictureBoxLoading";
			this.pictureBoxLoading.TabStop = false;
			this.cbsel.BackColor = Color.Transparent;
			this.cbsel.Checked = true;
			this.cbsel.CheckState = CheckState.Checked;
			componentResourceManager.ApplyResources(this.cbsel, "cbsel");
			this.cbsel.Name = "cbsel";
			this.cbsel.UseVisualStyleBackColor = false;
			this.cbsel.CheckedChanged += new System.EventHandler(this.cbsel_CheckedChanged);
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
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			dataGridViewCellStyle.Font = new Font("SimSun", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.MaxInputLength = 39;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.cbISGMeterType.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.cbISGMeterType, "cbISGMeterType");
			this.cbISGMeterType.Name = "cbISGMeterType";
			componentResourceManager.ApplyResources(this.dgvtbOtherDevID, "dgvtbOtherDevID");
			this.dgvtbOtherDevID.Name = "dgvtbOtherDevID";
			this.dgvtbOtherDevID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbOtherDevIP, "dgvtbOtherDevIP");
			this.dgvtbOtherDevIP.Name = "dgvtbOtherDevIP";
			this.dgvtbOtherDevIP.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbOtherDevType, "dgvtbOtherDevType");
			this.dgvtbOtherDevType.Name = "dgvtbOtherDevType";
			this.dgvtbOtherDevType.ReadOnly = true;
			dataGridViewCellStyle2.Font = new Font("SimSun", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dgvtbOtherDevName.DefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.dgvtbOtherDevName, "dgvtbOtherDevName");
			this.dgvtbOtherDevName.MaxInputLength = 39;
			this.dgvtbOtherDevName.Name = "dgvtbOtherDevName";
			componentResourceManager.ApplyResources(this.datasel, "datasel");
			this.datasel.Name = "datasel";
			this.dgvAutoDevice.AllowUserToAddRows = false;
			this.dgvAutoDevice.AllowUserToDeleteRows = false;
			this.dgvAutoDevice.AllowUserToResizeColumns = false;
			this.dgvAutoDevice.AllowUserToResizeRows = false;
			this.dgvAutoDevice.BackgroundColor = Color.White;
			this.dgvAutoDevice.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvAutoDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvAutoDevice.Columns.AddRange(new DataGridViewColumn[]
			{
				this.datasel,
				this.dgvtbOtherDevName,
				this.dgvtbOtherDevType,
				this.dgvtbOtherDevIP,
				this.dgvtbOtherDevID
			});
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = SystemColors.Window;
			dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
			this.dgvAutoDevice.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvAutoDevice.EditMode = DataGridViewEditMode.EditOnEnter;
			this.dgvAutoDevice.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvAutoDevice, "dgvAutoDevice");
			this.dgvAutoDevice.MultiSelect = false;
			this.dgvAutoDevice.Name = "dgvAutoDevice";
			dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle5.ForeColor = Color.Black;
			dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
			this.dgvAutoDevice.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.dgvAutoDevice.RowHeadersVisible = false;
			this.dgvAutoDevice.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvAutoDevice.RowTemplate.Height = 23;
			this.dgvAutoDevice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAutoDevice.StandardTab = true;
			this.dgvAutoDevice.TabStop = false;
			this.dgvAutoDevice.CellValueChanged += new DataGridViewCellEventHandler(this.dgvAutoDevice_CellValueChanged);
			this.dgvAutoDevice.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAutoDevice_CurrentCellDirtyStateChanged);
			this.dgvAutoDevice.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgvAutoDevice_EditingControlShowing);
			this.dgvAutoDevice.SelectionChanged += new System.EventHandler(this.dgvAutoDevice_SelectionChanged);
			this.dgvAutoDevice.KeyPress += new KeyPressEventHandler(this.dgvAutoDevice_KeyPress);
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.cbISGMeterType);
			base.Controls.Add(this.cbsel);
			base.Controls.Add(this.butAdd);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.dgvAutoDevice);
			base.Controls.Add(this.pictureBoxLoading);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "OtherDeviceAddDevDlg";
			base.ShowInTaskbar = false;
			((ISupportInitialize)this.pictureBoxLoading).EndInit();
			((ISupportInitialize)this.dgvAutoDevice).EndInit();
			base.ResumeLayout(false);
		}
		public OtherDeviceAddDevDlg()
		{
			this.InitializeComponent();
			this.cbISGMeterType.Items.Add("Undefined");
			this.cbISGMeterType.Items.Add("IT Power");
			this.cbISGMeterType.Items.Add("Non IT Power");
			this.cbISGMeterType.SelectedIndex = 0;
			this.cbISGMeterType.Visible = false;
		}
		public void pageInit()
		{
			this.cbsel_changeonly = false;
			InSnergyService.UpdateLocalTree();
			System.Collections.Generic.Dictionary<string, string> gatewayList = InSnergyService.GetGatewayList(true);
			int num = 1;
			if (gatewayList != null && gatewayList.Count > 0)
			{
				foreach (System.Collections.Generic.KeyValuePair<string, string> current in gatewayList)
				{
					string value = current.Value;
					string text = current.Key;
					text = text.Replace("-", "");
					this.dgvAutoDevice.Rows.Add(new object[]
					{
						true,
						"ISG_" + text,
						"In-Snergy Gateway",
						value.Split(new char[]
						{
							':'
						})[0],
						current.Key
					});
					num++;
				}
			}
			if (this.dgvAutoDevice.Rows.Count > 0)
			{
				this.butAdd.Enabled = true;
				return;
			}
			this.butAdd.Enabled = false;
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
		private void butAdd_Click(object sender, System.EventArgs e)
		{
			if (this.dgvAutoDevice.Rows.Count == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_needselect, new string[0]));
				return;
			}
			bool flag = true;
			bool flag2 = false;
			for (int i = 0; i < this.dgvAutoDevice.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvAutoDevice.Rows[i];
				DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
				if (System.Convert.ToBoolean(dataGridViewRow.Cells["datasel"].Value))
				{
					if (System.Convert.ToString(dataGridViewRow.Cells["dgvtbOtherDevName"].Value).Equals(string.Empty))
					{
						dataGridViewCellStyle.BackColor = Color.Red;
						dataGridViewRow.Cells["dgvtbOtherDevName"].Style = dataGridViewCellStyle;
						this.dgvAutoDevice.CurrentCell = dataGridViewRow.Cells["dgvtbOtherDevName"];
						this.dgvAutoDevice.BeginEdit(true);
						flag = false;
					}
					else
					{
						string text = dataGridViewRow.Cells["dgvtbOtherDevName"].Value.ToString();
						text = text.Trim();
						dataGridViewRow.Cells["dgvtbOtherDevName"].Value = text;
						if (!Ecovalidate.ValidDevName(text) || !this.pduNmCheckAtDb(text) || !this.pduNmCheckAtControl(text, i, this.dgvAutoDevice.Rows))
						{
							dataGridViewCellStyle.BackColor = Color.Red;
							dataGridViewRow.Cells["dgvtbOtherDevName"].Style = dataGridViewCellStyle;
							this.dgvAutoDevice.CurrentCell = dataGridViewRow.Cells["dgvtbOtherDevName"];
							this.dgvAutoDevice.BeginEdit(true);
							flag = false;
						}
					}
					flag2 = true;
				}
			}
			if (!flag2)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_needselect, new string[0]));
				return;
			}
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_nameErr, new string[0]));
				return;
			}
			int arg_1EE_0 = this.cbISGMeterType.SelectedIndex;
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			for (int j = 0; j < this.dgvAutoDevice.Rows.Count; j++)
			{
				DataGridViewRow dataGridViewRow2 = this.dgvAutoDevice.Rows[j];
				if (System.Convert.ToBoolean(dataGridViewRow2.Cells["datasel"].Value))
				{
					string value = dataGridViewRow2.Cells["dgvtbOtherDevName"].Value.ToString();
					string value2 = dataGridViewRow2.Cells["dgvtbOtherDevType"].Value.ToString();
					string value3 = dataGridViewRow2.Cells["dgvtbOtherDevIP"].Value.ToString();
					string value4 = dataGridViewRow2.Cells["dgvtbOtherDevID"].Value.ToString();
					System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
					hashtable["ip"] = value3;
					hashtable["devNm"] = value;
					hashtable["type"] = value2;
					hashtable["devID"] = value4;
					list.Add(hashtable);
				}
			}
			this.butAdd.Enabled = false;
			this.butCancel.Enabled = false;
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.addDeviceProc), list);
		}
		private void addDeviceProc(object obj)
		{
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				PictureBox pictureBox = control as PictureBox;
				pictureBox.Show();
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this.pictureBoxLoading, null);
			System.Collections.Generic.List<System.Collections.Hashtable> list = obj as System.Collections.Generic.List<System.Collections.Hashtable>;
			this.otherDevSave(list);
			config = delegate(Control control, object param)
			{
				OtherDeviceAddDevDlg otherDeviceAddDevDlg = control as OtherDeviceAddDevDlg;
				otherDeviceAddDevDlg.butCancel.Enabled = true;
				otherDeviceAddDevDlg.DialogResult = DialogResult.OK;
				otherDeviceAddDevDlg.Close();
			};
			controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this, null);
		}
		private bool pduNmCheckAtControl(string name, int rowCount, DataGridViewRowCollection dgvrc)
		{
			for (int i = 0; i < rowCount; i++)
			{
				DataGridViewRow dataGridViewRow = dgvrc[i];
				if (System.Convert.ToBoolean(dataGridViewRow.Cells["datasel"].Value))
				{
					string value = dataGridViewRow.Cells["dgvtbOtherDevName"].Value.ToString();
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
			return InSnergyGateway.CheckGatewayName(devname);
		}
		private void otherDevSave(System.Collections.Generic.List<System.Collections.Hashtable> list)
		{
			foreach (System.Collections.Hashtable current in list)
			{
				string str_ip = (string)current["ip"];
				string text = (string)current["devNm"];
				string str_type = (string)current["type"];
				string text2 = (string)current["devID"];
				int i_usage = 0;
				System.Collections.Generic.List<Branch> list2 = new System.Collections.Generic.List<Branch>();
				System.Collections.Generic.List<string> branchList = InSnergyService.GetBranchList(text2);
				foreach (string current2 in branchList)
				{
					System.Collections.Generic.List<SubMeter> list3 = new System.Collections.Generic.List<SubMeter>();
					Branch item = new Branch(text2, current2, "", "", list3);
					list2.Add(item);
					System.Collections.Generic.List<string> meterList = InSnergyService.GetMeterList(text2, current2);
					foreach (string current3 in meterList)
					{
						SubMeter item2 = new SubMeter(text2, current2, current3, "", 0f, i_usage);
						list3.Add(item2);
					}
				}
				InSnergyGateway inSnergyGateway = new InSnergyGateway(text2, text, str_type, str_ip, list2);
				if (inSnergyGateway.Insert() > 0)
				{
					string valuePair = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair))
					{
						LogAPI.writeEventLog("0432010", new string[]
						{
							text,
							valuePair
						});
					}
					else
					{
						LogAPI.writeEventLog("0432010", new string[]
						{
							text
						});
					}
					InSnergyService.Manage(text2);
				}
			}
		}
	}
}
