using System;
namespace ecoProtocols
{
	public class ClientConfig
	{
		public bool ssl;
		public string hostName;
		public int port;
		public int maxConnections;
		public int interval;
		public bool bKeepAlive;
		public int uid;
		public int vid;
		public string info4Login = "";
		public ClientConfig()
		{
			this.ssl = false;
			this.uid = -1;
			this.info4Login = "";
			this.hostName = "";
			this.port = 8888;
			this.maxConnections = 3000;
			this.interval = 10000;
			this.bKeepAlive = true;
		}
	}
}
