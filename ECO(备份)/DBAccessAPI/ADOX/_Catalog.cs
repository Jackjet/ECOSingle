using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ADOX
{
	[DefaultMember("Tables"), CompilerGenerated, Guid("00000603-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface _Catalog
	{
		Tables Tables
		{
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
		object ActiveConnection
		{
			[DispId(1)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
			[DispId(1)]
			[param: MarshalAs(UnmanagedType.IDispatch)]
			set;
		}
		void _VtblGap1_1();
		void _VtblGap2_4();
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Struct)]
		object Create([MarshalAs(UnmanagedType.BStr)] [In] string ConnectString);
	}
}
