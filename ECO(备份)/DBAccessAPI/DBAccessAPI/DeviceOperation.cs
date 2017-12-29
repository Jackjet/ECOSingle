using CommonAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Threading;
namespace DBAccessAPI
{
	public class DeviceOperation
	{
		public const string DEVICE_BASE_COLUMN = "device";
		public const string DEVICE_RACK_COLUMN = "rackname";
		private static DateTime dt_last_voltage = new DateTime(1990, 1, 1, 0, 0, 0);
		private static object _lock_voltage = new object();
		public static DateTime Last_Update_Voltage
		{
			get
			{
				DateTime result;
				lock (DeviceOperation._lock_voltage)
				{
					result = DeviceOperation.dt_last_voltage;
				}
				return result;
			}
			set
			{
				lock (DeviceOperation._lock_voltage)
				{
					DeviceOperation.dt_last_voltage = value;
				}
			}
		}
		public static void RefreshDBCache(bool b_run_as_service)
		{
			DBCacheStatus.Device = true;
			if (b_run_as_service)
			{
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_AP_Device"
				});
				return;
			}
			DBCacheStatus.DBSyncEventSet(true, new string[]
			{
				"DBSyncEventName_Service_Device"
			});
		}
		public static int AddNewDevice(DBConn conn, DeviceInfo dev_info, List<PortInfo> portlists, List<SensorInfo> sensorlists, List<BankInfo> banklists, List<LineInfo> linelists)
		{
			DbCommand dbCommand = null;
			int num = 0;
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					DbTransaction transaction = DBConn.GetTransaction(conn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.Transaction = transaction;
					if (DBUrl.SERVERMODE)
					{
						string commandText = "insert into device_base_info (device_ip,device_nm,mac,privacypw,authenpw,privacy,authen,timeout,retry,user_name,portid,snmpVersion,model_nm,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,rack_id,fw_version,pop_flag,pop_threshold,door,device_capacity,outlet_pop,pop_lifo,pop_priority ) values(?device_ip,?device_nm,?mac,?privacypw,?authenpw,?privacy,?authen,?timeout,?retry,?user_name,?portid,?snmpVersion,?model_nm,?max_voltage,?min_voltage,?max_power_diss,?min_power_diss,?max_power,?min_power,?max_current,?min_current,?rack_id,?fw_version,?pop_flag,?pop_threshold,?door,?device_capacity,?outlet_pop,?pop_lifo,?pop_priority )";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_ip", dev_info.DeviceIP, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_nm", dev_info.DeviceName, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?mac", dev_info.Mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacypw", dev_info.PrivacyPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authenpw", dev_info.AuthenPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacy", dev_info.Privacy, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authen", dev_info.Authentication, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?timeout", dev_info.Timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?retry", dev_info.Retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?user_name", dev_info.Username, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?portid", dev_info.Port, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?snmpVersion", dev_info.SnmpVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?model_nm", dev_info.ModelNm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", dev_info.Max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", dev_info.Min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", dev_info.Max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", dev_info.Min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", dev_info.Max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", dev_info.Min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", dev_info.Max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", dev_info.Min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?rack_id", dev_info.RackID, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?fw_version", dev_info.FWVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_flag", dev_info.POPEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_threshold", dev_info.POPThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?door", dev_info.DoorSensor, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_capacity", dev_info.Capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?outlet_pop", dev_info.OutletPOPMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_lifo", dev_info.BankPOPLIFOMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_priority", dev_info.BankPOPPriorityMode, dbCommand));
					}
					else
					{
						string commandText = "insert into device_base_info (device_ip,device_nm,mac,privacypw,authenpw,privacy,authen,timeout,retry,user_name,portid,snmpVersion,model_nm,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,rack_id,fw_version,pop_flag,pop_threshold,door,device_capacity,outlet_pop,pop_lifo,pop_priority,b_priority,reference_voltage ) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_ip", dev_info.DeviceIP, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_nm", dev_info.DeviceName, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@mac", dev_info.Mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacypw", dev_info.PrivacyPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authenpw", dev_info.AuthenPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacy", dev_info.Privacy, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authen", dev_info.Authentication, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@timeout", dev_info.Timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@retry", dev_info.Retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@user_name", dev_info.Username, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@portid", dev_info.Port, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@snmpVersion", dev_info.SnmpVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@model_nm", dev_info.ModelNm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", dev_info.Max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", dev_info.Min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", dev_info.Max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", dev_info.Min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", dev_info.Max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", dev_info.Min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", dev_info.Max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", dev_info.Min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@rack_id", dev_info.RackID, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@fw_version", dev_info.FWVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_flag", dev_info.POPEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_threshold", dev_info.POPThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@door", dev_info.DoorSensor, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_capacity", dev_info.Capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@outlet_pop", dev_info.OutletPOPMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_lifo", dev_info.BankPOPLIFOMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_priority", dev_info.BankPOPPriorityMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@b_priority", dev_info.Bank_Priority, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@reference_voltage", dev_info.ReferenceVoltage, dbCommand));
					}
					int num2 = dbCommand.ExecuteNonQuery();
					if (num2 < 0)
					{
						dbCommand.Transaction.Rollback();
						int result = num2;
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
					dbCommand.Parameters.Clear();
					num = Convert.ToInt32(dbCommand.ExecuteScalar());
					if (num < 1)
					{
						dbCommand.Transaction.Rollback();
						int result = -1;
						return result;
					}
					if (portlists != null && portlists.Count > 0)
					{
						foreach (PortInfo current in portlists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into port_info (device_id,port_num,port_nm,port_confirmation,port_ondelay_time,port_offdelay_time,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,shutdown_method,mac) values(?device_id,?port_num,?port_nm,?port_confirmation,?port_ondelay_time,?port_offdelay_time,?max_voltage,?min_voltage,?max_power_diss,?min_power_diss,?max_power,?min_power,?max_current,?min_current,?shutdown_method,?mac)";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_num", current.PortNum, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_nm", current.PortName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_confirmation", current.OutletConfirmation, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_ondelay_time", current.OutletOnDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_offdelay_time", current.OutletOffDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", current.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", current.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", current.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", current.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", current.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", current.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", current.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", current.Min_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?shutdown_method", current.OutletShutdownMethod, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?mac", current.OutletMAC, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into port_info (device_id,port_num,port_nm,port_confirmation,port_ondelay_time,port_offdelay_time,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,shutdown_method,mac) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_num", current.PortNum, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_nm", current.PortName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_confirmation", current.OutletConfirmation, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_ondelay_time", current.OutletOnDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_offdelay_time", current.OutletOffDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", current.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", current.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", current.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", current.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", current.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", current.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", current.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", current.Min_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@shutdown_method", current.OutletShutdownMethod, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@mac", current.OutletMAC, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "SELECT LAST_INSERT_ID()";
							}
							else
							{
								dbCommand.CommandText = "select @@identity";
							}
							int num3 = Convert.ToInt32(dbCommand.ExecuteScalar());
							if (num3 < 1)
							{
								dbCommand.Transaction.Rollback();
								int result = -1;
								return result;
							}
						}
					}
					if (sensorlists != null && sensorlists.Count > 0)
					{
						foreach (SensorInfo current2 in sensorlists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into device_sensor_info (device_id,sensor_nm,max_humidity,min_humidity,max_temperature,min_temperature,max_press,min_press,sensor_type,location_type) values(?device_id,?sensor_nm,?max_humidity,?min_humidity,?max_temperature,?min_temperature,?max_press,?min_press,?sensor_type,?location_type)";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_nm", current2.SensorName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_humidity", current2.Max_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_humidity", current2.Min_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_temperature", current2.Max_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_temperature", current2.Min_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_press", current2.Max_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_press", current2.Min_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_type", current2.Type, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?location_type", current2.Location, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into device_sensor_info (device_id,sensor_nm,max_humidity,min_humidity,max_temperature,min_temperature,max_press,min_press,sensor_type,location_type) values(?,?,?,?,?,?,?,?,?,?)";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_nm", current2.SensorName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_humidity", current2.Max_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_humidity", current2.Min_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_temperature", current2.Max_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_temperature", current2.Min_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_press", current2.Max_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_press", current2.Min_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_type", current2.Type, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@location_type", current2.Location, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
						}
					}
					if (banklists != null && banklists.Count > 0)
					{
						foreach (BankInfo current3 in banklists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into bank_info (device_id,port_nums,bank_nm,voltage,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current) values(?device_id,?port_nums,?bank_nm,?voltage,?max_voltage,?min_voltage,?max_power_diss,?min_power_diss,?max_power,?min_power,?max_current,?min_current )";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_nums", current3.PortLists, dbCommand));
								if (current3.BankName != null && current3.BankName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?bank_nm", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?bank_nm", current3.BankName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("?voltage", current3.Voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", current3.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", current3.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", current3.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", current3.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", current3.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", current3.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", current3.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", current3.Min_current, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into bank_info (device_id,port_nums,bank_nm,voltage,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current) values(?,?,?,?,?,?,?,?,?,?,?,? )";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_nums", current3.PortLists, dbCommand));
								if (current3.BankName != null && current3.BankName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@bank_nm", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@bank_nm", current3.BankName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("@voltage", current3.Voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", current3.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", current3.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", current3.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", current3.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", current3.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", current3.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", current3.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", current3.Min_current, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
						}
					}
					if (linelists != null && linelists.Count > 0)
					{
						foreach (LineInfo current4 in linelists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into line_info (device_id,line_name,line_number,max_voltage,min_voltage,max_power,min_power,max_current,min_current) values(?device_id,?line_name,?line_number,?max_voltage,?min_voltage,?max_power,?min_power,?max_current,?min_current )";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								if (current4.LineName != null && current4.LineName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?line_name", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?line_name", current4.LineName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("?line_number", current4.LineNumber, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", current4.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", current4.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", current4.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", current4.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", current4.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", current4.Min_current, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into line_info (device_id,line_name,line_number,max_voltage,min_voltage,max_power,min_power,max_current,min_current) values(?,?,?,?,?,?,?,?,? )";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								if (current4.LineName != null && current4.LineName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@line_name", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@line_name", current4.LineName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("@line_number", current4.LineNumber, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", current4.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", current4.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", current4.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", current4.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", current4.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", current4.Min_current, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
						}
					}
					dbCommand.Transaction.Commit();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				dbCommand.Transaction.Rollback();
				if (ex.Message.IndexOf(" duplicate values ") > 0)
				{
					int result = -2;
					return result;
				}
			}
			finally
			{
				dbCommand.Dispose();
			}
			return num;
		}
		public static int AddNewDevice(DeviceInfo dev_info, List<PortInfo> portlists, List<SensorInfo> sensorlists, List<BankInfo> banklists, List<LineInfo> linelists)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			int num = 0;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.Transaction = transaction;
					if (DBUrl.SERVERMODE)
					{
						string commandText = "insert into device_base_info (device_ip,device_nm,mac,privacypw,authenpw,privacy,authen,timeout,retry,user_name,portid,snmpVersion,model_nm,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,rack_id,fw_version,pop_flag,pop_threshold,door,device_capacity,outlet_pop,pop_lifo,pop_priority ) values(?device_ip,?device_nm,?mac,?privacypw,?authenpw,?privacy,?authen,?timeout,?retry,?user_name,?portid,?snmpVersion,?model_nm,?max_voltage,?min_voltage,?max_power_diss,?min_power_diss,?max_power,?min_power,?max_current,?min_current,?rack_id,?fw_version,?pop_flag,?pop_threshold,?door,?device_capacity,?outlet_pop,?pop_lifo,?pop_priority )";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_ip", dev_info.DeviceIP, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_nm", dev_info.DeviceName, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?mac", dev_info.Mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacypw", dev_info.PrivacyPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authenpw", dev_info.AuthenPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacy", dev_info.Privacy, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authen", dev_info.Authentication, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?timeout", dev_info.Timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?retry", dev_info.Retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?user_name", dev_info.Username, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?portid", dev_info.Port, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?snmpVersion", dev_info.SnmpVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?model_nm", dev_info.ModelNm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", dev_info.Max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", dev_info.Min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", dev_info.Max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", dev_info.Min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", dev_info.Max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", dev_info.Min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", dev_info.Max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", dev_info.Min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?rack_id", dev_info.RackID, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?fw_version", dev_info.FWVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_flag", dev_info.POPEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_threshold", dev_info.POPThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?door", dev_info.DoorSensor, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_capacity", dev_info.Capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?outlet_pop", dev_info.OutletPOPMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_lifo", dev_info.BankPOPLIFOMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_priority", dev_info.BankPOPPriorityMode, dbCommand));
					}
					else
					{
						string commandText = "insert into device_base_info (device_ip,device_nm,mac,privacypw,authenpw,privacy,authen,timeout,retry,user_name,portid,snmpVersion,model_nm,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,rack_id,fw_version,pop_flag,pop_threshold,door,device_capacity,outlet_pop,pop_lifo,pop_priority,b_priority,reference_voltage ) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,? )";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_ip", dev_info.DeviceIP, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_nm", dev_info.DeviceName, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@mac", dev_info.Mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacypw", dev_info.PrivacyPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authenpw", dev_info.AuthenPassword, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacy", dev_info.Privacy, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authen", dev_info.Authentication, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@timeout", dev_info.Timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@retry", dev_info.Retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@user_name", dev_info.Username, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@portid", dev_info.Port, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@snmpVersion", dev_info.SnmpVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@model_nm", dev_info.ModelNm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", dev_info.Max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", dev_info.Min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", dev_info.Max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", dev_info.Min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", dev_info.Max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", dev_info.Min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", dev_info.Max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", dev_info.Min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@rack_id", dev_info.RackID, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@fw_version", dev_info.FWVersion, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_flag", dev_info.POPEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_threshold", dev_info.POPThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@door", dev_info.DoorSensor, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_capacity", dev_info.Capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@outlet_pop", dev_info.OutletPOPMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_lifo", dev_info.BankPOPLIFOMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_priority", dev_info.BankPOPPriorityMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@b_priority", dev_info.Bank_Priority, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@reference_voltage", dev_info.ReferenceVoltage, dbCommand));
					}
					int num2 = dbCommand.ExecuteNonQuery();
					if (num2 < 0)
					{
						dbCommand.Transaction.Rollback();
						int result = num2;
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
					dbCommand.Parameters.Clear();
					num = Convert.ToInt32(dbCommand.ExecuteScalar());
					if (num < 1)
					{
						dbCommand.Transaction.Rollback();
						int result = -1;
						return result;
					}
					if (portlists != null && portlists.Count > 0)
					{
						foreach (PortInfo current in portlists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into port_info (device_id,port_num,port_nm,port_confirmation,port_ondelay_time,port_offdelay_time,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,shutdown_method,mac) values(?device_id,?port_num,?port_nm,?port_confirmation,?port_ondelay_time,?port_offdelay_time,?max_voltage,?min_voltage,?max_power_diss,?min_power_diss,?max_power,?min_power,?max_current,?min_current,?shutdown_method,?mac)";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_num", current.PortNum, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_nm", current.PortName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_confirmation", current.OutletConfirmation, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_ondelay_time", current.OutletOnDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_offdelay_time", current.OutletOffDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", current.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", current.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", current.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", current.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", current.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", current.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", current.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", current.Min_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?shutdown_method", current.OutletShutdownMethod, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?mac", current.OutletMAC, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into port_info (device_id,port_num,port_nm,port_confirmation,port_ondelay_time,port_offdelay_time,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current,shutdown_method,mac) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_num", current.PortNum, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_nm", current.PortName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_confirmation", current.OutletConfirmation, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_ondelay_time", current.OutletOnDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_offdelay_time", current.OutletOffDelayTime, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", current.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", current.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", current.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", current.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", current.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", current.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", current.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", current.Min_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@shutdown_method", current.OutletShutdownMethod, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@mac", current.OutletMAC, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "SELECT LAST_INSERT_ID()";
							}
							else
							{
								dbCommand.CommandText = "select @@identity";
							}
							int num3 = Convert.ToInt32(dbCommand.ExecuteScalar());
							if (num3 < 1)
							{
								dbCommand.Transaction.Rollback();
								int result = -1;
								return result;
							}
						}
					}
					if (sensorlists != null && sensorlists.Count > 0)
					{
						foreach (SensorInfo current2 in sensorlists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into device_sensor_info (device_id,sensor_nm,max_humidity,min_humidity,max_temperature,min_temperature,max_press,min_press,sensor_type,location_type) values(?device_id,?sensor_nm,?max_humidity,?min_humidity,?max_temperature,?min_temperature,?max_press,?min_press,?sensor_type,?location_type)";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_nm", current2.SensorName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_humidity", current2.Max_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_humidity", current2.Min_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_temperature", current2.Max_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_temperature", current2.Min_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_press", current2.Max_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_press", current2.Min_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_type", current2.Type, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?location_type", current2.Location, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into device_sensor_info (device_id,sensor_nm,max_humidity,min_humidity,max_temperature,min_temperature,max_press,min_press,sensor_type,location_type) values(?,?,?,?,?,?,?,?,?,?)";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_nm", current2.SensorName, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_humidity", current2.Max_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_humidity", current2.Min_humidity, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_temperature", current2.Max_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_temperature", current2.Min_temperature, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_press", current2.Max_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_press", current2.Min_press, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_type", current2.Type, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@location_type", current2.Location, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
						}
					}
					if (banklists != null && banklists.Count > 0)
					{
						foreach (BankInfo current3 in banklists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into bank_info (device_id,port_nums,bank_nm,voltage,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current) values(?device_id,?port_nums,?bank_nm,?voltage,?max_voltage,?min_voltage,?max_power_diss,?min_power_diss,?max_power,?min_power,?max_current,?min_current )";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?port_nums", current3.PortLists, dbCommand));
								if (current3.BankName != null && current3.BankName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?bank_nm", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?bank_nm", current3.BankName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("?voltage", current3.Voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", current3.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", current3.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", current3.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", current3.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", current3.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", current3.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", current3.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", current3.Min_current, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into bank_info (device_id,port_nums,bank_nm,voltage,max_voltage,min_voltage,max_power_diss,min_power_diss,max_power,min_power,max_current,min_current) values(?,?,?,?,?,?,?,?,?,?,?,? )";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@port_nums", current3.PortLists, dbCommand));
								if (current3.BankName != null && current3.BankName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@bank_nm", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@bank_nm", current3.BankName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("@voltage", current3.Voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", current3.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", current3.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", current3.Max_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", current3.Min_power_diss, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", current3.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", current3.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", current3.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", current3.Min_current, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
						}
					}
					if (linelists != null && linelists.Count > 0)
					{
						foreach (LineInfo current4 in linelists)
						{
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into line_info (device_id,line_name,line_number,max_voltage,min_voltage,max_power,min_power,max_current,min_current) values(?device_id,?line_name,?line_number,?max_voltage,?min_voltage,?max_power,?min_power,?max_current,?min_current )";
								dbCommand.Parameters.Add(DBTools.GetParameter("?device_id", num, dbCommand));
								if (current4.LineName != null && current4.LineName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?line_name", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("?line_name", current4.LineName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("?line_number", current4.LineNumber, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", current4.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", current4.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", current4.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", current4.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", current4.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", current4.Min_current, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into line_info (device_id,line_name,line_number,max_voltage,min_voltage,max_power,min_power,max_current,min_current) values(?,?,?,?,?,?,?,?,? )";
								dbCommand.Parameters.Add(DBTools.GetParameter("@device_id", num, dbCommand));
								if (current4.LineName != null && current4.LineName.Equals("\r\n"))
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@line_name", "", dbCommand));
								}
								else
								{
									dbCommand.Parameters.Add(DBTools.GetParameter("@line_name", current4.LineName, dbCommand));
								}
								dbCommand.Parameters.Add(DBTools.GetParameter("@line_number", current4.LineNumber, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", current4.Max_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", current4.Min_voltage, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", current4.Max_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", current4.Min_power, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", current4.Max_current, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", current4.Min_current, dbCommand));
							}
							num2 = dbCommand.ExecuteNonQuery();
							if (num2 < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num2;
								return result;
							}
							dbCommand.Parameters.Clear();
						}
					}
					dbCommand.Transaction.Commit();
					DBCacheStatus.Device = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Device"
					});
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				dbCommand.Transaction.Rollback();
				if (ex.Message.IndexOf(" duplicate values ") > 0)
				{
					int result = -2;
					return result;
				}
			}
			finally
			{
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return num;
		}
		public static Dictionary<long, CommParaClass> GetPortModelMapping()
		{
			Dictionary<long, CommParaClass> dictionary = new Dictionary<long, CommParaClass>();
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				Hashtable devicePortMap = DBCache.GetDevicePortMap();
				Hashtable portCache = DBCache.GetPortCache();
				if (deviceCache != null && deviceCache.Count > 0 && devicePortMap != null && devicePortMap.Count > 0 && portCache != null && portCache.Count > 0)
				{
					ICollection values = portCache.Values;
					foreach (PortInfo portInfo in values)
					{
						int deviceID = portInfo.DeviceID;
						int iD = portInfo.ID;
						if (deviceCache.ContainsKey(deviceID) && devicePortMap.ContainsKey(deviceID))
						{
							DeviceInfo deviceInfo = (DeviceInfo)deviceCache[deviceID];
							List<int> list = (List<int>)devicePortMap[deviceID];
							CommParaClass commParaClass = new CommParaClass();
							commParaClass.Long_First = (long)iD;
							commParaClass.String_First = deviceInfo.ModelNm;
							commParaClass.Integer_First = list.Count;
							commParaClass.String_2 = deviceInfo.FWVersion;
							dictionary.Add(Convert.ToInt64(iD), commParaClass);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				string arg_16C_0 = ex.Message;
			}
			return dictionary;
		}
		public static Dictionary<long, CommParaClass> GetDeviceModelMapping()
		{
			Dictionary<long, CommParaClass> dictionary = new Dictionary<long, CommParaClass>();
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (deviceCache != null && deviceCache.Count > 0)
				{
					ICollection values = deviceCache.Values;
					foreach (DeviceInfo deviceInfo in values)
					{
						int deviceID = deviceInfo.DeviceID;
						CommParaClass commParaClass = new CommParaClass();
						commParaClass.String_First = deviceInfo.ModelNm;
						commParaClass.String_2 = deviceInfo.FWVersion;
						dictionary.Add(Convert.ToInt64(deviceID), commParaClass);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				string arg_C0_0 = ex.Message;
			}
			return dictionary;
		}
		public static Dictionary<long, CommParaClass> GetDeviceRackMapping()
		{
			Dictionary<long, CommParaClass> dictionary = new Dictionary<long, CommParaClass>();
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				Hashtable rackCache = DBCache.GetRackCache();
				if (deviceCache != null && deviceCache.Count > 0 && rackCache != null && rackCache.Count > 0)
				{
					bool flag = false;
					if (Sys_Para.GetRackFullNameflag() == 1)
					{
						flag = true;
					}
					ICollection values = deviceCache.Values;
					foreach (DeviceInfo deviceInfo in values)
					{
						int deviceID = deviceInfo.DeviceID;
						long rackID = deviceInfo.RackID;
						if (rackCache.ContainsKey(Convert.ToInt32(rackID)))
						{
							RackInfo rackInfo = (RackInfo)rackCache[Convert.ToInt32(rackID)];
							string text = "";
							if (flag)
							{
								try
								{
									text = rackInfo.RackFullName;
									if (text == null || text.Length == 0)
									{
										text = rackInfo.OriginalName;
									}
									goto IL_E8;
								}
								catch
								{
									text = rackInfo.OriginalName;
									goto IL_E8;
								}
								goto IL_DF;
							}
							goto IL_DF;
							IL_E8:
							CommParaClass commParaClass = new CommParaClass();
							commParaClass.Long_First = rackID;
							commParaClass.String_First = text;
							dictionary.Add(Convert.ToInt64(deviceID), commParaClass);
							continue;
							IL_DF:
							text = rackInfo.OriginalName;
							goto IL_E8;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				string arg_165_0 = ex.Message;
			}
			return dictionary;
		}
		public static List<DeviceInfo> GetAllDevice()
		{
			List<DeviceInfo> list = new List<DeviceInfo>();
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (deviceCache != null && deviceCache.Count > 0)
				{
					ICollection values = deviceCache.Values;
					foreach (DeviceInfo item in values)
					{
						list.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			list.Sort((DeviceInfo x, DeviceInfo y) => x.DeviceName.CompareTo(y.DeviceName));
			return list;
		}
		public static List<DeviceInfo> GetAllDeviceByModel(string str_model)
		{
			List<DeviceInfo> list = new List<DeviceInfo>();
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (deviceCache != null && deviceCache.Count > 0)
				{
					ICollection values = deviceCache.Values;
					foreach (DeviceInfo deviceInfo in values)
					{
						if (deviceInfo.ModelNm.Equals(str_model))
						{
							list.Add(deviceInfo);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			list.Sort((DeviceInfo x, DeviceInfo y) => x.DeviceName.CompareTo(y.DeviceName));
			return list;
		}
		public static DeviceInfo getDeviceByName(string str_name)
		{
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (deviceCache != null && deviceCache.Count > 0)
				{
					ICollection values = deviceCache.Values;
					foreach (DeviceInfo deviceInfo in values)
					{
						if (deviceInfo.DeviceName.Equals(str_name))
						{
							return deviceInfo;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static DeviceInfo getDeviceByID(int l_id)
		{
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (deviceCache != null && deviceCache.Count > 0 && deviceCache.ContainsKey(l_id))
				{
					DeviceInfo deviceInfo = (DeviceInfo)deviceCache[l_id];
					return deviceInfo.CloneDeviceInfo();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DeviceInfo struct by ID error : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static DeviceInfo getDeviceByMac(string mac)
		{
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (deviceCache != null && deviceCache.Count > 0)
				{
					ICollection values = deviceCache.Values;
					foreach (DeviceInfo deviceInfo in values)
					{
						if (deviceInfo.Mac.Equals(mac))
						{
							return deviceInfo;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static bool StartDBCleanupThread()
		{
			try
			{
				Thread thread = new Thread(new ThreadStart(DeviceOperation.cleanupdatadb));
				thread.Start();
				return true;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Start thread to cleanup datadb error : " + ex.Message + "\n" + ex.StackTrace);
			}
			return false;
		}
		private static void cleanupdatadb()
		{
			if (DBUrl.SERVERMODE)
			{
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				DbDataAdapter dbDataAdapter = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						try
						{
							dbCommand.CommandText = "delete from device_data_daily where device_id not in (select id from device_base_info )";
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand.CommandText = "delete from device_data_hourly where device_id not in (select id from device_base_info )";
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand.CommandText = "delete from bank_data_hourly where bank_id not in (select id from bank_info )";
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand.CommandText = "delete from bank_data_daily where bank_id not in (select id from bank_info )";
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand.CommandText = "delete from port_data_hourly where port_id not in (select id from port_info )";
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							dbCommand.CommandText = "delete from port_data_daily where port_id not in (select id from port_info )";
							dbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						DataTable dataTable = new DataTable();
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where table_name like '%_auto_info%' and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							foreach (DataRow dataRow in dataTable.Rows)
							{
								string text = Convert.ToString(dataRow[0]);
								if (text.IndexOf("device") > -1)
								{
									dbCommand.CommandText = "delete from " + Convert.ToString(dataRow[0]) + " where device_id not in (select id from device_base_info )";
								}
								else
								{
									if (text.IndexOf("bank") > -1)
									{
										dbCommand.CommandText = "delete from " + Convert.ToString(dataRow[0]) + " where bank_id not in (select id from bank_info )";
									}
									else
									{
										if (text.IndexOf("port") > -1)
										{
											dbCommand.CommandText = "delete from " + Convert.ToString(dataRow[0]) + " where port_id not in (select id from port_info )";
										}
									}
								}
								try
								{
									dbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
						}
						dataTable = new DataTable();
						dbCommand.Dispose();
						dBConn.Close();
					}
					return;
				}
				catch (Exception)
				{
					if (dbDataAdapter != null)
					{
						try
						{
							dbDataAdapter.Dispose();
						}
						catch
						{
						}
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
					}
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					return;
				}
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				Hashtable hashtable = new Hashtable();
				Hashtable hashtable2 = new Hashtable();
				Hashtable hashtable3 = new Hashtable();
				Hashtable deviceCache = DBCache.GetDeviceCache();
				Hashtable bankCache = DBCache.GetBankCache();
				Hashtable portCache = DBCache.GetPortCache();
				if (deviceCache == null || deviceCache.Count <= 0 || bankCache == null || bankCache.Count <= 0 || portCache == null || portCache.Count <= 0)
				{
					return;
				}
				DBConn dBConn2 = null;
				DbCommand dbCommand2 = null;
				DbDataAdapter dbDataAdapter2 = null;
				try
				{
					dBConn2 = DBConnPool.getDynaConnection();
					if (dBConn2 != null && dBConn2.con != null)
					{
						dbCommand2 = dBConn2.con.CreateCommand();
						DataTable dataTable2 = new DataTable();
						dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
						dbCommand2.CommandText = "select distinct(device_id) from device_data_daily ";
						dbDataAdapter2.SelectCommand = dbCommand2;
						dbDataAdapter2.Fill(dataTable2);
						dbDataAdapter2.Dispose();
						if (dataTable2 != null)
						{
							foreach (DataRow dataRow2 in dataTable2.Rows)
							{
								int num = Convert.ToInt32(dataRow2[0]);
								if (!deviceCache.ContainsKey(num))
								{
									try
									{
										hashtable.Add(num, num);
									}
									catch
									{
									}
									try
									{
										dbCommand2.CommandText = "delete from device_data_daily where device_id = " + num;
										dbCommand2.ExecuteNonQuery();
									}
									catch
									{
									}
								}
							}
						}
						dataTable2 = new DataTable();
						dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
						dbCommand2.CommandText = "select distinct(device_id) from device_data_hourly ";
						dbDataAdapter2.SelectCommand = dbCommand2;
						dbDataAdapter2.Fill(dataTable2);
						dbDataAdapter2.Dispose();
						if (dataTable2 != null)
						{
							foreach (DataRow dataRow3 in dataTable2.Rows)
							{
								int num2 = Convert.ToInt32(dataRow3[0]);
								if (!deviceCache.ContainsKey(num2))
								{
									try
									{
										dbCommand2.CommandText = "delete from device_data_hourly where device_id = " + num2;
										dbCommand2.ExecuteNonQuery();
									}
									catch
									{
									}
								}
							}
						}
						dataTable2 = new DataTable();
						dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
						dbCommand2.CommandText = "select distinct(bank_id) from bank_data_daily ";
						dbDataAdapter2.SelectCommand = dbCommand2;
						dbDataAdapter2.Fill(dataTable2);
						dbDataAdapter2.Dispose();
						if (dataTable2 != null)
						{
							foreach (DataRow dataRow4 in dataTable2.Rows)
							{
								int num3 = Convert.ToInt32(dataRow4[0]);
								if (!bankCache.ContainsKey(num3))
								{
									try
									{
										hashtable2.Add(num3, num3);
									}
									catch
									{
									}
									try
									{
										dbCommand2.CommandText = "delete from bank_data_daily where bank_id = " + num3;
										dbCommand2.ExecuteNonQuery();
									}
									catch
									{
									}
								}
							}
						}
						dataTable2 = new DataTable();
						dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
						dbCommand2.CommandText = "select distinct(bank_id) from bank_data_hourly ";
						dbDataAdapter2.SelectCommand = dbCommand2;
						dbDataAdapter2.Fill(dataTable2);
						dbDataAdapter2.Dispose();
						if (dataTable2 != null)
						{
							foreach (DataRow dataRow5 in dataTable2.Rows)
							{
								int num4 = Convert.ToInt32(dataRow5[0]);
								if (!bankCache.ContainsKey(num4))
								{
									try
									{
										dbCommand2.CommandText = "delete from bank_data_hourly where bank_id = " + num4;
										dbCommand2.ExecuteNonQuery();
									}
									catch
									{
									}
								}
							}
						}
						dataTable2 = new DataTable();
						dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
						dbCommand2.CommandText = "select distinct(port_id) from port_data_hourly ";
						dbDataAdapter2.SelectCommand = dbCommand2;
						dbDataAdapter2.Fill(dataTable2);
						dbDataAdapter2.Dispose();
						if (dataTable2 != null)
						{
							foreach (DataRow dataRow6 in dataTable2.Rows)
							{
								int num5 = Convert.ToInt32(dataRow6[0]);
								if (!portCache.ContainsKey(num5))
								{
									try
									{
										dbCommand2.CommandText = "delete from port_data_hourly where port_id = " + num5;
										dbCommand2.ExecuteNonQuery();
									}
									catch
									{
									}
								}
							}
						}
						dataTable2 = new DataTable();
						dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
						dbCommand2.CommandText = "select distinct(port_id) from port_data_daily ";
						dbDataAdapter2.SelectCommand = dbCommand2;
						dbDataAdapter2.Fill(dataTable2);
						dbDataAdapter2.Dispose();
						if (dataTable2 != null)
						{
							foreach (DataRow dataRow7 in dataTable2.Rows)
							{
								int num6 = Convert.ToInt32(dataRow7[0]);
								if (!portCache.ContainsKey(num6))
								{
									try
									{
										hashtable3.Add(num6, num6);
									}
									catch
									{
									}
									try
									{
										dbCommand2.CommandText = "delete from port_data_daily where port_id = " + num6;
										dbCommand2.ExecuteNonQuery();
									}
									catch
									{
									}
								}
							}
						}
						dataTable2 = new DataTable();
						dbDataAdapter2 = DBConn.GetDataAdapter(dBConn2.con);
						dbCommand2.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where table_name like '%_auto_info%' and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						dbDataAdapter2.SelectCommand = dbCommand2;
						dbDataAdapter2.Fill(dataTable2);
						dbDataAdapter2.Dispose();
						if (dataTable2 != null)
						{
							foreach (DataRow dataRow8 in dataTable2.Rows)
							{
								string text2 = Convert.ToString(dataRow8[0]);
								if (text2.IndexOf("device") > -1)
								{
									ICollection keys = hashtable.Keys;
									IEnumerator enumerator2 = keys.GetEnumerator();
									try
									{
										while (enumerator2.MoveNext())
										{
											int num7 = (int)enumerator2.Current;
											try
											{
												dbCommand2.CommandText = string.Concat(new object[]
												{
													"delete from ",
													Convert.ToString(dataRow8[0]),
													" where device_id = ",
													num7
												});
												dbCommand2.ExecuteNonQuery();
											}
											catch
											{
											}
										}
										continue;
									}
									finally
									{
										IDisposable disposable = enumerator2 as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
								}
								if (text2.IndexOf("bank") > -1)
								{
									ICollection keys2 = hashtable2.Keys;
									IEnumerator enumerator2 = keys2.GetEnumerator();
									try
									{
										while (enumerator2.MoveNext())
										{
											int num8 = (int)enumerator2.Current;
											try
											{
												dbCommand2.CommandText = string.Concat(new object[]
												{
													"delete from ",
													Convert.ToString(dataRow8[0]),
													" where bank_id = ",
													num8
												});
												dbCommand2.ExecuteNonQuery();
											}
											catch
											{
											}
										}
										continue;
									}
									finally
									{
										IDisposable disposable = enumerator2 as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
								}
								if (text2.IndexOf("port") > -1)
								{
									ICollection keys3 = hashtable3.Keys;
									foreach (int num9 in keys3)
									{
										try
										{
											dbCommand2.CommandText = string.Concat(new object[]
											{
												"delete from ",
												Convert.ToString(dataRow8[0]),
												" where port_id = ",
												num9
											});
											dbCommand2.ExecuteNonQuery();
										}
										catch
										{
										}
									}
								}
							}
						}
						dataTable2 = new DataTable();
						dbCommand2.Dispose();
						dBConn2.Close();
					}
					return;
				}
				catch (Exception)
				{
					if (dbDataAdapter2 != null)
					{
						try
						{
							dbDataAdapter2.Dispose();
						}
						catch
						{
						}
					}
					if (dbCommand2 != null)
					{
						try
						{
							dbCommand2.Dispose();
						}
						catch
						{
						}
					}
					if (dBConn2 != null)
					{
						try
						{
							dBConn2.Close();
						}
						catch
						{
						}
					}
					return;
				}
			}
			Hashtable deviceCache2 = DBCache.GetDeviceCache();
			Hashtable bankCache2 = DBCache.GetBankCache();
			Hashtable portCache2 = DBCache.GetPortCache();
			if (deviceCache2 != null && deviceCache2.Count > 0 && bankCache2 != null && bankCache2.Count > 0 && portCache2 != null && portCache2.Count > 0)
			{
				string text3 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
				if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
				{
					text3 += Path.DirectorySeparatorChar;
				}
				if (Directory.Exists(text3))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(text3);
					FileInfo[] files = directoryInfo.GetFiles();
					if (files.Length != 0)
					{
						FileInfo[] array = files;
						for (int i = 0; i < array.Length; i++)
						{
							FileInfo fileInfo = array[i];
							if (fileInfo.Name.IndexOf("datadb") == 0 && fileInfo.Extension.ToLower().Equals(".mdb"))
							{
								DataTable dataTable3 = new DataTable();
								DbConnection dbConnection = null;
								DbDataAdapter dbDataAdapter3 = null;
								DbCommand dbCommand3 = null;
								string str = text3 + fileInfo.Name;
								try
								{
									dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str + ";Jet OLEDB:Database Password=root");
									dbConnection.Open();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3 = dbConnection.CreateCommand();
									dbCommand3.CommandText = "select distinct(device_id) from device_data_daily ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow9 in dataTable3.Rows)
										{
											int num10 = Convert.ToInt32(dataRow9[0]);
											if (!deviceCache2.ContainsKey(num10))
											{
												try
												{
													dbCommand3.CommandText = "delete from device_data_daily where device_id = " + num10;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(device_id) from device_data_hourly ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow10 in dataTable3.Rows)
										{
											int num11 = Convert.ToInt32(dataRow10[0]);
											if (!deviceCache2.ContainsKey(num11))
											{
												try
												{
													dbCommand3.CommandText = "delete from device_data_hourly where device_id = " + num11;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(device_id) from device_auto_info ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow11 in dataTable3.Rows)
										{
											int num12 = Convert.ToInt32(dataRow11[0]);
											if (!deviceCache2.ContainsKey(num12))
											{
												try
												{
													dbCommand3.CommandText = "delete from device_auto_info where device_id = " + num12;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(bank_id) from bank_auto_info ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow12 in dataTable3.Rows)
										{
											int num13 = Convert.ToInt32(dataRow12[0]);
											if (!bankCache2.ContainsKey(num13))
											{
												try
												{
													dbCommand3.CommandText = "delete from bank_auto_info where bank_id = " + num13;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(bank_id) from bank_data_daily ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow13 in dataTable3.Rows)
										{
											int num14 = Convert.ToInt32(dataRow13[0]);
											if (!bankCache2.ContainsKey(num14))
											{
												try
												{
													dbCommand3.CommandText = "delete from bank_data_daily where bank_id = " + num14;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(bank_id) from bank_data_hourly ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow14 in dataTable3.Rows)
										{
											int num15 = Convert.ToInt32(dataRow14[0]);
											if (!bankCache2.ContainsKey(num15))
											{
												try
												{
													dbCommand3.CommandText = "delete from bank_data_hourly where bank_id = " + num15;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(port_id) from port_data_hourly ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow15 in dataTable3.Rows)
										{
											int num16 = Convert.ToInt32(dataRow15[0]);
											if (!portCache2.ContainsKey(num16))
											{
												try
												{
													dbCommand3.CommandText = "delete from port_data_hourly where port_id = " + num16;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(port_id) from port_data_daily ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow16 in dataTable3.Rows)
										{
											int num17 = Convert.ToInt32(dataRow16[0]);
											if (!portCache2.ContainsKey(num17))
											{
												try
												{
													dbCommand3.CommandText = "delete from port_data_daily where port_id = " + num17;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbDataAdapter3 = new OleDbDataAdapter();
									dbCommand3.CommandText = "select distinct(port_id) from port_auto_info ";
									dbDataAdapter3.SelectCommand = dbCommand3;
									dbDataAdapter3.Fill(dataTable3);
									dbDataAdapter3.Dispose();
									if (dataTable3 != null)
									{
										foreach (DataRow dataRow17 in dataTable3.Rows)
										{
											int num18 = Convert.ToInt32(dataRow17[0]);
											if (!portCache2.ContainsKey(num18))
											{
												try
												{
													dbCommand3.CommandText = "delete from port_auto_info where port_id = " + num18;
													dbCommand3.ExecuteNonQuery();
												}
												catch
												{
												}
											}
										}
									}
									dataTable3 = new DataTable();
									dbCommand3.Dispose();
									dbConnection.Close();
								}
								catch (Exception)
								{
									if (dbDataAdapter3 != null)
									{
										try
										{
											dbDataAdapter3.Dispose();
										}
										catch
										{
										}
									}
									if (dbCommand3 != null)
									{
										try
										{
											dbCommand3.Dispose();
										}
										catch
										{
										}
									}
									if (dbConnection != null)
									{
										try
										{
											dbConnection.Close();
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
		}
		public static int DeleteDeviceByID(DBConn conn, int l_id)
		{
			Hashtable devicePortMap = DBCache.GetDevicePortMap();
			int result = -1;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (conn != null && conn.con != null)
				{
					DbTransaction transaction = DBConn.GetTransaction(conn.con);
					dbCommand = conn.con.CreateCommand();
					dbCommand.CommandType = CommandType.Text;
					dbCommand.Transaction = transaction;
					dbCommand.CommandText = "delete from device_sensor_info where device_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from bank_info where device_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from line_info where device_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from port_info where device_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from group_detail where grouptype = 'dev' and dest_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					try
					{
						if (devicePortMap.ContainsKey(l_id))
						{
							List<int> list = (List<int>)devicePortMap[l_id];
							foreach (int current in list)
							{
								dbCommand.CommandText = "delete from group_detail where grouptype = 'outlet' and dest_id = " + current;
								dbCommand.ExecuteNonQuery();
							}
						}
					}
					catch
					{
					}
					dbCommand.CommandText = "delete from device_base_info where id = " + l_id;
					result = dbCommand.ExecuteNonQuery();
					try
					{
						dbCommand.CommandText = "DELETE FROM device_voltage where id =" + l_id;
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.Transaction.Commit();
					DeviceOperation.ModifyUser4DeleteDevice((long)l_id, conn);
				}
			}
			catch (Exception ex)
			{
				try
				{
					dbCommand.Transaction.Rollback();
				}
				catch
				{
				}
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				dbCommand.Dispose();
			}
			return result;
		}
		public static int DeleteDeviceByID(int l_id)
		{
			int result = -1;
			DBConn dBConn = null;
			DBConn dBConn2 = null;
			DbCommand dbCommand = new OleDbCommand();
			DbCommand dbCommand2 = new OleDbCommand();
			"delete from device_base_info where id = " + l_id;
			try
			{
				dBConn = DBConnPool.getConnection();
				dBConn2 = DBConnPool.getDynaConnection();
				if (dBConn.con != null && dBConn2.con != null)
				{
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand2 = DBConn.GetCommandObject(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn2.con);
					dbCommand2.CommandType = CommandType.Text;
					dbCommand.CommandType = CommandType.Text;
					dbCommand2.Transaction = transaction;
					dbCommand.CommandText = "delete from device_auto_info where device_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from device_data_daily where device_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from device_data_hourly where device_id = " + l_id;
					dbCommand.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from device_sensor_info where device_id = " + l_id;
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "select id from bank_info where device_id = " + l_id;
					DbDataReader dbDataReader = dbCommand2.ExecuteReader();
					List<string> list = new List<string>();
					while (dbDataReader.Read())
					{
						long num = Convert.ToInt64(dbDataReader.GetValue(0));
						list.Add("delete from bank_auto_info where bank_id = " + num);
						list.Add("delete from bank_data_daily where bank_id = " + num);
						list.Add("delete from bank_data_hourly where bank_id = " + num);
					}
					dbDataReader.Close();
					foreach (string current in list)
					{
						dbCommand.CommandText = current;
						dbCommand.ExecuteNonQuery();
					}
					dbCommand2.CommandText = "delete from bank_info where device_id = " + l_id;
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "select id from port_info where device_id = " + l_id;
					DbDataReader dbDataReader2 = dbCommand2.ExecuteReader();
					List<string> list2 = new List<string>();
					List<string> list3 = new List<string>();
					while (dbDataReader2.Read())
					{
						long num2 = Convert.ToInt64(dbDataReader2.GetValue(0));
						list2.Add("delete from port_auto_info where port_id = " + num2);
						list2.Add("delete from port_data_daily where port_id = " + num2);
						list2.Add("delete from port_data_hourly where port_id = " + num2);
						list3.Add("delete from group_detail where grouptype = 'outlet' and dest_id = " + num2);
					}
					dbDataReader2.Close();
					foreach (string current2 in list2)
					{
						dbCommand.CommandText = current2;
						dbCommand.ExecuteNonQuery();
					}
					foreach (string current3 in list3)
					{
						dbCommand2.CommandText = current3;
						dbCommand2.ExecuteNonQuery();
					}
					DeviceOperation.ModifyUser4DeleteDevice((long)l_id, dBConn);
					dbCommand2.CommandText = "delete from port_info where device_id = " + l_id;
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from group_detail where grouptype = 'dev' and dest_id = " + l_id;
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from device_base_info where id = " + l_id;
					result = dbCommand2.ExecuteNonQuery();
					dbCommand2.Transaction.Commit();
					DBCacheStatus.Device = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Device"
					});
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbCommand2.Transaction.Rollback();
				}
				catch
				{
				}
			}
			finally
			{
				dbCommand2.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
				dbCommand.Dispose();
				if (dBConn2 != null)
				{
					dBConn2.close();
				}
			}
			return result;
		}
		public static void ModifyUser4DeleteDevice(long l_id, DBConn conn)
		{
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandText = "delete from udp where did = " + l_id;
					dbCommand.ExecuteNonQuery();
					DBCacheStatus.User = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_User"
					});
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
			}
		}
		public static void ModifyUser4DeleteDevice(long l_id)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = "delete from udp where did = " + l_id;
					dbCommand.ExecuteNonQuery();
					DBCacheStatus.User = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_User"
					});
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
		}
		public static List<PortInfo> getAllPort()
		{
			List<PortInfo> list = new List<PortInfo>();
			try
			{
				Hashtable portCache = DBCache.GetPortCache();
				if (portCache != null && portCache.Count > 0)
				{
					ICollection values = portCache.Values;
					foreach (PortInfo item in values)
					{
						list.Add(item);
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}
		public static List<PortInfo> getAllPortByDeviceID(int l_deviceid)
		{
			List<PortInfo> list = new List<PortInfo>();
			try
			{
				Hashtable devicePortMap = DBCache.GetDevicePortMap();
				Hashtable portCache = DBCache.GetPortCache();
				if (devicePortMap != null && devicePortMap.Count > 0 && portCache != null && portCache.Count > 0 && devicePortMap.ContainsKey(l_deviceid))
				{
					List<int> list2 = (List<int>)devicePortMap[l_deviceid];
					foreach (int current in list2)
					{
						if (portCache.ContainsKey(current))
						{
							list.Add((PortInfo)portCache[current]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				string arg_D2_0 = ex.Message;
			}
			return list;
		}
		public static List<SensorInfo> GetSensorInfo(int d_id)
		{
			List<SensorInfo> list = new List<SensorInfo>();
			try
			{
				Hashtable deviceSensorMap = DBCache.GetDeviceSensorMap();
				Hashtable sensorCache = DBCache.GetSensorCache();
				if (deviceSensorMap != null && deviceSensorMap.Count > 0 && sensorCache != null && sensorCache.Count > 0 && deviceSensorMap.ContainsKey(d_id))
				{
					List<int> list2 = (List<int>)deviceSensorMap[d_id];
					foreach (int current in list2)
					{
						if (sensorCache.ContainsKey(current))
						{
							list.Add((SensorInfo)sensorCache[current]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				string arg_D2_0 = ex.Message;
			}
			return list;
		}
		public static PortInfo GetPortInfoByID(int l_id)
		{
			try
			{
				Hashtable portCache = DBCache.GetPortCache();
				if (portCache != null && portCache.ContainsKey(l_id))
				{
					return (PortInfo)portCache[l_id];
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static BankInfo GetBankInfoByID(int l_id)
		{
			try
			{
				Hashtable bankCache = DBCache.GetBankCache();
				if (bankCache != null && bankCache.ContainsKey(l_id))
				{
					return (BankInfo)bankCache[l_id];
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static LineInfo GetLineInfoByID(int l_id)
		{
			try
			{
				Hashtable lineCache = DBCache.GetLineCache();
				if (lineCache != null && lineCache.ContainsKey(l_id))
				{
					return (LineInfo)lineCache[l_id];
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static float GetVoltage(string device_ids, string port_ids)
		{
			try
			{
				Hashtable deviceVoltage = DBCache.GetDeviceVoltage();
				if (deviceVoltage != null && deviceVoltage.Count > 0)
				{
					if (port_ids != null && port_ids.Length > 0)
					{
						Hashtable portCache = DBCache.GetPortCache();
						if (portCache != null && portCache.Count > 0)
						{
							string[] separator = new string[]
							{
								","
							};
							string[] array = port_ids.Split(separator, StringSplitOptions.RemoveEmptyEntries);
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string value = array2[i];
								int num = Convert.ToInt32(value);
								if (portCache.ContainsKey(num))
								{
									PortInfo portInfo = (PortInfo)portCache[num];
									int deviceID = portInfo.DeviceID;
									if (deviceVoltage.ContainsKey(deviceID))
									{
										float result = (float)deviceVoltage[deviceID];
										return result;
									}
								}
							}
						}
					}
					else
					{
						string[] separator2 = new string[]
						{
							","
						};
						string[] array3 = device_ids.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
						string[] array4 = array3;
						for (int j = 0; j < array4.Length; j++)
						{
							string value2 = array4[j];
							int num2 = Convert.ToInt32(value2);
							if (deviceVoltage.ContainsKey(num2))
							{
								float result = (float)deviceVoltage[num2];
								return result;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return 0f;
		}
		public static void UpdateDeviceVoltage(Dictionary<int, string> dv)
		{
			bool flag = false;
			string text = "";
			string str = "";
			foreach (KeyValuePair<int, string> current in dv)
			{
				int key = current.Key;
				text = text + key + ",";
				str = str + current.Value + ",";
			}
			if (text.EndsWith(","))
			{
				text = text.Substring(0, text.Length - 1);
			}
			DBConn dBConn = null;
			OleDbCommand oleDbCommand = new OleDbCommand();
			List<string> list = new List<string>();
			try
			{
				if (DBCacheStatus.b_occupa)
				{
					flag = true;
				}
				else
				{
					dBConn = DBConnPool.getConnection();
					if (dBConn.con != null)
					{
						Hashtable hashtable = new Hashtable();
						DataTable dataTable = new DataTable();
						try
						{
							if (dBConn != null && dBConn.con != null)
							{
								DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn.con);
								oleDbCommand = (OleDbCommand)dBConn.CreateCommand();
								oleDbCommand.CommandText = "select id from device_voltage ";
								dataAdapter.SelectCommand = oleDbCommand;
								dataAdapter.Fill(dataTable);
								dataAdapter.Dispose();
								if (dataTable != null)
								{
									hashtable = new Hashtable();
									foreach (DataRow dataRow in dataTable.Rows)
									{
										try
										{
											int num = Convert.ToInt32(dataRow[0]);
											if (!hashtable.ContainsKey(num))
											{
												hashtable.Add(num, num);
											}
										}
										catch
										{
										}
									}
								}
								dataTable = new DataTable();
							}
						}
						catch (Exception)
						{
						}
						Hashtable deviceCache = DBCache.GetDeviceCache(dBConn);
						oleDbCommand = (OleDbCommand)dBConn.CreateCommand();
						foreach (KeyValuePair<int, string> current2 in dv)
						{
							if (DBCacheStatus.b_occupa)
							{
								flag = true;
								break;
							}
							string value = current2.Value;
							try
							{
								Convert.ToDouble(value);
							}
							catch
							{
								continue;
							}
							if (value != null && value.Length > 0 && current2.Key > 0)
							{
								int key2 = current2.Key;
								if (deviceCache.ContainsKey(key2))
								{
									if (hashtable.ContainsKey(key2))
									{
										oleDbCommand.CommandText = string.Concat(new object[]
										{
											"update device_voltage set voltage = ",
											current2.Value,
											" where id = ",
											current2.Key
										});
									}
									else
									{
										oleDbCommand.CommandText = string.Concat(new object[]
										{
											"insert into device_voltage (id,voltage) values (",
											current2.Key,
											",",
											current2.Value,
											")"
										});
									}
									try
									{
										oleDbCommand.ExecuteNonQuery();
									}
									catch (Exception ex)
									{
										DebugCenter.GetInstance().appendToFile(ex.Message + "\r\n" + ex.StackTrace);
									}
								}
							}
						}
						if (hashtable.Count > deviceCache.Count)
						{
							ICollection keys = hashtable.Keys;
							foreach (int num2 in keys)
							{
								if (!deviceCache.ContainsKey(num2))
								{
									list.Add("delete from device_voltage where id = " + num2);
								}
							}
							foreach (string current3 in list)
							{
								if (DBCacheStatus.b_occupa)
								{
									flag = true;
									break;
								}
								try
								{
									oleDbCommand.CommandText = current3;
									oleDbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
							}
						}
						try
						{
							oleDbCommand.Dispose();
						}
						catch (Exception ex2)
						{
							DebugCenter.GetInstance().appendToFile(ex2.Message + "\r\n" + ex2.StackTrace);
						}
						try
						{
							dBConn.close();
						}
						catch
						{
						}
					}
				}
			}
			catch (Exception ex3)
			{
				DebugCenter.GetInstance().appendToFile(ex3.Message + "\r\n" + ex3.StackTrace);
				try
				{
					oleDbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					if (dBConn != null)
					{
						dBConn.close();
					}
				}
				catch
				{
				}
			}
			finally
			{
				if (flag)
				{
					DebugCenter.GetInstance().appendToFile("~~~~ UpdateDeviceVoltage yielded to threshold");
				}
				else
				{
					DeviceOperation.Last_Update_Voltage = DateTime.Now;
				}
				DBCacheStatus.DeviceVoltage = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_AP_DeviceVoltage"
				});
				try
				{
					oleDbCommand.Dispose();
				}
				catch (Exception ex4)
				{
					DebugCenter.GetInstance().appendToFile(ex4.Message + "\r\n" + ex4.StackTrace);
				}
				try
				{
					dBConn.close();
				}
				catch
				{
				}
			}
		}
		public static int UpdateDeviceDefine(DBConn conn, string str_model, string str_ver, string str_data1, string str_data2)
		{
			int result = -1;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (conn == null)
				{
					conn = DBConnPool.getConnection();
				}
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandText = string.Concat(new string[]
					{
						"update devicedefine set first_data = '",
						str_data1,
						"',second_data = '",
						str_data2,
						"' where model_nm = '",
						str_model,
						"' and dev_ver = '",
						str_ver,
						"' "
					});
					result = dbCommand.ExecuteNonQuery();
					dbCommand.Dispose();
				}
			}
			catch (DbException ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
			}
			return result;
		}
		public static int UpdateDeviceDefine(string str_model, string str_ver, string str_data1, string str_data2)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = string.Concat(new string[]
					{
						"update devicedefine set first_data = '",
						str_data1,
						"',second_data = '",
						str_data2,
						"' where model_nm = '",
						str_model,
						"' and dev_ver = '",
						str_ver,
						"' "
					});
					result = dbCommand.ExecuteNonQuery();
					dbCommand.Dispose();
					if (dBConn != null)
					{
						dBConn.close();
					}
				}
			}
			catch (DbException ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return result;
		}
		public static int InsertDeviceDefine(string str_model, string str_ver, string str_data1, string str_data2)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = string.Concat(new string[]
					{
						"insert into devicedefine (model_nm,dev_ver,first_data,second_data) values ('",
						str_model,
						"','",
						str_ver,
						"','",
						str_data1,
						"','",
						str_data2,
						"' )"
					});
					result = dbCommand.ExecuteNonQuery();
					dBConn.close();
				}
			}
			catch (DbException)
			{
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return result;
		}
		public static int InsertDeviceDefine(DBConn conn, string str_model, string str_ver, string str_data1, string str_data2)
		{
			int result = -1;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (conn == null)
				{
					conn = DBConnPool.getConnection();
				}
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandText = string.Concat(new string[]
					{
						"insert into devicedefine (model_nm,dev_ver,first_data,second_data) values ('",
						str_model,
						"','",
						str_ver,
						"','",
						str_data1,
						"','",
						str_data2,
						"' )"
					});
					result = dbCommand.ExecuteNonQuery();
				}
			}
			catch (DbException)
			{
				dbCommand.Dispose();
			}
			return result;
		}
		public static string[] GetDeviceDefine(string str_model, string str_ver)
		{
			string[] array = new string[]
			{
				"",
				""
			};
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = string.Concat(new string[]
					{
						"select first_data,second_data from devicedefine where model_nm = '",
						str_model,
						"' and dev_ver = '",
						str_ver,
						"' "
					});
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						array[0] = Convert.ToString(dbDataReader.GetValue(0));
						array[1] = Convert.ToString(dbDataReader.GetValue(1));
					}
					dbDataReader.Close();
					dBConn.close();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return array;
		}
		public static List<Dictionary<string, string>> GetDeviceDefine()
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = "select model_nm,dev_ver,first_data,second_data from devicedefine ";
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						string value = Convert.ToString(dbDataReader.GetValue(0));
						string value2 = Convert.ToString(dbDataReader.GetValue(1));
						string value3 = Convert.ToString(dbDataReader.GetValue(2));
						string value4 = Convert.ToString(dbDataReader.GetValue(3));
						dictionary.Add("modelname", value);
						dictionary.Add("version", value2);
						dictionary.Add("basic", value3);
						dictionary.Add("extra", value4);
						list.Add(dictionary);
					}
					dbDataReader.Close();
					dBConn.close();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return list;
		}
		public static int ResetRestoreFlag(DBConn conn, string str_devid)
		{
			DbCommand dbCommand = null;
			try
			{
				if (conn.con != null)
				{
					dbCommand = conn.con.CreateCommand();
					dbCommand.CommandText = "update device_base_info set restoreflag = 0 where id = " + str_devid;
					dbCommand.ExecuteNonQuery();
					dbCommand.Dispose();
					return 1;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
			}
			return -1;
		}
		public static int ResetRestoreFlag(string str_devid)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "update device_base_info set restoreflag = 0 where id = " + str_devid;
					dbCommand.ExecuteNonQuery();
					dbCommand.Dispose();
					dBConn.Close();
					return 1;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				dBConn.Close();
			}
			return -1;
		}
		public static int ResetRestoreFlag(string[] arr_devid)
		{
			if (arr_devid.Length < 1)
			{
				return 1;
			}
			string text = "";
			for (int i = 0; i < arr_devid.Length; i++)
			{
				string text2 = arr_devid[i];
				if (text2 != null && text2.Length >= 1)
				{
					text = text + text2 + ",";
				}
			}
			if (text.Length > 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (text.Length < 1)
			{
				return -1;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "update device_base_info set restoreflag = 0 where id in (" + text + " )";
					dbCommand.ExecuteNonQuery();
					dbCommand.Dispose();
					dBConn.Close();
					return 1;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				dBConn.Close();
			}
			return -1;
		}
		public static void InsertPower_M(Hashtable ht_power, DateTime DT_in, int i_table)
		{
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			DateTime arg_09_0 = DateTime.Now;
			try
			{
				mySqlConnection = new MySqlConnection("Database=eco;Data Source=127.0.0.1;Port=3306;User Id=root;Password=password;Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;");
				mySqlConnection.Open();
				if (mySqlConnection != null)
				{
					mySqlCommand = mySqlConnection.CreateCommand();
					mySqlCommand.Parameters.Clear();
					switch (i_table)
					{
					case 0:
						mySqlCommand.CommandText = "insert into device_auto_info" + DT_in.ToString("yyyyMMdd") + " (device_id,power,insert_time ) values( ?device_id,?power,?insert_time)";
						mySqlCommand.Parameters.Add("?device_id", MySqlDbType.Int32);
						break;
					case 1:
						mySqlCommand.CommandText = "insert into bank_auto_info" + DT_in.ToString("yyyyMMdd") + " (bank_id,power,insert_time ) values( ?bank_id,?power,?insert_time)";
						mySqlCommand.Parameters.Add("?bank_id", MySqlDbType.Int32);
						break;
					case 2:
						mySqlCommand.CommandText = "insert into port_auto_info" + DT_in.ToString("yyyyMMdd") + " (port_id,power,insert_time ) values( ?port_id,?power,?insert_time)";
						mySqlCommand.Parameters.Add("?port_id", MySqlDbType.Int32);
						break;
					}
					mySqlCommand.Parameters.Add("?power", MySqlDbType.Int64);
					mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.DateTime);
					mySqlCommand.Prepare();
					ICollection keys = ht_power.Keys;
					foreach (int num in keys)
					{
						try
						{
							mySqlCommand.Parameters[0].Value = num;
							double num2 = Convert.ToDouble(ht_power[num]);
							long num3 = Convert.ToInt64(num2 * 10000.0);
							mySqlCommand.Parameters[1].Value = num3;
							mySqlCommand.Parameters[2].Value = DT_in.ToString("yyyy-MM-dd HH:mm:ss");
							mySqlCommand.ExecuteNonQuery();
						}
						catch
						{
						}
					}
					if (mySqlCommand != null)
					{
						try
						{
							mySqlCommand.Dispose();
						}
						catch
						{
						}
					}
					if (mySqlConnection != null)
					{
						try
						{
							mySqlConnection.Close();
						}
						catch
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (mySqlCommand != null)
				{
					try
					{
						mySqlCommand.Dispose();
					}
					catch
					{
					}
				}
				if (mySqlConnection != null)
				{
					try
					{
						mySqlConnection.Close();
					}
					catch
					{
					}
				}
				Console.WriteLine(ex.Message);
			}
		}
		public static void UpdateDaily_M(Hashtable ht_pd, DateTime DT_in, int i_table)
		{
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			DateTime arg_09_0 = DateTime.Now;
			DataTable dataTable = new DataTable();
			MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
			Hashtable hashtable = new Hashtable();
			try
			{
				mySqlConnection = new MySqlConnection("Database=eco;Data Source=127.0.0.1;Port=3306;User Id=root;Password=password;Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;");
				mySqlConnection.Open();
				if (mySqlConnection != null)
				{
					mySqlCommand = mySqlConnection.CreateCommand();
					mySqlCommand.Parameters.Clear();
					switch (i_table)
					{
					case 0:
						mySqlCommand.CommandText = "select device_id from device_data_daily where insert_time = '" + DT_in.ToString("yyyy-MM-dd") + "' ";
						break;
					case 1:
						mySqlCommand.CommandText = "select device_id from device_data_hourly where insert_time = '" + DT_in.ToString("yyyy-MM-dd HH") + ":30:00' ";
						break;
					case 2:
						mySqlCommand.CommandText = "select bank_id from bank_data_daily where insert_time = '" + DT_in.ToString("yyyy-MM-dd") + "' ";
						break;
					case 3:
						mySqlCommand.CommandText = "select bank_id from bank_data_hourly where insert_time = '" + DT_in.ToString("yyyy-MM-dd HH") + ":30:00' ";
						break;
					case 4:
						mySqlCommand.CommandText = "select port_id from port_data_daily where insert_time = '" + DT_in.ToString("yyyy-MM-dd") + "' ";
						break;
					case 5:
						mySqlCommand.CommandText = "select port_id from port_data_hourly where insert_time = '" + DT_in.ToString("yyyy-MM-dd HH") + ":30:00' ";
						break;
					}
					mySqlDataAdapter.SelectCommand = mySqlCommand;
					mySqlDataAdapter.Fill(dataTable);
					mySqlDataAdapter.Dispose();
					if (dataTable != null)
					{
						foreach (DataRow dataRow in dataTable.Rows)
						{
							string text = Convert.ToString(dataRow[0]);
							if (!hashtable.ContainsKey(text))
							{
								hashtable.Add(text, text);
							}
						}
					}
					dataTable = new DataTable();
					mySqlCommand.Parameters.Clear();
					switch (i_table)
					{
					case 0:
						mySqlCommand.CommandText = "update device_data_daily set power_consumption = power_consumption + ?power_consumption where device_id = ?device_id and insert_time = ?insert_time ";
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?device_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.Date);
						break;
					case 1:
						mySqlCommand.CommandText = "update device_data_hourly set power_consumption = power_consumption + ?power_consumption where device_id = ?device_id and insert_time = ?insert_time ";
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?device_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.DateTime);
						break;
					case 2:
						mySqlCommand.CommandText = "update bank_data_daily set power_consumption = power_consumption + ?power_consumption where bank_id = ?bank_id and insert_time = ?insert_time ";
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?bank_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.Date);
						break;
					case 3:
						mySqlCommand.CommandText = "update bank_data_hourly set power_consumption = power_consumption + ?power_consumption where bank_id = ?bank_id and insert_time = ?insert_time ";
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?bank_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.DateTime);
						break;
					case 4:
						mySqlCommand.CommandText = "update port_data_daily set power_consumption = power_consumption + ?power_consumption where port_id = ?port_id and insert_time = ?insert_time ";
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?port_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.Date);
						break;
					case 5:
						mySqlCommand.CommandText = "update port_data_hourly set power_consumption = power_consumption + ?power_consumption where port_id = ?port_id and insert_time = ?insert_time ";
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?port_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.DateTime);
						break;
					}
					mySqlCommand.Prepare();
					ICollection keys = ht_pd.Keys;
					foreach (int num in keys)
					{
						try
						{
							double num2 = Convert.ToDouble(ht_pd[num]);
							long num3 = Convert.ToInt64(num2 * 10000.0);
							mySqlCommand.Parameters[0].Value = num3;
							mySqlCommand.Parameters[1].Value = num;
							if (i_table == 0 || i_table == 2 || i_table == 4)
							{
								mySqlCommand.Parameters[2].Value = DT_in.ToString("yyyy-MM-dd");
							}
							else
							{
								mySqlCommand.Parameters[2].Value = DT_in.ToString("yyyy-MM-dd HH") + ":30:00";
							}
							mySqlCommand.ExecuteNonQuery();
						}
						catch
						{
						}
					}
					mySqlCommand.Parameters.Clear();
					switch (i_table)
					{
					case 0:
						mySqlCommand.CommandText = "insert into device_data_daily (device_id,power_consumption,insert_time ) values ( ?device_id, ?power_consumption, ?insert_time )";
						mySqlCommand.Parameters.Add("?device_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.Date);
						break;
					case 1:
						mySqlCommand.CommandText = "insert into device_data_hourly (device_id,power_consumption,insert_time ) values ( ?device_id, ?power_consumption, ?insert_time )";
						mySqlCommand.Parameters.Add("?device_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.DateTime);
						break;
					case 2:
						mySqlCommand.CommandText = "insert into bank_data_daily (bank_id,power_consumption,insert_time ) values ( ?bank_id, ?power_consumption, ?insert_time )";
						mySqlCommand.Parameters.Add("?bank_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.Date);
						break;
					case 3:
						mySqlCommand.CommandText = "insert into bank_data_hourly (bank_id,power_consumption,insert_time ) values ( ?bank_id, ?power_consumption, ?insert_time )";
						mySqlCommand.Parameters.Add("?bank_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.DateTime);
						break;
					case 4:
						mySqlCommand.CommandText = "insert into port_data_daily (port_id,power_consumption,insert_time ) values ( ?port_id, ?power_consumption, ?insert_time )";
						mySqlCommand.Parameters.Add("?port_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.Date);
						break;
					case 5:
						mySqlCommand.CommandText = "insert into port_data_hourly (port_id,power_consumption,insert_time ) values ( ?port_id, ?power_consumption, ?insert_time )";
						mySqlCommand.Parameters.Add("?port_id", MySqlDbType.Int32);
						mySqlCommand.Parameters.Add("?power_consumption", MySqlDbType.Int64);
						mySqlCommand.Parameters.Add("?insert_time", MySqlDbType.DateTime);
						break;
					}
					mySqlCommand.Prepare();
					foreach (int num4 in keys)
					{
						if (!hashtable.ContainsKey(string.Concat(num4)))
						{
							try
							{
								double num5 = Convert.ToDouble(ht_pd[num4]);
								long num6 = Convert.ToInt64(num5 * 10000.0);
								mySqlCommand.Parameters[0].Value = num4;
								mySqlCommand.Parameters[1].Value = num6;
								if (i_table == 0 || i_table == 2 || i_table == 4)
								{
									mySqlCommand.Parameters[2].Value = DT_in.ToString("yyyy-MM-dd");
								}
								else
								{
									mySqlCommand.Parameters[2].Value = DT_in.ToString("yyyy-MM-dd HH") + ":30:00";
								}
								mySqlCommand.ExecuteNonQuery();
							}
							catch
							{
							}
						}
					}
					if (mySqlCommand != null)
					{
						try
						{
							mySqlCommand.Dispose();
						}
						catch
						{
						}
					}
					if (mySqlConnection != null)
					{
						try
						{
							mySqlConnection.Close();
						}
						catch
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (mySqlCommand != null)
				{
					try
					{
						mySqlCommand.Dispose();
					}
					catch
					{
					}
				}
				if (mySqlConnection != null)
				{
					try
					{
						mySqlConnection.Close();
					}
					catch
					{
					}
				}
				Console.WriteLine(ex.Message);
			}
		}
		public static int Delete15tabledata(bool b_index)
		{
			MySqlConnection mySqlConnection = null;
			try
			{
				mySqlConnection = new MySqlConnection("Database=eco_t7071er0;Data Source=127.0.0.1;Port=3306;User Id=root;Password=password;Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;");
				mySqlConnection.Open();
				MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
				DateTime dateTime = new DateTime(2011, 1, 1, 0, 0, 0);
				int i = 0;
				string text = "";
				while (i < 100000)
				{
					i += 1000;
					text = text + i + ",";
				}
				if (text.EndsWith(","))
				{
					text = text.Substring(0, text.Length - 1);
				}
				if (b_index)
				{
					dateTime = new DateTime(2011, 1, 11, 0, 0, 0);
				}
				DateTime now = DateTime.Now;
				for (int j = 0; j < 10; j++)
				{
					DateTime dateTime2 = dateTime.AddDays((double)j);
					mySqlCommand.CommandText = string.Concat(new string[]
					{
						"delete from port_min_power_",
						dateTime2.ToString("yyyyMMdd"),
						" where port_id in (",
						text,
						")"
					});
					DateTime now2 = DateTime.Now;
					mySqlCommand.ExecuteNonQuery();
					DebugCenter.GetInstance().appendToFile(dateTime2.ToString("yyyyMMdd") + "delete spent : " + (DateTime.Now - now2).TotalSeconds);
				}
				DebugCenter.GetInstance().appendToFile("total delete spent : " + (DateTime.Now - now).TotalSeconds);
				mySqlCommand.Dispose();
				mySqlConnection.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile(ex.Message);
				mySqlConnection.Close();
			}
			return -1;
		}
		public static void CleanupGP()
		{
			OleDbConnection oleDbConnection = null;
			bool flag = false;
			try
			{
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor");
				oleDbConnection.Open();
				OleDbCommand oleDbCommand = oleDbConnection.CreateCommand();
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
				oleDbDataAdapter.SelectCommand = oleDbCommand;
				DataTable dataTable = new DataTable();
				oleDbCommand.CommandText = "select id from port_info";
				oleDbDataAdapter.Fill(dataTable);
				Hashtable hashtable = new Hashtable();
				foreach (DataRow dataRow in dataTable.Rows)
				{
					int num = Convert.ToInt32(dataRow[0]);
					if (!hashtable.ContainsKey(num))
					{
						hashtable.Add(num, num);
					}
				}
				dataTable = new DataTable();
				oleDbDataAdapter.Dispose();
				oleDbCommand.CommandText = "select * from group_detail where grouptype = 'outlet' ";
				OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
				List<int> list = new List<int>();
				while (oleDbDataReader.Read())
				{
					int @int = oleDbDataReader.GetInt32(2);
					if (!hashtable.ContainsKey(@int))
					{
						list.Add(@int);
					}
				}
				oleDbDataReader.Close();
				foreach (int current in list)
				{
					oleDbCommand.CommandText = "delete from group_detail where grouptype = 'outlet' and  dest_id = " + current;
					oleDbCommand.ExecuteNonQuery();
					flag = true;
				}
				oleDbCommand.Dispose();
				oleDbConnection.Close();
				if (flag)
				{
					DBCacheStatus.Group = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_AP_Group"
					});
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile(ex.Message + "\r\n" + ex.StackTrace);
				try
				{
					oleDbConnection.Close();
				}
				catch
				{
				}
			}
		}
	}
}
