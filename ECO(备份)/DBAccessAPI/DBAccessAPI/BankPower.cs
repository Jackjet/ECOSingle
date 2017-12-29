using CommonAPI;
using System;
using System.Collections.Generic;
namespace DBAccessAPI
{
	public class BankPower
	{
		private int bank_id;
		private double power_value = -1.0;
		private DateTime insert_time;
		public int BankId
		{
			get
			{
				return this.bank_id;
			}
			set
			{
				this.bank_id = value;
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
		public BankPower(int l_bankid, double f_power, DateTime para_datetime)
		{
			this.bank_id = l_bankid;
			this.power_value = f_power;
			this.insert_time = para_datetime;
		}
		public static void InsertBankPower(int i_bankid, double f_power, DateTime dt_inserttime, DBConn conn)
		{
			try
			{
				long num = Convert.ToInt64(f_power * 10000.0);
				if (DBUrl.SERVERMODE)
				{
					string item = string.Concat(new object[]
					{
						"insert into bank_auto_info",
						dt_inserttime.ToString("yyyyMMdd"),
						" (bank_id,power,insert_time ) values(",
						i_bankid,
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
							"insert into bank_auto_info",
							dt_inserttime.ToString("yyyyMMdd"),
							" (bank_id,power,insert_time ) values(",
							i_bankid,
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
						list.Add(string.Concat(i_bankid));
						list.Add(dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"));
						list.Add("bank_auto_info");
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
