using System;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public struct Instruction
	{
		public CPUOpCode OpCode;
		public CPUAdressingMode AdressingMode;
		public byte Operand;

		public Instruction(CPUOpCode opCode, byte operand = default, CPUAdressingMode adressingMode = default)
		{
			OpCode = opCode;
			AdressingMode = adressingMode;
			Operand = operand;
		}


	}
}
