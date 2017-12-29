using CommonAPI;
using CommonAPI.CultureTransfer;
using CommonAPI.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace DBAccessAPI
{
	public class DeviceInfo
	{
		public delegate void DelegateOnDbUpdate(int type, object diffUpdate);
		public const int THRESHOLD_EMPTY = -500;
		public const int POPMode_Enable = 2;
		public const int POPMode_Disable = 1;
		public const int DOORSENSOR_NOTINSTALL = 0;
		public const int DOORSENSOR_PHOTO = 1;
		public const int DOORSENSOR_INDUCTIVE = 2;
		public const int DOORSENSOR_REED = 3;
		public const int PUSHFLAG_ENABLE = 1;
		public const int PUSHFLAG_DISABLE = 0;
		public const int DEVThresholdRefreshFLG_haveNew = 1;
		public const int DEVThresholdRefreshFLG_used = 2;
		private int i_bankcount;
		private int i_portcount;
		private int i_sensorcount;
		private float max_voltage;
		private float min_voltage;
		private float max_power_diss;
		private float min_power_diss;
		private float max_power;
		private float min_power;
		private float max_current;
		private float min_current;
		private int d_id;
		private string fw_version = "";
		private string d_name;
		private string d_ip = string.Empty;
		private string d_mac = string.Empty;
		private string d_ppwd = string.Empty;
		private string d_apwd = string.Empty;
		private string d_pp = string.Empty;
		private string d_ap = string.Empty;
		private string d_un = string.Empty;
		private string d_model = string.Empty;
		private int d_retry;
		private int d_timeout = 3;
		private int d_snmpver = 1;
		private int d_snmpport = 161;
		private long rack_id;
		private int popEnableMode;
		private int outletpop;
		private int lifopop;
		private int prioritypop;
		private float popThreshold;
		private int door;
		private float capacity;
		private int pushflag;
		private string b_priority = "";
		private float f_reference_voltage = -500f;
		public static DeviceInfo.DelegateOnDbUpdate cbOnDBUpdated = null;
		private static int i_dashboardflag = 0;
		private static object _dashboardlock = new object();
		public float ReferenceVoltage
		{
			get
			{
				return this.f_reference_voltage;
			}
			set
			{
				this.f_reference_voltage = value;
			}
		}
		public string Bank_Priority
		{
			get
			{
				return this.b_priority;
			}
			set
			{
				this.b_priority = value;
			}
		}
		public int DoorSensor
		{
			get
			{
				return this.door;
			}
			set
			{
				this.door = value;
			}
		}
		public int POPEnableMode
		{
			get
			{
				return this.popEnableMode;
			}
			set
			{
				this.popEnableMode = value;
			}
		}
		public int OutletPOPMode
		{
			get
			{
				return this.outletpop;
			}
			set
			{
				this.outletpop = value;
			}
		}
		public int BankPOPLIFOMode
		{
			get
			{
				return this.lifopop;
			}
			set
			{
				this.lifopop = value;
			}
		}
		public int BankPOPPriorityMode
		{
			get
			{
				return this.prioritypop;
			}
			set
			{
				this.prioritypop = value;
			}
		}
		public float POPThreshold
		{
			get
			{
				return this.popThreshold;
			}
			set
			{
				this.popThreshold = value;
			}
		}
		public float Capacity
		{
			get
			{
				return this.capacity;
			}
			set
			{
				this.capacity = value;
			}
		}
		public long RackID
		{
			get
			{
				return this.rack_id;
			}
			set
			{
				this.rack_id = value;
			}
		}
		public int DeviceID
		{
			get
			{
				return this.d_id;
			}
			set
			{
				this.d_id = value;
			}
		}
		public string FWVersion
		{
			get
			{
				return this.fw_version;
			}
			set
			{
				this.fw_version = value;
			}
		}
		public string DeviceName
		{
			get
			{
				return this.d_name;
			}
			set
			{
				this.d_name = value;
			}
		}
		public string DeviceIP
		{
			get
			{
				return this.d_ip;
			}
			set
			{
				this.d_ip = value;
			}
		}
		public string Mac
		{
			get
			{
				return this.d_mac;
			}
			set
			{
				this.d_mac = value;
			}
		}
		public string PrivacyPassword
		{
			get
			{
				return this.d_ppwd;
			}
			set
			{
				this.d_ppwd = value;
			}
		}
		public string AuthenPassword
		{
			get
			{
				return this.d_apwd;
			}
			set
			{
				this.d_apwd = value;
			}
		}
		public string Privacy
		{
			get
			{
				return this.d_pp;
			}
			set
			{
				this.d_pp = value;
			}
		}
		public string Authentication
		{
			get
			{
				return this.d_ap;
			}
			set
			{
				this.d_ap = value;
			}
		}
		public string Username
		{
			get
			{
				return this.d_un;
			}
			set
			{
				this.d_un = value;
			}
		}
		public string ModelNm
		{
			get
			{
				return this.d_model;
			}
			set
			{
				this.d_model = value;
			}
		}
		public int Timeout
		{
			get
			{
				return this.d_timeout;
			}
			set
			{
				this.d_timeout = value;
			}
		}
		public int BANK_COUNT
		{
			get
			{
				return this.i_bankcount;
			}
			set
			{
				this.i_bankcount = value;
			}
		}
		public int PORT_COUNT
		{
			get
			{
				return this.i_portcount;
			}
			set
			{
				this.i_portcount = value;
			}
		}
		public int SENSOR_COUNT
		{
			get
			{
				return this.i_sensorcount;
			}
			set
			{
				this.i_sensorcount = value;
			}
		}
		public int Retry
		{
			get
			{
				return this.d_retry;
			}
			set
			{
				this.d_retry = value;
			}
		}
		public int SnmpVersion
		{
			get
			{
				return this.d_snmpver;
			}
			set
			{
				this.d_snmpver = value;
			}
		}
		public int Port
		{
			get
			{
				return this.d_snmpport;
			}
			set
			{
				this.d_snmpport = value;
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
		public int PUSH_FLAG
		{
			get
			{
				return this.pushflag;
			}
			set
			{
				this.pushflag = value;
			}
		}
		public DeviceInfo()
		{
		}
		public DeviceInfo CloneDeviceInfo()
		{
			return new DeviceInfo(this.d_id, this.d_ip, this.d_name, this.d_mac, this.d_ppwd, this.d_apwd, this.d_pp, this.d_ap, this.d_timeout, this.d_retry, this.d_un, this.d_snmpport, this.d_snmpver, this.d_model, this.max_voltage, this.min_voltage, this.max_power_diss, this.min_power_diss, this.max_power, this.min_power, this.max_current, this.min_current, this.rack_id, this.fw_version, this.popEnableMode, this.popThreshold, this.door, this.capacity, this.outletpop, this.lifopop, this.prioritypop, this.b_priority, this.f_reference_voltage)
			{
				PUSH_FLAG = this.pushflag
			};
		}
		public DeviceInfo(int l_id, string str_ip, string str_name, string str_mac, string str_ppwd, string str_apwd, string str_pp, string str_ap, int i_timeout, int i_retry, string str_username, int i_snmp_port, int i_snmpver, string str_model, float f_max_v, float f_min_v, float f_max_pd, float f_min_pd, float f_max_p, float f_min_p, float f_max_c, float f_min_c, long l_rackid, string str_version, int i_popflag, float f_popthreshold, int i_door, float f_capacity, int i_outletpop, int i_poplifo, int i_poppriority, string str_b_priority, float f_ref_voltage)
		{
			this.d_id = l_id;
			this.d_ip = str_ip;
			this.d_name = str_name;
			this.d_mac = str_mac;
			this.d_ppwd = str_ppwd;
			this.d_pp = str_pp;
			this.d_apwd = str_apwd;
			this.d_ap = str_ap;
			this.d_timeout = i_timeout;
			this.d_retry = i_retry;
			this.d_un = str_username;
			this.d_snmpport = i_snmp_port;
			this.d_snmpver = i_snmpver;
			this.d_model = str_model;
			this.max_voltage = f_max_v;
			this.min_voltage = f_min_v;
			this.max_power_diss = f_max_pd;
			this.min_power_diss = f_min_pd;
			this.max_power = f_max_p;
			this.min_power = f_min_p;
			this.max_current = f_max_c;
			this.min_current = f_min_c;
			this.rack_id = l_rackid;
			this.fw_version = str_version;
			this.popEnableMode = i_popflag;
			this.popThreshold = f_popthreshold;
			this.door = i_door;
			this.capacity = f_capacity;
			this.pushflag = 0;
			this.outletpop = i_outletpop;
			this.lifopop = i_poplifo;
			this.prioritypop = i_poppriority;
			this.b_priority = str_b_priority;
			this.f_reference_voltage = f_ref_voltage;
		}
		public void CopyThreshold(DeviceInfo tmp_di)
		{
			this.max_voltage = tmp_di.Max_voltage;
			this.min_voltage = tmp_di.Min_voltage;
			this.max_power_diss = tmp_di.Max_power_diss;
			this.min_power_diss = tmp_di.Min_power_diss;
			this.max_power = tmp_di.Max_power;
			this.min_power = tmp_di.Min_power;
			this.max_current = tmp_di.Max_current;
			this.min_current = tmp_di.Min_current;
		}
		public DeviceInfo(DataRow tmp_dr_d)
		{
			if (tmp_dr_d != null)
			{
				this.d_id = Convert.ToInt32(tmp_dr_d["id"]);
				this.d_ip = Convert.ToString(tmp_dr_d["device_ip"]);
				this.d_name = Convert.ToString(tmp_dr_d["device_nm"]);
				this.d_mac = Convert.ToString(tmp_dr_d["mac"]);
				this.d_ppwd = Convert.ToString(tmp_dr_d["privacypw"]);
				this.d_pp = Convert.ToString(tmp_dr_d["privacy"]);
				this.d_apwd = Convert.ToString(tmp_dr_d["authenpw"]);
				this.d_ap = Convert.ToString(tmp_dr_d["authen"]);
				this.d_timeout = Convert.ToInt32(tmp_dr_d["timeout"]);
				this.d_retry = Convert.ToInt32(tmp_dr_d["retry"]);
				this.d_un = Convert.ToString(tmp_dr_d["user_name"]);
				this.d_snmpport = Convert.ToInt32(tmp_dr_d["portid"]);
				this.d_snmpver = Convert.ToInt32(tmp_dr_d["snmpVersion"]);
				this.d_model = Convert.ToString(tmp_dr_d["model_nm"]);
				this.max_voltage = Convert.ToSingle(tmp_dr_d["max_voltage"]);
				this.min_voltage = Convert.ToSingle(tmp_dr_d["min_voltage"]);
				this.max_power_diss = Convert.ToSingle(tmp_dr_d["max_power_diss"]);
				this.min_power_diss = Convert.ToSingle(tmp_dr_d["min_power_diss"]);
				this.max_power = Convert.ToSingle(tmp_dr_d["max_power"]);
				this.min_power = Convert.ToSingle(tmp_dr_d["min_power"]);
				this.max_current = Convert.ToSingle(tmp_dr_d["max_current"]);
				this.min_current = Convert.ToSingle(tmp_dr_d["min_current"]);
				this.rack_id = Convert.ToInt64(tmp_dr_d["rack_id"]);
				this.fw_version = Convert.ToString(tmp_dr_d["fw_version"]);
				try
				{
					object obj = tmp_dr_d["pop_flag"];
					if (obj != null && obj != DBNull.Value)
					{
						this.popEnableMode = MyConvert.ToInt32(tmp_dr_d["pop_flag"]);
					}
					else
					{
						this.popEnableMode = 1;
					}
				}
				catch (Exception)
				{
					this.popEnableMode = 1;
				}
				try
				{
					object obj2 = tmp_dr_d["pop_threshold"];
					if (obj2 != null && obj2 != DBNull.Value)
					{
						this.popThreshold = MyConvert.ToSingle(tmp_dr_d["pop_threshold"]);
					}
					else
					{
						this.popThreshold = -1f;
					}
				}
				catch (Exception)
				{
					this.popThreshold = -1f;
				}
				try
				{
					object obj3 = tmp_dr_d["door"];
					if (obj3 != null && obj3 != DBNull.Value)
					{
						this.door = MyConvert.ToInt32(tmp_dr_d["door"]);
					}
					else
					{
						this.door = 0;
					}
				}
				catch (Exception)
				{
					this.door = 0;
				}
				try
				{
					object obj4 = tmp_dr_d["device_capacity"];
					if (obj4 != null && obj4 != DBNull.Value)
					{
						this.capacity = MyConvert.ToSingle(tmp_dr_d["device_capacity"]);
					}
					else
					{
						this.capacity = 0f;
					}
				}
				catch (Exception)
				{
					this.capacity = 0f;
				}
				try
				{
					object obj5 = tmp_dr_d["restoreflag"];
					if (obj5 != null && obj5 != DBNull.Value)
					{
						this.pushflag = MyConvert.ToInt32(tmp_dr_d["restoreflag"]);
					}
					else
					{
						this.pushflag = 0;
					}
				}
				catch (Exception)
				{
					this.pushflag = 0;
				}
				try
				{
					object obj6 = tmp_dr_d["outlet_pop"];
					if (obj6 != null && obj6 != DBNull.Value)
					{
						this.outletpop = MyConvert.ToInt32(obj6);
					}
					else
					{
						this.outletpop = 1;
					}
				}
				catch (Exception)
				{
					this.outletpop = 1;
				}
				try
				{
					object obj7 = tmp_dr_d["pop_lifo"];
					if (obj7 != null && obj7 != DBNull.Value)
					{
						this.lifopop = MyConvert.ToInt32(obj7);
					}
					else
					{
						this.lifopop = 1;
					}
				}
				catch (Exception)
				{
					this.lifopop = 1;
				}
				try
				{
					object obj8 = tmp_dr_d["pop_priority"];
					if (obj8 != null && obj8 != DBNull.Value)
					{
						this.prioritypop = MyConvert.ToInt32(obj8);
					}
					else
					{
						this.prioritypop = 1;
					}
				}
				catch (Exception)
				{
					this.prioritypop = 1;
				}
				try
				{
					object obj9 = tmp_dr_d["b_priority"];
					if (obj9 != null && obj9 != DBNull.Value)
					{
						this.b_priority = Convert.ToString(obj9);
					}
					else
					{
						this.b_priority = "";
					}
				}
				catch (Exception)
				{
					this.b_priority = "";
				}
				try
				{
					object obj10 = tmp_dr_d["reference_voltage"];
					if (obj10 != null && obj10 != DBNull.Value)
					{
						this.f_reference_voltage = MyConvert.ToSingle(obj10);
					}
					else
					{
						this.f_reference_voltage = -300f;
					}
				}
				catch (Exception)
				{
					this.f_reference_voltage = -300f;
				}
			}
		}
		public List<PortInfo> GetPortInfo()
		{
			List<PortInfo> list = new List<PortInfo>();
			try
			{
				Hashtable devicePortMap = DBCache.GetDevicePortMap();
				Hashtable portCache = DBCache.GetPortCache();
				if (devicePortMap != null && devicePortMap.Count > 0 && portCache != null && portCache.Count > 0 && devicePortMap.ContainsKey(this.d_id))
				{
					List<int> list2 = (List<int>)devicePortMap[this.d_id];
					if (list2 != null)
					{
						foreach (int current in list2)
						{
							PortInfo portInfo = (PortInfo)portCache[current];
							if (portInfo != null)
							{
								list.Add(portInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public List<BankInfo> GetBankInfo()
		{
			List<BankInfo> list = new List<BankInfo>();
			try
			{
				Hashtable deviceBankMap = DBCache.GetDeviceBankMap();
				Hashtable bankCache = DBCache.GetBankCache();
				if (deviceBankMap != null && deviceBankMap.Count > 0 && bankCache != null && bankCache.Count > 0 && deviceBankMap.ContainsKey(this.d_id))
				{
					List<int> list2 = (List<int>)deviceBankMap[this.d_id];
					if (list2 != null)
					{
						foreach (int current in list2)
						{
							BankInfo bankInfo = (BankInfo)bankCache[current];
							if (bankInfo != null)
							{
								list.Add(bankInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public List<LineInfo> GetLineInfo()
		{
			List<LineInfo> list = new List<LineInfo>();
			try
			{
				Hashtable deviceLineMap = DBCache.GetDeviceLineMap();
				Hashtable lineCache = DBCache.GetLineCache();
				if (deviceLineMap != null && deviceLineMap.Count > 0 && lineCache != null && lineCache.Count > 0 && deviceLineMap.ContainsKey(this.d_id))
				{
					List<int> list2 = (List<int>)deviceLineMap[this.d_id];
					if (list2 != null)
					{
						foreach (int current in list2)
						{
							LineInfo lineInfo = (LineInfo)lineCache[current];
							if (lineInfo != null)
							{
								list.Add(lineInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public List<SensorInfo> GetSensorInfo()
		{
			List<SensorInfo> list = new List<SensorInfo>();
			try
			{
				Hashtable deviceSensorMap = DBCache.GetDeviceSensorMap();
				Hashtable sensorCache = DBCache.GetSensorCache();
				if (deviceSensorMap != null && deviceSensorMap.Count > 0 && sensorCache != null && sensorCache.Count > 0 && deviceSensorMap.ContainsKey(this.d_id))
				{
					List<int> list2 = (List<int>)deviceSensorMap[this.d_id];
					if (list2 != null)
					{
						foreach (int current in list2)
						{
							SensorInfo sensorInfo = (SensorInfo)sensorCache[current];
							if (sensorInfo != null)
							{
								list.Add(sensorInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public int GetCountInfo(ref int i_b, ref int i_p, ref int i_s)
		{
			int result = 0;
			i_b = 0;
			i_p = 0;
			i_s = 0;
			try
			{
				Hashtable deviceBankMap = DBCache.GetDeviceBankMap();
				if (deviceBankMap != null && deviceBankMap.Count > 0 && deviceBankMap.ContainsKey(this.d_id))
				{
					List<int> list = (List<int>)deviceBankMap[this.d_id];
					if (list != null)
					{
						i_b = list.Count;
					}
				}
				Hashtable devicePortMap = DBCache.GetDevicePortMap();
				if (devicePortMap != null && devicePortMap.Count > 0 && devicePortMap.ContainsKey(this.d_id))
				{
					List<int> list2 = (List<int>)devicePortMap[this.d_id];
					if (list2 != null)
					{
						i_p = list2.Count;
					}
				}
				List<SensorInfo> sensorInfo = DeviceOperation.GetSensorInfo(this.d_id);
				if (sensorInfo != null)
				{
					i_s = sensorInfo.Count;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public int UpdateDeviceThreshold(DBConn conn)
		{
			if (this.d_id < 1)
			{
				return -1;
			}
			DbCommand dbCommand = null;
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandType = CommandType.Text;
					string text = "update device_base_info set max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?  where id=" + this.d_id;
					text = "update device_base_info set ";
					if (this.d_name == null)
					{
						this.d_name = "";
					}
					text = text + " device_nm='" + this.d_name + "' ";
					text = text + ",max_voltage=" + CultureTransfer.ToString(this.max_voltage);
					text = text + ",min_voltage=" + CultureTransfer.ToString(this.min_voltage);
					text = text + ",max_power_diss=" + CultureTransfer.ToString(this.max_power_diss);
					text = text + ",min_power_diss=" + CultureTransfer.ToString(this.min_power_diss);
					text = text + ",max_power=" + CultureTransfer.ToString(this.max_power);
					text = text + ",min_power=" + CultureTransfer.ToString(this.min_power);
					text = text + ",max_current=" + CultureTransfer.ToString(this.max_current);
					text = text + ",min_current=" + CultureTransfer.ToString(this.min_current);
					if (this.fw_version == null)
					{
						this.fw_version = "";
					}
					text = text + ",fw_version='" + this.fw_version + "'";
					text = text + ",pop_flag=" + MyConvert.ToString(this.popEnableMode);
					text = text + ",pop_threshold=" + MyConvert.ToString(this.popThreshold);
					text = text + ",door=" + MyConvert.ToString(this.door);
					text = text + ",device_capacity=" + MyConvert.ToString(this.capacity);
					text = text + ",outlet_pop=" + Convert.ToString(this.outletpop);
					text = text + ",pop_lifo=" + Convert.ToString(this.lifopop);
					text = text + ",pop_priority=" + Convert.ToString(this.prioritypop);
					text = text + ",b_priority='" + this.b_priority + "' ";
					text = text + ",reference_voltage=" + MyConvert.ToString(this.f_reference_voltage);
					text = text + "  where id= " + this.d_id;
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
			if (this.d_id < 1)
			{
				return -1;
			}
			DbCommand dbCommand = null;
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						string commandText = "update device_base_info set device_ip=?device_ip,device_nm=?device_nm,mac=?mac,privacypw=?privacypw,authenpw=?authenpw,privacy=?privacy,authen=?authen,timeout=?timeout,retry=?retry,user_name=?user_name,portid=?portid,snmpVersion=?snmpVersion,model_nm=?model_nm,max_voltage=?max_voltage,min_voltage=?min_voltage,max_power_diss=?max_power_diss,min_power_diss=?min_power_diss,max_power=?max_power,min_power=?min_power,max_current=?max_current,min_current=?min_current,rack_id=?rack_id,fw_version=?fw_version,pop_flag=?pop_flag,pop_threshold=?pop_threshold,door=?door,device_capacity=?device_capacity,outlet_pop=?outlet_pop,pop_lifo=?pop_lifo,pop_priority=?pop_priority  where id=" + this.d_id;
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_ip", this.d_ip, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_nm", this.d_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?mac", this.d_mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacypw", this.d_ppwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authenpw", this.d_apwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacy", this.d_pp, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authen", this.d_ap, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?timeout", this.d_timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?retry", this.d_retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?user_name", this.d_un, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?portid", this.d_snmpport, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?snmpVersion", this.d_snmpver, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?model_nm", this.d_model, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?rack_id", this.rack_id, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?fw_version", this.fw_version, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_flag", this.popEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_threshold", this.popThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?door", this.door, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_capacity", this.capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?outlet_pop", this.outletpop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_lifo", this.lifopop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_priority", this.prioritypop, dbCommand));
					}
					else
					{
						string commandText = "update device_base_info set device_ip=?,device_nm=?,mac=?,privacypw=?,authenpw=?,privacy=?,authen=?,timeout=?,retry=?,user_name=?,portid=?,snmpVersion=?,model_nm=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?,rack_id=?,fw_version=?,pop_flag=?,pop_threshold=?,door=?,device_capacity=?,outlet_pop=?,pop_lifo=?,pop_priority=?  where id=" + this.d_id;
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_ip", this.d_ip, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_nm", this.d_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@mac", this.d_mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacypw", this.d_ppwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authenpw", this.d_apwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacy", this.d_pp, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authen", this.d_ap, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@timeout", this.d_timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@retry", this.d_retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@user_name", this.d_un, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@portid", this.d_snmpport, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@snmpVersion", this.d_snmpver, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@model_nm", this.d_model, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@rack_id", this.rack_id, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@fw_version", this.fw_version, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_flag", this.popEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_threshold", this.popThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@door", this.door, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_capacity", this.capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@outlet_pop", this.outletpop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_lifo", this.lifopop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_priority", this.prioritypop, dbCommand));
					}
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
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
			if (this.d_id < 1)
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
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						string commandText = "update device_base_info set device_ip=?device_ip,device_nm=?device_nm,mac=?mac,privacypw=?privacypw,authenpw=?authenpw,privacy=?privacy,authen=?authen,timeout=?timeout,retry=?retry,user_name=?user_name,portid=?portid,snmpVersion=?snmpVersion,model_nm=?model_nm,max_voltage=?max_voltage,min_voltage=?min_voltage,max_power_diss=?max_power_diss,min_power_diss=?min_power_diss,max_power=?max_power,min_power=?min_power,max_current=?max_current,min_current=?min_current,rack_id=?rack_id,fw_version=?fw_version,pop_flag=?pop_flag,pop_threshold=?pop_threshold,door=?door,device_capacity=?device_capacity,outlet_pop=?outlet_pop,pop_lifo=?pop_lifo,pop_priority=?pop_priority  where id=" + this.d_id;
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_ip", this.d_ip, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_nm", this.d_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?mac", this.d_mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacypw", this.d_ppwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authenpw", this.d_apwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?privacy", this.d_pp, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?authen", this.d_ap, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?timeout", this.d_timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?retry", this.d_retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?user_name", this.d_un, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?portid", this.d_snmpport, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?snmpVersion", this.d_snmpver, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?model_nm", this.d_model, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?rack_id", this.rack_id, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?fw_version", this.fw_version, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_flag", this.popEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_threshold", this.popThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?door", this.door, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?device_capacity", this.capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?outlet_pop", this.outletpop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_lifo", this.lifopop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?pop_priority", this.prioritypop, dbCommand));
					}
					else
					{
						string commandText = "update device_base_info set device_ip=?,device_nm=?,mac=?,privacypw=?,authenpw=?,privacy=?,authen=?,timeout=?,retry=?,user_name=?,portid=?,snmpVersion=?,model_nm=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?,rack_id=?,fw_version=?,pop_flag=?,pop_threshold=?,door=?,device_capacity=?,outlet_pop=?,pop_lifo=?,pop_priority=?,b_priority=?,reference_voltage=?  where id=" + this.d_id;
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_ip", this.d_ip, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_nm", this.d_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@mac", this.d_mac, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacypw", this.d_ppwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authenpw", this.d_apwd, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@privacy", this.d_pp, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@authen", this.d_ap, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@timeout", this.d_timeout, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@retry", this.d_retry, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@user_name", this.d_un, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@portid", this.d_snmpport, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@snmpVersion", this.d_snmpver, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@model_nm", this.d_model, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@rack_id", this.rack_id, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@fw_version", this.fw_version, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_flag", this.popEnableMode, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_threshold", this.popThreshold, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@door", this.door, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@device_capacity", this.capacity, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@outlet_pop", this.outletpop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_lifo", this.lifopop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@pop_priority", this.prioritypop, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@b_priority", this.b_priority, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@reference_voltage", this.f_reference_voltage, dbCommand));
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
				Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
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
		public static void UpdateRefreshThresholdFlag(int b_flag, object diffUpdate = null)
		{
			DBCacheStatus.Device = true;
			DBCacheStatus.DBSyncEventSet(true, new string[]
			{
				"DBSyncEventName_AP_Device"
			});
			if (DeviceInfo.cbOnDBUpdated != null)
			{
				DeviceInfo.cbOnDBUpdated(b_flag, diffUpdate);
			}
		}
		public static int GetRefreshFlag()
		{
			int result;
			lock (DeviceInfo._dashboardlock)
			{
				result = DeviceInfo.i_dashboardflag;
			}
			return result;
		}
		public static void SetRefreshFlag(int i_value)
		{
			lock (DeviceInfo._dashboardlock)
			{
				DeviceInfo.i_dashboardflag = i_value;
			}
		}
	}
}
