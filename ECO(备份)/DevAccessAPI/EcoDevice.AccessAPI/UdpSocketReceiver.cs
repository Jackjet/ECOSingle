using CommonAPI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class UdpSocketReceiver : SocketReceiver
	{
		private int port;
		private System.Net.IPAddress ipAddress = System.Net.IPAddress.Any;
		private bool isListening = true;
		private System.Threading.Thread thread;
		private System.Net.Sockets.Socket serverSocket;
		public event System.EventHandler<SocketDataEventArgs> ResultHandler;
		public bool IsListening
		{
			get
			{
				return this.isListening;
			}
			set
			{
				this.isListening = value;
			}
		}
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
		public System.Net.IPAddress IpAddress
		{
			get
			{
				return this.ipAddress;
			}
			set
			{
				this.ipAddress = value;
			}
		}
		public void Listening()
		{
			this.thread = ThreadCreator.StartThread(new System.Threading.ThreadStart(this.baseListen), true);
		}
		private void baseListen()
		{
			bool flag = true;
			while (this.isListening)
			{
				byte[] array = new byte[63488];
				System.Net.EndPoint endPoint = null;
				try
				{
					endPoint = new System.Net.IPEndPoint(this.ipAddress, this.port);
					this.serverSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
					this.serverSocket.Bind(endPoint);
					if (flag)
					{
						DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_TrapPortNA, true);
						flag = false;
					}
					this.serverSocket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.ReceiveTimeout, 0);
					int num = this.serverSocket.ReceiveFrom(array, System.Net.Sockets.SocketFlags.None, ref endPoint);
					if (num > 0 && this.isListening)
					{
						string[] separator = new string[]
						{
							":"
						};
						string[] array2 = endPoint.ToString().Split(separator, System.StringSplitOptions.None);
						SocketMessager socketMessager = new SocketMessager();
						socketMessager.Target = array2[0];
						socketMessager.Port = System.Convert.ToInt32(array2[1]);
						socketMessager.DataLenth = num;
						socketMessager.DataBytes = array;
						System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.waitCallback), socketMessager);
					}
				}
				catch (System.Exception)
				{
					try
					{
						System.Threading.Thread.Sleep(100);
					}
					catch (System.Exception)
					{
					}
				}
				finally
				{
					if (this.serverSocket != null)
					{
						this.serverSocket.Close();
					}
				}
			}
		}
		private void waitCallback(object obj)
		{
			SocketMessager socketMessager = (SocketMessager)obj;
			AbstractSocketData socketData = socketMessager.decode();
			if (this.ResultHandler != null)
			{
				this.ResultHandler(this, new SocketDataEventArgs(socketData));
			}
		}
		public void Stop()
		{
			this.isListening = false;
			if (this.serverSocket != null)
			{
				this.serverSocket.Close();
				this.serverSocket = null;
			}
			if (this.thread != null && this.thread.IsAlive)
			{
				this.thread.Abort();
			}
		}
	}
}
