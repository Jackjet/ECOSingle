using DBAccessAPI;
using EcoDevice.AccessAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace EcoSensors.DevManDevice
{
	internal class DiscoverThread
	{
		private System.Collections.Generic.List<string[]> deviceList = new System.Collections.Generic.List<string[]>();
		private System.Threading.Thread scanThread;
		private int m_scanCompany;
		private string m_strip = string.Empty;
		private DevSnmpConfig m_SNMPPara;
		public DiscoverThread(int scanCompany, string strIPs, DevSnmpConfig SNMPPara)
		{
			this.m_scanCompany = scanCompany;
			this.m_strip = strIPs;
			this.m_SNMPPara = SNMPPara;
		}
		public void threadAbort()
		{
			if (this.scanThread != null && this.scanThread.IsAlive)
			{
				this.scanThread.Abort();
			}
		}
		public System.Collections.Generic.List<string[]> excute()
		{
			this.scanThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.DiscoveMethod));
			this.scanThread.IsBackground = true;
			this.scanThread.Start();
			this.scanThread.Join();
			return this.deviceList;
		}
		private void DiscoveMethod()
		{
			try
			{
				string[] array = this.m_strip.Split(new char[]
				{
					'.'
				});
				System.Collections.Generic.List<PropertiesMessage> list = null;
				if (array.Length == 4)
				{
					if (this.m_scanCompany == 1)
					{
						list = new DevDiscoverAPI(this.m_SNMPPara).Scan_ATEN(this.m_strip, this.m_strip);
					}
					else
					{
						if (this.m_scanCompany == 2)
						{
							list = new DevDiscoverAPI(this.m_SNMPPara).Scan_EATON(this.m_strip, this.m_strip);
						}
						else
						{
							if (this.m_scanCompany == 4)
							{
								list = new DevDiscoverAPI(this.m_SNMPPara).Scan_APC(this.m_strip, this.m_strip);
							}
						}
					}
				}
				else
				{
					if (array.Length == 3)
					{
						if (this.m_scanCompany == 1)
						{
							list = new DevDiscoverAPI(this.m_SNMPPara).Scan_ATEN(this.m_strip + ".1", this.m_strip + ".255");
						}
						else
						{
							if (this.m_scanCompany == 2)
							{
								list = new DevDiscoverAPI(this.m_SNMPPara).Scan_EATON(this.m_strip + ".1", this.m_strip + ".255");
							}
							else
							{
								if (this.m_scanCompany == 4)
								{
									list = new DevDiscoverAPI(this.m_SNMPPara).Scan_APC(this.m_strip + ".1", this.m_strip + ".255");
								}
							}
						}
					}
				}
				if (list != null && list.Count > 0)
				{
					using (System.Collections.Generic.List<PropertiesMessage>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PropertiesMessage current = enumerator.Current;
							string deviceName = current.DeviceName;
							string ipAddress = current.IpAddress;
							string macAddress = current.MacAddress;
							string firwWareVersion = current.FirwWareVersion;
							string modelName = current.ModelName;
							if (DeviceOperation.getDeviceByMac(macAddress) == null)
							{
								string text = current.DashboardRackname.Equals("\0") ? "" : current.DashboardRackname;
								if (!DevAccessCfg.GetInstance().getDeviceModelConfig(modelName, firwWareVersion).modelName.Equals(string.Empty))
								{
									string[] item = new string[]
									{
										deviceName,
										macAddress,
										ipAddress,
										modelName,
										text,
										firwWareVersion
									};
									this.deviceList.Add(item);
								}
							}
						}
						goto IL_268;
					}
				}
				this.deviceList.Count<string[]>();
				IL_268:;
			}
			catch (System.Exception)
			{
			}
		}
	}
}
