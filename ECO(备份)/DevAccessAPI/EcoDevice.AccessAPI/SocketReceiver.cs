using System;
using System.Net;
namespace EcoDevice.AccessAPI
{
	public interface SocketReceiver
	{
		event System.EventHandler<SocketDataEventArgs> ResultHandler;
		bool IsListening
		{
			get;
		}
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
		void Listening();
		void Stop();
	}
}
