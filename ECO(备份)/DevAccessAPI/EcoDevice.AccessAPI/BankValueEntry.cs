using System;
namespace EcoDevice.AccessAPI
{
	public class BankValueEntry
	{
		private int bankNumber = 1;
		private string current;
		private string voltage;
		private string power;
		private string powerDissipation;
		private BankStatus bankStatus = BankStatus.ON;
		public int BankNumber
		{
			get
			{
				return this.bankNumber;
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
		public BankStatus BankStatus
		{
			get
			{
				return this.bankStatus;
			}
			set
			{
				this.bankStatus = value;
			}
		}
		public BankValueEntry(int bnkNumber)
		{
			this.bankNumber = bnkNumber;
		}
	}
}
