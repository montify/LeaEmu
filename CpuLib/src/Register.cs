using System;
using System.Text;

namespace CpuLib
{
	// http://www.obelisk.me.uk/6502/registers.html
	public class Register
	{
		private ushort PC = 0x0000;
		private byte SP = 0xFF; //Set to 0xFF because the stack grows down towards 0x0100, so SP range is 0xFF 0x00
		private byte REG_A;
		private byte REG_X;
		private byte REG_Y;
		private bool CarryFlag;
		private bool ZeroFlag;
		private ushort m_StackOffset = 0x0100; //Because Stack starts at 0x0100 we must add this Value to SP because SP is a byte .

		public ushort Read_SP() => (ushort)(SP + m_StackOffset);
		public void Increment_SP(byte size) //Stack grows down, from 0x01FF to 0x0100
		{
			SP += size;
		}
		public void Decrement_SP(byte size) //Stack grows down, from 0x01FF to 0x0100
		{
			SP -= size;
		}

		public bool Read_Carry_Flag() => CarryFlag;
		public bool Set_Carry_Flag(bool value) => CarryFlag = value;

		public bool Read_Zero_Flag() => ZeroFlag;
		public bool Set_Zero_Flag(bool value) => ZeroFlag = value;

		public void Write_PC(ushort value) { PC = value; }
		public ushort Read_PC() => PC;

		public byte Read_REG_A() => REG_A;
		public byte Read_REG_X() => REG_X;
		public byte Read_REG_Y() => REG_Y;
		public void Write_REG_A(byte value) => REG_A = value;
		public void Write_REG_X(byte value) => REG_X = value;
		public void Write_REG_y(byte value) => REG_Y = value;

		public void Increment_REG_A() => REG_A++;
		public void Increment_REG_X() => REG_X++;
		public void Increment_REG_Y() => REG_Y++;

	}
}
