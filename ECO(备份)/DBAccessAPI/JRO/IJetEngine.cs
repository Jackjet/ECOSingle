using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace JRO
{
	[CompilerGenerated, Guid("9F63D980-FF25-11D1-BB6F-00C04FAE22DA"), TypeIdentifier]
	[ComImport]
	public interface IJetEngine
	{
		[DispId(1610743808)]
		void CompactDatabase([MarshalAs(UnmanagedType.BStr)] [In] string SourceConnection, [MarshalAs(UnmanagedType.BStr)] [In] string Destconnection);
	}
}
