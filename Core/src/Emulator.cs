using System;
using System.Globalization;
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
			for (ushort i = 0; i < byteCode.Length; i += 2)
			{
				var singleByteString = byteCode.Substring(i, 2);
				var singleByte = byte.Parse(singleByteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

				m_Cpu.m_memory.Write(memoryOffset++, singleByte);
			}
			m_Cpu.m_register.Write_PC(0x00);
		}

		public async void Execute()
		{
			while (true)
			{
				// for dirty demo purpose ;)
				await Task.Delay(1000);

				var instruction = m_OpCodeLookUpTable.GetCpuInstruction(m_Cpu.m_memory.Read(m_Cpu.m_register.Read_PC()));

				// Fetch firstOperand, dont care if any operand is needed for now
				// For some Ops like PHA, INX,.. no Opernad is needed
				// For JMP all both are needed because jmp needs a 16bit address
				instruction.FirstOperand = ReadOperand(instruction.InstructionLenghtInByte - 1);
				instruction.SecondOperand = ReadOperand(instruction.InstructionLenghtInByte - 2);

				m_Cpu.Execute(instruction);
				
				DebugPrint();
			

			}
		}

		private byte ReadOperand(int offsetFromOpCode)
		{
			return m_Cpu.m_memory.Read((byte)(m_Cpu.m_register.Read_PC() + offsetFromOpCode)); //peek
		}

		public void DebugPrint()
		{
			Console.Clear();
			System.Console.WriteLine("--------REGISTERS---------");
			m_Cpu.m_register.PrintRegister();
			System.Console.WriteLine();
			System.Console.WriteLine("--------MEMORY---------");

			m_Cpu.m_memory.DebugMemory(0x00, 0xFF);

			// m_Cpu.m_memory.DebugStackRegion();
		}
	}
}