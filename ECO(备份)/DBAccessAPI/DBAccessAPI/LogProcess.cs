using CommonAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Threading;
namespace DBAccessAPI
{
	public class LogProcess : IDisposable
	{
		protected object _lockHandler = new object();
		private string _threadName = "Log Processor";
		private DbConnection _con;
		private DateTime _create_time;
		private int _stopping;
		private Thread _procThread;
		private ManualResetEvent _stoppedEvent = new ManualResetEvent(true);
		private ManualResetEvent _abortEvent = new ManualResetEvent(false);
		private WaitHandle[] _waitHandles;
		private ManualResetEvent _eventMessage;
		private Queue<List<string>> _queueLogContent;
		public void Dispose()
		{
		}
		public LogProcess()
		{
			WaitHandle[] waitHandles = new WaitHandle[3];
			this._waitHandles = waitHandles;
			this._eventMessage = new ManualResetEvent(false);
			this._queueLogContent = new Queue<List<string>>();
			base..ctor();
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			this._eventMessage.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = this._eventMessage;
			this._waitHandles[2] = LogDBStatus.stopDBEvent;
			this._queueLogContent.Clear();
		}
		public bool Start()
		{
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			this._eventMessage.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = this._eventMessage;
			this._waitHandles[2] = LogDBStatus.stopDBEvent;
			this._queueLogContent.Clear();
			this._procThread = new Thread(new ParameterizedThreadStart(this.WorkThread));
			this._procThread.Name = this._threadName;
			this._procThread.Start();
			return true;
		}
		public void Stop()
		{
			DebugCenter.GetInstance().appendToFile("Stopping Log Processor " + this._threadName + " thread");
			Interlocked.Exchange(ref this._stopping, 1);
			if (this._abortEvent != null)
			{
				this._abortEvent.Set();
			}
			try
			{
				if (!this._stoppedEvent.WaitOne(3000))
				{
					DebugCenter.GetInstance().appendToFile("Abort a dead " + this._threadName + " thread");
					this._procThread.Abort();
				}
				this._procThread.Join(500);
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile(ex.Message);
			}
			Interlocked.Exchange(ref this._stopping, 0);
			DebugCenter.GetInstance().appendToFile(this._threadName + " stopped");
		}
		public virtual void PutLog(List<string> lt_logcontent)
		{
			lock (this._lockHandler)
			{
				if (lt_logcontent != null && this._queueLogContent != null && lt_logcontent.Count > 0)
				{
					this._queueLogContent.Enqueue(lt_logcontent);
					this._eventMessage.Set();
				}
			}
		}
		private void WorkThread(object state)
		{
			DbCommand dbCommand = null;
			try
			{
				int num = 0;
				while (this._stopping == 0)
				{
					num = WaitHandle.WaitAny(this._waitHandles, 500);
					if (num == 0)
					{
						break;
					}
					if (num == 1)
					{
						if (DBUrl.SERVERMODE)
						{
							List<string> list = null;
							if (this._con != null)
							{
								if (this._con.State == ConnectionState.Open)
								{
									goto IL_130;
								}
							}
							try
							{
								string cONNECT_STRING = DBUrl.CONNECT_STRING;
								string[] array = cONNECT_STRING.Split(new string[]
								{
									","
								}, StringSplitOptions.RemoveEmptyEntries);
								this._con = new MySqlConnection(string.Concat(new string[]
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
								this._con.Open();
								this._create_time = DateTime.Now;
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
								continue;
							}
							IL_130:
							lock (this._lockHandler)
							{
								if (this._queueLogContent.Count > 0)
								{
									list = this._queueLogContent.Dequeue();
								}
								else
								{
									this._eventMessage.Reset();
								}
							}
							if (list == null)
							{
								continue;
							}
							string text = "";
							string obj_value = list[0];
							string value = list[1];
							try
							{
								for (int i = 2; i < list.Count; i++)
								{
									if (i == list.Count - 1)
									{
										text += list[i];
									}
									else
									{
										text = text + list[i] + "^|^";
									}
								}
								dbCommand = this._con.CreateCommand();
								dbCommand.CommandText = "insert into logrecords (ticks,eventid,logpara ) values(?ticks,?eventid,?logpara) ";
								dbCommand.Parameters.Add(DBTools.GetParameter("?ticks", Convert.ToDateTime(value), dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?eventid", obj_value, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?logpara", text, dbCommand));
								dbCommand.ExecuteNonQuery();
								dbCommand.Parameters.Clear();
								dbCommand.Dispose();
							}
							catch (Exception)
							{
							}
							if (Math.Abs((DateTime.Now - this._create_time).TotalSeconds) <= 10.0)
							{
								goto IL_530;
							}
							try
							{
								this._con.Close();
								this._con = null;
								goto IL_530;
							}
							catch (Exception)
							{
								goto IL_530;
							}
						}
						if (LogDBStatus.GetDBStatus() != 1)
						{
							Thread.Sleep(50);
							break;
						}
						List<string> list2 = null;
						lock (this._lockHandler)
						{
							if (this._queueLogContent.Count > 0)
							{
								list2 = this._queueLogContent.Dequeue();
							}
							else
							{
								this._eventMessage.Reset();
							}
						}
						if (list2 == null)
						{
							continue;
						}
						if (this._con == null || this._con.State != ConnectionState.Open)
						{
							if (this._stopping == 1)
							{
								return;
							}
							try
							{
								this._con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb;Jet OLEDB:Database Password=ecoSensorlog");
								this._con.Open();
								this._create_time = DateTime.Now;
							}
							catch (Exception ex2)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
								continue;
							}
						}
						string text2 = "";
						string obj_value2 = list2[0];
						string value2 = list2[1];
						try
						{
							if (this._stopping == 1)
							{
								return;
							}
							for (int j = 2; j < list2.Count; j++)
							{
								if (j == list2.Count - 1)
								{
									text2 += list2[j];
								}
								else
								{
									text2 = text2 + list2[j] + "^|^";
								}
							}
							dbCommand = this._con.CreateCommand();
							if (this._stopping == 1)
							{
								return;
							}
							dbCommand.CommandText = "insert into logrecords (ticks,eventid,logpara ) values(?,?,?) ";
							dbCommand.Parameters.Add(DBTools.GetParameter("@ticks", Convert.ToDateTime(value2), dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@eventid", obj_value2, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@logpara", text2, dbCommand));
							if (this._stopping == 1)
							{
								return;
							}
							dbCommand.ExecuteNonQuery();
							if (this._stopping == 1)
							{
								return;
							}
							dbCommand.Parameters.Clear();
							dbCommand.Dispose();
						}
						catch (Exception)
						{
						}
						if (Math.Abs((DateTime.Now - this._create_time).TotalSeconds) > 10.0)
						{
							try
							{
								this._con.Close();
								this._con = null;
							}
							catch (Exception)
							{
							}
						}
					}
					IL_530:
					if (num == 2)
					{
						LogDBStatus.stopDBEvent.Reset();
						try
						{
							this._con.Close();
							this._con = null;
						}
						catch (Exception)
						{
						}
						LogDBStatus.SetDBStatus(-2);
					}
				}
				DebugCenter.GetInstance().appendToFile("[" + this._threadName + "] thread end");
				this._stoppedEvent.Set();
			}
			catch
			{
			}
			finally
			{
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
				try
				{
					if (this._con != null)
					{
						this._con.Close();
					}
				}
				catch
				{
				}
				this._stoppedEvent.Set();
			}
		}
		public void OnStart()
		{
		}
		public virtual void OnClose()
		{
		}
	}
}
