using System;
namespace RawInput
{
	[Flags]
	internal enum RawInputDeviceFlags
	{
		NONE = 0,
		REMOVE = 1,
		EXCLUDE = 16,
		PAGEONLY = 32,
		NOLEGACY = 48,
		INPUTSINK = 256,
		CAPTUREMOUSE = 512,
		NOHOTKEYS = 512,
		APPKEYS = 1024,
		EXINPUTSINK = 4096,
		DEVNOTIFY = 8192
	}
}
