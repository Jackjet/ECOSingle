using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public abstract class TrapMessage
	{
		private int port;
		private string agentIpAddress;
		private SnmpVersionType snmpVersion;
		private System.Collections.Generic.Dictionary<string, string> varBindings = new System.Collections.Generic.Dictionary<string, string>();
		private System.DateTime receiveTime;
		public System.DateTime ReceiveTime
		{
			get
			{
				return this.receiveTime;
			}
			set
			{
				this.receiveTime = value;
			}
		}
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
		public string AgentIpAddress
		{
			get
			{
				return this.agentIpAddress;
			}
			set
			{
				this.agentIpAddress = value;
			}
		}
		public SnmpVersionType SnmpVersion
		{
			get
			{
				return this.snmpVersion;
			}
			set
			{
				this.snmpVersion = value;
			}
		}
		public System.Collections.Generic.Dictionary<string, string> VarBindings
		{
			get
			{
				return this.varBindings;
			}
		}
	}
}
