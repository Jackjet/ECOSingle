using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
namespace DBAccessAPI
{
	public class DataCrossHourProcessing
	{
		private static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static string _dbName = "";
		private static string _dbConnectString = "";
		private static DateTime _tNextHour2Handle = DateTime.MinValue;
		private static DateTime _tNextDay2Handle = DateTime.MinValue;
		public DataCrossHourProcessing()
		{
			DataCrossHourProcessing._dbConnectString = string.Concat(new object[]
			{
				"Database=",
				DBUrl.DB_CURRENT_NAME,
				";Data Source=",
				DBUrl.CURRENT_HOST_PATH,
				";Port=",
				DBUrl.CURRENT_PORT,
				";User Id=",
				DBUrl.CURRENT_USER_NAME,
				";Password=",
				DBUrl.CURRENT_PWD,
				";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
			});
			DataCrossHourProcessing._dbName = "mycalculatordb";
			if (!DataCrossHourProcessing.sCodebase.EndsWith("/") && !DataCrossHourProcessing.sCodebase.EndsWith("\\"))
			{
				DataCrossHourProcessing.sCodebase += "\\";
			}
			this.getTimePeriod();
		}
		private string getDbTime(string itemName)
		{
			string result = "";
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			try
			{
				mySqlConnection = new MySqlConnection(DataCrossHourProcessing._dbConnectString);
				mySqlConnection.Open();
				mySqlCommand = mySqlConnection.CreateCommand();
				mySqlCommand.CommandText = "create table IF NOT EXISTS `dbextendinfo` (`serverid` varchar(255),`serverip` varchar(255),`servername` varchar(128),`servermac` varchar(128),`createtime` datetime,`lasthour` datetime,`lastday` datetime,KEY `index1` (`serverip`),KEY `index2` (`serverid`) ) ENGINE=MYISAM;";
				mySqlCommand.ExecuteNonQuery();
				mySqlCommand.CommandText = "select count(*) from dbextendinfo ";
				object obj = mySqlCommand.ExecuteScalar();
				if (obj == null || Convert.ToInt32(obj) == 0)
				{
					mySqlCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + DataCrossHourProcessing._dbName + "' )";
					mySqlCommand.ExecuteNonQuery();
				}
				mySqlCommand.CommandText = "select " + itemName + " from dbextendinfo ";
				object obj2 = mySqlCommand.ExecuteScalar();
				if (obj2 == null)
				{
					string result2 = null;
					return result2;
				}
				if (obj2 != DBNull.Value)
				{
					result = Convert.ToString(obj2);
				}
			}
			catch (Exception)
			{
				string result2 = null;
				return result2;
			}
			finally
			{
				try
				{
					if (mySqlCommand != null)
					{
						mySqlCommand.Dispose();
					}
					if (mySqlConnection != null)
					{
						mySqlConnection.Close();
						mySqlConnection.Dispose();
					}
				}
				catch
				{
				}
			}
			return result;
		}
		private void setDbTime(string itemName, string itemValue)
		{
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			try
			{
				mySqlConnection = new MySqlConnection(DataCrossHourProcessing._dbConnectString);
				mySqlConnection.Open();
				mySqlCommand = mySqlConnection.CreateCommand();
				mySqlCommand.CommandText = string.Concat(new string[]
				{
					"update dbextendinfo set ",
					itemName,
					"='",
					itemValue,
					"'"
				});
				mySqlCommand.ExecuteNonQuery();
			}
			catch (Exception)
			{
			}
			finally
			{
				try
				{
					if (mySqlCommand != null)
					{
						mySqlCommand.Dispose();
					}
					if (mySqlConnection != null)
					{
						mySqlConnection.Close();
						mySqlConnection.Dispose();
					}
				}
				catch
				{
				}
			}
		}
		private void getTimePeriod()
		{
			DataCrossHourProcessing._tNextHour2Handle = DateTime.MinValue;
			DataCrossHourProcessing._tNextDay2Handle = DateTime.MinValue;
			string dbTime = this.getDbTime("lasthour");
			if (!string.IsNullOrEmpty(dbTime))
			{
				DateTime dateTime = Convert.ToDateTime(dbTime);
				DataCrossHourProcessing._tNextHour2Handle = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
				DataCrossHourProcessing._tNextHour2Handle = DataCrossHourProcessing._tNextHour2Handle.AddHours(1.0);
			}
			string dbTime2 = this.getDbTime("lastday");
			if (!string.IsNullOrEmpty(dbTime2))
			{
				DateTime dateTime = Convert.ToDateTime(dbTime2);
				DataCrossHourProcessing._tNextDay2Handle = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				DataCrossHourProcessing._tNextDay2Handle = DataCrossHourProcessing._tNextDay2Handle.AddDays(1.0);
			}
		}
		public void Execute(CrossHourContext context)
		{
			if (!context.bCrossHour)
			{
				return;
			}
			Thread thread = new Thread(new ParameterizedThreadStart(this.CrossHourProcessing));
			thread.Name = "CrossHourProcessing";
			thread.IsBackground = true;
			thread.Start(context);
			thread.Join();
		}
		private void CrossHourProcessing(object obj)
		{
			CrossHourContext crossHourContext = (CrossHourContext)obj;
			DateTime tInsertTime = crossHourContext.tInsertTime;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<DateTime> list3 = new List<DateTime>();
			List<string> list4 = new List<string>();
			List<DateTime> list5 = new List<DateTime>();
			DateTime dateTime = new DateTime(tInsertTime.Year, tInsertTime.Month, tInsertTime.Day, tInsertTime.Hour, 0, 0);
			DateTime dateTime2 = DataCrossHourProcessing._tNextHour2Handle;
			if (dateTime2 == DateTime.MinValue)
			{
				dateTime2 = dateTime;
			}
			while (dateTime2 <= dateTime)
			{
				this.PrepareTables(dateTime2);
				string text = dateTime2.ToString("yyyyMMdd");
				string text2 = string.Format("insert into dev_hour_pd_{0} (device_id,power_consumption,power_max,insert_time) ", text);
				text2 += string.Format(" (select device_id,sum(power_consumption),max(power),{0} from dev_min_pw_pd_{1} ", dateTime2.Hour, text);
				text2 += string.Format(" where (insert_time >= {0} and insert_time <= {1}) group by device_id)", dateTime2.Hour * 60, dateTime2.Hour * 60 + 59);
				list2.Add(text2);
				list3.Add(dateTime2);
				if (!list.Contains("dev_hour_pd_" + text))
				{
					list.Add("dev_hour_pd_" + text);
				}
				text2 = string.Format("insert into port_hour_pd_{0} (port_id,power_consumption,power_max,insert_time) ", text);
				text2 += string.Format(" (select port_id,sum(power_consumption),max(power),{0} from port_min_pw_pd_{1} ", dateTime2.Hour, text);
				text2 += string.Format(" where (insert_time >= {0} and insert_time <= {1}) group by port_id)", dateTime2.Hour * 60, dateTime2.Hour * 60 + 59);
				list2.Add(text2);
				list3.Add(dateTime2);
				if (!list.Contains("port_hour_pd_" + text))
				{
					list.Add("port_hour_pd_" + text);
				}
				dateTime2 = dateTime2.AddHours(1.0);
			}
			DateTime dateTime3 = new DateTime(tInsertTime.Year, tInsertTime.Month, tInsertTime.Day, 0, 0, 0);
			DateTime dateTime4 = DataCrossHourProcessing._tNextDay2Handle;
			if (dateTime4 == DateTime.MinValue)
			{
				dateTime4 = dateTime3;
			}
			while (dateTime4 <= dateTime3 && crossHourContext.bCrossDay)
			{
				string text = dateTime4.ToString("yyyyMMdd");
				string text2 = string.Format("insert into dev_day_pd_pm_{0} (device_id,power_consumption,power_max) ", text);
				text2 += string.Format(" (select device_id,sum(power_consumption),max(power) from dev_hour_pd_{0} group by device_id)", text);
				list4.Add(text2);
				list5.Add(dateTime4);
				if (!list.Contains("dev_day_pd_pm_" + text))
				{
					list.Add("dev_day_pd_pm_" + text);
				}
				text2 = string.Format("insert into port_day_pd_pm_{0} (port_id,power_consumption,power_max) ", text);
				text2 += string.Format(" (select port_id,sum(power_consumption),max(power) from port_hour_pd_{0} group by port_id)", text);
				list4.Add(text2);
				list5.Add(dateTime4);
				if (!list.Contains("port_day_pd_pm_" + text))
				{
					list.Add("port_day_pd_pm_" + text);
				}
				dateTime4 = dateTime4.AddDays(1.0);
			}
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				dbConnection = new MySqlConnection(DataCrossHourProcessing._dbConnectString);
				dbConnection.Open();
				dbCommand = dbConnection.CreateCommand();
				for (int i = 0; i < list3.Count; i++)
				{
					try
					{
						dbCommand.CommandText = list2[i];
						dbCommand.ExecuteNonQuery();
						this.setDbTime("lasthour", list3[i].ToString("yyyy-MM-dd HH:mm:ss"));
					}
					catch (Exception)
					{
					}
				}
				if (crossHourContext.bCrossDay)
				{
					for (int j = 0; j < list5.Count; j++)
					{
						try
						{
							dbCommand.CommandText = list4[j];
							dbCommand.ExecuteNonQuery();
							this.setDbTime("lastday", list5[j].ToString("yyyy-MM-dd HH:mm:ss"));
						}
						catch (Exception)
						{
						}
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					dbCommand.CommandText = "OPTIMIZE TABLE " + list[k];
					dbCommand.ExecuteNonQuery();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				try
				{
					if (dbCommand != null)
					{
						dbCommand.Dispose();
					}
					if (dbConnection != null)
					{
						dbConnection.Close();
						dbConnection.Dispose();
					}
				}
				catch
				{
				}
			}
		}
		private bool IsNumeric(string value)
		{
			string text = "0123456789";
			for (int i = 0; i < value.Length; i++)
			{
				char value2 = value[i];
				if (text.IndexOf(value2) < 0)
				{
					return false;
				}
			}
			return true;
		}
		private List<string> GetInsertTimeList(string strTableName)
		{
			List<string> list = new List<string>();
			MySqlConnection mySqlConnection = null;
			try
			{
				mySqlConnection = new MySqlConnection(DataCrossHourProcessing._dbConnectString);
				mySqlConnection.Open();
				MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
				mySqlCommand.CommandText = string.Format("SELECT insert_time FROM {0} group by insert_time", strTableName);
				DbDataReader dbDataReader = mySqlCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					string text = Convert.ToString(dbDataReader.GetValue(0));
					if (!list.Contains(text) && this.IsNumeric(text))
					{
						list.Add(text);
					}
				}
				dbDataReader.Close();
				mySqlCommand.Dispose();
				mySqlConnection.Close();
			}
			catch (Exception)
			{
				if (mySqlConnection != null)
				{
					mySqlConnection.Close();
				}
			}
			list.Sort();
			return list;
		}
		private List<string> GetAllTableSuffixes(string strTablePrefix)
		{
			List<string> list = new List<string>();
			MySqlConnection mySqlConnection = null;
			try
			{
				mySqlConnection = new MySqlConnection(DataCrossHourProcessing._dbConnectString);
				mySqlConnection.Open();
				MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
				mySqlCommand.CommandText = string.Format("SELECT table_name FROM INFORMATION_SCHEMA.TABLES where table_name like '{0}%' and table_schema = '" + DataCrossHourProcessing._dbName + "' ", strTablePrefix);
				DbDataReader dbDataReader = mySqlCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					string text = Convert.ToString(dbDataReader.GetValue(0));
					text = text.Trim();
					if (text.Length > 8)
					{
						string text2 = text.Substring(text.Length - 8, 8);
						if (!list.Contains(text2) && this.IsNumeric(text2))
						{
							list.Add(text2);
						}
					}
				}
				dbDataReader.Close();
				mySqlCommand.Dispose();
				mySqlConnection.Close();
			}
			catch (Exception)
			{
				if (mySqlConnection != null)
				{
					mySqlConnection.Close();
				}
			}
			list.Sort();
			return list;
		}
		private void PrepareTables(DateTime dataTime)
		{
			string arg = dataTime.ToString("yyyyMMdd");
			string text = "CREATE TABLE IF NOT EXISTS `dev_hour_pd_{0}` (";
			text += "`device_id` smallint NOT NULL,";
			text += "`power_consumption` int DEFAULT NULL,";
			text += "`power_max` int DEFAULT NULL,";
			text += "`insert_time` tinyint NOT NULL";
			text += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text2 = "CREATE TABLE IF NOT EXISTS `dev_day_pd_pm_{0}` (";
			text2 += "`device_id` smallint NOT NULL,";
			text2 += "`power_consumption` bigint DEFAULT NULL,";
			text2 += "`power_max` int DEFAULT NULL";
			text2 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text3 = "CREATE TABLE IF NOT EXISTS `port_hour_pd_{0}` (";
			text3 += "`port_id` mediumint NOT NULL,";
			text3 += "`power_consumption` int DEFAULT NULL,";
			text3 += "`power_max` int DEFAULT NULL,";
			text3 += "`insert_time` tinyint NOT NULL";
			text3 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text4 = "CREATE TABLE IF NOT EXISTS `port_day_pd_pm_{0}` (";
			text4 += "`port_id` mediumint NOT NULL,";
			text4 += "`power_consumption` int DEFAULT NULL,";
			text4 += "`power_max` int NOT NULL";
			text4 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			try
			{
				mySqlConnection = new MySqlConnection(DataCrossHourProcessing._dbConnectString);
				mySqlConnection.Open();
				mySqlCommand = mySqlConnection.CreateCommand();
				mySqlCommand.CommandText = string.Format(text, arg);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text2, arg);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text3, arg);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text4, arg);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					mySqlCommand.Dispose();
				}
				catch
				{
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				try
				{
					if (mySqlCommand != null)
					{
						mySqlCommand.Dispose();
					}
					if (mySqlConnection != null)
					{
						mySqlConnection.Close();
						mySqlConnection.Dispose();
					}
				}
				catch
				{
				}
			}
		}
	}
}
