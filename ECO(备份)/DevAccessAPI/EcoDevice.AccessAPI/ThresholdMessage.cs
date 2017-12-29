using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class ThresholdMessage : Msg
	{
		private DeviceThreshold deviceEnvironment;
		private System.Collections.Generic.Dictionary<int, SensorThreshold> sensorEnvironment;
		private System.Collections.Generic.Dictionary<int, OutletThreshold> outletEnvironment;
		private System.Collections.Generic.Dictionary<int, BankThreshold> bankEnvironment;
		private System.Collections.Generic.Dictionary<int, LineThreshold> lineEnvironment;
		public DeviceThreshold DeviceThreshold
		{
			get
			{
				return this.deviceEnvironment;
			}
			set
			{
				this.deviceEnvironment = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, SensorThreshold> SensorThreshold
		{
			get
			{
				return this.sensorEnvironment;
			}
			set
			{
				this.sensorEnvironment = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, OutletThreshold> OutletThreshold
		{
			get
			{
				return this.outletEnvironment;
			}
			set
			{
				this.outletEnvironment = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, BankThreshold> BankThreshold
		{
			get
			{
				return this.bankEnvironment;
			}
			set
			{
				this.bankEnvironment = value;
			}
		}
		public System.Collections.Generic.Dictionary<int, LineThreshold> LineThreshold
		{
			get
			{
				return this.lineEnvironment;
			}
			set
			{
				this.lineEnvironment = value;
			}
		}
	}
}
