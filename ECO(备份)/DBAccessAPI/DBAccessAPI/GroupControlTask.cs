using CommonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace DBAccessAPI
{
	public class GroupControlTask
	{
		public const int POWERNONE = -1;
		public const int POWERON = 1;
		public const int POWEROFF = 0;
		public const int TASK_TYPE_DAILY = 0;
		public const int TASK_TYPE_WEEKLY = 1;
		public const int TASK_TYPE_YEARLY = 2;
		private string taskname = "";
		private long taskid;
		private int tasktype;
		private long groupid;
		private string groupname = "";
		private int dayofweek = 8;
		private int operation;
		private int int_status;
		private string string_reserve = "";
		private List<SpecialDay> specialday = new List<SpecialDay>();
		private string[,] task_schedule;
		public int Status
		{
			get
			{
				return this.int_status;
			}
			set
			{
				this.int_status = value;
			}
		}
		public string Reserve
		{
			get
			{
				return this.string_reserve;
			}
			set
			{
				this.string_reserve = value;
			}
		}
		public long ID
		{
			get
			{
				return this.taskid;
			}
			set
			{
				this.taskid = value;
			}
		}
		public string TaskName
		{
			get
			{
				return this.taskname;
			}
			set
			{
				this.taskname = value;
			}
		}
		public int TaskType
		{
			get
			{
				return this.tasktype;
			}
			set
			{
				this.tasktype = value;
			}
		}
		public long GroupID
		{
			get
			{
				return this.groupid;
			}
			set
			{
				this.groupid = value;
			}
		}
		public string GroupName
		{
			get
			{
				return this.groupname;
			}
			set
			{
				this.groupname = value;
			}
		}
		public string[,] TaskSchedule
		{
			get
			{
				if (this.task_schedule != null)
				{
					return this.task_schedule;
				}
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				try
				{
					dBConn = DBConnPool.getConnection();
					if (dBConn.con != null)
					{
						dbCommand = DBConn.GetCommandObject(dBConn.con);
						dbCommand.CommandType = CommandType.Text;
						dbCommand.CommandText = "select * from taskschedule where optype < 5 and groupid = " + this.groupid;
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						if (dbDataReader.HasRows)
						{
							DataTable dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
							if (dataTable != null && dataTable.Rows.Count > 0 && Convert.ToInt32(dataTable.Rows[0]["dayofweek"]) < 8)
							{
								string[,] array = new string[7, 2];
								array[0, 0] = "";
								array[0, 1] = "";
								array[1, 0] = "";
								array[1, 1] = "";
								array[2, 0] = "";
								array[2, 1] = "";
								array[3, 0] = "";
								array[3, 1] = "";
								array[4, 0] = "";
								array[4, 1] = "";
								array[5, 0] = "";
								array[5, 1] = "";
								array[6, 0] = "";
								array[6, 1] = "";
								this.task_schedule = array;
							}
							if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Rows.Count < 3 && Convert.ToInt32(dataTable.Rows[0]["dayofweek"]) == 8)
							{
								string[,] array2 = new string[1, 2];
								array2[0, 0] = "";
								array2[0, 1] = "";
								this.task_schedule = array2;
							}
							foreach (DataRow dataRow in dataTable.Rows)
							{
								int num = Convert.ToInt32(dataRow["dayofweek"]);
								int num2 = Convert.ToInt32(dataRow["optype"]);
								string text = "";
								try
								{
									text = DateTime.ParseExact(Convert.ToDateTime(dataRow["scheduletime"]).ToString("HH:mm:ss"), "HH:mm:ss", null).ToString("HH:mm:ss");
								}
								catch (Exception ex)
								{
									DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
								}
								if (num == 8)
								{
									if (num2 == 1)
									{
										this.task_schedule[0, 0] = text;
									}
									else
									{
										this.task_schedule[0, 1] = text;
									}
								}
								else
								{
									if (num < 8)
									{
										if (num2 == 1)
										{
											this.task_schedule[num - 1, 0] = text;
										}
										else
										{
											this.task_schedule[num - 1, 1] = text;
										}
									}
								}
							}
						}
						dbDataReader.Close();
					}
				}
				catch (Exception ex2)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
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
					if (dBConn != null)
					{
						dBConn.close();
					}
				}
				return this.task_schedule;
			}
			set
			{
				this.task_schedule = value;
			}
		}
		public List<SpecialDay> SpecialDates
		{
			get
			{
				if (this.specialday != null && this.specialday.Count > 0)
				{
					return this.specialday;
				}
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				try
				{
					dBConn = DBConnPool.getConnection();
					if (dBConn.con != null)
					{
						dbCommand = DBConn.GetCommandObject(dBConn.con);
						dbCommand.CommandType = CommandType.Text;
						dbCommand.CommandText = "select * from taskschedule where optype < 5 and groupid = " + this.groupid + " order by reserve ";
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						if (dbDataReader.HasRows)
						{
							DataTable dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
							this.specialday = new List<SpecialDay>();
							Dictionary<string, SpecialDay> dictionary = new Dictionary<string, SpecialDay>();
							foreach (DataRow dataRow in dataTable.Rows)
							{
								string text = "";
								string text2 = "";
								string text3 = "";
								try
								{
									text = Convert.ToString(dataRow["reserve"]);
								}
								catch (Exception)
								{
									text = "";
								}
								SpecialDay specialDay = null;
								int num = Convert.ToInt32(dataRow["dayofweek"]);
								int num2 = Convert.ToInt32(dataRow["optype"]);
								string text4 = "";
								try
								{
									text4 = DateTime.ParseExact(Convert.ToDateTime(dataRow["scheduletime"]).ToString("HH:mm:ss"), "HH:mm:ss", null).ToString("HH:mm:ss");
								}
								catch (Exception)
								{
								}
								if (num > 8)
								{
									if (num2 == 1)
									{
										text2 = text4;
										specialDay = new SpecialDay(text, text2, "");
									}
									else
									{
										if (num2 == 0)
										{
											text3 = text4;
											specialDay = new SpecialDay(text, "", text3);
										}
										else
										{
											if (num2 == -1)
											{
												specialDay = new SpecialDay(text, "", "");
											}
										}
									}
									if (dictionary.ContainsKey(text))
									{
										specialDay = dictionary[text];
										if (num2 == 1)
										{
											specialDay.ONTime = text2;
										}
										else
										{
											if (num2 == 0)
											{
												specialDay.OFFTime = text3;
											}
											else
											{
												if (num2 == -1)
												{
													specialDay.ONTime = "";
													specialDay.OFFTime = "";
												}
											}
										}
										dictionary[text] = specialDay;
									}
									else
									{
										dictionary.Add(text, specialDay);
									}
								}
							}
							List<SpecialDay> list = new List<SpecialDay>();
							IEnumerator<SpecialDay> enumerator2 = dictionary.Values.GetEnumerator();
							while (enumerator2.MoveNext())
							{
								SpecialDay current = enumerator2.Current;
								list.Add(current);
							}
							if (list.Count > 0)
							{
								this.specialday = list;
							}
							else
							{
								this.specialday = new List<SpecialDay>();
							}
						}
						dbDataReader.Close();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
					if (dBConn != null)
					{
						dBConn.close();
					}
				}
				return this.specialday;
			}
			set
			{
				this.specialday = value;
			}
		}
		public GroupControlTask(DataRow dr_gct)
		{
			this.taskid = Convert.ToInt64(dr_gct["id"]);
			this.taskname = Convert.ToString(dr_gct["taskname"]);
			this.groupname = Convert.ToString(dr_gct["groupname"]);
			this.groupid = Convert.ToInt64(dr_gct["groupid"]);
			this.tasktype = Convert.ToInt32(dr_gct["tasktype"]);
			object obj = dr_gct["status"];
			if (obj != null && obj != DBNull.Value)
			{
				try
				{
					this.int_status = Convert.ToInt32(dr_gct["status"]);
				}
				catch
				{
				}
			}
			obj = dr_gct["reserve"];
			if (obj != null && obj != DBNull.Value)
			{
				try
				{
					this.string_reserve = Convert.ToString(dr_gct["reserve"]);
				}
				catch
				{
				}
			}
		}
		public static string[,] GetTaskScheduleByTaskID(long i_taskid)
		{
			string[,] result = null;
			try
			{
				Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
				if (groupTaskCache != null && groupTaskCache.Count > 0)
				{
					if (groupTaskCache.ContainsKey(i_taskid))
					{
						GroupControlTask groupControlTask = (GroupControlTask)groupTaskCache[i_taskid];
						result = groupControlTask.TaskSchedule;
					}
				}
				else
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetTaskScheduleByTaskID error : Could not get task data from DBCache");
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static List<SpecialDay> GetSpecialDatesByTaskID(long i_taskid)
		{
			List<SpecialDay> result = new List<SpecialDay>();
			try
			{
				Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
				if (groupTaskCache != null && groupTaskCache.Count > 0)
				{
					if (groupTaskCache.ContainsKey(i_taskid))
					{
						GroupControlTask groupControlTask = (GroupControlTask)groupTaskCache[i_taskid];
						result = groupControlTask.SpecialDates;
					}
				}
				else
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetSpecialDatesByTaskID error : Could not get task data from DBCache");
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetSpecialDatesByTaskID ERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public int UpdateTask()
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					GroupInfo groupInfo = null;
					Hashtable groupCache = DBCache.GetGroupCache(dBConn);
					int result;
					if (groupCache == null || groupCache.Count <= 0)
					{
						result = -1;
						return result;
					}
					if (!groupCache.ContainsKey(this.groupid))
					{
						result = -1;
						return result;
					}
					groupInfo = (GroupInfo)groupCache[this.groupid];
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.Transaction = transaction;
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "delete from taskschedule where optype < 5 and groupid in (select groupid from groupcontroltask where id = " + this.taskid + ")";
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					int num = dbCommand.ExecuteNonQuery();
					if (num < 0)
					{
						dbCommand.Transaction.Rollback();
						result = -1;
						return result;
					}
					int num3;
					for (int i = this.task_schedule.GetLowerBound(0); i <= this.task_schedule.GetUpperBound(0); i++)
					{
						for (int j = this.task_schedule.GetLowerBound(1); j <= this.task_schedule.GetUpperBound(1); j++)
						{
							int num2 = 1;
							if (j > 0)
							{
								num2 = 0;
							}
							string text = this.task_schedule[i, j];
							if (text != null && text.Length > 0)
							{
								if (this.tasktype == 0)
								{
									dbCommand.CommandType = CommandType.Text;
									string commandText = string.Concat(new object[]
									{
										"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
										this.groupid,
										",8,",
										num2,
										",#",
										text,
										"#)"
									});
									dbCommand.CommandText = commandText;
									if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
									{
										dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
									}
									num3 = dbCommand.ExecuteNonQuery();
									if (num3 < 0)
									{
										dbCommand.Transaction.Rollback();
										result = num3;
										return result;
									}
								}
								else
								{
									if (this.tasktype == 1 || this.tasktype == 2)
									{
										dbCommand.CommandType = CommandType.Text;
										string commandText2 = string.Concat(new object[]
										{
											"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
											this.groupid,
											",",
											i + 1,
											",",
											num2,
											",#",
											text,
											"#)"
										});
										dbCommand.CommandText = commandText2;
										if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
										{
											dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
										}
										num3 = dbCommand.ExecuteNonQuery();
										if (num3 < 0)
										{
											dbCommand.Transaction.Rollback();
											result = num3;
											return result;
										}
									}
								}
							}
						}
					}
					if (this.tasktype == 2 && this.specialday != null && this.specialday.Count > 0)
					{
						foreach (SpecialDay current in this.specialday)
						{
							dbCommand.CommandType = CommandType.Text;
							if (current.ONTime != null && current.ONTime.Length > 0)
							{
								string commandText3 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime,reserve) values (",
									groupInfo.ID,
									",9,1,#",
									current.ONTime,
									"#,'",
									current.SpecialDate,
									"')"
								});
								dbCommand.CommandText = commandText3;
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
								}
								num3 = dbCommand.ExecuteNonQuery();
								if (num3 < 0)
								{
									dbCommand.Transaction.Rollback();
									result = num3;
									return result;
								}
							}
							if (current.OFFTime != null && current.OFFTime.Length > 0)
							{
								string commandText4 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime,reserve) values (",
									groupInfo.ID,
									",9,0,#",
									current.OFFTime,
									"#,'",
									current.SpecialDate,
									"')"
								});
								dbCommand.CommandText = commandText4;
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
								}
								num3 = dbCommand.ExecuteNonQuery();
								if (num3 < 0)
								{
									dbCommand.Transaction.Rollback();
									result = num3;
									return result;
								}
							}
							if ((current.ONTime == null || current.ONTime.Length == 0) && (current.OFFTime == null || current.OFFTime.Length == 0))
							{
								string commandText5 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime,reserve) values (",
									groupInfo.ID,
									",9,-1,#23:59:59#,'",
									current.SpecialDate,
									"')"
								});
								dbCommand.CommandText = commandText5;
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
								}
								num3 = dbCommand.ExecuteNonQuery();
								if (num3 < 0)
								{
									dbCommand.Transaction.Rollback();
									result = num3;
									return result;
								}
							}
						}
					}
					dbCommand.CommandText = string.Concat(new object[]
					{
						"update groupcontroltask set taskname = '",
						this.taskname,
						"' , tasktype = ",
						this.tasktype,
						", groupid =",
						this.groupid,
						", groupname = '",
						groupInfo.GroupName,
						"' where id = ",
						this.taskid
					});
					num3 = dbCommand.ExecuteNonQuery();
					if (num3 < 0)
					{
						dbCommand.Transaction.Rollback();
						result = num3;
						return result;
					}
					dbCommand.Transaction.Commit();
					DBCacheStatus.GroupTask = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_GroupTask"
					});
					result = 1;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				dbCommand.Transaction.Rollback();
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
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return -1;
		}
		public static GroupControlTask GetTaskByGroupID(long l_gid)
		{
			GroupControlTask result = null;
			try
			{
				Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
				if (groupTaskCache != null && groupTaskCache.Count > 0)
				{
					ICollection values = groupTaskCache.Values;
					IEnumerator enumerator = values.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							GroupControlTask groupControlTask = (GroupControlTask)enumerator.Current;
							long groupID = groupControlTask.GroupID;
							if (groupID == l_gid)
							{
								result = groupControlTask;
								return groupControlTask;
							}
						}
						goto IL_75;
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
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetTaskByGroupID error : Could not get task data from DBCache");
				IL_75:;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetTaskByGroupID ERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static GroupControlTask GetTaskByID(long i_taskid)
		{
			GroupControlTask result = null;
			try
			{
				Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
				if (groupTaskCache != null && groupTaskCache.Count > 0)
				{
					if (groupTaskCache.ContainsKey(i_taskid))
					{
						result = (GroupControlTask)groupTaskCache[i_taskid];
					}
				}
				else
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetTaskByID error : Could not get task data from DBCache");
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetTaskByID ERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static List<GroupControlTask> GetAllTask()
		{
			List<GroupControlTask> list = new List<GroupControlTask>();
			try
			{
				Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
				if (groupTaskCache != null && groupTaskCache.Count > 0)
				{
					ICollection values = groupTaskCache.Values;
					foreach (GroupControlTask item in values)
					{
						list.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetAllTask ERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			list.Sort((GroupControlTask x, GroupControlTask y) => x.taskname.CompareTo(y.taskname));
			return list;
		}
		public static int DeleteTaskByID(long i_taskid)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "delete from taskschedule where optype < 5 and groupid in ( select g.groupid from groupcontroltask g where g.id = " + i_taskid + " )";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from groupcontroltask where id = " + i_taskid;
					dbCommand.ExecuteNonQuery();
					DBCacheStatus.GroupTask = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_GroupTask"
					});
					return 1;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return -1;
		}
		public static int CreateTask(string taskname, long l_gid, int i_type, string[,] arr_schedule)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					Hashtable groupCache = DBCache.GetGroupCache(dBConn);
					int result;
					if (groupCache == null || groupCache.Count <= 0)
					{
						result = -1;
						return result;
					}
					if (!groupCache.ContainsKey(l_gid))
					{
						result = -1;
						return result;
					}
					GroupInfo groupInfo = (GroupInfo)groupCache[l_gid];
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.Transaction = transaction;
					dbCommand.CommandText = string.Concat(new object[]
					{
						"insert into groupcontroltask (taskname,groupname,groupid,tasktype) values ('",
						taskname,
						"','",
						groupInfo.GroupName,
						"',",
						groupInfo.ID,
						",",
						i_type,
						")"
					});
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					int num = dbCommand.ExecuteNonQuery();
					if (num < 0)
					{
						dbCommand.Transaction.Rollback();
						result = num;
						return result;
					}
					for (int i = arr_schedule.GetLowerBound(0); i <= arr_schedule.GetUpperBound(0); i++)
					{
						for (int j = arr_schedule.GetLowerBound(1); j <= arr_schedule.GetUpperBound(1); j++)
						{
							int num2 = 1;
							if (j > 0)
							{
								num2 = 0;
							}
							string text = arr_schedule[i, j];
							if (text != null && text.Length > 0)
							{
								if (i_type == 0)
								{
									dbCommand.CommandType = CommandType.Text;
									string commandText = string.Concat(new object[]
									{
										"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
										groupInfo.ID,
										",8,",
										num2,
										",#",
										text,
										"#)"
									});
									dbCommand.CommandText = commandText;
									if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
									{
										dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
									}
									num = dbCommand.ExecuteNonQuery();
									if (num < 0)
									{
										dbCommand.Transaction.Rollback();
										result = num;
										return result;
									}
								}
								else
								{
									if (i_type == 1)
									{
										dbCommand.CommandType = CommandType.Text;
										string commandText2 = string.Concat(new object[]
										{
											"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
											groupInfo.ID,
											",",
											i + 1,
											",",
											num2,
											",#",
											text,
											"#)"
										});
										dbCommand.CommandText = commandText2;
										if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
										{
											dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
										}
										num = dbCommand.ExecuteNonQuery();
										if (num < 0)
										{
											dbCommand.Transaction.Rollback();
											result = num;
											return result;
										}
									}
								}
							}
						}
					}
					dbCommand.Transaction.Commit();
					DBCacheStatus.GroupTask = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_GroupTask"
					});
					result = 1;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbCommand.Transaction.Rollback();
				}
				catch
				{
				}
				if (ex.Message.IndexOf(" duplicate values ") > 0)
				{
					int result = -2;
					return result;
				}
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
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return -10;
		}
		public static int CreateTask(string taskname, long l_gid, int i_type, string[,] arr_schedule, List<SpecialDay> list_sd)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					Hashtable groupCache = DBCache.GetGroupCache(dBConn);
					int result;
					if (groupCache == null || groupCache.Count <= 0)
					{
						result = -1;
						return result;
					}
					if (!groupCache.ContainsKey(l_gid))
					{
						result = -1;
						return result;
					}
					GroupInfo groupInfo = (GroupInfo)groupCache[l_gid];
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.Transaction = transaction;
					dbCommand.CommandText = string.Concat(new object[]
					{
						"insert into groupcontroltask (taskname,groupname,groupid,tasktype) values ('",
						taskname,
						"','",
						groupInfo.GroupName,
						"',",
						groupInfo.ID,
						",",
						i_type,
						")"
					});
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
					}
					int num = dbCommand.ExecuteNonQuery();
					if (num < 0)
					{
						dbCommand.Transaction.Rollback();
						result = num;
						return result;
					}
					for (int i = arr_schedule.GetLowerBound(0); i <= arr_schedule.GetUpperBound(0); i++)
					{
						for (int j = arr_schedule.GetLowerBound(1); j <= arr_schedule.GetUpperBound(1); j++)
						{
							int num2 = 1;
							if (j > 0)
							{
								num2 = 0;
							}
							string text = arr_schedule[i, j];
							if (text != null && text.Length > 0)
							{
								if (i_type == 0)
								{
									dbCommand.CommandType = CommandType.Text;
									string commandText = string.Concat(new object[]
									{
										"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
										groupInfo.ID,
										",8,",
										num2,
										",#",
										text,
										"#)"
									});
									dbCommand.CommandText = commandText;
									if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
									{
										dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
									}
									num = dbCommand.ExecuteNonQuery();
									if (num < 0)
									{
										dbCommand.Transaction.Rollback();
										result = num;
										return result;
									}
								}
								else
								{
									if (i_type == 1 || i_type == 2)
									{
										dbCommand.CommandType = CommandType.Text;
										string commandText2 = string.Concat(new object[]
										{
											"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
											groupInfo.ID,
											",",
											i + 1,
											",",
											num2,
											",#",
											text,
											"#)"
										});
										dbCommand.CommandText = commandText2;
										if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
										{
											dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
										}
										num = dbCommand.ExecuteNonQuery();
										if (num < 0)
										{
											dbCommand.Transaction.Rollback();
											result = num;
											return result;
										}
									}
								}
							}
						}
					}
					if (i_type == 2)
					{
						foreach (SpecialDay current in list_sd)
						{
							dbCommand.CommandType = CommandType.Text;
							if (current.ONTime != null && current.ONTime.Length > 0)
							{
								string commandText3 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime,reserve) values (",
									groupInfo.ID,
									",9,1,#",
									current.ONTime,
									"#,'",
									current.SpecialDate,
									"')"
								});
								dbCommand.CommandText = commandText3;
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
								}
								num = dbCommand.ExecuteNonQuery();
								if (num < 0)
								{
									dbCommand.Transaction.Rollback();
									result = num;
									return result;
								}
							}
							if (current.OFFTime != null && current.OFFTime.Length > 0)
							{
								string commandText4 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime,reserve) values (",
									groupInfo.ID,
									",9,0,#",
									current.OFFTime,
									"#,'",
									current.SpecialDate,
									"')"
								});
								dbCommand.CommandText = commandText4;
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
								}
								num = dbCommand.ExecuteNonQuery();
								if (num < 0)
								{
									dbCommand.Transaction.Rollback();
									result = num;
									return result;
								}
							}
							if ((current.ONTime == null || current.ONTime.Length == 0) && (current.OFFTime == null || current.OFFTime.Length == 0))
							{
								string commandText5 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime,reserve) values (",
									groupInfo.ID,
									",9,-1,#23:59:59#,'",
									current.SpecialDate,
									"')"
								});
								dbCommand.CommandText = commandText5;
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = dbCommand.CommandText.Replace("#", "'");
								}
								num = dbCommand.ExecuteNonQuery();
								if (num < 0)
								{
									dbCommand.Transaction.Rollback();
									result = num;
									return result;
								}
							}
						}
					}
					dbCommand.Transaction.Commit();
					DBCacheStatus.GroupTask = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_GroupTask"
					});
					result = 1;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbCommand.Transaction.Rollback();
				}
				catch
				{
				}
				if (ex.Message.IndexOf(" duplicate values ") > 0)
				{
					int result = -2;
					return result;
				}
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
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return -10;
		}
		public static Dictionary<int, TaskInfo> GetTaskContent()
		{
			Dictionary<int, TaskInfo> dictionary = new Dictionary<int, TaskInfo>();
			int dayOfWeek = GroupControlTask.getDayOfWeek();
			int hour = DateTime.Now.Hour;
			int minute = DateTime.Now.Minute;
			string text = string.Concat(hour);
			if (text.Length < 2)
			{
				text = "0" + hour;
			}
			string text2 = string.Concat(minute);
			if (text2.Length < 2)
			{
				text2 = "0" + minute;
			}
			string text3 = text + ":" + text2 + ":00";
			string text4 = text + ":" + text2 + ":59";
			string.Concat(new object[]
			{
				"select s.groupid,s.optype,t.taskname from taskschedule s  inner join groupcontroltask t on s.groupid = t.groupid where s.optype < 5 and (s.dayofweek = ",
				dayOfWeek,
				" or s.dayofweek = 8) and s.scheduletime >= #",
				text3,
				"# and s.scheduletime <= #",
				text4,
				"# "
			});
			Hashtable hashtable = new Hashtable();
			try
			{
				Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
				if (groupTaskCache != null && groupTaskCache.Count > 0)
				{
					ICollection values = groupTaskCache.Values;
					foreach (GroupControlTask groupControlTask in values)
					{
						DateTime value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + text3);
						DateTime value2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + text4);
						long groupID = groupControlTask.GroupID;
						List<SpecialDay> specialDates = groupControlTask.SpecialDates;
						if (specialDates != null && specialDates.Count > 0)
						{
							foreach (SpecialDay current in specialDates)
							{
								if (current.SpecialDate.Equals(DateTime.Now.ToString("yyyy-MM-dd")))
								{
									if (!hashtable.ContainsKey(groupID))
									{
										hashtable.Add(groupID, groupID);
									}
									if (current.ONTime.Length != 0 || current.OFFTime.Length != 0)
									{
										if (current.ONTime.Length > 0)
										{
											DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + current.ONTime);
											if (dateTime.CompareTo(value) >= 0 && dateTime.CompareTo(value2) < 0)
											{
												TaskInfo value3 = new TaskInfo(groupControlTask.TaskName, groupID, 1);
												dictionary.Add(Convert.ToInt32(groupID), value3);
											}
										}
										if (current.OFFTime.Length > 0)
										{
											DateTime dateTime2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + current.OFFTime);
											if (dateTime2.CompareTo(value) >= 0 && dateTime2.CompareTo(value2) < 0)
											{
												TaskInfo value4 = new TaskInfo(groupControlTask.TaskName, groupID, 0);
												dictionary.Add(Convert.ToInt32(groupID), value4);
											}
										}
									}
								}
							}
						}
						if (!hashtable.ContainsKey(groupID))
						{
							string[,] taskSchedule = groupControlTask.TaskSchedule;
							if (taskSchedule != null && taskSchedule.Length > 0)
							{
								if (taskSchedule.GetUpperBound(0) > 2)
								{
									try
									{
										string text5 = taskSchedule[dayOfWeek - 1, 0];
										if (text5 != null && text5.Length > 0)
										{
											DateTime dateTime3 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + text5);
											if (dateTime3.CompareTo(value) >= 0 && dateTime3.CompareTo(value2) < 0)
											{
												TaskInfo value5 = new TaskInfo(groupControlTask.TaskName, groupID, 1);
												dictionary.Add(Convert.ToInt32(groupID), value5);
											}
										}
										text5 = taskSchedule[dayOfWeek - 1, 1];
										if (text5 != null && text5.Length > 0)
										{
											DateTime dateTime4 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + text5);
											if (dateTime4.CompareTo(value) >= 0 && dateTime4.CompareTo(value2) < 0)
											{
												TaskInfo value6 = new TaskInfo(groupControlTask.TaskName, groupID, 0);
												dictionary.Add(Convert.ToInt32(groupID), value6);
											}
										}
										continue;
									}
									catch (Exception ex)
									{
										DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GroupControlTask-->GetTaskContent-->Get Weekly Data ERROR : " + ex.Message + "\n" + ex.StackTrace);
										continue;
									}
								}
								try
								{
									string text6 = taskSchedule[0, 0];
									if (text6 != null && text6.Length > 0)
									{
										DateTime dateTime5 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + text6);
										if (dateTime5.CompareTo(value) >= 0 && dateTime5.CompareTo(value2) < 0)
										{
											TaskInfo value7 = new TaskInfo(groupControlTask.TaskName, groupID, 1);
											dictionary.Add(Convert.ToInt32(groupID), value7);
										}
									}
									text6 = taskSchedule[0, 1];
									if (text6 != null && text6.Length > 0)
									{
										DateTime dateTime6 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + text6);
										if (dateTime6.CompareTo(value) >= 0 && dateTime6.CompareTo(value2) < 0)
										{
											TaskInfo value8 = new TaskInfo(groupControlTask.TaskName, groupID, 0);
											dictionary.Add(Convert.ToInt32(groupID), value8);
										}
									}
								}
								catch (Exception ex2)
								{
									DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GroupControlTask-->GetTaskContent-->Get Daily Data ERROR : " + ex2.Message + "\n" + ex2.StackTrace);
								}
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GroupControlTask-->GetTaskContent ERROR : " + ex3.Message + "\n" + ex3.StackTrace);
			}
			return dictionary;
		}
		public static int getDayOfWeek()
		{
			switch (DateTime.Now.DayOfWeek)
			{
			case DayOfWeek.Sunday:
				return 7;
			case DayOfWeek.Monday:
				return 1;
			case DayOfWeek.Tuesday:
				return 2;
			case DayOfWeek.Wednesday:
				return 3;
			case DayOfWeek.Thursday:
				return 4;
			case DayOfWeek.Friday:
				return 5;
			case DayOfWeek.Saturday:
				return 6;
			default:
				return -1;
			}
		}
		public static Dictionary<long, string> GetUnusedGroup(long l_taskid)
		{
			Dictionary<long, string> dictionary = new Dictionary<long, string>();
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0)
				{
					Hashtable hashtable = new Hashtable();
					Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
					if (groupTaskCache != null && groupTaskCache.Count > 0)
					{
						ICollection values = groupTaskCache.Values;
						foreach (GroupControlTask groupControlTask in values)
						{
							if (groupControlTask.ID != l_taskid)
							{
								hashtable.Add(groupControlTask.GroupID, groupControlTask.GroupID);
							}
						}
					}
					ICollection values2 = groupCache.Values;
					foreach (GroupInfo groupInfo in values2)
					{
						if (!hashtable.ContainsKey(groupInfo.ID))
						{
							dictionary.Add(groupInfo.ID, groupInfo.GroupName);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GroupControlTask-->GetUnusedGroup ERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return dictionary;
		}
		public static bool CheckName(long l_id, string str_name)
		{
			try
			{
				Hashtable groupTaskCache = DBCache.GetGroupTaskCache();
				if (groupTaskCache != null && groupTaskCache.Count > 0)
				{
					ICollection values = groupTaskCache.Values;
					foreach (GroupControlTask groupControlTask in values)
					{
						long iD = groupControlTask.ID;
						string taskName = groupControlTask.TaskName;
						if (taskName.Equals(str_name))
						{
							bool result;
							if (iD == l_id)
							{
								result = true;
								return result;
							}
							result = false;
							return result;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GroupControlTask-->CheckName ERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return true;
		}
	}
}
