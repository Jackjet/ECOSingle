using System;
namespace RawInput
{
	public struct BroadcastDeviceInterface
	{
		public int dbcc_size;
		public BroadcastDeviceType BroadcastDeviceType;
		private int dbcc_reserved;
		public Guid dbcc_classguid;
		public char dbcc_name;
	}
}
