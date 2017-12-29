using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public struct DevModelConfig
	{
		public const int PopReadingNo = 1;
		public const int PopReadingYes = 2;
		public string firmwareVer;
		public string autoBasicInfo;
		public string autoRatingInfo;
		public string modelName;
		public int portNum;
		public int sensorNum;
		public int bankNum;
		public int lineNum;
		public uint bankCtrlflg;
		public uint bankOpt_nameempty;
		public int switchable;
		public int perportreading;
		public int perbankReading;
		public int perlineReading;
		public int leakCurrent;
		public int popNewRule;
		public int popPrioritySupport;
		public int popReading;
		public int popDefault;
		public int popUdefmax;
		public int doorReading;
		public string devcapacity;
		public uint deviceMeasureOpt;
		public uint bankMeasureOpt;
		public uint outletMeasureOpt;
		public uint lineMeasureOpt;
		public uint maxDevThresholdOpt;
		public uint minDevThresholdOpt;
		public uint maxBankThresholdOpt;
		public uint minBankThresholdOpt;
		public uint maxPortThresholdOpt;
		public uint minPortThresholdOpt;
		public uint maxLineThresholdOpt;
		public uint minLineThresholdOpt;
		public int bankStatusSupport;
		public int outletStatusSupport;
		public System.Collections.Generic.List<stru_CommRange> ampcapicity;
		public int commonThresholdFlag;
		public float energyboxVoltage;
		public ulong switchableOutlets;
		public ulong killPowerDisableRebootOutlets;
		public ulong perportreadingOutlets;
		public DevThreshold devThresholds;
		public System.Collections.Generic.List<BankOutlets> bankOutlets;
		private FW_stru m_FW;
		public static ulong[] OutletBitmap = new ulong[]
		{
			1uL,
			2uL,
			4uL,
			8uL,
			16uL,
			32uL,
			64uL,
			128uL,
			256uL,
			512uL,
			1024uL,
			2048uL,
			4096uL,
			8192uL,
			16384uL,
			32768uL,
			65536uL,
			131072uL,
			262144uL,
			524288uL,
			1048576uL,
			2097152uL,
			4194304uL,
			8388608uL,
			16777216uL,
			33554432uL,
			67108864uL,
			134217728uL,
			268435456uL,
			536870912uL,
			1073741824uL,
			2147483648uL,
			4294967296uL,
			8589934592uL,
			17179869184uL,
			34359738368uL,
			68719476736uL,
			137438953472uL,
			274877906944uL,
			549755813888uL,
			1099511627776uL,
			2199023255552uL,
			4398046511104uL,
			8796093022208uL,
			17592186044416uL,
			35184372088832uL,
			70368744177664uL,
			140737488355328uL,
			281474976710656uL,
			562949953421312uL,
			1125899906842624uL,
			2251799813685248uL,
			4503599627370496uL,
			9007199254740992uL,
			18014398509481984uL,
			36028797018963968uL,
			72057594037927936uL,
			144115188075855872uL,
			288230376151711744uL,
			576460752303423488uL,
			1152921504606846976uL,
			2305843009213693952uL,
			4611686018427387904uL,
			9223372036854775808uL
		};
		public int FWvalidate
		{
			get
			{
				return this.m_FW.FWvalidate;
			}
			set
			{
				this.m_FW.FWvalidate = value;
			}
		}
		public string FWnms
		{
			get
			{
				return this.m_FW.FWnms;
			}
			set
			{
				this.m_FW.FWnms = value;
			}
		}
		public string FWext
		{
			get
			{
				return this.m_FW.FWext;
			}
			set
			{
				this.m_FW.FWext = value;
			}
		}
		public static int GetSensorCount(string str_model, string str_fwversion)
		{
			try
			{
				return DevAccessCfg.GetInstance().getDeviceModelConfig(str_model, str_fwversion).sensorNum;
			}
			catch
			{
			}
			return -1;
		}
		public DevModelConfig(string mdName)
		{
			this.firmwareVer = "n/a";
			this.autoBasicInfo = string.Empty;
			this.autoRatingInfo = string.Empty;
			this.modelName = mdName;
			this.portNum = 0;
			this.sensorNum = 0;
			this.bankNum = 0;
			this.lineNum = 0;
			this.bankCtrlflg = 0u;
			this.bankOpt_nameempty = 0u;
			this.switchable = 1;
			this.perportreading = 1;
			this.perbankReading = 1;
			this.perlineReading = 1;
			this.leakCurrent = 1;
			this.popNewRule = 1;
			this.popPrioritySupport = 1;
			this.popReading = 1;
			this.popDefault = 32;
			this.popUdefmax = 32;
			this.doorReading = 1;
			this.devcapacity = "0";
			this.commonThresholdFlag = 0;
			this.energyboxVoltage = 0f;
			this.switchableOutlets = 0uL;
			this.killPowerDisableRebootOutlets = 0uL;
			this.perportreadingOutlets = 0uL;
			this.deviceMeasureOpt = 0u;
			this.bankMeasureOpt = 0u;
			this.outletMeasureOpt = 0u;
			this.lineMeasureOpt = 0u;
			this.maxDevThresholdOpt = 0u;
			this.minDevThresholdOpt = 0u;
			this.maxBankThresholdOpt = 0u;
			this.minBankThresholdOpt = 0u;
			this.maxPortThresholdOpt = 0u;
			this.minPortThresholdOpt = 0u;
			this.maxLineThresholdOpt = 0u;
			this.minLineThresholdOpt = 0u;
			this.bankStatusSupport = 1;
			this.outletStatusSupport = 1;
			this.ampcapicity = new System.Collections.Generic.List<stru_CommRange>();
			this.devThresholds = default(DevThreshold);
			this.bankOutlets = new System.Collections.Generic.List<BankOutlets>();
			this.m_FW = default(FW_stru);
			this.m_FW.init();
		}
		public void copy(DevModelConfig src)
		{
			this.firmwareVer = src.firmwareVer;
			this.autoBasicInfo = src.autoBasicInfo;
			this.autoRatingInfo = src.autoRatingInfo;
			this.modelName = src.modelName;
			this.portNum = src.portNum;
			this.sensorNum = src.sensorNum;
			this.bankNum = src.bankNum;
			this.lineNum = src.lineNum;
			this.bankCtrlflg = src.bankCtrlflg;
			this.bankOpt_nameempty = src.bankOpt_nameempty;
			this.switchable = src.switchable;
			this.perportreading = src.perportreading;
			this.perbankReading = src.perbankReading;
			this.perlineReading = src.perlineReading;
			this.leakCurrent = src.leakCurrent;
			this.popNewRule = src.popNewRule;
			this.popPrioritySupport = src.popPrioritySupport;
			this.popReading = src.popReading;
			this.popDefault = src.popDefault;
			this.popUdefmax = src.popUdefmax;
			this.doorReading = src.doorReading;
			this.devcapacity = src.devcapacity;
			this.deviceMeasureOpt = src.deviceMeasureOpt;
			this.bankMeasureOpt = src.bankMeasureOpt;
			this.outletMeasureOpt = src.outletMeasureOpt;
			this.lineMeasureOpt = src.lineMeasureOpt;
			this.maxDevThresholdOpt = src.maxDevThresholdOpt;
			this.minDevThresholdOpt = src.minDevThresholdOpt;
			this.maxBankThresholdOpt = src.maxBankThresholdOpt;
			this.minBankThresholdOpt = src.minBankThresholdOpt;
			this.maxPortThresholdOpt = src.maxPortThresholdOpt;
			this.minPortThresholdOpt = src.minPortThresholdOpt;
			this.maxLineThresholdOpt = src.maxLineThresholdOpt;
			this.minLineThresholdOpt = src.minLineThresholdOpt;
			this.bankStatusSupport = src.bankStatusSupport;
			this.outletStatusSupport = src.outletStatusSupport;
			this.ampcapicity = new System.Collections.Generic.List<stru_CommRange>();
			for (int i = 0; i < src.ampcapicity.Count; i++)
			{
				stru_CommRange src2 = src.ampcapicity[i];
				stru_CommRange item = default(stru_CommRange);
				item.copy(src2);
				this.ampcapicity.Add(item);
			}
			this.commonThresholdFlag = src.commonThresholdFlag;
			this.energyboxVoltage = src.energyboxVoltage;
			this.switchableOutlets = src.switchableOutlets;
			this.killPowerDisableRebootOutlets = src.killPowerDisableRebootOutlets;
			this.perportreadingOutlets = src.perportreadingOutlets;
			this.devThresholds = default(DevThreshold);
			this.devThresholds.copy(src.devThresholds);
			this.bankOutlets = new System.Collections.Generic.List<BankOutlets>();
			for (int j = 0; j < src.bankOutlets.Count; j++)
			{
				BankOutlets src3 = src.bankOutlets[j];
				BankOutlets item2 = default(BankOutlets);
				item2.copy(src3);
				this.bankOutlets.Add(item2);
			}
			this.m_FW = default(FW_stru);
			this.m_FW.copy(src.m_FW);
		}
		public bool isOutletSwitchable(int outlet)
		{
			return (this.switchableOutlets & DevModelConfig.OutletBitmap[outlet]) > 0uL;
		}
		public bool isOutletReadable(int outlet)
		{
			return (this.perportreadingOutlets & DevModelConfig.OutletBitmap[outlet]) > 0uL;
		}
		public bool isOutletRebootable(int outlet)
		{
			return (this.killPowerDisableRebootOutlets & DevModelConfig.OutletBitmap[outlet]) > 0uL;
		}
	}
}
