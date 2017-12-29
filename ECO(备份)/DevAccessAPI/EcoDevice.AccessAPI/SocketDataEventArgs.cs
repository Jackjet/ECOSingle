using System;
namespace EcoDevice.AccessAPI
{
	public class SocketDataEventArgs : System.EventArgs
	{
		private AbstractSocketData socketData;
		public AbstractSocketData SocketData
		{
			get
			{
				return this.socketData;
			}
		}
		public SocketDataEventArgs(AbstractSocketData socketData)
		{
			this.socketData = socketData;
		}
	}
}
