using System;
namespace MySql.Data.MySqlClient.Authentication
{
	public struct SECURITY_HANDLE
	{
		public IntPtr LowPart;
		public IntPtr HighPart;
		public SECURITY_HANDLE(int dummy)
		{
			this.LowPart = (this.HighPart = new IntPtr(0));
		}
	}
}
