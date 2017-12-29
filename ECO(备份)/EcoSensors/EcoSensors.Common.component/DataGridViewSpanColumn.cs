using System;
using System.Windows.Forms;
namespace EcoSensors.Common.component
{
	internal class DataGridViewSpanColumn : DataGridViewTextBoxColumn
	{
		public DataGridViewSpanColumn()
		{
			this.CellTemplate = new DataGridViewSpanCell();
		}
	}
}
