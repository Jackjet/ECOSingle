using CommonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class ZoneInfo
	{
		private long z_id;
		private string z_name = string.Empty;
		private string z_racklist = string.Empty;
		private long[] z_rackarr;
		private int z_s_x;
		private int z_s_y;
		private int z_e_x;
		private int z_e_y;
		private string z_color = string.Empty;
		private int z_reserve;
		public long ZoneID
		{
			get
			{
				return this.z_id;
			}
		}
		public string ZoneName
		{
			get
			{
				return this.z_name;
			}
			set
			{
				this.z_name = value;
			}
		}
		public string RackInfo
		{
			get
			{
				return this.z_racklist;
			}
			set
			{
				this.z_racklist = value;
			}
		}
		public int StartPointX
		{
			get
			{
				return this.z_s_x;
			}
			set
			{
				this.z_s_x = value;
			}
		}
		public int StartPointY
		{
			get
			{
				return this.z_s_y;
			}
			set
			{
				this.z_s_y = value;
			}
		}
		public int EndPointX
		{
			get
			{
				return this.z_e_x;
			}
			set
			{
				this.z_e_x = value;
			}
		}
		public int EndPointY
		{
			get
			{
				return this.z_e_y;
			}
			set
			{
				this.z_e_y = value;
			}
		}
		public string ZoneColor
		{
			get
			{
				return this.z_color;
			}
			set
			{
				this.z_color = value;
			}
		}
		public ZoneInfo()
		{
		}
		public ZoneInfo(long l_id, string str_name, string str_racklist, int i_sx, int i_sy, int i_ex, int i_ey, string str_color)
		{
			this.z_id = l_id;
			this.z_name = str_name;
			this.z_racklist = str_racklist;
			this.z_s_x = i_sx;
			this.z_s_y = i_sy;
			this.z_e_x = i_ex;
			this.z_e_y = i_ey;
			this.z_color = str_color;
		}
		public ZoneInfo(DataRow tmp_dr_z)
		{
			if (tmp_dr_z != null)
			{
				this.z_id = Convert.ToInt64(tmp_dr_z["id"]);
				this.z_name = Convert.ToString(tmp_dr_z["zone_nm"]);
				this.z_racklist = Convert.ToString(tmp_dr_z["racks"]);
				this.z_s_x = (int)Convert.ToInt16(tmp_dr_z["sx"]);
				this.z_s_y = (int)Convert.ToInt16(tmp_dr_z["sy"]);
				this.z_e_x = (int)Convert.ToInt16(tmp_dr_z["ex"]);
				this.z_e_y = (int)Convert.ToInt16(tmp_dr_z["ey"]);
				this.z_color = Convert.ToString(tmp_dr_z["color"]);
			}
		}
		public ZoneInfo(DataTable tmp_dr_z)
		{
			DataRow[] array = tmp_dr_z.Select();
			if (array != null && array.Length > 0)
			{
				this.z_id = Convert.ToInt64(array[0]["id"]);
				this.z_name = Convert.ToString(array[0]["zone_nm"]);
				this.z_racklist = Convert.ToString(array[0]["racks"]);
				this.z_s_x = (int)Convert.ToInt16(array[0]["sx"]);
				this.z_s_y = (int)Convert.ToInt16(array[0]["sy"]);
				this.z_e_x = (int)Convert.ToInt16(array[0]["ex"]);
				this.z_e_y = (int)Convert.ToInt16(array[0]["ey"]);
				this.z_color = Convert.ToString(array[0]["color"]);
			}
		}
		public static int CreateZoneInfo(string str_name, string str_racklist, int i_sx, int i_sy, int i_ex, int i_ey, string str_color)
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
						string commandText = "insert into zone_info ( zone_nm,racks,sx,sy,ex,ey,color,reserve ) values(?zone_nm,?racks,?sx,?sy,?ex,?ey,?color,?reserve)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Clear();
						dbCommand.Parameters.Add(DBTools.GetParameter("?zone_nm", str_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?racks", str_racklist, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sx", i_sx, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sy", i_sy, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ex", i_ex, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ey", i_ey, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?color", str_color, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?reserve", Convert.ToInt32("0"), dbCommand));
					}
					else
					{
						string commandText = "insert into zone_info ( zone_nm,racks,sx,sy,ex,ey,color,reserve ) values(?,?,?,?,?,?,?,?)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Clear();
						dbCommand.Parameters.Add(DBTools.GetParameter("@zone_nm", str_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@racks", str_racklist, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sx", i_sx, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sy", i_sy, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ex", i_ex, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ey", i_ey, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@color", str_color, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@reserve", Convert.ToInt32("0"), dbCommand));
					}
					int num = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					DBCacheStatus.ZONE = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
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
		public int UpdateZone()
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
						string commandText = "update zone_info set zone_nm=?zone_nm , racks=?racks , sx=?sx ,sy=?sy, ex=?ex ,ey=?ey, color=?color , reserve=?reserve where id= " + this.z_id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?zone_nm", this.z_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?racks", this.z_racklist, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sx", this.z_s_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?sy", this.z_s_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ex", this.z_e_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ey", this.z_e_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?color", this.z_color, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?reserve", this.z_reserve, dbCommand));
					}
					else
					{
						string commandText = "update zone_info set zone_nm=? , racks=? , sx=? ,sy=?, ex=? ,ey=?, color=? , reserve=? where id= " + this.z_id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@zone_nm", this.z_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@racks", this.z_racklist, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sx", this.z_s_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@sy", this.z_s_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ex", this.z_e_x, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ey", this.z_e_y, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@color", this.z_color, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@reserve", this.z_reserve, dbCommand));
					}
					int num = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					DBCacheStatus.ZONE = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
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
		public static int DeleteByName(string str_zonename)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					string commandText = "select * from zone_info z inner join group_detail g on z.id = g.dest_id where z.zone_nm = '" + str_zonename + "' and g.grouptype = 'zone'";
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = commandText;
					DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn.con);
					dataAdapter.SelectCommand = dbCommand;
					DataTable dataTable = new DataTable();
					dataAdapter.Fill(dataTable);
					if (dataTable.Rows.Count > 0)
					{
						string commandText2 = string.Concat(new object[]
						{
							"delete from group_detail where grouptype = 'zone' and group_id = ",
							dataTable.Rows[0]["group_id"],
							" and dest_id =",
							dataTable.Rows[0]["dest_id"]
						});
						dbCommand.CommandText = commandText2;
						dbCommand.ExecuteNonQuery();
					}
					string commandText3 = "delete from zone_info where zone_nm = '" + str_zonename + "'";
					dbCommand.CommandText = commandText3;
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					DBCacheStatus.ZONE = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
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
		public static ArrayList getAllZone()
		{
			List<ZoneInfo> list = new List<ZoneInfo>();
			try
			{
				Hashtable zoneCache = DBCache.GetZoneCache();
				if (zoneCache != null && zoneCache.Count > 0)
				{
					ICollection values = zoneCache.Values;
					foreach (ZoneInfo item in values)
					{
						list.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get all zone error : " + ex.Message + "\n" + ex.StackTrace);
			}
			list.Sort((ZoneInfo x, ZoneInfo y) => x.z_name.CompareTo(y.z_name));
			ArrayList arrayList = new ArrayList();
			foreach (ZoneInfo current in list)
			{
				arrayList.Add(current);
			}
			return arrayList;
		}
		public static ZoneInfo getZoneByID(long l_id)
		{
			ZoneInfo result = null;
			try
			{
				Hashtable zoneCache = DBCache.GetZoneCache();
				if (zoneCache != null && zoneCache.Count > 0 && zoneCache.ContainsKey(l_id))
				{
					result = (ZoneInfo)zoneCache[l_id];
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Get all zone error : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static bool CheckName(long l_id, string str_name)
		{
			try
			{
				Hashtable zoneCache = DBCache.GetZoneCache();
				if (zoneCache != null && zoneCache.Count > 0)
				{
					ICollection values = zoneCache.Values;
					foreach (ZoneInfo zoneInfo in values)
					{
						long zoneID = zoneInfo.ZoneID;
						string zoneName = zoneInfo.ZoneName;
						if (zoneName.Equals(str_name))
						{
							bool result;
							if (zoneID == l_id)
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
			}
			return true;
		}
		public static bool CheckColor(long l_id, int i_color)
		{
			try
			{
				Hashtable zoneCache = DBCache.GetZoneCache();
				if (zoneCache != null && zoneCache.Count > 0)
				{
					ICollection values = zoneCache.Values;
					foreach (ZoneInfo zoneInfo in values)
					{
						long zoneID = zoneInfo.ZoneID;
						string zoneColor = zoneInfo.ZoneColor;
						if (zoneColor.Equals(string.Concat(i_color)))
						{
							bool result;
							if (zoneID == l_id)
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
			}
			return true;
		}
	}
}
