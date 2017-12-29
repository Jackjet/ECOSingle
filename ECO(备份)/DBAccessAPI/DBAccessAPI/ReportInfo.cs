using CommonAPI;
using System;
using System.Data;
using System.Data.Common;
namespace DBAccessAPI
{
	public class ReportInfo
	{
		private int id;
		private string title = "";
		private string writer = "";
		private DateTime reporttime;
		private string reportpath = "";
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
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
			}
		}
		public string Writer
		{
			get
			{
				return this.writer;
			}
			set
			{
				this.writer = value;
			}
		}
		public string ReportPath
		{
			get
			{
				return this.reportpath;
			}
			set
			{
				this.reportpath = value;
			}
		}
		public DateTime ReportTime
		{
			get
			{
				return this.reporttime;
			}
			set
			{
				this.reporttime = value;
			}
		}
		public ReportInfo(DataRow dr)
		{
			this.id = Convert.ToInt32(dr["id"]);
			this.title = Convert.ToString(dr["title"]);
			this.writer = Convert.ToString(dr["writer"]);
			this.reportpath = Convert.ToString(dr["reportpath"]);
			this.reporttime = Convert.ToDateTime(dr["reporttime"]);
		}
		public static int InsertBillReport(string str_title, string str_writer, DateTime dt_time, string str_path)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					if (DBUrl.SERVERMODE)
					{
						string commandText = "insert into reportbill(Title,Writer,ReportTime,ReportPath) values(?Title,?Writer,?ReportTime,?ReportPath)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?Title", str_title, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?Writer", str_writer, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ReportTime", dt_time.ToString("yyyy-MM-dd HH:mm:ss"), dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ReportPath", str_path, dbCommand));
					}
					else
					{
						string commandText = "insert into reportbill(Title,Writer,ReportTime,ReportPath) values(?,?,?,?)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@Title", str_title, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@Writer", str_writer, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ReportTime", dt_time.ToString("yyyy-MM-dd HH:mm:ss"), dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ReportPath", str_path, dbCommand));
					}
					result = dbCommand.ExecuteNonQuery();
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					dbCommand.Dispose();
				}
				if (dBConn.con != null)
				{
					dBConn.Close();
				}
			}
			return result;
		}
		public static int InsertThermalReport(string str_title, string str_writer, DateTime dt_time, string str_path)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					if (DBUrl.SERVERMODE)
					{
						string commandText = "insert into reportthermal(Title,Writer,ReportTime,ReportPath) values(?Title,?Writer,?ReportTime,?ReportPath)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?Title", str_title, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?Writer", str_writer, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ReportTime", dt_time.ToString("yyyy-MM-dd HH:mm:ss"), dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ReportPath", str_path, dbCommand));
					}
					else
					{
						string commandText = "insert into reportthermal(Title,Writer,ReportTime,ReportPath) values(?,?,?,?)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@Title", str_title, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@Writer", str_writer, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ReportTime", dt_time.ToString("yyyy-MM-dd HH:mm:ss"), dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ReportPath", str_path, dbCommand));
					}
					result = dbCommand.ExecuteNonQuery();
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					dbCommand.Dispose();
				}
				if (dBConn.con != null)
				{
					dBConn.Close();
				}
			}
			return result;
		}
		public static int InsertReport(string str_title, string str_writer, DateTime dt_time, string str_path)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					if (DBUrl.SERVERMODE)
					{
						string commandText = "insert into reportinfo(Title,Writer,ReportTime,ReportPath) values(?Title,?Writer,?ReportTime,?ReportPath)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?Title", str_title, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?Writer", str_writer, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ReportTime", dt_time.ToString("yyyy-MM-dd HH:mm:ss"), dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ReportPath", str_path, dbCommand));
					}
					else
					{
						string commandText = "insert into reportinfo(Title,Writer,ReportTime,ReportPath) values(?,?,?,?)";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@Title", str_title, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@Writer", str_writer, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ReportTime", dt_time.ToString("yyyy-MM-dd HH:mm:ss"), dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ReportPath", str_path, dbCommand));
					}
					result = dbCommand.ExecuteNonQuery();
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					dbCommand.Dispose();
				}
				if (dBConn.con != null)
				{
					dBConn.Close();
				}
			}
			return result;
		}
	}
}
