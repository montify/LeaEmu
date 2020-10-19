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

		public int InstructionLenghtInByte;

		public CPUInstruction(CPUOpCode opCode, CPUAdressingMode adressingMode, int instructionLenghtInByte, byte firstOperand = 0x00, byte secondOperand = 0x00)
		{
			OpCode = opCode;
			AdressingMode = adressingMode;
			FirstOperand = firstOperand;
			SecondOperand = secondOperand;
			InstructionLenghtInByte = instructionLenghtInByte;
		}


	}
}
