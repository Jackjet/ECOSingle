using ecoProtocols;
using System;
using System.Collections.Generic;
namespace EcoMessenger
{
	public class BillingHandler : MessageBase
	{
		public BillingContext _receiver;
		public BillingHandler()
		{
			this._receiver = new BillingContext();
		}
		public override void OnClose(ConnectContext ctx)
		{
		}
		public override byte[] BuildFirstDispatch(DispatchAttribute attr)
		{
			return null;
		}
		public override byte[] BuildNextDispatch(DispatchAttribute attr)
		{
			return null;
		}
		protected override void ClientProcessing(ecoMessage msg)
		{
			try
			{
				BillingContext c = (BillingContext)msg._c;
				ulong arg_12_0 = msg._header;
				byte[] arg_1E_0 = (byte[])msg._attached;
				new List<byte[]>();
				BufferState bufferState = new BufferState();
				bufferState._buffer = (byte[])msg._attached;
				int num = 0;
				List<byte[]> list = this._receiver.ReceivedBuffer(bufferState, bufferState._buffer.Length, ref num);
				if (list != null)
				{
					foreach (byte[] current in list)
					{
						msg._from.AsyncSend(c, current);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
		}
	}
}
