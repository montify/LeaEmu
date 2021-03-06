using System;
using MemoryLib;

namespace CpuLib
{
	// http://www.obelisk.me.uk/6502/registers.html
	// https://en.wikibooks.org/wiki/6502_Assembly
	public class Cpu
	{
		public Register m_register { get; private set; }
		public Memory m_memory { get; private set; }
		public Alu m_alu { get; private set; }

		public Cpu(Memory memory)
		{
			m_register = new Register();
			m_memory = memory;
			m_alu = new Alu(m_register);
		}

		public void Execute(CPUInstruction instruction)
		{
			// Set PC at first, because when we jump, we dont need to advance the PC in the current Location
			// so it gets overriden in the JMP branch anyway ;)
			m_register.Write_PC((ushort)(m_register.Read_PC() + instruction.InstructionLenghtInByte));

			switch (instruction.OpCode)
			{
				case CPUOpCode.LDA:
					HandleLDA(instruction);
					break;
				case CPUOpCode.LDX:
					HandleLDX(instruction);
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
				case CPUOpCode.TXA:
					HandleTXA(instruction);
					break;
				case CPUOpCode.INX:
					HandleINX(instruction);
					break;
				case CPUOpCode.ADC:
					HandleADC(instruction);
					break;
				case CPUOpCode.SBC:
					HandleSBC(instruction);
					break;
				case CPUOpCode.PHA:
					HandlePHA(instruction);
					break;
				case CPUOpCode.PLA:
					HandlePLA(instruction);
					break;
				case CPUOpCode.SEC:
					m_register.Set_Carry_Flag(true);
					break;
				case CPUOpCode.CLC:
					m_register.Set_Carry_Flag(false);
					break;
				case CPUOpCode.JMP:
					m_register.Write_PC((ushort)(instruction.FirstOperand + instruction.SecondOperand));
					break;
				case CPUOpCode.BRK:
					throw new NotImplementedException();
				default:
					throw new Exception($"Bad Instruction or not Implemented");
			}
		}

		private void HandleTXA(CPUInstruction instruction)
		{
			m_register.Write_REG_A(m_register.Read_REG_X());
		}

