using System;
using System.Windows.Forms;
namespace EcoSensors.Common.component
{
	internal class DataGridViewDisableCheckBoxColumn : DataGridViewCheckBoxColumn
	{
		public DataGridViewDisableCheckBoxColumn()
		{
			this.CellTemplate = new DataGridViewDisableCheckBoxCell();
		}
	}
}
