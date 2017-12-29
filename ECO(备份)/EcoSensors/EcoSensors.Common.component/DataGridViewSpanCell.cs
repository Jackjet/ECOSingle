using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.Common.component
{
	internal class DataGridViewSpanCell : DataGridViewTextBoxCell
	{
		private int ColumnSpan;
		private int RowSpan;
		private Color m_backColor = Color.White;
		private Color m_selectedbackColor = Color.Blue;
		private bool m_ifseleced;
		public Color BackColor
		{
			get
			{
				return this.m_backColor;
			}
			set
			{
				this.m_backColor = value;
			}
		}
		public Color SelectionBackColor
		{
			set
			{
				this.m_selectedbackColor = value;
			}
		}
		public bool ifSelected
		{
			set
			{
				this.m_ifseleced = value;
			}
		}
		public int GetColumnSpan()
		{
			return this.ColumnSpan;
		}
		public void SetColumnSpan(int value)
		{
			this.ColumnSpan = value;
		}
		public int GetRowSpan()
		{
			return this.RowSpan;
		}
		public void SetRowSpan(int value)
		{
			this.RowSpan = value;
		}
		public override object Clone()
		{
			DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)base.Clone();
			dataGridViewSpanCell.ColumnSpan = this.ColumnSpan;
			dataGridViewSpanCell.RowSpan = this.RowSpan;
			return dataGridViewSpanCell;
		}
		public DataGridViewSpanCell()
		{
			this.ColumnSpan = 0;
			this.RowSpan = 0;
		}
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (this.ColumnSpan == 0 && this.RowSpan == 0)
			{
				if (this.m_ifseleced)
				{
					cellStyle.BackColor = this.m_selectedbackColor;
				}
				else
				{
					cellStyle.BackColor = this.m_backColor;
				}
				base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
				return;
			}
			try
			{
				new Pen(Brushes.Black);
				if (this.m_ifseleced)
				{
					graphics.FillRectangle(new SolidBrush(this.m_selectedbackColor), cellBounds);
				}
				else
				{
					graphics.FillRectangle(new SolidBrush(this.m_backColor), cellBounds);
				}
				if (this.ColumnSpan != 0)
				{
					graphics.DrawLine(new Pen(new SolidBrush(Color.Silver)), cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
				}
				else
				{
					graphics.DrawLine(new Pen(new SolidBrush(Color.Silver)), cellBounds.Right - 1, cellBounds.Top, cellBounds.Right - 1, cellBounds.Bottom);
				}
				if (this.ColumnSpan < 0)
				{
					graphics.DrawLine(new Pen(new SolidBrush(Color.Silver)), cellBounds.Right - 1, cellBounds.Top, cellBounds.Right - 1, cellBounds.Bottom);
				}
				if (this.RowSpan < 0)
				{
					graphics.DrawLine(new Pen(new SolidBrush(Color.Silver)), cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
				}
			}
			catch (System.Exception ex)
			{
				Trace.WriteLine(ex.ToString());
			}
		}
	}
}
