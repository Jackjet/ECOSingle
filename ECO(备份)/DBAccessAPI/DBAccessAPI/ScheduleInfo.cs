using System;
using System.Data;
namespace DBAccessAPI
{
	public class ScheduleInfo
	{
		private int obj_id;
		private int i_dayofweek;
		private int i_optype;
		private DateTime dt_scheduletime;
		private int i_status;
		private string str_reserve;
		public int ObjectID
		{
			get
			{
				return this.obj_id;
			}
			set
			{
				this.obj_id = value;
			}
		}
		public int DayOfWeek
		{
			get
			{
				return this.i_dayofweek;
			}
			set
			{
				this.i_dayofweek = value;
			}
		}
		public int OperationType
		{
			get
			{
				return this.i_optype;
			}
			set
			{
				this.i_optype = value;
			}
		}
		public DateTime ScheduleTime
		{
			get
			{
				return this.dt_scheduletime;
			}
			set
			{
				this.dt_scheduletime = value;
			}
		}
		public int Status
		{
			get
			{
				return this.i_status;
			}
			set
			{
				this.i_status = value;
			}
		}
		public string Reserve
		{
			get
			{
				return this.str_reserve;
			}
			set
			{
				this.str_reserve = value;
			}
		}
		public ScheduleInfo(DataRow row)
		{
			this.obj_id = Convert.ToInt32(row["groupid"]);
			this.i_dayofweek = Convert.ToInt32(row["dayofweek"]);
			this.i_optype = Convert.ToInt32(row["optype"]);
			try
			{
				this.dt_scheduletime = Convert.ToDateTime(row["scheduletime"]);
				this.dt_scheduletime = DateTime.ParseExact(this.dt_scheduletime.ToString("HH:mm:ss"), "HH:mm:ss", null);
			}
			catch (Exception)
			{
			}
			object obj = row["status"];
			if (obj != null && obj != DBNull.Value)
			{
				try
				{
					this.i_status = Convert.ToInt32(row["status"]);
				}
				catch
				{
				}
			}
			obj = row["reserve"];
			if (obj != null && obj != DBNull.Value)
			{
				try
				{
					this.str_reserve = Convert.ToString(row["reserve"]);
				}
				catch
				{
				}
			}
		}
	}
}
