using System;
using System.IO;
using System.IO.Compression;
namespace PDFjet.NET
{
	public class Decompressor
	{
		private MemoryStream outStream;
		private MemoryStream inStream;
		public Decompressor(byte[] data)
		{
			this.outStream = new MemoryStream();
			this.inStream = new MemoryStream(data, 2, data.Length - 6);
			DeflateStream deflateStream = new DeflateStream(this.inStream, CompressionMode.Decompress, true);
			byte[] array = new byte[4096];
			int count;
			while ((count = deflateStream.Read(array, 0, array.Length)) != 0)
			{
				this.outStream.Write(array, 0, count);
			}
		}
		public byte[] getDecompressedData()
		{
			return this.outStream.ToArray();
		}
	}
}
