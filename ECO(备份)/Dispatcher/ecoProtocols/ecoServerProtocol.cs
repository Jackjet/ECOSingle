using System;
namespace ecoProtocols
{
	public static class ecoServerProtocol
	{
		public enum CompressAlgorithms
		{
			NONE,
			LZ4,
			GZIP
		}
		public enum MessageProcessorTypes
		{
			MSG_SERVER,
			MSG_CLIENT
		}
		public enum ServerTypes
		{
			UDP,
			TCP,
			TLS,
			HTTP,
			HTTPS
		}
		public static class ProtocolConstant
		{
			public const int FSM_IDLE = 0;
			public const int FSM_HEAD = 1;
			public const int HEADER_SIZE = 8;
			public const int BUFFER_SIZE = 4096;
		}
		public struct ProtocolAndPort
		{
			public int Port;
			public ecoServerProtocol.ServerTypes Protocol;
			public ProtocolAndPort(ecoServerProtocol.ServerTypes protocol, int port)
			{
				this.Protocol = protocol;
				this.Port = port;
			}
		}
		public enum CommandTypes
		{
			PACKET_DCENTER = 1,
			PACKET_CLIENTS = 10
		}
		public enum BlockTypes
		{
			LAST_BLOCK = 2,
			FIRST_BLOCK = 1
		}
		public enum PacketTypes
		{
			PACKET_MSG_FOR_SERVICE = 1,
			PACKET_MSG_FOR_SERVICE_ACK = 129,
			PACKET_DC_KICK = 32,
			PACKET_DC_KICK_ACK = 160,
			PACKET_DC_SERVICE_DOWN = 48,
			PACKET_DC_SERVICE_DOWN_ACK = 176,
			PACKET_DC_KEEP_ALIVE = 256,
			PACKET_DC_KEEP_ALIVE_ACK = 384,
			PACKET_DC_CONNECT = 257,
			PACKET_DC_CONNECT_ACK = 385,
			PACKET_DC_DATASET = 258,
			PACKET_DC_DATASET_ACK = 386,
			PACKET_DC_WEB_REQUEST = 259,
			PACKET_DC_WEB_REQUEST_ACK = 387,
			PACKET_DC_BROADCAST = 260,
			PACKET_CA_KEEP_ALIVE = 35328,
			PACKET_CA_KEEP_ALIVE_ACK = 35456,
			PACKET_CA_LOGIN = 35329,
			PACKET_CA_LOGIN_ACK = 35457,
			PACKET_CA_LOGOUT = 35330,
			PACKET_CA_LOGOUT_ACK = 35458,
			PACKET_CA_REQUEST = 35331,
			PACKET_CA_REQUEST_ACK = 35459,
			PACKET_CA_MESSAGE = 35332,
			PACKET_CA_MESSAGE_ACK = 35460,
			PACKET_CA_ERROR = 35333,
			PACKET_CA_ERROR_ACK = 35461,
			PACKET_CA_AUTH = 35343,
			PACKET_CA_AUTH_ACK = 35471
		}
		public enum HeaderIndex
		{
			INDEX_PREFIX = 32,
			INDEX_TYPE = 0,
			INDEX_LEN = 16
		}
		public enum HeaderBits
		{
			BITS_PREFIX = 32,
			BITS_TYPE = 16,
			BITS_LEN = 16
		}
		public const ulong _packetPrefix = 455389875uL;
		public const uint _maskExternal = 128u;
		public static ushort swap16(ushort value)
		{
			return (ushort)(((int)(value & 255) << 8) + (value >> 8));
		}
		public static uint swap32(uint value)
		{
			return ((value & 255u) << 24) + ((value >> 8 & 255u) << 16) + ((value >> 16 & 255u) << 8) + (value >> 24 & 255u);
		}
		public static ulong swap64(ulong value)
		{
			return ((value & 255uL) << 56) + ((value >> 8 & 255uL) << 48) + ((value >> 16 & 255uL) << 40) + ((value >> 24 & 255uL) << 32) + ((value >> 32 & 255uL) << 24) + ((value >> 40 & 255uL) << 16) + ((value >> 48 & 255uL) << 8) + (value >> 56 & 255uL);
		}
		public static void setPacketPrefix(ref ulong header)
		{
			ulong num = 455389875uL;
			header |= (num & (ulong)-1) << 32;
		}
		public static void setPacketType(ref ulong header, ulong value)
		{
			header |= (value & 65535uL);
		}
		public static void setPacketLen(ref ulong header, ulong value)
		{
			header |= (value & 65535uL) << 16;
		}
		public static uint getPacketPrefix(ulong header)
		{
			ulong num = header >> 32 & (ulong)-1;
			return (uint)num;
		}
		public static uint getPacketType(ulong header)
		{
			ulong num = header & 65535uL;
			ulong num2 = num;
			return (uint)num2;
		}
		public static uint getPacketLength(ulong header)
		{
			ulong num = header >> 16 & 65535uL;
			ulong num2 = num;
			return (uint)num2;
		}
	}
}
