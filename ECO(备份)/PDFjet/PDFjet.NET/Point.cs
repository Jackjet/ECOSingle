using System;
namespace PDFjet.NET
{
	public class Point : IDrawable
	{
		public static int INVISIBLE = -1;
		public static int CIRCLE = 0;
		public static int DIAMOND = 1;
		public static int BOX = 2;
		public static int PLUS = 3;
		public static int H_DASH = 4;
		public static int V_DASH = 5;
		public static int MULTIPLY = 6;
		public static int STAR = 7;
		public static int X_MARK = 8;
		public static int UP_ARROW = 9;
		public static int DOWN_ARROW = 10;
		public static int LEFT_ARROW = 11;
		public static int RIGHT_ARROW = 12;
		public static bool CONTROL_POINT = true;
		internal float x;
		internal float y;
		internal float r = 2f;
		internal int shape = Point.CIRCLE;
		internal int color;
		internal float lineWidth = 0.3f;
		internal string linePattern = "[] 0";
		internal bool fillShape;
		internal bool isControlPoint;
		private string text;
		private string uri;
		private bool drawLineTo;
		private float box_x;
		private float box_y;
		public Point()
		{
		}
		public Point(double x, double y) : this((float)x, (float)y)
		{
		}
		public Point(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		public Point(double x, double y, bool isControlPoint) : this((float)x, (float)y, isControlPoint)
		{
		}
		public Point(float x, float y, bool isControlPoint)
		{
			this.x = x;
			this.y = y;
			this.isControlPoint = isControlPoint;
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
		public void SetX(double x)
		{
			this.x = (float)x;
		}
		public void SetX(float x)
		{
			this.x = x;
		}
		public float GetX()
		{
			return this.x;
		}
		public void SetY(double y)
		{
			this.y = (float)y;
		}
		public void SetY(float y)
		{
			this.y = y;
		}
		public float GetY()
		{
			return this.y;
		}
		public void SetRadius(double r)
		{
			this.r = (float)r;
		}
		public void SetRadius(float r)
		{
			this.r = r;
		}
		public float GetRadius()
		{
			return this.r;
		}
		public void SetShape(int shape)
		{
			this.shape = shape;
		}
		public int GetShape()
		{
			return this.shape;
		}
		public void SetFillShape(bool fill)
		{
			this.fillShape = fill;
		}
		public bool GetFillShape()
		{
			return this.fillShape;
		}
		public void SetColor(int color)
		{
			this.color = color;
		}
		public int GetColor()
		{
			return this.color;
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
		public void SetLinePattern(string linePattern)
		{
			this.linePattern = linePattern;
		}
		public string GetLinePattern()
		{
			return this.linePattern;
		}
		public void SetDrawLineTo(bool drawLineTo)
		{
			this.drawLineTo = drawLineTo;
		}
		public bool GetDrawLineTo()
		{
			return this.drawLineTo;
		}
		public void SetURIAction(string uri)
		{
			this.uri = uri;
		}
		public string GetURIAction()
		{
			return this.uri;
		}
		public void SetText(string text)
		{
			this.text = text;
		}
		public string GetText()
		{
			return this.text;
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
		public void DrawOn(Page page)
		{
			page.SetPenWidth(this.lineWidth);
			page.SetLinePattern(this.linePattern);
			if (this.fillShape)
			{
				page.SetBrushColor(this.color);
			}
			else
			{
				page.SetPenColor(this.color);
			}
			this.x += this.box_x;
			this.y += this.box_y;
			page.DrawPoint(this);
			this.x -= this.box_x;
			this.y -= this.box_y;
		}
	}
}
