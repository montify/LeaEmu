using System;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public struct CPUInstruction
	{
		public CPUOpCode OpCode;
		public CPUAdressingMode AdressingMode;
		public byte FirstOperand;
		public byte SecondOperand;

		public CPUInstruction(CPUOpCode opCode, CPUAdressingMode adressingMode = CPUAdressingMode.Accumulator, byte firstOperand = 0x00, byte secondOperand = 0x00)
		{
			OpCode = opCode;
			AdressingMode = adressingMode;
			FirstOperand = firstOperand;
			SecondOperand = secondOperand;
		}

		
	}
}
