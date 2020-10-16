using System;

namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public enum CPUAdressingMode
	{
		Implicit,
		Accumulator,
		Immediate,
		ZeroPage,
		ZeroPageX,
		ZeroPageY,
		Relative,
		Absolute,
		AbsoluteX,
		AbsoluteY,
		Indirect,
		IndexedIndirect,
		IndirectIndexed
	}

}
