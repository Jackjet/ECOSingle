using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace PDFjet.NET
{
	public class TextBox : IDrawable
	{
		protected Font font;
		protected string text;
		protected float x;
		protected float y;
		protected float width = 300f;
		protected float height = 200f;
		protected float spacing = 3f;
		protected float margin = 1f;
		private float lineWidth;
		private int background = 16777215;
		private int pen;
		private int brush;
		private int valign;
		private Font fallbackFont;
		private Dictionary<string, int> colors;
		private int properties = 983041;
		public TextBox(Font font)
		{
			this.font = font;
		}
		public TextBox(Font font, string text, double width, double height) : this(font, text, (float)width, (float)height)
		{
		}
		public TextBox(Font font, string text, float width, float height)
		{
			this.font = font;
			this.text = text;
			this.width = width;
			this.height = height;
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
			this.x = x;
			this.y = y;
		}
		public float GetX()
		{
			return this.x;
		}
		public float GetY()
		{
			return this.y;
		}
		public void SetWidth(double width)
		{
			this.width = (float)width;
		}
		public void SetWidth(float width)
		{
			this.width = width;
		}
		public float GetWidth()
		{
			return this.width;
		}
		public void SetHeight(double height)
		{
			this.height = (float)height;
		}
		public void SetHeight(float height)
		{
			this.height = height;
		}
		public float GetHeight()
		{
			return this.height;
		}
		public void SetMargin(double margin)
		{
			this.margin = (float)margin;
		}
		public void SetMargin(float margin)
		{
			this.margin = margin;
		}
		public float GetMargin()
		{
			return this.margin;
		}
		public void SetLineWidth(double lineWidth)
		{
			this.lineWidth = (float)lineWidth;
		}
		public void SetLineWidth(float lineWidth)
		{
			this.lineWidth = lineWidth;
		}
		public float GetLineWidth()
		{
			return this.lineWidth;
		}
		public void SetSpacing(double spacing)
		{
			this.spacing = (float)spacing;
		}
		public void SetSpacing(float spacing)
		{
			this.spacing = spacing;
		}
		public float GetSpacing()
		{
			return this.spacing;
		}
		public void SetBgColor(int color)
		{
			this.background = color;
		}
		public void SetBgColor(int[] color)
		{
			this.background = (color[0] << 16 | color[1] << 8 | color[2]);
		}
		public void SetBgColor(double[] color)
		{
			this.SetBgColor(new int[]
			{
				(int)color[0],
				(int)color[1],
				(int)color[2]
			});
		}
		public int GetBgColor()
		{
			return this.background;
		}
		public void SetFgColor(int color)
		{
			this.pen = color;
			this.brush = color;
		}
		public void SetFgColor(int[] color)
		{
			this.pen = (color[0] << 16 | color[1] << 8 | color[2]);
			this.brush = this.pen;
		}
		public void SetFgColor(double[] color)
		{
			this.SetPenColor(new int[]
			{
				(int)color[0],
				(int)color[1],
				(int)color[2]
			});
			this.SetBrushColor(this.pen);
		}
		public void SetPenColor(int color)
		{
			this.pen = color;
		}
		public void SetPenColor(int[] color)
		{
			this.pen = (color[0] << 16 | color[1] << 8 | color[2]);
		}
		public void SetPenColor(double[] color)
		{
			this.SetPenColor(new int[]
			{
				(int)color[0],
				(int)color[1],
				(int)color[2]
			});
		}
		public int GetPenColor()
		{
			return this.pen;
		}
		public void SetBrushColor(int color)
		{
			this.brush = color;
		}
		public void SetBrushColor(int[] color)
		{
			this.brush = (color[0] << 16 | color[1] << 8 | color[2]);
		}
		public void SetBrushColor(double[] color)
		{
			this.SetBrushColor(new int[]
			{
				(int)color[0],
				(int)color[1],
				(int)color[2]
			});
		}
		public int GetBrushColor()
		{
			return this.brush;
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
		public void SetFallbackFont(Font font)
		{
			this.fallbackFont = font;
		}
		public void SetVerticalAlignment(int alignment)
		{
			this.valign = alignment;
		}
		public int GetVerticalAlignment()
		{
			return this.valign;
		}
		public void SetTextColors(Dictionary<string, int> colors)
		{
			this.colors = colors;
		}
		public void DrawOn(Page page)
		{
			if (this.GetBgColor() != 16777215)
			{
				this.DrawBackground(page);
			}
			this.DrawBorders(page);
			this.DrawText(page);
		}
		private void DrawBackground(Page page)
		{
			page.SetBrushColor(this.background);
			page.FillRect((double)this.x, (double)this.y, (double)this.width, (double)this.height);
		}
		private void DrawBorders(Page page)
		{
			page.SetPenColor(this.pen);
			page.SetPenWidth(this.lineWidth);
			if (this.GetBorder(65536) && this.GetBorder(131072) && this.GetBorder(262144) && this.GetBorder(524288))
			{
				page.DrawRect((double)this.x, (double)this.y, (double)this.width, (double)this.height);
				return;
			}
			if (this.GetBorder(65536))
			{
				page.MoveTo(this.x, this.y);
				page.LineTo(this.x + this.width, this.y);
				page.StrokePath();
			}
			if (this.GetBorder(131072))
			{
				page.MoveTo(this.x, this.y + this.height);
				page.LineTo(this.x + this.width, this.y + this.height);
				page.StrokePath();
			}
			if (this.GetBorder(262144))
			{
				page.MoveTo(this.x, this.y);
				page.LineTo(this.x, this.y + this.height);
				page.StrokePath();
			}
			if (this.GetBorder(524288))
			{
				page.MoveTo(this.x + this.width, this.y);
				page.LineTo(this.x + this.width, this.y + this.height);
				page.StrokePath();
			}
		}
		private void DrawText(Page page)
		{
			page.SetPenColor(this.pen);
			page.SetBrushColor(this.brush);
			page.SetPenWidth(this.font.underlineThickness);
			float num = this.width - 2f * this.margin;
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = Regex.Split(this.text, "\n");
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (this.font.StringWidth(text) < num)
				{
					list.Add(text);
				}
				else
				{
					stringBuilder.Length = 0;
					string[] array3 = Regex.Split(text, "\\s+");
					string[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						string text2 = array4[j];
						if (this.font.StringWidth(stringBuilder.ToString() + " " + text2) < num)
						{
							stringBuilder.Append(" " + text2);
						}
						else
						{
							list.Add(stringBuilder.ToString().Trim());
							stringBuilder.Length = 0;
							stringBuilder.Append(text2);
						}
					}
					if (!stringBuilder.ToString().Trim().Equals(""))
					{
						list.Add(stringBuilder.ToString().Trim());
					}
				}
			}
			array = list.ToArray();
			float num2 = this.font.GetBodyHeight() + this.spacing;
			float num3 = this.y + this.font.ascent + this.margin;
			if (this.valign == 1)
			{
				num3 += this.height - (float)array.Length * num2;
			}
			else
			{
				if (this.valign == 2)
				{
					num3 += (this.height - (float)array.Length * num2) / 2f;
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				float num4;
				if (this.GetTextAlignment() == 2097152)
				{
					num4 = this.x + this.width - (this.font.StringWidth(array[k]) + this.margin);
				}
				else
				{
					if (this.GetTextAlignment() == 1048576)
					{
						num4 = this.x + (this.width - this.font.StringWidth(array[k])) / 2f;
					}
					else
					{
						num4 = this.x + this.margin;
					}
				}
				if (num3 + this.font.GetBodyHeight() + this.spacing + this.font.GetDescent() >= this.y + this.height && k < array.Length - 1)
				{
					string text3 = array[k];
					int num5 = text3.LastIndexOf(' ');
					if (num5 != -1)
					{
						array[k] = text3.Substring(0, num5) + " ...";
					}
					else
					{
						array[k] = text3 + " ...";
					}
				}
				if (num3 + this.font.GetDescent() < this.y + this.height)
				{
					if (this.fallbackFont == null)
					{
						if (this.colors == null)
						{
							page.DrawString(this.font, array[k], num4, num3);
						}
						else
						{
							page.DrawString(this.font, array[k], num4, num3, this.colors);
						}
					}
					else
					{
						page.DrawString(this.font, this.fallbackFont, array[k], num4, num3);
					}
					float num6 = this.font.StringWidth(array[k]);
					if (this.GetUnderline())
					{
						float underlinePosition = this.font.underlinePosition;
						page.MoveTo(num4, num3 + underlinePosition);
						page.LineTo(num4 + num6, num3 + underlinePosition);
						page.StrokePath();
					}
					if (this.GetStrikeout())
					{
						float num7 = this.font.body_height / 4f;
						page.MoveTo(num4, num3 - num7);
						page.LineTo(num4 + num6, num3 - num7);
						page.StrokePath();
					}
					num3 += this.font.GetBodyHeight() + this.spacing;
				}
			}
		}
	}
}
