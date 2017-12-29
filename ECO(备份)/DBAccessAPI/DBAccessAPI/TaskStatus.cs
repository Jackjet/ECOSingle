using CommonAPI;
using CommonAPI.InterProcess;
using System;
namespace DBAccessAPI
{
	public class TaskStatus
	{
		public const int TS_NEEDCLEAN = 1;
		public const int TS_WAITDBSHUTDOWN = 2;
		public const int TS_WAITDBPOOLREADY = 3;
		public const int TS_WAITDBQUEUE = 4;
		public const int TS_CHGDBFINISH = 5;
		public const int TS_COMPACTFINISH = 6;
		public const int DB_AVAILABLE = 1;
		public const int DB_CLOSING = -1;
		private static int i_dbstatus = 1;
		private static object thisLock = new object();
		public static string TMPDB_path = "";
		public static string DB_compact_tmp = "";
		public static string DB_compact_final = "";
		public static int GetDBStatus()
		{
			int result;
			lock (TaskStatus.thisLock)
			{
				result = TaskStatus.i_dbstatus;
			}
			return result;
		}
		public static void SetDBStatus(int i_st)
		{
			lock (TaskStatus.thisLock)
			{
				TaskStatus.i_dbstatus = i_st;
				try
				{
					if (i_st == 1)
					{
						InterProcessEvent.setGlobalEvent("Database_Maintaining", false);
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}
	}
}
