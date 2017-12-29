using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Tasks
{
	public class GpPowerTask : UserControl
	{
		private IContainer components;
		private GroupBox groupBoxTask;
		private DataGridView dataGridViewCtrlTasks;
		private Button butDelete;
		private Button butModify;
		private Button butAdd;
		private DataGridViewTextBoxColumn Number;
		private DataGridViewTextBoxColumn TaskName;
		private DataGridViewTextBoxColumn TaskType;
		private DataGridViewTextBoxColumn TaskTarget;
		private DataGridViewTextBoxColumn taskID;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private Panel panel1;
		public GpPowerTask()
		{
			this.InitializeComponent();
		}
		public void pageInit()
		{
			this.dataGridViewCtrlTasks.Rows.Clear();
			System.Collections.Generic.List<GroupControlTask> allTask = GroupControlTask.GetAllTask();
			int num = 0;
			foreach (GroupControlTask current in allTask)
			{
				string msg = EcoLanguage.getMsg(LangRes.Task_TPDaily, new string[0]);
				switch (current.TaskType)
				{
				case 0:
					msg = EcoLanguage.getMsg(LangRes.Task_TPDaily, new string[0]);
					break;
				case 1:
					msg = EcoLanguage.getMsg(LangRes.Task_TPWeekly, new string[0]);
					break;
				case 2:
					msg = EcoLanguage.getMsg(LangRes.Task_TPYearly, new string[0]);
					break;
				}
				object[] values = new object[]
				{
					num + 1,
					current.TaskName,
					msg,
					current.GroupName,
					current.ID.ToString()
				};
				this.dataGridViewCtrlTasks.Rows.Add(values);
				num++;
			}
			if (this.dataGridViewCtrlTasks.Rows.Count == 0)
			{
				this.butModify.Enabled = false;
				this.butDelete.Enabled = false;
				return;
			}
			this.butModify.Enabled = true;
			this.butDelete.Enabled = true;
		}
		private void butAdd_Click(object sender, System.EventArgs e)
		{
			GpPowerTaskDlg gpPowerTaskDlg = new GpPowerTaskDlg(-1L);
			if (gpPowerTaskDlg.ShowDialog(this) == DialogResult.OK)
			{
				this.pageInit();
			}
		}
		private void dataGridViewTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			string value = this.dataGridViewCtrlTasks.Rows[rowIndex].Cells[4].Value.ToString();
			long num = (long)System.Convert.ToInt32(value);
			GpPowerTaskDlg gpPowerTaskDlg = new GpPowerTaskDlg(num);
			if (gpPowerTaskDlg.ShowDialog(this) == DialogResult.OK)
			{
				this.pageInit();
			}
		}
		private void butModify_Click(object sender, System.EventArgs e)
		{
			string value = this.dataGridViewCtrlTasks.CurrentRow.Cells[4].Value.ToString();
			long num = (long)System.Convert.ToInt32(value);
			GpPowerTaskDlg gpPowerTaskDlg = new GpPowerTaskDlg(num);
			if (gpPowerTaskDlg.ShowDialog(this) == DialogResult.OK)
			{
				this.pageInit();
			}
		}
		private void butDelete_Click(object sender, System.EventArgs e)
		{
			string value = this.dataGridViewCtrlTasks.CurrentRow.Cells[4].Value.ToString();
			long i_taskid = (long)System.Convert.ToInt32(value);
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Task_delCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			GroupControlTask taskByID = GroupControlTask.GetTaskByID(i_taskid);
			int num = GroupControlTask.DeleteTaskByID(i_taskid);
			if (num < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0530001", new string[]
				{
					taskByID.TaskName,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0530001", new string[]
				{
					taskByID.TaskName
				});
			}
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GpPowerTask));
			this.groupBoxTask = new GroupBox();
			this.panel1 = new Panel();
			this.dataGridViewCtrlTasks = new DataGridView();
			this.Number = new DataGridViewTextBoxColumn();
			this.TaskName = new DataGridViewTextBoxColumn();
			this.TaskType = new DataGridViewTextBoxColumn();
			this.TaskTarget = new DataGridViewTextBoxColumn();
			this.taskID = new DataGridViewTextBoxColumn();
			this.butDelete = new Button();
			this.butAdd = new Button();
			this.butModify = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.groupBoxTask.SuspendLayout();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.dataGridViewCtrlTasks).BeginInit();
			base.SuspendLayout();
			this.groupBoxTask.Controls.Add(this.panel1);
			componentResourceManager.ApplyResources(this.groupBoxTask, "groupBoxTask");
			this.groupBoxTask.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBoxTask.Name = "groupBoxTask";
			this.groupBoxTask.TabStop = false;
			this.panel1.Controls.Add(this.dataGridViewCtrlTasks);
			this.panel1.Controls.Add(this.butDelete);
			this.panel1.Controls.Add(this.butAdd);
			this.panel1.Controls.Add(this.butModify);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.dataGridViewCtrlTasks.AllowUserToAddRows = false;
			this.dataGridViewCtrlTasks.AllowUserToDeleteRows = false;
			this.dataGridViewCtrlTasks.AllowUserToResizeColumns = false;
			this.dataGridViewCtrlTasks.AllowUserToResizeRows = false;
			this.dataGridViewCtrlTasks.BackgroundColor = Color.WhiteSmoke;
			this.dataGridViewCtrlTasks.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = Color.Black;
			dataGridViewCellStyle.SelectionBackColor = Color.FromArgb(206, 206, 206);
			dataGridViewCellStyle.SelectionForeColor = Color.Black;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dataGridViewCtrlTasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridViewCtrlTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridViewCtrlTasks.Columns.AddRange(new DataGridViewColumn[]
			{
				this.Number,
				this.TaskName,
				this.TaskType,
				this.TaskTarget,
				this.taskID
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.White;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = Color.FromArgb(20, 73, 160);
			dataGridViewCellStyle2.SelectionBackColor = Color.White;
			dataGridViewCellStyle2.SelectionForeColor = Color.Black;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dataGridViewCtrlTasks.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridViewCtrlTasks.GridColor = Color.White;
			componentResourceManager.ApplyResources(this.dataGridViewCtrlTasks, "dataGridViewCtrlTasks");
			this.dataGridViewCtrlTasks.MultiSelect = false;
			this.dataGridViewCtrlTasks.Name = "dataGridViewCtrlTasks";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.White;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = Color.Black;
			dataGridViewCellStyle3.SelectionBackColor = Color.White;
			dataGridViewCellStyle3.SelectionForeColor = Color.Black;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dataGridViewCtrlTasks.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridViewCtrlTasks.RowHeadersVisible = false;
			this.dataGridViewCtrlTasks.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle4.BackColor = Color.White;
			dataGridViewCellStyle4.ForeColor = Color.Black;
			dataGridViewCellStyle4.SelectionBackColor = Color.Blue;
			dataGridViewCellStyle4.SelectionForeColor = Color.White;
			this.dataGridViewCtrlTasks.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridViewCtrlTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewCtrlTasks.StandardTab = true;
			this.dataGridViewCtrlTasks.TabStop = false;
			this.dataGridViewCtrlTasks.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridViewTasks_CellDoubleClick);
			componentResourceManager.ApplyResources(this.Number, "Number");
			this.Number.Name = "Number";
			this.Number.ReadOnly = true;
			componentResourceManager.ApplyResources(this.TaskName, "TaskName");
			this.TaskName.Name = "TaskName";
			this.TaskName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.TaskType, "TaskType");
			this.TaskType.Name = "TaskType";
			this.TaskType.ReadOnly = true;
			this.TaskTarget.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.TaskTarget, "TaskTarget");
			this.TaskTarget.Name = "TaskTarget";
			this.TaskTarget.ReadOnly = true;
			componentResourceManager.ApplyResources(this.taskID, "taskID");
			this.taskID.Name = "taskID";
			this.taskID.ReadOnly = true;
			this.butDelete.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butDelete, "butDelete");
			this.butDelete.ForeColor = SystemColors.ControlText;
			this.butDelete.Name = "butDelete";
			this.butDelete.UseVisualStyleBackColor = false;
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			this.butAdd.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butAdd, "butAdd");
			this.butAdd.ForeColor = SystemColors.ControlText;
			this.butAdd.Name = "butAdd";
			this.butAdd.UseVisualStyleBackColor = false;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			this.butModify.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butModify, "butModify");
			this.butModify.ForeColor = SystemColors.ControlText;
			this.butModify.Name = "butModify";
			this.butModify.UseVisualStyleBackColor = false;
			this.butModify.Click += new System.EventHandler(this.butModify_Click);
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBoxTask);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "GpPowerTask";
			this.groupBoxTask.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((ISupportInitialize)this.dataGridViewCtrlTasks).EndInit();
			base.ResumeLayout(false);
		}
	}
}
