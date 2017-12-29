using CommonAPI;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class SMTPSetting
	{
		public const int SMTPSERVER_ENABLE = 1;
		public const int SMTPSERVER_DISABLE = 0;
		public const int AUTHENTICATION_REQUIRED = 1;
		public const int AUTHENTICATION_DISABLE = 0;
		private int smtp_enable = 1;
		private string smtp_server = string.Empty;
		private int smtp_port;
		private string smtp_from = string.Empty;
		private string smtp_to = string.Empty;
		private string smtp_event = string.Empty;
		private int smtp_auth;
		private string smtp_account = string.Empty;
		private string smtp_pwd = string.Empty;
		public int Status
		{
			get
			{
				return this.smtp_enable;
			}
			set
			{
				this.smtp_enable = value;
			}
		}
		public string ServerIP
		{
			get
			{
				return this.smtp_server;
			}
			set
			{
				this.smtp_server = value;
			}
		}
		public int Port
		{
			get
			{
				return this.smtp_port;
			}
			set
			{
				this.smtp_port = value;
			}
		}
		public string Sender
		{
			get
			{
				return this.smtp_from;
			}
			set
			{
				this.smtp_from = value;
			}
		}
		public string Receiver
		{
			get
			{
				return this.smtp_to;
			}
			set
			{
				this.smtp_to = value;
			}
		}
		public string EVENT
		{
			get
			{
				return this.smtp_event;
			}
			set
			{
				this.smtp_event = value;
			}
		}
		public int AuthenticationFlag
		{
			get
			{
				return this.smtp_auth;
			}
			set
			{
				this.smtp_auth = value;
			}
		}
		public string AccountName
		{
			get
			{
				return this.smtp_account;
			}
			set
			{
				this.smtp_account = value;
			}
		}
		public string AccountPwd
		{
			get
			{
				return this.smtp_pwd;
			}
			set
			{
				this.smtp_pwd = value;
			}
		}
		public SMTPSetting(int i_enable, string str_ip, int i_port, string str_from, string str_to, string str_event, int i_authflag, string str_account, string str_pwd)
		{
			this.smtp_enable = i_enable;
			this.smtp_server = str_ip;
			this.smtp_port = i_port;
			this.smtp_from = str_from;
			this.smtp_to = str_to;
			this.smtp_event = str_event;
			this.smtp_auth = i_authflag;
			this.smtp_account = str_account;
			this.smtp_pwd = str_pwd;
		}
		public SMTPSetting()
		{
			try
			{
				SMTPSetting sMTPSetting = DBCache.GetSMTPSetting();
				if (sMTPSetting != null)
				{
					this.smtp_enable = sMTPSetting.Status;
					this.smtp_server = sMTPSetting.ServerIP;
					this.smtp_port = sMTPSetting.Port;
					this.smtp_from = sMTPSetting.Sender;
					this.smtp_to = sMTPSetting.Receiver;
					this.smtp_event = sMTPSetting.EVENT;
					this.smtp_auth = sMTPSetting.AuthenticationFlag;
					this.smtp_account = sMTPSetting.AccountName;
					this.smtp_pwd = sMTPSetting.AccountPwd;
				}
				else
				{
					this.smtp_enable = 0;
					this.smtp_server = "";
					this.smtp_port = 25;
					this.smtp_from = "";
					this.smtp_to = "";
					this.smtp_event = "";
					this.smtp_auth = 0;
					this.smtp_account = "";
					this.smtp_pwd = "";
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public int UpdateSetting()
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
						string commandText = "update smtpsetting set EnableSMTP=?EnableSMTP , ServerAddress=?ServerAddress , PortId=?PortId , EmailFrom=?EmailFrom , EmailTo=?EmailTo , SendEvent=?SendEvent , EnableAuth=?EnableAuth, Account=?Account, UserPwd=?UserPwd ";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?EnableSMTP", this.smtp_enable, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?ServerAddress", this.smtp_server, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?PortId", this.smtp_port, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?EmailFrom", this.smtp_from, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?EmailTo", this.smtp_to, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?SendEvent", this.smtp_event, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?EnableAuth", this.smtp_auth, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?Account", this.smtp_account, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?UserPwd", this.smtp_pwd, dbCommand));
					}
					else
					{
						string commandText = "update smtpsetting set EnableSMTP=? , ServerAddress=? , PortId=? , EmailFrom=? , EmailTo=? , SendEvent=? , EnableAuth=?, Account=?, UserPwd=? ";
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@EnableSMTP", this.smtp_enable, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@ServerAddress", this.smtp_server, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@PortId", this.smtp_port, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@EmailFrom", this.smtp_from, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@EmailTo", this.smtp_to, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@SendEvent", this.smtp_event, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@EnableAuth", this.smtp_auth, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@Account", this.smtp_account, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@UserPwd", this.smtp_pwd, dbCommand));
					}
					int num = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					DBCacheStatus.Smtp = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Smtp"
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
	}
}
