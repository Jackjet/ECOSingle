using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcBankConfigMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.8.1.1";
		public string MinCurrentConfig
		{
			get
			{
				return ApcBankConfigMib.Entry + ".5." + this.index;
			}
		}
		public string MaxCurrentConfig
		{
			get
			{
				return ApcBankConfigMib.Entry + ".7." + this.index;
			}
		}
		public string NearCurrentConfig
		{
			get
			{
				return ApcBankConfigMib.Entry + ".6." + this.index;
			}
		}
		public ApcBankConfigMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
