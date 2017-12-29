using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveOutletStatusValueMib
	{
		private int outletIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string outletStatusValueEntry = ".2.3";
		public static string Status = ".2.3.";
		public string SlaveOutletStatus
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletStatusValueMib.Entry,
					this.slave,
					SlaveOutletStatusValueMib.Status,
					this.outletIndex
				});
			}
		}
		public SlaveOutletStatusValueMib(int index, int outletnumber)
		{
			this.slave += index;
			this.outletIndex = outletnumber;
		}
	}
}
