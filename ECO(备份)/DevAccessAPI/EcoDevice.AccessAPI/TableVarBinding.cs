using System;
namespace EcoDevice.AccessAPI
{
	public class TableVarBinding : VarBinding
	{
		private string tableEntryOid;
		private string columnOid;
		private MaxRepetition maxRepetition = MaxRepetition.Ten;
		public MaxRepetition MaxRepetition
		{
			get
			{
				return this.maxRepetition;
			}
			set
			{
				this.maxRepetition = value;
			}
		}
		public string TableEntryOid
		{
			get
			{
				return this.tableEntryOid;
			}
		}
		public string ColumnOid
		{
			get
			{
				return this.columnOid;
			}
		}
		public TableVarBinding(string tableEntryOid, string columnOid)
		{
			this.tableEntryOid = tableEntryOid;
			this.columnOid = columnOid;
		}
		public TableVarBinding(string tableEntryOid) : this(tableEntryOid, null)
		{
		}
	}
}
