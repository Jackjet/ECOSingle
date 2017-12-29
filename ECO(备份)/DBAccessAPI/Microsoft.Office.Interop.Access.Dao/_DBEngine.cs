using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Microsoft.Office.Interop.Access.Dao
{
	[DefaultMember("Workspaces"), CompilerGenerated, Guid("00000021-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface _DBEngine : _DAO
	{
		void _VtblGap1_15();
		[DispId(1610809358)]
		[return: MarshalAs(UnmanagedType.Interface)]
		Database OpenDatabase([MarshalAs(UnmanagedType.BStr)] [In] string Name, [MarshalAs(UnmanagedType.Struct)] [In] object Options = null, [MarshalAs(UnmanagedType.Struct)] [In] object ReadOnly = null, [MarshalAs(UnmanagedType.Struct)] [In] object Connect = null);
	}
}
