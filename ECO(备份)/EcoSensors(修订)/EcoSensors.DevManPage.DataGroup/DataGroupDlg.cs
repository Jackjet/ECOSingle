using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace EcoSensors.DevManPage.DataGroup
{
	public class DataGroupDlg : Form
	{
		private bool cbselAll_changeonly;
		private IContainer components;
		private ComboBox cboType;
		private GroupBox groupBox1;
		private CheckBox chkAll;
		private Label lbname;
		private TextBox tbname;
		private Button btnCancel;
		private Button btnSave;
		private Label label1;
		private DataGridView dgvGpmember;
		public DataGroupDlg()
		{
			this.InitializeComponent();
			this.tbname.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.cboType.Items.Clear();
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPZone, new string[0]));
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPRack, new string[0]));
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPDev, new string[0]));
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPOutlet, new string[0]));
			this.cboType.SelectedIndex = 0;
		}
		private void cboType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.chkAll.Checked)
			{
				this.cbselAll_changeonly = true;
				this.chkAll.Checked = false;
			}
			this.FillList();
		}
		private void FillList()
		{
			this.dgvGpmember.DataSource = null;
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("objID", typeof(string));
			switch (this.cboType.SelectedIndex)
			{
			case 0:
			{
				dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
				System.Collections.ArrayList allZone = ZoneInfo.getAllZone();
				System.Collections.IEnumerator enumerator = allZone.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ZoneInfo zoneInfo = (ZoneInfo)enumerator.Current;
						string text = zoneInfo.ZoneID.ToString();
						string[] values = new string[]
						{
							text,
							zoneInfo.ZoneName
						};
						dataTable.Rows.Add(values);
					}
					goto IL_46A;
				}
				finally
				{
					System.IDisposable disposable = enumerator as System.IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				break;
			}
			case 1:
				break;
			case 2:
				goto IL_285;
			case 3:
				goto IL_36E;
			default:
				goto IL_46A;
			}
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
			System.Collections.ArrayList allZone2 = ZoneInfo.getAllZone();
			System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
			System.Collections.IEnumerator enumerator2 = allRack_NoEmpty.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					RackInfo rackInfo = (RackInfo)enumerator2.Current;
					string displayRackName = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
					string text2 = rackInfo.RackID.ToString();
					bool flag = false;
					string text3 = "";
					foreach (ZoneInfo zoneInfo2 in allZone2)
					{
						text3 = zoneInfo2.ZoneName;
						string[] source = zoneInfo2.RackInfo.Split(new char[]
						{
							','
						});
						if (source.Contains(text2))
						{
							flag = true;
							break;
						}
					}
					string[] values;
					if (flag)
					{
						values = new string[]
						{
							text2,
							displayRackName,
							text3
						};
					}
					else
					{
						values = new string[]
						{
							text2,
							displayRackName,
							""
						};
					}
					dataTable.Rows.Add(values);
				}
				goto IL_46A;
			}
			finally
			{
				System.IDisposable disposable3 = enumerator2 as System.IDisposable;
				if (disposable3 != null)
				{
					disposable3.Dispose();
				}
			}
			IL_285:
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
			System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			using (System.Collections.Generic.List<DeviceInfo>.Enumerator enumerator4 = allDevice.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					DeviceInfo current = enumerator4.Current;
					string deviceName = current.DeviceName;
					string text4 = current.DeviceID.ToString();
					RackInfo rackByID = RackInfo.getRackByID(current.RackID);
					string displayRackName2 = rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
					string[] values = new string[]
					{
						text4,
						deviceName,
						displayRackName2
					};
					dataTable.Rows.Add(values);
				}
				goto IL_46A;
			}
			IL_36E:
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMOutlet, new string[0]), typeof(string));
			dataTable.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
			allDevice = DeviceOperation.GetAllDevice();
			foreach (DeviceInfo current2 in allDevice)
			{
				System.Collections.Generic.List<PortInfo> portInfo = current2.GetPortInfo();
				foreach (PortInfo current3 in portInfo)
				{
					string[] values = new string[]
					{
						current3.ID.ToString(),
						current3.PortName,
						current2.DeviceName
					};
					dataTable.Rows.Add(values);
				}
			}
			IL_46A:
			this.dgvGpmember.DataSource = dataTable;
			this.dgvGpmember.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.dgvGpmember.Columns[0].Visible = false;
			int selectedIndex = this.cboType.SelectedIndex;
			if (selectedIndex == 0)
			{
				this.dgvGpmember.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				return;
			}
			this.dgvGpmember.Columns[1].Width = 140;
			this.dgvGpmember.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}
		private void chkAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbselAll_changeonly)
			{
				this.cbselAll_changeonly = false;
				return;
			}
			if (this.chkAll.Checked)
			{
				this.dgvGpmember.SelectAll();
				return;
			}
			this.dgvGpmember.ClearSelection();
		}
		private void txtname_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == '\b' || keyChar == ' ')
			{
				return;
			}
			e.Handled = true;
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			string str_type = "zone";
			switch (this.cboType.SelectedIndex)
			{
			case 0:
				str_type = "zone";
				break;
			case 1:
				str_type = "rack";
				break;
			case 2:
				str_type = "dev";
				break;
			case 3:
				str_type = "outlet";
				break;
			}
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbname, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbname.Text
				}));
				this.tbname.Focus();
				return;
			}
			string text = this.tbname.Text;
			System.Collections.Generic.List<long> list = new System.Collections.Generic.List<long>();
			for (int i = 0; i < this.dgvGpmember.Rows.Count; i++)
			{
				if (this.dgvGpmember.Rows[i].Selected)
				{
					string value = this.dgvGpmember.Rows[i].Cells[0].Value.ToString();
					list.Add((long)System.Convert.ToInt32(value));
				}
			}
			if (list.Count == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_selectneed, new string[0]));
				this.dgvGpmember.Focus();
				return;
			}
			int num = GroupInfo.CreateNewGroup(text, str_type, "", list);
			if (num == -2)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Group_nmdup, new string[]
				{
					text
				}));
				return;
			}
			if (num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			EcoGlobalVar.setDashBoardFlg(512uL, "", 64);
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			base.Close();
		}
		private void dgvGpmember_SelectionChanged(object sender, System.EventArgs e)
		{
			int count = this.dgvGpmember.SelectedRows.Count;
			int count2 = this.dgvGpmember.Rows.Count;
			if (count != count2 && this.chkAll.Checked)
			{
				this.cbselAll_changeonly = true;
				this.chkAll.Checked = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataGroupDlg));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.cboType = new ComboBox();
			this.groupBox1 = new GroupBox();
			this.dgvGpmember = new DataGridView();
			this.chkAll = new CheckBox();
			this.lbname = new Label();
			this.tbname = new TextBox();
			this.btnCancel = new Button();
			this.btnSave = new Button();
			this.label1 = new Label();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.dgvGpmember).BeginInit();
			base.SuspendLayout();
			this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cboType, "cboType");
			this.cboType.FormattingEnabled = true;
			this.cboType.Name = "cboType";
			this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
			this.groupBox1.Controls.Add(this.dgvGpmember);
			this.groupBox1.Controls.Add(this.chkAll);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.dgvGpmember.AllowUserToAddRows = false;
			this.dgvGpmember.AllowUserToDeleteRows = false;
			this.dgvGpmember.AllowUserToResizeColumns = false;
			this.dgvGpmember.AllowUserToResizeRows = false;
			this.dgvGpmember.BackgroundColor = Color.White;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvGpmember.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvGpmember, "dgvGpmember");
			this.dgvGpmember.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvGpmember.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvGpmember.Name = "dgvGpmember";
			this.dgvGpmember.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvGpmember.RowHeadersVisible = false;
			this.dgvGpmember.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvGpmember.RowTemplate.ReadOnly = true;
			this.dgvGpmember.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvGpmember.StandardTab = true;
			this.dgvGpmember.TabStop = false;
			this.dgvGpmember.SelectionChanged += new System.EventHandler(this.dgvGpmember_SelectionChanged);
			this.chkAll.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.chkAll, "chkAll");
			this.chkAll.ForeColor = Color.Black;
			this.chkAll.Name = "chkAll";
			this.chkAll.UseVisualStyleBackColor = false;
			this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
			componentResourceManager.ApplyResources(this.lbname, "lbname");
			this.lbname.Name = "lbname";
			componentResourceManager.ApplyResources(this.tbname, "tbname");
			this.tbname.Name = "tbname";
			this.tbname.KeyPress += new KeyPressEventHandler(this.txtname_KeyPress);
			this.btnCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.btnCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.cboType);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.lbname);
			base.Controls.Add(this.tbname);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnSave);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DataGroupDlg";
			base.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.dgvGpmember).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
