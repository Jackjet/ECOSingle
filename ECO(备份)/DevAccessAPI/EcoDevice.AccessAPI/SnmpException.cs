using System;
namespace EcoDevice.AccessAPI
{
	public class SnmpException : System.Exception
	{
		public SnmpException()
		{
		}
		public SnmpException(string message) : base(message)
		{
		}
		public SnmpException(string message, System.Exception e) : base(message, e)
		{
		}
	}
}
