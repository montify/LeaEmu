using System;
using MemoryLib;
using CpuLib;

namespace VideoLib
{
	public class TestVid
	{
		private Memory m_Memory;
		private Cpu m_Cpu;

		public TestVid(Memory memory, Cpu cpu)
		{
			m_Memory = memory;
			m_Cpu = cpu;
		}

		public void DebugPrint()
		{
			Console.Clear();
			System.Console.WriteLine("--------REGISTERS---------");
			m_Cpu.m_register.PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");

			//m_Cpu.m_memory.DebugMemory(0x00, 0xFF);

			m_Cpu.m_memory.DebugStackRegion();
		}
	}
}
