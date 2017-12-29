using CommonAPI.Global;
using CommonAPI.network;
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
	public class OtherDevicesAllDev : UserControl
	{
		private const int DTbCol0_Name = 0;
		private const int DTbCol1_Type = 1;
		private const int DTbCol2_IP = 2;
		private const int DTbCol3_ST = 3;
		private const int DTbCol4_ID = 4;
		private IContainer components;
		private TableLayoutPanel tableLayoutPanel1;
		private Panel panel1;
		private Panel panel2;
		private Button butOthDevicesDel;
		private Button butAddOthDevices;
		private Button butOthDeviceSetup;
		private TabControl tabCtrlOtherDevSetting;
		private TabPage tabISGateway;
		private DataGridView dgvAllOtherDevices;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private Button butSaveISGSetting;
		private GroupBox groupBox1;
		private CheckBox cbAtenPDU;
		private TextBox tbISGServicePort;
		private Label lbServicePort;
		private CheckBox cbEnableISGateway;
		private DataGridViewTextBoxColumn dgvtbOthDeviceNm;
		private DataGridViewTextBoxColumn dgvtbOthDeviceType;
		private DataGridViewTextBoxColumn dgvtbOthDeviceIp;
		private DataGridViewTextBoxColumn dgvtbOthDeviceStatus;
		private DataGridViewTextBoxColumn dgvtbOthDeviceID;
		private DataGridViewTextBoxColumn dgvtbcdeviceId;
		private OtherDevices m_pParent;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OtherDevicesAllDev));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.panel1 = new Panel();
			this.tabCtrlOtherDevSetting = new TabControl();
			this.tabISGateway = new TabPage();
			this.butSaveISGSetting = new Button();
			this.groupBox1 = new GroupBox();
			this.cbAtenPDU = new CheckBox();
			this.tbISGServicePort = new TextBox();
			this.lbServicePort = new Label();
			this.cbEnableISGateway = new CheckBox();
			this.panel2 = new Panel();
			this.dgvAllOtherDevices = new DataGridView();
			this.butOthDevicesDel = new Button();
			this.butAddOthDevices = new Button();
			this.butOthDeviceSetup = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
			this.dgvtbOthDeviceNm = new DataGridViewTextBoxColumn();
			this.dgvtbOthDeviceType = new DataGridViewTextBoxColumn();
			this.dgvtbOthDeviceIp = new DataGridViewTextBoxColumn();
			this.dgvtbOthDeviceStatus = new DataGridViewTextBoxColumn();
			this.dgvtbOthDeviceID = new DataGridViewTextBoxColumn();
			this.dgvtbcdeviceId = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabCtrlOtherDevSetting.SuspendLayout();
			this.tabISGateway.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			((ISupportInitialize)this.dgvAllOtherDevices).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.panel1.BorderStyle = BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.tabCtrlOtherDevSetting);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.tabCtrlOtherDevSetting.Controls.Add(this.tabISGateway);
			componentResourceManager.ApplyResources(this.tabCtrlOtherDevSetting, "tabCtrlOtherDevSetting");
			this.tabCtrlOtherDevSetting.Name = "tabCtrlOtherDevSetting";
			this.tabCtrlOtherDevSetting.SelectedIndex = 0;
			this.tabISGateway.BackColor = Color.WhiteSmoke;
			this.tabISGateway.BorderStyle = BorderStyle.FixedSingle;
			this.tabISGateway.Controls.Add(this.butSaveISGSetting);
			this.tabISGateway.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this.tabISGateway, "tabISGateway");
			this.tabISGateway.Name = "tabISGateway";
			this.butSaveISGSetting.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSaveISGSetting, "butSaveISGSetting");
			this.butSaveISGSetting.Name = "butSaveISGSetting";
			this.butSaveISGSetting.UseVisualStyleBackColor = false;
			this.butSaveISGSetting.Click += new System.EventHandler(this.butSaveISGSetting_Click);
			this.groupBox1.Controls.Add(this.cbAtenPDU);
			this.groupBox1.Controls.Add(this.tbISGServicePort);
			this.groupBox1.Controls.Add(this.lbServicePort);
			this.groupBox1.Controls.Add(this.cbEnableISGateway);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.cbAtenPDU, "cbAtenPDU");
			this.cbAtenPDU.Name = "cbAtenPDU";
			this.cbAtenPDU.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tbISGServicePort, "tbISGServicePort");
			this.tbISGServicePort.Name = "tbISGServicePort";
			this.tbISGServicePort.KeyPress += new KeyPressEventHandler(this.tbISGServicePort_KeyPress);
			componentResourceManager.ApplyResources(this.lbServicePort, "lbServicePort");
			this.lbServicePort.Name = "lbServicePort";
			componentResourceManager.ApplyResources(this.cbEnableISGateway, "cbEnableISGateway");
			this.cbEnableISGateway.Name = "cbEnableISGateway";
			this.cbEnableISGateway.UseVisualStyleBackColor = true;
			this.cbEnableISGateway.CheckedChanged += new System.EventHandler(this.checkBoxEnableISGateway_CheckedChanged);
			this.panel2.Controls.Add(this.dgvAllOtherDevices);
			this.panel2.Controls.Add(this.butOthDevicesDel);
			this.panel2.Controls.Add(this.butAddOthDevices);
			this.panel2.Controls.Add(this.butOthDeviceSetup);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.dgvAllOtherDevices.AllowUserToAddRows = false;
			this.dgvAllOtherDevices.AllowUserToDeleteRows = false;
			this.dgvAllOtherDevices.AllowUserToResizeColumns = false;
			this.dgvAllOtherDevices.AllowUserToResizeRows = false;
			this.dgvAllOtherDevices.BackgroundColor = Color.WhiteSmoke;
			this.dgvAllOtherDevices.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.SelectionForeColor = Color.Black;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvAllOtherDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvAllOtherDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAllOtherDevices.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dgvtbOthDeviceNm,
				this.dgvtbOthDeviceType,
				this.dgvtbOthDeviceIp,
				this.dgvtbOthDeviceStatus,
				this.dgvtbOthDeviceID,
				this.dgvtbcdeviceId
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.White;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = Color.White;
			dataGridViewCellStyle2.SelectionForeColor = Color.Black;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvAllOtherDevices.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvAllOtherDevices.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvAllOtherDevices, "dgvAllOtherDevices");
			this.dgvAllOtherDevices.Name = "dgvAllOtherDevices";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.White;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = Color.White;
			dataGridViewCellStyle3.SelectionForeColor = Color.Black;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvAllOtherDevices.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvAllOtherDevices.RowHeadersVisible = false;
			this.dgvAllOtherDevices.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle4.BackColor = Color.White;
			dataGridViewCellStyle4.ForeColor = Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = Color.Blue;
			dataGridViewCellStyle4.SelectionForeColor = Color.White;
			this.dgvAllOtherDevices.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvAllOtherDevices.RowTemplate.Height = 23;
			this.dgvAllOtherDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAllOtherDevices.StandardTab = true;
			this.dgvAllOtherDevices.TabStop = false;
			this.dgvAllOtherDevices.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvAllOtherDevices_CellDoubleClick);
			this.butOthDevicesDel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butOthDevicesDel, "butOthDevicesDel");
			this.butOthDevicesDel.Name = "butOthDevicesDel";
			this.butOthDevicesDel.UseVisualStyleBackColor = false;
			this.butOthDevicesDel.Click += new System.EventHandler(this.butOthDevicesDel_Click);
			this.butAddOthDevices.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butAddOthDevices, "butAddOthDevices");
			this.butAddOthDevices.Name = "butAddOthDevices";
			this.butAddOthDevices.UseVisualStyleBackColor = false;
			this.butAddOthDevices.Click += new System.EventHandler(this.butAddOthDevices_Click);
			this.butOthDeviceSetup.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butOthDeviceSetup, "butOthDeviceSetup");
			this.butOthDeviceSetup.Name = "butOthDeviceSetup";
			this.butOthDeviceSetup.UseVisualStyleBackColor = false;
			this.butOthDeviceSetup.Click += new System.EventHandler(this.butOthDeviceSetup_Click);
			dataGridViewCellStyle5.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle5;
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
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbOthDeviceNm, "dgvtbOthDeviceNm");
			this.dgvtbOthDeviceNm.Name = "dgvtbOthDeviceNm";
			this.dgvtbOthDeviceNm.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbOthDeviceType, "dgvtbOthDeviceType");
			this.dgvtbOthDeviceType.Name = "dgvtbOthDeviceType";
			this.dgvtbOthDeviceType.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbOthDeviceIp, "dgvtbOthDeviceIp");
			this.dgvtbOthDeviceIp.Name = "dgvtbOthDeviceIp";
			this.dgvtbOthDeviceIp.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbOthDeviceStatus, "dgvtbOthDeviceStatus");
			this.dgvtbOthDeviceStatus.Name = "dgvtbOthDeviceStatus";
			this.dgvtbOthDeviceStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbOthDeviceID, "dgvtbOthDeviceID");
			this.dgvtbOthDeviceID.Name = "dgvtbOthDeviceID";
			this.dgvtbOthDeviceID.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbcdeviceId, "dgvtbcdeviceId");
			this.dgvtbcdeviceId.Name = "dgvtbcdeviceId";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "OtherDevicesAllDev";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tabCtrlOtherDevSetting.ResumeLayout(false);
			this.tabISGateway.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel2.ResumeLayout(false);
			((ISupportInitialize)this.dgvAllOtherDevices).EndInit();
			base.ResumeLayout(false);
		}
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public OtherDevicesAllDev()
		{
			this.InitializeComponent();
		}
		public void pageInit(OtherDevices pParent, TreeNode rootNode)
		{
			this.m_pParent = pParent;
			if (Sys_Para.GetISGFlag() == 0)
			{
				this.cbEnableISGateway.Checked = false;
				this.tbISGServicePort.Enabled = false;
				this.cbAtenPDU.Enabled = false;
			}
			else
			{
				this.cbEnableISGateway.Checked = true;
				this.tbISGServicePort.Enabled = true;
				this.cbAtenPDU.Enabled = true;
			}
			this.tbISGServicePort.Text = System.Convert.ToString(Sys_Para.GetISGPort());
			if (Sys_Para.GetITPowerFlag() == 0)
			{
				this.cbAtenPDU.Checked = false;
			}
			else
			{
				this.cbAtenPDU.Checked = true;
			}
			this.dgvAllOtherDevices.Rows.Clear();
			System.Collections.Generic.List<InSnergyGateway> allGateWay = InSnergyGateway.GetAllGateWay();
			foreach (TreeNode treeNode in rootNode.Nodes)
			{
				string text = treeNode.Text;
				string name = treeNode.Name;
				bool flag = false;
				InSnergyGateway inSnergyGateway = null;
				for (int i = 0; i < allGateWay.Count; i++)
				{
					inSnergyGateway = allGateWay[i];
					if (inSnergyGateway.GatewayID == name)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					string gatewayIP = InSnergyService.getGatewayIP(inSnergyGateway.GatewayID);
					string msg = EcoLanguage.getMsg(LangRes.ISGST_offline, new string[0]);
					if (InSnergyService.IsGatewayOnline(name))
					{
						msg = EcoLanguage.getMsg(LangRes.ISGST_online, new string[0]);
					}
					string[] values = new string[]
					{
						text,
						inSnergyGateway.GatewayType,
						gatewayIP,
						msg,
						inSnergyGateway.GatewayID
					};
					this.dgvAllOtherDevices.Rows.Add(values);
				}
			}
			if (this.dgvAllOtherDevices.Rows.Count == 0)
			{
				this.butOthDevicesDel.Enabled = false;
				this.butOthDeviceSetup.Enabled = false;
				return;
			}
			this.butOthDevicesDel.Enabled = true;
			this.butOthDeviceSetup.Enabled = true;
		}
		private void butAddOthDevices_Click(object sender, System.EventArgs e)
		{
			OtherDeviceAddDevDlg otherDeviceAddDevDlg = new OtherDeviceAddDevDlg();
			DialogResult dialogResult = DialogResult.Cancel;
			try
			{
				otherDeviceAddDevDlg.pageInit();
				dialogResult = otherDeviceAddDevDlg.ShowDialog(this);
			}
			catch (System.Exception)
			{
			}
			if (dialogResult == DialogResult.OK)
			{
				this.changeTreeSelect("DevRoot");
			}
		}
		private void changeTreeSelect(string objID)
		{
			OtherDevicesAllDev.PostMessage(this.m_pParent.Handle, 63000u, 0, objID);
		}
		private void checkBoxEnableISGateway_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.cbEnableISGateway.Checked)
			{
				this.tbISGServicePort.Enabled = false;
				this.cbAtenPDU.Enabled = false;
				return;
			}
			this.tbISGServicePort.Enabled = true;
			this.cbAtenPDU.Enabled = true;
		}
		private void butSaveISGSetting_Click(object sender, System.EventArgs e)
		{
			int iSGFlag = Sys_Para.GetISGFlag();
			int num = 0;
			int iSGPort = Sys_Para.GetISGPort();
			int num2 = iSGPort;
			int iTPowerFlag = Sys_Para.GetITPowerFlag();
			int num3 = iTPowerFlag;
			bool flag = false;
			if (this.cbEnableISGateway.Checked)
			{
				num = 1;
				Ecovalidate.checkTextIsNull(this.tbISGServicePort, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbServicePort.Text
					}));
					return;
				}
				if (!Ecovalidate.Rangeint(this.tbISGServicePort, 1, 65535))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.lbServicePort.Text,
						"1",
						"65535"
					}));
					return;
				}
				num2 = System.Convert.ToInt32(this.tbISGServicePort.Text);
				if (iSGPort != num2)
				{
					bool flag2 = NetworkShareAccesser.TcpPortInUse(num2);
					if (flag2)
					{
						this.tbISGServicePort.Focus();
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Portconflict, new string[]
						{
							this.tbISGServicePort.Text
						}));
						return;
					}
				}
				if (!this.cbAtenPDU.Checked)
				{
					num3 = 0;
				}
				else
				{
					num3 = 1;
				}
			}
			Sys_Para.SetISGFlag(num);
			Sys_Para.SetISGPort(num2);
			Sys_Para.SetITPowerFlag(num3);
			if (iSGFlag != num || iSGPort != num2)
			{
				if (num == 0)
				{
					InSnergyService.Restart(false, num2);
				}
				else
				{
					InSnergyService.Restart(true, num2);
				}
			}
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0432000", new string[]
				{
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0432000", new string[0]);
			}
			if (iSGFlag != num || iTPowerFlag != num3)
			{
				EcoGlobalVar.setDashBoardFlg(32uL, "", 32);
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private void tbISGServicePort_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\b' && (e.KeyChar > '9' || e.KeyChar < '0'))
			{
				e.Handled = true;
			}
		}
		private void butOthDeviceSetup_Click(object sender, System.EventArgs e)
		{
			string objID = this.dgvAllOtherDevices.CurrentRow.Cells[4].Value.ToString();
			this.changeTreeSelect(objID);
		}
		private void dgvAllOtherDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			string objID = this.dgvAllOtherDevices.Rows[rowIndex].Cells[4].Value.ToString();
			this.changeTreeSelect(objID);
		}
		private void butOthDevicesDel_Click(object sender, System.EventArgs e)
		{
			if (this.dgvAllOtherDevices.SelectedRows.Count == 0)
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
			for (int i = 0; i < this.dgvAllOtherDevices.SelectedRows.Count; i++)
			{
				DataGridViewCellCollection cells = this.dgvAllOtherDevices.SelectedRows[i].Cells;
				string value = cells[4].Value.ToString();
				arrayList.Add(value);
			}
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_delDev, new string[0]), null, new progressPopup.ProcessInThread(this.delGateWayPro), arrayList, 0);
			progressPopup.ShowDialog();
			object return_V = progressPopup.Return_V;
			int? num = return_V as int?;
			if (!num.HasValue || num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
			}
			this.changeTreeSelect("DevRoot");
		}
		private object delGateWayPro(object param)
		{
			System.Collections.ArrayList arrayList = param as System.Collections.ArrayList;
			for (int i = 0; i < arrayList.Count; i++)
			{
				string text = (string)arrayList[i];
				InSnergyGateway gateWaybyGID = InSnergyGateway.GetGateWaybyGID(text);
				int num = InSnergyGateway.DeleteGateway(text);
				if (num < 0)
				{
					return num;
				}
				string valuePair = ValuePairs.getValuePair("Username");
				if (!string.IsNullOrEmpty(valuePair))
				{
					LogAPI.writeEventLog("0432011", new string[]
					{
						gateWaybyGID.GatewayName,
						valuePair
					});
				}
				else
				{
					LogAPI.writeEventLog("0432011", new string[]
					{
						gateWaybyGID.GatewayName
					});
				}
				InSnergyService.Unmanage(text);
			}
			return 1;
		}
		public void TimerProc()
		{
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvAllOtherDevices.Rows)
			{
				string gw = dataGridViewRow.Cells[4].Value.ToString();
				string gatewayIP = InSnergyService.getGatewayIP(gw);
				dataGridViewRow.Cells[2].Value = gatewayIP;
				if (InSnergyService.IsGatewayOnline(gw))
				{
					dataGridViewRow.Cells[3].Value = EcoLanguage.getMsg(LangRes.ISGST_online, new string[0]);
				}
				else
				{
					dataGridViewRow.Cells[3].Value = EcoLanguage.getMsg(LangRes.ISGST_offline, new string[0]);
				}
			}
		}
	}
}
