using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class SnmpV1Session : AbstractSnmpSession
	{
		public SnmpV1Session(SnmpV1Config config) : base(config)
		{
		}
		public override System.Collections.Generic.Dictionary<string, string> GetBulk(string startVariable)
		{
			throw new System.NotSupportedException("Snmpv1 does not support GetBulk opertion.");
		}
	}
}
