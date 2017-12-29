using System;
namespace EcoDevice.AccessAPI
{
	public class SensorThreshold
	{
		private int index = 1;
		private float minTemperatureMt = -500f;
		private float maxTemperatureMt = -500f;
		private float minHumidityMt = -500f;
		private float maxHumidityMt = -500f;
		private float minPressMt = -500f;
		private float maxPressMt = -500f;
		public int SensorNumber
		{
			get
			{
				return this.index;
			}
		}
		public float MaxPressMT
		{
			get
			{
				return this.maxPressMt;
			}
			set
			{
				this.maxPressMt = value;
			}
		}
		public float MinPressMT
		{
			get
			{
				return this.minPressMt;
			}
			set
			{
				this.minPressMt = value;
			}
		}
		public float MaxHumidityMT
		{
			get
			{
				return this.maxHumidityMt;
			}
			set
			{
				this.maxHumidityMt = value;
			}
		}
		public float MinHumidityMT
		{
			get
			{
				return this.minHumidityMt;
			}
			set
			{
				this.minHumidityMt = value;
			}
		}
		public float MaxTemperatureMT
		{
			get
			{
				return this.maxTemperatureMt;
			}
			set
			{
				this.maxTemperatureMt = value;
			}
		}
		public float MinTemperatureMT
		{
			get
			{
				return this.minTemperatureMt;
			}
			set
			{
				this.minTemperatureMt = value;
			}
		}
		public SensorThreshold(int sensorNumber)
		{
			this.index = sensorNumber;
		}
	}
}
