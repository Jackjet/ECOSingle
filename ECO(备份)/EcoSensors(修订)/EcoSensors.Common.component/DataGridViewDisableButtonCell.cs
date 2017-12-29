using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
namespace EcoSensors.Common.component
{
	internal class DataGridViewDisableButtonCell : DataGridViewButtonCell
	{
		private bool enabledValue;
		public bool Enabled
		{
			get
			{
				return this.enabledValue;
			}
			set
			{
				this.enabledValue = value;
			}
		}
		public override object Clone()
		{
			DataGridViewDisableButtonCell dataGridViewDisableButtonCell = (DataGridViewDisableButtonCell)base.Clone();
			dataGridViewDisableButtonCell.Enabled = this.Enabled;
			return dataGridViewDisableButtonCell;
		}
		public DataGridViewDisableButtonCell()
		{
			this.enabledValue = true;
		}
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (!this.enabledValue)
			{
				if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
				{
					SolidBrush solidBrush = new SolidBrush(cellStyle.BackColor);
					graphics.FillRectangle(solidBrush, cellBounds);
					solidBrush.Dispose();
				}
				if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
				{
					this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
				}
				Rectangle bounds = cellBounds;
				Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
				bounds.X += rectangle.X;
				bounds.Y += rectangle.Y;
				bounds.Height -= rectangle.Height;
				bounds.Width -= rectangle.Width;
				ButtonRenderer.DrawButton(graphics, bounds, PushButtonState.Disabled);
				if (base.FormattedValue is string)
				{
					TextRenderer.DrawText(graphics, (string)base.FormattedValue, base.DataGridView.Font, bounds, SystemColors.GrayText);
					return;
				}
			}
			else
			{
				base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
			}
		}
	}
}
