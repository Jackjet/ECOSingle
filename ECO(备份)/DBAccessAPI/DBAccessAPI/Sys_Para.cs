using CommonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Threading;
namespace DBAccessAPI
{
	public class Sys_Para
	{
		private class MyConvert
		{
			private static CultureInfo cultureInfo = new CultureInfo("en-US");
			public static string ToString(object f_value)
			{
				return Convert.ToString(f_value, Sys_Para.MyConvert.cultureInfo);
			}
			public static float ToSingle(object str)
			{
				return Convert.ToSingle(str, Sys_Para.MyConvert.cultureInfo);
			}
			public static float ToSingle(string str)
			{
				return Convert.ToSingle(str, Sys_Para.MyConvert.cultureInfo);
			}
			public static int ToInt32(object str)
			{
				return Convert.ToInt32(str, Sys_Para.MyConvert.cultureInfo);
			}
			public static int ToInt32(string str)
			{
				return Convert.ToInt32(str, Sys_Para.MyConvert.cultureInfo);
			}
			public static string ToString(float f_value)
			{
				return Convert.ToString(f_value, Sys_Para.MyConvert.cultureInfo);
			}
			public static string ToString(int i_value)
			{
				return Convert.ToString(i_value, Sys_Para.MyConvert.cultureInfo);
			}
			public static string ToString(float f_value, string style)
			{
				return f_value.ToString(style, Sys_Para.MyConvert.cultureInfo);
			}
			public static string ToString(int i_value, string style)
			{
				return i_value.ToString(style, Sys_Para.MyConvert.cultureInfo);
			}
			public static float floatParse(string szInput)
			{
				return float.Parse(szInput, Sys_Para.MyConvert.cultureInfo);
			}
			public static double doubleParse(string szInput)
			{
				return double.Parse(szInput, Sys_Para.MyConvert.cultureInfo);
			}
			public static void MyTest()
			{
				Console.Write("\r\n\r\nCurrentCulture - Before:\r\n");
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				Console.Write(currentCulture.Name);
				Console.Write(currentCulture.TwoLetterISOLanguageName);
				Console.Write(currentCulture.ThreeLetterISOLanguageName);
				Console.Write(currentCulture.ThreeLetterWindowsLanguageName);
				Console.Write(currentCulture.DisplayName);
				Console.Write(currentCulture.EnglishName);
				Console.Write("\r\n\r\nCurrentUICulture - Before:\r\n");
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				Console.Write(currentUICulture.Name);
				Console.Write(currentUICulture.TwoLetterISOLanguageName);
				Console.Write(currentUICulture.ThreeLetterISOLanguageName);
				Console.Write(currentUICulture.ThreeLetterWindowsLanguageName);
				Console.Write(currentUICulture.DisplayName);
				Console.Write(currentUICulture.EnglishName);
				Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-BE");
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-BE");
				Convert.ToString(123.567886f);
				Convert.ToString(123.567886f, Sys_Para.MyConvert.cultureInfo);
				123.567886f.ToString("F2");
				123.567886f.ToString("F4", new CultureInfo("fr-BE"));
				string text = 15.789f.ToString(CultureInfo.InvariantCulture);
				float.Parse(text, CultureInfo.InvariantCulture);
				string s = DateTime.Now.ToString(CultureInfo.InvariantCulture);
				DateTime.Parse(s, CultureInfo.InvariantCulture);
				Console.Write("InvariantCulture 15.798 -> " + text);
				string str = 15.789f.ToString();
				Console.Write("Default 15.798 -> " + str);
				float value = float.Parse("123,45");
				Convert.ToString(value);
				float value2 = float.Parse("123.45");
				Convert.ToString(value2);
				double.Parse("123.45");
				double.Parse("123,45");
				"123,45".Replace(",", ".");
				"123.45".Replace(",", ".");
				"123450".Replace(",", ".");
			}
		}
		public const int ENABLE_ISG = 1;
		public const int DISABLE_ISG = 0;
		public const int ENABLE_COMM = 1;
		public const int DISABLE_COMM = 0;
		public const int TREATITPOWER_CHECKED = 1;
		public const int TREATITPOWER_UNCHECKED = 0;
		public const int ENERGY_USERDEFINE = 0;
		public const int ENERGY_DEVICE = 1;
		public const int ENERGY_RANDOM_DEVICE = 2;
		public const int PORT_STATUS_CHANGED = 1;
		public const int PORT_STATUS_UNCHANGED = 0;
		public const int DEVICE_DB_CHANGED = 1;
		public const int DEVICE_DB_UNCHANGED = 0;
		public const int SNMP_V1 = 1;
		public const int SNMP_V2 = 2;
		public const int SNMP_V3 = 3;
		public const int TRAPSNMP_V12 = 1;
		public const int TRAPSNMP_V123 = 2;
		public const string SNMP_AUTH_NONE = "None";
		public const string SNMP_AUTH_MD5 = "MD5";
		public const string SNMP_AUTH_SHA = "SHA";
		public const string SNMP_PRIV_NONE = "None";
		public const string SNMP_PRIV_AES = "AES";
		public const string SNMP_PRIV_DES = "DES";
		public const string SYS_USERNAME = "USERNAME";
		public const string SYS_TRAP_COMMUNITY = "TUSERNAME";
		public const string SYS_PORTID = "PORT";
		public const string SYS_TRAPPORT = "TRAP_PORT";
		public const string SYS_TIMEOUT = "TIMEOUT";
		public const string SYS_RETRY = "RETRY";
		public const string SYS_SNMPVER = "SNMP_VER";
		public const string SYS_TRAP_SNMPVER = "TSNMP_VER";
		public const string SYS_TRAP_PRIVACY = "TPRIVACY";
		public const string SYS_TRAP_PPWD = "TPRIVACYPWD";
		public const string SYS_TRAP_AUTHEN = "TAUTHEN";
		public const string SYS_TRAP_APWD = "TAUTHENPWD";
		public const int BILL_RATETYPE_Single = 0;
		public const int BILL_RATETYPE_Multi = 1;
		private static CultureInfo cultureInfo = new CultureInfo("en-US");
		private int trap_snmpver = 1;
		private string trap_privacy = "AES";
		private string trap_authen = "MD5";
		private string trap_privacy_pwd;
		private string trap_authen_pwd;
		private string trap_user_name;
		private int i_dbupdateflag = 1;
		private DataSet DS_dbsource = new DataSet("sys_para");
		private int Service_Delay = 60;
		private int RackeffectRemainHours = 1;
		private int Energy_Box = 230;
		private int Port;
		private int Trap_Port;
		private int Timeout;
		private int Retry;
		private int Snmp_Version;
		private string Privacy = "AES";
		private string Authen = "MD5";
		private string Privacy_Pwd;
		private string Authen_Pwd;
		private string User_Name;
		public int TrapSnmpVersion
		{
			get
			{
				return this.trap_snmpver;
			}
			set
			{
				this.trap_snmpver = value;
			}
		}
		public string TrapPrivacy
		{
			get
			{
				return this.trap_privacy;
			}
			set
			{
				this.trap_privacy = value;
			}
		}
		public string TrapPrivacyPwd
		{
			get
			{
				return this.trap_privacy_pwd;
			}
			set
			{
				this.trap_privacy_pwd = value;
			}
		}
		public string TrapAuthen
		{
			get
			{
				return this.trap_authen;
			}
			set
			{
				this.trap_authen = value;
			}
		}
		public string TrapAuthenPwd
		{
			get
			{
				return this.trap_authen_pwd;
			}
			set
			{
				this.trap_authen_pwd = value;
			}
		}
		public string TrapUserName
		{
			get
			{
				return this.trap_user_name;
			}
			set
			{
				this.trap_user_name = value;
			}
		}
		public int ServiceDelay
		{
			get
			{
				if (this.Service_Delay <= 30)
				{
					return 30;
				}
				if (this.Service_Delay > 30 && this.Service_Delay <= 60)
				{
					return 60;
				}
				if (this.Service_Delay > 60 && this.Service_Delay <= 180)
				{
					return 180;
				}
				int num = this.Service_Delay % 30;
				if (num > 0)
				{
					int num2 = (this.Service_Delay / 30 + 1) * 30;
					if (num2 > 900)
					{
						num2 = 900;
					}
					return num2;
				}
				return this.Service_Delay;
			}
			set
			{
				this.Service_Delay = value;
			}
		}
		public int EnergyBox
		{
			get
			{
				return this.Energy_Box;
			}
			set
			{
				this.Energy_Box = value;
			}
		}
		public int port
		{
			get
			{
				return this.Port;
			}
			set
			{
				this.Port = value;
			}
		}
		public int TrapPort
		{
			get
			{
				return this.Trap_Port;
			}
			set
			{
				this.Trap_Port = value;
			}
		}
		public int timeout
		{
			get
			{
				return this.Timeout;
			}
			set
			{
				this.Timeout = value;
			}
		}
		public int retry
		{
			get
			{
				return this.Retry;
			}
			set
			{
				this.Retry = value;
			}
		}
		public int SnmpVersion
		{
			get
			{
				return this.Snmp_Version;
			}
			set
			{
				this.Snmp_Version = value;
			}
		}
		public string privacy
		{
			get
			{
				return this.Privacy;
			}
			set
			{
				this.Privacy = value;
			}
		}
		public string authen
		{
			get
			{
				return this.Authen;
			}
			set
			{
				this.Authen = value;
			}
		}
		public string privacypwd
		{
			get
			{
				return this.Privacy_Pwd;
			}
			set
			{
				this.Privacy_Pwd = value;
			}
		}
		public string authenpwd
		{
			get
			{
				return this.Authen_Pwd;
			}
			set
			{
				this.Authen_Pwd = value;
			}
		}
		public string username
		{
			get
			{
				return this.User_Name;
			}
			set
			{
				this.User_Name = value;
			}
		}
		public int rackeffectRemainHours
		{
			get
			{
				return this.RackeffectRemainHours;
			}
			set
			{
				this.RackeffectRemainHours = value;
			}
		}
		public Sys_Para()
		{
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0)
				{
					if (sysParameterCache.ContainsKey("SERVICEDELAY"))
					{
						this.Service_Delay = (int)Convert.ToInt16(sysParameterCache["SERVICEDELAY"]);
					}
					if (sysParameterCache.ContainsKey("ENERGY"))
					{
						this.Energy_Box = Convert.ToInt32(sysParameterCache["ENERGY"]);
					}
					if (sysParameterCache.ContainsKey("PORT"))
					{
						this.Port = (int)Convert.ToInt16(sysParameterCache["PORT"]);
					}
					if (sysParameterCache.ContainsKey("TRAPPORT"))
					{
						this.Trap_Port = Convert.ToInt32(sysParameterCache["TRAPPORT"]);
					}
					if (sysParameterCache.ContainsKey("TIMEOUT"))
					{
						this.Timeout = Convert.ToInt32(sysParameterCache["TIMEOUT"]);
					}
					if (sysParameterCache.ContainsKey("RETRY"))
					{
						this.Retry = Convert.ToInt32(sysParameterCache["RETRY"]);
					}
					if (sysParameterCache.ContainsKey("SNMP_VER"))
					{
						this.Snmp_Version = Convert.ToInt32(sysParameterCache["SNMP_VER"]);
					}
					if (sysParameterCache.ContainsKey("PRIVACY"))
					{
						this.Privacy = Convert.ToString(sysParameterCache["PRIVACY"]);
					}
					if (sysParameterCache.ContainsKey("AUTHEN"))
					{
						this.Authen = Convert.ToString(sysParameterCache["AUTHEN"]);
					}
					if (sysParameterCache.ContainsKey("USERNAME"))
					{
						this.User_Name = Convert.ToString(sysParameterCache["USERNAME"]);
					}
					if (sysParameterCache.ContainsKey("PRIVACYPWD"))
					{
						this.Privacy_Pwd = Convert.ToString(sysParameterCache["PRIVACYPWD"]);
					}
					if (sysParameterCache.ContainsKey("AUTHENPWD"))
					{
						this.Authen_Pwd = Convert.ToString(sysParameterCache["AUTHENPWD"]);
					}
					if (sysParameterCache.ContainsKey("TAUTHENPWD"))
					{
						this.trap_authen_pwd = Convert.ToString(sysParameterCache["TAUTHENPWD"]);
					}
					if (sysParameterCache.ContainsKey("TAUTHEN"))
					{
						this.trap_authen = Convert.ToString(sysParameterCache["TAUTHEN"]);
					}
					if (sysParameterCache.ContainsKey("TUSERNAME"))
					{
						this.trap_user_name = Convert.ToString(sysParameterCache["TUSERNAME"]);
					}
					if (sysParameterCache.ContainsKey("TSNMP_VER"))
					{
						this.trap_snmpver = Convert.ToInt32(sysParameterCache["TSNMP_VER"]);
					}
					if (sysParameterCache.ContainsKey("TPRIVACY"))
					{
						this.trap_privacy = Convert.ToString(sysParameterCache["TPRIVACY"]);
					}
					if (sysParameterCache.ContainsKey("TPRIVACYPWD"))
					{
						this.trap_privacy_pwd = Convert.ToString(sysParameterCache["TPRIVACYPWD"]);
					}
					if (sysParameterCache.ContainsKey("DEVICEUPDATEFLAG"))
					{
						this.i_dbupdateflag = Convert.ToInt32(sysParameterCache["DEVICEUPDATEFLAG"]);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public void update()
		{
			if (DBUrl.SERVERMODE)
			{
				DBConn dBConn = null;
				DbCommand dbCommand = new OleDbCommand();
				try
				{
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn.con != null)
						{
							dbCommand = DBConn.GetCommandObject(dBConn.con);
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "update snmpsetting set snmpusername=?snmpusername,snmpport=?snmpport,snmpver=?snmpver,snmppprl=?snmppprl,snmpppwd=?snmpppwd,snmpaprl=?snmpaprl,snmpapwd=?snmpapwd,snmptimeout=?snmptimeout,snmpretry=?snmpretry,trapusername=?trapusername,trapport=?trapport,trapver=?trapver,trappprl=?trappprl,trapppwd=?trapppwd,trapaprl=?trapaprl,trapapwd=?trapapwd";
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmpusername", this.User_Name, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmpport", this.Port, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmpver", this.Snmp_Version, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmppprl", this.Privacy, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmpppwd", this.Privacy_Pwd, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmpaprl", this.Authen, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmpapwd", this.Authen_Pwd, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmptimeout", this.Timeout, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?snmpretry", this.Retry, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?trapusername", this.trap_user_name, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?trapport", this.Trap_Port, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?trapver", this.trap_snmpver, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?trappprl", this.trap_privacy, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?trapppwd", this.trap_privacy_pwd, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?trapaprl", this.trap_authen, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?trapapwd", this.trap_authen_pwd, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "update snmpsetting set snmpusername=?,snmpport=?,snmpver=?,snmppprl=?,snmpppwd=?,snmpaprl=?,snmpapwd=?,snmptimeout=?,snmpretry=?,trapusername=?,trapport=?,trapver=?,trappprl=?,trapppwd=?,trapaprl=?,trapapwd=?";
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmpusername", this.User_Name, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmpport", this.Port, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmpver", this.Snmp_Version, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmppprl", this.Privacy, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmpppwd", this.Privacy_Pwd, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmpaprl", this.Authen, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmpapwd", this.Authen_Pwd, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmptimeout", this.Timeout, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@snmpretry", this.Retry, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@trapusername", this.trap_user_name, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@trapport", this.Trap_Port, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@trapver", this.trap_snmpver, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@trappprl", this.trap_privacy, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@trapppwd", this.trap_privacy_pwd, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@trapaprl", this.trap_authen, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@trapapwd", this.trap_authen_pwd, dbCommand));
							}
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "update systemparameter set ServiceDelay = " + this.Service_Delay;
							dbCommand.ExecuteNonQuery();
							DBCacheStatus.SystemParameter = true;
							DBCacheStatus.DBSyncEventSet(true, new string[]
							{
								"DBSyncEventName_Service_SystemParameter"
							});
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
					}
					return;
				}
				finally
				{
					if (dBConn != null)
					{
						dBConn.close();
					}
				}
			}
			DBConn dBConn2 = null;
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			if (this.DS_dbsource == null)
			{
				return;
			}
			"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBUrl.DEFAULT_HOST_PATH + ";Jet OLEDB:Database Password=" + DBUrl.DEFAULT_PWD;
			try
			{
				dBConn2 = DBConnPool.getConnection();
				oleDbDataAdapter.SelectCommand = new OleDbCommand("select * from sys_para where para_name in ('SERVICEDELAY','ENERGY','PORT','TRAPPORT','TIMEOUT','RETRY','SNMP_VER','PRIVACY','AUTHEN','USERNAME','PRIVACYPWD','AUTHENPWD','TSNMP_VER','TUSERNAME','TPRIVACY','TPRIVACYPWD','TAUTHEN','TAUTHENPWD') ", (OleDbConnection)dBConn2.con);
				OleDbCommandBuilder oleDbCommandBuilder = new OleDbCommandBuilder(oleDbDataAdapter);
				DataTable dataTable = new DataTable("sys_para");
				oleDbDataAdapter.Fill(dataTable);
				foreach (DataRow dataRow in dataTable.Rows)
				{
					if (dataRow[0].Equals("SERVICEDELAY"))
					{
						dataRow[2] = this.Service_Delay;
					}
					if (dataRow[0].Equals("ENERGY"))
					{
						dataRow[2] = this.Energy_Box;
					}
					if (dataRow[0].Equals("PORT"))
					{
						dataRow[2] = this.Port;
					}
					if (dataRow[0].Equals("TRAPPORT"))
					{
						dataRow[2] = this.Trap_Port;
					}
					if (dataRow[0].Equals("TIMEOUT"))
					{
						dataRow[2] = this.Timeout;
					}
					if (dataRow[0].Equals("RETRY"))
					{
						dataRow[2] = this.Retry;
					}
					if (dataRow[0].Equals("SNMP_VER"))
					{
						dataRow[2] = this.Snmp_Version;
					}
					if (dataRow[0].Equals("PRIVACY"))
					{
						dataRow[2] = this.Privacy;
					}
					if (dataRow[0].Equals("AUTHEN"))
					{
						dataRow[2] = this.Authen;
					}
					if (dataRow[0].Equals("USERNAME"))
					{
						dataRow[2] = this.User_Name;
					}
					if (dataRow[0].Equals("PRIVACYPWD"))
					{
						dataRow[2] = this.Privacy_Pwd;
					}
					if (dataRow[0].Equals("AUTHENPWD"))
					{
						dataRow[2] = this.Authen_Pwd;
					}
					if (dataRow[0].Equals("TSNMP_VER"))
					{
						dataRow[2] = this.trap_snmpver;
					}
					if (dataRow[0].Equals("TUSERNAME"))
					{
						dataRow[2] = this.trap_user_name;
					}
					if (dataRow[0].Equals("TPRIVACY"))
					{
						dataRow[2] = this.trap_privacy;
					}
					if (dataRow[0].Equals("TPRIVACYPWD"))
					{
						dataRow[2] = this.trap_privacy_pwd;
					}
					if (dataRow[0].Equals("TAUTHEN"))
					{
						dataRow[2] = this.trap_authen;
					}
					if (dataRow[0].Equals("TAUTHENPWD"))
					{
						dataRow[2] = this.trap_authen_pwd;
					}
				}
				oleDbDataAdapter.UpdateCommand = oleDbCommandBuilder.GetUpdateCommand();
				oleDbDataAdapter.Update(dataTable);
				oleDbDataAdapter.Dispose();
				DBCacheStatus.SystemParameter = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_Service_SystemParameter"
				});
			}
			catch (Exception)
			{
			}
			finally
			{
				try
				{
					oleDbDataAdapter.Dispose();
				}
				catch
				{
				}
				if (dBConn2 != null)
				{
					dBConn2.close();
				}
			}
		}
		public static string GetThermalPath()
		{
			string result = "";
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0 && sysParameterCache.ContainsKey("THERMALPATH"))
				{
					return Convert.ToString(sysParameterCache["THERMALPATH"]);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static string GetBillPath()
		{
			string result = "";
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0 && sysParameterCache.ContainsKey("BILLPATH"))
				{
					return Convert.ToString(sysParameterCache["BILLPATH"]);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static string GetDefinePath()
		{
			string result = "";
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0 && sysParameterCache.ContainsKey("DEFINEPATH"))
				{
					return Convert.ToString(sysParameterCache["DEFINEPATH"]);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static int UpdateThermalPath(string str_path)
		{
			int num = -1;
			string str = str_path.Replace("'", "''");
			string commandText = "update sys_para set para_value = '" + str + "' where  para_name = 'THERMALPATH' and para_type = 'String' ";
			string commandText2 = "insert into sys_para (para_name,para_type,para_value) values ('THERMALPATH','String','" + str + "')";
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = commandText;
					num = dbCommand.ExecuteNonQuery();
					if (num < 1)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText2;
						num = dbCommand.ExecuteNonQuery();
					}
				}
				dbCommand.Dispose();
				DBCacheStatus.SystemParameter = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_Service_SystemParameter"
				});
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
			return num;
		}
		public static int UpdateBillPath(string str_path)
		{
			int num = -1;
			string str = str_path.Replace("'", "''");
			string commandText = "update sys_para set para_value = '" + str + "' where  para_name = 'BILLPATH' and para_type = 'String' ";
			string commandText2 = "insert into sys_para (para_name,para_type,para_value) values ('BILLPATH','String','" + str + "')";
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = commandText;
					num = dbCommand.ExecuteNonQuery();
					if (num < 1)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText2;
						num = dbCommand.ExecuteNonQuery();
					}
				}
				dbCommand.Dispose();
				DBCacheStatus.SystemParameter = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_Service_SystemParameter"
				});
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
			return num;
		}
		public static int UpdateDefinePath(string str_path)
		{
			int num = -1;
			string str = str_path.Replace("'", "''");
			string commandText = "update sys_para set para_value = '" + str + "' where  para_name = 'DEFINEPATH' and para_type = 'String' ";
			string commandText2 = "insert into sys_para (para_name,para_type,para_value) values ('DEFINEPATH','String','" + str + "')";
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = commandText;
					num = dbCommand.ExecuteNonQuery();
					if (num < 1)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText2;
						num = dbCommand.ExecuteNonQuery();
					}
				}
				dbCommand.Dispose();
				DBCacheStatus.SystemParameter = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_Service_SystemParameter"
				});
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
			return num;
		}
		public static string GetParameter(string para_name, string para_type)
		{
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0 && sysParameterCache.ContainsKey(para_name))
				{
					return Convert.ToString(sysParameterCache[para_name]);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return "";
		}
		public static object GetParameter(string para_name, Type para_type)
		{
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0 && sysParameterCache.ContainsKey(para_name))
				{
					return sysParameterCache[para_name];
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return null;
		}
		public static int SetParameter(string para_name, Type para_type, object obj_value)
		{
			if (DBUrl.SERVERMODE)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("SERVICEDELAY", "ServiceDelay");
				dictionary.Add("RESOLUTION", "Layout");
				dictionary.Add("ENERGYTYPE", "EnergyType");
				dictionary.Add("ENERGYVALUE", "EnergyValue");
				dictionary.Add("ENERGYDEVICE", "ReferenceDevice");
				dictionary.Add("CO2KG", "CO2KG");
				dictionary.Add("CO2COST", "CO2COST");
				dictionary.Add("ELECTRICITY", "Electricitycost");
				dictionary.Add("TEMPERATUREUNIT", "TemperatureUnit");
				dictionary.Add("CURRENCY", "Currency");
				dictionary.Add("RACKFULLNAMEFLAG", "RackfullnameFlag");
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				dictionary2.Add("DBOPT_keepMMFLAG", "bytimeflag");
				dictionary2.Add("DBOPT_keepMM", "keepcount");
				dictionary2.Add("DBOPT_delOldFLAG", "byrecordflag");
				int num = -1;
				if (para_type == null)
				{
					return -1;
				}
				if (obj_value == null)
				{
					return -2;
				}
				string fullName;
				if ((fullName = para_type.FullName) == null || (!(fullName == "System.Int32") && !(fullName == "System.String") && !(fullName == "System.Double") && !(fullName == "System.Single") && !(fullName == "System.DateTime")))
				{
					return -3;
				}
				string commandText = string.Concat(new string[]
				{
					"insert into sys_para (para_name,para_type,para_value) values ('",
					para_name,
					"','",
					para_type.FullName,
					"','",
					Sys_Para.MyConvert.ToString(obj_value),
					"')"
				});
				string commandText2 = string.Concat(new string[]
				{
					"update sys_para set para_value = '",
					Sys_Para.MyConvert.ToString(obj_value),
					"'  where  para_name = '",
					para_name,
					"' and para_type = '",
					para_type.FullName,
					"'"
				});
				DBConn dBConn = null;
				DbCommand dbCommand = new OleDbCommand();
				DbTransaction dbTransaction = null;
				try
				{
					dBConn = DBConnPool.getConnection();
					if (dBConn.con != null)
					{
						dbCommand = DBConn.GetCommandObject(dBConn.con);
						if (dictionary.ContainsKey(para_name))
						{
							if (para_name.Equals("CURRENCY"))
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"update systemparameter set ",
									dictionary[para_name],
									" ='",
									Sys_Para.MyConvert.ToString(obj_value),
									"' "
								});
							}
							else
							{
								dbCommand.CommandText = "update systemparameter set " + dictionary[para_name] + " = " + Sys_Para.MyConvert.ToString(obj_value);
							}
							num = dbCommand.ExecuteNonQuery();
						}
						else
						{
							if (dictionary2.ContainsKey(para_name))
							{
								dbCommand.CommandText = "update cleanupsetting set " + dictionary2[para_name] + " = " + Sys_Para.MyConvert.ToString(obj_value);
								num = dbCommand.ExecuteNonQuery();
							}
							else
							{
								dbTransaction = dBConn.con.BeginTransaction();
								dbCommand.Transaction = dbTransaction;
								dbCommand.CommandType = CommandType.Text;
								dbCommand.CommandText = commandText2;
								num = dbCommand.ExecuteNonQuery();
								if (num < 1)
								{
									dbCommand.Parameters.Clear();
									dbCommand.CommandText = commandText;
									num = dbCommand.ExecuteNonQuery();
								}
								dbCommand.Transaction.Commit();
							}
						}
					}
					if (dbTransaction != null)
					{
						dbTransaction.Dispose();
					}
					dbCommand.Dispose();
					DBCacheStatus.SystemParameter = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_SystemParameter"
					});
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
				return num;
			}
			else
			{
				int num2 = -1;
				if (para_type == null)
				{
					return -1;
				}
				if (obj_value == null)
				{
					return -2;
				}
				string fullName2;
				if ((fullName2 = para_type.FullName) == null || (!(fullName2 == "System.Int32") && !(fullName2 == "System.String") && !(fullName2 == "System.Double") && !(fullName2 == "System.Single") && !(fullName2 == "System.DateTime")))
				{
					return -3;
				}
				string commandText3 = string.Concat(new string[]
				{
					"insert into sys_para (para_name,para_type,para_value) values ('",
					para_name,
					"','",
					para_type.FullName,
					"','",
					Sys_Para.MyConvert.ToString(obj_value).Replace("'", "''"),
					"')"
				});
				string commandText4 = string.Concat(new string[]
				{
					"update sys_para set para_value = '",
					Sys_Para.MyConvert.ToString(obj_value).Replace("'", "''"),
					"'  where  para_name = '",
					para_name,
					"' and para_type = '",
					para_type.FullName,
					"'"
				});
				DBConn dBConn2 = null;
				DbCommand dbCommand2 = new OleDbCommand();
				DbTransaction dbTransaction2 = null;
				try
				{
					dBConn2 = DBConnPool.getConnection();
					if (dBConn2.con != null)
					{
						dbCommand2 = dBConn2.con.CreateCommand();
						dbTransaction2 = dBConn2.con.BeginTransaction();
						dbCommand2.Transaction = dbTransaction2;
						dbCommand2.CommandType = CommandType.Text;
						dbCommand2.CommandText = commandText4;
						num2 = dbCommand2.ExecuteNonQuery();
						if (num2 < 1)
						{
							dbCommand2.Parameters.Clear();
							dbCommand2.CommandText = commandText3;
							num2 = dbCommand2.ExecuteNonQuery();
						}
						dbCommand2.Transaction.Commit();
					}
					dbTransaction2.Dispose();
					dbCommand2.Dispose();
					DBCacheStatus.SystemParameter = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_SystemParameter"
					});
				}
				catch (Exception)
				{
				}
				finally
				{
					try
					{
						dbCommand2.Dispose();
					}
					catch
					{
					}
					if (dBConn2 != null)
					{
						dBConn2.close();
					}
				}
				return num2;
			}
		}
		public static bool GetEnablePowerControlFlag()
		{
			object parameter = Sys_Para.GetParameter("ENABLEPOWERCTRL", typeof(int));
			if (parameter == null)
			{
				return true;
			}
			int num = -1;
			try
			{
				num = Convert.ToInt32(parameter);
			}
			catch
			{
			}
			return num != 1;
		}
		public static int SetEnablePowerControlFlag(bool b_v)
		{
			if (b_v)
			{
				return Sys_Para.SetParameter("ENABLEPOWERCTRL", typeof(int), 0);
			}
			return Sys_Para.SetParameter("ENABLEPOWERCTRL", typeof(int), 1);
		}
		public static int GetEnergyType()
		{
			object parameter = Sys_Para.GetParameter("ENERGYTYPE", typeof(int));
			if (parameter == null)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetEnergyType(int i_type)
		{
			if (i_type < 0 || i_type > 2)
			{
				return -1;
			}
			return Sys_Para.SetParameter("ENERGYTYPE", typeof(int), i_type);
		}
		public static float GetEnergyValue()
		{
			object parameter = Sys_Para.GetParameter("ENERGYVALUE", typeof(float));
			if (parameter != null)
			{
				return Sys_Para.MyConvert.ToSingle(parameter);
			}
			string parameter2 = Sys_Para.GetParameter("ENERGY", "int");
			if (parameter2.Length > 0)
			{
				return Sys_Para.MyConvert.ToSingle(parameter2);
			}
			return 230f;
		}
		public static int SetEnergyValue(float f_value)
		{
			if (!DBCacheStatus.EnergyDBReady)
			{
				return 0;
			}
			int result = Sys_Para.SetParameter("ENERGYVALUE", typeof(float), f_value);
			DBCacheStatus.EnergyDBReady = true;
			return result;
		}
		public static int GetReferenceDevice()
		{
			object parameter = Sys_Para.GetParameter("ENERGYDEVICE", typeof(int));
			if (parameter == null)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetReferenceDevice(int i_did)
		{
			if (i_did < 1)
			{
				return -1;
			}
			return Sys_Para.SetParameter("ENERGYDEVICE", typeof(int), i_did);
		}
		public static int GetServiceDelay()
		{
			try
			{
				Hashtable sysParameterCache = DBCache.GetSysParameterCache();
				if (sysParameterCache != null && sysParameterCache.Count > 0 && sysParameterCache.ContainsKey("SERVICEDELAY"))
				{
					int num = Convert.ToInt32(sysParameterCache["SERVICEDELAY"]);
					int result;
					if (num <= 30)
					{
						result = 30;
						return result;
					}
					if (num > 30 && num <= 60)
					{
						result = 60;
						return result;
					}
					if (num > 60 && num <= 180)
					{
						result = 180;
						return result;
					}
					int num2 = num % 30;
					if (num2 > 0)
					{
						int num3 = (num / 30 + 1) * 30;
						if (num3 > 900)
						{
							num3 = 900;
						}
						result = num3;
						return result;
					}
					result = num;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return -1;
		}
		public static int SetServiceDelay(int i_delay)
		{
			if (DBUrl.SERVERMODE)
			{
				return Sys_Para.SetParameter("SERVICEDELAY", typeof(int), i_delay);
			}
			int num = -1;
			string commandText = "insert into sys_para (para_name,para_type,para_value) values ('SERVICEDELAY','int','" + Sys_Para.MyConvert.ToString(i_delay) + "')";
			string commandText2 = "update sys_para set para_value = '" + Sys_Para.MyConvert.ToString(i_delay) + "'  where  para_name = 'SERVICEDELAY' and para_type = 'int'";
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = commandText2;
					num = dbCommand.ExecuteNonQuery();
					if (num < 1)
					{
						dbCommand.Parameters.Clear();
						dbCommand.CommandText = commandText;
						num = dbCommand.ExecuteNonQuery();
					}
				}
				dbCommand.Dispose();
				DBCacheStatus.SystemParameter = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_Service_SystemParameter"
				});
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
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return num;
		}
		public static float GetCO2KG()
		{
			object parameter = Sys_Para.GetParameter("CO2KG", typeof(float));
			if (parameter == null)
			{
				return 0.65f;
			}
			return Sys_Para.MyConvert.ToSingle(parameter);
		}
		public static int SetCO2KG(float f_value)
		{
			return Sys_Para.SetParameter("CO2KG", typeof(float), f_value);
		}
		public static float GetELECTRICITYCOST()
		{
			object parameter = Sys_Para.GetParameter("ELECTRICITY", typeof(float));
			if (parameter == null)
			{
				return 0.1f;
			}
			return Sys_Para.MyConvert.ToSingle(parameter);
		}
		public static int SetELECTRICITYCOST(float f_value)
		{
			return Sys_Para.SetParameter("ELECTRICITY", typeof(float), f_value);
		}
		public static float GetCO2COST()
		{
			object parameter = Sys_Para.GetParameter("CO2COST", typeof(float));
			if (parameter == null || parameter is DBNull)
			{
				return 23f;
			}
			return Sys_Para.MyConvert.ToSingle(parameter);
		}
		public static int SetCO2COST(float f_value)
		{
			return Sys_Para.SetParameter("CO2COST", typeof(float), f_value);
		}
		public static int GetTemperatureUnit()
		{
			object parameter = Sys_Para.GetParameter("TEMPERATUREUNIT", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetTemperatureUnit(int i_value)
		{
			return Sys_Para.SetParameter("TEMPERATUREUNIT", typeof(int), i_value);
		}
		public static int GetResolution()
		{
			object parameter = Sys_Para.GetParameter("RESOLUTION", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 2;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetResolution(int i_value)
		{
			return Sys_Para.SetParameter("RESOLUTION", typeof(int), i_value);
		}
		public static int GetISGFlag()
		{
			object parameter = Sys_Para.GetParameter("ISGFLAG", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetISGFlag(int i_value)
		{
			return Sys_Para.SetParameter("ISGFLAG", typeof(int), i_value);
		}
		public static int GetITPowerFlag()
		{
			object parameter = Sys_Para.GetParameter("ITPOWER", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetITPowerFlag(int i_value)
		{
			return Sys_Para.SetParameter("ITPOWER", typeof(int), i_value);
		}
		public static int GetISGPort()
		{
			object parameter = Sys_Para.GetParameter("ISGPORT", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetISGPort(int i_value)
		{
			return Sys_Para.SetParameter("ISGPORT", typeof(int), i_value);
		}
		public static int GetBPFlag()
		{
			object parameter = Sys_Para.GetParameter("BPFLAG", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetBPFlag(int i_value)
		{
			return Sys_Para.SetParameter("BPFLAG", typeof(int), i_value);
		}
		public static int GetBPPort()
		{
			object parameter = Sys_Para.GetParameter("BPPORT", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetBPPort(int i_value)
		{
			return Sys_Para.SetParameter("BPPORT", typeof(int), i_value);
		}
		public static string GetBPSecurity()
		{
			object parameter = Sys_Para.GetParameter("BPSecurity", typeof(string));
			if (parameter == null || parameter is DBNull)
			{
				return "";
			}
			return Sys_Para.MyConvert.ToString(parameter);
		}
		public static int SetBPSecurity(string s_value)
		{
			return Sys_Para.SetParameter("BPSecurity", typeof(string), s_value);
		}
		public static int GetDBOpt_keepMMflag()
		{
			object parameter = Sys_Para.GetParameter("DBOPT_keepMMFLAG", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 1;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetDBOpt_keepMMflag(int i_value)
		{
			return Sys_Para.SetParameter("DBOPT_keepMMFLAG", typeof(int), i_value);
		}
		public static int GetDBOpt_keepMM()
		{
			object parameter = Sys_Para.GetParameter("DBOPT_keepMM", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 36;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetDBOpt_keepMM(int i_value)
		{
			return Sys_Para.SetParameter("DBOPT_keepMM", typeof(int), i_value);
		}
		public static int GetDBOpt_deloldflag()
		{
			object parameter = Sys_Para.GetParameter("DBOPT_delOldFLAG", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 1;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetDBOpt_deloldflag(int i_value)
		{
			return Sys_Para.SetParameter("DBOPT_delOldFLAG", typeof(int), i_value);
		}
		public static string GetCurrency()
		{
			object parameter = Sys_Para.GetParameter("CURRENCY", typeof(string));
			if (parameter == null || parameter is DBNull)
			{
				return "$";
			}
			string text = Sys_Para.MyConvert.ToString(parameter);
			if (text.Equals("KOR$"))
			{
				text = "₩";
			}
			return text;
		}
		public static int SetCurrency(string s_value)
		{
			if (s_value.Equals("₩"))
			{
				s_value = "KOR$";
			}
			return Sys_Para.SetParameter("CURRENCY", typeof(string), s_value);
		}
		public static int GetRackFullNameflag()
		{
			object parameter = Sys_Para.GetParameter("RACKFULLNAMEFLAG", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetRackFullNameflag(int i_value)
		{
			return Sys_Para.SetParameter("RACKFULLNAMEFLAG", typeof(int), i_value);
		}
		public static int GetBill_ratetype()
		{
			object parameter = Sys_Para.GetParameter("BILL_RATETYPE", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 0;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetBill_ratetype(int i_value)
		{
			return Sys_Para.SetParameter("BILL_RATETYPE", typeof(int), i_value);
		}
		public static float GetBill_1rate()
		{
			object parameter = Sys_Para.GetParameter("BILL_1RATE", typeof(float));
			if (parameter == null || parameter is DBNull)
			{
				return 0f;
			}
			return Sys_Para.MyConvert.ToSingle(parameter);
		}
		public static int SetBill_1rate(float f_value)
		{
			return Sys_Para.SetParameter("BILL_1RATE", typeof(float), f_value);
		}
		public static string GetBill_2from1()
		{
			object parameter = Sys_Para.GetParameter("BILL_2FROM1", typeof(DateTime));
			DateTime dateTime;
			if (parameter == null || parameter is DBNull)
			{
				dateTime = DateTime.Now;
			}
			else
			{
				dateTime = Convert.ToDateTime(parameter);
			}
			return dateTime.ToString("HH:mm:ss");
		}
		public static int SetBill_2from1(string s_value)
		{
			return Sys_Para.SetParameter("BILL_2FROM1", typeof(DateTime), s_value);
		}
		public static int GetBill_2duration1()
		{
			object parameter = Sys_Para.GetParameter("BILL_2DUR1", typeof(int));
			if (parameter == null || parameter is DBNull)
			{
				return 1;
			}
			return Sys_Para.MyConvert.ToInt32(parameter);
		}
		public static int SetBill_2duration1(int i_value)
		{
			return Sys_Para.SetParameter("BILL_2DUR1", typeof(int), i_value);
		}
		public static float GetBill_2rate1()
		{
			object parameter = Sys_Para.GetParameter("BILL_2RATE1", typeof(float));
			if (parameter == null || parameter is DBNull)
			{
				return 0f;
			}
			return Sys_Para.MyConvert.ToSingle(parameter);
		}
		public static int SetBill_2rate1(float f_value)
		{
			return Sys_Para.SetParameter("BILL_2RATE1", typeof(float), f_value);
		}
		public static float GetBill_2rate2()
		{
			object parameter = Sys_Para.GetParameter("BILL_2RATE2", typeof(float));
			if (parameter == null || parameter is DBNull)
			{
				return 0f;
			}
			return Sys_Para.MyConvert.ToSingle(parameter);
		}
		public static int SetBill_2rate2(float f_value)
		{
			return Sys_Para.SetParameter("BILL_2RATE2", typeof(float), f_value);
		}
		public static int ResetAdminPWD()
		{
			int num = -1;
			string fileName = AppDomain.CurrentDomain.BaseDirectory + "ResetAdministratorPassword";
			"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor";
			try
			{
				FileInfo fileInfo = new FileInfo(fileName);
				if (!fileInfo.Exists)
				{
					int result = num;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				int result = num;
				return result;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				dbCommand = DBConn.GetCommandObject(dBConn.con);
				dbCommand.CommandType = CommandType.Text;
				dbCommand.CommandText = "update sys_users set userpwd = 'password' where user_name = 'administrator' ";
				num = dbCommand.ExecuteNonQuery();
				DBCacheStatus.User = true;
				DBCacheStatus.DBSyncEventSet(true, new string[]
				{
					"DBSyncEventName_Service_User"
				});
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
			}
			finally
			{
				dbCommand.Dispose();
				dBConn.Close();
			}
			return num;
		}
	}
}
