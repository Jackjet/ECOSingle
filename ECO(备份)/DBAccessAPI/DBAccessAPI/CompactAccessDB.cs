using CommonAPI;
using JRO;
using System;
using System.Data.OleDb;
using System.IO;
using System.Threading;
namespace DBAccessAPI
{
	public class CompactAccessDB
	{
		private string _srcDatabasePath;
		private string _dstDatabasePath;
		private string _password;
		private int _threadDone;
		private bool _result;
		private Thread thCompactThread;
		public CompactAccessDB()
		{
			this._srcDatabasePath = "";
			this._dstDatabasePath = "";
			this._password = "";
			Interlocked.Exchange(ref this._threadDone, 1);
		}
		public bool Start(string srcDBFile, string dstDBFile, string pass)
		{
			this._srcDatabasePath = srcDBFile;
			this._dstDatabasePath = dstDBFile;
			this._password = pass;
			Interlocked.Exchange(ref this._threadDone, 0);
			try
			{
				this.thCompactThread = new Thread(new ThreadStart(this.CompactThread));
				this.thCompactThread.Start();
				return true;
			}
			catch (ThreadStateException)
			{
			}
			catch (ThreadInterruptedException)
			{
			}
			return false;
		}
		public bool getResult()
		{
			return this._result;
		}
		public bool isDone()
		{
			return this._threadDone > 0;
		}
		public void WaitDone(int seconds)
		{
			int tickCount = Environment.TickCount;
			while (this._threadDone == 0)
			{
				if (seconds > 0)
				{
					int tickCount2 = Environment.TickCount;
					if (tickCount2 >= tickCount)
					{
						if (tickCount2 - tickCount > seconds * 1000 && this.thCompactThread != null)
						{
							this.thCompactThread.Interrupt();
							this.thCompactThread.Abort();
							break;
						}
					}
					else
					{
						tickCount = Environment.TickCount;
					}
				}
				try
				{
					Thread.Sleep(100);
				}
				catch (ThreadInterruptedException)
				{
				}
			}
			Interlocked.Exchange(ref this._threadDone, 1);
		}
		public void Stop()
		{
			this.WaitDone(1000);
			this.thCompactThread = null;
		}
		private void CompactThread()
		{
			Interlocked.Exchange(ref this._threadDone, 0);
			int arg_12_0 = Environment.TickCount;
			try
			{
				File.Delete(this._dstDatabasePath);
				this._result = this.CompactDB(this._srcDatabasePath, this._dstDatabasePath, this._password);
				string text = "";
				if (this._result)
				{
					text = this._dstDatabasePath;
				}
				else
				{
					text = this._srcDatabasePath;
				}
				int num = DBTools.ShudownAccess();
				if (num > 0)
				{
					try
					{
						if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb") && File.Exists(text))
						{
							string text2 = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyyMMddHHmmss") + "_tmpdatadb.mdb";
							bool flag = true;
							int num2 = 0;
							while (flag && num2 < 5)
							{
								try
								{
									FileInfo fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb");
									fileInfo.MoveTo(text2);
									FileInfo fileInfo2 = new FileInfo(text);
									fileInfo2.MoveTo(AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb");
									flag = false;
									TaskStatus.TMPDB_path = text2;
									try
									{
										File.Delete(TaskStatus.DB_compact_tmp);
										TaskStatus.DB_compact_tmp = "";
									}
									catch
									{
									}
									TaskStatus.SetDBStatus(1);
								}
								catch (Exception)
								{
								}
								num2++;
							}
							if (flag)
							{
								TaskStatus.SetDBStatus(1);
							}
							else
							{
								OleDbConnection oleDbConnection = null;
								OleDbCommand oleDbCommand = new OleDbCommand();
								try
								{
									string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TaskStatus.TMPDB_path + ";Jet OLEDB:Database Password=root";
									oleDbConnection = new OleDbConnection(connectionString);
									oleDbConnection.Open();
									oleDbCommand = oleDbConnection.CreateCommand();
									try
									{
										oleDbCommand.CommandText = "select device_id,power_consumption,insert_time from device_data_daily ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item = string.Concat(new string[]
											{
												"update device_data_daily set power_consumption = power_consumption + ",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												" where device_id = ",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												" and insert_time = #",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd"),
												"#"
											});
											WorkQueue<string>.getInstance_pd().WorkSequential = true;
											WorkQueue<string>.getInstance_pd().EnqueueItem(item);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select device_id,power_consumption,insert_time from device_data_hourly ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item2 = string.Concat(new string[]
											{
												"update device_data_hourly set power_consumption = power_consumption + ",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												" where device_id = ",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												" and insert_time = #",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd HH:mm:ss"),
												"#"
											});
											WorkQueue<string>.getInstance_pd().WorkSequential = true;
											WorkQueue<string>.getInstance_pd().EnqueueItem(item2);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select bank_id,power_consumption,insert_time from bank_data_hourly ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item3 = string.Concat(new string[]
											{
												"update bank_data_hourly set power_consumption = power_consumption + ",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												" where bank_id = ",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												" and insert_time = #",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd HH:mm:ss"),
												"#"
											});
											WorkQueue<string>.getInstance_pd().WorkSequential = true;
											WorkQueue<string>.getInstance_pd().EnqueueItem(item3);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select bank_id,power_consumption,insert_time from bank_data_daily ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item4 = string.Concat(new string[]
											{
												"update bank_data_daily set power_consumption = power_consumption + ",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												" where bank_id = ",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												" and insert_time = #",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd"),
												"#"
											});
											WorkQueue<string>.getInstance_pd().WorkSequential = true;
											WorkQueue<string>.getInstance_pd().EnqueueItem(item4);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select port_id,power_consumption,insert_time from port_data_daily ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item5 = string.Concat(new string[]
											{
												"update port_data_daily set power_consumption = power_consumption + ",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												" where port_id = ",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												" and insert_time = #",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd"),
												"#"
											});
											WorkQueue<string>.getInstance_pd().WorkSequential = true;
											WorkQueue<string>.getInstance_pd().EnqueueItem(item5);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select port_id,power_consumption,insert_time from port_data_hourly ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item6 = string.Concat(new string[]
											{
												"update port_data_hourly set power_consumption = power_consumption + ",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												" where port_id = ",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												" and insert_time = #",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd HH:mm:ss"),
												"#"
											});
											WorkQueue<string>.getInstance_pd().WorkSequential = true;
											WorkQueue<string>.getInstance_pd().EnqueueItem(item6);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select device_id,power,insert_time from device_auto_info ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item7 = string.Concat(new string[]
											{
												"insert into device_auto_info (device_id,power,insert_time ) values(",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												",",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												",#",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd HH:mm:ss"),
												"#)"
											});
											WorkQueue<string>.getInstance().WorkSequential = true;
											WorkQueue<string>.getInstance().EnqueueItem(item7);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select bank_id,power,insert_time from bank_auto_info ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item8 = string.Concat(new string[]
											{
												"insert into bank_auto_info (bank_id,power,insert_time ) values(",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												",",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												",#",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd HH:mm:ss"),
												"#)"
											});
											WorkQueue<string>.getInstance().WorkSequential = true;
											WorkQueue<string>.getInstance().EnqueueItem(item8);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									try
									{
										oleDbCommand.CommandText = "select port_id,power,insert_time from port_auto_info ";
										OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
										while (oleDbDataReader.Read())
										{
											string item9 = string.Concat(new string[]
											{
												"insert into port_auto_info (port_id,power,insert_time ) values(",
												Convert.ToString(oleDbDataReader.GetValue(0)),
												",",
												Convert.ToString(oleDbDataReader.GetValue(1)),
												",#",
												Convert.ToDateTime(oleDbDataReader.GetValue(2)).ToString("yyyy-MM-dd HH:mm:ss"),
												"#)"
											});
											WorkQueue<string>.getInstance().WorkSequential = true;
											WorkQueue<string>.getInstance().EnqueueItem(item9);
										}
										oleDbDataReader.Close();
									}
									catch
									{
									}
									oleDbCommand.Dispose();
									oleDbConnection.Close();
									try
									{
										TaskStatus.DB_compact_final = "";
										File.Delete(TaskStatus.TMPDB_path);
									}
									catch
									{
									}
									DebugCenter.GetInstance().appendToFile("Finish compact Access Database ");
								}
								catch (Exception)
								{
									oleDbCommand.Dispose();
									if (oleDbConnection != null)
									{
										oleDbConnection.Close();
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
			catch (Exception)
			{
			}
			Interlocked.Exchange(ref this._threadDone, 1);
		}
		private bool CompactDB(string srcDB, string dstDB, string pass)
		{
			try
			{
				string sourceConnection = string.Concat(new string[]
				{
					"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=",
					srcDB,
					"; Jet OLEDB:Database Password=",
					pass,
					";"
				});
				string destconnection = string.Concat(new string[]
				{
					"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=",
					dstDB,
					"; Jet OLEDB:Database Password=",
					pass,
					";"
				});
				JetEngine jetEngine = (JetEngine)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("DE88C160-FF2C-11D1-BB6F-00C04FAE22DA")));
				jetEngine.CompactDatabase(sourceConnection, destconnection);
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
