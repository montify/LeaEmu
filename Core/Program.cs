using System;
using System.Timers;

namespace Core
{

	class Program
	{
		static void Main(string[] args)
		{

			Console.Read();
		}

		static (byte upper, byte lower) ExtractBytes(ushort value)
		{
			byte upper = (byte)(value >> 8);
			byte lower = (byte)(value & 0xFF);

			return (upper, lower);
		}
	}


}
