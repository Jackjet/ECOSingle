using System;
namespace PDFjet.NET
{
	internal class State
	{
		private float[] pen;
		private float[] brush;
		private float pen_width;
		private int line_cap_style;
		private int line_join_style;
		private string linePattern;
		public State(float[] pen, float[] brush, float pen_width, int line_cap_style, int line_join_style, string linePattern)
		{
			this.pen = new float[]
			{
				pen[0],
				pen[1],
				pen[2]
			};
			this.brush = new float[]
			{
				brush[0],
				brush[1],
				brush[2]
			};
			this.pen_width = pen_width;
			this.line_cap_style = line_cap_style;
			this.line_join_style = line_join_style;
			this.linePattern = linePattern;
		}
		public float[] GetPen()
		{
			return this.pen;
		}
		public float[] GetBrush()
		{
			return this.brush;
		}
		public float GetPenWidth()
		{
			return this.pen_width;
		}
		public int GetLineCapStyle()
		{
			return this.line_cap_style;
		}
		public int GetLineJoinStyle()
		{
			return this.line_join_style;
		}
		public string GetLinePattern()
		{
			return this.linePattern;
		}
	}
}
