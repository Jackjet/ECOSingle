using System;
using System.Net.Sockets;
namespace InSnergyAPI.ApplicationLayer
{
	public class GatewayStatus
	{
		public const int TOP_NOT_CARE = 0;
		public const int TOP_CHANGE_FOUND = 1;
		public const int TOP_PEND_DBREGISTER = 2;
		public Socket socket;
		public DateTime tUptime;
		public string sGID;
		public string gatewayIP;
		public string sLastReport;
		public int tZone;
		public string tLoginTime;
		public long tLastReceiveTime;
		public string tLastSendTime;
		public int nTotalReport;
		public int nTotalPoll;
		public int TotalReceived;
		public int TotalSent;
		public bool bManaged;
		public int nTopStatus;
		public int nLoginCount;
		public GatewayStatus DeepClone()
		{
			return new GatewayStatus(this.sGID, this.socket, this.bManaged, this.tZone)
			{
				tUptime = this.tUptime,
				gatewayIP = this.gatewayIP,
				sLastReport = this.sLastReport,
				tLoginTime = this.tLoginTime,
				tLastReceiveTime = this.tLastReceiveTime,
				tLastSendTime = this.tLastSendTime,
				nTotalReport = this.nTotalReport,
				nTotalPoll = this.nTotalPoll,
				TotalReceived = this.TotalReceived,
				TotalSent = this.TotalSent,
				nTopStatus = this.nTopStatus,
				nLoginCount = this.nLoginCount
			};
		}
		public GatewayStatus(string gid, Socket sock, bool managed, int nTZone)
		{
			this.nTopStatus = 0;
			this.bManaged = managed;
			this.socket = sock;
			this.tUptime = DateTime.Now;
			this.sLastReport = "";
			this.tLoginTime = "";
			this.tLastReceiveTime = (long)Environment.TickCount;
			this.tLastSendTime = "";
			this.tZone = nTZone;
			this.nTotalReport = 0;
			this.nTotalPoll = 0;
			this.TotalReceived = 0;
			this.TotalSent = 0;
			this.nLoginCount = 0;
			this.sGID = gid;
			this.gatewayIP = "";
			try
			{
				if (this.socket != null && this.socket.Connected)
				{
					this.gatewayIP = this.socket.RemoteEndPoint.ToString();
				}
			}
			catch (Exception)
			{
			}
			this.tLoginTime = DateTime.Now.ToString("MMMdd HH:mm:ss");
		}
	}
}
