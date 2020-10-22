using System;
using MemoryLib;
using CpuLib;
using System.Text;

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
			PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");

			//m_Cpu.m_memory.DebugMemory(0x00, 0xFF);

			m_Cpu.m_memory.DebugStackRegion();
		}

		private void PrintRegister()
		{
			var sb = new StringBuilder();

			Console.Write("A: 		");
			System.Console.Write(m_Cpu.m_register.Read_REG_A().ToString("x"));
			System.Console.WriteLine();

			Console.Write("X: 		");
			System.Console.Write(m_Cpu.m_register.Read_REG_X().ToString("x"));
			System.Console.WriteLine();

			Console.Write("Y: 		");
			System.Console.Write(m_Cpu.m_register.Read_REG_Y().ToString("x"));
			System.Console.WriteLine();

			Console.Write("PC: 		");
			System.Console.Write(m_Cpu.m_register.Read_PC().ToString("x"));
			System.Console.WriteLine();

			Console.Write("SP: 		");
			System.Console.Write(m_Cpu.m_register.Read_SP().ToString("x"));
			System.Console.WriteLine();

			Console.Write("CarryFLAG: 	");
			System.Console.Write(m_Cpu.m_register.Read_Carry_Flag());
			System.Console.WriteLine();

			Console.Write("ZeroFLAG: 	");
			System.Console.Write(m_Cpu.m_register.Read_Zero_Flag());
			System.Console.WriteLine();
		}
	}
}
