using System;
using System.IO;
namespace PDFjet.NET
{
	public class Image : IDrawable
	{
		internal int objNumber;
		internal float x;
		internal float y;
		internal float w;
		internal float h;
		internal long size;
		internal string uri;
		internal string key;
		private float box_x;
		private float box_y;
		private bool rotate90;
		public Image(PDF pdf, JPGImage jpg)
		{
			this.w = (float)jpg.GetWidth();
			this.h = (float)jpg.GetHeight();
			this.size = jpg.GetFileSize();
			Stream inputStream = jpg.GetInputStream();
			if (jpg.GetColorComponents() == 1)
			{
				this.AddImage(pdf, inputStream, ImageType.JPG, this.w, this.h, this.size, "DeviceGray", 8);
			}
			else
			{
				if (jpg.GetColorComponents() == 3)
				{
					this.AddImage(pdf, inputStream, ImageType.JPG, this.w, this.h, this.size, "DeviceRGB", 8);
				}
				else
				{
					if (jpg.GetColorComponents() == 4)
					{
						this.AddImage(pdf, inputStream, ImageType.JPG, this.w, this.h, this.size, "DeviceCMYK", 8);
					}
				}
			}
			inputStream.Dispose();
		}
		public Image(PDF pdf, PDFImage raw)
		{
			this.w = (float)raw.GetWidth();
			this.h = (float)raw.GetHeight();
			this.size = raw.GetSize();
			Stream inputStream = raw.GetInputStream();
			if (raw.GetColorComponents() == 1)
			{
				this.AddImage(pdf, inputStream, ImageType.PDF, this.w, this.h, this.size, "DeviceGray", 8);
			}
			else
			{
				this.AddImage(pdf, inputStream, ImageType.PDF, this.w, this.h, this.size, "DeviceRGB", 8);
			}
			inputStream.Dispose();
		}
		public Image(PDF pdf, Stream inputStream, int imageType)
		{
			if (imageType == ImageType.JPG)
			{
				JPGImage jPGImage = new JPGImage(inputStream);
				byte[] data = jPGImage.GetData();
				this.w = (float)jPGImage.GetWidth();
				this.h = (float)jPGImage.GetHeight();
				if (jPGImage.GetColorComponents() == 1)
				{
					this.AddImage(pdf, data, null, imageType, "DeviceGray", 8);
				}
				else
				{
					if (jPGImage.GetColorComponents() == 3)
					{
						this.AddImage(pdf, data, null, imageType, "DeviceRGB", 8);
					}
					else
					{
						if (jPGImage.GetColorComponents() == 4)
						{
							this.AddImage(pdf, data, null, imageType, "DeviceCMYK", 8);
						}
					}
				}
			}
			else
			{
				if (imageType == ImageType.PNG)
				{
					PNGImage pNGImage = new PNGImage(inputStream);
					byte[] data = pNGImage.GetData();
					this.w = (float)pNGImage.GetWidth();
					this.h = (float)pNGImage.GetHeight();
					if (pNGImage.colorType == 0)
					{
						this.AddImage(pdf, data, null, imageType, "DeviceGray", (int)pNGImage.bitDepth);
					}
					else
					{
						if (pNGImage.bitDepth == 16)
						{
							this.AddImage(pdf, data, null, imageType, "DeviceRGB", 16);
						}
						else
						{
							this.AddImage(pdf, data, pNGImage.GetAlpha(), imageType, "DeviceRGB", 8);
						}
					}
				}
				else
				{
					if (imageType == ImageType.BMP)
					{
						BMPImage bMPImage = new BMPImage(inputStream);
						byte[] data = bMPImage.GetData();
						this.w = (float)bMPImage.GetWidth();
						this.h = (float)bMPImage.GetHeight();
						this.AddImage(pdf, data, null, imageType, "DeviceRGB", 8);
					}
				}
			}
			inputStream.Dispose();
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
		public void ScaleBy(double factor)
		{
			this.ScaleBy((float)factor, (float)factor);
		}
		public void ScaleBy(float factor)
		{
			this.ScaleBy(factor, factor);
		}
		public void ScaleBy(float widthFactor, float heightFactor)
		{
			this.w *= widthFactor;
			this.h *= heightFactor;
		}
		public void PlaceIn(Box box)
		{
			this.box_x = box.x;
			this.box_y = box.y;
		}
		public void SetURIAction(string uri)
		{
			this.uri = uri;
		}
		public void SetGoToAction(string key)
		{
			this.key = key;
		}
		public void SetRotateCW90(bool rotate90)
		{
			this.rotate90 = rotate90;
		}
		public void DrawOn(Page page)
		{
			this.x += this.box_x;
			this.y += this.box_y;
			page.Append("q\n");
			if (this.rotate90)
			{
				page.Append(this.h);
				page.Append(' ');
				page.Append(0.0);
				page.Append(' ');
				page.Append(0.0);
				page.Append(' ');
				page.Append(this.w);
				page.Append(' ');
				page.Append(this.x);
				page.Append(' ');
				page.Append(page.height - this.y);
				page.Append(" cm\n");
				page.Append("0.7071067811 -0.7071067811 0.7071067811 0.7071067811 0.0 0.0 cm\n");
				page.Append("0.7071067811 -0.7071067811 0.7071067811 0.7071067811 0.0 0.0 cm\n");
			}
			else
			{
				page.Append(this.w);
				page.Append(' ');
				page.Append(0.0);
				page.Append(' ');
				page.Append(0.0);
				page.Append(' ');
				page.Append(this.h);
				page.Append(' ');
				page.Append(this.x);
				page.Append(' ');
				page.Append(page.height - (this.y + this.h));
				page.Append(" cm\n");
			}
			page.Append("/Im");
			page.Append(this.objNumber);
			page.Append(" Do\n");
			page.Append("Q\n");
			if (this.uri != null || this.key != null)
			{
				page.annots.Add(new Annotation(this.uri, this.key, (double)this.x, (double)(page.height - this.y), (double)(this.x + this.w), (double)(page.height - (this.y + this.h))));
			}
		}
		public Image(PDF pdf, PDFobj obj)
		{
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /XObject\n");
			pdf.Append("/Subtype /Image\n");
			pdf.Append("/Filter ");
			pdf.Append(obj.GetValue(PDFobj.FILTER));
			pdf.Append('\n');
			pdf.Append("/Width ");
			pdf.Append(obj.GetValue(PDFobj.WIDTH));
			pdf.Append('\n');
			pdf.Append("/Height ");
			pdf.Append(obj.GetValue(PDFobj.HEIGHT));
			pdf.Append('\n');
			pdf.Append("/ColorSpace ");
			pdf.Append(obj.GetValue(PDFobj.COLORSPACE));
			pdf.Append('\n');
			pdf.Append("/BitsPerComponent ");
			pdf.Append(obj.GetValue(PDFobj.BITSPERCOMPONENT));
			pdf.Append('\n');
			pdf.Append("/Length ");
			pdf.Append(obj.stream.Length);
			pdf.Append('\n');
			pdf.Append(">>\n");
			pdf.Append("stream\n");
			pdf.Append(obj.stream, 0, obj.stream.Length);
			pdf.Append("\nendstream\n");
			pdf.Endobj();
			pdf.images.Add(this);
			this.objNumber = pdf.objNumber;
		}
		public float GetWidth()
		{
			return this.w;
		}
		public float GetHeight()
		{
			return this.h;
		}
		private void AddSoftMask(PDF pdf, byte[] data, string colorSpace, int bitsPerComponent)
		{
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /XObject\n");
			pdf.Append("/Subtype /Image\n");
			pdf.Append("/Filter /FlateDecode\n");
			pdf.Append("/Width ");
			pdf.Append((int)this.w);
			pdf.Append('\n');
			pdf.Append("/Height ");
			pdf.Append((int)this.h);
			pdf.Append('\n');
			pdf.Append("/ColorSpace /");
			pdf.Append(colorSpace);
			pdf.Append('\n');
			pdf.Append("/BitsPerComponent ");
			pdf.Append(bitsPerComponent);
			pdf.Append('\n');
			pdf.Append("/Length ");
			pdf.Append(data.Length);
			pdf.Append('\n');
			pdf.Append(">>\n");
			pdf.Append("stream\n");
			pdf.Append(data, 0, data.Length);
			pdf.Append("\nendstream\n");
			pdf.Endobj();
			this.objNumber = pdf.objNumber;
		}
		private void AddImage(PDF pdf, byte[] data, byte[] alpha, int imageType, string colorSpace, int bitsPerComponent)
		{
			if (alpha != null)
			{
				this.AddSoftMask(pdf, alpha, "DeviceGray", 8);
			}
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /XObject\n");
			pdf.Append("/Subtype /Image\n");
			if (imageType == ImageType.JPG)
			{
				pdf.Append("/Filter /DCTDecode\n");
			}
			else
			{
				if (imageType == ImageType.PNG || imageType == ImageType.BMP)
				{
					pdf.Append("/Filter /FlateDecode\n");
					if (alpha != null)
					{
						pdf.Append("/SMask ");
						pdf.Append(this.objNumber);
						pdf.Append(" 0 R\n");
					}
				}
			}
			pdf.Append("/Width ");
			pdf.Append((int)this.w);
			pdf.Append('\n');
			pdf.Append("/Height ");
			pdf.Append((int)this.h);
			pdf.Append('\n');
			pdf.Append("/ColorSpace /");
			pdf.Append(colorSpace);
			pdf.Append('\n');
			pdf.Append("/BitsPerComponent ");
			pdf.Append(bitsPerComponent);
			pdf.Append('\n');
			if (colorSpace.Equals("DeviceCMYK"))
			{
				pdf.Append("/Decode [1.0 0.0 1.0 0.0 1.0 0.0 1.0 0.0]\n");
			}
			pdf.Append("/Length ");
			pdf.Append(data.Length);
			pdf.Append('\n');
			pdf.Append(">>\n");
			pdf.Append("stream\n");
			pdf.Append(data, 0, data.Length);
			pdf.Append("\nendstream\n");
			pdf.Endobj();
			pdf.images.Add(this);
			this.objNumber = pdf.objNumber;
		}
		private void AddImage(PDF pdf, Stream stream, int imageType, float w, float h, long length, string colorSpace, int bitsPerComponent)
		{
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /XObject\n");
			pdf.Append("/Subtype /Image\n");
			if (imageType == ImageType.JPG)
			{
				pdf.Append("/Filter /DCTDecode\n");
			}
			else
			{
				if (imageType == ImageType.PDF)
				{
					pdf.Append("/Filter /FlateDecode\n");
				}
			}
			pdf.Append("/Width ");
			pdf.Append((int)w);
			pdf.Append('\n');
			pdf.Append("/Height ");
			pdf.Append((int)h);
			pdf.Append('\n');
			pdf.Append("/ColorSpace /");
			pdf.Append(colorSpace);
			pdf.Append('\n');
			pdf.Append("/BitsPerComponent ");
			pdf.Append(bitsPerComponent);
			pdf.Append('\n');
			if (colorSpace.Equals("DeviceCMYK"))
			{
				pdf.Append("/Decode [1.0 0.0 1.0 0.0 1.0 0.0 1.0 0.0]\n");
			}
			pdf.Append("/Length ");
			pdf.Append((int)length);
			pdf.Append('\n');
			pdf.Append(">>\n");
			pdf.Append("stream\n");
			byte[] array = new byte[2048];
			int len;
			while ((len = stream.Read(array, 0, array.Length)) != 0)
			{
				pdf.Append(array, 0, len);
			}
			pdf.Append("\nendstream\n");
			pdf.Endobj();
			pdf.images.Add(this);
			this.objNumber = pdf.objNumber;
		}
	}
}
