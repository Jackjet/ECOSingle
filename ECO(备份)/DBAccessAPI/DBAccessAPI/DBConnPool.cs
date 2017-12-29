using CommonAPI;
using MySql.Data.MySqlClient;
using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
namespace DBAccessAPI
{
	public class DBConnPool
	{
		private static DBConn[] dbConns;
		public static int itestcount = 0;
		private static int nConnCount = 30;
		private static object thisLock = new object();
		private static DBConn[] dynaConns;
		private static int nCount = 30;
		private static object curLock = new object();
		private static object _locksysdb = new object();
		private static object _lockdatadb = new object();
		private static object _lockthermaldb = new object();
		public static int CloseAllConnection()
		{
			if (DBUrl.RUNMODE == 2)
			{
				DBConn[] array = DBConnPool.dynaConns;
				for (int i = 0; i < array.Length; i++)
				{
					DBConn dBConn = array[i];
					if (dBConn.con != null)
					{
						try
						{
							dBConn.close();
							dBConn.con.Close();
							dBConn.con = null;
						}
						catch (Exception ex)
						{
							DebugCenter.GetInstance().appendToFile("Close DBConnection exception : " + ex.Message + "\n" + ex.StackTrace);
						}
					}
				}
				DBConn[] array2 = DBConnPool.dbConns;
				for (int j = 0; j < array2.Length; j++)
				{
					DBConn dBConn2 = array2[j];
					if (dBConn2.con != null)
					{
						try
						{
							dBConn2.close();
							dBConn2.con.Close();
							dBConn2.con = null;
						}
						catch (Exception ex2)
						{
							DebugCenter.GetInstance().appendToFile("Close DBConnection exception : " + ex2.Message + "\n" + ex2.StackTrace);
						}
					}
				}
				return 1;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return 1;
			}
			DBConn[] array3 = DBConnPool.dynaConns;
			for (int k = 0; k < array3.Length; k++)
			{
				DBConn dBConn3 = array3[k];
				if (dBConn3.con != null)
				{
					try
					{
						dBConn3.close();
						dBConn3.con.Close();
						dBConn3.con = null;
					}
					catch (Exception ex3)
					{
						DebugCenter.GetInstance().appendToFile("Close DBConnection exception : " + ex3.Message + "\n" + ex3.StackTrace);
					}
				}
			}
			return 1;
		}
		public static void DisconnectDatabase()
		{
		}
		public static DBConn getDynaConnection(DateTime dt_inserttime, bool b_create)
		{
			DBConn result;
			lock (DBConnPool._lockdatadb)
			{
				string str = "";
				if (DBUrl.SERVERMODE)
				{
					try
					{
						DBConn dBConn = new DBConn();
						string cONNECT_STRING = DBUrl.CONNECT_STRING;
						string[] array = cONNECT_STRING.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						dBConn.con = new MySqlConnection(string.Concat(new string[]
						{
							"Database=eco",
							DBUrl.SERVERID,
							";Data Source=",
							array[0],
							";Port=",
							array[1],
							";User Id=",
							array[2],
							";Password=",
							array[3],
							";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
						}));
						dBConn.con.Open();
						dBConn.setInUse();
						DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						result = dBConn;
						return result;
					}
					catch (Exception e)
					{
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e, false, "    "));
						result = null;
						return result;
					}
				}
				if (DBUrl.DB_CURRENT_TYPE.ToUpperInvariant().Equals("MYSQL"))
				{
					try
					{
						DBConn dBConn2 = new DBConn();
						dBConn2.con = new MySqlConnection(string.Concat(new object[]
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
						dBConn2.con.Open();
						dBConn2.setInUse();
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_Success, false);
						result = dBConn2;
						return result;
					}
					catch (Exception e2)
					{
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e2, false, "    "));
						result = null;
						return result;
					}
				}
				try
				{
					string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text[text.Length - 1] != Path.DirectorySeparatorChar)
					{
						text += Path.DirectorySeparatorChar;
					}
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					DateTime.Now.ToString("yyyyMMdd");
					dt_inserttime.ToString("yyyyMMdd");
					string text2 = text + "datadb_" + dt_inserttime.ToString("yyyyMMdd") + ".mdb";
					if (!File.Exists(text2) && b_create)
					{
						string sourceFileName = text + "datadb.org";
						File.Copy(sourceFileName, text2, true);
					}
					bool flag2 = false;
					try
					{
						DBConn dBConn3 = new DBConn();
						try
						{
							dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
							dBConn3.con.Open();
							flag2 = true;
						}
						catch (Exception ex)
						{
							str = ex.Message + "\r\n" + ex.StackTrace;
							int i = 0;
							while (i < 4)
							{
								Thread.Sleep(10);
								i++;
								try
								{
									dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
									dBConn3.con.Open();
									flag2 = true;
									break;
								}
								catch (Exception ex2)
								{
									str = ex2.Message + "\r\n" + ex2.StackTrace;
								}
							}
						}
						if (flag2)
						{
							dBConn3.setInUse();
							dBConn3.DBSource_Type = 2;
							try
							{
								StackTrace stackTrace = new StackTrace();
								MethodBase method = stackTrace.GetFrame(1).GetMethod();
								DBCache.OpenDataDB(dBConn3.GetHashCode(), method.Name);
								DBCache.PrintDataDB();
							}
							catch
							{
							}
							result = dBConn3;
						}
						else
						{
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + str);
							result = null;
						}
					}
					catch (Exception ex3)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex3.Message + "\n" + ex3.StackTrace);
						result = null;
					}
				}
				catch (Exception ex4)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex4.Message + "\n" + ex4.StackTrace);
					result = null;
				}
			}
			return result;
		}
		public static DBConn getDynaConnection(DateTime dt_inserttime)
		{
			DBConn result;
			lock (DBConnPool._lockdatadb)
			{
				string str = "";
				if (DBUrl.SERVERMODE)
				{
					try
					{
						DBConn dBConn = new DBConn();
						string cONNECT_STRING = DBUrl.CONNECT_STRING;
						string[] array = cONNECT_STRING.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						dBConn.con = new MySqlConnection(string.Concat(new string[]
						{
							"Database=eco",
							DBUrl.SERVERID,
							";Data Source=",
							array[0],
							";Port=",
							array[1],
							";User Id=",
							array[2],
							";Password=",
							array[3],
							";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
						}));
						dBConn.con.Open();
						dBConn.setInUse();
						DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						result = dBConn;
						return result;
					}
					catch (Exception e)
					{
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e, false, "    "));
						result = null;
						return result;
					}
				}
				if (DBUrl.DB_CURRENT_TYPE.ToUpperInvariant().Equals("MYSQL"))
				{
					try
					{
						DBConn dBConn2 = new DBConn();
						dBConn2.con = new MySqlConnection(string.Concat(new object[]
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
						dBConn2.con.Open();
						dBConn2.setInUse();
						DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						result = dBConn2;
						return result;
					}
					catch (Exception e2)
					{
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e2, false, "    "));
						result = null;
						return result;
					}
				}
				try
				{
					string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text[text.Length - 1] != Path.DirectorySeparatorChar)
					{
						text += Path.DirectorySeparatorChar;
					}
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					string text2 = DateTime.Now.ToString("yyyyMMdd");
					string value = dt_inserttime.ToString("yyyyMMdd");
					string text3 = text + "datadb_" + dt_inserttime.ToString("yyyyMMdd") + ".mdb";
					if (!File.Exists(text3))
					{
						if (!text2.Equals(value))
						{
							result = null;
							return result;
						}
						string sourceFileName = text + "datadb.org";
						File.Copy(sourceFileName, text3, true);
					}
					bool flag2 = false;
					try
					{
						DBConn dBConn3 = new DBConn();
						try
						{
							dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text3 + ";Jet OLEDB:Database Password=root");
							dBConn3.con.Open();
							flag2 = true;
						}
						catch (Exception ex)
						{
							str = ex.Message + "\r\n" + ex.StackTrace;
							int i = 0;
							while (i < 4)
							{
								Thread.Sleep(10);
								i++;
								try
								{
									dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text3 + ";Jet OLEDB:Database Password=root");
									dBConn3.con.Open();
									flag2 = true;
									break;
								}
								catch (Exception ex2)
								{
									str = ex2.Message + "\r\n" + ex2.StackTrace;
								}
							}
						}
						if (flag2)
						{
							dBConn3.setInUse();
							dBConn3.DBSource_Type = 2;
							try
							{
								StackTrace stackTrace = new StackTrace();
								MethodBase method = stackTrace.GetFrame(1).GetMethod();
								DBCache.OpenDataDB(dBConn3.GetHashCode(), method.Name);
								DBCache.PrintDataDB();
							}
							catch
							{
							}
							result = dBConn3;
						}
						else
						{
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + str);
							result = null;
						}
					}
					catch (Exception ex3)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex3.Message + "\n" + ex3.StackTrace);
						result = null;
					}
				}
				catch (Exception ex4)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex4.Message + "\n" + ex4.StackTrace);
					result = null;
				}
			}
			return result;
		}
		public static DBConn getDynaConnection()
		{
			DBConn result;
			lock (DBConnPool._lockdatadb)
			{
				string str = "";
				if (DBUrl.SERVERMODE)
				{
					try
					{
						DBConn dBConn = new DBConn();
						string cONNECT_STRING = DBUrl.CONNECT_STRING;
						string[] array = cONNECT_STRING.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						dBConn.con = new MySqlConnection(string.Concat(new string[]
						{
							"Database=eco",
							DBUrl.SERVERID,
							";Data Source=",
							array[0],
							";Port=",
							array[1],
							";User Id=",
							array[2],
							";Password=",
							array[3],
							";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
						}));
						dBConn.con.Open();
						dBConn.setInUse();
						result = dBConn;
						return result;
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex.Message + "\n" + ex.StackTrace);
						result = null;
						return result;
					}
				}
				if (DBUrl.DB_CURRENT_TYPE.ToUpperInvariant().Equals("MYSQL"))
				{
					try
					{
						DBConn dBConn2 = new DBConn();
						dBConn2.con = new MySqlConnection(string.Concat(new object[]
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
						dBConn2.con.Open();
						dBConn2.setInUse();
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_Success, false);
						result = dBConn2;
						return result;
					}
					catch (Exception e)
					{
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e, false, "    "));
						result = null;
						return result;
					}
				}
				try
				{
					string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text[text.Length - 1] != Path.DirectorySeparatorChar)
					{
						text += Path.DirectorySeparatorChar;
					}
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					string text2 = text + "datadb_" + DateTime.Now.ToString("yyyyMMdd") + ".mdb";
					if (!File.Exists(text2))
					{
						string sourceFileName = text + "datadb.org";
						File.Copy(sourceFileName, text2, true);
					}
					bool flag2 = false;
					try
					{
						DBConn dBConn3 = new DBConn();
						try
						{
							dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
							dBConn3.con.Open();
							flag2 = true;
						}
						catch (Exception ex2)
						{
							str = ex2.Message + "\r\n" + ex2.StackTrace;
							int i = 0;
							while (i < 4)
							{
								Thread.Sleep(10);
								i++;
								try
								{
									dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
									dBConn3.con.Open();
									flag2 = true;
									break;
								}
								catch (Exception ex3)
								{
									str = ex3.Message + "\r\n" + ex3.StackTrace;
								}
							}
						}
						if (flag2)
						{
							dBConn3.setInUse();
							dBConn3.DBSource_Type = 2;
							try
							{
								StackTrace stackTrace = new StackTrace();
								MethodBase method = stackTrace.GetFrame(1).GetMethod();
								DBCache.OpenDataDB(dBConn3.GetHashCode(), method.Name);
								DBCache.PrintDataDB();
							}
							catch
							{
							}
							result = dBConn3;
						}
						else
						{
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + str);
							result = null;
						}
					}
					catch (Exception ex4)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex4.Message + "\n" + ex4.StackTrace);
						result = null;
					}
				}
				catch (Exception ex5)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get DynaDB Connection Error : " + ex5.Message + "\n" + ex5.StackTrace);
					result = null;
				}
			}
			return result;
		}
		public static DBConn getConnection()
		{
			DBConn result;
			lock (DBConnPool._locksysdb)
			{
				string str = "";
				if (DBUrl.SERVERMODE)
				{
					try
					{
						DBConn dBConn = new DBConn();
						string cONNECT_STRING = DBUrl.CONNECT_STRING;
						string[] array = cONNECT_STRING.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						dBConn.con = new MySqlConnection(string.Concat(new string[]
						{
							"Database=eco",
							DBUrl.SERVERID,
							";Data Source=",
							array[0],
							";Port=",
							array[1],
							";User Id=",
							array[2],
							";Password=",
							array[3],
							";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
						}));
						dBConn.con.Open();
						dBConn.setInUse();
						result = dBConn;
						return result;
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get SysDB Connection Error : " + ex.Message + "\n" + ex.StackTrace);
						result = null;
						return result;
					}
				}
				bool flag2 = false;
				try
				{
					DBConn dBConn2 = new DBConn();
					try
					{
						dBConn2.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor");
						dBConn2.con.Open();
						flag2 = true;
					}
					catch (Exception ex2)
					{
						str = ex2.Message + "\r\n" + ex2.StackTrace;
						int i = 0;
						while (i < 4)
						{
							Thread.Sleep(10);
							i++;
							try
							{
								dBConn2.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor");
								dBConn2.con.Open();
								flag2 = true;
								break;
							}
							catch (Exception ex3)
							{
								str = ex3.Message + "\r\n" + ex3.StackTrace;
							}
						}
					}
					if (flag2)
					{
						dBConn2.setInUse();
						dBConn2.DBSource_Type = 1;
						try
						{
							StackTrace stackTrace = new StackTrace();
							MethodBase method = stackTrace.GetFrame(1).GetMethod();
							DBCache.OpenSysDB(dBConn2.GetHashCode(), method.Name);
							DBCache.PrintSysDB();
						}
						catch
						{
						}
						result = dBConn2;
					}
					else
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get SysDB Connection Error : " + str);
						DBCache.PrintSysDB();
						result = null;
					}
				}
				catch (Exception ex4)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get SysDB Connection Error : " + ex4.Message + "\n" + ex4.StackTrace);
					DBCache.PrintSysDB();
					result = null;
				}
			}
			return result;
		}
		public static DBConn getLogConnection()
		{
			string str = "";
			DBConn result;
			if (DBUrl.SERVERMODE)
			{
				try
				{
					DBConn dBConn = new DBConn();
					string cONNECT_STRING = DBUrl.CONNECT_STRING;
					string[] array = cONNECT_STRING.Split(new string[]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					dBConn.con = new MySqlConnection(string.Concat(new string[]
					{
						"Database=eco",
						DBUrl.SERVERID,
						";Data Source=",
						array[0],
						";Port=",
						array[1],
						";User Id=",
						array[2],
						";Password=",
						array[3],
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
					dBConn.con.Open();
					dBConn.setInUse();
					result = dBConn;
					return result;
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get LogDB Connection Error : " + ex.Message + "\n" + ex.StackTrace);
					result = null;
					return result;
				}
			}
			bool flag = false;
			DBConn dBConn2 = new DBConn();
			try
			{
				try
				{
					dBConn2.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb;Jet OLEDB:Database Password=ecoSensorlog");
					dBConn2.con.Open();
					flag = true;
				}
				catch (Exception ex2)
				{
					str = ex2.Message + "\r\n" + ex2.StackTrace;
					int i = 0;
					while (i < 4)
					{
						Thread.Sleep(10);
						i++;
						try
						{
							dBConn2.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb;Jet OLEDB:Database Password=ecoSensorlog");
							dBConn2.con.Open();
							flag = true;
							break;
						}
						catch (Exception ex3)
						{
							str = ex3.Message + "\r\n" + ex3.StackTrace;
						}
					}
				}
				if (flag)
				{
					dBConn2.setInUse();
					result = dBConn2;
				}
				else
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get LogDB Connection Error : " + str);
					result = null;
				}
			}
			catch (Exception ex4)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get LogDB Connection Error : " + ex4.Message + "\n" + ex4.StackTrace);
				result = null;
			}
			return result;
		}
		public static DBConn getThermalConnection()
		{
			DBConn result;
			lock (DBConnPool._lockthermaldb)
			{
				string str = "";
				if (DBUrl.SERVERMODE)
				{
					try
					{
						DBConn dBConn = new DBConn();
						string cONNECT_STRING = DBUrl.CONNECT_STRING;
						string[] array = cONNECT_STRING.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						dBConn.con = new MySqlConnection(string.Concat(new string[]
						{
							"Database=eco",
							DBUrl.SERVERID,
							";Data Source=",
							array[0],
							";Port=",
							array[1],
							";User Id=",
							array[2],
							";Password=",
							array[3],
							";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
						}));
						dBConn.con.Open();
						dBConn.setInUse();
						result = dBConn;
						return result;
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get ThermalDB Connection Error : " + ex.Message + "\n" + ex.StackTrace);
						result = null;
						return result;
					}
				}
				if (DBUrl.DB_CURRENT_TYPE.ToUpperInvariant().Equals("MYSQL"))
				{
					try
					{
						DBConn dBConn2 = new DBConn();
						dBConn2.con = new MySqlConnection(string.Concat(new object[]
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
						dBConn2.con.Open();
						dBConn2.setInUse();
						DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						result = dBConn2;
						return result;
					}
					catch (Exception e)
					{
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e, false, "    "));
						result = null;
						return result;
					}
				}
				bool flag2 = false;
				try
				{
					DBCache.PrintThermalDB();
					string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text[text.Length - 1] != Path.DirectorySeparatorChar)
					{
						text += Path.DirectorySeparatorChar;
					}
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					string text2 = text + "thermaldb.mdb";
					if (!File.Exists(text2))
					{
						if (File.Exists(text + "compacted_thermaldb.mdb"))
						{
							try
							{
								File.Copy(text + "compacted_thermaldb.mdb", text2, true);
								File.Delete(text + "compacted_thermaldb.mdb");
								goto IL_31D;
							}
							catch
							{
								goto IL_31D;
							}
						}
						string sourceFileName = text + "datadb.org";
						File.Copy(sourceFileName, text2, true);
					}
					else
					{
						if (File.Exists(text + "compacted_thermaldb.mdb"))
						{
							try
							{
								File.Copy(text + "compacted_thermaldb.mdb", text2, true);
								File.Delete(text + "compacted_thermaldb.mdb");
							}
							catch
							{
							}
						}
					}
					IL_31D:
					DBConn dBConn3 = new DBConn();
					try
					{
						dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "thermaldb.mdb;Jet OLEDB:Database Password=root");
						dBConn3.con.Open();
						flag2 = true;
					}
					catch (Exception ex2)
					{
						str = ex2.Message + "\r\n" + ex2.StackTrace;
						int i = 0;
						while (i < 4)
						{
							Thread.Sleep(10);
							i++;
							try
							{
								dBConn3.con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "thermaldb.mdb;Jet OLEDB:Database Password=root");
								dBConn3.con.Open();
								flag2 = true;
								break;
							}
							catch (Exception ex3)
							{
								str = ex3.Message + "\r\n" + ex3.StackTrace;
							}
						}
					}
					if (flag2)
					{
						dBConn3.setInUse();
						dBConn3.DBSource_Type = 3;
						try
						{
							StackTrace stackTrace = new StackTrace();
							MethodBase method = stackTrace.GetFrame(1).GetMethod();
							DBCache.OpenThermalDB(dBConn3.GetHashCode(), method.Name);
							DBCache.PrintThermalDB();
						}
						catch
						{
						}
						result = dBConn3;
					}
					else
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get ThermalDB Connection Error : " + str);
						result = null;
					}
				}
				catch (Exception ex4)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get ThermalDB Connection Error : " + ex4.Message + "\n" + ex4.StackTrace);
					result = null;
				}
			}
			return result;
		}
	}
}
