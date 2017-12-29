using System;
using System.IO;
using System.Management;
namespace CommonAPI.WMI
{
	public class Query
	{
		public static int getCpuUsage()
		{
			int num = 0;
			int num2 = 0;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					object value = managementObject["PercentProcessorTime"];
					object arg_43_0 = managementObject["Name"];
					num2 += Convert.ToInt32(value);
					num++;
				}
			}
			if (num > 0)
			{
				return num2 / num;
			}
			return 0;
		}
		private static byte[] Hex2Array(string hex)
		{
			string text = "00112233445566778899aAbBcCdDeEfF";
			int num = 0;
			byte[] array = new byte[hex.Length / 2];
			for (int i = 0; i < hex.Length / 2; i++)
			{
				int num2 = text.IndexOf(hex.Substring(2 * i, 1));
				int num3 = text.IndexOf(hex.Substring(2 * i + 1, 1));
				if (num2 < 0 || num3 < 0)
				{
					break;
				}
				array[i] = (byte)(num2 / 2 << 4);
				byte[] expr_5E_cp_0 = array;
				int expr_5E_cp_1 = i;
				expr_5E_cp_0[expr_5E_cp_1] += (byte)(num3 / 2);
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			byte[] array2 = new byte[num];
			Array.Copy(array, 0, array2, 0, num);
			return array2;
		}
		public static string getDriveLetter(string strPath)
		{
			string pathRoot = Path.GetPathRoot(strPath);
			if (pathRoot.IndexOf(":") < 0)
			{
				pathRoot = Path.GetPathRoot(Environment.CurrentDirectory);
			}
			return pathRoot.Replace("\\", "");
		}
		public static string getDiskDriveSerialNo(string strPath = null)
		{
			string driveLetter;
			if (string.IsNullOrEmpty(strPath))
			{
				driveLetter = Query.getDriveLetter(Environment.SystemDirectory);
			}
			else
			{
				driveLetter = Query.getDriveLetter(strPath);
			}
			string value = "";
			string text = "";
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_LogicalDiskToPartition");
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						string text2 = managementObject["Antecedent"].ToString();
						string text3 = managementObject["Dependent"].ToString();
						int num = text3.IndexOf("Win32_LogicalDisk.DeviceID=");
						if (num >= 0)
						{
							string text4 = text3.Substring(num + "Win32_LogicalDisk.DeviceID=".Length);
							text4 = text4.Replace("\"", "");
							if (text4.Equals(driveLetter))
							{
								text2 = text2.Replace(" ", "");
								num = text2.IndexOf("Win32_DiskPartition.DeviceID=");
								if (num >= 0)
								{
									string text5 = text2.Substring(num + "Win32_DiskPartition.DeviceID=".Length);
									num = text5.IndexOf(",");
									if (num >= 0)
									{
										text5 = text5.Substring(0, num);
										text5 = text5.Replace("Disk#", "");
										text5 = text5.Replace("\"", "");
										value = text5;
										break;
									}
								}
							}
						}
					}
				}
				if (!string.IsNullOrEmpty(value))
				{
					managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
					using (ManagementObjectCollection.ManagementObjectEnumerator enumerator2 = managementObjectSearcher.Get().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ManagementObject managementObject2 = (ManagementObject)enumerator2.Current;
							string text6 = managementObject2["DeviceID"].ToString();
							int num2 = text6.IndexOf("PHYSICALDRIVE");
							if (num2 >= 0)
							{
								text6 = text6.Substring(num2 + "PHYSICALDRIVE".Length);
								if (text6.Equals(value))
								{
									if (managementObject2["SerialNumber"] == null)
									{
										break;
									}
									string value2 = managementObject2["SerialNumber"].ToString().Trim();
									if (!string.IsNullOrEmpty(value2))
									{
										text += managementObject2["Model"].ToString().Trim();
										text += " ";
										text += managementObject2["SerialNumber"].ToString().Trim();
										break;
									}
									break;
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			text = text.Replace(" ", "_");
			return text;
		}
		public static string getBaseBoardSerialNo()
		{
			string text = "";
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_BaseBoard");
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						if (managementObject["SerialNumber"] != null)
						{
							if (!string.IsNullOrEmpty(text))
							{
								text += "_";
							}
							text += managementObject["SerialNumber"].ToString().Trim();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return text;
		}
		public static DateTime getLastBootUpTime()
		{
			DateTime result = DateTime.Now;
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						if (managementObject["LastBootUpTime"] != null)
						{
							result = ManagementDateTimeConverter.ToDateTime(managementObject["LastBootUpTime"].ToString().Trim());
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
		public static string getProcessorID()
		{
			string text = "";
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_Processor");
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						if (managementObject["ProcessorId"] != null)
						{
							if (!string.IsNullOrEmpty(text))
							{
								text += "_";
							}
							text += managementObject["ProcessorId"].ToString().Trim();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return text;
		}
		public static string getSystemID(string strPath = null)
		{
			string text = "";
			string diskDriveSerialNo = Query.getDiskDriveSerialNo(strPath);
			string baseBoardSerialNo = Query.getBaseBoardSerialNo();
			string processorID = Query.getProcessorID();
			if (!string.IsNullOrEmpty(diskDriveSerialNo))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "_";
				}
				text += diskDriveSerialNo;
			}
			if (!string.IsNullOrEmpty(baseBoardSerialNo))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "_";
				}
				text += baseBoardSerialNo;
			}
			if (!string.IsNullOrEmpty(processorID))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "_";
				}
				text += processorID;
			}
			return text;
		}
	}
}
