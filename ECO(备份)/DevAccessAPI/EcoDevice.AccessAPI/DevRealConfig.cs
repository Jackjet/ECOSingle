using System;
namespace EcoDevice.AccessAPI
{
	public struct DevRealConfig
	{
		public int inputNum;
		public int portNum;
		public int bankNum;
		public int lineNum;
		public int sensorNum;
		public int temperatureNum;
		public int humidityNum;
		public int contactNum;
		public int switchable;
		public int perportreading;
		public int perbankReading;
		public int perdoorReading;
		public void init()
		{
			this.inputNum = 0;
			this.portNum = 0;
			this.bankNum = 0;
			this.lineNum = 0;
			this.sensorNum = 0;
			this.temperatureNum = 0;
			this.humidityNum = 0;
			this.contactNum = 0;
			this.switchable = 0;
			this.perportreading = 0;
			this.perbankReading = 0;
			this.perdoorReading = 0;
		}
	}
}
