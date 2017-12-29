using System;
namespace EcoDevice.AccessAPI
{
	public class BankAutoValue
	{
		private int id;
		private int bank_id;
		private float current_value = -1f;
		private float voltage_value = -1f;
		private float power_value = -1f;
		private float power_consumption;
		private string bank_state = BankStatus.OFF.ToString();
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
		public int BankId
		{
			get
			{
				return this.bank_id;
			}
			set
			{
				this.bank_id = value;
			}
		}
		public string BankState
		{
			get
			{
				return this.bank_state;
			}
			set
			{
				this.bank_state = value;
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
	}
}
