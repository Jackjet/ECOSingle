using System;
namespace EcoDevice.AccessAPI
{
	public class GroupOPThread
	{
		public delegate void GroupOPCallback(bool result, DevSnmpConfig snmpCfg);
		public struct GroupOPContext
		{
			public DevSnmpConfig devsnmpcfg;
			public GroupOPThread.GroupOPCallback opCallback;
		}
		public static void TurnOn(object obj)
		{
			bool result = true;
			GroupOPThread.GroupOPContext groupOPContext = (GroupOPThread.GroupOPContext)obj;
			DevSnmpConfig devsnmpcfg = groupOPContext.devsnmpcfg;
			DevAccessAPI devAccessAPI = new DevAccessAPI(devsnmpcfg);
			if (!devAccessAPI.TurnOnGroupOutlets(devsnmpcfg.groupOutlets))
			{
				result = false;
			}
			if (groupOPContext.opCallback != null)
			{
				groupOPContext.opCallback(result, devsnmpcfg);
			}
		}
		public static void TurnOff(object obj)
		{
			bool result = true;
			GroupOPThread.GroupOPContext groupOPContext = (GroupOPThread.GroupOPContext)obj;
			DevSnmpConfig devsnmpcfg = groupOPContext.devsnmpcfg;
			DevAccessAPI devAccessAPI = new DevAccessAPI(devsnmpcfg);
			if (!devAccessAPI.TurnOffGroupOutlets(devsnmpcfg.groupOutlets))
			{
				result = false;
			}
			if (groupOPContext.opCallback != null)
			{
				groupOPContext.opCallback(result, devsnmpcfg);
			}
		}
		public static void Reboot(object obj)
		{
			bool result = true;
			GroupOPThread.GroupOPContext groupOPContext = (GroupOPThread.GroupOPContext)obj;
			DevSnmpConfig devsnmpcfg = groupOPContext.devsnmpcfg;
			DevAccessAPI devAccessAPI = new DevAccessAPI(devsnmpcfg);
			if (!devAccessAPI.RebootGroupOutlets(devsnmpcfg.groupOutlets))
			{
				result = false;
			}
			if (groupOPContext.opCallback != null)
			{
				groupOPContext.opCallback(result, devsnmpcfg);
			}
		}
		public static void GetStatus(object obj)
		{
			bool result = true;
			GroupOPThread.GroupOPContext groupOPContext = (GroupOPThread.GroupOPContext)obj;
			DevSnmpConfig devsnmpcfg = groupOPContext.devsnmpcfg;
			DevAccessAPI devAccessAPI = new DevAccessAPI(devsnmpcfg);
			devAccessAPI.GetDeviceOutletsStatus();
			if (groupOPContext.opCallback != null)
			{
				groupOPContext.opCallback(result, devsnmpcfg);
			}
		}
	}
}
