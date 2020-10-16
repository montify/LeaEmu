using System;

namespace Core
{
	class Program
	{
		static void Main(string[] args)
		{
			var cpu = new Cpu();
			cpu.Execute(new Instruction(CPUOpCode.LDA, 0xFA, CPUAdressingMode.Immediate)); // Load 0x80 in REG_A
			cpu.Execute(new Instruction(CPUOpCode.STA, 0x01, CPUAdressingMode.Immediate)); // Store value from REG_A in memory[0x01] 
            cpu.Execute(new Instruction(CPUOpCode.ADC, 0x01, CPUAdressingMode.Immediate)); // Store value from REG_A in memory[0x01] 
           
            
            
		



			System.Console.WriteLine("--------REGISTERS---------");
			cpu.m_register.PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");

			cpu.m_memory.ShowMemory(0x00, 0xFF, 10);

			Console.Read();
		}
	}
}
