using System;
namespace RawInput
{
	public struct DeviceInfoHID
	{
		public uint VendorID;
		public uint ProductID;
		public uint VersionNumber;
		public ushort UsagePage;
		public ushort Usage;
		public override string ToString()
		{
			return string.Format("HidInfo\n VendorID: {0}\n ProductID: {1}\n VersionNumber: {2}\n UsagePage: {3}\n Usage: {4}\n", new object[]
			{
				this.VendorID,
				this.ProductID,
				this.VersionNumber,
				this.UsagePage,
				this.Usage
			});
		}
	}
}
