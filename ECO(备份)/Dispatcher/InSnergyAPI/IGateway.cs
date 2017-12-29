using System;
using System.Collections.Generic;
namespace InSnergyAPI
{
	public class IGateway
	{
		public string sGid;
		public string sIP;
		public DateTime tUptime;
		public Dictionary<string, IBranch> listDevice = new Dictionary<string, IBranch>();
		public IGateway DeepClone()
		{
			IGateway gateway = new IGateway(this.sGid, this.sIP, this.tUptime);
			gateway.listDevice = new Dictionary<string, IBranch>();
			foreach (KeyValuePair<string, IBranch> current in this.listDevice)
			{
				gateway.listDevice.Add(current.Key, current.Value.DeepClone());
			}
			return gateway;
		}
		public IGateway(string gid, string ip, DateTime t)
		{
			this.sGid = gid;
			this.sIP = ip;
			this.tUptime = t;
		}
		public void Update(IGateway gateway)
		{
			if (this.sGid != gateway.sGid)
			{
				return;
			}
			this.sIP = gateway.sIP;
			this.tUptime = gateway.tUptime;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, IBranch> current in this.listDevice)
			{
				list.Add(current.Key);
			}
			foreach (KeyValuePair<string, IBranch> current2 in gateway.listDevice)
			{
				if (this.listDevice.ContainsKey(current2.Key))
				{
					list.Remove(current2.Key);
					this.listDevice[current2.Key].Update(current2.Value);
				}
				else
				{
					this.listDevice.Add(current2.Key, current2.Value);
				}
			}
			foreach (string current3 in list)
			{
				this.listDevice.Remove(current3);
			}
		}
	}
}
