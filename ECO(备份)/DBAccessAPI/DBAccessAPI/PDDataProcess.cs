using CommonAPI;
using Microsoft.Office.Interop.Access.Dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
namespace DBAccessAPI
{
	public class PDDataProcess
	{
		public const int STATUS_IDLE = 0;
		public const int STATUS_START = 1;
		public const int STATUS_RUNNING = 2;
		public const int STATUS_FINISH = 3;
		private static PDDataProcess mdp_instance;
		protected object _lockHandler = new object();
		private bool update_pd;
		private bool update_power;
		private int i_trace;
		private List<StringBuilder> lt_delete_dd = new List<StringBuilder>();
		private List<StringBuilder> lt_delete_dh = new List<StringBuilder>();
		private List<StringBuilder> lt_delete_bd = new List<StringBuilder>();
		private List<StringBuilder> lt_delete_bh = new List<StringBuilder>();
		private List<StringBuilder> lt_delete_pd = new List<StringBuilder>();
		private List<StringBuilder> lt_delete_ph = new List<StringBuilder>();
		private StringBuilder sb_dd = new StringBuilder();
		private StringBuilder sb_dh = new StringBuilder();
		private StringBuilder sb_bd = new StringBuilder();
		private StringBuilder sb_bh = new StringBuilder();
		private StringBuilder sb_pd = new StringBuilder();
		private StringBuilder sb_ph = new StringBuilder();
		private DataTable dt_dp = new DataTable();
		private DataTable dt_dd = new DataTable();
		private DataTable dt_dh = new DataTable();
		private DataTable dt_bp = new DataTable();
		private DataTable dt_bd = new DataTable();
		private DataTable dt_bh = new DataTable();
		private DataTable dt_pp = new DataTable();
		private DataTable dt_pd = new DataTable();
		private DataTable dt_ph = new DataTable();
		private Hashtable ht_ddpd = new Hashtable();
		private Hashtable ht_dhpd = new Hashtable();
		private Hashtable ht_bdpd = new Hashtable();
		private Hashtable ht_bhpd = new Hashtable();
		private Hashtable ht_pdpd = new Hashtable();
		private Hashtable ht_phpd = new Hashtable();
		private string _threadName = "MinuteData Processor";
		private int i_processor_status;
		private DateTime _taskstart_time;
		private int _stopping;
		private Thread _procThread;
		private ManualResetEvent _stoppedEvent = new ManualResetEvent(true);
		private ManualResetEvent _abortEvent = new ManualResetEvent(false);
		private WaitHandle[] _waitHandles;
		private ManualResetEvent _eventMessage;
		private Queue<List<string>> _queueData;
		public static PDDataProcess GetInstance()
		{
			if (PDDataProcess.mdp_instance == null)
			{
				PDDataProcess.mdp_instance = new PDDataProcess();
				PDDataProcess.mdp_instance.Start();
			}
			return PDDataProcess.mdp_instance;
		}
		public void Dispose()
		{
		}
		private PDDataProcess()
		{
			WaitHandle[] waitHandles = new WaitHandle[2];
			this._waitHandles = waitHandles;
			this._eventMessage = new ManualResetEvent(false);
			this._queueData = new Queue<List<string>>();
			base..ctor();
			Interlocked.Exchange(ref this._stopping, 0);
			this.dt_dp = new DataTable();
			this.dt_dd = new DataTable();
			this.dt_dh = new DataTable();
			this.dt_bp = new DataTable();
			this.dt_bd = new DataTable();
			this.dt_bh = new DataTable();
			this.dt_pp = new DataTable();
			this.dt_pd = new DataTable();
			this.dt_ph = new DataTable();
			this.ht_ddpd = new Hashtable();
			this.ht_dhpd = new Hashtable();
			this.ht_bdpd = new Hashtable();
			this.ht_bhpd = new Hashtable();
			this.ht_pdpd = new Hashtable();
			this.ht_phpd = new Hashtable();
			this.sb_dd = new StringBuilder();
			this.sb_dh = new StringBuilder();
			this.sb_bd = new StringBuilder();
			this.sb_bh = new StringBuilder();
			this.sb_pd = new StringBuilder();
			this.sb_ph = new StringBuilder();
			this.lt_delete_dd = new List<StringBuilder>();
			this.lt_delete_dh = new List<StringBuilder>();
			this.lt_delete_bd = new List<StringBuilder>();
			this.lt_delete_bh = new List<StringBuilder>();
			this.lt_delete_pd = new List<StringBuilder>();
			this.lt_delete_ph = new List<StringBuilder>();
			this.update_pd = false;
			this.update_power = false;
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			this._eventMessage.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = this._eventMessage;
			this._queueData.Clear();
		}
		public bool Start()
		{
			this.update_pd = false;
			this.update_power = false;
			this.sb_dd = new StringBuilder();
			this.sb_dh = new StringBuilder();
			this.sb_bd = new StringBuilder();
			this.sb_bh = new StringBuilder();
			this.sb_pd = new StringBuilder();
			this.sb_ph = new StringBuilder();
			this.dt_dp = new DataTable();
			this.dt_dd = new DataTable();
			this.dt_dh = new DataTable();
			this.dt_bp = new DataTable();
			this.dt_bd = new DataTable();
			this.dt_bh = new DataTable();
			this.dt_pp = new DataTable();
			this.dt_pd = new DataTable();
			this.dt_ph = new DataTable();
			this.ht_ddpd = new Hashtable();
			this.ht_dhpd = new Hashtable();
			this.ht_bdpd = new Hashtable();
			this.ht_bhpd = new Hashtable();
			this.ht_pdpd = new Hashtable();
			this.ht_phpd = new Hashtable();
			this.lt_delete_dd = new List<StringBuilder>();
			this.lt_delete_dh = new List<StringBuilder>();
			this.lt_delete_bd = new List<StringBuilder>();
			this.lt_delete_bh = new List<StringBuilder>();
			this.lt_delete_pd = new List<StringBuilder>();
			this.lt_delete_ph = new List<StringBuilder>();
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			this._eventMessage.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = this._eventMessage;
			this._queueData.Clear();
			this._procThread = new Thread(new ParameterizedThreadStart(this.WorkThread));
			this._procThread.Name = this._threadName;
			this._procThread.Start();
			return true;
		}
		public void Stop()
		{
			DebugCenter.GetInstance().appendToFile("Stopping MinuteData Processor " + this._threadName + " thread");
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
		public virtual void PutItem(List<string> lt_datacontent)
		{
			lock (this._lockHandler)
			{
				try
				{
					if (lt_datacontent != null && this._queueData != null && lt_datacontent.Count > 0)
					{
						this._queueData.Enqueue(lt_datacontent);
						this._eventMessage.Set();
					}
				}
				catch (Exception)
				{
				}
			}
		}
		public long GetQueueCount()
		{
			return (long)this._queueData.Count;
		}
		private void WorkThread(object state)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				while (this._stopping == 0)
				{
					int num = WaitHandle.WaitAny(this._waitHandles, 500);
					if (num == 0)
					{
						break;
					}
					if (num == 1)
					{
						List<string> list = null;
						lock (this._lockHandler)
						{
							if (this._queueData.Count > 0)
							{
								list = this._queueData.Dequeue();
							}
							else
							{
								this._eventMessage.Reset();
							}
						}
						if (list != null)
						{
							string text = list[0];
							string s = list[1];
							DateTime dateTime = DateTime.Parse(s);
							if (text.Equals("POWERSTART"))
							{
								Random random = new Random((int)DateTime.Now.Ticks);
								this.i_trace = random.Next(0, 111111);
								DebugCenter.GetInstance().appendToFile("<.><.><.> Begin update Power Data (" + this.i_trace + ")");
								this.update_power = true;
								this.preparedatabase_pd(dateTime, false);
							}
							else
							{
								if (text.Equals("PDSTART"))
								{
									this.update_pd = true;
									this.preparedatabase_pd(dateTime, true);
								}
								else
								{
									if (text.Equals("BOTHSTART"))
									{
										this.update_pd = true;
										this.update_power = true;
										this.preparedatabase_pd(dateTime, true);
									}
									else
									{
										if (text.Equals("END"))
										{
											try
											{
												if (this.sb_dd.Length > 0)
												{
													this.lt_delete_dd.Add(this.sb_dd);
													this.sb_dd = new StringBuilder();
												}
												if (this.sb_dh.Length > 0)
												{
													this.lt_delete_dh.Add(this.sb_dh);
													this.sb_dh = new StringBuilder();
												}
												if (this.sb_bd.Length > 0)
												{
													this.lt_delete_bd.Add(this.sb_bd);
													this.sb_bd = new StringBuilder();
												}
												if (this.sb_bh.Length > 0)
												{
													this.lt_delete_bh.Add(this.sb_bh);
													this.sb_bh = new StringBuilder();
												}
												if (this.sb_pd.Length > 0)
												{
													this.lt_delete_pd.Add(this.sb_pd);
													this.sb_pd = new StringBuilder();
												}
												if (this.sb_ph.Length > 0)
												{
													this.lt_delete_ph.Add(this.sb_ph);
													this.sb_ph = new StringBuilder();
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
												string text3 = text2 + "datadb_" + dateTime.ToString("yyyyMMdd") + ".mdb";
												DBEngine dBEngine = null;
												try
												{
													bool flag2 = true;
													try
													{
														dBEngine = (DBEngine)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("CD7791B9-43FD-42C5-AE42-8DD2811F0419")));
													}
													catch (Exception)
													{
														flag2 = false;
													}
													if (flag2)
													{
														Database database = dBEngine.OpenDatabase(text3, false, false, ";pwd=root");
														if (this.update_pd)
														{
															foreach (StringBuilder current in this.lt_delete_dd)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																database.Execute("DELETE FROM device_data_daily where device_id in (" + current.ToString() + " ) ", Missing.Value);
															}
															foreach (StringBuilder current2 in this.lt_delete_bd)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																database.Execute("DELETE FROM bank_data_daily where bank_id in (" + current2.ToString() + " ) ", Missing.Value);
															}
															foreach (StringBuilder current3 in this.lt_delete_pd)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																database.Execute("DELETE FROM port_data_daily where port_id in (" + current3.ToString() + " ) ", Missing.Value);
															}
															foreach (StringBuilder current4 in this.lt_delete_dh)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																database.Execute(string.Concat(new string[]
																{
																	"DELETE FROM device_data_hourly where insert_time = #",
																	dateTime.ToString("yyyy-MM-dd HH"),
																	":30:00# and device_id in (",
																	current4.ToString(),
																	" )"
																}), Missing.Value);
															}
															foreach (StringBuilder current5 in this.lt_delete_bh)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																database.Execute(string.Concat(new string[]
																{
																	"DELETE FROM bank_data_hourly where insert_time = #",
																	dateTime.ToString("yyyy-MM-dd HH"),
																	":30:00# and bank_id in (",
																	current5.ToString(),
																	" )"
																}), Missing.Value);
															}
															foreach (StringBuilder current6 in this.lt_delete_ph)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																database.Execute(string.Concat(new string[]
																{
																	"DELETE FROM port_data_hourly where insert_time = #",
																	dateTime.ToString("yyyy-MM-dd HH"),
																	":30:00# and port_id in (",
																	current6.ToString(),
																	" )"
																}), Missing.Value);
															}
														}
														if (this.update_power)
														{
															bool flag3 = false;
															Recordset recordset = database.OpenRecordset("device_auto_info", Missing.Value, Missing.Value, Missing.Value);
															Field[] array = new Field[this.dt_dp.Columns.Count];
															for (int i = 0; i < this.dt_dp.Rows.Count; i++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset.AddNew();
																for (int j = 0; j < this.dt_dp.Columns.Count; j++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag3)
																	{
																		array[j] = recordset.Fields[this.dt_dp.Columns[j].ColumnName];
																	}
																	array[j].Value = this.dt_dp.Rows[i][j];
																}
																recordset.Update(1, false);
																flag3 = true;
															}
															recordset.Close();
															flag3 = false;
															recordset = database.OpenRecordset("bank_auto_info", Missing.Value, Missing.Value, Missing.Value);
															array = new Field[this.dt_bp.Columns.Count];
															for (int k = 0; k < this.dt_bp.Rows.Count; k++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset.AddNew();
																for (int l = 0; l < this.dt_bp.Columns.Count; l++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag3)
																	{
																		array[l] = recordset.Fields[this.dt_bp.Columns[l].ColumnName];
																	}
																	array[l].Value = this.dt_bp.Rows[k][l];
																}
																recordset.Update(1, false);
																flag3 = true;
															}
															recordset.Close();
															flag3 = false;
															recordset = database.OpenRecordset("port_auto_info", Missing.Value, Missing.Value, Missing.Value);
															array = new Field[this.dt_pp.Columns.Count];
															for (int m = 0; m < this.dt_pp.Rows.Count; m++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset.AddNew();
																for (int n = 0; n < this.dt_pp.Columns.Count; n++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag3)
																	{
																		array[n] = recordset.Fields[this.dt_pp.Columns[n].ColumnName];
																	}
																	array[n].Value = this.dt_pp.Rows[m][n];
																}
																recordset.Update(1, false);
																flag3 = true;
															}
															recordset.Close();
															DBCacheStatus.LastInsertTime = DateTime.Now;
														}
														if (this.update_pd)
														{
															bool flag4 = false;
															Recordset recordset2 = database.OpenRecordset("device_data_daily", Missing.Value, Missing.Value, Missing.Value);
															Field[] array2 = new Field[this.dt_dd.Columns.Count];
															for (int num2 = 0; num2 < this.dt_dd.Rows.Count; num2++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset2.AddNew();
																for (int num3 = 0; num3 < this.dt_dd.Columns.Count; num3++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag4)
																	{
																		array2[num3] = recordset2.Fields[this.dt_dd.Columns[num3].ColumnName];
																	}
																	array2[num3].Value = this.dt_dd.Rows[num2][num3];
																}
																recordset2.Update(1, false);
																flag4 = true;
															}
															recordset2.Close();
															flag4 = false;
															recordset2 = database.OpenRecordset("device_data_hourly", Missing.Value, Missing.Value, Missing.Value);
															array2 = new Field[this.dt_dh.Columns.Count];
															for (int num4 = 0; num4 < this.dt_dh.Rows.Count; num4++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset2.AddNew();
																for (int num5 = 0; num5 < this.dt_dh.Columns.Count; num5++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag4)
																	{
																		array2[num5] = recordset2.Fields[this.dt_dh.Columns[num5].ColumnName];
																	}
																	if (num5 == 2)
																	{
																		DateTime dateTime2 = Convert.ToDateTime(this.dt_dh.Rows[num4][num5]);
																		DateTime dateTime3 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, dateTime2.Hour, 30, 0);
																		array2[num5].Value = dateTime3;
																	}
																	else
																	{
																		array2[num5].Value = this.dt_dh.Rows[num4][num5];
																	}
																}
																recordset2.Update(1, false);
																flag4 = true;
															}
															recordset2.Close();
															flag4 = false;
															recordset2 = database.OpenRecordset("bank_data_hourly", Missing.Value, Missing.Value, Missing.Value);
															array2 = new Field[this.dt_bh.Columns.Count];
															for (int num6 = 0; num6 < this.dt_bh.Rows.Count; num6++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset2.AddNew();
																for (int num7 = 0; num7 < this.dt_bh.Columns.Count; num7++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag4)
																	{
																		array2[num7] = recordset2.Fields[this.dt_bh.Columns[num7].ColumnName];
																	}
																	if (num7 == 2)
																	{
																		DateTime dateTime4 = Convert.ToDateTime(this.dt_bh.Rows[num6][num7]);
																		DateTime dateTime5 = new DateTime(dateTime4.Year, dateTime4.Month, dateTime4.Day, dateTime4.Hour, 30, 0);
																		array2[num7].Value = dateTime5;
																	}
																	else
																	{
																		array2[num7].Value = this.dt_bh.Rows[num6][num7];
																	}
																}
																recordset2.Update(1, false);
																flag4 = true;
															}
															recordset2.Close();
															flag4 = false;
															recordset2 = database.OpenRecordset("bank_data_daily", Missing.Value, Missing.Value, Missing.Value);
															array2 = new Field[this.dt_bd.Columns.Count];
															for (int num8 = 0; num8 < this.dt_bd.Rows.Count; num8++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset2.AddNew();
																for (int num9 = 0; num9 < this.dt_bd.Columns.Count; num9++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag4)
																	{
																		array2[num9] = recordset2.Fields[this.dt_bd.Columns[num9].ColumnName];
																	}
																	array2[num9].Value = this.dt_bd.Rows[num8][num9];
																}
																recordset2.Update(1, false);
																flag4 = true;
															}
															recordset2.Close();
															flag4 = false;
															recordset2 = database.OpenRecordset("port_data_daily", Missing.Value, Missing.Value, Missing.Value);
															array2 = new Field[this.dt_pd.Columns.Count];
															for (int num10 = 0; num10 < this.dt_pd.Rows.Count; num10++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset2.AddNew();
																for (int num11 = 0; num11 < this.dt_pd.Columns.Count; num11++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag4)
																	{
																		array2[num11] = recordset2.Fields[this.dt_pd.Columns[num11].ColumnName];
																	}
																	array2[num11].Value = this.dt_pd.Rows[num10][num11];
																}
																recordset2.Update(1, false);
																flag4 = true;
															}
															recordset2.Close();
															flag4 = false;
															recordset2 = database.OpenRecordset("port_data_hourly", Missing.Value, Missing.Value, Missing.Value);
															array2 = new Field[this.dt_ph.Columns.Count];
															for (int num12 = 0; num12 < this.dt_ph.Rows.Count; num12++)
															{
																if (this._stopping == 1)
																{
																	return;
																}
																recordset2.AddNew();
																for (int num13 = 0; num13 < this.dt_ph.Columns.Count; num13++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	if (!flag4)
																	{
																		array2[num13] = recordset2.Fields[this.dt_ph.Columns[num13].ColumnName];
																	}
																	if (num13 == 2)
																	{
																		DateTime dateTime6 = Convert.ToDateTime(this.dt_ph.Rows[num12][num13]);
																		DateTime dateTime7 = new DateTime(dateTime6.Year, dateTime6.Month, dateTime6.Day, dateTime6.Hour, 30, 0);
																		array2[num13].Value = dateTime7;
																	}
																	else
																	{
																		array2[num13].Value = this.dt_ph.Rows[num12][num13];
																	}
																}
																recordset2.Update(1, false);
																flag4 = true;
															}
															recordset2.Close();
														}
														database.Close();
													}
													else
													{
														try
														{
															dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text3 + ";Jet OLEDB:Database Password=root");
															dbConnection.Open();
															dbCommand = dbConnection.CreateCommand();
															if (this.update_power)
															{
																for (int num14 = 0; num14 < this.dt_dp.Rows.Count; num14++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime8 = (DateTime)this.dt_dp.Rows[num14][2];
																	dbCommand.CommandText = string.Concat(new object[]
																	{
																		"insert into device_auto_info  (device_id,power,insert_time ) values(",
																		this.dt_dp.Rows[num14][0],
																		",",
																		this.dt_dp.Rows[num14][1],
																		",#",
																		dateTime8.ToString("yyyy-MM-dd HH:mm:ss"),
																		"#)"
																	});
																	dbCommand.ExecuteNonQuery();
																}
																for (int num15 = 0; num15 < this.dt_bp.Rows.Count; num15++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime9 = (DateTime)this.dt_bp.Rows[num15][2];
																	dbCommand.CommandText = string.Concat(new object[]
																	{
																		"insert into bank_auto_info (bank_id,power,insert_time ) values(",
																		this.dt_bp.Rows[num15][0],
																		",",
																		this.dt_bp.Rows[num15][1],
																		",#",
																		dateTime9.ToString("yyyy-MM-dd HH:mm:ss"),
																		"#)"
																	});
																	dbCommand.ExecuteNonQuery();
																}
																for (int num16 = 0; num16 < this.dt_pp.Rows.Count; num16++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime10 = (DateTime)this.dt_pp.Rows[num16][2];
																	dbCommand.CommandText = string.Concat(new object[]
																	{
																		"insert into port_auto_info (port_id,power,insert_time ) values(",
																		this.dt_pp.Rows[num16][0],
																		",",
																		this.dt_pp.Rows[num16][1],
																		",#",
																		dateTime10.ToString("yyyy-MM-dd HH:mm:ss"),
																		"#)"
																	});
																	dbCommand.ExecuteNonQuery();
																}
															}
															if (this.update_pd)
															{
																for (int num17 = 0; num17 < this.dt_dd.Rows.Count; num17++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime11 = (DateTime)this.dt_dd.Rows[num17][2];
																	if (this.ht_ddpd.ContainsKey(Convert.ToString(this.dt_dd.Rows[num17][0])))
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"update device_data_daily set power_consumption = ",
																			this.dt_dd.Rows[num17][1],
																			" where device_id = ",
																			this.dt_dd.Rows[num17][0],
																			" and insert_time = #",
																			dateTime11.ToString("yyyy-MM-dd"),
																			"#"
																		});
																	}
																	else
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"insert into device_data_daily (device_id,power_consumption,insert_time ) values(",
																			this.dt_dd.Rows[num17][0],
																			",",
																			this.dt_dd.Rows[num17][1],
																			",#",
																			dateTime11.ToString("yyyy-MM-dd"),
																			"#)"
																		});
																	}
																	dbCommand.ExecuteNonQuery();
																}
																for (int num18 = 0; num18 < this.dt_dh.Rows.Count; num18++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime12 = (DateTime)this.dt_dh.Rows[num18][2];
																	if (this.ht_dhpd.ContainsKey(Convert.ToString(this.dt_dh.Rows[num18][0])))
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"update device_data_hourly set power_consumption = ",
																			this.dt_dh.Rows[num18][1],
																			" where device_id = ",
																			this.dt_dh.Rows[num18][0],
																			" and insert_time = #",
																			dateTime12.ToString("yyyy-MM-dd HH"),
																			":30:00#"
																		});
																	}
																	else
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"insert into device_data_hourly (device_id,power_consumption,insert_time ) values(",
																			this.dt_dh.Rows[num18][0],
																			",",
																			this.dt_dh.Rows[num18][1],
																			",#",
																			dateTime12.ToString("yyyy-MM-dd HH"),
																			":30:00#)"
																		});
																	}
																	dbCommand.ExecuteNonQuery();
																}
																for (int num19 = 0; num19 < this.dt_bd.Rows.Count; num19++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime13 = (DateTime)this.dt_bd.Rows[num19][2];
																	if (this.ht_bdpd.ContainsKey(Convert.ToString(this.dt_bd.Rows[num19][0])))
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"update bank_data_daily set power_consumption = ",
																			this.dt_bd.Rows[num19][1],
																			" where bank_id = ",
																			this.dt_bd.Rows[num19][0],
																			" and insert_time = #",
																			dateTime13.ToString("yyyy-MM-dd"),
																			"#"
																		});
																	}
																	else
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"insert into bank_data_daily (bank_id,power_consumption,insert_time ) values(",
																			this.dt_bd.Rows[num19][0],
																			",",
																			this.dt_bd.Rows[num19][1],
																			",#",
																			dateTime13.ToString("yyyy-MM-dd"),
																			"#)"
																		});
																	}
																	dbCommand.ExecuteNonQuery();
																}
																for (int num20 = 0; num20 < this.dt_bh.Rows.Count; num20++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime14 = (DateTime)this.dt_bh.Rows[num20][2];
																	if (this.ht_bhpd.ContainsKey(Convert.ToString(this.dt_bh.Rows[num20][0])))
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"update bank_data_hourly set power_consumption = ",
																			this.dt_bh.Rows[num20][1],
																			" where bank_id = ",
																			this.dt_bh.Rows[num20][0],
																			" and insert_time = #",
																			dateTime14.ToString("yyyy-MM-dd HH"),
																			":30:00#"
																		});
																	}
																	else
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"insert into bank_data_hourly (bank_id,power_consumption,insert_time ) values(",
																			this.dt_bh.Rows[num20][0],
																			",",
																			this.dt_bh.Rows[num20][1],
																			",#",
																			dateTime14.ToString("yyyy-MM-dd HH"),
																			":30:00#)"
																		});
																	}
																	dbCommand.ExecuteNonQuery();
																}
																for (int num21 = 0; num21 < this.dt_pd.Rows.Count; num21++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime15 = (DateTime)this.dt_pd.Rows[num21][2];
																	if (this.ht_pdpd.ContainsKey(Convert.ToString(this.dt_pd.Rows[num21][0])))
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"update port_data_daily set power_consumption = ",
																			this.dt_pd.Rows[num21][1],
																			" where port_id = ",
																			this.dt_pd.Rows[num21][0],
																			" and insert_time = #",
																			dateTime15.ToString("yyyy-MM-dd"),
																			"#"
																		});
																	}
																	else
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"insert into port_data_daily (port_id,power_consumption,insert_time ) values(",
																			this.dt_pd.Rows[num21][0],
																			",",
																			this.dt_pd.Rows[num21][1],
																			",#",
																			dateTime15.ToString("yyyy-MM-dd"),
																			"#)"
																		});
																	}
																	dbCommand.ExecuteNonQuery();
																}
																for (int num22 = 0; num22 < this.dt_ph.Rows.Count; num22++)
																{
																	if (this._stopping == 1)
																	{
																		return;
																	}
																	DateTime dateTime16 = (DateTime)this.dt_ph.Rows[num22][2];
																	if (this.ht_phpd.ContainsKey(Convert.ToString(this.dt_ph.Rows[num22][0])))
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"update port_data_hourly set power_consumption = ",
																			this.dt_ph.Rows[num22][1],
																			" where port_id = ",
																			this.dt_ph.Rows[num22][0],
																			" and insert_time = #",
																			dateTime16.ToString("yyyy-MM-dd HH"),
																			":30:00#"
																		});
																	}
																	else
																	{
																		dbCommand.CommandText = string.Concat(new object[]
																		{
																			"insert into port_data_hourly (port_id,power_consumption,insert_time ) values(",
																			this.dt_ph.Rows[num22][0],
																			",",
																			this.dt_ph.Rows[num22][1],
																			",#",
																			dateTime16.ToString("yyyy-MM-dd HH"),
																			":30:00#)"
																		});
																	}
																	dbCommand.ExecuteNonQuery();
																}
															}
															dbCommand.Dispose();
															dbConnection.Close();
														}
														catch (Exception ex)
														{
															try
															{
																DBTools.Write_DBERROR_Log();
															}
															catch
															{
															}
															DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Process MinuteData Error : " + ex.Message + "\n" + ex.StackTrace);
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
														}
													}
													this.update_pd = false;
													this.update_power = false;
												}
												catch (Exception ex2)
												{
													DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Prepare DataDB Error : " + ex2.Message + "\n" + ex2.StackTrace);
												}
												finally
												{
													if (dBEngine != null)
													{
														Marshal.ReleaseComObject(dBEngine);
													}
													dBEngine = null;
												}
												DBCacheStatus.LastInsertTime = DateTime.Now;
												DBCacheStatus.PDDataDBReady = true;
												DebugCenter.GetInstance().appendToFile("<.><.><.> Finish update Power Data (" + this.i_trace + ")");
												continue;
											}
											catch (Exception)
											{
												if (this.update_power)
												{
													DBCacheStatus.LastInsertTime = DateTime.Now;
												}
												DBCacheStatus.PDDataDBReady = true;
												DebugCenter.GetInstance().appendToFile("<.><.><.> Finish update Power Data (" + this.i_trace + ")");
												continue;
											}
										}
										try
										{
											this.i_processor_status = 2;
											string text4 = list[2];
											string value = list[3];
											string key;
											switch (key = text4)
											{
											case "device_auto_info":
											{
												DataRow dataRow = this.dt_dp.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												dataRow[1] = Convert.ToInt32(value);
												dataRow[2] = dateTime;
												this.dt_dp.Rows.Add(dataRow);
												break;
											}
											case "device_data_daily":
											{
												DataRow dataRow = this.dt_dd.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												if (this.ht_ddpd.ContainsKey(text))
												{
													int num24 = Convert.ToInt32(this.ht_ddpd[text]);
													num24 += Convert.ToInt32(value);
													dataRow[1] = num24;
													if (this.sb_dd.Length > 1024)
													{
														this.lt_delete_dd.Add(this.sb_dd);
														this.sb_dd = new StringBuilder();
														this.sb_dd.Append(text);
													}
													else
													{
														if (this.sb_dd.Length == 0)
														{
															this.sb_dd.Append(text);
														}
														else
														{
															this.sb_dd.Append("," + text);
														}
													}
												}
												else
												{
													dataRow[1] = Convert.ToInt32(value);
												}
												dataRow[2] = dateTime;
												this.dt_dd.Rows.Add(dataRow);
												break;
											}
											case "device_data_hourly":
											{
												DataRow dataRow = this.dt_dh.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												if (this.ht_dhpd.ContainsKey(text))
												{
													int num25 = Convert.ToInt32(this.ht_dhpd[text]);
													num25 += Convert.ToInt32(value);
													dataRow[1] = num25;
													if (this.sb_dh.Length > 1024)
													{
														this.lt_delete_dh.Add(this.sb_dh);
														this.sb_dh = new StringBuilder();
														this.sb_dh.Append(text);
													}
													else
													{
														if (this.sb_dh.Length == 0)
														{
															this.sb_dh.Append(text);
														}
														else
														{
															this.sb_dh.Append("," + text);
														}
													}
												}
												else
												{
													dataRow[1] = Convert.ToInt32(value);
												}
												dataRow[2] = dateTime;
												this.dt_dh.Rows.Add(dataRow);
												break;
											}
											case "bank_auto_info":
											{
												DataRow dataRow = this.dt_bp.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												dataRow[1] = Convert.ToInt32(value);
												dataRow[2] = dateTime;
												this.dt_bp.Rows.Add(dataRow);
												break;
											}
											case "bank_data_daily":
											{
												DataRow dataRow = this.dt_bd.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												if (this.ht_bdpd.ContainsKey(text))
												{
													int num26 = Convert.ToInt32(this.ht_bdpd[text]);
													num26 += Convert.ToInt32(value);
													dataRow[1] = num26;
													if (this.sb_bd.Length > 1024)
													{
														this.lt_delete_bd.Add(this.sb_bd);
														this.sb_bd = new StringBuilder();
														this.sb_bd.Append(text);
													}
													else
													{
														if (this.sb_bd.Length == 0)
														{
															this.sb_bd.Append(text);
														}
														else
														{
															this.sb_bd.Append("," + text);
														}
													}
												}
												else
												{
													dataRow[1] = Convert.ToInt32(value);
												}
												dataRow[2] = dateTime;
												this.dt_bd.Rows.Add(dataRow);
												break;
											}
											case "bank_data_hourly":
											{
												DataRow dataRow = this.dt_bh.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												if (this.ht_bhpd.ContainsKey(text))
												{
													int num27 = Convert.ToInt32(this.ht_bhpd[text]);
													num27 += Convert.ToInt32(value);
													dataRow[1] = num27;
													if (this.sb_bh.Length > 1024)
													{
														this.lt_delete_bh.Add(this.sb_bh);
														this.sb_bh = new StringBuilder();
														this.sb_bh.Append(text);
													}
													else
													{
														if (this.sb_bh.Length == 0)
														{
															this.sb_bh.Append(text);
														}
														else
														{
															this.sb_bh.Append("," + text);
														}
													}
												}
												else
												{
													dataRow[1] = Convert.ToInt32(value);
												}
												dataRow[2] = dateTime;
												this.dt_bh.Rows.Add(dataRow);
												break;
											}
											case "port_auto_info":
											{
												DataRow dataRow = this.dt_pp.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												dataRow[1] = Convert.ToInt32(value);
												dataRow[2] = dateTime;
												this.dt_pp.Rows.Add(dataRow);
												break;
											}
											case "port_data_daily":
											{
												DataRow dataRow = this.dt_pd.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												if (this.ht_pdpd.ContainsKey(text))
												{
													int num28 = Convert.ToInt32(this.ht_pdpd[text]);
													num28 += Convert.ToInt32(value);
													dataRow[1] = num28;
													if (this.sb_pd.Length > 1024)
													{
														this.lt_delete_pd.Add(this.sb_pd);
														this.sb_pd = new StringBuilder();
														this.sb_pd.Append(text);
													}
													else
													{
														if (this.sb_pd.Length == 0)
														{
															this.sb_pd.Append(text);
														}
														else
														{
															this.sb_pd.Append("," + text);
														}
													}
												}
												else
												{
													dataRow[1] = Convert.ToInt32(value);
												}
												dataRow[2] = dateTime;
												this.dt_pd.Rows.Add(dataRow);
												break;
											}
											case "port_data_hourly":
											{
												DataRow dataRow = this.dt_ph.NewRow();
												dataRow[0] = Convert.ToInt32(text);
												if (this.ht_phpd.ContainsKey(text))
												{
													int num29 = Convert.ToInt32(this.ht_phpd[text]);
													num29 += Convert.ToInt32(value);
													dataRow[1] = num29;
													if (this.sb_ph.Length > 1024)
													{
														this.lt_delete_ph.Add(this.sb_ph);
														this.sb_ph = new StringBuilder();
														this.sb_ph.Append(text);
													}
													else
													{
														if (this.sb_ph.Length == 0)
														{
															this.sb_ph.Append(text);
														}
														else
														{
															this.sb_ph.Append("," + text);
														}
													}
												}
												else
												{
													dataRow[1] = Convert.ToInt32(value);
												}
												dataRow[2] = dateTime;
												this.dt_ph.Rows.Add(dataRow);
												break;
											}
											}
										}
										catch (Exception)
										{
											try
											{
												DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
												{
													"Received data was not correct : ",
													list[0],
													" ",
													list[1],
													" ",
													list[2],
													" ",
													list[3]
												}));
											}
											catch
											{
											}
										}
									}
								}
							}
						}
					}
				}
				DBCacheStatus.PDDataDBReady = true;
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
					if (dbConnection != null)
					{
						dbConnection.Close();
					}
				}
				catch
				{
				}
				this._stoppedEvent.Set();
			}
		}
		private void preparedatabase_pd(DateTime dt_inserttime, bool b_pd)
		{
			try
			{
				this.sb_dd = new StringBuilder();
				this.sb_dh = new StringBuilder();
				this.sb_bd = new StringBuilder();
				this.sb_bh = new StringBuilder();
				this.sb_pd = new StringBuilder();
				this.sb_ph = new StringBuilder();
				this.lt_delete_dd = new List<StringBuilder>();
				this.lt_delete_dh = new List<StringBuilder>();
				this.lt_delete_bd = new List<StringBuilder>();
				this.lt_delete_bh = new List<StringBuilder>();
				this.lt_delete_pd = new List<StringBuilder>();
				this.lt_delete_ph = new List<StringBuilder>();
				this.dt_dp = new DataTable("device_auto_info");
				DataColumn dataColumn = new DataColumn();
				dataColumn.ColumnName = "device_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_dp.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_dp.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_dp.Columns.Add(dataColumn);
				this.dt_dd = new DataTable("device_data_daily");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "device_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_dd.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power_consumption";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_dd.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_dd.Columns.Add(dataColumn);
				this.dt_dh = new DataTable("device_data_hourly");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "device_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_dh.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power_consumption";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_dh.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_dh.Columns.Add(dataColumn);
				this.dt_bp = new DataTable("bank_auto_info");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "bank_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_bp.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_bp.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_bp.Columns.Add(dataColumn);
				this.dt_bd = new DataTable("bank_data_daily");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "bank_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_bd.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power_consumption";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_bd.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_bd.Columns.Add(dataColumn);
				this.dt_bh = new DataTable("bank_data_hourly");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "bank_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_bh.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power_consumption";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_bh.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_bh.Columns.Add(dataColumn);
				this.dt_pp = new DataTable("port_auto_info");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "port_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_pp.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_pp.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_pp.Columns.Add(dataColumn);
				this.dt_pd = new DataTable("port_data_daily");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "port_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_pd.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power_consumption";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_pd.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_pd.Columns.Add(dataColumn);
				this.dt_ph = new DataTable("port_data_hourly");
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "port_id";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_ph.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "power_consumption";
				dataColumn.DataType = Type.GetType("System.Int32");
				this.dt_ph.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.ColumnName = "insert_time";
				dataColumn.DataType = Type.GetType("System.DateTime");
				this.dt_ph.Columns.Add(dataColumn);
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
				if (b_pd)
				{
					this.ht_ddpd = new Hashtable();
					this.ht_dhpd = new Hashtable();
					this.ht_bdpd = new Hashtable();
					this.ht_bhpd = new Hashtable();
					this.ht_pdpd = new Hashtable();
					this.ht_phpd = new Hashtable();
				}
				if (!File.Exists(text2))
				{
					string sourceFileName = text + "datadb.org";
					File.Copy(sourceFileName, text2, true);
				}
				else
				{
					if (b_pd)
					{
						DataTable dataTable = new DataTable();
						DBConn dBConn = null;
						DbCommand dbCommand = null;
						DbDataAdapter dbDataAdapter = null;
						try
						{
							bool b_create = true;
							dBConn = DBConnPool.getDynaConnection(dt_inserttime, b_create);
							if (dBConn != null && dBConn.con != null)
							{
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand = dBConn.con.CreateCommand();
								dbCommand.CommandText = "select device_id,power_consumption,insert_time from device_data_daily order by device_id ASC ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									this.ht_ddpd = new Hashtable();
									foreach (DataRow dataRow in dataTable.Rows)
									{
										string key = Convert.ToString(dataRow[0]);
										long num = Convert.ToInt64(dataRow[1]);
										if (this.ht_ddpd.ContainsKey(key))
										{
											long num2 = Convert.ToInt64(this.ht_ddpd[key]);
											this.ht_ddpd[key] = num2 + num;
										}
										else
										{
											this.ht_ddpd.Add(key, num);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand.CommandText = "select device_id,power_consumption,insert_time from device_data_hourly where insert_time = #" + dt_inserttime.ToString("yyyy-MM-dd HH") + ":30:00# order by device_id ASC ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									this.ht_dhpd = new Hashtable();
									foreach (DataRow dataRow2 in dataTable.Rows)
									{
										string key2 = Convert.ToString(dataRow2[0]);
										long num3 = Convert.ToInt64(dataRow2[1]);
										if (this.ht_dhpd.ContainsKey(key2))
										{
											long num4 = Convert.ToInt64(this.ht_dhpd[key2]);
											this.ht_dhpd[key2] = num4 + num3;
										}
										else
										{
											this.ht_dhpd.Add(key2, num3);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand.CommandText = "select bank_id,power_consumption,insert_time from bank_data_daily order by bank_id ASC ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									this.ht_bdpd = new Hashtable();
									foreach (DataRow dataRow3 in dataTable.Rows)
									{
										string key3 = Convert.ToString(dataRow3[0]);
										long num5 = Convert.ToInt64(dataRow3[1]);
										if (this.ht_bdpd.ContainsKey(key3))
										{
											long num6 = Convert.ToInt64(this.ht_bdpd[key3]);
											this.ht_bdpd[key3] = num6 + num5;
										}
										else
										{
											this.ht_bdpd.Add(key3, num5);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand.CommandText = "select bank_id,power_consumption,insert_time from bank_data_hourly where insert_time = #" + dt_inserttime.ToString("yyyy-MM-dd HH") + ":30:00# order by bank_id ASC ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									this.ht_bhpd = new Hashtable();
									foreach (DataRow dataRow4 in dataTable.Rows)
									{
										string key4 = Convert.ToString(dataRow4[0]);
										long num7 = Convert.ToInt64(dataRow4[1]);
										if (this.ht_bhpd.ContainsKey(key4))
										{
											long num8 = Convert.ToInt64(this.ht_bhpd[key4]);
											this.ht_bhpd[key4] = num8 + num7;
										}
										else
										{
											this.ht_bhpd.Add(key4, num7);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand.CommandText = "select port_id,power_consumption,insert_time from port_data_daily order by port_id ASC ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									this.ht_pdpd = new Hashtable();
									foreach (DataRow dataRow5 in dataTable.Rows)
									{
										string key5 = Convert.ToString(dataRow5[0]);
										long num9 = Convert.ToInt64(dataRow5[1]);
										if (this.ht_pdpd.ContainsKey(key5))
										{
											long num10 = Convert.ToInt64(this.ht_pdpd[key5]);
											this.ht_pdpd[key5] = num10 + num9;
										}
										else
										{
											this.ht_pdpd.Add(key5, num9);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand.CommandText = "select port_id,power_consumption,insert_time from port_data_hourly where insert_time = #" + dt_inserttime.ToString("yyyy-MM-dd HH") + ":30:00# order by port_id ASC ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									this.ht_phpd = new Hashtable();
									foreach (DataRow dataRow6 in dataTable.Rows)
									{
										string key6 = Convert.ToString(dataRow6[0]);
										long num11 = Convert.ToInt64(dataRow6[1]);
										if (this.ht_phpd.ContainsKey(key6))
										{
											long num12 = Convert.ToInt64(this.ht_phpd[key6]);
											this.ht_phpd[key6] = num12 + num11;
										}
										else
										{
											this.ht_phpd.Add(key6, num11);
										}
									}
								}
								dataTable = new DataTable();
							}
						}
						catch (Exception ex)
						{
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Prepare PD History Data ERROR : " + ex.Message + "\n" + ex.StackTrace);
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
							if (dBConn != null)
							{
								dBConn.close();
							}
						}
					}
				}
				this.i_processor_status = 1;
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Prepare DataDB Error : " + ex2.Message + "\n" + ex2.StackTrace);
			}
		}
		private void DeleteDir(string aimPath)
		{
			try
			{
				if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
				{
					aimPath += Path.DirectorySeparatorChar;
				}
				string[] fileSystemEntries = Directory.GetFileSystemEntries(aimPath);
				string[] array = fileSystemEntries;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					if (Directory.Exists(path))
					{
						this.DeleteDir(aimPath + Path.GetFileName(path));
					}
					else
					{
						File.Delete(aimPath + Path.GetFileName(path));
					}
				}
				Directory.Delete(aimPath, true);
			}
			catch
			{
			}
		}
	}
}
