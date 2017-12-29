using CommonAPI;
using CommonAPI.Tools;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class InSnergyGateway
	{
		public const int ITPOWER_HOUR = 0;
		public const int NONITPOWER_HOUR = 1;
		public const int ITPOWER_DAY = 2;
		public const int NONITPOWER_DAY = 3;
		public const int ITPOWER_WEEK = 4;
		public const int NONITPOWER_WEEK = 5;
		private static object _lock_last = new object();
		private static double d_IT_HOUR = 0.0;
		private static double d_TT_HOUR = 0.0;
		private static double d_IT_DAY = 0.0;
		private static double d_TT_DAY = 0.0;
		private static double d_IT_WEEK = 0.0;
		private static double d_TT_WEEK = 0.0;
		private static bool b_need = true;
		private static object _lockneed = new object();
		private string gid;
		private string gname;
		private string type;
		private string ip;
		private List<Branch> list_children = new List<Branch>();
		public static double Last_IT_HOUR
		{
			get
			{
				double result;
				lock (InSnergyGateway._lock_last)
				{
					result = InSnergyGateway.d_IT_HOUR;
				}
				return result;
			}
			set
			{
				lock (InSnergyGateway._lock_last)
				{
					InSnergyGateway.d_IT_HOUR = value;
				}
			}
		}
		public static double Last_TT_HOUR
		{
			get
			{
				double result;
				lock (InSnergyGateway._lock_last)
				{
					result = InSnergyGateway.d_TT_HOUR;
				}
				return result;
			}
			set
			{
				lock (InSnergyGateway._lock_last)
				{
					InSnergyGateway.d_TT_HOUR = value;
				}
			}
		}
		public static double Last_IT_DAY
		{
			get
			{
				double result;
				lock (InSnergyGateway._lock_last)
				{
					result = InSnergyGateway.d_IT_DAY;
				}
				return result;
			}
			set
			{
				lock (InSnergyGateway._lock_last)
				{
					InSnergyGateway.d_IT_DAY = value;
				}
			}
		}
		public static double Last_TT_DAY
		{
			get
			{
				double result;
				lock (InSnergyGateway._lock_last)
				{
					result = InSnergyGateway.d_TT_DAY;
				}
				return result;
			}
			set
			{
				lock (InSnergyGateway._lock_last)
				{
					InSnergyGateway.d_TT_DAY = value;
				}
			}
		}
		public static double Last_IT_WEEK
		{
			get
			{
				double result;
				lock (InSnergyGateway._lock_last)
				{
					result = InSnergyGateway.d_IT_WEEK;
				}
				return result;
			}
			set
			{
				lock (InSnergyGateway._lock_last)
				{
					InSnergyGateway.d_IT_WEEK = value;
				}
			}
		}
		public static double Last_TT_WEEK
		{
			get
			{
				double result;
				lock (InSnergyGateway._lock_last)
				{
					result = InSnergyGateway.d_TT_WEEK;
				}
				return result;
			}
			set
			{
				lock (InSnergyGateway._lock_last)
				{
					InSnergyGateway.d_TT_WEEK = value;
				}
			}
		}
		public static bool Need_Calculate_PUE
		{
			get
			{
				bool result;
				lock (InSnergyGateway._lockneed)
				{
					result = InSnergyGateway.b_need;
				}
				return result;
			}
			set
			{
				lock (InSnergyGateway._lockneed)
				{
					InSnergyGateway.b_need = value;
				}
			}
		}
		public string GatewayID
		{
			get
			{
				return this.gid;
			}
			set
			{
				this.gid = value;
			}
		}
		public string GatewayName
		{
			get
			{
				return this.gname;
			}
			set
			{
				this.gname = value;
			}
		}
		public string GatewayType
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}
		public string IP
		{
			get
			{
				return this.ip;
			}
			set
			{
				this.ip = value;
			}
		}
		public List<Branch> BranchList
		{
			get
			{
				return this.list_children;
			}
			set
			{
				this.list_children = value;
			}
		}
		public InSnergyGateway()
		{
			this.gid = "";
			this.gname = "";
			this.type = "";
			this.ip = "";
			this.list_children = new List<Branch>();
		}
		public InSnergyGateway(string str_id, string str_name, string str_type, string str_ip, List<Branch> list_branch)
		{
			this.gid = str_id;
			this.gname = str_name;
			this.type = str_type;
			this.ip = str_ip;
			this.list_children = list_branch;
		}
		public int Insert()
		{
			DbTransaction dbTransaction = null;
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			try
			{
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbTransaction = dBConn.con.BeginTransaction();
				dbCommand.Transaction = dbTransaction;
				int num = 1;
				if (this.list_children != null && this.list_children.Count > 0)
				{
					foreach (Branch current in this.list_children)
					{
						if (current.SubMeterList != null && current.SubMeterList.Count > 0)
						{
							foreach (SubMeter current2 in current.SubMeterList)
							{
								dbCommand.CommandText = string.Concat(new object[]
								{
									"insert into gatewaytable (gid,bid,sid,slevel,disname,capacity,eleflag) values ('",
									current2.GatewayID,
									"','",
									current2.BranchID,
									"','",
									current2.SubmeterID,
									"',2,'Meter",
									current2.SubmeterID,
									"',",
									0f,
									",",
									current2.ElectricityUsage,
									" )"
								});
								dbCommand.ExecuteNonQuery();
							}
						}
						dbCommand.CommandText = string.Concat(new object[]
						{
							"insert into gatewaytable (gid,bid,sid,slevel,disname,location) values ('",
							current.GatewayID,
							"','",
							current.BranchID,
							"','0',1,'Branch",
							num,
							"','",
							current.Location,
							"' )"
						});
						dbCommand.ExecuteNonQuery();
						num++;
					}
				}
				dbCommand.CommandText = string.Concat(new string[]
				{
					"insert into gatewaytable (gid,bid,sid,slevel,disname,distype,ip) values ('",
					this.gid,
					"','0','0',0,'",
					this.gname,
					"','",
					this.type,
					"','",
					this.ip,
					"' )"
				});
				dbCommand.ExecuteNonQuery();
				dbCommand.Transaction.Commit();
				dbTransaction.Dispose();
				dbCommand.Dispose();
				dBConn.Close();
				return 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			return -1;
		}
		public static int DeleteGateway(string str_gid, OleDbConnection con)
		{
			OleDbCommand oleDbCommand = null;
			OleDbTransaction oleDbTransaction = null;
			int result;
			try
			{
				oleDbCommand = con.CreateCommand();
				oleDbTransaction = con.BeginTransaction();
				oleDbCommand.Transaction = oleDbTransaction;
				oleDbCommand.CommandText = "delete from gatewaylastpd where bid in (select distinct bid from gatewaytable where gid = '" + str_gid + "') ";
				oleDbCommand.ExecuteNonQuery();
				oleDbCommand.CommandText = "delete from gatewaytable where gid='" + str_gid + "'";
				oleDbCommand.ExecuteNonQuery();
				oleDbCommand.Transaction.Commit();
				oleDbTransaction.Dispose();
				oleDbCommand.Dispose();
				result = 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Transaction.Rollback();
						oleDbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (oleDbTransaction != null)
				{
					try
					{
						oleDbTransaction.Dispose();
					}
					catch (Exception)
					{
					}
				}
				result = -1;
			}
			return result;
		}
		public static int DeleteGateway(string str_gid)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbTransaction dbTransaction = null;
			int result;
			try
			{
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbTransaction = dBConn.con.BeginTransaction();
				dbCommand.Transaction = dbTransaction;
				dbCommand.CommandText = "delete from gatewaylastpd where bid in (select distinct bid from gatewaytable where gid = '" + str_gid + "') ";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "delete from gatewaytable where gid='" + str_gid + "'";
				dbCommand.ExecuteNonQuery();
				dbCommand.Transaction.Commit();
				dbTransaction.Dispose();
				dbCommand.Dispose();
				dBConn.Close();
				result = 1;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Transaction.Rollback();
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
				result = -1;
			}
			return result;
		}
		public static List<string> GetAllGatewayIDList()
		{
			List<string> list = new List<string>();
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			DbDataReader dbDataReader = null;
			try
			{
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandText = "select distinct gid from gatewaytable where slevel = 0";
				dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.HasRows)
				{
					while (dbDataReader.Read())
					{
						list.Add(Convert.ToString(dbDataReader.GetValue(0)));
					}
				}
				dbDataReader.Close();
				dbCommand.Dispose();
				dBConn.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			return list;
		}
		public static int ModifyName(string str_gid, string str_name)
		{
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandText = string.Concat(new string[]
				{
					"update gatewaytable set disname = '",
					str_name,
					"' where slevel = 0 and gid = '",
					str_gid,
					"' "
				});
				int result = dbCommand.ExecuteNonQuery();
				dbCommand.Dispose();
				dBConn.Close();
				return result;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			return -1;
		}
		public static InSnergyGateway GetGateWaybyBID(string str_bid)
		{
			List<InSnergyGateway> list = new List<InSnergyGateway>();
			new List<Branch>();
			new List<SubMeter>();
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			DbDataReader dbDataReader = null;
			Dictionary<string, InSnergyGateway> dictionary = new Dictionary<string, InSnergyGateway>();
			Dictionary<string, Branch> dictionary2 = new Dictionary<string, Branch>();
			try
			{
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandText = string.Concat(new string[]
				{
					"select * from gatewaytable where bid='",
					str_bid,
					"' or (slevel=0 and gid in(select distinct gid from gatewaytable where bid ='",
					str_bid,
					"'))  order by slevel ASC,disname ASC"
				});
				dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.HasRows)
				{
					while (dbDataReader.Read())
					{
						string text = Convert.ToString(dbDataReader.GetValue(0));
						string text2 = Convert.ToString(dbDataReader.GetValue(1));
						string str_sid = Convert.ToString(dbDataReader.GetValue(2));
						int num = Convert.ToInt32(dbDataReader.GetValue(3));
						string str_name = Convert.ToString(dbDataReader.GetValue(4));
						switch (num)
						{
						case 0:
						{
							string str_type = Convert.ToString(dbDataReader.GetValue(7));
							string str_ip = Convert.ToString(dbDataReader.GetValue(9));
							InSnergyGateway inSnergyGateway = new InSnergyGateway(text, str_name, str_type, str_ip, null);
							inSnergyGateway.BranchList = new List<Branch>();
							dictionary.Add(text, inSnergyGateway);
							list.Add(inSnergyGateway);
							break;
						}
						case 1:
						{
							string str_locaton = Convert.ToString(dbDataReader.GetValue(8));
							Branch branch = new Branch(text, text2, str_name, str_locaton, null);
							branch.SubMeterList = new List<SubMeter>();
							dictionary2.Add(text2, branch);
							if (dictionary.ContainsKey(text))
							{
								InSnergyGateway inSnergyGateway = dictionary[text];
								inSnergyGateway.BranchList.Add(branch);
							}
							break;
						}
						case 2:
						{
							float f_capacity = MyConvert.ToSingle(dbDataReader.GetValue(5));
							int i_usage = Convert.ToInt32(dbDataReader.GetValue(6));
							SubMeter item = new SubMeter(text, text2, str_sid, str_name, f_capacity, i_usage);
							if (dictionary2.ContainsKey(text2))
							{
								Branch branch = dictionary2[text2];
								branch.SubMeterList.Add(item);
							}
							break;
						}
						}
					}
				}
				dbDataReader.Close();
				dbCommand.Dispose();
				dBConn.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			if (list.Count > 0)
			{
				return list[0];
			}
			return null;
		}
		public static InSnergyGateway GetGateWaybyGID(string str_gid)
		{
			List<InSnergyGateway> list = new List<InSnergyGateway>();
			new List<Branch>();
			new List<SubMeter>();
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			DbDataReader dbDataReader = null;
			Dictionary<string, InSnergyGateway> dictionary = new Dictionary<string, InSnergyGateway>();
			Dictionary<string, Branch> dictionary2 = new Dictionary<string, Branch>();
			try
			{
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandText = "select * from gatewaytable as c where c.gid = '" + str_gid + "' order by c.slevel ASC,c.disname ASC";
				dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.HasRows)
				{
					while (dbDataReader.Read())
					{
						string text = Convert.ToString(dbDataReader.GetValue(0));
						string text2 = Convert.ToString(dbDataReader.GetValue(1));
						string str_sid = Convert.ToString(dbDataReader.GetValue(2));
						int num = Convert.ToInt32(dbDataReader.GetValue(3));
						string str_name = Convert.ToString(dbDataReader.GetValue(4));
						switch (num)
						{
						case 0:
						{
							string str_type = Convert.ToString(dbDataReader.GetValue(7));
							string str_ip = Convert.ToString(dbDataReader.GetValue(9));
							InSnergyGateway inSnergyGateway = new InSnergyGateway(text, str_name, str_type, str_ip, null);
							inSnergyGateway.BranchList = new List<Branch>();
							dictionary.Add(text, inSnergyGateway);
							list.Add(inSnergyGateway);
							break;
						}
						case 1:
						{
							string str_locaton = Convert.ToString(dbDataReader.GetValue(8));
							Branch branch = new Branch(text, text2, str_name, str_locaton, null);
							branch.SubMeterList = new List<SubMeter>();
							dictionary2.Add(text2, branch);
							if (dictionary.ContainsKey(text))
							{
								InSnergyGateway inSnergyGateway = dictionary[text];
								inSnergyGateway.BranchList.Add(branch);
							}
							break;
						}
						case 2:
						{
							float f_capacity = MyConvert.ToSingle(dbDataReader.GetValue(5));
							int i_usage = Convert.ToInt32(dbDataReader.GetValue(6));
							SubMeter item = new SubMeter(text, text2, str_sid, str_name, f_capacity, i_usage);
							if (dictionary2.ContainsKey(text2))
							{
								Branch branch = dictionary2[text2];
								branch.SubMeterList.Add(item);
							}
							break;
						}
						}
					}
				}
				dbDataReader.Close();
				dbCommand.Dispose();
				dBConn.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			if (list.Count > 0)
			{
				return list[0];
			}
			return null;
		}
		public static List<InSnergyGateway> GetAllGateWay()
		{
			List<InSnergyGateway> list = new List<InSnergyGateway>();
			new List<Branch>();
			new List<SubMeter>();
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			DbDataReader dbDataReader = null;
			Dictionary<string, InSnergyGateway> dictionary = new Dictionary<string, InSnergyGateway>();
			Dictionary<string, Branch> dictionary2 = new Dictionary<string, Branch>();
			try
			{
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandText = "select * from gatewaytable as c order by c.slevel ASC,c.disname ASC";
				dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.HasRows)
				{
					while (dbDataReader.Read())
					{
						string text = Convert.ToString(dbDataReader.GetValue(0));
						string text2 = Convert.ToString(dbDataReader.GetValue(1));
						string str_sid = Convert.ToString(dbDataReader.GetValue(2));
						int num = Convert.ToInt32(dbDataReader.GetValue(3));
						string str_name = Convert.ToString(dbDataReader.GetValue(4));
						switch (num)
						{
						case 0:
						{
							string str_type = Convert.ToString(dbDataReader.GetValue(7));
							string str_ip = Convert.ToString(dbDataReader.GetValue(9));
							InSnergyGateway inSnergyGateway = new InSnergyGateway(text, str_name, str_type, str_ip, null);
							inSnergyGateway.BranchList = new List<Branch>();
							dictionary.Add(text, inSnergyGateway);
							list.Add(inSnergyGateway);
							break;
						}
						case 1:
						{
							string str_locaton = Convert.ToString(dbDataReader.GetValue(8));
							Branch branch = new Branch(text, text2, str_name, str_locaton, null);
							branch.SubMeterList = new List<SubMeter>();
							dictionary2.Add(text2, branch);
							if (dictionary.ContainsKey(text))
							{
								InSnergyGateway inSnergyGateway = dictionary[text];
								inSnergyGateway.BranchList.Add(branch);
							}
							break;
						}
						case 2:
						{
							float f_capacity = MyConvert.ToSingle(dbDataReader.GetValue(5));
							int i_usage = Convert.ToInt32(dbDataReader.GetValue(6));
							SubMeter item = new SubMeter(text, text2, str_sid, str_name, f_capacity, i_usage);
							if (dictionary2.ContainsKey(text2))
							{
								Branch branch = dictionary2[text2];
								branch.SubMeterList.Add(item);
							}
							break;
						}
						}
					}
				}
				dbDataReader.Close();
				dbCommand.Dispose();
				dBConn.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			return list;
		}
		public static bool CheckGatewayName(string str_name)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbDataReader dbDataReader = null;
			int i = 0;
			while (i < 3)
			{
				try
				{
					dBConn = DBConnPool.getConnection();
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = "select gid from gatewaytable where slevel=0 and disname ='" + str_name + "' ";
					dbDataReader = dbCommand.ExecuteReader();
					bool result;
					if (dbDataReader.HasRows)
					{
						dbDataReader.Close();
						dbCommand.Dispose();
						dBConn.Close();
						result = false;
						return result;
					}
					dbDataReader.Close();
					dbCommand.Dispose();
					dBConn.Close();
					result = true;
					return result;
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
					if (dbDataReader != null)
					{
						try
						{
							dbDataReader.Close();
						}
						catch (Exception)
						{
						}
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch (Exception)
						{
						}
					}
					if (dBConn.con != null)
					{
						try
						{
							dBConn.Close();
						}
						catch (Exception)
						{
						}
					}
					i++;
				}
			}
			return false;
		}
		public static int RefreshGateWay(string str_gid, Dictionary<string, Dictionary<string, string>> dic_branch)
		{
			List<string> list = new List<string>();
			Dictionary<string, Dictionary<string, string>> bidMap = InSnergyGateway.getBidMap();
			string text = "";
			int num = 1;
			foreach (KeyValuePair<string, Dictionary<string, string>> current in dic_branch)
			{
				text = current.Key;
				bool flag = false;
				if (bidMap.ContainsKey(text))
				{
					flag = true;
				}
				Dictionary<string, string> value = current.Value;
				foreach (KeyValuePair<string, string> current2 in value)
				{
					string key = current2.Key;
					if (flag)
					{
						if (!bidMap[text].ContainsKey(key))
						{
							string item = string.Concat(new object[]
							{
								"insert into gatewaytable (gid,bid,sid,slevel,disname,capacity,eleflag) values ('",
								str_gid,
								"','",
								text,
								"','",
								key,
								"',2,'Meter",
								key,
								"',",
								0f,
								",",
								0,
								" )"
							});
							list.Add(item);
						}
					}
					else
					{
						string item = string.Concat(new object[]
						{
							"insert into gatewaytable (gid,bid,sid,slevel,disname,capacity,eleflag) values ('",
							str_gid,
							"','",
							text,
							"','",
							key,
							"',2,'Meter",
							key,
							"',",
							0f,
							",",
							0,
							" )"
						});
						list.Add(item);
					}
				}
				if (flag)
				{
					if (!bidMap[text].ContainsValue(str_gid))
					{
						string item = string.Concat(new string[]
						{
							"update gatewaytable set gid='",
							str_gid,
							"' where bid='",
							text,
							"' "
						});
						list.Add(item);
					}
				}
				else
				{
					string item = string.Concat(new object[]
					{
						"insert into gatewaytable (gid,bid,sid,slevel,disname,location) values ('",
						str_gid,
						"','",
						text,
						"','0',1,'Branch",
						num,
						"','' )"
					});
					list.Add(item);
				}
				num++;
			}
			if (list.Count > 0)
			{
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				DbTransaction dbTransaction = null;
				try
				{
					dBConn = DBConnPool.getConnection();
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbTransaction = dBConn.con.BeginTransaction();
					dbCommand.Transaction = dbTransaction;
					foreach (string current3 in list)
					{
						dbCommand.CommandText = current3;
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.Transaction.Commit();
					dbTransaction.Dispose();
					dbCommand.Dispose();
					dBConn.Close();
					int result = 1;
					return result;
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Transaction.Rollback();
						}
						catch (Exception)
						{
						}
					}
					if (dbTransaction != null)
					{
						try
						{
							dbTransaction.Dispose();
						}
						catch (Exception)
						{
						}
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch (Exception)
						{
						}
					}
					if (dBConn.con != null)
					{
						try
						{
							dBConn.Close();
						}
						catch (Exception)
						{
						}
					}
					int result = -1;
					return result;
				}
				return 1;
			}
			return 1;
		}
		public static int RefreshGateWay(string str_gid, Dictionary<string, Dictionary<string, string>> dic_branch, DbConnection conn)
		{
			List<string> list = new List<string>();
			Dictionary<string, Dictionary<string, string>> bidMap = InSnergyGateway.getBidMap(conn);
			string text = "";
			int num = 1;
			foreach (KeyValuePair<string, Dictionary<string, string>> current in dic_branch)
			{
				text = current.Key;
				bool flag = false;
				if (bidMap.ContainsKey(text))
				{
					flag = true;
				}
				Dictionary<string, string> value = current.Value;
				foreach (KeyValuePair<string, string> current2 in value)
				{
					string key = current2.Key;
					if (flag)
					{
						if (!bidMap[text].ContainsKey(key))
						{
							string item = string.Concat(new object[]
							{
								"insert into gatewaytable (gid,bid,sid,slevel,disname,capacity,eleflag) values ('",
								str_gid,
								"','",
								text,
								"','",
								key,
								"',2,'Meter",
								key,
								"',",
								0f,
								",",
								0,
								" )"
							});
							list.Add(item);
						}
					}
					else
					{
						string item = string.Concat(new object[]
						{
							"insert into gatewaytable (gid,bid,sid,slevel,disname,capacity,eleflag) values ('",
							str_gid,
							"','",
							text,
							"','",
							key,
							"',2,'Meter",
							key,
							"',",
							0f,
							",",
							0,
							" )"
						});
						list.Add(item);
					}
				}
				if (flag)
				{
					if (!bidMap[text].ContainsValue(str_gid))
					{
						string item = string.Concat(new string[]
						{
							"update gatewaytable set gid='",
							str_gid,
							"' where bid='",
							text,
							"' "
						});
						list.Add(item);
					}
				}
				else
				{
					string item = string.Concat(new object[]
					{
						"insert into gatewaytable (gid,bid,sid,slevel,disname,location) values ('",
						str_gid,
						"','",
						text,
						"','0',1,'Branch",
						num,
						"','' )"
					});
					list.Add(item);
				}
				num++;
			}
			if (list.Count > 0)
			{
				DbCommand dbCommand = null;
				DbTransaction dbTransaction = null;
				try
				{
					dbCommand = conn.CreateCommand();
					dbTransaction = conn.BeginTransaction();
					dbCommand.Transaction = dbTransaction;
					foreach (string current3 in list)
					{
						dbCommand.CommandText = current3;
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.Transaction.Commit();
					dbTransaction.Dispose();
					dbCommand.Dispose();
					int result = 1;
					return result;
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Transaction.Rollback();
						}
						catch (Exception)
						{
						}
					}
					if (dbTransaction != null)
					{
						try
						{
							dbTransaction.Dispose();
						}
						catch (Exception)
						{
						}
					}
					if (dbCommand != null)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch (Exception)
						{
						}
					}
					int result = -1;
					return result;
				}
				return 1;
			}
			return 1;
		}
		public static Dictionary<string, Dictionary<string, string>> getBidMap()
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbDataReader dbDataReader = null;
			Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
			try
			{
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandText = "select * from gatewaytable where slevel>0 order by bid ASC,slevel ASC";
				dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.HasRows)
				{
					while (dbDataReader.Read())
					{
						string text = Convert.ToString(dbDataReader.GetValue(0));
						string key = Convert.ToString(dbDataReader.GetValue(1));
						string key2 = Convert.ToString(dbDataReader.GetValue(2));
						int num = Convert.ToInt32(dbDataReader.GetValue(3));
						try
						{
							switch (num)
							{
							case 1:
								dictionary.Add(key, new Dictionary<string, string>
								{

									{
										text,
										text
									}
								});
								break;
							case 2:
								if (dictionary.ContainsKey(key))
								{
									dictionary[key].Add(key2, text);
								}
								break;
							}
						}
						catch
						{
						}
					}
				}
				dbDataReader.Close();
				dbCommand.Dispose();
				dBConn.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn.con != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			return dictionary;
		}
		public static Dictionary<string, Dictionary<string, string>> getBidMap(DbConnection conn)
		{
			DbCommand dbCommand = null;
			DbDataReader dbDataReader = null;
			Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
			try
			{
				dbCommand = conn.CreateCommand();
				dbCommand.CommandText = "select * from gatewaytable where slevel>0 order by bid ASC,slevel ASC";
				dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.HasRows)
				{
					while (dbDataReader.Read())
					{
						string text = Convert.ToString(dbDataReader.GetValue(0));
						string key = Convert.ToString(dbDataReader.GetValue(1));
						string key2 = Convert.ToString(dbDataReader.GetValue(2));
						int num = Convert.ToInt32(dbDataReader.GetValue(3));
						try
						{
							switch (num)
							{
							case 1:
								dictionary.Add(key, new Dictionary<string, string>
								{

									{
										text,
										text
									}
								});
								break;
							case 2:
								if (dictionary.ContainsKey(key))
								{
									dictionary[key].Add(key2, text);
								}
								break;
							}
						}
						catch
						{
						}
					}
				}
				dbDataReader.Close();
				dbCommand.Dispose();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
			}
			return dictionary;
		}
		public static double GetPUE(int i_type)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbDataReader dbDataReader = null;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			try
			{
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandText = "select * from currentpue ";
				dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.HasRows)
				{
					dbDataReader.Read();
					DateTime dateTime = dbDataReader.GetDateTime(9);
					num = dbDataReader.GetDouble(3);
					num2 = dbDataReader.GetDouble(4);
					num3 = dbDataReader.GetDouble(5);
					num4 = dbDataReader.GetDouble(6);
					num5 = dbDataReader.GetDouble(7);
					num6 = dbDataReader.GetDouble(8);
					dbDataReader.Close();
					dbDataReader.Dispose();
					string text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
					string text2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
					string text3 = InSnergyGateway.getweek().ToString("yyyy-MM-dd HH:mm:ss");
					string text4 = "";
					if (InSnergyGateway.checkweek(dateTime))
					{
						if (InSnergyGateway.checkday(dateTime))
						{
							if (!InSnergyGateway.checkhour(dateTime))
							{
								text4 = string.Concat(new string[]
								{
									"update currentpue set curhour=#",
									text,
									"#,curday=#",
									text2,
									"#,curweek=#",
									text3,
									"#,nonithourpue=0,ithourpue=0,lasttime=#",
									DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
									"# "
								});
								num = 0.0;
								num2 = 0.0;
							}
						}
						else
						{
							text4 = string.Concat(new string[]
							{
								"update currentpue set curhour=#",
								text,
								"#,curday=#",
								text2,
								"#,curweek=#",
								text3,
								"#,ithourpue=0,nonithourpue=0,itdaypue=0,nonitdaypue=0,lasttime=#",
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
								"# "
							});
							num = 0.0;
							num2 = 0.0;
							num3 = 0.0;
							num4 = 0.0;
						}
					}
					else
					{
						text4 = string.Concat(new string[]
						{
							"update currentpue set curhour=#",
							text,
							"#,curday=#",
							text2,
							"#,curweek=#",
							text3,
							"#,ithourpue=0,nonithourpue=0,itdaypue=0,nonitdaypue=0,itweekpue=0,nonitweekpue=0,lasttime=#",
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
							"# "
						});
						num = 0.0;
						num2 = 0.0;
						num3 = 0.0;
						num4 = 0.0;
						num5 = 0.0;
						num6 = 0.0;
					}
					if (text4.Length > 0)
					{
						DbTransaction dbTransaction = dBConn.BeginTransaction();
						dbCommand.Transaction = (OleDbTransaction)dbTransaction;
						if (DBUrl.SERVERMODE)
						{
							text4 = text4.Replace("#", "'");
						}
						dbCommand.CommandText = text4;
						dbCommand.ExecuteNonQuery();
						dbCommand.Transaction.Commit();
						dbTransaction.Dispose();
					}
				}
				try
				{
					dbDataReader.Close();
					dbDataReader.Dispose();
				}
				catch (Exception)
				{
				}
				dbCommand.Dispose();
				dBConn.Close();
				switch (i_type)
				{
				case 0:
				{
					double result = num;
					return result;
				}
				case 1:
				{
					double result = num2;
					return result;
				}
				case 2:
				{
					double result = num3;
					return result;
				}
				case 3:
				{
					double result = num4;
					return result;
				}
				case 4:
				{
					double result = num5;
					return result;
				}
				case 5:
				{
					double result = num6;
					return result;
				}
				}
			}
			catch (Exception)
			{
				if (dbDataReader != null)
				{
					try
					{
						dbDataReader.Close();
						dbDataReader.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch (Exception)
					{
					}
				}
				if (dBConn != null)
				{
					try
					{
						dBConn.Close();
					}
					catch (Exception)
					{
					}
				}
			}
			return 0.0;
		}
		private static DateTime getweek()
		{
			DateTime result = DateTime.Now;
			switch (DateTime.Now.DayOfWeek)
			{
			case DayOfWeek.Sunday:
				result = DateTime.Now.AddDays(-6.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Monday:
				result = DateTime.Now;
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Tuesday:
				result = DateTime.Now.AddDays(-1.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Wednesday:
				result = DateTime.Now.AddDays(-2.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Thursday:
				result = DateTime.Now.AddDays(-3.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Friday:
				result = DateTime.Now.AddDays(-4.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			case DayOfWeek.Saturday:
				result = DateTime.Now.AddDays(-5.0);
				result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0);
				break;
			}
			return result;
		}
		private static bool checkweek(DateTime dt_time)
		{
			switch (DateTime.Now.DayOfWeek)
			{
			case DayOfWeek.Sunday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-6.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Monday:
			{
				DateTime dateTime = DateTime.Now;
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Tuesday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-1.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Wednesday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-2.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Thursday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-3.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Friday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-4.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			case DayOfWeek.Saturday:
			{
				DateTime dateTime = DateTime.Now.AddDays(-5.0);
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				int num = dateTime.CompareTo(dt_time);
				return num <= 0;
			}
			default:
				return false;
			}
		}
		private static bool checkday(DateTime dt_time)
		{
			return dt_time.Year == DateTime.Now.Year && dt_time.Month == DateTime.Now.Month && dt_time.Day == DateTime.Now.Day;
		}
		private static bool checkhour(DateTime dt_time)
		{
			return dt_time.Year == DateTime.Now.Year && dt_time.Month == DateTime.Now.Month && dt_time.Day == DateTime.Now.Day && dt_time.Hour == DateTime.Now.Hour;
		}
	}
}
