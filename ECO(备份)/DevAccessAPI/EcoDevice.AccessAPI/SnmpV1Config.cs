using System;
namespace EcoDevice.AccessAPI
{
	public class SnmpV1Config : SnmpConfig
	{
		private string community = "public";
		public string Community
		{
			get
			{
				return this.community;
			}
			set
			{
				this.community = value;
			}
		}
		public SnmpV1Config(string agentIp) : base(agentIp)
		{
			this.version = SnmpVersionType.Ver1;
		}
	}
}
