using System;
namespace EcoDevice.AccessAPI
{
	public abstract class SnmpConfig
	{
		protected string agentIp;
		protected int port = 161;
		protected int timeout = 500;
		protected int retry;
		protected SnmpVersionType version = SnmpVersionType.Ver3;
		public string AgentIp
		{
			get
			{
				return this.agentIp;
			}
		}
		public SnmpVersionType Version
		{
			get
			{
				return this.version;
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
		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				this.timeout = value;
			}
		}
		public int Retry
		{
			get
			{
				return this.retry;
			}
			set
			{
				this.retry = value;
			}
		}
		public SnmpConfig(string agentIp)
		{
			this.agentIp = agentIp;
		}
	}
}
