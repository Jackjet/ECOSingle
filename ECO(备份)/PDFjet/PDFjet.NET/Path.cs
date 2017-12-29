using System;
using System.Collections.Generic;
namespace PDFjet.NET
{
	public class Path : IDrawable
	{
		private int color;
		private float width = 0.3f;
		private string pattern = "[] 0";
		private bool fill_shape;
		private bool close_path;
		private List<Point> points;
		private float box_x;
		private float box_y;
		private int lineCapStyle;
		private int lineJoinStyle;
		public Path()
		{
			this.points = new List<Point>();
		}
		public void Add(Point point)
		{
			this.points.Add(point);
		}
		public void SetPattern(string pattern)
		{
			this.pattern = pattern;
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
		public void SetClosePath(bool close_path)
		{
			this.close_path = close_path;
		}
		public void SetFillShape(bool fill_shape)
		{
			this.fill_shape = fill_shape;
		}
		public void SetLineCapStyle(int style)
		{
			this.lineCapStyle = style;
		}
		public int GetLineCapStyle()
		{
			return this.lineCapStyle;
		}
		public void SetLineJoinStyle(int style)
		{
			this.lineJoinStyle = style;
		}
		public int GetLineJoinStyle()
		{
			return this.lineJoinStyle;
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
			for (int i = 0; i < this.points.Count; i++)
			{
				Point point = this.points[i];
				point.x *= factor;
				point.y *= factor;
			}
		}
		public static List<Point> GetCurvePoints(float x, float y, float r1, float r2, int segment)
		{
			float num = 0.551784f;
			List<Point> list = new List<Point>();
			if (segment == 0)
			{
				list.Add(new Point(x, y - r2));
				list.Add(new Point(x + num * r1, y - r2, Point.CONTROL_POINT));
				list.Add(new Point(x + r1, y - num * r2, Point.CONTROL_POINT));
				list.Add(new Point(x + r1, y));
			}
			else
			{
				if (segment == 1)
				{
					list.Add(new Point(x + r1, y));
					list.Add(new Point(x + r1, y + num * r2, Point.CONTROL_POINT));
					list.Add(new Point(x + num * r1, y + r2, Point.CONTROL_POINT));
					list.Add(new Point(x, y + r2));
				}
				else
				{
					if (segment == 2)
					{
						list.Add(new Point(x, y + r2));
						list.Add(new Point(x - num * r1, y + r2, Point.CONTROL_POINT));
						list.Add(new Point(x - r1, y + num * r2, Point.CONTROL_POINT));
						list.Add(new Point(x - r1, y));
					}
					else
					{
						if (segment == 3)
						{
							list.Add(new Point(x - r1, y));
							list.Add(new Point(x - r1, y - num * r2, Point.CONTROL_POINT));
							list.Add(new Point(x - num * r1, y - r2, Point.CONTROL_POINT));
							list.Add(new Point(x, y - r2));
						}
					}
				}
			}
			return list;
		}
		public void DrawOn(Page page)
		{
			if (this.fill_shape)
			{
				page.SetBrushColor(this.color);
			}
			else
			{
				page.SetPenColor(this.color);
			}
			page.SetPenWidth(this.width);
			page.SetLinePattern(this.pattern);
			page.SetLineCapStyle(this.lineCapStyle);
			page.SetLineJoinStyle(this.lineJoinStyle);
			for (int i = 0; i < this.points.Count; i++)
			{
				Point point = this.points[i];
				point.x += this.box_x;
				point.y += this.box_y;
			}
			if (this.fill_shape)
			{
				page.DrawPath(this.points, 'f');
			}
			else
			{
				if (this.close_path)
				{
					page.DrawPath(this.points, 's');
				}
				else
				{
					page.DrawPath(this.points, 'S');
				}
			}
			for (int j = 0; j < this.points.Count; j++)
			{
				Point point2 = this.points[j];
				point2.x -= this.box_x;
				point2.y -= this.box_y;
			}
		}
	}
}
