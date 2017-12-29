using System;
namespace RawInput
{
	public struct DeviceInfoMouse
	{
		public uint Id;
		public uint NumberOfButtons;
		public uint SampleRate;
		public bool HasHorizontalWheel;
		public override string ToString()
		{
			return string.Format("MouseInfo\n Id: {0}\n NumberOfButtons: {1}\n SampleRate: {2}\n HorizontalWheel: {3}\n", new object[]
			{
				this.Id,
				this.NumberOfButtons,
				this.SampleRate,
				this.HasHorizontalWheel
			});
		}
	}
}
