using System;
namespace EcoDevice.AccessAPI
{
	public class LineMapping
	{
		private int lineId;
		private int lineNum;
		public int LineNumber
		{
			get
			{
				return this.lineNum;
			}
			set
			{
				this.lineNum = value;
			}
		}
		public int LineId
		{
			get
			{
				return this.lineId;
			}
			set
			{
				this.lineId = value;
			}
		}
		public LineMapping(int lineNum)
		{
			this.lineNum = lineNum;
		}
	}
}
