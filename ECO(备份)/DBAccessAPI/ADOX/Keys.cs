using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ADOX
{
	[DefaultMember("Item"), CompilerGenerated, Guid("00000623-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface Keys : _Collection, IEnumerable
	{
		void _VtblGap1_4();
		[DispId(1610809345)]
		void Append([MarshalAs(UnmanagedType.Struct)] [In] object Item, [In] KeyTypeEnum Type = KeyTypeEnum.adKeyPrimary, [MarshalAs(UnmanagedType.Struct)] [In] object Column = null, [MarshalAs(UnmanagedType.BStr)] [In] string RelatedTable = "", [MarshalAs(UnmanagedType.BStr)] [In] string RelatedColumn = "");
	}
}
