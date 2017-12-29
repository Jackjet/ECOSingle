using System;
namespace EcoDevice.AccessAPI
{
	public enum SnmpOperationType
	{
		Get = 160,
		GetNext,
		Response,
		Set,
		Trap,
		GetBulk,
		Inform,
		V2Trap,
		Report,
		GetTable = 201,
		Walk
	}
}
