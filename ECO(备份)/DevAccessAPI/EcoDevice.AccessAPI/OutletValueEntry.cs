using System;
namespace EcoDevice.AccessAPI
{
	public class OutletValueEntry
	{
		private int outletNumber = 1;
		private string current;
		private string voltage;
		private string power;
		private string powerDissipation;
		private OutletStatus outletStatues = OutletStatus.ON;
		public int OutletNumber
		{
			get
			{
				return this.outletNumber;
			}
		}
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
		public OutletStatus OutletStatus
		{
			get
			{
				return this.outletStatues;
			}
			set
			{
				this.outletStatues = value;
			}
		}
		public OutletValueEntry(int outletNumber)
		{
			this.outletNumber = outletNumber;
		}
	}
}
