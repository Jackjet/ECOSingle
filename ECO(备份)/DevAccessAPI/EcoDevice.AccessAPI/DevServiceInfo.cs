using System;
namespace EcoDevice.AccessAPI
{
	public struct DevServiceInfo
	{
		public int httpPort;
		public string fwVersion;
		public DevServiceInfo(string myMac)
		{
			this.httpPort = 0;
			this.fwVersion = string.Empty;
		}
	}
}
