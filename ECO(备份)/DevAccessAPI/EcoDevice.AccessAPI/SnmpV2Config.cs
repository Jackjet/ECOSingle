using System;
namespace EcoDevice.AccessAPI
{
	public class SnmpV2Config : SnmpConfig
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
		public SnmpV2Config(string agentIp) : base(agentIp)
		{
			this.version = SnmpVersionType.Ver2;
		}
	}
}
