namespace Core
{
	// http://www.obelisk.me.uk/6502/registers.html
	public enum CPUOpCode
	{
		//Load/Store Operations
		LDA,
		LDX,
		LDY,
		STA,
		STX,
		STY,

		//Register Transfers
		TAX,
		TAY,
		TXA,
		TYA,

		//Stack Operations
		TSX,
		TXS,
		PHA,
		PHP,
		PLA,
		PLP,

		//Logical
		AND,
		EOR,
		ORA,
		BIT,

		//Arithmetic
		ADC,
		SBC,
		CMP,
		CPX,
		CPY,

		//Increments & Decrements
		INC,
		INX,
		INY,
		DEV,
		DEX,
		DEY,

		//Shifts
		ASL,
		LSR,
		ROL,
		ROR,

		//Jumps & Calls
		JMP,
		JSR,
		RTS,

		//Branches
		BCC,
		BCS,
		BEQ,
		BMI,
		BNE,
		BPL,
		BVC,
		BVS,

		//Status Flag Changes
		CLC,
		CLD,
		CLI,
		CLV,
		SEC,
		SED,
		SEI,

		//System Functions
		BRK,
		NOP,
		RTI
	}
}
