using System;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public class Alu
	{
		private readonly Register m_register;

		public Alu(Register register)
		{
			m_register = register;
		}

		public void BinaryAdd(byte left, byte right)
		{
			int sum = right + left;

			if (sum > 0xFF) //Carry Bit ;)
			{
				var bytes = UShortToTwoBytes((ushort)sum);
				//TODO: WHat to do with bytes.lower?
				m_register.Write_REG_A(bytes.upperPart);
				m_register.Set_Carry_Flag(true);

				if (bytes.upperPart == 0)
					m_register.Set_Zero_Flag(true);
			}
			else
			{
				m_register.Write_REG_A((byte)sum);
				m_register.Set_Carry_Flag(false);  //??
			}
		}

		private (byte lowerPart, byte upperPart) UShortToTwoBytes(ushort value)
		{
			return ((byte)(value >> 8) , (byte)(value & 0x00FF));
		}

	}
}
