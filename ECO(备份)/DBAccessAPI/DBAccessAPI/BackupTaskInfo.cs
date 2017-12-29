using System;
using System.Data;
namespace DBAccessAPI
{
	public class BackupTaskInfo
	{
		private int i_id;
		private string str_taskname = "";
		private int i_ttype;
		private int i_sttype = 1;
		private string str_usr = "";
		private string str_pwd = "";
		private string str_path = "";
		private string str_host = "";
		private int i_port;
		public int ID
		{
			get
			{
				return this.i_id;
			}
			set
			{
				this.i_id = value;
			}
		}
		public int TaskType
		{
			get
			{
				return this.i_ttype;
			}
			set
			{
				this.i_ttype = value;
			}
		}
		public int Port
		{
			get
			{
				return this.i_port;
			}
			set
			{
				this.i_port = value;
			}
		}
		public string Host
		{
			get
			{
				return this.str_host;
			}
			set
			{
				this.str_host = value;
			}
		}
		public string TaskName
		{
			get
			{
				return this.str_taskname;
			}
			set
			{
				this.str_taskname = value;
			}
		}
		public int OperationType
		{
			get
			{
				return this.i_sttype;
			}
			set
			{
				this.i_sttype = value;
			}
		}
		public string UserName
		{
			get
			{
				return this.str_usr;
			}
			set
			{
				this.str_usr = value;
			}
		}
		public string Pwd
		{
			get
			{
				return this.str_pwd;
			}
			set
			{
				this.str_pwd = value;
			}
		}
		public string FilePath
		{
			get
			{
				return this.str_path;
			}
			set
			{
				this.str_path = value;
			}
		}
		public BackupTaskInfo(string str_name, int i_op, string strhost, int iport, string susr, string spwd, string spath)
		{
			this.str_taskname = str_name;
			this.i_sttype = i_op;
			this.str_usr = susr;
			this.str_pwd = spwd;
			this.str_path = spath;
			this.str_host = strhost;
			this.i_port = iport;
		}
		public BackupTaskInfo(DataRow dr_bct)
		{
			this.i_id = Convert.ToInt32(dr_bct["id"]);
			this.str_taskname = Convert.ToString(dr_bct["taskname"]);
			this.i_sttype = Convert.ToInt32(dr_bct["storetype"]);
			this.i_ttype = Convert.ToInt32(dr_bct["tasktype"]);
			this.str_usr = Convert.ToString(dr_bct["username"]);
			this.str_pwd = Convert.ToString(dr_bct["pwd"]);
			this.str_host = Convert.ToString(dr_bct["host"]);
			this.str_path = Convert.ToString(dr_bct["filepath"]);
			this.i_port = Convert.ToInt32(dr_bct["port"]);
		}
	}
}
