using CommonAPI;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Threading;
namespace DBAccessAPI
{
	public class DBUrl
	{
		public const string MYSQL = "Mysql";
		public const string ACCESS = "Access";
		public const int DB_TYPE_CURRENT = 2;
		public const int DB_TYPE_DEFAULT = 1;
		public const int RUNMODE_SINGLE = 1;
		public const int RUNMODE_SERVER = 2;
		public const int RUNMODE_UNKNOW = 0;
		public static bool IsServer;
		private static DataSet DS_dbsource;
		private static string svrid;
		private static int runenv;
		private static string db_cur_type;
		private static string db_def_type;
		private static string db_cur_name;
		private static string db_def_name;
		private static string host_cur_path;
		private static string host_def_path;
		private static string user_cur_name;
		private static string user_def_name;
		private static string cur_pwd;
		private static string def_pwd;
		private static string cur_adapter;
		private static string def_adapter;
		private static string db_cur_class;
		private static string db_def_class;
		private static int active_flag;
		private static int cur_port;
		private static int def_port;
		private static int timeout;
		private static string db_source_name;
		private static string db_location;
		private static string connectstr;
		public static bool SERVERMODE
		{
			get
			{
				return DBUrl.runenv == 2;
			}
		}
		public static string CONNECT_STRING
		{
			get
			{
				return DBUrl.connectstr;
			}
			set
			{
				DBUrl.connectstr = value;
			}
		}
		public static string SERVERID
		{
			get
			{
				return DBUrl.svrid;
			}
			set
			{
				DBUrl.svrid = value;
			}
		}
		public static int RUNMODE
		{
			get
			{
				return DBUrl.runenv;
			}
			set
			{
				if (value <= 1)
				{
					DBUrl.runenv = 1;
					DBUrl.initconfig();
					if (DBUrl.db_cur_type.ToUpper().Equals("MYSQL"))
					{
						DBUrl.preparedatadb();
						return;
					}
				}
				else
				{
					DBUrl.runenv = value;
				}
			}
		}
		public static string DB_LOCATION
		{
			get
			{
				if (DBUrl.runenv == 2)
				{
					string text = DBUrl.connectstr;
					string[] array = text.Split(new string[]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					return DBTools.GetMySQLDataPath(DBUrl.svrid, array[0], Convert.ToInt32(array[1]), array[2], array[3]);
				}
				string empty = string.Empty;
				if (DBUrl.db_cur_type == null || DBUrl.db_cur_type.Length == 0)
				{
					DBUrl.initconfig();
				}
				if (!DBUrl.db_cur_type.Equals("ACCESS"))
				{
					return DBTools.GetMySQLDataPath(DBUrl.db_cur_name, DBUrl.host_cur_path, DBUrl.cur_port, DBUrl.user_cur_name, DBUrl.cur_pwd);
				}
				if (DBUrl.host_cur_path.LastIndexOf(Path.DirectorySeparatorChar) > -1)
				{
					return DBUrl.host_cur_path;
				}
				return AppDomain.CurrentDomain.BaseDirectory + DBUrl.host_cur_path;
			}
		}
		public static string DB_LOCATION2
		{
			get
			{
				string arg_05_0 = string.Empty;
				if (DBUrl.runenv != 1)
				{
					return "";
				}
				if (DBUrl.db_cur_type == null || DBUrl.db_cur_type.Length == 0)
				{
					DBUrl.initconfig();
				}
				if (DBUrl.host_def_path.LastIndexOf(Path.DirectorySeparatorChar) > -1)
				{
					return DBUrl.host_def_path;
				}
				return AppDomain.CurrentDomain.BaseDirectory + DBUrl.host_def_path;
			}
		}
		public static string DB_CURRENT_TYPE
		{
			get
			{
				if (DBUrl.runenv == 1)
				{
					if (DBUrl.db_cur_type == null || DBUrl.db_cur_type.Length == 0)
					{
						DBUrl.initconfig();
					}
					return DBUrl.db_cur_type;
				}
				return "MYSQL";
			}
		}
		public static string DB_DEFAULT_TYPE
		{
			get
			{
				if (DBUrl.runenv == 2)
				{
					return "MYSQL";
				}
				if (DBUrl.db_def_type == null || DBUrl.db_def_type.Length == 0)
				{
					DBUrl.initconfig();
				}
				return DBUrl.db_def_type;
			}
		}
		public static string DB_CURRENT_NAME
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.db_cur_name == null || DBUrl.db_cur_name.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.db_cur_name;
			}
		}
		public static string DB_DEFAULT_NAME
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.db_def_name == null || DBUrl.db_def_name.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.db_def_name;
			}
		}
		public static string CURRENT_HOST_PATH
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.host_cur_path == null || DBUrl.host_cur_path.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.host_cur_path;
			}
		}
		public static string DEFAULT_HOST_PATH
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.host_def_path == null || DBUrl.host_def_path.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.host_def_path;
			}
		}
		public static string CURRENT_USER_NAME
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.user_cur_name == null || DBUrl.user_cur_name.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.user_cur_name;
			}
		}
		public static string DEFAULT_USER_NAME
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.user_def_name == null || DBUrl.user_def_name.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.user_def_name;
			}
		}
		public static string CURRENT_PWD
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.cur_pwd == null || DBUrl.cur_pwd.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.cur_pwd;
			}
		}
		public static string DEFAULT_PWD
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.def_pwd == null || DBUrl.def_pwd.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.def_pwd;
			}
		}
		public static string CURRENT_ADAPTER
		{
			get
			{
				if (DBUrl.cur_adapter == null || DBUrl.cur_adapter.Length == 0)
				{
					DBUrl.initconfig();
				}
				return DBUrl.cur_adapter;
			}
		}
		public static string CURRENT_DB_CLASS
		{
			get
			{
				if (DBUrl.db_cur_class == null || DBUrl.db_cur_class.Length == 0)
				{
					DBUrl.initconfig();
				}
				return DBUrl.db_cur_class;
			}
		}
		public static int ACTIVE_FLAG
		{
			get
			{
				return DBUrl.active_flag;
			}
		}
		public static int CURRENT_PORT
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.db_cur_name == null || DBUrl.db_cur_name.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.cur_port;
			}
		}
		public static int DEFAULT_PORT
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.db_cur_name == null || DBUrl.db_cur_name.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.def_port;
			}
		}
		public static int CURRENT_TIMEOUT
		{
			get
			{
				if (DBUrl.runenv == 1 && (DBUrl.db_cur_name == null || DBUrl.db_cur_name.Length == 0))
				{
					DBUrl.initconfig();
				}
				return DBUrl.timeout;
			}
		}
		static DBUrl()
		{
			DBUrl.IsServer = false;
			DBUrl.DS_dbsource = new DataSet("datasource");
			DBUrl.svrid = "";
			DBUrl.db_cur_type = string.Empty;
			DBUrl.db_def_type = string.Empty;
			DBUrl.db_cur_name = string.Empty;
			DBUrl.db_def_name = string.Empty;
			DBUrl.host_cur_path = string.Empty;
			DBUrl.host_def_path = string.Empty;
			DBUrl.user_cur_name = string.Empty;
			DBUrl.user_def_name = string.Empty;
			DBUrl.cur_pwd = string.Empty;
			DBUrl.def_pwd = string.Empty;
			DBUrl.cur_adapter = string.Empty;
			DBUrl.def_adapter = string.Empty;
			DBUrl.db_cur_class = string.Empty;
			DBUrl.db_def_class = string.Empty;
			DBUrl.active_flag = 0;
			DBUrl.db_source_name = string.Empty;
			DBUrl.db_location = string.Empty;
			DBUrl.connectstr = "127.0.0.1,3306,root,password";
			DBUrl.runenv = 1;
		}
		private static void preparedatadb()
		{
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				DbCommand dbCommand = null;
				DbDataReader dbDataReader = null;
				DBConn dBConn = new DBConn();
				try
				{
					dBConn.con = new MySqlConnection(string.Concat(new object[]
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
					dBConn.con.Open();
					dBConn.setInUse();
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where (table_name like '%_auto_info%' or table_name like '%_data_daily%' or table_name like '%_data_hourly%' or table_name like 'rackthermal_hourly20%' or table_name like 'rackthermal_daily20%') and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
					dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						string text = Convert.ToString(dbDataReader.GetValue(0));
						if (!DBTools.ht_tablename.ContainsKey(text))
						{
							DBTools.ht_tablename.Add(text, text);
						}
					}
					dbDataReader.Close();
					dbCommand.Dispose();
					dBConn.Close();
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex.Message + "\n" + ex.StackTrace);
					try
					{
						dbDataReader.Close();
					}
					catch (Exception)
					{
					}
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
		}
		public static int CheckDatabase4Service(string str_dbip, string str_dbport, string str_dbuser, string str_dbpwd, string str_serid)
		{
			int result = -1;
			int i = 0;
			while (i < 9)
			{
				i++;
				try
				{
					DbConnection dbConnection = new MySqlConnection(string.Concat(new string[]
					{
						"Database=eco;Data Source=127.0.0.1;Port=",
						str_dbport,
						";User Id=",
						str_dbuser,
						";Password=",
						str_dbpwd,
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
					dbConnection.Open();
					dbConnection.Close();
					return 1;
				}
				catch (Exception)
				{
					Thread.Sleep(10000);
				}
			}
			return result;
		}
		public static int CheckDatabase(string dbname, string str_dbip, string str_dbport, string str_dbuser, string str_dbpwd, string str_serid)
		{
			int result = -1;
			if (DBUrl.SERVERMODE)
			{
				try
				{
					DbConnection dbConnection = new MySqlConnection(string.Concat(new string[]
					{
						"Database=",
						dbname,
						";Data Source=",
						str_dbip,
						";Port=",
						str_dbport,
						";User Id=",
						str_dbuser,
						";Password=",
						str_dbpwd,
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
					dbConnection.Open();
					dbConnection.Close();
					int result2 = 1;
					return result2;
				}
				catch (Exception ex)
				{
					string message = ex.Message;
					if (message.IndexOf("Unable to connect to any of the specified MySQL hosts") > -1)
					{
						result = -1;
					}
					if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Access denied for user") > -1)
					{
						result = -2;
					}
					if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Unknown database") > -1)
					{
						result = -3;
					}
					return result;
				}
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				try
				{
					DbConnection dbConnection = new MySqlConnection(string.Concat(new string[]
					{
						"Database=",
						dbname,
						";Data Source=",
						str_dbip,
						";Port=",
						str_dbport,
						";User Id=",
						str_dbuser,
						";Password=",
						str_dbpwd,
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
					dbConnection.Open();
					dbConnection.Close();
					int result2 = 1;
					return result2;
				}
				catch (Exception ex2)
				{
					string message2 = ex2.Message;
					if (message2.IndexOf("Unable to connect to any of the specified MySQL hosts") > -1)
					{
						result = -1;
					}
					if (message2.IndexOf("Authentication to host") > -1 && message2.IndexOf("Access denied for user") > -1)
					{
						result = -2;
					}
					if (message2.IndexOf("Authentication to host") > -1 && message2.IndexOf("Unknown database") > -1)
					{
						result = -3;
					}
				}
			}
			return result;
		}
		public static void initconfig(string dbhost, string dbport, string dbusr, string dbpwd, string serverid)
		{
			DBUrl.runenv = 2;
			DBUrl.SERVERID = serverid;
			DBUrl.connectstr = string.Concat(new string[]
			{
				dbhost,
				",",
				dbport,
				",",
				dbusr,
				",",
				dbpwd
			});
			DBUrl.db_cur_type = "MYSQL";
			DBUrl.db_cur_name = "eco" + serverid;
			DBUrl.host_cur_path = dbhost;
			DBUrl.user_cur_name = dbusr;
			DBUrl.cur_pwd = dbpwd;
			DBUrl.cur_adapter = "MYSQLOLEDB";
			DBUrl.db_cur_class = "MYSQLOLEDB";
			DBUrl.active_flag = 2;
			DBUrl.cur_port = Convert.ToInt32(dbport);
			DBUrl.timeout = 30;
			DBUrl.db_def_type = "MYSQL";
			DBUrl.db_def_name = "eco" + serverid;
			DBUrl.host_def_path = dbhost;
			DBUrl.user_def_name = dbusr;
			DBUrl.def_pwd = dbpwd;
			DBUrl.def_adapter = "MYSQLOLEDB";
			DBUrl.db_def_class = "MYSQLOLEDB";
			DBUrl.active_flag = 2;
			DBUrl.def_port = Convert.ToInt32(dbport);
			DBUrl.timeout = 30;
		}
		public static void initconfig(string str_dbfile)
		{
			if (DBUrl.runenv == 2)
			{
				string cONNECT_STRING = DBUrl.CONNECT_STRING;
				string[] array = cONNECT_STRING.Split(new string[]
				{
					","
				}, StringSplitOptions.RemoveEmptyEntries);
				DBUrl.db_cur_type = "MYSQL";
				DBUrl.db_cur_name = "eco";
				DBUrl.host_cur_path = array[0];
				DBUrl.user_cur_name = array[2];
				DBUrl.cur_pwd = array[3];
				DBUrl.cur_adapter = "MYSQLOLEDB";
				DBUrl.db_cur_class = "MYSQLOLEDB";
				DBUrl.active_flag = 2;
				DBUrl.cur_port = Convert.ToInt32(array[1]);
				DBUrl.timeout = 30;
				DBUrl.db_def_type = "MYSQL";
				DBUrl.db_def_name = "eco";
				DBUrl.host_def_path = array[0];
				DBUrl.user_def_name = array[2];
				DBUrl.def_pwd = array[3];
				DBUrl.def_adapter = "MYSQLOLEDB";
				DBUrl.db_def_class = "MYSQLOLEDB";
				DBUrl.active_flag = 2;
				DBUrl.def_port = Convert.ToInt32(array[1]);
				DBUrl.timeout = 30;
				return;
			}
			FileInfo fileInfo = new FileInfo(str_dbfile);
			if (!fileInfo.Exists)
			{
				return;
			}
			string connectionString = string.Concat(new object[]
			{
				"Driver={Microsoft Access Driver (*.mdb)};Dbq=",
				fileInfo.DirectoryName,
				Path.DirectorySeparatorChar,
				"sysdb.mdb;Uid=Admin;Pwd=^tenec0Sensor;"
			});
			using (OdbcConnection odbcConnection = new OdbcConnection(connectionString))
			{
				odbcConnection.Open();
				OdbcCommand odbcCommand = new OdbcCommand();
				odbcCommand.Connection = odbcConnection;
				odbcCommand.CommandText = "select * from dbsource where active_flag = 2 ";
				DbDataReader dbDataReader = odbcCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					DBUrl.db_cur_type = Convert.ToString(dbDataReader.GetValue(2));
					DBUrl.db_cur_name = Convert.ToString(dbDataReader.GetValue(3));
					if (DBUrl.db_cur_type.ToUpper().Equals("MYSQL"))
					{
						DBUrl.host_cur_path = Convert.ToString(dbDataReader.GetValue(4));
					}
					else
					{
						if (Convert.ToString(dbDataReader.GetValue(4)).IndexOf(Path.DirectorySeparatorChar) <= 0)
						{
							DBUrl.host_cur_path = fileInfo.DirectoryName + Path.DirectorySeparatorChar + Convert.ToString(dbDataReader.GetValue(4));
						}
						else
						{
							DBUrl.host_cur_path = Convert.ToString(dbDataReader.GetValue(4));
						}
					}
					DBUrl.user_cur_name = Convert.ToString(dbDataReader.GetValue(6));
					DBUrl.cur_pwd = Convert.ToString(dbDataReader.GetValue(7));
					DBUrl.cur_adapter = Convert.ToString(dbDataReader.GetValue(8));
					DBUrl.db_cur_class = Convert.ToString(dbDataReader.GetValue(9));
					DBUrl.active_flag = Convert.ToInt32(dbDataReader.GetValue(1));
					DBUrl.cur_port = Convert.ToInt32(dbDataReader.GetValue(5));
					DBUrl.timeout = Convert.ToInt32(dbDataReader.GetValue(10));
				}
				dbDataReader.Close();
				dbDataReader.Dispose();
				odbcCommand.CommandText = "select * from dbsource where active_flag = 1 ";
				DbDataReader dbDataReader2 = odbcCommand.ExecuteReader();
				if (dbDataReader2.Read())
				{
					DBUrl.db_def_type = Convert.ToString(dbDataReader2.GetValue(2));
					DBUrl.db_def_name = Convert.ToString(dbDataReader2.GetValue(3));
					DBUrl.host_def_path = fileInfo.DirectoryName + Path.DirectorySeparatorChar + Convert.ToString(dbDataReader2.GetValue(4));
					DBUrl.user_def_name = Convert.ToString(dbDataReader2.GetValue(6));
					DBUrl.def_pwd = Convert.ToString(dbDataReader2.GetValue(7));
					DBUrl.def_adapter = Convert.ToString(dbDataReader2.GetValue(8));
					DBUrl.db_def_class = Convert.ToString(dbDataReader2.GetValue(9));
					DBUrl.active_flag = Convert.ToInt32(dbDataReader2.GetValue(1));
					DBUrl.def_port = Convert.ToInt32(dbDataReader2.GetValue(5));
					DBUrl.timeout = Convert.ToInt32(dbDataReader2.GetValue(10));
				}
				dbDataReader2.Close();
				dbDataReader2.Dispose();
				odbcCommand.Dispose();
			}
		}
		public static void initconfig()
		{
			RegistryKey arg_05_0 = Registry.CurrentUser;
			string connectionString = "Driver={Microsoft Access Driver (*.mdb)};Dbq=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Uid=Admin;Pwd=^tenec0Sensor;";
			using (OdbcConnection odbcConnection = new OdbcConnection(connectionString))
			{
				odbcConnection.Open();
				OdbcCommand odbcCommand = new OdbcCommand();
				odbcCommand.Connection = odbcConnection;
				odbcCommand.CommandText = "select * from dbsource where active_flag = 2 ";
				DbDataReader dbDataReader = odbcCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					DBUrl.db_cur_type = Convert.ToString(dbDataReader.GetValue(2));
					DBUrl.db_cur_name = Convert.ToString(dbDataReader.GetValue(3));
					if (DBUrl.db_cur_type.ToUpper().Equals("MYSQL"))
					{
						DBUrl.host_cur_path = Convert.ToString(dbDataReader.GetValue(4));
					}
					else
					{
						if (Convert.ToString(dbDataReader.GetValue(4)).IndexOf(Path.DirectorySeparatorChar) <= 0)
						{
							DBUrl.host_cur_path = AppDomain.CurrentDomain.BaseDirectory + Convert.ToString(dbDataReader.GetValue(4));
						}
						else
						{
							DBUrl.host_cur_path = Convert.ToString(dbDataReader.GetValue(4));
						}
					}
					DBUrl.user_cur_name = Convert.ToString(dbDataReader.GetValue(6));
					DBUrl.cur_pwd = Convert.ToString(dbDataReader.GetValue(7));
					DBUrl.cur_adapter = Convert.ToString(dbDataReader.GetValue(8));
					DBUrl.db_cur_class = Convert.ToString(dbDataReader.GetValue(9));
					DBUrl.active_flag = Convert.ToInt32(dbDataReader.GetValue(1));
					DBUrl.cur_port = Convert.ToInt32(dbDataReader.GetValue(5));
					DBUrl.timeout = Convert.ToInt32(dbDataReader.GetValue(10));
				}
				dbDataReader.Close();
				dbDataReader.Dispose();
				odbcCommand.CommandText = "select * from dbsource where active_flag = 1 ";
				DbDataReader dbDataReader2 = odbcCommand.ExecuteReader();
				if (dbDataReader2.Read())
				{
					DBUrl.db_def_type = Convert.ToString(dbDataReader2.GetValue(2));
					DBUrl.db_def_name = Convert.ToString(dbDataReader2.GetValue(3));
					DBUrl.host_def_path = AppDomain.CurrentDomain.BaseDirectory + Convert.ToString(dbDataReader2.GetValue(4));
					DBUrl.user_def_name = Convert.ToString(dbDataReader2.GetValue(6));
					DBUrl.def_pwd = Convert.ToString(dbDataReader2.GetValue(7));
					DBUrl.def_adapter = Convert.ToString(dbDataReader2.GetValue(8));
					DBUrl.db_def_class = Convert.ToString(dbDataReader2.GetValue(9));
					DBUrl.active_flag = Convert.ToInt32(dbDataReader2.GetValue(1));
					DBUrl.def_port = Convert.ToInt32(dbDataReader2.GetValue(5));
					DBUrl.timeout = Convert.ToInt32(dbDataReader2.GetValue(10));
				}
				dbDataReader2.Close();
				dbDataReader2.Dispose();
				odbcCommand.Dispose();
			}
		}
		public static int updatesetting(int i_port, string str_usr, string str_pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = new OleDbCommand();
			int result;
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=",
					DBUrl.DB_CURRENT_NAME,
					";Data Source=127.0.0.1;Port=",
					i_port,
					";User Id=",
					str_usr,
					";Password=",
					str_pwd,
					";Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				dbCommand = new MySqlCommand();
				dbCommand.Connection = dbConnection;
				dbCommand.CommandText = "select * from device_auto_info";
				dbCommand.ExecuteScalar();
				string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor";
				OleDbCommand oleDbCommand = new OleDbCommand();
				using (OleDbConnection oleDbConnection = new OleDbConnection(connectionString))
				{
					try
					{
						oleDbConnection.Open();
						oleDbCommand.Connection = oleDbConnection;
						oleDbCommand.CommandType = CommandType.Text;
						oleDbCommand.CommandText = string.Concat(new object[]
						{
							"update dbsource set db_type='MYSQL',db_name='eco',host_path='127.0.0.1',port= ",
							i_port,
							",user_name = '",
							str_usr,
							"',pwd='",
							str_pwd,
							"' where active_flag = 2 "
						});
						int num = oleDbCommand.ExecuteNonQuery();
						if (num < 0)
						{
							result = -5;
							return result;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
						result = -5;
						return result;
					}
					finally
					{
						oleDbCommand.Dispose();
					}
				}
				DBConnPool.DisconnectDatabase();
				DBUrl.initconfig();
				result = DebugCenter.ST_Success;
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
				result = -5;
			}
			finally
			{
				dbCommand.Dispose();
				if (dbConnection != null)
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static int updatesetting4newdb(int i_port, string str_usr, string str_pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = new OleDbCommand();
			int result;
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=",
					DBUrl.DB_CURRENT_NAME,
					";Data Source=127.0.0.1;Port=",
					i_port,
					";User Id=",
					str_usr,
					";Password=",
					str_pwd,
					";Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				dbCommand = new MySqlCommand();
				dbCommand.Connection = dbConnection;
				string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor";
				OleDbCommand oleDbCommand = new OleDbCommand();
				using (OleDbConnection oleDbConnection = new OleDbConnection(connectionString))
				{
					try
					{
						oleDbConnection.Open();
						oleDbCommand.Connection = oleDbConnection;
						oleDbCommand.CommandType = CommandType.Text;
						oleDbCommand.CommandText = string.Concat(new object[]
						{
							"update dbsource set db_type='MYSQL',db_name='eco',host_path='127.0.0.1',port= ",
							i_port,
							",user_name = '",
							str_usr,
							"',pwd='",
							str_pwd,
							"' where active_flag = 2 "
						});
						int num = oleDbCommand.ExecuteNonQuery();
						if (num < 0)
						{
							result = -5;
							return result;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
						result = -5;
						return result;
					}
					finally
					{
						oleDbCommand.Dispose();
					}
				}
				DBConnPool.DisconnectDatabase();
				DBUrl.initconfig();
				result = 100;
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
				result = -5;
			}
			finally
			{
				dbCommand.Dispose();
				if (dbConnection != null)
				{
					dbConnection.Close();
				}
			}
			return result;
		}
	}
}
