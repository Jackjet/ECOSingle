using System;
namespace EcoDevice.AccessAPI
{
	public class SnmpSessionFactory
	{
		private SnmpSessionFactory()
		{
		}
		public static SnmpSession CreateSession(SnmpConfig config)
		{
			if (config is SnmpV3Config)
			{
				return new SnmpV3Session((SnmpV3Config)config);
			}
			if (config is SnmpV2Config)
			{
				return new SnmpV2Session((SnmpV2Config)config);
			}
			return new SnmpV1Session((SnmpV1Config)config);
		}
	}
}
