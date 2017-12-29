using System;
namespace EcoDevice.AccessAPI
{
	public class SensorMapping
	{
		private int sensorId;
		private int sensorNum;
		private int sensorLocation;
		private string sensorName;
		public int SensorNumber
		{
			get
			{
				return this.sensorNum;
			}
			set
			{
				this.sensorNum = value;
			}
		}
		public int SensorId
		{
			get
			{
				return this.sensorId;
			}
			set
			{
				this.sensorId = value;
			}
		}
		public int SensorLocation
		{
			get
			{
				return this.sensorLocation;
			}
			set
			{
				this.sensorLocation = value;
			}
		}
		public string SensorName
		{
			get
			{
				return this.sensorName;
			}
			set
			{
				this.sensorName = value;
			}
		}
		public SensorMapping(int sensorNum)
		{
			this.sensorNum = sensorNum;
		}
	}
}
