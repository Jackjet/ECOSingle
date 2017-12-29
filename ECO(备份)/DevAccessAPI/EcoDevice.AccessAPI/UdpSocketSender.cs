using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class UdpSocketSender : SocketSender
	{
		private int port;
		private System.Net.IPAddress ipAddress;
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
		public void Send<T>(T socketData) where T : AbstractSocketData, new()
		{
			if (socketData == null)
			{
				return;
			}
			if (this.ipAddress == null || this.ipAddress.Equals(System.Net.IPAddress.Any))
			{
				throw new System.ArgumentException("The IpAddress is uncertain.");
			}
			if (this.port <= 0)
			{
				throw new System.ArgumentException("The Port was not set.");
			}
			ThreadCreator.StartThread(new System.Threading.ParameterizedThreadStart(this.baseSend), socketData, true);
		}
		private void baseSend(object obj)
		{
			AbstractSocketData sockeData = obj as AbstractSocketData;
			SocketMessager socketMessager = new SocketMessager();
			socketMessager.encode(sockeData);
			byte[] dataBytes = socketMessager.DataBytes;
			if (dataBytes != null && dataBytes.Length > 0)
			{
				System.Net.Sockets.Socket socket = null;
				try
				{
					System.Net.EndPoint remoteEP = new System.Net.IPEndPoint(this.ipAddress, this.port);
					socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
					socket.SendTo(dataBytes, dataBytes.Length, System.Net.Sockets.SocketFlags.None, remoteEP);
					socket.Shutdown(System.Net.Sockets.SocketShutdown.Send);
				}
				catch (System.Exception)
				{
				}
				finally
				{
					if (socket != null)
					{
						socket.Close();
					}
				}
			}
		}
	}
}
