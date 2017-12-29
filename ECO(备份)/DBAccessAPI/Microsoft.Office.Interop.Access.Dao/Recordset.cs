using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Microsoft.Office.Interop.Access.Dao
{
	[DefaultMember("Fields"), CompilerGenerated, Guid("00000031-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface Recordset : _DAO
	{
		Fields Fields
		{
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
		void _VtblGap1_38();
		void _VtblGap2_2();
		[DispId(132)]
		void AddNew();
		[DispId(133)]
		void Close();
		void _VtblGap3_39();
		[DispId(168)]
		void Update([In] int UpdateType = 1, [In] bool Force = false);
	}
}
