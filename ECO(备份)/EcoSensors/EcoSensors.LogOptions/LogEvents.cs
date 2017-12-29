using DBAccessAPI;
using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.LogOptions
{
	public class LogEvents : UserControl
	{
		private bool cbselLog_changeonly;
		private bool cbselEmail_changeonly;
		private int indexFTSE;
		private IContainer components;
		private DataGridView dataGridViewEvent;
		private Button btnsave;
		private CheckBox cbselLog;
		private CheckBox cbselEmail;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
		private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn No;
		private DataGridViewCheckBoxColumn Log;
		private DataGridViewCheckBoxColumn email;
		private DataGridViewTextBoxColumn Category;
		private DataGridViewTextBoxColumn Severity;
		private DataGridViewTextBoxColumn Event;
		private DataGridViewTextBoxColumn logKey;
		public LogEvents()
		{
			this.InitializeComponent();
		}
		public void pageInit()
		{
			DataTable eventList = LogSetting.GetEventList();
			this.cbselLog_changeonly = false;
			this.cbselEmail_changeonly = false;
			this.dataGridViewEvent.Rows.Clear();
			bool flag = true;
			bool flag2 = true;
			int num = 0;
			this.indexFTSE = -1;
			foreach (DataRow dataRow in eventList.Rows)
			{
				string text = (string)dataRow["eventid"];
				if (!LogKey.isISGEvent(text) || EcoGlobalVar.gl_supportISG)
				{
					string text2 = LogKey.strSeverity(text);
					string text3 = LogKey.strCategory(text);
					string text4 = LogKey.strEvent(text);
					bool flag3 = false;
					bool flag4 = false;
					int num2 = System.Convert.ToInt32(dataRow["logflag"]);
					if (num2 == 1)
					{
						flag3 = true;
					}
					else
					{
						flag = false;
					}
					if (!text.Equals("0120062"))
					{
						num2 = System.Convert.ToInt32(dataRow["mailflag"]);
						if (num2 == 1)
						{
							flag4 = true;
						}
						else
						{
							flag2 = false;
						}
					}
					else
					{
						this.indexFTSE = num;
					}
					this.dataGridViewEvent.Rows.Add(new object[]
					{
						num + 1,
						flag3,
						flag4,
						text3,
						text2,
						text4,
						text
					});
					num++;
				}
			}
			if (this.indexFTSE >= 0)
			{
				this.dataGridViewEvent.Rows[this.indexFTSE].Cells[2].ReadOnly = true;
			}
			if (flag)
			{
				if (!this.cbselLog.Checked)
				{
					this.cbselLog_changeonly = true;
					this.cbselLog.Checked = true;
				}
			}
			else
			{
				if (this.cbselLog.Checked)
				{
					this.cbselLog_changeonly = true;
					this.cbselLog.Checked = false;
				}
			}
			if (flag2)
			{
				if (!this.cbselEmail.Checked)
				{
					this.cbselEmail_changeonly = true;
					this.cbselEmail.Checked = true;
					return;
				}
			}
			else
			{
				if (this.cbselEmail.Checked)
				{
					this.cbselEmail_changeonly = true;
					this.cbselEmail.Checked = false;
				}
			}
		}
		private void cbselLog_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbselLog_changeonly)
			{
				this.cbselLog_changeonly = false;
				return;
			}
			if (this.cbselLog.Checked)
			{
				for (int i = 0; i < this.dataGridViewEvent.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dataGridViewEvent.Rows.SharedRow(i).Cells;
					cells[1].Value = true;
				}
			}
			else
			{
				for (int j = 0; j < this.dataGridViewEvent.Rows.Count; j++)
				{
					DataGridViewCellCollection cells2 = this.dataGridViewEvent.Rows.SharedRow(j).Cells;
					cells2[1].Value = false;
				}
			}
			this.cbselLog_changeonly = false;
		}
		private void cbselEmail_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbselEmail_changeonly)
			{
				this.cbselEmail_changeonly = false;
				return;
			}
			if (this.cbselEmail.Checked)
			{
				for (int i = 0; i < this.dataGridViewEvent.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dataGridViewEvent.Rows.SharedRow(i).Cells;
					if (i == this.indexFTSE)
					{
						cells[2].Value = false;
					}
					else
					{
						cells[2].Value = true;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.dataGridViewEvent.Rows.Count; j++)
				{
					DataGridViewCellCollection cells2 = this.dataGridViewEvent.Rows.SharedRow(j).Cells;
					cells2[2].Value = false;
				}
			}
			this.cbselEmail_changeonly = false;
		}
		private void dataGridViewEvent_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dataGridViewEvent.IsCurrentCellDirty)
			{
				this.dataGridViewEvent.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void dataGridViewEvent_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			Point point = new Point(e.ColumnIndex, e.RowIndex);
			if (e.ColumnIndex == 1)
			{
				DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)this.dataGridViewEvent[point.X, point.Y];
				if ((bool)dataGridViewCheckBoxCell.Value)
				{
					for (int i = 0; i < this.dataGridViewEvent.Rows.Count; i++)
					{
						DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dataGridViewEvent.Rows.SharedRow(i).Cells[1];
						if (!(bool)dataGridViewCheckBoxCell2.Value)
						{
							return;
						}
					}
					if (!this.cbselLog.Checked)
					{
						this.cbselLog_changeonly = true;
						this.cbselLog.Checked = true;
						return;
					}
				}
				else
				{
					if (this.cbselLog.Checked)
					{
						this.cbselLog_changeonly = true;
						this.cbselLog.Checked = false;
						return;
					}
				}
			}
			else
			{
				if (e.ColumnIndex == 2)
				{
					DataGridViewCheckBoxCell dataGridViewCheckBoxCell3 = (DataGridViewCheckBoxCell)this.dataGridViewEvent[point.X, point.Y];
					if ((bool)dataGridViewCheckBoxCell3.Value)
					{
						for (int j = 0; j < this.dataGridViewEvent.Rows.Count; j++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell4 = (DataGridViewCheckBoxCell)this.dataGridViewEvent.Rows.SharedRow(j).Cells[2];
							if (!(bool)dataGridViewCheckBoxCell4.Value && j != this.indexFTSE)
							{
								return;
							}
						}
						if (!this.cbselEmail.Checked)
						{
							this.cbselEmail_changeonly = true;
							this.cbselEmail.Checked = true;
							return;
						}
					}
					else
					{
						if (this.cbselEmail.Checked)
						{
							this.cbselEmail_changeonly = true;
							this.cbselEmail.Checked = false;
						}
					}
				}
			}
		}
		private void btnsave_Click(object sender, System.EventArgs e)
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "eventid";
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "logflag";
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "mailflag";
			dataTable.Columns.Add(dataColumn);
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dataGridViewEvent.Rows)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["eventid"] = dataGridViewRow.Cells[6].Value.ToString();
				DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)dataGridViewRow.Cells[1];
				if ((bool)dataGridViewCheckBoxCell.Value)
				{
					dataRow["logflag"] = 1;
				}
				else
				{
					dataRow["logflag"] = 0;
				}
				dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)dataGridViewRow.Cells[2];
				if ((bool)dataGridViewCheckBoxCell.Value)
				{
					dataRow["mailflag"] = 1;
				}
				else
				{
					dataRow["mailflag"] = 0;
				}
				dataTable.Rows.Add(dataRow);
			}
			bool flag = LogSetting.SetEventInfo(dataTable);
			if (flag)
			{
				ClientAPI.RemoteCall(101, 1, "1", 10000);
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LogEvents));
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			this.dataGridViewEvent = new DataGridView();
			this.No = new DataGridViewTextBoxColumn();
			this.Log = new DataGridViewCheckBoxColumn();
			this.email = new DataGridViewCheckBoxColumn();
			this.Category = new DataGridViewTextBoxColumn();
			this.Severity = new DataGridViewTextBoxColumn();
			this.Event = new DataGridViewTextBoxColumn();
			this.logKey = new DataGridViewTextBoxColumn();
			this.btnsave = new Button();
			this.cbselLog = new CheckBox();
			this.cbselEmail = new CheckBox();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
			this.dataGridViewCheckBoxColumn2 = new DataGridViewCheckBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			((ISupportInitialize)this.dataGridViewEvent).BeginInit();
			base.SuspendLayout();
			this.dataGridViewEvent.AllowUserToAddRows = false;
			this.dataGridViewEvent.AllowUserToDeleteRows = false;
			this.dataGridViewEvent.AllowUserToResizeColumns = false;
			this.dataGridViewEvent.AllowUserToResizeRows = false;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dataGridViewEvent.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridViewEvent.BackgroundColor = Color.White;
			this.dataGridViewEvent.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = SystemColors.Control;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			this.dataGridViewEvent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.dataGridViewEvent, "dataGridViewEvent");
			this.dataGridViewEvent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridViewEvent.Columns.AddRange(new DataGridViewColumn[]
			{
				this.No,
				this.Log,
				this.email,
				this.Category,
				this.Severity,
				this.Event,
				this.logKey
			});
			this.dataGridViewEvent.GridColor = Color.White;
			this.dataGridViewEvent.Name = "dataGridViewEvent";
			this.dataGridViewEvent.RowHeadersVisible = false;
			this.dataGridViewEvent.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridViewEvent.StandardTab = true;
			this.dataGridViewEvent.TabStop = false;
			this.dataGridViewEvent.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridViewEvent_CellValueChanged);
			this.dataGridViewEvent.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewEvent_CurrentCellDirtyStateChanged);
			this.No.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle3.Padding = new Padding(5, 0, 5, 0);
			this.No.DefaultCellStyle = dataGridViewCellStyle3;
			this.No.Frozen = true;
			componentResourceManager.ApplyResources(this.No, "No");
			this.No.Name = "No";
			this.No.ReadOnly = true;
			this.Log.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.Log.Frozen = true;
			this.Log.Name = "Log";
			this.email.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.email.Name = "email";
			this.Category.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle4.Padding = new Padding(5, 0, 5, 0);
			this.Category.DefaultCellStyle = dataGridViewCellStyle4;
			componentResourceManager.ApplyResources(this.Category, "Category");
			this.Category.Name = "Category";
			this.Category.ReadOnly = true;
			this.Severity.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle5.Padding = new Padding(5, 0, 5, 0);
			this.Severity.DefaultCellStyle = dataGridViewCellStyle5;
			componentResourceManager.ApplyResources(this.Severity, "Severity");
			this.Severity.Name = "Severity";
			this.Severity.ReadOnly = true;
			this.Event.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle6.Padding = new Padding(5, 0, 5, 0);
			this.Event.DefaultCellStyle = dataGridViewCellStyle6;
			componentResourceManager.ApplyResources(this.Event, "Event");
			this.Event.Name = "Event";
			this.Event.ReadOnly = true;
			componentResourceManager.ApplyResources(this.logKey, "logKey");
			this.logKey.Name = "logKey";
			this.btnsave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsave, "btnsave");
			this.btnsave.ForeColor = SystemColors.ControlText;
			this.btnsave.Name = "btnsave";
			this.btnsave.UseVisualStyleBackColor = false;
			this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
			this.cbselLog.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.cbselLog, "cbselLog");
			this.cbselLog.ForeColor = Color.Black;
			this.cbselLog.Name = "cbselLog";
			this.cbselLog.UseVisualStyleBackColor = false;
			this.cbselLog.CheckedChanged += new System.EventHandler(this.cbselLog_CheckedChanged);
			this.cbselEmail.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.cbselEmail, "cbselEmail");
			this.cbselEmail.ForeColor = Color.Black;
			this.cbselEmail.Name = "cbselEmail";
			this.cbselEmail.UseVisualStyleBackColor = false;
			this.cbselEmail.CheckedChanged += new System.EventHandler(this.cbselEmail_CheckedChanged);
			this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn1.Frozen = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewCheckBoxColumn1.Frozen = true;
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			this.dataGridViewCheckBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
			this.dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.cbselEmail);
			base.Controls.Add(this.cbselLog);
			base.Controls.Add(this.btnsave);
			base.Controls.Add(this.dataGridViewEvent);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "LogEvents";
			((ISupportInitialize)this.dataGridViewEvent).EndInit();
			base.ResumeLayout(false);
		}
	}
}
