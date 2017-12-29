using CommonAPI;
using CommonAPI.CultureTransfer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class PortInfo
	{
		public const int SHUTDOWN_KILLPOWER = 1;
		public const int SHUTDOWN_WAKEONLAN = 2;
		public const int SHUTDOWN_ACBACK = 3;
		private int id;
		private int device_id;
		private int port_num;
		private string port_nm = string.Empty;
		private float max_voltage;
		private float min_voltage;
		private float max_power_diss;
		private float min_power_diss;
		private float max_power;
		private float min_power;
		private float max_current;
		private float min_current;
		private int port_confirmation;
		private int port_ondelay_time;
		private int port_offdelay_time;
		private int shutdown_method;
		private string port_mac = "";
		public string OutletMAC
		{
			get
			{
				return this.port_mac;
			}
			set
			{
				this.port_mac = value;
			}
		}
		public int OutletConfirmation
		{
			get
			{
				return this.port_confirmation;
			}
			set
			{
				this.port_confirmation = value;
			}
		}
		public int OutletOnDelayTime
		{
			get
			{
				return this.port_ondelay_time;
			}
			set
			{
				this.port_ondelay_time = value;
			}
		}
		public int OutletOffDelayTime
		{
			get
			{
				return this.port_offdelay_time;
			}
			set
			{
				this.port_offdelay_time = value;
			}
		}
		public int OutletShutdownMethod
		{
			get
			{
				return this.shutdown_method;
			}
			set
			{
				this.shutdown_method = value;
			}
		}
		public float Min_current
		{
			get
			{
				return this.min_current;
			}
			set
			{
				this.min_current = value;
			}
		}
		public float Max_current
		{
			get
			{
				return this.max_current;
			}
			set
			{
				this.max_current = value;
			}
		}
		public float Min_power
		{
			get
			{
				return this.min_power;
			}
			set
			{
				this.min_power = value;
			}
		}
		public float Max_power
		{
			get
			{
				return this.max_power;
			}
			set
			{
				this.max_power = value;
			}
		}
		public float Max_power_diss
		{
			get
			{
				return this.max_power_diss;
			}
			set
			{
				this.max_power_diss = value;
			}
		}
		public float Min_power_diss
		{
			get
			{
				return this.min_power_diss;
			}
			set
			{
				this.min_power_diss = value;
			}
		}
		public float Max_voltage
		{
			get
			{
				return this.max_voltage;
			}
			set
			{
				this.max_voltage = value;
			}
		}
		public float Min_voltage
		{
			get
			{
				return this.min_voltage;
			}
			set
			{
				this.min_voltage = value;
			}
		}
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public int DeviceID
		{
			get
			{
				return this.device_id;
			}
			set
			{
				this.device_id = value;
			}
		}
		public string PortName
		{
			get
			{
				return this.port_nm;
			}
			set
			{
				this.port_nm = value;
			}
		}
		public int PortNum
		{
			get
			{
				return this.port_num;
			}
			set
			{
				this.port_num = value;
			}
		}
		public PortInfo()
		{
		}
		public PortInfo(int l_id, int l_did, int i_portnum, string str_name, float f_max_v, float f_min_v, float f_max_pd, float f_min_pd, float f_max_p, float f_min_p, float f_max_c, float f_min_c, int i_confirm, int i_ondelay, int i_offdelay, int i_shutdown, string MAC)
		{
			this.id = l_id;
			this.device_id = l_did;
			this.port_num = i_portnum;
			this.port_nm = str_name;
			this.max_voltage = f_max_v;
			this.min_voltage = f_min_v;
			this.max_power_diss = f_max_pd;
			this.min_power_diss = f_min_pd;
			this.max_power = f_max_p;
			this.min_power = f_min_p;
			this.max_current = f_max_c;
			this.min_current = f_min_c;
			this.port_confirmation = i_confirm;
			this.port_ondelay_time = i_ondelay;
			this.port_offdelay_time = i_offdelay;
			this.shutdown_method = i_shutdown;
			this.port_mac = MAC;
		}
		public int UpdatePortThreshold(DBConn conn)
		{
			if (this.id < 1)
			{
				return -1;
			}
			if (this.device_id < 1)
			{
				return -1;
			}
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandType = CommandType.Text;
					string text = "update port_info set port_nm=?,port_confirmation=?,port_ondelay_time=?,port_offdelay_time=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?,shutdown_method=?,mac=? where id=" + this.id;
					text = "update port_info set ";
					text = text + "port_nm='" + this.port_nm + "'";
					text = text + ",port_confirmation=" + CultureTransfer.ToString(this.port_confirmation);
					text = text + ",port_ondelay_time=" + CultureTransfer.ToString(this.port_ondelay_time);
					text = text + ",port_offdelay_time=" + CultureTransfer.ToString(this.port_offdelay_time);
					text = text + ",max_voltage=" + CultureTransfer.ToString(this.max_voltage);
					text = text + ",min_voltage=" + CultureTransfer.ToString(this.min_voltage);
					text = text + ",max_power_diss=" + CultureTransfer.ToString(this.max_power_diss);
					text = text + ",min_power_diss=" + CultureTransfer.ToString(this.min_power_diss);
					text = text + ",max_power=" + CultureTransfer.ToString(this.max_power);
					text = text + ",min_power=" + CultureTransfer.ToString(this.min_power);
					text = text + ",max_current=" + CultureTransfer.ToString(this.max_current);
					text = text + ",min_current=" + CultureTransfer.ToString(this.min_current);
					text = text + ",shutdown_method=" + CultureTransfer.ToString(this.shutdown_method);
					text = text + ",mac='" + this.port_mac + "'";
					text = text + "  where id= " + this.id;
					dbCommand.CommandText = text;
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					dbCommand.Dispose();
					return result;
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
		public int Update(DBConn conn)
		{
			if (this.id < 1)
			{
				return -1;
			}
			if (this.device_id < 1)
			{
				return -1;
			}
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						string commandText = "update port_info set port_nm=?port_nm,port_confirmation=?port_confirmation,port_ondelay_time=?port_ondelay_time,port_offdelay_time=?port_offdelay_time,max_voltage=?max_voltage,min_voltage=?min_voltage,max_power_diss=?max_power_diss,min_power_diss=?min_power_diss,max_power=?max_power,min_power=?min_power,max_current=?max_current,min_current=?min_current,shutdown_method=?shutdown_method ,mac=?mac where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_nm", this.port_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_confirmation", this.port_confirmation, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_ondelay_time", this.port_ondelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_offdelay_time", this.port_offdelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?shutdown_method", this.shutdown_method, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?mac", this.port_mac, dbCommand));
					}
					else
					{
						string commandText = "update port_info set port_nm=?,port_confirmation=?,port_ondelay_time=?,port_offdelay_time=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?,shutdown_method=? ,mac=? where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_nm", this.port_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_confirmation", this.port_confirmation, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_ondelay_time", this.port_ondelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_offdelay_time", this.port_offdelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@shutdown_method", this.shutdown_method, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@mac", this.port_mac, dbCommand));
					}
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					return result;
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
			return -1;
		}
		public int Update()
		{
			if (this.id < 1)
			{
				return -1;
			}
			if (this.device_id < 1)
			{
				return -1;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						string commandText = "update port_info set port_nm=?port_nm,port_confirmation=?port_confirmation,port_ondelay_time=?port_ondelay_time,port_offdelay_time=?port_offdelay_time,max_voltage=?max_voltage,min_voltage=?min_voltage,max_power_diss=?max_power_diss,min_power_diss=?min_power_diss,max_power=?max_power,min_power=?min_power,max_current=?max_current,min_current=?min_current,shutdown_method=?shutdown_method ,mac=?mac where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_nm", this.port_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_confirmation", this.port_confirmation, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_ondelay_time", this.port_ondelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_offdelay_time", this.port_offdelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?shutdown_method", this.shutdown_method, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?mac", this.port_mac, dbCommand));
					}
					else
					{
						string commandText = "update port_info set port_nm=?,port_confirmation=?,port_ondelay_time=?,port_offdelay_time=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?,shutdown_method=? ,mac=? where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_nm", this.port_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_confirmation", this.port_confirmation, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_ondelay_time", this.port_ondelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@port_offdelay_time", this.port_offdelay_time, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@shutdown_method", this.shutdown_method, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@mac", this.port_mac, dbCommand));
					}
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					DBCacheStatus.Device = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Device"
					});
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return -1;
		}
		public PortInfo(int l_did, int i_portnum)
		{
			try
			{
				Hashtable devicePortMap = DBCache.GetDevicePortMap();
				Hashtable portCache = DBCache.GetPortCache();
				if (devicePortMap != null && devicePortMap.Count > 0 && portCache != null && portCache.Count > 0 && devicePortMap.ContainsKey(l_did))
				{
					List<int> list = (List<int>)devicePortMap[l_did];
					if (list != null)
					{
						foreach (int current in list)
						{
							PortInfo portInfo = (PortInfo)portCache[current];
							if (portInfo != null && portInfo.PortNum == i_portnum)
							{
								this.id = portInfo.ID;
								this.device_id = portInfo.DeviceID;
								this.port_num = portInfo.PortNum;
								this.port_nm = portInfo.PortName;
								this.max_voltage = portInfo.Max_voltage;
								this.min_voltage = portInfo.Min_voltage;
								this.max_power_diss = portInfo.Max_power_diss;
								this.min_power_diss = portInfo.Min_power_diss;
								this.max_power = portInfo.Max_power;
								this.min_power = portInfo.Min_power;
								this.max_current = portInfo.Max_current;
								this.min_current = portInfo.Min_current;
								this.port_confirmation = portInfo.OutletConfirmation;
								this.port_ondelay_time = portInfo.OutletOnDelayTime;
								this.port_offdelay_time = portInfo.OutletOffDelayTime;
								this.shutdown_method = portInfo.OutletShutdownMethod;
								try
								{
									this.port_mac = portInfo.OutletMAC;
								}
								catch
								{
									this.port_mac = "";
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public PortInfo(DataRow tmp_dr_p)
		{
			if (tmp_dr_p != null)
			{
				this.id = Convert.ToInt32(tmp_dr_p["id"]);
				this.device_id = Convert.ToInt32(tmp_dr_p["device_id"]);
				this.port_num = Convert.ToInt32(tmp_dr_p["port_num"]);
				this.port_nm = Convert.ToString(tmp_dr_p["port_nm"]);
				this.port_mac = Convert.ToString(tmp_dr_p["mac"]);
				this.max_voltage = Convert.ToSingle(tmp_dr_p["max_voltage"]);
				this.min_voltage = Convert.ToSingle(tmp_dr_p["min_voltage"]);
				this.max_power_diss = Convert.ToSingle(tmp_dr_p["max_power_diss"]);
				this.min_power_diss = Convert.ToSingle(tmp_dr_p["min_power_diss"]);
				this.max_power = Convert.ToSingle(tmp_dr_p["max_power"]);
				this.min_power = Convert.ToSingle(tmp_dr_p["min_power"]);
				this.max_current = Convert.ToSingle(tmp_dr_p["max_current"]);
				this.min_current = Convert.ToSingle(tmp_dr_p["min_current"]);
				this.port_confirmation = Convert.ToInt32(tmp_dr_p["port_confirmation"]);
				this.port_ondelay_time = Convert.ToInt32(tmp_dr_p["port_ondelay_time"]);
				this.port_offdelay_time = Convert.ToInt32(tmp_dr_p["port_offdelay_time"]);
				this.shutdown_method = Convert.ToInt32(tmp_dr_p["shutdown_method"]);
				try
				{
					this.port_mac = Convert.ToString(tmp_dr_p["mac"]);
				}
				catch
				{
					this.port_mac = "";
				}
			}
		}
	}
}
