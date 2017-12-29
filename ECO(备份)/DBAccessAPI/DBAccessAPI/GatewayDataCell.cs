using System;
namespace DBAccessAPI
{
	public class GatewayDataCell
	{
		private string gid;
		private string bid;
		private string sid;
		private double pd;
		private DateTime gtime;
		public string GatewayID
		{
			get
			{
				return this.gid;
			}
			set
			{
				this.gid = value;
			}
		}
		public string BranchID
		{
			get
			{
				return this.bid;
			}
			set
			{
				this.bid = value;
			}
		}
		public string SubmeterID
		{
			get
			{
				return this.sid;
			}
			set
			{
				this.sid = value;
			}
		}
		public double PD
		{
			get
			{
				return this.pd;
			}
			set
			{
				this.pd = value;
			}
		}
		public DateTime RecordTime
		{
			get
			{
				return this.gtime;
			}
			set
			{
				this.gtime = value;
			}
		}
	}
}
