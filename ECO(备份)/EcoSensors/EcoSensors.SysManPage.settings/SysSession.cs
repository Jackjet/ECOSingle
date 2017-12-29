using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using SessionManager;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class SysSession : UserControl
	{
		private bool cbsel_changeonly;
		private IContainer components;
		private CheckBox cbsel;
		private Button btnendsession;
		private DataGridView dgvSessions;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn No;
		private new DataGridViewCheckBoxColumn Select;
		private DataGridViewTextBoxColumn usernm;
		private DataGridViewTextBoxColumn clientip;
		private DataGridViewTextBoxColumn logintime;
		private DataGridViewTextBoxColumn uid;
		public SysSession()
		{
			this.InitializeComponent();
		}
		public void pageInit()
		{
			this.cbsel_changeonly = false;
			this.cbsel.Checked = false;
			this.dgvSessions.Rows.Clear();
			int num = 0;
			DataTable dataTable = (DataTable)ClientAPI.RemoteCall(102, 1, "", 10000);
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					int num2 = (int)dataRow[SessionAPI.session_uid];
					string text = (string)dataRow[SessionAPI.session_user];
					string text2 = (string)dataRow[SessionAPI.session_ip];
					System.DateTime dateTime = (System.DateTime)dataRow[SessionAPI.session_login];
					this.dgvSessions.Rows.Add(new object[]
					{
						num + 1,
						false,
						text,
						text2,
						dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
						num2
					});
					num++;
				}
			}
			this.btnendsession.Enabled = false;
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
				for (int i = 0; i < this.dgvSessions.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dgvSessions.Rows.SharedRow(i).Cells;
					cells[1].Value = true;
				}
				this.btnendsession.Enabled = true;
				return;
			}
			for (int j = 0; j < this.dgvSessions.Rows.Count; j++)
			{
				DataGridViewCellCollection cells2 = this.dgvSessions.Rows.SharedRow(j).Cells;
				cells2[1].Value = false;
			}
			this.btnendsession.Enabled = false;
		}
		private void dgvSessions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
			{
				return;
			}
			new Point(e.ColumnIndex, e.RowIndex);
			if (e.ColumnIndex == 1)
			{
				try
				{
					DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)this.dgvSessions.Rows[e.RowIndex].Cells[1];
					if ((bool)dataGridViewCheckBoxCell.Value)
					{
						this.btnendsession.Enabled = true;
						for (int i = 0; i < this.dgvSessions.Rows.Count; i++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dgvSessions.Rows[i].Cells[1];
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
						for (int i = 0; i < this.dgvSessions.Rows.Count; i++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dgvSessions.Rows[i].Cells[1];
							if ((bool)dataGridViewCheckBoxCell2.Value)
							{
								this.btnendsession.Enabled = true;
								return;
							}
						}
						this.btnendsession.Enabled = false;
					}
				}
				catch (System.Exception)
				{
				}
			}
		}
		private void dgvSessions_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dgvSessions.IsCurrentCellDirty)
			{
				this.dgvSessions.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void btnendsession_Click(object sender, System.EventArgs e)
		{
			if (this.dgvSessions.Rows.Count == 0)
			{
				return;
			}
			string text = "";
			for (int i = 0; i < this.dgvSessions.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.dgvSessions.Rows[i];
				if (System.Convert.ToBoolean(dataGridViewRow.Cells[1].Value))
				{
					string str = System.Convert.ToString(dataGridViewRow.Cells[5].Value);
					text = text + str + ",";
				}
			}
			ClientAPI.RemoteCall(103, 1, text, 10000);
			EcoMessageBox.ShowInfo(this, EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			this.pageInit();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysSession));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
			this.cbsel = new CheckBox();
			this.btnendsession = new Button();
			this.dgvSessions = new DataGridView();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.No = new DataGridViewTextBoxColumn();
			this.Select = new DataGridViewCheckBoxColumn();
			this.usernm = new DataGridViewTextBoxColumn();
			this.clientip = new DataGridViewTextBoxColumn();
			this.logintime = new DataGridViewTextBoxColumn();
			this.uid = new DataGridViewTextBoxColumn();
			((ISupportInitialize)this.dgvSessions).BeginInit();
			base.SuspendLayout();
			this.cbsel.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.cbsel, "cbsel");
			this.cbsel.ForeColor = Color.Black;
			this.cbsel.Name = "cbsel";
			this.cbsel.UseVisualStyleBackColor = false;
			this.cbsel.CheckedChanged += new System.EventHandler(this.cbsel_CheckedChanged);
			this.btnendsession.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnendsession, "btnendsession");
			this.btnendsession.ForeColor = SystemColors.ControlText;
			this.btnendsession.Name = "btnendsession";
			this.btnendsession.UseVisualStyleBackColor = false;
			this.btnendsession.Click += new System.EventHandler(this.btnendsession_Click);
			this.dgvSessions.AllowUserToAddRows = false;
			this.dgvSessions.AllowUserToDeleteRows = false;
			this.dgvSessions.AllowUserToResizeColumns = false;
			this.dgvSessions.AllowUserToResizeRows = false;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dgvSessions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.dgvSessions.BackgroundColor = Color.White;
			this.dgvSessions.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = SystemColors.Control;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			this.dgvSessions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.dgvSessions, "dgvSessions");
			this.dgvSessions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSessions.Columns.AddRange(new DataGridViewColumn[]
			{
				this.No,
				this.Select,
				this.usernm,
				this.clientip,
				this.logintime,
				this.uid
			});
			this.dgvSessions.GridColor = Color.White;
			this.dgvSessions.Name = "dgvSessions";
			this.dgvSessions.RowHeadersVisible = false;
			this.dgvSessions.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvSessions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvSessions.StandardTab = true;
			this.dgvSessions.TabStop = false;
			this.dgvSessions.CellValueChanged += new DataGridViewCellEventHandler(this.dgvSessions_CellValueChanged);
			this.dgvSessions.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSessions_CurrentCellDirtyStateChanged);
			this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle3.Padding = new Padding(5, 0, 5, 0);
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridViewTextBoxColumn1.Frozen = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewCheckBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewCheckBoxColumn1.Frozen = true;
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle4.Padding = new Padding(5, 0, 5, 0);
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle5.Padding = new Padding(5, 0, 5, 0);
			this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle6.Padding = new Padding(5, 0, 5, 0);
			this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.No.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle7.Padding = new Padding(5, 0, 5, 0);
			this.No.DefaultCellStyle = dataGridViewCellStyle7;
			this.No.Frozen = true;
			componentResourceManager.ApplyResources(this.No, "No");
			this.No.Name = "No";
			this.No.ReadOnly = true;
			this.Select.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.Select.Frozen = true;
			componentResourceManager.ApplyResources(this.Select, "Select");
			this.Select.Name = "Select";
			this.usernm.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle8.Padding = new Padding(5, 0, 5, 0);
			this.usernm.DefaultCellStyle = dataGridViewCellStyle8;
			componentResourceManager.ApplyResources(this.usernm, "usernm");
			this.usernm.Name = "usernm";
			this.usernm.ReadOnly = true;
			this.clientip.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle9.Padding = new Padding(5, 0, 5, 0);
			this.clientip.DefaultCellStyle = dataGridViewCellStyle9;
			componentResourceManager.ApplyResources(this.clientip, "clientip");
			this.clientip.Name = "clientip";
			this.clientip.ReadOnly = true;
			this.logintime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle10.Padding = new Padding(5, 0, 5, 0);
			this.logintime.DefaultCellStyle = dataGridViewCellStyle10;
			componentResourceManager.ApplyResources(this.logintime, "logintime");
			this.logintime.Name = "logintime";
			this.logintime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.uid, "uid");
			this.uid.Name = "uid";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.cbsel);
			base.Controls.Add(this.btnendsession);
			base.Controls.Add(this.dgvSessions);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysSession";
			((ISupportInitialize)this.dgvSessions).EndInit();
			base.ResumeLayout(false);
		}
	}
}
