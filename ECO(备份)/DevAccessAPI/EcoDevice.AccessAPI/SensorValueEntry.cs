using System;
namespace EcoDevice.AccessAPI
{
	public class SensorValueEntry
	{
		private int sensorNumber;
		private string temperature;
		private string humidity;
		private string pressure;
		public string Temperature
		{
			get
			{
				return this.temperature;
			}
			set
			{
				this.temperature = value;
			}
		}
		public string Humidity
		{
			get
			{
				return this.humidity;
			}
			set
			{
				this.humidity = value;
			}
		}
		public string Pressure
		{
			get
			{
				return this.pressure;
			}
			set
			{
				this.pressure = value;
			}
		}
		public int SensorNumber
		{
			get
			{
				return this.sensorNumber;
			}
			set
			{
				this.sensorNumber = value;
			}
		}
		public SensorValueEntry(int sensorNumber)
		{
			this.sensorNumber = sensorNumber;
		}
	}
}
