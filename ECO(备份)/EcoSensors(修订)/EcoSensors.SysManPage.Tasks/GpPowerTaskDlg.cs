using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.component;
using EventLogAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Tasks
{
	public class GpPowerTaskDlg : Form
	{
		private const int dgvSPDaysCol_date = 1;
		private const int dgvSPDaysCol_on = 2;
		private const int dgvSPDaysCol_ontime = 3;
		private const int dgvSPDaysCol_off = 4;
		private const int dgvSPDaysCol_offtime = 5;
		private Combobox_item m_TPDaily = new Combobox_item(0.ToString(), EcoLanguage.getMsg(LangRes.Task_TPDaily, new string[0]));
		private Combobox_item m_TPWeekly = new Combobox_item(1.ToString(), EcoLanguage.getMsg(LangRes.Task_TPWeekly, new string[0]));
		private Combobox_item m_TPYearly = new Combobox_item(2.ToString(), EcoLanguage.getMsg(LangRes.Task_TPYearly, new string[0]));
		private Combobox_item m_SPMonth_all = new Combobox_item("All", EcoLanguage.getMsg(LangRes.Task_SPMonth_All, new string[0]));
		private Combobox_item m_curGP;
		private bool cbSPonAll_changeonly;
		private bool cbSPoffAll_changeonly;
		private bool cbSPCell_changeonly;
		private long m_taskID = -1L;
		private IContainer components;
		private Button butSave;
		private TextBox textBoxTaskName;
		private ComboBox comboBoxTaskType;
		private Label label3;
		private ComboBox comboBoxTaskTarget;
		private Label label2;
		private GroupBox groupBoxTaskInfo;
		private Label label1;
		private Panel panelDaily;
		private TableLayoutPanel tblPanelDay;
		private DateTimePicker dtPickerOnD00;
		private Label label21;
		private Label label20;
		private DateTimePicker dtPickerOffD00;
		private GroupBox groupBox1;
		private Button butCancel;
		private TabControl tcYearly;
		private TabPage tpWeekbase;
		private GpPower_Week m__WeekBase;
		private TabPage tpSpecial;
		private CheckBox cbSPoffAll;
		private CheckBox cbSPonAll;
		private DataGridView dgvSPDays;
		private GpPower_Week m_Weekly;
		private Button btDelspec;
		private Button btAddSpec;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewCalendarColumn dataGridViewCalendarColumn1;
		private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
		private DataGridViewCalendarColumn dataGridViewCalendarColumn2;
		private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
		private DataGridViewCalendarColumn dataGridViewCalendarColumn3;
		private DataGridViewTextBoxColumn No;
		private DataGridViewCalendarColumn date;
		private DataGridViewCheckBoxColumn onAll;
		private DataGridViewCalendarColumn onTime;
		private DataGridViewCheckBoxColumn offAll;
		private DataGridViewCalendarColumn offTime;
		public GpPowerTaskDlg()
		{
			this.InitializeComponent();
			this.textBoxTaskName.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public GpPowerTaskDlg(long taskID)
		{
			this.InitializeComponent();
			this.cbSPonAll_changeonly = false;
			this.cbSPoffAll_changeonly = false;
			this.cbSPCell_changeonly = false;
			this.m_taskID = taskID;
			this.comboBoxTaskType.Items.Clear();
			this.comboBoxTaskType.Items.Add(this.m_TPDaily);
			this.comboBoxTaskType.Items.Add(this.m_TPWeekly);
			this.comboBoxTaskType.Items.Add(this.m_TPYearly);
			this.comboBoxTaskType.SelectedItem = this.m_TPDaily;
			long num = -1L;
			if (taskID > 0L)
			{
				GroupControlTask taskByID = GroupControlTask.GetTaskByID(taskID);
				this.textBoxTaskName.Text = taskByID.TaskName;
				num = taskByID.GroupID;
				string[,] taskSchedule = taskByID.TaskSchedule;
				switch (taskByID.TaskType)
				{
				case 0:
					this.comboBoxTaskType.SelectedItem = this.m_TPDaily;
					this.dtPickerOnD00.Text = taskSchedule[0, 0];
					this.dtPickerOffD00.Text = taskSchedule[0, 1];
					break;
				case 1:
					this.comboBoxTaskType.SelectedItem = this.m_TPWeekly;
					this.m_Weekly.Init(taskSchedule);
					break;
				case 2:
				{
					this.comboBoxTaskType.SelectedItem = this.m_TPYearly;
					this.m__WeekBase.Init(taskSchedule);
					System.Collections.Generic.List<SpecialDay> specialDates = taskByID.SpecialDates;
					this.initSpecial(specialDates);
					break;
				}
				}
			}
			else
			{
				this.initSpecial(null);
			}
			System.Collections.Generic.Dictionary<long, string> unusedGroup = GroupControlTask.GetUnusedGroup(taskID);
			this.comboBoxTaskTarget.Items.Clear();
			foreach (long current in unusedGroup.Keys)
			{
				Combobox_item combobox_item = new Combobox_item(current.ToString(), unusedGroup[current]);
				this.comboBoxTaskTarget.Items.Add(combobox_item);
				if (current == num)
				{
					this.m_curGP = combobox_item;
				}
			}
			if (this.m_curGP != null)
			{
				this.comboBoxTaskTarget.SelectedItem = this.m_curGP;
				return;
			}
			if (this.comboBoxTaskTarget.Items.Count > 0)
			{
				this.comboBoxTaskTarget.SelectedIndex = 0;
			}
		}
		private void initSpecial(System.Collections.Generic.List<SpecialDay> list_sd)
		{
			this.dgvSPDays.Rows.Clear();
			if (list_sd == null)
			{
				return;
			}
			int count = list_sd.Count;
			if (count == 0)
			{
				return;
			}
			this.dgvSPDays.RowCount = count;
			int num = 0;
			this.cbSPCell_changeonly = true;
			foreach (SpecialDay current in list_sd)
			{
				DataGridViewRow dataGridViewRow = this.dgvSPDays.Rows[num];
				dataGridViewRow.Cells[0].Value = num + 1;
				num++;
				dataGridViewRow.Cells[1].Value = System.DateTime.ParseExact(current.SpecialDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
				dataGridViewRow.Cells[1].Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " ddd";
				if (current.ONTime.Length == 0)
				{
					dataGridViewRow.Cells[2].Value = false;
					dataGridViewRow.Cells[3].Value = System.DateTime.Now;
				}
				else
				{
					dataGridViewRow.Cells[2].Value = true;
					dataGridViewRow.Cells[3].Value = System.DateTime.ParseExact(current.ONTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
				}
				if (current.OFFTime.Length == 0)
				{
					dataGridViewRow.Cells[4].Value = false;
					dataGridViewRow.Cells[5].Value = System.DateTime.Now;
				}
				else
				{
					dataGridViewRow.Cells[4].Value = true;
					dataGridViewRow.Cells[5].Value = System.DateTime.ParseExact(current.OFFTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
				}
			}
			this.cbSPCell_changeonly = false;
			bool flag = true;
			bool flag2 = true;
			foreach (DataGridViewRow dataGridViewRow2 in (System.Collections.IEnumerable)this.dgvSPDays.Rows)
			{
				if ((bool)dataGridViewRow2.Cells[2].Value)
				{
					dataGridViewRow2.Cells[3].ReadOnly = false;
				}
				else
				{
					flag = false;
					dataGridViewRow2.Cells[3].ReadOnly = true;
				}
				if ((bool)dataGridViewRow2.Cells[4].Value)
				{
					dataGridViewRow2.Cells[5].ReadOnly = false;
				}
				else
				{
					flag2 = false;
					dataGridViewRow2.Cells[5].ReadOnly = true;
				}
			}
			if (flag)
			{
				this.cbSPonAll_changeonly = true;
				this.cbSPonAll.Checked = true;
			}
			if (flag2)
			{
				this.cbSPoffAll_changeonly = true;
				this.cbSPoffAll.Checked = true;
			}
		}
		private void comboBoxTaskType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Combobox_item combobox_item = (Combobox_item)this.comboBoxTaskType.SelectedItem;
			if (combobox_item == this.m_TPDaily)
			{
				this.panelDaily.Visible = true;
				this.tcYearly.Visible = false;
				this.tblPanelDay.Visible = true;
				this.m_Weekly.Visible = false;
				return;
			}
			if (combobox_item == this.m_TPWeekly)
			{
				this.panelDaily.Visible = true;
				this.tcYearly.Visible = false;
				this.tblPanelDay.Visible = false;
				this.m_Weekly.Visible = true;
				return;
			}
			if (combobox_item == this.m_TPYearly)
			{
				this.panelDaily.Visible = false;
				this.tcYearly.Visible = true;
			}
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			if (!this.checkValue())
			{
				return;
			}
			string text = this.textBoxTaskName.Text;
			System.Collections.Generic.List<SpecialDay> list = new System.Collections.Generic.List<SpecialDay>();
			object selectedItem = this.comboBoxTaskType.SelectedItem;
			int num;
			string[,] array;
			if (selectedItem.Equals(this.m_TPDaily))
			{
				num = 0;
				array = new string[1, 2];
				string text2 = this.dtPickerOnD00.Text;
				string text3 = this.dtPickerOffD00.Text;
				array[0, 0] = text2 + ":00";
				array[0, 1] = text3 + ":00";
			}
			else
			{
				if (selectedItem.Equals(this.m_TPWeekly))
				{
					num = 1;
					array = new string[7, 2];
					this.m_Weekly.getSchedule(array);
				}
				else
				{
					num = 2;
					array = new string[7, 2];
					this.m__WeekBase.getSchedule(array);
					foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSPDays.Rows)
					{
						string st_day = ((System.DateTime)dataGridViewRow.Cells[1].Value).ToString("yyyy-MM-dd");
						string text2 = "";
						string text3 = "";
						if ((bool)dataGridViewRow.Cells[2].Value)
						{
							text2 = ((System.DateTime)dataGridViewRow.Cells[3].Value).ToString("HH:mm") + ":00";
						}
						if ((bool)dataGridViewRow.Cells[4].Value)
						{
							text3 = ((System.DateTime)dataGridViewRow.Cells[5].Value).ToString("HH:mm") + ":00";
						}
						SpecialDay item = new SpecialDay(st_day, text2, text3);
						list.Add(item);
					}
				}
			}
			Combobox_item combobox_item = (Combobox_item)this.comboBoxTaskTarget.SelectedItem;
			long num2 = (long)System.Convert.ToInt32(combobox_item.getKey());
			int num3;
			if (this.m_taskID < 0L)
			{
				if (selectedItem.Equals(this.m_TPYearly))
				{
					num3 = GroupControlTask.CreateTask(text, num2, num, array, list);
				}
				else
				{
					num3 = GroupControlTask.CreateTask(text, num2, num, array);
				}
				if (num3 > 0)
				{
					string valuePair = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair))
					{
						LogAPI.writeEventLog("0530000", new string[]
						{
							text,
							valuePair
						});
					}
					else
					{
						LogAPI.writeEventLog("0530000", new string[]
						{
							text
						});
					}
				}
			}
			else
			{
				GroupControlTask taskByID = GroupControlTask.GetTaskByID(this.m_taskID);
				taskByID.TaskName = text;
				taskByID.TaskType = num;
				taskByID.GroupID = num2;
				taskByID.TaskSchedule = array;
				if (selectedItem.Equals(this.m_TPYearly))
				{
					taskByID.SpecialDates = list;
				}
				num3 = taskByID.UpdateTask();
				if (num3 > 0)
				{
					string valuePair2 = ValuePairs.getValuePair("Username");
					if (!string.IsNullOrEmpty(valuePair2))
					{
						LogAPI.writeEventLog("0530002", new string[]
						{
							text,
							valuePair2
						});
					}
					else
					{
						LogAPI.writeEventLog("0530002", new string[]
						{
							text
						});
					}
				}
			}
			if (num3 < 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			base.DialogResult = DialogResult.OK;
			base.Close();
		}
		private bool checkValue()
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.textBoxTaskName, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.label1.Text
				}));
				return false;
			}
			string text = this.textBoxTaskName.Text;
			if (!GroupControlTask.CheckName(this.m_taskID, text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_nmdup, new string[]
				{
					text
				}));
				this.textBoxTaskName.Focus();
				return false;
			}
			if (this.comboBoxTaskTarget.SelectedItem == null)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.label3.Text
				}));
				this.comboBoxTaskTarget.Focus();
				return false;
			}
			object selectedItem = this.comboBoxTaskType.SelectedItem;
			if (selectedItem.Equals(this.m_TPDaily))
			{
				string text2 = this.dtPickerOnD00.Text;
				string text3 = this.dtPickerOffD00.Text;
				if (text2.Equals(text3))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timedup, new string[0]));
					this.dtPickerOnD00.Focus();
					return false;
				}
			}
			else
			{
				if (selectedItem.Equals(this.m_TPWeekly))
				{
					if (this.m_Weekly.checkBoxOnW01.Checked && this.m_Weekly.checkBoxOffW01.Checked && !this.checkonoffpair(this.m_Weekly.dtPickerOnW01, this.m_Weekly.dtPickerOffW01, 0))
					{
						return false;
					}
					if (this.m_Weekly.checkBoxOnW02.Checked && this.m_Weekly.checkBoxOffW02.Checked && !this.checkonoffpair(this.m_Weekly.dtPickerOnW02, this.m_Weekly.dtPickerOffW02, 0))
					{
						return false;
					}
					if (this.m_Weekly.checkBoxOnW03.Checked && this.m_Weekly.checkBoxOffW03.Checked && !this.checkonoffpair(this.m_Weekly.dtPickerOnW03, this.m_Weekly.dtPickerOffW03, 0))
					{
						return false;
					}
					if (this.m_Weekly.checkBoxOnW04.Checked && this.m_Weekly.checkBoxOffW04.Checked && !this.checkonoffpair(this.m_Weekly.dtPickerOnW04, this.m_Weekly.dtPickerOffW04, 0))
					{
						return false;
					}
					if (this.m_Weekly.checkBoxOnW05.Checked && this.m_Weekly.checkBoxOffW05.Checked && !this.checkonoffpair(this.m_Weekly.dtPickerOnW05, this.m_Weekly.dtPickerOffW05, 0))
					{
						return false;
					}
					if (this.m_Weekly.checkBoxOnW06.Checked && this.m_Weekly.checkBoxOffW06.Checked && !this.checkonoffpair(this.m_Weekly.dtPickerOnW06, this.m_Weekly.dtPickerOffW06, 0))
					{
						return false;
					}
					if (this.m_Weekly.checkBoxOnW07.Checked && this.m_Weekly.checkBoxOffW07.Checked && !this.checkonoffpair(this.m_Weekly.dtPickerOnW07, this.m_Weekly.dtPickerOffW07, 0))
					{
						return false;
					}
					if (!this.m_Weekly.checkBoxOnW01.Checked && !this.m_Weekly.checkBoxOnW02.Checked && !this.m_Weekly.checkBoxOnW03.Checked && !this.m_Weekly.checkBoxOnW04.Checked && !this.m_Weekly.checkBoxOnW05.Checked && !this.m_Weekly.checkBoxOnW06.Checked && !this.m_Weekly.checkBoxOnW07.Checked)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timeWonneed, new string[0]));
						return false;
					}
					if (!this.m_Weekly.checkBoxOffW01.Checked && !this.m_Weekly.checkBoxOffW02.Checked && !this.m_Weekly.checkBoxOffW03.Checked && !this.m_Weekly.checkBoxOffW04.Checked && !this.m_Weekly.checkBoxOffW05.Checked && !this.m_Weekly.checkBoxOffW06.Checked && !this.m_Weekly.checkBoxOffW07.Checked)
					{
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timeWoffneed, new string[0]));
						return false;
					}
				}
				else
				{
					if (this.tcYearly.SelectedTab.Equals(this.tpWeekbase))
					{
						if (!this.checkweekBase(1))
						{
							return false;
						}
						if (!this.checkdayspecial(1))
						{
							return false;
						}
					}
					else
					{
						if (!this.checkdayspecial(2))
						{
							return false;
						}
						if (!this.checkweekBase(2))
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		private bool checkweekBase(int curpage)
		{
			if (this.m__WeekBase.checkBoxOnW01.Checked && this.m__WeekBase.checkBoxOffW01.Checked && !this.checkonoffpair(this.m__WeekBase.dtPickerOnW01, this.m__WeekBase.dtPickerOffW01, curpage))
			{
				return false;
			}
			if (this.m__WeekBase.checkBoxOnW02.Checked && this.m__WeekBase.checkBoxOffW02.Checked && !this.checkonoffpair(this.m__WeekBase.dtPickerOnW02, this.m__WeekBase.dtPickerOffW02, curpage))
			{
				return false;
			}
			if (this.m__WeekBase.checkBoxOnW03.Checked && this.m__WeekBase.checkBoxOffW03.Checked && !this.checkonoffpair(this.m__WeekBase.dtPickerOnW03, this.m__WeekBase.dtPickerOffW03, curpage))
			{
				return false;
			}
			if (this.m__WeekBase.checkBoxOnW04.Checked && this.m__WeekBase.checkBoxOffW04.Checked && !this.checkonoffpair(this.m__WeekBase.dtPickerOnW04, this.m__WeekBase.dtPickerOffW04, curpage))
			{
				return false;
			}
			if (this.m__WeekBase.checkBoxOnW05.Checked && this.m__WeekBase.checkBoxOffW05.Checked && !this.checkonoffpair(this.m__WeekBase.dtPickerOnW05, this.m__WeekBase.dtPickerOffW05, curpage))
			{
				return false;
			}
			if (this.m__WeekBase.checkBoxOnW06.Checked && this.m__WeekBase.checkBoxOffW06.Checked && !this.checkonoffpair(this.m__WeekBase.dtPickerOnW06, this.m__WeekBase.dtPickerOffW06, curpage))
			{
				return false;
			}
			if (this.m__WeekBase.checkBoxOnW07.Checked && this.m__WeekBase.checkBoxOffW07.Checked && !this.checkonoffpair(this.m__WeekBase.dtPickerOnW07, this.m__WeekBase.dtPickerOffW07, curpage))
			{
				return false;
			}
			if (!this.m__WeekBase.checkBoxOnW01.Checked && !this.m__WeekBase.checkBoxOnW02.Checked && !this.m__WeekBase.checkBoxOnW03.Checked && !this.m__WeekBase.checkBoxOnW04.Checked && !this.m__WeekBase.checkBoxOnW05.Checked && !this.m__WeekBase.checkBoxOnW06.Checked && !this.m__WeekBase.checkBoxOnW07.Checked)
			{
				if (curpage == 2)
				{
					this.tcYearly.SelectedTab = this.tpWeekbase;
				}
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timeWonneed, new string[0]));
				return false;
			}
			if (!this.m__WeekBase.checkBoxOffW01.Checked && !this.m__WeekBase.checkBoxOffW02.Checked && !this.m__WeekBase.checkBoxOffW03.Checked && !this.m__WeekBase.checkBoxOffW04.Checked && !this.m__WeekBase.checkBoxOffW05.Checked && !this.m__WeekBase.checkBoxOffW06.Checked && !this.m__WeekBase.checkBoxOffW07.Checked)
			{
				if (curpage == 2)
				{
					this.tcYearly.SelectedTab = this.tpWeekbase;
				}
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timeWoffneed, new string[0]));
				return false;
			}
			return true;
		}
		private bool checkdayspecial(int curpage)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSPDays.Rows)
			{
				string text = ((System.DateTime)dataGridViewRow.Cells[1].Value).ToString("yyyy-MM-dd");
				string text2 = null;
				try
				{
					text2 = dictionary[text];
				}
				catch (System.Collections.Generic.KeyNotFoundException)
				{
					dictionary.Add(text, text);
				}
				if (text2 != null)
				{
					if (curpage == 1)
					{
						this.tcYearly.SelectedTab = this.tpSpecial;
					}
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_dayExisted, new string[]
					{
						text2
					}));
					bool result = false;
					return result;
				}
				if ((bool)dataGridViewRow.Cells[2].Value && (bool)dataGridViewRow.Cells[4].Value)
				{
					string text3 = ((System.DateTime)dataGridViewRow.Cells[3].Value).ToString("HH:mm");
					string value = ((System.DateTime)dataGridViewRow.Cells[5].Value).ToString("HH:mm");
					if (text3.Equals(value))
					{
						if (curpage == 1)
						{
							this.tcYearly.SelectedTab = this.tpSpecial;
						}
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timedup, new string[0]));
						bool result = false;
						return result;
					}
				}
			}
			return true;
		}
		private bool checkonoffpair(DateTimePicker onPicker, DateTimePicker offPicker, int curpage)
		{
			string text = onPicker.Text;
			string text2 = offPicker.Text;
			if (text.Equals(text2))
			{
				if (curpage == 2)
				{
					this.tcYearly.SelectedTab = this.tpWeekbase;
				}
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_timedup, new string[0]));
				onPicker.Focus();
				return false;
			}
			return true;
		}
		private void textBoxTaskName_KeyPress(object sender, KeyPressEventArgs e)
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
		private void dgvDays_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
		{
			if (this.dgvSPDays.IsCurrentCellDirty)
			{
				this.dgvSPDays.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void dgvDays_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				return;
			}
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}
			DataGridViewCell currentCell = this.dgvSPDays.Rows[e.RowIndex].Cells[e.ColumnIndex];
			if (e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 5)
			{
				this.dgvSPDays.CurrentCell = currentCell;
				this.dgvSPDays.BeginEdit(false);
			}
		}
		private string getuniqueDay()
		{
			string text = System.DateTime.Now.ToString("yyyy-MM-dd");
			System.DateTime dateTime = System.DateTime.Now;
			int num = 0;
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSPDays.Rows)
			{
				System.DateTime dateTime2 = (System.DateTime)dataGridViewRow.Cells[1].Value;
				if (num == 0)
				{
					dateTime = dateTime2;
					num = 1;
				}
				else
				{
					if (dateTime.CompareTo(dateTime2) < 0)
					{
						dateTime = dateTime2;
					}
				}
				string text2 = dateTime2.ToString("yyyy-MM-dd");
				if (text2.Equals(text))
				{
					num = 2;
				}
			}
			if (num == 0 || num == 1)
			{
				return text;
			}
			text = dateTime.AddDays(1.0).ToString("yyyy-MM-dd");
			return text;
		}
		private void btAddSpec_Click(object sender, System.EventArgs e)
		{
			int rowCount = this.dgvSPDays.RowCount;
			string s = this.getuniqueDay();
			this.dgvSPDays.RowCount = rowCount + 1;
			DataGridViewRow dataGridViewRow = this.dgvSPDays.Rows[rowCount];
			dataGridViewRow.Cells[0].Value = rowCount + 1;
			dataGridViewRow.Cells[1].Value = System.DateTime.ParseExact(s, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
			dataGridViewRow.Cells[1].Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " ddd";
			dataGridViewRow.Cells[2].Value = false;
			dataGridViewRow.Cells[3].Value = System.DateTime.Now;
			dataGridViewRow.Cells[4].Value = false;
			dataGridViewRow.Cells[5].Value = System.DateTime.Now;
			this.dgvSPDays.CurrentCell = dataGridViewRow.Cells[0];
		}
		private void btDelspec_Click(object sender, System.EventArgs e)
		{
			if (this.dgvSPDays.CurrentCell != null)
			{
				this.dgvSPDays.Rows.RemoveAt(this.dgvSPDays.CurrentCell.RowIndex);
			}
			int num = 1;
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSPDays.Rows)
			{
				dataGridViewRow.Cells[0].Value = num;
				num++;
			}
		}
		private void cbonAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbSPonAll_changeonly)
			{
				this.cbSPonAll_changeonly = false;
				return;
			}
			if (this.cbSPonAll.Checked)
			{
				for (int i = 0; i < this.dgvSPDays.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dgvSPDays.Rows.SharedRow(i).Cells;
					cells[2].Value = true;
					cells[3].ReadOnly = false;
				}
			}
			else
			{
				for (int j = 0; j < this.dgvSPDays.Rows.Count; j++)
				{
					DataGridViewCellCollection cells2 = this.dgvSPDays.Rows.SharedRow(j).Cells;
					cells2[2].Value = false;
					cells2[3].ReadOnly = true;
				}
			}
			this.cbSPonAll_changeonly = false;
		}
		private void cboffAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbSPoffAll_changeonly)
			{
				this.cbSPoffAll_changeonly = false;
				return;
			}
			if (this.cbSPoffAll.Checked)
			{
				for (int i = 0; i < this.dgvSPDays.Rows.Count; i++)
				{
					DataGridViewCellCollection cells = this.dgvSPDays.Rows.SharedRow(i).Cells;
					cells[4].Value = true;
					cells[5].ReadOnly = false;
				}
			}
			else
			{
				for (int j = 0; j < this.dgvSPDays.Rows.Count; j++)
				{
					DataGridViewCellCollection cells2 = this.dgvSPDays.Rows.SharedRow(j).Cells;
					cells2[4].Value = false;
					cells2[5].ReadOnly = true;
				}
			}
			this.cbSPoffAll_changeonly = false;
		}
		private void dgvSPDays_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (this.cbSPCell_changeonly)
			{
				return;
			}
			Point point = new Point(e.ColumnIndex, e.RowIndex);
			if (point.X < 0 || point.Y < 0)
			{
				return;
			}
			if (e.ColumnIndex == 2)
			{
				DataGridViewCheckBoxCell dataGridViewCheckBoxCell = (DataGridViewCheckBoxCell)this.dgvSPDays[point.X, point.Y];
				DataGridViewRow dataGridViewRow = this.dgvSPDays.Rows[dataGridViewCheckBoxCell.RowIndex];
				if ((bool)dataGridViewCheckBoxCell.Value)
				{
					dataGridViewRow.Cells[3].ReadOnly = false;
					for (int i = 0; i < this.dgvSPDays.Rows.Count; i++)
					{
						DataGridViewCheckBoxCell dataGridViewCheckBoxCell2 = (DataGridViewCheckBoxCell)this.dgvSPDays.Rows.SharedRow(i).Cells[2];
						if (!(bool)dataGridViewCheckBoxCell2.Value)
						{
							return;
						}
					}
					if (!this.cbSPonAll.Checked)
					{
						this.cbSPonAll_changeonly = true;
						this.cbSPonAll.Checked = true;
						return;
					}
				}
				else
				{
					dataGridViewRow.Cells[3].ReadOnly = true;
					if (this.cbSPonAll.Checked)
					{
						this.cbSPonAll_changeonly = true;
						this.cbSPonAll.Checked = false;
						return;
					}
				}
			}
			else
			{
				if (e.ColumnIndex == 4)
				{
					DataGridViewCheckBoxCell dataGridViewCheckBoxCell3 = (DataGridViewCheckBoxCell)this.dgvSPDays[point.X, point.Y];
					DataGridViewRow dataGridViewRow2 = this.dgvSPDays.Rows[dataGridViewCheckBoxCell3.RowIndex];
					if ((bool)dataGridViewCheckBoxCell3.Value)
					{
						dataGridViewRow2.Cells[5].ReadOnly = false;
						for (int j = 0; j < this.dgvSPDays.Rows.Count; j++)
						{
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell4 = (DataGridViewCheckBoxCell)this.dgvSPDays.Rows.SharedRow(j).Cells[4];
							if (!(bool)dataGridViewCheckBoxCell4.Value)
							{
								return;
							}
						}
						if (!this.cbSPoffAll.Checked)
						{
							this.cbSPoffAll_changeonly = true;
							this.cbSPoffAll.Checked = true;
							return;
						}
					}
					else
					{
						dataGridViewRow2.Cells[5].ReadOnly = true;
						if (this.cbSPoffAll.Checked)
						{
							this.cbSPoffAll_changeonly = true;
							this.cbSPoffAll.Checked = false;
							return;
						}
					}
				}
				else
				{
					if (e.ColumnIndex == 1)
					{
						DataGridViewCalendarCell dataGridViewCalendarCell = (DataGridViewCalendarCell)this.dgvSPDays[point.X, point.Y];
						DataGridViewRow dataGridViewRow3 = this.dgvSPDays.Rows[dataGridViewCalendarCell.RowIndex];
						string value = ((System.DateTime)dataGridViewRow3.Cells[1].Value).ToString("yyyy-MM-dd");
						foreach (DataGridViewRow dataGridViewRow4 in (System.Collections.IEnumerable)this.dgvSPDays.Rows)
						{
							if (dataGridViewRow4.Index != dataGridViewCalendarCell.RowIndex)
							{
								string text = ((System.DateTime)dataGridViewRow4.Cells[1].Value).ToString("yyyy-MM-dd");
								if (text.Equals(value))
								{
									this.dgvSPDays.BeginEdit(true);
									EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Task_dayExisted, new string[]
									{
										text
									}));
									break;
								}
							}
						}
					}
				}
			}
		}
		private void cbSP_Paint(object sender, PaintEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox.Focused)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GpPowerTaskDlg));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.butSave = new Button();
			this.textBoxTaskName = new TextBox();
			this.comboBoxTaskType = new ComboBox();
			this.label3 = new Label();
			this.comboBoxTaskTarget = new ComboBox();
			this.label2 = new Label();
			this.groupBoxTaskInfo = new GroupBox();
			this.label1 = new Label();
			this.panelDaily = new Panel();
			this.tblPanelDay = new TableLayoutPanel();
			this.dtPickerOnD00 = new DateTimePicker();
			this.label21 = new Label();
			this.label20 = new Label();
			this.dtPickerOffD00 = new DateTimePicker();
			this.m_Weekly = new GpPower_Week();
			this.groupBox1 = new GroupBox();
			this.tcYearly = new TabControl();
			this.tpWeekbase = new TabPage();
			this.m__WeekBase = new GpPower_Week();
			this.tpSpecial = new TabPage();
			this.btDelspec = new Button();
			this.btAddSpec = new Button();
			this.cbSPoffAll = new CheckBox();
			this.cbSPonAll = new CheckBox();
			this.dgvSPDays = new DataGridView();
			this.No = new DataGridViewTextBoxColumn();
			this.date = new DataGridViewCalendarColumn();
			this.onAll = new DataGridViewCheckBoxColumn();
			this.onTime = new DataGridViewCalendarColumn();
			this.offAll = new DataGridViewCheckBoxColumn();
			this.offTime = new DataGridViewCalendarColumn();
			this.butCancel = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewCalendarColumn1 = new DataGridViewCalendarColumn();
			this.dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
			this.dataGridViewCalendarColumn2 = new DataGridViewCalendarColumn();
			this.dataGridViewCheckBoxColumn2 = new DataGridViewCheckBoxColumn();
			this.dataGridViewCalendarColumn3 = new DataGridViewCalendarColumn();
			this.groupBoxTaskInfo.SuspendLayout();
			this.panelDaily.SuspendLayout();
			this.tblPanelDay.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tcYearly.SuspendLayout();
			this.tpWeekbase.SuspendLayout();
			this.tpSpecial.SuspendLayout();
			((ISupportInitialize)this.dgvSPDays).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.butSave, "butSave");
			this.butSave.Name = "butSave";
			this.butSave.UseVisualStyleBackColor = true;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			componentResourceManager.ApplyResources(this.textBoxTaskName, "textBoxTaskName");
			this.textBoxTaskName.Name = "textBoxTaskName";
			this.textBoxTaskName.KeyPress += new KeyPressEventHandler(this.textBoxTaskName_KeyPress);
			this.comboBoxTaskType.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.comboBoxTaskType, "comboBoxTaskType");
			this.comboBoxTaskType.FormattingEnabled = true;
			this.comboBoxTaskType.Name = "comboBoxTaskType";
			this.comboBoxTaskType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTaskType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = SystemColors.ControlText;
			this.label3.Name = "label3";
			this.comboBoxTaskTarget.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.comboBoxTaskTarget, "comboBoxTaskTarget");
			this.comboBoxTaskTarget.FormattingEnabled = true;
			this.comboBoxTaskTarget.Name = "comboBoxTaskTarget";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = SystemColors.ControlText;
			this.label2.Name = "label2";
			this.groupBoxTaskInfo.Controls.Add(this.textBoxTaskName);
			this.groupBoxTaskInfo.Controls.Add(this.comboBoxTaskType);
			this.groupBoxTaskInfo.Controls.Add(this.comboBoxTaskTarget);
			this.groupBoxTaskInfo.Controls.Add(this.label3);
			this.groupBoxTaskInfo.Controls.Add(this.label2);
			this.groupBoxTaskInfo.Controls.Add(this.label1);
			componentResourceManager.ApplyResources(this.groupBoxTaskInfo, "groupBoxTaskInfo");
			this.groupBoxTaskInfo.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBoxTaskInfo.Name = "groupBoxTaskInfo";
			this.groupBoxTaskInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = SystemColors.ControlText;
			this.label1.Name = "label1";
			this.panelDaily.Controls.Add(this.tblPanelDay);
			this.panelDaily.Controls.Add(this.m_Weekly);
			componentResourceManager.ApplyResources(this.panelDaily, "panelDaily");
			this.panelDaily.ForeColor = SystemColors.ControlText;
			this.panelDaily.Name = "panelDaily";
			this.tblPanelDay.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.tblPanelDay, "tblPanelDay");
			this.tblPanelDay.Controls.Add(this.dtPickerOnD00, 0, 1);
			this.tblPanelDay.Controls.Add(this.label21, 0, 0);
			this.tblPanelDay.Controls.Add(this.label20, 1, 0);
			this.tblPanelDay.Controls.Add(this.dtPickerOffD00, 1, 1);
			this.tblPanelDay.Name = "tblPanelDay";
			componentResourceManager.ApplyResources(this.dtPickerOnD00, "dtPickerOnD00");
			this.dtPickerOnD00.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnD00.Name = "dtPickerOnD00";
			this.dtPickerOnD00.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.label21, "label21");
			this.label21.Name = "label21";
			componentResourceManager.ApplyResources(this.label20, "label20");
			this.label20.Name = "label20";
			componentResourceManager.ApplyResources(this.dtPickerOffD00, "dtPickerOffD00");
			this.dtPickerOffD00.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffD00.Name = "dtPickerOffD00";
			this.dtPickerOffD00.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.m_Weekly, "m_Weekly");
			this.m_Weekly.Name = "m_Weekly";
			this.groupBox1.Controls.Add(this.tcYearly);
			this.groupBox1.Controls.Add(this.panelDaily);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.tcYearly.Controls.Add(this.tpWeekbase);
			this.tcYearly.Controls.Add(this.tpSpecial);
			componentResourceManager.ApplyResources(this.tcYearly, "tcYearly");
			this.tcYearly.Name = "tcYearly";
			this.tcYearly.SelectedIndex = 0;
			this.tpWeekbase.BackColor = Color.WhiteSmoke;
			this.tpWeekbase.Controls.Add(this.m__WeekBase);
			this.tpWeekbase.ForeColor = Color.Black;
			componentResourceManager.ApplyResources(this.tpWeekbase, "tpWeekbase");
			this.tpWeekbase.Name = "tpWeekbase";
			componentResourceManager.ApplyResources(this.m__WeekBase, "m__WeekBase");
			this.m__WeekBase.Name = "m__WeekBase";
			this.tpSpecial.BackColor = Color.WhiteSmoke;
			this.tpSpecial.Controls.Add(this.btDelspec);
			this.tpSpecial.Controls.Add(this.btAddSpec);
			this.tpSpecial.Controls.Add(this.cbSPoffAll);
			this.tpSpecial.Controls.Add(this.cbSPonAll);
			this.tpSpecial.Controls.Add(this.dgvSPDays);
			componentResourceManager.ApplyResources(this.tpSpecial, "tpSpecial");
			this.tpSpecial.Name = "tpSpecial";
			componentResourceManager.ApplyResources(this.btDelspec, "btDelspec");
			this.btDelspec.ForeColor = Color.Black;
			this.btDelspec.Name = "btDelspec";
			this.btDelspec.UseVisualStyleBackColor = true;
			this.btDelspec.Click += new System.EventHandler(this.btDelspec_Click);
			componentResourceManager.ApplyResources(this.btAddSpec, "btAddSpec");
			this.btAddSpec.ForeColor = Color.Black;
			this.btAddSpec.Name = "btAddSpec";
			this.btAddSpec.UseVisualStyleBackColor = true;
			this.btAddSpec.Click += new System.EventHandler(this.btAddSpec_Click);
			this.cbSPoffAll.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.cbSPoffAll, "cbSPoffAll");
			this.cbSPoffAll.ForeColor = Color.Black;
			this.cbSPoffAll.Name = "cbSPoffAll";
			this.cbSPoffAll.UseVisualStyleBackColor = false;
			this.cbSPoffAll.CheckedChanged += new System.EventHandler(this.cboffAll_CheckedChanged);
			this.cbSPoffAll.Paint += new PaintEventHandler(this.cbSP_Paint);
			this.cbSPonAll.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.cbSPonAll, "cbSPonAll");
			this.cbSPonAll.ForeColor = Color.Black;
			this.cbSPonAll.Name = "cbSPonAll";
			this.cbSPonAll.UseVisualStyleBackColor = false;
			this.cbSPonAll.CheckedChanged += new System.EventHandler(this.cbonAll_CheckedChanged);
			this.cbSPonAll.Paint += new PaintEventHandler(this.cbSP_Paint);
			this.dgvSPDays.AllowUserToAddRows = false;
			this.dgvSPDays.AllowUserToDeleteRows = false;
			this.dgvSPDays.AllowUserToResizeColumns = false;
			this.dgvSPDays.AllowUserToResizeRows = false;
			this.dgvSPDays.BackgroundColor = Color.White;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvSPDays.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvSPDays, "dgvSPDays");
			this.dgvSPDays.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSPDays.Columns.AddRange(new DataGridViewColumn[]
			{
				this.No,
				this.date,
				this.onAll,
				this.onTime,
				this.offAll,
				this.offTime
			});
			this.dgvSPDays.MultiSelect = false;
			this.dgvSPDays.Name = "dgvSPDays";
			this.dgvSPDays.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvSPDays.RowHeadersVisible = false;
			this.dgvSPDays.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle2.ForeColor = Color.Black;
			this.dgvSPDays.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.dgvSPDays.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvSPDays.StandardTab = true;
			this.dgvSPDays.TabStop = false;
			this.dgvSPDays.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgvDays_CellMouseDown);
			this.dgvSPDays.CellValueChanged += new DataGridViewCellEventHandler(this.dgvSPDays_CellValueChanged);
			this.dgvSPDays.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvDays_CurrentCellDirtyStateChanged);
			componentResourceManager.ApplyResources(this.No, "No");
			this.No.Name = "No";
			this.No.ReadOnly = true;
			componentResourceManager.ApplyResources(this.date, "date");
			this.date.Name = "date";
			this.date.SortMode = DataGridViewColumnSortMode.Automatic;
			componentResourceManager.ApplyResources(this.onAll, "onAll");
			this.onAll.Name = "onAll";
			componentResourceManager.ApplyResources(this.onTime, "onTime");
			this.onTime.Name = "onTime";
			componentResourceManager.ApplyResources(this.offAll, "offAll");
			this.offAll.Name = "offAll";
			this.offTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.offTime, "offTime");
			this.offTime.Name = "offTime";
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.dataGridViewCalendarColumn1, "dataGridViewCalendarColumn1");
			this.dataGridViewCalendarColumn1.Name = "dataGridViewCalendarColumn1";
			this.dataGridViewCalendarColumn1.SortMode = DataGridViewColumnSortMode.Automatic;
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
			componentResourceManager.ApplyResources(this.dataGridViewCalendarColumn2, "dataGridViewCalendarColumn2");
			this.dataGridViewCalendarColumn2.Name = "dataGridViewCalendarColumn2";
			this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
			componentResourceManager.ApplyResources(this.dataGridViewCheckBoxColumn2, "dataGridViewCheckBoxColumn2");
			this.dataGridViewCalendarColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.dataGridViewCalendarColumn3, "dataGridViewCalendarColumn3");
			this.dataGridViewCalendarColumn3.Name = "dataGridViewCalendarColumn3";
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.butSave);
			base.Controls.Add(this.groupBoxTaskInfo);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.butCancel);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "GpPowerTaskDlg";
			base.ShowInTaskbar = false;
			this.groupBoxTaskInfo.ResumeLayout(false);
			this.groupBoxTaskInfo.PerformLayout();
			this.panelDaily.ResumeLayout(false);
			this.tblPanelDay.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tcYearly.ResumeLayout(false);
			this.tpWeekbase.ResumeLayout(false);
			this.tpSpecial.ResumeLayout(false);
			((ISupportInitialize)this.dgvSPDays).EndInit();
			base.ResumeLayout(false);
		}
	}
}
