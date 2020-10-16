using System;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html

	// https://en.wikibooks.org/wiki/6502_Assembly
	public class Cpu
	{
		public Register m_register { get; private set; }
		public Memory m_memory { get; private set; }
		public Alu m_alu { get; private set; }


		public Cpu()
		{
			m_register = new Register();
			m_memory = new Memory();
			m_alu = new Alu(m_register);
		}


		public void Execute(CPUInstruction instruction)
		{
			switch (instruction.OpCode)
			{
				case CPUOpCode.LDA:
					HandleLDA(instruction);
					break;
				case CPUOpCode.STA:
					HandleSTA(instruction);
					break;
				case CPUOpCode.STX:
					HandleSTX(instruction);
					break;
				case CPUOpCode.TAX:
					HandleTAX(instruction);
					break;
				case CPUOpCode.INX:
					HandleINX(instruction);
					break;
				case CPUOpCode.ADC:
					HandleADC(instruction);
					break;
				case CPUOpCode.BRK:
					break;
				default:
					throw new Exception($"Bad Instruction or not Implemented");
			}
		}

		private void HandleSTX(CPUInstruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Absolute:
					m_memory.Store((ushort)(instruction.FirstOperand + instruction.SecondOperand), m_register.REG_X);
					break;
				case CPUAdressingMode.ZeroPageY:
					throw new NotImplementedException("");
				default:
					throw new NotImplementedException("");

			}
		}

		private void HandleADC(CPUInstruction instruction)
		{

			//http://www.obelisk.me.uk/6502/addressing.html
			// http://www.6502.org/tutorials/6502opcodes.html#ADC
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_alu.BinaryAdd(m_register.REG_A, instruction.FirstOperand);
					break;
				case CPUAdressingMode.ZeroPage:
					m_alu.BinaryAdd(m_register.REG_A, m_memory.Read(instruction.FirstOperand));
					break;
				case CPUAdressingMode.Absolute:
					m_alu.BinaryAdd(m_register.REG_A, m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand)));
					break;
				case CPUAdressingMode.AbsoluteX:
					m_alu.BinaryAdd(m_register.REG_A, m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.REG_X)));
					break;
				case CPUAdressingMode.AbsoluteY:
					m_alu.BinaryAdd(m_register.REG_A, m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.REG_Y)));
					break;
				case CPUAdressingMode.ZeroPageX:
					m_alu.BinaryAdd(m_register.REG_A, m_memory.Read((byte)(instruction.FirstOperand + m_register.REG_X)));
					break;
				default:
				throw new Exception("Wrong AdressMode for ADC");
			}

		}

		private void HandleINX(CPUInstruction instruction)
		{
			m_register.REG_X++;
		}

		private void HandleTAX(CPUInstruction instruction)
		{
			m_register.REG_X = m_register.REG_A;
		}

		private void HandleSTA(CPUInstruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_memory.Store(instruction.FirstOperand, m_register.REG_A);
					break;
				case CPUAdressingMode.Absolute:
					m_memory.Store((ushort)(instruction.FirstOperand + instruction.SecondOperand), m_register.REG_A);
					break;
				case CPUAdressingMode.AbsoluteX:
					m_memory.Store((byte)(instruction.FirstOperand + instruction.SecondOperand + m_register.REG_X), m_register.REG_A);
					break;
				case CPUAdressingMode.AbsoluteY:
					m_memory.Store((byte)(instruction.FirstOperand + instruction.SecondOperand + m_register.REG_Y), m_register.REG_A);
					break;
				case CPUAdressingMode.ZeroPage:
					m_memory.Store((byte)(instruction.FirstOperand), m_register.REG_A);
					break;
				default:
						throw new Exception("Wrong AdressMode for STA");
			}
		}

		private void HandleLDA(CPUInstruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_register.Write_REG_A(AdressMode_Immediate(instruction));
					break;
				case CPUAdressingMode.ZeroPage:
					m_register.Write_REG_A(m_memory.Read(AdressMode_ZeroPage(instruction)));
					break;
				case CPUAdressingMode.ZeroPageX:
					m_register.Write_REG_A(m_memory.Read(AdressMode_ZeroPageX(instruction)));
					break;
				case CPUAdressingMode.Absolute:
					m_register.Write_REG_A(m_memory.Read(AdressMode_Absolute(instruction)));
					break;
				case CPUAdressingMode.AbsoluteX:
					m_register.Write_REG_A(m_memory.Read(AdressMode_AbsoluteX(instruction)));
					break;
				case CPUAdressingMode.AbsoluteY:
					m_register.Write_REG_A(m_memory.Read(AdressMode_AbsoluteY(instruction)));
					break;
				default:
					throw new Exception("Wrong AdressMode for LDA");
			}
		}

		//http://www.obelisk.me.uk/6502/addressing.html
		private byte AdressMode_Accumulator(CPUInstruction instruction) => default;
		private byte AdressMode_Immediate(CPUInstruction instruction) => instruction.FirstOperand;
		private byte AdressMode_ZeroPage(CPUInstruction instruction) => (byte)(instruction.FirstOperand);
		private byte AdressMode_ZeroPageX(CPUInstruction instruction) => (byte)(instruction.FirstOperand + m_register.REG_X);
		private byte AdressMode_ZeroPageY(CPUInstruction instruction) => (byte)(instruction.FirstOperand + m_register.REG_Y);

		//private short AdressMode_Relative(CPUInstruction instruction) => ;

		private ushort AdressMode_Absolute(CPUInstruction instruction) => (ushort)(instruction.FirstOperand + instruction.SecondOperand);
		private ushort AdressMode_AbsoluteX(CPUInstruction instruction) => (ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.REG_X);
		private ushort AdressMode_AbsoluteY(CPUInstruction instruction) => (ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.REG_Y);

		//	private byte AdressMode_Indirect(CPUInstruction instruction) => ;

		//	private byte AdressMode_IndexedIndirect(CPUInstruction instruction) => ;

		//	private byte AdressMode_IndirectIndexed(CPUInstruction instruction) => ;

	}
}
