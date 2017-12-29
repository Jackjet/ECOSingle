using CommonAPI;
using CommonAPI.CultureTransfer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace DBAccessAPI
{
	public class LineInfo
	{
		private int id;
		private int device_id;
		private string line_name = "";
		private int line_number;
		private float max_voltage;
		private float min_voltage;
		private float max_power;
		private float min_power;
		private float max_current;
		private float min_current;
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
		public string LineName
		{
			get
			{
				return this.line_name;
			}
			set
			{
				this.line_name = value;
			}
		}
		public int LineNumber
		{
			get
			{
				return this.line_number;
			}
			set
			{
				this.line_number = value;
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
		public LineInfo(int l_id, int l_did, string str_name, int i_number, float f_max_v, float f_min_v, float f_max_p, float f_min_p, float f_max_c, float f_min_c)
		{
			this.id = l_id;
			this.device_id = l_did;
			this.line_name = str_name;
			this.line_number = i_number;
			this.max_voltage = f_max_v;
			this.min_voltage = f_min_v;
			this.max_power = f_max_p;
			this.min_power = f_min_p;
			this.max_current = f_max_c;
			this.min_current = f_min_c;
		}
		public LineInfo(int l_did, int i_linenum)
		{
			try
			{
				Hashtable deviceLineMap = DBCache.GetDeviceLineMap();
				Hashtable lineCache = DBCache.GetLineCache();
				if (deviceLineMap != null && deviceLineMap.Count > 0 && lineCache != null && lineCache.Count > 0 && deviceLineMap.ContainsKey(l_did))
				{
					List<int> list = (List<int>)deviceLineMap[l_did];
					if (list != null)
					{
						foreach (int current in list)
						{
							LineInfo lineInfo = (LineInfo)lineCache[current];
							if (lineInfo != null && lineInfo.LineNumber == i_linenum)
							{
								this.id = lineInfo.ID;
								this.device_id = lineInfo.DeviceID;
								this.line_name = lineInfo.LineName;
								this.line_number = lineInfo.LineNumber;
								this.max_voltage = lineInfo.Max_voltage;
								this.min_voltage = lineInfo.Min_voltage;
								this.max_power = lineInfo.Max_power;
								this.min_power = lineInfo.Min_power;
								this.max_current = lineInfo.Max_current;
								this.min_current = lineInfo.Min_current;
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
		public LineInfo(DataRow tmp_dr_b)
		{
			if (tmp_dr_b != null)
			{
				this.id = Convert.ToInt32(tmp_dr_b["id"]);
				this.device_id = Convert.ToInt32(tmp_dr_b["device_id"]);
				this.line_name = Convert.ToString(tmp_dr_b["line_name"]);
				this.line_number = Convert.ToInt32(tmp_dr_b["line_number"]);
				this.max_voltage = Convert.ToSingle(tmp_dr_b["max_voltage"]);
				this.min_voltage = Convert.ToSingle(tmp_dr_b["min_voltage"]);
				this.max_power = Convert.ToSingle(tmp_dr_b["max_power"]);
				this.min_power = Convert.ToSingle(tmp_dr_b["min_power"]);
				this.max_current = Convert.ToSingle(tmp_dr_b["max_current"]);
				this.min_current = Convert.ToSingle(tmp_dr_b["min_current"]);
			}
		}
		public void CopyThreshold(LineInfo tmp_bi)
		{
			this.max_voltage = tmp_bi.Max_voltage;
			this.min_voltage = tmp_bi.Min_voltage;
			this.max_power = tmp_bi.Max_power;
			this.min_power = tmp_bi.Min_power;
			this.max_current = tmp_bi.Max_current;
			this.min_current = tmp_bi.Min_current;
		}
		public int UpdateLineThreshold(DBConn conn)
		{
			if (this.id < 1)
			{
				return -1;
			}
			if (this.device_id < 1)
			{
				return -1;
			}
			DbCommand dbCommand = null;
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.Connection = conn.con;
					dbCommand.CommandType = CommandType.Text;
					string text = "update line_info set ";
					text = text + "max_voltage=" + CultureTransfer.ToString(this.max_voltage);
					text = text + ",min_voltage=" + CultureTransfer.ToString(this.min_voltage);
					text = text + ",max_power=" + CultureTransfer.ToString(this.max_power);
					text = text + ",min_power=" + CultureTransfer.ToString(this.min_power);
					text = text + ",max_current=" + CultureTransfer.ToString(this.max_current);
					text = text + ",min_current=" + CultureTransfer.ToString(this.min_current);
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
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
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
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					string commandText = "update line_info set max_voltage=?,min_voltage=?,max_power=?,min_power=?,max_current=?,min_current=?  where id=" + this.id;
					dbCommand.CommandText = commandText;
					dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
					dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
					dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
					dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
					dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
					dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
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
	}
}
