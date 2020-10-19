using System;
using System.Collections.Generic;

namespace Core
{
	class Program
	{
		static void Main(string[] args)
		{

			Emulator emulator = new Emulator();

			//A2 01 = LDX value 0x01
			//E8 = Increment LDX
			//4C 0002 = Jump to 0x02 (NOTE: Jump always 16bit)
			emulator.LoadProgramm("A201E84C0002");


			emulator.Execute();

			Console.Read();
		}

		private static (byte lowerPart, byte upperPart) UShortToTwoBytes(ushort value)
		{
			return ((byte)(value >> 8), (byte)(value & 0x00FF));
		}
	}
}
