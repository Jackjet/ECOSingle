using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveBankStatusValueMib
	{
		private int bankIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string bankStatusValueEntry = ".2.3";
		public static string Status = ".2.3.";
		public string SlaveBankStatus
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankStatusValueMib.Entry,
					this.slave,
					SlaveBankStatusValueMib.Status,
					this.bankIndex
				});
			}
		}
		public SlaveBankStatusValueMib(int index, int banknumber)
		{
			this.slave += index;
			this.bankIndex = banknumber;
		}
	}
}
