using CommonAPI;
using CommonAPI.InterProcess;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading;
namespace DBAccessAPI
{
	public class DBMaintain
	{
		private static int MYSQL_VERSION_ROLE = 0;
		private static object _ConvertLock = new object();
		private static bool b_convertfinish = true;
		public static bool ConvertOldDataFinish
		{
			get
			{
				bool result;
				lock (DBMaintain._ConvertLock)
				{
					string interProcessKeyValue = InterProcessShared<Dictionary<string, string>>.getInterProcessKeyValue("CONVERTDATATHREADSTATUS");
					if (interProcessKeyValue.Equals("TRUE"))
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
				return result;
			}
		}
		public static int GetMySQLVersionRole()
		{
			return DBMaintain.MYSQL_VERSION_ROLE;
		}
		public static void SetMySQLVersionRole(int i_role)
		{
			DBMaintain.MYSQL_VERSION_ROLE = i_role;
		}
		public static string EditVersionString(string str_src)
		{
			try
			{
				string text = Regex.Replace(str_src, "\\D", ".");
				while (text.EndsWith("."))
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
			catch
			{
			}
			return str_src;
		}
		private static string getMajorVersion(string str_ver)
		{
			string[] array = str_ver.Split(new string[]
			{
				"."
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 1)
			{
				return str_ver;
			}
			return array[0] + "." + array[1];
		}
		public static bool CompareMySQLVersion(string str_curver, string str_filever)
		{
			switch (DBMaintain.GetMySQLVersionRole())
			{
			case 0:
				return str_curver.CompareTo(str_filever) >= 0;
			case 1:
			{
				string majorVersion = DBMaintain.getMajorVersion(str_curver);
				string majorVersion2 = DBMaintain.getMajorVersion(str_filever);
				return majorVersion.CompareTo(majorVersion2) >= 0;
			}
			default:
				return true;
			}
		}
		public static int GetWorkPath(string str_path, long l_space, long l_tspace, long l_fspace)
		{
			Hashtable hashtable = new Hashtable();
			bool flag = false;
			try
			{
				string pathRoot = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
				string pathRoot2 = Path.GetPathRoot(str_path);
				int result;
				if (string.IsNullOrEmpty(pathRoot2))
				{
					result = -1;
					return result;
				}
				DriveInfo[] drives = DriveInfo.GetDrives();
				DriveInfo[] array = drives;
				for (int i = 0; i < array.Length; i++)
				{
					DriveInfo driveInfo = array[i];
					if (driveInfo.DriveType == DriveType.Fixed)
					{
						if (hashtable.ContainsKey(driveInfo.Name))
						{
							hashtable[driveInfo.Name] = driveInfo.AvailableFreeSpace;
						}
						else
						{
							hashtable.Add(driveInfo.Name, driveInfo.AvailableFreeSpace);
						}
					}
				}
				long num = Convert.ToInt64(hashtable[pathRoot]);
				if (num > l_space)
				{
					result = 1;
					return result;
				}
				if (num > l_tspace)
				{
					flag = true;
				}
				if (!hashtable.ContainsKey(pathRoot2))
				{
					string text = str_path;
					if (text[text.Length - 1] != Path.DirectorySeparatorChar)
					{
						text += Path.DirectorySeparatorChar;
					}
					text += "tmpdbexchangefolder";
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
						Thread.Sleep(150);
						if (!Directory.Exists(text))
						{
							result = -1;
							return result;
						}
					}
					try
					{
						DirectorySecurity directorySecurity = new DirectorySecurity();
						directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
						Directory.SetAccessControl(text, directorySecurity);
					}
					catch
					{
						result = -1;
						return result;
					}
					result = 4;
					return result;
				}
				num = Convert.ToInt64(hashtable[pathRoot2]);
				if (num > l_space)
				{
					string text2 = str_path;
					if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
					{
						text2 += Path.DirectorySeparatorChar;
					}
					text2 += "tmpdbexchangefolder";
					if (!Directory.Exists(text2))
					{
						Directory.CreateDirectory(text2);
						Thread.Sleep(150);
						if (!Directory.Exists(text2))
						{
							result = -1;
							return result;
						}
					}
					try
					{
						DirectorySecurity directorySecurity2 = new DirectorySecurity();
						directorySecurity2.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
						Directory.SetAccessControl(text2, directorySecurity2);
					}
					catch
					{
						result = -1;
						return result;
					}
					result = 2;
					return result;
				}
				if (num > l_fspace && flag)
				{
					string text3 = str_path;
					if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
					{
						text3 += Path.DirectorySeparatorChar;
					}
					text3 += "tmpdbexchangefolder";
					if (!Directory.Exists(text3))
					{
						Directory.CreateDirectory(text3);
						Thread.Sleep(150);
						if (!Directory.Exists(text3))
						{
							result = -1;
							return result;
						}
					}
					try
					{
						DirectorySecurity directorySecurity3 = new DirectorySecurity();
						directorySecurity3.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
						Directory.SetAccessControl(text3, directorySecurity3);
					}
					catch
					{
						result = -1;
						return result;
					}
					result = 3;
					return result;
				}
				result = -1;
				return result;
			}
			catch (Exception)
			{
			}
			return -1;
		}
		public static void SetConvertOldDataStatus(bool b_status)
		{
			lock (DBMaintain._ConvertLock)
			{
				DBMaintain.b_convertfinish = b_status;
				if (b_status)
				{
					InterProcessShared<Dictionary<string, string>>.setInterProcessKeyValue("CONVERTDATATHREADSTATUS", "TRUE");
				}
				else
				{
					InterProcessShared<Dictionary<string, string>>.setInterProcessKeyValue("CONVERTDATATHREADSTATUS", "FALSE");
				}
			}
		}
		public static bool GetConvertOldDataStatus()
		{
			bool result;
			lock (DBMaintain._ConvertLock)
			{
				result = DBMaintain.b_convertfinish;
			}
			return result;
		}
		public static bool IsLocalIP(string str_ip)
		{
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface[] array = allNetworkInterfaces;
				for (int i = 0; i < array.Length; i++)
				{
					NetworkInterface networkInterface = array[i];
					IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
					UnicastIPAddressInformationCollection unicastAddresses = iPProperties.UnicastAddresses;
					foreach (UnicastIPAddressInformation current in unicastAddresses)
					{
						if (str_ip.Equals(current.Address.ToString()))
						{
							return true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile(ex.Message);
			}
			return false;
		}
		public static int InitMySQLDatabase4Master(string host, int port, string user, string pwd, ref string dbname)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=;Data Source=",
					host,
					";Port=",
					port,
					";User Id=",
					user,
					";Password=",
					pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					dbCommand = dbConnection.CreateCommand();
					dbCommand.CommandText = "set global max_connections=5000 ";
					dbCommand.ExecuteNonQuery();
					string text = DBUtil.genSrvID();
					int result;
					if (text == null || text.Length < 1)
					{
						result = -1;
						return result;
					}
					string text2 = "eco_" + text;
					DBUtil.SetServerID(text);
					dbCommand.CommandText = "CREATE SCHEMA  " + text2;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "USE " + text2;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `bank_data_daily` (`bank_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` date NOT NULL,KEY `bddind1` (`bank_id`),KEY `bddind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `bank_data_hourly` (`bank_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `bdhind1` (`bank_id`),KEY `bdhind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `device_data_daily` (`device_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` date NOT NULL,KEY `dddind1` (`device_id`),KEY `dddind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `device_data_hourly` (`device_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `ddhind1` (`device_id`),KEY `ddhind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `port_data_daily` (`port_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` date NOT NULL,KEY `pddind1` (`port_id`),KEY `pddind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `port_data_hourly` (`port_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `pdhind1` (`port_id`),KEY `pdhind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `rack_effect` (`id` int(11) NOT NULL AUTO_INCREMENT,`RCI_High` float(16,4) DEFAULT NULL,`RCI_Low` float(16,4) DEFAULT NULL,`RHI_High` float(16,4) DEFAULT NULL,`RHI_Low` float(16,4) DEFAULT NULL,`RPI_High` float(16,4) DEFAULT NULL,`RPI_Low` float(16,4) DEFAULT NULL,`RAI_High` float(16,4) DEFAULT NULL,`RAI_Low` float(16,4) DEFAULT NULL,`RTI` float(16,4) DEFAULT NULL,`Fan_Saving` float(16,4) DEFAULT '0.0000',`Chiller_Saving` float(16,4) DEFAULT '0.0000',`Aggressive_Saving` float(16,4) DEFAULT '0.0000',`Insert_time` datetime DEFAULT NULL,PRIMARY KEY (`id`),KEY `reind1` (`Insert_time`)) ENGINE=MYISAM";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `rackthermal_daily` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `rackthermal_hourly` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `rci_daily` (`rci_high` float(16,4) DEFAULT NULL,`rci_lo` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `index_rcid` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `rci_hourly` (`rci_high` float(16,4) DEFAULT NULL,`rci_lo` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `index_rcih` (`insert_time`)) ENGINE=MYISAM ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "CREATE TABLE `tmpid` (`id` int(11) NOT NULL,PRIMARY KEY (`id`)) ENGINE=MYISAM";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "create table `dbextendinfo` (`serverid` varchar(255),`serverip` varchar(255),`servername` varchar(128),`servermac` varchar(128),`createtime` datetime,KEY `index1` (`serverip`),KEY `index2` (`serverid`) ) ENGINE=MYISAM";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + text + "' )";
					dbCommand.ExecuteNonQuery();
					dbname = text2;
					result = 1;
					return result;
				}
			}
			catch (Exception)
			{
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
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
			}
			return -1;
		}
		public static int InitMySQLDatabase4Master(ref string dbname, string host, int port, string user, string pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			string text = "";
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=;Data Source=",
					host,
					";Port=",
					port,
					";User Id=",
					user,
					";Password=",
					pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					dbCommand = dbConnection.CreateCommand();
					dbCommand.CommandText = "set global max_connections=5000 ";
					dbCommand.ExecuteNonQuery();
					int result;
					string text2;
					if (dbname.Equals("eco"))
					{
						text = DBUtil.genSrvID();
						if (text == null || text.Length < 1)
						{
							result = -1;
							return result;
						}
						text2 = "eco_" + text;
						string srcdbpath = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder" + Path.DirectorySeparatorChar;
						int num = DBUtil.SetServerID(srcdbpath, text);
						if (num < 0)
						{
							result = -1;
							return result;
						}
					}
					else
					{
						text2 = dbname;
					}
					dbCommand.CommandText = "CREATE SCHEMA  " + text2;
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "USE " + text2;
					dbCommand.ExecuteNonQuery();
					if (dbname.Equals("eco"))
					{
						dbCommand.CommandText = "create table `dbextendinfo` (`serverid` varchar(255),`serverip` varchar(255),`servername` varchar(128),`servermac` varchar(128),`createtime` datetime,KEY `index1` (`serverip`),KEY `index2` (`serverid`) ) ENGINE=MYISAM";
						dbCommand.ExecuteNonQuery();
						dbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + text + "' )";
						dbCommand.ExecuteNonQuery();
						dbname = text2;
					}
					result = 1;
					return result;
				}
			}
			catch (Exception)
			{
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
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
			}
			return -1;
		}
		public static DbConnection GetDBConnection(string str_dbfile)
		{
			DbConnection dbConnection = null;
			try
			{
				if (str_dbfile.IndexOf("sysdb.mdb") > 0)
				{
					dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str_dbfile + ";Jet OLEDB:Database Password=^tenec0Sensor");
				}
				else
				{
					dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str_dbfile + ";Jet OLEDB:Database Password=root");
				}
				dbConnection.Open();
			}
			catch
			{
			}
			return dbConnection;
		}
		public static int SetMySQLConnectionInfo(DbConnection con, string dbname, string host, int i_port, string usr, string pwd)
		{
			DbCommand dbCommand = null;
			if (con != null && con.State == ConnectionState.Open)
			{
				try
				{
					dbCommand = con.CreateCommand();
					string text = host;
					try
					{
						if (DBMaintain.IsLocalIP(host))
						{
							text = "127.0.0.1";
						}
					}
					catch
					{
					}
					dbCommand.CommandText = string.Concat(new object[]
					{
						"update dbsource set db_type='MYSQL',db_name='",
						dbname,
						"',host_path='",
						text,
						"',port= ",
						i_port,
						",user_name = '",
						usr,
						"',pwd='",
						pwd,
						"' where active_flag = 2 "
					});
					int num = dbCommand.ExecuteNonQuery();
					int result;
					if (num < 0)
					{
						result = -1;
						return result;
					}
					result = 1;
					return result;
				}
				catch
				{
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
			return -1;
		}
		public static int SetAccessConnectionInfo(DbConnection con)
		{
			DbCommand dbCommand = null;
			if (con != null && con.State == ConnectionState.Open)
			{
				try
				{
					dbCommand = con.CreateCommand();
					dbCommand.CommandText = "update dbsource set db_type='ACCESS',db_name='datadb',host_path='datadb.mdb',port= 0,user_name = 'root',pwd='root' where active_flag = 2 ";
					int num = dbCommand.ExecuteNonQuery();
					int result;
					if (num < 0)
					{
						result = -1;
						return result;
					}
					result = 1;
					return result;
				}
				catch
				{
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
			return -1;
		}
	}
}
