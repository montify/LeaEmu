using System;
using System.Text;

namespace Core
{
	// http://www.obelisk.me.uk/6502/architecture.html
	public class Memory
	{
		private byte[] m_memory;
		private ushort m_Size;

		public Memory(ushort size)
		{
			m_memory = new byte[size];
			m_Size = size;
		}

		public void ClearAll()
		{
			m_memory = new byte[m_Size];
		}

		public void Write(ushort adress, byte value)
		{
			if (adress > m_memory.Length)
				throw new Exception("Out of Bounds");

			m_memory[adress] = value;
		}

		public byte Read(ushort adress)
		{
			if (adress >= m_memory.Length)
				throw new Exception("Out of Bounds");

			return m_memory[adress];
		}

		public void DebugMemory(ushort start, ushort end, int lineBreak)
		{
			if (start < 0 && end > m_memory.Length)
				throw new OutOfMemoryException("");

			// Row Number
			for (int i = 0; i < lineBreak; i++)
			{
				if (i == 0)
					Console.Write("       ");

				Console.Write(i.ToString("X2") + "   ");
			}

			System.Console.WriteLine();



			//Column Number
			for (int i = start; i < end + 1; i++)
			{
				if (i > start && i % lineBreak == 0)
					System.Console.Write('\n');

				if (i % lineBreak == 0)
					if (i < 100)
						System.Console.Write(i.ToString("X2") + ":   ");
					else
						System.Console.Write(i.ToString("X2") + ":  ");

				if (i >= 0x0100 && i <= 0x01FF) //Stack Region
					Console.ForegroundColor = ConsoleColor.Blue;

			//	if (m_memory[i] != 0x0000) //Color every byte that are not 0x00
			//		Console.ForegroundColor = ConsoleColor.Green;

				System.Console.Write("0x");
				System.Console.Write(m_memory[i].ToString("X2") + " ");

				Console.ResetColor();


			}
		}

		public void DebugStackRegion()
		{
			DebugMemory(0x0100, 0x01FF, 10);
		}
	}
}
