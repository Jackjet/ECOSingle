using CommonAPI.Global;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice._Dev
{
	public class PropPOP : UserControl
	{
		private const string Prompt_NA = "N/A";
		private const string Prompt_Outlet = "Outlet ";
		private const string Prompt_Priority = "Priority";
		private IContainer components;
		private GroupBox gbPOPConfig;
		private GroupBox gbDevInfo;
		private Label lbDevNM;
		private Label lbDevIP;
		private Label lbDevRack;
		private Label lbDevModel;
		private Label labDevNm;
		private Label labDevIp;
		private Label labDevModel;
		private Label labDevRackNm;
		private GroupBox gbPop2;
		private CheckBox pop2_cbBankPriority;
		private CheckBox pop2_cbBankLIFO;
		private CheckBox pop2_cbOutlet;
		private GroupBox gbPop;
		private Label lbUdefUnit;
		private TextBox tbPopThreshold;
		private RadioButton rbPopUsrdef;
		private RadioButton rbPopMaxBankC;
		private Label lbPop;
		private CheckBox cbPopEnable;
		private Button butSave;
		private ToolTip toolTip1;
		private GroupBox gbPOPBankPriority;
		private DataGridView dgvBank1_PList;
		private Label lbbank1;
		private DataGridViewTextBoxColumn priority;
		private DataGridViewComboBoxColumn port;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private Label lbbank2;
		private DataGridView dgvBank2_PList;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
		private string PopMaxBank_txt;
		private System.Collections.Generic.List<string> m_Bank1_Priority_ComboStrings;
		private System.Collections.Generic.List<string> m_Bank2_Priority_ComboStrings;
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropPOP));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			this.gbPOPConfig = new GroupBox();
			this.gbDevInfo = new GroupBox();
			this.lbDevNM = new Label();
			this.lbDevIP = new Label();
			this.lbDevRack = new Label();
			this.lbDevModel = new Label();
			this.labDevNm = new Label();
			this.labDevIp = new Label();
			this.labDevModel = new Label();
			this.labDevRackNm = new Label();
			this.gbPop = new GroupBox();
			this.lbUdefUnit = new Label();
			this.tbPopThreshold = new TextBox();
			this.rbPopUsrdef = new RadioButton();
			this.rbPopMaxBankC = new RadioButton();
			this.lbPop = new Label();
			this.cbPopEnable = new CheckBox();
			this.gbPop2 = new GroupBox();
			this.pop2_cbBankPriority = new CheckBox();
			this.pop2_cbBankLIFO = new CheckBox();
			this.pop2_cbOutlet = new CheckBox();
			this.gbPOPBankPriority = new GroupBox();
			this.lbbank2 = new Label();
			this.dgvBank2_PList = new DataGridView();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewComboBoxColumn1 = new DataGridViewComboBoxColumn();
			this.lbbank1 = new Label();
			this.dgvBank1_PList = new DataGridView();
			this.priority = new DataGridViewTextBoxColumn();
			this.port = new DataGridViewComboBoxColumn();
			this.butSave = new Button();
			this.toolTip1 = new ToolTip(this.components);
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.gbPOPConfig.SuspendLayout();
			this.gbDevInfo.SuspendLayout();
			this.gbPop.SuspendLayout();
			this.gbPop2.SuspendLayout();
			this.gbPOPBankPriority.SuspendLayout();
			((ISupportInitialize)this.dgvBank2_PList).BeginInit();
			((ISupportInitialize)this.dgvBank1_PList).BeginInit();
			base.SuspendLayout();
			this.gbPOPConfig.Controls.Add(this.gbDevInfo);
			this.gbPOPConfig.Controls.Add(this.gbPop);
			this.gbPOPConfig.Controls.Add(this.gbPop2);
			this.gbPOPConfig.Controls.Add(this.gbPOPBankPriority);
			componentResourceManager.ApplyResources(this.gbPOPConfig, "gbPOPConfig");
			this.gbPOPConfig.ForeColor = SystemColors.ControlText;
			this.gbPOPConfig.Name = "gbPOPConfig";
			this.gbPOPConfig.TabStop = false;
			this.gbDevInfo.Controls.Add(this.lbDevNM);
			this.gbDevInfo.Controls.Add(this.lbDevIP);
			this.gbDevInfo.Controls.Add(this.lbDevRack);
			this.gbDevInfo.Controls.Add(this.lbDevModel);
			this.gbDevInfo.Controls.Add(this.labDevNm);
			this.gbDevInfo.Controls.Add(this.labDevIp);
			this.gbDevInfo.Controls.Add(this.labDevModel);
			this.gbDevInfo.Controls.Add(this.labDevRackNm);
			componentResourceManager.ApplyResources(this.gbDevInfo, "gbDevInfo");
			this.gbDevInfo.ForeColor = SystemColors.ControlText;
			this.gbDevInfo.Name = "gbDevInfo";
			this.gbDevInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.lbDevNM, "lbDevNM");
			this.lbDevNM.ForeColor = Color.Black;
			this.lbDevNM.Name = "lbDevNM";
			componentResourceManager.ApplyResources(this.lbDevIP, "lbDevIP");
			this.lbDevIP.ForeColor = Color.Black;
			this.lbDevIP.Name = "lbDevIP";
			componentResourceManager.ApplyResources(this.lbDevRack, "lbDevRack");
			this.lbDevRack.ForeColor = Color.Black;
			this.lbDevRack.Name = "lbDevRack";
			componentResourceManager.ApplyResources(this.lbDevModel, "lbDevModel");
			this.lbDevModel.ForeColor = Color.Black;
			this.lbDevModel.Name = "lbDevModel";
			this.labDevNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevNm, "labDevNm");
			this.labDevNm.ForeColor = Color.Black;
			this.labDevNm.Name = "labDevNm";
			this.labDevIp.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevIp, "labDevIp");
			this.labDevIp.ForeColor = Color.Black;
			this.labDevIp.Name = "labDevIp";
			this.labDevModel.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevModel, "labDevModel");
			this.labDevModel.ForeColor = Color.Black;
			this.labDevModel.Name = "labDevModel";
			this.labDevRackNm.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.labDevRackNm, "labDevRackNm");
			this.labDevRackNm.ForeColor = Color.Black;
			this.labDevRackNm.Name = "labDevRackNm";
			this.gbPop.Controls.Add(this.lbUdefUnit);
			this.gbPop.Controls.Add(this.tbPopThreshold);
			this.gbPop.Controls.Add(this.rbPopUsrdef);
			this.gbPop.Controls.Add(this.rbPopMaxBankC);
			this.gbPop.Controls.Add(this.lbPop);
			this.gbPop.Controls.Add(this.cbPopEnable);
			componentResourceManager.ApplyResources(this.gbPop, "gbPop");
			this.gbPop.Name = "gbPop";
			this.gbPop.TabStop = false;
			componentResourceManager.ApplyResources(this.lbUdefUnit, "lbUdefUnit");
			this.lbUdefUnit.ForeColor = Color.Black;
			this.lbUdefUnit.Name = "lbUdefUnit";
			componentResourceManager.ApplyResources(this.tbPopThreshold, "tbPopThreshold");
			this.tbPopThreshold.Name = "tbPopThreshold";
			this.tbPopThreshold.KeyPress += new KeyPressEventHandler(this.tbPopThreshold_KeyPress);
			componentResourceManager.ApplyResources(this.rbPopUsrdef, "rbPopUsrdef");
			this.rbPopUsrdef.Name = "rbPopUsrdef";
			this.rbPopUsrdef.TabStop = true;
			this.rbPopUsrdef.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.rbPopMaxBankC, "rbPopMaxBankC");
			this.rbPopMaxBankC.Name = "rbPopMaxBankC";
			this.rbPopMaxBankC.TabStop = true;
			this.rbPopMaxBankC.UseVisualStyleBackColor = true;
			this.rbPopMaxBankC.CheckedChanged += new System.EventHandler(this.rbPopMaxBankC_CheckedChanged);
			componentResourceManager.ApplyResources(this.lbPop, "lbPop");
			this.lbPop.Name = "lbPop";
			componentResourceManager.ApplyResources(this.cbPopEnable, "cbPopEnable");
			this.cbPopEnable.Name = "cbPopEnable";
			this.cbPopEnable.UseVisualStyleBackColor = true;
			this.cbPopEnable.CheckedChanged += new System.EventHandler(this.cbPopEnable_CheckedChanged);
			this.gbPop2.Controls.Add(this.pop2_cbBankPriority);
			this.gbPop2.Controls.Add(this.pop2_cbBankLIFO);
			this.gbPop2.Controls.Add(this.pop2_cbOutlet);
			componentResourceManager.ApplyResources(this.gbPop2, "gbPop2");
			this.gbPop2.Name = "gbPop2";
			this.gbPop2.TabStop = false;
			componentResourceManager.ApplyResources(this.pop2_cbBankPriority, "pop2_cbBankPriority");
			this.pop2_cbBankPriority.Name = "pop2_cbBankPriority";
			this.pop2_cbBankPriority.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.pop2_cbBankLIFO, "pop2_cbBankLIFO");
			this.pop2_cbBankLIFO.Name = "pop2_cbBankLIFO";
			this.pop2_cbBankLIFO.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.pop2_cbOutlet, "pop2_cbOutlet");
			this.pop2_cbOutlet.Name = "pop2_cbOutlet";
			this.pop2_cbOutlet.UseVisualStyleBackColor = true;
			this.gbPOPBankPriority.Controls.Add(this.lbbank2);
			this.gbPOPBankPriority.Controls.Add(this.dgvBank2_PList);
			this.gbPOPBankPriority.Controls.Add(this.lbbank1);
			this.gbPOPBankPriority.Controls.Add(this.dgvBank1_PList);
			componentResourceManager.ApplyResources(this.gbPOPBankPriority, "gbPOPBankPriority");
			this.gbPOPBankPriority.Name = "gbPOPBankPriority";
			this.gbPOPBankPriority.TabStop = false;
			this.lbbank2.BackColor = Color.Gainsboro;
			this.lbbank2.BorderStyle = BorderStyle.FixedSingle;
			componentResourceManager.ApplyResources(this.lbbank2, "lbbank2");
			this.lbbank2.ForeColor = Color.FromArgb(20, 73, 160);
			this.lbbank2.Name = "lbbank2";
			this.dgvBank2_PList.AllowUserToAddRows = false;
			this.dgvBank2_PList.AllowUserToDeleteRows = false;
			this.dgvBank2_PList.AllowUserToResizeColumns = false;
			this.dgvBank2_PList.AllowUserToResizeRows = false;
			this.dgvBank2_PList.BackgroundColor = Color.WhiteSmoke;
			this.dgvBank2_PList.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.SelectionForeColor = Color.Black;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvBank2_PList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvBank2_PList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvBank2_PList.ColumnHeadersVisible = false;
			this.dgvBank2_PList.Columns.AddRange(new DataGridViewColumn[]
			{
				this.dataGridViewTextBoxColumn2,
				this.dataGridViewComboBoxColumn1
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.White;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle2.ForeColor = Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = Color.White;
			dataGridViewCellStyle2.SelectionForeColor = Color.Black;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvBank2_PList.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvBank2_PList.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvBank2_PList, "dgvBank2_PList");
			this.dgvBank2_PList.Name = "dgvBank2_PList";
			this.dgvBank2_PList.RowHeadersVisible = false;
			this.dgvBank2_PList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvBank2_PList.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvBank2_PList_CellMouseDown);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewComboBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewComboBoxColumn1, "dataGridViewComboBoxColumn1");
			this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
			this.lbbank1.BackColor = Color.Gainsboro;
			this.lbbank1.BorderStyle = BorderStyle.FixedSingle;
			componentResourceManager.ApplyResources(this.lbbank1, "lbbank1");
			this.lbbank1.ForeColor = Color.FromArgb(20, 73, 160);
			this.lbbank1.Name = "lbbank1";
			this.dgvBank1_PList.AllowUserToAddRows = false;
			this.dgvBank1_PList.AllowUserToDeleteRows = false;
			this.dgvBank1_PList.AllowUserToResizeColumns = false;
			this.dgvBank1_PList.AllowUserToResizeRows = false;
			this.dgvBank1_PList.BackgroundColor = Color.WhiteSmoke;
			this.dgvBank1_PList.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle3.SelectionForeColor = Color.Black;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvBank1_PList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvBank1_PList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvBank1_PList.ColumnHeadersVisible = false;
			this.dgvBank1_PList.Columns.AddRange(new DataGridViewColumn[]
			{
				this.priority,
				this.port
			});
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = Color.White;
			dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 9.75f);
			dataGridViewCellStyle4.ForeColor = Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = Color.White;
			dataGridViewCellStyle4.SelectionForeColor = Color.Black;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
			this.dgvBank1_PList.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvBank1_PList.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dgvBank1_PList, "dgvBank1_PList");
			this.dgvBank1_PList.Name = "dgvBank1_PList";
			this.dgvBank1_PList.RowHeadersVisible = false;
			this.dgvBank1_PList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvBank1_PList.CellMouseDown += new DataGridViewCellMouseEventHandler(this.BankPList1_CellMouseDown);
			componentResourceManager.ApplyResources(this.priority, "priority");
			this.priority.Name = "priority";
			this.priority.ReadOnly = true;
			this.port.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.port, "port");
			this.port.Name = "port";
			this.butSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSave, "butSave");
			this.butSave.Name = "butSave";
			this.butSave.UseVisualStyleBackColor = false;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.butSave);
			base.Controls.Add(this.gbPOPConfig);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropPOP";
			this.gbPOPConfig.ResumeLayout(false);
			this.gbDevInfo.ResumeLayout(false);
			this.gbPop.ResumeLayout(false);
			this.gbPop.PerformLayout();
			this.gbPop2.ResumeLayout(false);
			this.gbPOPBankPriority.ResumeLayout(false);
			((ISupportInitialize)this.dgvBank2_PList).EndInit();
			((ISupportInitialize)this.dgvBank1_PList).EndInit();
			base.ResumeLayout(false);
		}
		public PropPOP()
		{
			this.InitializeComponent();
			this.PopMaxBank_txt = this.rbPopMaxBankC.Text;
			this.tbPopThreshold.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit(int devID, bool onlinest)
		{
			this.butSave.Enabled = onlinest;
			DeviceInfo deviceByID = DeviceOperation.getDeviceByID(devID);
			this.labDevNm.Text = deviceByID.DeviceName;
			this.labDevModel.Text = deviceByID.ModelNm;
			this.labDevModel.Tag = devID.ToString();
			string text = deviceByID.ModelNm;
			if (DevAccessCfg.GetInstance().isAutodectDev(deviceByID.ModelNm, deviceByID.FWVersion))
			{
				text = text + " (F/W: " + deviceByID.FWVersion + ")";
			}
			this.toolTip1.SetToolTip(this.labDevModel, text);
			this.labDevIp.Text = deviceByID.DeviceIP;
			this.labDevIp.Tag = deviceByID.FWVersion;
			this.labDevNm.Text = deviceByID.DeviceName;
			RackInfo rackByID = RackInfo.getRackByID(deviceByID.RackID);
			this.labDevRackNm.Text = rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(deviceByID.ModelNm, deviceByID.FWVersion);
			if (deviceModelConfig.popReading != 2)
			{
				this.gbPop.Hide();
				this.gbPop2.Hide();
				this.gbPOPBankPriority.Hide();
				return;
			}
			if (deviceModelConfig.popNewRule == Constant.YES)
			{
				this.gbPop.Hide();
				this.gbPop2.Show();
				if (deviceModelConfig.perportreading == Constant.NO)
				{
					this.pop2_cbOutlet.Hide();
					this.pop2_cbBankLIFO.Hide();
				}
				else
				{
					this.pop2_cbOutlet.Show();
					this.pop2_cbBankLIFO.Show();
				}
				if (deviceByID.OutletPOPMode == 1)
				{
					this.pop2_cbOutlet.Checked = false;
				}
				else
				{
					this.pop2_cbOutlet.Checked = true;
				}
				if (deviceByID.BankPOPLIFOMode == 1)
				{
					this.pop2_cbBankLIFO.Checked = false;
				}
				else
				{
					this.pop2_cbBankLIFO.Checked = true;
				}
				if (deviceByID.BankPOPPriorityMode == 1)
				{
					this.pop2_cbBankPriority.Checked = false;
				}
				else
				{
					this.pop2_cbBankPriority.Checked = true;
				}
				if (deviceModelConfig.popPrioritySupport == Constant.NO)
				{
					this.gbPOPBankPriority.Hide();
					return;
				}
				this.gbPOPBankPriority.Show();
				string[] array = deviceByID.Bank_Priority.Split(new char[]
				{
					'#'
				});
				string[] array2 = array[0].Split(new char[]
				{
					','
				});
				string[] array3 = null;
				if (array.Length >= 2)
				{
					array3 = array[1].Split(new char[]
					{
						','
					});
				}
				this.dgvBank1_PList.Rows.Clear();
				DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)this.dgvBank1_PList.Columns[1];
				this.m_Bank1_Priority_ComboStrings = new System.Collections.Generic.List<string>();
				dataGridViewComboBoxColumn.Items.Clear();
				dataGridViewComboBoxColumn.Items.Add("N/A");
				this.m_Bank1_Priority_ComboStrings.Add("N/A");
				int num;
				int num2;
				if (deviceModelConfig.bankNum == 0)
				{
					num = 1;
					num2 = deviceModelConfig.portNum;
				}
				else
				{
					num = deviceModelConfig.bankOutlets[0].fromPort;
					num2 = deviceModelConfig.bankOutlets[0].toPort;
				}
				for (int i = num; i <= num2; i++)
				{
					if (deviceModelConfig.isOutletSwitchable(i - 1))
					{
						dataGridViewComboBoxColumn.Items.Add("Outlet " + i.ToString());
						this.m_Bank1_Priority_ComboStrings.Add("Outlet " + i.ToString());
					}
				}
				int num3 = 0;
				for (int j = num; j <= num2; j++)
				{
					if (deviceModelConfig.isOutletSwitchable(j - 1))
					{
						if (array2[num3].Equals("0"))
						{
							this.dgvBank1_PList.Rows.Add(new object[]
							{
								"Priority" + (num3 + 1).ToString(),
								"N/A"
							});
						}
						else
						{
							string text2 = "Outlet " + System.Convert.ToInt16(array2[num3]).ToString();
							if (this.m_Bank1_Priority_ComboStrings.Contains(text2))
							{
								this.dgvBank1_PList.Rows.Add(new object[]
								{
									"Priority" + (num3 + 1).ToString(),
									text2
								});
							}
							else
							{
								this.dgvBank1_PList.Rows.Add(new object[]
								{
									"Priority" + (num3 + 1).ToString(),
									"N/A"
								});
							}
						}
						num3++;
					}
				}
				if (deviceModelConfig.bankNum >= 2)
				{
					this.lbbank2.Visible = true;
					this.dgvBank2_PList.Visible = true;
					this.dgvBank2_PList.Rows.Clear();
					dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)this.dgvBank2_PList.Columns[1];
					this.m_Bank2_Priority_ComboStrings = new System.Collections.Generic.List<string>();
					dataGridViewComboBoxColumn.Items.Clear();
					dataGridViewComboBoxColumn.Items.Add("N/A");
					this.m_Bank2_Priority_ComboStrings.Add("N/A");
					for (int k = deviceModelConfig.bankOutlets[1].fromPort; k <= deviceModelConfig.bankOutlets[1].toPort; k++)
					{
						if (deviceModelConfig.isOutletSwitchable(k - 1))
						{
							dataGridViewComboBoxColumn.Items.Add("Outlet " + k.ToString());
							this.m_Bank2_Priority_ComboStrings.Add("Outlet " + k.ToString());
						}
					}
					num3 = 0;
					for (int l = deviceModelConfig.bankOutlets[1].fromPort; l <= deviceModelConfig.bankOutlets[1].toPort; l++)
					{
						if (deviceModelConfig.isOutletSwitchable(l - 1))
						{
							if (array3[num3].Equals("0"))
							{
								this.dgvBank2_PList.Rows.Add(new object[]
								{
									"Priority" + (num3 + 1).ToString(),
									"N/A"
								});
							}
							else
							{
								string text2 = "Outlet " + System.Convert.ToInt16(array3[num3]).ToString();
								if (this.m_Bank2_Priority_ComboStrings.Contains(text2))
								{
									this.dgvBank2_PList.Rows.Add(new object[]
									{
										"Priority" + (num3 + 1).ToString(),
										text2
									});
								}
								else
								{
									this.dgvBank2_PList.Rows.Add(new object[]
									{
										"Priority" + (num3 + 1).ToString(),
										"N/A"
									});
								}
							}
							num3++;
						}
					}
					return;
				}
				this.lbbank2.Visible = false;
				this.dgvBank2_PList.Visible = false;
				return;
			}
			else
			{
				if (deviceByID.POPThreshold == -500f)
				{
					this.gbPop.Hide();
					this.gbPop2.Hide();
					this.gbPOPBankPriority.Hide();
					return;
				}
				this.gbPop.Show();
				this.gbPop2.Hide();
				this.gbPOPBankPriority.Hide();
				this.rbPopMaxBankC.Text = this.PopMaxBank_txt + " " + deviceModelConfig.popDefault.ToString("F1") + "A";
				if (deviceByID.POPEnableMode == 1)
				{
					this.cbPopEnable.Checked = false;
					this.rbPopMaxBankC.Checked = true;
					this.tbPopThreshold.Text = "";
					this.cbPopEnable_CheckedChanged(this.cbPopEnable, null);
					return;
				}
				if (deviceByID.POPThreshold < 0f)
				{
					this.cbPopEnable.Checked = true;
					this.rbPopMaxBankC.Checked = true;
					this.tbPopThreshold.Text = "";
					return;
				}
				this.cbPopEnable.Checked = true;
				this.rbPopUsrdef.Checked = true;
				this.tbPopThreshold.Text = deviceByID.POPThreshold.ToString("F1");
				return;
			}
		}
		public void TimerProc(bool onlinest, int haveThresholdChange)
		{
			if (haveThresholdChange == 1)
			{
				string value = this.labDevModel.Tag.ToString();
				int devID = System.Convert.ToInt32(value);
				this.pageInit(devID, onlinest);
				return;
			}
			this.butSave.Enabled = onlinest;
		}
		private void cbPopEnable_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.cbPopEnable.Checked)
			{
				this.rbPopMaxBankC.Enabled = false;
				this.rbPopUsrdef.Enabled = false;
				this.tbPopThreshold.Enabled = false;
				return;
			}
			this.rbPopMaxBankC.Enabled = true;
			this.rbPopUsrdef.Enabled = true;
			if (!this.rbPopMaxBankC.Checked)
			{
				this.tbPopThreshold.Enabled = true;
				return;
			}
			this.tbPopThreshold.Enabled = false;
		}
		private void rbPopMaxBankC_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rbPopMaxBankC.Checked)
			{
				this.tbPopThreshold.Enabled = false;
				return;
			}
			this.tbPopThreshold.Enabled = true;
		}
		private void tbPopThreshold_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(tb, e.KeyChar, 1);
			if (flag)
			{
				return;
			}
			e.Handled = true;
		}
		private bool devConfigCheck()
		{
			string value = this.labDevModel.Tag.ToString();
			string fmwareVer = this.labDevIp.Tag.ToString();
			System.Convert.ToInt32(value);
			DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(this.labDevModel.Text, fmwareVer);
			bool flag = true;
			if (this.gbPop.Visible && this.cbPopEnable.Checked && this.rbPopUsrdef.Checked)
			{
				flag = false;
				Ecovalidate.checkTextIsNull(this.tbPopThreshold, ref flag);
				if (flag)
				{
					this.tbPopThreshold.Focus();
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.rbPopUsrdef.Text
					}));
					return false;
				}
				float num = 0f;
				try
				{
					num = System.Convert.ToSingle(this.tbPopThreshold.Text);
				}
				catch (System.Exception)
				{
					this.tbPopThreshold.Focus();
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					bool result = false;
					return result;
				}
				if (num > (float)deviceModelConfig.popUdefmax)
				{
					this.tbPopThreshold.Focus();
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.rbPopUsrdef.Text,
						"0",
						deviceModelConfig.popUdefmax.ToString()
					}));
					return false;
				}
				return true;
			}
			return true;
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.devConfigCheck())
				{
					string text = this.labDevModel.Text;
					string value = this.labDevModel.Tag.ToString();
					int l_id = System.Convert.ToInt32(value);
					DeviceInfo deviceByID = DeviceOperation.getDeviceByID(l_id);
					DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(text, deviceByID.FWVersion);
					if (this.gbPop2.Visible)
					{
						if (this.pop2_cbOutlet.Visible)
						{
							deviceByID.OutletPOPMode = (this.pop2_cbOutlet.Checked ? 2 : 1);
						}
						if (this.pop2_cbBankLIFO.Visible)
						{
							deviceByID.BankPOPLIFOMode = (this.pop2_cbBankLIFO.Checked ? 2 : 1);
						}
						deviceByID.BankPOPPriorityMode = (this.pop2_cbBankPriority.Checked ? 2 : 1);
					}
					if (this.gbPOPBankPriority.Visible)
					{
						string text2 = "";
						int num;
						int num2;
						if (deviceModelConfig.bankNum == 0)
						{
							num = 1;
							num2 = deviceModelConfig.portNum;
						}
						else
						{
							num = deviceModelConfig.bankOutlets[0].fromPort;
							num2 = deviceModelConfig.bankOutlets[0].toPort;
						}
						for (int i = 0; i < this.dgvBank1_PList.Rows.Count; i++)
						{
							DataGridViewComboBoxCell dataGridViewComboBoxCell = (DataGridViewComboBoxCell)this.dgvBank1_PList.Rows[i].Cells[1];
							string text3 = dataGridViewComboBoxCell.Value.ToString();
							if (text3.Equals("N/A"))
							{
								text2 += "0,";
							}
							else
							{
								text3 = text3.Substring("Outlet ".Length);
								text2 = text2 + ((int)System.Convert.ToInt16(text3)).ToString() + ",";
							}
						}
						for (int j = 0; j < num2 - num + 1 - this.dgvBank1_PList.Rows.Count; j++)
						{
							text2 += "0,";
						}
						text2 = text2.Substring(0, text2.Length - 1);
						if (this.dgvBank2_PList.Visible)
						{
							num = deviceModelConfig.bankOutlets[1].fromPort;
							num2 = deviceModelConfig.bankOutlets[1].toPort;
							text2 += "#";
							for (int k = 0; k < this.dgvBank2_PList.Rows.Count; k++)
							{
								DataGridViewComboBoxCell dataGridViewComboBoxCell = (DataGridViewComboBoxCell)this.dgvBank2_PList.Rows[k].Cells[1];
								string text3 = dataGridViewComboBoxCell.Value.ToString();
								if (text3.Equals("N/A"))
								{
									text2 += "0,";
								}
								else
								{
									text3 = text3.Substring("Outlet ".Length);
									text2 = text2 + ((int)System.Convert.ToInt16(text3)).ToString() + ",";
								}
							}
							for (int l = 0; l < num2 - num + 1 - this.dgvBank1_PList.Rows.Count; l++)
							{
								text2 += "0,";
							}
							text2 = text2.Substring(0, text2.Length - 1);
						}
						deviceByID.Bank_Priority = text2;
					}
					if (this.gbPop.Visible)
					{
						if (!this.cbPopEnable.Checked)
						{
							deviceByID.POPEnableMode = 1;
						}
						else
						{
							deviceByID.POPEnableMode = 2;
							if (this.rbPopMaxBankC.Checked)
							{
								deviceByID.POPThreshold = -0.1f;
							}
							else
							{
								deviceByID.POPThreshold = System.Convert.ToSingle(this.tbPopThreshold.Text);
							}
						}
					}
					string mac = deviceByID.Mac;
					DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
					DevAccessAPI devAccessAPI = new DevAccessAPI(sNMPpara);
					DevicePOPSettings devicePOPSettings = new DevicePOPSettings();
					devicePOPSettings.PopEnableMode = deviceByID.POPEnableMode;
					devicePOPSettings.PopThreshold = deviceByID.POPThreshold;
					devicePOPSettings.PopModeOutlet = deviceByID.OutletPOPMode;
					devicePOPSettings.PopModeLIFO = deviceByID.BankPOPLIFOMode;
					devicePOPSettings.PopModePriority = deviceByID.BankPOPPriorityMode;
					devicePOPSettings.PopPriorityList = deviceByID.Bank_Priority;
					if (deviceModelConfig.commonThresholdFlag != Constant.EatonPDUThreshold && !devAccessAPI.SetDevicePOPSettings(devicePOPSettings, mac))
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
					}
					else
					{
						deviceByID.Update();
						string valuePair = ValuePairs.getValuePair("Username");
						if (!string.IsNullOrEmpty(valuePair))
						{
							LogAPI.writeEventLog("0630005", new string[]
							{
								deviceByID.DeviceName,
								deviceByID.Mac,
								deviceByID.DeviceIP,
								valuePair
							});
						}
						else
						{
							LogAPI.writeEventLog("0630005", new string[]
							{
								deviceByID.DeviceName,
								deviceByID.Mac,
								deviceByID.DeviceIP
							});
						}
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.Dev_ThresholdSucc, new string[0]));
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("PropDev Exception" + ex.Message);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Dev_ThresholdFail, new string[0]));
			}
		}
		private void BankPList1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				return;
			}
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}
			DataGridViewCell dataGridViewCell = this.dgvBank1_PList.Rows[e.RowIndex].Cells[e.ColumnIndex];
			if (e.ColumnIndex == 1)
			{
				this.dgvBank1_PList.CurrentCell = dataGridViewCell;
				this.dgvBank1_PList.BeginEdit(false);
				ComboBox comboBox = (ComboBox)this.dgvBank1_PList.EditingControl;
				string value = dataGridViewCell.Value.ToString();
				comboBox.Items.Clear();
				System.Collections.Generic.List<string> availStrings = this.getAvailStrings(this.m_Bank1_Priority_ComboStrings, this.dgvBank1_PList, e.RowIndex);
				int selectedIndex = 0;
				for (int i = 0; i < availStrings.Count; i++)
				{
					string text = availStrings[i];
					comboBox.Items.Add(text);
					if (text.Equals(value))
					{
						selectedIndex = i;
					}
				}
				comboBox.SelectedIndex = selectedIndex;
				comboBox.DroppedDown = true;
			}
		}
		private void dgvBank2_PList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				return;
			}
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}
			DataGridViewCell dataGridViewCell = this.dgvBank2_PList.Rows[e.RowIndex].Cells[e.ColumnIndex];
			if (e.ColumnIndex == 1)
			{
				this.dgvBank2_PList.CurrentCell = dataGridViewCell;
				this.dgvBank2_PList.BeginEdit(false);
				ComboBox comboBox = (ComboBox)this.dgvBank2_PList.EditingControl;
				string value = dataGridViewCell.Value.ToString();
				comboBox.Items.Clear();
				System.Collections.Generic.List<string> availStrings = this.getAvailStrings(this.m_Bank2_Priority_ComboStrings, this.dgvBank2_PList, e.RowIndex);
				int selectedIndex = 0;
				for (int i = 0; i < availStrings.Count; i++)
				{
					string text = availStrings[i];
					comboBox.Items.Add(text);
					if (text.Equals(value))
					{
						selectedIndex = i;
					}
				}
				comboBox.SelectedIndex = selectedIndex;
				comboBox.DroppedDown = true;
			}
		}
		private System.Collections.Generic.List<string> getAvailStrings(System.Collections.Generic.List<string> BankPriority_ComboStrings, DataGridView dgvBank, int curRowindex)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			list.Add("N/A");
			foreach (string current in BankPriority_ComboStrings)
			{
				if (!current.Equals("N/A"))
				{
					bool flag = false;
					for (int i = 0; i < dgvBank.Rows.Count; i++)
					{
						if (i != curRowindex)
						{
							DataGridViewRow dataGridViewRow = dgvBank.Rows[i];
							DataGridViewComboBoxCell dataGridViewComboBoxCell = (DataGridViewComboBoxCell)dataGridViewRow.Cells[1];
							string text = dataGridViewComboBoxCell.Value.ToString();
							if (!text.Equals("N/A") && text.Equals(current))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						list.Add(current);
					}
				}
			}
			return list;
		}
	}
}
