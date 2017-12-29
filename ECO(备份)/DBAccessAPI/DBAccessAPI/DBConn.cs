using CommonAPI;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Runtime.Remoting;
using System.Threading;
using System.Transactions;
namespace DBAccessAPI
{
	public class DBConn
	{
		public DbConnection con;
		private bool b_frompool = true;
		public int DBSource_Type = 1;
		private bool bNotInUse;
		private long m_lastestAccess;
		public static DbTransaction GetTransaction(DbConnection conn)
		{
			if (conn is MySqlConnection)
			{
				return (MySqlTransaction)conn.BeginTransaction();
			}
			return (OleDbTransaction)conn.BeginTransaction();
		}
		public static DbDataAdapter GetDataAdapter(DbConnection conn)
		{
			if (conn is MySqlConnection)
			{
				return new MySqlDataAdapter();
			}
			return new OleDbDataAdapter();
		}
		public static DbCommand GetCommandObject(DbConnection com_conn)
		{
			if (com_conn is MySqlConnection)
			{
				return new MySqlCommand
				{
					Connection = (MySqlConnection)com_conn
				};
			}
			return new OleDbCommand
			{
				Connection = (OleDbConnection)com_conn
			};
		}
		public static DataTable BuildAndInitDataTable(DataTable dt_schema)
		{
			DataTable dataTable = null;
			if (dt_schema == null)
			{
				return null;
			}
			dataTable = new DataTable();
			foreach (DataRow dataRow in dt_schema.Rows)
			{
				Type type = Type.GetType(Convert.ToString(dataRow["DataType"]));
				DataColumn column = new DataColumn(Convert.ToString(dataRow["ColumnName"]), type);
				dataTable.Columns.Add(column);
			}
			return dataTable;
		}
		public static DataTable BuildAndInitDataTable(int Field_Count)
		{
			if (Field_Count <= 0)
			{
				return null;
			}
			DataTable dataTable = new DataTable();
			for (int i = 0; i < Field_Count; i++)
			{
				DataColumn column = new DataColumn(i.ToString());
				dataTable.Columns.Add(column);
			}
			return dataTable;
		}
		public static DataTable ConvertOleDbReaderToDataTable(DbDataReader reader)
		{
			int fieldCount = reader.FieldCount;
			DataTable schemaTable = reader.GetSchemaTable();
			DataTable dataTable = DBConn.BuildAndInitDataTable(schemaTable);
			if (dataTable == null)
			{
				return null;
			}
			while (reader.Read())
			{
				DataRow dataRow = dataTable.NewRow();
				for (int i = 0; i < fieldCount; i++)
				{
					dataRow[i] = reader[i];
				}
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
		public static DataTable ConvertOleDbReaderToDataTable(OleDbDataReader reader)
		{
			int fieldCount = reader.FieldCount;
			DataTable schemaTable = reader.GetSchemaTable();
			DataTable dataTable = DBConn.BuildAndInitDataTable(schemaTable);
			if (dataTable == null)
			{
				return null;
			}
			while (reader.Read())
			{
				DataRow dataRow = dataTable.NewRow();
				for (int i = 0; i < fieldCount; i++)
				{
					dataRow[i] = reader[i];
				}
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
		public DBConn()
		{
			this.bNotInUse = true;
			this.b_frompool = true;
		}
		public DBConn(bool b_from)
		{
			this.bNotInUse = true;
			this.b_frompool = b_from;
		}
		public void Open()
		{
			if (this.con != null)
			{
				this.con.Open();
			}
		}
		public bool createDynaConnection(int i_index)
		{
			bool result;
			try
			{
				if (this.con != null)
				{
					if (this.con.State == ConnectionState.Open || this.con.State == ConnectionState.Connecting)
					{
						result = true;
						return result;
					}
					this.con.Close();
					this.con = null;
				}
				if (DBUrl.RUNMODE == 1)
				{
					string dB_CURRENT_TYPE;
					string connectionString;
					if ((dB_CURRENT_TYPE = DBUrl.DB_CURRENT_TYPE) != null)
					{
						if (dB_CURRENT_TYPE == "ACCESS")
						{
							connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.CURRENT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.CURRENT_PWD;
							int num = 0;
							while (TaskStatus.GetDBStatus() != 1 && num < 600)
							{
								Thread.Sleep(50);
								DebugCenter.GetInstance().appendToFile(this.ToString() + "Waiting DBConnection : " + num);
								num++;
							}
							this.con = new OleDbConnection(connectionString);
							this.con.Open();
							result = true;
							return result;
						}
						if (dB_CURRENT_TYPE == "MYSQL")
						{
							this.con = new MySqlConnection(string.Concat(new object[]
							{
								"Database=",
								DBUrl.DB_CURRENT_NAME,
								";Data Source=",
								DBUrl.CURRENT_HOST_PATH,
								";Port=",
								DBUrl.CURRENT_PORT,
								";User Id=",
								DBUrl.CURRENT_USER_NAME,
								";Password=",
								DBUrl.CURRENT_PWD,
								";Default Command Timeout=0;charset=utf8;"
							}));
							this.con.Open();
							result = true;
							return result;
						}
					}
					connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
					this.con = new OleDbConnection(connectionString);
					this.con.Open();
					result = true;
				}
				else
				{
					string cONNECT_STRING = DBUrl.CONNECT_STRING;
					string[] array = cONNECT_STRING.Split(new string[]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					this.con = new MySqlConnection(string.Concat(new string[]
					{
						"Database=eco",
						DBUrl.SERVERID,
						";Data Source=",
						array[0],
						";Port=",
						array[1],
						";User Id=",
						array[2],
						";Password=",
						array[3],
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
					this.con.Open();
					result = true;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				result = false;
			}
			return result;
		}
		public bool createConnection(int i_index)
		{
			try
			{
				bool result;
				if (this.con != null)
				{
					if (this.con.State == ConnectionState.Open || this.con.State == ConnectionState.Connecting)
					{
						result = true;
						return result;
					}
					this.con.Close();
					this.con = null;
				}
				if (DBUrl.RUNMODE == 1)
				{
					string dB_DEFAULT_TYPE;
					string connectionString;
					if ((dB_DEFAULT_TYPE = DBUrl.DB_DEFAULT_TYPE) != null)
					{
						if (dB_DEFAULT_TYPE == "ACCESS")
						{
							connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
							this.con = new OleDbConnection(connectionString);
							this.con.Open();
							result = true;
							return result;
						}
						if (dB_DEFAULT_TYPE == "MYSQL")
						{
							return true;
						}
					}
					connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
					this.con = new OleDbConnection(connectionString);
					this.con.Open();
					result = true;
					return result;
				}
				string cONNECT_STRING = DBUrl.CONNECT_STRING;
				string[] array = cONNECT_STRING.Split(new string[]
				{
					","
				}, StringSplitOptions.RemoveEmptyEntries);
				this.con = new MySqlConnection(string.Concat(new string[]
				{
					"Database=eco",
					DBUrl.SERVERID,
					";Data Source=",
					array[0],
					";Port=",
					array[1],
					";User Id=",
					array[2],
					";Password=",
					array[3],
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				this.con.Open();
				result = true;
				return result;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				bool result = false;
				return result;
			}
			return true;
		}
		public bool isInnerClose()
		{
			bool result;
			try
			{
				if (this.con == null)
				{
					result = true;
				}
				else
				{
					this.con.Close();
					result = true;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				result = true;
			}
			return result;
		}
		public void setLastestAccess()
		{
			this.m_lastestAccess = DateTime.Now.Ticks;
		}
		public void setInUse()
		{
			this.bNotInUse = false;
			this.setLastestAccess();
		}
		public bool isInUse()
		{
			return !this.bNotInUse;
		}
		public void innerClose()
		{
			if (this.isInUse())
			{
				return;
			}
			this.m_lastestAccess = 0L;
			try
			{
				this.con.Close();
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				this.con = null;
			}
		}
		public void close()
		{
			try
			{
				this.bNotInUse = true;
				this.con.Close();
				if (this.DBSource_Type == 1)
				{
					DBCache.CloseSysDB(this.GetHashCode());
				}
				else
				{
					if (this.DBSource_Type == 2)
					{
						DBCache.CloseDataDB(this.GetHashCode());
					}
					else
					{
						if (this.DBSource_Type == 3)
						{
							DBCache.CloseThermalDB(this.GetHashCode());
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
		public DbTransaction BeginTransaction()
		{
			return this.con.BeginTransaction();
		}
		public DbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
		{
			return this.con.BeginTransaction(isolationLevel);
		}
		public void ChangeDatabase(string databaseName)
		{
			this.con.ChangeDatabase(databaseName);
		}
		public void Close()
		{
			try
			{
				this.bNotInUse = true;
				this.con.Close();
				if (this.DBSource_Type == 1)
				{
					DBCache.CloseSysDB(this.GetHashCode());
				}
				else
				{
					if (this.DBSource_Type == 2)
					{
						DBCache.CloseDataDB(this.GetHashCode());
					}
					else
					{
						if (this.DBSource_Type == 3)
						{
							DBCache.CloseThermalDB(this.GetHashCode());
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
		public DbCommand CreateCommand()
		{
			return this.con.CreateCommand();
		}
		public ObjRef CreateObjRef(Type requestedType)
		{
			return this.con.CreateObjRef(requestedType);
		}
		public void Dispose()
		{
			this.con.Dispose();
		}
		public void EnlistTransaction(Transaction transaction)
		{
			this.con.EnlistTransaction(transaction);
		}
		public new bool Equals(object obj)
		{
			return this.con.Equals(obj);
		}
		public new int GetHashCode()
		{
			return this.con.GetHashCode();
		}
		public object GetLifetimeService()
		{
			return this.con.GetLifetimeService();
		}
		public DataTable GetSchema()
		{
			return this.con.GetSchema();
		}
		public DataTable GetSchema(string collectionName)
		{
			return this.con.GetSchema(collectionName);
		}
		public DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			return this.con.GetSchema(collectionName, restrictionValues);
		}
		public new Type GetType()
		{
			return this.con.GetType();
		}
		public object InitializeLifetimeService()
		{
			return this.con.InitializeLifetimeService();
		}
	}
}
