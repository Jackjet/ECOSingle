using System;
using System.Windows.Forms;
namespace EcoSensors.Common.component
{
	internal class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
	{
		public DataGridViewDisableButtonColumn()
		{
			this.CellTemplate = new DataGridViewDisableButtonCell();
		}
	}
}
