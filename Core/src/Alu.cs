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

			if (sum > 0xFF)
			{
				var bytes = SplitIntoBytes((ushort)sum);
				//TODO: WHat to do with bytes.lower?
				m_register.Write_REG_A(bytes.upper);
				m_register.Write_Carry_Flag(true);

				if (bytes.upper == 0)
					m_register.Write_Zero_Flag(true);
			}
			else
			{
				m_register.Write_REG_A((byte)sum);
				m_register.Write_Carry_Flag(false);  //??
			}
		}

		private (byte lower, byte upper) SplitIntoBytes(ushort value)
		{
			var least = (byte)(value & 0x00FF);
			var most = (byte)(value >> 8);

			return (most, least);
		}

	}
}
