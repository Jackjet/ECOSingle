using System;
namespace EcoDevice.AccessAPI
{
	public struct BankOutlets
	{
		public int fromPort;
		public int toPort;
		public void copy(BankOutlets src)
		{
			this.fromPort = src.fromPort;
			this.toPort = src.toPort;
		}
	}
}
