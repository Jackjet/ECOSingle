using System;
namespace RawInput
{
	public enum HidUsage : ushort
	{
		Undefined,
		Pointer,
		Mouse,
		Joystick = 4,
		Gamepad,
		Keyboard,
		Keypad,
		SystemControl = 128,
		Tablet = 128,
		Consumer = 12
	}
}
