using System;
namespace EcoDevice.AccessAPI
{
	public class SensorAutoValue
	{
		private int id;
		private int device_id;
		private float humidity;
		private float temperature;
		private float press_value;
		private int sensor_type = 1;
		private System.DateTime insert_time;
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public int DeviceId
		{
			get
			{
				return this.device_id;
			}
			set
			{
				this.device_id = value;
			}
		}
		public int Type
		{
			get
			{
				return this.sensor_type;
			}
			set
			{
				this.sensor_type = value;
			}
		}
		public System.DateTime InsertTime
		{
			get
			{
				return this.insert_time;
			}
			set
			{
				this.insert_time = value;
			}
		}
		public float Temperature
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
		public float Press
		{
			get
			{
				return this.press_value;
			}
			set
			{
				this.press_value = value;
			}
		}
		public float Humidity
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
	}
}
