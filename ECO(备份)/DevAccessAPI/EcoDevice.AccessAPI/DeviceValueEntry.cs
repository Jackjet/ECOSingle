using System;
namespace EcoDevice.AccessAPI
{
	public class DeviceValueEntry
	{
		private string current;
		private string voltage;
		private string power;
		private string powerDissipation;
		private int doorSensorStatus = -500;
		private int leakCurrentStatus = -500;
		public string PowerDissipation
		{
			get
			{
				return this.powerDissipation;
			}
			set
			{
				this.powerDissipation = value;
			}
		}
		public string Power
		{
			get
			{
				return this.power;
			}
			set
			{
				this.power = value;
			}
		}
		public string Current
		{
			get
			{
				return this.current;
			}
			set
			{
				this.current = value;
			}
		}
		public string Voltage
		{
			get
			{
				return this.voltage;
			}
			set
			{
				this.voltage = value;
			}
		}
		public int DoorSensorStatus
		{
			get
			{
				return this.doorSensorStatus;
			}
			set
			{
				this.doorSensorStatus = value;
			}
		}
		public int LeakCurrentStatus
		{
			get
			{
				return this.leakCurrentStatus;
			}
			set
			{
				this.leakCurrentStatus = value;
			}
		}
	}
}
