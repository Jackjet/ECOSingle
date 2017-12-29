using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ADOX
{
	[DefaultMember("Columns"), CompilerGenerated, Guid("00000610-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface _Table
	{
		Columns Columns
		{
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
		string Name
		{
			[DispId(1)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(1)]
			[param: MarshalAs(UnmanagedType.BStr)]
			set;
		}
		Keys Keys
		{
			[DispId(4)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
		Catalog ParentCatalog
		{
			[DispId(8)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
			[DispId(8)]
			[param: MarshalAs(UnmanagedType.Interface)]
			set;
		}
		void _VtblGap1_2();
		void _VtblGap2_3();
		void _VtblGap3_1();
	}
}
