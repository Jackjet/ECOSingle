using CommonAPI;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Threading;
namespace DBAccessAPI
{
	public class DBWorkThread
	{
		public const int DB_MYSQL = 1;
		public const int DB_ACCESS = 0;
		public static bool NEEDLOG = true;
		public static bool STOP_THREAD = false;
		private string str_lasterr = "";
		private DateTime dt_lasterr;
		private DbConnection con;
		private int DBTYPE;
		private DbCommand cmd = new OleDbCommand();
		private DbTransaction dbt;
		private DebugCenter debug;
		public DBWorkThread(int i_type)
		{
			if (DBUrl.SERVERMODE)
			{
				string cONNECT_STRING = DBUrl.CONNECT_STRING;
				string[] array = cONNECT_STRING.Split(new string[]
				{
					","
				}, StringSplitOptions.RemoveEmptyEntries);
				this.con = new MySqlConnection(string.Concat(new string[]
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
					";Pooling=true;Min Pool Size=0;Max Pool Size=50;Default Command Timeout=0;charset=utf8;"
				}));
				this.con.Open();
			}
			else
			{
				this.DBTYPE = i_type;
				switch (this.DBTYPE)
				{
				case 0:
					break;
				case 1:
					try
					{
						this.con = new MySqlConnection(string.Concat(new object[]
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
						this.con.Open();
						DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						goto IL_25B;
					}
					catch (Exception e)
					{
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
						DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e, false, "    "));
						goto IL_25B;
					}
					break;
				default:
					goto IL_25B;
				}
				try
				{
					int num = 0;
					while (TaskStatus.GetDBStatus() == -1 && num < 600)
					{
						Thread.Sleep(50);
						DebugCenter.GetInstance().appendToFile(this.ToString() + "Waiting DBConnection : " + num);
						num++;
					}
					DBConn dynaConnection = DBConnPool.getDynaConnection();
					if (dynaConnection != null)
					{
						this.con = dynaConnection.con;
					}
					else
					{
						this.con = null;
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				}
			}
			IL_25B:
			this.debug = DebugCenter.GetInstance();
		}
		private int NeedU2I(string str_sql)
		{
			if (str_sql != null && str_sql.Length > 0)
			{
				if (str_sql.IndexOf("update device_data_daily20") > -1)
				{
					return 1;
				}
				if (str_sql.IndexOf("update device_data_hourly20") > -1)
				{
					return 2;
				}
				if (str_sql.IndexOf("update bank_data_daily20") > -1)
				{
					return 3;
				}
				if (str_sql.IndexOf("update bank_data_hourly20") > -1)
				{
					return 4;
				}
				if (str_sql.IndexOf("update port_data_daily20") > -1)
				{
					return 5;
				}
				if (str_sql.IndexOf("update port_data_hourly20") > -1)
				{
					return 6;
				}
			}
			return -1;
		}
		private string Search_string(string s, string s1, string s2)
		{
			int num = s.IndexOf(s1, 0) + s1.Length;
			int num2 = s.IndexOf(s2, num);
			return s.Substring(num, num2 - num);
		}
		private string ChgU2I(string str_sql, int i_type)
		{
			string result = "";
			string text = this.Search_string(str_sql, "+ ", " where");
			string text2 = this.Search_string(str_sql, "#", "#");
			string text3 = text2;
			int num = text3.IndexOf(" ");
			if (num >= 0)
			{
				text3 = text3.Substring(0, num);
			}
			text3 = text3.Trim();
			text3 = text3.Replace("-", "");
			switch (i_type)
			{
			case 1:
			{
				string text4 = this.Search_string(str_sql, "where device_id = ", " and insert_time");
				return string.Concat(new string[]
				{
					"insert into device_data_daily",
					text3,
					" (device_id,power_consumption,insert_time ) values(",
					text4,
					",",
					text,
					",#",
					text2,
					"#)"
				});
			}
			case 2:
			{
				string text4 = this.Search_string(str_sql, "where device_id = ", " and insert_time =");
				return string.Concat(new string[]
				{
					"insert into device_data_hourly",
					text3,
					" (device_id,power_consumption,insert_time ) values(",
					text4,
					",",
					text,
					",#",
					text2,
					"#)"
				});
			}
			case 3:
			{
				string text4 = this.Search_string(str_sql, "where bank_id = ", " and insert_time =");
				return string.Concat(new string[]
				{
					"insert into bank_data_daily",
					text3,
					" (bank_id,power_consumption,insert_time ) values(",
					text4,
					",",
					text,
					",#",
					text2,
					"#)"
				});
			}
			case 4:
			{
				string text4 = this.Search_string(str_sql, "where bank_id = ", " and insert_time =");
				return string.Concat(new string[]
				{
					"insert into bank_data_hourly",
					text3,
					" (bank_id,power_consumption,insert_time ) values(",
					text4,
					",",
					text,
					",#",
					text2,
					"#)"
				});
			}
			case 5:
			{
				string text4 = this.Search_string(str_sql, "where port_id = ", " and insert_time =");
				return string.Concat(new string[]
				{
					"insert into port_data_daily",
					text3,
					" (port_id,power_consumption,insert_time ) values(",
					text4,
					",",
					text,
					",#",
					text2,
					"#)"
				});
			}
			case 6:
			{
				string text4 = this.Search_string(str_sql, "where port_id = ", " and insert_time =");
				return string.Concat(new string[]
				{
					"insert into port_data_hourly",
					text3,
					" (port_id,power_consumption,insert_time ) values(",
					text4,
					",",
					text,
					",#",
					text2,
					"#)"
				});
			}
			default:
				return result;
			}
		}
		public int CloseDBConnection()
		{
			if (this.DBTYPE == 1)
			{
				return 1;
			}
			if (this.con != null)
			{
				int i = 0;
				while (i < 600)
				{
					if (this.con.State == ConnectionState.Closed)
					{
						goto IL_38;
					}
					if (this.con.State == ConnectionState.Open)
					{
						goto Block_4;
					}
					IL_8B:
					DebugCenter.GetInstance().appendToFile("Close DBConnection exception : " + this.con.State);
					i++;
					Thread.Sleep(50);
					continue;
					Block_4:
					try
					{
						try
						{
							IL_38:
							this.con.Close();
						}
						catch
						{
						}
						try
						{
							this.con.Dispose();
						}
						catch
						{
						}
						this.con = null;
						return 1;
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("Close DBConnection exception : " + ex.Message + "\n" + ex.StackTrace);
						break;
					}
					goto IL_8B;
				}
				return -1;
			}
			return 1;
		}
		public void workQueue_DBWork(object sender, WorkQueue<string>.EnqueueEventArgs e)
		{
			try
			{
				if (!DBWorkThread.STOP_THREAD)
				{
					string text = "";
					if (TaskStatus.GetDBStatus() == -1)
					{
						try
						{
							this.con.Close();
							this.con.Dispose();
							this.con = null;
						}
						catch
						{
						}
					}
					if (this.con == null)
					{
						if (DBUrl.SERVERMODE)
						{
							string cONNECT_STRING = DBUrl.CONNECT_STRING;
							string[] array = cONNECT_STRING.Split(new string[]
							{
								","
							}, StringSplitOptions.RemoveEmptyEntries);
							this.con = new MySqlConnection(string.Concat(new string[]
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
								";Pooling=true;Min Pool Size=0;Max Pool Size=50;Default Command Timeout=0;charset=utf8;"
							}));
							this.con.Open();
						}
						else
						{
							if (DBWorkThread.STOP_THREAD)
							{
								return;
							}
							switch (this.DBTYPE)
							{
							case 0:
								break;
							case 1:
								try
								{
									this.con = new MySqlConnection(string.Concat(new object[]
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
									this.con.Open();
									DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
									goto IL_2A6;
								}
								catch (Exception e2)
								{
									DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
									DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e2, false, "    "));
									goto IL_2A6;
								}
								break;
							default:
								goto IL_2A6;
							}
							try
							{
								"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.CURRENT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.CURRENT_PWD;
								int num = 0;
								while (TaskStatus.GetDBStatus() != 1 && num > -1)
								{
									Thread.Sleep(50);
									DebugCenter.GetInstance().appendToFile(this.ToString() + "Waiting DBConnection : " + num);
									num++;
								}
								DBConn dynaConnection = DBConnPool.getDynaConnection();
								if (dynaConnection != null)
								{
									this.con = dynaConnection.con;
								}
								else
								{
									this.con = null;
								}
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
							}
						}
					}
					try
					{
						IL_2A6:
						if (this.con.State != ConnectionState.Open)
						{
							try
							{
								this.con.Close();
								this.con.Open();
							}
							catch (Exception e3)
							{
								this.con = null;
								DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_MYSQLCONNECT_LOST, true);
								DebugCenter.GetInstance().appendToFile("Could not create MySQL connection : \r\n" + CommonAPI.ReportException(0, e3, false, "    "));
							}
						}
					}
					catch
					{
					}
					try
					{
						if (this.cmd != null)
						{
							this.cmd.Dispose();
						}
					}
					catch (Exception ex2)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
					}
					try
					{
						string item = e.Item;
						if (item.StartsWith("PDEND"))
						{
							InSnergyGateway.Need_Calculate_PUE = true;
							DBWorkThread.NEEDLOG = true;
						}
						else
						{
							if (item.IndexOf("END") > -1)
							{
								DBCacheStatus.LastInsertTime = DateTime.Now;
								DBWorkThread.NEEDLOG = true;
							}
							else
							{
								this.cmd = this.con.CreateCommand();
								int num2 = this.NeedU2I(item);
								if (this.DBTYPE == 1 || DBUrl.SERVERMODE)
								{
									string commandText = item.Replace("#", "'");
									this.cmd.CommandText = commandText;
								}
								else
								{
									this.cmd.CommandText = item;
								}
								text = this.cmd.CommandText;
								if (!DBWorkThread.STOP_THREAD)
								{
									int num3 = this.cmd.ExecuteNonQuery();
									if (num3 < 1 && num2 > 0)
									{
										if (this.DBTYPE == 1 || DBUrl.SERVERMODE)
										{
											string text2 = this.ChgU2I(item, num2);
											text2 = text2.Replace("#", "'");
											this.cmd.CommandText = text2;
										}
										else
										{
											this.cmd.CommandText = this.ChgU2I(item, num2);
										}
										text = this.cmd.CommandText;
										if (DBWorkThread.STOP_THREAD)
										{
											return;
										}
										this.cmd.ExecuteNonQuery();
									}
									this.cmd.Dispose();
									if (TaskStatus.GetDBStatus() == -1)
									{
										try
										{
											this.con.Close();
										}
										catch
										{
										}
										try
										{
											this.con.Dispose();
										}
										catch
										{
										}
										this.con = null;
									}
								}
							}
						}
					}
					catch (Exception ex3)
					{
						if (ex3.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
						{
							string tableName = DBUtil.GetTableName(text);
							if (tableName != null && tableName.Length > 0)
							{
								DBUtil.SetMySQLInfo(tableName);
								DebugCenter.GetInstance().appendToFile("MySQL database is marked as crashed, EcoSensor Monitor Service will be shutdown ");
								DBUtil.StopService();
							}
						}
						if (ex3.Message.ToLower().IndexOf("fatal error encountered during command execution") < 0)
						{
							if (text.IndexOf("rack_effect") > 0)
							{
								try
								{
									DBTools.Write_DBERROR_Log();
									goto IL_56F;
								}
								catch
								{
									goto IL_56F;
								}
							}
							if (DBWorkThread.NEEDLOG)
							{
								try
								{
									DBTools.Write_DBERROR_Log();
								}
								catch
								{
								}
								DBWorkThread.NEEDLOG = false;
							}
							IL_56F:
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex3.Message + "\n" + ex3.StackTrace);
						}
						try
						{
							this.cmd.Dispose();
							this.con.Close();
						}
						catch (Exception)
						{
						}
						this.con = null;
					}
				}
			}
			finally
			{
				try
				{
					if (this.con != null)
					{
						this.con.Close();
					}
					this.con = null;
				}
				catch
				{
				}
			}
		}
	}
}
