using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcSensorConfigMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.10.2.1.1";
		public string MaxTemperature
		{
			get
			{
				return ApcSensorConfigMib.Entry + ".10." + this.index;
			}
		}
		public string HighTemperature
		{
			get
			{
				return ApcSensorConfigMib.Entry + ".11." + this.index;
			}
		}
		public string LowHumidity
		{
			get
			{
				return ApcSensorConfigMib.Entry + ".13." + this.index;
			}
		}
		public string MinHumidity
		{
			get
			{
				return ApcSensorConfigMib.Entry + ".14." + this.index;
			}
		}
		public ApcSensorConfigMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
