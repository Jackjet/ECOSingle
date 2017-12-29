using System;
namespace EcoDevice.AccessAPI
{
	internal class BankStatusValueMib
	{
		private int bankNumber = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.3.2.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.3.2.1.7";
		public string BankStatus
		{
			get
			{
				return BankStatusValueMib.Column + "." + this.bankNumber;
			}
		}
		public BankStatusValueMib(int bankNumber)
		{
			this.bankNumber = bankNumber;
		}
	}
}
