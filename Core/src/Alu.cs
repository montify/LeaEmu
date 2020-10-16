using System;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public class Alu
	{
		private readonly Register m_register;

		public Alu(Register register)
		{
			//TEST
			this.m_register = register;
		}

		public void BinaryAdd(byte left, byte right)
		{
			int sum = left + right;

			if (sum > 0xFF) // if greater as one byte 
			{
				byte va = (byte)(sum & 0x0000FFFF); // get the most significant bit 
				m_register.Write_REG_A(va);
				m_register.CarryFlag = true;
			}
			else
			{
				m_register.Write_REG_A((byte)sum);
			}
			
		}

	}
}
