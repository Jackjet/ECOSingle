using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace ADOX
{
	[DefaultMember("Item"), CompilerGenerated, Guid("00000611-0000-0010-8000-00AA006D2EA4"), TypeIdentifier]
	[ComImport]
	public interface Tables : _Collection, IEnumerable
	{
		void _VtblGap1_4();
		[DispId(1610809345)]
		void Append([MarshalAs(UnmanagedType.Struct)] [In] object Item);
	}
}
