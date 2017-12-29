using System;
using System.Windows.Forms;
namespace EcoSensors.Common.component
{
	internal class DataGridViewCalendarColumn : DataGridViewColumn
	{
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewCalendarCell)))
				{
					throw new System.InvalidCastException("Must be a CalendarCell");
				}
				base.CellTemplate = value;
			}
		}
		public DataGridViewCalendarColumn() : base(new DataGridViewCalendarCell())
		{
		}
	}
}
