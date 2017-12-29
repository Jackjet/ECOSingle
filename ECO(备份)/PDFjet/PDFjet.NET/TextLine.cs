using System;
namespace PDFjet.NET
{
	public class TextLine : IDrawable
	{
		internal float x;
		internal float y;
		internal Font font;
		internal Font fallbackFont;
		internal string str;
		private string uri;
		private string key;
		private bool underline;
		private bool strikeout;
		private int degrees;
		private int color;
		private float box_x;
		private float box_y;
		private int textEffect;
		public TextLine(Font font)
		{
			this.font = font;
		}
		public TextLine(Font font, string text)
		{
			this.font = font;
			this.str = text;
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
		public void SetText(string text)
		{
			this.str = text;
		}
		public void SetFont(Font font)
		{
			this.font = font;
		}
		public Font GetFont()
		{
			return this.font;
		}
		public void SetFallbackFont(Font fallbackFont)
		{
			this.fallbackFont = fallbackFont;
		}
		public void SetColor(int color)
		{
			this.color = color;
		}
		public string GetText()
		{
			return this.str;
		}
		public double GetDestinationY()
		{
			return (double)(this.y - this.font.GetSize());
		}
		public float GetWidth()
		{
			if (this.fallbackFont == null)
			{
				return this.font.StringWidth(this.str);
			}
			return this.font.StringWidth(this.fallbackFont, this.str);
		}
		public double GetHeight()
		{
			return (double)this.font.GetHeight();
		}
		public int GetColor()
		{
			return this.color;
		}
		public void SetURIAction(string uri)
		{
			this.uri = uri;
		}
		public string GetURIAction()
		{
			return this.uri;
		}
		public void SetGoToAction(string key)
		{
			this.key = key;
		}
		public string GetGoToAction()
		{
			return this.key;
		}
		public void SetUnderline(bool underline)
		{
			this.underline = underline;
		}
		public bool GetUnderline()
		{
			return this.underline;
		}
		public void SetStrikeLine(bool strike)
		{
			this.strikeout = strike;
		}
		public void SetStrikeout(bool strike)
		{
			this.strikeout = strike;
		}
		public bool GetStrikeout()
		{
			return this.strikeout;
		}
		public void SetTextDirection(int degrees)
		{
			this.degrees = degrees;
		}
		public int GetTextEffect()
		{
			return this.textEffect;
		}
		public void SetTextEffect(int textEffect)
		{
			this.textEffect = textEffect;
		}
		public void PlaceIn(Box box)
		{
			this.PlaceIn(box, 0.0, 0.0);
		}
		public void PlaceIn(Box box, double x_offset, double y_offset)
		{
			this.PlaceIn(box, (float)x_offset, (float)y_offset);
		}
		public void PlaceIn(Box box, float x_offset, float y_offset)
		{
			this.box_x = box.x + x_offset;
			this.box_y = box.y + y_offset;
		}
		public void DrawOn(Page page)
		{
			this.DrawOn(page, true);
		}
		internal void DrawOn(Page page, bool draw)
		{
			if (page == null || !draw)
			{
				return;
			}
			page.SetTextDirection(this.degrees);
			this.x += this.box_x;
			this.y += this.box_y;
			if (this.uri != null || this.key != null)
			{
				page.annots.Add(new Annotation(this.uri, this.key, (double)this.x, (double)(page.height - (this.y - this.font.ascent)), (double)(this.x + this.font.StringWidth(this.str)), (double)(page.height - (this.y - this.font.descent))));
			}
			if (this.str != null)
			{
				page.SetBrushColor(this.color);
				if (this.fallbackFont == null)
				{
					page.DrawString(this.font, this.str, this.x, this.y);
				}
				else
				{
					page.DrawString(this.font, this.fallbackFont, this.str, this.x, this.y);
				}
			}
			if (this.underline)
			{
				page.SetPenWidth(this.font.underlineThickness);
				page.SetPenColor(this.color);
				double num = (double)this.font.StringWidth(this.str);
				double num2 = 3.1415926535897931 * (double)this.degrees / 180.0;
				double num3 = (double)this.font.underlinePosition * Math.Sin(num2);
				double num4 = (double)this.font.underlinePosition * Math.Cos(num2);
				double num5 = (double)this.x + num * Math.Cos(num2);
				double num6 = (double)this.y - num * Math.Sin(num2);
				page.MoveTo((double)this.x + num3, (double)this.y + num4);
				page.LineTo(num5 + num3, num6 + num4);
				page.StrokePath();
			}
			if (this.strikeout)
			{
				page.SetPenWidth(this.font.underlineThickness);
				page.SetPenColor(this.color);
				double num7 = (double)this.font.StringWidth(this.str);
				double num8 = 3.1415926535897931 * (double)this.degrees / 180.0;
				double num9 = (double)this.font.body_height / 4.0 * Math.Sin(num8);
				double num10 = (double)this.font.body_height / 4.0 * Math.Cos(num8);
				double num11 = (double)this.x + num7 * Math.Cos(num8);
				double num12 = (double)this.y - num7 * Math.Sin(num8);
				page.MoveTo((double)this.x - num9, (double)this.y - num10);
				page.LineTo(num11 - num9, num12 - num10);
				page.StrokePath();
			}
			page.SetTextDirection(0);
		}
	}
}
