using Microsoft.Win32;
using System;
namespace RawInput
{
	internal static class RegistryAccess
	{
		internal static RegistryKey GetDeviceKey(string device)
		{
			if (device.Length < 5)
			{
				return null;
			}
			string[] array = device.Substring(4).Split(new char[]
			{
				'#'
			});
			if (array.Length < 3)
			{
				return null;
			}
			string arg = array[0];
			string arg2 = array[1];
			string arg3 = array[2];
			return Registry.LocalMachine.OpenSubKey(string.Format("System\\CurrentControlSet\\Enum\\{0}\\{1}\\{2}", arg, arg2, arg3));
		}
		internal static string GetClassType(string classGuid)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + classGuid);
			if (registryKey == null)
			{
				return string.Empty;
			}
			return (string)registryKey.GetValue("Class");
		}
	}
}
