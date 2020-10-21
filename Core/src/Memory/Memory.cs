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

		public void DebugMemory(ushort start, ushort end)
		{
			if (start < 0 && end > m_memory.Length)
				throw new OutOfMemoryException("");

			StringBuilder stringbuilder = new StringBuilder();

			stringbuilder.Append($@"  {0}	 {1}	 {2}	 {3}	 {4}	 {5}	 {6}	 {7}	 {8}	 {9}");
			stringbuilder.Append($"\n");


			int count = 0;
			for (int i = start; i < end + 1; i++)
			{
				count++;

				stringbuilder.Append("0x");
				stringbuilder.Append(m_memory[i].ToString("X2"));
				stringbuilder.Append("	");

				if (count % 10 == 0)
					stringbuilder.Append("\n");
			}

			System.Console.WriteLine(stringbuilder.ToString());
		}

		public void DebugStackRegion()
		{
			DebugMemory(0x0100, 0x01FF);
		}
	}
}
