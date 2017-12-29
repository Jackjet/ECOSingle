using DBAccessAPI;
using EcoDevice.AccessAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace EcoSensors.Common
{
	internal class commUtil
	{
		private const int WM_SETREDRAW = 11;
		public static DevSnmpConfig getSNMPpara(DeviceInfo pDevInfo)
		{
			DevSnmpConfig devSnmpConfig = new DevSnmpConfig();
			devSnmpConfig.devID = pDevInfo.DeviceID;
			devSnmpConfig.devMac = pDevInfo.Mac;
			devSnmpConfig.modelName = pDevInfo.ModelNm;
			devSnmpConfig.devIP = pDevInfo.DeviceIP;
			devSnmpConfig.devPort = pDevInfo.Port;
			devSnmpConfig.fmwareVer = pDevInfo.FWVersion;
			switch (pDevInfo.SnmpVersion)
			{
			case 1:
				devSnmpConfig.snmpVer = 0;
				break;
			case 2:
				devSnmpConfig.snmpVer = 1;
				break;
			case 3:
				devSnmpConfig.snmpVer = 3;
				break;
			}
			devSnmpConfig.userName = pDevInfo.Username;
			devSnmpConfig.retry = pDevInfo.Retry;
			devSnmpConfig.timeout = pDevInfo.Timeout;
			devSnmpConfig.authPSW = pDevInfo.AuthenPassword;
			devSnmpConfig.authType = pDevInfo.Authentication;
			devSnmpConfig.privPSW = pDevInfo.PrivacyPassword;
			devSnmpConfig.privType = pDevInfo.Privacy;
			return devSnmpConfig;
		}
		public static string uniqueIDs(string ids)
		{
			string[] array = ids.Split(new char[]
			{
				','
			});
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.Length != 0 && !dictionary.ContainsKey(text))
				{
					dictionary.Add(text, "");
				}
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary)
			{
				string key = current.Key;
				stringBuilder.Append(key + ",");
			}
			return stringBuilder.ToString();
		}
		public static void TB_showColumn(DataTable src)
		{
			foreach (DataColumn arg_19_0 in src.Columns)
			{
			}
		}
		public static void TB_show(DataTable src)
		{
			foreach (DataColumn arg_19_0 in src.Columns)
			{
			}
			foreach (DataRow dataRow in src.Rows)
			{
				object[] itemArray = dataRow.ItemArray;
				for (int i = 0; i < itemArray.Length; i++)
				{
					object arg_61_0 = itemArray[i];
				}
			}
		}
		public static void TB_show(DataRow[] src)
		{
			for (int i = 0; i < src.Length; i++)
			{
				DataRow dataRow = src[i];
				object[] itemArray = dataRow.ItemArray;
				for (int j = 0; j < itemArray.Length; j++)
				{
					object arg_1A_0 = itemArray[j];
				}
			}
		}
		public static void ShowInfo_DEBUG(string msg)
		{
		}
		public static void BeginControlUpdate(Control control)
		{
			Message message = Message.Create(control.Handle, 11, System.IntPtr.Zero, System.IntPtr.Zero);
			NativeWindow nativeWindow = NativeWindow.FromHandle(control.Handle);
			nativeWindow.DefWndProc(ref message);
		}
		public static void EndControlUpdate(Control control)
		{
			System.IntPtr wparam = new System.IntPtr(1);
			Message message = Message.Create(control.Handle, 11, wparam, System.IntPtr.Zero);
			NativeWindow nativeWindow = NativeWindow.FromHandle(control.Handle);
			nativeWindow.DefWndProc(ref message);
			control.Invalidate();
			control.Refresh();
		}
	}
}
