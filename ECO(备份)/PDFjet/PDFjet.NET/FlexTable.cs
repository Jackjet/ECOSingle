using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace PDFjet.NET
{
	public class FlexTable
	{
		public static int DATA_HAS_0_HEADER_ROWS = 0;
		public static int DATA_HAS_1_HEADER_ROWS = 1;
		public static int DATA_HAS_2_HEADER_ROWS = 2;
		public static int DATA_HAS_3_HEADER_ROWS = 3;
		public static int DATA_HAS_4_HEADER_ROWS = 4;
		public static int DATA_HAS_5_HEADER_ROWS = 5;
		public static int DATA_HAS_6_HEADER_ROWS = 6;
		public static int DATA_HAS_7_HEADER_ROWS = 7;
		public static int DATA_HAS_8_HEADER_ROWS = 8;
		public static int DATA_HAS_9_HEADER_ROWS = 9;
		private int rendered;
		private int numOfPages;
		private List<List<AbstractCell>> tableData;
		private int numOfHeaderRows;
		private float x1;
		private float y1;
		private float margin = 1f;
		private float bottom_margin = 30f;
		public FlexTable()
		{
			this.tableData = new List<List<AbstractCell>>();
		}
		public void SetPosition(double x, double y)
		{
			this.x1 = (float)x;
			this.y1 = (float)y;
		}
		public void SetPosition(float x, float y)
		{
			this.SetLocation(x, y);
		}
		public void SetLocation(float x, float y)
		{
			this.x1 = x;
			this.y1 = y;
		}
		public void SetCellPadding(double margin)
		{
			this.margin = (float)margin;
		}
		public void SetCellPadding(float margin)
		{
			this.margin = margin;
		}
		public void SetCellMargin(double margin)
		{
			this.margin = (float)margin;
		}
		public void SetCellMargin(float margin)
		{
			this.margin = margin;
		}
		public void SetBottomMargin(double bottom_margin)
		{
			this.bottom_margin = (float)bottom_margin;
		}
		public void SetBottomMargin(float bottom_margin)
		{
			this.bottom_margin = bottom_margin;
		}
		public void SetData(List<List<AbstractCell>> tableData)
		{
			this.tableData = tableData;
			this.numOfHeaderRows = 0;
			this.rendered = this.numOfHeaderRows;
		}
		public void SetData(List<List<AbstractCell>> tableData, int numOfHeaderRows)
		{
			this.tableData = tableData;
			this.numOfHeaderRows = numOfHeaderRows;
			this.rendered = numOfHeaderRows;
		}
		public void AutoAdjustColumnWidths(float tableWidth)
		{
			float[] array = new float[this.tableData[0].Count];
			foreach (List<AbstractCell> current in this.tableData)
			{
				for (int i = 0; i < current.Count; i++)
				{
					AbstractCell abstractCell = current[i];
					if (abstractCell.GetColSpan() == 1)
					{
						abstractCell.ComputeWidth();
						array[i] = Math.Max(array[i], abstractCell.GetWidth());
					}
				}
			}
			if (tableWidth > 0f)
			{
				float num = 0f;
				for (int j = 0; j < array.Length; j++)
				{
					num += array[j];
					if (num > tableWidth)
					{
						array[j] = Math.Max(0f, array[j] - (num - tableWidth));
					}
				}
			}
			foreach (List<AbstractCell> current2 in this.tableData)
			{
				for (int k = 0; k < current2.Count; k++)
				{
					AbstractCell abstractCell2 = current2[k];
					if (abstractCell2.GetColSpan() <= 1)
					{
						abstractCell2.SetWidth(array[k] + 3f * this.margin);
					}
					else
					{
						float num2 = 0f;
						int num3 = k;
						while (num3 < current2.Count && num3 < k + abstractCell2.GetColSpan())
						{
							num2 += array[num3];
							num3++;
						}
						abstractCell2.SetWidth(num2 + 3f * this.margin);
					}
				}
			}
		}
		public void AutoAdjustColumnWidths()
		{
			this.AutoAdjustColumnWidths(-1f);
		}
		public void RightAlignNumbers()
		{
			for (int i = this.numOfHeaderRows; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list = this.tableData[i];
				foreach (object current in list)
				{
					if (current is Cell)
					{
						Cell cell = (Cell)current;
						if (cell.GetText() != null)
						{
							string text = cell.text;
							int length = text.Length;
							bool flag = true;
							int j = 0;
							while (j < length)
							{
								char c = text[j++];
								if (!char.IsNumber(c) && c != '(' && c != ')' && c != '-' && c != '.' && c != ',' && c != '\'')
								{
									flag = false;
								}
							}
							if (flag)
							{
								cell.SetTextAlignment(2097152);
							}
						}
					}
				}
			}
		}
		public void RemoveLineBetweenRows(int index1, int index2)
		{
			for (int i = index1; i < index2; i++)
			{
				List<AbstractCell> list = this.tableData[i];
				foreach (AbstractCell current in list)
				{
					current.SetBorder(131072, false);
				}
				list = this.tableData[i + 1];
				foreach (AbstractCell current2 in list)
				{
					current2.SetBorder(65536, false);
				}
			}
		}
		public void SetTextAlignInColumn(int index, int alignment)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list = this.tableData[i];
				if (index < list.Count)
				{
					list[index].SetTextAlignment(alignment);
				}
			}
		}
		public void SetTextColorInColumn(int index, int color)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list = this.tableData[i];
				if (index < list.Count)
				{
					list[index].SetBrushColor(color);
				}
			}
		}
		[Obsolete("")]
		public void SetTextFontInColumn(int index, Font font, double size)
		{
			foreach (List<AbstractCell> current in this.tableData)
			{
				if (index < current.Count)
				{
					object obj = current[index];
					if (obj is Cell)
					{
						Cell cell = (Cell)obj;
						font.SetSize(size);
						cell.SetFont(font);
					}
				}
			}
		}
		public void SetFontInColumn(int index, Font font)
		{
			foreach (List<AbstractCell> current in this.tableData)
			{
				if (index < current.Count)
				{
					object obj = current[index];
					if (obj is Cell)
					{
						Cell cell = (Cell)obj;
						cell.SetFont(font);
					}
				}
			}
		}
		public void SetTextColorInRow(int index, int color)
		{
			List<AbstractCell> list = this.tableData[index];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetBrushColor(color);
			}
		}
		[Obsolete("")]
		public void SetTextFontInRow(int index, Font font, double size)
		{
			List<AbstractCell> list = this.tableData[index];
			foreach (object current in list)
			{
				if (current is Cell)
				{
					Cell cell = (Cell)current;
					font.SetSize(size);
					cell.SetFont(font);
				}
			}
		}
		public void SetFontInRow(int index, Font font)
		{
			List<AbstractCell> list = this.tableData[index];
			foreach (object current in list)
			{
				if (current is Cell)
				{
					Cell cell = (Cell)current;
					cell.SetFont(font);
				}
			}
		}
		public void SetColumnWidth(int index, double width)
		{
			foreach (List<AbstractCell> current in this.tableData)
			{
				if (index < current.Count)
				{
					current[index].SetWidth((float)width);
				}
			}
		}
		[Obsolete("")]
		public AbstractCell GetCellAt(int row, int col)
		{
			if (row >= 0)
			{
				return (TextCell)this.tableData[row][col];
			}
			return (TextCell)this.tableData[this.tableData.Count + row][col];
		}
		public AbstractCell GetCellAtRowColumn(int row, int col)
		{
			if (row >= 0)
			{
				return this.tableData[row][col];
			}
			return this.tableData[this.tableData.Count + row][col];
		}
		[Obsolete("")]
		public List<AbstractCell> GetRow(int index)
		{
			return this.tableData[index];
		}
		public List<AbstractCell> GetRowAtIndex(int index)
		{
			return this.tableData[index];
		}
		[Obsolete("")]
		public List<AbstractCell> GetColumn(int index)
		{
			List<AbstractCell> list = new List<AbstractCell>();
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list2 = this.tableData[i];
				if (index < list2.Count)
				{
					list.Add((TextCell)list2[index]);
				}
			}
			return list;
		}
		public List<AbstractCell> GetColumnAtIndex(int index)
		{
			List<AbstractCell> list = new List<AbstractCell>();
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list2 = this.tableData[i];
				if (index < list2.Count)
				{
					list.Add(list2[index]);
				}
			}
			return list;
		}
		public int GetNumberOfPages(Page page)
		{
			this.numOfPages = 1;
			while (this.HasMoreData())
			{
				this.DrawOn(page, false);
			}
			this.ResetRenderedPagesCount();
			return this.numOfPages;
		}
		public Point DrawOn(Page page)
		{
			return this.DrawOn(page, true);
		}
		public Point DrawOn(Page page, bool draw)
		{
			float num = this.x1;
			float num2 = this.y1;
			float num3 = 0f;
			for (int i = 0; i < this.numOfHeaderRows; i++)
			{
				num3 = 0f;
				List<AbstractCell> list = this.tableData[i];
				foreach (AbstractCell current in list)
				{
					num3 = Math.Max(num3, current.GetComputedHeight(2f * this.margin) + 2f * this.margin);
				}
				for (int j = 0; j < list.Count; j++)
				{
					AbstractCell abstractCell = list[j];
					float num4 = abstractCell.GetComputedHeight(2f * this.margin) + 2f * this.margin;
					if (num4 > num3)
					{
						num3 = num4;
					}
					float num5 = abstractCell.GetWidth();
					int colSpan = abstractCell.GetColSpan();
					for (int k = 1; k < colSpan; k++)
					{
						num5 += list[++j].GetWidth();
					}
					if (draw)
					{
						page.SetBrushColor(abstractCell.GetBrushColor());
						abstractCell.Paint(page, num, num2, num5, num3, this.margin);
					}
					num += num5;
				}
				num = this.x1;
				num2 += num3;
			}
			for (int l = this.rendered; l < this.tableData.Count; l++)
			{
				num3 = 0f;
				List<AbstractCell> list2 = this.tableData[l];
				foreach (AbstractCell current2 in list2)
				{
					num3 = Math.Max(num3, current2.GetComputedHeight(2f * this.margin) + 2f * this.margin);
				}
				for (int m = 0; m < list2.Count; m++)
				{
					AbstractCell abstractCell2 = list2[m];
					float num5 = abstractCell2.GetWidth();
					int colSpan2 = abstractCell2.GetColSpan();
					for (int n = 1; n < colSpan2; n++)
					{
						num5 += list2[++m].GetWidth();
					}
					if (draw)
					{
						page.SetBrushColor(abstractCell2.GetBrushColor());
						abstractCell2.Paint(page, num, num2, num5, num3, this.margin);
					}
					num += num5;
				}
				num = this.x1;
				num2 += num3;
				if (l < this.tableData.Count - 1)
				{
					List<AbstractCell> list3 = this.tableData[l + 1];
					for (int num6 = 0; num6 < list3.Count; num6++)
					{
						AbstractCell abstractCell3 = list3[num6];
						float num7 = abstractCell3.GetComputedHeight(2f * this.margin) + 2f * this.margin;
						if (num7 > num3)
						{
							num3 = num7;
						}
					}
				}
				if (num2 + num3 > page.height - this.bottom_margin)
				{
					if (l == this.tableData.Count - 1)
					{
						this.rendered = -1;
					}
					else
					{
						this.rendered = l + 1;
						this.numOfPages++;
					}
					return new Point(num, num2);
				}
			}
			this.rendered = -1;
			return new Point(num, num2);
		}
		public bool HasMoreData()
		{
			return this.rendered != -1;
		}
		public float GetWidth()
		{
			float num = 0f;
			List<AbstractCell> list = this.tableData[0];
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].GetWidth();
			}
			return num;
		}
		public int GetRowsRendered()
		{
			if (this.rendered != -1)
			{
				return this.rendered - this.numOfHeaderRows;
			}
			return this.rendered;
		}
		public void WrapAroundCellTextPreventPagebreaksInRows()
		{
			foreach (List<AbstractCell> current in this.tableData)
			{
				foreach (object current2 in current)
				{
					if (current2 is Cell)
					{
						TextCell textCell = (TextCell)current2;
						textCell.SetWordwrap(true);
					}
				}
			}
		}
		public void WrapAroundCellText()
		{
			List<List<AbstractCell>> list = new List<List<AbstractCell>>();
			foreach (List<AbstractCell> current in this.tableData)
			{
				bool flag = false;
				int num = 1;
				foreach (object current2 in current)
				{
					if (current2 is TextCell)
					{
						TextCell textCell = (TextCell)current2;
						num = Math.Max(num, textCell.GetNumVerCells(this.margin));
					}
					else
					{
						if (current2 is ImageCell)
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					List<AbstractCell> list2 = new List<AbstractCell>();
					foreach (object current3 in current)
					{
						if (current3 is TextCell)
						{
							TextCell textCell2 = (TextCell)current3;
							textCell2.SetWordwrap(true);
						}
						list2.Add((AbstractCell)current3);
					}
					list.Add(list2);
				}
				else
				{
					for (int i = 0; i < num; i++)
					{
						List<AbstractCell> list3 = new List<AbstractCell>();
						foreach (object current4 in current)
						{
							TextCell textCell4;
							if (current4 is TextCell)
							{
								TextCell textCell3 = (TextCell)current4;
								textCell4 = new TextCell(textCell3.GetFont(), "");
								if (i == 0)
								{
									textCell4.SetText(textCell3.GetText());
								}
								textCell4.SetPoint(textCell3.GetPoint());
								textCell4.SetCompositeTextLine(textCell3.GetCompositeTextLine());
								textCell4.SetWidth(textCell3.GetWidth());
								textCell4.SetHeight(textCell3.GetHeight());
								textCell4.SetLineWidth(textCell3.GetLineWidth());
								textCell4.SetBgColor(textCell3.GetBgColor());
								textCell4.SetPenColor(textCell3.GetPenColor());
								textCell4.SetBrushColor(textCell3.GetBrushColor());
								textCell4.SetProperties(textCell3.GetProperties());
							}
							else
							{
								textCell4 = new TextCell(null);
							}
							list3.Add(textCell4);
						}
						list.Add(list3);
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				List<AbstractCell> list4 = list[j];
				for (int k = 0; k < list4.Count; k++)
				{
					object obj = list4[k];
					if (obj is TextCell)
					{
						TextCell textCell5 = (TextCell)obj;
						if (!textCell5.IsWordwrap() && textCell5.text != null)
						{
							string[] array = Regex.Split(textCell5.GetText(), "\\s+");
							int num2 = 0;
							StringBuilder stringBuilder = new StringBuilder();
							for (int l = 0; l < array.Length; l++)
							{
								string text = array[l];
								if (textCell5.font.StringWidth(stringBuilder.ToString() + " " + text) > textCell5.GetWidth() - this.margin)
								{
									((TextCell)list[j + num2][k]).SetText(stringBuilder.ToString());
									stringBuilder = new StringBuilder(text);
									num2++;
								}
								else
								{
									if (l > 0)
									{
										stringBuilder.Append(" ");
									}
									stringBuilder.Append(text);
								}
							}
							((TextCell)list[j + num2][k]).SetText(stringBuilder.ToString());
						}
					}
				}
			}
			this.tableData = list;
		}
		public void SetNoCellBorders()
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					this.tableData[i][j].SetNoBorders();
				}
			}
		}
		public void SetCellBordersColor(int color)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					AbstractCell abstractCell = this.tableData[i][j];
					abstractCell.SetPenColor(color);
				}
			}
		}
		public void SetCellBordersWidth(float width)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<AbstractCell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					AbstractCell abstractCell = this.tableData[i][j];
					abstractCell.SetLineWidth(width);
				}
			}
		}
		public void ResetRenderedPagesCount()
		{
			this.rendered = this.numOfHeaderRows;
		}
	}
}
