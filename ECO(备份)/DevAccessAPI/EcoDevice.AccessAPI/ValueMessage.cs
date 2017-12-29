using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class ValueMessage : Msg
	{
		private DeviceValueEntry deviceValue;
		private System.Collections.Generic.Dictionary<int, SensorValueEntry> sensorValue = new System.Collections.Generic.Dictionary<int, SensorValueEntry>();
		private System.Collections.Generic.Dictionary<int, OutletValueEntry> outletValue = new System.Collections.Generic.Dictionary<int, OutletValueEntry>();
		private System.Collections.Generic.Dictionary<int, BankValueEntry> bankValue = new System.Collections.Generic.Dictionary<int, BankValueEntry>();
		private System.Collections.Generic.Dictionary<int, LineValueEntry> lineValue = new System.Collections.Generic.Dictionary<int, LineValueEntry>();
		public DeviceValueEntry DeviceValue
		{
			get
			{
				return this.deviceValue;
			}
			set
			{
				this.deviceValue = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, SensorValueEntry> SensorValue
		{
			get
			{
				return this.sensorValue;
			}
			set
			{
				this.sensorValue = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, OutletValueEntry> OutletValue
		{
			get
			{
				return this.outletValue;
			}
			set
			{
				this.outletValue = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, BankValueEntry> BankValue
		{
			get
			{
				return this.bankValue;
			}
			set
			{
				this.bankValue = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, LineValueEntry> LineValue
		{
			get
			{
				return this.lineValue;
			}
			set
			{
				this.lineValue = value;
			}
		}
	}
}
