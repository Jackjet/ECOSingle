using System;
namespace DBAccessAPI
{
	public class CrossHourContext
	{
		public DateTime tInsertTime;
		public bool bCrossHour;
		public bool bCrossDay;
		public CrossHourContext()
		{
			this.tInsertTime = DateTime.Now;
			this.bCrossHour = false;
			this.bCrossDay = false;
		}
	}
}
