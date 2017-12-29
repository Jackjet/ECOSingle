using System;
using System.Runtime.InteropServices;
namespace RawInput
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct Rawmouse
	{
		[FieldOffset(0)]
		public ushort usFlags;
		[FieldOffset(4)]
		public uint ulButtons;
		[FieldOffset(4)]
		public ushort usButtonFlags;
		[FieldOffset(6)]
		public ushort usButtonData;
		[FieldOffset(8)]
		public uint ulRawButtons;
		[FieldOffset(12)]
		public int lLastX;
		[FieldOffset(16)]
		public int lLastY;
		[FieldOffset(20)]
		public uint ulExtraInformation;
	}
}
