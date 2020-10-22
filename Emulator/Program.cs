using System;

namespace EmulatorLib
{
	class Program
	{
		static void Main(string[] args)
		{

			var emulator = new Emulator();

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

			emulator.LoadProgramm("A9FF48A9FF48A2014C0B00E88A484C0B00");
			emulator.Execute();
		}
	}
}
