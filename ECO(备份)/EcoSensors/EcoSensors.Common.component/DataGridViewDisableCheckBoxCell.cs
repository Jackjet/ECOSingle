using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
namespace EcoSensors.Common.component
{
	internal class DataGridViewDisableCheckBoxCell : DataGridViewCheckBoxCell
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
			DataGridViewDisableCheckBoxCell dataGridViewDisableCheckBoxCell = (DataGridViewDisableCheckBoxCell)base.Clone();
			dataGridViewDisableCheckBoxCell.Enabled = this.Enabled;
			return dataGridViewDisableCheckBoxCell;
		}
		public DataGridViewDisableCheckBoxCell()
		{
			this.enabledValue = true;
		}
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (!this.enabledValue)
			{
				if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
				{
					Brush brush = new SolidBrush(this.Selected ? cellStyle.SelectionBackColor : cellStyle.BackColor);
					graphics.FillRectangle(brush, cellBounds);
					brush.Dispose();
				}
				if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
				{
					this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
				}
				CheckState checkState = CheckState.Unchecked;
				if (formattedValue != null)
				{
					if (formattedValue is CheckState)
					{
						checkState = (CheckState)formattedValue;
					}
					else
					{
						if (formattedValue is bool && (bool)formattedValue)
						{
							checkState = CheckState.Checked;
						}
					}
				}
				CheckBoxState state = (checkState == CheckState.Checked) ? CheckBoxState.CheckedDisabled : CheckBoxState.UncheckedDisabled;
				Size glyphSize = CheckBoxRenderer.GetGlyphSize(graphics, state);
				Point glyphLocation = new Point(cellBounds.X, cellBounds.Y);
				glyphLocation.X += (cellBounds.Width - glyphSize.Width) / 2;
				glyphLocation.Y += (cellBounds.Height - glyphSize.Height) / 2;
				if (glyphSize.Width + 4 < cellBounds.Width)
				{
					CheckBoxRenderer.DrawCheckBox(graphics, glyphLocation, state);
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
