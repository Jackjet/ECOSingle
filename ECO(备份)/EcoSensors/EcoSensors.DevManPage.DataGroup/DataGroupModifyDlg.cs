using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Properties;
using EventLogAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace EcoSensors.DevManPage.DataGroup
{
	public class DataGroupModifyDlg : Form
	{
		private IContainer components;
		private Button btnCancel;
		private Button btnSave;
		private Label lbname;
		private TextBox tbname;
		private Label label1;
		private GroupBox availObj;
		private DataGridView dgvAvail;
		private CheckBox AvailchkAll;
		private Button btnAvFilter;
		private TextBox tbAvlKey;
		private GroupBox gpMember;
		private Button btnMemberFilter;
		private TextBox tbMemberKey;
		private DataGridView dgvMembers;
		private CheckBox MemberchkAll;
		private Button btnAdd;
		private Button btnMove;
		private ComboBox cboType;
		private DataGroup m_Parent2;
		private long m_groupID;
		private string m_groupType;
		private bool AvailchkAll_changeonly;
		private bool MemberchkAll_changeonly;
		private DataTable Avail_tb;
		private DataTable member_tb;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataGroupModifyDlg));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			this.btnCancel = new Button();
			this.btnSave = new Button();
			this.lbname = new Label();
			this.tbname = new TextBox();
			this.label1 = new Label();
			this.availObj = new GroupBox();
			this.btnAvFilter = new Button();
			this.tbAvlKey = new TextBox();
			this.dgvAvail = new DataGridView();
			this.AvailchkAll = new CheckBox();
			this.gpMember = new GroupBox();
			this.btnMemberFilter = new Button();
			this.tbMemberKey = new TextBox();
			this.dgvMembers = new DataGridView();
			this.MemberchkAll = new CheckBox();
			this.btnAdd = new Button();
			this.btnMove = new Button();
			this.cboType = new ComboBox();
			this.availObj.SuspendLayout();
			((ISupportInitialize)this.dgvAvail).BeginInit();
			this.gpMember.SuspendLayout();
			((ISupportInitialize)this.dgvMembers).BeginInit();
			base.SuspendLayout();
			this.btnCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			componentResourceManager.ApplyResources(this.lbname, "lbname");
			this.lbname.Name = "lbname";
			componentResourceManager.ApplyResources(this.tbname, "tbname");
			this.tbname.Name = "tbname";
			this.tbname.KeyPress += new KeyPressEventHandler(this.txtname_KeyPress);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.availObj.Controls.Add(this.btnAvFilter);
			this.availObj.Controls.Add(this.tbAvlKey);
			this.availObj.Controls.Add(this.dgvAvail);
			this.availObj.Controls.Add(this.AvailchkAll);
			componentResourceManager.ApplyResources(this.availObj, "availObj");
			this.availObj.ForeColor = Color.FromArgb(20, 73, 160);
			this.availObj.Name = "availObj";
			this.availObj.TabStop = false;
			componentResourceManager.ApplyResources(this.btnAvFilter, "btnAvFilter");
			this.btnAvFilter.ForeColor = SystemColors.ControlText;
			this.btnAvFilter.Image = Resources.filter;
			this.btnAvFilter.Name = "btnAvFilter";
			this.btnAvFilter.UseVisualStyleBackColor = true;
			this.btnAvFilter.Click += new System.EventHandler(this.btnAvFilter_Click);
			componentResourceManager.ApplyResources(this.tbAvlKey, "tbAvlKey");
			this.tbAvlKey.Name = "tbAvlKey";
			this.dgvAvail.AllowUserToAddRows = false;
			this.dgvAvail.AllowUserToDeleteRows = false;
			this.dgvAvail.AllowUserToResizeColumns = false;
			this.dgvAvail.AllowUserToResizeRows = false;
			this.dgvAvail.BackgroundColor = Color.White;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvAvail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvAvail, "dgvAvail");
			this.dgvAvail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvAvail.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvAvail.Name = "dgvAvail";
			this.dgvAvail.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvAvail.RowHeadersVisible = false;
			this.dgvAvail.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvAvail.RowTemplate.ReadOnly = true;
			this.dgvAvail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvAvail.StandardTab = true;
			this.dgvAvail.TabStop = false;
			this.dgvAvail.SelectionChanged += new System.EventHandler(this.dgvAvail_SelectionChanged);
			this.AvailchkAll.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.AvailchkAll, "AvailchkAll");
			this.AvailchkAll.ForeColor = Color.Black;
			this.AvailchkAll.Name = "AvailchkAll";
			this.AvailchkAll.UseVisualStyleBackColor = false;
			this.AvailchkAll.CheckedChanged += new System.EventHandler(this.AvailchkAll_CheckedChanged);
			this.gpMember.Controls.Add(this.btnMemberFilter);
			this.gpMember.Controls.Add(this.tbMemberKey);
			this.gpMember.Controls.Add(this.dgvMembers);
			this.gpMember.Controls.Add(this.MemberchkAll);
			componentResourceManager.ApplyResources(this.gpMember, "gpMember");
			this.gpMember.ForeColor = Color.FromArgb(20, 73, 160);
			this.gpMember.Name = "gpMember";
			this.gpMember.TabStop = false;
			componentResourceManager.ApplyResources(this.btnMemberFilter, "btnMemberFilter");
			this.btnMemberFilter.ForeColor = SystemColors.ControlText;
			this.btnMemberFilter.Image = Resources.filter;
			this.btnMemberFilter.Name = "btnMemberFilter";
			this.btnMemberFilter.UseVisualStyleBackColor = true;
			this.btnMemberFilter.Click += new System.EventHandler(this.btnMemberFilter_Click);
			componentResourceManager.ApplyResources(this.tbMemberKey, "tbMemberKey");
			this.tbMemberKey.Name = "tbMemberKey";
			this.dgvMembers.AllowUserToAddRows = false;
			this.dgvMembers.AllowUserToDeleteRows = false;
			this.dgvMembers.AllowUserToResizeColumns = false;
			this.dgvMembers.AllowUserToResizeRows = false;
			this.dgvMembers.BackgroundColor = Color.White;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvMembers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			componentResourceManager.ApplyResources(this.dgvMembers, "dgvMembers");
			this.dgvMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = SystemColors.Window;
			dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
			this.dgvMembers.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvMembers.Name = "dgvMembers";
			this.dgvMembers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvMembers.RowHeadersVisible = false;
			this.dgvMembers.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvMembers.RowTemplate.ReadOnly = true;
			this.dgvMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvMembers.StandardTab = true;
			this.dgvMembers.TabStop = false;
			this.dgvMembers.SelectionChanged += new System.EventHandler(this.dgvMembers_SelectionChanged);
			this.MemberchkAll.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.MemberchkAll, "MemberchkAll");
			this.MemberchkAll.ForeColor = Color.Black;
			this.MemberchkAll.Name = "MemberchkAll";
			this.MemberchkAll.UseVisualStyleBackColor = false;
			this.MemberchkAll.CheckedChanged += new System.EventHandler(this.MemberchkAll_CheckedChanged);
			componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			componentResourceManager.ApplyResources(this.btnMove, "btnMove");
			this.btnMove.Name = "btnMove";
			this.btnMove.UseVisualStyleBackColor = true;
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cboType, "cboType");
			this.cboType.FormattingEnabled = true;
			this.cboType.Name = "cboType";
			this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.btnCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.cboType);
			base.Controls.Add(this.btnMove);
			base.Controls.Add(this.btnAdd);
			base.Controls.Add(this.gpMember);
			base.Controls.Add(this.availObj);
			base.Controls.Add(this.lbname);
			base.Controls.Add(this.tbname);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnSave);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DataGroupModifyDlg";
			base.ShowInTaskbar = false;
			this.availObj.ResumeLayout(false);
			this.availObj.PerformLayout();
			((ISupportInitialize)this.dgvAvail).EndInit();
			this.gpMember.ResumeLayout(false);
			this.gpMember.PerformLayout();
			((ISupportInitialize)this.dgvMembers).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public DataGroupModifyDlg()
		{
			this.InitializeComponent();
			this.tbname.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbAvlKey.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMemberKey.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public DataGroupModifyDlg(DataGroup parent, long groupID)
		{
			this.InitializeComponent();
			this.tbname.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbAvlKey.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbMemberKey.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.m_Parent2 = parent;
			this.m_groupID = groupID;
			this.cboType.Items.Clear();
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPZone, new string[0]));
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPRack, new string[0]));
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPDev, new string[0]));
			this.cboType.Items.Add(EcoLanguage.getMsg(LangRes.Group_TPOutlet, new string[0]));
			if (this.m_groupID >= 0L)
			{
				GroupInfo groupByID = GroupInfo.GetGroupByID(this.m_groupID);
				this.m_groupType = groupByID.GroupType;
				this.tbname.Text = groupByID.GroupName;
				this.cboType.Enabled = false;
				string groupType;
				switch (groupType = groupByID.GroupType)
				{
				case "zone":
					this.cboType.SelectedIndex = 0;
					break;
				case "rack":
				case "allrack":
					this.cboType.SelectedIndex = 1;
					break;
				case "dev":
				case "alldev":
					this.cboType.SelectedIndex = 2;
					break;
				case "outlet":
				case "alloutlet":
					this.cboType.SelectedIndex = 3;
					break;
				}
				this.cboType.Enabled = false;
				return;
			}
			this.cboType.SelectedIndex = 0;
			this.cboType.Enabled = true;
		}


        System.Collections.Generic.Dictionary<string, int> cct = null;

		private void init_Avail_data()
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			if (this.m_groupID >= 0L)
			{
				GroupInfo groupByID = GroupInfo.GetGroupByID(this.m_groupID);
				string memberList = groupByID.GetMemberList();
				if (memberList != null && memberList.Length > 0)
				{
					string[] array = memberList.Split(new string[]
					{
						","
					}, System.StringSplitOptions.RemoveEmptyEntries);
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string value = array2[i];
						long key = (long)System.Convert.ToInt32(value);
						dictionary.Add(key, "");
					}
				}
			}
			this.dgvAvail.DataSource = null;
			this.Avail_tb = new DataTable();
			this.Avail_tb.Columns.Add("objID", typeof(string));
			this.Avail_tb.PrimaryKey = new DataColumn[]
			{
				this.Avail_tb.Columns[0]
			};
			string groupType;
			if ((groupType = this.m_groupType) != null)
			{
				if (cct == null)
				{
					cct = new System.Collections.Generic.Dictionary<string, int>(7)
					{

						{
							"zone",
							0
						},

						{
							"rack",
							1
						},

						{
							"allrack",
							2
						},

						{
							"dev",
							3
						},

						{
							"alldev",
							4
						},

						{
							"outlet",
							5
						},

						{
							"alloutlet",
							6
						}
					};
				}
				int num;
                if (cct.TryGetValue(groupType, out num))
				{
					switch (num)
					{
					case 0:
					{
						this.Avail_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
						System.Collections.ArrayList allZone = ZoneInfo.getAllZone();
						System.Collections.IEnumerator enumerator = allZone.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								ZoneInfo zoneInfo = (ZoneInfo)enumerator.Current;
								string text = zoneInfo.ZoneID.ToString();
								if (!dictionary.ContainsKey(zoneInfo.ZoneID))
								{
									string[] values = new string[]
									{
										text,
										zoneInfo.ZoneName
									};
									this.Avail_tb.Rows.Add(values);
								}
							}
							return;
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
					case 2:
						break;
					case 3:
					case 4:
						goto IL_401;
					case 5:
					case 6:
						goto IL_50F;
					default:
						return;
					}
					this.Avail_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
					this.Avail_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
					System.Collections.ArrayList allZone2 = ZoneInfo.getAllZone();
					System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
					System.Collections.IEnumerator enumerator2 = allRack_NoEmpty.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							RackInfo rackInfo = (RackInfo)enumerator2.Current;
							if (!dictionary.ContainsKey(rackInfo.RackID))
							{
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
								this.Avail_tb.Rows.Add(values);
							}
						}
						return;
					}
					finally
					{
						System.IDisposable disposable3 = enumerator2 as System.IDisposable;
						if (disposable3 != null)
						{
							disposable3.Dispose();
						}
					}
					IL_401:
					this.Avail_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
					this.Avail_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
					System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
					using (System.Collections.Generic.List<DeviceInfo>.Enumerator enumerator4 = allDevice.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							DeviceInfo current = enumerator4.Current;
							if (!dictionary.ContainsKey((long)current.DeviceID))
							{
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
								this.Avail_tb.Rows.Add(values);
							}
						}
						return;
					}
					IL_50F:
					this.Avail_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMOutlet, new string[0]), typeof(string));
					this.Avail_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
					allDevice = DeviceOperation.GetAllDevice();
					foreach (DeviceInfo current2 in allDevice)
					{
						System.Collections.Generic.List<PortInfo> portInfo = current2.GetPortInfo();
						foreach (PortInfo current3 in portInfo)
						{
							if (!dictionary.ContainsKey((long)current3.ID))
							{
								string[] values = new string[]
								{
									current3.ID.ToString(),
									current3.PortName,
									current2.DeviceName
								};
								this.Avail_tb.Rows.Add(values);
							}
						}
					}
				}
			}
        
		}
		private void show_Avail_tb()
		{
			string text = this.tbAvlKey.Text;
			DataTable dataTable = this.Avail_tb.Clone();
			foreach (DataRow dataRow in this.Avail_tb.Rows)
			{
				string groupType;
				if ((groupType = this.m_groupType) != null && groupType == "zone")
				{
					string text2 = (string)dataRow[1];
					if (text.Length <= 0 || text2.Contains(text))
					{
						dataTable.ImportRow(dataRow);
					}
				}
				else
				{
					string text2 = (string)dataRow[1];
					string text3 = (string)dataRow[2];
					if (text.Length <= 0 || text2.Contains(text) || text3.Contains(text))
					{
						dataTable.ImportRow(dataRow);
					}
				}
			}
			DataGridViewColumn sortedColumn = this.dgvAvail.SortedColumn;
			SortOrder sortOrder = this.dgvAvail.SortOrder;
			this.dgvAvail.DataSource = null;
			this.dgvAvail.DataSource = dataTable;
			this.dgvAvail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.dgvAvail.Columns[0].Visible = false;
			string groupType2;
			if ((groupType2 = this.m_groupType) != null && groupType2 == "zone")
			{
				this.dgvAvail.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			else
			{
				this.dgvAvail.Columns[1].Width = 140;
				this.dgvAvail.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			if (sortedColumn != null)
			{
				ListSortDirection direction = (sortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
				this.dgvAvail.Sort(this.dgvAvail.Columns[sortedColumn.HeaderText], direction);
			}
			if (this.dgvAvail.Rows.Count == 0)
			{
				this.btnAdd.Enabled = false;
				return;
			}
			this.btnAdd.Enabled = true;
		}
		private void init_member_tb()
		{
			this.member_tb = new DataTable();
			this.member_tb.Columns.Add("objID", typeof(string));
			this.member_tb.PrimaryKey = new DataColumn[]
			{
				this.member_tb.Columns[0]
			};
			string groupType;
			switch (groupType = this.m_groupType)
			{
			case "zone":
				this.member_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
				break;
			case "rack":
			case "allrack":
				this.member_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
				this.member_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMZone, new string[0]), typeof(string));
				break;
			case "dev":
			case "alldev":
				this.member_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
				this.member_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMRack, new string[0]), typeof(string));
				break;
			case "outlet":
			case "alloutlet":
				this.member_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMOutlet, new string[0]), typeof(string));
				this.member_tb.Columns.Add(EcoLanguage.getMsg(LangRes.Group_NMDev, new string[0]), typeof(string));
				break;
			}
			if (this.m_groupID >= 0L)
			{
				DataTable memberNameInfo = GroupInfo.GetMemberNameInfo(this.m_groupID);
				string groupType2;
				if ((groupType2 = this.m_groupType) != null && groupType2 == "zone")
				{
					System.Collections.IEnumerator enumerator = memberNameInfo.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							this.member_tb.Rows.Add(new object[]
							{
								dataRow["memberid"],
								dataRow["name"]
							});
						}
						return;
					}
					finally
					{
						System.IDisposable disposable = enumerator as System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				foreach (DataRow dataRow2 in memberNameInfo.Rows)
				{
					this.member_tb.Rows.Add(new object[]
					{
						dataRow2["memberid"],
						dataRow2["name"],
						dataRow2["parentname"]
					});
				}
			}
		}
		private void show_member_tb()
		{
			string text = this.tbMemberKey.Text;
			DataTable dataTable = this.member_tb.Clone();
			foreach (DataRow dataRow in this.member_tb.Rows)
			{
				string groupType;
				if ((groupType = this.m_groupType) != null && groupType == "zone")
				{
					string text2 = (string)dataRow[1];
					if (text.Length <= 0 || text2.Contains(text))
					{
						dataTable.ImportRow(dataRow);
					}
				}
				else
				{
					string text2 = (string)dataRow[1];
					string text3 = (string)dataRow[2];
					if (text.Length <= 0 || text2.Contains(text) || text3.Contains(text))
					{
						dataTable.ImportRow(dataRow);
					}
				}
			}
			DataGridViewColumn sortedColumn = this.dgvMembers.SortedColumn;
			SortOrder sortOrder = this.dgvMembers.SortOrder;
			this.dgvMembers.DataSource = null;
			this.dgvMembers.DataSource = dataTable;
			this.dgvMembers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.dgvMembers.Columns[0].Visible = false;
			string groupType2;
			if ((groupType2 = this.m_groupType) != null && groupType2 == "zone")
			{
				this.dgvMembers.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			else
			{
				this.dgvMembers.Columns[1].Width = 140;
				this.dgvMembers.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			if (sortedColumn != null)
			{
				ListSortDirection direction = (sortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
				this.dgvMembers.Sort(this.dgvMembers.Columns[sortedColumn.HeaderText], direction);
			}
			if (this.dgvMembers.Rows.Count == 0)
			{
				this.btnMove.Enabled = false;
				return;
			}
			this.btnMove.Enabled = true;
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
			if (!GroupInfo.CheckGroupName(this.m_groupID, text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Group_nmdup, new string[]
				{
					text
				}));
				this.tbname.Focus();
				return;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.List<long> list = new System.Collections.Generic.List<long>();
			string text2;
			foreach (DataRow dataRow in this.member_tb.Rows)
			{
				text2 = (string)dataRow[0];
				stringBuilder.Append(text2 + ",");
				list.Add((long)System.Convert.ToInt32(text2));
			}
			text2 = stringBuilder.ToString();
			if (text2.Length == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_selectneed, new string[0]));
				return;
			}
			if (this.m_groupID >= 0L)
			{
				GroupInfo groupByID = GroupInfo.GetGroupByID(this.m_groupID);
				groupByID.GroupName = text;
				groupByID.Members = text2;
				switch (groupByID.Update())
				{
				case -2:
				case -1:
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
					return;
				case 0:
					break;
				case 1:
					LogAPI.writeEventLog("0430032", new string[]
					{
						groupByID.GroupName,
						groupByID.GroupType,
						EcoGlobalVar.gl_LoginUser.UserName
					});
					EcoGlobalVar.setDashBoardFlg(520uL, "", 64);
					EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
					base.DialogResult = DialogResult.OK;
					base.Close();
					return;
				default:
					return;
				}
			}
			else
			{
				int num = GroupInfo.CreateNewGroup(text, this.m_groupType, "", list);
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
				LogAPI.writeEventLog("0430030", new string[]
				{
					text,
					this.m_groupType,
					EcoGlobalVar.gl_LoginUser.UserName
				});
				EcoGlobalVar.setDashBoardFlg(512uL, "", 64);
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}
		private void AvailchkAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.AvailchkAll_changeonly)
			{
				this.AvailchkAll_changeonly = false;
				return;
			}
			if (this.AvailchkAll.Checked)
			{
				this.dgvAvail.SelectAll();
				return;
			}
			this.dgvAvail.ClearSelection();
		}
		private void MemberchkAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.MemberchkAll_changeonly)
			{
				this.MemberchkAll_changeonly = false;
				return;
			}
			if (this.MemberchkAll.Checked)
			{
				this.dgvMembers.SelectAll();
				return;
			}
			this.dgvMembers.ClearSelection();
		}
		private void btnAvFilter_Click(object sender, System.EventArgs e)
		{
			this.show_Avail_tb();
		}
		private void btnMemberFilter_Click(object sender, System.EventArgs e)
		{
			this.show_member_tb();
		}
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			for (int i = 0; i < this.dgvAvail.Rows.Count; i++)
			{
				if (this.dgvAvail.Rows[i].Selected)
				{
					string key = this.dgvAvail.Rows[i].Cells[0].Value.ToString();
					DataRow row = this.Avail_tb.Rows.Find(key);
					this.member_tb.ImportRow(row);
					this.Avail_tb.Rows.Remove(row);
				}
			}
			this.tbMemberKey.Text = "";
			this.show_Avail_tb();
			this.show_member_tb();
			this.AvailchkAll.Checked = false;
			this.MemberchkAll.Checked = false;
		}
		private void btnMove_Click(object sender, System.EventArgs e)
		{
			for (int i = 0; i < this.dgvMembers.Rows.Count; i++)
			{
				if (this.dgvMembers.Rows[i].Selected)
				{
					string key = this.dgvMembers.Rows[i].Cells[0].Value.ToString();
					DataRow row = this.member_tb.Rows.Find(key);
					this.Avail_tb.ImportRow(row);
					this.member_tb.Rows.Remove(row);
				}
			}
			this.tbAvlKey.Text = "";
			this.show_Avail_tb();
			this.show_member_tb();
			this.AvailchkAll.Checked = false;
			this.MemberchkAll.Checked = false;
		}
		private void dgvAvail_SelectionChanged(object sender, System.EventArgs e)
		{
			int count = this.dgvAvail.SelectedRows.Count;
			int count2 = this.dgvAvail.Rows.Count;
			if (count != count2 && this.AvailchkAll.Checked)
			{
				this.AvailchkAll_changeonly = true;
				this.AvailchkAll.Checked = false;
			}
		}
		private void dgvMembers_SelectionChanged(object sender, System.EventArgs e)
		{
			int count = this.dgvMembers.SelectedRows.Count;
			int count2 = this.dgvMembers.Rows.Count;
			if (count != count2 && this.MemberchkAll.Checked)
			{
				this.MemberchkAll_changeonly = true;
				this.MemberchkAll.Checked = false;
			}
		}
		private void cboType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.AvailchkAll.Checked)
			{
				this.AvailchkAll_changeonly = true;
				this.AvailchkAll.Checked = false;
			}
			if (this.MemberchkAll.Checked)
			{
				this.MemberchkAll_changeonly = true;
				this.MemberchkAll.Checked = false;
			}
			switch (this.cboType.SelectedIndex)
			{
			case 0:
				this.m_groupType = "zone";
				break;
			case 1:
				this.m_groupType = "rack";
				break;
			case 2:
				this.m_groupType = "dev";
				break;
			case 3:
				this.m_groupType = "outlet";
				break;
			}
			this.init_Avail_data();
			this.show_Avail_tb();
			this.init_member_tb();
			this.show_member_tb();
		}
	}
}
