using CommonAPI;
using CommonAPI.Global;
using CommonAPI.InterProcess;
using CommonAPI.Timers;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading;
namespace DBAccessAPI
{
	public class MultiThreadQuery
	{
		private class PeakInfo
		{
			public string _PeakPeriod;
			public double _dblPeakPower;
			public DateTime _PeriodFrom;
			public DateTime _PeriodTo;
			public PeakInfo()
			{
				this._PeakPeriod = "";
				this._dblPeakPower = -1.7976931348623157E+308;
				this._PeriodFrom = DateTime.MinValue;
				this._PeriodTo = DateTime.MinValue;
			}
			public PeakInfo(string keyPeriod, double valuePower, DateTime tEarliest, DateTime tLatest)
			{
				this._PeakPeriod = keyPeriod;
				this._dblPeakPower = valuePower;
				this._PeriodFrom = tEarliest;
				this._PeriodTo = tLatest;
			}
		}
		private class QueryContext
		{
			public string _DBType;
			public string _sqlPrerequisite;
			public string _sSelectDefine;
			public string _sConditionFrom;
			public string _sConditionTo;
			public string _sMoreConditions;
			public string _tablePrefix;
			public string _tableSuffix;
			public string _sGroupBy;
			public string _sOrderBy;
			public string _filterList;
			public bool _isPort;
			public string _CalcColName;
			public string _CalcType;
			public Semaphore _finishSignal;
			public SemaphoreSlim _finishCounter;
			public DataTable _resultInfo;
			public ConcurrentQueue<DataTable> _ResultQueue;
			public ConcurrentQueue<long> _ResultElapsed;
			public QueryContext()
			{
				this._DBType = "ACCESS";
				this._sqlPrerequisite = "";
				this._isPort = false;
				this._sSelectDefine = "";
				this._sConditionFrom = "";
				this._sConditionTo = "";
				this._sMoreConditions = "";
				this._tablePrefix = "";
				this._tableSuffix = "";
				this._sGroupBy = "";
				this._sOrderBy = "";
				this._CalcColName = "";
				this._CalcType = "";
				this._filterList = "";
				this._resultInfo = null;
				this._ResultQueue = null;
				this._ResultElapsed = null;
				this._finishSignal = null;
				this._finishCounter = null;
			}
			public string GetSqlFormat()
			{
				string text = this._sSelectDefine;
				text += " from ";
				text += this._tablePrefix;
				if (this._DBType.Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase))
				{
					text += this._tableSuffix;
				}
				string text2 = "";
				if (!string.IsNullOrEmpty(this._sConditionFrom))
				{
					text2 += this._sConditionFrom;
				}
				if (!string.IsNullOrEmpty(this._sConditionTo))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " and ";
					}
					text2 += this._sConditionTo;
				}
				if (!string.IsNullOrEmpty(this._filterList))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " and ";
					}
					if (this._isPort)
					{
						text2 += "port_id in ({0})";
					}
					else
					{
						text2 += "device_id in ({0})";
					}
				}
				if (!string.IsNullOrEmpty(this._sMoreConditions))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " and ";
					}
					text2 += this._sMoreConditions;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + " where " + text2;
				}
				if (!string.IsNullOrEmpty(this._sGroupBy))
				{
					text += " group by ";
					text += this._sGroupBy;
				}
				if (!string.IsNullOrEmpty(this._sOrderBy))
				{
					text += " order by ";
					text += this._sOrderBy;
				}
				return text;
			}
			public string getPrerequisiteSQL(bool bUsePeriod = true)
			{
				if (string.IsNullOrEmpty(this._sqlPrerequisite))
				{
					return "";
				}
				string text = this._sqlPrerequisite;
				text += " from ";
				text += this._tablePrefix;
				if (this._DBType.Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase))
				{
					text += this._tableSuffix;
				}
				string text2 = "";
				if (bUsePeriod)
				{
					if (!string.IsNullOrEmpty(this._sConditionFrom))
					{
						text2 += this._sConditionFrom;
					}
					if (!string.IsNullOrEmpty(this._sConditionTo))
					{
						if (!string.IsNullOrEmpty(text2))
						{
							text2 += " and ";
						}
						text2 += this._sConditionTo;
					}
				}
				if (!string.IsNullOrEmpty(this._filterList))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " and ";
					}
					if (this._isPort)
					{
						text2 += "port_id in (";
					}
					else
					{
						text2 += "device_id in (";
					}
					text2 += this._filterList;
					text2 += ")";
				}
				if (!string.IsNullOrEmpty(this._sMoreConditions))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " and ";
					}
					text2 += this._sMoreConditions;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + " where " + text2;
				}
				return text;
			}
			public string getSQL(bool bUsePeriod = true)
			{
				string text = this._sSelectDefine;
				text += " from ";
				text += this._tablePrefix;
				if (this._DBType.Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase))
				{
					text += this._tableSuffix;
				}
				string text2 = "";
				if (bUsePeriod)
				{
					if (!string.IsNullOrEmpty(this._sConditionFrom))
					{
						text2 += this._sConditionFrom;
					}
					if (!string.IsNullOrEmpty(this._sConditionTo))
					{
						if (!string.IsNullOrEmpty(text2))
						{
							text2 += " and ";
						}
						text2 += this._sConditionTo;
					}
				}
				if (!string.IsNullOrEmpty(this._filterList))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " and ";
					}
					if (this._isPort)
					{
						text2 += "port_id in (";
					}
					else
					{
						text2 += "device_id in (";
					}
					text2 += this._filterList;
					text2 += ")";
				}
				if (!string.IsNullOrEmpty(this._sMoreConditions))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " and ";
					}
					text2 += this._sMoreConditions;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + " where " + text2;
				}
				if (!string.IsNullOrEmpty(this._sGroupBy))
				{
					text += " group by ";
					text += this._sGroupBy;
				}
				if (!string.IsNullOrEmpty(this._sOrderBy))
				{
					text += " order by ";
					text += this._sOrderBy;
				}
				return text;
			}
		}
		public const int _HOUR = 1;
		public const int _DAY = 2;
		public const int _WEEK = 3;
		public const int _MONTH = 4;
		public const int _QUARTER = 5;
		public const int _YEAR = 6;
		public static int _nThreadCount = 2;
		public static int _nMergeAfterThreadDone = 1;
		public static int _nUseOrderBy = 0;
		private string _sqlPrerequisite;
		private string _StartTime;
		private string _EndTime;
		private bool _IsPort;
		private string _FilterList;
		private string _CalcColName;
		private string _CalcType;
		private Dictionary<string, double> _MergedResult;
		private string _MergedType;
		private string _MergedKeyDateFormat;
		private string _MergedKeyColName;
		private string _MergedValueColName;
		public ConcurrentQueue<DataTable> _ResultQueue;
		public ConcurrentQueue<long> _ResultElapsed;
		private string _Select;
		private string _TablePrefix;
		private string _MoreConditions;
		private string _GroupBy;
		private string _OrderBy;
		private bool _EnableProfile;
		public ArrayList _resultTables;
		public static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static object _lockLog = new object();
		private static string _xmlFile = "Config.xml";
		private static string _logFile = "";
		private static Dictionary<string, string> _cfgFile = null;
		public string PrerequisiteSQL
		{
			get
			{
				return this._sqlPrerequisite;
			}
			set
			{
				this._sqlPrerequisite = value;
			}
		}
		public string CalcColName
		{
			get
			{
				return this._CalcColName;
			}
			set
			{
				this._CalcColName = value;
			}
		}
		public string CalcType
		{
			get
			{
				return this._CalcType;
			}
			set
			{
				this._CalcType = value;
			}
		}
		public bool IsPort
		{
			get
			{
				return this._IsPort;
			}
			set
			{
				this._IsPort = value;
			}
		}
		public string FilterList
		{
			get
			{
				return this._FilterList;
			}
			set
			{
				this._FilterList = value;
			}
		}
		public string MoreConditions
		{
			get
			{
				return this._MoreConditions;
			}
			set
			{
				this._MoreConditions = value;
			}
		}
		public string OrderBy
		{
			get
			{
				return this._OrderBy;
			}
			set
			{
				this._OrderBy = value;
			}
		}
		public string GroupBy
		{
			get
			{
				return this._GroupBy;
			}
			set
			{
				this._GroupBy = value;
			}
		}
		public string Select
		{
			get
			{
				return this._Select;
			}
			set
			{
				this._Select = value;
			}
		}
		public string TablePrefix
		{
			get
			{
				return this._TablePrefix;
			}
			set
			{
				this._TablePrefix = value;
			}
		}
		public string StartTime
		{
			get
			{
				return this._StartTime;
			}
			set
			{
				this._StartTime = value;
			}
		}
		public string EndTime
		{
			get
			{
				return this._EndTime;
			}
			set
			{
				this._EndTime = value;
			}
		}
		public bool EnableProfile
		{
			get
			{
				return this._EnableProfile;
			}
			set
			{
				this._EnableProfile = value;
			}
		}
		public MultiThreadQuery()
		{
			this._EnableProfile = false;
			this._StartTime = "";
			this._EndTime = "";
			this._sqlPrerequisite = "";
			this._IsPort = false;
			this._FilterList = "";
			this._TablePrefix = "";
			this._Select = "";
			this._MoreConditions = "";
			this._MergedResult = null;
			this._MergedType = "";
			this._MergedKeyColName = "";
			this._MergedKeyDateFormat = "";
			this._MergedValueColName = "";
			this._ResultQueue = null;
			this._ResultElapsed = new ConcurrentQueue<long>();
			this._GroupBy = "";
			this._OrderBy = "";
			this._resultTables = new ArrayList();
			MultiThreadQuery._nThreadCount = MultiThreadQuery.getQueryThreads();
			MultiThreadQuery._nMergeAfterThreadDone = 1;
			string testOption = MultiThreadQuery.GetTestOption("merge_after_thread_done");
			if (!string.IsNullOrEmpty(testOption))
			{
				MultiThreadQuery._nMergeAfterThreadDone = Convert.ToInt32(testOption);
			}
			MultiThreadQuery._nUseOrderBy = 0;
			string testOption2 = MultiThreadQuery.GetTestOption("use_order_by");
			if (!string.IsNullOrEmpty(testOption2))
			{
				MultiThreadQuery._nUseOrderBy = Convert.ToInt32(testOption2);
			}
		}
		public static void DBErrorCodeTest()
		{
			if (!DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return;
			}
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new object[]
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
				}));
				dbConnection.Open();
				dbCommand = dbConnection.CreateCommand();
				dbCommand.CommandText = "select * from ABBA";
				dbCommand.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				if (ex.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
				{
					MySqlException ex2 = (MySqlException)ex;
					MultiThreadQuery.WriteLog("Error Code [{0}], {1}", new string[]
					{
						ex2.Number.ToString(),
						ex2.Message
					});
				}
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
		}
		public ArrayList Execute(int nThreads)
		{
			DateTime dateTime = DateTime.MinValue;
			DateTime t = DateTime.MinValue;
			try
			{
				dateTime = Convert.ToDateTime(this._StartTime);
				t = Convert.ToDateTime(this._EndTime);
				if (dateTime > t)
				{
					ArrayList result = null;
					return result;
				}
			}
			catch (Exception)
			{
				ArrayList result = null;
				return result;
			}
			string sConditionFrom = "insert_time >= #" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "#";
			string sConditionTo = "insert_time < #" + t.ToString("yyyy-MM-dd HH:mm:ss") + "#";
			List<MultiThreadQuery.QueryContext> list = new List<MultiThreadQuery.QueryContext>();
			DateTime t2 = dateTime;
			DateTime t3 = new DateTime(t.Year, t.Month, t.Day, 23, 59, 59);
			List<string> allTableSuffixes = this.GetAllTableSuffixes(this._TablePrefix);
			while (t2 <= t3)
			{
				MultiThreadQuery.QueryContext queryContext = new MultiThreadQuery.QueryContext();
				queryContext._DBType = DBUrl.DB_CURRENT_TYPE.ToUpper();
				queryContext._sqlPrerequisite = this._sqlPrerequisite;
				queryContext._CalcColName = this._CalcColName;
				queryContext._CalcType = this._CalcType;
				queryContext._isPort = this._IsPort;
				queryContext._tablePrefix = this._TablePrefix;
				queryContext._filterList = this._FilterList;
				queryContext._sMoreConditions = this._MoreConditions;
				queryContext._tableSuffix = string.Format("{0:0000}{1:00}{2:00}", t2.Year, t2.Month, t2.Day);
				if (!allTableSuffixes.Contains(queryContext._tableSuffix))
				{
					t2 = t2.AddDays(1.0);
				}
				else
				{
					if (t2.Year == t.Year && t2.Month == t.Month && t2.Day == t.Day && t.Hour == 0 && t.Minute == 0 && t.Second == 0)
					{
						t2 = t2.AddDays(1.0);
					}
					else
					{
						queryContext._sSelectDefine = this._Select;
						queryContext._sConditionFrom = sConditionFrom;
						queryContext._sConditionTo = sConditionTo;
						if (t2.ToString("yyyy-MM-dd 00:00:00") == dateTime.ToString("yyyy-MM-dd HH:mm:ss"))
						{
							queryContext._sConditionFrom = "";
						}
						if (!t2.ToString("yyyy-MM-dd").Equals(dateTime.ToString("yyyy-MM-dd")))
						{
							queryContext._sConditionFrom = "";
						}
						if (!t2.ToString("yyyy-MM-dd").Equals(t.ToString("yyyy-MM-dd")))
						{
							queryContext._sConditionTo = "";
						}
						queryContext._sGroupBy = this._GroupBy;
						queryContext._sOrderBy = this._OrderBy;
						if (MultiThreadQuery._nUseOrderBy == 0)
						{
							queryContext._sOrderBy = "null";
						}
						queryContext._ResultQueue = this._ResultQueue;
						queryContext._ResultElapsed = this._ResultElapsed;
						list.Add(queryContext);
						t2 = t2.AddDays(1.0);
					}
				}
			}
			if (list.Count > 1)
			{
				list[0]._sConditionTo = "";
				if (list.Count > 2)
				{
					for (int i = 1; i <= list.Count - 2; i++)
					{
						list[i]._sConditionFrom = "";
						list[i]._sConditionTo = "";
					}
				}
				list[list.Count - 1]._sConditionFrom = "";
			}
			if (!this.ParallelQuery(Math.Max(1, nThreads), list))
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			if (this._ResultQueue != null)
			{
				return arrayList;
			}
			foreach (MultiThreadQuery.QueryContext current in list)
			{
				arrayList.Add(current._resultInfo);
			}
			return arrayList;
		}
		private List<MultiThreadQuery.QueryContext> PrepareQueryContext()
		{
			DateTime dateTime = DateTime.MinValue;
			DateTime t = DateTime.MinValue;
			try
			{
				dateTime = Convert.ToDateTime(this._StartTime);
				t = Convert.ToDateTime(this._EndTime);
				if (dateTime > t)
				{
					List<MultiThreadQuery.QueryContext> result = null;
					return result;
				}
			}
			catch (Exception)
			{
				List<MultiThreadQuery.QueryContext> result = null;
				return result;
			}
			string sConditionFrom = "insert_time >= #" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "#";
			string sConditionTo = "insert_time < #" + t.ToString("yyyy-MM-dd HH:mm:ss") + "#";
			List<MultiThreadQuery.QueryContext> list = new List<MultiThreadQuery.QueryContext>();
			DateTime t2 = dateTime;
			DateTime t3 = new DateTime(t.Year, t.Month, t.Day, 23, 59, 59);
			List<string> allTableSuffixes = this.GetAllTableSuffixes(this._TablePrefix);
			while (t2 <= t3)
			{
				MultiThreadQuery.QueryContext queryContext = new MultiThreadQuery.QueryContext();
				queryContext._DBType = DBUrl.DB_CURRENT_TYPE.ToUpper();
				queryContext._sqlPrerequisite = this._sqlPrerequisite;
				queryContext._CalcColName = this._CalcColName;
				queryContext._CalcType = this._CalcType;
				queryContext._isPort = this._IsPort;
				queryContext._tablePrefix = this._TablePrefix;
				queryContext._filterList = this._FilterList;
				queryContext._sMoreConditions = this._MoreConditions;
				queryContext._tableSuffix = string.Format("{0:0000}{1:00}{2:00}", t2.Year, t2.Month, t2.Day);
				if (!allTableSuffixes.Contains(queryContext._tableSuffix))
				{
					t2 = t2.AddDays(1.0);
				}
				else
				{
					if (t2.Year == t.Year && t2.Month == t.Month && t2.Day == t.Day && t.Hour == 0 && t.Minute == 0 && t.Second == 0)
					{
						t2 = t2.AddDays(1.0);
					}
					else
					{
						queryContext._sSelectDefine = this._Select;
						queryContext._sConditionFrom = sConditionFrom;
						queryContext._sConditionTo = sConditionTo;
						if (t2.ToString("yyyy-MM-dd 00:00:00") == dateTime.ToString("yyyy-MM-dd HH:mm:ss"))
						{
							queryContext._sConditionFrom = "";
						}
						if (!t2.ToString("yyyy-MM-dd").Equals(dateTime.ToString("yyyy-MM-dd")))
						{
							queryContext._sConditionFrom = "";
						}
						if (!t2.ToString("yyyy-MM-dd").Equals(t.ToString("yyyy-MM-dd")))
						{
							queryContext._sConditionTo = "";
						}
						queryContext._sGroupBy = this._GroupBy;
						queryContext._sOrderBy = this._OrderBy;
						if (MultiThreadQuery._nUseOrderBy == 0)
						{
							queryContext._sOrderBy = "null";
						}
						list.Add(queryContext);
						t2 = t2.AddDays(1.0);
					}
				}
			}
			if (list.Count > 1)
			{
				list[0]._sConditionTo = "";
				if (list.Count > 2)
				{
					for (int i = 1; i <= list.Count - 2; i++)
					{
						list[i]._sConditionFrom = "";
						list[i]._sConditionTo = "";
					}
				}
				list[list.Count - 1]._sConditionFrom = "";
			}
			return list;
		}
		public static int getQueryThreads()
		{
			if (!MultiThreadQuery.sCodebase.EndsWith("/") && !MultiThreadQuery.sCodebase.EndsWith("\\"))
			{
				MultiThreadQuery.sCodebase += "\\";
			}
			try
			{
				string path = MultiThreadQuery.sCodebase + "testCfg.xml";
				if (File.Exists(path))
				{
					using (StreamReader streamReader = new StreamReader(path))
					{
						string text = streamReader.ReadToEnd();
						text = text.Trim().ToLower();
						string[] array = text.Split(new string[]
						{
							"="
						}, StringSplitOptions.RemoveEmptyEntries);
						if (array.Length == 2 && array[0].Equals("thread_count"))
						{
							int result = Math.Max(1, Convert.ToInt32(array[1]));
							return result;
						}
						goto IL_DC;
					}
				}
				string testOption = MultiThreadQuery.GetTestOption("thread_count");
				if (!string.IsNullOrEmpty(testOption))
				{
					int result = Math.Max(1, Convert.ToInt32(testOption));
					return result;
				}
				IL_DC:;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return 2;
		}
		public static string GetTestOption(string optionName)
		{
			if (!MultiThreadQuery.sCodebase.EndsWith("/") && !MultiThreadQuery.sCodebase.EndsWith("\\"))
			{
				MultiThreadQuery.sCodebase += "\\";
			}
			try
			{
				string path = MultiThreadQuery.sCodebase + "testOptions.xml";
				if (File.Exists(path))
				{
					using (StreamReader streamReader = new StreamReader(path))
					{
						string text = streamReader.ReadToEnd();
						text = text.Trim().ToLower();
						text = text.Replace("\r", "\n");
						text = text.Replace("\n\n", "\n");
						string[] array = text.Split(new string[]
						{
							"\n"
						}, StringSplitOptions.RemoveEmptyEntries);
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text2 = array2[i];
							string[] array3 = text2.Split(new string[]
							{
								"="
							}, StringSplitOptions.RemoveEmptyEntries);
							if (array3.Length == 2 && array3[0].Equals(optionName, StringComparison.InvariantCultureIgnoreCase))
							{
								return array3[1];
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return "";
		}
		public static void WriteLog(string format, params string[] list)
		{
			lock (MultiThreadQuery._lockLog)
			{
				if (!MultiThreadQuery.sCodebase.EndsWith("/") && !MultiThreadQuery.sCodebase.EndsWith("\\"))
				{
					MultiThreadQuery.sCodebase += "\\";
				}
				if (MultiThreadQuery._cfgFile == null)
				{
					MultiThreadQuery._logFile = "";
					MultiThreadQuery._cfgFile = ValuePairs.LoadValueKeyFromXML(MultiThreadQuery._xmlFile);
					if (MultiThreadQuery._cfgFile.ContainsKey("LogFile"))
					{
						MultiThreadQuery._logFile = MultiThreadQuery._cfgFile["LogFile"];
					}
				}
				if (MultiThreadQuery._logFile != null && !(MultiThreadQuery._logFile == ""))
				{
					try
					{
						string str = format;
						if (list != null && list.Length > 0)
						{
							str = string.Format(format, list);
						}
						DateTime now = DateTime.Now;
						string str2 = now.ToString("MM-dd HH:mm:ss.fff");
						string text = MultiThreadQuery.sCodebase + "\\debuglog\\" + MultiThreadQuery._logFile;
						int num = text.LastIndexOf(".");
						if (num >= 0)
						{
							text = text.Substring(0, num);
						}
						if (DBUtil.ServiceHandle != null)
						{
							text = text + "(" + now.ToString("yyyy-MM-dd") + ")s.log";
						}
						else
						{
							text = text + "(" + now.ToString("yyyy-MM-dd") + ")c.log";
						}
						using (StreamWriter streamWriter = File.AppendText(text))
						{
							if (DBUtil.ServiceHandle != null)
							{
								streamWriter.WriteLine(str2 + " [S] " + str, streamWriter);
							}
							else
							{
								streamWriter.WriteLine(str2 + " [-] " + str, streamWriter);
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}
		public bool TableExists(DbConnection conn, string table)
		{
			return conn.GetSchema("Tables", new string[]
			{
				null,
				null,
				table,
				"TABLE"
			}).Rows.Count > 0;
		}
		private ArrayList CreateAccessTablesByLongSQL(DBConn conn, string str_sqlformat, string opIDs)
		{
			ArrayList arrayList = new ArrayList();
			string text = opIDs;
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			tickTimer.Update();
			int num = 0;
			while (text.Length > 0 || num == 0)
			{
				string text2;
				if (text.Length > 30000)
				{
					text2 = text.Substring(0, 30000);
					int num2 = text2.LastIndexOf(',');
					text2 = text.Substring(0, num2);
					text = text.Substring(num2 + 1);
				}
				else
				{
					text2 = text;
					text = "";
				}
				string commandText = string.Format(str_sqlformat, text2);
				DataTable dataTable = new DataTable();
				DbDataAdapter dbDataAdapter = null;
				DbCommand dbCommand = null;
				try
				{
					if (conn.con != null)
					{
						dbDataAdapter = DBConn.GetDataAdapter(conn.con);
						dbCommand = DBConn.GetCommandObject(conn.con);
						dbCommand.CommandType = CommandType.Text;
						dbCommand.CommandText = commandText;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					try
					{
						if (dbDataAdapter != null)
						{
							dbDataAdapter.Dispose();
						}
					}
					catch
					{
					}
					try
					{
						if (dbCommand != null)
						{
							dbCommand.Dispose();
						}
					}
					catch
					{
					}
				}
				arrayList.Add(dataTable);
				num++;
			}
			MultiThreadQuery.WriteLog(string.Format("LongSQL Query, table_count={0}, elapsed={1}", arrayList.Count, tickTimer.getElapsed()), new string[0]);
			return arrayList;
		}
		public static void WriteProfile(string fileID, string lineInfo)
		{
			lock (MultiThreadQuery._lockLog)
			{
				if (!MultiThreadQuery.sCodebase.EndsWith("/") && !MultiThreadQuery.sCodebase.EndsWith("\\"))
				{
					MultiThreadQuery.sCodebase += "\\";
				}
				try
				{
					string path = MultiThreadQuery.sCodebase + "\\debuglog\\profile_" + fileID + ".log";
					using (StreamWriter streamWriter = File.AppendText(path))
					{
						streamWriter.WriteLine(lineInfo, streamWriter);
					}
				}
				catch (Exception)
				{
				}
			}
		}
		private DataTable CreateDataTableBySQL(object obj)
		{
			DataTable dataTable = new DataTable();
			MultiThreadQuery.QueryContext queryContext = (MultiThreadQuery.QueryContext)obj;
			DBConn dBConn = null;
			if (queryContext._DBType.Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase))
			{
				MySqlConnection mySqlConnection = null;
				try
				{
					mySqlConnection = new MySqlConnection(string.Concat(new object[]
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
					}));
					mySqlConnection.Open();
					DbCommand dbCommand = mySqlConnection.CreateCommand();
					string text = queryContext.getPrerequisiteSQL(true);
					if (!string.IsNullOrEmpty(text))
					{
						string testOption = MultiThreadQuery.GetTestOption("Pre-Select");
						if (testOption.Equals("yes") || testOption.Equals("1"))
						{
							dbCommand.CommandText = text.Replace("#", "'");
							dbCommand.ExecuteScalar();
						}
					}
					if (this.EnableProfile)
					{
						dbCommand.CommandText = "set profiling=1";
						dbCommand.ExecuteScalar();
					}
					text = queryContext.getSQL(true);
					dbCommand.CommandText = text.Replace("#", "'");
					DbDataAdapter dataAdapter = DBConn.GetDataAdapter(mySqlConnection);
					dataAdapter.SelectCommand = dbCommand;
					try
					{
						dataAdapter.Fill(dataTable);
					}
					catch
					{
						dataAdapter.Dispose();
					}
					dataAdapter.Dispose();
					if (this.EnableProfile)
					{
						int num = -1;
						dbCommand.CommandText = "show profiles";
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						if (dbDataReader.HasRows)
						{
							while (dbDataReader.Read())
							{
								string value = Convert.ToString(dbDataReader.GetValue(0));
								if (!string.IsNullOrEmpty(value))
								{
									num = Convert.ToInt32(value);
								}
							}
						}
						dbDataReader.Close();
						dbDataReader.Dispose();
						if (num > 0)
						{
							string fileID = queryContext._tablePrefix + queryContext._tableSuffix;
							dbCommand.CommandText = "show profile all for query " + num;
							dbDataReader = dbCommand.ExecuteReader();
							if (dbDataReader.HasRows)
							{
								string text2 = "";
								List<string> list = new List<string>();
								for (int i = 0; i < dbDataReader.FieldCount; i++)
								{
									if (i > 0)
									{
										text2 += ",";
									}
									text2 += dbDataReader.GetName(i);
									list.Add(dbDataReader.GetName(i));
								}
								text2 = text2 + "," + queryContext._tablePrefix;
								text2 = text2 + "," + queryContext._tableSuffix;
								text2 = text2 + "," + queryContext._sSelectDefine.Replace(',', ' ');
								MultiThreadQuery.WriteProfile(fileID, text2);
								while (dbDataReader.Read())
								{
									string text3 = "";
									for (int j = 0; j < dbDataReader.FieldCount; j++)
									{
										string text4 = Convert.ToString(dbDataReader.GetValue(j));
										if (j > 0)
										{
											text3 += ",";
										}
										if (!string.IsNullOrEmpty(text4))
										{
											text3 += text4;
										}
									}
									if (!string.IsNullOrEmpty(text3))
									{
										MultiThreadQuery.WriteProfile(fileID, text3 + ",,,");
									}
								}
							}
							dbDataReader.Close();
							dbDataReader.Dispose();
						}
					}
					dbCommand.Dispose();
					mySqlConnection.Close();
					return dataTable;
				}
				catch (Exception)
				{
					if (mySqlConnection != null)
					{
						mySqlConnection.Close();
					}
					return dataTable;
				}
			}
			try
			{
				if (queryContext._DBType.Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase))
				{
					dBConn = DBConnPool.getDynaConnection();
				}
				else
				{
					string text5 = queryContext._tableSuffix;
					text5 = text5.Insert(6, "-");
					text5 = text5.Insert(4, "-");
					text5 += " 00:00:00";
					DateTime dt_inserttime = Convert.ToDateTime(text5);
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
				}
				if (dBConn != null && dBConn.con != null)
				{
					string sqlFormat = queryContext.GetSqlFormat();
					ArrayList arrayList = this.CreateAccessTablesByLongSQL(dBConn, sqlFormat, queryContext._filterList);
					foreach (DataTable dataTable2 in arrayList)
					{
						if (dataTable.Columns.Count == 0)
						{
							dataTable = dataTable2.Copy();
						}
						else
						{
							dataTable.Merge(dataTable2);
						}
					}
					dBConn.Close();
					DataTable dataTable3 = ((DataTable)arrayList[0]).Clone();
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					for (int k = 0; k < arrayList.Count; k++)
					{
						int num2 = -1;
						if (((DataTable)arrayList[k]).Columns[queryContext._CalcColName].DataType == Type.GetType("System.Double"))
						{
							num2 = 0;
						}
						else
						{
							if (((DataTable)arrayList[k]).Columns[queryContext._CalcColName].DataType == Type.GetType("System.Single"))
							{
								num2 = 1;
							}
							else
							{
								if (((DataTable)arrayList[k]).Columns[queryContext._CalcColName].DataType == Type.GetType("System.Int64"))
								{
									num2 = 2;
								}
								else
								{
									if (((DataTable)arrayList[k]).Columns[queryContext._CalcColName].DataType == Type.GetType("System.Int32"))
									{
										num2 = 3;
									}
								}
							}
						}
						foreach (DataRow dataRow in ((DataTable)arrayList[k]).Rows)
						{
							if (num2 < 0)
							{
								break;
							}
							string key = Convert.ToString(dataRow["period"]);
							if (num2 == 0 || num2 == 1)
							{
								double num3 = Convert.ToDouble(dataRow[queryContext._CalcColName]);
								if (dictionary.ContainsKey(key))
								{
									if (queryContext._CalcType == "MAX")
									{
										if (num3 > (double)dictionary[key])
										{
											dictionary[key] = num3;
										}
									}
									else
									{
										if (queryContext._CalcType == "MIN")
										{
											if (num3 < (double)dictionary[key])
											{
												dictionary[key] = num3;
											}
										}
										else
										{
											if (queryContext._CalcType == "SUM")
											{
												num3 += (double)dictionary[key];
											}
										}
									}
									dictionary[key] = num3;
								}
								else
								{
									dictionary.Add(key, num3);
								}
							}
							else
							{
								long num4 = Convert.ToInt64(dataRow[queryContext._CalcColName]);
								if (dictionary.ContainsKey(key))
								{
									if (queryContext._CalcType == "MAX")
									{
										if (num4 > (long)dictionary[key])
										{
											dictionary[key] = num4;
										}
									}
									else
									{
										if (queryContext._CalcType == "MIN")
										{
											if (num4 < (long)dictionary[key])
											{
												dictionary[key] = num4;
											}
										}
										else
										{
											if (queryContext._CalcType == "SUM")
											{
												num4 += (long)dictionary[key];
											}
										}
									}
									dictionary[key] = num4;
								}
								else
								{
									dictionary.Add(key, num4);
								}
							}
						}
					}
					dataTable3.Clear();
					foreach (string current in dictionary.Keys)
					{
						dataTable3.Rows.Add(new object[]
						{
							dictionary[current],
							current
						});
					}
					dataTable = new DataView(dataTable3)
					{
						Sort = "period ASC"
					}.ToTable();
				}
			}
			catch (Exception)
			{
				if (dBConn != null)
				{
					dBConn.Close();
				}
			}
			return dataTable;
		}
		private void OneTableQuery(object obj)
		{
			MultiThreadQuery.QueryContext queryContext = (MultiThreadQuery.QueryContext)obj;
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			MultiThreadQuery.WriteLog(string.Format("EEEEEEEEEEEEEEEEE [{0}] started: ", Thread.CurrentThread.Name) + queryContext.GetSqlFormat().Replace("{0}", "[?]"), new string[0]);
			DataTable dataTable = null;
			try
			{
				dataTable = this.CreateDataTableBySQL(obj);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					if (queryContext._ResultQueue != null)
					{
						queryContext._ResultQueue.Enqueue(dataTable);
					}
					else
					{
						queryContext._resultInfo = dataTable;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (dataTable != null)
				{
					MultiThreadQuery.WriteLog(string.Format("----------------- [{0}] finished, count={1}, elapsed={2}", Thread.CurrentThread.Name, dataTable.Rows.Count, tickTimer.getElapsed().ToString()), new string[0]);
				}
				else
				{
					MultiThreadQuery.WriteLog(string.Format("----------------- [{0}] finished, elapsed={1}", Thread.CurrentThread.Name, tickTimer.getElapsed().ToString()), new string[0]);
				}
				if (queryContext._ResultElapsed != null)
				{
					queryContext._ResultElapsed.Enqueue(tickTimer.getElapsed());
				}
				if (queryContext._finishSignal != null)
				{
					queryContext._finishSignal.Release();
				}
				if (queryContext._finishCounter != null)
				{
					queryContext._finishCounter.Release();
				}
			}
		}
		private void MergeResult()
		{
			if (this._ResultQueue != null && this._MergedResult != null)
			{
				DataTable dataTable;
				while (this._ResultQueue.Count > 0 && this._ResultQueue.TryDequeue(out dataTable))
				{
					int columnIndex = dataTable.Columns.IndexOf(this._MergedKeyColName);
					int columnIndex2 = dataTable.Columns.IndexOf(this._MergedValueColName);
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						foreach (DataRow dataRow in dataTable.Rows)
						{
							string text;
							if (string.IsNullOrEmpty(this._MergedKeyDateFormat))
							{
								text = Convert.ToString(dataRow[columnIndex]);
							}
							else
							{
								if (this._MergedKeyDateFormat.Equals("q", StringComparison.InvariantCultureIgnoreCase))
								{
									DateTime dateTime = Convert.ToDateTime(dataRow[columnIndex]);
									int month = dateTime.Month;
									int num = (month - 1) / 3 + 1;
									text = string.Format("{0}Q{1}", dateTime.ToString("yyyy"), num);
								}
								else
								{
									text = Convert.ToDateTime(dataRow[columnIndex]).ToString(this._MergedKeyDateFormat);
								}
							}
							double num2 = Convert.ToDouble(dataRow[columnIndex2]);
							if (!this._MergedResult.ContainsKey(text))
							{
								this._MergedResult.Add(text, num2);
							}
							else
							{
								if (this._MergedType.Equals("MAX"))
								{
									if (num2 > this._MergedResult[text])
									{
										this._MergedResult[text] = num2;
									}
								}
								else
								{
									if (this._MergedType.Equals("MIN"))
									{
										if (num2 < this._MergedResult[text])
										{
											this._MergedResult[text] = num2;
										}
									}
									else
									{
										if (this._MergedType.Equals("SUM"))
										{
											Dictionary<string, double> mergedResult;
											string key;
											(mergedResult = this._MergedResult)[key = text] = mergedResult[key] + num2;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private bool ParallelQuery(int nMaxConcurrentThreads, List<MultiThreadQuery.QueryContext> contexts)
		{
			if (contexts.Count <= 0)
			{
				return false;
			}
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			List<MultiThreadQuery.QueryContext> list = new List<MultiThreadQuery.QueryContext>();
			for (int i = 0; i < contexts.Count; i++)
			{
				list.Add(contexts[i]);
			}
			int num = 0;
			long num2 = 0L;
			double num3 = 0.0;
			string testOption = MultiThreadQuery.GetTestOption("query_thread_gap(ms)");
			if (!string.IsNullOrEmpty(testOption))
			{
				num = Convert.ToInt32(testOption);
			}
			if (num == 0)
			{
				num3 = 0.05;
				string testOption2 = MultiThreadQuery.GetTestOption("query_thread_delay_factor");
				if (!string.IsNullOrEmpty(testOption2))
				{
					num3 = Convert.ToDouble(testOption2);
				}
			}
			Semaphore semaphore = new Semaphore(Math.Min(nMaxConcurrentThreads, contexts.Count), Math.Min(nMaxConcurrentThreads, contexts.Count));
			SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0, contexts.Count);
			long num4 = 0L;
			TickTimer tickTimer2 = new TickTimer();
			num4 = 0L;
			tickTimer2.Start();
			while (true)
			{
				string interProcessKeyValue = InterProcessShared<Dictionary<string, string>>.getInterProcessKeyValue("MultiThreadUpdateStatus");
				if (!string.IsNullOrEmpty(interProcessKeyValue) && interProcessKeyValue.Equals("busy", StringComparison.InvariantCultureIgnoreCase))
				{
					if (num4 == 0L)
					{
						MultiThreadQuery.WriteLog("==============>Report delayed by update, waiting ...", new string[0]);
					}
					num4 += 1L;
					try
					{
						Thread.Sleep(100);
						continue;
					}
					catch (Exception)
					{
						continue;
					}
					break;
				}
				break;
			}
			if (num4 > 0L)
			{
				MultiThreadQuery.WriteLog("==============>Report continued after update blocking, yield:{0}", new string[]
				{
					tickTimer2.getElapsed().ToString()
				});
			}
			int num5 = 1;
			while (list.Count > 0)
			{
				semaphore.WaitOne();
				num4 = 0L;
				tickTimer2.Start();
				while (true)
				{
					string interProcessKeyValue2 = InterProcessShared<Dictionary<string, string>>.getInterProcessKeyValue("MultiThreadUpdateStatus");
					if (!string.IsNullOrEmpty(interProcessKeyValue2) && interProcessKeyValue2.Equals("busy", StringComparison.InvariantCultureIgnoreCase))
					{
						if (num4 == 0L)
						{
							MultiThreadQuery.WriteLog("==============>Report interrupted by update, waiting ...", new string[0]);
						}
						num4 += 1L;
						try
						{
							Thread.Sleep(100);
							continue;
						}
						catch (Exception)
						{
							continue;
						}
						break;
					}
					break;
				}
				if (num4 > 0L)
				{
					MultiThreadQuery.WriteLog("==============>Report continued after update interrupt, yield:{0}", new string[]
					{
						tickTimer2.getElapsed().ToString()
					});
				}
				MultiThreadQuery.QueryContext queryContext = list[0];
				list.RemoveAt(0);
				try
				{
					long num6 = 0L;
					if (this._ResultElapsed.Count > 0 && this._ResultElapsed.TryDequeue(out num6))
					{
						num2 += num6;
						num2 >>= 1;
					}
					int num7 = num;
					if (num == 0)
					{
						num7 = Convert.ToInt32(num3 * (double)num2);
						MultiThreadQuery.WriteLog("              Adjusted thread delay: {0}, avg={1}, last={2}", new string[]
						{
							num7.ToString(),
							num2.ToString(),
							num6.ToString()
						});
					}
					if (num7 > 0)
					{
						Thread.Sleep(num7);
					}
				}
				catch (Exception)
				{
				}
				Thread thread = new Thread(new ParameterizedThreadStart(this.OneTableQuery));
				thread.Name = string.Format("MultiThreadQuery #{0}", num5++);
				thread.IsBackground = true;
				queryContext._ResultElapsed = this._ResultElapsed;
				queryContext._finishSignal = semaphore;
				queryContext._finishCounter = semaphoreSlim;
				thread.Start(queryContext);
				this.MergeResult();
			}
			try
			{
				while (semaphoreSlim.CurrentCount < contexts.Count)
				{
					Thread.Sleep(20);
				}
				this.MergeResult();
				MultiThreadQuery.WriteLog("All queries done: elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			}
			catch (Exception ex)
			{
				MultiThreadQuery.WriteLog(ex.Message, new string[0]);
			}
			return true;
		}
		public bool IsNumeric(string value)
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
		public List<string> GetAllTableSuffixes(string strTablePrefix = "")
		{
			List<string> list = new List<string>();
			DBConn dBConn = null;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper() == "MYSQL")
			{
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn == null || dBConn.con == null)
					{
						return list;
					}
					DbCommand dbCommand = dBConn.con.CreateCommand();
					if (string.IsNullOrEmpty(strTablePrefix))
					{
						dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where (table_name like '%_auto_info%' or table_name like '%_data_daily%' or table_name like '%_data_hourly%') and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
					}
					else
					{
						dbCommand.CommandText = string.Format("SELECT table_name FROM INFORMATION_SCHEMA.TABLES where table_name like '{0}%' and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ", strTablePrefix);
					}
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
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
					dbCommand.Dispose();
					dBConn.Close();
					return list;
				}
				catch (Exception)
				{
					if (dBConn != null)
					{
						dBConn.Close();
					}
					return list;
				}
			}
			try
			{
				string text3 = AppDomain.CurrentDomain.BaseDirectory;
				if (!text3.EndsWith("/") && !text3.EndsWith("\\"))
				{
					text3 += "\\";
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(text3 + "datadb\\");
				FileInfo[] files = directoryInfo.GetFiles("*.mdb");
				for (int i = 0; i < files.Length; i++)
				{
					FileInfo fileInfo = files[i];
					string text4 = fileInfo.FullName;
					int num = text4.LastIndexOf(".");
					if (num >= 0)
					{
						text4 = text4.Substring(0, num);
					}
					if (text4.Length >= 8)
					{
						text4 = text4.Substring(text4.Length - 8, 8);
						if (!list.Contains(text4) && this.IsNumeric(text4))
						{
							list.Add(text4);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}
		private static DataTable MergeDataTable(ArrayList listDataTable, string PrimaryKey)
		{
			DataTable dataTable = null;
			if (listDataTable.Count == 1)
			{
				dataTable = (DataTable)listDataTable[0];
			}
			else
			{
				if (listDataTable.Count > 1)
				{
					dataTable = (DataTable)listDataTable[0];
					dataTable.PrimaryKey = new DataColumn[]
					{
						dataTable.Columns[PrimaryKey]
					};
					for (int i = 1; i < listDataTable.Count; i++)
					{
						DataTable dataTable2 = (DataTable)listDataTable[i];
						dataTable2.PrimaryKey = new DataColumn[]
						{
							dataTable2.Columns[PrimaryKey]
						};
						dataTable.Merge(dataTable2);
					}
				}
			}
			return dataTable;
		}
		public static DataTable GetChart1Data(string str_Start, string str_End, string device_id, string portid, string groupby, string strgrouptype, string dblibnameDev, string dblibnamePort)
		{
			try
			{
				MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.Select = "select sum(power_consumption) as power_consumption," + groupby + " as period";
				multiThreadQuery.StartTime = str_Start;
				multiThreadQuery.EndTime = str_End;
				multiThreadQuery.GroupBy = groupby;
				multiThreadQuery.OrderBy = groupby + " ASC";
				multiThreadQuery.CalcColName = "power_consumption";
				multiThreadQuery.CalcType = "SUM";
				if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
				{
					multiThreadQuery.IsPort = true;
					multiThreadQuery.TablePrefix = dblibnamePort;
					multiThreadQuery.FilterList = portid;
				}
				else
				{
					multiThreadQuery.IsPort = false;
					multiThreadQuery.TablePrefix = dblibnameDev;
					multiThreadQuery.FilterList = device_id;
				}
				if (MultiThreadQuery._nMergeAfterThreadDone > 0)
				{
					multiThreadQuery._MergedType = "SUM";
					multiThreadQuery._MergedKeyColName = "period";
					multiThreadQuery._MergedKeyDateFormat = "";
					multiThreadQuery._MergedValueColName = "power_consumption";
					multiThreadQuery._MergedResult = new Dictionary<string, double>();
					multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
					ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
					if (arrayList != null && multiThreadQuery._MergedResult != null)
					{
						DataTable dataTable = new DataTable();
						dataTable.Columns.Add(multiThreadQuery._MergedValueColName, typeof(double));
						dataTable.Columns.Add(multiThreadQuery._MergedKeyColName, typeof(string));
						foreach (string current in multiThreadQuery._MergedResult.Keys)
						{
							dataTable.Rows.Add(new object[]
							{
								multiThreadQuery._MergedResult[current],
								current
							});
						}
						dataTable = new DataView(dataTable)
						{
							Sort = "period ASC"
						}.ToTable();
						DataTable result = dataTable;
						return result;
					}
				}
				else
				{
					List<MultiThreadQuery.QueryContext> list = multiThreadQuery.PrepareQueryContext();
					List<MultiThreadQuery.QueryContext> list2 = new List<MultiThreadQuery.QueryContext>();
					Dictionary<string, double> dictionary = new Dictionary<string, double>();
					DataTable dataTable2 = null;
					for (int i = 0; i < list.Count; i++)
					{
						MultiThreadQuery.QueryContext item = list[i];
						list2.Add(item);
						if (list2.Count >= MultiThreadQuery._nThreadCount || i >= list.Count - 1)
						{
							if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list2))
							{
								foreach (MultiThreadQuery.QueryContext current2 in list2)
								{
									DataTable resultInfo = current2._resultInfo;
									if (resultInfo != null)
									{
										if (dataTable2 == null)
										{
											dataTable2 = resultInfo.Clone();
										}
										foreach (DataRow dataRow in resultInfo.Rows)
										{
											string key = Convert.ToString(dataRow["period"]);
											double num = Convert.ToDouble(dataRow["power_consumption"]);
											if (dictionary.ContainsKey(key))
											{
												num += dictionary[key];
												dictionary[key] = num;
											}
											else
											{
												dictionary.Add(key, num);
											}
										}
										current2._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list2 = new List<MultiThreadQuery.QueryContext>();
						}
					}
					if (dataTable2 != null)
					{
						foreach (string current3 in dictionary.Keys)
						{
							dataTable2.Rows.Add(new object[]
							{
								dictionary[current3],
								current3
							});
						}
						dataTable2 = new DataView(dataTable2)
						{
							Sort = "period ASC"
						}.ToTable();
						DataTable result = dataTable2;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetChart1Data: " + ex.Message + "\r\n" + str, new string[0]);
			}
			return new DataTable();
		}
		public static DataTable GetChart2Data(int nThreadCount, string str_Start, string str_End, string device_id, string portid, string groupby, string strgrouptype)
		{
			DataTable result;
			try
			{
				MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.Select = "select sum(power) as power_value, insert_time as period ";
				multiThreadQuery.StartTime = str_Start;
				multiThreadQuery.EndTime = str_End;
				multiThreadQuery.GroupBy = "insert_time";
				multiThreadQuery.OrderBy = "";
				multiThreadQuery.CalcColName = "power_value";
				multiThreadQuery.CalcType = "SUM";
				if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
				{
					multiThreadQuery.IsPort = true;
					multiThreadQuery.TablePrefix = "port_auto_info";
					multiThreadQuery.FilterList = portid;
				}
				else
				{
					multiThreadQuery.IsPort = false;
					multiThreadQuery.TablePrefix = "device_auto_info";
					multiThreadQuery.FilterList = device_id;
				}
				int num = 0;
				if (groupby != null)
				{
					if (<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x600002d-1 == null)
					{
						<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x600002d-1 = new Dictionary<string, int>(8)
						{

							{
								"date_format(insert_time, '%Y-%m-%d %H')",
								0
							},

							{
								"FORMAT(insert_time, 'yyyy-MM-dd HH')",
								1
							},

							{
								"date_format(insert_time, '%Y-%m-%d')",
								2
							},

							{
								"FORMAT(insert_time, 'yyyy-MM-dd')",
								3
							},

							{
								"date_format(insert_time, '%Y-%m')",
								4
							},

							{
								"FORMAT(insert_time, 'yyyy-MM')",
								5
							},

							{
								"concat(date_format(insert_time, '%Y'),'Q',quarter(insert_time))",
								6
							},

							{
								"FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q')",
								7
							}
						};
					}
					int num2;
					if (<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x600002d-1.TryGetValue(groupby, out num2))
					{
						switch (num2)
						{
						case 0:
						case 1:
							num = 1;
							break;
						case 2:
						case 3:
							num = 2;
							break;
						case 4:
						case 5:
							num = 4;
							break;
						case 6:
						case 7:
							num = 5;
							if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
							{
								multiThreadQuery.Select = "select sum(power) as power_value, insert_time as period ";
							}
							else
							{
								multiThreadQuery.Select = "select sum(power) as power_value, insert_time as period";
							}
							break;
						default:
							goto IL_197;
						}
						Dictionary<string, MultiThreadQuery.PeakInfo> dictionary = new Dictionary<string, MultiThreadQuery.PeakInfo>();
						if (MultiThreadQuery._nMergeAfterThreadDone > 0)
						{
							multiThreadQuery._MergedType = "MAX";
							multiThreadQuery._MergedKeyColName = "period";
							multiThreadQuery._MergedKeyDateFormat = "";
							multiThreadQuery._MergedValueColName = "power_value";
							multiThreadQuery._MergedResult = new Dictionary<string, double>();
							multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
							if (num == 1)
							{
								multiThreadQuery._MergedKeyDateFormat = "yyyy-MM-dd HH";
							}
							else
							{
								if (num == 2)
								{
									multiThreadQuery._MergedKeyDateFormat = "yyyy-MM-dd";
								}
								else
								{
									if (num == 4)
									{
										multiThreadQuery._MergedKeyDateFormat = "yyyy-MM";
									}
									else
									{
										if (num == 5)
										{
											multiThreadQuery._MergedKeyDateFormat = "q";
										}
									}
								}
							}
							ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
							if (arrayList == null || multiThreadQuery._MergedResult == null)
							{
								goto IL_499;
							}
							using (Dictionary<string, double>.KeyCollection.Enumerator enumerator = multiThreadQuery._MergedResult.Keys.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									string current = enumerator.Current;
									dictionary.Add(current, new MultiThreadQuery.PeakInfo(current, multiThreadQuery._MergedResult[current], DateTime.Now, DateTime.Now));
								}
								goto IL_499;
							}
						}
						List<MultiThreadQuery.QueryContext> list = multiThreadQuery.PrepareQueryContext();
						List<MultiThreadQuery.QueryContext> list2 = new List<MultiThreadQuery.QueryContext>();
						for (int i = 0; i < list.Count; i++)
						{
							MultiThreadQuery.QueryContext item = list[i];
							list2.Add(item);
							if (list2.Count >= MultiThreadQuery._nThreadCount || i >= list.Count - 1)
							{
								if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list2))
								{
									foreach (MultiThreadQuery.QueryContext current2 in list2)
									{
										DataTable resultInfo = current2._resultInfo;
										if (resultInfo != null)
										{
											foreach (DataRow dataRow in resultInfo.Rows)
											{
												string text;
												if (num == 1)
												{
													text = Convert.ToDateTime(dataRow[1]).ToString("yyyy-MM-dd HH");
												}
												else
												{
													if (num == 2)
													{
														text = Convert.ToDateTime(dataRow[1]).ToString("yyyy-MM-dd");
													}
													else
													{
														if (num == 4)
														{
															text = Convert.ToDateTime(dataRow[1]).ToString("yyyy-MM");
														}
														else
														{
															if (num != 5)
															{
																break;
															}
															text = Convert.ToString(dataRow[2]);
														}
													}
												}
												double num3 = Convert.ToDouble(dataRow[0]);
												if (dictionary.ContainsKey(text))
												{
													if (num3 > dictionary[text]._dblPeakPower)
													{
														dictionary[text]._dblPeakPower = num3;
													}
												}
												else
												{
													dictionary.Add(text, new MultiThreadQuery.PeakInfo(text, num3, DateTime.Now, DateTime.Now));
												}
											}
											current2._resultInfo = null;
										}
									}
								}
								GC.Collect();
								list2 = new List<MultiThreadQuery.QueryContext>();
							}
						}
						IL_499:
						DataTable dataTable = new DataTable();
						DataColumn dataColumn = new DataColumn("power");
						dataColumn.DataType = Type.GetType("System.Double");
						dataTable.Columns.Add(dataColumn);
						DataColumn dataColumn2 = new DataColumn("starttime");
						dataColumn2.DataType = Type.GetType("System.DateTime");
						dataTable.Columns.Add(dataColumn2);
						DataColumn dataColumn3 = new DataColumn("endtime");
						dataColumn3.DataType = Type.GetType("System.DateTime");
						dataTable.Columns.Add(dataColumn3);
						DataColumn dataColumn4 = new DataColumn("period");
						dataColumn4.DataType = Type.GetType("System.String");
						dataTable.Columns.Add(dataColumn4);
						foreach (KeyValuePair<string, MultiThreadQuery.PeakInfo> current3 in dictionary)
						{
							DataRow dataRow2 = dataTable.NewRow();
							dataRow2[0] = current3.Value._dblPeakPower / 1000.0;
							dataRow2[1] = current3.Value._PeriodFrom;
							dataRow2[2] = current3.Value._PeriodTo;
							dataRow2[3] = current3.Value._PeakPeriod;
							dataTable.Rows.Add(dataRow2);
						}
						result = dataTable;
						return result;
					}
				}
				IL_197:
				result = null;
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetChart2Data: " + ex.Message + "\r\n" + str, new string[0]);
				result = new DataTable();
			}
			return result;
		}
		public static DataTable GetOutLetPowerAndName(string strgrouptype, string strBegin, string strEnd, string portid, string invalid_device_ids)
		{
			DataTable result;
			try
			{
				MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.StartTime = strBegin;
				multiThreadQuery.EndTime = strEnd;
				multiThreadQuery.CalcType = "SUM";
				Hashtable hashtable = new Hashtable();
				if (strgrouptype != "alloutlet" && strgrouptype != "outlet")
				{
					multiThreadQuery.IsPort = false;
					multiThreadQuery.TablePrefix = "device_auto_info";
					multiThreadQuery.FilterList = invalid_device_ids;
					multiThreadQuery.Select = "select device_id,max(power)/1000 as power";
					multiThreadQuery.PrerequisiteSQL = "";
					multiThreadQuery.GroupBy = "device_id";
					multiThreadQuery.OrderBy = "";
					if (MultiThreadQuery._nMergeAfterThreadDone > 0)
					{
						multiThreadQuery._MergedType = "MAX";
						multiThreadQuery._MergedKeyColName = "device_id";
						multiThreadQuery._MergedKeyDateFormat = "";
						multiThreadQuery._MergedValueColName = "power";
						multiThreadQuery._MergedResult = new Dictionary<string, double>();
						multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
						ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
						if (arrayList == null || multiThreadQuery._MergedResult == null)
						{
							goto IL_2DB;
						}
						using (Dictionary<string, double>.KeyCollection.Enumerator enumerator = multiThreadQuery._MergedResult.Keys.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string current = enumerator.Current;
								hashtable.Add(Convert.ToInt32(current), multiThreadQuery._MergedResult[current]);
							}
							goto IL_2DB;
						}
					}
					List<MultiThreadQuery.QueryContext> list = multiThreadQuery.PrepareQueryContext();
					List<MultiThreadQuery.QueryContext> list2 = new List<MultiThreadQuery.QueryContext>();
					for (int i = 0; i < list.Count; i++)
					{
						MultiThreadQuery.QueryContext item = list[i];
						list2.Add(item);
						if (list2.Count >= MultiThreadQuery._nThreadCount || i >= list.Count - 1)
						{
							if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list2))
							{
								foreach (MultiThreadQuery.QueryContext current2 in list2)
								{
									DataTable resultInfo = current2._resultInfo;
									if (resultInfo != null)
									{
										foreach (DataRow dataRow in resultInfo.Rows)
										{
											int num = Convert.ToInt32(dataRow["device_id"]);
											double num2 = Convert.ToDouble(dataRow["power"]);
											if (hashtable != null && hashtable.ContainsKey(num))
											{
												double num3 = (double)hashtable[num];
												if (num2 > num3)
												{
													hashtable[num] = num2;
												}
											}
											else
											{
												hashtable.Add(num, num2);
											}
										}
										current2._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list2 = new List<MultiThreadQuery.QueryContext>();
						}
					}
				}
				IL_2DB:
				multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.StartTime = strBegin;
				multiThreadQuery.EndTime = strEnd;
				multiThreadQuery.CalcType = "SUM";
				multiThreadQuery.IsPort = true;
				multiThreadQuery.TablePrefix = "port_auto_info";
				multiThreadQuery.Select = "select port_id,max(power)/1000 as power";
				multiThreadQuery.PrerequisiteSQL = "";
				multiThreadQuery.FilterList = portid;
				multiThreadQuery.GroupBy = "port_id";
				multiThreadQuery.OrderBy = "";
				Hashtable hashtable2 = new Hashtable();
				if (MultiThreadQuery._nMergeAfterThreadDone > 0)
				{
					multiThreadQuery._MergedType = "MAX";
					multiThreadQuery._MergedKeyColName = "port_id";
					multiThreadQuery._MergedKeyDateFormat = "";
					multiThreadQuery._MergedValueColName = "power";
					multiThreadQuery._MergedResult = new Dictionary<string, double>();
					multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
					ArrayList arrayList2 = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
					if (arrayList2 == null || multiThreadQuery._MergedResult == null)
					{
						goto IL_5A1;
					}
					using (Dictionary<string, double>.KeyCollection.Enumerator enumerator4 = multiThreadQuery._MergedResult.Keys.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							string current3 = enumerator4.Current;
							hashtable2.Add(Convert.ToInt32(current3), multiThreadQuery._MergedResult[current3]);
						}
						goto IL_5A1;
					}
				}
				List<MultiThreadQuery.QueryContext> list3 = multiThreadQuery.PrepareQueryContext();
				List<MultiThreadQuery.QueryContext> list4 = new List<MultiThreadQuery.QueryContext>();
				for (int j = 0; j < list3.Count; j++)
				{
					MultiThreadQuery.QueryContext item2 = list3[j];
					list4.Add(item2);
					if (list4.Count >= MultiThreadQuery._nThreadCount || j >= list3.Count - 1)
					{
						if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list4))
						{
							foreach (MultiThreadQuery.QueryContext current4 in list4)
							{
								DataTable resultInfo2 = current4._resultInfo;
								if (resultInfo2 != null)
								{
									foreach (DataRow dataRow2 in resultInfo2.Rows)
									{
										int num4 = Convert.ToInt32(dataRow2["port_id"]);
										double num5 = Convert.ToDouble(dataRow2["power"]);
										if (hashtable2 != null && hashtable2.ContainsKey(num4))
										{
											double num6 = (double)hashtable2[num4];
											if (num5 > num6)
											{
												hashtable2[num4] = num5;
											}
										}
										else
										{
											hashtable2.Add(num4, num5);
										}
									}
									current4._resultInfo = null;
								}
							}
						}
						GC.Collect();
						list4 = new List<MultiThreadQuery.QueryContext>();
					}
				}
				IL_5A1:
				DataTable dataTable = new DataTable();
				DataColumn dataColumn = new DataColumn("power");
				dataColumn.DataType = Type.GetType("System.Double");
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn("server_name");
				dataColumn.DataType = Type.GetType("System.String");
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn("server_id");
				dataColumn.DataType = Type.GetType("System.Int32");
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn("pdu_id");
				dataColumn.DataType = Type.GetType("System.Int32");
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn("device_name");
				dataColumn.DataType = Type.GetType("System.String");
				dataTable.Columns.Add(dataColumn);
				dataColumn = new DataColumn("port_id");
				dataColumn.DataType = Type.GetType("System.Int32");
				dataTable.Columns.Add(dataColumn);
				Hashtable deviceCache = DBCache.GetDeviceCache();
				Hashtable portCache = DBCache.GetPortCache();
				if (hashtable != null && hashtable.Count > 0)
				{
					ICollection keys = hashtable.Keys;
					foreach (int num7 in keys)
					{
						if (deviceCache != null && deviceCache.ContainsKey(num7))
						{
							DeviceInfo deviceInfo = (DeviceInfo)deviceCache[num7];
							DataRow dataRow3 = dataTable.NewRow();
							dataRow3[0] = (double)hashtable[num7];
							dataRow3[1] = deviceInfo.DeviceName;
							dataRow3[2] = num7;
							dataRow3[3] = num7;
							dataRow3[4] = deviceInfo.DeviceName;
							dataRow3[5] = 0;
							dataTable.Rows.Add(dataRow3);
						}
					}
				}
				if (hashtable2 != null && hashtable2.Count > 0)
				{
					ICollection keys2 = hashtable2.Keys;
					foreach (int num8 in keys2)
					{
						if (portCache != null && portCache.ContainsKey(num8))
						{
							PortInfo portInfo = (PortInfo)portCache[num8];
							if (deviceCache != null && deviceCache.ContainsKey(portInfo.DeviceID))
							{
								DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache[portInfo.DeviceID];
								DataRow dataRow4 = dataTable.NewRow();
								dataRow4[0] = (double)hashtable2[num8];
								dataRow4[1] = deviceInfo2.DeviceName + " " + portInfo.PortName;
								dataRow4[2] = num8;
								dataRow4[3] = portInfo.DeviceID;
								dataRow4[4] = deviceInfo2.DeviceName;
								dataRow4[5] = num8;
								dataTable.Rows.Add(dataRow4);
							}
						}
					}
				}
				DataTable dataTable2 = new DataView(dataTable)
				{
					Sort = "device_name, port_id"
				}.ToTable();
				dataTable2.Columns.Remove(dataTable2.Columns["device_name"]);
				dataTable2.Columns.Remove(dataTable2.Columns["port_id"]);
				result = dataTable2;
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetOutLetPowerAndName: " + ex.Message + "\r\n" + str, new string[0]);
				result = new DataTable();
			}
			return result;
		}
		public static DataTable GetOutLetPDAndName(string strgrouptype, string dblibnamePort, string dblibnameDev, string strBegin, string strEnd, string portid, string invalid_device_ids)
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("server_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("power_consumption");
			dataColumn.DataType = Type.GetType("System.Double");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("server_name");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("pdu_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("device_name");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("port_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			DataTable result;
			try
			{
				MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.StartTime = strBegin;
				multiThreadQuery.EndTime = strEnd;
				multiThreadQuery.CalcType = "SUM";
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (strgrouptype != "alloutlet" && strgrouptype != "outlet")
				{
					multiThreadQuery.IsPort = false;
					if (dblibnameDev.IndexOf("hourly") >= 0)
					{
						multiThreadQuery.Select = "select device_id,sum(power_consumption) as power_consumption";
						multiThreadQuery.GroupBy = "device_id";
					}
					else
					{
						multiThreadQuery.Select = "select device_id,power_consumption as power_consumption";
						multiThreadQuery.GroupBy = "";
					}
					multiThreadQuery.TablePrefix = dblibnameDev;
					multiThreadQuery.FilterList = invalid_device_ids;
					multiThreadQuery.OrderBy = "";
					if (MultiThreadQuery._nMergeAfterThreadDone > 0)
					{
						multiThreadQuery._MergedType = "SUM";
						multiThreadQuery._MergedKeyColName = "device_id";
						multiThreadQuery._MergedKeyDateFormat = "";
						multiThreadQuery._MergedValueColName = "power_consumption";
						multiThreadQuery._MergedResult = new Dictionary<string, double>();
						multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
						ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
						if (arrayList == null || multiThreadQuery._MergedResult == null)
						{
							goto IL_53C;
						}
						using (Dictionary<string, double>.KeyCollection.Enumerator enumerator = multiThreadQuery._MergedResult.Keys.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string current = enumerator.Current;
								int num = Convert.ToInt32(current);
								if (deviceCache != null && deviceCache.ContainsKey(num))
								{
									DeviceInfo deviceInfo = (DeviceInfo)deviceCache[num];
									DataRow dataRow = dataTable.NewRow();
									dataRow[0] = num;
									dataRow[1] = multiThreadQuery._MergedResult[current];
									dataRow[2] = deviceInfo.DeviceName;
									dataRow[3] = num;
									dataRow[4] = deviceInfo.DeviceName;
									dataRow[5] = 0;
									dataTable.Rows.Add(dataRow);
								}
							}
							goto IL_53C;
						}
					}
					List<MultiThreadQuery.QueryContext> list = multiThreadQuery.PrepareQueryContext();
					List<MultiThreadQuery.QueryContext> list2 = new List<MultiThreadQuery.QueryContext>();
					Dictionary<int, double> dictionary = new Dictionary<int, double>();
					for (int i = 0; i < list.Count; i++)
					{
						MultiThreadQuery.QueryContext item = list[i];
						list2.Add(item);
						if (list2.Count >= MultiThreadQuery._nThreadCount || i >= list.Count - 1)
						{
							if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list2))
							{
								foreach (MultiThreadQuery.QueryContext current2 in list2)
								{
									DataTable resultInfo = current2._resultInfo;
									if (resultInfo != null)
									{
										foreach (DataRow dataRow2 in resultInfo.Rows)
										{
											int num2 = Convert.ToInt32(dataRow2["device_id"]);
											if (dictionary.ContainsKey(num2))
											{
												Dictionary<int, double> dictionary2;
												int key;
												(dictionary2 = dictionary)[key = num2] = dictionary2[key] + Convert.ToDouble(dataRow2["power_consumption"]);
											}
											else
											{
												dictionary[num2] = Convert.ToDouble(dataRow2["power_consumption"]);
											}
										}
										current2._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list2 = new List<MultiThreadQuery.QueryContext>();
						}
					}
					foreach (KeyValuePair<int, double> current3 in dictionary)
					{
						int key2 = current3.Key;
						if (deviceCache != null && deviceCache.ContainsKey(key2))
						{
							DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache[key2];
							DataRow dataRow3 = dataTable.NewRow();
							dataRow3[0] = key2;
							dataRow3[1] = current3.Value;
							dataRow3[2] = deviceInfo2.DeviceName;
							dataRow3[3] = key2;
							dataRow3[4] = deviceInfo2.DeviceName;
							dataRow3[5] = 0;
							dataTable.Rows.Add(dataRow3);
						}
					}
				}
				IL_53C:
				multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.StartTime = strBegin;
				multiThreadQuery.EndTime = strEnd;
				multiThreadQuery.CalcType = "SUM";
				multiThreadQuery.IsPort = true;
				multiThreadQuery.TablePrefix = dblibnamePort;
				if (dblibnamePort.IndexOf("hourly") >= 0)
				{
					multiThreadQuery.Select = "select port_id,sum(power_consumption) as power_consumption";
					multiThreadQuery.GroupBy = "port_id";
				}
				else
				{
					multiThreadQuery.Select = "select port_id,power_consumption as power_consumption";
					multiThreadQuery.GroupBy = "";
				}
				multiThreadQuery.FilterList = portid;
				multiThreadQuery.OrderBy = "";
				if (MultiThreadQuery._nMergeAfterThreadDone > 0)
				{
					multiThreadQuery._MergedType = "SUM";
					multiThreadQuery._MergedKeyColName = "port_id";
					multiThreadQuery._MergedKeyDateFormat = "";
					multiThreadQuery._MergedValueColName = "power_consumption";
					multiThreadQuery._MergedResult = new Dictionary<string, double>();
					multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
					ArrayList arrayList2 = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
					if (arrayList2 == null || multiThreadQuery._MergedResult == null)
					{
						goto IL_9FC;
					}
					Hashtable portCache = DBCache.GetPortCache();
					using (Dictionary<string, double>.KeyCollection.Enumerator enumerator5 = multiThreadQuery._MergedResult.Keys.GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							string current4 = enumerator5.Current;
							int num3 = Convert.ToInt32(current4);
							if (portCache != null && portCache.ContainsKey(num3))
							{
								PortInfo portInfo = (PortInfo)portCache[num3];
								DataRow dataRow4 = dataTable.NewRow();
								dataRow4[0] = num3;
								dataRow4[1] = multiThreadQuery._MergedResult[current4];
								dataRow4[2] = portInfo.PortName;
								dataRow4[3] = portInfo.DeviceID;
								dataRow4[4] = "";
								if (deviceCache != null && deviceCache.ContainsKey(portInfo.DeviceID))
								{
									DeviceInfo deviceInfo3 = (DeviceInfo)deviceCache[portInfo.DeviceID];
									dataRow4[4] = deviceInfo3.DeviceName;
								}
								dataRow4[5] = num3;
								dataTable.Rows.Add(dataRow4);
							}
						}
						goto IL_9FC;
					}
				}
				List<MultiThreadQuery.QueryContext> list3 = multiThreadQuery.PrepareQueryContext();
				List<MultiThreadQuery.QueryContext> list4 = new List<MultiThreadQuery.QueryContext>();
				Dictionary<int, double> dictionary3 = new Dictionary<int, double>();
				for (int j = 0; j < list3.Count; j++)
				{
					MultiThreadQuery.QueryContext item2 = list3[j];
					list4.Add(item2);
					if (list4.Count >= MultiThreadQuery._nThreadCount || j >= list3.Count - 1)
					{
						if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list4))
						{
							foreach (MultiThreadQuery.QueryContext current5 in list4)
							{
								DataTable resultInfo2 = current5._resultInfo;
								if (resultInfo2 != null)
								{
									foreach (DataRow dataRow5 in resultInfo2.Rows)
									{
										int num4 = Convert.ToInt32(dataRow5["port_id"]);
										if (dictionary3.ContainsKey(num4))
										{
											Dictionary<int, double> dictionary4;
											int key3;
											(dictionary4 = dictionary3)[key3 = num4] = dictionary4[key3] + Convert.ToDouble(dataRow5["power_consumption"]);
										}
										else
										{
											dictionary3[num4] = Convert.ToDouble(dataRow5["power_consumption"]);
										}
									}
									current5._resultInfo = null;
								}
							}
						}
						GC.Collect();
						list4 = new List<MultiThreadQuery.QueryContext>();
					}
				}
				Hashtable portCache2 = DBCache.GetPortCache();
				foreach (KeyValuePair<int, double> current6 in dictionary3)
				{
					int key4 = current6.Key;
					if (portCache2 != null && portCache2.ContainsKey(key4))
					{
						PortInfo portInfo2 = (PortInfo)portCache2[key4];
						DataRow dataRow6 = dataTable.NewRow();
						dataRow6[0] = key4;
						dataRow6[1] = current6.Value;
						dataRow6[2] = portInfo2.PortName;
						dataRow6[3] = portInfo2.DeviceID;
						dataRow6[4] = "";
						if (deviceCache != null && deviceCache.ContainsKey(portInfo2.DeviceID))
						{
							DeviceInfo deviceInfo4 = (DeviceInfo)deviceCache[portInfo2.DeviceID];
							dataRow6[4] = deviceInfo4.DeviceName;
						}
						dataRow6[5] = key4;
						dataTable.Rows.Add(dataRow6);
					}
				}
				IL_9FC:
				DataTable dataTable2 = new DataView(dataTable)
				{
					Sort = "device_name, port_id"
				}.ToTable();
				dataTable2.Columns.Remove(dataTable2.Columns["device_name"]);
				dataTable2.Columns.Remove(dataTable2.Columns["port_id"]);
				result = dataTable2;
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetOutLetPDAndName: " + ex.Message + "\r\n" + str, new string[0]);
				result = new DataTable();
			}
			return result;
		}
		public static Hashtable GetRackPDSum(int i_period_type)
		{
			Hashtable hashtable = new Hashtable();
			try
			{
				MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.Select = "select sum(power_consumption)/" + 10000.0 + " as power_con,device_id";
				multiThreadQuery.GroupBy = "device_id";
				multiThreadQuery.OrderBy = "device_id ASC";
				multiThreadQuery.CalcType = "SUM";
				multiThreadQuery.IsPort = false;
				multiThreadQuery.FilterList = "";
				DateTime now = DateTime.Now;
				multiThreadQuery.TablePrefix = "device_data_daily";
				if (i_period_type + 1 == 1)
				{
					multiThreadQuery.TablePrefix = "device_data_hourly";
					multiThreadQuery.StartTime = now.ToString("yyyy-MM-dd HH:00:00");
					multiThreadQuery.EndTime = now.AddHours(1.0).ToString("yyyy-MM-dd HH:00:00");
				}
				else
				{
					if (i_period_type + 1 == 2)
					{
						multiThreadQuery.StartTime = now.ToString("yyyy-MM-dd 00:00:00");
						multiThreadQuery.EndTime = now.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00");
					}
					else
					{
						if (i_period_type + 1 == 3)
						{
							DateTime dateTime = now.AddDays((double)(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))));
							if (now.DayOfWeek == DayOfWeek.Sunday)
							{
								dateTime = dateTime.AddDays(-7.0);
							}
							multiThreadQuery.StartTime = dateTime.ToString("yyyy-MM-dd 00:00:00");
							multiThreadQuery.EndTime = dateTime.AddDays(7.0).ToString("yyyy-MM-dd 00:00:00");
						}
						else
						{
							if (i_period_type + 1 == 4)
							{
								DateTime dateTime2 = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
								multiThreadQuery.StartTime = dateTime2.ToString("yyyy-MM-dd 00:00:00");
								multiThreadQuery.EndTime = dateTime2.AddMonths(1).ToString("yyyy-MM-dd 00:00:00");
							}
							else
							{
								if (i_period_type + 1 == 5)
								{
									DateTime dateTime3 = new DateTime(now.Year, (now.Month - 1) / 3 * 3 + 1, 1, 0, 0, 0);
									multiThreadQuery.StartTime = dateTime3.ToString("yyyy-MM-dd 00:00:00");
									multiThreadQuery.EndTime = dateTime3.AddMonths(3).ToString("yyyy-MM-dd 00:00:00");
								}
								else
								{
									if (i_period_type + 1 == 6)
									{
										DateTime dateTime4 = new DateTime(now.Year, 1, 1, 0, 0, 0);
										multiThreadQuery.StartTime = dateTime4.ToString("yyyy-MM-dd 00:00:00");
										multiThreadQuery.EndTime = dateTime4.AddYears(1).ToString("yyyy-MM-dd 00:00:00");
									}
								}
							}
						}
					}
				}
				if (MultiThreadQuery._nMergeAfterThreadDone > 0)
				{
					multiThreadQuery._MergedType = "SUM";
					multiThreadQuery._MergedKeyColName = "device_id";
					multiThreadQuery._MergedKeyDateFormat = "";
					multiThreadQuery._MergedValueColName = "power_con";
					multiThreadQuery._MergedResult = new Dictionary<string, double>();
					multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
					ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
					if (arrayList == null || multiThreadQuery._MergedResult == null)
					{
						goto IL_580;
					}
					Hashtable deviceCache = DBCache.GetDeviceCache();
					using (Dictionary<string, double>.KeyCollection.Enumerator enumerator = multiThreadQuery._MergedResult.Keys.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current;
							int num = Convert.ToInt32(current);
							if (deviceCache.ContainsKey(num))
							{
								DeviceInfo deviceInfo = (DeviceInfo)deviceCache[num];
								long rackID = deviceInfo.RackID;
								double num2 = multiThreadQuery._MergedResult[current];
								if (hashtable.ContainsKey(rackID))
								{
									double num3 = (double)hashtable[rackID];
									hashtable[rackID] = num2 + num3;
								}
								else
								{
									hashtable.Add(rackID, num2);
								}
							}
						}
						goto IL_580;
					}
				}
				List<MultiThreadQuery.QueryContext> list = multiThreadQuery.PrepareQueryContext();
				List<MultiThreadQuery.QueryContext> list2 = new List<MultiThreadQuery.QueryContext>();
				Hashtable deviceCache2 = DBCache.GetDeviceCache();
				if (deviceCache2 != null && deviceCache2.Count > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						MultiThreadQuery.QueryContext item = list[i];
						list2.Add(item);
						if (list2.Count >= MultiThreadQuery._nThreadCount || i > list.Count - 1)
						{
							if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list2))
							{
								foreach (MultiThreadQuery.QueryContext current2 in list2)
								{
									DataTable resultInfo = current2._resultInfo;
									if (resultInfo != null)
									{
										foreach (DataRow dataRow in resultInfo.Rows)
										{
											int num4 = Convert.ToInt32(dataRow[1]);
											if (deviceCache2.ContainsKey(num4))
											{
												DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache2[num4];
												long rackID2 = deviceInfo2.RackID;
												double num5 = Convert.ToDouble(dataRow[0]);
												if (hashtable.ContainsKey(rackID2))
												{
													double num6 = (double)hashtable[rackID2];
													hashtable[rackID2] = num5 + num6;
												}
												else
												{
													hashtable.Add(rackID2, num5);
												}
											}
										}
										current2._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list2 = new List<MultiThreadQuery.QueryContext>();
						}
					}
				}
				IL_580:;
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetRackPDSum: " + ex.Message + "\r\n" + str, new string[0]);
			}
			return hashtable;
		}
		public static double GetDataCenterPDSum(int i_period_type)
		{
			double num = 0.0;
			try
			{
				MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
				multiThreadQuery.Select = "select sum(power_consumption)/" + 10000.0 + " as power_con";
				multiThreadQuery.GroupBy = "";
				multiThreadQuery.OrderBy = "";
				multiThreadQuery.CalcType = "SUM";
				multiThreadQuery.IsPort = false;
				multiThreadQuery.FilterList = "";
				DateTime now = DateTime.Now;
				multiThreadQuery.TablePrefix = "device_data_daily";
				if (i_period_type + 1 == 1)
				{
					multiThreadQuery.TablePrefix = "device_data_hourly";
					multiThreadQuery.StartTime = now.ToString("yyyy-MM-dd HH:00:00");
					multiThreadQuery.EndTime = now.AddHours(1.0).ToString("yyyy-MM-dd HH:00:00");
				}
				else
				{
					if (i_period_type + 1 == 2)
					{
						multiThreadQuery.StartTime = now.ToString("yyyy-MM-dd 00:00:00");
						multiThreadQuery.EndTime = now.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00");
					}
					else
					{
						if (i_period_type + 1 == 3)
						{
							DateTime dateTime = now.AddDays((double)(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))));
							if (now.DayOfWeek == DayOfWeek.Sunday)
							{
								dateTime = dateTime.AddDays(-7.0);
							}
							multiThreadQuery.StartTime = dateTime.ToString("yyyy-MM-dd 00:00:00");
							multiThreadQuery.EndTime = dateTime.AddDays(7.0).ToString("yyyy-MM-dd 00:00:00");
						}
						else
						{
							if (i_period_type + 1 == 4)
							{
								DateTime dateTime2 = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
								multiThreadQuery.StartTime = dateTime2.ToString("yyyy-MM-dd 00:00:00");
								multiThreadQuery.EndTime = dateTime2.AddMonths(1).ToString("yyyy-MM-dd 00:00:00");
							}
							else
							{
								if (i_period_type + 1 == 5)
								{
									DateTime dateTime3 = new DateTime(now.Year, (now.Month - 1) / 3 * 3 + 1, 1, 0, 0, 0);
									multiThreadQuery.StartTime = dateTime3.ToString("yyyy-MM-dd 00:00:00");
									multiThreadQuery.EndTime = dateTime3.AddMonths(3).ToString("yyyy-MM-dd 00:00:00");
								}
								else
								{
									if (i_period_type + 1 != 6)
									{
										return num;
									}
									DateTime dateTime4 = new DateTime(now.Year, 1, 1, 0, 0, 0);
									multiThreadQuery.StartTime = dateTime4.ToString("yyyy-MM-dd 00:00:00");
									multiThreadQuery.EndTime = dateTime4.AddYears(1).ToString("yyyy-MM-dd 00:00:00");
								}
							}
						}
					}
				}
				if (MultiThreadQuery._nMergeAfterThreadDone > 0)
				{
					ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
					if (arrayList == null)
					{
						goto IL_471;
					}
					IEnumerator enumerator = arrayList.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataTable dataTable = (DataTable)enumerator.Current;
							if (dataTable != null)
							{
								foreach (DataRow dataRow in dataTable.Rows)
								{
									if (dataRow["power_con"] != DBNull.Value)
									{
										num += Convert.ToDouble(dataRow["power_con"]);
									}
								}
							}
						}
						goto IL_471;
					}
					finally
					{
						IDisposable disposable2 = enumerator as IDisposable;
						if (disposable2 != null)
						{
							disposable2.Dispose();
						}
					}
				}
				List<MultiThreadQuery.QueryContext> list = multiThreadQuery.PrepareQueryContext();
				List<MultiThreadQuery.QueryContext> list2 = new List<MultiThreadQuery.QueryContext>();
				for (int i = 0; i < list.Count; i++)
				{
					MultiThreadQuery.QueryContext item = list[i];
					list2.Add(item);
					if (list2.Count >= MultiThreadQuery._nThreadCount || i >= list.Count - 1)
					{
						if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list2))
						{
							foreach (MultiThreadQuery.QueryContext current in list2)
							{
								DataTable resultInfo = current._resultInfo;
								if (resultInfo != null)
								{
									foreach (DataRow dataRow2 in resultInfo.Rows)
									{
										if (dataRow2["power_con"] != DBNull.Value)
										{
											num += Convert.ToDouble(dataRow2["power_con"]);
										}
									}
									current._resultInfo = null;
								}
							}
						}
						GC.Collect();
						list2 = new List<MultiThreadQuery.QueryContext>();
					}
				}
				IL_471:;
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetDataCenterPDSum: " + ex.Message + "\r\n" + str, new string[0]);
			}
			return num;
		}
		public static Hashtable GetCurrentTimePD2IDMap(DateTime time4query)
		{
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			Hashtable hashtable3 = new Hashtable();
			Hashtable hashtable4 = new Hashtable();
			Hashtable hashtable5 = new Hashtable();
			Hashtable hashtable6 = new Hashtable();
			Hashtable hashtable7 = new Hashtable();
			bool flag = false;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.IsServer)
			{
				flag = true;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			DataTable dataTable = new DataTable();
			try
			{
				dBConn = DBConnPool.getDynaConnection(time4query);
				if (dBConn != null && dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					if (flag)
					{
						dbCommand.CommandText = "select device_id from device_data_daily" + time4query.ToString("yyyyMMdd") + " ";
					}
					else
					{
						dbCommand.CommandText = "select device_id from device_data_daily ";
					}
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					hashtable2 = new Hashtable();
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						foreach (DataRow dataRow in dataTable.Rows)
						{
							int num = Convert.ToInt32(dataRow[0]);
							if (!hashtable2.ContainsKey(num))
							{
								hashtable2.Add(num, num);
							}
						}
					}
					dataTable = new DataTable();
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					if (flag)
					{
						dbCommand.CommandText = string.Concat(new string[]
						{
							"select device_id from device_data_hourly",
							time4query.ToString("yyyyMMdd"),
							" where insert_time = '",
							time4query.ToString("yyyy-MM-dd HH"),
							":30:00' "
						});
					}
					else
					{
						dbCommand.CommandText = "select device_id from device_data_hourly where insert_time = #" + time4query.ToString("yyyy-MM-dd HH") + ":30:00# ";
					}
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					hashtable3 = new Hashtable();
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						foreach (DataRow dataRow2 in dataTable.Rows)
						{
							int num2 = Convert.ToInt32(dataRow2[0]);
							if (!hashtable3.ContainsKey(num2))
							{
								hashtable3.Add(num2, num2);
							}
						}
					}
					dataTable = new DataTable();
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					if (flag)
					{
						dbCommand.CommandText = "select port_id from port_data_daily" + time4query.ToString("yyyyMMdd") + " ";
					}
					else
					{
						dbCommand.CommandText = "select port_id from port_data_daily ";
					}
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					hashtable6 = new Hashtable();
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						foreach (DataRow dataRow3 in dataTable.Rows)
						{
							int num3 = Convert.ToInt32(dataRow3[0]);
							if (!hashtable6.ContainsKey(num3))
							{
								hashtable6.Add(num3, num3);
							}
						}
					}
					dataTable = new DataTable();
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					if (flag)
					{
						dbCommand.CommandText = string.Concat(new string[]
						{
							"select port_id from port_data_hourly",
							time4query.ToString("yyyyMMdd"),
							" where insert_time = '",
							time4query.ToString("yyyy-MM-dd HH"),
							":30:00' "
						});
					}
					else
					{
						dbCommand.CommandText = "select port_id from port_data_hourly where insert_time = #" + time4query.ToString("yyyy-MM-dd HH") + ":30:00# ";
					}
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					hashtable7 = new Hashtable();
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						foreach (DataRow dataRow4 in dataTable.Rows)
						{
							int num4 = Convert.ToInt32(dataRow4[0]);
							if (!hashtable7.ContainsKey(num4))
							{
								hashtable7.Add(num4, num4);
							}
						}
					}
					dataTable = new DataTable();
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					if (flag)
					{
						dbCommand.CommandText = "select bank_id from bank_data_daily" + time4query.ToString("yyyyMMdd") + " ";
					}
					else
					{
						dbCommand.CommandText = "select bank_id from bank_data_daily ";
					}
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					hashtable4 = new Hashtable();
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						foreach (DataRow dataRow5 in dataTable.Rows)
						{
							int num5 = Convert.ToInt32(dataRow5[0]);
							if (!hashtable4.ContainsKey(num5))
							{
								hashtable4.Add(num5, num5);
							}
						}
					}
					dataTable = new DataTable();
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					if (flag)
					{
						dbCommand.CommandText = string.Concat(new string[]
						{
							"select bank_id from bank_data_hourly",
							time4query.ToString("yyyyMMdd"),
							" where insert_time = '",
							time4query.ToString("yyyy-MM-dd HH"),
							":30:00' "
						});
					}
					else
					{
						dbCommand.CommandText = "select bank_id from bank_data_hourly where insert_time = #" + time4query.ToString("yyyy-MM-dd HH") + ":30:00# ";
					}
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					hashtable5 = new Hashtable();
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						foreach (DataRow dataRow6 in dataTable.Rows)
						{
							int num6 = Convert.ToInt32(dataRow6[0]);
							if (!hashtable5.ContainsKey(num6))
							{
								hashtable5.Add(num6, num6);
							}
						}
					}
					dataTable = new DataTable();
				}
			}
			catch (Exception)
			{
				return null;
			}
			finally
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
			hashtable.Add("DEVICE_DAILY", hashtable2);
			hashtable.Add("DEVICE_HOURLY", hashtable3);
			hashtable.Add("BANK_DAILY", hashtable4);
			hashtable.Add("BANK_HOURLY", hashtable5);
			hashtable.Add("PORT_DAILY", hashtable6);
			hashtable.Add("PORT_HOURLY", hashtable7);
			return hashtable;
		}
		public static List<BillReportInfo> GetBillReportInfo(int nThreads, string str_Start, string str_End, string device_id, string portid)
		{
			List<BillReportInfo> list = new List<BillReportInfo>();
			int num = 0;
			int monthHour = DBTools.GetMonthHour(str_Start);
			if (monthHour < 0)
			{
				return list;
			}
			long num2 = 0L;
			long num3 = 0L;
			bool flag = false;
			if (portid != null && portid.Length > 0)
			{
				flag = true;
			}
			if (!DBUrl.SERVERMODE && !DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return list;
			}
			try
			{
				if (Sys_Para.GetBill_ratetype() == 1)
				{
					string text = Sys_Para.GetBill_2from1();
					text = text.Substring(0, 2);
					int bill_2duration = Sys_Para.GetBill_2duration1();
					num = DBTools.GetPeakMonthHour(str_Start, bill_2duration);
					string str = DBTools.gethourstring(text, bill_2duration);
					MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
					multiThreadQuery.StartTime = str_Start;
					multiThreadQuery.EndTime = str_End;
					multiThreadQuery.Select = "select sum(power_consumption) as kwh";
					multiThreadQuery.CalcType = "SUM";
					multiThreadQuery.MoreConditions = "date_format(insert_time, '%H') in " + str;
					if (flag)
					{
						multiThreadQuery.IsPort = true;
						multiThreadQuery.FilterList = portid;
						multiThreadQuery.TablePrefix = "port_data_hourly";
					}
					else
					{
						multiThreadQuery.IsPort = false;
						multiThreadQuery.FilterList = device_id;
						multiThreadQuery.TablePrefix = "device_data_hourly";
					}
					if (MultiThreadQuery._nMergeAfterThreadDone > 0)
					{
						ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
						if (arrayList == null || arrayList.Count <= 0)
						{
							goto IL_30A;
						}
						IEnumerator enumerator = arrayList.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DataTable dataTable = (DataTable)enumerator.Current;
								if (dataTable != null)
								{
									foreach (DataRow dataRow in dataTable.Rows)
									{
										if (dataRow["kwh"] != DBNull.Value)
										{
											num2 += Convert.ToInt64(dataRow["kwh"]);
										}
									}
								}
							}
							goto IL_30A;
						}
						finally
						{
							IDisposable disposable2 = enumerator as IDisposable;
							if (disposable2 != null)
							{
								disposable2.Dispose();
							}
						}
					}
					List<MultiThreadQuery.QueryContext> list2 = multiThreadQuery.PrepareQueryContext();
					List<MultiThreadQuery.QueryContext> list3 = new List<MultiThreadQuery.QueryContext>();
					for (int i = 0; i < list2.Count; i++)
					{
						MultiThreadQuery.QueryContext item = list2[i];
						list3.Add(item);
						if (list3.Count >= MultiThreadQuery._nThreadCount || i >= list2.Count - 1)
						{
							if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list3))
							{
								foreach (MultiThreadQuery.QueryContext current in list3)
								{
									DataTable resultInfo = current._resultInfo;
									if (resultInfo != null)
									{
										foreach (DataRow dataRow2 in resultInfo.Rows)
										{
											if (dataRow2["kwh"] != DBNull.Value)
											{
												num2 += Convert.ToInt64(dataRow2["kwh"]);
											}
										}
										current._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list3 = new List<MultiThreadQuery.QueryContext>();
						}
					}
					IL_30A:
					multiThreadQuery = new MultiThreadQuery();
					multiThreadQuery.StartTime = str_Start;
					multiThreadQuery.EndTime = str_End;
					multiThreadQuery.Select = "select sum(power_consumption) as kwh";
					multiThreadQuery.CalcType = "SUM";
					multiThreadQuery.MoreConditions = "date_format(insert_time, '%H')  not in " + str;
					if (flag)
					{
						multiThreadQuery.IsPort = true;
						multiThreadQuery.FilterList = portid;
						multiThreadQuery.TablePrefix = "port_data_hourly";
					}
					else
					{
						multiThreadQuery.IsPort = false;
						multiThreadQuery.FilterList = device_id;
						multiThreadQuery.TablePrefix = "device_data_hourly";
					}
					if (MultiThreadQuery._nMergeAfterThreadDone > 0)
					{
						ArrayList arrayList2 = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
						if (arrayList2 == null || arrayList2.Count <= 0)
						{
							goto IL_594;
						}
						IEnumerator enumerator5 = arrayList2.GetEnumerator();
						try
						{
							while (enumerator5.MoveNext())
							{
								DataTable dataTable2 = (DataTable)enumerator5.Current;
								if (dataTable2 != null)
								{
									foreach (DataRow dataRow3 in dataTable2.Rows)
									{
										if (dataRow3["kwh"] != DBNull.Value)
										{
											num3 += Convert.ToInt64(dataRow3["kwh"]);
										}
									}
								}
							}
							goto IL_594;
						}
						finally
						{
							IDisposable disposable5 = enumerator5 as IDisposable;
							if (disposable5 != null)
							{
								disposable5.Dispose();
							}
						}
					}
					List<MultiThreadQuery.QueryContext> list4 = multiThreadQuery.PrepareQueryContext();
					List<MultiThreadQuery.QueryContext> list5 = new List<MultiThreadQuery.QueryContext>();
					for (int j = 0; j < list4.Count; j++)
					{
						MultiThreadQuery.QueryContext item2 = list4[j];
						list5.Add(item2);
						if (list5.Count >= MultiThreadQuery._nThreadCount || j >= list4.Count - 1)
						{
							if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list5))
							{
								foreach (MultiThreadQuery.QueryContext current2 in list5)
								{
									DataTable resultInfo2 = current2._resultInfo;
									if (resultInfo2 != null)
									{
										foreach (DataRow dataRow4 in resultInfo2.Rows)
										{
											if (dataRow4["kwh"] != DBNull.Value)
											{
												num3 += Convert.ToInt64(dataRow4["kwh"]);
											}
										}
										current2._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list5 = new List<MultiThreadQuery.QueryContext>();
						}
					}
					IL_594:
					BillReportInfo item3 = new BillReportInfo(num2, num, "PEAK");
					list.Add(item3);
					BillReportInfo item4 = new BillReportInfo(num3, monthHour - num, "NON_PEAK");
					list.Add(item4);
				}
				else
				{
					MultiThreadQuery multiThreadQuery2 = new MultiThreadQuery();
					multiThreadQuery2.StartTime = str_Start;
					multiThreadQuery2.EndTime = str_End;
					multiThreadQuery2.Select = "select sum(power_consumption) as kwh";
					multiThreadQuery2.CalcType = "SUM";
					if (flag)
					{
						multiThreadQuery2.IsPort = true;
						multiThreadQuery2.FilterList = portid;
						multiThreadQuery2.TablePrefix = "port_data_hourly";
					}
					else
					{
						multiThreadQuery2.IsPort = false;
						multiThreadQuery2.FilterList = device_id;
						multiThreadQuery2.TablePrefix = "device_data_hourly";
					}
					if (MultiThreadQuery._nMergeAfterThreadDone > 0)
					{
						ArrayList arrayList3 = multiThreadQuery2.Execute(MultiThreadQuery._nThreadCount);
						if (arrayList3 == null || arrayList3.Count <= 0)
						{
							goto IL_838;
						}
						IEnumerator enumerator9 = arrayList3.GetEnumerator();
						try
						{
							while (enumerator9.MoveNext())
							{
								DataTable dataTable3 = (DataTable)enumerator9.Current;
								if (dataTable3 != null)
								{
									foreach (DataRow dataRow5 in dataTable3.Rows)
									{
										if (dataRow5["kwh"] != DBNull.Value)
										{
											num2 += Convert.ToInt64(dataRow5["kwh"]);
										}
									}
								}
							}
							goto IL_838;
						}
						finally
						{
							IDisposable disposable = enumerator9 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					List<MultiThreadQuery.QueryContext> list6 = multiThreadQuery2.PrepareQueryContext();
					List<MultiThreadQuery.QueryContext> list7 = new List<MultiThreadQuery.QueryContext>();
					for (int k = 0; k < list6.Count; k++)
					{
						MultiThreadQuery.QueryContext item5 = list6[k];
						list7.Add(item5);
						if (list7.Count >= MultiThreadQuery._nThreadCount || k >= list6.Count - 1)
						{
							if (multiThreadQuery2.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list7))
							{
								foreach (MultiThreadQuery.QueryContext current3 in list7)
								{
									DataTable resultInfo3 = current3._resultInfo;
									if (resultInfo3 != null)
									{
										foreach (DataRow dataRow6 in resultInfo3.Rows)
										{
											if (dataRow6["kwh"] != DBNull.Value)
											{
												num2 += Convert.ToInt64(dataRow6["kwh"]);
											}
										}
										current3._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list7 = new List<MultiThreadQuery.QueryContext>();
						}
					}
					IL_838:
					BillReportInfo item6 = new BillReportInfo(num2, monthHour, "ALL");
					list.Add(item6);
				}
			}
			catch (Exception ex)
			{
				string str2 = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetBillReportInfo: " + ex.Message + "\r\n" + str2, new string[0]);
			}
			return list;
		}
		public static DataTable GetThermalData(int charttype, string str_Start, string str_End, string rack_ids, string groupby, string TablePrefix)
		{
			try
			{
				MultiThreadQuery multiThreadQuery = new MultiThreadQuery();
				string text;
				switch (charttype)
				{
				case 1:
					text = "intakepeak";
					multiThreadQuery.Select = "select max(intakepeak) as intakepeak," + groupby + " as period";
					break;
				case 2:
					text = "exhaustpeak";
					multiThreadQuery.Select = "select max(exhaustpeak) as exhaustpeak," + groupby + " as period";
					break;
				default:
					text = "diffpeak";
					multiThreadQuery.Select = "select max(differencepeak) as diffpeak," + groupby + " as period";
					break;
				}
				multiThreadQuery.StartTime = str_Start;
				multiThreadQuery.EndTime = str_End;
				multiThreadQuery.GroupBy = groupby;
				multiThreadQuery.OrderBy = groupby + " ASC";
				multiThreadQuery.CalcColName = text;
				multiThreadQuery.CalcType = "MAX";
				multiThreadQuery.TablePrefix = TablePrefix;
				multiThreadQuery.FilterList = "";
				multiThreadQuery._MoreConditions = "rack_id in (" + rack_ids + ")";
				if (MultiThreadQuery._nMergeAfterThreadDone > 0)
				{
					multiThreadQuery._MergedType = "MAX";
					multiThreadQuery._MergedKeyColName = "period";
					multiThreadQuery._MergedKeyDateFormat = "";
					multiThreadQuery._MergedValueColName = text;
					multiThreadQuery._MergedResult = new Dictionary<string, double>();
					multiThreadQuery._ResultQueue = new ConcurrentQueue<DataTable>();
					ArrayList arrayList = multiThreadQuery.Execute(MultiThreadQuery._nThreadCount);
					if (arrayList != null && multiThreadQuery._MergedResult != null)
					{
						DataTable dataTable = new DataTable();
						dataTable.Columns.Add(multiThreadQuery._MergedValueColName, typeof(double));
						dataTable.Columns.Add(multiThreadQuery._MergedKeyColName, typeof(string));
						foreach (string current in multiThreadQuery._MergedResult.Keys)
						{
							dataTable.Rows.Add(new object[]
							{
								multiThreadQuery._MergedResult[current],
								current
							});
						}
						dataTable = new DataView(dataTable)
						{
							Sort = "period ASC"
						}.ToTable();
						DataTable result = dataTable;
						return result;
					}
				}
				else
				{
					List<MultiThreadQuery.QueryContext> list = multiThreadQuery.PrepareQueryContext();
					List<MultiThreadQuery.QueryContext> list2 = new List<MultiThreadQuery.QueryContext>();
					DataTable dataTable2 = null;
					Dictionary<string, double> dictionary = new Dictionary<string, double>();
					for (int i = 0; i < list.Count; i++)
					{
						MultiThreadQuery.QueryContext item = list[i];
						list2.Add(item);
						if (list2.Count >= MultiThreadQuery._nThreadCount || i >= list.Count - 1)
						{
							if (multiThreadQuery.ParallelQuery(Math.Max(1, MultiThreadQuery._nThreadCount), list2))
							{
								foreach (MultiThreadQuery.QueryContext current2 in list2)
								{
									DataTable resultInfo = current2._resultInfo;
									if (resultInfo != null)
									{
										foreach (DataRow arg_2BB_0 in resultInfo.Rows)
										{
											if (resultInfo != null)
											{
												if (dataTable2 == null)
												{
													dataTable2 = resultInfo.Clone();
												}
												foreach (DataRow dataRow in resultInfo.Rows)
												{
													string key = Convert.ToString(dataRow["period"]);
													double num = Convert.ToDouble(dataRow[text]);
													if (dictionary.ContainsKey(key))
													{
														if (num > dictionary[key])
														{
															dictionary[key] = num;
														}
													}
													else
													{
														dictionary.Add(key, num);
													}
												}
											}
										}
										current2._resultInfo = null;
									}
								}
							}
							GC.Collect();
							list2 = new List<MultiThreadQuery.QueryContext>();
						}
					}
					if (dataTable2 != null)
					{
						foreach (string current3 in dictionary.Keys)
						{
							dataTable2.Rows.Add(new object[]
							{
								dictionary[current3],
								current3
							});
						}
						dataTable2 = new DataView(dataTable2)
						{
							Sort = "period ASC"
						}.ToTable();
						DataTable result = dataTable2;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				MultiThreadQuery.WriteLog("GetThermalData: " + ex.Message + "\r\n" + str, new string[0]);
			}
			return new DataTable();
		}
	}
}
