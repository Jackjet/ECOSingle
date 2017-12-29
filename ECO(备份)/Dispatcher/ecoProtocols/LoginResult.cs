using System;
namespace ecoProtocols
{
	public enum LoginResult : long
	{
		Success = 1L,
		Pending = 0L,
		Timeout = -1L,
		AuthFailed = -2L,
		VersionNotMatch = -3L,
		DBNotReady = -4L,
		WrongSerialNo = -5L,
		SerialNoInUse = -6L,
		SessionCountLimited = -7L,
		SerialNoExpired = -8L
	}
}
