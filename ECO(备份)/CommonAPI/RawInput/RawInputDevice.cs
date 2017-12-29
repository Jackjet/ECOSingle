using System;
namespace RawInput
{
	internal struct RawInputDevice
	{
		internal HidUsagePage UsagePage;
		internal HidUsage Usage;
		internal RawInputDeviceFlags Flags;
		internal IntPtr Target;
		public override string ToString()
		{
			return string.Format("{0}/{1}, flags: {2}, target: {3}", new object[]
			{
				this.UsagePage,
				this.Usage,
				this.Flags,
				this.Target
			});
		}
	}
}
