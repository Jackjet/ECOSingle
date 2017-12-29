using System;
namespace DBAccessAPI
{
	public class BillReportInfo
	{
		public const string PERIOD_PEAK = "PEAK";
		public const string PERIOD_NONPEAK = "NON_PEAK";
		public const string PERIOD_ALL = "ALL";
		private long l_kwh;
		private int i_hour;
		private string str_month = "";
		private string str_rackname = "";
		private string str_rackid = "";
		private string str_groupid = "";
		private string str_groupname = "";
		private string str_period = "";
		public string PERIOD
		{
			get
			{
				return this.str_period;
			}
			set
			{
				this.str_period = value;
			}
		}
		public string GROUPNAME
		{
			get
			{
				return this.str_groupname;
			}
			set
			{
				this.str_groupname = value;
			}
		}
		public string GROUPID
		{
			get
			{
				return this.str_groupid;
			}
			set
			{
				this.str_groupid = value;
			}
		}
		public string RACKID
		{
			get
			{
				return this.str_rackid;
			}
			set
			{
				this.str_rackid = value;
			}
		}
		public long KWH
		{
			get
			{
				return this.l_kwh;
			}
			set
			{
				this.l_kwh = value;
			}
		}
		public int TIMESPAN_HOUR
		{
			get
			{
				return this.i_hour;
			}
			set
			{
				this.i_hour = value;
			}
		}
		public string MONTH
		{
			get
			{
				return this.str_month;
			}
			set
			{
				this.str_month = value;
			}
		}
		public string RACKNAME
		{
			get
			{
				return this.str_rackname;
			}
			set
			{
				this.str_rackname = value;
			}
		}
		public BillReportInfo(long kwh, int hour, string period)
		{
			this.l_kwh = kwh;
			this.i_hour = hour;
			this.str_period = period;
		}
	}
}
