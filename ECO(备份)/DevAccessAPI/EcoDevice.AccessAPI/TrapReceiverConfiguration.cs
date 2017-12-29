using System;
using System.Net;
namespace EcoDevice.AccessAPI
{
	public class TrapReceiverConfiguration
	{
		private int index = 1;
		private TrapEnabled enabled = TrapEnabled.Yes;
		private System.Net.IPAddress receiverIp;
		private int trapPort = -1;
		private int agentversion = 1;
		private int trapversion = 1;
		private string community = string.Empty;
		private string username = string.Empty;
		private string authpassword = string.Empty;
		private string privpassword = string.Empty;
		public int Index
		{
			get
			{
				return this.index;
			}
		}
		public TrapEnabled Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}
		public System.Net.IPAddress ReceiverIp
		{
			get
			{
				return this.receiverIp;
			}
			set
			{
				this.receiverIp = value;
			}
		}
		public int TrapPort
		{
			get
			{
				return this.trapPort;
			}
			set
			{
				this.trapPort = value;
			}
		}
		public int AgentVersion
		{
			get
			{
				return this.agentversion;
			}
			set
			{
				this.agentversion = value;
			}
		}
		public int TrapVersion
		{
			get
			{
				return this.trapversion;
			}
			set
			{
				this.trapversion = value;
			}
		}
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
		public string Username
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
		public string AuthPassword
		{
			get
			{
				return this.authpassword;
			}
			set
			{
				this.authpassword = value;
			}
		}
		public string PrivPassword
		{
			get
			{
				return this.privpassword;
			}
			set
			{
				this.privpassword = value;
			}
		}
		public TrapReceiverConfiguration(int index)
		{
			if (index < 1 || index > Constant.TrapReceiverNumber)
			{
				throw new System.ArgumentException("[TrapReceiver]Invalid index.");
			}
			this.index = index;
		}
	}
}
