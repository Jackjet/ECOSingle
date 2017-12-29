using CommonAPI;
using CommonAPI.Global;
using CommonAPI.InterProcess;
using CommonAPI.Timers;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Threading;
namespace DBAccessAPI
{
	public class MultiThreadUpdate
	{
		private class UpdateContext
		{
			public Dictionary<int, string> _deviceVoltageList;
			public Dictionary<int, Dictionary<int, double>> _lastSensorMaxValue;
			public Hashtable _AllData4Access;
			public Hashtable _htExisted;
			public Hashtable _htData;
			public string _sKeyColumnName;
			public string _sValueColumnName;
			public DateTime _tDataTime;
			public string _sDataTime;
			public string _tablePrefix;
			public string _tableSuffix;
			public double _dataRatio;
			public Semaphore _finishSignal;
			public SemaphoreSlim _finishCounter;
			public UpdateContext()
			{
				this._htExisted = null;
				this._htData = null;
				this._dataRatio = 1.0;
				this._sKeyColumnName = "";
				this._tDataTime = DateTime.Now;
				this._tablePrefix = "";
				this._tableSuffix = "";
				this._finishSignal = null;
				this._finishCounter = null;
				this._deviceVoltageList = null;
				this._lastSensorMaxValue = null;
				this._AllData4Access = null;
			}
		}
		public static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private Hashtable ht_All;
		private Hashtable ht_powerPD;
		private Hashtable ht_power;
		private Hashtable ht_pdDev;
		private Hashtable ht_pdBank;
		private Hashtable ht_pdPort;
		private Hashtable ht_pwDev;
		private Hashtable ht_pwBank;
		private Hashtable ht_pwPort;
		private Dictionary<int, string> _deviceVoltageList;
		private Dictionary<int, Dictionary<int, double>> _lastSensorMaxValue;
		private long _maxPacketLength;
		private static object _lockLog = new object();
		private static string _xmlFile = "Config.xml";
		private static string _logFile = "";
		private static Dictionary<string, string> _cfgFile = null;
		public MultiThreadUpdate()
		{
			this.ht_All = null;
			this.ht_powerPD = null;
			this.ht_power = null;
			this.ht_pdDev = null;
			this.ht_pdBank = null;
			this.ht_pdPort = null;
			this.ht_pwDev = null;
			this.ht_pwBank = null;
			this.ht_pwPort = null;
			if (!MultiThreadUpdate.sCodebase.EndsWith("/") && !MultiThreadUpdate.sCodebase.EndsWith("\\"))
			{
				MultiThreadUpdate.sCodebase += "\\";
			}
			this._deviceVoltageList = null;
			this._lastSensorMaxValue = null;
			string connectString = string.Concat(new object[]
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
			this._maxPacketLength = MultiThreadUpdate.GetMySqlPacketLength(connectString);
		}
		public void AddVoltageThread(Dictionary<int, string> deviceVoltageList)
		{
			this._deviceVoltageList = deviceVoltageList;
		}
		public void AddRackThermalThread(Dictionary<int, Dictionary<int, double>> lastSensorMaxValue)
		{
			this._lastSensorMaxValue = lastSensorMaxValue;
		}
		public void Execute(DateTime tDataTime, Hashtable data2DB, Hashtable existed)
		{
			if (data2DB == null || data2DB.Count <= 0)
			{
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
					if (this.ht_powerPD.ContainsKey("BANK"))
					{
						this.ht_pdBank = (Hashtable)this.ht_powerPD["BANK"];
						if (this.ht_pdBank.Count <= 0)
						{
							this.ht_pdBank = null;
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
					if (this.ht_power.ContainsKey("BANK"))
					{
						this.ht_pwBank = (Hashtable)this.ht_power["BANK"];
						if (this.ht_pwBank.Count <= 0)
						{
							this.ht_pwBank = null;
						}
					}
				}
			}
			string tableSuffix = tDataTime.ToString("yyyyMMdd");
			List<MultiThreadUpdate.UpdateContext> list = new List<MultiThreadUpdate.UpdateContext>();
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				this.ht_All = null;
				if (this.ht_pdDev != null && existed != null)
				{
					MultiThreadUpdate.UpdateContext updateContext = new MultiThreadUpdate.UpdateContext();
					updateContext._tDataTime = tDataTime;
					updateContext._sDataTime = tDataTime.ToString("yyyy-MM-dd HH:30:00");
					if (existed.ContainsKey("DEVICE_HOURLY"))
					{
						updateContext._htExisted = (Hashtable)existed["DEVICE_HOURLY"];
					}
					updateContext._htData = this.ht_pdDev;
					updateContext._dataRatio = 10000.0;
					updateContext._tablePrefix = "device_data_hourly";
					updateContext._tableSuffix = tableSuffix;
					updateContext._sKeyColumnName = "device_id";
					updateContext._sValueColumnName = "power_consumption";
					list.Add(updateContext);
					updateContext = new MultiThreadUpdate.UpdateContext();
					updateContext._tDataTime = tDataTime;
					updateContext._sDataTime = tDataTime.ToString("yyyy-MM-dd");
					if (existed.ContainsKey("DEVICE_DAILY"))
					{
						updateContext._htExisted = (Hashtable)existed["DEVICE_DAILY"];
					}
					updateContext._htData = this.ht_pdDev;
					updateContext._dataRatio = 10000.0;
					updateContext._tablePrefix = "device_data_daily";
					updateContext._tableSuffix = tableSuffix;
					updateContext._sKeyColumnName = "device_id";
					updateContext._sValueColumnName = "power_consumption";
					list.Add(updateContext);
				}
				if (this.ht_pdPort != null && existed != null)
				{
					MultiThreadUpdate.UpdateContext updateContext = new MultiThreadUpdate.UpdateContext();
					updateContext._tDataTime = tDataTime;
					updateContext._sDataTime = tDataTime.ToString("yyyy-MM-dd HH:30:00");
					updateContext._htData = this.ht_pdPort;
					updateContext._dataRatio = 10000.0;
					if (existed.ContainsKey("PORT_HOURLY"))
					{
						updateContext._htExisted = (Hashtable)existed["PORT_HOURLY"];
					}
					updateContext._tablePrefix = "port_data_hourly";
					updateContext._tableSuffix = tableSuffix;
					updateContext._sKeyColumnName = "port_id";
					updateContext._sValueColumnName = "power_consumption";
					list.Add(updateContext);
					updateContext = new MultiThreadUpdate.UpdateContext();
					updateContext._tDataTime = tDataTime;
					updateContext._sDataTime = tDataTime.ToString("yyyy-MM-dd");
					updateContext._htData = this.ht_pdPort;
					updateContext._dataRatio = 10000.0;
					if (existed.ContainsKey("PORT_DAILY"))
					{
						updateContext._htExisted = (Hashtable)existed["PORT_DAILY"];
					}
					updateContext._tablePrefix = "port_data_daily";
					updateContext._tableSuffix = tableSuffix;
					updateContext._sKeyColumnName = "port_id";
					updateContext._sValueColumnName = "power_consumption";
					list.Add(updateContext);
				}
				if (this.ht_pdBank != null && existed != null)
				{
					MultiThreadUpdate.UpdateContext updateContext = new MultiThreadUpdate.UpdateContext();
					updateContext._tDataTime = tDataTime;
					updateContext._sDataTime = tDataTime.ToString("yyyy-MM-dd HH:30:00");
					updateContext._htData = this.ht_pdBank;
					updateContext._dataRatio = 10000.0;
					if (existed.ContainsKey("BANK_HOURLY"))
					{
						updateContext._htExisted = (Hashtable)existed["BANK_HOURLY"];
					}
					updateContext._tablePrefix = "bank_data_hourly";
					updateContext._tableSuffix = tableSuffix;
					updateContext._sKeyColumnName = "bank_id";
					updateContext._sValueColumnName = "power_consumption";
					list.Add(updateContext);
					updateContext = new MultiThreadUpdate.UpdateContext();
					updateContext._tDataTime = tDataTime;
					updateContext._sDataTime = tDataTime.ToString("yyyy-MM-dd");
					updateContext._htData = this.ht_pdBank;
					updateContext._dataRatio = 10000.0;
					if (existed.ContainsKey("BANK_DAILY"))
					{
						updateContext._htExisted = (Hashtable)existed["BANK_DAILY"];
					}
					updateContext._tablePrefix = "bank_data_daily";
					updateContext._tableSuffix = tableSuffix;
					updateContext._sKeyColumnName = "bank_id";
					updateContext._sValueColumnName = "power_consumption";
					list.Add(updateContext);
				}
				if (this.ht_pwDev != null)
				{
					list.Add(new MultiThreadUpdate.UpdateContext
					{
						_tDataTime = tDataTime,
						_sDataTime = tDataTime.ToString("yyyy-MM-dd HH:mm:ss"),
						_htData = this.ht_pwDev,
						_dataRatio = 10000.0,
						_htExisted = null,
						_tablePrefix = "device_auto_info",
						_tableSuffix = tableSuffix,
						_sKeyColumnName = "device_id",
						_sValueColumnName = "power"
					});
				}
				if (this.ht_pwPort != null)
				{
					list.Add(new MultiThreadUpdate.UpdateContext
					{
						_tDataTime = tDataTime,
						_sDataTime = tDataTime.ToString("yyyy-MM-dd HH:mm:ss"),
						_htData = this.ht_pwPort,
						_dataRatio = 10000.0,
						_htExisted = null,
						_tablePrefix = "port_auto_info",
						_tableSuffix = tableSuffix,
						_sKeyColumnName = "port_id",
						_sValueColumnName = "power"
					});
				}
				if (this.ht_pwBank != null)
				{
					list.Add(new MultiThreadUpdate.UpdateContext
					{
						_tDataTime = tDataTime,
						_sDataTime = tDataTime.ToString("yyyy-MM-dd HH:mm:ss"),
						_htData = this.ht_pwBank,
						_dataRatio = 10000.0,
						_htExisted = null,
						_tablePrefix = "bank_auto_info",
						_tableSuffix = tableSuffix,
						_sKeyColumnName = "bank_id",
						_sValueColumnName = "power"
					});
				}
			}
			else
			{
				if (this.ht_All != null)
				{
					list.Add(new MultiThreadUpdate.UpdateContext
					{
						_tDataTime = tDataTime,
						_tablePrefix = "UpdateForAccess",
						_AllData4Access = this.ht_All
					});
				}
			}
			if (this._deviceVoltageList != null)
			{
				list.Add(new MultiThreadUpdate.UpdateContext
				{
					_tDataTime = tDataTime,
					_tablePrefix = "Voltage",
					_deviceVoltageList = this._deviceVoltageList
				});
			}
			if (this._lastSensorMaxValue != null)
			{
				list.Add(new MultiThreadUpdate.UpdateContext
				{
					_tDataTime = tDataTime,
					_tablePrefix = "SensorThermal",
					_lastSensorMaxValue = this._lastSensorMaxValue
				});
			}
			int nMaxConcurrentThreads = 4;
			string testOption = MultiThreadQuery.GetTestOption("update_thread_count");
			if (!string.IsNullOrEmpty(testOption))
			{
				nMaxConcurrentThreads = Convert.ToInt32(testOption);
			}
			ArrayList arrayList = new ArrayList();
			string text = MultiThreadQuery.GetTestOption("update_by_group");
			if (string.IsNullOrEmpty(text))
			{
				text = "port_data_hourly,port_data_daily,port_auto_info,bank_data_hourly,bank_data_daily,bank_auto_info,device_data_hourly,device_data_daily,device_auto_info,SensorThermal,Voltage";
			}
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new string[]
				{
					";"
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						arrayList.Add(array[i]);
					}
				}
			}
			InterProcessShared<Dictionary<string, string>>.setInterProcessKeyValue("MultiThreadUpdateStatus", "busy");
			DateTime now = DateTime.Now;
			for (int j = 0; j < arrayList.Count; j++)
			{
				string text2 = (string)arrayList[j];
				if (!string.IsNullOrEmpty(text2))
				{
					text2 = text2.Trim();
					if (!string.IsNullOrEmpty(text2))
					{
						string[] array2 = text2.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						List<MultiThreadUpdate.UpdateContext> list2 = new List<MultiThreadUpdate.UpdateContext>();
						for (int k = 0; k < array2.Length; k++)
						{
							string text3 = array2[k];
							if (!string.IsNullOrEmpty(text3))
							{
								text3 = text3.Trim();
								if (!string.IsNullOrEmpty(text3))
								{
									for (int l = list.Count - 1; l >= 0; l--)
									{
										if (text3.Equals(list[l]._tablePrefix, StringComparison.InvariantCultureIgnoreCase))
										{
											list2.Add(list[l]);
											list.RemoveAt(l);
										}
									}
								}
							}
						}
						if (list2.Count > 0)
						{
							this.ParallelUpdate(nMaxConcurrentThreads, list2);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				this.ParallelUpdate(nMaxConcurrentThreads, list);
			}
			InterProcessShared<Dictionary<string, string>>.setInterProcessKeyValue("MultiThreadUpdateStatus", "idle");
			TimeSpan timeSpan = DateTime.Now - now;
			MultiThreadUpdate.WriteLog("########## Update Finished, groups={0}, elapsed={1} ms", new string[]
			{
				arrayList.Count.ToString(),
				timeSpan.TotalMilliseconds.ToString()
			});
		}
		private static void WriteLog(string format, params string[] list)
		{
			lock (MultiThreadUpdate._lockLog)
			{
				if (!MultiThreadUpdate.sCodebase.EndsWith("/") && !MultiThreadUpdate.sCodebase.EndsWith("\\"))
				{
					MultiThreadUpdate.sCodebase += "\\";
				}
				if (MultiThreadUpdate._cfgFile == null)
				{
					MultiThreadUpdate._logFile = "";
					MultiThreadUpdate._cfgFile = ValuePairs.LoadValueKeyFromXML(MultiThreadUpdate._xmlFile);
					if (MultiThreadUpdate._cfgFile.ContainsKey("LogFile"))
					{
						MultiThreadUpdate._logFile = MultiThreadUpdate._cfgFile["LogFile"];
					}
				}
				if (MultiThreadUpdate._logFile != null && !(MultiThreadUpdate._logFile == ""))
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
						string text = MultiThreadUpdate.sCodebase + "\\debuglog\\" + MultiThreadUpdate._logFile;
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
		public static void ProcessMinuteData(Hashtable ht_data, int updatepower, DateTime DT_insert)
		{
			bool flag = false;
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection = null;
			OleDbDataAdapter oleDbDataAdapter = null;
			DataTable dataTable = new DataTable();
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string text2 = text + "datadb_" + DT_insert.ToString("yyyyMMdd") + ".mdb";
			if (!File.Exists(text2))
			{
				string sourceFileName = text + "datadb.org";
				File.Copy(sourceFileName, text2, true);
			}
			try
			{
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				oleDbCommand = oleDbConnection.CreateCommand();
				Hashtable hashtable = new Hashtable();
				Hashtable hashtable2 = new Hashtable();
				Hashtable hashtable3 = new Hashtable();
				Hashtable hashtable4 = new Hashtable();
				Hashtable hashtable5 = new Hashtable();
				Hashtable hashtable6 = new Hashtable();
				Hashtable hashtable7 = new Hashtable();
				Hashtable hashtable8 = new Hashtable();
				IEnumerator enumerator;
				if (updatepower > 0)
				{
					if (ht_data.ContainsKey("POWER"))
					{
						hashtable2 = (Hashtable)ht_data["POWER"];
					}
					if (hashtable2.ContainsKey("DEVICE"))
					{
						hashtable6 = (Hashtable)hashtable2["DEVICE"];
					}
					if (hashtable2.ContainsKey("BANK"))
					{
						hashtable7 = (Hashtable)hashtable2["BANK"];
					}
					if (hashtable2.ContainsKey("OUTLET"))
					{
						hashtable8 = (Hashtable)hashtable2["OUTLET"];
					}
					oleDbCommand.Parameters.Clear();
					oleDbCommand.CommandText = "insert into port_auto_info (port_id,power,insert_time ) values( ?,?,?)";
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
					oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
					oleDbCommand.Prepare();
					ICollection keys = hashtable8.Keys;
					MultiThreadUpdate.WriteLog("        Start insert port_auto_info ", new string[0]);
					enumerator = keys.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							int num = (int)enumerator.Current;
							try
							{
								oleDbCommand.Parameters[0].Value = num;
								double num2 = Convert.ToDouble(hashtable8[num]);
								long num3 = Convert.ToInt64(num2 * 10000.0);
								oleDbCommand.Parameters[1].Value = num3;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH:mm:ss");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					MultiThreadUpdate.WriteLog("        Start insert bank_auto_info ", new string[0]);
					oleDbCommand.Parameters.Clear();
					oleDbCommand.CommandText = "insert into bank_auto_info (bank_id,power,insert_time ) values( ?,?,?)";
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
					oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
					oleDbCommand.Prepare();
					ICollection keys2 = hashtable7.Keys;
					enumerator = keys2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							int num4 = (int)enumerator.Current;
							try
							{
								oleDbCommand.Parameters[0].Value = num4;
								double num5 = Convert.ToDouble(hashtable7[num4]);
								long num6 = Convert.ToInt64(num5 * 10000.0);
								oleDbCommand.Parameters[1].Value = num6;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH:mm:ss");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					MultiThreadUpdate.WriteLog("        Start insert device_auto_info ", new string[0]);
					oleDbCommand.Parameters.Clear();
					oleDbCommand.CommandText = "insert into device_auto_info (device_id,power,insert_time ) values( ?,?,?)";
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
					oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
					oleDbCommand.Prepare();
					ICollection keys3 = hashtable6.Keys;
					enumerator = keys3.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							int num7 = (int)enumerator.Current;
							try
							{
								oleDbCommand.Parameters[0].Value = num7;
								double num8 = Convert.ToDouble(hashtable6[num7]);
								long num9 = Convert.ToInt64(num8 * 10000.0);
								oleDbCommand.Parameters[1].Value = num9;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH:mm:ss");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					MultiThreadUpdate.WriteLog("        Finish insert all_auto_info ", new string[0]);
				}
				if (ht_data.ContainsKey("PD"))
				{
					hashtable = (Hashtable)ht_data["PD"];
				}
				if (hashtable.ContainsKey("DEVICE"))
				{
					hashtable3 = (Hashtable)hashtable["DEVICE"];
				}
				if (hashtable.ContainsKey("BANK"))
				{
					hashtable4 = (Hashtable)hashtable["BANK"];
				}
				if (hashtable.ContainsKey("OUTLET"))
				{
					hashtable5 = (Hashtable)hashtable["OUTLET"];
				}
				MultiThreadUpdate.WriteLog("        Start query port_data_daily ", new string[0]);
				dataTable = new DataTable();
				oleDbDataAdapter = new OleDbDataAdapter();
				oleDbCommand.Parameters.Clear();
				oleDbCommand.CommandText = "select port_id from port_data_daily where insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				oleDbCommand.Parameters[0].Value = DT_insert.ToString("yyyy-MM-dd");
				oleDbDataAdapter.SelectCommand = oleDbCommand;
				oleDbDataAdapter.Fill(dataTable);
				oleDbDataAdapter.Dispose();
				Hashtable hashtable9 = new Hashtable();
				if (dataTable != null)
				{
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							string text3 = Convert.ToString(dataRow[0]);
							if (!hashtable9.ContainsKey(text3))
							{
								hashtable9.Add(text3, text3);
							}
						}
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
				dataTable = new DataTable();
				MultiThreadUpdate.WriteLog("        Start update port_data_daily ", new string[0]);
				oleDbCommand.Parameters.Clear();
				oleDbCommand.CommandText = "update port_data_daily set power_consumption = power_consumption + ? where port_id = ? and insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				ICollection keys4 = hashtable5.Keys;
				enumerator = keys4.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num10 = (int)enumerator.Current;
						if (hashtable9.ContainsKey(string.Concat(num10)))
						{
							try
							{
								double num11 = Convert.ToDouble(hashtable5[num10]);
								long num12 = Convert.ToInt64(num11 * 10000.0);
								oleDbCommand.Parameters[0].Value = num12;
								oleDbCommand.Parameters[1].Value = num10;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start insert port_data_daily ", new string[0]);
				oleDbCommand.CommandText = "insert into port_data_daily (port_id,power_consumption,insert_time ) values( ?,?,?)";
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				enumerator = keys4.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num13 = (int)enumerator.Current;
						if (!hashtable9.ContainsKey(string.Concat(num13)))
						{
							try
							{
								double num14 = Convert.ToDouble(hashtable5[num13]);
								long num15 = Convert.ToInt64(num14 * 10000.0);
								oleDbCommand.Parameters[0].Value = num13;
								oleDbCommand.Parameters[1].Value = num15;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				MultiThreadUpdate.WriteLog("        Start query port_data_hourly ", new string[0]);
				dataTable = new DataTable();
				oleDbDataAdapter = new OleDbDataAdapter();
				oleDbCommand.Parameters.Clear();
				oleDbCommand.CommandText = "select port_id from port_data_hourly where insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				oleDbCommand.Parameters[0].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
				oleDbDataAdapter.SelectCommand = oleDbCommand;
				oleDbDataAdapter.Fill(dataTable);
				oleDbDataAdapter.Dispose();
				hashtable9 = new Hashtable();
				if (dataTable != null)
				{
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow2 = (DataRow)enumerator.Current;
							string text4 = Convert.ToString(dataRow2[0]);
							if (!hashtable9.ContainsKey(text4))
							{
								hashtable9.Add(text4, text4);
							}
						}
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
				dataTable = new DataTable();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start update port_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "update port_data_hourly set power_consumption = power_consumption + ? where port_id = ? and insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				ICollection keys5 = hashtable5.Keys;
				enumerator = keys4.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num16 = (int)enumerator.Current;
						if (hashtable9.ContainsKey(string.Concat(num16)))
						{
							try
							{
								double num17 = Convert.ToDouble(hashtable5[num16]);
								long num18 = Convert.ToInt64(num17 * 10000.0);
								oleDbCommand.Parameters[0].Value = num18;
								oleDbCommand.Parameters[1].Value = num16;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start insert port_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "insert into port_data_hourly (port_id,power_consumption,insert_time ) values( ?,?,?)";
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				enumerator = keys5.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num19 = (int)enumerator.Current;
						if (!hashtable9.ContainsKey(string.Concat(num19)))
						{
							try
							{
								double num20 = Convert.ToDouble(hashtable5[num19]);
								long num21 = Convert.ToInt64(num20 * 10000.0);
								oleDbCommand.Parameters[0].Value = num19;
								oleDbCommand.Parameters[1].Value = num21;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				dataTable = new DataTable();
				oleDbDataAdapter = new OleDbDataAdapter();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start query bank_data_daily ", new string[0]);
				oleDbCommand.CommandText = "select bank_id from bank_data_daily where insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				oleDbCommand.Parameters[0].Value = DT_insert.ToString("yyyy-MM-dd");
				oleDbDataAdapter.SelectCommand = oleDbCommand;
				oleDbDataAdapter.Fill(dataTable);
				oleDbDataAdapter.Dispose();
				hashtable9 = new Hashtable();
				if (dataTable != null)
				{
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow3 = (DataRow)enumerator.Current;
							string text5 = Convert.ToString(dataRow3[0]);
							if (!hashtable9.ContainsKey(text5))
							{
								hashtable9.Add(text5, text5);
							}
						}
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
				dataTable = new DataTable();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start update bank_data_daily ", new string[0]);
				oleDbCommand.CommandText = "update bank_data_daily set power_consumption = power_consumption + ? where bank_id = ? and insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				ICollection keys6 = hashtable4.Keys;
				enumerator = keys6.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num22 = (int)enumerator.Current;
						if (hashtable9.ContainsKey(string.Concat(num22)))
						{
							try
							{
								double num23 = Convert.ToDouble(hashtable4[num22]);
								long num24 = Convert.ToInt64(num23 * 10000.0);
								oleDbCommand.Parameters[0].Value = num24;
								oleDbCommand.Parameters[1].Value = num22;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start insert bank_data_daily ", new string[0]);
				oleDbCommand.CommandText = "insert into bank_data_daily (bank_id,power_consumption,insert_time ) values( ?,?,?)";
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				enumerator = keys6.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num25 = (int)enumerator.Current;
						if (!hashtable9.ContainsKey(string.Concat(num25)))
						{
							try
							{
								double num26 = Convert.ToDouble(hashtable4[num25]);
								long num27 = Convert.ToInt64(num26 * 10000.0);
								oleDbCommand.Parameters[0].Value = num25;
								oleDbCommand.Parameters[1].Value = num27;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				dataTable = new DataTable();
				oleDbDataAdapter = new OleDbDataAdapter();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start query bank_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "select bank_id from bank_data_hourly where insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				oleDbCommand.Parameters[0].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
				oleDbDataAdapter.SelectCommand = oleDbCommand;
				oleDbDataAdapter.Fill(dataTable);
				oleDbDataAdapter.Dispose();
				hashtable9 = new Hashtable();
				if (dataTable != null)
				{
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow4 = (DataRow)enumerator.Current;
							string text6 = Convert.ToString(dataRow4[0]);
							if (!hashtable9.ContainsKey(text6))
							{
								hashtable9.Add(text6, text6);
							}
						}
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
				dataTable = new DataTable();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start update bank_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "update bank_data_hourly set power_consumption = power_consumption + ? where bank_id = ? and insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				keys6 = hashtable4.Keys;
				enumerator = keys6.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num28 = (int)enumerator.Current;
						if (hashtable9.ContainsKey(string.Concat(num28)))
						{
							try
							{
								double num29 = Convert.ToDouble(hashtable4[num28]);
								long num30 = Convert.ToInt64(num29 * 10000.0);
								oleDbCommand.Parameters[0].Value = num30;
								oleDbCommand.Parameters[1].Value = num28;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start insert bank_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "insert into bank_data_hourly (bank_id,power_consumption,insert_time ) values( ?,?,?)";
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				enumerator = keys6.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num31 = (int)enumerator.Current;
						if (!hashtable9.ContainsKey(string.Concat(num31)))
						{
							try
							{
								double num32 = Convert.ToDouble(hashtable4[num31]);
								long num33 = Convert.ToInt64(num32 * 10000.0);
								oleDbCommand.Parameters[0].Value = num31;
								oleDbCommand.Parameters[1].Value = num33;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				dataTable = new DataTable();
				oleDbDataAdapter = new OleDbDataAdapter();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start query device_data_daily ", new string[0]);
				oleDbCommand.CommandText = "select device_id from device_data_daily where insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				oleDbCommand.Parameters[0].Value = DT_insert.ToString("yyyy-MM-dd");
				oleDbDataAdapter.SelectCommand = oleDbCommand;
				oleDbDataAdapter.Fill(dataTable);
				oleDbDataAdapter.Dispose();
				hashtable9 = new Hashtable();
				if (dataTable != null)
				{
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow5 = (DataRow)enumerator.Current;
							string text7 = Convert.ToString(dataRow5[0]);
							if (!hashtable9.ContainsKey(text7))
							{
								hashtable9.Add(text7, text7);
							}
						}
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
				dataTable = new DataTable();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start update device_data_daily ", new string[0]);
				oleDbCommand.CommandText = "update device_data_daily set power_consumption = power_consumption + ? where device_id = ? and insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				ICollection keys7 = hashtable3.Keys;
				enumerator = keys7.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num34 = (int)enumerator.Current;
						if (hashtable9.ContainsKey(string.Concat(num34)))
						{
							try
							{
								double num35 = Convert.ToDouble(hashtable3[num34]);
								long num36 = Convert.ToInt64(num35 * 10000.0);
								oleDbCommand.Parameters[0].Value = num36;
								oleDbCommand.Parameters[1].Value = num34;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start insert device_data_daily ", new string[0]);
				oleDbCommand.CommandText = "insert into device_data_daily (device_id,power_consumption,insert_time ) values( ?,?,?)";
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				enumerator = keys7.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num37 = (int)enumerator.Current;
						if (!hashtable9.ContainsKey(string.Concat(num37)))
						{
							try
							{
								double num38 = Convert.ToDouble(hashtable3[num37]);
								long num39 = Convert.ToInt64(num38 * 10000.0);
								oleDbCommand.Parameters[0].Value = num37;
								oleDbCommand.Parameters[1].Value = num39;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd");
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				dataTable = new DataTable();
				oleDbDataAdapter = new OleDbDataAdapter();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start query device_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "select device_id from device_data_hourly where insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				oleDbCommand.Parameters[0].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
				oleDbDataAdapter.SelectCommand = oleDbCommand;
				oleDbDataAdapter.Fill(dataTable);
				oleDbDataAdapter.Dispose();
				hashtable9 = new Hashtable();
				if (dataTable != null)
				{
					enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow6 = (DataRow)enumerator.Current;
							string text8 = Convert.ToString(dataRow6[0]);
							if (!hashtable9.ContainsKey(text8))
							{
								hashtable9.Add(text8, text8);
							}
						}
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
				dataTable = new DataTable();
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start update device_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "update device_data_hourly set power_consumption = power_consumption + ? where device_id = ? and insert_time = ? ";
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				keys6 = hashtable4.Keys;
				enumerator = keys7.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num40 = (int)enumerator.Current;
						if (hashtable9.ContainsKey(string.Concat(num40)))
						{
							try
							{
								double num41 = Convert.ToDouble(hashtable3[num40]);
								long num42 = Convert.ToInt64(num41 * 10000.0);
								oleDbCommand.Parameters[0].Value = num42;
								oleDbCommand.Parameters[1].Value = num40;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				oleDbCommand.Parameters.Clear();
				MultiThreadUpdate.WriteLog("        Start insert device_data_hourly ", new string[0]);
				oleDbCommand.CommandText = "insert into device_data_hourly (device_id,power_consumption,insert_time ) values( ?,?,?)";
				oleDbCommand.Parameters.Add("?", OleDbType.Integer);
				oleDbCommand.Parameters.Add("?", OleDbType.BigInt);
				oleDbCommand.Parameters.Add("?", OleDbType.DBTimeStamp);
				oleDbCommand.Prepare();
				enumerator = keys7.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int num43 = (int)enumerator.Current;
						if (!hashtable9.ContainsKey(string.Concat(num43)))
						{
							try
							{
								double num44 = Convert.ToDouble(hashtable3[num43]);
								long num45 = Convert.ToInt64(num44 * 10000.0);
								oleDbCommand.Parameters[0].Value = num43;
								oleDbCommand.Parameters[1].Value = num45;
								oleDbCommand.Parameters[2].Value = DT_insert.ToString("yyyy-MM-dd HH") + ":30:00";
								oleDbCommand.ExecuteNonQuery();
							}
							catch
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				if (oleDbDataAdapter != null)
				{
					try
					{
						oleDbDataAdapter.Dispose();
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
			}
			catch
			{
				if (oleDbDataAdapter != null)
				{
					try
					{
						oleDbDataAdapter.Dispose();
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
				flag = true;
			}
			if (flag)
			{
				DBTools.Write_DBERROR_Log();
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
		private void OneTableUpdate(object obj)
		{
			MultiThreadUpdate.UpdateContext updateContext = (MultiThreadUpdate.UpdateContext)obj;
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			MultiThreadUpdate.WriteLog(string.Format("EEEEEEEEEEEEEEEEE [{0}] Table: {1}", Thread.CurrentThread.Name, updateContext._tablePrefix + updateContext._tableSuffix), new string[0]);
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				if (updateContext._deviceVoltageList != null)
				{
					DeviceOperation.UpdateDeviceVoltage(updateContext._deviceVoltageList);
				}
				else
				{
					if (updateContext._lastSensorMaxValue != null)
					{
						RackInfo.GenerateAllRackThermal(updateContext._lastSensorMaxValue, updateContext._tDataTime);
					}
					else
					{
						if (updateContext._AllData4Access != null)
						{
							MultiThreadUpdate.ProcessMinuteData(updateContext._AllData4Access, 1, updateContext._tDataTime);
						}
						else
						{
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
							}
							catch (Exception ex)
							{
								if (ex.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
								{
									DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
									DBTools.Write_DBERROR_Log();
									DBUtil.StopService();
									return;
								}
							}
							try
							{
								string format = string.Format("insert into {0}{1} ({2},{3},insert_time) values ({{{4}}},{{{5}}},'{6}')", new object[]
								{
									updateContext._tablePrefix,
									updateContext._tableSuffix,
									updateContext._sKeyColumnName,
									updateContext._sValueColumnName,
									"0",
									"1",
									updateContext._sDataTime
								});
								string format2 = string.Format("update {0}{1} set {2}={3}+{{{4}}} where {5}={{{6}}} and insert_time='{7}'", new object[]
								{
									updateContext._tablePrefix,
									updateContext._tableSuffix,
									updateContext._sValueColumnName,
									updateContext._sValueColumnName,
									"1",
									updateContext._sKeyColumnName,
									"0",
									updateContext._sDataTime
								});
								string text = "";
								StringBuilder stringBuilder = new StringBuilder();
								int count = updateContext._htData.Count;
								int num = 0;
								foreach (DictionaryEntry dictionaryEntry in updateContext._htData)
								{
									num++;
									try
									{
										double value = updateContext._dataRatio * Convert.ToDouble(dictionaryEntry.Value);
										string arg = string.Format("{0}", Convert.ToInt64(value));
										text = "";
										if (updateContext._htExisted != null && updateContext._htExisted.ContainsKey(dictionaryEntry.Key))
										{
											string text2 = string.Format(format2, dictionaryEntry.Key, arg);
											dbCommand.CommandText = text2;
											text = text2;
											dbCommand.ExecuteNonQuery();
										}
										else
										{
											string text2 = string.Format(format, dictionaryEntry.Key, arg);
											if ((long)(stringBuilder.Length + text2.Length) > this._maxPacketLength / 2L)
											{
												dbCommand.CommandText = stringBuilder.ToString();
												text = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
												stringBuilder.Clear();
											}
											if (stringBuilder.Length == 0)
											{
												stringBuilder.Append(text2);
											}
											else
											{
												int num2 = text2.LastIndexOf("values");
												if (num2 >= 0)
												{
													stringBuilder.Append(",");
													stringBuilder.Append(text2.Substring(num2 + 6));
												}
											}
										}
										if (num >= count && stringBuilder.Length > 0)
										{
											dbCommand.CommandText = stringBuilder.ToString();
											text = dbCommand.CommandText;
											dbCommand.ExecuteNonQuery();
											stringBuilder.Clear();
										}
									}
									catch (Exception ex2)
									{
										if (ex2.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
										{
											try
											{
												if (!string.IsNullOrEmpty(text))
												{
													dbCommand.CommandText = text;
													dbCommand.ExecuteNonQuery();
												}
											}
											catch (Exception ex3)
											{
												MultiThreadUpdate.WriteLog("Updating: {0}{1}, Error:{2}", new string[]
												{
													updateContext._tablePrefix,
													updateContext._tableSuffix,
													ex2.Message
												});
												if (!string.IsNullOrEmpty(text) && ex3.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
												{
													string tableName = DBUtil.GetTableName(text);
													if (tableName != null && tableName.Length > 0)
													{
														DBUtil.SetMySQLInfo(tableName);
													}
													MultiThreadUpdate.WriteLog("        MySQL database may be crashed, EcoSensor Monitor Service will shutdown ", new string[0]);
													DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex3.Message + "\n" + ex3.StackTrace);
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
				if (updateContext._htData != null)
				{
					MultiThreadUpdate.WriteLog(string.Format("----------------- [{0}] finished, count={1}, elapsed={2}", Thread.CurrentThread.Name + " " + updateContext._tablePrefix, updateContext._htData.Count, tickTimer.getElapsed().ToString()), new string[0]);
				}
				else
				{
					MultiThreadUpdate.WriteLog(string.Format("----------------- [{0}] finished, elapsed={1}", Thread.CurrentThread.Name + " " + updateContext._tablePrefix, tickTimer.getElapsed().ToString()), new string[0]);
				}
				if (updateContext._finishSignal != null)
				{
					updateContext._finishSignal.Release();
				}
				if (updateContext._finishCounter != null)
				{
					updateContext._finishCounter.Release();
				}
			}
		}
		private bool ParallelUpdate(int nMaxConcurrentThreads, List<MultiThreadUpdate.UpdateContext> contexts)
		{
			if (contexts.Count <= 0)
			{
				return false;
			}
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			List<MultiThreadUpdate.UpdateContext> list = new List<MultiThreadUpdate.UpdateContext>();
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
				MultiThreadUpdate.UpdateContext updateContext = list[0];
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
				updateContext._finishSignal = semaphore;
				updateContext._finishCounter = semaphoreSlim;
				thread.Start(updateContext);
			}
			try
			{
				while (semaphoreSlim.CurrentCount < contexts.Count)
				{
					Thread.Sleep(20);
				}
				MultiThreadUpdate.WriteLog("########## All updates done: elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			}
			catch (Exception ex)
			{
				MultiThreadUpdate.WriteLog(ex.Message, new string[0]);
			}
			return true;
		}
	}
}
