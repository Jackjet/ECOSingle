using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class SnmpConfiger
	{
		private SnmpConfig snmpConfig;
		private DevModelConfig modelConfig;
		private DevRealConfig realConfig;
		private string deviceModel = "";
		private int switchable = 1;
		private int perportreading = 1;
		private int portnumber = 1;
		private int sensornumber = 2;
		private int banknumber;
		private int linenumber;
		private int perbankreading;
		private int perdoorreading;
		private string deviceMac = "";
		private int deviceID;
		private ThresholdMessage restoreThresholds;
		private System.Collections.Generic.List<int> groupOutlets = new System.Collections.Generic.List<int>();
		public SnmpConfig SnmpConfig
		{
			get
			{
				return this.snmpConfig;
			}
		}
		public DevModelConfig DevModelConfig
		{
			get
			{
				return this.modelConfig;
			}
		}
		public DevRealConfig DevRealConfig
		{
			get
			{
				return this.realConfig;
			}
		}
		public string DeviceMac
		{
			get
			{
				return this.deviceMac;
			}
		}
		public int DeviceID
		{
			get
			{
				return this.deviceID;
			}
		}
		public string DevModel
		{
			get
			{
				return this.deviceModel;
			}
		}
		public int PortNumbers
		{
			get
			{
				return this.portnumber;
			}
		}
		public int Switchable
		{
			get
			{
				return this.switchable;
			}
		}
		public int SensorNumber
		{
			get
			{
				return this.sensornumber;
			}
		}
		public int BankNumber
		{
			get
			{
				return this.banknumber;
			}
		}
		public int LineNumber
		{
			get
			{
				return this.linenumber;
			}
		}
		public int PerPortReading
		{
			get
			{
				return this.perportreading;
			}
		}
		public int PerBankReading
		{
			get
			{
				return this.perbankreading;
			}
		}
		public int PerDoorReading
		{
			get
			{
				return this.perdoorreading;
			}
		}
		public ThresholdMessage RestoreThresholds
		{
			get
			{
				return this.restoreThresholds;
			}
			set
			{
				this.restoreThresholds = value;
			}
		}
		public System.Collections.Generic.List<int> GroupOutlets
		{
			get
			{
				return this.groupOutlets;
			}
			set
			{
				this.groupOutlets = value;
			}
		}
		public SnmpConfiger(SnmpConfig snmpConfig, int portNums)
		{
			this.snmpConfig = snmpConfig;
			this.portnumber = portNums;
		}
		public SnmpConfiger(SnmpConfig snmpConfig) : this(snmpConfig, 1)
		{
		}
		public SnmpConfiger(SnmpConfig snmpConfig, DevModelConfig modelCfg)
		{
			this.snmpConfig = snmpConfig;
			this.modelConfig = modelCfg;
			this.portnumber = modelCfg.portNum;
			this.deviceModel = modelCfg.modelName;
			this.switchable = modelCfg.switchable;
			this.sensornumber = modelCfg.sensorNum;
			this.banknumber = modelCfg.bankNum;
			this.linenumber = modelCfg.lineNum;
			this.perportreading = modelCfg.perportreading;
			this.perbankreading = modelCfg.perbankReading;
			this.perdoorreading = modelCfg.doorReading;
		}
		public SnmpConfiger(SnmpConfig snmpConfig, DevModelConfig modelCfg, string devMac, int devID)
		{
			this.snmpConfig = snmpConfig;
			this.modelConfig = modelCfg;
			this.portnumber = modelCfg.portNum;
			this.deviceModel = modelCfg.modelName;
			this.switchable = modelCfg.switchable;
			this.sensornumber = modelCfg.sensorNum;
			this.banknumber = modelCfg.bankNum;
			this.linenumber = modelCfg.lineNum;
			this.perportreading = modelCfg.perportreading;
			this.perbankreading = modelCfg.perbankReading;
			this.perdoorreading = modelCfg.doorReading;
			this.deviceMac = devMac;
			this.deviceID = devID;
		}
		public SnmpConfiger(SnmpConfig snmpConfig, DevModelConfig modelCfg, string devMac, int devID, DevRealConfig realCfg)
		{
			this.snmpConfig = snmpConfig;
			this.modelConfig = modelCfg;
			this.deviceModel = modelCfg.modelName;
			this.switchable = modelCfg.switchable;
			this.realConfig = realCfg;
			this.portnumber = realCfg.portNum;
			this.banknumber = realCfg.bankNum;
			this.linenumber = realCfg.lineNum;
			this.sensornumber = realCfg.sensorNum;
			this.perdoorreading = realCfg.contactNum;
			this.perportreading = modelCfg.perportreading;
			this.perbankreading = modelCfg.perbankReading;
			this.deviceMac = devMac;
			this.deviceID = devID;
		}
	}
}
