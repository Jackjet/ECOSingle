using CommonAPI;
using CommonAPI.network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Net.NetworkInformation;
namespace DBAccessAPI
{
	public class Backuptask
	{
		public const int TASK_TYPE_DAILY = 0;
		public const int TASK_TYPE_WEEKLY = 1;
		public const int STORE_TYPE_LOCAL = 0;
		public const int STORE_TYPE_FTP = 1;
		public const int STORE_TYPE_SMB = 2;
		public const int FOLDER_MaxLEN = 255;
		public const string DBBACKUPFOLDER = "tmpbackupfolder";
		public const string RESTOREFOLDER = "tmprestorefolder";
		private string taskname = "";
		private long taskid;
		private string[] task_schedule;
		private int tasktype;
		private int storetype;
		private string username = "";
		private string password = "";
		private string host = "";
		private int port;
		private string filepath = "";
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
		public int STOREType
		{
			get
			{
				return this.storetype;
			}
			set
			{
				this.storetype = value;
			}
		}
		public string UserName
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
			}
		}
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}
		public string Host
		{
			get
			{
				return this.host;
			}
			set
			{
				this.host = value;
			}
		}
		public string Filepath
		{
			get
			{
				return this.filepath;
			}
			set
			{
				this.filepath = value;
			}
		}
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
		public string[] TaskSchedule
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
						dbCommand.CommandText = "select * from taskschedule where groupid = " + this.taskid + " and optype = 5 ";
						DbDataReader dbDataReader = dbCommand.ExecuteReader();
						if (dbDataReader.HasRows)
						{
							DataTable dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
							if (dataTable != null && dataTable.Rows.Count > 0 && Convert.ToInt32(dataTable.Rows[0]["dayofweek"]) < 8)
							{
								this.task_schedule = new string[]
								{
									"",
									"",
									"",
									"",
									"",
									"",
									""
								};
							}
							if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Rows.Count < 2 && Convert.ToInt32(dataTable.Rows[0]["dayofweek"]) == 8)
							{
								this.task_schedule = new string[]
								{
									""
								};
							}
							foreach (DataRow dataRow in dataTable.Rows)
							{
								int num = Convert.ToInt32(dataRow["dayofweek"]);
								Convert.ToInt32(dataRow["optype"]);
								string text = "";
								try
								{
									text = Convert.ToDateTime(dataRow["scheduletime"]).ToString("HH:mm:ss");
								}
								catch (Exception)
								{
								}
								if (num == 8)
								{
									this.task_schedule[0] = text;
								}
								else
								{
									this.task_schedule[num - 1] = text;
								}
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
				return this.task_schedule;
			}
			set
			{
				this.task_schedule = value;
			}
		}
		public Backuptask(DataRow dr_bct)
		{
			this.taskid = Convert.ToInt64(dr_bct["id"]);
			this.taskname = Convert.ToString(dr_bct["taskname"]);
			this.storetype = Convert.ToInt32(dr_bct["storetype"]);
			this.tasktype = Convert.ToInt32(dr_bct["tasktype"]);
			this.username = Convert.ToString(dr_bct["username"]);
			this.password = Convert.ToString(dr_bct["pwd"]);
			this.host = Convert.ToString(dr_bct["host"]);
			this.filepath = Convert.ToString(dr_bct["filepath"]);
			this.port = Convert.ToInt32(dr_bct["port"]);
		}
		public static string[] GetTaskScheduleByTaskID(long i_taskid)
		{
			string[] array = null;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "select * from taskschedule where optype = 5 and groupid = " + i_taskid;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.HasRows)
					{
						DataTable dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
						if (dataTable != null && dataTable.Rows.Count > 0 && Convert.ToInt32(dataTable.Rows[0]["dayofweek"]) < 8)
						{
							array = new string[]
							{
								"",
								"",
								"",
								"",
								"",
								"",
								""
							};
						}
						if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Rows.Count < 3 && Convert.ToInt32(dataTable.Rows[0]["dayofweek"]) == 8)
						{
							array = new string[]
							{
								""
							};
						}
						foreach (DataRow dataRow in dataTable.Rows)
						{
							int num = Convert.ToInt32(dataRow["dayofweek"]);
							Convert.ToInt32(dataRow["optype"]);
							string text = "";
							try
							{
								text = Convert.ToDateTime(dataRow["scheduletime"]).ToString("HH:mm:ss");
							}
							catch (Exception)
							{
							}
							if (num == 8)
							{
								array[0] = text;
							}
							else
							{
								array[num - 1] = text;
							}
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
			return array;
		}
		public int UpdateTask()
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.Transaction = transaction;
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "delete from taskschedule where optype = 5 and groupid  = " + this.taskid;
					int num = dbCommand.ExecuteNonQuery();
					int result;
					if (num < 0)
					{
						dbCommand.Transaction.Rollback();
						result = -1;
						return result;
					}
					int num2;
					for (int i = this.task_schedule.GetLowerBound(0); i <= this.task_schedule.GetUpperBound(0); i++)
					{
						string text = this.task_schedule[i];
						if (text != null && text.Length > 0)
						{
							dbCommand.CommandType = CommandType.Text;
							string text2 = "";
							if (this.tasktype == 0)
							{
								text2 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
									this.taskid,
									",8,5,#",
									text,
									"#)"
								});
							}
							else
							{
								if (this.tasktype == 1)
								{
									text2 = string.Concat(new object[]
									{
										"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
										this.taskid,
										",",
										i + 1,
										",5,#",
										text,
										"#)"
									});
								}
							}
							if (DBUrl.SERVERMODE)
							{
								text2 = text2.Replace("#", "'");
							}
							dbCommand.CommandText = text2;
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								result = num2;
								return result;
							}
						}
					}
					dbCommand.CommandText = string.Concat(new object[]
					{
						"update backuptask set taskname = '",
						this.taskname,
						"' , tasktype = ",
						this.tasktype,
						", storetype =",
						this.storetype,
						", username = '",
						this.username,
						"',pwd = '",
						this.password,
						"',host = '",
						this.host,
						"',port = ",
						this.port,
						",filepath = '",
						this.filepath,
						"' where id = ",
						this.taskid
					});
					num2 = dbCommand.ExecuteNonQuery();
					if (num2 < 0)
					{
						dbCommand.Transaction.Rollback();
						result = num2;
						return result;
					}
					dbCommand.Transaction.Commit();
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
		public static Backuptask GetTaskByID(long i_taskid)
		{
			Backuptask result = null;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "select * from backuptask where id = " + i_taskid;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.HasRows)
					{
						DataTable dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
						result = new Backuptask(dataTable.Rows[0]);
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
			return result;
		}
		public static List<Backuptask> GetAllTask()
		{
			List<Backuptask> list = new List<Backuptask>();
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "select * from backuptask order by taskname ASC";
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.HasRows)
					{
						DataTable dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
						foreach (DataRow dr_bct in dataTable.Rows)
						{
							Backuptask item = new Backuptask(dr_bct);
							list.Add(item);
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
			return list;
		}
		public static int DeleteTaskByID(long i_taskid)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "delete from taskschedule t where t.optype = 5 and t.groupid  = " + i_taskid;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from backuptask where id = " + i_taskid;
					dbCommand.ExecuteNonQuery();
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
		public static bool CheckName(long l_id, string str_name)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				string commandText = "select id from backuptask where taskname ='" + str_name + "'";
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (!dbDataReader.Read())
					{
						dbDataReader.Close();
					}
					else
					{
						long num = Convert.ToInt64(dbDataReader.GetValue(0));
						bool result;
						if (num == l_id)
						{
							result = true;
							return result;
						}
						dbDataReader.Close();
						result = false;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				bool result = false;
				return result;
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
				dBConn.close();
			}
			return true;
		}
		public static int CreateTask(string taskname, int i_type, int i_store, string str_usr, string str_pwd, string str_host, int i_port, string str_path, string[] arr_schedule)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.Transaction = transaction;
					dbCommand.CommandText = string.Concat(new object[]
					{
						"insert into backuptask (taskname,tasktype,storetype,username,pwd,host,port,filepath) values ('",
						taskname,
						"',",
						i_type,
						",",
						i_store,
						",'",
						str_usr,
						"','",
						str_pwd,
						"','",
						str_host,
						"',",
						i_port,
						",'",
						str_path,
						"' )"
					});
					int num = dbCommand.ExecuteNonQuery();
					int result;
					if (num < 0)
					{
						dbCommand.Transaction.Rollback();
						result = num;
						return result;
					}
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = "SELECT LAST_INSERT_ID()";
					}
					else
					{
						dbCommand.CommandText = "SELECT @@IDENTITY";
					}
					int num2 = Convert.ToInt32(dbCommand.ExecuteScalar());
					if (num2 < 1)
					{
						dbCommand.Transaction.Rollback();
						result = -1;
						return result;
					}
					for (int i = arr_schedule.GetLowerBound(0); i <= arr_schedule.GetUpperBound(0); i++)
					{
						string text = arr_schedule[i];
						if (text != null && text.Length > 0)
						{
							dbCommand.CommandType = CommandType.Text;
							string text2 = "";
							if (i_type == 0)
							{
								text2 = string.Concat(new object[]
								{
									"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
									num2,
									",8,5,#",
									text,
									"#)"
								});
							}
							else
							{
								if (i_type == 1)
								{
									text2 = string.Concat(new object[]
									{
										"insert into taskschedule (groupid,dayofweek,optype,scheduletime) values (",
										num2,
										",",
										i + 1,
										",5,#",
										text,
										"#)"
									});
								}
							}
							if (DBUrl.SERVERMODE)
							{
								text2 = text2.Replace("#", "'");
							}
							dbCommand.CommandText = text2;
							num = dbCommand.ExecuteNonQuery();
							if (num < 0)
							{
								dbCommand.Transaction.Rollback();
								result = num;
								return result;
							}
						}
					}
					dbCommand.Transaction.Commit();
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
		public static List<BackupTaskInfo> GetBackupTaskContent()
		{
			List<BackupTaskInfo> list = new List<BackupTaskInfo>();
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			int dayOfWeek = Backuptask.getDayOfWeek();
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
			string text5 = string.Concat(new object[]
			{
				"select b.taskname,b.tasktype,b.storetype,b.username,b.pwd,b.filepath,b.host,b.port from backuptask b inner join taskschedule s on b.id = s.groupid where s.optype = 5 and ( s.dayofweek = ",
				dayOfWeek,
				" or s.dayofweek = 8) and s.scheduletime >= #",
				text3,
				"# and s.scheduletime <= #",
				text4,
				"# "
			});
			string text6 = "";
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						text5 = text5.Replace("#", "'");
					}
					dbCommand.CommandText = text5;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						if (text6.Length < 1)
						{
							text6 = Backuptask.createbakfile();
						}
						if (text6.Length > 0)
						{
							string str_name = Convert.ToString(dbDataReader.GetValue(0));
							int i_op = Convert.ToInt32(dbDataReader.GetValue(2));
							string susr = Convert.ToString(dbDataReader.GetValue(3));
							string spwd = Convert.ToString(dbDataReader.GetValue(4));
							string spath = Convert.ToString(dbDataReader.GetValue(5));
							string strhost = Convert.ToString(dbDataReader.GetValue(6));
							int iport = Convert.ToInt32(dbDataReader.GetValue(7));
							BackupTaskInfo item = new BackupTaskInfo(str_name, i_op, strhost, iport, susr, spwd, spath);
							list.Add(item);
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
			return list;
		}
		public static void ExecuteTask(object obj)
		{
			try
			{
				List<BackupTaskInfo> list = (List<BackupTaskInfo>)obj;
				if (list != null && list.Count > 0)
				{
					string text = "";
					if (DBUrl.SERVERMODE)
					{
						text = DBTools.BackupConfiguration4ServerMode();
					}
					else
					{
						text = Backuptask.createbakfile();
					}
					if (text != null && text.Length > 0)
					{
						foreach (BackupTaskInfo current in list)
						{
							try
							{
								Backuptask.BackupDB(text, current.OperationType, current.Host, current.Port, current.UserName, current.Pwd, current.FilePath);
							}
							catch
							{
							}
						}
						if (DBUrl.SERVERMODE)
						{
							File.Delete(text);
						}
						else
						{
							Backuptask.DeleteDir(Path.GetDirectoryName(text));
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public static int BackupDBTaskPolling()
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			int dayOfWeek = Backuptask.getDayOfWeek();
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
			string text5 = string.Concat(new object[]
			{
				"select b.taskname,b.tasktype,b.storetype,b.username,b.pwd,b.filepath from backuptask b inner join taskschedule s on b.id = s.groupid where s.optype = 5 and ( s.dayofweek = ",
				dayOfWeek,
				" or s.dayofweek = 8) and s.scheduletime >= #",
				text3,
				"# and s.scheduletime <= #",
				text4,
				"# "
			});
			string text6 = "";
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						text5 = text5.Replace("#", "'");
					}
					dbCommand.CommandText = text5;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						if (text6.Length < 1)
						{
							text6 = Backuptask.createbakfile();
						}
						if (text6.Length > 0)
						{
							Convert.ToString(dbDataReader.GetValue(0));
							Convert.ToInt32(dbDataReader.GetValue(2));
							Convert.ToString(dbDataReader.GetValue(3));
							Convert.ToString(dbDataReader.GetValue(4));
							Convert.ToString(dbDataReader.GetValue(5));
						}
					}
					dbDataReader.Close();
				}
				return 1;
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
		public static string createbakfile()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb";
			if (!File.Exists(text))
			{
				return "";
			}
			string text2 = AppDomain.CurrentDomain.BaseDirectory;
			if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
			{
				text2 += Path.DirectorySeparatorChar;
			}
			text2 += "tmpbackupfolder";
			if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
			{
				text2 += Path.DirectorySeparatorChar;
			}
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			else
			{
				Backuptask.DeleteDir(text2);
				Directory.CreateDirectory(text2);
			}
			try
			{
				File.Copy(text, text2 + Path.GetFileName(text), true);
				string str = DateTime.Now.ToString("yyyyMMddHHmmss");
				string str2 = "Databasebackup" + str + "." + "zip";
				string str3 = "config_backup" + str + "." + "dat";
				using (ZipArchive zipArchive = ZipArchive.CreateZipFile(text2 + str2))
				{
					FileInfo fileInfo = new FileInfo(text2 + Path.GetFileName(text));
					int startIndex = Path.GetDirectoryName(text2 + Path.GetFileName(text)).Length + 1;
					using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
					{
						using (Stream stream = zipArchive.AddFile(fileInfo.FullName.Substring(startIndex)).SetStream())
						{
							byte[] array = new byte[67108863];
							for (int i = fileStream.Read(array, 0, array.Length); i > 0; i = fileStream.Read(array, 0, array.Length))
							{
								stream.Write(array, 0, i);
							}
							stream.Flush();
							stream.Close();
						}
					}
				}
				if (File.Exists(text2 + str2))
				{
					Hashtable hashtable = new Hashtable();
					string currentDBVersion = DBTools.GetCurrentDBVersion();
					hashtable.Add("SYSDBVERSION", currentDBVersion);
					hashtable.Add("HEADVERSION", "1.0.0");
					string str_ver_type;
					if (DBUrl.IsServer)
					{
						str_ver_type = "MASTER";
					}
					else
					{
						str_ver_type = "SINGLE";
					}
					byte[] array2 = DBUtil.GenerateHead(hashtable, str_ver_type);
					if (array2 == null || array2.Length < 23)
					{
						Backuptask.DeleteDir(text2);
						string result = "";
						return result;
					}
					if (DBUrl.IsServer)
					{
						AESEncryptionUtility.Encrypt(array2, text2 + str2, text2 + str3, "ertsaM^tenBakCfgPassw0rd");
					}
					else
					{
						AESEncryptionUtility.Encrypt(array2, text2 + str2, text2 + str3, "^tenBakCfgPassw0rd");
					}
				}
				File.Delete(text2 + str2);
				File.Delete(text2 + Path.GetFileName(text));
				if (File.Exists(text2 + str3))
				{
					string result = text2 + str3;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("*&^%$#@!@#$%^&**&^%$#@!!@#$%^&* Create Backup file error : " + ex.Message + "\n" + ex.StackTrace);
				Backuptask.DeleteDir(text2);
			}
			return "";
		}
		public static void BackupDB(string source_file, int i_store, string str_host, int i_port, string str_usr, string str_pwd, string str_path)
		{
			NetworkShareAccesser networkShareAccesser = null;
			if (source_file.Length > 0)
			{
				try
				{
					switch (i_store)
					{
					case 0:
					case 2:
					{
						bool flag = false;
						if (str_path.StartsWith("\\"))
						{
							flag = true;
							try
							{
								string text = str_path;
								while (text.StartsWith("\\"))
								{
									text = text.Substring(1);
								}
								int num = text.IndexOf('\\');
								if (num > 0)
								{
									text = text.Substring(0, num);
								}
								networkShareAccesser = NetworkShareAccesser.Access(text, str_usr, str_pwd);
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile("&#&#&#&#&#&#&# Task is executing  ... authentication  Error : " + ex.Message + "\n" + ex.StackTrace);
							}
						}
						if (str_path[str_path.Length - 1] != Path.DirectorySeparatorChar)
						{
							str_path += Path.DirectorySeparatorChar;
						}
						if (flag)
						{
							if (networkShareAccesser.Result.Length <= 0)
							{
								File.Copy(source_file, str_path + Path.GetFileName(source_file), true);
							}
							else
							{
								File.Copy(source_file, str_path + Path.GetFileName(source_file), true);
							}
						}
						else
						{
							File.Copy(source_file, str_path + Path.GetFileName(source_file), true);
						}
						break;
					}
					case 1:
					{
						string userName = str_usr;
						string text2 = str_pwd;
						if (str_usr == null || str_usr.Length == 0)
						{
							userName = "anonymous";
							text2 = "ftpusr@aten.com";
						}
						FTP fTP = new FTP(str_host, i_port, userName, text2);
						fTP.upload(str_path + "/" + Path.GetFileName(source_file), source_file);
						break;
					}
					}
				}
				catch (Exception ex2)
				{
					DebugCenter.GetInstance().appendToFile("&#&#&#&#&#&#&# Task is executing  ... Error : " + ex2.Message + "\n" + ex2.StackTrace);
				}
			}
		}
		public static void DeleteDir(string aimPath)
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
						Backuptask.DeleteDir(aimPath + Path.GetFileName(path));
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
		public static int RestoreConfig(string str_file)
		{
			string text = "";
			string text2 = "";
			if (DBUrl.SERVERMODE)
			{
				return DBTools.RestoreConfiguration4ServerMode(str_file);
			}
			string destFileName = AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb";
			try
			{
				if (File.Exists(str_file))
				{
					string text3 = AppDomain.CurrentDomain.BaseDirectory;
					if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
					{
						text3 += Path.DirectorySeparatorChar;
					}
					text3 += "tmprestorefolder";
					if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
					{
						text3 += Path.DirectorySeparatorChar;
					}
					if (!Directory.Exists(text3))
					{
						Directory.CreateDirectory(text3);
					}
					else
					{
						Backuptask.DeleteDir(text3);
						Directory.CreateDirectory(text3);
					}
					string text4 = text3 + "sysdb.zip";
					try
					{
						int num = 0;
						int i_length = 0;
						Hashtable dBFileInfo_newversion = DBUtil.GetDBFileInfo_newversion(str_file);
						if (dBFileInfo_newversion == null || dBFileInfo_newversion.Count < 2)
						{
							num = 0;
						}
						int result;
						if (dBFileInfo_newversion.ContainsKey("ECOTYPE"))
						{
							string text5 = (string)dBFileInfo_newversion["ECOTYPE"];
							if (text5.Equals("MASTER"))
							{
								num = 1;
								if (!DBUrl.IsServer)
								{
									result = -1;
									return result;
								}
							}
							else
							{
								num = 2;
							}
						}
						if (dBFileInfo_newversion.ContainsKey("INFOLENGTH"))
						{
							i_length = Convert.ToInt32(dBFileInfo_newversion["INFOLENGTH"]);
						}
						if (dBFileInfo_newversion.ContainsKey("SYSDBVERSION"))
						{
							text = Convert.ToString(dBFileInfo_newversion["SYSDBVERSION"]);
						}
						if (dBFileInfo_newversion.ContainsKey("HEADVERSION"))
						{
							text2 = Convert.ToString(dBFileInfo_newversion["HEADVERSION"]);
						}
						if (text2.Length > 0 && text2.CompareTo("1.0.0") > 0)
						{
							result = -1;
							return result;
						}
						if (text.Length > 0 && "1.4.0.7".CompareTo(text) < 0)
						{
							result = -1;
							return result;
						}
						switch (num)
						{
						case 0:
							if (DBUrl.IsServer)
							{
								try
								{
									AESEncryptionUtility.Decrypt(str_file, text4, "ertsaM^tenBakCfgPassw0rd");
									break;
								}
								catch
								{
									try
									{
										File.Delete(text4);
									}
									catch
									{
									}
									result = -1;
									return result;
								}
							}
							AESEncryptionUtility.Decrypt(str_file, text4, "^tenBakCfgPassw0rd");
							break;
						case 1:
							if (!DBUrl.IsServer)
							{
								result = -1;
								return result;
							}
							AESEncryptionUtility.Decrypt(i_length, str_file, text4, "ertsaM^tenBakCfgPassw0rd");
							break;
						case 2:
							AESEncryptionUtility.Decrypt(i_length, str_file, text4, "^tenBakCfgPassw0rd");
							break;
						}
						if (!File.Exists(text3 + "sysdb.zip"))
						{
							result = -1;
							return result;
						}
						using (ZipArchive zipArchive = ZipArchive.OpenOnFile(text4))
						{
							string text6 = text3;
							foreach (ZipArchive.ZipFileInfo current in zipArchive.Files)
							{
								if (!current.FolderFlag)
								{
									using (Stream stream = current.GetStream())
									{
										string path = text6 + Path.GetDirectoryName(current.Name) + Path.DirectorySeparatorChar;
										if (!Directory.Exists(path))
										{
											Directory.CreateDirectory(path);
										}
										using (FileStream fileStream = new FileStream(text6 + current.Name, FileMode.Create))
										{
											byte[] array = new byte[67108863];
											for (int i = stream.Read(array, 0, array.Length); i > 0; i = stream.Read(array, 0, array.Length))
											{
												fileStream.Write(array, 0, i);
											}
											fileStream.Flush();
										}
									}
								}
							}
						}
						if (File.Exists(text3 + "sysdb.mdb"))
						{
							if (DBUrl.IsServer)
							{
								DbConnection dBConnection = DBMaintain.GetDBConnection(text3 + "sysdb.mdb");
								if (dBConnection == null || dBConnection.State != ConnectionState.Open)
								{
									Backuptask.DeleteDir(text3);
									result = -1;
									return result;
								}
								if (DBUtil.SetSysDBSerial(dBConnection) < 0)
								{
									try
									{
										dBConnection.Close();
									}
									catch
									{
									}
									Backuptask.DeleteDir(text3);
									result = -1;
									return result;
								}
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
								{
									if (DBMaintain.SetMySQLConnectionInfo(dBConnection, DBUrl.DB_CURRENT_NAME, DBUrl.CURRENT_HOST_PATH, DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD) < 0)
									{
										try
										{
											dBConnection.Close();
										}
										catch
										{
										}
										Backuptask.DeleteDir(text3);
										result = -1;
										return result;
									}
									string serverID = DBUtil.GetServerID();
									if (serverID != null && serverID.Length >= 1 && !serverID.Equals("DBERROR"))
									{
										if (!serverID.Equals("DBNONE"))
										{
											if (DBUtil.SetServerID(dBConnection, serverID) < 0)
											{
												try
												{
													dBConnection.Close();
												}
												catch
												{
												}
												Backuptask.DeleteDir(text3);
												result = -1;
												return result;
											}
											goto IL_4C7;
										}
									}
									try
									{
										dBConnection.Close();
									}
									catch
									{
									}
									Backuptask.DeleteDir(text3);
									result = -1;
									return result;
								}
								else
								{
									if (DBMaintain.SetAccessConnectionInfo(dBConnection) < 0)
									{
										try
										{
											dBConnection.Close();
										}
										catch
										{
										}
										Backuptask.DeleteDir(text3);
										result = -1;
										return result;
									}
								}
								try
								{
									IL_4C7:
									dBConnection.Close();
								}
								catch
								{
								}
								File.Copy(text3 + "sysdb.mdb", destFileName, true);
							}
							else
							{
								try
								{
									DbConnection dBConnection2 = DBMaintain.GetDBConnection(text3 + "sysdb.mdb");
									if (dBConnection2 == null || dBConnection2.State != ConnectionState.Open)
									{
										Backuptask.DeleteDir(text3);
										result = -1;
										return result;
									}
									if (DBUtil.SetSysDBSerial(dBConnection2) < 0)
									{
										try
										{
											dBConnection2.Close();
										}
										catch
										{
										}
									}
									if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
									{
										if (DBMaintain.SetMySQLConnectionInfo(dBConnection2, DBUrl.DB_CURRENT_NAME, DBUrl.CURRENT_HOST_PATH, DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD) < 0)
										{
											try
											{
												dBConnection2.Close();
											}
											catch
											{
											}
										}
										string serverID2 = DBUtil.GetServerID();
										if (serverID2 != null && serverID2.Length >= 1 && !serverID2.Equals("DBERROR"))
										{
											if (!serverID2.Equals("DBNONE"))
											{
												goto IL_5B9;
											}
										}
										try
										{
											dBConnection2.Close();
										}
										catch
										{
										}
										IL_5B9:
										if (DBUtil.SetServerID(dBConnection2, serverID2) >= 0)
										{
											goto IL_5E7;
										}
										try
										{
											dBConnection2.Close();
											goto IL_5E7;
										}
										catch
										{
											goto IL_5E7;
										}
									}
									if (DBMaintain.SetAccessConnectionInfo(dBConnection2) < 0)
									{
										try
										{
											dBConnection2.Close();
										}
										catch
										{
										}
									}
									try
									{
										IL_5E7:
										dBConnection2.Close();
									}
									catch
									{
									}
									File.Copy(text3 + "sysdb.mdb", destFileName, true);
								}
								catch
								{
									Backuptask.DeleteDir(text3);
									result = -1;
									return result;
								}
							}
							Backuptask.DeleteDir(text3);
							Backuptask.SetRestoreFlag();
							result = 1;
							return result;
						}
						File.Delete(text4);
						result = -1;
						return result;
					}
					catch
					{
						File.Delete(text4);
						int result = -1;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return -1;
		}
		private static int SetRestoreFlag()
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = new OleDbCommand();
			int result;
			try
			{
				string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor";
				oleDbConnection = new OleDbConnection(connectionString);
				oleDbConnection.Open();
				oleDbCommand = new OleDbCommand();
				oleDbCommand.Connection = oleDbConnection;
				oleDbCommand.CommandText = "update device_base_info set restoreflag = 1 ";
				oleDbCommand.ExecuteNonQuery();
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
				result = 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~~Set Restore Flag Error : " + ex.Message + "\n" + ex.StackTrace);
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
				result = -1;
			}
			return result;
		}
		public static bool IsSameServer()
		{
			string text = DBUrl.CONNECT_STRING.Split(new string[]
			{
				","
			}, StringSplitOptions.RemoveEmptyEntries)[0];
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface[] array = allNetworkInterfaces;
				for (int i = 0; i < array.Length; i++)
				{
					NetworkInterface networkInterface = array[i];
					IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
					UnicastIPAddressInformationCollection unicastAddresses = iPProperties.UnicastAddresses;
					foreach (UnicastIPAddressInformation current in unicastAddresses)
					{
						if (text.Equals(current.Address.ToString()))
						{
							return true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile(ex.Message);
			}
			return false;
		}
		public static int BackupConfig4UI(string str_folder)
		{
			int num = -1;
			int result;
			if (DBUrl.SERVERMODE)
			{
				if (!Backuptask.IsSameServer())
				{
					return -2;
				}
				if (str_folder[str_folder.Length - 1] != Path.DirectorySeparatorChar)
				{
					str_folder += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(str_folder))
				{
					try
					{
						Directory.CreateDirectory(str_folder);
					}
					catch
					{
						result = num;
						return result;
					}
				}
				string text = DBTools.BackupConfiguration4ServerMode();
				if (text != null && text.Length > 0)
				{
					try
					{
						string str = DateTime.Now.ToString("yyyyMMddHHmmss");
						string str2 = "config_backup" + str + "." + "dat";
						File.Copy(text, str_folder + str2, true);
					}
					catch
					{
						File.Delete(text);
						result = num;
						return result;
					}
					File.Delete(text);
					return 5;
				}
				return -3;
			}
			else
			{
				if (str_folder[str_folder.Length - 1] != Path.DirectorySeparatorChar)
				{
					str_folder += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(str_folder))
				{
					try
					{
						Directory.CreateDirectory(str_folder);
					}
					catch
					{
						result = num;
						return result;
					}
				}
				string text2 = Backuptask.createbakfile();
				if (text2 != null && text2.Length > 0)
				{
					try
					{
						File.Copy(text2, str_folder + Path.GetFileName(text2), true);
					}
					catch
					{
						Backuptask.DeleteDir(Path.GetDirectoryName(text2));
						result = num;
						return result;
					}
					Backuptask.DeleteDir(Path.GetDirectoryName(text2));
					return 5;
				}
				return num;
			}
			return result;
		}
	}
}
