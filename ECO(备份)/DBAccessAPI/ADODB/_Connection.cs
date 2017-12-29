using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ADODB
{
	[DefaultMember("ConnectionString"), CompilerGenerated, Guid("00000550-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface _Connection : Connection15, _ADO
	{
		void _VtblGap1_8();
		[DispId(5)]
		void Close();
		void _VtblGap2_4();
		[DispId(10)]
		void Open([MarshalAs(UnmanagedType.BStr)] [In] string ConnectionString = "", [MarshalAs(UnmanagedType.BStr)] [In] string UserID = "", [MarshalAs(UnmanagedType.BStr)] [In] string Password = "", [In] int Options = -1);
	}
}
