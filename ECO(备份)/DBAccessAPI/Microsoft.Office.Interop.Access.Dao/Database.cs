using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Microsoft.Office.Interop.Access.Dao
{
	[DefaultMember("TableDefs"), CompilerGenerated, Guid("00000071-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface Database : _DAO
	{
		void _VtblGap1_15();
		[DispId(1610809358)]
		void Close();
		[DispId(1610809359)]
		void Execute([MarshalAs(UnmanagedType.BStr)] [In] string Query, [MarshalAs(UnmanagedType.Struct)] [In] object Options = null);
		void _VtblGap2_23();
		[DispId(1610809383)]
		[return: MarshalAs(UnmanagedType.Interface)]
		Recordset OpenRecordset([MarshalAs(UnmanagedType.BStr)] [In] string Name, [MarshalAs(UnmanagedType.Struct)] [In] object Type = null, [MarshalAs(UnmanagedType.Struct)] [In] object Options = null, [MarshalAs(UnmanagedType.Struct)] [In] object LockEdit = null);
	}
}
