using System;
namespace RawInput
{
	public struct DeviceInfoKeyboard
	{
		public uint Type;
		public uint SubType;
		public uint KeyboardMode;
		public uint NumberOfFunctionKeys;
		public uint NumberOfIndicators;
		public uint NumberOfKeysTotal;
		public override string ToString()
		{
			return string.Format("DeviceInfoKeyboard\n Type: {0}\n SubType: {1}\n KeyboardMode: {2}\n NumberOfFunctionKeys: {3}\n NumberOfIndicators {4}\n NumberOfKeysTotal: {5}\n", new object[]
			{
				this.Type,
				this.SubType,
				this.KeyboardMode,
				this.NumberOfFunctionKeys,
				this.NumberOfIndicators,
				this.NumberOfKeysTotal
			});
		}
	}
}
