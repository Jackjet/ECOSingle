using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class DevScheduleAPI
	{
		private DevSnmpConfig snmpCfg;
		private DevModelConfig mc;
		private SnmpConfig sc;
		private SnmpExecutor se;
		private DevAccessCfg cfg;
		private System.Collections.Generic.List<DevSnmpConfig> devsnmpconfigs;
		private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> devMap = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>>();
		public DevScheduleAPI(System.Collections.Generic.List<DevSnmpConfig> devcfgs)
		{
			this.devsnmpconfigs = devcfgs;
		}
		public System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> TurnOnGroupOutlets()
		{
			this.devMap.Clear();
			foreach (DevSnmpConfig current in this.devsnmpconfigs)
			{
				this.cfg = DevAccessCfg.GetInstance();
				this.snmpCfg = current;
				this.sc = this.cfg.getSnmpConfig(this.snmpCfg);
				this.mc = this.cfg.getDeviceModelConfig(this.snmpCfg.modelName, this.snmpCfg.fmwareVer);
				if (this.mc.switchable == 2)
				{
					this.se = new DefaultSnmpExecutor(new SnmpConfiger(this.sc, this.mc));
					bool flag = this.se.TurnOnGroupOutlets(current.groupOutlets);
					if (flag)
					{
						this.devMap.Add(current.devID, current.groupOutlets);
					}
				}
			}
			return this.devMap;
		}
		public System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> TurnOffGroupOutlets()
		{
			this.devMap.Clear();
			foreach (DevSnmpConfig current in this.devsnmpconfigs)
			{
				this.cfg = DevAccessCfg.GetInstance();
				this.snmpCfg = current;
				this.sc = this.cfg.getSnmpConfig(this.snmpCfg);
				this.mc = this.cfg.getDeviceModelConfig(this.snmpCfg.modelName, this.snmpCfg.fmwareVer);
				if (this.mc.switchable == 2)
				{
					this.se = new DefaultSnmpExecutor(new SnmpConfiger(this.sc, this.mc));
					bool flag = this.se.TurnOffGroupOutlets(current.groupOutlets);
					if (flag)
					{
						this.devMap.Add(current.devID, current.groupOutlets);
					}
				}
			}
			return this.devMap;
		}
		public System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> TurnOnDeviceOutlets()
		{
			this.devMap.Clear();
			foreach (DevSnmpConfig current in this.devsnmpconfigs)
			{
				this.cfg = DevAccessCfg.GetInstance();
				this.snmpCfg = current;
				this.sc = this.cfg.getSnmpConfig(this.snmpCfg);
				this.mc = this.cfg.getDeviceModelConfig(this.snmpCfg.modelName, this.snmpCfg.fmwareVer);
				if (this.mc.switchable == 2)
				{
					this.se = new DefaultSnmpExecutor(new SnmpConfiger(this.sc, this.mc));
					bool flag = this.se.TurnOnOutlets();
					if (flag)
					{
						for (int i = 1; i <= this.mc.portNum; i++)
						{
							current.groupOutlets.Add(i);
						}
						this.devMap.Add(current.devID, current.groupOutlets);
					}
				}
			}
			return this.devMap;
		}
		public System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> TurnOffDeviceOutlets()
		{
			this.devMap.Clear();
			foreach (DevSnmpConfig current in this.devsnmpconfigs)
			{
				this.cfg = DevAccessCfg.GetInstance();
				this.snmpCfg = current;
				this.sc = this.cfg.getSnmpConfig(this.snmpCfg);
				this.mc = this.cfg.getDeviceModelConfig(this.snmpCfg.modelName, this.snmpCfg.fmwareVer);
				if (this.mc.switchable == 2)
				{
					this.se = new DefaultSnmpExecutor(new SnmpConfiger(this.sc, this.mc));
					bool flag = this.se.TurnOffOutlets();
					if (flag)
					{
						for (int i = 1; i <= this.mc.portNum; i++)
						{
							current.groupOutlets.Add(i);
						}
						this.devMap.Add(current.devID, current.groupOutlets);
					}
				}
			}
			return this.devMap;
		}
	}
}
