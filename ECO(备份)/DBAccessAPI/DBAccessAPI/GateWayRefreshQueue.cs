using CommonAPI;
using CommonAPI.Tools;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class GateWayRefreshQueue
	{
		public static WorkQueue<Dictionary<string, Dictionary<string, Dictionary<string, string>>>> wq;
		public static WorkQueue<GatewayDataCell> wq_data;
		public static CallBackEventHandler CBWork = null;
		private DbConnection con;
		public static int GetUpdateQueueSize()
		{
			if (GateWayRefreshQueue.wq != null)
			{
				return GateWayRefreshQueue.wq.getSize();
			}
			return 0;
		}
		public static int GetDataQueueSize()
		{
			if (GateWayRefreshQueue.wq_data != null)
			{
				return GateWayRefreshQueue.wq_data.getSize();
			}
			return 0;
		}
		public static int CleanDataTask()
		{
			int result;
			try
			{
				if (GateWayRefreshQueue.wq_data != null)
				{
					GateWayRefreshQueue.wq_data.CleanQueue();
					result = 1;
				}
				else
				{
					result = -1;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				result = -1;
			}
			return result;
		}
		public static int InsertDataTask(GatewayDataCell gdc_itm)
		{
			int result;
			try
			{
				if (GateWayRefreshQueue.wq_data == null)
				{
					GateWayRefreshQueue.wq_data = new WorkQueue<GatewayDataCell>(10000, "GateWayData");
					GateWayRefreshQueue @object = new GateWayRefreshQueue();
					GateWayRefreshQueue.wq_data.UserWork += new UserWorkEventHandler<GatewayDataCell>(@object.QueueWork4Data);
				}
				GateWayRefreshQueue.wq_data.EnqueueItem(gdc_itm);
				result = 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				result = -1;
			}
			return result;
		}
		public static int Insertnewtask(Dictionary<string, Dictionary<string, Dictionary<string, string>>> dic_item)
		{
			int result;
			try
			{
				if (GateWayRefreshQueue.wq == null)
				{
					GateWayRefreshQueue.wq = new WorkQueue<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(10000, "InsGateway");
					GateWayRefreshQueue @object = new GateWayRefreshQueue();
					GateWayRefreshQueue.wq.UserWork += new UserWorkEventHandler<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(@object.QueueWork);
				}
				GateWayRefreshQueue.wq.EnqueueItem(dic_item);
				result = 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				result = -1;
			}
			return result;
		}
		private DbConnection getConnection()
		{
			try
			{
				DbConnection result;
				if (DBUrl.SERVERMODE)
				{
					string cONNECT_STRING = DBUrl.CONNECT_STRING;
					cONNECT_STRING = DBUrl.CONNECT_STRING;
					string[] array = cONNECT_STRING.Split(new string[]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					DbConnection dbConnection = new MySqlConnection(string.Concat(new string[]
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
					dbConnection.Open();
					result = dbConnection;
					return result;
				}
				DbConnection dbConnection2 = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor");
				dbConnection2.Open();
				result = dbConnection2;
				return result;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public GateWayRefreshQueue()
		{
			try
			{
				if (DBUrl.SERVERMODE)
				{
					this.con = this.getConnection();
				}
				else
				{
					string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
					this.con = new OleDbConnection(connectionString);
					this.con.Open();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public GateWayRefreshQueue(CallBackEventHandler d_callback)
		{
			try
			{
				GateWayRefreshQueue.CBWork = new CallBackEventHandler(d_callback.Invoke);
				if (DBUrl.SERVERMODE)
				{
					this.con = this.getConnection();
				}
				else
				{
					string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
					this.con = new OleDbConnection(connectionString);
					this.con.Open();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public void QueueWork(object sender, WorkQueue<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>.EnqueueEventArgs e)
		{
			if (this.con == null || this.con.State != ConnectionState.Open)
			{
				if (this.con != null)
				{
					try
					{
						this.con.Close();
					}
					catch (Exception)
					{
					}
				}
				try
				{
					if (DBUrl.SERVERMODE)
					{
						this.con = this.getConnection();
					}
					else
					{
						string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
						this.con = new OleDbConnection(connectionString);
						this.con.Open();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				}
			}
			try
			{
				Dictionary<string, Dictionary<string, Dictionary<string, string>>> item = e.Item;
				foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> current in item)
				{
					string key = current.Key;
					Dictionary<string, Dictionary<string, string>> value = current.Value;
					InSnergyGateway.RefreshGateWay(key, value, this.con);
					if (GateWayRefreshQueue.CBWork != null)
					{
						GateWayRefreshQueue.CBWork(key);
					}
				}
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
			}
		}
		public void QueueWork4Data(object sender, WorkQueue<GatewayDataCell>.EnqueueEventArgs e)
		{
			DbCommand dbCommand = null;
			DbDataReader dbDataReader = null;
			if (this.con == null || this.con.State != ConnectionState.Open)
			{
				if (this.con != null)
				{
					try
					{
						this.con.Close();
					}
					catch (Exception)
					{
					}
				}
				try
				{
					if (DBUrl.SERVERMODE)
					{
						this.con = this.getConnection();
					}
					else
					{
						string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
						this.con = new OleDbConnection(connectionString);
						this.con.Open();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				}
			}
			try
			{
				GatewayDataCell item = e.Item;
				dbCommand = this.con.CreateCommand();
				dbCommand.CommandText = string.Concat(new string[]
				{
					"select eleflag from gatewaytable where gid='",
					item.GatewayID,
					"' and bid='",
					item.BranchID,
					"' and sid='",
					item.SubmeterID,
					"' "
				});
				object obj = dbCommand.ExecuteScalar();
				if (obj == null || obj is DBNull || Convert.ToInt32(obj) < 1)
				{
					dbCommand.Dispose();
				}
				else
				{
					dbCommand.Dispose();
					int num = Convert.ToInt32(obj);
					dbCommand = this.con.CreateCommand();
					dbCommand.CommandText = string.Concat(new string[]
					{
						"select * from gatewaylastpd where bid='",
						item.BranchID,
						"' and sid='",
						item.SubmeterID,
						"' "
					});
					dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.HasRows)
					{
						dbDataReader.Read();
						int num2 = MyConvert.ToInt32(dbDataReader.GetValue(2));
						double @double = dbDataReader.GetDouble(3);
						DateTime dateTime = dbDataReader.GetDateTime(4);
						dbDataReader.Close();
						dbDataReader.Dispose();
						dbCommand.Dispose();
						if (num == num2)
						{
							double num3 = item.PD - @double;
							double num4 = 0.0;
							double num5 = 0.0;
							double num6 = 0.0;
							bool flag = this.checkweek(dateTime);
							bool flag2 = this.checkday(dateTime);
							bool flag3 = this.checkhour(dateTime);
							if (flag)
							{
								num4 = num3;
							}
							if (flag2)
							{
								num5 = num3;
							}
							if (flag3)
							{
								num6 = num3;
							}
							dbCommand = this.con.CreateCommand();
							dbCommand.CommandText = "select lasttime from currentpue ";
							obj = dbCommand.ExecuteScalar();
							string text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
							string text2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
							string text3 = this.getweek().ToString("yyyy-MM-dd HH:mm:ss");
							if (obj == null || obj is DBNull)
							{
								if (num == 1)
								{
									dbCommand.CommandText = string.Concat(new object[]
									{
										"insert into currentpue (curhour,curday,curweek,ithourpue,nonithourpue,itdaypue,nonitdaypue,itweekpue,nonitweekpue,lasttime) values(#",
										text,
										"#,#",
										text2,
										"#,#",
										text3,
										"#,",
										num6,
										",0,",
										num5,
										",0,",
										num4,
										",0,#",
										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
										"# ) "
									});
								}
								else
								{
									if (num == 2)
									{
										dbCommand.CommandText = string.Concat(new object[]
										{
											"insert into currentpue (curhour,curday,curweek,ithourpue,nonithourpue,itdaypue,nonitdaypue,itweekpue,nonitweekpue,lasttime) values(#",
											text,
											"#,#",
											text2,
											"#,#",
											text3,
											"#,0,",
											num6,
											",0,",
											num5,
											",0,",
											num4,
											",#",
											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
											"# ) "
										});
									}
								}
								if (DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
								}
							}
							else
							{
								DateTime dt_time = Convert.ToDateTime(obj);
								string text4 = "";
								if (this.checkweek(dt_time))
								{
									if (this.checkday(dt_time))
									{
										if (this.checkhour(dt_time))
										{
											if (num == 1)
											{
												text4 = string.Concat(new object[]
												{
													"update currentpue set curhour=#",
													text,
													"#,curday=#",
													text2,
													"#,curweek=#",
													text3,
													"#,ithourpue=ithourpue+",
													num6,
													",itdaypue=itdaypue+",
													num5,
													",itweekpue=itweekpue+",
													num4,
													",lasttime=#",
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
											}
											else
											{
												if (num == 2)
												{
													text4 = string.Concat(new object[]
													{
														"update currentpue set curhour=#",
														text,
														"#,curday=#",
														text2,
														"#,curweek=#",
														text3,
														"#,nonithourpue=nonithourpue+",
														num6,
														",nonitdaypue=nonitdaypue+",
														num5,
														",nonitweekpue=nonitweekpue+",
														num4,
														",lasttime=#",
														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
														"# "
													});
												}
											}
										}
										else
										{
											if (num == 1)
											{
												text4 = string.Concat(new object[]
												{
													"update currentpue set curhour=#",
													text,
													"#,curday=#",
													text2,
													"#,curweek=#",
													text3,
													"#,nonithourpue=0,ithourpue=",
													num6,
													",itdaypue=itdaypue+",
													num5,
													",itweekpue=itweekpue+",
													num4,
													",lasttime=#",
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
											}
											else
											{
												if (num == 2)
												{
													text4 = string.Concat(new object[]
													{
														"update currentpue set curhour=#",
														text,
														"#,curday=#",
														text2,
														"#,curweek=#",
														text3,
														"#,ithourpue=0,nonithourpue=",
														num6,
														",nonitdaypue=nonitdaypue+",
														num5,
														",nonitweekpue=nonitweekpue+",
														num4,
														",lasttime=#",
														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
														"# "
													});
												}
											}
										}
									}
									else
									{
										if (num == 1)
										{
											text4 = string.Concat(new object[]
											{
												"update currentpue set curhour=#",
												text,
												"#,curday=#",
												text2,
												"#,curweek=#",
												text3,
												"#,ithourpue=",
												num6,
												",nonithourpue=0,itdaypue=",
												num5,
												",nonitdaypue=0,itweekpue=itweekpue+",
												num4,
												",lasttime=#",
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
												"# "
											});
										}
										else
										{
											if (num == 2)
											{
												text4 = string.Concat(new object[]
												{
													"update currentpue set curhour=#",
													text,
													"#,curday=#",
													text2,
													"#,curweek=#",
													text3,
													"#,nonithourpue=",
													num6,
													",ithourpue=0,nonitdaypue=",
													num5,
													",itdaypue=0,nonitweekpue=nonitweekpue+",
													num4,
													",lasttime=#",
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
											}
										}
									}
								}
								else
								{
									text4 = string.Concat(new string[]
									{
										"update currentpue set curhour=#",
										text,
										"#,curday=#",
										text2,
										"#,curweek=#",
										text3,
										"#,ithourpue=0,nonithourpue=0,itdaypue=0,nonitdaypue=0,itweekpue=0,nonitweekpue=0,lasttime=#",
										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
										"# "
									});
								}
								if (DBUrl.SERVERMODE)
								{
									text4 = text4.Replace("#", "'");
								}
								dbCommand.CommandText = text4;
							}
							DbTransaction dbTransaction = this.con.BeginTransaction();
							dbCommand.Transaction = dbTransaction;
							dbCommand.ExecuteNonQuery();
							dbCommand.Transaction.Commit();
							dbTransaction.Dispose();
							dbCommand.Dispose();
						}
						this.ModifyLastPD(this.con, item.BranchID, item.SubmeterID, num, item.PD, item.RecordTime, "UPDATE");
					}
					else
					{
						dbDataReader.Close();
						dbDataReader.Dispose();
						dbCommand.Dispose();
						this.ModifyLastPD(this.con, item.BranchID, item.SubmeterID, num, item.PD, item.RecordTime, "INSERT");
					}
				}
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
						dbDataReader.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Transaction.Rollback();
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
			}
		}
		private int ModifyLastPD(DbConnection tmp_con, string str_bid, string str_sid, int i_flag, double d_pd, DateTime dt_time, string str_mode)
		{
			DbCommand dbCommand = null;
			DbTransaction dbTransaction = null;
			try
			{
				dbCommand = tmp_con.CreateCommand();
				dbTransaction = tmp_con.BeginTransaction();
				dbCommand.Transaction = dbTransaction;
				string text;
				if (str_mode.Equals("UPDATE"))
				{
					text = string.Concat(new object[]
					{
						"update gatewaylastpd set eleflag=",
						i_flag,
						",pd=",
						MyConvert.ToString(d_pd),
						",timemark=#",
						dt_time.ToString("yyyy-MM-dd HH:mm:ss"),
						"# where bid='",
						str_bid,
						"' and sid='",
						str_sid,
						"' "
					});
				}
				else
				{
					text = string.Concat(new object[]
					{
						"insert into gatewaylastpd (bid,sid,eleflag,pd,timemark) values ('",
						str_bid,
						"','",
						str_sid,
						"',",
						i_flag,
						",",
						MyConvert.ToString(d_pd),
						",#",
						dt_time.ToString("yyyy-MM-dd HH:mm:ss"),
						"# ) "
					});
				}
				if (DBUrl.SERVERMODE)
				{
					text = text.Replace("#", "'");
				}
				dbCommand.CommandText = text;
				dbCommand.ExecuteNonQuery();
				dbCommand.Transaction.Commit();
				dbTransaction.Dispose();
				dbCommand.Dispose();
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
			}
			return -1;
		}
		private DateTime getweek()
		{
			DateTime result = DateTime.Now;
			switch (DateTime.Now.DayOfWeek)
			{
			case DayOfWeek.Sunday:
				result = DateTime.Now.AddDays(-6.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Monday:
				result = DateTime.Now;
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Tuesday:
				result = DateTime.Now.AddDays(-1.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Wednesday:
				result = DateTime.Now.AddDays(-2.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Thursday:
				result = DateTime.Now.AddDays(-3.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Friday:
				result = DateTime.Now.AddDays(-4.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Saturday:
				result = DateTime.Now.AddDays(-5.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			}
			return result;
		}
		private bool checkweek(DateTime dt_time)
		{
			switch (DateTime.Now.DayOfWeek)
			{
			case DayOfWeek.Sunday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-6.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Monday:
			{
				DateTime dateTime = DateTime.Now;
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Tuesday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-1.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Wednesday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-2.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Thursday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-3.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Friday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-4.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Saturday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-5.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			default:
				return false;
			}
		}
		private bool checkday(DateTime dt_time)
		{
			return dt_time.Year == DateTime.Now.Year && dt_time.Month == DateTime.Now.Month && dt_time.Day == DateTime.Now.Day;
		}
		private bool checkhour(DateTime dt_time)
		{
			return dt_time.Year == DateTime.Now.Year && dt_time.Month == DateTime.Now.Month && dt_time.Day == DateTime.Now.Day && dt_time.Hour == DateTime.Now.Hour;
		}
	}
}
