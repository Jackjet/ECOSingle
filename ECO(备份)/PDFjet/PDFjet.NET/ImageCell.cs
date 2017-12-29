using System;
namespace PDFjet.NET
{
	public class ImageCell : AbstractCell
	{
		private Image image;
		private bool keepAspect = true;
		private bool increaseRowSize = true;
		private int maxWidth = 200;
		public ImageCell(Image image)
		{
			this.image = image;
		}
		public ImageCell(Image image, int maxWidth)
		{
			this.image = image;
			this.maxWidth = maxWidth;
		}
		public ImageCell(Image image, bool keepAspect, bool increaseRowSize, int maxWidth)
		{
			this.image = image;
			this.keepAspect = keepAspect;
			this.increaseRowSize = increaseRowSize;
			this.maxWidth = maxWidth;
		}
		public override void ComputeWidth()
		{
			if (this.increaseRowSize)
			{
				base.SetWidth(Math.Min((float)this.maxWidth, this.image.GetWidth()));
			}
		}
		internal override void Paint(Page page, float x, float y, float w, float h, float margin)
		{
			this.image.SetPosition(x + margin, y + margin);
			if (this.keepAspect)
			{
				float num = Math.Min((w - margin * 2f) / this.image.GetWidth(), (h - margin * 2f) / this.image.GetHeight());
				this.image.ScaleBy(num, num);
			}
			else
			{
				this.image.w = w - margin * 2f;
				this.image.h = h - margin * 2f;
			}
			this.image.DrawOn(page);
		}
		public override float GetComputedHeight(float xmargin)
		{
			if (this.increaseRowSize)
			{
				return this.image.GetHeight() * ((base.GetWidth() - xmargin) / this.image.GetWidth());
			}
			return 0f;
		}
	}
}
