using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ADOX
{
	[DefaultMember("Item"), CompilerGenerated, Guid("0000061D-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface Columns : _Collection, IEnumerable
	{
		void _VtblGap1_4();
		[DispId(1610809345)]
		void Append([MarshalAs(UnmanagedType.Struct)] [In] object Item, [In] DataTypeEnum Type = DataTypeEnum.adVarWChar, [In] int DefinedSize = 0);
	}
}
