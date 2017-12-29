using ecoProtocols;
using System;
namespace EcoMessenger
{
	public class ecoMessage
	{
		public enum MessageTypes
		{
			MSG_FROM_DATA_CENTER = 1,
			MSG_FROM_SERVER,
			MSG_FROM_CLIENT,
			MSG_BROADCAST,
			MSG_TIMER,
			MSG_PACKET_PAYLOAD,
			MSG_SOCKET_RAW
		}
		public IConnectInterface _from;
		public object _c;
		public int _type;
		public ulong _header;
		public object _attached;
		public ecoMessage()
		{
			this._c = null;
			this._from = null;
			this._type = 0;
			this._header = 0uL;
			this._attached = null;
		}
		public ecoMessage(IConnectInterface from, object c, int type, ulong header, object received)
		{
			this._from = from;
			this._c = c;
			this._type = type;
			this._header = header;
			this._attached = received;
		}
	}
}
