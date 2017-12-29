using System;
namespace PDFjet.NET
{
	public class BitBuffer
	{
		private byte[] buffer;
		private int length;
		private int increments = 32;
		public BitBuffer()
		{
			this.buffer = new byte[this.increments];
			this.length = 0;
		}
		public byte[] GetBuffer()
		{
			return this.buffer;
		}
		public int GetLengthInBits()
		{
			return this.length;
		}
		public void Put(int num, int length)
		{
			for (int i = 0; i < length; i++)
			{
				this.Put(((uint)num >> length - i - 1 & 1u) == 1u);
			}
		}
		public void Put(bool bit)
		{
			if (this.length == this.buffer.Length * 8)
			{
				byte[] destinationArray = new byte[this.buffer.Length + this.increments];
				Array.Copy(this.buffer, 0, destinationArray, 0, this.buffer.Length);
				this.buffer = destinationArray;
			}
			if (bit)
			{
				byte[] expr_5A_cp_0 = this.buffer;
				int expr_5A_cp_1 = this.length / 8;
				expr_5A_cp_0[expr_5A_cp_1] |= (byte)(128u >> this.length % 8);
			}
			this.length++;
		}
	}
}
