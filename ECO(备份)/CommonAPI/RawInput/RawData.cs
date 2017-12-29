using System;
using System.Runtime.InteropServices;
namespace RawInput
{
	[StructLayout(LayoutKind.Explicit)]
	public struct RawData
	{
		[FieldOffset(0)]
		internal Rawmouse mouse;
		[FieldOffset(0)]
		internal Rawkeyboard keyboard;
		[FieldOffset(0)]
		internal Rawhid hid;
	}
}
