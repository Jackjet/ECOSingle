using System;
namespace EcoDevice.AccessAPI
{
	public class DeviceAutoValue
	{
		private int id;
		private int device_id;
		private float voltage_value = -1f;
		private float current_value = -1f;
		private float power_value = -1f;
		private float power_consumption;
		private int device_state;
		private int doorsensor_status = -500;
		private int leakcurrent_status = -500;
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
		public int DeviceState
		{
			get
			{
				return this.device_state;
			}
			set
			{
				this.device_state = value;
			}
		}
		public float PowerDissipation
		{
			get
			{
				return this.power_consumption;
			}
			set
			{
				this.power_consumption = value;
			}
		}
		public float Power
		{
			get
			{
				return this.power_value;
			}
			set
			{
				this.power_value = value;
			}
		}
		public float Current
		{
			get
			{
				return this.current_value;
			}
			set
			{
				this.current_value = value;
			}
		}
		public float Voltage
		{
			get
			{
				return this.voltage_value;
			}
			set
			{
				this.voltage_value = value;
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
		public int DoorSensorStatus
		{
			get
			{
				return this.doorsensor_status;
			}
			set
			{
				this.doorsensor_status = value;
			}
		}
		public int LeakCurrentStatus
		{
			get
			{
				return this.leakcurrent_status;
			}
			set
			{
				this.leakcurrent_status = value;
			}
		}
	}
}
