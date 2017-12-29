using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveSensorEnvironmentMib
	{
		private int sensorIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string sensorThresholdEntry = ".1.7.1";
		public static string MinTemperatureMt = ".1.7.1.2.";
		public static string MaxTemperatureMt = ".1.7.1.3.";
		public static string MinHumidityMt = ".1.7.1.4.";
		public static string MaxHumidityMt = ".1.7.1.5.";
		public static string MinPressMt = ".1.7.1.6.";
		public static string MaxPressMt = ".1.7.1.7.";
		public string SlaveMinTemperatureMt
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorEnvironmentMib.Entry,
					this.slave,
					SlaveSensorEnvironmentMib.MinTemperatureMt,
					this.sensorIndex
				});
			}
		}
		public string SlaveMaxTemperatureMt
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorEnvironmentMib.Entry,
					this.slave,
					SlaveSensorEnvironmentMib.MaxTemperatureMt,
					this.sensorIndex
				});
			}
		}
		public string SlaveMinHumidityMt
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorEnvironmentMib.Entry,
					this.slave,
					SlaveSensorEnvironmentMib.MinHumidityMt,
					this.sensorIndex
				});
			}
		}
		public string SlaveMaxHumidityMt
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorEnvironmentMib.Entry,
					this.slave,
					SlaveSensorEnvironmentMib.MaxHumidityMt,
					this.sensorIndex
				});
			}
		}
		public string SlaveMinPressMt
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorEnvironmentMib.Entry,
					this.slave,
					SlaveSensorEnvironmentMib.MinPressMt,
					this.sensorIndex
				});
			}
		}
		public string SlaveMaxPressMt
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveSensorEnvironmentMib.Entry,
					this.slave,
					SlaveSensorEnvironmentMib.MaxPressMt,
					this.sensorIndex
				});
			}
		}
		public SlaveSensorEnvironmentMib(int index, int sensorNumber)
		{
			this.slave += index;
			this.sensorIndex = sensorNumber;
		}
	}
}
