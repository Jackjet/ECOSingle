using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcDeviceConfigMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.4.1.1";
		public string MinPowerConfig
		{
			get
			{
				return ApcDeviceConfigMib.Entry + ".7." + this.index;
			}
		}
		public string MaxPowerConfig
		{
			get
			{
				return ApcDeviceConfigMib.Entry + ".9." + this.index;
			}
		}
		public string NearPowerConfig
		{
			get
			{
				return ApcDeviceConfigMib.Entry + ".8." + this.index;
			}
		}
		public ApcDeviceConfigMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
