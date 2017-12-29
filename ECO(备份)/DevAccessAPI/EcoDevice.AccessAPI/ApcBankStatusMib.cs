using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcBankStatusMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.8.3.1";
		public string CurrentStatus
		{
			get
			{
				return ApcBankStatusMib.Entry + ".5." + this.index;
			}
		}
		public ApcBankStatusMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
