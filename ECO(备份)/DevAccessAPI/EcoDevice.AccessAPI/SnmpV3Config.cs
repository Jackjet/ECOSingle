using System;
namespace EcoDevice.AccessAPI
{
	public class SnmpV3Config : SnmpConfig
	{
		private string username = "test";
		private Authentication auth;
		private string authSecret = "";
		private Privacy privacy;
		private string privSecret = "";
		private string contextName = "";
		public string ContextName
		{
			get
			{
				return this.contextName;
			}
			set
			{
				this.contextName = value;
			}
		}
		public string UserName
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
			}
		}
		public Authentication Authentication
		{
			get
			{
				return this.auth;
			}
			set
			{
				this.auth = value;
			}
		}
		public Privacy Privacy
		{
			get
			{
				return this.privacy;
			}
			set
			{
				this.privacy = value;
			}
		}
		public string AuthSecret
		{
			get
			{
				return this.authSecret;
			}
			set
			{
				this.authSecret = value;
			}
		}
		public string PrivacySecret
		{
			get
			{
				return this.privSecret;
			}
			set
			{
				this.privSecret = value;
			}
		}
		public SnmpV3Config(string agentIp) : base(agentIp)
		{
			this.version = SnmpVersionType.Ver3;
		}
	}
}
