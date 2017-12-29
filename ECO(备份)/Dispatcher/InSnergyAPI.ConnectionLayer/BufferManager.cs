using System;
using System.Collections.Generic;
using System.Net.Sockets;
namespace InSnergyAPI.ConnectionLayer
{
	internal class BufferManager
	{
		private int totalBytesInBufferBlock;
		private byte[] bufferBlock;
		private int bufferSize;
		private int maxIndexCount;
		private Stack<int> freeIndexes;
		public BufferManager(int totalBytes, int nBufferSize)
		{
			this.totalBytesInBufferBlock = totalBytes;
			this.bufferSize = nBufferSize;
			this.maxIndexCount = totalBytes / nBufferSize;
			this.freeIndexes = new Stack<int>();
		}
		internal void InitBuffer()
		{
			int num = 0;
			this.bufferBlock = new byte[this.totalBytesInBufferBlock];
			for (int i = 0; i < this.maxIndexCount; i++)
			{
				this.freeIndexes.Push(num);
				num += this.bufferSize;
			}
		}
		internal bool SetBuffer(SocketAsyncEventArgs args)
		{
			if (this.freeIndexes.Count <= 0)
			{
				return false;
			}
			int offset = this.freeIndexes.Pop();
			args.SetBuffer(this.bufferBlock, offset, this.bufferSize);
			return true;
		}
		internal void FreeBuffer(SocketAsyncEventArgs args)
		{
			this.freeIndexes.Push(args.Offset);
			args.SetBuffer(null, 0, 0);
		}
	}
}
