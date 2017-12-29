using System;
namespace ecoProtocols
{
	public delegate void ServerEventHandlerByVal<S, T>(S s, T e);
	public delegate void ServerEventHandlerByVal<S, T1, T2>(S s, T1 t1, T2 t2);
}
