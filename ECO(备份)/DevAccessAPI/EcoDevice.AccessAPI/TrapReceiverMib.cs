using System;
namespace EcoDevice.AccessAPI
{
	internal class TrapReceiverMib
	{
		private int index = 1;
		public string TrapEnabled = "1.3.6.1.4.1.21317.1.3.2.2.3.4.7.1.1.0";
		public string TrapVersion = "1.3.6.1.4.1.21317.1.3.2.2.3.4.7.1.2.0";
		public string configurationNotifyEnabled = "1.3.6.1.4.1.21317.1.3.2.2.3.4.7.9.1.0";
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.3.4.7.1.3.1";
		public string ReceiverIp
		{
			get
			{
				return TrapReceiverMib.Entry + ".2." + this.index;
			}
		}
		public string TrapPort
		{
			get
			{
				return TrapReceiverMib.Entry + ".3." + this.index;
			}
		}
		public string Community
		{
			get
			{
				return TrapReceiverMib.Entry + ".4." + this.index;
			}
		}
		public string Username
		{
			get
			{
				return TrapReceiverMib.Entry + ".5." + this.index;
			}
		}
		public string AuthPassword
		{
			get
			{
				return TrapReceiverMib.Entry + ".6." + this.index;
			}
		}
		public string PrivPassword
		{
			get
			{
				return TrapReceiverMib.Entry + ".7." + this.index;
			}
		}
		public TrapReceiverMib(int index)
		{
			this.index = index;
		}
	}
}
