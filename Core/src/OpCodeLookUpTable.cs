using System;
using System.Collections.Generic;
using Core;

public class OpCodeLookUpTable
{
	public Dictionary<string, CPUInstruction> OpCodeLookUp = new Dictionary<string, CPUInstruction>();


	public List<CPUInstruction> cpuInstructions = new List<CPUInstruction>();

	public OpCodeLookUpTable()
	{
		Generate();
	}

	private void Generate()
	{
		OpCodeLookUp.Add("A9", new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate));
		OpCodeLookUp.Add("85", new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage));
	}

	private void ComposeInstruction()
	{

	}

	public void GetNextInstruction()
	{

		string input = "A9FF85DD";


		for (int i = 0; i < input.Length; i++)
		{
			var inst = input.Substring(i++, 2);
			if (OpCodeLookUp.TryGetValue(inst, out var t))
				System.Console.WriteLine(t.OpCode);
		}

	}
}