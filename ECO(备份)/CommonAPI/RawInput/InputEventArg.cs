using System;
namespace RawInput
{
	public class InputEventArg : EventArgs
	{
		public RawInputEvent RawInputEvent
		{
			get;
			private set;
		}
		public InputEventArg(RawInputEvent arg)
		{
			this.RawInputEvent = arg;
		}
		private InputEventArg()
		{
		}
	}
}
