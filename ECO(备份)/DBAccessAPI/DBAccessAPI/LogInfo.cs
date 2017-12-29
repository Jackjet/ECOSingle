using CommonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class LogInfo
	{
		public const string PARA_SEPARATOR = "^|^";
		public const int RANGE_TYPE_ALL = 0;
		public const int RANGE_TYPE_INCLUDE = 1;
		public const int RANGE_TYPE_EXCLUDE = 2;
		public const string LOG_DATETIME = "insert_time";
		public const string LOG_ID = "eventid";
		public const string LOG_PARA = "parametervalue";
		public const string LOG_KEY = "id";
		public static LogProcess lp_thread;
		private static int m_type = -1;
		private static DateTime mdt_start = new DateTime(2000, 1, 1, 0, 0, 0);
		private static DateTime mdt_end = new DateTime(2000, 1, 1, 0, 0, 0);
		private static Hashtable ht_cache = new Hashtable();
		private long id;
		private string category = string.Empty;
		private string severity = string.Empty;
		private string eventid = string.Empty;
		private string parameters = string.Empty;
		private DateTime RecordTime;
		public DateTime InsertTime
		{
			get
			{
				return this.RecordTime;
			}
			set
			{
				this.RecordTime = value;
			}
		}
		public long ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public string Event
		{
			get
			{
				return this.eventid;
			}
			set
			{
				this.eventid = value;
			}
		}
		public string Parameters
		{
			get
			{
				return this.parameters;
			}
			set
			{
				this.parameters = value;
			}
		}
		public string Category
		{
			get
			{
				return this.category;
			}
			set
			{
				this.category = value;
			}
		}
		public string Severity
		{
			get
			{
				return this.severity;
			}
			set
			{
				this.severity = value;
			}
		}
		public LogInfo(DataRow tmp_dr_li)
		{
			if (tmp_dr_li != null)
			{
				this.id = Convert.ToInt64(tmp_dr_li["id"]);
				this.eventid = Convert.ToString(tmp_dr_li["eventid"]);
				this.category = this.eventid.Substring(0, 2);
				this.severity = this.eventid.Substring(2, 1);
				this.parameters = Convert.ToString(tmp_dr_li["logpara"]);
				this.RecordTime = new DateTime(Convert.ToInt64(tmp_dr_li["ticks"]));
			}
		}
		public LogInfo(long l_id, string str_event, string str_para, DateTime dt_inserttime)
		{
			this.id = l_id;
			this.eventid = str_event;
			this.parameters = str_para;
			this.category = this.eventid.Substring(0, 2);
			this.severity = this.eventid.Substring(2, 1);
			this.RecordTime = dt_inserttime;
		}
		public static DataTable GetSearchResult(int i_range_type, DateTime dt_start, DateTime dt_end, int i_pagenum, int i_pagesize, int i_total)
		{
			DateTime now = DateTime.Now;
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("insert_time");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("eventid");
			dataColumn2.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn2);
			DataColumn dataColumn3 = new DataColumn("parametervalue");
			dataColumn3.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn3);
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			long arg_95_0 = dt_start.Ticks;
			long arg_9D_0 = dt_end.Ticks;
			string text = "";
			switch (i_range_type)
			{
			case 0:
				text = " 2=2 ";
				break;
			case 1:
				text = string.Concat(new string[]
				{
					" 2=2 and ticks >= #",
					dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
					"# and ticks <= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# "
				});
				break;
			case 2:
				text = string.Concat(new string[]
				{
					" 2=2 and (ticks <= #",
					dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
					"# or ticks >= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# )"
				});
				break;
			}
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (i_pagenum == 0 && i_pagesize == 0)
					{
						dbCommand.CommandText = "select ticks,eventid,logpara from logrecords where " + text + " order by id desc";
						if (DBUrl.SERVERMODE)
						{
							dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
						}
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							string value = Convert.ToDateTime(dbDataReader.GetValue(0)).ToString("yyyy-MM-dd HH:mm:ss");
							string @string = dbDataReader.GetString(1);
							string string2 = dbDataReader.GetString(2);
							DataRow dataRow = dataTable.NewRow();
							dataRow["insert_time"] = value;
							dataRow["eventid"] = @string;
							dataRow["parametervalue"] = string2;
							dataTable.Rows.Add(dataRow);
						}
						dbDataReader.Close();
					}
					else
					{
						if (LogInfo.m_type == i_range_type && DateTime.Compare(LogInfo.mdt_start, dt_start) == 0 && DateTime.Compare(LogInfo.mdt_end, dt_end) == 0)
						{
							long num;
							long num2;
							if (i_pagenum * i_pagesize > i_total)
							{
								num = Convert.ToInt64(LogInfo.ht_cache[(i_pagenum - 1) * i_pagesize + 1]);
								num2 = Convert.ToInt64(LogInfo.ht_cache[i_total]);
							}
							else
							{
								num = Convert.ToInt64(LogInfo.ht_cache[(i_pagenum - 1) * i_pagesize + 1]);
								num2 = Convert.ToInt64(LogInfo.ht_cache[i_pagenum * i_pagesize]);
							}
							dbCommand.CommandText = string.Concat(new object[]
							{
								"select * from logrecords where id >= ",
								num2,
								" and id <= ",
								num,
								" order by id desc"
							});
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
							}
							DbDataReader dbDataReader2 = dbCommand.ExecuteReader();
							while (dbDataReader2.Read())
							{
								string value2 = Convert.ToDateTime(dbDataReader2.GetValue(1)).ToString("yyyy-MM-dd HH:mm:ss");
								string string3 = dbDataReader2.GetString(2);
								string string4 = dbDataReader2.GetString(3);
								DataRow dataRow2 = dataTable.NewRow();
								dataRow2["insert_time"] = value2;
								dataRow2["eventid"] = string3;
								dataRow2["parametervalue"] = string4;
								dataTable.Rows.Add(dataRow2);
							}
							dbDataReader2.Close();
						}
						else
						{
							LogInfo.m_type = i_range_type;
							LogInfo.mdt_start = dt_start;
							LogInfo.mdt_end = dt_end;
							LogInfo.ht_cache = new Hashtable();
							dbCommand.CommandText = "select id from logrecords where " + text + " order by id desc";
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
							}
							DbDataReader dbDataReader3 = dbCommand.ExecuteReader();
							int num3 = 1;
							while (dbDataReader3.Read())
							{
								LogInfo.ht_cache.Add(num3, dbDataReader3.GetValue(0));
								num3++;
							}
							dbDataReader3.Close();
							new DataTable();
							long num5;
							long num6;
							if (i_pagenum * i_pagesize > i_total)
							{
								int num4 = i_pagenum * i_pagesize - i_total;
								string.Concat(new object[]
								{
									"select top ",
									i_pagesize - num4,
									" id  from (select top ",
									i_pagesize - num4,
									" id from logrecords where ",
									text,
									" order by id asc) order by id desc"
								});
								num5 = Convert.ToInt64(LogInfo.ht_cache[(i_pagenum - 1) * i_pagesize + 1]);
								num6 = Convert.ToInt64(LogInfo.ht_cache[i_total]);
							}
							else
							{
								num5 = Convert.ToInt64(LogInfo.ht_cache[(i_pagenum - 1) * i_pagesize + 1]);
								num6 = Convert.ToInt64(LogInfo.ht_cache[i_pagenum * i_pagesize]);
								if (i_pagenum * i_pagesize > i_total / 2)
								{
									string.Concat(new object[]
									{
										"select top ",
										i_pagesize,
										" id  from (select top ",
										i_total - i_pagenum * i_pagesize + i_pagesize,
										" id from logrecords where ",
										text,
										" order by id asc) order by id desc"
									});
								}
								else
								{
									string.Concat(new object[]
									{
										"select id from (select top ",
										i_pagesize,
										" id  from (select top ",
										i_pagenum * i_pagesize,
										" id from logrecords where ",
										text,
										" order by id desc) order by id asc) order by id desc"
									});
								}
							}
							dbCommand.CommandText = string.Concat(new object[]
							{
								"select * from logrecords where id >= ",
								num6,
								" and id <= ",
								num5,
								" order by id desc"
							});
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
							}
							DbDataReader dbDataReader4 = dbCommand.ExecuteReader();
							while (dbDataReader4.Read())
							{
								string value3 = Convert.ToDateTime(dbDataReader4.GetValue(1)).ToString("yyyy-MM-dd HH:mm:ss");
								string string5 = dbDataReader4.GetString(2);
								string string6 = dbDataReader4.GetString(3);
								DataRow dataRow3 = dataTable.NewRow();
								dataRow3["insert_time"] = value3;
								dataRow3["eventid"] = string5;
								dataRow3["parametervalue"] = string6;
								dataTable.Rows.Add(dataRow3);
							}
							dbDataReader4.Close();
						}
					}
					dbCommand.Dispose();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			Console.WriteLine("million sencods is : " + (DateTime.Now - now).TotalMilliseconds.ToString());
			return dataTable;
		}
		public static DataTable GetNextPage(int i_key, int i_range_type, DateTime dt_start, DateTime dt_end, int i_pagenum, int i_pagesize, int i_total)
		{
			DateTime now = DateTime.Now;
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("insert_time");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("eventid");
			dataColumn2.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn2);
			DataColumn dataColumn3 = new DataColumn("parametervalue");
			dataColumn3.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn3);
			DataColumn dataColumn4 = new DataColumn("id");
			dataColumn4.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn4);
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			long arg_BF_0 = dt_start.Ticks;
			long arg_C7_0 = dt_end.Ticks;
			string text = "";
			switch (i_range_type)
			{
			case 0:
				text = string.Concat(new object[]
				{
					" 2=2 id < ",
					i_key,
					" and ticks <= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# "
				});
				break;
			case 1:
				text = string.Concat(new object[]
				{
					" 2=2 id < ",
					i_key,
					" and ticks >= #",
					dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
					"# and ticks <= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# "
				});
				break;
			case 2:
				text = string.Concat(new object[]
				{
					" 2=2 id < ",
					i_key,
					" and (ticks <= #",
					dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
					"# or ticks >= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# )"
				});
				break;
			}
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					long num = 0L;
					long num2 = 0L;
					DataTable dataTable2 = new DataTable();
					string commandText;
					if (i_pagenum * i_pagesize > i_total)
					{
						int num3 = i_pagenum * i_pagesize - i_total;
						commandText = string.Concat(new object[]
						{
							"select top ",
							i_pagesize - num3,
							" id from logrecords where ",
							text,
							" order by id desc "
						});
					}
					else
					{
						commandText = string.Concat(new object[]
						{
							"select id from ( select top ",
							i_pagesize,
							" id from logrecords where ",
							text,
							" order by id asc ) order by id desc"
						});
					}
					dbCommand.CommandText = commandText;
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.HasRows)
					{
						dataTable2 = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
					}
					dbDataReader.Close();
					DataRow[] array = dataTable2.Select("", "id DESC");
					if (array.Length > 0)
					{
						num2 = Convert.ToInt64(array[0]["id"]);
						num = Convert.ToInt64(array[array.Length - 1]["id"]);
					}
					dbCommand.CommandText = string.Concat(new object[]
					{
						"select * from logrecords where id >= ",
						num,
						" and id <= ",
						num2,
						" order by id desc"
					});
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					DbDataReader dbDataReader2 = dbCommand.ExecuteReader();
					while (dbDataReader2.Read())
					{
						string value = Convert.ToDateTime(dbDataReader2.GetValue(1)).ToString("yyyy-MM-dd HH:mm:ss");
						string @string = dbDataReader2.GetString(2);
						string string2 = dbDataReader2.GetString(3);
						int num4 = Convert.ToInt32(dbDataReader2.GetValue(0));
						DataRow dataRow = dataTable.NewRow();
						dataRow["insert_time"] = value;
						dataRow["eventid"] = @string;
						dataRow["parametervalue"] = string2;
						dataRow["id"] = num4;
						dataTable.Rows.Add(dataRow);
					}
					dbDataReader2.Close();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			Console.WriteLine("million sencods is : " + (DateTime.Now - now).TotalMilliseconds.ToString());
			return dataTable;
		}
		public static DataTable GetPrePage(int i_key, int i_range_type, DateTime dt_start, DateTime dt_end, int i_pagenum, int i_pagesize, int i_total)
		{
			DateTime now = DateTime.Now;
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("insert_time");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("eventid");
			dataColumn2.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn2);
			DataColumn dataColumn3 = new DataColumn("parametervalue");
			dataColumn3.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn3);
			DataColumn dataColumn4 = new DataColumn("id");
			dataColumn4.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn4);
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			long arg_BF_0 = dt_start.Ticks;
			long arg_C7_0 = dt_end.Ticks;
			string text = "";
			switch (i_range_type)
			{
			case 0:
				text = string.Concat(new object[]
				{
					" 2=2 id > ",
					i_key,
					" and ticks <= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# "
				});
				break;
			case 1:
				text = string.Concat(new object[]
				{
					" 2=2 id > ",
					i_key,
					" and ticks >= #",
					dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
					"# and ticks <= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# "
				});
				break;
			case 2:
				text = string.Concat(new object[]
				{
					" 2=2 id > ",
					i_key,
					" and (ticks <= #",
					dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
					"# or ticks >= #",
					dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
					"# )"
				});
				break;
			}
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					long num = 0L;
					long num2 = 0L;
					DataTable dataTable2 = new DataTable();
					string commandText;
					if (i_pagenum * i_pagesize > i_total)
					{
						int num3 = i_pagenum * i_pagesize - i_total;
						commandText = string.Concat(new object[]
						{
							"select top ",
							i_pagesize - num3,
							" id  from (select top ",
							i_pagesize - num3,
							" id from logrecords where ",
							text,
							" order by id asc) order by id desc"
						});
					}
					else
					{
						commandText = string.Concat(new object[]
						{
							"select id from ( select top ",
							i_pagesize,
							" id from logrecords where ",
							text,
							" order by id asc ) order by id desc"
						});
					}
					dbCommand.CommandText = commandText;
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.HasRows)
					{
						dataTable2 = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
					}
					dbDataReader.Close();
					DataRow[] array = dataTable2.Select("", "id DESC");
					if (array.Length > 0)
					{
						num2 = Convert.ToInt64(array[0]["id"]);
						num = Convert.ToInt64(array[array.Length - 1]["id"]);
					}
					dbCommand.CommandText = string.Concat(new object[]
					{
						"select * from logrecords where id >= ",
						num,
						" and id <= ",
						num2,
						" order by id desc"
					});
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					DbDataReader dbDataReader2 = dbCommand.ExecuteReader();
					while (dbDataReader2.Read())
					{
						string value = Convert.ToDateTime(dbDataReader2.GetValue(1)).ToString("yyyy-MM-dd HH:mm:ss");
						string @string = dbDataReader2.GetString(2);
						string string2 = dbDataReader2.GetString(3);
						int num4 = Convert.ToInt32(dbDataReader2.GetValue(0));
						DataRow dataRow = dataTable.NewRow();
						dataRow["insert_time"] = value;
						dataRow["eventid"] = @string;
						dataRow["parametervalue"] = string2;
						dataRow["id"] = num4;
						dataTable.Rows.Add(dataRow);
					}
					dbDataReader2.Close();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			Console.WriteLine("million sencods is : " + (DateTime.Now - now).TotalMilliseconds.ToString());
			return dataTable;
		}
		public static int GetRowCount(int i_range_type, DateTime dt_start, DateTime dt_end)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			long arg_0F_0 = dt_start.Ticks;
			long arg_17_0 = dt_end.Ticks;
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					switch (i_range_type)
					{
					case 0:
						dbCommand.CommandText = "select count(id) from logrecords";
						break;
					case 1:
						dbCommand.CommandText = string.Concat(new string[]
						{
							"select count(id) from logrecords where ticks >= #",
							dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
							"# and ticks <= #",
							dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
							"# "
						});
						break;
					case 2:
						dbCommand.CommandText = string.Concat(new string[]
						{
							"select count(id) from logrecords where ticks <= #",
							dt_start.ToString("yyyy-MM-dd HH:mm:ss"),
							"# or ticks >= #",
							dt_end.ToString("yyyy-MM-dd HH:mm:ss"),
							"# "
						});
						break;
					}
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					object obj = dbCommand.ExecuteScalar();
					dbCommand.Dispose();
					int result;
					if (obj == null || obj is DBNull || Convert.ToInt32(obj) < 1)
					{
						result = -1;
						return result;
					}
					result = Convert.ToInt32(obj);
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return -1;
		}
		public static int InsertNewLog(ref int mailflag, string str_eventid, params string[] str_parameters)
		{
			int result = -1;
			int num = 1;
			int num2 = 1;
			try
			{
				Hashtable eventLogCache = DBCache.GetEventLogCache();
				Hashtable eventMailCache = DBCache.GetEventMailCache();
				if (eventLogCache != null && eventLogCache.Count > 0)
				{
					object obj = eventLogCache[str_eventid];
					if (obj != null)
					{
						num = Convert.ToInt32(obj);
					}
					obj = eventMailCache[str_eventid];
					if (obj != null)
					{
						num2 = Convert.ToInt32(obj);
					}
				}
				else
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Insert New Log Error : Could not get event log flag from DBCache");
				}
				mailflag = num2;
				if (num < 1)
				{
					return 1;
				}
				if (LogInfo.lp_thread == null)
				{
					LogInfo.lp_thread = new LogProcess();
					LogInfo.lp_thread.Start();
				}
				List<string> list = new List<string>();
				list.Add(str_eventid);
				list.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				string str = "";
				for (int i = 0; i < str_parameters.Length; i++)
				{
					list.Add(str_parameters[i]);
					if (i == str_parameters.Length - 1)
					{
						str += str_parameters[i];
					}
					else
					{
						str = str + str_parameters[i] + "^|^";
					}
				}
				LogInfo.lp_thread.PutLog(list);
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Insert New Log Error : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static long DeleteLogByDay(int iDays)
		{
			long result = 0L;
			DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).AddDays((double)(-(double)(iDays - 1)));
			new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
			string commandText = "delete from logrecords where ticks < #" + DateTime.Now.AddHours((double)(-24 * iDays)).ToString("yyyy-MM-dd HH:mm:ss") + "#";
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText;
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					result = (long)dbCommand.ExecuteNonQuery();
				}
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return result;
		}
		public static long DeleteLogByRecords(int records)
		{
			long result = 0L;
			string commandText = "select min(log.id) from (select top " + records + " id from logrecords order by id desc ) as log";
			string arg = "delete from logrecords where id < ";
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						commandText = "select min(logg.id) from (select id from logrecords order by id desc LIMIT " + records + " OFFSET 0 ) as logg";
					}
					dbCommand.CommandText = commandText;
					object obj = dbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						int num = 0;
						try
						{
							num = Convert.ToInt32(obj);
						}
						catch
						{
						}
						dbCommand.CommandText = arg + num;
						result = (long)dbCommand.ExecuteNonQuery();
					}
					dbCommand.Dispose();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return result;
		}
	}
}
