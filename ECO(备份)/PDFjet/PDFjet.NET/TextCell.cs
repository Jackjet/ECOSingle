using System;
using System.Text.RegularExpressions;
namespace PDFjet.NET
{
	public class TextCell : AbstractCell
	{
		internal Font font;
		internal string text;
		internal CompositeTextLine compositeTextLine;
		internal bool wordwrap;
		public TextCell(Font font)
		{
			this.font = font;
		}
		public TextCell(Font font, string text)
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
		public void SetCompositeTextLine(CompositeTextLine compositeTextLine)
		{
			this.compositeTextLine = compositeTextLine;
		}
		public CompositeTextLine GetCompositeTextLine()
		{
			return this.compositeTextLine;
		}
		public override void ComputeWidth()
		{
			if (this.text != null)
			{
				base.SetWidth(this.font.StringWidth(this.text));
			}
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
		internal override void Paint(Page page, float x, float y, float w, float h, float margin)
		{
			if (this.text != null && this.text.Length > 0)
			{
				this.DrawBackground(page, x, y, w, h);
			}
			this.DrawBorders(page, x, y, w, h);
			if (this.point != null)
			{
				this.point.x = x + w - (this.font.ascent / 2f + margin);
				this.point.y = y + (this.font.ascent / 2f + margin);
				this.point.r = this.font.ascent / 3f;
				page.DrawPoint(this.point);
			}
			if (this.text != null)
			{
				this.DrawText(page, x, y, w, margin);
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
			if (base.GetBorder(65536) && base.GetBorder(131072) && base.GetBorder(262144) && base.GetBorder(524288))
			{
				page.DrawRect((double)x, (double)y, (double)cell_w, (double)cell_h);
				return;
			}
			if (base.GetBorder(65536))
			{
				page.MoveTo(x, y);
				page.LineTo(x + cell_w, y);
				page.StrokePath();
			}
			if (base.GetBorder(131072))
			{
				page.MoveTo(x, y + cell_h);
				page.LineTo(x + cell_w, y + cell_h);
				page.StrokePath();
			}
			if (base.GetBorder(262144))
			{
				page.MoveTo(x, y);
				page.LineTo(x, y + cell_h);
				page.StrokePath();
			}
			if (base.GetBorder(524288))
			{
				page.MoveTo(x + cell_w, y);
				page.LineTo(x + cell_w, y + cell_h);
				page.StrokePath();
			}
		}
		private void DrawText(Page page, float x, float y, float cell_w, float margin)
		{
			float y_text = y + this.font.ascent + margin;
			page.SetPenColor(this.pen);
			page.SetBrushColor(this.brush);
			if (!this.wordwrap || base.GetColSpan() > 1)
			{
				this.DrawSingleTextLine(page, x, cell_w, margin, y_text, this.text);
			}
		}
		private void DrawSingleTextLine(Page page, float x, float cell_w, float margin, float y_text, string text)
		{
			if (base.GetTextAlignment() == 2097152)
			{
				if (this.compositeTextLine != null)
				{
					this.compositeTextLine.SetPosition((double)(x + cell_w) - (this.compositeTextLine.GetWidth() + (double)margin), (double)y_text);
					this.compositeTextLine.DrawOn(page);
					return;
				}
				float x2 = x + cell_w - (this.font.StringWidth(text) + margin);
				page.DrawString(this.font, text, x2, y_text);
				if (this.GetUnderline())
				{
					this.UnderlineText(page, this.font, text, x2, y_text);
				}
				if (this.GetStrikeout())
				{
					this.StrikeoutText(page, this.font, text, x2, y_text);
					return;
				}
			}
			else
			{
				if (base.GetTextAlignment() == 1048576)
				{
					if (this.compositeTextLine != null)
					{
						this.compositeTextLine.SetPosition((double)x + ((double)cell_w - this.compositeTextLine.GetWidth()) / 2.0, (double)y_text);
						this.compositeTextLine.DrawOn(page);
						return;
					}
					float x3 = x + (cell_w - this.font.StringWidth(text)) / 2f;
					page.DrawString(this.font, text, x3, y_text);
					if (this.GetUnderline())
					{
						this.UnderlineText(page, this.font, text, x3, y_text);
					}
					if (this.GetStrikeout())
					{
						this.StrikeoutText(page, this.font, text, x3, y_text);
						return;
					}
				}
				else
				{
					float x4 = x + margin;
					if (this.compositeTextLine == null)
					{
						page.DrawString(this.font, text, x4, y_text);
						if (this.GetUnderline())
						{
							this.UnderlineText(page, this.font, text, x4, y_text);
						}
						if (this.GetStrikeout())
						{
							this.StrikeoutText(page, this.font, text, x4, y_text);
							return;
						}
					}
					else
					{
						this.compositeTextLine.SetPosition(x4, y_text);
						this.compositeTextLine.DrawOn(page);
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
		private void StrikeoutText(Page page, Font font, string text, float x, float y)
		{
			page.SetPenWidth(font.underlineThickness);
			page.MoveTo((double)x, (double)y - (double)font.GetAscent() / 3.0);
			page.LineTo((double)(x + font.StringWidth(text)), (double)y - (double)font.GetAscent() / 3.0);
			page.StrokePath();
		}
		internal int GetNumVerCells(float margin)
		{
			int num = 1;
			float num2 = base.GetWidth() - margin;
			string[] array = Regex.Split(this.text, "\\s+");
			float num3 = 0f;
			float num4 = this.font.StringWidth(" ");
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string str = array2[i];
				float num5 = this.font.StringWidth(str);
				if (num3 + num4 + num5 < num2)
				{
					num3 += num4 + num5;
				}
				else
				{
					num3 = num5;
					num++;
				}
			}
			return num;
		}
		public override float GetComputedHeight(float xmargin)
		{
			if (this.wordwrap && base.GetColSpan() <= 1)
			{
				return (float)this.GetNumVerCells(xmargin) * this.font.body_height;
			}
			return this.font.body_height;
		}
		public bool IsWordwrap()
		{
			return this.wordwrap;
		}
		public void SetWordwrap(bool wordwrap)
		{
			this.wordwrap = wordwrap;
		}
	}
}
