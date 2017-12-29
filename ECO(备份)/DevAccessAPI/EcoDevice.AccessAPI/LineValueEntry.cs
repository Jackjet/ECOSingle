using System;
namespace EcoDevice.AccessAPI
{
	public class LineValueEntry
	{
		private int lineNumber = 1;
		private string current;
		private string voltage;
		private string power;
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}
		public string Power
		{
			get
			{
				return this.power;
			}
			set
			{
				this.power = value;
			}
		}
		public string Current
		{
			get
			{
				return this.current;
			}
			set
			{
				this.current = value;
			}
		}
		public string Voltage
		{
			get
			{
				return this.voltage;
			}
			set
			{
				this.voltage = value;
			}
		}
		public LineValueEntry(int lnNumber)
		{
			this.lineNumber = lnNumber;
		}
	}
}
