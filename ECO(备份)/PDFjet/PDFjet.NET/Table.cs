using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace PDFjet.NET
{
	public class Table
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
		private List<List<Cell>> tableData;
		private int numOfHeaderRows;
		private float x1;
		private float y1;
		private float padding = 1f;
		private float bottom_margin = 30f;
		public Table()
		{
			this.tableData = new List<List<Cell>>();
		}
		public void SetPosition(double x, double y)
		{
			this.SetPosition((float)x, (float)y);
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
		public void SetCellPadding(double padding)
		{
			this.padding = (float)padding;
		}
		public void SetCellPadding(float padding)
		{
			this.padding = padding;
		}
		public void SetCellMargin(double padding)
		{
			this.padding = (float)padding;
		}
		public void SetCellMargin(float padding)
		{
			this.padding = padding;
		}
		public void SetBottomMargin(double bottom_margin)
		{
			this.bottom_margin = (float)bottom_margin;
		}
		public void SetBottomMargin(float bottom_margin)
		{
			this.bottom_margin = bottom_margin;
		}
		public void SetData(List<List<Cell>> tableData)
		{
			this.tableData = tableData;
			this.numOfHeaderRows = 0;
			this.rendered = this.numOfHeaderRows;
		}
		public void SetData(List<List<Cell>> tableData, int numOfHeaderRows)
		{
			this.tableData = tableData;
			this.numOfHeaderRows = numOfHeaderRows;
			this.rendered = numOfHeaderRows;
		}
		public void AutoAdjustColumnWidths()
		{
			double[] array = new double[this.tableData[0].Count];
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					Cell cell = list[j];
					if (cell.GetColSpan() == 1 && cell.text != null)
					{
						cell.SetWidth((double)cell.font.StringWidth(cell.text));
						if (array[j] == 0.0 || (double)cell.GetWidth() > array[j])
						{
							array[j] = (double)cell.GetWidth();
						}
					}
				}
			}
			for (int k = 0; k < this.tableData.Count; k++)
			{
				List<Cell> list2 = this.tableData[k];
				for (int l = 0; l < list2.Count; l++)
				{
					Cell cell2 = list2[l];
					cell2.SetWidth(array[l] + (double)(3f * this.padding));
				}
			}
		}
		public void RightAlignNumbers()
		{
			for (int i = this.numOfHeaderRows; i < this.tableData.Count; i++)
			{
				List<Cell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					Cell cell = list[j];
					if (cell.text != null)
					{
						string text = cell.text;
						int length = text.Length;
						bool flag = true;
						int k = 0;
						while (k < length)
						{
							char c = text[k++];
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
		public void RemoveLineBetweenRows(int index1, int index2)
		{
			for (int i = index1; i < index2; i++)
			{
				List<Cell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					Cell cell = list[j];
					cell.SetBorder(131072, false);
				}
				list = this.tableData[i + 1];
				for (int k = 0; k < list.Count; k++)
				{
					Cell cell2 = list[k];
					cell2.SetBorder(65536, false);
				}
			}
		}
		public void SetTextAlignInColumn(int index, int alignment)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list = this.tableData[i];
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
				List<Cell> list = this.tableData[i];
				if (index < list.Count)
				{
					list[index].SetBrushColor(color);
				}
			}
		}
		public void SetTextFontInColumn(int index, Font font, double size)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list = this.tableData[i];
				if (index < list.Count)
				{
					list[index].font = font;
					list[index].font.SetSize(size);
				}
			}
		}
		public void SetTextColorInRow(int index, int color)
		{
			List<Cell> list = this.tableData[index];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetBrushColor(color);
			}
		}
		public void SetTextFontInRow(int index, Font font, double size)
		{
			List<Cell> list = this.tableData[index];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].font = font;
				list[i].font.SetSize(size);
			}
		}
		public void SetColumnWidth(int index, double width)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list = this.tableData[i];
				if (index < list.Count)
				{
					list[index].SetWidth(width);
				}
			}
		}
		public float GetColumnWidth(int index)
		{
			return this.GetCellAtRowColumn(0, index).GetWidth();
		}
		public Cell GetCellAt(int row, int col)
		{
			if (row >= 0)
			{
				return this.tableData[row][col];
			}
			return this.tableData[this.tableData.Count + row][col];
		}
		public Cell GetCellAtRowColumn(int row, int col)
		{
			return this.GetCellAt(row, col);
		}
		public List<Cell> GetRow(int index)
		{
			return this.tableData[index];
		}
		public List<Cell> GetColumn(int index)
		{
			List<Cell> list = new List<Cell>();
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list2 = this.tableData[i];
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
			for (int i = 0; i < this.numOfHeaderRows; i++)
			{
				float num3 = 0f;
				List<Cell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					Cell cell = list[j];
					float num4 = cell.GetHeight() + 2f * this.padding;
					if (num4 > num3)
					{
						num3 = num4;
					}
					float num5 = cell.GetWidth();
					for (int k = 1; k < cell.GetColSpan(); k++)
					{
						num5 += list[++j].GetWidth();
					}
					if (draw)
					{
						page.SetBrushColor(cell.GetBrushColor());
						cell.Paint(page, num, num2, num5, num3, this.padding);
					}
					num += num5;
				}
				num = this.x1;
				num2 += num3;
			}
			for (int l = this.rendered; l < this.tableData.Count; l++)
			{
				float num3 = 0f;
				List<Cell> list2 = this.tableData[l];
				for (int m = 0; m < list2.Count; m++)
				{
					Cell cell2 = list2[m];
					float num6 = cell2.GetHeight() + 2f * this.padding;
					if (num6 > num3)
					{
						num3 = num6;
					}
					float num5 = cell2.GetWidth();
					for (int n = 1; n < cell2.GetColSpan(); n++)
					{
						num5 += list2[++m].GetWidth();
					}
					if (draw)
					{
						page.SetBrushColor(cell2.GetBrushColor());
						cell2.Paint(page, num, num2, num5, num3, this.padding);
					}
					num += num5;
				}
				num = this.x1;
				num2 += num3;
				if (l < this.tableData.Count - 1)
				{
					List<Cell> list3 = this.tableData[l + 1];
					for (int num7 = 0; num7 < list3.Count; num7++)
					{
						Cell cell3 = list3[num7];
						float num8 = cell3.GetHeight() + 2f * this.padding;
						if (num8 > num3)
						{
							num3 = num8;
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
			List<Cell> list = this.tableData[0];
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
		public void WrapAroundCellText()
		{
			List<List<Cell>> list = new List<List<Cell>>();
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list2 = this.tableData[i];
				int num = 1;
				for (int j = 0; j < list2.Count; j++)
				{
					Cell cell = list2[j];
					int colSpan = cell.GetColSpan();
					for (int k = 1; k < colSpan; k++)
					{
						Cell cell2 = list2[j + k];
						cell.SetWidth((double)(cell.GetWidth() + cell2.GetWidth()));
						cell2.SetWidth(0.0);
					}
					int numVerCells = cell.GetNumVerCells(this.padding);
					if (numVerCells > num)
					{
						num = numVerCells;
					}
				}
				for (int l = 0; l < num; l++)
				{
					List<Cell> list3 = new List<Cell>();
					for (int m = 0; m < list2.Count; m++)
					{
						Cell cell3 = list2[m];
						Cell cell4 = new Cell(cell3.GetFont(), "");
						cell4.SetPoint(cell3.GetPoint());
						cell4.SetCompositeTextLine(cell3.GetCompositeTextLine());
						cell4.SetWidth((double)cell3.GetWidth());
						if (l == 0)
						{
							cell4.SetTopPadding(cell3.top_padding);
						}
						if (l == num - 1)
						{
							cell4.SetBottomPadding(cell3.bottom_padding);
						}
						cell4.SetLeftPadding(cell3.left_padding);
						cell4.SetRightPadding(cell3.right_padding);
						cell4.SetLineWidth(cell3.GetLineWidth());
						cell4.SetBgColor(cell3.GetBgColor());
						cell4.SetPenColor(cell3.GetPenColor());
						cell4.SetBrushColor(cell3.GetBrushColor());
						cell4.SetProperties(cell3.GetProperties());
						if (l == 0)
						{
							cell4.SetText(cell3.GetText());
						}
						else
						{
							if (l > 0)
							{
								cell4.SetBorder(65536, false);
								if (l < num - 1)
								{
									cell4.SetBorder(131072, false);
								}
							}
						}
						list3.Add(cell4);
					}
					list.Add(list3);
				}
			}
			for (int n = 0; n < list.Count; n++)
			{
				List<Cell> list4 = list[n];
				for (int num2 = 0; num2 < list4.Count; num2++)
				{
					Cell cell5 = list4[num2];
					if (cell5.text != null)
					{
						int num3 = 0;
						string[] array = Regex.Split(cell5.GetText(), "\\s+");
						StringBuilder stringBuilder = new StringBuilder();
						if (array.Length == 1)
						{
							stringBuilder.Append(array[0]);
						}
						else
						{
							for (int num4 = 0; num4 < array.Length; num4++)
							{
								string text = array[num4];
								if (cell5.font.StringWidth(stringBuilder.ToString() + " " + text) > cell5.GetWidth() - (cell5.left_padding + cell5.right_padding + 2f * this.padding))
								{
									list[n + num3][num2].SetText(stringBuilder.ToString());
									stringBuilder = new StringBuilder(text);
									num3++;
								}
								else
								{
									if (num4 > 0)
									{
										stringBuilder.Append(" ");
									}
									stringBuilder.Append(text);
								}
							}
						}
						list[n + num3][num2].SetText(stringBuilder.ToString());
					}
				}
			}
			this.tableData = list;
		}
		public void SetNoCellBorders()
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list = this.tableData[i];
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
				List<Cell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					this.tableData[i][j].SetPenColor(color);
				}
			}
		}
		public void SetCellBordersWidth(float width)
		{
			for (int i = 0; i < this.tableData.Count; i++)
			{
				List<Cell> list = this.tableData[i];
				for (int j = 0; j < list.Count; j++)
				{
					this.tableData[i][j].SetLineWidth(width);
				}
			}
		}
		public void ResetRenderedPagesCount()
		{
			this.rendered = this.numOfHeaderRows;
		}
	}
}
