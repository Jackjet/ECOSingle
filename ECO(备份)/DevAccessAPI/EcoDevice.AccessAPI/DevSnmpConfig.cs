using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class DevSnmpConfig
	{
		public string devIP = "127.0.0.1";
		public int devPort = 161;
		public string modelName = "UNKNOWN";
		public string fmwareVer = "N/A";
		public int retry;
		public int timeout = 5;
		public int snmpVer;
		public string userName = "administrator";
		public string authType = "MD5";
		public string authPSW = "password";
		public string privType = "DES";
		public string privPSW = "privacypsw";
		public System.Collections.Generic.List<int> groupOutlets = new System.Collections.Generic.List<int>();
		public int devID;
		public string devMac = "";
		public void copyConfig(DevSnmpConfig cfg)
		{
			this.devIP = cfg.devIP;
			this.devPort = cfg.devPort;
			this.modelName = cfg.modelName;
			this.fmwareVer = cfg.fmwareVer;
			this.retry = cfg.retry;
			this.timeout = cfg.timeout;
			this.snmpVer = cfg.snmpVer;
			this.userName = cfg.userName;
			this.authType = cfg.authType;
			this.authPSW = cfg.authPSW;
			this.privType = cfg.privType;
			this.privPSW = cfg.privPSW;
			this.groupOutlets = cfg.groupOutlets;
			this.devID = cfg.devID;
			this.devMac = cfg.devMac;
		}
	}
}
