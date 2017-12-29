using System;
namespace EcoDevice.AccessAPI
{
	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Field, AllowMultiple = false)]
	public class Encode : System.Attribute
	{
		private bool needEncoded;
		public bool NeedEncoded
		{
			get
			{
				return this.needEncoded;
			}
		}
		public Encode() : this(true)
		{
		}
		public Encode(bool needEncoded)
		{
			this.needEncoded = needEncoded;
		}
	}
}
