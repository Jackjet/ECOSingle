using System;
using System.Text;
using System.Text.RegularExpressions;
namespace PDFjet.NET
{
	public class Cell
	{
		internal Font font;
		internal string text;
		internal Point point;
		internal CompositeTextLine compositeTextLine;
		internal float width = 70f;
		internal float top_padding;
		internal float bottom_padding;
		internal float left_padding;
		internal float right_padding;
		internal float lineWidth;
		private int background = 16777215;
		private int pen;
		private int brush;
		private int properties = 983041;
		public Cell(Font font)
		{
			this.font = font;
		}
		public Cell(Font font, string text)
		{
			this.font = font;
			this.text = text;
		}
		public void SetFont(Font font)
		{
			this.font = font;
		}
		public Font GetFont()
		{
			return this.font;
		}
		public void SetText(string text)
		{
			this.text = text;
		}
		public string GetText()
		{
			return this.text;
		}
		public void SetPoint(Point point)
		{
			this.point = point;
		}
		public Point GetPoint()
		{
			return this.point;
		}
		public void SetCompositeTextLine(CompositeTextLine compositeTextLine)
		{
			this.compositeTextLine = compositeTextLine;
		}
		public CompositeTextLine GetCompositeTextLine()
		{
			return this.compositeTextLine;
		}
		public void SetWidth(double width)
		{
			this.width = (float)width;
		}
		public float GetWidth()
		{
			return this.width;
		}
		public void SetTopPadding(float padding)
		{
			this.top_padding = padding;
		}
		public void SetBottomPadding(float padding)
		{
			this.bottom_padding = padding;
		}
		public void SetLeftPadding(float padding)
		{
			this.left_padding = padding;
		}
		public void SetRightPadding(float padding)
		{
			this.right_padding = padding;
		}
		public float GetHeight()
		{
			return this.font.body_height + this.top_padding + this.bottom_padding;
		}
		public void SetLineWidth(float lineWidth)
		{
			this.lineWidth = lineWidth;
		}
		public float GetLineWidth()
		{
			return this.lineWidth;
		}
		public void SetBgColor(int color)
		{
			this.background = color;
		}
		public int GetBgColor()
		{
			return this.background;
		}
		public void SetPenColor(int color)
		{
			this.pen = color;
		}
		public int GetPenColor()
		{
			return this.pen;
		}
		public void SetBrushColor(int color)
		{
			this.brush = color;
		}
		public int GetBrushColor()
		{
			return this.brush;
		}
		public void SetFgColor(int color)
		{
			this.pen = color;
			this.brush = color;
		}
		internal void SetProperties(int properties)
		{
			this.properties = properties;
		}
		internal int GetProperties()
		{
			return this.properties;
		}
		public void SetColSpan(int colspan)
		{
			this.properties &= 16711680;
			this.properties |= (colspan & 65535);
		}
		public int GetColSpan()
		{
			return this.properties & 65535;
		}
		public void SetBorder(int border, bool visible)
		{
			if (visible)
			{
				this.properties |= border;
				return;
			}
			this.properties &= (~border & 16777215);
		}
		public bool GetBorder(int border)
		{
			return (this.properties & border) != 0;
		}
		public void SetNoBorders()
		{
			this.properties &= 15794175;
		}
		public void SetTextAlignment(int alignment)
		{
			this.properties &= 13631487;
			this.properties |= (alignment & 3145728);
		}
		public int GetTextAlignment()
		{
			return this.properties & 3145728;
		}
		public void SetUnderline(bool underline)
		{
			if (underline)
			{
				this.properties |= 4194304;
				return;
			}
			this.properties &= 12582911;
		}
		public bool GetUnderline()
		{
			return (this.properties & 4194304) != 0;
		}
		public void SetStrikeout(bool strikeout)
		{
			if (strikeout)
			{
				this.properties |= 8388608;
				return;
			}
			this.properties &= 8388607;
		}
		public bool GetStrikeout()
		{
			return (this.properties & 8388608) != 0;
		}
		internal virtual void Paint(Page page, float x, float y, float w, float h, float padding)
		{
			if (this.text != null)
			{
				this.DrawBackground(page, x, y, w, h);
			}
			this.DrawBorders(page, x, y, w, h);
			if (this.point != null)
			{
				this.point.x = x + w - (this.font.ascent / 2f + this.left_padding);
				this.point.y = y + (this.font.ascent / 2f + this.top_padding);
				this.point.r = this.font.ascent / 3f;
				page.DrawPoint(this.point);
			}
			if (this.text != null)
			{
				this.DrawText(page, x, y, w, padding);
			}
		}
		private void DrawBackground(Page page, float x, float y, float cell_w, float cell_h)
		{
			page.SetBrushColor(this.background);
			page.FillRect((double)x, (double)y, (double)cell_w, (double)cell_h);
		}
		private void DrawBorders(Page page, float x, float y, float cell_w, float cell_h)
		{
			page.SetPenColor(this.pen);
			page.SetPenWidth(this.lineWidth);
			if (this.GetBorder(65536) && this.GetBorder(131072) && this.GetBorder(262144) && this.GetBorder(524288))
			{
				page.DrawRect((double)x, (double)y, (double)cell_w, (double)cell_h);
				return;
			}
			if (this.GetBorder(65536))
			{
				page.MoveTo(x, y);
				page.LineTo(x + cell_w, y);
				page.StrokePath();
			}
			if (this.GetBorder(131072))
			{
				page.MoveTo(x, y + cell_h);
				page.LineTo(x + cell_w, y + cell_h);
				page.StrokePath();
			}
			if (this.GetBorder(262144))
			{
				page.MoveTo(x, y);
				page.LineTo(x, y + cell_h);
				page.StrokePath();
			}
			if (this.GetBorder(524288))
			{
				page.MoveTo(x + cell_w, y);
				page.LineTo(x + cell_w, y + cell_h);
				page.StrokePath();
			}
		}
		private void DrawText(Page page, float x, float y, float cell_w, float padding)
		{
			float y2 = y + this.font.ascent + this.top_padding + padding;
			page.SetPenColor(this.pen);
			page.SetBrushColor(this.brush);
			if (this.GetTextAlignment() == 2097152)
			{
				if (this.compositeTextLine != null)
				{
					float x2 = x + cell_w - ((float)this.compositeTextLine.GetWidth() + this.right_padding + padding);
					this.compositeTextLine.SetPosition(x2, y2);
					this.compositeTextLine.DrawOn(page);
					return;
				}
				float x3 = x + cell_w - (this.font.StringWidth(this.text) + this.right_padding + padding);
				page.DrawString(this.font, this.text, x3, y2);
				if (this.GetUnderline())
				{
					this.UnderlineText(page, this.font, this.text, x3, y2);
					return;
				}
			}
			else
			{
				if (this.GetTextAlignment() == 1048576)
				{
					if (this.compositeTextLine != null)
					{
						float x4 = x + this.left_padding + padding + (cell_w - (this.left_padding + this.right_padding + 2f * padding) - (float)this.compositeTextLine.GetWidth()) / 2f;
						this.compositeTextLine.SetPosition(x4, y2);
						this.compositeTextLine.DrawOn(page);
						return;
					}
					float x5 = x + this.left_padding + padding + (cell_w - (this.left_padding + this.right_padding + 2f * padding) - this.font.StringWidth(this.text)) / 2f;
					page.DrawString(this.font, this.text, x5, y2);
					if (this.GetUnderline())
					{
						this.UnderlineText(page, this.font, this.text, x5, y2);
						return;
					}
				}
				else
				{
					if (this.GetTextAlignment() != 0)
					{
						throw new Exception("Invalid Text Alignment!");
					}
					float x6 = x + this.left_padding + padding;
					if (this.compositeTextLine != null)
					{
						this.compositeTextLine.SetPosition(x6, y2);
						this.compositeTextLine.DrawOn(page);
						return;
					}
					page.DrawString(this.font, this.text, x6, y2);
					if (this.GetUnderline())
					{
						this.UnderlineText(page, this.font, this.text, x6, y2);
						return;
					}
				}
			}
		}
		private void UnderlineText(Page page, Font font, string text, float x, float y)
		{
			float descent = font.GetDescent();
			page.SetPenWidth(font.underlineThickness);
			page.MoveTo(x, y + descent);
			page.LineTo(x + font.StringWidth(text), y + descent);
			page.StrokePath();
		}
		internal int GetNumVerCells(float padding)
		{
			int num = 1;
			string[] array = Regex.Split(this.text, "\\s+");
			if (array.Length == 1)
			{
				return num;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (this.font.StringWidth(stringBuilder.ToString() + " " + text) > this.GetWidth() - (this.left_padding + this.right_padding + 2f * padding))
				{
					stringBuilder = new StringBuilder(text);
					num++;
				}
				else
				{
					if (i > 0)
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append(text);
				}
			}
			return num;
		}
	}
}
