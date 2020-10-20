using System.Collections.Generic;
using Core;

public class OpCodeTable
{
	public Dictionary<byte, CPUInstruction> OpCodeLookUp = new Dictionary<byte, CPUInstruction>();

	public OpCodeTable()
	{
		GenerateOpCodeLookUp();
	}

	private void GenerateOpCodeLookUp()
	{
		OpCodeLookUp.Add(0xA9, new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 2));
		OpCodeLookUp.Add(0x85, new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 2));
		OpCodeLookUp.Add(0xA2, new CPUInstruction(CPUOpCode.LDX, CPUAdressingMode.Immediate, 2));
		OpCodeLookUp.Add(0x4C, new CPUInstruction(CPUOpCode.JMP, CPUAdressingMode.Absolute, 3));
		OpCodeLookUp.Add(0xE8, new CPUInstruction(CPUOpCode.INX, CPUAdressingMode.Accumulator, 1));
		OpCodeLookUp.Add(0x8A, new CPUInstruction(CPUOpCode.TXA, CPUAdressingMode.Implicit, 1));
		OpCodeLookUp.Add(0x48, new CPUInstruction(CPUOpCode.PHA, CPUAdressingMode.Implicit, 1));
	}

	public CPUInstruction ConvertHexToCpuInstrucution(byte instruction)
	{
		OpCodeLookUp.TryGetValue(instruction, out var cPUInstruction);
		return cPUInstruction;
	}
}