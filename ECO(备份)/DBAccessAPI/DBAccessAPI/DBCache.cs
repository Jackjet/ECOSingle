using CommonAPI;
using DBAccessAPI.user;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace DBAccessAPI
{
	public class DBCache
	{
		public delegate int DelegateGetSensorCount(string str_model, string str_fwversion);
		public static string DEVICE_TABLE_SQL = "select id,device_ip,device_nm,mac,privacypw,authenpw,privacy,authen,timeout,retry,user_name,portid,snmpVersion,model_nm,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,rack_id,fw_version,pop_flag,pop_threshold,door,device_capacity,restoreflag ,outlet_pop,pop_lifo,pop_priority,b_priority,reference_voltage from device_base_info ";
		public static string BANK_TABLE_SQL = "select id,device_id,port_nums,bank_nm,voltage,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current from bank_info ";
		public static string LINE_TABLE_SQL = "select id,device_id,line_name,line_number,max_voltage,min_voltage,max_power,min_power,max_current,min_current from line_info ";
		public static string PORT_TABLE_SQL = "select id,device_id,port_num,port_nm,port_confirmation,port_ondelay_time,port_offdelay_time,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,shutdown_method,mac from port_info ";
		public static string SENSOR_TABLE_SQL = "select id,device_id,sensor_nm,max_humidity,min_humidity,max_temperature,min_temperature,max_press,min_press,sensor_type,location_type from device_sensor_info ";
		public static DBCache.DelegateGetSensorCount GetSensorCount = null;
		private static Hashtable ht_linecache = new Hashtable();
		private static Hashtable ht_devcache = new Hashtable();
		private static Hashtable ht_bankcache = new Hashtable();
		private static Hashtable ht_portcache = new Hashtable();
		private static Hashtable ht_sensorcache = new Hashtable();
		private static Hashtable ht_devbankmap = new Hashtable();
		private static Hashtable ht_devlinemap = new Hashtable();
		private static Hashtable ht_devportmap = new Hashtable();
		private static Hashtable ht_devsensormap = new Hashtable();
		private static Hashtable ht_rackdevmap = new Hashtable();
		private static object _lockHandler = new object();
		private static object _lockMU = new object();
		private static object _lockinterval = new object();
		private static long _l_mu = 0L;
		private static int _i_interval = 900;
		private static Hashtable ht_rackcache = new Hashtable();
		private static object _lockrack = new object();
		private static Hashtable ht_zonecache = new Hashtable();
		private static Hashtable ht_rackzonemap = new Hashtable();
		private static object _lockzone = new object();
		private static Hashtable ht_backuptaskcache = new Hashtable();
		private static Hashtable ht_backuptaskschedule = new Hashtable();
		private static object _lockbackuptask = new object();
		private static Hashtable ht_usercache = new Hashtable();
		private static Hashtable ht_usergroupmap = new Hashtable();
		private static Hashtable ht_userdevmap = new Hashtable();
		private static object _lockuser = new object();
		private static Hashtable ht_grouptaskcache = new Hashtable();
		private static Hashtable ht_grouptaskschedule = new Hashtable();
		private static object _lockgrouptask = new object();
		private static Hashtable ht_groupcache = new Hashtable();
		private static Hashtable ht_groupdestidmap = new Hashtable();
		private static object _lockgroup = new object();
		private static Hashtable ht_eventlogcache = new Hashtable();
		private static Hashtable ht_eventmailcache = new Hashtable();
		private static object _lockevent = new object();
		private static SMTPSetting smtp = null;
		private static object _locksmtp = new object();
		private static Hashtable ht_keyvaluecache = new Hashtable();
		private static object _locksys = new object();
		private static Hashtable ht_devvolcache = new Hashtable();
		private static object _lockdevvol = new object();
		private static object _locksysdb = new object();
		private static int i_sysdb = 0;
		private static Hashtable ht_sysdb = new Hashtable();
		private static object _lockdatadb = new object();
		private static int i_datadb = 0;
		private static Hashtable ht_datadb = new Hashtable();
		private static object _lockthermaldb = new object();
		private static int i_thermaldb = 0;
		private static Hashtable ht_thermaldb = new Hashtable();
		public static void DBCacheInit(bool b_run_as_service)
		{
			DBCacheStatus.BackupTask = true;
			DBCacheStatus.Device = true;
			DBCacheStatus.DeviceVoltage = true;
			DBCacheStatus.Event = true;
			DBCacheStatus.Group = true;
			DBCacheStatus.GroupTask = true;
			DBCacheStatus.Rack = true;
			DBCacheStatus.Smtp = true;
			DBCacheStatus.SystemParameter = true;
			DBCacheStatus.User = true;
			DBCacheStatus.ZONE = true;
			DBConn dBConn = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn != null && dBConn.con != null)
				{
					DBCache.GetDeviceCache(dBConn);
					DBCache.GetRackCache(dBConn);
					DBCache.GetZoneCache(dBConn);
					DBCache.GetBackupTaskCache(dBConn);
					DBCache.GetUserCache(dBConn);
					DBCache.GetGroupTaskCache(dBConn);
					DBCache.GetGroupCache(dBConn);
					DBCache.GetEventLogCache(dBConn);
					DBCache.GetSMTPSetting(dBConn);
					DBCache.GetSysParameterCache(dBConn);
					dBConn.Close();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Init DBCache Error : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
			}
		}
		public static void RefreshDBCache(DBConn conn)
		{
			lock (DBCache._lockHandler)
			{
				DataTable dataTable = new DataTable();
				DbCommand dbCommand = null;
				DbDataAdapter dbDataAdapter = null;
				try
				{
					if (conn != null && conn.con != null)
					{
						dbDataAdapter = DBConn.GetDataAdapter(conn.con);
						dbCommand = conn.con.CreateCommand();
						dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_devcache = new Hashtable();
							DBCache.ht_rackdevmap = new Hashtable();
							foreach (DataRow tmp_dr_d in dataTable.Rows)
							{
								DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
								DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
								int deviceID = deviceInfo.DeviceID;
								int num = Convert.ToInt32(deviceInfo.RackID);
								if (DBCache.ht_rackdevmap.ContainsKey(num))
								{
									List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
									list.Add(deviceID);
								}
								else
								{
									List<int> list2 = new List<int>();
									list2.Add(deviceID);
									DBCache.ht_rackdevmap.Add(num, list2);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(conn.con);
						dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_bankcache = new Hashtable();
							DBCache.ht_devbankmap = new Hashtable();
							foreach (DataRow tmp_dr_b in dataTable.Rows)
							{
								BankInfo bankInfo = new BankInfo(tmp_dr_b);
								DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
								int iD = bankInfo.ID;
								int deviceID2 = bankInfo.DeviceID;
								if (DBCache.ht_devbankmap.ContainsKey(deviceID2))
								{
									List<int> list3 = (List<int>)DBCache.ht_devbankmap[deviceID2];
									list3.Add(iD);
								}
								else
								{
									List<int> list4 = new List<int>();
									list4.Add(iD);
									DBCache.ht_devbankmap.Add(deviceID2, list4);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(conn.con);
						dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_portcache = new Hashtable();
							DBCache.ht_devportmap = new Hashtable();
							foreach (DataRow tmp_dr_p in dataTable.Rows)
							{
								PortInfo portInfo = new PortInfo(tmp_dr_p);
								DBCache.ht_portcache.Add(portInfo.ID, portInfo);
								int iD2 = portInfo.ID;
								int deviceID3 = portInfo.DeviceID;
								if (DBCache.ht_devportmap.ContainsKey(deviceID3))
								{
									List<int> list5 = (List<int>)DBCache.ht_devportmap[deviceID3];
									list5.Add(iD2);
								}
								else
								{
									List<int> list6 = new List<int>();
									list6.Add(iD2);
									DBCache.ht_devportmap.Add(deviceID3, list6);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(conn.con);
						dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_sensorcache = new Hashtable();
							DBCache.ht_devsensormap = new Hashtable();
							foreach (DataRow tmp_dr_s in dataTable.Rows)
							{
								SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
								DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
								int iD3 = sensorInfo.ID;
								int device_ID = sensorInfo.Device_ID;
								if (DBCache.ht_devsensormap.ContainsKey(device_ID))
								{
									List<int> list7 = (List<int>)DBCache.ht_devsensormap[device_ID];
									list7.Add(iD3);
								}
								else
								{
									List<int> list8 = new List<int>();
									list8.Add(iD3);
									DBCache.ht_devsensormap.Add(device_ID, list8);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(conn.con);
						dbCommand.CommandText = "select id,rack_nm,sx,sy,ex,ey,reserve from device_addr_info";
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_rackcache = new Hashtable();
							foreach (DataRow tmp_dr_z in dataTable.Rows)
							{
								RackInfo rackInfo = new RackInfo(tmp_dr_z);
								DBCache.ht_rackcache.Add(Convert.ToInt32(rackInfo.RackID), rackInfo);
							}
						}
						dataTable = new DataTable();
						dbCommand.Dispose();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
				}
			}
		}
		public static void RefreshDBCache()
		{
			lock (DBCache._lockHandler)
			{
				DataTable dataTable = new DataTable();
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				DbDataAdapter dbDataAdapter = null;
				try
				{
					dBConn = DBConnPool.getConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_devcache = new Hashtable();
							DBCache.ht_rackdevmap = new Hashtable();
							foreach (DataRow tmp_dr_d in dataTable.Rows)
							{
								DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
								DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
								int deviceID = deviceInfo.DeviceID;
								int num = Convert.ToInt32(deviceInfo.RackID);
								if (DBCache.ht_rackdevmap.ContainsKey(num))
								{
									List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
									list.Add(deviceID);
								}
								else
								{
									List<int> list2 = new List<int>();
									list2.Add(deviceID);
									DBCache.ht_rackdevmap.Add(num, list2);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_bankcache = new Hashtable();
							DBCache.ht_devbankmap = new Hashtable();
							foreach (DataRow tmp_dr_b in dataTable.Rows)
							{
								BankInfo bankInfo = new BankInfo(tmp_dr_b);
								DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
								int iD = bankInfo.ID;
								int deviceID2 = bankInfo.DeviceID;
								if (DBCache.ht_devbankmap.ContainsKey(deviceID2))
								{
									List<int> list3 = (List<int>)DBCache.ht_devbankmap[deviceID2];
									list3.Add(iD);
								}
								else
								{
									List<int> list4 = new List<int>();
									list4.Add(iD);
									DBCache.ht_devbankmap.Add(deviceID2, list4);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_portcache = new Hashtable();
							DBCache.ht_devportmap = new Hashtable();
							foreach (DataRow tmp_dr_p in dataTable.Rows)
							{
								PortInfo portInfo = new PortInfo(tmp_dr_p);
								DBCache.ht_portcache.Add(portInfo.ID, portInfo);
								int iD2 = portInfo.ID;
								int deviceID3 = portInfo.DeviceID;
								if (DBCache.ht_devportmap.ContainsKey(deviceID3))
								{
									List<int> list5 = (List<int>)DBCache.ht_devportmap[deviceID3];
									list5.Add(iD2);
								}
								else
								{
									List<int> list6 = new List<int>();
									list6.Add(iD2);
									DBCache.ht_devportmap.Add(deviceID3, list6);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_sensorcache = new Hashtable();
							DBCache.ht_devsensormap = new Hashtable();
							foreach (DataRow tmp_dr_s in dataTable.Rows)
							{
								SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
								DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
								int iD3 = sensorInfo.ID;
								int device_ID = sensorInfo.Device_ID;
								if (DBCache.ht_devsensormap.ContainsKey(device_ID))
								{
									List<int> list7 = (List<int>)DBCache.ht_devsensormap[device_ID];
									list7.Add(iD3);
								}
								else
								{
									List<int> list8 = new List<int>();
									list8.Add(iD3);
									DBCache.ht_devsensormap.Add(device_ID, list8);
								}
							}
						}
						dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand.CommandText = "select id,rack_nm,sx,sy,ex,ey,reserve from device_addr_info";
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							DBCache.ht_rackcache = new Hashtable();
							foreach (DataRow tmp_dr_z in dataTable.Rows)
							{
								RackInfo rackInfo = new RackInfo(tmp_dr_z);
								DBCache.ht_rackcache.Add(Convert.ToInt32(rackInfo.RackID), rackInfo);
							}
						}
						dataTable = new DataTable();
						dbCommand.Dispose();
						dBConn.Close();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
			}
		}
		public static long GetMU()
		{
			long l_mu;
			lock (DBCache._lockMU)
			{
				l_mu = DBCache._l_mu;
			}
			return l_mu;
		}
		public static void SetMU(long lmu)
		{
			lock (DBCache._lockMU)
			{
				DBCache._l_mu = lmu;
			}
		}
		public static int GetInterval()
		{
			int i_interval;
			lock (DBCache._lockinterval)
			{
				i_interval = DBCache._i_interval;
			}
			return i_interval;
		}
		public static void SetInterval(int i_second)
		{
			lock (DBCache._lockinterval)
			{
				DBCache._i_interval = i_second;
			}
		}
		public static Hashtable GetLineCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_linecache;
				}
				else
				{
					result = DBCache.ht_linecache;
				}
			}
			return result;
		}
		public static Hashtable GetLineCache()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_linecache;
				}
				else
				{
					result = DBCache.ht_linecache;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_devcache;
				}
				else
				{
					result = DBCache.ht_devcache;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceCache()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_devcache;
				}
				else
				{
					result = DBCache.ht_devcache;
				}
			}
			return result;
		}
		public static Hashtable GetBankCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_bankcache;
				}
				else
				{
					result = DBCache.ht_bankcache;
				}
			}
			return result;
		}
		public static Hashtable GetBankCache()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_bankcache;
				}
				else
				{
					result = DBCache.ht_bankcache;
				}
			}
			return result;
		}
		public static Hashtable GetPortCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_portcache;
				}
				else
				{
					result = DBCache.ht_portcache;
				}
			}
			return result;
		}
		public static Hashtable GetPortCache()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_portcache;
				}
				else
				{
					result = DBCache.ht_portcache;
				}
			}
			return result;
		}
		public static Hashtable GetSensorCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_sensorcache;
				}
				else
				{
					result = DBCache.ht_sensorcache;
				}
			}
			return result;
		}
		public static Hashtable GetSensorCache()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_sensorcache;
				}
				else
				{
					result = DBCache.ht_sensorcache;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceBankMap(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_devbankmap;
				}
				else
				{
					result = DBCache.ht_devbankmap;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceBankMap()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_devbankmap;
				}
				else
				{
					result = DBCache.ht_devbankmap;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceLineMap(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_devlinemap;
				}
				else
				{
					result = DBCache.ht_devlinemap;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceLineMap()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_devlinemap;
				}
				else
				{
					result = DBCache.ht_devlinemap;
				}
			}
			return result;
		}
		public static Hashtable GetDevicePortMap(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_devportmap;
				}
				else
				{
					result = DBCache.ht_devportmap;
				}
			}
			return result;
		}
		public static Hashtable GetDevicePortMap()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_devportmap;
				}
				else
				{
					result = DBCache.ht_devportmap;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceSensorMap(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_devsensormap;
				}
				else
				{
					result = DBCache.ht_devsensormap;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceSensorMap()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_devsensormap;
				}
				else
				{
					result = DBCache.ht_devsensormap;
				}
			}
			return result;
		}
		public static Hashtable GetRackDeviceMap(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_rackdevmap;
				}
				else
				{
					result = DBCache.ht_rackdevmap;
				}
			}
			return result;
		}
		public static Hashtable GetRackDeviceMap()
		{
			Hashtable result;
			lock (DBCache._lockHandler)
			{
				if (DBCacheStatus.Device)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = DBCache.DEVICE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devcache = new Hashtable();
								DBCache.ht_rackdevmap = new Hashtable();
								foreach (DataRow tmp_dr_d in dataTable.Rows)
								{
									DeviceInfo deviceInfo = new DeviceInfo(tmp_dr_d);
									DBCache.ht_devcache.Add(deviceInfo.DeviceID, deviceInfo);
									int deviceID = deviceInfo.DeviceID;
									int num = Convert.ToInt32(deviceInfo.RackID);
									if (DBCache.ht_rackdevmap.ContainsKey(num))
									{
										List<int> list = (List<int>)DBCache.ht_rackdevmap[num];
										list.Add(deviceID);
									}
									else
									{
										List<int> list2 = new List<int>();
										list2.Add(deviceID);
										DBCache.ht_rackdevmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.LINE_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_linecache = new Hashtable();
								DBCache.ht_devlinemap = new Hashtable();
								foreach (DataRow tmp_dr_b in dataTable.Rows)
								{
									LineInfo lineInfo = new LineInfo(tmp_dr_b);
									DBCache.ht_linecache.Add(lineInfo.ID, lineInfo);
									int iD = lineInfo.ID;
									int deviceID2 = lineInfo.DeviceID;
									if (DBCache.ht_devlinemap.ContainsKey(deviceID2))
									{
										List<int> list3 = (List<int>)DBCache.ht_devlinemap[deviceID2];
										list3.Add(iD);
									}
									else
									{
										List<int> list4 = new List<int>();
										list4.Add(iD);
										DBCache.ht_devlinemap.Add(deviceID2, list4);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.BANK_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_bankcache = new Hashtable();
								DBCache.ht_devbankmap = new Hashtable();
								foreach (DataRow tmp_dr_b2 in dataTable.Rows)
								{
									BankInfo bankInfo = new BankInfo(tmp_dr_b2);
									DBCache.ht_bankcache.Add(bankInfo.ID, bankInfo);
									int iD2 = bankInfo.ID;
									int deviceID3 = bankInfo.DeviceID;
									if (DBCache.ht_devbankmap.ContainsKey(deviceID3))
									{
										List<int> list5 = (List<int>)DBCache.ht_devbankmap[deviceID3];
										list5.Add(iD2);
									}
									else
									{
										List<int> list6 = new List<int>();
										list6.Add(iD2);
										DBCache.ht_devbankmap.Add(deviceID3, list6);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.PORT_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_portcache = new Hashtable();
								DBCache.ht_devportmap = new Hashtable();
								foreach (DataRow tmp_dr_p in dataTable.Rows)
								{
									PortInfo portInfo = new PortInfo(tmp_dr_p);
									DBCache.ht_portcache.Add(portInfo.ID, portInfo);
									int iD3 = portInfo.ID;
									int deviceID4 = portInfo.DeviceID;
									if (DBCache.ht_devportmap.ContainsKey(deviceID4))
									{
										List<int> list7 = (List<int>)DBCache.ht_devportmap[deviceID4];
										list7.Add(iD3);
									}
									else
									{
										List<int> list8 = new List<int>();
										list8.Add(iD3);
										DBCache.ht_devportmap.Add(deviceID4, list8);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand.CommandText = DBCache.SENSOR_TABLE_SQL;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_sensorcache = new Hashtable();
								DBCache.ht_devsensormap = new Hashtable();
								foreach (DataRow tmp_dr_s in dataTable.Rows)
								{
									SensorInfo sensorInfo = new SensorInfo(tmp_dr_s);
									int iD4 = sensorInfo.ID;
									int device_ID = sensorInfo.Device_ID;
									int sensorCountByDid = DBCache.GetSensorCountByDid(DBCache.ht_devcache, device_ID);
									if (sensorCountByDid < 0)
									{
										DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
									}
									else
									{
										if (sensorInfo.Type <= sensorCountByDid)
										{
											DBCache.ht_sensorcache.Add(sensorInfo.ID, sensorInfo);
										}
									}
									if (DBCache.ht_devsensormap.ContainsKey(device_ID))
									{
										List<int> list9 = (List<int>)DBCache.ht_devsensormap[device_ID];
										list9.Add(iD4);
									}
									else
									{
										List<int> list10 = new List<int>();
										list10.Add(iD4);
										DBCache.ht_devsensormap.Add(device_ID, list10);
									}
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Device = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_rackdevmap;
				}
				else
				{
					result = DBCache.ht_rackdevmap;
				}
			}
			return result;
		}
		public static Hashtable GetRackCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockrack)
			{
				if (DBCacheStatus.Rack)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,rack_nm,sx,sy,ex,ey,reserve from device_addr_info";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_rackcache = new Hashtable();
								foreach (DataRow tmp_dr_z in dataTable.Rows)
								{
									RackInfo rackInfo = new RackInfo(tmp_dr_z);
									DBCache.ht_rackcache.Add(Convert.ToInt32(rackInfo.RackID), rackInfo);
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							DBCacheStatus.Rack = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshRackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_rackcache;
				}
				else
				{
					result = DBCache.ht_rackcache;
				}
			}
			return result;
		}
		public static Hashtable GetRackCache()
		{
			Hashtable result;
			lock (DBCache._lockrack)
			{
				if (DBCacheStatus.Rack)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,rack_nm,sx,sy,ex,ey,reserve from device_addr_info";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_rackcache = new Hashtable();
								foreach (DataRow tmp_dr_z in dataTable.Rows)
								{
									RackInfo rackInfo = new RackInfo(tmp_dr_z);
									DBCache.ht_rackcache.Add(Convert.ToInt32(rackInfo.RackID), rackInfo);
								}
							}
							dataTable = new DataTable();
							dbCommand.Dispose();
							dBConn.Close();
							DBCacheStatus.Rack = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshRackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
				}
				result = DBCache.ht_rackcache;
			}
			return result;
		}
		public static Hashtable GetZoneCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockzone)
			{
				if (DBCacheStatus.ZONE)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,zone_nm,racks,sx,sy,ex,ey,color,reserve from zone_info  order by zone_nm ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_zonecache = new Hashtable();
								DBCache.ht_rackzonemap = new Hashtable();
								foreach (DataRow tmp_dr_z in dataTable.Rows)
								{
									ZoneInfo zoneInfo = new ZoneInfo(tmp_dr_z);
									long zoneID = zoneInfo.ZoneID;
									DBCache.ht_zonecache.Add(zoneID, zoneInfo);
									string rackInfo = zoneInfo.RackInfo;
									string[] array = null;
									if (rackInfo != null && rackInfo.Length > 0)
									{
										string[] separator = new string[]
										{
											","
										};
										array = rackInfo.Split(separator, StringSplitOptions.RemoveEmptyEntries);
									}
									if (array != null && array.Length > 0)
									{
										string[] array2 = array;
										for (int i = 0; i < array2.Length; i++)
										{
											string value = array2[i];
											if (DBCache.ht_rackzonemap.ContainsKey(Convert.ToInt64(value)))
											{
												List<long> list = (List<long>)DBCache.ht_rackzonemap[Convert.ToInt64(value)];
												list.Add(zoneID);
											}
											else
											{
												List<long> list2 = new List<long>();
												list2.Add(zoneID);
												DBCache.ht_rackzonemap.Add(Convert.ToInt64(value), list2);
											}
										}
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.ZONE = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_zonecache;
				}
				else
				{
					result = DBCache.ht_zonecache;
				}
			}
			return result;
		}
		public static Hashtable GetZoneCache()
		{
			Hashtable result;
			lock (DBCache._lockzone)
			{
				if (DBCacheStatus.ZONE)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,zone_nm,racks,sx,sy,ex,ey,color,reserve from zone_info order by zone_nm ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_zonecache = new Hashtable();
								DBCache.ht_rackzonemap = new Hashtable();
								foreach (DataRow tmp_dr_z in dataTable.Rows)
								{
									ZoneInfo zoneInfo = new ZoneInfo(tmp_dr_z);
									long zoneID = zoneInfo.ZoneID;
									DBCache.ht_zonecache.Add(zoneID, zoneInfo);
									string rackInfo = zoneInfo.RackInfo;
									string[] array = null;
									if (rackInfo != null && rackInfo.Length > 0)
									{
										string[] separator = new string[]
										{
											","
										};
										array = rackInfo.Split(separator, StringSplitOptions.RemoveEmptyEntries);
									}
									if (array != null && array.Length > 0)
									{
										string[] array2 = array;
										for (int i = 0; i < array2.Length; i++)
										{
											string value = array2[i];
											if (DBCache.ht_rackzonemap.ContainsKey(Convert.ToInt64(value)))
											{
												List<long> list = (List<long>)DBCache.ht_rackzonemap[Convert.ToInt64(value)];
												list.Add(zoneID);
											}
											else
											{
												List<long> list2 = new List<long>();
												list2.Add(zoneID);
												DBCache.ht_rackzonemap.Add(Convert.ToInt64(value), list2);
											}
										}
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.ZONE = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_zonecache;
				}
				else
				{
					result = DBCache.ht_zonecache;
				}
			}
			return result;
		}
		public static Hashtable GetRackZoneMap(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockzone)
			{
				if (DBCacheStatus.ZONE)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,zone_nm,racks,sx,sy,ex,ey,color,reserve from zone_info  order by zone_nm ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_zonecache = new Hashtable();
								DBCache.ht_rackzonemap = new Hashtable();
								foreach (DataRow tmp_dr_z in dataTable.Rows)
								{
									ZoneInfo zoneInfo = new ZoneInfo(tmp_dr_z);
									long zoneID = zoneInfo.ZoneID;
									DBCache.ht_zonecache.Add(zoneID, zoneInfo);
									string rackInfo = zoneInfo.RackInfo;
									string[] array = null;
									if (rackInfo != null && rackInfo.Length > 0)
									{
										string[] separator = new string[]
										{
											","
										};
										array = rackInfo.Split(separator, StringSplitOptions.RemoveEmptyEntries);
									}
									if (array != null && array.Length > 0)
									{
										string[] array2 = array;
										for (int i = 0; i < array2.Length; i++)
										{
											string value = array2[i];
											if (DBCache.ht_rackzonemap.ContainsKey(Convert.ToInt64(value)))
											{
												List<long> list = (List<long>)DBCache.ht_rackzonemap[Convert.ToInt64(value)];
												list.Add(zoneID);
											}
											else
											{
												List<long> list2 = new List<long>();
												list2.Add(zoneID);
												DBCache.ht_rackzonemap.Add(Convert.ToInt64(value), list2);
											}
										}
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.ZONE = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_rackzonemap;
				}
				else
				{
					result = DBCache.ht_rackzonemap;
				}
			}
			return result;
		}
		public static Hashtable GetRackZoneMap()
		{
			Hashtable result;
			lock (DBCache._lockzone)
			{
				if (DBCacheStatus.ZONE)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,zone_nm,racks,sx,sy,ex,ey,color,reserve from zone_info  order by zone_nm ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_zonecache = new Hashtable();
								DBCache.ht_rackzonemap = new Hashtable();
								foreach (DataRow tmp_dr_z in dataTable.Rows)
								{
									ZoneInfo zoneInfo = new ZoneInfo(tmp_dr_z);
									long zoneID = zoneInfo.ZoneID;
									DBCache.ht_zonecache.Add(zoneID, zoneInfo);
									string rackInfo = zoneInfo.RackInfo;
									string[] array = null;
									if (rackInfo != null && rackInfo.Length > 0)
									{
										string[] separator = new string[]
										{
											","
										};
										array = rackInfo.Split(separator, StringSplitOptions.RemoveEmptyEntries);
									}
									if (array != null && array.Length > 0)
									{
										string[] array2 = array;
										for (int i = 0; i < array2.Length; i++)
										{
											string value = array2[i];
											if (DBCache.ht_rackzonemap.ContainsKey(Convert.ToInt64(value)))
											{
												List<long> list = (List<long>)DBCache.ht_rackzonemap[Convert.ToInt64(value)];
												list.Add(zoneID);
											}
											else
											{
												List<long> list2 = new List<long>();
												list2.Add(zoneID);
												DBCache.ht_rackzonemap.Add(Convert.ToInt64(value), list2);
											}
										}
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.ZONE = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDBCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_rackzonemap;
				}
				else
				{
					result = DBCache.ht_rackzonemap;
				}
			}
			return result;
		}
		public static Hashtable GetBackupTaskCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockbackuptask)
			{
				if (DBCacheStatus.BackupTask)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,tasktype,storetype,username,pwd,host,port,filepath from backuptask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskcache = new Hashtable();
								foreach (DataRow dr_bct in dataTable.Rows)
								{
									BackupTaskInfo backupTaskInfo = new BackupTaskInfo(dr_bct);
									DBCache.ht_backuptaskcache.Add(backupTaskInfo.ID, backupTaskInfo);
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype = 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_backuptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_backuptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_backuptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.BackupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshBackupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_backuptaskcache;
				}
				else
				{
					result = DBCache.ht_backuptaskcache;
				}
			}
			return result;
		}
		public static Hashtable GetBackupTaskCache()
		{
			Hashtable result;
			lock (DBCache._lockbackuptask)
			{
				if (DBCacheStatus.BackupTask)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,tasktype,storetype,username,pwd,host,port,filepath from backuptask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskcache = new Hashtable();
								foreach (DataRow dr_bct in dataTable.Rows)
								{
									BackupTaskInfo backupTaskInfo = new BackupTaskInfo(dr_bct);
									DBCache.ht_backuptaskcache.Add(backupTaskInfo.ID, backupTaskInfo);
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype = 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_backuptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_backuptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_backuptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.BackupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshBackupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_backuptaskcache;
				}
				else
				{
					result = DBCache.ht_backuptaskcache;
				}
			}
			return result;
		}
		public static Hashtable GetBackupTaskSchedule(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockbackuptask)
			{
				if (DBCacheStatus.BackupTask)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,tasktype,storetype,username,pwd,host,port,filepath from backuptask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskcache = new Hashtable();
								foreach (DataRow dr_bct in dataTable.Rows)
								{
									BackupTaskInfo backupTaskInfo = new BackupTaskInfo(dr_bct);
									DBCache.ht_backuptaskcache.Add(backupTaskInfo.ID, backupTaskInfo);
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype = 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_backuptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_backuptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_backuptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.BackupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshBackupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_backuptaskschedule;
				}
				else
				{
					result = DBCache.ht_backuptaskschedule;
				}
			}
			return result;
		}
		public static Hashtable GetBackupTaskSchedule()
		{
			Hashtable result;
			lock (DBCache._lockbackuptask)
			{
				if (DBCacheStatus.BackupTask)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,tasktype,storetype,username,pwd,host,port,filepath from backuptask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskcache = new Hashtable();
								foreach (DataRow dr_bct in dataTable.Rows)
								{
									BackupTaskInfo backupTaskInfo = new BackupTaskInfo(dr_bct);
									DBCache.ht_backuptaskcache.Add(backupTaskInfo.ID, backupTaskInfo);
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype = 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_backuptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_backuptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_backuptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_backuptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.BackupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshBackupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_backuptaskschedule;
				}
				else
				{
					result = DBCache.ht_backuptaskschedule;
				}
			}
			return result;
		}
		public static Hashtable GetUserCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockuser)
			{
				if (DBCacheStatus.User)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select uid,gid from ugp ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_usergroupmap = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									long num = Convert.ToInt64(dataRow[0]);
									long num2 = Convert.ToInt64(dataRow[1]);
									if (DBCache.ht_usergroupmap.ContainsKey(num))
									{
										DBCache.ht_usergroupmap[num] = DBCache.ht_usergroupmap[num] + "," + num2;
									}
									else
									{
										DBCache.ht_usergroupmap.Add(num, string.Concat(num2));
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select uid,did from udp ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_userdevmap = new Hashtable();
								foreach (DataRow dataRow2 in dataTable.Rows)
								{
									long num3 = Convert.ToInt64(dataRow2[0]);
									long num4 = Convert.ToInt64(dataRow2[1]);
									if (DBCache.ht_userdevmap.ContainsKey(num3))
									{
										DBCache.ht_userdevmap[num3] = DBCache.ht_userdevmap[num3] + "," + num4;
									}
									else
									{
										DBCache.ht_userdevmap.Add(num3, string.Concat(num4));
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,user_name,userpwd,enabled,role_type,user_right,port_nm,devices from sys_users order by user_name ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_usercache = new Hashtable();
								foreach (DataRow tmp_dr_u in dataTable.Rows)
								{
									UserInfo userInfo = new UserInfo(tmp_dr_u);
									if (DBCache.ht_usergroupmap.ContainsKey(userInfo.UserID))
									{
										userInfo.UserGroup = (string)DBCache.ht_usergroupmap[userInfo.UserID];
									}
									else
									{
										userInfo.UserGroup = "";
									}
									if (DBCache.ht_userdevmap.ContainsKey(userInfo.UserID))
									{
										userInfo.UserDevice = (string)DBCache.ht_userdevmap[userInfo.UserID];
									}
									else
									{
										userInfo.UserDevice = "";
									}
									DBCache.ht_usercache.Add(userInfo.UserID, userInfo);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.User = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshUserCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_usercache;
				}
				else
				{
					result = DBCache.ht_usercache;
				}
			}
			return result;
		}
		public static Hashtable GetUserCache()
		{
			Hashtable result;
			lock (DBCache._lockuser)
			{
				if (DBCacheStatus.User)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select uid,gid from ugp ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_usergroupmap = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									long num = Convert.ToInt64(dataRow[0]);
									long num2 = Convert.ToInt64(dataRow[1]);
									if (DBCache.ht_usergroupmap.ContainsKey(num))
									{
										DBCache.ht_usergroupmap[num] = DBCache.ht_usergroupmap[num] + "," + num2;
									}
									else
									{
										DBCache.ht_usergroupmap.Add(num, string.Concat(num2));
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select uid,did from udp ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_userdevmap = new Hashtable();
								foreach (DataRow dataRow2 in dataTable.Rows)
								{
									long num3 = Convert.ToInt64(dataRow2[0]);
									long num4 = Convert.ToInt64(dataRow2[1]);
									if (DBCache.ht_userdevmap.ContainsKey(num3))
									{
										DBCache.ht_userdevmap[num3] = DBCache.ht_userdevmap[num3] + "," + num4;
									}
									else
									{
										DBCache.ht_userdevmap.Add(num3, string.Concat(num4));
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,user_name,userpwd,enabled,role_type,user_right,port_nm,devices from sys_users order by user_name ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_usercache = new Hashtable();
								foreach (DataRow tmp_dr_u in dataTable.Rows)
								{
									UserInfo userInfo = new UserInfo(tmp_dr_u);
									if (DBCache.ht_usergroupmap.ContainsKey(userInfo.UserID))
									{
										userInfo.UserGroup = (string)DBCache.ht_usergroupmap[userInfo.UserID];
									}
									else
									{
										userInfo.UserGroup = "";
									}
									if (DBCache.ht_userdevmap.ContainsKey(userInfo.UserID))
									{
										userInfo.UserDevice = (string)DBCache.ht_userdevmap[userInfo.UserID];
									}
									else
									{
										userInfo.UserDevice = "";
									}
									DBCache.ht_usercache.Add(userInfo.UserID, userInfo);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.User = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshUserCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_usercache;
				}
				else
				{
					result = DBCache.ht_usercache;
				}
			}
			return result;
		}
		public static Hashtable GetGroupTaskCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockgrouptask)
			{
				if (DBCacheStatus.GroupTask)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype < 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_grouptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_grouptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,groupname,groupid,tasktype,status,reserve from groupcontroltask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskcache = new Hashtable();
								foreach (DataRow dr_gct in dataTable.Rows)
								{
									GroupControlTask groupControlTask = new GroupControlTask(dr_gct);
									if (DBCache.ht_grouptaskschedule.ContainsKey(Convert.ToInt32(groupControlTask.GroupID)))
									{
										Dictionary<string, SpecialDay> dictionary = new Dictionary<string, SpecialDay>();
										string[,] array = null;
										int num = 0;
										List<ScheduleInfo> list3 = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[Convert.ToInt32(groupControlTask.GroupID)];
										foreach (ScheduleInfo current in list3)
										{
											if (current.DayOfWeek == 8)
											{
												if (num == 0)
												{
													string[,] array2 = new string[1, 2];
													array2[0, 0] = "";
													array2[0, 1] = "";
													array = array2;
												}
												string text = "";
												try
												{
													DateTime arg_259_0 = current.ScheduleTime;
													text = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[0, 0] = text;
												}
												else
												{
													array[0, 1] = text;
												}
												num++;
											}
											if (current.DayOfWeek < 8)
											{
												if (num == 0)
												{
													string[,] array3 = new string[7, 2];
													array3[0, 0] = "";
													array3[0, 1] = "";
													array3[1, 0] = "";
													array3[1, 1] = "";
													array3[2, 0] = "";
													array3[2, 1] = "";
													array3[3, 0] = "";
													array3[3, 1] = "";
													array3[4, 0] = "";
													array3[4, 1] = "";
													array3[5, 0] = "";
													array3[5, 1] = "";
													array3[6, 0] = "";
													array3[6, 1] = "";
													array = array3;
												}
												string text2 = "";
												try
												{
													DateTime arg_391_0 = current.ScheduleTime;
													text2 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[current.DayOfWeek - 1, 0] = text2;
												}
												else
												{
													array[current.DayOfWeek - 1, 1] = text2;
												}
												num++;
											}
											if (current.DayOfWeek > 8)
											{
												string text3 = "";
												string text4 = "";
												string text5 = "";
												text3 = current.Reserve;
												if (text3 == null)
												{
													text3 = "";
												}
												SpecialDay specialDay = null;
												string text6 = "";
												try
												{
													DateTime arg_42D_0 = current.ScheduleTime;
													text6 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													text4 = text6;
													specialDay = new SpecialDay(text3, text4, "");
												}
												else
												{
													if (current.OperationType == 0)
													{
														text5 = text6;
														specialDay = new SpecialDay(text3, "", text5);
													}
													else
													{
														if (current.OperationType == -1)
														{
															specialDay = new SpecialDay(text3, "", "");
														}
													}
												}
												if (dictionary.ContainsKey(text3))
												{
													specialDay = dictionary[text3];
													if (current.OperationType == 1)
													{
														specialDay.ONTime = text4;
													}
													else
													{
														if (current.OperationType == 0)
														{
															specialDay.OFFTime = text5;
														}
														else
														{
															if (current.OperationType == -1)
															{
																specialDay.ONTime = "";
																specialDay.OFFTime = "";
															}
														}
													}
													dictionary[text3] = specialDay;
												}
												else
												{
													dictionary.Add(text3, specialDay);
												}
											}
										}
										if (array != null)
										{
											groupControlTask.TaskSchedule = array;
										}
										List<SpecialDay> list4 = new List<SpecialDay>();
										groupControlTask.SpecialDates = null;
										IEnumerator<SpecialDay> enumerator4 = dictionary.Values.GetEnumerator();
										while (enumerator4.MoveNext())
										{
											SpecialDay current2 = enumerator4.Current;
											list4.Add(current2);
										}
										if (list4.Count > 0)
										{
											groupControlTask.SpecialDates = list4;
										}
										else
										{
											groupControlTask.SpecialDates = new List<SpecialDay>();
										}
									}
									DBCache.ht_grouptaskcache.Add(groupControlTask.ID, groupControlTask);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.GroupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_grouptaskcache;
				}
				else
				{
					result = DBCache.ht_grouptaskcache;
				}
			}
			return result;
		}
		public static Hashtable GetGroupTaskCache()
		{
			Hashtable result;
			lock (DBCache._lockgrouptask)
			{
				if (DBCacheStatus.GroupTask)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype < 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_grouptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_grouptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,groupname,groupid,tasktype,status,reserve from groupcontroltask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskcache = new Hashtable();
								foreach (DataRow dr_gct in dataTable.Rows)
								{
									GroupControlTask groupControlTask = new GroupControlTask(dr_gct);
									if (DBCache.ht_grouptaskschedule.ContainsKey(Convert.ToInt32(groupControlTask.GroupID)))
									{
										Dictionary<string, SpecialDay> dictionary = new Dictionary<string, SpecialDay>();
										string[,] array = null;
										int num = 0;
										List<ScheduleInfo> list3 = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[Convert.ToInt32(groupControlTask.GroupID)];
										foreach (ScheduleInfo current in list3)
										{
											if (current.DayOfWeek == 8)
											{
												if (num == 0)
												{
													string[,] array2 = new string[1, 2];
													array2[0, 0] = "";
													array2[0, 1] = "";
													array = array2;
												}
												string text = "";
												try
												{
													DateTime arg_269_0 = current.ScheduleTime;
													text = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[0, 0] = text;
												}
												else
												{
													array[0, 1] = text;
												}
												num++;
											}
											if (current.DayOfWeek < 8)
											{
												if (num == 0)
												{
													string[,] array3 = new string[7, 2];
													array3[0, 0] = "";
													array3[0, 1] = "";
													array3[1, 0] = "";
													array3[1, 1] = "";
													array3[2, 0] = "";
													array3[2, 1] = "";
													array3[3, 0] = "";
													array3[3, 1] = "";
													array3[4, 0] = "";
													array3[4, 1] = "";
													array3[5, 0] = "";
													array3[5, 1] = "";
													array3[6, 0] = "";
													array3[6, 1] = "";
													array = array3;
												}
												string text2 = "";
												try
												{
													DateTime arg_3A1_0 = current.ScheduleTime;
													text2 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[current.DayOfWeek - 1, 0] = text2;
												}
												else
												{
													array[current.DayOfWeek - 1, 1] = text2;
												}
												num++;
											}
											if (current.DayOfWeek > 8)
											{
												string text3 = "";
												string text4 = "";
												string text5 = "";
												text3 = current.Reserve;
												if (text3 == null)
												{
													text3 = "";
												}
												SpecialDay specialDay = null;
												string text6 = "";
												try
												{
													DateTime arg_43D_0 = current.ScheduleTime;
													text6 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													text4 = text6;
													specialDay = new SpecialDay(text3, text4, "");
												}
												else
												{
													if (current.OperationType == 0)
													{
														text5 = text6;
														specialDay = new SpecialDay(text3, "", text5);
													}
													else
													{
														if (current.OperationType == -1)
														{
															specialDay = new SpecialDay(text3, "", "");
														}
													}
												}
												if (dictionary.ContainsKey(text3))
												{
													specialDay = dictionary[text3];
													if (current.OperationType == 1)
													{
														specialDay.ONTime = text4;
													}
													else
													{
														if (current.OperationType == 0)
														{
															specialDay.OFFTime = text5;
														}
														else
														{
															if (current.OperationType == -1)
															{
																specialDay.ONTime = "";
																specialDay.OFFTime = "";
															}
														}
													}
													dictionary[text3] = specialDay;
												}
												else
												{
													dictionary.Add(text3, specialDay);
												}
											}
										}
										if (array != null)
										{
											groupControlTask.TaskSchedule = array;
										}
										List<SpecialDay> list4 = new List<SpecialDay>();
										groupControlTask.SpecialDates = null;
										IEnumerator<SpecialDay> enumerator4 = dictionary.Values.GetEnumerator();
										while (enumerator4.MoveNext())
										{
											SpecialDay current2 = enumerator4.Current;
											list4.Add(current2);
										}
										if (list4.Count > 0)
										{
											groupControlTask.SpecialDates = list4;
										}
										else
										{
											groupControlTask.SpecialDates = new List<SpecialDay>();
										}
									}
									DBCache.ht_grouptaskcache.Add(groupControlTask.ID, groupControlTask);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.GroupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_grouptaskcache;
				}
				else
				{
					result = DBCache.ht_grouptaskcache;
				}
			}
			return result;
		}
		public static Hashtable GetGroupTaskSchedule(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockgrouptask)
			{
				if (DBCacheStatus.GroupTask)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype < 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_grouptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_grouptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,groupname,groupid,tasktype,status,reserve from groupcontroltask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskcache = new Hashtable();
								foreach (DataRow dr_gct in dataTable.Rows)
								{
									GroupControlTask groupControlTask = new GroupControlTask(dr_gct);
									if (DBCache.ht_grouptaskschedule.ContainsKey(Convert.ToInt32(groupControlTask.GroupID)))
									{
										Dictionary<string, SpecialDay> dictionary = new Dictionary<string, SpecialDay>();
										string[,] array = null;
										int num = 0;
										List<ScheduleInfo> list3 = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[Convert.ToInt32(groupControlTask.GroupID)];
										foreach (ScheduleInfo current in list3)
										{
											if (current.DayOfWeek == 8)
											{
												if (num == 0)
												{
													string[,] array2 = new string[1, 2];
													array2[0, 0] = "";
													array2[0, 1] = "";
													array = array2;
												}
												string text = "";
												try
												{
													DateTime arg_259_0 = current.ScheduleTime;
													text = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[0, 0] = text;
												}
												else
												{
													array[0, 1] = text;
												}
												num++;
											}
											if (current.DayOfWeek < 8)
											{
												if (num == 0)
												{
													string[,] array3 = new string[7, 2];
													array3[0, 0] = "";
													array3[0, 1] = "";
													array3[1, 0] = "";
													array3[1, 1] = "";
													array3[2, 0] = "";
													array3[2, 1] = "";
													array3[3, 0] = "";
													array3[3, 1] = "";
													array3[4, 0] = "";
													array3[4, 1] = "";
													array3[5, 0] = "";
													array3[5, 1] = "";
													array3[6, 0] = "";
													array3[6, 1] = "";
													array = array3;
												}
												string text2 = "";
												try
												{
													DateTime arg_391_0 = current.ScheduleTime;
													text2 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[current.DayOfWeek - 1, 0] = text2;
												}
												else
												{
													array[current.DayOfWeek - 1, 1] = text2;
												}
												num++;
											}
											if (current.DayOfWeek > 8)
											{
												string text3 = "";
												string text4 = "";
												string text5 = "";
												text3 = current.Reserve;
												if (text3 == null)
												{
													text3 = "";
												}
												SpecialDay specialDay = null;
												string text6 = "";
												try
												{
													DateTime arg_42D_0 = current.ScheduleTime;
													text6 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													text4 = text6;
													specialDay = new SpecialDay(text3, text4, "");
												}
												else
												{
													if (current.OperationType == 0)
													{
														text5 = text6;
														specialDay = new SpecialDay(text3, "", text5);
													}
													else
													{
														if (current.OperationType == -1)
														{
															specialDay = new SpecialDay(text3, "", "");
														}
													}
												}
												if (dictionary.ContainsKey(text3))
												{
													specialDay = dictionary[text3];
													if (current.OperationType == 1)
													{
														specialDay.ONTime = text4;
													}
													else
													{
														if (current.OperationType == 0)
														{
															specialDay.OFFTime = text5;
														}
														else
														{
															if (current.OperationType == -1)
															{
																specialDay.ONTime = "";
																specialDay.OFFTime = "";
															}
														}
													}
													dictionary[text3] = specialDay;
												}
												else
												{
													dictionary.Add(text3, specialDay);
												}
											}
										}
										if (array != null)
										{
											groupControlTask.TaskSchedule = array;
										}
										groupControlTask.SpecialDates = new List<SpecialDay>();
										IEnumerator<SpecialDay> enumerator4 = dictionary.Values.GetEnumerator();
										while (enumerator4.MoveNext())
										{
											SpecialDay current2 = enumerator4.Current;
											groupControlTask.SpecialDates.Add(current2);
										}
									}
									DBCache.ht_grouptaskcache.Add(groupControlTask.ID, groupControlTask);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.GroupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_grouptaskschedule;
				}
				else
				{
					result = DBCache.ht_grouptaskschedule;
				}
			}
			return result;
		}
		public static Hashtable GetGroupTaskSchedule()
		{
			Hashtable result;
			lock (DBCache._lockgrouptask)
			{
				if (DBCacheStatus.GroupTask)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select groupid,dayofweek,optype,scheduletime,status,reserve from taskschedule where optype < 5 ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskschedule = new Hashtable();
								foreach (DataRow row in dataTable.Rows)
								{
									ScheduleInfo scheduleInfo = new ScheduleInfo(row);
									if (DBCache.ht_grouptaskschedule.ContainsKey(scheduleInfo.ObjectID))
									{
										List<ScheduleInfo> list = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[scheduleInfo.ObjectID];
										list.Add(scheduleInfo);
									}
									else
									{
										List<ScheduleInfo> list2 = new List<ScheduleInfo>();
										list2.Add(scheduleInfo);
										DBCache.ht_grouptaskschedule.Add(scheduleInfo.ObjectID, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,taskname,groupname,groupid,tasktype,status,reserve from groupcontroltask ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_grouptaskcache = new Hashtable();
								foreach (DataRow dr_gct in dataTable.Rows)
								{
									GroupControlTask groupControlTask = new GroupControlTask(dr_gct);
									if (DBCache.ht_grouptaskschedule.ContainsKey(Convert.ToInt32(groupControlTask.GroupID)))
									{
										Dictionary<string, SpecialDay> dictionary = new Dictionary<string, SpecialDay>();
										string[,] array = null;
										int num = 0;
										List<ScheduleInfo> list3 = (List<ScheduleInfo>)DBCache.ht_grouptaskschedule[Convert.ToInt32(groupControlTask.GroupID)];
										foreach (ScheduleInfo current in list3)
										{
											if (current.DayOfWeek == 8)
											{
												if (num == 0)
												{
													string[,] array2 = new string[1, 2];
													array2[0, 0] = "";
													array2[0, 1] = "";
													array = array2;
												}
												string text = "";
												try
												{
													DateTime arg_269_0 = current.ScheduleTime;
													text = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[0, 0] = text;
												}
												else
												{
													array[0, 1] = text;
												}
												num++;
											}
											if (current.DayOfWeek < 8)
											{
												if (num == 0)
												{
													string[,] array3 = new string[7, 2];
													array3[0, 0] = "";
													array3[0, 1] = "";
													array3[1, 0] = "";
													array3[1, 1] = "";
													array3[2, 0] = "";
													array3[2, 1] = "";
													array3[3, 0] = "";
													array3[3, 1] = "";
													array3[4, 0] = "";
													array3[4, 1] = "";
													array3[5, 0] = "";
													array3[5, 1] = "";
													array3[6, 0] = "";
													array3[6, 1] = "";
													array = array3;
												}
												string text2 = "";
												try
												{
													DateTime arg_3A1_0 = current.ScheduleTime;
													text2 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													array[current.DayOfWeek - 1, 0] = text2;
												}
												else
												{
													array[current.DayOfWeek - 1, 1] = text2;
												}
												num++;
											}
											if (current.DayOfWeek > 8)
											{
												string text3 = "";
												string text4 = "";
												string text5 = "";
												text3 = current.Reserve;
												if (text3 == null)
												{
													text3 = "";
												}
												SpecialDay specialDay = null;
												string text6 = "";
												try
												{
													DateTime arg_43D_0 = current.ScheduleTime;
													text6 = current.ScheduleTime.ToString("HH:mm:ss");
												}
												catch (Exception)
												{
												}
												if (current.OperationType == 1)
												{
													text4 = text6;
													specialDay = new SpecialDay(text3, text4, "");
												}
												else
												{
													if (current.OperationType == 0)
													{
														text5 = text6;
														specialDay = new SpecialDay(text3, "", text5);
													}
													else
													{
														if (current.OperationType == -1)
														{
															specialDay = new SpecialDay(text3, "", "");
														}
													}
												}
												if (dictionary.ContainsKey(text3))
												{
													specialDay = dictionary[text3];
													if (current.OperationType == 1)
													{
														specialDay.ONTime = text4;
													}
													else
													{
														if (current.OperationType == 0)
														{
															specialDay.OFFTime = text5;
														}
														else
														{
															if (current.OperationType == -1)
															{
																specialDay.ONTime = "";
																specialDay.OFFTime = "";
															}
														}
													}
													dictionary[text3] = specialDay;
												}
												else
												{
													dictionary.Add(text3, specialDay);
												}
											}
										}
										if (array != null)
										{
											groupControlTask.TaskSchedule = array;
										}
										groupControlTask.SpecialDates = new List<SpecialDay>();
										IEnumerator<SpecialDay> enumerator4 = dictionary.Values.GetEnumerator();
										while (enumerator4.MoveNext())
										{
											SpecialDay current2 = enumerator4.Current;
											groupControlTask.SpecialDates.Add(current2);
										}
									}
									DBCache.ht_grouptaskcache.Add(groupControlTask.ID, groupControlTask);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.GroupTask = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupTackCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_grouptaskschedule;
				}
				else
				{
					result = DBCache.ht_grouptaskschedule;
				}
			}
			return result;
		}
		public static Hashtable GetGroupCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockgroup)
			{
				if (DBCacheStatus.Group)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select group_id,grouptype,dest_id from group_detail ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_groupdestidmap = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									long num = Convert.ToInt64(dataRow[0]);
									long item = Convert.ToInt64(dataRow[2]);
									if (DBCache.ht_groupdestidmap.ContainsKey(num))
									{
										List<long> list = (List<long>)DBCache.ht_groupdestidmap[num];
										list.Add(item);
										DBCache.ht_groupdestidmap[num] = list;
									}
									else
									{
										List<long> list2 = new List<long>();
										list2.Add(item);
										DBCache.ht_groupdestidmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,groupname,grouptype,linecolor,isselect,thermalflag,billflag from data_group ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_groupcache = new Hashtable();
								foreach (DataRow dr_group in dataTable.Rows)
								{
									GroupInfo groupInfo = new GroupInfo(dr_group);
									if (DBCache.ht_groupdestidmap.ContainsKey(groupInfo.ID))
									{
										List<long> list3 = (List<long>)DBCache.ht_groupdestidmap[groupInfo.ID];
										StringBuilder stringBuilder = new StringBuilder();
										foreach (long current in list3)
										{
											if (stringBuilder.Length == 0)
											{
												stringBuilder.Append(current);
											}
											else
											{
												stringBuilder.Append("," + current);
											}
										}
										string members = stringBuilder.ToString();
										groupInfo.Members = members;
									}
									else
									{
										string groupType;
										if ((groupType = groupInfo.GroupType) != null)
										{
											if (!(groupType == "allrack"))
											{
												if (!(groupType == "alldev"))
												{
													if (groupType == "alloutlet")
													{
														Hashtable portCache = DBCache.GetPortCache();
														if (portCache != null && portCache.Count > 0)
														{
															StringBuilder stringBuilder2 = new StringBuilder("");
															ICollection keys = portCache.Keys;
															foreach (int num2 in keys)
															{
																stringBuilder2.Append("," + num2);
															}
															if (stringBuilder2.Length > 1)
															{
																stringBuilder2 = new StringBuilder(stringBuilder2.ToString().Substring(1));
															}
															else
															{
																stringBuilder2 = new StringBuilder("");
															}
															groupInfo.Members = stringBuilder2.ToString();
														}
													}
												}
												else
												{
													Hashtable deviceCache = DBCache.GetDeviceCache();
													if (deviceCache != null && deviceCache.Count > 0)
													{
														StringBuilder stringBuilder3 = new StringBuilder("");
														ICollection keys2 = deviceCache.Keys;
														foreach (int num3 in keys2)
														{
															stringBuilder3.Append("," + num3);
														}
														if (stringBuilder3.Length > 1)
														{
															stringBuilder3 = new StringBuilder(stringBuilder3.ToString().Substring(1));
														}
														else
														{
															stringBuilder3 = new StringBuilder("");
														}
														groupInfo.Members = stringBuilder3.ToString();
													}
												}
											}
											else
											{
												Hashtable rackCache = DBCache.GetRackCache();
												if (rackCache != null && rackCache.Count > 0)
												{
													StringBuilder stringBuilder4 = new StringBuilder("");
													ICollection keys3 = rackCache.Keys;
													foreach (int num4 in keys3)
													{
														stringBuilder4.Append("," + num4);
													}
													if (stringBuilder4.Length > 1)
													{
														stringBuilder4 = new StringBuilder(stringBuilder4.ToString().Substring(1));
													}
													else
													{
														stringBuilder4 = new StringBuilder("");
													}
													groupInfo.Members = stringBuilder4.ToString();
												}
											}
										}
									}
									DBCache.ht_groupcache.Add(groupInfo.ID, groupInfo);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Group = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_groupcache;
				}
				else
				{
					result = DBCache.ht_groupcache;
				}
			}
			return result;
		}
		public static Hashtable GetGroupCache()
		{
			Hashtable result;
			lock (DBCache._lockgroup)
			{
				if (DBCacheStatus.Group || DBCacheStatus.Device || DBCacheStatus.Rack)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select group_id,grouptype,dest_id from group_detail ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_groupdestidmap = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									long num = Convert.ToInt64(dataRow[0]);
									long item = Convert.ToInt64(dataRow[2]);
									if (DBCache.ht_groupdestidmap.ContainsKey(num))
									{
										List<long> list = (List<long>)DBCache.ht_groupdestidmap[num];
										list.Add(item);
										DBCache.ht_groupdestidmap[num] = list;
									}
									else
									{
										List<long> list2 = new List<long>();
										list2.Add(item);
										DBCache.ht_groupdestidmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,groupname,grouptype,linecolor,isselect,thermalflag,billflag from data_group ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_groupcache = new Hashtable();
								foreach (DataRow dr_group in dataTable.Rows)
								{
									GroupInfo groupInfo = new GroupInfo(dr_group);
									if (DBCache.ht_groupdestidmap.ContainsKey(groupInfo.ID))
									{
										List<long> list3 = (List<long>)DBCache.ht_groupdestidmap[groupInfo.ID];
										StringBuilder stringBuilder = new StringBuilder();
										foreach (long current in list3)
										{
											if (stringBuilder.Length == 0)
											{
												stringBuilder.Append(current);
											}
											else
											{
												stringBuilder.Append("," + current);
											}
										}
										string members = stringBuilder.ToString();
										groupInfo.Members = members;
									}
									else
									{
										string groupType;
										if ((groupType = groupInfo.GroupType) != null)
										{
											if (!(groupType == "allrack"))
											{
												if (!(groupType == "alldev"))
												{
													if (groupType == "alloutlet")
													{
														Hashtable portCache = DBCache.GetPortCache();
														if (portCache != null && portCache.Count > 0)
														{
															StringBuilder stringBuilder2 = new StringBuilder("");
															ICollection keys = portCache.Keys;
															foreach (int num2 in keys)
															{
																stringBuilder2.Append("," + num2);
															}
															if (stringBuilder2.Length > 1)
															{
																stringBuilder2 = new StringBuilder(stringBuilder2.ToString().Substring(1));
															}
															else
															{
																stringBuilder2 = new StringBuilder("");
															}
															groupInfo.Members = stringBuilder2.ToString();
														}
													}
												}
												else
												{
													Hashtable deviceCache = DBCache.GetDeviceCache();
													if (deviceCache != null && deviceCache.Count > 0)
													{
														StringBuilder stringBuilder3 = new StringBuilder("");
														ICollection keys2 = deviceCache.Keys;
														foreach (int num3 in keys2)
														{
															stringBuilder3.Append("," + num3);
														}
														if (stringBuilder3.Length > 1)
														{
															stringBuilder3 = new StringBuilder(stringBuilder3.ToString().Substring(1));
														}
														else
														{
															stringBuilder3 = new StringBuilder("");
														}
														groupInfo.Members = stringBuilder3.ToString();
													}
												}
											}
											else
											{
												Hashtable rackCache = DBCache.GetRackCache();
												if (rackCache != null && rackCache.Count > 0)
												{
													StringBuilder stringBuilder4 = new StringBuilder("");
													ICollection keys3 = rackCache.Keys;
													foreach (int num4 in keys3)
													{
														stringBuilder4.Append("," + num4);
													}
													if (stringBuilder4.Length > 1)
													{
														stringBuilder4 = new StringBuilder(stringBuilder4.ToString().Substring(1));
													}
													else
													{
														stringBuilder4 = new StringBuilder("");
													}
													groupInfo.Members = stringBuilder4.ToString();
												}
											}
										}
									}
									DBCache.ht_groupcache.Add(groupInfo.ID, groupInfo);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Group = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_groupcache;
				}
				else
				{
					result = DBCache.ht_groupcache;
				}
			}
			return result;
		}
		public static Hashtable GetGroupDestIDMap(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockgroup)
			{
				if (DBCacheStatus.Group || DBCacheStatus.Device || DBCacheStatus.Rack)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select group_id,grouptype,dest_id from group_detail ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_groupdestidmap = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									long num = Convert.ToInt64(dataRow[0]);
									long item = Convert.ToInt64(dataRow[2]);
									if (DBCache.ht_groupdestidmap.ContainsKey(num))
									{
										List<long> list = (List<long>)DBCache.ht_groupdestidmap[num];
										list.Add(item);
										DBCache.ht_groupdestidmap[num] = list;
									}
									else
									{
										List<long> list2 = new List<long>();
										list2.Add(item);
										DBCache.ht_groupdestidmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,groupname,grouptype,linecolor,isselect,thermalflag,billflag from data_group ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_groupcache = new Hashtable();
								foreach (DataRow dr_group in dataTable.Rows)
								{
									GroupInfo groupInfo = new GroupInfo(dr_group);
									if (DBCache.ht_groupdestidmap.ContainsKey(groupInfo.ID))
									{
										List<long> list3 = (List<long>)DBCache.ht_groupdestidmap[groupInfo.ID];
										StringBuilder stringBuilder = new StringBuilder();
										foreach (long current in list3)
										{
											if (stringBuilder.Length == 0)
											{
												stringBuilder.Append(current);
											}
											else
											{
												stringBuilder.Append("," + current);
											}
										}
										string members = stringBuilder.ToString();
										groupInfo.Members = members;
									}
									else
									{
										string groupType;
										if ((groupType = groupInfo.GroupType) != null)
										{
											if (!(groupType == "allrack"))
											{
												if (!(groupType == "alldev"))
												{
													if (groupType == "alloutlet")
													{
														Hashtable portCache = DBCache.GetPortCache();
														if (portCache != null && portCache.Count > 0)
														{
															StringBuilder stringBuilder2 = new StringBuilder("");
															ICollection keys = portCache.Keys;
															foreach (int num2 in keys)
															{
																stringBuilder2.Append("," + num2);
															}
															if (stringBuilder2.Length > 1)
															{
																stringBuilder2 = new StringBuilder(stringBuilder2.ToString().Substring(1));
															}
															else
															{
																stringBuilder2 = new StringBuilder("");
															}
															groupInfo.Members = stringBuilder2.ToString();
														}
													}
												}
												else
												{
													Hashtable deviceCache = DBCache.GetDeviceCache();
													if (deviceCache != null && deviceCache.Count > 0)
													{
														StringBuilder stringBuilder3 = new StringBuilder("");
														ICollection keys2 = deviceCache.Keys;
														foreach (int num3 in keys2)
														{
															stringBuilder3.Append("," + num3);
														}
														if (stringBuilder3.Length > 1)
														{
															stringBuilder3 = new StringBuilder(stringBuilder3.ToString().Substring(1));
														}
														else
														{
															stringBuilder3 = new StringBuilder("");
														}
														groupInfo.Members = stringBuilder3.ToString();
													}
												}
											}
											else
											{
												Hashtable rackCache = DBCache.GetRackCache();
												if (rackCache != null && rackCache.Count > 0)
												{
													StringBuilder stringBuilder4 = new StringBuilder("");
													ICollection keys3 = rackCache.Keys;
													foreach (int num4 in keys3)
													{
														stringBuilder4.Append("," + num4);
													}
													if (stringBuilder4.Length > 1)
													{
														stringBuilder4 = new StringBuilder(stringBuilder4.ToString().Substring(1));
													}
													else
													{
														stringBuilder4 = new StringBuilder("");
													}
													groupInfo.Members = stringBuilder4.ToString();
												}
											}
										}
									}
									DBCache.ht_groupcache.Add(groupInfo.ID, groupInfo);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Group = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_groupdestidmap;
				}
				else
				{
					result = DBCache.ht_groupdestidmap;
				}
			}
			return result;
		}
		public static Hashtable GetGroupDestIDMap()
		{
			Hashtable result;
			lock (DBCache._lockgroup)
			{
				if (DBCacheStatus.Group || DBCacheStatus.Device || DBCacheStatus.Rack)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select group_id,grouptype,dest_id from group_detail ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_groupdestidmap = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									long num = Convert.ToInt64(dataRow[0]);
									long item = Convert.ToInt64(dataRow[2]);
									if (DBCache.ht_groupdestidmap.ContainsKey(num))
									{
										List<long> list = (List<long>)DBCache.ht_groupdestidmap[num];
										list.Add(item);
										DBCache.ht_groupdestidmap[num] = list;
									}
									else
									{
										List<long> list2 = new List<long>();
										list2.Add(item);
										DBCache.ht_groupdestidmap.Add(num, list2);
									}
								}
							}
							dataTable = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,groupname,grouptype,linecolor,isselect,thermalflag,billflag from data_group ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_groupcache = new Hashtable();
								foreach (DataRow dr_group in dataTable.Rows)
								{
									GroupInfo groupInfo = new GroupInfo(dr_group);
									if (DBCache.ht_groupdestidmap.ContainsKey(groupInfo.ID))
									{
										List<long> list3 = (List<long>)DBCache.ht_groupdestidmap[groupInfo.ID];
										StringBuilder stringBuilder = new StringBuilder();
										foreach (long current in list3)
										{
											if (stringBuilder.Length == 0)
											{
												stringBuilder.Append(current);
											}
											else
											{
												stringBuilder.Append("," + current);
											}
										}
										string members = stringBuilder.ToString();
										groupInfo.Members = members;
									}
									DBCache.ht_groupcache.Add(groupInfo.ID, groupInfo);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Group = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshGroupCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_groupdestidmap;
				}
				else
				{
					result = DBCache.ht_groupdestidmap;
				}
			}
			return result;
		}
		public static Hashtable GetEventLogCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockevent)
			{
				if (DBCacheStatus.Event)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select eventid,logflag,mailflag from event_info order by eventid ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_eventlogcache = new Hashtable();
								DBCache.ht_eventmailcache = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									string key = Convert.ToString(dataRow[0]);
									int num = Convert.ToInt32(dataRow[1]);
									int num2 = Convert.ToInt32(dataRow[2]);
									if (DBCache.ht_eventlogcache.ContainsKey(key))
									{
										DBCache.ht_eventlogcache[key] = num;
									}
									else
									{
										DBCache.ht_eventlogcache.Add(key, num);
									}
									if (DBCache.ht_eventmailcache.ContainsKey(key))
									{
										DBCache.ht_eventmailcache[key] = num2;
									}
									else
									{
										DBCache.ht_eventmailcache.Add(key, num2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Event = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshEventCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_eventlogcache;
				}
				else
				{
					result = DBCache.ht_eventlogcache;
				}
			}
			return result;
		}
		public static Hashtable GetEventLogCache()
		{
			Hashtable result;
			lock (DBCache._lockevent)
			{
				if (DBCacheStatus.Event)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select eventid,logflag,mailflag from event_info  order by eventid ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_eventlogcache = new Hashtable();
								DBCache.ht_eventmailcache = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									string key = Convert.ToString(dataRow[0]);
									int num = Convert.ToInt32(dataRow[1]);
									int num2 = Convert.ToInt32(dataRow[2]);
									if (DBCache.ht_eventlogcache.ContainsKey(key))
									{
										DBCache.ht_eventlogcache[key] = num;
									}
									else
									{
										DBCache.ht_eventlogcache.Add(key, num);
									}
									if (DBCache.ht_eventmailcache.ContainsKey(key))
									{
										DBCache.ht_eventmailcache[key] = num2;
									}
									else
									{
										DBCache.ht_eventmailcache.Add(key, num2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Event = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshEventCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_eventlogcache;
				}
				else
				{
					result = DBCache.ht_eventlogcache;
				}
			}
			return result;
		}
		public static Hashtable GetEventMailCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockevent)
			{
				if (DBCacheStatus.Event)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select eventid,logflag,mailflag from event_info  order by eventid ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_eventlogcache = new Hashtable();
								DBCache.ht_eventmailcache = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									string key = Convert.ToString(dataRow[0]);
									int num = Convert.ToInt32(dataRow[1]);
									int num2 = Convert.ToInt32(dataRow[2]);
									if (DBCache.ht_eventlogcache.ContainsKey(key))
									{
										DBCache.ht_eventlogcache[key] = num;
									}
									else
									{
										DBCache.ht_eventlogcache.Add(key, num);
									}
									if (DBCache.ht_eventmailcache.ContainsKey(key))
									{
										DBCache.ht_eventmailcache[key] = num2;
									}
									else
									{
										DBCache.ht_eventmailcache.Add(key, num2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Event = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshEventCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_eventmailcache;
				}
				else
				{
					result = DBCache.ht_eventmailcache;
				}
			}
			return result;
		}
		public static Hashtable GetEventMailCache()
		{
			Hashtable result;
			lock (DBCache._lockevent)
			{
				if (DBCacheStatus.Event)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select eventid,logflag,mailflag from event_info  order by eventid ASC";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_eventlogcache = new Hashtable();
								DBCache.ht_eventmailcache = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									string key = Convert.ToString(dataRow[0]);
									int num = Convert.ToInt32(dataRow[1]);
									int num2 = Convert.ToInt32(dataRow[2]);
									if (DBCache.ht_eventlogcache.ContainsKey(key))
									{
										DBCache.ht_eventlogcache[key] = num;
									}
									else
									{
										DBCache.ht_eventlogcache.Add(key, num);
									}
									if (DBCache.ht_eventmailcache.ContainsKey(key))
									{
										DBCache.ht_eventmailcache[key] = num2;
									}
									else
									{
										DBCache.ht_eventmailcache.Add(key, num2);
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Event = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshEventCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_eventmailcache;
				}
				else
				{
					result = DBCache.ht_eventmailcache;
				}
			}
			return result;
		}
		public static SMTPSetting GetSMTPSetting(DBConn conn)
		{
			SMTPSetting result;
			lock (DBCache._locksmtp)
			{
				if (DBCacheStatus.Smtp)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select EnableSMTP,ServerAddress,PortId,EmailFrom,EmailTo,SendEvent,EnableAuth,Account,UserPwd from smtpsetting ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								foreach (DataRow dataRow in dataTable.Rows)
								{
									int i_enable = Convert.ToInt32(dataRow[0]);
									string str_ip = Convert.ToString(dataRow[1]);
									int i_port = Convert.ToInt32(dataRow[2]);
									string str_from = Convert.ToString(dataRow[3]);
									string str_to = Convert.ToString(dataRow[4]);
									string str_event = Convert.ToString(dataRow[5]);
									int i_authflag = Convert.ToInt32(dataRow[6]);
									string str_account = Convert.ToString(dataRow[7]);
									string str_pwd = Convert.ToString(dataRow[8]);
									DBCache.smtp = new SMTPSetting(i_enable, str_ip, i_port, str_from, str_to, str_event, i_authflag, str_account, str_pwd);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Smtp = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshSMTPCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.smtp;
				}
				else
				{
					result = DBCache.smtp;
				}
			}
			return result;
		}
		public static SMTPSetting GetSMTPSetting()
		{
			SMTPSetting result;
			lock (DBCache._locksmtp)
			{
				if (DBCacheStatus.Smtp)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select EnableSMTP,ServerAddress,PortId,EmailFrom,EmailTo,SendEvent,EnableAuth,Account,UserPwd from smtpsetting ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow in dataTable.Rows)
								{
									int i_enable = Convert.ToInt32(dataRow[0]);
									string str_ip = Convert.ToString(dataRow[1]);
									int i_port = Convert.ToInt32(dataRow[2]);
									string str_from = Convert.ToString(dataRow[3]);
									string str_to = Convert.ToString(dataRow[4]);
									string str_event = Convert.ToString(dataRow[5]);
									int i_authflag = Convert.ToInt32(dataRow[6]);
									string str_account = Convert.ToString(dataRow[7]);
									string str_pwd = Convert.ToString(dataRow[8]);
									DBCache.smtp = new SMTPSetting(i_enable, str_ip, i_port, str_from, str_to, str_event, i_authflag, str_account, str_pwd);
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.Smtp = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshSMTPCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.smtp;
				}
				else
				{
					result = DBCache.smtp;
				}
			}
			return result;
		}
		public static Hashtable GetSysParameterCache(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._locksys)
			{
				if (DBCacheStatus.SystemParameter)
				{
					if (DBUrl.SERVERMODE)
					{
						DataTable dataTable = new DataTable();
						DbCommand dbCommand = null;
						DbDataAdapter dbDataAdapter = null;
						try
						{
							if (conn != null && conn.con != null)
							{
								dbDataAdapter = DBConn.GetDataAdapter(conn.con);
								dbCommand = conn.con.CreateCommand();
								dbCommand.CommandText = "select para_name,para_type,para_value from sys_para ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									DBCache.ht_keyvaluecache = new Hashtable();
									foreach (DataRow dataRow in dataTable.Rows)
									{
										string key = Convert.ToString(dataRow[0]);
										string value = "";
										if (!dataRow.IsNull(2))
										{
											value = Convert.ToString(dataRow[2]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey(key))
										{
											DBCache.ht_keyvaluecache[key] = value;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add(key, value);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(conn.con);
								dbCommand = conn.con.CreateCommand();
								dbCommand.CommandText = "select * from cleanupsetting ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									foreach (DataRow dataRow2 in dataTable.Rows)
									{
										string value2 = "";
										string value3 = "";
										string value4 = "";
										if (!dataRow2.IsNull(0))
										{
											value2 = Convert.ToString(dataRow2[0]);
										}
										if (!dataRow2.IsNull(1))
										{
											value3 = Convert.ToString(dataRow2[1]);
										}
										if (!dataRow2.IsNull(2))
										{
											value4 = Convert.ToString(dataRow2[2]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("DBOPT_keepMMFLAG"))
										{
											DBCache.ht_keyvaluecache["DBOPT_keepMMFLAG"] = value2;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("DBOPT_keepMMFLAG", value2);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("DBOPT_keepMM"))
										{
											DBCache.ht_keyvaluecache["DBOPT_keepMM"] = value4;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("DBOPT_keepMM", value4);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("DBOPT_delOldFLAG"))
										{
											DBCache.ht_keyvaluecache["DBOPT_delOldFLAG"] = value3;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("DBOPT_delOldFLAG", value3);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(conn.con);
								dbCommand = conn.con.CreateCommand();
								dbCommand.CommandText = "select snmpusername,snmpport,snmpver,snmppprl,snmpppwd,snmpaprl,snmpapwd,snmptimeout,snmpretry,trapusername,trapport,trapver,trappprl,trapppwd,trapaprl,trapapwd from snmpsetting ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									foreach (DataRow dataRow3 in dataTable.Rows)
									{
										string value5 = "";
										string value6 = "";
										string value7 = "";
										string value8 = "";
										string value9 = "";
										string value10 = "";
										string value11 = "";
										string value12 = "";
										string value13 = "";
										string value14 = "";
										string value15 = "";
										string value16 = "";
										string value17 = "";
										string value18 = "";
										string value19 = "";
										string value20 = "";
										if (!dataRow3.IsNull(0))
										{
											value5 = Convert.ToString(dataRow3[0]);
										}
										if (!dataRow3.IsNull(1))
										{
											value6 = Convert.ToString(dataRow3[1]);
										}
										if (!dataRow3.IsNull(2))
										{
											value7 = Convert.ToString(dataRow3[2]);
										}
										if (!dataRow3.IsNull(3))
										{
											value8 = Convert.ToString(dataRow3[3]);
										}
										if (!dataRow3.IsNull(4))
										{
											value9 = Convert.ToString(dataRow3[4]);
										}
										if (!dataRow3.IsNull(5))
										{
											value10 = Convert.ToString(dataRow3[5]);
										}
										if (!dataRow3.IsNull(6))
										{
											value11 = Convert.ToString(dataRow3[6]);
										}
										if (!dataRow3.IsNull(7))
										{
											value12 = Convert.ToString(dataRow3[7]);
										}
										if (!dataRow3.IsNull(8))
										{
											value13 = Convert.ToString(dataRow3[8]);
										}
										if (!dataRow3.IsNull(9))
										{
											value14 = Convert.ToString(dataRow3[9]);
										}
										if (!dataRow3.IsNull(10))
										{
											value15 = Convert.ToString(dataRow3[10]);
										}
										if (!dataRow3.IsNull(11))
										{
											value16 = Convert.ToString(dataRow3[11]);
										}
										if (!dataRow3.IsNull(12))
										{
											value17 = Convert.ToString(dataRow3[12]);
										}
										if (!dataRow3.IsNull(13))
										{
											value18 = Convert.ToString(dataRow3[13]);
										}
										if (!dataRow3.IsNull(14))
										{
											value19 = Convert.ToString(dataRow3[14]);
										}
										if (!dataRow3.IsNull(15))
										{
											value20 = Convert.ToString(dataRow3[15]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpusername"))
										{
											DBCache.ht_keyvaluecache["snmpusername"] = value5;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpusername", value5);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpport"))
										{
											DBCache.ht_keyvaluecache["snmpport"] = value6;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpport", value6);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpver"))
										{
											DBCache.ht_keyvaluecache["snmpver"] = value7;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpver", value7);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmppprl"))
										{
											DBCache.ht_keyvaluecache["snmppprl"] = value8;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmppprl", value8);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpppwd"))
										{
											DBCache.ht_keyvaluecache["snmpppwd"] = value9;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpppwd", value9);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpaprl"))
										{
											DBCache.ht_keyvaluecache["snmpaprl"] = value10;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpaprl", value10);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpapwd"))
										{
											DBCache.ht_keyvaluecache["snmpapwd"] = value11;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpapwd", value11);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmptimeout"))
										{
											DBCache.ht_keyvaluecache["snmptimeout"] = value12;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmptimeout", value12);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpretry"))
										{
											DBCache.ht_keyvaluecache["snmpretry"] = value13;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpretry", value13);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapusername"))
										{
											DBCache.ht_keyvaluecache["trapusername"] = value14;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapusername", value14);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapport"))
										{
											DBCache.ht_keyvaluecache["trapport"] = value15;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapport", value15);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapver"))
										{
											DBCache.ht_keyvaluecache["trapver"] = value16;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapver", value16);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trappprl"))
										{
											DBCache.ht_keyvaluecache["trappprl"] = value17;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trappprl", value17);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapppwd"))
										{
											DBCache.ht_keyvaluecache["trapppwd"] = value18;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapppwd", value18);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapaprl"))
										{
											DBCache.ht_keyvaluecache["trapaprl"] = value19;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapaprl", value19);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapapwd"))
										{
											DBCache.ht_keyvaluecache["trapapwd"] = value20;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapapwd", value20);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(conn.con);
								dbCommand = conn.con.CreateCommand();
								dbCommand.CommandText = "select ServiceDelay,Layout,EnergyType,EnergyValue,ReferenceDevice,CO2KG,CO2COST,Electricitycost,TemperatureUnit,Currency,RackfullnameFlag from systemparameter ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								dbCommand.Dispose();
								if (dataTable != null)
								{
									foreach (DataRow dataRow4 in dataTable.Rows)
									{
										string value21 = "";
										string value22 = "";
										string value23 = "";
										string value24 = "";
										string value25 = "";
										string value26 = "";
										string value27 = "";
										string value28 = "";
										string value29 = "";
										string value30 = "";
										string value31 = "";
										if (!dataRow4.IsNull(0))
										{
											value21 = Convert.ToString(dataRow4[0]);
										}
										if (!dataRow4.IsNull(1))
										{
											value22 = Convert.ToString(dataRow4[1]);
										}
										if (!dataRow4.IsNull(2))
										{
											value23 = Convert.ToString(dataRow4[2]);
										}
										if (!dataRow4.IsNull(3))
										{
											value24 = Convert.ToString(dataRow4[3]);
										}
										if (!dataRow4.IsNull(4))
										{
											value25 = Convert.ToString(dataRow4[4]);
										}
										if (!dataRow4.IsNull(5))
										{
											value26 = Convert.ToString(dataRow4[5]);
										}
										if (!dataRow4.IsNull(6))
										{
											value27 = Convert.ToString(dataRow4[6]);
										}
										if (!dataRow4.IsNull(7))
										{
											value28 = Convert.ToString(dataRow4[7]);
										}
										if (!dataRow4.IsNull(8))
										{
											value29 = Convert.ToString(dataRow4[8]);
										}
										if (!dataRow4.IsNull(9))
										{
											value30 = Convert.ToString(dataRow4[9]);
										}
										if (!dataRow4.IsNull(10))
										{
											value31 = Convert.ToString(dataRow4[10]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("SERVICEDELAY"))
										{
											DBCache.ht_keyvaluecache["SERVICEDELAY"] = value21;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("SERVICEDELAY", value21);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("RESOLUTION"))
										{
											DBCache.ht_keyvaluecache["RESOLUTION"] = value22;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("RESOLUTION", value22);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ENERGYTYPE"))
										{
											DBCache.ht_keyvaluecache["ENERGYTYPE"] = value23;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ENERGYTYPE", value23);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ENERGYVALUE"))
										{
											DBCache.ht_keyvaluecache["ENERGYVALUE"] = value24;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ENERGYVALUE", value24);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ENERGYDEVICE"))
										{
											DBCache.ht_keyvaluecache["ENERGYDEVICE"] = value25;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ENERGYDEVICE", value25);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("CO2KG"))
										{
											DBCache.ht_keyvaluecache["CO2KG"] = value26;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("CO2KG", value26);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("CO2COST"))
										{
											DBCache.ht_keyvaluecache["CO2COST"] = value27;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("CO2COST", value27);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ELECTRICITY"))
										{
											DBCache.ht_keyvaluecache["ELECTRICITY"] = value28;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ELECTRICITY", value28);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("TEMPERATUREUNIT"))
										{
											DBCache.ht_keyvaluecache["TEMPERATUREUNIT"] = value29;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("TEMPERATUREUNIT", value29);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("CURRENCY"))
										{
											DBCache.ht_keyvaluecache["CURRENCY"] = value30;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("CURRENCY", value30);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("RACKFULLNAMEFLAG"))
										{
											DBCache.ht_keyvaluecache["RACKFULLNAMEFLAG"] = value31;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("RACKFULLNAMEFLAG", value31);
										}
									}
								}
								dataTable = new DataTable();
								DBCacheStatus.SystemParameter = false;
							}
							goto IL_E76;
						}
						catch (Exception ex)
						{
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshSysParaCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
							goto IL_E76;
						}
					}
					DataTable dataTable2 = new DataTable();
					DbCommand dbCommand2 = null;
					DbDataAdapter dbDataAdapter2 = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter2 = DBConn.GetDataAdapter(conn.con);
							dbCommand2 = conn.con.CreateCommand();
							dbCommand2.CommandText = "select para_name,para_type,para_value from sys_para ";
							dbDataAdapter2.SelectCommand = dbCommand2;
							dbDataAdapter2.Fill(dataTable2);
							dbDataAdapter2.Dispose();
							dbCommand2.Dispose();
							if (dataTable2 != null)
							{
								DBCache.ht_keyvaluecache = new Hashtable();
								foreach (DataRow dataRow5 in dataTable2.Rows)
								{
									string key2 = Convert.ToString(dataRow5[0]);
									string value32 = "";
									if (!dataRow5.IsNull(2))
									{
										value32 = Convert.ToString(dataRow5[2]);
									}
									if (DBCache.ht_keyvaluecache.ContainsKey(key2))
									{
										DBCache.ht_keyvaluecache[key2] = value32;
									}
									else
									{
										DBCache.ht_keyvaluecache.Add(key2, value32);
									}
								}
							}
							dataTable2 = new DataTable();
							DBCacheStatus.SystemParameter = false;
						}
					}
					catch (Exception ex2)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshSysParaCache Error : " + ex2.Message + "\n" + ex2.StackTrace);
						try
						{
							dbDataAdapter2.Dispose();
						}
						catch
						{
						}
						try
						{
							dbCommand2.Dispose();
						}
						catch
						{
						}
					}
					IL_E76:
					result = DBCache.ht_keyvaluecache;
				}
				else
				{
					result = DBCache.ht_keyvaluecache;
				}
			}
			return result;
		}
		public static Hashtable GetSysParameterCache()
		{
			Hashtable result;
			lock (DBCache._locksys)
			{
				if (DBCacheStatus.SystemParameter)
				{
					if (DBUrl.SERVERMODE)
					{
						DataTable dataTable = new DataTable();
						DBConn dBConn = null;
						DbCommand dbCommand = null;
						DbDataAdapter dbDataAdapter = null;
						try
						{
							dBConn = DBConnPool.getConnection();
							if (dBConn != null && dBConn.con != null)
							{
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand = dBConn.con.CreateCommand();
								dbCommand.CommandText = "select para_name,para_type,para_value from sys_para ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									DBCache.ht_keyvaluecache = new Hashtable();
									foreach (DataRow dataRow in dataTable.Rows)
									{
										string key = Convert.ToString(dataRow[0]);
										string value = "";
										if (!dataRow.IsNull(2))
										{
											value = Convert.ToString(dataRow[2]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey(key))
										{
											DBCache.ht_keyvaluecache[key] = value;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add(key, value);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand = dBConn.con.CreateCommand();
								dbCommand.CommandText = "select * from cleanupsetting ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									foreach (DataRow dataRow2 in dataTable.Rows)
									{
										string value2 = "";
										string value3 = "";
										string value4 = "";
										if (!dataRow2.IsNull(0))
										{
											value2 = Convert.ToString(dataRow2[0]);
										}
										if (!dataRow2.IsNull(1))
										{
											value3 = Convert.ToString(dataRow2[1]);
										}
										if (!dataRow2.IsNull(2))
										{
											value4 = Convert.ToString(dataRow2[2]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("DBOPT_keepMMFLAG"))
										{
											DBCache.ht_keyvaluecache["DBOPT_keepMMFLAG"] = value2;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("DBOPT_keepMMFLAG", value2);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("DBOPT_keepMM"))
										{
											DBCache.ht_keyvaluecache["DBOPT_keepMM"] = value4;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("DBOPT_keepMM", value4);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("DBOPT_delOldFLAG"))
										{
											DBCache.ht_keyvaluecache["DBOPT_delOldFLAG"] = value3;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("DBOPT_delOldFLAG", value3);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand = dBConn.con.CreateCommand();
								dbCommand.CommandText = "select snmpusername,snmpport,snmpver,snmppprl,snmpppwd,snmpaprl,snmpapwd,snmptimeout,snmpretry,trapusername,trapport,trapver,trappprl,trapppwd,trapaprl,trapapwd from snmpsetting ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									foreach (DataRow dataRow3 in dataTable.Rows)
									{
										string value5 = "";
										string value6 = "";
										string value7 = "";
										string value8 = "";
										string value9 = "";
										string value10 = "";
										string value11 = "";
										string value12 = "";
										string value13 = "";
										string value14 = "";
										string value15 = "";
										string value16 = "";
										string value17 = "";
										string value18 = "";
										string value19 = "";
										string value20 = "";
										if (!dataRow3.IsNull(0))
										{
											value5 = Convert.ToString(dataRow3[0]);
										}
										if (!dataRow3.IsNull(1))
										{
											value6 = Convert.ToString(dataRow3[1]);
										}
										if (!dataRow3.IsNull(2))
										{
											value7 = Convert.ToString(dataRow3[2]);
										}
										if (!dataRow3.IsNull(3))
										{
											value8 = Convert.ToString(dataRow3[3]);
										}
										if (!dataRow3.IsNull(4))
										{
											value9 = Convert.ToString(dataRow3[4]);
										}
										if (!dataRow3.IsNull(5))
										{
											value10 = Convert.ToString(dataRow3[5]);
										}
										if (!dataRow3.IsNull(6))
										{
											value11 = Convert.ToString(dataRow3[6]);
										}
										if (!dataRow3.IsNull(7))
										{
											value12 = Convert.ToString(dataRow3[7]);
										}
										if (!dataRow3.IsNull(8))
										{
											value13 = Convert.ToString(dataRow3[8]);
										}
										if (!dataRow3.IsNull(9))
										{
											value14 = Convert.ToString(dataRow3[9]);
										}
										if (!dataRow3.IsNull(10))
										{
											value15 = Convert.ToString(dataRow3[10]);
										}
										if (!dataRow3.IsNull(11))
										{
											value16 = Convert.ToString(dataRow3[11]);
										}
										if (!dataRow3.IsNull(12))
										{
											value17 = Convert.ToString(dataRow3[12]);
										}
										if (!dataRow3.IsNull(13))
										{
											value18 = Convert.ToString(dataRow3[13]);
										}
										if (!dataRow3.IsNull(14))
										{
											value19 = Convert.ToString(dataRow3[14]);
										}
										if (!dataRow3.IsNull(15))
										{
											value20 = Convert.ToString(dataRow3[15]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpusername"))
										{
											DBCache.ht_keyvaluecache["snmpusername"] = value5;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpusername", value5);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpport"))
										{
											DBCache.ht_keyvaluecache["snmpport"] = value6;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpport", value6);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpver"))
										{
											DBCache.ht_keyvaluecache["snmpver"] = value7;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpver", value7);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmppprl"))
										{
											DBCache.ht_keyvaluecache["snmppprl"] = value8;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmppprl", value8);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpppwd"))
										{
											DBCache.ht_keyvaluecache["snmpppwd"] = value9;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpppwd", value9);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpaprl"))
										{
											DBCache.ht_keyvaluecache["snmpaprl"] = value10;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpaprl", value10);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpapwd"))
										{
											DBCache.ht_keyvaluecache["snmpapwd"] = value11;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpapwd", value11);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmptimeout"))
										{
											DBCache.ht_keyvaluecache["snmptimeout"] = value12;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmptimeout", value12);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("snmpretry"))
										{
											DBCache.ht_keyvaluecache["snmpretry"] = value13;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("snmpretry", value13);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapusername"))
										{
											DBCache.ht_keyvaluecache["trapusername"] = value14;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapusername", value14);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapport"))
										{
											DBCache.ht_keyvaluecache["trapport"] = value15;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapport", value15);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapver"))
										{
											DBCache.ht_keyvaluecache["trapver"] = value16;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapver", value16);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trappprl"))
										{
											DBCache.ht_keyvaluecache["trappprl"] = value17;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trappprl", value17);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapppwd"))
										{
											DBCache.ht_keyvaluecache["trapppwd"] = value18;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapppwd", value18);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapaprl"))
										{
											DBCache.ht_keyvaluecache["trapaprl"] = value19;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapaprl", value19);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("trapapwd"))
										{
											DBCache.ht_keyvaluecache["trapapwd"] = value20;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("trapapwd", value20);
										}
									}
								}
								dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbCommand = dBConn.con.CreateCommand();
								dbCommand.CommandText = "select ServiceDelay,Layout,EnergyType,EnergyValue,ReferenceDevice,CO2KG,CO2COST,Electricitycost,TemperatureUnit,Currency,RackfullnameFlag from systemparameter ";
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								dbCommand.Dispose();
								dBConn.Close();
								if (dataTable != null)
								{
									foreach (DataRow dataRow4 in dataTable.Rows)
									{
										string value21 = "";
										string value22 = "";
										string value23 = "";
										string value24 = "";
										string value25 = "";
										string value26 = "";
										string value27 = "";
										string value28 = "";
										string value29 = "";
										string value30 = "";
										string value31 = "";
										if (!dataRow4.IsNull(0))
										{
											value21 = Convert.ToString(dataRow4[0]);
										}
										if (!dataRow4.IsNull(1))
										{
											value22 = Convert.ToString(dataRow4[1]);
										}
										if (!dataRow4.IsNull(2))
										{
											value23 = Convert.ToString(dataRow4[2]);
										}
										if (!dataRow4.IsNull(3))
										{
											value24 = Convert.ToString(dataRow4[3]);
										}
										if (!dataRow4.IsNull(4))
										{
											value25 = Convert.ToString(dataRow4[4]);
										}
										if (!dataRow4.IsNull(5))
										{
											value26 = Convert.ToString(dataRow4[5]);
										}
										if (!dataRow4.IsNull(6))
										{
											value27 = Convert.ToString(dataRow4[6]);
										}
										if (!dataRow4.IsNull(7))
										{
											value28 = Convert.ToString(dataRow4[7]);
										}
										if (!dataRow4.IsNull(8))
										{
											value29 = Convert.ToString(dataRow4[8]);
										}
										if (!dataRow4.IsNull(9))
										{
											value30 = Convert.ToString(dataRow4[9]);
										}
										if (!dataRow4.IsNull(10))
										{
											value31 = Convert.ToString(dataRow4[10]);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("SERVICEDELAY"))
										{
											DBCache.ht_keyvaluecache["SERVICEDELAY"] = value21;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("SERVICEDELAY", value21);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("RESOLUTION"))
										{
											DBCache.ht_keyvaluecache["RESOLUTION"] = value22;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("RESOLUTION", value22);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ENERGYTYPE"))
										{
											DBCache.ht_keyvaluecache["ENERGYTYPE"] = value23;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ENERGYTYPE", value23);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ENERGYVALUE"))
										{
											DBCache.ht_keyvaluecache["ENERGYVALUE"] = value24;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ENERGYVALUE", value24);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ENERGYDEVICE"))
										{
											DBCache.ht_keyvaluecache["ENERGYDEVICE"] = value25;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ENERGYDEVICE", value25);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("CO2KG"))
										{
											DBCache.ht_keyvaluecache["CO2KG"] = value26;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("CO2KG", value26);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("CO2COST"))
										{
											DBCache.ht_keyvaluecache["CO2COST"] = value27;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("CO2COST", value27);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("ELECTRICITY"))
										{
											DBCache.ht_keyvaluecache["ELECTRICITY"] = value28;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("ELECTRICITY", value28);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("TEMPERATUREUNIT"))
										{
											DBCache.ht_keyvaluecache["TEMPERATUREUNIT"] = value29;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("TEMPERATUREUNIT", value29);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("CURRENCY"))
										{
											DBCache.ht_keyvaluecache["CURRENCY"] = value30;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("CURRENCY", value30);
										}
										if (DBCache.ht_keyvaluecache.ContainsKey("RACKFULLNAMEFLAG"))
										{
											DBCache.ht_keyvaluecache["RACKFULLNAMEFLAG"] = value31;
										}
										else
										{
											DBCache.ht_keyvaluecache.Add("RACKFULLNAMEFLAG", value31);
										}
									}
								}
								dataTable = new DataTable();
								DBCacheStatus.SystemParameter = false;
							}
							goto IL_EB4;
						}
						catch (Exception ex)
						{
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshSysParaCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
							goto IL_EB4;
						}
					}
					DataTable dataTable2 = new DataTable();
					DBConn dBConn2 = null;
					DbCommand dbCommand2 = null;
					DbDataAdapter dbDataAdapter2 = null;
					try
					{
						dBConn2 = DBConnPool.getConnection();
						if (dBConn2 != null && dBConn2.con != null)
						{
							dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
							dbCommand2 = dBConn2.con.CreateCommand();
							dbCommand2.CommandText = "select para_name,para_type,para_value from sys_para ";
							dbDataAdapter2.SelectCommand = dbCommand2;
							dbDataAdapter2.Fill(dataTable2);
							dbDataAdapter2.Dispose();
							dbCommand2.Dispose();
							dBConn2.Close();
							if (dataTable2 != null)
							{
								DBCache.ht_keyvaluecache = new Hashtable();
								foreach (DataRow dataRow5 in dataTable2.Rows)
								{
									string key2 = Convert.ToString(dataRow5[0]);
									string value32 = "";
									if (!dataRow5.IsNull(2))
									{
										value32 = Convert.ToString(dataRow5[2]);
									}
									if (DBCache.ht_keyvaluecache.ContainsKey(key2))
									{
										DBCache.ht_keyvaluecache[key2] = value32;
									}
									else
									{
										DBCache.ht_keyvaluecache.Add(key2, value32);
									}
								}
							}
							dataTable2 = new DataTable();
							DBCacheStatus.SystemParameter = false;
						}
					}
					catch (Exception ex2)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshSysParaCache Error : " + ex2.Message + "\n" + ex2.StackTrace);
						try
						{
							dbDataAdapter2.Dispose();
						}
						catch
						{
						}
						try
						{
							dbCommand2.Dispose();
						}
						catch
						{
						}
						try
						{
							dBConn2.Close();
						}
						catch
						{
						}
					}
					IL_EB4:
					result = DBCache.ht_keyvaluecache;
				}
				else
				{
					result = DBCache.ht_keyvaluecache;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceVoltage(DBConn conn)
		{
			Hashtable result;
			lock (DBCache._lockdevvol)
			{
				if (DBCacheStatus.DeviceVoltage)
				{
					DataTable dataTable = new DataTable();
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						if (conn != null && conn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand = conn.con.CreateCommand();
							dbCommand.CommandText = "select id,voltage from device_voltage ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							if (dataTable != null)
							{
								DBCache.ht_devvolcache = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									try
									{
										int num = Convert.ToInt32(dataRow[0]);
										float num2 = 0f;
										if (!dataRow.IsNull(1))
										{
											num2 = Convert.ToSingle(dataRow[1]);
										}
										if (num2 <= 0f)
										{
											dbCommand.CommandText = "delete from device_voltage where id = " + num;
											try
											{
												dbCommand.ExecuteNonQuery();
											}
											catch
											{
											}
										}
										if (DBCache.ht_devvolcache.ContainsKey(num))
										{
											if (num2 > 0f)
											{
												DBCache.ht_devvolcache[num] = num2;
											}
											else
											{
												DBCache.ht_devvolcache.Remove(num);
											}
										}
										else
										{
											if (num2 > 0f)
											{
												DBCache.ht_devvolcache.Add(num, num2);
											}
										}
									}
									catch
									{
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.DeviceVoltage = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDeviceVoltageCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
					}
					result = DBCache.ht_devvolcache;
				}
				else
				{
					result = DBCache.ht_devvolcache;
				}
			}
			return result;
		}
		public static Hashtable GetDeviceVoltage()
		{
			Hashtable result;
			lock (DBCache._lockdevvol)
			{
				if (DBCacheStatus.DeviceVoltage)
				{
					DataTable dataTable = new DataTable();
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataAdapter dbDataAdapter = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "select id,voltage from device_voltage ";
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								DBCache.ht_devvolcache = new Hashtable();
								foreach (DataRow dataRow in dataTable.Rows)
								{
									try
									{
										int num = Convert.ToInt32(dataRow[0]);
										float num2 = 0f;
										if (!dataRow.IsNull(1))
										{
											num2 = Convert.ToSingle(dataRow[1]);
										}
										if (DBCache.ht_devvolcache.ContainsKey(num))
										{
											if (num2 > 0f)
											{
												DBCache.ht_devvolcache[num] = num2;
											}
											else
											{
												DBCache.ht_devvolcache.Remove(num);
											}
										}
										else
										{
											if (num2 > 0f)
											{
												DBCache.ht_devvolcache.Add(num, num2);
											}
										}
									}
									catch
									{
									}
								}
							}
							dataTable = new DataTable();
							DBCacheStatus.DeviceVoltage = false;
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshDeviceVoltageCache Error : " + ex.Message + "\n" + ex.StackTrace);
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
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					result = DBCache.ht_devvolcache;
				}
				else
				{
					result = DBCache.ht_devvolcache;
				}
			}
			return result;
		}
		public static void OpenSysDB(int i_tmp, string str_method)
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			lock (DBCache._locksysdb)
			{
				DBCache.i_sysdb++;
				try
				{
					DBCache.ht_sysdb.Add(i_tmp, str_method);
				}
				catch
				{
				}
			}
		}
		public static void CloseSysDB(int i_tmp)
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			lock (DBCache._locksysdb)
			{
				DBCache.i_sysdb--;
				try
				{
					DBCache.ht_sysdb.Remove(i_tmp);
				}
				catch
				{
				}
			}
		}
		public static void PrintSysDB()
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			try
			{
				lock (DBCache._locksysdb)
				{
					if (DBCache.ht_sysdb != null && DBCache.ht_sysdb.Count > 4)
					{
						DebugCenter.GetInstance().appendToFile("SSSSS : There were " + DBCache.ht_sysdb.Count + " SysDB connections were opened ");
						ICollection keys = DBCache.ht_sysdb.Keys;
						foreach (int num in keys)
						{
							DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
							{
								"SysDB ",
								num,
								" keeps opening, it was opened by ",
								(string)DBCache.ht_sysdb[num]
							}));
						}
					}
				}
			}
			catch
			{
			}
		}
		public static void OpenDataDB(int i_tmp, string str_method)
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			lock (DBCache._lockdatadb)
			{
				DBCache.i_datadb++;
				try
				{
					DBCache.ht_datadb.Add(i_tmp, str_method);
				}
				catch
				{
				}
			}
		}
		public static void CloseDataDB(int i_tmp)
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			lock (DBCache._lockdatadb)
			{
				DBCache.i_datadb--;
				try
				{
					DBCache.ht_datadb.Remove(i_tmp);
				}
				catch
				{
				}
			}
		}
		public static void PrintDataDB()
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			try
			{
				lock (DBCache._lockdatadb)
				{
					if (DBCache.ht_datadb != null && DBCache.ht_datadb.Count > 4)
					{
						DebugCenter.GetInstance().appendToFile("DDDDDDDDDDDDDDDDDDDDDDDDDDDD   There were " + DBCache.ht_datadb.Count + " data database connections were opened ");
						ICollection keys = DBCache.ht_datadb.Keys;
						foreach (int num in keys)
						{
							DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
							{
								"DataDB ",
								num,
								" keeps opening, it was opened by ",
								(string)DBCache.ht_datadb[num]
							}));
						}
					}
				}
			}
			catch
			{
			}
		}
		public static void OpenThermalDB(int i_tmp, string str_method)
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			lock (DBCache._lockthermaldb)
			{
				DBCache.i_thermaldb++;
				try
				{
					DBCache.ht_thermaldb.Add(i_tmp, str_method);
				}
				catch
				{
				}
			}
		}
		public static void CloseThermalDB(int i_tmp)
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			lock (DBCache._lockthermaldb)
			{
				DBCache.i_thermaldb--;
				try
				{
					DBCache.ht_thermaldb.Remove(i_tmp);
				}
				catch
				{
				}
			}
		}
		public static void PrintThermalDB()
		{
			if (!DBCacheStatus.b_showdebug)
			{
				return;
			}
			try
			{
				lock (DBCache._lockthermaldb)
				{
					if (DBCache.ht_thermaldb != null && DBCache.ht_thermaldb.Count > 4)
					{
						DebugCenter.GetInstance().appendToFile("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT  There were " + DBCache.ht_thermaldb.Count + " thermal database connections were opened ");
						ICollection keys = DBCache.ht_thermaldb.Keys;
						foreach (int num in keys)
						{
							DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
							{
								"ThermalDB ",
								num,
								" keeps opening, it was opened by ",
								(string)DBCache.ht_thermaldb[num]
							}));
						}
					}
				}
			}
			catch
			{
			}
		}
		public static int GetSensorCountByDid(Hashtable ht_device, int did)
		{
			try
			{
				if (ht_device != null && ht_device.ContainsKey(did))
				{
					DeviceInfo deviceInfo = (DeviceInfo)ht_device[did];
					string modelNm = deviceInfo.ModelNm;
					string fWVersion = deviceInfo.FWVersion;
					if (DBCache.GetSensorCount != null)
					{
						return DBCache.GetSensorCount(modelNm, fWVersion);
					}
				}
			}
			catch
			{
			}
			return -1;
		}
	}
}
