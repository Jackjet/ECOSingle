using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Microsoft.Office.Interop.Access.Dao
{
	[DefaultMember("Value"), CompilerGenerated, Guid("00000051-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface _Field : _DAO
	{
		object Value
		{
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
			[DispId(0)]
			[param: MarshalAs(UnmanagedType.Struct)]
			set;
		}
		void _VtblGap1_10();
	}
}
