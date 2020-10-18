﻿using System;
using System.Collections.Generic;

namespace Core
{
	class Program
	{
		static void Main(string[] args)
		{
			var cpu = new Cpu();

			//SETUP
			/*
						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x8));//Number1 LO
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x00));
						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x00));//Number1 HI
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x01));
						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x5));//Number2 LO
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x02));
						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x00));//Number2 HI
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x03));

						//Addition
						// 0x08 + 0x05 = 0x0D
						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x00));
						cpu.Execute(new CPUInstruction(CPUOpCode.CLC)); // Note Carry-Flag must set to 0 for Addition (Borrow)
						cpu.Execute(new CPUInstruction(CPUOpCode.ADC, CPUAdressingMode.ZeroPage, 0x02));
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xA));

						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x01));
						cpu.Execute(new CPUInstruction(CPUOpCode.ADC, CPUAdressingMode.ZeroPage, 0x03));
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xB));

						//Substract 
						// 0x08 - 0x05 = 0x03
						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x00));
						cpu.Execute(new CPUInstruction(CPUOpCode.SEC)); // Note Carry-Flag must set to 1 for Substraction (Borrow)
						cpu.Execute(new CPUInstruction(CPUOpCode.SBC, CPUAdressingMode.ZeroPage, 0x02));
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xA));

						cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x01));
						cpu.Execute(new CPUInstruction(CPUOpCode.SBC, CPUAdressingMode.ZeroPage, 0x03));
						cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xB));

			*/







			// This above is only 1byte addition / subtraction
			// here comes the real meat ;)
			//Calcuate with 2byte value (eg. ushort) instead with 1 byte

			//Addition
			//0xAAA0 + 0x1110 = BBB0
			/*	cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0xAA));//Number1 LO
				cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x00));
				cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0xA0));//Number1 HI
				cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x01));

				cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x11));//Number2 LO
				cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x02));
				cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x10));//Number2 HI
				cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x03));



				cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x00));
				cpu.Execute(new CPUInstruction(CPUOpCode.CLC)); // Note Carry-Flag must set to 0 for Addition (Borrow)
				cpu.Execute(new CPUInstruction(CPUOpCode.ADC, CPUAdressingMode.ZeroPage, 0x02));
				cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xA));
				cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x01));
				cpu.Execute(new CPUInstruction(CPUOpCode.CLC)); // Note Carry-Flag must set to 0 for Addition (Borrow)
				cpu.Execute(new CPUInstruction(CPUOpCode.ADC, CPUAdressingMode.ZeroPage, 0x03));
				cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xB));

	*/


			//Substraction
			// 0xFFF0 - 0x2220 = 0xDDD0
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0xFF));//Number1 LO
			cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x00));
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0xF0));//Number1 HI
			cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x01));

			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x22));//Number2 LO
			cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x02));
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.Immediate, 0x20));//Number2 HI
			cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0x03));

			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x00));
			cpu.Execute(new CPUInstruction(CPUOpCode.SEC)); // Note Carry-Flag must set to 1 for Substraction (Borrow)
			cpu.Execute(new CPUInstruction(CPUOpCode.SBC, CPUAdressingMode.ZeroPage, 0x02));
			cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xA));
			cpu.Execute(new CPUInstruction(CPUOpCode.LDA, CPUAdressingMode.ZeroPage, 0x01));
			cpu.Execute(new CPUInstruction(CPUOpCode.SEC)); // Note Carry-Flag must set to 1 for Substraction (Borrow)
			cpu.Execute(new CPUInstruction(CPUOpCode.SBC, CPUAdressingMode.ZeroPage, 0x03));
			cpu.Execute(new CPUInstruction(CPUOpCode.STA, CPUAdressingMode.ZeroPage, 0xB));


			System.Console.WriteLine("--------REGISTERS---------");
			cpu.m_register.PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");


			cpu.m_memory.DebugMemory(0x0, 0xFF, 10);
			//cpu.m_memory.DebugStackRegion();

			Console.Read();
		}

		private static (byte lowerPart, byte upperPart) UShortToTwoBytes(ushort value)
		{
			return ((byte)(value >> 8), (byte)(value & 0x00FF));
		}
	}
}
