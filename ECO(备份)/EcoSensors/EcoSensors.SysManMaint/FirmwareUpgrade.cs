using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors.Common;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
namespace EcoSensors.SysManMaint
{
	internal class FirmwareUpgrade
	{
		public delegate void Upgrading(UpgradeStatus status, int devid, string upgradedVersion);
		private const int No_Response = -100;
		private const int upgradeFail = 0;
		private const int Initail = 1;
		private const int inUpgrading = 2;
		private const int NotTheNewestFile = 3;
		private const int ErrorCode4 = 4;
		private const int ErrorCode5 = 5;
		private int m_devID;
		private string m_devIP;
		private string m_httpPort;
		private string filename;
		private int checkVersion;
		private int pollingstatus;
		public int DevID
		{
			set
			{
				this.m_devID = value;
			}
		}
		public string DevIP
		{
			set
			{
				this.m_devIP = value;
			}
		}
		public string httpPort
		{
			set
			{
				this.m_httpPort = value;
			}
		}
		public string FileName
		{
			set
			{
				this.filename = value;
			}
		}
		public int CheckVersion
		{
			set
			{
				this.checkVersion = value;
			}
		}
		public void Upgrade(FirmwareUpgrade.Upgrading upgrading)
		{
			if (upgrading == null)
			{
				upgrading = delegate(UpgradeStatus us, int devid, string newVersion)
				{
				};
			}
			upgrading(UpgradeStatus.Starting, this.m_devID, "");
			int num = this.postCheckVersion();
			if (num == 0)
			{
				upgrading(UpgradeStatus.Uploading, this.m_devID, "");
				System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.uploadFwFile));
				thread.Start();
				int num2 = 0;
				while (thread.IsAlive)
				{
					if (num2 == 0)
					{
						upgrading(UpgradeStatus.Uploading, this.m_devID, ".");
					}
					else
					{
						if (num2 == 1)
						{
							upgrading(UpgradeStatus.Uploading, this.m_devID, "..");
						}
						else
						{
							if (num2 == 2)
							{
								upgrading(UpgradeStatus.Uploading, this.m_devID, "...");
							}
							else
							{
								if (num2 == 3)
								{
									upgrading(UpgradeStatus.Uploading, this.m_devID, "....");
								}
								else
								{
									if (num2 == 4)
									{
										upgrading(UpgradeStatus.Uploading, this.m_devID, ".....");
									}
									else
									{
										if (num2 == 5)
										{
											upgrading(UpgradeStatus.Uploading, this.m_devID, "......");
										}
									}
								}
							}
						}
					}
					num2 = (num2 + 1) % 6;
					System.Threading.Thread.Sleep(1000);
				}
				upgrading(UpgradeStatus.Upgrading, this.m_devID, "");
				this.pollingstatus = -100;
				System.Threading.Thread thread2 = new System.Threading.Thread(new System.Threading.ThreadStart(this.pollingUpgradingStatus));
				thread2.Start();
				num2 = 0;
				while (thread2.IsAlive)
				{
					if (num2 % 6 == 0)
					{
						upgrading(UpgradeStatus.Upgrading, this.m_devID, ".");
					}
					else
					{
						if (num2 % 6 == 1)
						{
							upgrading(UpgradeStatus.Upgrading, this.m_devID, "..");
						}
						else
						{
							if (num2 % 6 == 2)
							{
								upgrading(UpgradeStatus.Upgrading, this.m_devID, "...");
							}
							else
							{
								if (num2 % 6 == 3)
								{
									upgrading(UpgradeStatus.Upgrading, this.m_devID, "....");
								}
								else
								{
									if (num2 % 6 == 4)
									{
										upgrading(UpgradeStatus.Upgrading, this.m_devID, ".....");
									}
									else
									{
										if (num2 % 6 == 5)
										{
											upgrading(UpgradeStatus.Upgrading, this.m_devID, "......");
										}
									}
								}
							}
						}
					}
					num2++;
					if (num2 > 180)
					{
						thread2.Abort();
						thread2.Join();
						break;
					}
					System.Threading.Thread.Sleep(1000);
				}
				if (this.pollingstatus == -100)
				{
					upgrading(UpgradeStatus.UpgradeFailed, this.m_devID, "");
					return;
				}
				this.handleTheResult(upgrading, this.pollingstatus);
				return;
			}
			else
			{
				if (num == 1)
				{
					upgrading(UpgradeStatus.ServerBusy, this.m_devID, "");
					return;
				}
				upgrading(UpgradeStatus.ServerUnconnected, this.m_devID, "");
				return;
			}
		}
		private void pollingUpgradingStatus()
		{
			int num = 180;
			int num2 = 1;
			this.pollingstatus = -100;
			while (num2++ <= num)
			{
				this.pollingstatus = this.getUpgradeResult();
				if (this.pollingstatus == -100)
				{
					try
					{
						System.Threading.Thread.Sleep(1000);
						continue;
					}
					catch (System.Exception)
					{
						continue;
					}
				}
				if (this.pollingstatus != 2)
				{
					break;
				}
				try
				{
					System.Threading.Thread.Sleep(1000);
				}
				catch (System.Exception)
				{
				}
			}
		}
		private void handleTheResult(FirmwareUpgrade.Upgrading upgrading, int status)
		{
			switch (status)
			{
			case 0:
				upgrading(UpgradeStatus.UpgradeFailed, this.m_devID, "");
				return;
			case 2:
				upgrading(UpgradeStatus.Upgrading, this.m_devID, "");
				return;
			case 3:
				upgrading(UpgradeStatus.NoNeedToUpgrade, this.m_devID, "");
				return;
			case 4:
				upgrading(UpgradeStatus.UpgradeFailed, this.m_devID, "");
				return;
			case 5:
				upgrading(UpgradeStatus.UpgradeFailed, this.m_devID, "");
				return;
			}
			string text = string.Empty;
			DefaultSnmpExecutor defaultSnmpExecutor = null;
			try
			{
				DeviceInfo deviceByID = DeviceOperation.getDeviceByID(this.m_devID);
				DevSnmpConfig sNMPpara = commUtil.getSNMPpara(deviceByID);
				SnmpConfig snmpConfig = DevAccessCfg.GetInstance().getSnmpConfig(sNMPpara);
				defaultSnmpExecutor = new DefaultSnmpExecutor(new SnmpConfiger(snmpConfig, 1));
			}
			catch (System.Exception)
			{
			}
			int num = 0;
			while (num++ < 20)
			{
				try
				{
					PropertiesMessage properties_ATEN = defaultSnmpExecutor.GetProperties_ATEN();
					text = properties_ATEN.FirwWareVersion;
					break;
				}
				catch (System.Exception)
				{
					System.Threading.Thread.Sleep(1000);
				}
			}
			if (text.Equals(string.Empty))
			{
				upgrading(UpgradeStatus.UpgradeFailed, this.m_devID, "");
				return;
			}
			upgrading(UpgradeStatus.UpgradeSucceed, this.m_devID, text);
		}
		private int getUpgradeResult()
		{
			XmlDocument xmlDoc = this.getXmlDoc(string.Concat(new string[]
			{
				"http://",
				this.m_devIP,
				":",
				this.m_httpPort,
				"/xml/eco_sensor_fwup_info.xml?SID=ecoSensors"
			}));
			if (xmlDoc == null)
			{
				return -100;
			}
			XmlNode xmlNode = xmlDoc.GetElementsByTagName("FWupgradeResult")[0];
			return System.Convert.ToInt32(xmlNode.InnerText);
		}
		private void uploadFwFile()
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Concat(new string[]
				{
					"http://",
					this.m_devIP,
					":",
					this.m_httpPort,
					"/maintenance/fwupgrade.cgi"
				}));
				string str = "-------------" + System.DateTime.Now.Ticks.ToString("x");
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "multipart/form-data;boundary=" + str;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.Append("--" + str);
				stringBuilder.Append("\r\nContent-Disposition:form-data;name=\"eco Sensors\";filename=\"");
				stringBuilder.Append(System.IO.Path.GetFileName(this.filename));
				stringBuilder.Append("\"\r\n");
				stringBuilder.Append("Content-Type:application/octet-stream\r\n\r\n");
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
				byte[] bytes2 = System.Text.Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
				System.IO.Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				System.IO.FileStream fileStream = new System.IO.FileStream(this.filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				byte[] array = new byte[4096];
				for (int i = fileStream.Read(array, 0, array.Length); i > 0; i = fileStream.Read(array, 0, array.Length))
				{
					requestStream.Write(array, 0, i);
				}
				fileStream.Close();
				requestStream.Write(bytes2, 0, bytes2.Length);
				requestStream.Close();
				WebResponse response = httpWebRequest.GetResponse();
				response.Close();
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("uploadFwFile exception### " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ex.Message);
			}
		}
		private XmlDocument getXmlDoc(string url)
		{
			XmlDocument xmlDocument = null;
			try
			{
				WebRequest webRequest = WebRequest.Create(url);
				System.IO.Stream responseStream = webRequest.GetResponse().GetResponseStream();
				System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				string value;
				while ((value = streamReader.ReadLine()) != null)
				{
					stringBuilder.Append(value);
				}
				responseStream.Close();
				streamReader.Close();
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(stringBuilder.ToString());
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("getXmlDoc exception### " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ex.Message);
			}
			return xmlDocument;
		}
		private int postCheckVersion()
		{
			new System.Text.StringBuilder();
			try
			{
				string url = string.Concat(new object[]
				{
					"http://",
					this.m_devIP,
					":",
					this.m_httpPort,
					"/xml/firmware_upgrade.xml?CheckVersion=",
					this.checkVersion,
					"&name=eco Sensors&ECOUpgrade=1&SID=ecoSensors"
				});
				XmlDocument xmlDoc = this.getXmlDoc(url);
				XmlNodeList elementsByTagName = xmlDoc.GetElementsByTagName("FirmwareUpgradeFlag");
				return System.Convert.ToInt32(elementsByTagName[0].InnerText);
			}
			catch (System.Exception)
			{
			}
			return -100;
		}
	}
}
