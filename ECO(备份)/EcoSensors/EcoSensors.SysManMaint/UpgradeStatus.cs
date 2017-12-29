using System;
namespace EcoSensors.SysManMaint
{
	public enum UpgradeStatus
	{
		Starting,
		ServerUnconnected,
		UpgradeSucceed,
		UpgradeFailed,
		ServerBusy,
		NoNeedToUpgrade,
		Uploading,
		Upgrading,
		WrongFile
	}
}
