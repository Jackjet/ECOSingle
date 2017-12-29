using CommonAPI;
using System;
using System.Threading;
namespace DBAccessAPI
{
	public class LogDBStatus
	{
		public const int DB_AVAILABLE = 1;
		public const int DB_CLOSING = -1;
		public const int DB_CLOSED = -2;
		private static int i_dbstatus = 1;
		public static ManualResetEvent stopDBEvent = new ManualResetEvent(false);
		private static object thisLock = new object();
		public static int GetDBStatus()
		{
			int result;
			lock (LogDBStatus.thisLock)
			{
				result = LogDBStatus.i_dbstatus;
			}
			return result;
		}
		public static void SetDBStatus(int i_st)
		{
			lock (LogDBStatus.thisLock)
			{
				LogDBStatus.i_dbstatus = i_st;
				if (LogDBStatus.i_dbstatus == -1)
				{
					try
					{
						LogDBStatus.stopDBEvent.Set();
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("Set Log DB Maintaining Event~~~~~~~~~~~ERROR : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
		}
	}
}