		private void HandleSBC(CPUInstruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_alu.BinarySub(m_register.Read_REG_A(), instruction.FirstOperand);
					break;
				case CPUAdressingMode.ZeroPage:
					m_alu.BinarySub(m_register.Read_REG_A(), m_memory.Read(instruction.FirstOperand));
					break;
				case CPUAdressingMode.Absolute:
					m_alu.BinarySub(m_register.Read_REG_A(), m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand)));
					break;
				case CPUAdressingMode.AbsoluteX:
					m_alu.BinarySub(m_register.Read_REG_A(), m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.Read_REG_X())));
					break;
				case CPUAdressingMode.AbsoluteY:
					m_alu.BinarySub(m_register.Read_REG_A(), m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.Read_REG_Y())));
					break;
				case CPUAdressingMode.ZeroPageX:
					m_alu.BinarySub(m_register.Read_REG_A(), m_memory.Read((byte)(instruction.FirstOperand + m_register.Read_REG_X())));
					break;
				default:
					throw new Exception("Wrong AdressMode for ADC");
			}
		}

		private void HandlePLA(CPUInstruction instruction)
		{
			var stackValue = PopFromStack();
			m_register.Write_REG_A(stackValue);
		}

		private void HandlePHA(CPUInstruction instruction)
		{
			PushToStack(m_register.Read_REG_A());
		}

		private void HandleLDX(CPUInstruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_register.Write_REG_X(AdressMode_Immediate(instruction));
					break;
				case CPUAdressingMode.ZeroPage:
					m_register.Write_REG_X(m_memory.Read(AdressMode_ZeroPage(instruction)));
					break;
				case CPUAdressingMode.ZeroPageY:
					m_register.Write_REG_X(m_memory.Read(AdressMode_ZeroPageY(instruction)));
					break;
				case CPUAdressingMode.Absolute:
					m_register.Write_REG_X(m_memory.Read(AdressMode_Absolute(instruction)));
					break;
				case CPUAdressingMode.AbsoluteY:
					m_register.Write_REG_X(m_memory.Read(AdressMode_AbsoluteY(instruction)));
					break;
				default:
					throw new Exception("Wrong AdressMode for LDA");
			}
		}

		private void HandleSTX(CPUInstruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.ZeroPage:
					m_memory.Write(AdressMode_ZeroPage(instruction), m_register.Read_REG_X());
					break;
				case CPUAdressingMode.ZeroPageY:
					m_memory.Write(AdressMode_ZeroPageY(instruction), m_register.Read_REG_X());
					break;
				case CPUAdressingMode.Absolute:
					m_memory.Write(AdressMode_Absolute(instruction), m_register.Read_REG_X());
					break;
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
					m_alu.BinaryAdd(m_register.Read_REG_A(), instruction.FirstOperand);
					break;
				case CPUAdressingMode.ZeroPage:
					m_alu.BinaryAdd(m_register.Read_REG_A(), m_memory.Read(instruction.FirstOperand));
					break;
				case CPUAdressingMode.Absolute:
					m_alu.BinaryAdd(m_register.Read_REG_A(), m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand)));
					break;
				case CPUAdressingMode.AbsoluteX:
					m_alu.BinaryAdd(m_register.Read_REG_A(), m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.Read_REG_X())));
					break;
				case CPUAdressingMode.AbsoluteY:
					m_alu.BinaryAdd(m_register.Read_REG_A(), m_memory.Read((ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.Read_REG_Y())));
					break;
				case CPUAdressingMode.ZeroPageX:
					m_alu.BinaryAdd(m_register.Read_REG_A(), m_memory.Read((byte)(instruction.FirstOperand + m_register.Read_REG_X())));
					break;
				default:
					throw new Exception("Wrong AdressMode for ADC");
			}
		}

		private void HandleINX(CPUInstruction instruction)
		{
			m_register.Increment_REG_X();
		}

		private void HandleTAX(CPUInstruction instruction)
		{
			m_register.Write_REG_X(m_register.Read_REG_A());
		}

		private void HandleSTA(CPUInstruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_memory.Write(instruction.FirstOperand, m_register.Read_REG_A());
					break;
				case CPUAdressingMode.Absolute:
					m_memory.Write(AdressMode_Absolute(instruction), m_register.Read_REG_A());
					break;
				case CPUAdressingMode.AbsoluteX:
					m_memory.Write(AdressMode_AbsoluteX(instruction), m_register.Read_REG_A());
					break;
				case CPUAdressingMode.AbsoluteY:
					m_memory.Write(AdressMode_AbsoluteY(instruction), m_register.Read_REG_A());
					break;
				case CPUAdressingMode.ZeroPage:
					m_memory.Write((byte)(instruction.FirstOperand), m_register.Read_REG_A());
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

		// NOTE: The address calculation wraps around if the sum of the base address and the register exceed $FF
		private byte AdressMode_ZeroPageX(CPUInstruction instruction) => (byte)(instruction.FirstOperand + m_register.Read_REG_X());
		private byte AdressMode_ZeroPageY(CPUInstruction instruction) => (byte)(instruction.FirstOperand + m_register.Read_REG_Y());

		//private sbyte AdressMode_Relative(CPUInstruction instruction) => (sbyte)((~instruction.FirstOperand) + m_register.Read_PC());

		private ushort AdressMode_Absolute(CPUInstruction instruction) => (ushort)(instruction.FirstOperand + instruction.SecondOperand);
		private ushort AdressMode_AbsoluteX(CPUInstruction instruction) => (ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.Read_REG_X());
		private ushort AdressMode_AbsoluteY(CPUInstruction instruction) => (ushort)(instruction.FirstOperand + instruction.SecondOperand + m_register.Read_REG_Y());

		//	private byte AdressMode_Indirect(CPUInstruction instruction) => ;

		//	private byte AdressMode_IndexedIndirect(CPUInstruction instruction) => ;

		//	private byte AdressMode_IndirectIndexed(CPUInstruction instruction) => ;

		private void PushToStack(byte value)
		{
			var stackPtr = m_register.Read_SP();
			m_memory.Write(stackPtr, value);
			m_register.Decrement_SP(0x01);
		}
		
		private byte PopFromStack()
		{
			m_register.Increment_SP(0x01);
			var stackPtr = m_register.Read_SP();
			var stackValue = m_memory.Read(stackPtr);
			return stackValue;
		}

	}
}
