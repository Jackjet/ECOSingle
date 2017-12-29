using ecoProtocols;
using SocketServer;
using System;
using System.Net.Sockets;
namespace EcoMessenger
{
	public class BillingMessage
	{
		public BaseTCPServer<BillingContext, BillingHandler> server;
		public BillingContext c;
		public SocketAsyncEventArgs e;
	}
}
