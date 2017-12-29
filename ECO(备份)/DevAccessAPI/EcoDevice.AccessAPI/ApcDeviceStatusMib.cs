using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcDeviceStatusMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.4.3.1";
		public string PowerStatus
		{
			get
			{
				return ApcDeviceStatusMib.Entry + ".5." + this.index;
			}
		}
		public string PowerDsptStatus
		{
			get
			{
				return ApcDeviceStatusMib.Entry + ".9." + this.index;
			}
		}
		public ApcDeviceStatusMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
