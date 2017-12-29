using System;
namespace PDFjet.NET
{
	public class Box : IDrawable
	{
		internal float x;
		internal float y;
		private float w;
		private float h;
		private int color;
		private float width = 0.3f;
		private string pattern = "[] 0";
		private bool fill_shape;
		internal string uri;
		internal string key;
		public Box()
		{
		}
		public Box(double x, double y, double w, double h) : this((float)x, (float)y, (float)w, (float)h)
		{
		}
		public Box(float x, float y, float w, float h)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
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
		public void SetSize(double w, double h)
		{
			this.SetSize((float)w, (float)h);
		}
		public void SetSize(float w, float h)
		{
			this.w = w;
			this.h = h;
		}
		public void SetColor(int color)
		{
			this.color = color;
		}
		public void SetLineWidth(double width)
		{
			this.width = (float)width;
		}
		public void SetLineWidth(float width)
		{
			this.width = width;
		}
		public void SetURIAction(string uri)
		{
			this.uri = uri;
		}
		public void SetGoToAction(string key)
		{
			this.key = key;
		}
		public void SetPattern(string pattern)
		{
			this.pattern = pattern;
		}
		public void SetFillShape(bool fill_shape)
		{
			this.fill_shape = fill_shape;
		}
		public void PlaceIn(Box box, double x_offset, double y_offset)
		{
			this.PlaceIn(box, (float)x_offset, (float)y_offset);
		}
		public void PlaceIn(Box box, float x_offset, float y_offset)
		{
			this.x = box.x + x_offset;
			this.y = box.y + y_offset;
		}
		public void ScaleBy(double factor)
		{
			this.ScaleBy((float)factor);
		}
		public void ScaleBy(float factor)
		{
			this.x *= factor;
			this.y *= factor;
		}
		public void DrawOn(Page page)
		{
			page.SetPenWidth(this.width);
			page.SetLinePattern(this.pattern);
			page.MoveTo(this.x, this.y);
			page.LineTo(this.x + this.w, this.y);
			page.LineTo(this.x + this.w, this.y + this.h);
			page.LineTo(this.x, this.y + this.h);
			if (this.fill_shape)
			{
				page.SetBrushColor(this.color);
				page.FillPath();
			}
			else
			{
				page.SetPenColor(this.color);
				page.ClosePath();
			}
			if (this.uri != null || this.key != null)
			{
				page.annots.Add(new Annotation(this.uri, this.key, (double)this.x, (double)(page.height - this.y), (double)(this.x + this.w), (double)(page.height - (this.y + this.h))));
			}
		}
	}
}
