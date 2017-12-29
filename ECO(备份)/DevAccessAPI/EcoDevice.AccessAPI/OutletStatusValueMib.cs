using System;
namespace EcoDevice.AccessAPI
{
	internal class OutletStatusValueMib
	{
		private int outletNumber = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.1.5.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.1.5.1.2";
		public string OutletStatus
		{
			get
			{
				return OutletStatusValueMib.Column + "." + this.outletNumber;
			}
		}
		public OutletStatusValueMib(int outletNumber)
		{
			this.outletNumber = outletNumber;
		}
	}
}
