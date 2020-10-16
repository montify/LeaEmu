using System;
using System.Timers;

namespace Core
{

	class Program
	{
		static void Main(string[] args)
		{
<<<<<<< HEAD
=======
			var cpu = new Cpu();
			cpu.Execute(new Instruction(CPUOpCode.LDA, 0xFA, CPUAdressingMode.Immediate)); // Load 0x80 in REG_A
			cpu.Execute(new Instruction(CPUOpCode.STA, 0x01, CPUAdressingMode.Immediate)); // Store value from REG_A in memory[0x01] 
            cpu.Execute(new Instruction(CPUOpCode.ADC, 0x01, CPUAdressingMode.Immediate)); // Store value from REG_A in memory[0x01] 
              cpu.Execute(new Instruction(CPUOpCode.STA, 0xAE, CPUAdressingMode.Immediate)); // Store value from REG_A in memory[0x01] 
            
            
		


>>>>>>> parent of bc4a1b0... Add TestSln

			Console.Read();
		}

		static (byte upper, byte lower) ExtractBytes(ushort value)
		{
			byte upper = (byte)(value >> 8);
			byte lower = (byte)(value & 0xFF);

			return (upper, lower);
		}
	}


}
