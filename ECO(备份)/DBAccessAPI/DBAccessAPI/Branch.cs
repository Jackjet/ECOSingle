using CommonAPI;
using System;
using System.Collections.Generic;
using System.Data.Common;
namespace DBAccessAPI
{
	public class Branch
	{
		private string gid;
		private string bid;
		private string bname;
		private string location;
		private List<SubMeter> list_children = new List<SubMeter>();
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
		public string BranchID
		{
			get
			{
				return this.bid;
			}
			set
			{
				this.bid = value;
			}
		}
		public string BranchName
		{
			get
			{
				return this.bname;
			}
			set
			{
				this.bname = value;
			}
		}
		public string Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}
		public List<SubMeter> SubMeterList
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
		public int SubMeterCount
		{
			get
			{
				if (this.list_children != null)
				{
					return this.list_children.Count;
				}
				return 0;
			}
		}
		public Branch()
		{
			this.gid = "";
			this.bid = "";
			this.bname = "";
			this.location = "";
			this.list_children = new List<SubMeter>();
		}
		public Branch(string str_gid, string str_bid, string str_name, string str_locaton, List<SubMeter> list_child)
		{
			this.gid = str_gid;
			this.bid = str_bid;
			this.bname = str_name;
			this.location = str_locaton;
			this.list_children = list_child;
		}
		public int Save()
		{
			DbCommand dbCommand = null;
			DBConn dBConn = null;
			try
			{
				int result;
				if (this.gid == null || this.gid.Length < 1 || this.gid.Equals("0"))
				{
					result = -1;
					return result;
				}
				if (this.bid == null || this.bid.Length < 1 || this.bid.Equals("0"))
				{
					result = -1;
					return result;
				}
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
				dBConn = DBConnPool.getConnection();
				dbCommand = dBConn.con.CreateCommand();
				DbTransaction transaction = dBConn.con.BeginTransaction();
				dbCommand.Transaction = transaction;
				foreach (SubMeter current in this.list_children)
				{
					dbCommand.CommandText = string.Concat(new object[]
					{
						"update gatewaytable set eleflag=",
						current.ElectricityUsage,
						",capacity=",
						current.Capacity,
						" where slevel=2 and gid='",
						this.gid,
						"' and bid='",
						this.bid,
						"' and sid='",
						current.SubmeterID,
						"' "
					});
					dbCommand.ExecuteNonQuery();
				}
				dbCommand.CommandText = string.Concat(new string[]
				{
					"update gatewaytable set disname = '",
					this.bname,
					"',location='",
					this.location,
					"' where slevel = 1 and gid = '",
					this.gid,
					"' and bid='",
					this.bid,
					"' "
				});
				int num = dbCommand.ExecuteNonQuery();
				dbCommand.Transaction.Commit();
				dbCommand.Dispose();
				dBConn.Close();
				result = num;
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
	}
}
