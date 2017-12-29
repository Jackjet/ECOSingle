using System;
namespace EcoDevice.AccessAPI
{
	internal class BankControlMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.3.4.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.3.4.1.2";
		public BankControlMib(int bankNumber)
		{
			this.index = bankNumber;
		}
		public static string BankStatus(int bankIndex)
		{
			return BankControlMib.Column + "." + bankIndex;
		}
	}
}
