using System;
namespace EcoDevice.AccessAPI
{
	[Encode(false)]
	public sealed class SocketData : AbstractSocketData
	{
		public override string getAssamble()
		{
			return "Com.Aten.Util";
		}
	}
}
