using ADODB;
using ADOX;
using CommonAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
namespace DBAccessAPI
{
	public class AccessDBUpdate
	{
		public delegate void Delegateshowbar(int i_count);
		public const string MANAGE_DB = "history.mdb";
		public static int i_percent = 1;
		public static bool b_cancel_flag = false;
		public static AccessDBUpdate.Delegateshowbar showOnDBChg = null;
		public static int GetPercent(long l_cur, long l_total)
		{
			int num = 1;
			try
			{
				double num2 = Convert.ToDouble(l_cur);
				double num3 = Convert.ToDouble(l_total);
				double num4 = num2 / num3 * 100.0;
				if (num4 < 1.0)
				{
					num4 = 1.1;
				}
				num = Convert.ToInt32(num4);
			}
			catch
			{
			}
			if (num < 1)
			{
				num = 1;
			}
			if (num > 100)
			{
				num = 99;
			}
			return num;
		}
		public static List<DateTime> GetDataDBDate()
		{
			return null;
		}
		public static int ExportDataDB2MySQL(string dbname, string host, int port, string usr, string pwd)
		{
			List<DateTime> dataDBFileNameList = AccessDBUpdate.GetDataDBFileNameList();
			if (dataDBFileNameList != null && dataDBFileNameList.Count > 0)
			{
				DbConnection dbConnection = null;
				DbCommand dbCommand = new OleDbCommand();
				DBConn dBConn = null;
				DBConn dBConn2 = null;
				try
				{
					DirectorySecurity directorySecurity = new DirectorySecurity();
					directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
					Directory.SetAccessControl(AppDomain.CurrentDomain.BaseDirectory, directorySecurity);
					dbConnection = new MySqlConnection(string.Concat(new object[]
					{
						"Database=",
						dbname,
						";Data Source=",
						host,
						";Port=",
						port,
						";User Id=",
						usr,
						";Password=",
						pwd,
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
					dbConnection.Open();
					string text = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
					if (text[text.Length - 1] != Path.DirectorySeparatorChar)
					{
						text += Path.DirectorySeparatorChar;
					}
					double num = 100.0;
					double num2 = Convert.ToDouble(dataDBFileNameList.Count);
					double num3 = 0.0;
					try
					{
						num3 = num / (num2 * 9.0 + 5.0);
					}
					catch
					{
					}
					double num4 = 0.0;
					int result;
					foreach (DateTime current in dataDBFileNameList)
					{
						if (AccessDBUpdate.b_cancel_flag)
						{
							result = -1;
							return result;
						}
						dBConn = DBConnPool.getDynaConnection(current);
						if (dBConn != null && dBConn.con != null)
						{
							DBTools.preparetable(dbConnection, current);
							if (Directory.Exists(text))
							{
								DBTools.DelFile(text);
								Directory.CreateDirectory(text);
							}
							else
							{
								Directory.CreateDirectory(text);
							}
							dbCommand = dbConnection.CreateCommand();
							int num5 = 0;
							while (num5 < 5 && !Directory.Exists(text))
							{
								Directory.CreateDirectory(text);
								Thread.Sleep(50);
								if (num5 == 4)
								{
									DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~ExportDataDB2MySQL Error : Could not create folder " + text);
									result = -1;
									return result;
								}
								num5++;
							}
							string text2 = text.Replace("\\", "\\\\");
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "port_data_daily", text + "port_data_daily.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "port_data_daily.csv") && new FileInfo(text + "port_data_daily.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"port_data_daily.csv' REPLACE INTO TABLE port_data_daily",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "port_data_hourly", text + "port_data_hourly.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "port_data_hourly.csv") && new FileInfo(text + "port_data_hourly.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"port_data_hourly.csv' REPLACE INTO TABLE port_data_hourly",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "port_auto_info", text + "port_auto_info.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "port_auto_info.csv") && new FileInfo(text + "port_auto_info.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"port_auto_info.csv' REPLACE INTO TABLE port_auto_info",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "device_auto_info", text + "device_auto_info.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "device_auto_info.csv") && new FileInfo(text + "device_auto_info.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"device_auto_info.csv' REPLACE INTO TABLE device_auto_info",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "device_data_daily", text + "device_data_daily.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "device_data_daily.csv") && new FileInfo(text + "device_data_daily.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"device_data_daily.csv' REPLACE INTO TABLE device_data_daily",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "device_data_hourly", text + "device_data_hourly.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "device_data_hourly.csv") && new FileInfo(text + "device_data_hourly.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"device_data_hourly.csv' REPLACE INTO TABLE device_data_hourly",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "bank_auto_info", text + "bank_auto_info.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "bank_auto_info.csv") && new FileInfo(text + "bank_auto_info.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"bank_auto_info.csv' REPLACE INTO TABLE bank_auto_info",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "bank_data_daily", text + "bank_data_daily.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "bank_data_daily.csv") && new FileInfo(text + "bank_data_daily.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"bank_data_daily.csv' REPLACE INTO TABLE bank_data_daily",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "bank_data_hourly", text + "bank_data_hourly.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "bank_data_hourly.csv") && new FileInfo(text + "bank_data_hourly.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"bank_data_hourly.csv' REPLACE INTO TABLE bank_data_hourly",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "rackthermal_hourly", text + "rackthermal_hourly.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "rackthermal_hourly.csv") && new FileInfo(text + "rackthermal_hourly.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"rackthermal_hourly.csv' REPLACE INTO TABLE rackthermal_hourly",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn, "rackthermal_daily", text + Path.DirectorySeparatorChar + "rackthermal_daily.csv") < 0)
							{
								result = -1;
								return result;
							}
							if (File.Exists(text + "rackthermal_daily.csv") && new FileInfo(text + "rackthermal_daily.csv").Length > 0L)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"LOAD DATA LOCAL INFILE '",
									text2,
									"rackthermal_daily.csv' REPLACE INTO TABLE rackthermal_daily",
									current.ToString("yyyyMMdd"),
									" fields terminated by ',' "
								});
								dbCommand.ExecuteNonQuery();
								num4 += num3;
								int num6 = Convert.ToInt32(num4);
								if (num6 < 1)
								{
									num6 = 1;
								}
								if (num6 > 100)
								{
									num6 = 99;
								}
								AccessDBUpdate.i_percent = num6;
							}
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
						}
					}
					dBConn2 = DBConnPool.getThermalConnection();
					if (dBConn2 != null && dBConn2.con != null)
					{
						if (Directory.Exists(text))
						{
							DBTools.DelFile(text);
						}
						else
						{
							Directory.CreateDirectory(text);
							Thread.Sleep(50);
						}
						int num7 = 0;
						while (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
							Thread.Sleep(50);
							num7++;
							if (num7 > 4)
							{
								break;
							}
						}
						string str = text.Replace("\\", "\\\\");
						if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn2, "rack_effect", text + "rack_effect.csv") < 0)
						{
							result = -1;
							return result;
						}
						if (File.Exists(text + "rack_effect.csv") && new FileInfo(text + "rack_effect.csv").Length > 0L)
						{
							dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str + "rack_effect.csv' REPLACE INTO TABLE rack_effect fields terminated by ',' ";
							dbCommand.ExecuteNonQuery();
							num4 += num3;
							int num6 = Convert.ToInt32(num4);
							if (num6 < 1)
							{
								num6 = 1;
							}
							if (num6 > 100)
							{
								num6 = 99;
							}
							AccessDBUpdate.i_percent = num6;
						}
						if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn2, "rci_hourly", text + Path.DirectorySeparatorChar + "rci_hourly.csv") < 0)
						{
							result = -1;
							return result;
						}
						if (File.Exists(text + "rci_hourly.csv") && new FileInfo(text + "rci_hourly.csv").Length > 0L)
						{
							dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str + "rci_hourly.csv' REPLACE INTO TABLE rci_hourly fields terminated by ',' ";
							dbCommand.ExecuteNonQuery();
							num4 += num3;
							int num6 = Convert.ToInt32(num4);
							if (num6 < 1)
							{
								num6 = 1;
							}
							if (num6 > 100)
							{
								num6 = 99;
							}
							AccessDBUpdate.i_percent = num6;
						}
						if (AccessDBUpdate.ExportAccessTableDataToCSVFile(dBConn2, "rci_daily", text + Path.DirectorySeparatorChar + "rci_daily.csv") < 0)
						{
							result = -1;
							return result;
						}
						if (File.Exists(text + "rci_daily.csv") && new FileInfo(text + "rci_daily.csv").Length > 0L)
						{
							dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str + "rci_daily.csv' REPLACE INTO TABLE rci_daily fields terminated by ',' ";
							dbCommand.ExecuteNonQuery();
							num4 += num3;
							int num6 = Convert.ToInt32(num4);
							if (num6 < 1)
							{
								num6 = 1;
							}
							if (num6 > 100)
							{
								num6 = 99;
							}
							AccessDBUpdate.i_percent = num6;
						}
						dBConn2.Close();
					}
					DBTools.DeleteDir(text);
					result = 1;
					return result;
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~ExportDataDB2MySQL Error : " + ex.Message + "\n" + ex.StackTrace);
				}
				finally
				{
					AccessDBUpdate.b_cancel_flag = false;
					if (dBConn2 != null)
					{
						try
						{
							dBConn2.Close();
						}
						catch
						{
						}
					}
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
					if (dbConnection != null)
					{
						try
						{
							dbConnection.Close();
						}
						catch
						{
						}
					}
				}
				return -1;
			}
			return -1;
		}
		public static int ExportDataDB2Access()
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbDataReader dbDataReader = null;
			DbDataAdapter dbDataAdapter = null;
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection2 = null;
			OleDbCommand oleDbCommand2 = null;
			long num = 1L;
			long l_total = 1L;
			string str = "";
			try
			{
				l_total = DBTools.EvaluateCount();
				dBConn = DBConnPool.getDynaConnection();
				if (dBConn != null && dBConn.con != null)
				{
					AccessDBUpdate.DeleteAllAccessDB();
					DataTable dataTable = new DataTable();
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where (table_name like '%_auto_info%' or table_name like '%_data_daily%' or table_name like '%_data_hourly%' or table_name like 'rackthermal_hourly20%' or table_name like 'rackthermal_daily20%' ) and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
					str = dbCommand.CommandText;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
					dbDataAdapter.Dispose();
					int result;
					if (dataTable != null)
					{
						foreach (DataRow dataRow in dataTable.Rows)
						{
							if (AccessDBUpdate.b_cancel_flag)
							{
								result = -1;
								return result;
							}
							string text = Convert.ToString(dataRow[0]);
							string text2 = text;
							string text3 = "";
							string text4 = "";
							string text5 = "";
							DateTime dateTime = DateTime.Now;
							if (text2.Length >= 22)
							{
								try
								{
									text3 = text2.Substring(text2.Length - 8, 4);
									text4 = text2.Substring(text2.Length - 4, 2);
									text5 = text2.Substring(text2.Length - 2, 2);
									dateTime = new DateTime(Convert.ToInt32(text3), Convert.ToInt32(text4), Convert.ToInt32(text5), 0, 0, 0);
								}
								catch
								{
								}
							}
							text2 = string.Concat(new string[]
							{
								text3,
								"-",
								text4,
								"-",
								text5,
								" 00:00:00"
							});
							DateTime dt_inserttime = dateTime;
							oleDbConnection = AccessDBUpdate.getDynaConnection(dt_inserttime);
							if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
							{
								oleDbCommand = oleDbConnection.CreateCommand();
								if (text.IndexOf("device_data_daily") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into device_data_daily (device_id,power_consumption,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select device_id,power_consumption,insert_time from " + text;
									str = dbCommand.CommandText;
									dbDataReader = dbCommand.ExecuteReader();
									while (dbDataReader.Read())
									{
										if (AccessDBUpdate.b_cancel_flag)
										{
											result = -1;
											return result;
										}
										try
										{
											oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
											oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
											oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
											oleDbCommand.ExecuteNonQuery();
										}
										catch
										{
										}
										num += 1L;
									}
									int percent = AccessDBUpdate.GetPercent(num, l_total);
									AccessDBUpdate.i_percent = percent;
									dbDataReader.Close();
								}
								if (text.IndexOf("device_data_hourly") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into device_data_hourly (device_id,power_consumption,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select device_id,power_consumption,insert_time from " + text;
									str = dbCommand.CommandText;
									dbDataReader = dbCommand.ExecuteReader();
									while (dbDataReader.Read())
									{
										if (AccessDBUpdate.b_cancel_flag)
										{
											result = -1;
											return result;
										}
										try
										{
											oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
											oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
											oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
											oleDbCommand.ExecuteNonQuery();
										}
										catch
										{
										}
										num += 1L;
									}
									int percent = AccessDBUpdate.GetPercent(num, l_total);
									AccessDBUpdate.i_percent = percent;
									dbDataReader.Close();
								}
								if (text.IndexOf("device_auto_info") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into device_auto_info (device_id,power,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select device_id,power,insert_time from " + text;
									str = dbCommand.CommandText;
									try
									{
										dbDataReader = dbCommand.ExecuteReader();
										if (dbDataReader != null)
										{
											while (dbDataReader.Read())
											{
												if (AccessDBUpdate.b_cancel_flag)
												{
													result = -1;
													return result;
												}
												try
												{
													oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
													oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
													oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
													oleDbCommand.ExecuteNonQuery();
												}
												catch
												{
												}
												num += 1L;
											}
											int percent = AccessDBUpdate.GetPercent(num, l_total);
											AccessDBUpdate.i_percent = percent;
											dbDataReader.Close();
										}
									}
									catch
									{
									}
								}
								if (text.IndexOf("bank_data_daily") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into bank_data_daily (bank_id,power_consumption,insert_time) values ( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select bank_id,power_consumption,insert_time from " + text;
									str = dbCommand.CommandText;
									dbDataReader = dbCommand.ExecuteReader();
									while (dbDataReader.Read())
									{
										if (AccessDBUpdate.b_cancel_flag)
										{
											result = -1;
											return result;
										}
										try
										{
											oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
											oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
											oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
											oleDbCommand.ExecuteNonQuery();
										}
										catch
										{
										}
										num += 1L;
									}
									int percent = AccessDBUpdate.GetPercent(num, l_total);
									AccessDBUpdate.i_percent = percent;
									dbDataReader.Close();
								}
								if (text.IndexOf("bank_data_hourly") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into bank_data_hourly (bank_id,power_consumption,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select bank_id,power_consumption,insert_time from " + text;
									str = dbCommand.CommandText;
									dbDataReader = dbCommand.ExecuteReader();
									while (dbDataReader.Read())
									{
										if (AccessDBUpdate.b_cancel_flag)
										{
											result = -1;
											return result;
										}
										try
										{
											oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
											oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
											oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
											oleDbCommand.ExecuteNonQuery();
										}
										catch
										{
										}
										num += 1L;
									}
									int percent = AccessDBUpdate.GetPercent(num, l_total);
									AccessDBUpdate.i_percent = percent;
									dbDataReader.Close();
								}
								if (text.IndexOf("bank_auto_info") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into bank_auto_info (bank_id,power,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select bank_id,power,insert_time from " + text;
									str = dbCommand.CommandText;
									try
									{
										dbDataReader = dbCommand.ExecuteReader();
										if (dbDataReader != null)
										{
											while (dbDataReader.Read())
											{
												if (AccessDBUpdate.b_cancel_flag)
												{
													result = -1;
													return result;
												}
												try
												{
													oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
													oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
													oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
													oleDbCommand.ExecuteNonQuery();
												}
												catch
												{
												}
												num += 1L;
											}
											int percent = AccessDBUpdate.GetPercent(num, l_total);
											AccessDBUpdate.i_percent = percent;
											dbDataReader.Close();
										}
									}
									catch
									{
									}
								}
								if (text.IndexOf("port_data_daily") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into port_data_daily (port_id,power_consumption,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select port_id,power_consumption,insert_time from " + text;
									str = dbCommand.CommandText;
									dbDataReader = dbCommand.ExecuteReader();
									while (dbDataReader.Read())
									{
										if (AccessDBUpdate.b_cancel_flag)
										{
											result = -1;
											return result;
										}
										try
										{
											oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
											oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
											oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
											oleDbCommand.ExecuteNonQuery();
										}
										catch
										{
										}
										num += 1L;
									}
									int percent = AccessDBUpdate.GetPercent(num, l_total);
									AccessDBUpdate.i_percent = percent;
									dbDataReader.Close();
								}
								if (text.IndexOf("port_data_hourly") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into port_data_hourly (port_id,power_consumption,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select port_id,power_consumption,insert_time from " + text;
									str = dbCommand.CommandText;
									dbDataReader = dbCommand.ExecuteReader();
									while (dbDataReader.Read())
									{
										if (AccessDBUpdate.b_cancel_flag)
										{
											result = -1;
											return result;
										}
										try
										{
											oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
											oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
											oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
											oleDbCommand.ExecuteNonQuery();
										}
										catch
										{
										}
										num += 1L;
									}
									int percent = AccessDBUpdate.GetPercent(num, l_total);
									AccessDBUpdate.i_percent = percent;
									dbDataReader.Close();
								}
								if (text.IndexOf("port_auto_info") >= 0)
								{
									oleDbCommand.Parameters.Clear();
									oleDbCommand.CommandText = "insert into port_auto_info (port_id,power,insert_time ) values( ?,?,?)";
									oleDbCommand.Parameters.Add("?", OleDbType.Integer);
									oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
									oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
									oleDbCommand.Prepare();
									dbCommand.CommandText = "select port_id,power,insert_time from " + text;
									str = dbCommand.CommandText;
									try
									{
										dbDataReader = dbCommand.ExecuteReader();
										if (dbDataReader != null)
										{
											while (dbDataReader.Read())
											{
												if (AccessDBUpdate.b_cancel_flag)
												{
													result = -1;
													return result;
												}
												try
												{
													oleDbCommand.Parameters[0].Value = Convert.ToInt64(dbDataReader.GetValue(0));
													oleDbCommand.Parameters[1].Value = Convert.ToInt64(dbDataReader.GetValue(1));
													oleDbCommand.Parameters[2].Value = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
													oleDbCommand.ExecuteNonQuery();
												}
												catch
												{
												}
												num += 1L;
											}
											int percent = AccessDBUpdate.GetPercent(num, l_total);
											AccessDBUpdate.i_percent = percent;
											dbDataReader.Close();
										}
									}
									catch
									{
									}
								}
								oleDbCommand.Parameters.Clear();
								if (text.IndexOf("rackthermal_daily") >= 0)
								{
									dbCommand.CommandText = "select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from " + text;
									str = dbCommand.CommandText;
									try
									{
										dbDataReader = dbCommand.ExecuteReader();
										while (dbDataReader.Read())
										{
											if (AccessDBUpdate.b_cancel_flag)
											{
												result = -1;
												return result;
											}
											string text6 = Convert.ToString(dbDataReader.GetValue(1));
											if (text6 == null || text6.Length < 1 || text6.ToLower().Equals("null"))
											{
												text6 = "0";
											}
											string text7 = Convert.ToString(dbDataReader.GetValue(2));
											if (text7 == null || text7.Length < 1 || text7.ToLower().Equals("null"))
											{
												text7 = "0";
											}
											string text8 = Convert.ToString(dbDataReader.GetValue(3));
											if (text8 == null || text8.Length < 1 || text8.ToLower().Equals("null"))
											{
												text8 = "0";
											}
											string text9 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
											if (text9 != null && text9.Length >= 1 && !text9.ToLower().Equals("null"))
											{
												oleDbCommand.CommandText = string.Concat(new string[]
												{
													"insert into rackthermal_daily values (",
													Convert.ToString(dbDataReader.GetValue(0)),
													",",
													text6,
													",",
													text7,
													",",
													text8,
													",#",
													text9,
													"# )"
												});
												str = oleDbCommand.CommandText;
												oleDbCommand.ExecuteNonQuery();
												num += 1L;
											}
										}
										int percent = AccessDBUpdate.GetPercent(num, l_total);
										AccessDBUpdate.i_percent = percent;
										dbDataReader.Close();
									}
									catch
									{
									}
								}
								if (text.IndexOf("rackthermal_hourly") >= 0)
								{
									dbCommand.CommandText = "select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from " + text;
									str = dbCommand.CommandText;
									try
									{
										dbDataReader = dbCommand.ExecuteReader();
										while (dbDataReader.Read())
										{
											if (AccessDBUpdate.b_cancel_flag)
											{
												result = -1;
												return result;
											}
											string text10 = Convert.ToString(dbDataReader.GetValue(1));
											if (text10 == null || text10.Length < 1 || text10.ToLower().Equals("null"))
											{
												text10 = "0";
											}
											string text11 = Convert.ToString(dbDataReader.GetValue(2));
											if (text11 == null || text11.Length < 1 || text11.ToLower().Equals("null"))
											{
												text11 = "0";
											}
											string text12 = Convert.ToString(dbDataReader.GetValue(3));
											if (text12 == null || text12.Length < 1 || text12.ToLower().Equals("null"))
											{
												text12 = "0";
											}
											string text13 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
											if (text13 != null && text13.Length >= 1 && !text13.ToLower().Equals("null"))
											{
												oleDbCommand.CommandText = string.Concat(new string[]
												{
													"insert into rackthermal_hourly values (",
													Convert.ToString(dbDataReader.GetValue(0)),
													",",
													text10,
													",",
													text11,
													",",
													text12,
													",#",
													text13,
													"# )"
												});
												str = oleDbCommand.CommandText;
												oleDbCommand.ExecuteNonQuery();
												num += 1L;
											}
										}
										int percent = AccessDBUpdate.GetPercent(num, l_total);
										AccessDBUpdate.i_percent = percent;
										dbDataReader.Close();
									}
									catch
									{
									}
								}
								oleDbConnection.Close();
							}
						}
					}
					dataTable = new DataTable();
					if (AccessDBUpdate.b_cancel_flag)
					{
						result = -1;
						return result;
					}
					oleDbConnection2 = AccessDBUpdate.getThermalConnection();
					if (oleDbConnection2 != null && oleDbConnection2.State == ConnectionState.Open)
					{
						oleDbCommand2 = oleDbConnection2.CreateCommand();
						dbCommand.CommandText = "select * from rack_effect";
						str = dbCommand.CommandText;
						dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							if (AccessDBUpdate.b_cancel_flag)
							{
								result = -1;
								return result;
							}
							string text14 = "0";
							string text15 = "0";
							string text16 = "0";
							string text17 = "0";
							string text18 = "0";
							string text19 = "0";
							string text20 = "0";
							string text21 = "0";
							string text22 = "0";
							string text23 = "0";
							string text24 = "0";
							string text25 = "0";
							string text26 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							object value = dbDataReader.GetValue(1);
							if (value != null && value != DBNull.Value)
							{
								text14 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(2);
							if (value != null && value != DBNull.Value)
							{
								text15 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(3);
							if (value != null && value != DBNull.Value)
							{
								text16 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(4);
							if (value != null && value != DBNull.Value)
							{
								text17 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(5);
							if (value != null && value != DBNull.Value)
							{
								text18 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(6);
							if (value != null && value != DBNull.Value)
							{
								text19 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(7);
							if (value != null && value != DBNull.Value)
							{
								text20 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(8);
							if (value != null && value != DBNull.Value)
							{
								text21 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(9);
							if (value != null && value != DBNull.Value)
							{
								text22 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(10);
							if (value != null && value != DBNull.Value)
							{
								text23 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(11);
							if (value != null && value != DBNull.Value)
							{
								text24 = Convert.ToString(value);
							}
							value = dbDataReader.GetValue(12);
							if (value != null && value != DBNull.Value)
							{
								text25 = Convert.ToString(value);
							}
							text26 = dbDataReader.GetDateTime(13).ToString("yyyy-MM-dd HH:mm:ss");
							oleDbCommand2.CommandText = string.Concat(new string[]
							{
								"insert into rack_effect values (",
								Convert.ToString(dbDataReader.GetValue(0)),
								",",
								text14,
								",",
								text15,
								",",
								text16,
								",",
								text17,
								",",
								text18,
								",",
								text19,
								",",
								text20,
								",",
								text21,
								",",
								text22,
								",",
								text23,
								",",
								text24,
								",",
								text25,
								",#",
								text26,
								"# )"
							});
							str = oleDbCommand2.CommandText;
							oleDbCommand2.ExecuteNonQuery();
							num += 1L;
						}
						int percent = AccessDBUpdate.GetPercent(num, l_total);
						AccessDBUpdate.i_percent = percent;
						dbDataReader.Close();
						dbCommand.CommandText = "select rci_high,rci_lo,insert_time from rci_daily";
						str = dbCommand.CommandText;
						dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							if (AccessDBUpdate.b_cancel_flag)
							{
								result = -1;
								return result;
							}
							string text27 = Convert.ToString(dbDataReader.GetValue(1));
							if (text27 == null || text27.Length < 1 || text27.ToLower().Equals("null"))
							{
								text27 = "0";
							}
							string text28 = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
							if (text28 != null && text28.Length >= 1 && !text28.ToLower().Equals("null"))
							{
								oleDbCommand2.CommandText = string.Concat(new string[]
								{
									"insert into rci_daily values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text27,
									",#",
									text28,
									"# )"
								});
								str = oleDbCommand2.CommandText;
								oleDbCommand2.ExecuteNonQuery();
								num += 1L;
							}
						}
						percent = AccessDBUpdate.GetPercent(num, l_total);
						AccessDBUpdate.i_percent = percent;
						dbDataReader.Close();
						dbCommand.CommandText = "select rci_high,rci_lo,insert_time from rci_hourly";
						str = dbCommand.CommandText;
						dbDataReader = dbCommand.ExecuteReader();
						while (dbDataReader.Read())
						{
							if (AccessDBUpdate.b_cancel_flag)
							{
								result = -1;
								return result;
							}
							string text29 = Convert.ToString(dbDataReader.GetValue(1));
							if (text29 == null || text29.Length < 1 || text29.ToLower().Equals("null"))
							{
								text29 = "0";
							}
							string text30 = dbDataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
							if (text30 != null && text30.Length >= 1 && !text30.ToLower().Equals("null"))
							{
								oleDbCommand2.CommandText = string.Concat(new string[]
								{
									"insert into rci_hourly values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text29,
									",#",
									text30,
									"# )"
								});
								str = oleDbCommand2.CommandText;
								oleDbCommand2.ExecuteNonQuery();
								num += 1L;
							}
						}
						percent = AccessDBUpdate.GetPercent(num, l_total);
						AccessDBUpdate.i_percent = percent;
						dbDataReader.Close();
						try
						{
							oleDbCommand2.Dispose();
						}
						catch
						{
						}
						try
						{
							oleDbConnection2.Close();
						}
						catch
						{
						}
					}
					if (oleDbCommand2 != null)
					{
						try
						{
							oleDbCommand2.Dispose();
						}
						catch
						{
						}
					}
					if (oleDbConnection2 != null)
					{
						try
						{
							oleDbConnection2.Close();
						}
						catch
						{
						}
					}
					if (dbDataReader != null)
					{
						try
						{
							dbDataReader.Close();
						}
						catch
						{
						}
					}
					if (dbDataAdapter != null)
					{
						try
						{
							dbDataAdapter.Dispose();
						}
						catch
						{
						}
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					DBTools.DropMySQLDatabase(DBUrl.DB_CURRENT_NAME, DBUrl.CURRENT_HOST_PATH, DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
					result = 1;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~ExportDataDB2aAccess Error : " + ex.Message + "\n" + ex.StackTrace);
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~SQL is : " + str);
			}
			finally
			{
				AccessDBUpdate.b_cancel_flag = false;
				if (oleDbCommand2 != null)
				{
					try
					{
						oleDbCommand2.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection2 != null)
				{
					try
					{
						oleDbConnection2.Close();
					}
					catch
					{
					}
				}
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
					}
					catch
					{
					}
				}
				if (dbDataAdapter != null)
				{
					try
					{
						dbDataAdapter.Dispose();
					}
					catch
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dBConn != null)
				{
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static List<DateTime> GetDataDBFileNameList()
		{
			List<DateTime> list = new List<DateTime>();
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			if (Directory.Exists(text))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				FileInfo[] files = directoryInfo.GetFiles();
				if (files.Length != 0)
				{
					FileInfo[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						FileInfo fileInfo = array[i];
						if (fileInfo.Name.IndexOf("datadb") == 0 && fileInfo.Extension.ToLower().Equals(".mdb"))
						{
							try
							{
								string name = fileInfo.Name;
								if (name.Length >= 15)
								{
									DateTime item = Convert.ToDateTime(string.Concat(new string[]
									{
										name.Substring(7, 4),
										"-",
										name.Substring(11, 2),
										"-",
										name.Substring(13, 2),
										" 00:00:00"
									}));
									list.Add(item);
								}
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~GetDataDBFileNameList Error : " + ex.Message + "\n" + ex.StackTrace);
							}
						}
					}
				}
			}
			return list;
		}
		public static int UP2V1212(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "ALTER TABLE port_info ADD COLUMN mac TEXT(120)";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.2' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.2 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1213(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "delete from data_group where grouptype in ('allrack','alldev','alloutlet')";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				dbCommand.CommandText = "insert into data_group (groupname,grouptype,isselect,thermalflag) values ('(Built-in) All Racks','allrack',0,0)";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "insert into data_group (groupname,grouptype,isselect,thermalflag) values ('(Built-in) All Devices','alldev',0,0)";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "insert into data_group (groupname,grouptype,isselect,thermalflag) values ('(Built-in) All Outlets','alloutlet',0,0)";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.3' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.3 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1214(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create table event_info (eventid varchar(10),logflag byte,mailflag byte,reserve integer,PRIMARY KEY(eventid))";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.4' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.4 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1215(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "ALTER TABLE port_info ALTER COLUMN device_id long ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "ALTER TABLE bank_info ALTER COLUMN device_id long ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.5' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.5 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1216(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN pop_flag int null";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN pop_threshold real null";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update device_base_info set pop_flag = 1,pop_threshold = -1 ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.6' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.6 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1217(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create table deviceflag (dev_fresh_flag integer not null,primary key(dev_fresh_flag))";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "create table portflag (port_fresh_flag integer not null,primary key(port_fresh_flag))";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "insert into deviceflag (dev_fresh_flag) values (1)";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "insert into portflag (port_fresh_flag) values (1)";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.7' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.7 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1218(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create table gatewaytable (gid varchar(128),bid varchar(128),sid varchar(128),slevel int,disname varchar(254),capacity real,eleflag int,distype varchar(64),location varchar(254),ip varchar(254),primary key(gid,bid,sid) )";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.8' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				Sys_Para.SetResolution(0);
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.8 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1219(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create table gatewaylastpd (bid varchar(128),sid varchar(128),eleflag int,pd double,timemark datetime,primary key(bid,sid) )";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "create table currentpue (curhour datetime,curday datetime,curweek datetime,ithourpue double,nonithourpue double,itdaypue double,nonitdaypue double,itweekpue double,nonitweekpue double,lasttime datetime )";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.1.9' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.1.9 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1221(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "delete from data_group where grouptype in ('allrack','alldev','alloutlet')";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				dbCommand.CommandText = "update sys_para set para_value = '1.2.2.1' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.2.1 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1222(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN door int null";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update device_base_info set door = 0 ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.2.2' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.2.2 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1223(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create table backuptask (id autoincrement primary key,taskname varchar(128),tasktype int,storetype int,username varchar(255),pwd varchar(255),host varchar(255),port int,filepath varchar(255) )";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.2.3' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.2.3 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1225(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "select * into reportbill from reportthermal where 1=2 ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "ALTER TABLE data_group ADD COLUMN billflag int null";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update data_group set billflag=0 ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.2.5' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.2.5 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int RenewDeviceVoltage(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "drop table device_voltage ";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				dbCommand.CommandText = "create TABLE device_voltage ( id integer not null,voltage real,primary key(id) )";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to renew device_voltage Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1226(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create TABLE device_voltage( id integer not null,voltage real,primary key(id) )";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.2.6' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.2.6 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1227(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "update device_base_info set pop_threshold = -1 where pop_threshold is null or pop_threshold < 0 ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update device_base_info set pop_flag = 1 where pop_flag <> 2 or pop_flag is null ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update device_base_info set door = 0 where door < 0 or door > 3 or door is null ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update data_group set billflag = 0 where billflag < 0 or billflag > 2 or billflag is null ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN device_capacity real ";
				int result2;
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					if (ex.Message.IndexOf("exist") <= 0)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						result2 = -1;
						return result2;
					}
				}
				dbCommand.CommandText = "update device_base_info set device_capacity = 0 ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.2.2.7' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				result2 = 1;
				return result2;
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.2.7 Error : " + ex2.Message + "\n" + ex2.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1230(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				try
				{
					dbCommand.CommandText = "update logrecords set logpara = logpara+'^|^N/A' where (eventid = '0130010' or eventid = '0130011' or eventid = '0430000' or eventid = '0430001' or eventid = '0630010' or eventid = '0630011' or eventid = '0630020' or eventid = '0630021')";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "update logrecords set logpara = logpara+'^|^N/A' where (eventid = '0630053' or eventid = '0630054' or eventid = '0630055') and  logpara  not like '*^|^*^|^*^|^*' ";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				dbCommand.CommandText = "update sys_para set para_value = '1.2.3.0' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.3.0 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1231(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN restoreflag int ";
				int result2;
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					if (ex.Message.IndexOf("exist") <= 0)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						result2 = -1;
						return result2;
					}
				}
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (File.Exists(text + "restore.flag"))
				{
					dbCommand.CommandText = "update device_base_info set restoreflag = 1 ";
					dbCommand.ExecuteNonQuery();
					File.Delete(text + "restore.flag");
				}
				else
				{
					dbCommand.CommandText = "update device_base_info set restoreflag = 0 ";
					dbCommand.ExecuteNonQuery();
				}
				dbCommand.CommandText = "create table devicedefine(model_nm varchar(255),dev_ver varchar(255),first_data memo,second_data memo,compatible int,reserve varchar(255),PRIMARY KEY(model_nm,dev_ver))";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception ex2)
				{
					if (ex2.Message.IndexOf("exist") <= 0)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						result2 = -1;
						return result2;
					}
				}
				dbCommand.CommandText = "update sys_para set para_value = '1.2.3.1' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				result2 = 1;
				return result2;
			}
			catch (Exception ex3)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.2.3.1 Error : " + ex3.Message + "\n" + ex3.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1325(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create table ugp (uid  integer not null,gid integer not null,status integer, PRIMARY KEY(uid,gid))";
				int result2;
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					if (ex.Message.IndexOf("exist") <= 0)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						result2 = -1;
						return result2;
					}
				}
				dbCommand.CommandText = "update sys_para set para_value = '1.3.2.5' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				result2 = 1;
				return result2;
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.3.2.5 Error : " + ex2.Message + "\n" + ex2.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UPMYSQL2V1214(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "create index did_index on device_data_hourly (device_id desc)  WITH DISALLOW NULL";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "create index pid_index on port_data_hourly (port_id desc)  WITH DISALLOW NULL";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "create index bid_index on bank_data_hourly (bank_id desc)  WITH DISALLOW NULL";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.Dispose();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update mysql database to 1.2.1.4 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UPMYSQL2V1221(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				try
				{
					dbCommand.CommandText = "create index index_rackeffect on rack_effect (id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_bankp on bank_auto_info (bank_id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_bankpdd on bank_data_daily (bank_id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_portpdh on port_data_hourly (port_id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_bankpdh on bank_data_hourly (bank_id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_devicepdh on device_data_hourly (device_id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_rth on rackthermal_hourly (rack_id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_rtd on rackthermal_daily (rack_id,insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_rcih on rci_hourly (insert_time)";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "create index index_rcid on rci_daily (insert_time)";
					dbCommand.ExecuteNonQuery();
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
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update mysql database to 1.2.1.4 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UPMYSQL2V1325(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			try
			{
				DataTable dataTable = new DataTable();
				dbDataAdapter = DBConn.GetDataAdapter(conn.con);
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "select distinct(date_format(insert_time, '%Y-%m-%d')) from device_auto_info  order by insert_time ASC";
				dbDataAdapter.SelectCommand = dbCommand;
				dbDataAdapter.Fill(dataTable);
				dbDataAdapter.Dispose();
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						string text = Convert.ToString(dataRow[0]);
						text += " 00:00:00";
						DateTime dateTime = Convert.ToDateTime(text);
						string str = "device_auto_info" + dateTime.ToString("yyyyMMdd");
						string str2 = "bank_auto_info" + dateTime.ToString("yyyyMMdd");
						string str3 = "port_auto_info" + dateTime.ToString("yyyyMMdd");
						dbCommand.CommandText = "CREATE TABLE `" + str + "` (`device_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `daiind1` (`device_id`),KEY `daiind2` (`insert_time`)) ENGINE=MYISAM ";
						try
						{
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand.CommandText = "CREATE TABLE `" + str2 + "` (`bank_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `baiind1` (`bank_id`),KEY `baiind2` (`insert_time`)) ENGINE=MYISAM ";
						try
						{
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand.CommandText = "CREATE TABLE `" + str3 + "` (`port_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `paiind1` (`port_id`),KEY `paiind2` (`insert_time`)) ENGINE=MYISAM ";
						try
						{
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand.CommandText = string.Concat(new string[]
						{
							"insert into device_auto_info",
							dateTime.ToString("yyyyMMdd"),
							" (device_id,power,insert_time) select device_id,power,insert_time from device_auto_info  where insert_time >= '",
							dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand.CommandText = string.Concat(new string[]
						{
							"insert into bank_auto_info",
							dateTime.ToString("yyyyMMdd"),
							" (bank_id,power,insert_time) select bank_id,power,insert_time from bank_auto_info  where insert_time >= '",
							dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand.CommandText = string.Concat(new string[]
						{
							"insert into port_auto_info",
							dateTime.ToString("yyyyMMdd"),
							" (port_id,power,insert_time) select port_id,power,insert_time from port_auto_info  where insert_time >= '",
							dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
					}
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update mysql database to 1.2.1.4 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataAdapter != null)
				{
					try
					{
						dbDataAdapter.Dispose();
					}
					catch
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UPACCESS2V1325()
		{
			int result = -1;
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb";
			if (!File.Exists(text))
			{
				return -1;
			}
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			try
			{
				dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + ";Jet OLEDB:Database Password=root");
				dbConnection.Open();
				DataTable dataTable = new DataTable();
				dbDataAdapter = DBConn.GetDataAdapter(dbConnection);
				dbCommand = dbConnection.CreateCommand();
				dbCommand.CommandText = "select distinct(FORMAT(insert_time, 'yyyy-MM-dd')) from device_data_daily ";
				dbDataAdapter.SelectCommand = dbCommand;
				dbDataAdapter.Fill(dataTable);
				dbDataAdapter.Dispose();
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						string text2 = Convert.ToString(dataRow[0]);
						text2 += " 00:00:00";
						DateTime dt_inserttime = Convert.ToDateTime(text2);
						OleDbConnection dynaConnection = AccessDBUpdate.getDynaConnection(dt_inserttime);
						if (dynaConnection != null && dynaConnection.State == ConnectionState.Open)
						{
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select device_id,power_consumption,insert_time from device_data_daily where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							DbDataReader dbDataReader = dbCommand.ExecuteReader();
							OleDbCommand oleDbCommand = dynaConnection.CreateCommand();
							while (dbDataReader.Read())
							{
								try
								{
									oleDbCommand.CommandText = string.Concat(new string[]
									{
										"insert into device_data_daily values (",
										Convert.ToString(dbDataReader.GetValue(0)),
										",",
										Convert.ToString(dbDataReader.GetValue(1)),
										",#",
										Convert.ToString(dbDataReader.GetValue(2)),
										"# )"
									});
									oleDbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
							dbDataReader.Close();
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select device_id,power_consumption,insert_time from device_data_hourly where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								try
								{
									oleDbCommand.CommandText = string.Concat(new string[]
									{
										"insert into device_data_hourly values (",
										Convert.ToString(dbDataReader.GetValue(0)),
										",",
										Convert.ToString(dbDataReader.GetValue(1)),
										",#",
										Convert.ToString(dbDataReader.GetValue(2)),
										"# )"
									});
									oleDbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
							dbDataReader.Close();
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select device_id,power,insert_time from device_auto_info where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							try
							{
								dbDataReader = dbCommand.ExecuteReader();
							}
							catch
							{
							}
							if (dbDataReader != null)
							{
								while (dbDataReader.Read())
								{
									try
									{
										oleDbCommand.CommandText = string.Concat(new string[]
										{
											"insert into device_auto_info values (",
											Convert.ToString(dbDataReader.GetValue(0)),
											",",
											Convert.ToString(dbDataReader.GetValue(1)),
											",#",
											Convert.ToString(dbDataReader.GetValue(2)),
											"# )"
										});
										oleDbCommand.ExecuteNonQuery();
									}
									catch
									{
									}
								}
								dbDataReader.Close();
							}
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select bank_id,power_consumption,insert_time from bank_data_daily where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								try
								{
									oleDbCommand.CommandText = string.Concat(new string[]
									{
										"insert into bank_data_daily values (",
										Convert.ToString(dbDataReader.GetValue(0)),
										",",
										Convert.ToString(dbDataReader.GetValue(1)),
										",#",
										Convert.ToString(dbDataReader.GetValue(2)),
										"# )"
									});
									oleDbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
							dbDataReader.Close();
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select bank_id,power_consumption,insert_time from bank_data_hourly where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								try
								{
									oleDbCommand.CommandText = string.Concat(new string[]
									{
										"insert into bank_data_hourly values (",
										Convert.ToString(dbDataReader.GetValue(0)),
										",",
										Convert.ToString(dbDataReader.GetValue(1)),
										",#",
										Convert.ToString(dbDataReader.GetValue(2)),
										"# )"
									});
									oleDbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
							dbDataReader.Close();
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select bank_id,power,insert_time from bank_auto_info where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							try
							{
								dbDataReader = dbCommand.ExecuteReader();
							}
							catch
							{
							}
							if (dbDataReader != null)
							{
								while (dbDataReader.Read())
								{
									try
									{
										oleDbCommand.CommandText = string.Concat(new string[]
										{
											"insert into bank_auto_info values (",
											Convert.ToString(dbDataReader.GetValue(0)),
											",",
											Convert.ToString(dbDataReader.GetValue(1)),
											",#",
											Convert.ToString(dbDataReader.GetValue(2)),
											"# )"
										});
										oleDbCommand.ExecuteNonQuery();
									}
									catch
									{
									}
								}
								dbDataReader.Close();
							}
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select port_id,power_consumption,insert_time from port_data_daily where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								try
								{
									oleDbCommand.CommandText = string.Concat(new string[]
									{
										"insert into port_data_daily values (",
										Convert.ToString(dbDataReader.GetValue(0)),
										",",
										Convert.ToString(dbDataReader.GetValue(1)),
										",#",
										Convert.ToString(dbDataReader.GetValue(2)),
										"# )"
									});
									oleDbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
							dbDataReader.Close();
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select port_id,power_consumption,insert_time from port_data_hourly where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							dbDataReader = dbCommand.ExecuteReader();
							while (dbDataReader.Read())
							{
								try
								{
									oleDbCommand.CommandText = string.Concat(new string[]
									{
										"insert into port_data_hourly values (",
										Convert.ToString(dbDataReader.GetValue(0)),
										",",
										Convert.ToString(dbDataReader.GetValue(1)),
										",#",
										Convert.ToString(dbDataReader.GetValue(2)),
										"# )"
									});
									oleDbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
							dbDataReader.Close();
							dbCommand.CommandText = string.Concat(new string[]
							{
								"select port_id,power,insert_time from port_auto_info where insert_time >= #",
								dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
								"# and insert_time < #",
								dt_inserttime.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							try
							{
								dbDataReader = dbCommand.ExecuteReader();
							}
							catch
							{
							}
							if (dbDataReader != null)
							{
								while (dbDataReader.Read())
								{
									try
									{
										oleDbCommand.CommandText = string.Concat(new string[]
										{
											"insert into port_auto_info values (",
											Convert.ToString(dbDataReader.GetValue(0)),
											",",
											Convert.ToString(dbDataReader.GetValue(1)),
											",#",
											Convert.ToString(dbDataReader.GetValue(2)),
											"# )"
										});
										oleDbCommand.ExecuteNonQuery();
									}
									catch
									{
									}
								}
								dbDataReader.Close();
							}
							dynaConnection.Close();
						}
					}
				}
				OleDbConnection thermalConnection = AccessDBUpdate.getThermalConnection();
				if (thermalConnection != null && thermalConnection.State == ConnectionState.Open)
				{
					OleDbCommand oleDbCommand2 = thermalConnection.CreateCommand();
					dbCommand.CommandText = "select * from rack_effect";
					DbDataReader dbDataReader2 = dbCommand.ExecuteReader();
					while (dbDataReader2.Read())
					{
						string text3 = "0";
						string text4 = "0";
						string text5 = "0";
						string text6 = "0";
						string text7 = "0";
						string text8 = "0";
						string text9 = "0";
						string text10 = "0";
						string text11 = "0";
						string text12 = "0";
						string text13 = "0";
						string text14 = "0";
						string text15 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						object value = dbDataReader2.GetValue(1);
						if (value != null && value != DBNull.Value)
						{
							text3 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(2);
						if (value != null && value != DBNull.Value)
						{
							text4 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(3);
						if (value != null && value != DBNull.Value)
						{
							text5 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(4);
						if (value != null && value != DBNull.Value)
						{
							text6 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(5);
						if (value != null && value != DBNull.Value)
						{
							text7 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(6);
						if (value != null && value != DBNull.Value)
						{
							text8 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(7);
						if (value != null && value != DBNull.Value)
						{
							text9 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(8);
						if (value != null && value != DBNull.Value)
						{
							text10 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(9);
						if (value != null && value != DBNull.Value)
						{
							text11 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(10);
						if (value != null && value != DBNull.Value)
						{
							text12 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(11);
						if (value != null && value != DBNull.Value)
						{
							text13 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(12);
						if (value != null && value != DBNull.Value)
						{
							text14 = Convert.ToString(value);
						}
						value = dbDataReader2.GetValue(13);
						if (value != null && value != DBNull.Value)
						{
							text15 = Convert.ToString(value);
						}
						try
						{
							oleDbCommand2.CommandText = string.Concat(new string[]
							{
								"insert into rack_effect values (",
								Convert.ToString(dbDataReader2.GetValue(0)),
								",",
								text3,
								",",
								text4,
								",",
								text5,
								",",
								text6,
								",",
								text7,
								",",
								text8,
								",",
								text9,
								",",
								text10,
								",",
								text11,
								",",
								text12,
								",",
								text13,
								",",
								text14,
								",#",
								text15,
								"# )"
							});
							oleDbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
					}
					dbDataReader2.Close();
					dbCommand.CommandText = "select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_daily";
					dbDataReader2 = dbCommand.ExecuteReader();
					while (dbDataReader2.Read())
					{
						try
						{
							oleDbCommand2.CommandText = string.Concat(new string[]
							{
								"insert into rackthermal_daily values (",
								Convert.ToString(dbDataReader2.GetValue(0)),
								",",
								Convert.ToString(dbDataReader2.GetValue(1)),
								",",
								Convert.ToString(dbDataReader2.GetValue(2)),
								",",
								Convert.ToString(dbDataReader2.GetValue(3)),
								",#",
								Convert.ToString(dbDataReader2.GetValue(4)),
								"# )"
							});
							oleDbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
					}
					dbDataReader2.Close();
					dbCommand.CommandText = "select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_hourly";
					dbDataReader2 = dbCommand.ExecuteReader();
					while (dbDataReader2.Read())
					{
						try
						{
							oleDbCommand2.CommandText = string.Concat(new string[]
							{
								"insert into rackthermal_hourly values (",
								Convert.ToString(dbDataReader2.GetValue(0)),
								",",
								Convert.ToString(dbDataReader2.GetValue(1)),
								",",
								Convert.ToString(dbDataReader2.GetValue(2)),
								",",
								Convert.ToString(dbDataReader2.GetValue(3)),
								",#",
								Convert.ToString(dbDataReader2.GetValue(4)),
								"# )"
							});
							oleDbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
					}
					dbDataReader2.Close();
					dbCommand.CommandText = "select rci_high,rci_lo,insert_time from rci_daily";
					dbDataReader2 = dbCommand.ExecuteReader();
					while (dbDataReader2.Read())
					{
						try
						{
							oleDbCommand2.CommandText = string.Concat(new string[]
							{
								"insert into rci_daily values (",
								Convert.ToString(dbDataReader2.GetValue(0)),
								",",
								Convert.ToString(dbDataReader2.GetValue(1)),
								",#",
								Convert.ToString(dbDataReader2.GetValue(2)),
								"# )"
							});
							oleDbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
					}
					dbDataReader2.Close();
					dbCommand.CommandText = "select rci_high,rci_lo,insert_time from rci_hourly";
					dbDataReader2 = dbCommand.ExecuteReader();
					while (dbDataReader2.Read())
					{
						try
						{
							oleDbCommand2.CommandText = string.Concat(new string[]
							{
								"insert into rci_hourly values (",
								Convert.ToString(dbDataReader2.GetValue(0)),
								",",
								Convert.ToString(dbDataReader2.GetValue(1)),
								",#",
								Convert.ToString(dbDataReader2.GetValue(2)),
								"# )"
							});
							oleDbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
					}
					dbDataReader2.Close();
					try
					{
						oleDbCommand2.Dispose();
					}
					catch
					{
					}
					try
					{
						thermalConnection.Close();
					}
					catch
					{
					}
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
					dbConnection.Close();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to Multi files Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataAdapter != null)
				{
					try
					{
						dbDataAdapter.Dispose();
					}
					catch
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UPDATADB2V1325()
		{
			int result = -1;
			DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Start Transfer database(by files) to Multi files");
			List<string> list = new List<string>();
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb";
			if (!File.Exists(text))
			{
				return -1;
			}
			string text2 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
			{
				text2 += Path.DirectorySeparatorChar;
			}
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			if (!File.Exists(text2 + "history.mdb") && AccessDBUpdate.createdb() < 0)
			{
				return -1;
			}
			Hashtable historyDataStatus = DBTools.GetHistoryDataStatus();
			if (historyDataStatus == null)
			{
				return -1;
			}
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			bool flag = true;
			bool flag2 = false;
			try
			{
				dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + ";Jet OLEDB:Database Password=root");
				dbConnection.Open();
				DataTable dataTable = new DataTable();
				dbDataAdapter = DBConn.GetDataAdapter(dbConnection);
				dbCommand = dbConnection.CreateCommand();
				dbCommand.CommandText = "select distinct(FORMAT(insert_time, 'yyyy-MM-dd')) from device_data_daily order by FORMAT(insert_time, 'yyyy-MM-dd') DESC ";
				dbDataAdapter.SelectCommand = dbCommand;
				dbDataAdapter.Fill(dataTable);
				dbDataAdapter.Dispose();
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
				int result2;
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						DataRow dataRow = dataTable.Rows[i];
						string text3 = Convert.ToString(dataRow[0]);
						DateTime d = Convert.ToDateTime(text3 + " 00:00:00");
						if (d.CompareTo(DateTime.Now) <= 0)
						{
							if (Math.Abs((DateTime.Now - d).TotalDays) < 1.0)
							{
								flag2 = true;
							}
							else
							{
								string text4 = "datadb_" + d.ToString("yyyyMMdd") + ".mdb";
								if (historyDataStatus.ContainsKey(text4))
								{
									DBTools.UpdateHistoryDataStatus(text4, "-2");
								}
								else
								{
									DBTools.InsertHistoryDataStatus(text4, "-2");
								}
							}
							list.Add(text3);
						}
					}
					if (flag2)
					{
						string text5 = text2 + "datadb_" + DateTime.Now.ToString("yyyyMMdd") + ".mdb";
						DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Start copy today file " + text5);
						if (DBTools.FileCopy(text, text5) < 0)
						{
							result2 = -1;
							return result2;
						}
						DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Finish copy today file " + text5);
						int num = DBTools.FilterData(text5, DateTime.Now.ToString("yyyy-MM-dd"), false);
						if (num < 0)
						{
							result2 = -1;
							return result2;
						}
						num = DBTools.CompactAccess(text5);
						if (num < 0)
						{
							result2 = -1;
							return result2;
						}
						if (num > 0 && File.Exists(text5))
						{
							FileInfo fileInfo = new FileInfo(text5);
							long num2 = fileInfo.Length * 3L;
							DBTools.UpdateHistoryDataStatus(fileInfo.Name, string.Concat(num2));
						}
					}
					if (flag)
					{
						flag = false;
						string text6 = text2 + "thermaldb.mdb";
						DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Start copy thermaldb file " + text6);
						if (DBTools.FileCopy(text, text6) < 0)
						{
							result2 = -1;
							return result2;
						}
						DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Finish copy thermaldb file " + text6);
						int num3 = DBTools.FilterData(text6, DateTime.Now.ToString("yyyy-MM-dd"), true);
						if (num3 < 0)
						{
							result2 = -1;
							return result2;
						}
						num3 = DBTools.CompactAccess(text6);
						if (num3 < 0)
						{
							result2 = -1;
							return result2;
						}
						if (num3 > 0 && File.Exists(text6))
						{
							FileInfo fileInfo2 = new FileInfo(text6);
							long num4 = fileInfo2.Length * 3L;
							DBTools.UpdateHistoryDataStatus("thermaldb.mdb", string.Concat(num4));
						}
					}
				}
				result2 = 1;
				return result2;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database(by files) to Multi files Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataAdapter != null)
				{
					try
					{
						dbDataAdapter.Dispose();
					}
					catch
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UPLog2V1325(DBConn conn)
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb";
			if (!File.Exists(path))
			{
				return -1;
			}
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				DataTable dataTable = new DataTable();
				DbDataAdapter dataAdapter = DBConn.GetDataAdapter(conn.con);
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "select ticks,eventid,logpara from logrecords order by ticks ASC ";
				dataAdapter.SelectCommand = dbCommand;
				dataAdapter.Fill(dataTable);
				dataAdapter.Dispose();
				int result2;
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					OleDbConnection oleDbConnection = null;
					try
					{
						oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb;Jet OLEDB:Database Password=ecoSensorlog");
						oleDbConnection.Open();
					}
					catch
					{
						result2 = -1;
						return result2;
					}
					if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
					{
						OleDbCommand oleDbCommand = oleDbConnection.CreateCommand();
						try
						{
							oleDbCommand.CommandText = "delete from logrecords ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						foreach (DataRow dataRow in dataTable.Rows)
						{
							try
							{
								oleDbCommand.CommandText = "insert into logrecords (ticks,eventid,logpara ) values(?,?,?) ";
								oleDbCommand.Parameters.AddWithValue("@ticks", dataRow[0]);
								oleDbCommand.Parameters.AddWithValue("@eventid", dataRow[1]);
								oleDbCommand.Parameters.AddWithValue("@logpara", dataRow[2]);
								oleDbCommand.ExecuteNonQuery();
								oleDbCommand.Parameters.Clear();
							}
							catch
							{
								try
								{
									oleDbCommand.Parameters.Clear();
								}
								catch
								{
								}
							}
						}
						try
						{
							oleDbCommand.Dispose();
						}
						catch
						{
						}
						try
						{
							oleDbConnection.Close();
						}
						catch
						{
						}
					}
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				result2 = 1;
				return result2;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update log database to 1.3.2.5 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		private static int ExportAccessTableDataToCSVFile(DBConn conn, string str_tablename, string str_filefullname)
		{
			if (AccessDBUpdate.b_cancel_flag)
			{
				return -1;
			}
			int result = -1;
			StreamWriter streamWriter = null;
			DbCommand dbCommand = new OleDbCommand();
			DbDataReader dbDataReader = null;
			try
			{
				if (File.Exists(str_filefullname))
				{
					File.Delete(str_filefullname);
				}
				FileStream stream = new FileStream(str_filefullname, FileMode.CreateNew, FileAccess.Write, FileShare.None);
				streamWriter = new StreamWriter(stream, Encoding.ASCII);
				string commandText = "select * from " + str_tablename;
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText;
					dbDataReader = dbCommand.ExecuteReader();
					int result2;
					while (dbDataReader.Read())
					{
						if (AccessDBUpdate.b_cancel_flag)
						{
							result2 = -1;
							return result2;
						}
						string text = "";
						for (int i = 0; i < dbDataReader.FieldCount; i++)
						{
							object value = dbDataReader.GetValue(i);
							Type fieldType = dbDataReader.GetFieldType(i);
							string str;
							if (value == null || value is DBNull)
							{
								if (fieldType.FullName.Equals("System.Int32") || fieldType.FullName.Equals("System.Double") || fieldType.FullName.Equals("System.Int64") || fieldType.FullName.Equals("System.Single"))
								{
									str = "0,";
								}
								else
								{
									str = ",";
								}
							}
							else
							{
								if (fieldType.FullName.Equals("System.DateTime"))
								{
									str = dbDataReader.GetDateTime(i).ToString("yyyy-MM-dd HH:mm:ss") + ",";
								}
								else
								{
									str = Convert.ToString(value, new CultureInfo("en-US")) + ",";
								}
							}
							text += str;
						}
						if (text.EndsWith(","))
						{
							text = text.Substring(0, text.Length - 1);
						}
						streamWriter.WriteLine(text);
					}
					dbDataReader.Close();
					result2 = 1;
					return result2;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Export Access DB  data error : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Dispose();
					}
					catch
					{
					}
				}
				if (streamWriter != null)
				{
					streamWriter.Flush();
					streamWriter.Close();
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
			}
			return result;
		}
		public static void InitAccessDataDB()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			if (Directory.Exists(text))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				FileInfo[] files = directoryInfo.GetFiles();
				if (files.Length != 0)
				{
					FileInfo[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						FileInfo fileInfo = array[i];
						if (fileInfo.Extension.ToLower().Equals(".mdb"))
						{
							try
							{
								fileInfo.Delete();
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~DeleteAllAccessDB Error : " + ex.Message + "\n" + ex.StackTrace);
							}
						}
					}
				}
				string sourceFileName = text + "datadb.org";
				try
				{
					File.Copy(sourceFileName, text + "thermaldb.mdb", true);
				}
				catch
				{
				}
			}
		}
		private static void DeleteAllAccessDB()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			if (Directory.Exists(text))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				FileInfo[] files = directoryInfo.GetFiles();
				if (files.Length != 0)
				{
					FileInfo[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						FileInfo fileInfo = array[i];
						if (fileInfo.Extension.ToLower().Equals(".mdb"))
						{
							try
							{
								fileInfo.Delete();
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~DeleteAllAccessDB Error : " + ex.Message + "\n" + ex.StackTrace);
							}
						}
					}
				}
				string sourceFileName = text + "datadb.org";
				try
				{
					File.Copy(sourceFileName, text + "thermaldb.mdb", true);
				}
				catch
				{
				}
			}
		}
		private static OleDbConnection getDynaConnection(DateTime dt_inserttime)
		{
			OleDbConnection result;
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
				string text2 = text + "datadb_" + dt_inserttime.ToString("yyyyMMdd") + ".mdb";
				if (!File.Exists(text2))
				{
					string sourceFileName = text + "datadb.org";
					File.Copy(sourceFileName, text2, true);
				}
				OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				result = oleDbConnection;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~getDynaConnection Error : " + ex.Message + "\n" + ex.StackTrace);
				result = null;
			}
			return result;
		}
		private static OleDbConnection getThermalConnection()
		{
			OleDbConnection result;
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
				string text2 = text + "thermaldb.mdb";
				if (!File.Exists(text2))
				{
					string sourceFileName = text + "datadb.org";
					File.Copy(sourceFileName, text2, true);
				}
				OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				result = oleDbConnection;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~getThermalConnection Error : " + ex.Message + "\n" + ex.StackTrace);
				result = null;
			}
			return result;
		}
		private static int createdb()
		{
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!File.Exists(text + "history.mdb"))
				{
					Catalog catalog = (Catalog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000602-0000-0010-8000-00AA006D2EA4")));
					catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "history.mdb;Jet OLEDB:Database Password=root");
					Connection connection = (Connection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000514-0000-0010-8000-00AA006D2EA4")));
					if (File.Exists(text + "history.mdb"))
					{
						connection.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "history.mdb;Jet OLEDB:Database Password=root", null, null, -1);
						catalog.ActiveConnection = connection;
						Table table = (Table)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000609-0000-0010-8000-00AA006D2EA4")));
						table.ParentCatalog = catalog;
						table.Name = "compactdb";
						Column column = (Column)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0000061B-0000-0010-8000-00AA006D2EA4")));
						column.ParentCatalog = catalog;
						column.Name = "dbname";
						column.Type = DataTypeEnum.adVarWChar;
						table.Columns.Append(column, DataTypeEnum.adVarWChar, 128);
						table.Keys.Append("dbnamePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null);
						table.Columns.Append("dbsize", DataTypeEnum.adVarWChar, 128);
						catalog.Tables.Append(table);
						connection.Close();
						return 1;
					}
				}
			}
			catch
			{
			}
			return -1;
		}
		public static int UP2V1366(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				string text = DateTime.Now.ToString("yyyyMMdd");
				DateTime dateTime = DateTime.Now.AddDays(1.0);
				DateTime dateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
				DateTime dateTime3 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				DBConn dBConn = null;
				DbCommand dbCommand2 = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand2 = dBConn.con.CreateCommand();
						dbCommand2.CommandText = "drop table device_data_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table bank_data_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table port_data_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table device_data_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table bank_data_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table port_data_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table rackthermal_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table rackthermal_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text2 = "CREATE TABLE `bank_data_daily" + text + "` (";
						text2 += "`bank_id` int(11) NOT NULL,";
						text2 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text2 += "`insert_time` date NOT NULL,";
						text2 += "KEY `index_bankpdd` (`bank_id`,`insert_time`),  KEY `bdd_idx1` (`bank_id`),  KEY `bdd_idx2` (`insert_time`)";
						text2 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text2;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text3 = "CREATE TABLE `bank_data_hourly" + text + "` (";
						text3 += "`bank_id` int(11) NOT NULL,";
						text3 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text3 += "`insert_time` datetime NOT NULL,";
						text3 += "KEY `index_bankpdh` (`bank_id`,`insert_time`),  KEY `bdh_idx1` (`bank_id`),  KEY `bdh_idx2` (`insert_time`)";
						text3 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text3;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text4 = "CREATE TABLE `device_data_daily" + text + "` (";
						text4 += "`device_id` int(11) NOT NULL,";
						text4 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text4 += "`insert_time` date NOT NULL,";
						text4 += "KEY `index_dev_daily` (`device_id`,`insert_time`),  KEY `ddd_idx1` (`device_id`),  KEY `ddd_idx2` (`insert_time`)";
						text4 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text4;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text5 = "CREATE TABLE `device_data_hourly" + text + "` (";
						text5 += "`device_id` int(11) NOT NULL,";
						text5 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text5 += "`insert_time` datetime NOT NULL,";
						text5 += "KEY `index_devicepdh` (`device_id`,`insert_time`),  KEY `ddh_idx1` (`device_id`),  KEY `ddh_idx2` (`insert_time`)";
						text5 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text5;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text6 = "CREATE TABLE `port_data_daily" + text + "` (";
						text6 += "`port_id` int(11) NOT NULL,";
						text6 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text6 += "`insert_time` date NOT NULL,";
						text6 += "KEY `index_port_daily` (`port_id`,`insert_time`),  KEY `pdd_idx1` (`port_id`),  KEY `pdd_idx2` (`insert_time`)";
						text6 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text6;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text7 = "CREATE TABLE `port_data_hourly" + text + "` (";
						text7 += "`port_id` int(11) NOT NULL,";
						text7 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text7 += "`insert_time` datetime NOT NULL,";
						text7 += "KEY `index_portpdh` (`port_id`,`insert_time`), KEY `pdh_idx1` (`port_id`),  KEY `pdh_idx2` (`insert_time`)";
						text7 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text7;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_hourly" + text + "` ( `rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_daily" + text + "` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_daily",
							text,
							" select * from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_hourly",
							text,
							" select * from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into device_data_daily",
							text,
							" select * from device_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into device_data_hourly",
							text,
							" select * from device_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into bank_data_daily",
							text,
							" select * from bank_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into bank_data_hourly",
							text,
							" select * from bank_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into port_data_daily",
							text,
							" select * from port_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into port_data_hourly",
							text,
							" select * from port_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from device_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from device_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from bank_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from bank_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from port_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from port_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand2.Dispose();
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
						dbCommand = conn.con.CreateCommand();
						dbCommand.CommandText = "update sys_para set para_value = '1.3.6.6' where para_name = 'DBVERSION' and para_type = 'String'";
						dbCommand.ExecuteNonQuery();
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						int result2 = 1;
						return result2;
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand2.Dispose();
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
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
				}
				return result;
			}
			DateTime.Now.ToString("yyyyMMdd");
			DateTime dateTime4 = DateTime.Now.AddDays(1.0);
			DateTime dateTime5 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
			DateTime dateTime6 = new DateTime(dateTime4.Year, dateTime4.Month, dateTime4.Day, 0, 0, 0);
			DBConn dBConn2 = null;
			DbCommand dbCommand3 = null;
			DBConn dBConn3 = null;
			DbCommand dbCommand4 = null;
			try
			{
				dBConn2 = DBConnPool.getDynaConnection();
				dBConn3 = DBConnPool.getThermalConnection();
				if (dBConn2 != null && dBConn2.con != null && dBConn3 != null && dBConn3.con != null)
				{
					dbCommand4 = dBConn3.con.CreateCommand();
					dbCommand3 = dBConn2.con.CreateCommand();
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_daily where insert_time >= #",
						dateTime5.ToString("yyyy-MM-dd"),
						"# and insert_time < #",
						dateTime6.ToString("yyyy-MM-dd"),
						"#"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text8 = Convert.ToString(dbDataReader.GetValue(1));
							if (text8 == null || text8.Length < 1 || text8.ToLower().Equals("null"))
							{
								text8 = "0";
							}
							string text9 = Convert.ToString(dbDataReader.GetValue(2));
							if (text9 == null || text9.Length < 1 || text9.ToLower().Equals("null"))
							{
								text9 = "0";
							}
							string text10 = Convert.ToString(dbDataReader.GetValue(3));
							if (text10 == null || text10.Length < 1 || text10.ToLower().Equals("null"))
							{
								text10 = "0";
							}
							string text11 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd");
							if (text11 != null && text11.Length >= 1 && !text11.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_daily values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text8,
									",",
									text9,
									",",
									text10,
									",#",
									text11,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_hourly where insert_time >= '",
						dateTime5.ToString("yyyy-MM-dd HH:mm:ss"),
						"' and insert_time <'",
						dateTime6.ToString("yyyy-MM-dd HH:mm:ss"),
						"'"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text12 = Convert.ToString(dbDataReader.GetValue(1));
							if (text12 == null || text12.Length < 1 || text12.ToLower().Equals("null"))
							{
								text12 = "0";
							}
							string text13 = Convert.ToString(dbDataReader.GetValue(2));
							if (text13 == null || text13.Length < 1 || text13.ToLower().Equals("null"))
							{
								text13 = "0";
							}
							string text14 = Convert.ToString(dbDataReader.GetValue(3));
							if (text14 == null || text14.Length < 1 || text14.ToLower().Equals("null"))
							{
								text14 = "0";
							}
							string text15 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
							if (text15 != null && text15.Length >= 1 && !text15.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_hourly values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text12,
									",",
									text13,
									",",
									text14,
									",#",
									text15,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand3.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn2.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand4.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn3.Close();
					}
					catch
					{
					}
					dbCommand = conn.con.CreateCommand();
					dbCommand.CommandText = "update sys_para set para_value = '1.3.6.6' where para_name = 'DBVERSION' and para_type = 'String'";
					dbCommand.ExecuteNonQuery();
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					int result2 = 1;
					return result2;
				}
			}
			catch (Exception)
			{
				try
				{
					dbCommand3.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn2.Close();
				}
				catch
				{
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int V1366UP1370(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				string text = DateTime.Now.ToString("yyyyMMdd");
				DateTime dateTime = DateTime.Now.AddDays(1.0);
				DateTime dateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
				DateTime dateTime3 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				DBConn dBConn = null;
				DbCommand dbCommand2 = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand2 = dBConn.con.CreateCommand();
						dbCommand2.CommandText = "drop table rackthermal_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table rackthermal_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_hourly" + text + "` ( `rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_daily" + text + "` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_daily",
							text,
							" select * from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_hourly",
							text,
							" select * from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand2.Dispose();
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
						dbCommand = conn.con.CreateCommand();
						dbCommand.CommandText = "update sys_para set para_value = '1.3.7.0' where para_name = 'DBVERSION' and para_type = 'String'";
						dbCommand.ExecuteNonQuery();
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						int result2 = 1;
						return result2;
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand2.Dispose();
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
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
				}
				return result;
			}
			DateTime.Now.ToString("yyyyMMdd");
			DateTime dateTime4 = DateTime.Now.AddDays(1.0);
			DateTime dateTime5 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
			DateTime dateTime6 = new DateTime(dateTime4.Year, dateTime4.Month, dateTime4.Day, 0, 0, 0);
			DBConn dBConn2 = null;
			DbCommand dbCommand3 = null;
			DBConn dBConn3 = null;
			DbCommand dbCommand4 = null;
			try
			{
				dBConn2 = DBConnPool.getDynaConnection();
				dBConn3 = DBConnPool.getThermalConnection();
				if (dBConn2 != null && dBConn2.con != null && dBConn3 != null && dBConn3.con != null)
				{
					dbCommand4 = dBConn3.con.CreateCommand();
					dbCommand3 = dBConn2.con.CreateCommand();
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_daily where insert_time >= #",
						dateTime5.ToString("yyyy-MM-dd"),
						"# and insert_time < #",
						dateTime6.ToString("yyyy-MM-dd"),
						"#"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text2 = Convert.ToString(dbDataReader.GetValue(1));
							if (text2 == null || text2.Length < 1 || text2.ToLower().Equals("null"))
							{
								text2 = "0";
							}
							string text3 = Convert.ToString(dbDataReader.GetValue(2));
							if (text3 == null || text3.Length < 1 || text3.ToLower().Equals("null"))
							{
								text3 = "0";
							}
							string text4 = Convert.ToString(dbDataReader.GetValue(3));
							if (text4 == null || text4.Length < 1 || text4.ToLower().Equals("null"))
							{
								text4 = "0";
							}
							string text5 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd");
							if (text5 != null && text5.Length >= 1 && !text5.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_daily values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text2,
									",",
									text3,
									",",
									text4,
									",#",
									text5,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_hourly where insert_time >= '",
						dateTime5.ToString("yyyy-MM-dd HH:mm:ss"),
						"' and insert_time <'",
						dateTime6.ToString("yyyy-MM-dd HH:mm:ss"),
						"'"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text6 = Convert.ToString(dbDataReader.GetValue(1));
							if (text6 == null || text6.Length < 1 || text6.ToLower().Equals("null"))
							{
								text6 = "0";
							}
							string text7 = Convert.ToString(dbDataReader.GetValue(2));
							if (text7 == null || text7.Length < 1 || text7.ToLower().Equals("null"))
							{
								text7 = "0";
							}
							string text8 = Convert.ToString(dbDataReader.GetValue(3));
							if (text8 == null || text8.Length < 1 || text8.ToLower().Equals("null"))
							{
								text8 = "0";
							}
							string text9 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
							if (text9 != null && text9.Length >= 1 && !text9.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_hourly values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text6,
									",",
									text7,
									",",
									text8,
									",#",
									text9,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand3.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn2.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand4.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn3.Close();
					}
					catch
					{
					}
					dbCommand = conn.con.CreateCommand();
					dbCommand.CommandText = "update sys_para set para_value = '1.3.7.0' where para_name = 'DBVERSION' and para_type = 'String'";
					dbCommand.ExecuteNonQuery();
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					int result2 = 1;
					return result2;
				}
			}
			catch (Exception)
			{
				try
				{
					dbCommand3.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn2.Close();
				}
				catch
				{
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int V1399UP1401(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				string text = DateTime.Now.ToString("yyyyMMdd");
				DateTime dateTime = DateTime.Now.AddDays(1.0);
				DateTime dateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
				DateTime dateTime3 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				DBConn dBConn = null;
				DbCommand dbCommand2 = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand2 = dBConn.con.CreateCommand();
						dbCommand2.CommandText = "drop table rackthermal_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table rackthermal_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_hourly" + text + "` ( `rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_daily" + text + "` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_daily",
							text,
							" select * from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_hourly",
							text,
							" select * from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand2.Dispose();
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
						dbCommand = conn.con.CreateCommand();
						dbCommand.CommandText = "update sys_para set para_value = '1.4.0.1' where para_name = 'DBVERSION' and para_type = 'String'";
						dbCommand.ExecuteNonQuery();
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						int result2 = 1;
						return result2;
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand2.Dispose();
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
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
				}
				return result;
			}
			DateTime.Now.ToString("yyyyMMdd");
			DateTime dateTime4 = DateTime.Now.AddDays(1.0);
			DateTime dateTime5 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
			DateTime dateTime6 = new DateTime(dateTime4.Year, dateTime4.Month, dateTime4.Day, 0, 0, 0);
			DBConn dBConn2 = null;
			DbCommand dbCommand3 = null;
			DBConn dBConn3 = null;
			DbCommand dbCommand4 = null;
			try
			{
				dBConn2 = DBConnPool.getDynaConnection();
				dBConn3 = DBConnPool.getThermalConnection();
				if (dBConn2 != null && dBConn2.con != null && dBConn3 != null && dBConn3.con != null)
				{
					dbCommand4 = dBConn3.con.CreateCommand();
					dbCommand3 = dBConn2.con.CreateCommand();
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_daily where insert_time >= #",
						dateTime5.ToString("yyyy-MM-dd"),
						"# and insert_time < #",
						dateTime6.ToString("yyyy-MM-dd"),
						"#"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text2 = Convert.ToString(dbDataReader.GetValue(1));
							if (text2 == null || text2.Length < 1 || text2.ToLower().Equals("null"))
							{
								text2 = "0";
							}
							string text3 = Convert.ToString(dbDataReader.GetValue(2));
							if (text3 == null || text3.Length < 1 || text3.ToLower().Equals("null"))
							{
								text3 = "0";
							}
							string text4 = Convert.ToString(dbDataReader.GetValue(3));
							if (text4 == null || text4.Length < 1 || text4.ToLower().Equals("null"))
							{
								text4 = "0";
							}
							string text5 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd");
							if (text5 != null && text5.Length >= 1 && !text5.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_daily values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text2,
									",",
									text3,
									",",
									text4,
									",#",
									text5,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_hourly where insert_time >= '",
						dateTime5.ToString("yyyy-MM-dd HH:mm:ss"),
						"' and insert_time <'",
						dateTime6.ToString("yyyy-MM-dd HH:mm:ss"),
						"'"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text6 = Convert.ToString(dbDataReader.GetValue(1));
							if (text6 == null || text6.Length < 1 || text6.ToLower().Equals("null"))
							{
								text6 = "0";
							}
							string text7 = Convert.ToString(dbDataReader.GetValue(2));
							if (text7 == null || text7.Length < 1 || text7.ToLower().Equals("null"))
							{
								text7 = "0";
							}
							string text8 = Convert.ToString(dbDataReader.GetValue(3));
							if (text8 == null || text8.Length < 1 || text8.ToLower().Equals("null"))
							{
								text8 = "0";
							}
							string text9 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
							if (text9 != null && text9.Length >= 1 && !text9.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_hourly values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text6,
									",",
									text7,
									",",
									text8,
									",#",
									text9,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand3.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn2.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand4.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn3.Close();
					}
					catch
					{
					}
					dbCommand = conn.con.CreateCommand();
					dbCommand.CommandText = "update sys_para set para_value = '1.4.0.1' where para_name = 'DBVERSION' and para_type = 'String'";
					dbCommand.ExecuteNonQuery();
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					int result2 = 1;
					return result2;
				}
			}
			catch (Exception)
			{
				try
				{
					dbCommand3.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn2.Close();
				}
				catch
				{
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UP2V1370(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				string text = DateTime.Now.ToString("yyyyMMdd");
				DateTime dateTime = DateTime.Now.AddDays(1.0);
				DateTime dateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
				DateTime dateTime3 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				DBConn dBConn = null;
				DbCommand dbCommand2 = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand2 = dBConn.con.CreateCommand();
						dbCommand2.CommandText = "drop table device_data_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table bank_data_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table port_data_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table device_data_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table bank_data_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table port_data_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table rackthermal_hourly" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "drop table rackthermal_daily" + text;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text2 = "CREATE TABLE `bank_data_daily" + text + "` (";
						text2 += "`bank_id` int(11) NOT NULL,";
						text2 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text2 += "`insert_time` date NOT NULL,";
						text2 += "KEY `index_bankpdd` (`bank_id`,`insert_time`),  KEY `bdd_idx1` (`bank_id`),  KEY `bdd_idx2` (`insert_time`)";
						text2 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text2;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text3 = "CREATE TABLE `bank_data_hourly" + text + "` (";
						text3 += "`bank_id` int(11) NOT NULL,";
						text3 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text3 += "`insert_time` datetime NOT NULL,";
						text3 += "KEY `index_bankpdh` (`bank_id`,`insert_time`),  KEY `bdh_idx1` (`bank_id`),  KEY `bdh_idx2` (`insert_time`)";
						text3 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text3;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text4 = "CREATE TABLE `device_data_daily" + text + "` (";
						text4 += "`device_id` int(11) NOT NULL,";
						text4 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text4 += "`insert_time` date NOT NULL,";
						text4 += "KEY `index_dev_daily` (`device_id`,`insert_time`),  KEY `ddd_idx1` (`device_id`),  KEY `ddd_idx2` (`insert_time`)";
						text4 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text4;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text5 = "CREATE TABLE `device_data_hourly" + text + "` (";
						text5 += "`device_id` int(11) NOT NULL,";
						text5 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text5 += "`insert_time` datetime NOT NULL,";
						text5 += "KEY `index_devicepdh` (`device_id`,`insert_time`),  KEY `ddh_idx1` (`device_id`),  KEY `ddh_idx2` (`insert_time`)";
						text5 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text5;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text6 = "CREATE TABLE `port_data_daily" + text + "` (";
						text6 += "`port_id` int(11) NOT NULL,";
						text6 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text6 += "`insert_time` date NOT NULL,";
						text6 += "KEY `index_port_daily` (`port_id`,`insert_time`),  KEY `pdd_idx1` (`port_id`),  KEY `pdd_idx2` (`insert_time`)";
						text6 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text6;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						string text7 = "CREATE TABLE `port_data_hourly" + text + "` (";
						text7 += "`port_id` int(11) NOT NULL,";
						text7 += "`power_consumption` bigint(20) DEFAULT NULL,";
						text7 += "`insert_time` datetime NOT NULL,";
						text7 += "KEY `index_portpdh` (`port_id`,`insert_time`), KEY `pdh_idx1` (`port_id`),  KEY `pdh_idx2` (`insert_time`)";
						text7 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
						dbCommand2.CommandText = text7;
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_hourly" + text + "` ( `rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = "CREATE TABLE `rackthermal_daily" + text + "` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_daily",
							text,
							" select * from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into rackthermal_hourly",
							text,
							" select * from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into device_data_daily",
							text,
							" select * from device_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into device_data_hourly",
							text,
							" select * from device_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into bank_data_daily",
							text,
							" select * from bank_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into bank_data_hourly",
							text,
							" select * from bank_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into port_data_daily",
							text,
							" select * from port_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"insert into port_data_hourly",
							text,
							" select * from port_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from rackthermal_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from device_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from device_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from bank_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from bank_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from port_data_daily where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						dbCommand2.CommandText = string.Concat(new string[]
						{
							"delete from port_data_hourly where insert_time >= '",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						try
						{
							dbCommand2.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand2.Dispose();
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
						dbCommand = conn.con.CreateCommand();
						dbCommand.CommandText = "update sys_para set para_value = '1.3.7.0' where para_name = 'DBVERSION' and para_type = 'String'";
						dbCommand.ExecuteNonQuery();
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						int result2 = 1;
						return result2;
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand2.Dispose();
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
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
				}
				return result;
			}
			DateTime.Now.ToString("yyyyMMdd");
			DateTime dateTime4 = DateTime.Now.AddDays(1.0);
			DateTime dateTime5 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
			DateTime dateTime6 = new DateTime(dateTime4.Year, dateTime4.Month, dateTime4.Day, 0, 0, 0);
			DBConn dBConn2 = null;
			DbCommand dbCommand3 = null;
			DBConn dBConn3 = null;
			DbCommand dbCommand4 = null;
			try
			{
				dBConn2 = DBConnPool.getDynaConnection();
				dBConn3 = DBConnPool.getThermalConnection();
				if (dBConn2 != null && dBConn2.con != null && dBConn3 != null && dBConn3.con != null)
				{
					dbCommand4 = dBConn3.con.CreateCommand();
					dbCommand3 = dBConn2.con.CreateCommand();
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_daily where insert_time >= #",
						dateTime5.ToString("yyyy-MM-dd"),
						"# and insert_time < #",
						dateTime6.ToString("yyyy-MM-dd"),
						"#"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text8 = Convert.ToString(dbDataReader.GetValue(1));
							if (text8 == null || text8.Length < 1 || text8.ToLower().Equals("null"))
							{
								text8 = "0";
							}
							string text9 = Convert.ToString(dbDataReader.GetValue(2));
							if (text9 == null || text9.Length < 1 || text9.ToLower().Equals("null"))
							{
								text9 = "0";
							}
							string text10 = Convert.ToString(dbDataReader.GetValue(3));
							if (text10 == null || text10.Length < 1 || text10.ToLower().Equals("null"))
							{
								text10 = "0";
							}
							string text11 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd");
							if (text11 != null && text11.Length >= 1 && !text11.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_daily values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text8,
									",",
									text9,
									",",
									text10,
									",#",
									text11,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					dbCommand4.CommandText = string.Concat(new string[]
					{
						"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from rackthermal_hourly where insert_time >= '",
						dateTime5.ToString("yyyy-MM-dd HH:mm:ss"),
						"' and insert_time <'",
						dateTime6.ToString("yyyy-MM-dd HH:mm:ss"),
						"'"
					});
					try
					{
						DbDataReader dbDataReader = dbCommand4.ExecuteReader();
						while (dbDataReader.Read())
						{
							string text12 = Convert.ToString(dbDataReader.GetValue(1));
							if (text12 == null || text12.Length < 1 || text12.ToLower().Equals("null"))
							{
								text12 = "0";
							}
							string text13 = Convert.ToString(dbDataReader.GetValue(2));
							if (text13 == null || text13.Length < 1 || text13.ToLower().Equals("null"))
							{
								text13 = "0";
							}
							string text14 = Convert.ToString(dbDataReader.GetValue(3));
							if (text14 == null || text14.Length < 1 || text14.ToLower().Equals("null"))
							{
								text14 = "0";
							}
							string text15 = dbDataReader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
							if (text15 != null && text15.Length >= 1 && !text15.ToLower().Equals("null"))
							{
								dbCommand3.CommandText = string.Concat(new string[]
								{
									"insert into rackthermal_hourly values (",
									Convert.ToString(dbDataReader.GetValue(0)),
									",",
									text12,
									",",
									text13,
									",",
									text14,
									",#",
									text15,
									"# )"
								});
								dbCommand3.ExecuteNonQuery();
							}
						}
						dbDataReader.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand3.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn2.Close();
					}
					catch
					{
					}
					try
					{
						dbCommand4.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn3.Close();
					}
					catch
					{
					}
					dbCommand = conn.con.CreateCommand();
					dbCommand.CommandText = "update sys_para set para_value = '1.3.7.0' where para_name = 'DBVERSION' and para_type = 'String'";
					dbCommand.ExecuteNonQuery();
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					int result2 = 1;
					return result2;
				}
			}
			catch (Exception)
			{
				try
				{
					dbCommand3.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn2.Close();
				}
				catch
				{
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int V1370UP1403()
		{
			bool flag = false;
			DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Start Split Table ");
			int result = -1;
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn == null || dBConn.con == null)
				{
					int result2 = -1;
					return result2;
				}
			}
			catch
			{
				int result2 = -1;
				return result2;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				Hashtable hashtable = new Hashtable();
				Hashtable hashtable2 = new Hashtable();
				Hashtable hashtable3 = new Hashtable();
				Hashtable hashtable4 = new Hashtable();
				Hashtable hashtable5 = new Hashtable();
				Hashtable hashtable6 = new Hashtable();
				Hashtable hashtable7 = new Hashtable();
				Hashtable hashtable8 = new Hashtable();
				DBConn dBConn2 = null;
				DbCommand dbCommand2 = null;
				DataTable dataTable = new DataTable();
				try
				{
					dBConn2 = DBConnPool.getDynaConnection();
					if (dBConn2 != null && dBConn2.con != null)
					{
						dbCommand2 = dBConn2.con.CreateCommand();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from bank_data_daily  order by insert_time asc";
						DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable3 = new Hashtable();
							foreach (DataRow dataRow in dataTable.Rows)
							{
								string text = Convert.ToString(dataRow[0]);
								int year = Convert.ToInt32(text.Substring(0, 4));
								int month = Convert.ToInt32(text.Substring(4, 2));
								int day = Convert.ToInt32(text.Substring(6, 2));
								DateTime dateTime = new DateTime(year, month, day, 0, 0, 0);
								if (hashtable3.ContainsKey(text))
								{
									hashtable3[text] = dateTime;
								}
								else
								{
									hashtable3.Add(text, dateTime);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from bank_data_hourly  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable4 = new Hashtable();
							foreach (DataRow dataRow2 in dataTable.Rows)
							{
								string text2 = Convert.ToString(dataRow2[0]);
								int year2 = Convert.ToInt32(text2.Substring(0, 4));
								int month2 = Convert.ToInt32(text2.Substring(4, 2));
								int day2 = Convert.ToInt32(text2.Substring(6, 2));
								DateTime dateTime2 = new DateTime(year2, month2, day2, 0, 0, 0);
								if (hashtable4.ContainsKey(text2))
								{
									hashtable4[text2] = dateTime2;
								}
								else
								{
									hashtable4.Add(text2, dateTime2);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from device_data_daily  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable = new Hashtable();
							foreach (DataRow dataRow3 in dataTable.Rows)
							{
								string text3 = Convert.ToString(dataRow3[0]);
								int year3 = Convert.ToInt32(text3.Substring(0, 4));
								int month3 = Convert.ToInt32(text3.Substring(4, 2));
								int day3 = Convert.ToInt32(text3.Substring(6, 2));
								DateTime dateTime3 = new DateTime(year3, month3, day3, 0, 0, 0);
								if (hashtable.ContainsKey(text3))
								{
									hashtable[text3] = dateTime3;
								}
								else
								{
									hashtable.Add(text3, dateTime3);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from device_data_hourly  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable2 = new Hashtable();
							foreach (DataRow dataRow4 in dataTable.Rows)
							{
								string text4 = Convert.ToString(dataRow4[0]);
								int year4 = Convert.ToInt32(text4.Substring(0, 4));
								int month4 = Convert.ToInt32(text4.Substring(4, 2));
								int day4 = Convert.ToInt32(text4.Substring(6, 2));
								DateTime dateTime4 = new DateTime(year4, month4, day4, 0, 0, 0);
								if (hashtable2.ContainsKey(text4))
								{
									hashtable2[text4] = dateTime4;
								}
								else
								{
									hashtable2.Add(text4, dateTime4);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from port_data_daily  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable6 = new Hashtable();
							foreach (DataRow dataRow5 in dataTable.Rows)
							{
								string text5 = Convert.ToString(dataRow5[0]);
								int year5 = Convert.ToInt32(text5.Substring(0, 4));
								int month5 = Convert.ToInt32(text5.Substring(4, 2));
								int day5 = Convert.ToInt32(text5.Substring(6, 2));
								DateTime dateTime5 = new DateTime(year5, month5, day5, 0, 0, 0);
								if (hashtable6.ContainsKey(text5))
								{
									hashtable6[text5] = dateTime5;
								}
								else
								{
									hashtable6.Add(text5, dateTime5);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from port_data_hourly  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable5 = new Hashtable();
							foreach (DataRow dataRow6 in dataTable.Rows)
							{
								string text6 = Convert.ToString(dataRow6[0]);
								int year6 = Convert.ToInt32(text6.Substring(0, 4));
								int month6 = Convert.ToInt32(text6.Substring(4, 2));
								int day6 = Convert.ToInt32(text6.Substring(6, 2));
								DateTime dateTime6 = new DateTime(year6, month6, day6, 0, 0, 0);
								if (hashtable5.ContainsKey(text6))
								{
									hashtable5[text6] = dateTime6;
								}
								else
								{
									hashtable5.Add(text6, dateTime6);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from rackthermal_daily  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable7 = new Hashtable();
							foreach (DataRow dataRow7 in dataTable.Rows)
							{
								string text7 = Convert.ToString(dataRow7[0]);
								int year7 = Convert.ToInt32(text7.Substring(0, 4));
								int month7 = Convert.ToInt32(text7.Substring(4, 2));
								int day7 = Convert.ToInt32(text7.Substring(6, 2));
								DateTime dateTime7 = new DateTime(year7, month7, day7, 0, 0, 0);
								if (hashtable7.ContainsKey(text7))
								{
									hashtable7[text7] = dateTime7;
								}
								else
								{
									hashtable7.Add(text7, dateTime7);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from rackthermal_hourly  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable8 = new Hashtable();
							foreach (DataRow dataRow8 in dataTable.Rows)
							{
								string text8 = Convert.ToString(dataRow8[0]);
								int year8 = Convert.ToInt32(text8.Substring(0, 4));
								int month8 = Convert.ToInt32(text8.Substring(4, 2));
								int day8 = Convert.ToInt32(text8.Substring(6, 2));
								DateTime dateTime8 = new DateTime(year8, month8, day8, 0, 0, 0);
								if (hashtable8.ContainsKey(text8))
								{
									hashtable8[text8] = dateTime8;
								}
								else
								{
									hashtable8.Add(text8, dateTime8);
								}
							}
						}
						dataTable = new DataTable();
						int result2;
						if (AccessDBUpdate.SplitOneTable(hashtable, "device_data_daily", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable2, "device_data_hourly", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable6, "port_data_daily", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable5, "port_data_hourly", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable3, "bank_data_daily", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable4, "bank_data_hourly", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable8, "rackthermal_hourly", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable7, "rackthermal_daily", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = "update sys_para set para_value = '1.4.0.3' where para_name = 'DBVERSION' and para_type = 'String'";
						dbCommand.ExecuteNonQuery();
						DBTools.CURRENT_VERSION = "1.4.0.3";
						DBMaintain.SetConvertOldDataStatus(true);
						DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Success Split Table 1370-1403");
						result2 = 1;
						return result2;
					}
				}
				catch (Exception ex)
				{
					flag = true;
					DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Split Table Exception 1370-1403 : " + ex.Message);
				}
				finally
				{
					try
					{
						dbCommand2.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn2.Close();
					}
					catch
					{
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					if (flag)
					{
						DBUtil.StopService();
					}
				}
				return result;
			}
			Hashtable hashtable9 = new Hashtable();
			Hashtable hashtable10 = new Hashtable();
			DBConn dBConn3 = null;
			DbCommand dbCommand3 = null;
			DBConn dBConn4 = null;
			DbCommand dbCommand4 = null;
			DataTable dataTable2 = new DataTable();
			try
			{
				dBConn4 = DBConnPool.getThermalConnection();
				if (dBConn4 != null && dBConn4.con != null)
				{
					dbCommand4 = dBConn4.con.CreateCommand();
					dbCommand4.CommandText = "select  distinct (FORMAT(insert_time, 'yyyyMMdd'))   from rackthermal_daily ";
					DbDataAdapter dataAdapter2 = DBConn.GetDataAdapter(dBConn4.con);
					dataAdapter2.SelectCommand = dbCommand4;
					dataAdapter2.Fill(dataTable2);
					dataAdapter2.Dispose();
					if (dataTable2 != null)
					{
						hashtable9 = new Hashtable();
						foreach (DataRow dataRow9 in dataTable2.Rows)
						{
							string text9 = Convert.ToString(dataRow9[0]);
							int year9 = Convert.ToInt32(text9.Substring(0, 4));
							int month9 = Convert.ToInt32(text9.Substring(4, 2));
							int day9 = Convert.ToInt32(text9.Substring(6, 2));
							DateTime dateTime9 = new DateTime(year9, month9, day9, 0, 0, 0);
							if (hashtable9.ContainsKey(text9))
							{
								hashtable9[text9] = dateTime9;
							}
							else
							{
								hashtable9.Add(text9, dateTime9);
							}
						}
					}
					dataTable2 = new DataTable();
					dbCommand4.CommandText = "select  distinct (FORMAT(insert_time, 'yyyyMMdd'))  from rackthermal_hourly";
					dataAdapter2 = DBConn.GetDataAdapter(dBConn4.con);
					dataAdapter2.SelectCommand = dbCommand4;
					dataAdapter2.Fill(dataTable2);
					dataAdapter2.Dispose();
					if (dataTable2 != null)
					{
						hashtable10 = new Hashtable();
						foreach (DataRow dataRow10 in dataTable2.Rows)
						{
							string text10 = Convert.ToString(dataRow10[0]);
							int year10 = Convert.ToInt32(text10.Substring(0, 4));
							int month10 = Convert.ToInt32(text10.Substring(4, 2));
							int day10 = Convert.ToInt32(text10.Substring(6, 2));
							DateTime dateTime10 = new DateTime(year10, month10, day10, 0, 0, 0);
							if (hashtable10.ContainsKey(text10))
							{
								hashtable10[text10] = dateTime10;
							}
							else
							{
								hashtable10.Add(text10, dateTime10);
							}
						}
					}
					dataTable2 = new DataTable();
					int result2;
					if (AccessDBUpdate.SplitRackThermalTable(hashtable10, "rackthermal_hourly", dbCommand4) < 0)
					{
						flag = true;
						result2 = -1;
						return result2;
					}
					if (AccessDBUpdate.SplitRackThermalTable(hashtable9, "rackthermal_daily", dbCommand4) < 0)
					{
						flag = true;
						result2 = -1;
						return result2;
					}
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "update sys_para set para_value = '1.4.0.3' where para_name = 'DBVERSION' and para_type = 'String'";
					dbCommand.ExecuteNonQuery();
					DBTools.CURRENT_VERSION = "1.4.0.3";
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
					DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Success Split Table 1370-1403");
					DBMaintain.SetConvertOldDataStatus(true);
					result2 = 1;
					return result2;
				}
			}
			catch (Exception ex2)
			{
				flag = true;
				DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Split Table Exception 1370-1403 : " + ex2.Message);
			}
			finally
			{
				try
				{
					dbCommand3.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn3.Close();
				}
				catch
				{
				}
				try
				{
					dbCommand4.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn4.Close();
				}
				catch
				{
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
				if (flag)
				{
					DBUtil.StopService();
				}
			}
			return result;
		}
		public static int V1401UP1403()
		{
			bool flag = false;
			DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Start Split Table 1401-1403");
			int result = -1;
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn == null || dBConn.con == null)
				{
					int result2 = -1;
					return result2;
				}
			}
			catch
			{
				int result2 = -1;
				return result2;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				new Hashtable();
				new Hashtable();
				new Hashtable();
				new Hashtable();
				new Hashtable();
				new Hashtable();
				Hashtable hashtable = new Hashtable();
				Hashtable hashtable2 = new Hashtable();
				DBConn dBConn2 = null;
				DbCommand dbCommand2 = null;
				DataTable dataTable = new DataTable();
				try
				{
					dBConn2 = DBConnPool.getDynaConnection();
					if (dBConn2 != null && dBConn2.con != null)
					{
						dbCommand2 = dBConn2.con.CreateCommand();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from rackthermal_daily  order by insert_time asc";
						DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable = new Hashtable();
							foreach (DataRow dataRow in dataTable.Rows)
							{
								string text = Convert.ToString(dataRow[0]);
								int year = Convert.ToInt32(text.Substring(0, 4));
								int month = Convert.ToInt32(text.Substring(4, 2));
								int day = Convert.ToInt32(text.Substring(6, 2));
								DateTime dateTime = new DateTime(year, month, day, 0, 0, 0);
								if (hashtable.ContainsKey(text))
								{
									hashtable[text] = dateTime;
								}
								else
								{
									hashtable.Add(text, dateTime);
								}
							}
						}
						dataTable = new DataTable();
						dbCommand2.CommandText = "select  distinct (DATE_FORMAT(insert_time,'%Y%m%d'))  from rackthermal_hourly  order by insert_time asc";
						dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
						dataAdapter.SelectCommand = dbCommand2;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							hashtable2 = new Hashtable();
							foreach (DataRow dataRow2 in dataTable.Rows)
							{
								string text2 = Convert.ToString(dataRow2[0]);
								int year2 = Convert.ToInt32(text2.Substring(0, 4));
								int month2 = Convert.ToInt32(text2.Substring(4, 2));
								int day2 = Convert.ToInt32(text2.Substring(6, 2));
								DateTime dateTime2 = new DateTime(year2, month2, day2, 0, 0, 0);
								if (hashtable2.ContainsKey(text2))
								{
									hashtable2[text2] = dateTime2;
								}
								else
								{
									hashtable2.Add(text2, dateTime2);
								}
							}
						}
						dataTable = new DataTable();
						int result2;
						if (AccessDBUpdate.SplitOneTable(hashtable2, "rackthermal_hourly", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						if (AccessDBUpdate.SplitOneTable(hashtable, "rackthermal_daily", dbCommand2) < 0)
						{
							flag = true;
							result2 = -1;
							return result2;
						}
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = "update sys_para set para_value = '1.4.0.3' where para_name = 'DBVERSION' and para_type = 'String'";
						dbCommand.ExecuteNonQuery();
						DBTools.CURRENT_VERSION = "1.4.0.3";
						DBMaintain.SetConvertOldDataStatus(true);
						DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Success Split Table 1401-1403");
						result2 = 1;
						return result2;
					}
				}
				catch (Exception ex)
				{
					flag = true;
					DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Split Table Exception 1401-1403 : " + ex.Message);
				}
				finally
				{
					try
					{
						dbCommand2.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn2.Close();
					}
					catch
					{
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					if (flag)
					{
						DBUtil.StopService();
					}
				}
				return result;
			}
			Hashtable hashtable3 = new Hashtable();
			Hashtable hashtable4 = new Hashtable();
			DBConn dBConn3 = null;
			DbCommand dbCommand3 = null;
			DBConn dBConn4 = null;
			DbCommand dbCommand4 = null;
			DataTable dataTable2 = new DataTable();
			try
			{
				dBConn4 = DBConnPool.getThermalConnection();
				if (dBConn4 != null && dBConn4.con != null)
				{
					dbCommand4 = dBConn4.con.CreateCommand();
					dbCommand4.CommandText = "select  distinct (FORMAT(insert_time, 'yyyyMMdd'))   from rackthermal_daily ";
					DbDataAdapter dataAdapter2 = DBConn.GetDataAdapter(dBConn4.con);
					dataAdapter2.SelectCommand = dbCommand4;
					dataAdapter2.Fill(dataTable2);
					dataAdapter2.Dispose();
					if (dataTable2 != null)
					{
						hashtable3 = new Hashtable();
						foreach (DataRow dataRow3 in dataTable2.Rows)
						{
							string text3 = Convert.ToString(dataRow3[0]);
							int year3 = Convert.ToInt32(text3.Substring(0, 4));
							int month3 = Convert.ToInt32(text3.Substring(4, 2));
							int day3 = Convert.ToInt32(text3.Substring(6, 2));
							DateTime dateTime3 = new DateTime(year3, month3, day3, 0, 0, 0);
							if (hashtable3.ContainsKey(text3))
							{
								hashtable3[text3] = dateTime3;
							}
							else
							{
								hashtable3.Add(text3, dateTime3);
							}
						}
					}
					dataTable2 = new DataTable();
					dbCommand4.CommandText = "select  distinct (FORMAT(insert_time, 'yyyyMMdd'))  from rackthermal_hourly";
					dataAdapter2 = DBConn.GetDataAdapter(dBConn4.con);
					dataAdapter2.SelectCommand = dbCommand4;
					dataAdapter2.Fill(dataTable2);
					dataAdapter2.Dispose();
					if (dataTable2 != null)
					{
						hashtable4 = new Hashtable();
						foreach (DataRow dataRow4 in dataTable2.Rows)
						{
							string text4 = Convert.ToString(dataRow4[0]);
							int year4 = Convert.ToInt32(text4.Substring(0, 4));
							int month4 = Convert.ToInt32(text4.Substring(4, 2));
							int day4 = Convert.ToInt32(text4.Substring(6, 2));
							DateTime dateTime4 = new DateTime(year4, month4, day4, 0, 0, 0);
							if (hashtable4.ContainsKey(text4))
							{
								hashtable4[text4] = dateTime4;
							}
							else
							{
								hashtable4.Add(text4, dateTime4);
							}
						}
					}
					dataTable2 = new DataTable();
					int result2;
					if (AccessDBUpdate.SplitRackThermalTable(hashtable4, "rackthermal_hourly", dbCommand4) < 0)
					{
						flag = true;
						result2 = -1;
						return result2;
					}
					if (AccessDBUpdate.SplitRackThermalTable(hashtable3, "rackthermal_daily", dbCommand4) < 0)
					{
						flag = true;
						result2 = -1;
						return result2;
					}
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "update sys_para set para_value = '1.4.0.3' where para_name = 'DBVERSION' and para_type = 'String'";
					dbCommand.ExecuteNonQuery();
					DBTools.CURRENT_VERSION = "1.4.0.3";
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
					DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Success Split Table 1401-1403");
					DBMaintain.SetConvertOldDataStatus(true);
					result2 = 1;
					return result2;
				}
			}
			catch (Exception ex2)
			{
				flag = true;
				DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW Split Table Exception 1401-1403 : " + ex2.Message);
			}
			finally
			{
				try
				{
					dbCommand3.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn3.Close();
				}
				catch
				{
				}
				try
				{
					dbCommand4.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn4.Close();
				}
				catch
				{
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
				if (flag)
				{
					DBUtil.StopService();
				}
			}
			return result;
		}
		public static int V1403UP1405(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "ALTER TABLE device_base_info drop COLUMN b_priority";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN b_priority varchar(255) Default '' ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "ALTER TABLE device_base_info drop COLUMN reference_voltage ";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN reference_voltage single Default -500 ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.4.0.5' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				DBTools.CURRENT_VERSION = "1.4.0.5";
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.4.0.5 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int V1405UP1406(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "DROP TABLE line_info ";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "create table line_info ( id AUTOINCREMENT,device_id int not null,line_name varchar(64),line_number int,max_voltage single,min_voltage single,max_power single,min_power single,max_current single,min_current single,PRIMARY KEY (id) )";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "update sys_para set para_value = '1.4.0.6' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				DBTools.CURRENT_VERSION = "1.4.0.6";
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.4.0.5 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int V1406UP1407(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "DROP TABLE udp ";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "create table udp ( uid int not null,did int not null,status int,PRIMARY KEY (uid,did) )";
				dbCommand.ExecuteNonQuery();
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
				dbCommand.CommandText = "select id from device_base_info ";
				oleDbDataAdapter.SelectCommand = (OleDbCommand)dbCommand;
				DataTable dataTable = new DataTable();
				Hashtable hashtable = new Hashtable();
				new Hashtable();
				oleDbDataAdapter.Fill(dataTable);
				foreach (DataRow dataRow in dataTable.Rows)
				{
					int num = Convert.ToInt32(dataRow[0]);
					if (!hashtable.ContainsKey(num))
					{
						hashtable.Add(num, num);
					}
				}
				dataTable = new DataTable();
				oleDbDataAdapter.Dispose();
				oleDbDataAdapter = new OleDbDataAdapter();
				dbCommand.CommandText = "select id,devices from sys_users ";
				oleDbDataAdapter.SelectCommand = (OleDbCommand)dbCommand;
				oleDbDataAdapter.Fill(dataTable);
				List<string> list = new List<string>();
				foreach (DataRow dataRow2 in dataTable.Rows)
				{
					int num2 = Convert.ToInt32(dataRow2[0]);
					string text = (string)dataRow2[1];
					if (text != null && text.Length > 0)
					{
						string[] array = text.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string value = array2[i];
							int num3 = Convert.ToInt32(value);
							if (hashtable.ContainsKey(num3))
							{
								list.Add(string.Concat(new object[]
								{
									"(",
									num2,
									",",
									num3,
									")"
								}));
							}
						}
					}
				}
				dataTable = new DataTable();
				oleDbDataAdapter.Dispose();
				if (list != null && list.Count > 0)
				{
					foreach (string current in list)
					{
						dbCommand.CommandText = "insert into udp (uid,did) values " + current;
						dbCommand.ExecuteNonQuery();
					}
				}
				dbCommand.CommandText = "update sys_para set para_value = '1.4.0.7' where para_name = 'DBVERSION' and para_type = 'String'";
				dbCommand.ExecuteNonQuery();
				DBTools.CURRENT_VERSION = "1.4.0.7";
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to 1.4.0.5 Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UPSYSDBSTRUCTURE()
		{
			Hashtable hashtable = new Hashtable();
			string text = AppDomain.CurrentDomain.BaseDirectory;
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			string str = text + "sysdb.mdb";
			OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str + ";Jet OLEDB:Database Password=^tenec0Sensor");
			oleDbConnection.Open();
			if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
			{
				try
				{
					DataTable dataTable = new DataTable();
					dataTable = oleDbConnection.GetSchema("Columns");
					int arg_91_0 = dataTable.Columns.Count;
					foreach (DataRow dataRow in dataTable.Rows)
					{
						Hashtable hashtable2 = new Hashtable();
						string text2 = Convert.ToString(dataRow["TABLE_NAME"]);
						string key = Convert.ToString(dataRow["COLUMN_NAME"]);
						if (hashtable.ContainsKey(text2))
						{
							hashtable2 = (Hashtable)hashtable[text2];
							if (!hashtable2.ContainsKey(key))
							{
								hashtable2.Add(key, text2);
								hashtable[text2] = hashtable2;
							}
						}
						else
						{
							if (!hashtable2.ContainsKey(key))
							{
								hashtable2.Add(key, text2);
								hashtable.Add(text2, hashtable2);
							}
						}
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile(ex.Message);
				}
				OleDbCommand oleDbCommand = oleDbConnection.CreateCommand();
				if (hashtable.ContainsKey("port_info"))
				{
					Hashtable hashtable3 = (Hashtable)hashtable["port_info"];
					if (!hashtable3.ContainsKey("mac"))
					{
						try
						{
							oleDbCommand.CommandText = "ALTER TABLE port_info ADD COLUMN mac TEXT(120)";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
							oleDbCommand.Dispose();
							oleDbConnection.Close();
							int result = -1;
							return result;
						}
					}
					try
					{
						oleDbCommand.CommandText = "ALTER TABLE port_info ALTER COLUMN device_id long ";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				if (!hashtable.ContainsKey("event_info"))
				{
					try
					{
						oleDbCommand.CommandText = "create table event_info (eventid varchar(10),logflag byte,mailflag byte,reserve integer,PRIMARY KEY(eventid))";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (hashtable.ContainsKey("bank_info"))
				{
					try
					{
						oleDbCommand.CommandText = "ALTER TABLE bank_info ALTER COLUMN device_id long ";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				if (hashtable.ContainsKey("device_base_info"))
				{
					Hashtable hashtable4 = (Hashtable)hashtable["device_base_info"];
					if (!hashtable4.ContainsKey("pop_flag"))
					{
						try
						{
							oleDbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN pop_flag int null";
							oleDbCommand.ExecuteNonQuery();
							oleDbCommand.CommandText = "update device_base_info set pop_flag = 1 ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
							oleDbCommand.Dispose();
							oleDbConnection.Close();
							int result = -1;
							return result;
						}
					}
					if (!hashtable4.ContainsKey("pop_threshold"))
					{
						try
						{
							oleDbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN pop_threshold real null";
							oleDbCommand.ExecuteNonQuery();
							oleDbCommand.CommandText = "update device_base_info set pop_threshold = -1 ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
							oleDbCommand.Dispose();
							oleDbConnection.Close();
							int result = -1;
							return result;
						}
					}
					if (!hashtable4.ContainsKey("door"))
					{
						try
						{
							oleDbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN door int null";
							oleDbCommand.ExecuteNonQuery();
							oleDbCommand.CommandText = "update device_base_info set door = 0 ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
							oleDbCommand.Dispose();
							oleDbConnection.Close();
							int result = -1;
							return result;
						}
					}
					if (!hashtable4.ContainsKey("device_capacity"))
					{
						try
						{
							oleDbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN device_capacity real ";
							oleDbCommand.ExecuteNonQuery();
							oleDbCommand.CommandText = "update device_base_info set device_capacity = 0 ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
							oleDbCommand.Dispose();
							oleDbConnection.Close();
							int result = -1;
							return result;
						}
					}
					if (!hashtable4.ContainsKey("restoreflag"))
					{
						try
						{
							oleDbCommand.CommandText = "ALTER TABLE device_base_info ADD COLUMN restoreflag int ";
							oleDbCommand.ExecuteNonQuery();
							string text3 = AppDomain.CurrentDomain.BaseDirectory;
							if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
							{
								text3 += Path.DirectorySeparatorChar;
							}
							if (File.Exists(text3 + "restore.flag"))
							{
								oleDbCommand.CommandText = "update device_base_info set restoreflag = 1 ";
								oleDbCommand.ExecuteNonQuery();
								File.Delete(text3 + "restore.flag");
							}
							else
							{
								oleDbCommand.CommandText = "update device_base_info set restoreflag = 0 ";
								oleDbCommand.ExecuteNonQuery();
							}
						}
						catch (Exception)
						{
							oleDbCommand.Dispose();
							oleDbConnection.Close();
							int result = -1;
							return result;
						}
					}
				}
				if (!hashtable.ContainsKey("deviceflag"))
				{
					try
					{
						oleDbCommand.CommandText = "create table deviceflag (dev_fresh_flag integer not null,primary key(dev_fresh_flag))";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "insert into deviceflag (dev_fresh_flag) values (1)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (!hashtable.ContainsKey("portflag"))
				{
					try
					{
						oleDbCommand.CommandText = "create table portflag (port_fresh_flag integer not null,primary key(port_fresh_flag))";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "insert into portflag (port_fresh_flag) values (1)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (!hashtable.ContainsKey("gatewaytable"))
				{
					try
					{
						oleDbCommand.CommandText = "create table gatewaytable (gid varchar(128),bid varchar(128),sid varchar(128),slevel int,disname varchar(254),capacity real,eleflag int,distype varchar(64),location varchar(254),ip varchar(254),primary key(gid,bid,sid) )";
						oleDbCommand.ExecuteNonQuery();
						Sys_Para.SetResolution(0);
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (!hashtable.ContainsKey("gatewaylastpd"))
				{
					try
					{
						oleDbCommand.CommandText = "create table gatewaylastpd (bid varchar(128),sid varchar(128),eleflag int,pd double,timemark datetime,primary key(bid,sid) )";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (!hashtable.ContainsKey("currentpue"))
				{
					try
					{
						oleDbCommand.CommandText = "create table currentpue (curhour datetime,curday datetime,curweek datetime,ithourpue double,nonithourpue double,itdaypue double,nonitdaypue double,itweekpue double,nonitweekpue double,lasttime datetime )";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (!hashtable.ContainsKey("backuptask"))
				{
					try
					{
						oleDbCommand.CommandText = "create table backuptask (id autoincrement primary key,taskname varchar(128),tasktype int,storetype int,username varchar(255),pwd varchar(255),host varchar(255),port int,filepath varchar(255) )";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (hashtable.ContainsKey("data_group"))
				{
					Hashtable hashtable5 = (Hashtable)hashtable["data_group"];
					if (!hashtable5.ContainsKey("billflag"))
					{
						try
						{
							oleDbCommand.CommandText = "ALTER TABLE data_group ADD COLUMN billflag int null";
							oleDbCommand.ExecuteNonQuery();
							oleDbCommand.CommandText = "update data_group set billflag=0 ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
							oleDbCommand.Dispose();
							oleDbConnection.Close();
							int result = -1;
							return result;
						}
					}
				}
				if (!hashtable.ContainsKey("device_voltage"))
				{
					try
					{
						oleDbCommand.CommandText = "create TABLE device_voltage( id integer not null,voltage real,primary key(id) )";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				if (!hashtable.ContainsKey("devicedefine"))
				{
					try
					{
						oleDbCommand.CommandText = "create table devicedefine(model_nm varchar(255),dev_ver varchar(255),first_data memo,second_data memo,compatible int,reserve varchar(255),PRIMARY KEY(model_nm,dev_ver))";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Dispose();
						oleDbConnection.Close();
						int result = -1;
						return result;
					}
				}
				oleDbCommand.Dispose();
				oleDbConnection.Close();
				return 1;
			}
			return -1;
		}
		private static int SplitOneTable(Hashtable ht_datetime, string str_tname, DbCommand cmd)
		{
			bool flag = false;
			try
			{
				ICollection keys = ht_datetime.Keys;
				foreach (string text in keys)
				{
					DateTime dateTime = Convert.ToDateTime(ht_datetime[text]);
					DateTime dateTime2 = dateTime.AddDays(1.0);
					cmd.CommandText = "select count(*) from " + str_tname + text;
					object obj = null;
					try
					{
						obj = cmd.ExecuteScalar();
						flag = true;
					}
					catch (Exception)
					{
						flag = false;
					}
					if (flag)
					{
						long num = -2L;
						long num2 = -2L;
						if (obj != null && obj != DBNull.Value)
						{
							num = Convert.ToInt64(obj);
						}
						bool flag2;
						if (num >= 0L)
						{
							cmd.CommandText = string.Concat(new string[]
							{
								"select count(*) from ",
								str_tname,
								" where  insert_time >= '",
								dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
								"' and insert_time < '",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"'"
							});
							obj = cmd.ExecuteScalar();
							if (obj != null && obj != DBNull.Value)
							{
								num2 = Convert.ToInt64(obj);
							}
							flag2 = (num2 > num);
						}
						else
						{
							flag2 = true;
						}
						if (flag2)
						{
							cmd.CommandText = "drop table " + str_tname + text;
							try
							{
								cmd.ExecuteNonQuery();
							}
							catch
							{
							}
							string createSQL = AccessDBUpdate.getCreateSQL(str_tname, text);
							cmd.CommandText = createSQL;
							cmd.ExecuteNonQuery();
							cmd.CommandText = string.Concat(new string[]
							{
								"insert into ",
								str_tname,
								text,
								" select * from ",
								str_tname,
								" where insert_time >= '",
								dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
								"' and insert_time <'",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"'"
							});
							cmd.ExecuteNonQuery();
							cmd.CommandText = string.Concat(new string[]
							{
								"delete from ",
								str_tname,
								" where insert_time >= '",
								dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
								"' and insert_time <'",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"'"
							});
							cmd.ExecuteNonQuery();
						}
						else
						{
							cmd.CommandText = string.Concat(new string[]
							{
								"delete from ",
								str_tname,
								" where insert_time >= '",
								dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
								"' and insert_time <'",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"'"
							});
							cmd.ExecuteNonQuery();
						}
					}
					else
					{
						string createSQL2 = AccessDBUpdate.getCreateSQL(str_tname, text);
						cmd.CommandText = createSQL2;
						cmd.ExecuteNonQuery();
						cmd.CommandText = string.Concat(new string[]
						{
							"insert into ",
							str_tname,
							text,
							" select * from ",
							str_tname,
							" where insert_time >= '",
							dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						cmd.ExecuteNonQuery();
						cmd.CommandText = string.Concat(new string[]
						{
							"delete from ",
							str_tname,
							" where insert_time >= '",
							dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							"' and insert_time <'",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"'"
						});
						cmd.ExecuteNonQuery();
					}
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW  Split " + str_tname + "Table Exception : " + ex.Message);
			}
			return -1;
		}
		private static int SplitRackThermalTable(Hashtable ht_datetime, string str_tname, DbCommand cmd_s)
		{
			bool flag = false;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				ICollection keys = ht_datetime.Keys;
				foreach (string key in keys)
				{
					DateTime dt_inserttime = Convert.ToDateTime(ht_datetime[key]);
					DateTime dateTime = dt_inserttime.AddDays(1.0);
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = "select count(*) from " + str_tname;
						object obj = null;
						try
						{
							obj = dbCommand.ExecuteScalar();
							flag = true;
						}
						catch (Exception)
						{
							flag = false;
						}
						if (flag)
						{
							long num = -2L;
							long num2 = -2L;
							if (obj != null && obj != DBNull.Value)
							{
								num = Convert.ToInt64(obj);
							}
							bool flag2;
							if (num >= 0L)
							{
								cmd_s.CommandText = string.Concat(new string[]
								{
									"select count(*) from ",
									str_tname,
									" where  insert_time >= #",
									dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
									"# and insert_time < #",
									dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
									"#"
								});
								obj = cmd_s.ExecuteScalar();
								if (obj != null && obj != DBNull.Value)
								{
									num2 = Convert.ToInt64(obj);
								}
								flag2 = (num2 > num);
							}
							else
							{
								flag2 = true;
							}
							if (flag2)
							{
								dbCommand.CommandText = "delete from " + str_tname;
								try
								{
									dbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
								cmd_s.CommandText = string.Concat(new string[]
								{
									"select rack_id,intakepeak,exhaustpeak,differencepeak,insert_time from ",
									str_tname,
									" where insert_time >= #",
									dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
									"# and insert_time < #",
									dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
									"#"
								});
								try
								{
									DbDataReader dbDataReader = cmd_s.ExecuteReader();
									while (dbDataReader.Read())
									{
										string text = Convert.ToString(dbDataReader.GetValue(1));
										if (text == null || text.Length < 1 || text.ToLower().Equals("null"))
										{
											text = "0";
										}
										string text2 = Convert.ToString(dbDataReader.GetValue(2));
										if (text2 == null || text2.Length < 1 || text2.ToLower().Equals("null"))
										{
											text2 = "0";
										}
										string text3 = Convert.ToString(dbDataReader.GetValue(3));
										if (text3 == null || text3.Length < 1 || text3.ToLower().Equals("null"))
										{
											text3 = "0";
										}
										DateTime dateTime2 = dbDataReader.GetDateTime(4);
										string text4;
										if (str_tname.IndexOf("rackthermal_daily") >= 0)
										{
											text4 = dateTime2.ToString("yyyy-MM-dd");
										}
										else
										{
											text4 = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
										}
										if (text4 != null && text4.Length >= 1 && !text4.ToLower().Equals("null"))
										{
											dbCommand.CommandText = string.Concat(new string[]
											{
												"insert into ",
												str_tname,
												" values (",
												Convert.ToString(dbDataReader.GetValue(0)),
												",",
												text,
												",",
												text2,
												",",
												text3,
												",#",
												text4,
												"# )"
											});
											dbCommand.ExecuteNonQuery();
										}
									}
									dbDataReader.Close();
								}
								catch
								{
								}
								cmd_s.CommandText = string.Concat(new string[]
								{
									"delete from ",
									str_tname,
									" where insert_time >= #",
									dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
									"# and insert_time < #",
									dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
									"#"
								});
								cmd_s.ExecuteNonQuery();
							}
							else
							{
								cmd_s.CommandText = string.Concat(new string[]
								{
									"delete from ",
									str_tname,
									" where insert_time >= #",
									dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
									"# and insert_time < #",
									dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
									"#"
								});
								cmd_s.ExecuteNonQuery();
							}
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
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("WWWWWWWWWWWWWWW  Split " + str_tname + "Table Exception : " + ex.Message);
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
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
			}
			return -1;
		}
		private static string getCreateSQL(string stname, string str_timetail)
		{
			if (stname.IndexOf("rackthermal_daily") >= 0)
			{
				return "CREATE TABLE `rackthermal_daily" + str_timetail + "` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			if (stname.IndexOf("rackthermal_hourly") >= 0)
			{
				return "CREATE TABLE `rackthermal_hourly" + str_timetail + "` ( `rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			if (stname.IndexOf("bank_data_daily") >= 0)
			{
				string str = "CREATE TABLE `bank_data_daily" + str_timetail + "` (";
				str += "`bank_id` int(11) NOT NULL,";
				str += "`power_consumption` bigint(20) DEFAULT NULL,";
				str += "`insert_time` date NOT NULL,";
				str += "KEY `index_bankpdd` (`bank_id`,`insert_time`),  KEY `bdd_idx1` (`bank_id`),  KEY `bdd_idx2` (`insert_time`)";
				return str + ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			if (stname.IndexOf("bank_data_hourly") >= 0)
			{
				string str2 = "CREATE TABLE `bank_data_hourly" + str_timetail + "` (";
				str2 += "`bank_id` int(11) NOT NULL,";
				str2 += "`power_consumption` bigint(20) DEFAULT NULL,";
				str2 += "`insert_time` datetime NOT NULL,";
				str2 += "KEY `index_bankpdh` (`bank_id`,`insert_time`),  KEY `bdh_idx1` (`bank_id`),  KEY `bdh_idx2` (`insert_time`)";
				return str2 + ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			if (stname.IndexOf("device_data_daily") >= 0)
			{
				string str3 = "CREATE TABLE `device_data_daily" + str_timetail + "` (";
				str3 += "`device_id` int(11) NOT NULL,";
				str3 += "`power_consumption` bigint(20) DEFAULT NULL,";
				str3 += "`insert_time` date NOT NULL,";
				str3 += "KEY `index_dev_daily` (`device_id`,`insert_time`),  KEY `ddd_idx1` (`device_id`),  KEY `ddd_idx2` (`insert_time`)";
				return str3 + ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			if (stname.IndexOf("device_data_hourly") >= 0)
			{
				string str4 = "CREATE TABLE `device_data_hourly" + str_timetail + "` (";
				str4 += "`device_id` int(11) NOT NULL,";
				str4 += "`power_consumption` bigint(20) DEFAULT NULL,";
				str4 += "`insert_time` datetime NOT NULL,";
				str4 += "KEY `index_devicepdh` (`device_id`,`insert_time`),  KEY `ddh_idx1` (`device_id`),  KEY `ddh_idx2` (`insert_time`)";
				return str4 + ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			if (stname.IndexOf("port_data_daily") >= 0)
			{
				string str5 = "CREATE TABLE `port_data_daily" + str_timetail + "` (";
				str5 += "`port_id` int(11) NOT NULL,";
				str5 += "`power_consumption` bigint(20) DEFAULT NULL,";
				str5 += "`insert_time` date NOT NULL,";
				str5 += "KEY `index_port_daily` (`port_id`,`insert_time`),  KEY `pdd_idx1` (`port_id`),  KEY `pdd_idx2` (`insert_time`)";
				return str5 + ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			if (stname.IndexOf("port_data_hourly") >= 0)
			{
				string str6 = "CREATE TABLE `port_data_hourly" + str_timetail + "` (";
				str6 += "`port_id` int(11) NOT NULL,";
				str6 += "`power_consumption` bigint(20) DEFAULT NULL,";
				str6 += "`insert_time` datetime NOT NULL,";
				str6 += "KEY `index_portpdh` (`port_id`,`insert_time`), KEY `pdh_idx1` (`port_id`),  KEY `pdh_idx2` (`insert_time`)";
				return str6 + ") ENGINE=MyISAM DEFAULT CHARSET=utf8";
			}
			return "";
		}
		public static int UP2SupportPOP(DBConn conn)
		{
			int result = -1;
			DbCommand dbCommand = null;
			try
			{
				dbCommand = conn.con.CreateCommand();
				dbCommand.CommandText = "ALTER TABLE device_base_info add COLUMN outlet_pop  int DEFAULT 0";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "ALTER TABLE device_base_info add COLUMN pop_lifo  int DEFAULT 0";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				dbCommand.CommandText = "ALTER TABLE device_base_info add COLUMN pop_priority  int DEFAULT 0";
				try
				{
					dbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Update database to support pop Error : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return result;
		}
	}
}
