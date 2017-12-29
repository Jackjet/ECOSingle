using System;
namespace EcoDevice.AccessAPI
{
	public class LineAutoValue
	{
		private int id;
		private int line_id;
		private float current_value = -1f;
		private float voltage_value = -1f;
		private float power_value = -1f;
		private System.DateTime insert_time;
		public int Id
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
		public int LineId
		{
			get
			{
				return this.line_id;
			}
			set
			{
				this.line_id = value;
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
	}
}
