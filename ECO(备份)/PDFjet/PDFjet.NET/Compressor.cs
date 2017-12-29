using System;
using System.IO;
namespace PDFjet.NET
{
	public class Compressor
	{
		private MemoryStream buf1;
		private DeflaterOutputStream dos1;
		public Compressor(byte[] image)
		{
			this.buf1 = new MemoryStream();
			this.dos1 = new DeflaterOutputStream(this.buf1);
			this.dos1.Write(image, 0, image.Length);
			this.dos1.Finish();
		}
		public byte[] GetCompressedData()
		{
			return this.buf1.ToArray();
		}
	}
}
