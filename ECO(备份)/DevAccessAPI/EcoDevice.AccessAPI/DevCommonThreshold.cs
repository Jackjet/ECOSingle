using System;
namespace EcoDevice.AccessAPI
{
	public struct DevCommonThreshold
	{
		public string voltage;
		public string power;
		public string powerDissipation;
		public string temperature;
		public string humidity;
		public string pressure;
		public void copy(DevCommonThreshold src)
		{
			this.voltage = src.voltage;
			this.power = src.power;
			this.powerDissipation = src.powerDissipation;
			this.temperature = src.temperature;
			this.humidity = src.humidity;
			this.pressure = src.pressure;
		}
	}
}
