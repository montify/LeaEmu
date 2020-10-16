using System;
using System.Text;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public class Register
	{
		public ushort PC = 0x0600;
		public byte SP;
		public byte REG_A;
		public byte REG_X;
		public byte REG_Y;
		public bool CarryFlag;
		public bool ZeroFlag;

		public void Write_REG_A(byte value)
		{
			REG_A = value;
		}

		public void PrintRegister()
		{
			var sb = new StringBuilder();

			Console.Write("REG_A: 		");
			System.Console.Write(REG_A.ToString("x"));
			System.Console.WriteLine();

			Console.Write("IR_X: 		");
			System.Console.Write(REG_X.ToString("x"));
			System.Console.WriteLine();

			Console.Write("IR_Y: 		");
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
	}

}
