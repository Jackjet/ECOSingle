using System;
namespace EcoDevice.AccessAPI
{
	public enum OutletControl
	{
		OFF = 1,
		ON,
		Pending,
		Reboot,
		Fault,
		NoAuth,
		NA,
		POP
	}
}
