using CommonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;
namespace DBAccessAPI
{
	public class RackInfo
	{
		public struct RackThermalBitMap
		{
			public bool b_intake;
			public bool b_exhaust;
			public bool b_difference;
		}
		private static DateTime dt_last_thermal = new DateTime(1990, 1, 1, 0, 0, 0);
		private static object _lock_thermal = new object();
		private long r_id;
		private string r_name = string.Empty;
		private long[] z_rackarr;
		private int r_s_x;
		private int r_s_y;
		private int r_e_x;
		private int r_e_y;
		private string ids_device;
		private string r_reserve = "";
		private int i_namestyle = -1;
		public static DateTime Last_Update_Thermal
		{
			get
			{
				DateTime result;
				lock (RackInfo._lock_thermal)
				{
					result = RackInfo.dt_last_thermal;
				}
				return result;
			}
			set
			{
				lock (RackInfo._lock_thermal)
				{
					RackInfo.dt_last_thermal = value;
				}
			}
		}
		public long RackID
		{
			get
			{
				return this.r_id;
			}
			set
			{
				this.r_id = value;
			}
		}
		public string RackName
		{
			get
			{
				if (this.i_namestyle > -1)
				{
					if (this.i_namestyle == 1)
					{
						if (this.r_reserve != null && this.r_reserve.Length != 0)
						{
							return this.r_reserve;
						}
						if (this.r_name != null)
						{
							return this.r_name;
						}
						return "";
					}
					else
					{
						if (this.r_name != null)
						{
							return this.r_name;
						}
						return "";
					}
				}
				else
				{
					if (Sys_Para.GetRackFullNameflag() == 1)
					{
						if (this.r_reserve != null && this.r_reserve.Length != 0)
						{
							return this.r_reserve;
						}
						if (this.r_name != null)
						{
							return this.r_name;
						}
						return "";
					}
					else
					{
						if (this.r_name != null)
						{
							return this.r_name;
						}
						return "";
					}
				}
			}
		}
		public string RackFullName
		{
			get
			{
				return this.r_reserve;
			}
			set
			{
				this.r_reserve = value;
			}
		}
		public string OriginalName
		{
			get
			{
				return this.r_name;
			}
			set
			{
				this.r_name = value;
			}
		}
		public int StartPoint_X
		{
			get
			{
				return this.r_s_x;
			}
			set
			{
				this.r_s_x = value;
			}
		}
		public int StartPoint_Y
		{
			get
			{
				return this.r_s_y;
			}
			set
			{
				this.r_s_y = value;
			}
		}
		public int EndPoint_X
		{
			get
			{
				return this.r_e_x;
			}
			set
			{
				this.r_e_x = value;
			}
		}
		public int EndPoint_Y
		{
			get
			{
				return this.r_e_y;
			}
			set
			{
				this.r_e_y = value;
			}
		}
		public string DeviceInfo
		{
			get
			{
				if (this.ids_device == null)
				{
					return "";
				}
				return this.ids_device;
			}
			set
			{
				this.ids_device = value;
			}
		}
		public void setNameFlag(int i_f)
		{
			this.i_namestyle = i_f;
		}
		public RackInfo()
		{
		}
		public RackInfo getCopy()
		{
			return new RackInfo(this.r_id, this.r_name, this.r_s_x, this.r_s_y, this.r_e_x, this.r_e_y, this.ids_device, this.r_reserve);
		}
		public RackInfo(long l_id, string str_name, int i_sx, int i_sy, int i_ex, int i_ey, string devList, string str_fullname)
		{
			this.r_id = l_id;
			this.r_name = str_name;
			this.r_s_x = i_sx;
			this.r_s_y = i_sy;
			this.r_e_x = i_ex;
			this.r_e_y = i_ey;
			this.ids_device = devList;
			this.r_reserve = str_fullname;
		}
		public RackInfo(long l_id, string str_name, int i_sx, int i_sy, int i_ex, int i_ey, string str_fullname)
		{
			this.r_id = l_id;
			this.r_name = str_name;
			this.r_s_x = i_sx;
			this.r_s_y = i_sy;
			this.r_e_x = i_ex;
			this.r_e_y = i_ey;
			this.r_reserve = str_fullname;
		}
		public RackInfo(DataRow tmp_dr_z)
		{
			if (tmp_dr_z != null)
			{
				this.r_id = Convert.ToInt64(tmp_dr_z["id"]);
				this.r_name = Convert.ToString(tmp_dr_z["rack_nm"]);
				this.r_s_x = (int)Convert.ToInt16(tmp_dr_z["sx"]);
				this.r_s_y = (int)Convert.ToInt16(tmp_dr_z["sy"]);
				this.r_e_x = (int)Convert.ToInt16(tmp_dr_z["ex"]);
				this.r_e_y = (int)Convert.ToInt16(tmp_dr_z["ey"]);
				string text = "";
				try
				{
					text = Convert.ToString(tmp_dr_z["reserve"]);
				}
				catch
				{
				}
				this.r_reserve = text;
			}
		}
		public RackInfo(DataTable tmp_dr_z)
		{
			DataRow[] array = tmp_dr_z.Select();
			if (array != null && array.Length > 0)
			{
				this.r_id = Convert.ToInt64(array[0]["id"]);
				this.r_name = Convert.ToString(array[0]["rack_nm"]);
				this.r_s_x = (int)Convert.ToInt16(array[0]["sx"]);
				this.r_s_y = (int)Convert.ToInt16(array[0]["sy"]);
				this.r_e_x = (int)Convert.ToInt16(array[0]["ex"]);
				this.r_e_y = (int)Convert.ToInt16(array[0]["ey"]);
				string text = "";
				try
				{
					text = Convert.ToString(array[0]["reserve"]);
				}
				catch
				{
				}
				this.r_reserve = text;
			}
		}
		public static List<int> GetDeviceIDByOriginalName(string str_rackname)
		{
			List<int> result = new List<int>();
			try
			{
				Hashtable rackCache = DBCache.GetRackCache();
				Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
				if (rackCache != null && rackCache.Count > 0 && rackDeviceMap != null && rackDeviceMap.Count > 0)
				{
					ICollection values = rackCache.Values;
					foreach (RackInfo rackInfo in values)
					{
						if (rackInfo.OriginalName.Equals(str_rackname))
						{
							long rackID = rackInfo.RackID;
							if (rackDeviceMap.ContainsKey(Convert.ToInt32(rackID)))
							{
								return (List<int>)rackDeviceMap[Convert.ToInt32(rackID)];
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static List<int> GetDeviceIDByID(long r_id)
		{
			List<int> result = new List<int>();
			try
			{
				Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
				if (rackDeviceMap != null && rackDeviceMap.Count > 0 && rackDeviceMap.ContainsKey(Convert.ToInt32(r_id)))
				{
					return (List<int>)rackDeviceMap[Convert.ToInt32(r_id)];
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static long CreateRackInfo(string str_name, string str_devicelist, int i_sx, int i_sy, int i_ex, int i_ey, string str_fullname)
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
					string commandText = string.Concat(new object[]
					{
						"insert into device_addr_info ( rack_nm,sx,sy,ex,ey,reserve ) values('",
						str_name,
						"',",
						i_sx,
						",",
						i_sy,
						",",
						i_ex,
						",",
						i_ey,
						",'",
						str_fullname,
						"')"
					});
					dbCommand.CommandText = commandText;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "SELECT @@IDENTITY";
					dbCommand.Parameters.Clear();
					DBCacheStatus.Rack = true;
					DBCacheStatus.ZONE = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Rack",
						"DBSyncEventName_Service_ZONE"
					});
					long result = Convert.ToInt64(dbCommand.ExecuteScalar());
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (ex.Message.IndexOf(" duplicate values ") > 0)
				{
					long result = -2L;
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
			return -1L;
		}
		public string GetDisplayRackName(int i_FullNameFlag)
		{
			if (i_FullNameFlag == 1)
			{
				if (this.r_reserve != null && this.r_reserve.Length != 0)
				{
					return this.r_reserve;
				}
				if (this.r_name != null)
				{
					return this.r_name;
				}
				return "";
			}
			else
			{
				if (this.r_name != null)
				{
					return this.r_name;
				}
				return "";
			}
		}
		public int UpdateRack()
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
					if (DBUrl.SERVERMODE)
					{
						string commandText = "update device_addr_info set rack_nm=?rack_nm , sx=?sx ,sy=?sy, ex=?ex ,ey=?ey, reserve=?reserve where id= " + this.r_id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?rack_nm", this.r_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sx", this.r_s_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sy", this.r_s_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ex", this.r_e_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ey", this.r_e_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?reserve", this.r_reserve, dbCommand));
					}
					else
					{
						string commandText = "update device_addr_info set rack_nm=? , sx=? ,sy=?, ex=? ,ey=?, reserve=? where id= " + this.r_id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@rack_nm", this.r_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sx", this.r_s_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sy", this.r_s_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ex", this.r_e_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ey", this.r_e_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@reserve", this.r_reserve, dbCommand));
					}
					int num = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					DBCacheStatus.Rack = true;
					DBCacheStatus.ZONE = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Rack",
						"DBSyncEventName_Service_ZONE"
					});
					int result = num;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
			return -1;
		}
		public static int DeleteByID(long r_id)
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
					string text = Convert.ToString(r_id);
					string commandText = "delete from group_detail where grouptype = 'rack' and dest_id =" + text;
					dbCommand.CommandText = commandText;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select id,racks from zone_info ";
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					List<string> list = new List<string>();
					while (dbDataReader.Read())
					{
						bool flag = false;
						long num = Convert.ToInt64(dbDataReader.GetValue(0));
						string text2 = "";
						string @string = dbDataReader.GetString(1);
						string[] separator = new string[]
						{
							","
						};
						string[] array = @string.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text3 = array2[i];
							if (text3.Equals(Convert.ToString(text)))
							{
								flag = true;
							}
							else
							{
								text2 = text2 + text3 + ",";
							}
						}
						if (flag)
						{
							if (text2.EndsWith(","))
							{
								text2 = text2.Substring(0, text2.Length - 1);
							}
							list.Add(string.Concat(new object[]
							{
								"update zone_info set racks = '",
								text2,
								"' where id = ",
								num
							}));
						}
					}
					dbDataReader.Close();
					foreach (string current in list)
					{
						dbCommand.CommandText = current;
						dbCommand.ExecuteNonQuery();
					}
					string commandText2 = "delete from device_addr_info where id = " + text;
					dbCommand.CommandText = commandText2;
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					DBCacheStatus.Rack = true;
					DBCacheStatus.Group = true;
					DBCacheStatus.ZONE = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Rack",
						"DBSyncEventName_Service_Group",
						"DBSyncEventName_Service_ZONE"
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
		public static RackInfo getRackByID(long l_id)
		{
			try
			{
				Hashtable rackCache = DBCache.GetRackCache();
				Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
				if (rackCache != null && rackCache.ContainsKey(Convert.ToInt32(l_id)))
				{
					RackInfo rackInfo = (RackInfo)rackCache[Convert.ToInt32(l_id)];
					string text = "";
					if (rackDeviceMap.ContainsKey(Convert.ToInt32(rackInfo.RackID)))
					{
						List<int> list = (List<int>)rackDeviceMap[Convert.ToInt32(rackInfo.RackID)];
						StringBuilder stringBuilder = new StringBuilder();
						foreach (int current in list)
						{
							stringBuilder.Append("," + current);
						}
						text = stringBuilder.ToString();
						if (text.Length > 1)
						{
							text = text.Substring(1);
						}
						else
						{
							text = "";
						}
					}
					rackInfo.DeviceInfo = text;
					return rackInfo.getCopy();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static string GetZoneNamesByRackID(long l_rid)
		{
			string text = "";
			try
			{
				Hashtable rackZoneMap = DBCache.GetRackZoneMap();
				Hashtable zoneCache = DBCache.GetZoneCache();
				if (rackZoneMap != null && rackZoneMap.ContainsKey(l_rid) && zoneCache != null && zoneCache.Count > 0)
				{
					List<long> list = (List<long>)rackZoneMap[l_rid];
					foreach (long current in list)
					{
						if (zoneCache.ContainsKey(current))
						{
							ZoneInfo zoneInfo = (ZoneInfo)zoneCache[current];
							text = text + zoneInfo.ZoneName + ",";
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			if (text.EndsWith(","))
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}
		public static ZoneInfo GetZoneByRackID(long l_rid)
		{
			try
			{
				Hashtable rackZoneMap = DBCache.GetRackZoneMap();
				Hashtable zoneCache = DBCache.GetZoneCache();
				if (rackZoneMap != null && rackZoneMap.ContainsKey(l_rid) && zoneCache != null && zoneCache.Count > 0)
				{
					List<long> list = (List<long>)rackZoneMap[l_rid];
					foreach (long current in list)
					{
						if (zoneCache.ContainsKey(current))
						{
							return (ZoneInfo)zoneCache[current];
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
		public static RackInfo getRackByOriginalName(string str_name)
		{
			try
			{
				Hashtable rackCache = DBCache.GetRackCache();
				Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
				if (rackCache != null && rackCache.Count > 0)
				{
					ICollection values = rackCache.Values;
					foreach (RackInfo rackInfo in values)
					{
						if (rackInfo.OriginalName.Equals(str_name))
						{
							string text = "";
							if (rackDeviceMap.ContainsKey((int)rackInfo.RackID))
							{
								List<int> list = (List<int>)rackDeviceMap[(int)rackInfo.RackID];
								StringBuilder stringBuilder = new StringBuilder();
								foreach (int current in list)
								{
									stringBuilder.Append("," + current);
								}
								text = stringBuilder.ToString();
								if (text.Length > 1)
								{
									text = text.Substring(1);
								}
								else
								{
									text = "";
								}
							}
							rackInfo.DeviceInfo = text;
							return rackInfo.getCopy();
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
		public static ArrayList GetAllRack_NoEmpty()
		{
			int rackFullNameflag = Sys_Para.GetRackFullNameflag();
			List<RackInfo> list = new List<RackInfo>();
			try
			{
				Hashtable rackCache = DBCache.GetRackCache();
				Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
				if (rackCache != null && rackCache.Count > 0 && rackDeviceMap != null && rackDeviceMap.Count > 0)
				{
					ICollection keys = rackDeviceMap.Keys;
					foreach (int num in keys)
					{
						if (rackCache.ContainsKey(num))
						{
							RackInfo rackInfo = (RackInfo)rackCache[num];
							string text = "";
							if (rackDeviceMap.ContainsKey(num))
							{
								List<int> list2 = (List<int>)rackDeviceMap[num];
								StringBuilder stringBuilder = new StringBuilder();
								foreach (int current in list2)
								{
									stringBuilder.Append("," + current);
								}
								text = stringBuilder.ToString();
								if (text.Length > 1)
								{
									text = text.Substring(1);
								}
								else
								{
									text = "";
								}
							}
							RackInfo rackInfo2 = new RackInfo(rackInfo.RackID, rackInfo.OriginalName, rackInfo.StartPoint_X, rackInfo.StartPoint_Y, rackInfo.EndPoint_X, rackInfo.EndPoint_Y, rackInfo.RackFullName);
							rackInfo2.DeviceInfo = text;
							rackInfo2.setNameFlag(rackFullNameflag);
							list.Add(rackInfo2);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			list.Sort((RackInfo x, RackInfo y) => x.RackName.CompareTo(y.RackName));
			ArrayList arrayList = new ArrayList();
			foreach (RackInfo current2 in list)
			{
				arrayList.Add(current2);
			}
			return arrayList;
		}
		public static ArrayList getAllRack()
		{
			int rackFullNameflag = Sys_Para.GetRackFullNameflag();
			List<RackInfo> list = new List<RackInfo>();
			try
			{
				Hashtable rackCache = DBCache.GetRackCache();
				Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
				if (rackCache != null && rackCache.Count > 0)
				{
					ICollection values = rackCache.Values;
					foreach (RackInfo rackInfo in values)
					{
						RackInfo rackInfo2 = new RackInfo(rackInfo.RackID, rackInfo.OriginalName, rackInfo.StartPoint_X, rackInfo.StartPoint_Y, rackInfo.EndPoint_X, rackInfo.EndPoint_Y, rackInfo.RackFullName);
						rackInfo2.setNameFlag(rackFullNameflag);
						string text = "";
						if (rackDeviceMap.ContainsKey((int)rackInfo2.RackID))
						{
							List<int> list2 = (List<int>)rackDeviceMap[(int)rackInfo2.RackID];
							StringBuilder stringBuilder = new StringBuilder();
							foreach (int current in list2)
							{
								stringBuilder.Append("," + current);
							}
							text = stringBuilder.ToString();
							if (text.Length > 1)
							{
								text = text.Substring(1);
							}
							else
							{
								text = "";
							}
						}
						rackInfo2.DeviceInfo = text;
						list.Add(rackInfo2);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			list.Sort((RackInfo x, RackInfo y) => x.RackName.CompareTo(y.RackName));
			ArrayList arrayList = new ArrayList();
			foreach (RackInfo current2 in list)
			{
				arrayList.Add(current2);
			}
			return arrayList;
		}
		public static bool CheckOriginalName(long l_id, string str_name)
		{
			try
			{
				Hashtable rackCache = DBCache.GetRackCache();
				if (rackCache != null && rackCache.Count > 0)
				{
					ICollection values = rackCache.Values;
					foreach (RackInfo rackInfo in values)
					{
						if (rackInfo.OriginalName.Equals(str_name))
						{
							bool result;
							if (rackInfo.RackID == l_id)
							{
								result = true;
								return result;
							}
							result = false;
							return result;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				bool result = false;
				return result;
			}
			return true;
		}
		public static DataTable GetRackNameAndDeviceID()
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("rack_nm");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("device_ids");
			dataColumn2.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn2);
			try
			{
				Hashtable rackCache = DBCache.GetRackCache();
				Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
				if (rackCache != null && rackCache.Count > 0 && rackDeviceMap != null && rackDeviceMap.Count > 0)
				{
					ICollection keys = rackDeviceMap.Keys;
					foreach (int num in keys)
					{
						if (rackCache.ContainsKey(num))
						{
							RackInfo rackInfo = (RackInfo)rackCache[num];
							List<int> list = (List<int>)rackDeviceMap[num];
							string text = "";
							foreach (int current in list)
							{
								text = text + "," + current;
							}
							if (text.Length > 1)
							{
								text = text.Substring(1);
							}
							else
							{
								text = "";
							}
							DataRow dataRow = dataTable.NewRow();
							dataRow["rack_nm"] = rackInfo.OriginalName;
							dataRow["device_ids"] = text;
							dataTable.Rows.Add(dataRow);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return dataTable;
		}
		public static void GenerateAllRackThermal(Dictionary<int, Dictionary<int, double>> D_input)
		{
			RackInfo.Last_Update_Thermal = DateTime.Now;
			DateTime arg_0F_0 = DateTime.Now;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
			Dictionary<int, double> dictionary3 = new Dictionary<int, double>();
			Dictionary<int, RackInfo.RackThermalBitMap> dictionary4 = new Dictionary<int, RackInfo.RackThermalBitMap>();
			DataTable dataTable = new DataTable();
			DataTable dataTable2 = new DataTable();
			DataTable dataTable3 = new DataTable();
			DataTable dataTable4 = new DataTable();
			string commandText = "select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_hourly where insert_time = #" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss") + "#";
			string commandText2 = "select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_daily where insert_time = #" + DateTime.Now.ToString("yyyy-MM-dd") + "#";
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				commandText = "select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_hourly where insert_time = '" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss") + "'";
				commandText2 = "select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_daily where insert_time = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
			}
			string commandText3 = "select id,rack_id from device_base_info ";
			string commandText4 = "select device_id,sensor_type,location_type from device_sensor_info ";
			DBConn dBConn = null;
			DBConn dBConn2 = null;
			DbCommand dbCommand = new OleDbCommand();
			DbCommand dbCommand2 = new OleDbCommand();
			DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
			DateTime now = DateTime.Now;
			int arg_1B7_0 = DateTime.Now.Year;
			int arg_1C6_0 = DateTime.Now.Month;
			int arg_1D5_0 = DateTime.Now.Day;
			int arg_1E4_0 = DateTime.Now.Hour;
			try
			{
				if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					dBConn = DBConnPool.getDynaConnection();
				}
				else
				{
					dBConn = DBConnPool.getThermalConnection();
				}
				dBConn2 = DBConnPool.getConnection();
				if (dBConn.con != null && dBConn2.con != null)
				{
					dbCommand2 = dBConn2.con.CreateCommand();
					dbCommand2.CommandType = CommandType.Text;
					dbCommand2.CommandText = commandText3;
					DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn2.con);
					dataAdapter.SelectCommand = dbCommand2;
					dataAdapter.Fill(dataTable);
					dbCommand2.Parameters.Clear();
					dbCommand2.CommandType = CommandType.Text;
					dbCommand2.CommandText = commandText4;
					dataAdapter.SelectCommand = dbCommand2;
					dataAdapter.Fill(dataTable2);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.Parameters.Clear();
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText;
					dataAdapter = DBConn.GetDataAdapter(dBConn.con);
					dataAdapter.SelectCommand = dbCommand;
					dataAdapter.Fill(dataTable3);
					dbCommand.Parameters.Clear();
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText2;
					dataAdapter.SelectCommand = dbCommand;
					dataAdapter.Fill(dataTable4);
					dataAdapter.Dispose();
					IEnumerator<int> enumerator = D_input.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						int current = enumerator.Current;
						DataRow[] array = dataTable.Select(" id = " + current);
						if (array.Length > 0)
						{
							int key = Convert.ToInt32(array[0]["rack_id"]);
							if (!dictionary4.ContainsKey(key))
							{
								dictionary4.Add(key, new RackInfo.RackThermalBitMap
								{
									b_intake = false,
									b_exhaust = false,
									b_difference = false
								});
							}
							Dictionary<int, double> dictionary5 = D_input[current];
							IEnumerator<int> enumerator2 = dictionary5.Keys.GetEnumerator();
							while (enumerator2.MoveNext())
							{
								int current2 = enumerator2.Current;
								DataRow[] array2 = dataTable2.Select(string.Concat(new object[]
								{
									" device_id = ",
									current,
									" and sensor_type =",
									current2
								}));
								if (array2.Length > 0)
								{
									int num = Convert.ToInt32(array2[0]["location_type"]);
									double num2 = dictionary5[current2];
									if (num == 0)
									{
										RackInfo.RackThermalBitMap value = dictionary4[key];
										value.b_intake = true;
										if (value.b_exhaust)
										{
											value.b_difference = true;
										}
										dictionary4[key] = value;
										if (dictionary.ContainsKey(key))
										{
											double num3 = dictionary[key];
											if (num3 < num2)
											{
												dictionary[key] = num2;
											}
										}
										else
										{
											dictionary.Add(key, num2);
										}
										if (dictionary2.ContainsKey(key))
										{
											double num4 = dictionary2[key];
											if (num2 < num4)
											{
												dictionary2[key] = num2;
											}
										}
										else
										{
											dictionary2.Add(key, num2);
										}
									}
									else
									{
										if (num == 1)
										{
											RackInfo.RackThermalBitMap value2 = dictionary4[key];
											value2.b_exhaust = true;
											if (value2.b_intake)
											{
												value2.b_difference = true;
											}
											dictionary4[key] = value2;
											if (dictionary3.ContainsKey(key))
											{
												double num5 = dictionary3[key];
												if (num5 < num2)
												{
													dictionary3[key] = num2;
												}
											}
											else
											{
												dictionary3.Add(key, num2);
											}
										}
									}
								}
							}
						}
					}
					IEnumerator<int> enumerator3 = dictionary4.Keys.GetEnumerator();
					while (enumerator3.MoveNext())
					{
						double num6 = -2.0;
						double num7 = -2.0;
						double num8 = -2.0;
						int current3 = enumerator3.Current;
						RackInfo.RackThermalBitMap rackThermalBitMap = dictionary4[current3];
						string text = "";
						string text2 = "";
						if (rackThermalBitMap.b_intake || rackThermalBitMap.b_exhaust || rackThermalBitMap.b_difference)
						{
							DataRow[] array3 = dataTable4.Select(" rack_id = " + current3);
							if (array3 != null && array3.Length > 0)
							{
								DateTime value3 = Convert.ToDateTime(array3[0]["insert_time"]);
								if (dateTime.CompareTo(value3) > 0)
								{
									if (rackThermalBitMap.b_difference)
									{
										text = string.Concat(new object[]
										{
											"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
											current3,
											",",
											dictionary[current3],
											",",
											dictionary3[current3],
											",",
											dictionary3[current3] - dictionary2[current3],
											",'",
											DateTime.Now.ToString("yyyy-MM-dd"),
											"' )"
										});
									}
									if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
									{
										text = string.Concat(new object[]
										{
											"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
											current3,
											",",
											dictionary[current3],
											",null,null,'",
											DateTime.Now.ToString("yyyy-MM-dd"),
											"' )"
										});
									}
									if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
									{
										text = string.Concat(new object[]
										{
											"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
											current3,
											",null,",
											dictionary3[current3],
											",null,'",
											DateTime.Now.ToString("yyyy-MM-dd"),
											"' )"
										});
									}
									dbCommand.Parameters.Clear();
									dbCommand.CommandType = CommandType.Text;
									dbCommand.CommandText = text;
									string arg_7EE_0 = dbCommand.CommandText;
									dbCommand.ExecuteNonQuery();
								}
								else
								{
									if (dateTime.CompareTo(value3) == 0)
									{
										try
										{
											num6 = Convert.ToDouble(array3[0]["intakepeak"]);
										}
										catch
										{
										}
										try
										{
											num7 = Convert.ToDouble(array3[0]["exhaustpeak"]);
										}
										catch
										{
										}
										try
										{
											num8 = Convert.ToDouble(array3[0]["differencepeak"]);
										}
										catch
										{
										}
										bool flag = false;
										if (rackThermalBitMap.b_difference)
										{
											if (num6 < dictionary[current3])
											{
												num6 = dictionary[current3];
												flag = true;
											}
											if (num7 < dictionary3[current3])
											{
												num7 = dictionary3[current3];
												flag = true;
											}
											if (num8 < dictionary3[current3] - dictionary2[current3])
											{
												num8 = dictionary3[current3] - dictionary2[current3];
												flag = true;
											}
											if (flag)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text3 = "null";
												string text4 = "null";
												string text5 = "null";
												if (num6 >= 0.0)
												{
													text3 = string.Concat(num6);
												}
												if (num7 >= 0.0)
												{
													text4 = string.Concat(num7);
												}
												if (num8 >= 0.0)
												{
													text5 = string.Concat(num8);
												}
												text = string.Concat(new object[]
												{
													"update rackthermal_daily  set intakepeak = ",
													text3,
													", exhaustpeak = ",
													text4,
													",differencepeak = ",
													text5,
													" where rack_id = ",
													current3,
													" and insert_time = #",
													value3.ToString("yyyy-MM-dd"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text = text.Replace("#", "'");
												}
												dbCommand.CommandText = text;
												string arg_9F5_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
										if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
										{
											if (num6 < dictionary[current3])
											{
												num6 = dictionary[current3];
												flag = true;
											}
											if (flag)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text6 = "null";
												if (num6 >= 0.0)
												{
													text6 = string.Concat(num6);
												}
												text = string.Concat(new object[]
												{
													"update rackthermal_daily  set intakepeak = ",
													text6,
													"  where rack_id = ",
													current3,
													" and insert_time = #",
													value3.ToString("yyyy-MM-dd"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text = text.Replace("#", "'");
												}
												dbCommand.CommandText = text;
												string arg_AFB_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
										if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
										{
											if (num7 < dictionary3[current3])
											{
												num7 = dictionary3[current3];
												flag = true;
											}
											if (flag)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text7 = "null";
												if (num7 >= 0.0)
												{
													text7 = string.Concat(num7);
												}
												text = string.Concat(new object[]
												{
													"update rackthermal_daily  set exhaustpeak = ",
													text7,
													"  where rack_id = ",
													current3,
													" and insert_time = #",
													value3.ToString("yyyy-MM-dd"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text = text.Replace("#", "'");
												}
												dbCommand.CommandText = text;
												string arg_C01_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
									}
								}
							}
							else
							{
								dbCommand.Parameters.Clear();
								dbCommand.CommandType = CommandType.Text;
								if (rackThermalBitMap.b_difference)
								{
									text = string.Concat(new object[]
									{
										"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
										current3,
										",",
										dictionary[current3],
										",",
										dictionary3[current3],
										",",
										dictionary3[current3] - dictionary2[current3],
										",'",
										DateTime.Now.ToString("yyyy-MM-dd"),
										"' )"
									});
								}
								if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
								{
									text = string.Concat(new object[]
									{
										"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
										current3,
										",",
										dictionary[current3],
										",null,null,'",
										DateTime.Now.ToString("yyyy-MM-dd"),
										"' )"
									});
								}
								if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
								{
									text = string.Concat(new object[]
									{
										"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
										current3,
										",null,",
										dictionary3[current3],
										",null,'",
										DateTime.Now.ToString("yyyy-MM-dd"),
										"' )"
									});
								}
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
								{
									text = text.Replace("#", "'");
								}
								dbCommand.CommandText = text;
								string arg_E04_0 = dbCommand.CommandText;
								dbCommand.ExecuteNonQuery();
							}
							DataRow[] array4 = dataTable3.Select(" rack_id = " + current3);
							if (array4 != null && array4.Length > 0)
							{
								DateTime value4 = Convert.ToDateTime(array4[0]["insert_time"]);
								if (now.CompareTo(value4) > 0)
								{
									if (now.Year == value4.Year && now.Month == value4.Month && now.Day == value4.Day && now.Hour == value4.Hour)
									{
										try
										{
											num6 = Convert.ToDouble(array4[0]["intakepeak"]);
										}
										catch
										{
										}
										try
										{
											num7 = Convert.ToDouble(array4[0]["exhaustpeak"]);
										}
										catch
										{
										}
										try
										{
											num8 = Convert.ToDouble(array4[0]["differencepeak"]);
										}
										catch
										{
										}
										bool flag2 = false;
										if (rackThermalBitMap.b_difference)
										{
											if (num6 < dictionary[current3])
											{
												num6 = dictionary[current3];
												flag2 = true;
											}
											if (num7 < dictionary3[current3])
											{
												num7 = dictionary3[current3];
												flag2 = true;
											}
											if (num8 < dictionary3[current3] - dictionary2[current3])
											{
												num8 = dictionary3[current3] - dictionary2[current3];
												flag2 = true;
											}
											if (flag2)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text8 = "null";
												string text9 = "null";
												string text10 = "null";
												if (num6 >= 0.0)
												{
													text8 = string.Concat(num6);
												}
												if (num7 >= 0.0)
												{
													text9 = string.Concat(num7);
												}
												if (num8 >= 0.0)
												{
													text10 = string.Concat(num8);
												}
												text2 = string.Concat(new object[]
												{
													"update rackthermal_hourly  set intakepeak = ",
													text8,
													", exhaustpeak = ",
													text9,
													",differencepeak = ",
													text10,
													" where rack_id = ",
													current3,
													" and insert_time = #",
													new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text2 = text2.Replace("#", "'");
												}
												dbCommand.CommandText = text2;
												string arg_10B9_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
										if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
										{
											if (num6 < dictionary[current3])
											{
												num6 = dictionary[current3];
												flag2 = true;
											}
											if (flag2)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text11 = "null";
												if (num6 >= 0.0)
												{
													text11 = string.Concat(num6);
												}
												text2 = string.Concat(new object[]
												{
													"update rackthermal_hourly  set intakepeak = ",
													text11,
													"  where rack_id = ",
													current3,
													" and insert_time = #",
													new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text2 = text2.Replace("#", "'");
												}
												dbCommand.CommandText = text2;
												string arg_11E5_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
										if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
										{
											if (num7 < dictionary3[current3])
											{
												num7 = dictionary3[current3];
												flag2 = true;
											}
											if (flag2)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text12 = "null";
												if (num7 >= 0.0)
												{
													text12 = string.Concat(num7);
												}
												text2 = string.Concat(new object[]
												{
													"update rackthermal_hourly  set exhaustpeak = ",
													text12,
													"  where rack_id = ",
													current3,
													" and insert_time = #",
													new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text2 = text2.Replace("#", "'");
												}
												dbCommand.CommandText = text2;
												string arg_1311_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
									}
									else
									{
										dbCommand.Parameters.Clear();
										dbCommand.CommandType = CommandType.Text;
										if (rackThermalBitMap.b_difference)
										{
											text2 = string.Concat(new object[]
											{
												"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
												current3,
												",",
												dictionary[current3],
												",",
												dictionary3[current3],
												",",
												dictionary3[current3] - dictionary2[current3],
												",'",
												new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"' )"
											});
										}
										if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
										{
											text2 = string.Concat(new object[]
											{
												"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
												current3,
												",",
												dictionary[current3],
												",null,null,'",
												new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"' )"
											});
										}
										if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
										{
											text2 = string.Concat(new object[]
											{
												"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
												current3,
												",null,",
												dictionary3[current3],
												",null,'",
												new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"' )"
											});
										}
										if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
										{
											text2 = text2.Replace("#", "'");
										}
										dbCommand.CommandText = text2;
										string arg_15D1_0 = dbCommand.CommandText;
										dbCommand.ExecuteNonQuery();
									}
								}
								else
								{
									if (now.CompareTo(value4) == 0)
									{
										try
										{
											num6 = Convert.ToDouble(array4[0]["intakepeak"]);
										}
										catch
										{
										}
										try
										{
											num7 = Convert.ToDouble(array4[0]["exhaustpeak"]);
										}
										catch
										{
										}
										try
										{
											num8 = Convert.ToDouble(array4[0]["differencepeak"]);
										}
										catch
										{
										}
										bool flag3 = false;
										if (rackThermalBitMap.b_difference)
										{
											if (num6 < dictionary[current3])
											{
												num6 = dictionary[current3];
												flag3 = true;
											}
											if (num7 < dictionary3[current3])
											{
												num7 = dictionary3[current3];
												flag3 = true;
											}
											if (num8 < dictionary3[current3] - dictionary2[current3])
											{
												num8 = dictionary3[current3] - dictionary2[current3];
												flag3 = true;
											}
											if (flag3)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text13 = "null";
												string text14 = "null";
												string text15 = "null";
												if (num6 >= 0.0)
												{
													text13 = string.Concat(num6);
												}
												if (num7 >= 0.0)
												{
													text14 = string.Concat(num7);
												}
												if (num8 >= 0.0)
												{
													text15 = string.Concat(num8);
												}
												text2 = string.Concat(new object[]
												{
													"update rackthermal_hourly  set intakepeak = ",
													text13,
													", exhaustpeak = ",
													text14,
													",differencepeak = ",
													text15,
													" where rack_id = ",
													current3,
													" and insert_time = #",
													new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text2 = text2.Replace("#", "'");
												}
												dbCommand.CommandText = text2;
												string arg_17FE_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
										if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
										{
											if (num6 < dictionary[current3])
											{
												num6 = dictionary[current3];
												flag3 = true;
											}
											if (flag3)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text16 = "null";
												if (num6 >= 0.0)
												{
													text16 = string.Concat(num6);
												}
												text2 = string.Concat(new object[]
												{
													"update rackthermal_hourly  set intakepeak = ",
													text16,
													"  where rack_id = ",
													current3,
													" and insert_time = #",
													new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text2 = text2.Replace("#", "'");
												}
												dbCommand.CommandText = text2;
												string arg_192A_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
										if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
										{
											if (num7 < dictionary3[current3])
											{
												num7 = dictionary3[current3];
												flag3 = true;
											}
											if (flag3)
											{
												dbCommand.Parameters.Clear();
												dbCommand.CommandType = CommandType.Text;
												string text17 = "null";
												if (num7 >= 0.0)
												{
													text17 = string.Concat(num7);
												}
												text2 = string.Concat(new object[]
												{
													"update rackthermal_hourly  set exhaustpeak = ",
													text17,
													"  where rack_id = ",
													current3,
													" and insert_time = #",
													new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
													"# "
												});
												if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
												{
													text2 = text2.Replace("#", "'");
												}
												dbCommand.CommandText = text2;
												string arg_1A56_0 = dbCommand.CommandText;
												dbCommand.ExecuteNonQuery();
											}
										}
									}
									else
									{
										if (now.Year == value4.Year && now.Month == value4.Month && now.Day == value4.Day && now.Hour == value4.Hour)
										{
											try
											{
												num6 = Convert.ToDouble(array4[0]["intakepeak"]);
											}
											catch
											{
											}
											try
											{
												num7 = Convert.ToDouble(array4[0]["exhaustpeak"]);
											}
											catch
											{
											}
											try
											{
												num8 = Convert.ToDouble(array4[0]["differencepeak"]);
											}
											catch
											{
											}
											bool flag4 = false;
											if (rackThermalBitMap.b_difference)
											{
												if (num6 < dictionary[current3])
												{
													num6 = dictionary[current3];
													flag4 = true;
												}
												if (num7 < dictionary3[current3])
												{
													num7 = dictionary3[current3];
													flag4 = true;
												}
												if (num8 < dictionary3[current3] - dictionary2[current3])
												{
													num8 = dictionary3[current3] - dictionary2[current3];
													flag4 = true;
												}
												if (flag4)
												{
													dbCommand.Parameters.Clear();
													dbCommand.CommandType = CommandType.Text;
													string text18 = "null";
													string text19 = "null";
													string text20 = "null";
													if (num6 >= 0.0)
													{
														text18 = string.Concat(num6);
													}
													if (num7 >= 0.0)
													{
														text19 = string.Concat(num7);
													}
													if (num8 >= 0.0)
													{
														text20 = string.Concat(num8);
													}
													text2 = string.Concat(new object[]
													{
														"update rackthermal_hourly  set intakepeak = ",
														text18,
														", exhaustpeak = ",
														text19,
														",differencepeak = ",
														text20,
														" where rack_id = ",
														current3,
														" and insert_time = #",
														new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
														"# "
													});
													if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
													{
														text2 = text2.Replace("#", "'");
													}
													dbCommand.CommandText = text2;
													string arg_1CC1_0 = dbCommand.CommandText;
													dbCommand.ExecuteNonQuery();
												}
											}
											if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
											{
												if (num6 < dictionary[current3])
												{
													num6 = dictionary[current3];
													flag4 = true;
												}
												if (flag4)
												{
													dbCommand.Parameters.Clear();
													dbCommand.CommandType = CommandType.Text;
													string text21 = "null";
													if (num6 >= 0.0)
													{
														text21 = string.Concat(num6);
													}
													text2 = string.Concat(new object[]
													{
														"update rackthermal_hourly  set intakepeak = ",
														text21,
														"  where rack_id = ",
														current3,
														" and insert_time = #",
														new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
														"# "
													});
													if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
													{
														text2 = text2.Replace("#", "'");
													}
													dbCommand.CommandText = text2;
													string arg_1DED_0 = dbCommand.CommandText;
													dbCommand.ExecuteNonQuery();
												}
											}
											if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
											{
												if (num7 < dictionary3[current3])
												{
													num7 = dictionary3[current3];
													flag4 = true;
												}
												if (flag4)
												{
													dbCommand.Parameters.Clear();
													dbCommand.CommandType = CommandType.Text;
													string text22 = "null";
													if (num7 >= 0.0)
													{
														text22 = string.Concat(num7);
													}
													text2 = string.Concat(new object[]
													{
														"update rackthermal_hourly  set exhaustpeak = ",
														text22,
														"  where rack_id = ",
														current3,
														" and insert_time = #",
														new DateTime(value4.Year, value4.Month, value4.Day, value4.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
														"# "
													});
													if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
													{
														text2 = text2.Replace("#", "'");
													}
													dbCommand.CommandText = text2;
													string arg_1F19_0 = dbCommand.CommandText;
													dbCommand.ExecuteNonQuery();
												}
											}
										}
									}
								}
							}
							else
							{
								dbCommand.Parameters.Clear();
								dbCommand.CommandType = CommandType.Text;
								if (rackThermalBitMap.b_difference)
								{
									text2 = string.Concat(new object[]
									{
										"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
										current3,
										",",
										dictionary[current3],
										",",
										dictionary3[current3],
										",",
										dictionary3[current3] - dictionary2[current3],
										",#",
										new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
										"# )"
									});
								}
								if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
								{
									text2 = string.Concat(new object[]
									{
										"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
										current3,
										",",
										dictionary[current3],
										",null,null,#",
										new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
										"# )"
									});
								}
								if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
								{
									text2 = string.Concat(new object[]
									{
										"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
										current3,
										",null,",
										dictionary3[current3],
										",null,#",
										new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
										"# )"
									});
								}
								if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
								{
									text2 = text2.Replace("#", "'");
								}
								dbCommand.CommandText = text2;
								string arg_21D9_0 = dbCommand.CommandText;
								dbCommand.ExecuteNonQuery();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
				{
					DBUtil.SetMySQLInfo("rackthermal_daily");
					DBUtil.SetMySQLInfo("rackthermal_hourly");
					DebugCenter.GetInstance().appendToFile("MySQL database is marked as crashed, EcoSensor Monitor Service will be shutdown ");
					DBUtil.StopService();
				}
			}
			finally
			{
				RackInfo.Last_Update_Thermal = DateTime.Now;
				dbCommand.Dispose();
				dbCommand2.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
				if (dBConn2 != null)
				{
					dBConn2.close();
				}
			}
		}
		public static void GenerateAllRackThermal(Dictionary<int, Dictionary<int, double>> D_input, DateTime DT_insert)
		{
			DBTools.preparetable(DT_insert);
			RackInfo.Last_Update_Thermal = DateTime.Now;
			DateTime arg_16_0 = DateTime.Now;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
			Dictionary<int, double> dictionary3 = new Dictionary<int, double>();
			Dictionary<int, RackInfo.RackThermalBitMap> dictionary4 = new Dictionary<int, RackInfo.RackThermalBitMap>();
			new DataTable();
			new DataTable();
			DataTable dataTable = new DataTable();
			DataTable dataTable2 = new DataTable();
			string commandText = "select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_hourly where insert_time = #" + new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss") + "#";
			string commandText2 = "select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_daily where insert_time = #" + DateTime.Now.ToString("yyyy-MM-dd") + "#";
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				commandText = string.Concat(new string[]
				{
					"select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_hourly",
					DT_insert.ToString("yyyyMMdd"),
					" where insert_time = '",
					new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
					"'"
				});
				commandText2 = string.Concat(new string[]
				{
					"select rack_id,insert_time,intakepeak,exhaustpeak,differencepeak from rackthermal_daily",
					DT_insert.ToString("yyyyMMdd"),
					" where insert_time = '",
					DT_insert.ToString("yyyy-MM-dd"),
					"'"
				});
			}
			Convert.ToDateTime(DT_insert.ToString("yyyy-MM-dd"));
			int arg_1A0_0 = DT_insert.Year;
			int arg_1A8_0 = DT_insert.Month;
			int arg_1B0_0 = DT_insert.Day;
			int arg_1B8_0 = DT_insert.Hour;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				Hashtable deviceCache = DBCache.GetDeviceCache();
				Hashtable sensorCache = DBCache.GetSensorCache();
				Hashtable deviceSensorMap = DBCache.GetDeviceSensorMap();
				if (deviceCache != null && deviceCache.Count >= 1 && sensorCache != null && sensorCache.Count >= 1 && deviceSensorMap != null && deviceSensorMap.Count >= 1)
				{
					if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
					{
						dBConn = DBConnPool.getDynaConnection();
					}
					else
					{
						dBConn = DBConnPool.getDynaConnection(DT_insert);
					}
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = DBConn.GetCommandObject(dBConn.con);
						dbCommand.Parameters.Clear();
						dbCommand.CommandType = CommandType.Text;
						dbCommand.CommandText = commandText;
						DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dataAdapter.SelectCommand = dbCommand;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						dbCommand.Parameters.Clear();
						dbCommand.CommandType = CommandType.Text;
						dbCommand.CommandText = commandText2;
						dataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dataAdapter.SelectCommand = dbCommand;
						dataAdapter.Fill(dataTable2);
						dataAdapter.Dispose();
						IEnumerator<int> enumerator = D_input.Keys.GetEnumerator();
						int num = 0;
						int num2 = 0;
						while (enumerator.MoveNext())
						{
							int current = enumerator.Current;
							if (deviceCache.ContainsKey(current))
							{
								DeviceInfo deviceInfo = (DeviceInfo)deviceCache[current];
								try
								{
									num = Convert.ToInt32(deviceInfo.RackID);
								}
								catch
								{
								}
								if (num > 0)
								{
									if (!dictionary4.ContainsKey(num))
									{
										dictionary4.Add(num, new RackInfo.RackThermalBitMap
										{
											b_intake = false,
											b_exhaust = false,
											b_difference = false
										});
									}
									Dictionary<int, double> dictionary5 = D_input[current];
									if (dictionary5 != null)
									{
										IEnumerator<int> enumerator2 = dictionary5.Keys.GetEnumerator();
										while (enumerator2.MoveNext())
										{
											int current2 = enumerator2.Current;
											if (deviceSensorMap.ContainsKey(current))
											{
												List<int> list = (List<int>)deviceSensorMap[current];
												if (list != null && list.Count >= 1)
												{
													num2 = -5;
													foreach (int current3 in list)
													{
														if (sensorCache.ContainsKey(current3))
														{
															SensorInfo sensorInfo = (SensorInfo)sensorCache[current3];
															if (sensorInfo != null && sensorInfo.Type == current2)
															{
																num2 = sensorInfo.Location;
															}
														}
													}
													if (num2 != -5)
													{
														double num3 = dictionary5[current2];
														if (num2 == 0)
														{
															RackInfo.RackThermalBitMap value = dictionary4[num];
															value.b_intake = true;
															if (value.b_exhaust)
															{
																value.b_difference = true;
															}
															dictionary4[num] = value;
															if (dictionary.ContainsKey(num))
															{
																double num4 = dictionary[num];
																if (num4 < num3)
																{
																	dictionary[num] = num3;
																}
															}
															else
															{
																dictionary.Add(num, num3);
															}
															if (dictionary2.ContainsKey(num))
															{
																double num5 = dictionary2[num];
																if (num3 < num5)
																{
																	dictionary2[num] = num3;
																}
															}
															else
															{
																dictionary2.Add(num, num3);
															}
														}
														else
														{
															if (num2 == 1)
															{
																RackInfo.RackThermalBitMap value2 = dictionary4[num];
																value2.b_exhaust = true;
																if (value2.b_intake)
																{
																	value2.b_difference = true;
																}
																dictionary4[num] = value2;
																if (dictionary3.ContainsKey(num))
																{
																	double num6 = dictionary3[num];
																	if (num6 < num3)
																	{
																		dictionary3[num] = num3;
																	}
																}
																else
																{
																	dictionary3.Add(num, num3);
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
						IEnumerator<int> enumerator4 = dictionary4.Keys.GetEnumerator();
						while (enumerator4.MoveNext())
						{
							double num7 = -900.0;
							double num8 = -900.0;
							double num9 = -900.0;
							int current4 = enumerator4.Current;
							RackInfo.RackThermalBitMap rackThermalBitMap = dictionary4[current4];
							string text = "";
							string text2 = "";
							if (rackThermalBitMap.b_intake || rackThermalBitMap.b_exhaust || rackThermalBitMap.b_difference)
							{
								DataRow[] array = dataTable2.Select(" rack_id = " + current4);
								if (array != null && array.Length > 0)
								{
									object obj = array[0]["intakepeak"];
									if (obj == null || obj == DBNull.Value || obj.ToString().ToLower().Equals("null"))
									{
										num7 = -900.0;
									}
									else
									{
										try
										{
											num7 = Convert.ToDouble(array[0]["intakepeak"]);
										}
										catch
										{
										}
									}
									obj = array[0]["exhaustpeak"];
									if (obj == null || obj == DBNull.Value || obj.ToString().ToLower().Equals("null"))
									{
										num8 = -900.0;
									}
									else
									{
										try
										{
											num8 = Convert.ToDouble(array[0]["exhaustpeak"]);
										}
										catch
										{
										}
									}
									obj = array[0]["differencepeak"];
									if (obj == null || obj == DBNull.Value || obj.ToString().ToLower().Equals("null"))
									{
										num9 = -900.0;
									}
									else
									{
										try
										{
											num9 = Convert.ToDouble(array[0]["differencepeak"]);
										}
										catch
										{
										}
									}
									bool flag = false;
									if (rackThermalBitMap.b_difference)
									{
										if (num7 < dictionary[current4])
										{
											num7 = dictionary[current4];
											flag = true;
										}
										if (num8 < dictionary3[current4])
										{
											num8 = dictionary3[current4];
											flag = true;
										}
										if (num9 < dictionary3[current4] - dictionary2[current4])
										{
											num9 = dictionary3[current4] - dictionary2[current4];
											flag = true;
										}
										if (flag)
										{
											dbCommand.Parameters.Clear();
											dbCommand.CommandType = CommandType.Text;
											string text3;
											if (num7 != -900.0)
											{
												text3 = string.Concat(num7);
											}
											else
											{
												text3 = "null";
											}
											string text4;
											if (num8 != -900.0)
											{
												text4 = string.Concat(num8);
											}
											else
											{
												text4 = "null";
											}
											string text5;
											if (num9 != -900.0 && num9 >= 0.0)
											{
												text5 = string.Concat(num9);
											}
											else
											{
												text5 = "null";
											}
											text = string.Concat(new object[]
											{
												"update rackthermal_daily  set intakepeak = ",
												text3,
												", exhaustpeak = ",
												text4,
												",differencepeak = ",
												text5,
												" where rack_id = ",
												current4,
												" and insert_time = #",
												DT_insert.ToString("yyyy-MM-dd"),
												"# "
											});
											if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
											{
												text = string.Concat(new object[]
												{
													"update rackthermal_daily",
													DT_insert.ToString("yyyyMMdd"),
													"  set intakepeak = ",
													text3,
													", exhaustpeak = ",
													text4,
													",differencepeak = ",
													text5,
													" where rack_id = ",
													current4,
													" and insert_time = '",
													DT_insert.ToString("yyyy-MM-dd"),
													"' "
												});
											}
											dbCommand.CommandText = text;
											string arg_979_0 = dbCommand.CommandText;
											dbCommand.ExecuteNonQuery();
										}
									}
									if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
									{
										if (num7 < dictionary[current4])
										{
											num7 = dictionary[current4];
											flag = true;
										}
										if (flag)
										{
											dbCommand.Parameters.Clear();
											dbCommand.CommandType = CommandType.Text;
											string text6;
											if (num7 != -900.0)
											{
												text6 = string.Concat(num7);
											}
											else
											{
												text6 = "null";
											}
											text = string.Concat(new object[]
											{
												"update rackthermal_daily  set intakepeak = ",
												text6,
												"  where rack_id = ",
												current4,
												" and insert_time = #",
												DT_insert.ToString("yyyy-MM-dd"),
												"# "
											});
											if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
											{
												text = string.Concat(new object[]
												{
													"update rackthermal_daily",
													DT_insert.ToString("yyyyMMdd"),
													"  set intakepeak = ",
													text6,
													"  where rack_id = ",
													current4,
													" and insert_time = '",
													DT_insert.ToString("yyyy-MM-dd"),
													"' "
												});
											}
											dbCommand.CommandText = text;
											string arg_AE5_0 = dbCommand.CommandText;
											dbCommand.ExecuteNonQuery();
										}
									}
									if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
									{
										if (num8 < dictionary3[current4])
										{
											num8 = dictionary3[current4];
											flag = true;
										}
										if (flag)
										{
											dbCommand.Parameters.Clear();
											dbCommand.CommandType = CommandType.Text;
											string text7;
											if (num8 != -900.0)
											{
												text7 = string.Concat(num8);
											}
											else
											{
												text7 = "null";
											}
											text = string.Concat(new object[]
											{
												"update rackthermal_daily  set exhaustpeak = ",
												text7,
												"  where rack_id = ",
												current4,
												" and insert_time = #",
												DT_insert.ToString("yyyy-MM-dd"),
												"# "
											});
											if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
											{
												text = string.Concat(new object[]
												{
													"update rackthermal_daily",
													DT_insert.ToString("yyyyMMdd"),
													"  set exhaustpeak = ",
													text7,
													"  where rack_id = ",
													current4,
													" and insert_time = '",
													DT_insert.ToString("yyyy-MM-dd"),
													"' "
												});
											}
											dbCommand.CommandText = text;
											string arg_C51_0 = dbCommand.CommandText;
											dbCommand.ExecuteNonQuery();
										}
									}
								}
								else
								{
									dbCommand.Parameters.Clear();
									dbCommand.CommandType = CommandType.Text;
									if (rackThermalBitMap.b_difference)
									{
										double num10 = dictionary3[current4] - dictionary2[current4];
										if (num10 >= 0.0)
										{
											text = string.Concat(new object[]
											{
												"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
												current4,
												",",
												dictionary[current4],
												",",
												dictionary3[current4],
												",",
												dictionary3[current4] - dictionary2[current4],
												",'",
												DT_insert.ToString("yyyy-MM-dd"),
												"' )"
											});
										}
										else
										{
											text = string.Concat(new object[]
											{
												"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
												current4,
												",",
												dictionary[current4],
												",",
												dictionary3[current4],
												",null,'",
												DT_insert.ToString("yyyy-MM-dd"),
												"' )"
											});
										}
									}
									if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
									{
										text = string.Concat(new object[]
										{
											"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
											current4,
											",",
											dictionary[current4],
											",null,null,'",
											DT_insert.ToString("yyyy-MM-dd"),
											"' )"
										});
									}
									if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
									{
										text = string.Concat(new object[]
										{
											"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
											current4,
											",null,",
											dictionary3[current4],
											",null,'",
											DT_insert.ToString("yyyy-MM-dd"),
											"' )"
										});
									}
									if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
									{
										text = text.Replace("#", "'");
										text = text.Replace("rackthermal_daily", "rackthermal_daily" + DT_insert.ToString("yyyyMMdd"));
									}
									dbCommand.CommandText = text;
									string arg_F04_0 = dbCommand.CommandText;
									dbCommand.ExecuteNonQuery();
								}
								num7 = -900.0;
								num8 = -900.0;
								num9 = -900.0;
								DataRow[] array2 = dataTable.Select(" rack_id = " + current4);
								if (array2 != null && array2.Length > 0)
								{
									object obj2 = array2[0]["intakepeak"];
									if (obj2 == null || obj2 == DBNull.Value || obj2.ToString().ToLower().Equals("null"))
									{
										num7 = -900.0;
									}
									else
									{
										try
										{
											num7 = Convert.ToDouble(array2[0]["intakepeak"]);
										}
										catch
										{
										}
									}
									obj2 = array2[0]["exhaustpeak"];
									if (obj2 == null || obj2 == DBNull.Value || obj2.ToString().ToLower().Equals("null"))
									{
										num8 = -900.0;
									}
									else
									{
										try
										{
											num8 = Convert.ToDouble(array2[0]["exhaustpeak"]);
										}
										catch
										{
										}
									}
									obj2 = array2[0]["differencepeak"];
									if (obj2 == null || obj2 == DBNull.Value || obj2.ToString().ToLower().Equals("null"))
									{
										num9 = -900.0;
									}
									else
									{
										try
										{
											num9 = Convert.ToDouble(array2[0]["differencepeak"]);
										}
										catch
										{
										}
									}
									bool flag2 = false;
									if (rackThermalBitMap.b_difference)
									{
										if (num7 < dictionary[current4])
										{
											num7 = dictionary[current4];
											flag2 = true;
										}
										if (num8 < dictionary3[current4])
										{
											num8 = dictionary3[current4];
											flag2 = true;
										}
										if (num9 < dictionary3[current4] - dictionary2[current4])
										{
											num9 = dictionary3[current4] - dictionary2[current4];
											flag2 = true;
										}
										if (flag2)
										{
											dbCommand.Parameters.Clear();
											dbCommand.CommandType = CommandType.Text;
											string text8;
											if (num7 != -900.0)
											{
												text8 = string.Concat(num7);
											}
											else
											{
												text8 = "null";
											}
											string text9;
											if (num8 != -900.0)
											{
												text9 = string.Concat(num8);
											}
											else
											{
												text9 = "null";
											}
											string text10;
											if (num9 != -900.0 && num9 >= 0.0)
											{
												text10 = string.Concat(num9);
											}
											else
											{
												text10 = "null";
											}
											text2 = string.Concat(new object[]
											{
												"update rackthermal_hourly  set intakepeak = ",
												text8,
												", exhaustpeak = ",
												text9,
												",differencepeak = ",
												text10,
												" where rack_id = ",
												current4,
												" and insert_time = #",
												new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"# "
											});
											if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
											{
												text2 = text2.Replace("#", "'");
												text2 = string.Concat(new object[]
												{
													"update rackthermal_hourly",
													DT_insert.ToString("yyyyMMdd"),
													"  set intakepeak = ",
													text8,
													", exhaustpeak = ",
													text9,
													",differencepeak = ",
													text10,
													" where rack_id = ",
													current4,
													" and insert_time = '",
													new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
													"' "
												});
											}
											dbCommand.CommandText = text2;
											string arg_1313_0 = dbCommand.CommandText;
											dbCommand.ExecuteNonQuery();
										}
									}
									if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
									{
										if (num7 < dictionary[current4])
										{
											num7 = dictionary[current4];
											flag2 = true;
										}
										if (flag2)
										{
											dbCommand.Parameters.Clear();
											dbCommand.CommandType = CommandType.Text;
											string text11;
											if (num7 != -900.0)
											{
												text11 = string.Concat(num7);
											}
											else
											{
												text11 = "null";
											}
											text2 = string.Concat(new object[]
											{
												"update rackthermal_hourly  set intakepeak = ",
												text11,
												"  where rack_id = ",
												current4,
												" and insert_time = #",
												new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"# "
											});
											if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
											{
												text2 = text2.Replace("#", "'");
												text2 = text2.Replace("rackthermal_hourly", "rackthermal_hourly" + DT_insert.ToString("yyyyMMdd"));
											}
											dbCommand.CommandText = text2;
											string arg_146C_0 = dbCommand.CommandText;
											dbCommand.ExecuteNonQuery();
										}
									}
									if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
									{
										if (num8 < dictionary3[current4])
										{
											num8 = dictionary3[current4];
											flag2 = true;
										}
										if (flag2)
										{
											dbCommand.Parameters.Clear();
											dbCommand.CommandType = CommandType.Text;
											string text12;
											if (num8 != -900.0)
											{
												text12 = string.Concat(num8);
											}
											else
											{
												text12 = "null";
											}
											text2 = string.Concat(new object[]
											{
												"update rackthermal_hourly  set exhaustpeak = ",
												text12,
												"  where rack_id = ",
												current4,
												" and insert_time = #",
												new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"# "
											});
											if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
											{
												text2 = text2.Replace("#", "'");
												text2 = text2.Replace("rackthermal_hourly", "rackthermal_hourly" + DT_insert.ToString("yyyyMMdd"));
											}
											dbCommand.CommandText = text2;
											string arg_15C5_0 = dbCommand.CommandText;
											dbCommand.ExecuteNonQuery();
										}
									}
								}
								else
								{
									dbCommand.Parameters.Clear();
									dbCommand.CommandType = CommandType.Text;
									if (rackThermalBitMap.b_difference)
									{
										double num11 = dictionary3[current4] - dictionary2[current4];
										if (num11 >= 0.0)
										{
											text2 = string.Concat(new object[]
											{
												"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
												current4,
												",",
												dictionary[current4],
												",",
												dictionary3[current4],
												",",
												dictionary3[current4] - dictionary2[current4],
												",#",
												new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"# )"
											});
										}
										else
										{
											text2 = string.Concat(new object[]
											{
												"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
												current4,
												",",
												dictionary[current4],
												",",
												dictionary3[current4],
												",null,#",
												new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
												"# )"
											});
										}
									}
									if (rackThermalBitMap.b_intake && !rackThermalBitMap.b_exhaust)
									{
										text2 = string.Concat(new object[]
										{
											"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
											current4,
											",",
											dictionary[current4],
											",null,null,#",
											new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
											"# )"
										});
									}
									if (rackThermalBitMap.b_exhaust && !rackThermalBitMap.b_intake)
									{
										text2 = string.Concat(new object[]
										{
											"insert into rackthermal_hourly (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
											current4,
											",null,",
											dictionary3[current4],
											",null,#",
											new DateTime(DT_insert.Year, DT_insert.Month, DT_insert.Day, DT_insert.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
											"# )"
										});
									}
									if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
									{
										text2 = text2.Replace("#", "'");
										text2 = text2.Replace("rackthermal_hourly", "rackthermal_hourly" + DT_insert.ToString("yyyyMMdd"));
									}
									dbCommand.CommandText = text2;
									string arg_191F_0 = dbCommand.CommandText;
									dbCommand.ExecuteNonQuery();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("GenerateAllRackThermal throw exception : " + ex.Message);
				if (ex.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
				{
					DBUtil.SetMySQLInfo("rackthermal_daily" + DT_insert.ToString("yyyyMMdd"));
					DBUtil.SetMySQLInfo("rackthermal_hourly" + DT_insert.ToString("yyyyMMdd"));
					DebugCenter.GetInstance().appendToFile("MySQL database is marked as crashed, EcoSensor Monitor Service will be shutdown ");
					DBUtil.StopService();
				}
			}
			finally
			{
				RackInfo.Last_Update_Thermal = DateTime.Now;
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
		public static void GenerateAllRackRCI(string str_RCIHigh, string str_RCILo)
		{
			string str_sql = "";
			bool flag = false;
			bool flag2 = false;
			if (str_RCIHigh == null || str_RCIHigh.Length < 1)
			{
				flag = true;
			}
			if (str_RCILo == null || str_RCILo.Length < 1)
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				return;
			}
			double num = 0.0;
			double num2 = 0.0;
			if (!flag)
			{
				num = Convert.ToDouble(str_RCIHigh);
			}
			if (!flag2)
			{
				num2 = Convert.ToDouble(str_RCILo);
			}
			bool flag3 = true;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			string commandText = "select * from rci_daily where insert_time = #" + DateTime.Now.ToString("yyyy-MM-dd") + "#";
			string commandText2 = "select * from rci_hourly where insert_time = #" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss") + "#";
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				commandText = "select * from rci_daily where insert_time = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
				commandText2 = "select * from rci_hourly where insert_time = '" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss") + "'";
			}
			try
			{
				if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					dBConn = DBConnPool.getDynaConnection();
				}
				else
				{
					dBConn = DBConnPool.getThermalConnection();
				}
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText;
					str_sql = dbCommand.CommandText;
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					string text = "";
					if (dbDataReader.Read())
					{
						flag3 = false;
						bool flag4 = false;
						string text2 = "null";
						string text3 = "null";
						DateTime dateTime = Convert.ToDateTime(dbDataReader.GetValue(2));
						object value = dbDataReader.GetValue(0);
						if (value == null || value is DBNull)
						{
							if (!flag)
							{
								text2 = string.Concat(num);
								flag4 = true;
							}
						}
						else
						{
							double num3 = Convert.ToDouble(value);
							if (!flag && num < num3)
							{
								num3 = num;
								flag4 = true;
							}
							text2 = string.Concat(num3);
						}
						value = dbDataReader.GetValue(1);
						if (value == null || value is DBNull)
						{
							if (!flag2)
							{
								text3 = string.Concat(num2);
								flag4 = true;
							}
						}
						else
						{
							double num4 = Convert.ToDouble(value);
							if (!flag2 && num2 < num4)
							{
								num4 = num2;
								flag4 = true;
							}
							text3 = string.Concat(num4);
						}
						if (flag4)
						{
							text = string.Concat(new string[]
							{
								"update rci_daily set rci_high = ",
								text2,
								",rci_lo = ",
								text3,
								" where insert_time = #",
								dateTime.ToString("yyyy-MM-dd"),
								"#"
							});
							if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
							{
								text = string.Concat(new string[]
								{
									"update rci_daily set rci_high = ",
									text2,
									",rci_lo = ",
									text3,
									" where insert_time = '",
									dateTime.ToString("yyyy-MM-dd"),
									"'"
								});
							}
						}
					}
					dbDataReader.Close();
					dbDataReader.Dispose();
					if (text.Length > 1)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = text;
						str_sql = dbCommand.CommandText;
						dbCommand.ExecuteNonQuery();
					}
					if (flag3)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandType = CommandType.Text;
						string text4 = "null";
						string text5 = "null";
						if (!flag)
						{
							text4 = string.Concat(num);
						}
						if (!flag2)
						{
							text5 = string.Concat(num2);
						}
						dbCommand.CommandText = string.Concat(new string[]
						{
							"insert into rci_daily (rci_high,rci_lo,insert_time) values (",
							text4,
							",",
							text5,
							",#",
							DateTime.Now.ToString("yyyy-MM-dd"),
							"# )"
						});
						if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							dbCommand.CommandText = string.Concat(new string[]
							{
								"insert into rci_daily (rci_high,rci_lo,insert_time) values (",
								text4,
								",",
								text5,
								",'",
								DateTime.Now.ToString("yyyy-MM-dd"),
								"' )"
							});
						}
						str_sql = dbCommand.CommandText;
						dbCommand.ExecuteNonQuery();
					}
					text = "";
					flag3 = true;
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText2;
					str_sql = dbCommand.CommandText;
					DbDataReader dbDataReader2 = dbCommand.ExecuteReader();
					if (dbDataReader2.Read())
					{
						flag3 = false;
						bool flag5 = false;
						string text6 = "null";
						string text7 = "null";
						DateTime dateTime2 = Convert.ToDateTime(dbDataReader2.GetValue(2));
						object value2 = dbDataReader2.GetValue(0);
						if (value2 == null || value2 is DBNull)
						{
							if (!flag)
							{
								text6 = string.Concat(num);
								flag5 = true;
							}
						}
						else
						{
							double num5 = Convert.ToDouble(value2);
							if (!flag && num < num5)
							{
								num5 = num;
								flag5 = true;
							}
							text6 = string.Concat(num5);
						}
						value2 = dbDataReader2.GetValue(1);
						if (value2 == null || value2 is DBNull)
						{
							if (!flag2)
							{
								text7 = string.Concat(num2);
								flag5 = true;
							}
						}
						else
						{
							double num6 = Convert.ToDouble(value2);
							if (!flag2 && num2 < num6)
							{
								num6 = num2;
								flag5 = true;
							}
							text7 = string.Concat(num6);
						}
						if (flag5)
						{
							text = string.Concat(new string[]
							{
								"update rci_hourly set rci_high = ",
								text6,
								",rci_lo = ",
								text7,
								" where insert_time = #",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"#"
							});
							if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
							{
								text = string.Concat(new string[]
								{
									"update rci_hourly set rci_high = ",
									text6,
									",rci_lo = ",
									text7,
									" where insert_time = '",
									dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
									"'"
								});
							}
						}
					}
					dbDataReader2.Close();
					dbDataReader2.Dispose();
					if (text.Length > 1)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = text;
						str_sql = dbCommand.CommandText;
						dbCommand.ExecuteNonQuery();
					}
					if (flag3)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandType = CommandType.Text;
						string text8 = "null";
						string text9 = "null";
						if (!flag)
						{
							text8 = string.Concat(num);
						}
						if (!flag2)
						{
							text9 = string.Concat(num2);
						}
						dbCommand.CommandText = string.Concat(new string[]
						{
							"insert into rci_hourly (rci_high,rci_lo,insert_time) values (",
							text8,
							",",
							text9,
							",#",
							new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
							"# )"
						});
						if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							dbCommand.CommandText = string.Concat(new string[]
							{
								"insert into rci_hourly (rci_high,rci_lo,insert_time) values (",
								text8,
								",",
								text9,
								",'",
								new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 30, 0).ToString("yyyy-MM-dd HH:mm:ss"),
								"' )"
							});
						}
						str_sql = dbCommand.CommandText;
						dbCommand.ExecuteNonQuery();
					}
				}
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (ex.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
				{
					string tableName = DBUtil.GetTableName(str_sql);
					if (tableName != null && tableName.Length > 0)
					{
						DBUtil.SetMySQLInfo(tableName);
						DebugCenter.GetInstance().appendToFile("MySQL database is marked as crashed, EcoSensor Monitor Service will be shutdown ");
						DBUtil.StopService();
					}
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
		}
	}
}
