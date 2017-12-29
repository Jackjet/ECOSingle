using System;
using System.Collections.Generic;
using System.IO;
namespace Packing
{
	public class MemoryTributary : Stream
	{
		protected long length;
		protected long blockSize = 65536L;
		protected List<byte[]> blocks = new List<byte[]>();
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}
		public override long Length
		{
			get
			{
				return this.length;
			}
		}
		public override long Position
		{
			get;
			set;
		}
		protected byte[] block
		{
			get
			{
				while ((long)this.blocks.Count <= this.blockId)
				{
					this.blocks.Add(new byte[this.blockSize]);
				}
				return this.blocks[(int)this.blockId];
			}
		}
		protected long blockId
		{
			get
			{
				return this.Position / this.blockSize;
			}
		}
		protected long blockOffset
		{
			get
			{
				return this.Position % this.blockSize;
			}
		}
		public MemoryTributary()
		{
			this.Position = 0L;
		}
		public MemoryTributary(byte[] source)
		{
			this.Write(source, 0, source.Length);
			this.Position = 0L;
		}
		public MemoryTributary(int length)
		{
			this.SetLength((long)length);
			this.Position = (long)length;
			byte[] arg_33_0 = this.block;
			this.Position = 0L;
		}
		public override void Flush()
		{
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			long num = (long)count;
			if (num < 0L)
			{
				throw new ArgumentOutOfRangeException("count", num, "Number of bytes to copy cannot be negative.");
			}
			long num2 = this.length - this.Position;
			if (num > num2)
			{
				num = num2;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "Destination offset cannot be negative.");
			}
			int num3 = 0;
			do
			{
				long num4 = Math.Min(num, this.blockSize - this.blockOffset);
				Buffer.BlockCopy(this.block, (int)this.blockOffset, buffer, offset, (int)num4);
				num -= num4;
				offset += (int)num4;
				num3 += (int)num4;
				this.Position += num4;
			}
			while (num > 0L);
			return num3;
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				this.Position = offset;
				break;
			case SeekOrigin.Current:
				this.Position += offset;
				break;
			case SeekOrigin.End:
				this.Position = this.Length - offset;
				break;
			}
			return this.Position;
		}
		public override void SetLength(long value)
		{
			this.length = value;
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			long position = this.Position;
			try
			{
				do
				{
					int num = Math.Min(count, (int)(this.blockSize - this.blockOffset));
					this.EnsureCapacity(this.Position + (long)num);
					Buffer.BlockCopy(buffer, offset, this.block, (int)this.blockOffset, num);
					count -= num;
					offset += num;
					this.Position += (long)num;
				}
				while (count > 0);
			}
			catch (Exception ex)
			{
				this.Position = position;
				throw ex;
			}
		}
		public override int ReadByte()
		{
			if (this.Position >= this.length)
			{
				return -1;
			}
			byte result = this.block[(int)checked((IntPtr)this.blockOffset)];
			this.Position += 1L;
			return (int)result;
		}
		public override void WriteByte(byte value)
		{
			this.EnsureCapacity(this.Position + 1L);
			this.block[(int)checked((IntPtr)this.blockOffset)] = value;
			this.Position += 1L;
		}
		protected void EnsureCapacity(long intended_length)
		{
			if (intended_length > this.length)
			{
				this.length = intended_length;
			}
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		public byte[] ToArray()
		{
			long position = this.Position;
			this.Position = 0L;
			byte[] array = new byte[this.Length];
			this.Read(array, 0, (int)this.Length);
			this.Position = position;
			return array;
		}
		public void ReadFrom(Stream source, long length)
		{
			byte[] buffer = new byte[4096];
			do
			{
				int num = source.Read(buffer, 0, (int)Math.Min(4096L, length));
				length -= (long)num;
				this.Write(buffer, 0, num);
			}
			while (length > 0L);
		}
		public void WriteTo(Stream destination)
		{
			long position = this.Position;
			this.Position = 0L;
			base.CopyTo(destination);
			this.Position = position;
		}
	}
}
