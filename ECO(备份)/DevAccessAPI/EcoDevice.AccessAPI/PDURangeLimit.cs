using System;
namespace EcoDevice.AccessAPI
{
	public enum PDURangeLimit
	{
		Unconnected = -500,
		ValueEmpty = -1000,
		ThresholdEmpty = -300,
		NotPresent = -600,
		FailedGet = -700,
		ErrorValue = -800
	}
}
