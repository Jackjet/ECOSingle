using CommonAPI;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class LogSetting
	{
		public const string COLNAME_EVENTID = "eventid";
		public const string COLNAME_LOGFLAG = "logflag";
		public const string COLNAME_MAILFLAG = "mailflag";
		public const int MAINTENANCE_PERIOD = 0;
		public const int MAINTENANCE_RECORDS = 1;
		public const int SAVERANGE_ALL = 1;
		public const int SAVERANGE_DISPLAY = 0;
		private int maintenance = 1;
		private int days = 7;
		private int recordNum = 1000;
		private int pageSize = 17;
		private int saveRange;
		public int Maintenance
		{
			get
			{
				return this.maintenance;
			}
			set
			{
				this.maintenance = value;
			}
		}
		public int Days
		{
			get
			{
				return this.days;
			}
			set
			{
				this.days = value;
			}
		}
		public int RecordNum
		{
			get
			{
				return this.recordNum;
			}
			set
			{
				this.recordNum = value;
			}
		}
		public int PageSize
		{
			get
			{
				if (this.pageSize > 0)
				{
					return this.pageSize;
				}
				return 17;
			}
			set
			{
				this.pageSize = value;
			}
		}
		public int SaveRange
		{
			get
			{
				return this.saveRange;
			}
			set
			{
				this.saveRange = value;
			}
		}
		public LogSetting()
		{
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0)
				{
					object obj = sysParameterCache["LIMITTYPE"];
					if (obj != null)
					{
						this.maintenance = (int)Convert.ToInt16(obj);
					}
					obj = sysParameterCache["DAYLIMIT"];
					if (obj != null)
					{
						this.days = Convert.ToInt32(obj);
					}
					obj = sysParameterCache["RECORDSLIMIT"];
					if (obj != null)
					{
						this.recordNum = Convert.ToInt32(obj);
					}
					obj = sysParameterCache["PAGESIZE"];
					if (obj != null)
					{
						this.pageSize = Convert.ToInt32(obj);
					}
					obj = sysParameterCache["SAVERANGE"];
					if (obj != null)
					{
						this.saveRange = Convert.ToInt32(obj);
					}
				}
				else
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get LogSetting Error : Could not get system parameter");
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get LogSetting Error : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public int update()
		{
			int num = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				string commandText = "update sys_para set para_value = '" + this.maintenance + "' where para_name = 'LIMITTYPE' and para_type = 'int' ";
				string commandText2 = "update sys_para set para_value = '" + this.days + "' where para_name = 'DAYLIMIT' and para_type = 'int' ";
				string commandText3 = "update sys_para set para_value = '" + this.recordNum + "' where para_name = 'RECORDSLIMIT' and para_type = 'int' ";
				string commandText4 = "update sys_para set para_value = '" + this.pageSize + "' where para_name = 'PAGESIZE' and para_type = 'int' ";
				string commandText5 = "update sys_para set para_value = '" + this.saveRange + "' where para_name = 'SAVERANGE' and para_type = 'int' ";
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandType = CommandType.Text;
				dbCommand.CommandText = commandText;
				num = dbCommand.ExecuteNonQuery();
				if (num < 1)
				{
					int result = num;
					return result;
				}
				dbCommand.CommandText = commandText2;
				num = dbCommand.ExecuteNonQuery();
				if (num < 1)
				{
					int result = num;
					return result;
				}
				dbCommand.CommandText = commandText3;
				num = dbCommand.ExecuteNonQuery();
				if (num < 1)
				{
					int result = num;
					return result;
				}
				dbCommand.CommandText = commandText4;
				num = dbCommand.ExecuteNonQuery();
				if (num < 1)
				{
					int result = num;
					return result;
				}
				dbCommand.CommandText = commandText5;
				num = dbCommand.ExecuteNonQuery();
				dbCommand.Dispose();
				DBCacheStatus.SystemParameter = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_Service_SystemParameter"
				});
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
			return num;
		}
		public static DataTable GetEventList()
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = Type.GetType("System.String");
			dataColumn.ColumnName = "eventid";
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn();
			dataColumn.DataType = Type.GetType("System.Int32");
			dataColumn.ColumnName = "logflag";
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn();
			dataColumn.DataType = Type.GetType("System.Int32");
			dataColumn.ColumnName = "mailflag";
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn();
			dataColumn.DataType = Type.GetType("System.Int64");
			dataColumn.ColumnName = "reserve";
			dataTable.Columns.Add(dataColumn);
			try
			{
				Hashtable eventLogCache = DBCache.GetEventLogCache();
				Hashtable eventMailCache = DBCache.GetEventMailCache();
				if (eventLogCache != null && eventLogCache.Count > 0)
				{
					ICollection keys = eventLogCache.Keys;
					IEnumerator enumerator = keys.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string text = (string)enumerator.Current;
							DataRow dataRow = dataTable.NewRow();
							dataRow[0] = text;
							dataRow[1] = Convert.ToInt32(eventLogCache[text]);
							dataRow[2] = Convert.ToInt32(eventMailCache[text]);
							dataTable.Rows.Add(dataRow);
						}
						goto IL_17B;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get LogEventList Error : Could not get data from DBCache");
				IL_17B:;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get LogEventList Error : " + ex.Message + "\n" + ex.StackTrace);
			}
			dataTable = new DataView(dataTable)
			{
				Sort = "eventid ASC"
			}.ToTable();
			return dataTable;
		}
		public static bool SetEventInfo(DataTable eventlist)
		{
			bool result = false;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					foreach (DataRow dataRow in eventlist.Rows)
					{
						dbCommand.CommandType = CommandType.Text;
						dbCommand.CommandText = string.Concat(new object[]
						{
							"update event_info set logflag = ",
							Convert.ToInt16(dataRow["logflag"]),
							", mailflag = ",
							Convert.ToInt16(dataRow["mailflag"]),
							" where eventid = '",
							Convert.ToString(dataRow["eventid"]),
							"' "
						});
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.Dispose();
					DBCacheStatus.Event = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Event"
					});
					return true;
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
