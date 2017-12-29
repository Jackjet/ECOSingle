using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using InSnergyAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.DevManPage.OtherDevices
{
	public class OtherDeviceISGBrPanel : UserControl
	{
		private OtherDevices m_pParent;
		private string m_gatewayID;
		private IContainer components;
		private GroupBox groupBox1;
		private Label lbName;
		private Label label11;
		private Label label16;
		private Label labDevType;
		private Label labDevIp;
		private TextBox tbDevName;
		private GroupBox groupBox2;
		private Button butSaveISGDev;
		private Button butUpdateISGDev;
		private DataGridView dgvBranchPanels;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private Button butSetupISGDev;
		private DataGridViewTextBoxColumn dgvtbISG_BPNo;
		private DataGridViewTextBoxColumn dgvtbISG_BPName;
		private DataGridViewTextBoxColumn dgvtbISG_BPLocation;
		private DataGridViewTextBoxColumn dgvtbISG_BPSubMeters;
		private DataGridViewTextBoxColumn dgvtbISG_BPDevID;
		private DataGridViewTextBoxColumn dgvtbISG_BPBranchID;
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(System.IntPtr hWnd, uint Msg, int wParam, string lParam);
		public OtherDeviceISGBrPanel()
		{
			this.InitializeComponent();
			this.tbDevName.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(OtherDevices pParent, string gatewayID, string devName)
		{
			this.m_pParent = pParent;
			this.m_gatewayID = gatewayID;
			InSnergyGateway gateWaybyGID = InSnergyGateway.GetGateWaybyGID(gatewayID);
			if (gateWaybyGID == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DevInfo_nofind, new string[]
				{
					devName
				}));
				return;
			}
			this.tbDevName.Text = gateWaybyGID.GatewayName;
			this.labDevIp.Text = InSnergyService.getGatewayIP(gateWaybyGID.GatewayID);
			this.labDevType.Text = gateWaybyGID.GatewayType;
			this.dgvBranchPanels.Rows.Clear();
			int num = 1;
			foreach (Branch current in gateWaybyGID.BranchList)
			{
				string[] values = new string[]
				{
					num.ToString(),
					current.BranchName,
					current.Location,
					current.SubMeterList.Count.ToString(),
					current.GatewayID,
					current.BranchID
				};
				this.dgvBranchPanels.Rows.Add(values);
				num++;
			}
		}
		private void changeTreeSelect(string objID)
		{
			OtherDeviceISGBrPanel.PostMessage(this.m_pParent.Handle, 63000u, 0, objID);
		}
		private void butSetupISGDev_Click(object sender, System.EventArgs e)
		{
			string str = this.dgvBranchPanels.CurrentRow.Cells[4].Value.ToString();
			string str2 = this.dgvBranchPanels.CurrentRow.Cells[5].Value.ToString();
			this.changeTreeSelect(str + ":" + str2);
		}
		private void dgvBranchPanels_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			string str = this.dgvBranchPanels.CurrentRow.Cells[4].Value.ToString();
			string str2 = this.dgvBranchPanels.Rows[rowIndex].Cells[5].Value.ToString();
			this.changeTreeSelect(str + ":" + str2);
		}
		public void TimerProc()
		{
			string gatewayIP = InSnergyService.getGatewayIP(this.m_gatewayID);
			this.labDevIp.Text = gatewayIP;
		}
		private void tbDevName_KeyPress(object sender, KeyPressEventArgs e)
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
		private void butSaveISGDev_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.devConfigCheck())
				{
					string text = this.tbDevName.Text.Trim();
					if (InSnergyGateway.ModifyName(this.m_gatewayID, text) >= 0)
					{
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							LogAPI.writeEventLog("0432012", new string[]
							{
								text,
								valuePair
							});
						}
						else
						{
							LogAPI.writeEventLog("0432012", new string[]
							{
								text
							});
						}
						this.changeTreeSelect(this.m_gatewayID);
					}
				}
			}
			catch (System.Exception)
			{
			}
		}
		private bool devConfigCheck()
		{
			string text = this.tbDevName.Text.Trim();
			this.tbDevName.Text = text;
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbDevName, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbName.Text
				}));
				this.tbDevName.BackColor = Color.Red;
				return false;
			}
			InSnergyGateway gateWaybyGID = InSnergyGateway.GetGateWaybyGID(this.m_gatewayID);
			if (text.Equals(gateWaybyGID.GatewayName))
			{
				this.tbDevName.BackColor = Color.White;
				return true;
			}
			if (!InSnergyGateway.CheckGatewayName(text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_nmdup, new string[]
				{
					this.tbDevName.Text
				}));
				this.tbDevName.Focus();
				this.tbDevName.BackColor = Color.Red;
				return false;
			}
			this.tbDevName.BackColor = Color.White;
			return true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OtherDeviceISGBrPanel));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			this.groupBox1 = new GroupBox();
			this.tbDevName = new TextBox();
			this.lbName = new Label();
			this.label11 = new Label();
			this.label16 = new Label();
			this.labDevType = new Label();
			this.labDevIp = new Label();
			this.groupBox2 = new GroupBox();
			this.dgvBranchPanels = new DataGridView();
			this.butSaveISGDev = new Button();
			this.butUpdateISGDev = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.butSetupISGDev = new Button();
			this.dgvtbISG_BPNo = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPName = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPLocation = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPSubMeters = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPDevID = new DataGridViewTextBoxColumn();
			this.dgvtbISG_BPBranchID = new DataGridViewTextBoxColumn();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.dgvBranchPanels).BeginInit();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.tbDevName);
			this.groupBox1.Controls.Add(this.lbName);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.labDevType);
			this.groupBox1.Controls.Add(this.labDevIp);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.tbDevName, "tbDevName");
			this.tbDevName.ForeColor = Color.Black;
			this.tbDevName.Name = "tbDevName";
			this.tbDevName.KeyPress += new KeyPressEventHandler(this.tbDevName_KeyPress);
			componentResourceManager.ApplyResources(this.lbName, "lbName");
			this.lbName.ForeColor = Color.Black;
			this.lbName.Name = "lbName";
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
			this.groupBox2.Controls.Add(this.dgvBranchPanels);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.dgvBranchPanels.AllowUserToAddRows = false;
			this.dgvBranchPanels.AllowUserToDeleteRows = false;
			this.dgvBranchPanels.AllowUserToResizeColumns = false;
			this.dgvBranchPanels.AllowUserToResizeRows = false;
			this.dgvBranchPanels.BackgroundColor = Color.WhiteSmoke;
			this.dgvBranchPanels.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.SelectionForeColor = Color.Black;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvBranchPanels.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvBranchPanels.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvBranchPanels.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dgvtbISG_BPNo,
				this.dgvtbISG_BPName,
				this.dgvtbISG_BPLocation,
				this.dgvtbISG_BPSubMeters,
				this.dgvtbISG_BPDevID,
				this.dgvtbISG_BPBranchID
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.White;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle2.ForeColor = Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = Color.White;
			dataGridViewCellStyle2.SelectionForeColor = Color.Black;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvBranchPanels.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvBranchPanels.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvBranchPanels, "dgvBranchPanels");
			this.dgvBranchPanels.Name = "dgvBranchPanels";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.White;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = Color.White;
			dataGridViewCellStyle3.SelectionForeColor = Color.Black;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvBranchPanels.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvBranchPanels.RowHeadersVisible = false;
			this.dgvBranchPanels.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle4.BackColor = Color.White;
			dataGridViewCellStyle4.ForeColor = Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = Color.Blue;
			dataGridViewCellStyle4.SelectionForeColor = Color.White;
			this.dgvBranchPanels.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvBranchPanels.RowTemplate.Height = 23;
			this.dgvBranchPanels.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvBranchPanels.StandardTab = true;
			this.dgvBranchPanels.TabStop = false;
			this.dgvBranchPanels.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvBranchPanels_CellDoubleClick);
			this.butSaveISGDev.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSaveISGDev, "butSaveISGDev");
			this.butSaveISGDev.Name = "butSaveISGDev";
			this.butSaveISGDev.UseVisualStyleBackColor = false;
			this.butSaveISGDev.Click += new System.EventHandler(this.butSaveISGDev_Click);
			this.butUpdateISGDev.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butUpdateISGDev, "butUpdateISGDev");
			this.butUpdateISGDev.Name = "butUpdateISGDev";
			this.butUpdateISGDev.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewCellStyle5.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.butSetupISGDev.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSetupISGDev, "butSetupISGDev");
			this.butSetupISGDev.Name = "butSetupISGDev";
			this.butSetupISGDev.UseVisualStyleBackColor = false;
			this.butSetupISGDev.Click += new System.EventHandler(this.butSetupISGDev_Click);
			componentResourceManager.ApplyResources(this.dgvtbISG_BPNo, "dgvtbISG_BPNo");
			this.dgvtbISG_BPNo.Name = "dgvtbISG_BPNo";
			this.dgvtbISG_BPNo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_BPName, "dgvtbISG_BPName");
			this.dgvtbISG_BPName.Name = "dgvtbISG_BPName";
			this.dgvtbISG_BPName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_BPLocation, "dgvtbISG_BPLocation");
			this.dgvtbISG_BPLocation.Name = "dgvtbISG_BPLocation";
			this.dgvtbISG_BPLocation.ReadOnly = true;
			this.dgvtbISG_BPSubMeters.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dgvtbISG_BPSubMeters, "dgvtbISG_BPSubMeters");
			this.dgvtbISG_BPSubMeters.Name = "dgvtbISG_BPSubMeters";
			this.dgvtbISG_BPSubMeters.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dgvtbISG_BPDevID, "dgvtbISG_BPDevID");
			this.dgvtbISG_BPDevID.Name = "dgvtbISG_BPDevID";
			componentResourceManager.ApplyResources(this.dgvtbISG_BPBranchID, "dgvtbISG_BPBranchID");
			this.dgvtbISG_BPBranchID.Name = "dgvtbISG_BPBranchID";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butSetupISGDev);
			base.Controls.Add(this.butUpdateISGDev);
			base.Controls.Add(this.butSaveISGDev);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "OtherDeviceISGBrPanel";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			((ISupportInitialize)this.dgvBranchPanels).EndInit();
			base.ResumeLayout(false);
		}
	}
}
