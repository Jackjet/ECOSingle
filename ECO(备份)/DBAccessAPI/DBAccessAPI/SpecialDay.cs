using System;
namespace DBAccessAPI
{
	public class SpecialDay
	{
		private string sp_day = "";
		private string st_on = "";
		private string st_off = "";
		public string SpecialDate
		{
			get
			{
				return this.sp_day;
			}
			set
			{
				this.sp_day = value;
			}
		}
		public string ONTime
		{
			get
			{
				return this.st_on;
			}
			set
			{
				this.st_on = value;
			}
		}
		public string OFFTime
		{
			get
			{
				return this.st_off;
			}
			set
			{
				this.st_off = value;
			}
		}
		public SpecialDay(string st_day, string str_on, string str_off)
		{
			this.sp_day = st_day;
			this.st_off = str_off;
			this.st_on = str_on;
		}
	}
}
