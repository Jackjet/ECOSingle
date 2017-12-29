using System;
using System.IO;
namespace PDFjet.NET
{
	public class PDFImage
	{
		private Stream stream;
		private long size;
		private int colorComponents;
		private int w;
		private int h;
		private int bitsPerComponent;
		public PDFImage(string path, Stream stream, long size)
		{
			string text = path.Substring(path.LastIndexOf("/") + 1);
			string[] array = text.Split(new char[]
			{
				'.'
			});
			string[] array2 = array[2].Split(new char[]
			{
				'x'
			});
			this.stream = stream;
			this.size = size;
			this.colorComponents = (array[1].Equals("rgb") ? 3 : 1);
			this.w = Convert.ToInt32(array2[0]);
			this.h = Convert.ToInt32(array2[1]);
			this.bitsPerComponent = Convert.ToInt32(array2[2]);
		}
		internal Stream GetInputStream()
		{
			return this.stream;
		}
		internal int GetWidth()
		{
			return this.w;
		}
		internal int GetHeight()
		{
			return this.h;
		}
		internal long GetSize()
		{
			return this.size;
		}
		internal int GetColorComponents()
		{
			return this.colorComponents;
		}
		internal int GetBitsPerComponent()
		{
			return this.bitsPerComponent;
		}
	}
}
