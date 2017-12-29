using ecoProtocols;
using InSnergyAPI.ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
namespace InSnergyAPI.ConnectionLayer
{
	public class ConnectionContext
	{
		public const int STATE_REMOVE = 0;
		public const int TIME_CONNECTED = 1;
		public const int TIME_LAST_RECEIVED = 2;
		public const int TIME_LAST_SENT = 3;
		public const int BYTES_LAST_RECEIVED = 4;
		public const int BYTES_LAST_SENT = 5;
		public const int STATE_AUTHORIZED = 6;
		public const int SIZE_RECEIVE_BUFFER = 1600;
		public const int TIMEOUT_AUTHORIZE = 60000;
		public static int TIMEOUT_RECEIVE = 120000;
		public ConnectionStatus status;
		public int nRemainingBytes;
		public byte[] receiveBuffer;
		public ConnectionContext(Socket sock)
		{
			this.status = new ConnectionStatus(sock);
			this.nRemainingBytes = 0;
			this.receiveBuffer = new byte[1600];
		}
		public bool IsTimeout()
		{
			long num = Common.ElapsedTime(this.status.tConnected);
			if (this.status.nAuthorized <= 0)
			{
				if ((int)num > 60000)
				{
					InSnergyService.PostLog("TIMEOUT_AUTHORIZE: " + 60000);
					return true;
				}
			}
			else
			{
				num = Common.ElapsedTime(this.status.tLastReceived);
				if (num > (long)ConnectionContext.TIMEOUT_RECEIVE)
				{
					InSnergyService.PostLog("TIMEOUT_RECEIVE: " + ConnectionContext.TIMEOUT_RECEIVE);
					return true;
				}
			}
			return false;
		}
		public void UpdateState(int type, int nParam = 0, string strParam = "")
		{
			switch (type)
			{
			case 1:
				this.nRemainingBytes = 0;
				this.status.tConnected = (long)Environment.TickCount;
				this.status.tLastReceived = (long)Environment.TickCount;
				return;
			case 2:
				this.status.tLastReceived = (long)Environment.TickCount;
				return;
			case 3:
				this.status.tLastSent = (long)Environment.TickCount;
				return;
			case 4:
				this.status.tLastReceived = (long)Environment.TickCount;
				this.status.nTotalReceived += nParam;
				return;
			case 5:
				this.status.nTotalSent += nParam;
				return;
			case 6:
				this.status.nAuthorized = nParam;
				this.status.gid = strParam;
				this.status.tLastReceived = (long)Environment.TickCount;
				return;
			default:
				return;
			}
		}
		public List<SocketAsyncEventArgs> IncommingHandler(SocketAsyncEventArgs receiveEventArg)
		{
			int i = 0;
			List<SocketAsyncEventArgs> list = new List<SocketAsyncEventArgs>();
			while (i < receiveEventArg.BytesTransferred)
			{
				int num = 1600 - this.nRemainingBytes;
				if (num == 0)
				{
					return null;
				}
				if (receiveEventArg.BytesTransferred - i < num)
				{
					num = receiveEventArg.BytesTransferred - i;
				}
				Buffer.BlockCopy(receiveEventArg.Buffer, receiveEventArg.Offset + i, this.receiveBuffer, this.nRemainingBytes, num);
				this.nRemainingBytes += num;
				i += num;
				while (this.nRemainingBytes > 0)
				{
					bool flag = false;
					int j;
					for (j = 0; j < this.nRemainingBytes; j++)
					{
						if (this.receiveBuffer[j] == 13 || this.receiveBuffer[j] == 10)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					int num2 = j;
					while (num2 < this.nRemainingBytes && (this.receiveBuffer[num2] == 13 || this.receiveBuffer[num2] == 10))
					{
						num2++;
					}
					if (j > 0)
					{
						byte[] array = ApplicationHandler.ProtocolParser(receiveEventArg.AcceptSocket, this.status.nAuthorized, this.receiveBuffer, 0, j);
						if (array != null)
						{
							SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
							if (socketAsyncEventArgs != null)
							{
								socketAsyncEventArgs.AcceptSocket = receiveEventArg.AcceptSocket;
								socketAsyncEventArgs.SetBuffer(array, 0, array.Length);
								socketAsyncEventArgs.UserToken = null;
								list.Add(socketAsyncEventArgs);
							}
						}
					}
					this.nRemainingBytes -= num2;
					if (this.nRemainingBytes > 0)
					{
						Buffer.BlockCopy(this.receiveBuffer, num2, this.receiveBuffer, 0, this.nRemainingBytes);
					}
				}
			}
			return list;
		}
	}
}
