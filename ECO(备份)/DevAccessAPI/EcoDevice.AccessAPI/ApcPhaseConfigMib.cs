using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcPhaseConfigMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.6.1.1";
		public string MinCurrentConfig
		{
			get
			{
				return ApcPhaseConfigMib.Entry + ".5." + this.index;
			}
		}
		public string MaxCurrentConfig
		{
			get
			{
				return ApcPhaseConfigMib.Entry + ".7." + this.index;
			}
		}
		public string NearCurrentConfig
		{
			get
			{
				return ApcPhaseConfigMib.Entry + ".6." + this.index;
			}
		}
		public ApcPhaseConfigMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
