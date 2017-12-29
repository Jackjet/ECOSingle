using System;
using System.Collections.Generic;
using System.Net.Sockets;
namespace ecoProtocols
{
	public class PacketReceiver
	{
		protected object _lockPacket = new object();
		public int _fsmReceive;
		public ulong _header;
		protected byte[] _headBytes;
		protected int _headerSize;
		protected byte[] _headerBuffer;
		protected int _receivedSize;
		public byte[] _receiveBuffer;
		public PacketReceiver()
		{
			this._header = 0uL;
			this._headerSize = 0;
			this._headerBuffer = new byte[8];
			this._receivedSize = 0;
			this._receiveBuffer = null;
		}
		public void Reset()
		{
			this._header = 0uL;
			this._headerSize = 0;
			this._receivedSize = 0;
			this._receiveBuffer = null;
		}
		public virtual List<SocketAsyncEventArgs> PacketProcess(byte[] payload)
		{
			return null;
		}
		public virtual List<byte[]> PacketReceived(byte[] payload)
		{
			return null;
		}
		public virtual List<byte[]> ReceivedBuffer(BufferState rState, int nSize, ref int nRequestCount)
		{
			List<byte[]> result;
			lock (this._lockPacket)
			{
				int i = 0;
				nRequestCount = 0;
				List<byte[]> list = new List<byte[]>();
				if (rState._buffer == null || nSize == 0)
				{
					result = list;
				}
				else
				{
					while (i < nSize)
					{
						if (this._fsmReceive == 0)
						{
							int num = Math.Min(8 - this._headerSize, nSize - i);
							Array.Copy(rState._buffer, i, this._headerBuffer, this._headerSize, num);
							this._headerSize += num;
							i += num;
							if (this._headerSize >= 8)
							{
								this._header = BitConverter.ToUInt64(this._headerBuffer, 0);
								this._fsmReceive = 1;
								this._receivedSize = 0;
								this._header = ecoServerProtocol.swap64(this._header);
								uint num2 = ecoServerProtocol.getPacketLength(this._header);
								if (num2 >= 2u)
								{
									num2 -= 2u;
								}
								if (num2 > 0u)
								{
									this._receiveBuffer = new byte[num2];
								}
								else
								{
									this._receiveBuffer = null;
								}
							}
						}
						if (this._fsmReceive == 1)
						{
							if (this._receiveBuffer != null && this._receivedSize < this._receiveBuffer.Length)
							{
								int num3 = Math.Min(this._receiveBuffer.Length - this._receivedSize, nSize - i);
								if (num3 > 0)
								{
									Array.Copy(rState._buffer, i, this._receiveBuffer, this._receivedSize, num3);
									this._receivedSize += num3;
									i += num3;
								}
							}
							if (this._receiveBuffer == null || this._receivedSize >= this._receiveBuffer.Length)
							{
								nRequestCount++;
								List<byte[]> list2 = this.PacketReceived(this._receiveBuffer);
								if (list2 != null && list2.Count > 0)
								{
									foreach (byte[] current in list2)
									{
										list.Add(current);
									}
								}
								this._header = 0uL;
								this._headerSize = 0;
								this._receivedSize = 0;
								this._receiveBuffer = null;
								this._fsmReceive = 0;
							}
						}
					}
					result = list;
				}
			}
			return result;
		}
	}
}
