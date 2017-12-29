using System;
using System.Runtime.InteropServices;
namespace RawInput
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DeviceInfo
	{
		[FieldOffset(0)]
		public int Size;
		[FieldOffset(4)]
		public int Type;
		[FieldOffset(8)]
		public DeviceInfoMouse MouseInfo;
		[FieldOffset(8)]
		public DeviceInfoKeyboard KeyboardInfo;
		[FieldOffset(8)]
		public DeviceInfoHID HIDInfo;
		public override string ToString()
		{
			return string.Format("DeviceInfo\n Size: {0}\n Type: {1}\n", this.Size, this.Type);
		}
	}
}
