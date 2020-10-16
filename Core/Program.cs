using System;

namespace Core
{
	class Program
	{
		static void Main(string[] args)
		{
			var cpu = new Cpu();
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0xC0)); 
			cpu.Execute(new CPUInstruction(CPUOpCode.TAX)); //ACCUMULATOR
			cpu.Execute(new CPUInstruction(CPUOpCode.INX));  //ACCUMULATOR
			cpu.Execute(new CPUInstruction(CPUOpCode.STX, CPUAdressingMode.ZeroPage, 0xA)); 
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0xA)); 
			


			//LDA #$c0  ;Load the hex value $c0 into the A register
			//TAX       ;Transfer the value in the A register to X
			//INX       ;Increment the value in the X register
			//ADC #$c4  ;Add the hex value $c4 to the A register
			//BRK       ;Break - we're done
			System.Console.WriteLine("--------REGISTERS---------");
			cpu.m_register.PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");

			cpu.m_memory.DebugMemory(0x00, 0xFF, 10);


			Console.Read();
		}


	}
}
