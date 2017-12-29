using System;
namespace PDFjet.NET
{
	internal class Chunk
	{
		private long chunkLength;
		internal byte[] type;
		private byte[] data;
		private long crc;
		public long GetLength()
		{
			return this.chunkLength;
		}
		public void SetLength(long chunkLength)
		{
			this.chunkLength = chunkLength;
		}
		public void SetType(byte[] type)
		{
			this.type = type;
		}
		public byte[] GetData()
		{
			return this.data;
		}
		public void SetData(byte[] data)
		{
			this.data = data;
		}
		public long GetCrc()
		{
			return this.crc;
		}
		public void SetCrc(long crc)
		{
			this.crc = crc;
		}
		public bool hasGoodCRC()
		{
			CRC32 cRC = new CRC32();
			cRC.Update(this.type, 0, 4);
			cRC.Update(this.data, 0, (int)this.chunkLength);
			return cRC.GetValue() == this.crc;
		}
	}
}
