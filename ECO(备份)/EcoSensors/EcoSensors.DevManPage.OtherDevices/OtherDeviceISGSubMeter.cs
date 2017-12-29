using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using InSnergyAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.DevManPage.OtherDevices
{
	public class OtherDeviceISGSubMeter : UserControl
	{
		public const string STR_ITPOWER = "IT Power";
		public const string STR_NONITPOWER = "Non IT Power";
		public const string STR_UNCONFIRMED = "Undefined";
		private const int DTbCol0_NO = 0;
		private const int DTbCol1_Cur = 1;
		private const int DTbCol2_Vol = 2;
		private const int DTbCol3_HZ = 3;
		private const int DTbCol4_PW = 4;
		private const int DTbCol5_PD = 5;
		private const int DTbCol6_PF = 6;
		private const int DTbCol7_Cap = 7;
		private const int DTbCol8_electp = 8;
		private const int DTbCol9_gatewayID = 9;
		private const int DTbCol10_branchID = 10;
		private const int DTbCol11_meterID = 11;
		private OtherDevices m_pParent;
		private string m_gatewayID;
		private string m_branchID;
		private IContainer components;
		private GroupBox groupBox1;
		private Label label2;
		private Label label11;
		private Label label16;
		private Label labDevType;
		private Label labDevIp;
		private GroupBox groupBox2;
		private TextBox tbBranchNm;
		private Label label1;
		private Label label3;
		private Label labDevName;
		private TextBox tbBranchLoct;
		private GroupBox groupBox3;
		private DataGridView dgvSubMeters;
		private Button butSaveISGDev;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
		private DataGridViewTextBoxColumn dgvtbISG_SubMNo;
		private DataGridViewTextBoxColumn dgvtbISG_SubMCurrent;
		private DataGridViewTextBoxColumn dgvtbISG_BPVoltage;
		private DataGridViewTextBoxColumn dgvtbISG_SubMHz;
		private DataGridViewTextBoxColumn dgvtbISG_SubMPower;
		private DataGridViewTextBoxColumn dgvtbISG_SubMPowerMeter;
		private DataGridViewTextBoxColumn dgvtbISG_SubMPF;
		private DataGridViewTextBoxColumn dgvtbISG_SubMCap;
		private DataGridViewComboBoxColumn dgvtbISG_SubMElecUsage;
		private DataGridViewTextBoxColumn dgvtbISG_BPDevID;
		private DataGridViewTextBoxColumn dgvtbISG_BPBranchID;
		private DataGridViewTextBoxColumn dgvtbISG_BPMeterId;
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public OtherDeviceISGSubMeter()
		{
			this.InitializeComponent();
			this.tbBranchNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbBranchLoct.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(OtherDevices pParent, string gatewayID, string BranchID)
		{
			this.m_pParent = pParent;
			InSnergyGateway gateWaybyBID = InSnergyGateway.GetGateWaybyBID(BranchID);
			if (gateWaybyBID == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DevInfo_nofind, new string[]
				{
					gatewayID
				}));
				return;
			}
			this.m_gatewayID = gatewayID;
			this.m_branchID = BranchID;
			this.labDevName.Text = gateWaybyBID.GatewayName;
			this.labDevIp.Text = InSnergyService.getGatewayIP(gateWaybyBID.GatewayID);
			this.labDevType.Text = gateWaybyBID.GatewayType;
			Branch branch = gateWaybyBID.BranchList[0];
			if (branch == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DevInfo_nofind, new string[]
				{
					BranchID
				}));
				return;
			}
			this.tbBranchNm.Text = branch.BranchName;
			this.tbBranchLoct.Text = branch.Location;
			this.dgvtbISG_SubMElecUsage.Items.Clear();
			this.dgvtbISG_SubMElecUsage.Items.Add("IT Power");
			this.dgvtbISG_SubMElecUsage.Items.Add("Non IT Power");
			this.dgvtbISG_SubMElecUsage.Items.Add("Undefined");
			this.dgvSubMeters.Rows.Clear();
			string text = "";
			int num = 1;
			System.Collections.Generic.Dictionary<string, IMeter> branch2 = InSnergyService.GetBranch(this.m_gatewayID, this.m_branchID);
			foreach (SubMeter current in branch.SubMeterList)
			{
				switch (current.ElectricityUsage)
				{
				case 0:
					text = "Undefined";
					break;
				case 1:
					text = "IT Power";
					break;
				case 2:
					text = "Non IT Power";
					break;
				}
				string submeterID = current.SubmeterID;
				string text2 = "";
				string text3 = "";
				string text4 = "";
				string text5 = "";
				string text6 = "";
				string text7 = "";
				if (branch2.ContainsKey(submeterID))
				{
				IMeter meter = branch2[submeterID];
					if (meter.listParam.ContainsKey(2))
					{
						double dvalue = meter.listParam[2].dvalue;
						text2 = dvalue.ToString("F2");
					}
					if (meter.listParam.ContainsKey(1))
					{
						double dvalue = meter.listParam[1].dvalue;
						text3 = dvalue.ToString("F2");
					}
					if (meter.listParam.ContainsKey(3))
					{
						double dvalue = meter.listParam[3].dvalue;
						text4 = dvalue.ToString("F1");
					}
					if (meter.listParam.ContainsKey(5))
					{
						double dvalue = meter.listParam[5].dvalue;
						text5 = dvalue.ToString("F2");
					}
					if (meter.listParam.ContainsKey(8))
					{
						double dvalue = meter.listParam[8].dvalue;
						text6 = dvalue.ToString("F3");
					}
					if (meter.listParam.ContainsKey(4))
					{
						double dvalue = meter.listParam[4].dvalue;
						text7 = dvalue.ToString("F2");
					}
				}
				string[] values = new string[]
				{
					num.ToString(),
					text2,
					text3,
					text4,
					text5,
					text6,
					text7,
					current.Capacity.ToString("F2"),
					text,
					current.GatewayID,
					current.BranchID,
					current.SubmeterID
				};
				this.dgvSubMeters.Rows.Add(values);
				num++;
			}
		}
		private void dgvSubMeters_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				return;
			}
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}
			DataGridViewCell currentCell = this.dgvSubMeters.Rows[e.RowIndex].Cells[e.ColumnIndex];
			if (e.ColumnIndex == 8)
			{
				this.dgvSubMeters.CurrentCell = currentCell;
				this.dgvSubMeters.BeginEdit(false);
				ComboBox comboBox = (ComboBox)this.dgvSubMeters.EditingControl;
				comboBox.DroppedDown = true;
			}
		}
		private void dgvSubMeters_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			e.Control.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			e.Control.KeyPress += new KeyPressEventHandler(this.dgvSubMeters_KeyPress);
		}
		private void dgvSubMeters_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar >= '0' && keyChar <= '9')
			{
				return;
			}
			if (keyChar == '.' || keyChar == ',' || keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void dgvSubMeters_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dgvSubMeters.IsCurrentCellDirty)
			{
				this.dgvSubMeters.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		public void TimerProc()
		{
			string gatewayIP = InSnergyService.getGatewayIP(this.m_gatewayID);
			this.labDevIp.Text = gatewayIP;
			System.Collections.Generic.Dictionary<string, IMeter> branch = InSnergyService.GetBranch(this.m_gatewayID, this.m_branchID);
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSubMeters.Rows)
			{
				string key = dataGridViewRow.Cells[11].Value.ToString();
				string value = "";
				string value2 = "";
				string value3 = "";
				string value4 = "";
				string value5 = "";
				string value6 = "";
				if (branch.ContainsKey(key))
				{
					IMeter meter = branch[key];
					if (meter.listParam.ContainsKey(2))
					{
						double dvalue = meter.listParam[2].dvalue;
						value = dvalue.ToString("F2");
					}
					if (meter.listParam.ContainsKey(1))
					{
						double dvalue = meter.listParam[1].dvalue;
						value2 = dvalue.ToString("F2");
					}
					if (meter.listParam.ContainsKey(3))
					{
						double dvalue = meter.listParam[3].dvalue;
						value3 = dvalue.ToString("F1");
					}
					if (meter.listParam.ContainsKey(5))
					{
						double dvalue = meter.listParam[5].dvalue;
						value4 = dvalue.ToString("F2");
					}
					if (meter.listParam.ContainsKey(8))
					{
						double dvalue = meter.listParam[8].dvalue;
						value5 = dvalue.ToString("F3");
					}
					if (meter.listParam.ContainsKey(4))
					{
						double dvalue = meter.listParam[4].dvalue;
						value6 = dvalue.ToString("F2");
					}
				}
				dataGridViewRow.Cells[1].Value = value;
				dataGridViewRow.Cells[2].Value = value2;
				dataGridViewRow.Cells[3].Value = value3;
				dataGridViewRow.Cells[4].Value = value4;
				dataGridViewRow.Cells[5].Value = value5;
				dataGridViewRow.Cells[6].Value = value6;
			}
		}
		private void butSaveISGDev_Click(object sender, System.EventArgs e)
		{
			bool flag = true;
			for (int i = 0; i < this.dgvSubMeters.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvSubMeters.Rows[i];
				DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
				try
				{
					string value = dataGridViewRow.Cells[7].Value.ToString();
					System.Convert.ToSingle(value);
					dataGridViewCellStyle.BackColor = Color.WhiteSmoke;
					dataGridViewRow.Cells[7].Style = dataGridViewCellStyle;
				}
				catch (System.Exception)
				{
					dataGridViewCellStyle.BackColor = Color.Red;
					dataGridViewRow.Cells[7].Style = dataGridViewCellStyle;
					this.dgvSubMeters.CurrentCell = dataGridViewRow.Cells[7];
					this.dgvSubMeters.BeginEdit(true);
					flag = false;
				}
			}
			if (!flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
				return;
			}
			InSnergyGateway gateWaybyBID = InSnergyGateway.GetGateWaybyBID(this.m_branchID);
			Branch branch = gateWaybyBID.BranchList[0];
			string branchName = branch.BranchName;
			branch.BranchName = this.tbBranchNm.Text;
			branch.Location = this.tbBranchLoct.Text;
			for (int j = 0; j < this.dgvSubMeters.Rows.Count; j++)
			{
				DataGridViewRow dataGridViewRow = this.dgvSubMeters.Rows[j];
				string value = dataGridViewRow.Cells[7].Value.ToString();
				string text = dataGridViewRow.Cells[8].Value.ToString();
				string value2 = dataGridViewRow.Cells[11].Value.ToString();
				for (int k = 0; k < branch.SubMeterCount; k++)
				{
					SubMeter subMeter = branch.SubMeterList[k];
					if (subMeter.SubmeterID.Equals(value2))
					{
						subMeter.Capacity = System.Convert.ToSingle(value);
						int electricityUsage;
						if (text.Equals("IT Power"))
						{
							electricityUsage = 1;
						}
						else
						{
							if (text.Equals("Non IT Power"))
							{
								electricityUsage = 2;
							}
							else
							{
								electricityUsage = 0;
							}
						}
						subMeter.ElectricityUsage = electricityUsage;
					}
				}
			}
			int num = branch.Save();
			if (num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
				return;
			}
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0432013", new string[]
				{
					branch.BranchName,
					gateWaybyBID.GatewayName,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0432013", new string[]
				{
					branch.BranchName,
					gateWaybyBID.GatewayName
				});
			}
			if (!branchName.Equals(branch.BranchName))
			{
				this.changeTreeSelect(this.m_gatewayID + ":" + this.m_branchID);
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
		}
		private void tbBranchNm_KeyPress(object sender, KeyPressEventArgs e)
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
		private void changeTreeSelect(string objID)
		{
			OtherDeviceISGSubMeter.PostMessage(this.m_pParent.Handle, 63000u, 0, objID);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OtherDeviceISGSubMeter));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			this.groupBox1 = new GroupBox();
			this.labDevName = new Label();
			this.label2 = new Label();
			this.label11 = new Label();
			this.label16 = new Label();
			this.labDevType = new Label();
			this.labDevIp = new Label();
			this.groupBox2 = new GroupBox();
			this.tbBranchLoct = new TextBox();
			this.tbBranchNm = new TextBox();
			this.label1 = new Label();
			this.label3 = new Label();
			this.groupBox3 = new GroupBox();
			this.dgvSubMeters = new DataGridView();
			this.dgvtbISG_SubMNo = new DataGridViewTextBoxColumn();
			this.dgvtbISG_SubMCurrent = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPVoltage = new DataGridViewTextBoxColumn();
			this.dgvtbISG_SubMHz = new DataGridViewTextBoxColumn();
			this.dgvtbISG_SubMPower = new DataGridViewTextBoxColumn();
			this.dgvtbISG_SubMPowerMeter = new DataGridViewTextBoxColumn();
			this.dgvtbISG_SubMPF = new DataGridViewTextBoxColumn();
			this.dgvtbISG_SubMCap = new DataGridViewTextBoxColumn();
			this.dgvtbISG_SubMElecUsage = new DataGridViewComboBoxColumn();
			this.dgvtbISG_BPDevID = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPBranchID = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPMeterId = new DataGridViewTextBoxColumn();
			this.butSaveISGDev = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((ISupportInitialize)this.dgvSubMeters).BeginInit();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.labDevName);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.labDevType);
			this.groupBox1.Controls.Add(this.labDevIp);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.labDevName.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevName, "labDevName");
			this.labDevName.ForeColor = Color.Black;
			this.labDevName.Name = "labDevName";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = Color.Black;
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.ForeColor = Color.Black;
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.label16, "label16");
			this.label16.ForeColor = Color.Black;
			this.label16.Name = "label16";
			this.labDevType.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevType, "labDevType");
			this.labDevType.ForeColor = Color.Black;
			this.labDevType.Name = "labDevType";
			this.labDevIp.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevIp, "labDevIp");
			this.labDevIp.ForeColor = Color.Black;
			this.labDevIp.Name = "labDevIp";
			this.groupBox2.Controls.Add(this.tbBranchLoct);
			this.groupBox2.Controls.Add(this.tbBranchNm);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label3);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.tbBranchLoct, "tbBranchLoct");
			this.tbBranchLoct.ForeColor = Color.Black;
			this.tbBranchLoct.Name = "tbBranchLoct";
			this.tbBranchLoct.KeyPress += new KeyPressEventHandler(this.tbBranchNm_KeyPress);
			componentResourceManager.ApplyResources(this.tbBranchNm, "tbBranchNm");
			this.tbBranchNm.ForeColor = Color.Black;
			this.tbBranchNm.Name = "tbBranchNm";
			this.tbBranchNm.KeyPress += new KeyPressEventHandler(this.tbBranchNm_KeyPress);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = Color.Black;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = Color.Black;
			this.label3.Name = "label3";
			this.groupBox3.Controls.Add(this.dgvSubMeters);
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			this.dgvSubMeters.AllowUserToAddRows = false;
			this.dgvSubMeters.AllowUserToDeleteRows = false;
			this.dgvSubMeters.AllowUserToResizeColumns = false;
			this.dgvSubMeters.AllowUserToResizeRows = false;
			this.dgvSubMeters.BackgroundColor = Color.WhiteSmoke;
			this.dgvSubMeters.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.SelectionForeColor = Color.Black;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvSubMeters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSubMeters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSubMeters.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dgvtbISG_SubMNo,
				this.dgvtbISG_SubMCurrent,
				this.dgvtbISG_BPVoltage,
				this.dgvtbISG_SubMHz,
				this.dgvtbISG_SubMPower,
				this.dgvtbISG_SubMPowerMeter,
				this.dgvtbISG_SubMPF,
				this.dgvtbISG_SubMCap,
				this.dgvtbISG_SubMElecUsage,
				this.dgvtbISG_BPDevID,
				this.dgvtbISG_BPBranchID,
				this.dgvtbISG_BPMeterId
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.White;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle2.ForeColor = Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = Color.White;
			dataGridViewCellStyle2.SelectionForeColor = Color.Black;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvSubMeters.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSubMeters.EditMode = DataGridViewEditMode.EditOnEnter;
			this.dgvSubMeters.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvSubMeters, "dgvSubMeters");
			this.dgvSubMeters.Name = "dgvSubMeters";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.White;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = Color.White;
			dataGridViewCellStyle3.SelectionForeColor = Color.Black;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvSubMeters.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvSubMeters.RowHeadersVisible = false;
			this.dgvSubMeters.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle4.BackColor = Color.White;
			dataGridViewCellStyle4.ForeColor = Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = Color.Blue;
			dataGridViewCellStyle4.SelectionForeColor = Color.White;
			this.dgvSubMeters.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvSubMeters.RowTemplate.Height = 23;
			this.dgvSubMeters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvSubMeters.StandardTab = true;
			this.dgvSubMeters.TabStop = false;
			this.dgvSubMeters.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvSubMeters_CellMouseDown);
			this.dgvSubMeters.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSubMeters_CurrentCellDirtyStateChanged);
			this.dgvSubMeters.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgvSubMeters_EditingControlShowing);
			this.dgvSubMeters.KeyPress += new KeyPressEventHandler(this.dgvSubMeters_KeyPress);
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMNo, "dgvtbISG_SubMNo");
			this.dgvtbISG_SubMNo.Name = "dgvtbISG_SubMNo";
			this.dgvtbISG_SubMNo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMCurrent, "dgvtbISG_SubMCurrent");
			this.dgvtbISG_SubMCurrent.Name = "dgvtbISG_SubMCurrent";
			this.dgvtbISG_SubMCurrent.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_BPVoltage, "dgvtbISG_BPVoltage");
			this.dgvtbISG_BPVoltage.Name = "dgvtbISG_BPVoltage";
			this.dgvtbISG_BPVoltage.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMHz, "dgvtbISG_SubMHz");
			this.dgvtbISG_SubMHz.Name = "dgvtbISG_SubMHz";
			this.dgvtbISG_SubMHz.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMPower, "dgvtbISG_SubMPower");
			this.dgvtbISG_SubMPower.Name = "dgvtbISG_SubMPower";
			this.dgvtbISG_SubMPower.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMPowerMeter, "dgvtbISG_SubMPowerMeter");
			this.dgvtbISG_SubMPowerMeter.Name = "dgvtbISG_SubMPowerMeter";
			this.dgvtbISG_SubMPowerMeter.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMPF, "dgvtbISG_SubMPF");
			this.dgvtbISG_SubMPF.Name = "dgvtbISG_SubMPF";
			this.dgvtbISG_SubMPF.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMCap, "dgvtbISG_SubMCap");
			this.dgvtbISG_SubMCap.MaxInputLength = 8;
			this.dgvtbISG_SubMCap.Name = "dgvtbISG_SubMCap";
			this.dgvtbISG_SubMElecUsage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dgvtbISG_SubMElecUsage, "dgvtbISG_SubMElecUsage");
			this.dgvtbISG_SubMElecUsage.Name = "dgvtbISG_SubMElecUsage";
			componentResourceManager.ApplyResources(this.dgvtbISG_BPDevID, "dgvtbISG_BPDevID");
			this.dgvtbISG_BPDevID.Name = "dgvtbISG_BPDevID";
			componentResourceManager.ApplyResources(this.dgvtbISG_BPBranchID, "dgvtbISG_BPBranchID");
			this.dgvtbISG_BPBranchID.Name = "dgvtbISG_BPBranchID";
			componentResourceManager.ApplyResources(this.dgvtbISG_BPMeterId, "dgvtbISG_BPMeterId");
			this.dgvtbISG_BPMeterId.Name = "dgvtbISG_BPMeterId";
			this.butSaveISGDev.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSaveISGDev, "butSaveISGDev");
			this.butSaveISGDev.Name = "butSaveISGDev";
			this.butSaveISGDev.UseVisualStyleBackColor = false;
			this.butSaveISGDev.Click += new System.EventHandler(this.butSaveISGDev_Click);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewCellStyle5.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
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
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.MaxInputLength = 8;
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butSaveISGDev);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "OtherDeviceISGSubMeter";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			((ISupportInitialize)this.dgvSubMeters).EndInit();
			base.ResumeLayout(false);
		}
	}
}
