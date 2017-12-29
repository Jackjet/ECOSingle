using System;
namespace EcoDevice.AccessAPI
{
	internal class SensorEnvironmentMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.1.7.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.1.7.1.2";
		public string MinTemperatureMt
		{
			get
			{
				return SensorEnvironmentMib.Entry + ".2." + this.index;
			}
		}
		public string MaxTemperatureMt
		{
			get
			{
				return SensorEnvironmentMib.Entry + ".3." + this.index;
			}
		}
		public string MinHumidityMt
		{
			get
			{
				return SensorEnvironmentMib.Entry + ".4." + this.index;
			}
		}
		public string MaxHumidityMt
		{
			get
			{
				return SensorEnvironmentMib.Entry + ".5." + this.index;
			}
		}
		public string MinPressMt
		{
			get
			{
				return SensorEnvironmentMib.Entry + ".6." + this.index;
			}
		}
		public string MaxPressMt
		{
			get
			{
				return SensorEnvironmentMib.Entry + ".7." + this.index;
			}
		}
		public SensorEnvironmentMib(int sensorNumber)
		{
			this.index = sensorNumber;
		}
	}
}
