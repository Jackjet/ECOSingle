using System;
using System.Net;
using System.Net.Sockets;
namespace CommonAPI.Tools
{
	public class MyListenPortChecker
	{
		public static bool IsUdpPortUsing(int port)
		{
			bool result = false;
			Socket socket = null;
			try
			{
				EndPoint localEP = new IPEndPoint(IPAddress.Any, port);
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.Bind(localEP);
			}
			catch (Exception)
			{
				result = true;
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
				}
			}
			return result;
		}
		public static bool IsTcpPortUsing(int port)
		{
			bool result = false;
			Socket socket = null;
			try
			{
				EndPoint localEP = new IPEndPoint(IPAddress.Any, port);
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.Bind(localEP);
			}
			catch (Exception)
			{
				result = true;
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
				}
			}
			return result;
		}
	}
}
