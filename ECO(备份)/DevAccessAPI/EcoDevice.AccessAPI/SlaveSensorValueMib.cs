using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveSensorValueMib
	{
		private int sensorIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string sensorValueEntry = ".1.4.1";
		public static string Temperature = ".1.4.1.2.";
		public static string Humidity = ".1.4.1.3.";
		public static string Pressure = ".1.4.1.4.";
		public string SlaveTemperature
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorValueMib.Entry,
					this.slave,
					SlaveSensorValueMib.Temperature,
					this.sensorIndex
				});
			}
		}
		public string SlaveHumidity
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorValueMib.Entry,
					this.slave,
					SlaveSensorValueMib.Humidity,
					this.sensorIndex
				});
			}
		}
		public string SlavePressure
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorValueMib.Entry,
					this.slave,
					SlaveSensorValueMib.Pressure,
					this.sensorIndex
				});
			}
		}
		public SlaveSensorValueMib(int index, int sensorNumber)
		{
			this.slave += index;
			this.sensorIndex = sensorNumber;
		}
	}
}
