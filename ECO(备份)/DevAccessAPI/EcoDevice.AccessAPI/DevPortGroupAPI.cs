using CommonAPI;
using System;
using System.Collections.Generic;
using System.Text;
namespace EcoDevice.AccessAPI
{
	public class DevPortGroupAPI
	{
		private System.Collections.Generic.List<SnmpConfiger> snmpConfigs = new System.Collections.Generic.List<SnmpConfiger>();
		private System.Collections.Generic.List<DevSnmpConfig> devsnmpconfigs;
		private int totalCount;
		private bool finalResult = true;
		private bool singleThread = true;
		private DefaultSnmpExecutors se;
		public DevPortGroupAPI(System.Collections.Generic.List<DevSnmpConfig> devcfgs)
		{
			this.devsnmpconfigs = devcfgs;
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			foreach (DevSnmpConfig current in devcfgs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger snmpConfiger = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				snmpConfiger.GroupOutlets = current.groupOutlets;
				this.snmpConfigs.Add(snmpConfiger);
			}
			this.se = new DefaultSnmpExecutors(this.snmpConfigs);
		}
		private void ReportPending(string strSuccessList, string failedList)
		{
			char[] separator = new char[]
			{
				';'
			};
			string[] array = strSuccessList.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (stringBuilder.Length + text.Length + 1 > 65023)
					{
						if (DevAccessAPI.cbOnDeviceChanged != null && stringBuilder.Length > 0)
						{
							DevAccessAPI.cbOnDeviceChanged("pending more", stringBuilder.ToString());
						}
						stringBuilder = new System.Text.StringBuilder();
					}
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(text);
				}
			}
			if (stringBuilder.Length > 0 && DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", stringBuilder.ToString());
			}
		}
		public bool TurnOnGroupOutlets()
		{
			System.DateTime now = System.DateTime.Now;
			string[] array = this.se.TurnOnGroupOutlets();
			string text = array[0];
			string strSuccessList = array[1];
			if (text.Length > 0)
			{
				this.finalResult = false;
			}
			System.TimeSpan timeSpan = System.DateTime.Now - now;
			DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
			{
				"Group turn on (",
				this.devsnmpconfigs.Count,
				"): ",
				timeSpan.TotalSeconds
			}));
			this.ReportPending(strSuccessList, text);
			return this.finalResult;
		}
		public bool TurnOffGroupOutlets()
		{
			System.DateTime now = System.DateTime.Now;
			string[] array = this.se.TurnOffGroupOutlets();
			string text = array[0];
			string strSuccessList = array[1];
			if (text.Length > 0)
			{
				this.finalResult = false;
			}
			System.TimeSpan timeSpan = System.DateTime.Now - now;
			DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
			{
				"Group turn off (",
				this.devsnmpconfigs.Count,
				"): ",
				timeSpan.TotalSeconds
			}));
			this.ReportPending(strSuccessList, text);
			return this.finalResult;
		}
		public bool RebootGroupOutlets()
		{
			System.DateTime now = System.DateTime.Now;
			string[] array = this.se.RebootGroupOutlets();
			string text = array[0];
			string strSuccessList = array[1];
			if (text.Length > 0)
			{
				this.finalResult = false;
			}
			this.ReportPending(strSuccessList, text);
			System.TimeSpan timeSpan = System.DateTime.Now - now;
			DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
			{
				"Group reboot (",
				this.devsnmpconfigs.Count,
				"): ",
				timeSpan.TotalSeconds
			}));
			return this.finalResult;
		}
		public bool GetGroupOutletsStatus()
		{
			System.DateTime now = System.DateTime.Now;
			string[] statusGroupOutlets = this.se.GetStatusGroupOutlets();
			string text = statusGroupOutlets[0];
			string arg_19_0 = statusGroupOutlets[1];
			if (text.Length > 0)
			{
				this.finalResult = false;
			}
			System.TimeSpan timeSpan = System.DateTime.Now - now;
			DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
			{
				"Group get status (",
				this.devsnmpconfigs.Count,
				"): ",
				timeSpan.TotalSeconds
			}));
			return this.finalResult;
		}
	}
}
