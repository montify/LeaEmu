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
			var result = left + right + Convert.ToByte(m_register.Read_Carry_Flag());

			if (result > 0xFF)
				m_register.Set_Carry_Flag(true);

			result = (byte)(result & 0xff);
			m_register.Set_Zero_Flag(!Convert.ToBoolean(result));

			m_register.Write_REG_A((byte)result);
		}

		internal void BinarySub(byte left, byte right)
		{
			var result = left + (right ^ 0xFF) + Convert.ToByte(m_register.Read_Carry_Flag());

			if (result > 0xFF)
				m_register.Set_Carry_Flag(true);

			result = (byte)(result & 0xFF);
			m_register.Set_Zero_Flag(!Convert.ToBoolean(result));

			m_register.Write_REG_A((byte)result);
		}

		private (byte lowerPart, byte upperPart) UShortToTwoBytes(ushort value)
		{
			return ((byte)(value >> 8), (byte)(value & 0x00FF));
		}
	}
}
