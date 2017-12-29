using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
namespace ecoProtocols
{
	public class ServerConfig
	{
		public bool ssl;
		public ServerEndPoint real;
		public IPAddress ip4mask;
		public ecoServerProtocol.ServerTypes Protocol;
		public int port;
		public string serverName;
		public string tokenList;
		public int MaxListen;
		public int MaxConnect;
		public int BufferSize;
		public int MinPort;
		public int MaxPort;
		public int UdpQueueSize;
		public int TcpMinAcceptBacklog;
		public int TcpMaxAcceptBacklog;
		public int TcpQueueSize;
		public X509Certificate2 TlsCertificate;
		public ServerConfig()
		{
			this.ssl = false;
			this.TlsCertificate = null;
			this.TcpMinAcceptBacklog = 1024;
			this.TcpMaxAcceptBacklog = 2048;
			this.TcpQueueSize = 8;
			this.tokenList = "12345678";
		}
	}
}
