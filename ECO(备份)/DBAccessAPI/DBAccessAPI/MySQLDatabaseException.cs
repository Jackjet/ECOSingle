using System;
namespace DBAccessAPI
{
	public class MySQLDatabaseException : Exception
	{
		public MySQLDatabaseException() : this("Database access error.")
		{
		}
		public MySQLDatabaseException(string reason) : base(reason)
		{
		}
		public MySQLDatabaseException(Exception e) : base(e.Message, e)
		{
		}
	}
}
