using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ADOX
{
	[DefaultMember("Name"), CompilerGenerated, Guid("0000061C-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface _Column
	{
		string Name
		{
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(0)]
			[param: MarshalAs(UnmanagedType.BStr)]
			set;
		}
		DataTypeEnum Type
		{
			[DispId(8)]
			get;
			[DispId(8)]
			set;
		}
		Catalog ParentCatalog
		{
			[DispId(10)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
			[DispId(10)]
			[param: MarshalAs(UnmanagedType.Interface)]
			set;
		}
		void _VtblGap1_12();
		void _VtblGap2_1();
		void _VtblGap3_1();
	}
}
