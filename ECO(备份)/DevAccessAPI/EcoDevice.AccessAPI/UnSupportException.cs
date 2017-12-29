using System;
namespace EcoDevice.AccessAPI
{
	public class UnSupportException : System.Exception
	{
		public UnSupportException(string msg) : base(msg)
		{
		}
	}
}
