using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcPhaseStatusMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.6.3.1";
		public string CurrentStatus
		{
			get
			{
				return ApcPhaseStatusMib.Entry + ".5." + this.index;
			}
		}
		public string VoltageStatus
		{
			get
			{
				return ApcPhaseStatusMib.Entry + ".6." + this.index;
			}
		}
		public ApcPhaseStatusMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
