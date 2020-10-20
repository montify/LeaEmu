using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
	public class Emulator
	{

		private Cpu m_Cpu;
		private Memory m_memory;

		private OpCodeTable m_OpCodeLookUpTable = new OpCodeTable();
		public Emulator()
		{
			m_memory = new Memory(ushort.MaxValue);
			m_Cpu = new Cpu(m_memory);
		}

		public void LoadProgramm(string byteCode)
		{
			ushort memoryOffset = 0;
			//A9FF
			for (ushort i = 0; i < byteCode.Length; i += 2)
			{
				var singleByteString = byteCode.Substring(i, 2);
				var singleByte = byte.Parse(singleByteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

				m_Cpu.m_memory.Write(memoryOffset++, singleByte);
			}

			m_Cpu.m_register.Write_PC(0x00);
		}

		public void Execute()
		{
			//for (int i = 0; i < 2; i++)
			while (true)
			{
				var inst = m_OpCodeLookUpTable.ConvertHexToCpuInstrucution(m_Cpu.m_memory.Read(m_Cpu.m_register.Read_PC()));

				//for dirty demo purpose ;)
				Thread.Sleep(100);

				//Fetch firstOperand, dont care if the operation need it or not
				inst.FirstOperand = m_Cpu.m_memory.Read((ushort)(m_Cpu.m_register.Read_PC() + (inst.InstructionLenghtInByte - 1))); //peek

				//Fetch seconOperand, dont care if the operation need it or not
				inst.SecondOperand = m_Cpu.m_memory.Read((ushort)(m_Cpu.m_register.Read_PC() + (inst.InstructionLenghtInByte - 2))); //peek

				if (inst.OpCode != CPUOpCode.JMP)
					m_Cpu.m_register.Write_PC((ushort)(m_Cpu.m_register.Read_PC() + inst.InstructionLenghtInByte));



				Console.Clear();
				m_Cpu.m_memory.DebugStackRegion();
				m_Cpu.Execute(inst);


			}
		}

		public void DebugPrint()
		{
			System.Console.WriteLine("--------REGISTERS---------");
			m_Cpu.m_register.PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");

			m_Cpu.m_memory.DebugMemory(0x0, 0xFF, 10);
			//m_Cpu.m_memory.DebugStackRegion();

		}
	}
}