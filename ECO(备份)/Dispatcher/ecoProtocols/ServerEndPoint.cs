using System;
using System.Net;
namespace ecoProtocols
{
	public class ServerEndPoint : IPEndPoint, IEquatable<ServerEndPoint>
	{
		public static ServerEndPoint NoneEndPoint = new ServerEndPoint(ecoServerProtocol.ServerTypes.TCP, IPAddress.None, 0);
		public ecoServerProtocol.ServerTypes Protocol
		{
			get;
			set;
		}
		public ecoServerProtocol.ProtocolAndPort ProtocolPort
		{
			get
			{
				return new ecoServerProtocol.ProtocolAndPort(this.Protocol, base.Port);
			}
		}
		public ServerEndPoint(ecoServerProtocol.ProtocolAndPort protocolPort, IPAddress address) : base(address, protocolPort.Port)
		{
			this.Protocol = protocolPort.Protocol;
		}
		public ServerEndPoint(ecoServerProtocol.ServerTypes protocol, IPAddress address, int port) : base(address, port)
		{
			this.Protocol = protocol;
		}
		public ServerEndPoint(ecoServerProtocol.ServerTypes protocol, IPEndPoint endpoint) : base(endpoint.Address, endpoint.Port)
		{
			this.Protocol = protocol;
		}
		public new bool Equals(object x)
		{
			return x is ServerEndPoint && this.Equals(x as ServerEndPoint);
		}
		public bool Equals(ServerEndPoint p)
		{
			return this.AddressFamily == p.AddressFamily && base.Port == p.Port && base.Address.Equals(p.Address) && this.Protocol == p.Protocol;
		}
		public bool Equals(ecoServerProtocol.ServerTypes protocol, IPEndPoint endpoint)
		{
			return this.AddressFamily == endpoint.AddressFamily && base.Port == endpoint.Port && base.Address.Equals(endpoint.Address) && this.Protocol == protocol;
		}
		public new string ToString()
		{
			return string.Format("{0}:{1}", this.Protocol.ToString(), base.ToString());
		}
		public ServerEndPoint Clone()
		{
			return new ServerEndPoint(this.Protocol, base.Address, base.Port);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ (int)this.Protocol;
		}
	}
}
