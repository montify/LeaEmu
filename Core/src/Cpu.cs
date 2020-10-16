using System;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
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

		public void Execute(Instruction instruction)
		{
			switch (instruction.OpCode)
			{
				case CPUOpCode.LDA:
					HandleLDA(instruction);
					break;
				case CPUOpCode.STA:
					HandleSTA(instruction);
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
					throw new Exception("Instruction not Implemented");

						
			}
		}

		private void HandleADC(Instruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_alu.BinaryAdd(m_register.REG_A, m_memory.Read(instruction.Operand));
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private void HandleINX(Instruction instruction)
		{
			if(m_register.REG_X < 0xFF)
				throw new OverflowException("");

			m_register.REG_X++;
			m_register.PC += 0x2;
		}

		private void HandleTAX(Instruction instruction)
		{
			m_register.REG_X = m_register.REG_A;
		}

		private void HandleSTA(Instruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_memory.Store(instruction.Operand, m_register.REG_A);
					m_register.PC += 0x2;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private void HandleLDA(Instruction instruction)
		{
			switch (instruction.AdressingMode)
			{
				case CPUAdressingMode.Immediate:
					m_register.Write_REG_A(instruction.Operand);
					m_register.PC += 0x2;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public void Clock_Tick()
		{
		}
	}
}
