using System;
namespace DBAccessAPI
{
	public class SubMeter
	{
		public const int ITPOWER = 1;
		public const int NONITPOWER = 2;
		public const int UNCONFIRMED = 0;
		private string gid;
		private string bid;
		private string sid;
		private string sname;
		private float capacity;
		private int usage;
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
		public string SubmeterName
		{
			get
			{
				return this.sname;
			}
			set
			{
				this.sname = value;
			}
		}
		public float Capacity
		{
			get
			{
				return this.capacity;
			}
			set
			{
				this.capacity = value;
			}
		}
		public int ElectricityUsage
		{
			get
			{
				return this.usage;
			}
			set
			{
				this.usage = value;
			}
		}
		public SubMeter(string str_gid, string str_bid, string str_sid, string str_name, float f_capacity, int i_usage)
		{
			this.gid = str_gid;
			this.bid = str_bid;
			this.sid = str_sid;
			this.sname = str_name;
			this.capacity = f_capacity;
			this.usage = i_usage;
		}
		public SubMeter()
		{
			this.gid = "";
			this.bid = "";
			this.sid = "";
			this.sname = "";
			this.capacity = 0f;
			this.usage = 2;
		}
	}
}
