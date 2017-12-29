using System;
namespace PDFjet.NET
{
	public abstract class AbstractCell
	{
		internal Point point;
		internal float width = 70f;
		internal float height;
		internal float lineWidth;
		internal int background = 16777215;
		internal int pen;
		internal int brush;
		internal int properties = 983041;
		public int GetColSpan()
		{
			return this.properties & 65535;
		}
		public float GetWidth()
		{
			return this.width;
		}
		public abstract void ComputeWidth();
		public void SetWidth(float width)
		{
			this.width = width;
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
		public void SetTextAlignment(int alignment)
		{
			this.properties &= 13631487;
			this.properties |= (alignment & 3145728);
		}
		public void SetBrushColor(int color)
		{
			this.brush = color;
		}
		public Point GetPoint()
		{
			return this.point;
		}
		public float GetLineWidth()
		{
			return this.lineWidth;
		}
		public float GetHeight()
		{
			return this.height;
		}
		public int GetBgColor()
		{
			return this.background;
		}
		public int GetPenColor()
		{
			return this.pen;
		}
		public int GetBrushColor()
		{
			return this.brush;
		}
		internal int GetProperties()
		{
			return this.properties;
		}
		public void SetNoBorders()
		{
			this.properties &= 15794175;
		}
		public void SetPenColor(int color)
		{
			this.pen = color;
		}
		public void SetLineWidth(float lineWidth)
		{
			this.lineWidth = lineWidth;
		}
		internal abstract void Paint(Page page, float x, float y, float cell_w, float cell_h, float margin);
		public abstract float GetComputedHeight(float xmargin);
		public void SetPoint(Point point)
		{
			this.point = point;
		}
		public void SetWidth(double width)
		{
			this.width = (float)width;
		}
		public void SetHeight(double height)
		{
			this.height = (float)height;
		}
		public void SetHeight(float height)
		{
			this.height = height;
		}
		public void SetLineWidth(double lineWidth)
		{
			this.lineWidth = (float)lineWidth;
		}
		public void SetBgColor(int color)
		{
			this.background = color;
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
		public void SetColSpan(int colspan)
		{
			this.properties &= 16711680;
			this.properties |= (colspan & 65535);
		}
		public bool GetBorder(int border)
		{
			return (this.properties & border) != 0;
		}
		public int GetTextAlignment()
		{
			return this.properties & 3145728;
		}
	}
}
