using System;
namespace EcoDevice.AccessAPI
{
	internal class LineEnvironmentMib
	{
		private int linenumber = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.4.3.1";
		public string LineMinCurMT
		{
			get
			{
				return LineEnvironmentMib.Entry + ".2." + this.linenumber;
			}
		}
		public string LineMaxCurMT
		{
			get
			{
				return LineEnvironmentMib.Entry + ".3." + this.linenumber;
			}
		}
		public string LineMinVolMT
		{
			get
			{
				return LineEnvironmentMib.Entry + ".4." + this.linenumber;
			}
		}
		public string LineMaxVolMT
		{
			get
			{
				return LineEnvironmentMib.Entry + ".5." + this.linenumber;
			}
		}
		public string LineMinPMT
		{
			get
			{
				return LineEnvironmentMib.Entry + ".6." + this.linenumber;
			}
		}
		public string LineMaxPMT
		{
			get
			{
				return LineEnvironmentMib.Entry + ".7." + this.linenumber;
			}
		}
		public LineEnvironmentMib(int linenumber)
		{
			this.linenumber = linenumber;
		}
	}
}
