using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CpuLib;
using MemoryLib;
using VideoLib;

namespace EmulatorLib
{
	public class Emulator
	{
		private Cpu m_Cpu;
		private Memory m_memory;
		private OpCodeTable m_OpCodeLookUpTable = new OpCodeTable();

		D3D11VideoDriver m_Video;

		public Emulator()
		{
			m_memory = new Memory(ushort.MaxValue);
			m_Cpu = new Cpu(m_memory);
			m_Video = new D3D11VideoDriver();


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

		public void Execute()
		{
			int cycles = 0;
			m_Video.Run(() =>
			{

				var instruction = m_OpCodeLookUpTable.GetCpuInstruction(m_Cpu.m_memory.Read(m_Cpu.m_register.Read_PC()));

				// Fetch firstOperand, dont care if any operand is needed for now
				// For some Ops like PHA, INX,.. no Opernad is needed
				// For JMP all both are needed because jmp needs a 16bit address
				instruction.FirstOperand = ReadOperand(instruction.InstructionLenghtInByte - 1);
				instruction.SecondOperand = ReadOperand(instruction.InstructionLenghtInByte - 2);

				ImGuiNET.ImGui.Text("REG_A: " + m_Cpu.m_register.Read_REG_A().ToString("X2"));
				ImGuiNET.ImGui.Text("REG_X: " + m_Cpu.m_register.Read_REG_X().ToString("X2"));
				ImGuiNET.ImGui.Text("REG_Y: " + m_Cpu.m_register.Read_REG_Y().ToString("X2"));
				ImGuiNET.ImGui.Text("CarryFlag: " + m_Cpu.m_register.Read_Carry_Flag());
				ImGuiNET.ImGui.Text("ZeroFlag: " + m_Cpu.m_register.Read_Zero_Flag());
				ImGuiNET.ImGui.Text("NextOp: " + instruction.OpCode + " |Value: " + instruction.FirstOperand.ToString("X2") + "," + instruction.SecondOperand.ToString("X2"));
				ImGuiNET.ImGui.Text("PC: " + m_Cpu.m_register.Read_PC().ToString("X2"));
				ImGuiNET.ImGui.Text("TotalCycles: " + cycles);
				
				ImGuiNET.ImGui.Begin("Stack");
				ImGuiNET.ImGui.Text(m_memory.DebugStackRegion().ToString());
				ImGuiNET.ImGui.End();

				ImGuiNET.ImGui.Begin("Memory");
				ImGuiNET.ImGui.Text(m_memory.DebugMemory(0x0000, 0x00FF).ToString());
				ImGuiNET.ImGui.End();


				if (ImGuiNET.ImGui.Button("Next Step"))
				{
					cycles++;
					m_Cpu.Execute(instruction);
				}
			});

			m_Video.Dispose();
		}


		private byte ReadOperand(int offsetFromOpCode)
		{
			return m_Cpu.m_memory.Read((byte)(m_Cpu.m_register.Read_PC() + offsetFromOpCode)); //peek
		}
	}
}