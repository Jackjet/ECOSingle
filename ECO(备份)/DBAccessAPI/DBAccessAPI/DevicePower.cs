using CommonAPI;
using System;
using System.Collections.Generic;
namespace DBAccessAPI
{
	public class DevicePower
	{
		private long device_id = -1L;
		private DateTime insert_time;
		private double power_value = -1.0;
		public long DeviceId
		{
			get
			{
				return this.device_id;
			}
			set
			{
				this.device_id = value;
			}
		}
		public double Power
		{
			get
			{
				return this.power_value;
			}
			set
			{
				this.power_value = value;
			}
		}
		public DateTime InsertTime
		{
			get
			{
				return this.insert_time;
			}
			set
			{
				this.insert_time = value;
			}
		}
		public DevicePower(int i_deviceid, double f_power, DateTime intime)
		{
			this.device_id = (long)i_deviceid;
			this.power_value = f_power;
			this.insert_time = intime;
		}
		public static void InsertDevicePower(int i_deviceid, double f_power, DateTime dt_inserttime, DBConn conn)
		{
			try
			{
				long num = Convert.ToInt64(f_power * 10000.0);
				if (DBUrl.SERVERMODE)
				{
					string item = string.Concat(new object[]
					{
						"insert into device_auto_info",
						dt_inserttime.ToString("yyyyMMdd"),
						" (device_id,power,insert_time ) values(",
						i_deviceid,
						",",
						num,
						",#",
						dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
						"#)"
					});
					WorkQueue<string>.getInstance().WorkSequential = true;
					WorkQueue<string>.getInstance().EnqueueItem(item);
				}
				else
				{
					if (DBUrl.DB_CURRENT_TYPE.Equals("MYSQL"))
					{
						string item2 = string.Concat(new object[]
						{
							"insert into device_auto_info",
							dt_inserttime.ToString("yyyyMMdd"),
							" (device_id,power,insert_time ) values(",
							i_deviceid,
							",",
							num,
							",#",
							dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
							"#)"
						});
						WorkQueue<string>.getInstance().WorkSequential = true;
						WorkQueue<string>.getInstance().EnqueueItem(item2);
					}
					else
					{
						List<string> list = new List<string>();
						list.Add(string.Concat(i_deviceid));
						list.Add(dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"));
						list.Add("device_auto_info");
						list.Add(string.Concat(num));
						PDDataProcess.GetInstance().PutItem(list);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
	}
}
