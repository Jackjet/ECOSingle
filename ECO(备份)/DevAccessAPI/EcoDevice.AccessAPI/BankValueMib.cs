using System;
namespace EcoDevice.AccessAPI
{
	internal class BankValueMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.3.2.1";
		public string Current
		{
			get
			{
				return BankValueMib.Entry + ".2." + this.index;
			}
		}
		public string Voltage
		{
			get
			{
				return BankValueMib.Entry + ".3." + this.index;
			}
		}
		public string Power
		{
			get
			{
				return BankValueMib.Entry + ".4." + this.index;
			}
		}
		public string PowerDissipation
		{
			get
			{
				return BankValueMib.Entry + ".5." + this.index;
			}
		}
		public BankValueMib(int bankNumber)
		{
			this.index = bankNumber;
		}
	}
}
