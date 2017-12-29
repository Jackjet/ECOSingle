using System;
using System.Net.Sockets;
namespace ecoProtocols
{
	public interface IConnectInterface
	{
		void AsyncSend(object c, byte[] data);
		void ReportMessage(IConnectInterface from, object c, ulong header, byte[] message);
		bool IsValidToken(string token);
		void setLoginState(int status);
		int getLoginState();
		void UpdateUID(int uid, int vid);
		void UpdateVID(int vid);
		void SendUrgency(Socket sock, int uid, byte[] op);
		void RequestDataset(int nDataType);
		void DispatchDataset(DispatchAttribute attr);
	}
}
