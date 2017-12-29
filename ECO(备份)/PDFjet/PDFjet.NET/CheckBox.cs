using System;
namespace PDFjet.NET
{
	public class CheckBox
	{
		private bool boxChecked = true;
		private float x;
		private float y;
		private float w = 12f;
		private float h = 12f;
		private int checkColor = 255;
		private int boxColor;
		private float penWidth = 0.3f;
		private float checkWidth = 3f;
		private int mark = 1;
		private Font font;
		private string text;
		public CheckBox()
		{
		}
		public CheckBox(Font font, string text)
		{
			this.font = font;
			this.text = text;
			this.boxChecked = false;
		}
		public CheckBox(bool boxChecked, int checkColor)
		{
			this.boxChecked = boxChecked;
			this.checkColor = checkColor;
		}
		public CheckBox(bool boxChecked)
		{
			this.boxChecked = boxChecked;
		}
		public void SetCheckColor(int checkColor)
		{
			this.checkColor = checkColor;
		}
		public void SetBoxColor(int boxColor)
		{
			this.boxColor = boxColor;
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
		public void SetSize(double size)
		{
			this.SetSize((float)size);
		}
		public void SetSize(float size)
		{
			this.h = size;
			this.w = size;
			this.checkWidth = size / 4f;
			this.penWidth = size / 40f;
		}
		public void SetMarkType(int mark)
		{
			if (mark > 0 && mark < 3)
			{
				this.mark = mark;
			}
		}
		public float GetHeight()
		{
			return this.h;
		}
		public float GetWidth()
		{
			return this.w;
		}
		public float GetXPosition()
		{
			return this.x;
		}
		public float GetYPosition()
		{
			return this.y;
		}
		public void SetChecked(bool boxChecked)
		{
			this.boxChecked = boxChecked;
		}
		public void DrawOn(Page page)
		{
			page.SetPenWidth(this.penWidth);
			page.MoveTo(this.x, this.y);
			page.LineTo(this.x + this.w, this.y);
			page.LineTo(this.x + this.w, this.y + this.h);
			page.LineTo(this.x, this.y + this.h);
			page.ClosePath();
			page.SetPenColor(this.boxColor);
			page.StrokePath();
			if (this.boxChecked)
			{
				page.SetPenWidth(this.checkWidth);
				if (this.mark == 1)
				{
					page.MoveTo(this.x + this.checkWidth / 2f, this.y + this.h / 2f);
					page.LineTo(this.x + this.w / 3f, this.y + this.h - this.checkWidth / 2f);
					page.LineTo(this.x + this.w - this.checkWidth / 2f, this.y + this.checkWidth / 2f);
				}
				else
				{
					page.MoveTo(this.x + this.checkWidth / 2f, this.y + this.checkWidth / 2f);
					page.LineTo(this.x + this.w - this.checkWidth / 2f, this.y + this.h - this.checkWidth / 2f);
					page.MoveTo(this.x + this.w - this.checkWidth / 2f, this.y + this.checkWidth / 2f);
					page.LineTo(this.x + this.checkWidth / 2f, this.y + this.h - this.checkWidth / 2f);
				}
				page.SetPenColor(this.checkColor);
				page.SetLineCapStyle(Cap.ROUND);
				page.StrokePath();
			}
			if (this.font != null && this.text != null)
			{
				page.DrawString(this.font, this.text, this.x + 5f * this.w / 4f, this.y + this.h);
			}
			page.SetPenWidth(0f);
			page.SetPenColor(0);
		}
	}
}
