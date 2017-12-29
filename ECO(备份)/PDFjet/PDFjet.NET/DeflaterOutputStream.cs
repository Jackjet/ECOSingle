using System;
using System.IO;
using System.IO.Compression;
namespace PDFjet.NET
{
	public class DeflaterOutputStream
	{
		private const uint prime = 65521u;
		private MemoryStream buf1;
		private MemoryStream buf2;
		private DeflateStream ds1;
		public DeflaterOutputStream(MemoryStream buf1)
		{
			this.buf1 = buf1;
			this.buf2 = new MemoryStream();
			this.buf2.WriteByte(88);
			this.buf2.WriteByte(133);
			this.ds1 = new DeflateStream(this.buf2, CompressionMode.Compress, true);
		}
		public void Write(byte[] buffer, int off, int len)
		{
			this.ds1.Write(buffer, off, len);
			this.ds1.Dispose();
			this.buf2.WriteTo(this.buf1);
			ulong num = 1uL;
			ulong num2 = 0uL;
			for (int i = 0; i < len; i++)
			{
				num = (num + (ulong)buffer[off + i]) % 65521uL;
				num2 = (num2 + num) % 65521uL;
			}
			this.appendAdler((num2 << 16) + num);
		}
		public void Finish()
		{
		}
		private void appendAdler(ulong adler)
		{
			this.buf1.WriteByte((byte)(adler >> 24));
			this.buf1.WriteByte((byte)(adler >> 16));
			this.buf1.WriteByte((byte)(adler >> 8));
			this.buf1.WriteByte((byte)adler);
			this.buf1.Flush();
		}
	}
}
