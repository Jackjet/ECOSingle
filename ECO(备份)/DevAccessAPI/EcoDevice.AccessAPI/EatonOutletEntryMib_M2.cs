using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonOutletEntryMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.6.1.1";
		public string OutletName
		{
			get
			{
				return EatonOutletEntryMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public EatonOutletEntryMib_M2(int outletNum)
		{
			this.index = outletNum;
		}
	}
}
