using System;
using System.Net;
namespace EcoDevice.AccessAPI
{
	public interface SocketSender
	{
		int Port
		{
			get;
			set;
		}
		System.Net.IPAddress IpAddress
		{
			get;
			set;
		}
		void Send<T>(T socketData) where T : AbstractSocketData, new();
	}
}
