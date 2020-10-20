using System;
using System.Collections.Generic;
using System.IO;

namespace Core
{
	class Program
	{
		static void Main(string[] args)
		{

			Emulator emulator = new Emulator();


			/*
			START
				LDA#FF
				PHA
				LDA #FF
				PHA
				LDX #01
				JMP LOOP

			LOOP
				INX
				TXA
				PHA
				JMP LOOP
			*/
			emulator.LoadProgramm("A9FF48A9FF48A2014C0B00E88A484C0b");

			emulator.DebugPrint();
			emulator.Execute();

			Console.Read();
		}

		private static (byte lowerPart, byte upperPart) UShortToTwoBytes(ushort value)
		{
			return ((byte)(value >> 8), (byte)(value & 0x00FF));
		}
	}
}
