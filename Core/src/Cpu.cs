using System;
using System.Timers;

namespace Core
{

	public class Cpu
	{
		public Register m_register { get; private set; }
		public Memory m_memory { get; private set; }
		public Cpu()
		{
			m_register = new Register();
			m_memory = new Memory();
		}


		public void Tick()
		{
			byte instruction = m_memory.Read(m_register.PC);

			switch (instruction)
			{
				case 0xAA:
				break;
				default:
				throw new Exception("Wrong OpCode");
			}

			
		}


		public void Run()
		{
			while(true)
			{
				Tick();
			}
		}
	}
}
