using System;
namespace DBAccessAPI
{
	public class TaskInfo
	{
		private string taskname = "";
		private long groupid;
		private int op_type = -1;
		public string TaskName
		{
			get
			{
				return this.taskname;
			}
			set
			{
				this.taskname = value;
			}
		}
		public int OperationType
		{
			get
			{
				return this.op_type;
			}
			set
			{
				this.op_type = value;
			}
		}
		public long GroupID
		{
			get
			{
				return this.groupid;
			}
			set
			{
				this.groupid = value;
			}
		}
		public TaskInfo(string str_name, long l_id, int i_op)
		{
			this.taskname = str_name;
			this.groupid = l_id;
			this.op_type = i_op;
		}
	}
}
