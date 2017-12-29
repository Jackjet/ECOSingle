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
	public class SensorInfo
	{
		public const int SENSOR_TYPE_INTAKE = 0;
		public const int SENSOR_TYPE_EXHAUST = 1;
		public const int SENSOR_TYPE_FLOOR = 2;
		private int id;
		private int device_id;
		private float max_humidity;
		private float min_humidity;
		private float max_temperature;
		private float min_temperature;
		private float max_press;
		private float min_press;
		private string sensor_nm = string.Empty;
		private int sensor_locationtype;
		private int sensor_type;
		public string SensorName
		{
			get
			{
				return this.sensor_nm;
			}
			set
			{
				this.sensor_nm = value;
			}
		}
		public int Location
		{
			get
			{
				return this.sensor_locationtype;
			}
			set
			{
				this.sensor_locationtype = value;
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
		public int Device_ID
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
		public int Type
		{
			get
			{
				return this.sensor_type;
			}
			set
			{
				this.sensor_type = value;
			}
		}
		public float Min_press
		{
			get
			{
				return this.min_press;
			}
			set
			{
				this.min_press = value;
			}
		}
		public float Max_press
		{
			get
			{
				return this.max_press;
			}
			set
			{
				this.max_press = value;
			}
		}
		public float Min_temperature
		{
			get
			{
				return this.min_temperature;
			}
			set
			{
				this.min_temperature = value;
			}
		}
		public float Max_temperature
		{
			get
			{
				return this.max_temperature;
			}
			set
			{
				this.max_temperature = value;
			}
		}
		public float Min_humidity
		{
			get
			{
				return this.min_humidity;
			}
			set
			{
				this.min_humidity = value;
			}
		}
		public float Max_humidity
		{
			get
			{
				return this.max_humidity;
			}
			set
			{
				this.max_humidity = value;
			}
		}
		public SensorInfo(int l_did, int i_sensortype)
		{
			try
			{
				Hashtable deviceSensorMap = DBCache.GetDeviceSensorMap();
				Hashtable sensorCache = DBCache.GetSensorCache();
				if (deviceSensorMap != null && deviceSensorMap.Count > 0 && sensorCache != null && sensorCache.Count > 0 && deviceSensorMap.ContainsKey(l_did))
				{
					List<int> list = (List<int>)deviceSensorMap[l_did];
					if (list != null)
					{
						foreach (int current in list)
						{
							SensorInfo sensorInfo = (SensorInfo)sensorCache[current];
							if (sensorInfo != null && sensorInfo.Type == i_sensortype)
							{
								this.id = sensorInfo.ID;
								this.device_id = sensorInfo.Device_ID;
								this.sensor_nm = sensorInfo.SensorName;
								this.sensor_locationtype = sensorInfo.Location;
								this.sensor_type = sensorInfo.Type;
								this.max_humidity = sensorInfo.Max_humidity;
								this.min_humidity = sensorInfo.Min_humidity;
								this.max_temperature = sensorInfo.Max_temperature;
								this.min_temperature = sensorInfo.Min_temperature;
								this.max_press = sensorInfo.Max_press;
								this.min_press = sensorInfo.Min_press;
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
		public void CopyThreshold(SensorInfo tmp_si)
		{
			this.max_humidity = tmp_si.Max_humidity;
			this.min_humidity = tmp_si.Min_humidity;
			this.max_temperature = tmp_si.Max_temperature;
			this.min_temperature = tmp_si.Min_temperature;
			this.max_press = tmp_si.Max_press;
			this.min_press = tmp_si.Min_press;
		}
		public SensorInfo(int l_id, int l_did, string str_name, int i_location, int i_sensortype, float f_max_h, float f_min_h, float f_max_t, float f_min_t, float f_max_p, float f_min_p)
		{
			this.id = l_id;
			this.device_id = l_did;
			this.sensor_nm = str_name;
			this.sensor_locationtype = i_location;
			this.sensor_type = i_sensortype;
			this.max_humidity = f_max_h;
			this.min_humidity = f_min_h;
			this.max_temperature = f_max_t;
			this.min_temperature = f_min_t;
			this.max_press = f_max_p;
			this.min_press = f_min_p;
		}
		public int UpdateSensorThreshold(DBConn conn)
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
					string text = "update device_sensor_info set max_humidity=?,min_humidity=?,max_temperature=?,min_temperature=?,max_press=?,min_press=?  where id=" + this.id;
					text = "update device_sensor_info set ";
					text = text + "max_humidity=" + CultureTransfer.ToString(this.max_humidity);
					text = text + ",min_humidity=" + CultureTransfer.ToString(this.min_humidity);
					text = text + ",max_temperature=" + CultureTransfer.ToString(this.max_temperature);
					text = text + ",min_temperature=" + CultureTransfer.ToString(this.min_temperature);
					text = text + ",max_press=" + CultureTransfer.ToString(this.max_press);
					text = text + ",min_press=" + CultureTransfer.ToString(this.min_press);
					text = text + "  where id= " + this.id;
					dbCommand.CommandText = text;
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
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
						string commandText = "update device_sensor_info set sensor_nm=?sensor_nm,max_humidity=?max_humidity,min_humidity=?min_humidity,max_temperature=?max_temperature,min_temperature=?min_temperature,max_press=?max_press,min_press=?min_press,sensor_type=?sensor_type,location_type=?location_type  where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_nm", this.sensor_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_humidity", this.max_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_humidity", this.min_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_temperature", this.max_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_temperature", this.min_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_press", this.max_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_press", this.min_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_type", this.sensor_type, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?location_type", this.sensor_locationtype, dbCommand));
					}
					else
					{
						string commandText = "update device_sensor_info set sensor_nm=?,max_humidity=?,min_humidity=?,max_temperature=?,min_temperature=?,max_press=?,min_press=?,sensor_type=?,location_type=?  where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_nm", this.sensor_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_humidity", this.max_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_humidity", this.min_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_temperature", this.max_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_temperature", this.min_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_press", this.max_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_press", this.min_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_type", this.sensor_type, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@location_type", this.sensor_locationtype, dbCommand));
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
						string commandText = "update device_sensor_info set sensor_nm=?sensor_nm,max_humidity=?max_humidity,min_humidity=?min_humidity,max_temperature=?max_temperature,min_temperature=?min_temperature,max_press=?max_press,min_press=?min_press,sensor_type=?sensor_type,location_type=?location_type  where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_nm", this.sensor_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_humidity", this.max_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_humidity", this.min_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_temperature", this.max_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_temperature", this.min_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_press", this.max_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_press", this.min_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sensor_type", this.sensor_type, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?location_type", this.sensor_locationtype, dbCommand));
					}
					else
					{
						string commandText = "update device_sensor_info set sensor_nm=?,max_humidity=?,min_humidity=?,max_temperature=?,min_temperature=?,max_press=?,min_press=?,sensor_type=?,location_type=?  where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_nm", this.sensor_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_humidity", this.max_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_humidity", this.min_humidity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_temperature", this.max_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_temperature", this.min_temperature, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_press", this.max_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_press", this.min_press, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sensor_type", this.sensor_type, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@location_type", this.sensor_locationtype, dbCommand));
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
		public SensorInfo(DataRow tmp_dr_s)
		{
			if (tmp_dr_s != null)
			{
				this.id = Convert.ToInt32(tmp_dr_s["id"]);
				this.device_id = Convert.ToInt32(tmp_dr_s["device_id"]);
				this.sensor_nm = Convert.ToString(tmp_dr_s["sensor_nm"]);
				this.sensor_locationtype = Convert.ToInt32(tmp_dr_s["location_type"]);
				this.sensor_type = Convert.ToInt32(tmp_dr_s["sensor_type"]);
				this.max_humidity = Convert.ToSingle(tmp_dr_s["max_humidity"]);
				this.min_humidity = Convert.ToSingle(tmp_dr_s["min_humidity"]);
				this.max_temperature = Convert.ToSingle(tmp_dr_s["max_temperature"]);
				this.min_temperature = Convert.ToSingle(tmp_dr_s["min_temperature"]);
				this.max_press = Convert.ToSingle(tmp_dr_s["max_press"]);
				this.min_press = Convert.ToSingle(tmp_dr_s["min_press"]);
			}
		}
	}
}
