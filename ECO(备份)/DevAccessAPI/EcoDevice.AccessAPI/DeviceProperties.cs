using System;
namespace EcoDevice.AccessAPI
{
	public class DeviceProperties : Msg
	{
		private string deviceName;
		private string macAddress;
		private string fwVersion;
		private int dashboardRow;
		private int dashboardColumn;
		private string dashboardRackname;
		private string autoBasicInfo = string.Empty;
		private string autoRatingInfo = string.Empty;
		public int DashboardColumn
		{
			get
			{
				return this.dashboardColumn;
			}
			set
			{
				this.dashboardColumn = value;
			}
		}
		public int DashboardRow
		{
			get
			{
				return this.dashboardRow;
			}
			set
			{
				this.dashboardRow = value;
			}
		}
		public string FirwWareVersion
		{
			get
			{
				return this.fwVersion;
			}
			set
			{
				this.fwVersion = value;
			}
		}
		public string DashboardRackname
		{
			get
			{
				return this.dashboardRackname;
			}
			set
			{
				this.dashboardRackname = value;
			}
		}
		public string MacAddress
		{
			get
			{
				return this.macAddress;
			}
			set
			{
				this.macAddress = value;
			}
		}
		public new string DeviceName
		{
			get
			{
				return this.deviceName;
			}
			set
			{
				this.deviceName = value;
			}
		}
		public new string AutoBasicInfo
		{
			get
			{
				return this.autoBasicInfo;
			}
			set
			{
				this.autoBasicInfo = value;
			}
		}
		public new string AutoRatingInfo
		{
			get
			{
				return this.autoRatingInfo;
			}
			set
			{
				this.autoRatingInfo = value;
			}
		}
	}
}
