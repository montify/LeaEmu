using System;
using System.Text;

namespace Core
{
	// http://www.obelisk.me.uk/6502/architecture.html
	public class Memory
	{
		public byte[] m_memory = new byte[65536];

		public Memory()
		{

		}

		public void ClearAll()
		{
			m_memory = new byte[65536];

		}

		public void Store(ushort adress, byte value)
		{
			if (adress > m_memory.Length)
				throw new Exception("Out of Bounds");

			m_memory[adress] = value;
		}

		public byte Read(ushort adress)
		{
			return m_memory[adress];
		}


		public void DebugShowMemory(ushort start, ushort end, int bytesPerRow)
		{
			if (start < 0 && end > m_memory.Length)
				throw new OutOfMemoryException("");

			// Row Number
			for (int i = 0; i < bytesPerRow; i++)
			{
				if (i == 0)
					Console.Write("       ");

				Console.Write(i + "    ");
			}

			System.Console.WriteLine();

			//Column Number
			for (int i = start; i < end + 1; i++)
			{
				if (i > start && i % bytesPerRow == 0)
					System.Console.Write('\n');

				if (i % bytesPerRow == 0)
					if (i < 100)
						System.Console.Write(i + ":   ");
					else
						System.Console.Write(i + ":  ");

				if (m_memory[i] != 0x00)
					Console.ForegroundColor = ConsoleColor.Green;

				System.Console.Write("0x");
				System.Console.Write(m_memory[i].ToString("X2") + " ");

				Console.ResetColor();
			}
		}
	}
}