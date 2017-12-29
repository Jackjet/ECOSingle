using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonSensorHumidityMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.7.2.1";
		public string HumidityValue
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".4.0." + this.index;
			}
		}
		public string HumidityStatus
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string HumidityLowerWarning
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".6.0." + this.index;
			}
		}
		public string HumidityLowerCritical
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public string HumidityUpperWarning
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".8.0." + this.index;
			}
		}
		public string HumidityUpperCritical
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".9.0." + this.index;
			}
		}
		public string MinHumidityMt
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".6.0." + this.index;
			}
		}
		public string MaxHumidityMT
		{
			get
			{
				return EatonSensorHumidityMib_M2.Entry + ".8.0." + this.index;
			}
		}
		public EatonSensorHumidityMib_M2(int sensorNum)
		{
			this.index = sensorNum;
		}
	}
}
