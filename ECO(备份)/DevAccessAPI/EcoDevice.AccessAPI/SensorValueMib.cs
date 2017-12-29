using System;
namespace EcoDevice.AccessAPI
{
	internal class SensorValueMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.1.4.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.1.4.1.2";
		public string Temperature
		{
			get
			{
				return SensorValueMib.Entry + ".2." + this.index;
			}
		}
		public string Humidity
		{
			get
			{
				return SensorValueMib.Entry + ".3." + this.index;
			}
		}
		public string Pressure
		{
			get
			{
				return SensorValueMib.Entry + ".4." + this.index;
			}
		}
		public SensorValueMib(int sensorNumber)
		{
			this.index = sensorNumber;
		}
	}
}
