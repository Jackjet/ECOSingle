using System;
namespace EcoDevice.AccessAPI
{
	internal class LineValueMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.4.1.1";
		public string Current
		{
			get
			{
				return LineValueMib.Entry + ".2." + this.index;
			}
		}
		public string Voltage
		{
			get
			{
				return LineValueMib.Entry + ".3." + this.index;
			}
		}
		public string Power
		{
			get
			{
				return LineValueMib.Entry + ".4." + this.index;
			}
		}
		public string MaxCurrent
		{
			get
			{
				return LineValueMib.Entry + ".5." + this.index;
			}
		}
		public LineValueMib(int lineNumber)
		{
			this.index = lineNumber;
		}
	}
}
