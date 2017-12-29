using System;
namespace ecoProtocols
{
	public class BufferState
	{
		public object _ctx;
		public byte[] _buffer;
		public BufferState()
		{
			this._ctx = null;
			this._buffer = null;
		}
	}
}
