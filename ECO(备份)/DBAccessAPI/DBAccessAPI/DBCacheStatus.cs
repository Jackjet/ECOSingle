using CommonAPI.InterProcess;
using System;
using System.IO;
using System.Threading;
namespace DBAccessAPI
{
	public class DBCacheStatus
	{
		public const string _dbSyncEventName = "Global\\EcoSensorsDBSyncGlobalEvent";
		public static bool b_showdebug;
		public static EventWaitHandle _dbSyncEvent;
		private static object _PollingDataLock;
		private static bool b_pollingdata;
		private static object _thresholdcount_lock;
		private static int i_upthrshd;
		private static object _upenergy_lock;
		private static bool b_occupa_energy;
		private static object _upvoltage_lock;
		private static bool b_occupa_vol;
		private static object _rackpd_lock;
		private static bool b_occrackpd;
		private static object _uppddata_lock;
		private static bool b_occupapd;
		private static object _upminutedata_lock;
		private static bool b_occupapdp;
		private static object _upthreshold_lock;
		public static bool b_occupa;
		private static object _rackeffectdb_lock;
		private static DateTime DT_LastUpRackEffect;
		private static bool b_occupa_rackeff;
		private static object _thermaldb_lock;
		private static DateTime DT_LastUpThermal;
		private static bool b_occupa_thermal;
		private static object _laskinsert;
		private static DateTime DT_Last;
		private static object _lockservice;
		private static bool b_service;
		private static object _lockdevtopo;
		private static bool b_devtopo;
		private static object _lockrack;
		private static bool b_rack;
		private static object _lockzone;
		private static bool b_zone;
		private static object _lockbackuptask;
		private static bool b_btask;
		private static object _lockuser;
		private static bool b_user;
		private static object _lockgrouptask;
		private static bool b_gtask;
		private static object _lockgroup;
		private static bool b_group;
		private static object _lockevent;
		private static bool b_event;
		private static object _locksmtp;
		private static bool b_smtp;
		private static object _locksyspara;
		private static bool b_syspara;
		private static object _lockdevvol;
		private static bool b_devvol;
		public static bool SaveDataThreadReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._PollingDataLock)
				{
					if (DBCacheStatus.b_pollingdata)
					{
						DBCacheStatus.b_pollingdata = false;
						result = true;
					}
					else
					{
						result = false;
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._PollingDataLock)
				{
					if (value)
					{
						DBCacheStatus.b_pollingdata = true;
					}
				}
			}
		}
		public static int FullUPThresholdCount
		{
			get
			{
				int result;
				lock (DBCacheStatus._thresholdcount_lock)
				{
					result = DBCacheStatus.i_upthrshd;
				}
				return result;
			}
		}
		public static bool EnergyDBReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._upenergy_lock)
				{
					if (DBCacheStatus.b_occupa_energy)
					{
						result = false;
					}
					else
					{
						DBCacheStatus.b_occupa_energy = true;
						result = true;
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._upenergy_lock)
				{
					if (value)
					{
						DBCacheStatus.b_occupa_energy = false;
					}
				}
			}
		}
		public static bool VoltageDBReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._upvoltage_lock)
				{
					if (DBCacheStatus.b_occupa_vol)
					{
						result = false;
					}
					else
					{
						DBCacheStatus.b_occupa_vol = true;
						result = true;
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._upvoltage_lock)
				{
					if (value)
					{
						DBCacheStatus.b_occupa_vol = false;
					}
				}
			}
		}
		public static bool RackPDReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._rackpd_lock)
				{
					if (DBCacheStatus.b_occrackpd)
					{
						result = false;
					}
					else
					{
						DBCacheStatus.b_occrackpd = true;
						result = true;
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._rackpd_lock)
				{
					if (value)
					{
						DBCacheStatus.b_occrackpd = false;
					}
				}
			}
		}
		public static bool PDDataDBReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._uppddata_lock)
				{
					if (DBCacheStatus.b_occupapd)
					{
						result = false;
					}
					else
					{
						DBCacheStatus.b_occupapd = true;
						result = true;
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._uppddata_lock)
				{
					if (value)
					{
						DBCacheStatus.b_occupapd = false;
					}
				}
			}
		}
		public static bool MinuteDataDBReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._upminutedata_lock)
				{
					if (DBCacheStatus.b_occupapdp)
					{
						result = false;
					}
					else
					{
						DBCacheStatus.b_occupapdp = true;
						result = true;
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._upminutedata_lock)
				{
					if (value)
					{
						DBCacheStatus.b_occupapdp = false;
						InSnergyGateway.Need_Calculate_PUE = true;
					}
				}
			}
		}
		public static bool ThresholdDBReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._upthreshold_lock)
				{
					if (DBCacheStatus.b_occupa)
					{
						result = false;
					}
					else
					{
						DBCacheStatus.b_occupa = true;
						result = true;
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._upthreshold_lock)
				{
					if (value)
					{
						DBCacheStatus.b_occupa = false;
					}
				}
			}
		}
		public static bool RackEffectDBReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._rackeffectdb_lock)
				{
					if (DBCacheStatus.b_occupa_rackeff)
					{
						result = false;
					}
					else
					{
						if (Math.Abs((DateTime.Now - DBCacheStatus.DT_LastUpRackEffect).TotalSeconds) > 5.0)
						{
							DBCacheStatus.b_occupa_rackeff = true;
							result = true;
						}
						else
						{
							result = false;
						}
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._rackeffectdb_lock)
				{
					if (value)
					{
						DBCacheStatus.DT_LastUpRackEffect = DateTime.Now;
						DBCacheStatus.b_occupa_rackeff = false;
					}
				}
			}
		}
		public static bool ThermalDBReady
		{
			get
			{
				bool result;
				lock (DBCacheStatus._thermaldb_lock)
				{
					if (DBCacheStatus.b_occupa_thermal)
					{
						result = false;
					}
					else
					{
						if (Math.Abs((DateTime.Now - DBCacheStatus.DT_LastUpThermal).TotalSeconds) > 15.0)
						{
							DBCacheStatus.b_occupa_thermal = true;
							result = true;
						}
						else
						{
							result = false;
						}
					}
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._thermaldb_lock)
				{
					if (value)
					{
						DBCacheStatus.DT_LastUpThermal = DateTime.Now;
						DBCacheStatus.b_occupa_thermal = false;
					}
				}
			}
		}
		public static DateTime LastInsertTime
		{
			get
			{
				DateTime dT_Last;
				lock (DBCacheStatus._laskinsert)
				{
					dT_Last = DBCacheStatus.DT_Last;
				}
				return dT_Last;
			}
			set
			{
				lock (DBCacheStatus._laskinsert)
				{
					DBCacheStatus.DT_Last = value;
				}
			}
		}
		public static bool ServiceRefreshFlag
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockservice)
				{
					result = DBCacheStatus.b_service;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockservice)
				{
					DBCacheStatus.b_service = value;
				}
			}
		}
		public static bool Device
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockdevtopo)
				{
					result = DBCacheStatus.b_devtopo;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockdevtopo)
				{
					DBCacheStatus.b_devtopo = value;
					if (value)
					{
						DBCacheStatus.b_group = true;
					}
				}
			}
		}
		public static bool Rack
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockrack)
				{
					result = DBCacheStatus.b_rack;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockrack)
				{
					DBCacheStatus.b_rack = value;
					if (value)
					{
						DBCacheStatus.b_group = true;
					}
				}
			}
		}
		public static bool ZONE
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockzone)
				{
					result = DBCacheStatus.b_zone;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockzone)
				{
					DBCacheStatus.b_zone = value;
					if (value)
					{
						DBCacheStatus.b_group = true;
					}
				}
			}
		}
		public static bool BackupTask
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockbackuptask)
				{
					result = DBCacheStatus.b_btask;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockbackuptask)
				{
					DBCacheStatus.b_btask = value;
				}
			}
		}
		public static bool User
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockuser)
				{
					result = DBCacheStatus.b_user;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockuser)
				{
					DBCacheStatus.b_user = value;
				}
			}
		}
		public static bool GroupTask
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockgrouptask)
				{
					result = DBCacheStatus.b_gtask;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockgrouptask)
				{
					DBCacheStatus.b_gtask = value;
				}
			}
		}
		public static bool Group
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockgroup)
				{
					result = DBCacheStatus.b_group;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockgroup)
				{
					DBCacheStatus.b_group = value;
				}
			}
		}
		public static bool Event
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockevent)
				{
					result = DBCacheStatus.b_event;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockevent)
				{
					DBCacheStatus.b_event = value;
				}
			}
		}
		public static bool Smtp
		{
			get
			{
				bool result;
				lock (DBCacheStatus._locksmtp)
				{
					result = DBCacheStatus.b_smtp;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._locksmtp)
				{
					DBCacheStatus.b_smtp = value;
				}
			}
		}
		public static bool SystemParameter
		{
			get
			{
				bool result;
				lock (DBCacheStatus._locksyspara)
				{
					result = DBCacheStatus.b_syspara;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._locksyspara)
				{
					DBCacheStatus.b_syspara = value;
				}
			}
		}
		public static bool DeviceVoltage
		{
			get
			{
				bool result;
				lock (DBCacheStatus._lockdevvol)
				{
					result = DBCacheStatus.b_devvol;
				}
				return result;
			}
			set
			{
				lock (DBCacheStatus._lockdevvol)
				{
					DBCacheStatus.b_devvol = value;
				}
			}
		}
		static DBCacheStatus()
		{
			DBCacheStatus.b_showdebug = true;
			DBCacheStatus._dbSyncEvent = null;
			DBCacheStatus._PollingDataLock = new object();
			DBCacheStatus.b_pollingdata = true;
			DBCacheStatus._thresholdcount_lock = new object();
			DBCacheStatus.i_upthrshd = 0;
			DBCacheStatus._upenergy_lock = new object();
			DBCacheStatus.b_occupa_energy = false;
			DBCacheStatus._upvoltage_lock = new object();
			DBCacheStatus.b_occupa_vol = false;
			DBCacheStatus._rackpd_lock = new object();
			DBCacheStatus.b_occrackpd = false;
			DBCacheStatus._uppddata_lock = new object();
			DBCacheStatus.b_occupapd = false;
			DBCacheStatus._upminutedata_lock = new object();
			DBCacheStatus.b_occupapdp = false;
			DBCacheStatus._upthreshold_lock = new object();
			DBCacheStatus.b_occupa = false;
			DBCacheStatus._rackeffectdb_lock = new object();
			DBCacheStatus.DT_LastUpRackEffect = new DateTime(1970, 1, 1, 0, 0, 0);
			DBCacheStatus.b_occupa_rackeff = false;
			DBCacheStatus._thermaldb_lock = new object();
			DBCacheStatus.DT_LastUpThermal = new DateTime(1970, 1, 1, 0, 0, 0);
			DBCacheStatus.b_occupa_thermal = false;
			DBCacheStatus._laskinsert = new object();
			DBCacheStatus.DT_Last = new DateTime(1970, 1, 1, 0, 0, 0);
			DBCacheStatus._lockservice = new object();
			DBCacheStatus.b_service = false;
			DBCacheStatus._lockdevtopo = new object();
			DBCacheStatus.b_devtopo = false;
			DBCacheStatus._lockrack = new object();
			DBCacheStatus.b_rack = false;
			DBCacheStatus._lockzone = new object();
			DBCacheStatus.b_zone = false;
			DBCacheStatus._lockbackuptask = new object();
			DBCacheStatus.b_btask = false;
			DBCacheStatus._lockuser = new object();
			DBCacheStatus.b_user = false;
			DBCacheStatus._lockgrouptask = new object();
			DBCacheStatus.b_gtask = false;
			DBCacheStatus._lockgroup = new object();
			DBCacheStatus.b_group = false;
			DBCacheStatus._lockevent = new object();
			DBCacheStatus.b_event = true;
			DBCacheStatus._locksmtp = new object();
			DBCacheStatus.b_smtp = true;
			DBCacheStatus._locksyspara = new object();
			DBCacheStatus.b_syspara = false;
			DBCacheStatus._lockdevvol = new object();
			DBCacheStatus.b_devvol = false;
			try
			{
				if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "debuglog.enable"))
				{
					DBCacheStatus.b_showdebug = true;
				}
				else
				{
					DBCacheStatus.b_showdebug = true;
				}
			}
			catch
			{
			}
		}
		public static void DBSyncEventInit(bool bInitValue)
		{
			if (DBCacheStatus._dbSyncEvent == null)
			{
				DBCacheStatus._dbSyncEvent = InterProcessBase.OpenGlobalEvent("Global\\EcoSensorsDBSyncGlobalEvent", bInitValue);
			}
		}
		public static void DBSyncEventSet(bool bValue, params string[] subEventNames)
		{
			for (int i = 0; i < subEventNames.Length; i++)
			{
				string name = subEventNames[i];
				InterProcessEvent.setGlobalEvent(name, bValue);
			}
			if (DBCacheStatus._dbSyncEvent != null)
			{
				DBCacheStatus._dbSyncEvent.Set();
			}
		}
		public static void IncreaseWaitLength()
		{
			lock (DBCacheStatus._thresholdcount_lock)
			{
				DBCacheStatus.i_upthrshd++;
			}
		}
		public static void ReduceWaitLength()
		{
			lock (DBCacheStatus._thresholdcount_lock)
			{
				DBCacheStatus.i_upthrshd--;
			}
		}
	}
}
