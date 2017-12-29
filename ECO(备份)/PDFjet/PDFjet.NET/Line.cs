using System;
namespace PDFjet.NET
{
	public class Line : IDrawable
	{
		private float x1;
		private float y1;
		private float x2;
		private float y2;
		private float box_x;
		private float box_y;
		private int color;
		private float width = 0.3f;
		private string pattern = "[] 0";
		private int capStyle;
		public Line()
		{
		}
		public Line(double x1, double y1, double x2, double y2) : this((float)x1, (float)y1, (float)x2, (float)y2)
		{
		}
		public Line(float x1, float y1, float x2, float y2)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}
		public void SetPattern(string pattern)
		{
			this.pattern = pattern;
		}
		public void SetStartPoint(double x, double y)
		{
			this.x1 = (float)x;
			this.y1 = (float)y;
		}
		public void SetStartPoint(float x, float y)
		{
			this.x1 = x;
			this.y1 = y;
		}
		public void SetPointA(float x, float y)
		{
			this.x1 = x;
			this.y1 = y;
		}
		public Point GetStartPoint()
		{
			return new Point(this.x1, this.y1);
		}
		public void SetEndPoint(double x, double y)
		{
			this.x2 = (float)x;
			this.y2 = (float)y;
		}
		public void SetEndPoint(float x, float y)
		{
			this.x2 = x;
			this.y2 = y;
		}
		public void SetPointB(float x, float y)
		{
			this.x2 = x;
			this.y2 = y;
		}
		public Point GetEndPoint()
		{
			return new Point(this.x2, this.y2);
		}
		public void SetWidth(double width)
		{
			this.width = (float)width;
		}
		public void SetWidth(float width)
		{
			this.width = width;
		}
		public void SetColor(int color)
		{
			this.color = color;
		}
		public void SetCapStyle(int style)
		{
			this.capStyle = style;
		}
		public int getCapStyle()
		{
			return this.capStyle;
		}
		public void PlaceIn(Box box)
		{
			this.PlaceIn(box, 0f, 0f);
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
		public void ScaleBy(double factor)
		{
			this.ScaleBy((float)factor);
		}
		public void ScaleBy(float factor)
		{
			this.x1 *= factor;
			this.x2 *= factor;
			this.y1 *= factor;
			this.y2 *= factor;
		}
		public void DrawOn(Page page)
		{
			page.SetPenColor(this.color);
			page.SetPenWidth(this.width);
			page.SetLineCapStyle(this.capStyle);
			page.SetLinePattern(this.pattern);
			page.DrawLine((double)(this.x1 + this.box_x), (double)(this.y1 + this.box_y), (double)(this.x2 + this.box_x), (double)(this.y2 + this.box_y));
		}
	}
}
