using System;
namespace EcoDevice.AccessAPI
{
	internal class OutletValueMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.2.1.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.2.1.1.2";
		public string Current
		{
			get
			{
				return OutletValueMib.Entry + ".2." + this.index;
			}
		}
		public string Voltage
		{
			get
			{
				return OutletValueMib.Entry + ".3." + this.index;
			}
		}
		public string Power
		{
			get
			{
				return OutletValueMib.Entry + ".4." + this.index;
			}
		}
		public string PowerDissipation
		{
			get
			{
				return OutletValueMib.Entry + ".5." + this.index;
			}
		}
		public OutletValueMib(int outletNumber)
		{
			this.index = outletNumber;
		}
	}
}
