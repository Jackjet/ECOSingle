using System;
using System.Net.Sockets;
namespace InSnergyAPI.ConnectionLayer
{
	public class ConnectionStatus
	{
		public Socket socket;
		public long tConnected;
		public string gid;
		public int nAuthorized;
		public long tLastReceived;
		public long tLastSent;
		public int nTotalReceived;
		public int nTotalSent;
		public ConnectionStatus(Socket sock)
		{
			this.socket = sock;
			this.tConnected = (long)Environment.TickCount;
			this.gid = "";
			this.nAuthorized = 0;
			this.tLastReceived = (long)Environment.TickCount;
			this.tLastSent = (long)Environment.TickCount;
			this.nTotalReceived = 0;
			this.nTotalSent = 0;
		}
		public void Reset()
		{
			this.socket = null;
			this.gid = "";
			this.nAuthorized = 0;
			this.tConnected = (long)Environment.TickCount;
			this.tLastReceived = (long)Environment.TickCount;
			this.tLastSent = (long)Environment.TickCount;
			this.nTotalReceived = 0;
			this.nTotalSent = 0;
		}
		public ConnectionStatus DeepClone()
		{
			return new ConnectionStatus(this.socket)
			{
				tConnected = this.tConnected,
				gid = this.gid,
				nAuthorized = this.nAuthorized,
				tLastReceived = this.tLastReceived,
				tLastSent = this.tLastSent,
				nTotalReceived = this.nTotalReceived,
				nTotalSent = this.nTotalSent
			};
		}
	}
}
