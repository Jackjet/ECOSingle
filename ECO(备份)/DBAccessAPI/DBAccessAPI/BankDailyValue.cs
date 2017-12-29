using CommonAPI;
using CommonAPI.CultureTransfer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class BankDailyValue
	{
		private int bank_id;
		private double power_consumption;
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
		public double PowerDispassion
		{
			get
			{
				return this.power_consumption;
			}
			set
			{
				this.power_consumption = value;
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
		public int Update(DBConn conn)
		{
			string text = this.insert_time.ToString("yyyyMMdd");
			try
			{
				long num = Convert.ToInt64(this.power_consumption * 10000.0);
				if (DBUrl.SERVERMODE)
				{
					string item = string.Concat(new object[]
					{
						"update bank_data_daily",
						text,
						" set power_consumption = power_consumption + ",
						CultureTransfer.ToString((float)num),
						" where bank_id = ",
						this.bank_id,
						" and insert_time = #",
						this.insert_time.ToString("yyyy-MM-dd"),
						"#"
					});
					string item2 = string.Concat(new object[]
					{
						"update bank_data_hourly",
						text,
						" set power_consumption = power_consumption + ",
						CultureTransfer.ToString((float)num),
						" where bank_id = ",
						this.bank_id,
						" and insert_time = #",
						new DateTime(this.insert_time.Year, this.insert_time.Month, this.insert_time.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
						"#"
					});
					WorkQueue<string>.getInstance_pd().WorkSequential = true;
					WorkQueue<string>.getInstance_pd().EnqueueItem(item);
					WorkQueue<string>.getInstance_pd().EnqueueItem(item2);
				}
				else
				{
					if (DBUrl.DB_CURRENT_TYPE.Equals("MYSQL"))
					{
						string item3 = string.Concat(new object[]
						{
							"update bank_data_daily",
							text,
							" set power_consumption = power_consumption + ",
							CultureTransfer.ToString((float)num),
							" where bank_id = ",
							this.bank_id,
							" and insert_time = #",
							this.insert_time.ToString("yyyy-MM-dd"),
							"#"
						});
						string item4 = string.Concat(new object[]
						{
							"update bank_data_hourly",
							text,
							" set power_consumption = power_consumption + ",
							CultureTransfer.ToString((float)num),
							" where bank_id = ",
							this.bank_id,
							" and insert_time = #",
							new DateTime(this.insert_time.Year, this.insert_time.Month, this.insert_time.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
							"#"
						});
						WorkQueue<string>.getInstance_pd().WorkSequential = true;
						WorkQueue<string>.getInstance_pd().EnqueueItem(item3);
						WorkQueue<string>.getInstance_pd().EnqueueItem(item4);
					}
					else
					{
						List<string> list = new List<string>();
						list.Add(string.Concat(this.bank_id));
						list.Add(this.insert_time.ToString("yyyy-MM-dd HH:mm:ss"));
						list.Add("bank_data_daily");
						list.Add(CultureTransfer.ToString((float)num));
						MinuteDataProcess.GetInstance().PutItem(list);
						List<string> list2 = new List<string>();
						list2.Add(string.Concat(this.bank_id));
						list2.Add(this.insert_time.ToString("yyyy-MM-dd HH:mm:ss"));
						list2.Add("bank_data_hourly");
						list2.Add(CultureTransfer.ToString((float)num));
						MinuteDataProcess.GetInstance().PutItem(list2);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return 0;
		}
		public static DataTable GetAllBankPD()
		{
			DataTable dataTable = new DataTable();
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			string commandText;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
			{
				commandText = "select * from bank_data_daily" + DateTime.Now.ToString("yyyyMMdd");
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = commandText;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						dbCommand.Dispose();
						dBConn.Close();
					}
					return dataTable;
				}
				catch (Exception)
				{
					try
					{
						dbDataAdapter.Dispose();
					}
					catch
					{
					}
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					return dataTable;
				}
			}
			commandText = "select * from bank_data_daily ";
			try
			{
				dBConn = DBConnPool.getDynaConnection(DateTime.Now);
				if (dBConn != null && dBConn.con != null)
				{
					dbDataAdapter = new OleDbDataAdapter();
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = commandText;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					dbCommand.Dispose();
					dBConn.Close();
				}
			}
			catch (Exception)
			{
				try
				{
					dbDataAdapter.Dispose();
				}
				catch
				{
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
			}
			return dataTable;
		}
		public static double GetLastDailyPD(DataTable dt_bankPD, int i_bid)
		{
			try
			{
				if (dt_bankPD != null)
				{
					DataRow[] array = dt_bankPD.Select(" bank_id = " + i_bid);
					double num = Convert.ToDouble(array[0][1]);
					return num / 10000.0;
				}
			}
			catch (Exception)
			{
			}
			return 0.0;
		}
	}
}
