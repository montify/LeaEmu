using System;

namespace Core
{
	class Program
	{
		static void Main(string[] args)
		{
			var cpu = new Cpu();


			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x01));
			cpu.Execute(new CPUInstruction(CPUOpCode.PHA));
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0xFF));
			cpu.Execute(new CPUInstruction(CPUOpCode.PLA));
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0xFF));

			for (int i = 0; i < 0xFF; i++)
			{
				cpu.Execute(new CPUInstruction(CPUOpCode.PHA));
			}


			/*	
			LDA #$01	!!
			STA $20		!
			LDA #$05	!
			STA $021!
			LDA #$08
			STA $22*/

			//LDA #$c0  ;Load the hex value $c0 into the A register
			//TAX       ;Transfer the value in the A register to X
			//INX       ;Increment the value in the X register
			//ADC #$c4  ;Add the hex value $c4 to the A register
			//BRK       ;Break - we're done
			System.Console.WriteLine("--------REGISTERS---------");
			cpu.m_register.PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");


			cpu.m_memory.DebugMemory(0x00, 0x208, 10);
			//cpu.m_memory.DebugStackRegion();

			Console.Read();
		}


	}
}
