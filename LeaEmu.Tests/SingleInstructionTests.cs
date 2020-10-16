using System;
using NUnit.Framework;

using Core;

namespace LeaEmu.Tests
{
	[TestFixture]
	public class SingleInstructionTests
	{
		Cpu cpu;

		[SetUp]
		public void Init()
		{
			cpu = new Cpu();
		}


		[Test]
		public void TestSingleInstruction()
		{
			cpu.Execute(new Instruction(CPUOpCode.LDA, 0xFA, CPUAdressingMode.Immediate)); // Load 0x80 in REG_A
			Assert.AreEqual(cpu.m_register.REG_A, 0xFA);


			cpu.Execute(new Instruction(CPUOpCode.STA, 0x01, CPUAdressingMode.Immediate)); // Store value from REG_A in memory[0x01] 
			Assert.AreEqual(cpu.m_memory.Read(0x01), 0xFA);

			cpu.Execute(new Instruction(CPUOpCode.ADC, 0x01, CPUAdressingMode.Immediate)); // Store value from REG_A in memory[0x01] 
			Assert.AreEqual(cpu.m_register.REG_A, 0xF4);

		}

	}
}
