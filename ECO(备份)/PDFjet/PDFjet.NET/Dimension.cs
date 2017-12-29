using System;
namespace PDFjet.NET
{
	public class Dimension
	{
		internal float w;
		internal float h;
		public Dimension(float width, float height)
		{
			this.w = width;
			this.h = height;
		}
		public float GetWidth()
		{
			return this.w;
		}
		public float GetHeight()
		{
			return this.h;
		}
	}
}
