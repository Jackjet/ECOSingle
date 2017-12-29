using System;
namespace RawInput
{
	public class RawInputEvent
	{
		public InputData RawInputData;
		public ulong mouseMessage;
		public string DeviceName = string.Empty;
		public string DeviceType = string.Empty;
		public IntPtr DeviceHandle = IntPtr.Zero;
		public string Name = string.Empty;
		private string _source = string.Empty;
		public int VKey;
		public string VKeyName = string.Empty;
		public uint Message;
		public string KeyPressState = string.Empty;
		public bool Filter;
		public string Source
		{
			get
			{
				return this._source;
			}
			set
			{
				this._source = string.Format("Keyboard_{0}", value.PadLeft(2, '0'));
			}
		}
		public override string ToString()
		{
			return string.Format("Device\n DeviceName: {0}\n DeviceType: {1}\n DeviceHandle: {2}\n Name: {3}\n", new object[]
			{
				this.DeviceName,
				this.DeviceType,
				this.DeviceHandle.ToInt64().ToString("X"),
				this.Name
			});
		}
	}
}
