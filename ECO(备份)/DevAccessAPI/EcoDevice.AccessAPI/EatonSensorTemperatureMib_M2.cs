using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonSensorTemperatureMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.7.1.1";
		public string TemperatureValue
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".4.0." + this.index;
			}
		}
		public string TemperatureStatus
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string TemperatureLowerWarning
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".6.0." + this.index;
			}
		}
		public string TemperatureLowerCritical
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public string TemperatureUpperWarning
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".8.0." + this.index;
			}
		}
		public string TemperatureUpperCritical
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".9.0." + this.index;
			}
		}
		public string MinTemperatureMt
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".6.0." + this.index;
			}
		}
		public string MaxTemperatureMT
		{
			get
			{
				return EatonSensorTemperatureMib_M2.Entry + ".8.0." + this.index;
			}
		}
		public EatonSensorTemperatureMib_M2(int sensorNum)
		{
			this.index = sensorNum;
		}
	}
}
