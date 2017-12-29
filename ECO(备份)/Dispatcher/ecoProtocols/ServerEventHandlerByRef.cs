using System;
namespace ecoProtocols
{
	public delegate void ServerEventHandlerByRef<S, T>(S s, ref T e);
	public delegate Return ServerEventHandlerByRef<S, T1, T2, Return>(S s, T1 t1, ref T2 t2);
}
