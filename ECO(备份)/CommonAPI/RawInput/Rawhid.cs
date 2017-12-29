using System;
namespace RawInput
{
	internal struct Rawhid
	{
		public uint dwSizHid;
		public uint dwCount;
		public byte bRawData;
		public override string ToString()
		{
			return string.Format("Rawhib\n dwSizeHid : {0}\n dwCount : {1}\n bRawData : {2}\n", this.dwSizHid, this.dwCount, this.bRawData);
		}
	}
}
