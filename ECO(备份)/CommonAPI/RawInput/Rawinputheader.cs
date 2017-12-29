using System;
namespace RawInput
{
	public struct Rawinputheader
	{
		public uint dwType;
		public uint dwSize;
		public IntPtr hDevice;
		public IntPtr wParam;
		public override string ToString()
		{
			return string.Format("RawInputHeader\n dwType : {0}\n dwSize : {1}\n hDevice : {2}\n wParam : {3}", new object[]
			{
				this.dwType,
				this.dwSize,
				this.hDevice,
				this.wParam
			});
		}
	}
}
