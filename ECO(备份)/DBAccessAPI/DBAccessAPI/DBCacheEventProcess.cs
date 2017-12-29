using CommonAPI;
using CommonAPI.InterProcess;
using System;
using System.Threading;
namespace DBAccessAPI
{
	public class DBCacheEventProcess
	{
		public const string DBSyncEventName_Service_Device = "DBSyncEventName_Service_Device";
		public const string DBSyncEventName_AP_Device = "DBSyncEventName_AP_Device";
		public const string DBSyncEventName_Service_Rack = "DBSyncEventName_Service_Rack";
		public const string DBSyncEventName_AP_Rack = "DBSyncEventName_AP_Rack";
		public const string DBSyncEventName_Service_ZONE = "DBSyncEventName_Service_ZONE";
		public const string DBSyncEventName_AP_ZONE = "DBSyncEventName_AP_ZONE";
		public const string DBSyncEventName_Service_BackupTask = "DBSyncEventName_Service_BackupTask";
		public const string DBSyncEventName_AP_BackupTask = "DBSyncEventName_AP_BackupTask";
		public const string DBSyncEventName_Service_User = "DBSyncEventName_Service_User";
		public const string DBSyncEventName_AP_User = "DBSyncEventName_AP_User";
		public const string DBSyncEventName_Service_GroupTask = "DBSyncEventName_Service_GroupTask";
		public const string DBSyncEventName_AP_GroupTask = "DBSyncEventName_AP_GroupTask";
		public const string DBSyncEventName_Service_Group = "DBSyncEventName_Service_Group";
		public const string DBSyncEventName_AP_Group = "DBSyncEventName_AP_Group";
		public const string DBSyncEventName_Service_Event = "DBSyncEventName_Service_Event";
		public const string DBSyncEventName_AP_Event = "DBSyncEventName_AP_Event";
		public const string DBSyncEventName_Service_Smtp = "DBSyncEventName_Service_Smtp";
		public const string DBSyncEventName_AP_Smtp = "DBSyncEventName_AP_Smtp";
		public const string DBSyncEventName_Service_SystemParameter = "DBSyncEventName_Service_SystemParameter";
		public const string DBSyncEventName_AP_SystemParameter = "DBSyncEventName_AP_SystemParameter";
		public const string DBSyncEventName_Service_DeviceVoltage = "DBSyncEventName_Service_DeviceVoltage";
		public const string DBSyncEventName_AP_DeviceVoltage = "DBSyncEventName_AP_DeviceVoltage";
		private static DBCacheEventProcess dbcep_instance;
		protected object _lockHandler = new object();
		private string _threadName = "DBCache Event Processor";
		private int _stopping;
		private Thread _procThread;
		private ManualResetEvent _stoppedEvent = new ManualResetEvent(true);
		private ManualResetEvent _abortEvent = new ManualResetEvent(false);
		private WaitHandle[] _waitHandles;
		private bool run_as_service;
		public static void StartRefreshThread(bool b_runasservice)
		{
			if (DBCacheEventProcess.dbcep_instance == null)
			{
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Device", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Device", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Rack", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Rack", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_ZONE", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_ZONE", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_BackupTask", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_BackupTask", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_User", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_User", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_GroupTask", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_GroupTask", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Group", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Group", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Event", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Event", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Smtp", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Smtp", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_SystemParameter", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_SystemParameter", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_DeviceVoltage", false);
				InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_DeviceVoltage", false);
				DBCacheEventProcess.dbcep_instance = new DBCacheEventProcess();
				DBCacheEventProcess.dbcep_instance.run_as_service = b_runasservice;
				DBCacheEventProcess.dbcep_instance.Start();
			}
		}
		public static bool GetRunASFlag()
		{
			return DBCacheEventProcess.dbcep_instance == null || DBCacheEventProcess.dbcep_instance.run_as_service;
		}
		public static void StopRefreshThread()
		{
			if (DBCacheEventProcess.dbcep_instance != null)
			{
				DBCacheEventProcess.dbcep_instance.Stop();
			}
		}
		public void Dispose()
		{
		}
		private DBCacheEventProcess()
		{
			WaitHandle[] waitHandles = new WaitHandle[2];
			this._waitHandles = waitHandles;
			base..ctor();
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			DBCacheStatus._dbSyncEvent.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = DBCacheStatus._dbSyncEvent;
		}
		public bool Start()
		{
			DebugCenter.GetInstance().appendToFile("Start DBCache Event Processor " + this._threadName + " thread");
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			DBCacheStatus._dbSyncEvent.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = DBCacheStatus._dbSyncEvent;
			this._procThread = new Thread(new ParameterizedThreadStart(this.WorkThread));
			this._procThread.Name = this._threadName;
			this._procThread.Start();
			return true;
		}
		public void Stop()
		{
			DebugCenter.GetInstance().appendToFile("Stopping DBCache Event Processor " + this._threadName + " thread");
			Interlocked.Exchange(ref this._stopping, 1);
			if (this._abortEvent != null)
			{
				this._abortEvent.Set();
			}
			try
			{
				if (!this._stoppedEvent.WaitOne(1000))
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
		private void WorkThread(object state)
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
					bool flag = false;
					if (this.run_as_service)
					{
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_BackupTask", false))
						{
							flag = true;
							DBCacheStatus.BackupTask = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_BackupTask", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Device", false))
						{
							flag = true;
							DBCacheStatus.Device = true;
							DBCacheStatus.Group = true;
							DBCacheStatus.ServiceRefreshFlag = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_Device", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_DeviceVoltage", false))
						{
							flag = true;
							DBCacheStatus.DeviceVoltage = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_DeviceVoltage", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Event", false))
						{
							flag = true;
							DBCacheStatus.Event = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_Event", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Group", false))
						{
							flag = true;
							DBCacheStatus.Group = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_Group", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_GroupTask", false))
						{
							flag = true;
							DBCacheStatus.GroupTask = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_GroupTask", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Rack", false))
						{
							flag = true;
							DBCacheStatus.Rack = true;
							DBCacheStatus.Group = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_Rack", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_Smtp", false))
						{
							flag = true;
							DBCacheStatus.Smtp = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_Smtp", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_SystemParameter", false))
						{
							flag = true;
							DBCacheStatus.SystemParameter = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_SystemParameter", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_User", false))
						{
							flag = true;
							DBCacheStatus.User = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_User", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_Service_ZONE", false))
						{
							flag = true;
							DBCacheStatus.ZONE = true;
							DBCacheStatus.Group = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_Service_ZONE", false);
						}
					}
					else
					{
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_BackupTask", false))
						{
							flag = true;
							DBCacheStatus.BackupTask = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_BackupTask", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Device", false))
						{
							flag = true;
							DBCacheStatus.Device = true;
							DBCacheStatus.Group = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_Device", false);
							DeviceInfo.SetRefreshFlag(1);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_DeviceVoltage", false))
						{
							flag = true;
							DBCacheStatus.DeviceVoltage = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_DeviceVoltage", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Event", false))
						{
							flag = true;
							DBCacheStatus.Event = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_Event", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Group", false))
						{
							flag = true;
							DBCacheStatus.Group = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_Group", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_GroupTask", false))
						{
							flag = true;
							DBCacheStatus.GroupTask = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_GroupTask", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Rack", false))
						{
							flag = true;
							DBCacheStatus.Rack = true;
							DBCacheStatus.Group = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_Rack", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_Smtp", false))
						{
							flag = true;
							DBCacheStatus.Smtp = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_Smtp", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_SystemParameter", false))
						{
							flag = true;
							DBCacheStatus.SystemParameter = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_SystemParameter", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_User", false))
						{
							flag = true;
							DBCacheStatus.User = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_User", false);
						}
						if (InterProcessEvent.getGlobalEvent("DBSyncEventName_AP_ZONE", false))
						{
							flag = true;
							DBCacheStatus.ZONE = true;
							DBCacheStatus.Group = true;
							InterProcessEvent.setGlobalEvent("DBSyncEventName_AP_ZONE", false);
						}
					}
					if (flag)
					{
						DBCacheStatus._dbSyncEvent.Reset();
					}
					else
					{
						Thread.Sleep(500);
					}
				}
			}
			DebugCenter.GetInstance().appendToFile("[" + this._threadName + "] thread end");
			this._stoppedEvent.Set();
		}
	}
}
