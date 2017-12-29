using System;
using System.Windows.Forms;
namespace EcoSensors.Common.component
{
	internal class DataGridViewCalendarCell : DataGridViewTextBoxCell
	{
		public const string FMT_HHmm = "HH:mm";
		public override System.Type EditType
		{
			get
			{
				return typeof(CalendarEditingControl);
			}
		}
		public override System.Type ValueType
		{
			get
			{
				return typeof(System.DateTime);
			}
		}
		public override object DefaultNewRowValue
		{
			get
			{
				return System.DateTime.Now;
			}
		}
		public DataGridViewCalendarCell()
		{
			base.Style.Format = "HH:mm";
		}
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			CalendarEditingControl calendarEditingControl = base.DataGridView.EditingControl as CalendarEditingControl;
			if (base.Style.Format.Equals("HH:mm"))
			{
				calendarEditingControl.Format = DateTimePickerFormat.Custom;
				calendarEditingControl.CustomFormat = "HH:mm";
				calendarEditingControl.ShowUpDown = true;
			}
			else
			{
				calendarEditingControl.Format = DateTimePickerFormat.Custom;
				calendarEditingControl.CustomFormat = Application.CurrentCulture.DateTimeFormat.ShortDatePattern + " ddd";
				calendarEditingControl.ShowUpDown = false;
			}
			if (base.Value == null)
			{
				calendarEditingControl.Value = (System.DateTime)this.DefaultNewRowValue;
				return;
			}
			calendarEditingControl.Value = (System.DateTime)base.Value;
		}
	}
}
