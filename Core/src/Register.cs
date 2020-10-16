using System;
using System.Text;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public class Register
	{
		private ushort PC = 0x0600;
		private byte SP;
		private byte REG_A;
		private byte REG_X;
		private byte REG_Y;
		private bool CarryFlag;
		private bool ZeroFlag;

		public void PrintRegister()
		{
			var sb = new StringBuilder();

			Console.Write("A: 		");
			System.Console.Write(REG_A.ToString("x"));
			System.Console.WriteLine();

			Console.Write("X: 		");
			System.Console.Write(REG_X.ToString("x"));
			System.Console.WriteLine();

			Console.Write("Y: 		");
			System.Console.Write(REG_Y.ToString("x"));
			System.Console.WriteLine();

			Console.Write("PC: 		");
			System.Console.Write(PC.ToString("x"));
			System.Console.WriteLine();

			Console.Write("SP: 		");
			System.Console.Write(SP.ToString("x"));
			System.Console.WriteLine();

			Console.Write("CarryFLAG: 	");
			System.Console.Write(CarryFlag);
			System.Console.WriteLine();

			Console.Write("ZeroFLAG: 	");
			System.Console.Write(ZeroFlag);
			System.Console.WriteLine();
		}

		public void Write_REG_A(byte value) => REG_A = value;
		public void Write_REG_X(byte value) => REG_X = value;
		public void Write_REG_y(byte value) => REG_Y = value;

		public bool Write_Carry_Flag(bool value) => CarryFlag = value;
		public bool Write_Zero_Flag(bool value) => ZeroFlag = value;
		public byte Read_REG_A() => REG_A;
		public byte Read_REG_X() => REG_X;
		public byte Read_REG_Y() => REG_Y;
		public bool Read_Carry_Flag() => CarryFlag;
		public bool Read_Zero_Flag() => ZeroFlag;

		public void Increment_REG_A() => REG_A++;
		public void Increment_REG_X() => REG_X++;
		public void Increment_REG_Y() => REG_Y++;
	}

}
