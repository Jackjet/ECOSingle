using CommonAPI;
using CommonAPI.Global;
using CommonAPI.InterProcess;
using CommonAPI.Timers;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading;
namespace DBAccessAPI
{
	public class DataWareHouse
	{
		private static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static string _dbConnectString = "";
		private static DateTime _tLastDataTime = DateTime.MinValue;
		private Hashtable ht_All;
		private Hashtable ht_powerPD;
		private Hashtable ht_power;
		private Hashtable ht_pdDev;
		private Hashtable ht_pdPort;
		private Hashtable ht_pwDev;
		private Hashtable ht_pwPort;
		private Dictionary<int, string> _deviceVoltageList;
		private Dictionary<int, Dictionary<int, double>> _lastSensorMaxValue;
		private long _maxPacketLength;
		private static object _lockLog = new object();
		private static string _xmlFile = "Config.xml";
		private static string _logFile = "";
		private static Dictionary<string, string> _cfgFile = null;
		public static bool IsNewSchema()
		{
			return false;
		}
		public static void PrepareTables(DateTime dataTime)
		{
			long num = (long)((DataWareHouse._tLastDataTime.Year << 16) + (DataWareHouse._tLastDataTime.Month << 8) + DataWareHouse._tLastDataTime.Day);
			long num2 = (long)((dataTime.Year << 16) + (dataTime.Month << 8) + dataTime.Day);
			if (num2 == num)
			{
				return;
			}
			DataWareHouse._dbConnectString = string.Concat(new object[]
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
			string text = dataTime.ToString("yyyyMMdd");
			bool flag = false;
			string text2 = "CREATE TABLE IF NOT EXISTS `dev_min_pw_pd_{0}` (";
			text2 += "`device_id`        smallint NOT NULL,";
			text2 += "`power`            int DEFAULT NULL,";
			text2 += "`power_consumption` int DEFAULT NULL,";
			text2 += "`insert_time`      smallint NOT NULL";
			text2 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text3 = "CREATE TABLE IF NOT EXISTS `dev_hour_pd_{0}` (";
			text3 += "`device_id`         smallint NOT NULL,";
			text3 += "`power_consumption` int DEFAULT NULL,";
			text3 += "`power_max`         int DEFAULT NULL,";
			text3 += "`insert_time`       tinyint NOT NULL";
			text3 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text4 = "CREATE TABLE IF NOT EXISTS `dev_day_pd_pm_{0}` (";
			text4 += "`device_id`          smallint NOT NULL,";
			text4 += "`power_consumption`  bigint DEFAULT NULL,";
			text4 += "`power_max`          int DEFAULT NULL";
			text4 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text5 = "CREATE TABLE IF NOT EXISTS `port_min_pw_pd_{0}` (";
			text5 += "`port_id`            mediumint NOT NULL,";
			text5 += "`power`              int DEFAULT NULL,";
			text5 += "`power_consumption`  int DEFAULT NULL,";
			text5 += "`insert_time`        smallint NOT NULL";
			text5 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text6 = "CREATE TABLE IF NOT EXISTS `port_hour_pd_{0}` (";
			text6 += "`port_id`         mediumint NOT NULL,";
			text6 += "`power_consumption` int DEFAULT NULL,";
			text6 += "`power_max`       int DEFAULT NULL,";
			text6 += "`insert_time`     tinyint NOT NULL";
			text6 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text7 = "CREATE TABLE IF NOT EXISTS `port_day_pd_pm_{0}` (";
			text7 += "`port_id`          mediumint NOT NULL,";
			text7 += "`power_consumption` int DEFAULT NULL,";
			text7 += "`power_max`        int NOT NULL";
			text7 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text8 = "CREATE TABLE IF NOT EXISTS `rack_hour_thermal_{0}` (";
			text8 += "`rack_id`         smallint NOT NULL,";
			text8 += "`intakepeak`      float DEFAULT NULL,";
			text8 += "`exhaustpeak`     float DEFAULT NULL,";
			text8 += "`differencepeak`  float DEFAULT NULL,";
			text8 += "`insert_time`     tinyint NOT NULL";
			text8 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text9 = "CREATE TABLE IF NOT EXISTS `rack_day_thermal_{0}` (";
			text9 += "`rack_id`          smallint NOT NULL,";
			text9 += "`intakepeak`       float DEFAULT NULL,";
			text9 += "`exhaustpeak`      float DEFAULT NULL,";
			text9 += "`differencepeak`   float DEFAULT NULL";
			text9 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			try
			{
				mySqlConnection = new MySqlConnection(DataWareHouse._dbConnectString);
				mySqlConnection.Open();
				mySqlCommand = mySqlConnection.CreateCommand();
				if (flag)
				{
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `dev_min_pw_pd_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `dev_hour_pd_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `dev_day_pd_pm_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `port_min_pw_pd_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `port_hour_pd_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `port_day_pd_pm_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `rack_hour_thermal_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `rack_day_thermal_" + text + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				mySqlCommand.CommandText = string.Format(text2, text);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text3, text);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text4, text);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text5, text);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text6, text);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text7, text);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text8, text);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text9, text);
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
			catch (Exception ex)
			{
				DataWareHouse.WriteLog("    Failed, " + ex.Message, new string[0]);
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
		private static long GetMySqlPacketLength(string connectString)
		{
			long result = 0L;
			try
			{
				MySqlConnection mySqlConnection = new MySqlConnection(connectString);
				mySqlConnection.Open();
				MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
				mySqlCommand.CommandText = "SHOW VARIABLES LIKE 'max_allowed_packet'";
				DbDataReader dbDataReader = mySqlCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					result = (long)Convert.ToInt32(dbDataReader.GetValue(1));
				}
				dbDataReader.Close();
				mySqlCommand.Dispose();
				mySqlConnection.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("getMySQLPath: " + ex.Message);
				result = 0L;
			}
			return result;
		}
		public static void WriteLog(string format, params string[] list)
		{
			lock (DataWareHouse._lockLog)
			{
				if (!DataWareHouse.sCodebase.EndsWith("/") && !DataWareHouse.sCodebase.EndsWith("\\"))
				{
					DataWareHouse.sCodebase += "\\";
				}
				if (DataWareHouse._cfgFile == null)
				{
					DataWareHouse._logFile = "";
					DataWareHouse._cfgFile = ValuePairs.LoadValueKeyFromXML(DataWareHouse._xmlFile);
					if (DataWareHouse._cfgFile.ContainsKey("LogFile"))
					{
						DataWareHouse._logFile = DataWareHouse._cfgFile["LogFile"];
					}
				}
				if (DataWareHouse._logFile != null && !(DataWareHouse._logFile == ""))
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
						string text = DataWareHouse.sCodebase + "\\debuglog\\" + DataWareHouse._logFile;
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
		public DataWareHouse()
		{
			this.ht_All = null;
			this.ht_powerPD = null;
			this.ht_power = null;
			this.ht_pdDev = null;
			this.ht_pdPort = null;
			this.ht_pwDev = null;
			this.ht_pwPort = null;
			if (!DataWareHouse.sCodebase.EndsWith("/") && !DataWareHouse.sCodebase.EndsWith("\\"))
			{
				DataWareHouse.sCodebase += "\\";
			}
			this._deviceVoltageList = null;
			this._lastSensorMaxValue = null;
			DataWareHouse._dbConnectString = string.Concat(new object[]
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
			this._maxPacketLength = DataWareHouse.GetMySqlPacketLength(DataWareHouse._dbConnectString);
		}
		public void AddRackThermalThread(Dictionary<int, Dictionary<int, double>> lastSensorMaxValue)
		{
			this._lastSensorMaxValue = lastSensorMaxValue;
		}
		public void Execute(DateTime tDataTime, Hashtable data2DB, Hashtable existed)
		{
			if (data2DB == null || data2DB.Count <= 0)
			{
				DataWareHouse._tLastDataTime = tDataTime;
				return;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("ACCESS"))
			{
				this.ht_All = data2DB;
			}
			if (data2DB.ContainsKey("PD"))
			{
				this.ht_powerPD = (Hashtable)data2DB["PD"];
				if (this.ht_powerPD != null && this.ht_powerPD.Count > 0)
				{
					if (this.ht_powerPD.ContainsKey("DEVICE"))
					{
						this.ht_pdDev = (Hashtable)this.ht_powerPD["DEVICE"];
						if (this.ht_pdDev.Count <= 0)
						{
							this.ht_pdDev = null;
						}
					}
					if (this.ht_powerPD.ContainsKey("OUTLET"))
					{
						this.ht_pdPort = (Hashtable)this.ht_powerPD["OUTLET"];
						if (this.ht_pdPort.Count <= 0)
						{
							this.ht_pdPort = null;
						}
					}
				}
			}
			if (data2DB.ContainsKey("POWER"))
			{
				this.ht_power = (Hashtable)data2DB["POWER"];
				if (this.ht_power != null && this.ht_power.Count > 0)
				{
					if (this.ht_power.ContainsKey("DEVICE"))
					{
						this.ht_pwDev = (Hashtable)this.ht_power["DEVICE"];
						if (this.ht_pwDev.Count <= 0)
						{
							this.ht_pwDev = null;
						}
					}
					if (this.ht_power.ContainsKey("OUTLET"))
					{
						this.ht_pwPort = (Hashtable)this.ht_power["OUTLET"];
						if (this.ht_pwPort.Count <= 0)
						{
							this.ht_pwPort = null;
						}
					}
				}
			}
			string tableSuffix = tDataTime.ToString("yyyyMMdd");
			List<StorageContext> list = new List<StorageContext>();
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				this.ht_All = null;
				if (this.ht_pwDev != null)
				{
					list.Add(new StorageContext
					{
						_tDataTime = tDataTime,
						_sDataTime = tDataTime.ToString("yyyy-MM-dd HH:mm:ss"),
						_htData = this.ht_pwDev,
						_dataRatio = 10000.0,
						_htExisted = null,
						_tablePrefix = "dev_min_pw_pd_",
						_tableSuffix = tableSuffix
					});
				}
				if (this.ht_pwPort != null)
				{
					list.Add(new StorageContext
					{
						_tDataTime = tDataTime,
						_sDataTime = tDataTime.ToString("yyyy-MM-dd HH:mm:ss"),
						_htData = this.ht_pwPort,
						_dataRatio = 10000.0,
						_htExisted = null,
						_tablePrefix = "port_min_pw_pd_",
						_tableSuffix = tableSuffix
					});
				}
			}
			else
			{
				if (this.ht_All != null)
				{
					list.Add(new StorageContext
					{
						_tDataTime = tDataTime,
						_tablePrefix = "UpdateForAccess",
						_AllData4Access = this.ht_All
					});
				}
			}
			if (this._deviceVoltageList != null)
			{
				list.Add(new StorageContext
				{
					_tDataTime = tDataTime,
					_tablePrefix = "Voltage",
					_deviceVoltageList = this._deviceVoltageList
				});
			}
			if (this._lastSensorMaxValue != null)
			{
				list.Add(new StorageContext
				{
					_tDataTime = tDataTime,
					_tablePrefix = "SensorThermal",
					_lastSensorMaxValue = this._lastSensorMaxValue
				});
			}
			DateTime now = DateTime.Now;
			InterProcessShared<Dictionary<string, string>>.setInterProcessKeyValue("MultiThreadUpdateStatus", "busy");
			this.ParallelUpdate(list.Count, list);
			InterProcessShared<Dictionary<string, string>>.setInterProcessKeyValue("MultiThreadUpdateStatus", "idle");
			DataWareHouse.WriteLog("########## Data2WareHouse, elapsed={0} ms", new string[]
			{
				(DateTime.Now - now).TotalMilliseconds.ToString()
			});
			DataWareHouse._tLastDataTime = tDataTime;
		}
		private bool ParallelUpdate(int nMaxConcurrentThreads, List<StorageContext> contexts)
		{
			if (contexts.Count <= 0)
			{
				return false;
			}
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			List<StorageContext> list = new List<StorageContext>();
			for (int i = 0; i < contexts.Count; i++)
			{
				list.Add(contexts[i]);
			}
			int num = nMaxConcurrentThreads;
			if (num == 0)
			{
				num = contexts.Count;
			}
			int num2 = 0;
			string testOption = MultiThreadQuery.GetTestOption("update_thread_gap(ms)");
			if (!string.IsNullOrEmpty(testOption))
			{
				num2 = Convert.ToInt32(testOption);
			}
			Semaphore semaphore = new Semaphore(Math.Min(num, contexts.Count), Math.Min(num, contexts.Count));
			SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0, contexts.Count);
			int num3 = 1;
			while (list.Count > 0)
			{
				semaphore.WaitOne();
				StorageContext storageContext = list[0];
				list.RemoveAt(0);
				try
				{
					if (num2 > 0)
					{
						Thread.Sleep(num2);
					}
				}
				catch (Exception)
				{
				}
				Thread thread = new Thread(new ParameterizedThreadStart(this.OneTableUpdate));
				thread.Name = string.Format("MultiThreadUpdate #{0}", num3++);
				thread.IsBackground = true;
				storageContext._finishSignal = semaphore;
				storageContext._finishCounter = semaphoreSlim;
				thread.Start(storageContext);
			}
			try
			{
				while (semaphoreSlim.CurrentCount < contexts.Count)
				{
					Thread.Sleep(20);
				}
				DataWareHouse.WriteLog("########## All updates done: elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			}
			catch (Exception ex)
			{
				DataWareHouse.WriteLog(ex.Message, new string[0]);
			}
			return true;
		}
		public static void AccessMinuteData(Hashtable ht_data, int updatepower, DateTime DT_insert)
		{
		}
		private void OneTableUpdate(object obj)
		{
			StorageContext storageContext = (StorageContext)obj;
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			DataWareHouse.WriteLog(string.Format("EEEEEEEEEEEEEEEEE [{0}] Table: {1}", Thread.CurrentThread.Name, storageContext._tablePrefix + storageContext._tableSuffix), new string[0]);
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				if (storageContext._deviceVoltageList != null)
				{
					DeviceOperation.UpdateDeviceVoltage(storageContext._deviceVoltageList);
				}
				else
				{
					if (storageContext._lastSensorMaxValue != null)
					{
						RackInfo.GenerateAllRackThermal(storageContext._lastSensorMaxValue, storageContext._tDataTime);
					}
					else
					{
						if (storageContext._AllData4Access != null)
						{
							DataWareHouse.AccessMinuteData(storageContext._AllData4Access, 1, storageContext._tDataTime);
						}
						else
						{
							try
							{
								string arg;
								if (storageContext._tablePrefix.StartsWith("dev_min_"))
								{
									arg = "device_id";
								}
								else
								{
									if (!storageContext._tablePrefix.StartsWith("port_min_"))
									{
										return;
									}
									arg = "port_id";
								}
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
								try
								{
									string text = string.Format("insert into {0}{1} ({2},power_consumption,power,insert_time) values ", storageContext._tablePrefix, storageContext._tableSuffix, arg);
									int num = storageContext._tDataTime.Hour * 60 + storageContext._tDataTime.Minute;
									string format = string.Format("({{{0}}},{{{1}}},{{{2}}},{3})", new object[]
									{
										"0",
										"1",
										"2",
										num
									});
									string text2 = "";
									StringBuilder stringBuilder = new StringBuilder();
									int count = storageContext._htData.Count;
									int num2 = 0;
									stringBuilder.Append(text);
									foreach (DictionaryEntry dictionaryEntry in storageContext._htData)
									{
										num2++;
										try
										{
											double value = storageContext._dataRatio * Convert.ToDouble(dictionaryEntry.Value);
											string arg2 = string.Format("{0}", Convert.ToInt64(value));
											text2 = "";
											string text3 = string.Format(format, dictionaryEntry.Key, "null", arg2);
											if (stringBuilder.Length > text.Length)
											{
												stringBuilder.Append(",");
											}
											stringBuilder.Append(text3);
											if ((num2 >= count || (long)(stringBuilder.Length + text3.Length) > this._maxPacketLength / 2L) && stringBuilder.Length > 0)
											{
												dbCommand.CommandText = stringBuilder.ToString();
												text2 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
												stringBuilder.Clear();
												stringBuilder.Append(text);
											}
										}
										catch (Exception ex)
										{
											if (ex.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
											{
												try
												{
													if (!string.IsNullOrEmpty(text2))
													{
														dbCommand.CommandText = text2;
														dbCommand.ExecuteNonQuery();
													}
												}
												catch (Exception ex2)
												{
													DataWareHouse.WriteLog("Insert data: {0}{1}, Error:{2}", new string[]
													{
														storageContext._tablePrefix,
														storageContext._tableSuffix,
														ex.Message
													});
													if (!string.IsNullOrEmpty(text2) && ex2.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
													{
														string tableName = DBUtil.GetTableName(text2);
														if (tableName != null && tableName.Length > 0)
														{
															DBUtil.SetMySQLInfo(tableName);
														}
														DataWareHouse.WriteLog("        MySQL database may be crashed, EcoSensor Monitor Service will shutdown ", new string[0]);
														DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
														DBTools.Write_DBERROR_Log();
														DBUtil.StopService();
														break;
													}
												}
											}
										}
									}
								}
								catch (Exception)
								{
								}
							}
							catch (Exception ex3)
							{
								if (ex3.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
								{
									DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex3.Message + "\n" + ex3.StackTrace);
									DBTools.Write_DBERROR_Log();
									DBUtil.StopService();
								}
							}
						}
					}
				}
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
				if (storageContext._htData != null)
				{
					DataWareHouse.WriteLog(string.Format("----------------- [{0}] finished, count={1}, elapsed={2}", Thread.CurrentThread.Name + " " + storageContext._tablePrefix + storageContext._tableSuffix, storageContext._htData.Count, tickTimer.getElapsed().ToString()), new string[0]);
				}
				else
				{
					DataWareHouse.WriteLog(string.Format("----------------- [{0}] finished, elapsed={1}", Thread.CurrentThread.Name + " " + storageContext._tablePrefix + storageContext._tableSuffix, tickTimer.getElapsed().ToString()), new string[0]);
				}
				if (storageContext._finishSignal != null)
				{
					storageContext._finishSignal.Release();
				}
				if (storageContext._finishCounter != null)
				{
					storageContext._finishCounter.Release();
				}
			}
		}
	}
}
