using CommonAPI;
using CommonAPI.CultureTransfer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace DBAccessAPI
{
	public class BankInfo
	{
		private int id;
		private int device_id;
		private int voltage = 110;
		private string portlists = string.Empty;
		private string bank_nm = string.Empty;
		private float max_voltage;
		private float min_voltage;
		private float max_power_diss;
		private float min_power_diss;
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
		public string BankName
		{
			get
			{
				return this.bank_nm;
			}
			set
			{
				this.bank_nm = value;
			}
		}
		public int Voltage
		{
			get
			{
				return this.voltage;
			}
			set
			{
				this.voltage = value;
			}
		}
		public string PortLists
		{
			get
			{
				return this.portlists;
			}
			set
			{
				this.portlists = value;
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
		public BankInfo(int l_did, int i_banknum)
		{
			try
			{
				Hashtable deviceBankMap = DBCache.GetDeviceBankMap();
				Hashtable bankCache = DBCache.GetBankCache();
				if (deviceBankMap != null && deviceBankMap.Count > 0 && bankCache != null && bankCache.Count > 0 && deviceBankMap.ContainsKey(l_did))
				{
					List<int> list = (List<int>)deviceBankMap[l_did];
					if (list != null)
					{
						foreach (int current in list)
						{
							BankInfo bankInfo = (BankInfo)bankCache[current];
							if (bankInfo != null && bankInfo.PortLists.Equals(string.Concat(i_banknum)))
							{
								this.id = bankInfo.ID;
								this.device_id = bankInfo.DeviceID;
								this.voltage = bankInfo.Voltage;
								this.portlists = bankInfo.PortLists;
								this.bank_nm = bankInfo.BankName;
								this.max_voltage = bankInfo.Max_voltage;
								this.min_voltage = bankInfo.Min_voltage;
								this.max_power_diss = bankInfo.Max_power_diss;
								this.min_power_diss = bankInfo.Min_power_diss;
								this.max_power = bankInfo.Max_power;
								this.min_power = bankInfo.Min_power;
								this.max_current = bankInfo.Max_current;
								this.min_current = bankInfo.Min_current;
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
		public BankInfo(int l_id, int l_did, int i_voltage, string str_ports, string str_name, float f_max_v, float f_min_v, float f_max_pd, float f_min_pd, float f_max_p, float f_min_p, float f_max_c, float f_min_c)
		{
			this.id = l_id;
			this.device_id = l_did;
			this.voltage = i_voltage;
			this.portlists = str_ports;
			this.bank_nm = str_name;
			this.max_voltage = f_max_v;
			this.min_voltage = f_min_v;
			this.max_power_diss = f_max_pd;
			this.min_power_diss = f_min_pd;
			this.max_power = f_max_p;
			this.min_power = f_min_p;
			this.max_current = f_max_c;
			this.min_current = f_min_c;
		}
		public void CopyThreshold(BankInfo tmp_bi)
		{
			this.max_voltage = tmp_bi.Max_voltage;
			this.min_voltage = tmp_bi.Min_voltage;
			this.max_power_diss = tmp_bi.Max_power_diss;
			this.min_power_diss = tmp_bi.Min_power_diss;
			this.max_power = tmp_bi.Max_power;
			this.min_power = tmp_bi.Min_power;
			this.max_current = tmp_bi.Max_current;
			this.min_current = tmp_bi.Min_current;
		}
		public int UpdateBankThreshold(DBConn conn)
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
					string text = "update bank_info set bank_nm=?,voltage=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?  where id=" + this.id;
					text = "update bank_info set ";
					if (!this.bank_nm.Equals("\r\n"))
					{
						text = text + "bank_nm='" + this.bank_nm + "',";
					}
					text = text + "voltage=" + CultureTransfer.ToString(this.voltage);
					text = text + ",max_voltage=" + CultureTransfer.ToString(this.max_voltage);
					text = text + ",min_voltage=" + CultureTransfer.ToString(this.min_voltage);
					text = text + ",max_power_diss=" + CultureTransfer.ToString(this.max_power_diss);
					text = text + ",min_power_diss=" + CultureTransfer.ToString(this.min_power_diss);
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
			DbCommand dbCommand = null;
			try
			{
				if (conn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(conn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						string commandText = "update bank_info set port_nums=?port_nums,bank_nm=?bank_nm,voltage=?voltage,max_voltage=?max_voltage,min_voltage=?min_voltage,max_power_diss=?max_power_diss,min_power_diss=?min_power_diss,max_power=?max_power,min_power=?min_power,max_current=?max_current,min_current=?min_current  where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_nums", this.portlists, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?bank_nm", this.bank_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?voltage", this.voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", this.min_current, dbCommand));
					}
					else
					{
						if (this.bank_nm != null && this.bank_nm.Equals("\r\n"))
						{
							string commandText = "update bank_info set port_nums=?,voltage=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?  where id=" + this.id;
							dbCommand.CommandText = commandText;
							dbCommand.Parameters.Add(DBTools.GetParameter("@port_nums", this.portlists, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@voltage", this.voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						}
						else
						{
							string commandText = "update bank_info set port_nums=?,bank_nm=?,voltage=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?  where id=" + this.id;
							dbCommand.CommandText = commandText;
							dbCommand.Parameters.Add(DBTools.GetParameter("@port_nums", this.portlists, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@bank_nm", this.bank_nm, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@voltage", this.voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						}
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
					if (DBUrl.SERVERMODE)
					{
						string commandText = "update bank_info set port_nums=?port_nums,bank_nm=?bank_nm,voltage=?voltage,max_voltage=?max_voltage,min_voltage=?min_voltage,max_power_diss=?max_power_diss,min_power_diss=?min_power_diss,max_power=?max_power,min_power=?min_power,max_current=?max_current,min_current=?min_current  where id=" + this.id;
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?port_nums", this.portlists, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?bank_nm", this.bank_nm, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?voltage", this.voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_voltage", this.max_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_voltage", this.min_voltage, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power_diss", this.max_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power_diss", this.min_power_diss, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_power", this.max_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_power", this.min_power, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?max_current", this.max_current, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?min_current", this.min_current, dbCommand));
					}
					else
					{
						if (this.bank_nm != null && this.bank_nm.Equals("\r\n"))
						{
							string commandText = "update bank_info set port_nums=?,voltage=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?  where id=" + this.id;
							dbCommand.CommandText = commandText;
							dbCommand.Parameters.Add(DBTools.GetParameter("@port_nums", this.portlists, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@voltage", this.voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						}
						else
						{
							string commandText = "update bank_info set port_nums=?,bank_nm=?,voltage=?,max_voltage=?,min_voltage=?,max_power_diss=?,min_power_diss=?,max_power=?,min_power=?,max_current=?,min_current=?  where id=" + this.id;
							dbCommand.CommandText = commandText;
							dbCommand.Parameters.Add(DBTools.GetParameter("@port_nums", this.portlists, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@bank_nm", this.bank_nm, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@voltage", this.voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_voltage", this.max_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_voltage", this.min_voltage, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power_diss", this.max_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power_diss", this.min_power_diss, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_power", this.max_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_power", this.min_power, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@max_current", this.max_current, dbCommand));
							dbCommand.Parameters.Add(DBTools.GetParameter("@min_current", this.min_current, dbCommand));
						}
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
		public BankInfo(DataRow tmp_dr_b)
		{
			if (tmp_dr_b != null)
			{
				this.id = Convert.ToInt32(tmp_dr_b["id"]);
				this.device_id = Convert.ToInt32(tmp_dr_b["device_id"]);
				this.voltage = Convert.ToInt32(tmp_dr_b["voltage"]);
				this.portlists = Convert.ToString(tmp_dr_b["port_nums"]);
				this.bank_nm = Convert.ToString(tmp_dr_b["bank_nm"]);
				this.max_voltage = Convert.ToSingle(tmp_dr_b["max_voltage"]);
				this.min_voltage = Convert.ToSingle(tmp_dr_b["min_voltage"]);
				this.max_power_diss = Convert.ToSingle(tmp_dr_b["max_power_diss"]);
				this.min_power_diss = Convert.ToSingle(tmp_dr_b["min_power_diss"]);
				this.max_power = Convert.ToSingle(tmp_dr_b["max_power"]);
				this.min_power = Convert.ToSingle(tmp_dr_b["min_power"]);
				this.max_current = Convert.ToSingle(tmp_dr_b["max_current"]);
				this.min_current = Convert.ToSingle(tmp_dr_b["min_current"]);
			}
		}
	}
}
