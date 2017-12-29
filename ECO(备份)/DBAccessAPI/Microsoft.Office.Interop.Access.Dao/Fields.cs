using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Microsoft.Office.Interop.Access.Dao
{
	[CompilerGenerated, Guid("00000053-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface Fields : _DynaCollection, _Collection, IEnumerable
	{
		Field this[[MarshalAs(UnmanagedType.Struct)] [In] object Item]
		{
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
		void _VtblGap1_5();
	}
}
