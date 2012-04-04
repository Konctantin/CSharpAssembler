using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Specifies the type of operand.
	/// </summary>
	[Flags]
	public enum OperandType
	{
		/// <summary>
		/// No operand.
		/// </summary>
		None = 0,
		/// <summary>
		/// A register.
		/// </summary>
		RegisterOperand = 0x0001,
		/// <summary>
		/// A fixed register.
		/// </summary>
		FixedRegister = 0x0002,
		/// <summary>
		/// An immediate value (imm).
		/// </summary>
		/// <remarks>
		/// In the AMD64 Architecture Programmer's Manual vol. 3,
		/// this type of operand is denoted with <c>imm</c>.
		/// </remarks>
		Immediate = 0x0004,
		/// <summary>
		/// A memory operand (mem).
		/// </summary>
		/// <remarks>
		/// In the AMD64 Architecture Programmer's Manual vol. 3,
		/// this type of operand is denoted with <c>mem</c>.
		/// </remarks>
		MemoryOperand = 0x0008,
		/// <summary>
		/// A memory offset (moffset).
		/// </summary>
		MemoryOffset = 0x0010,
		/// <summary>
		/// A far pointer (pntr).
		/// </summary>
		FarPointer = 0x0020,
		/// <summary>
		/// A register or memory operand (reg/mem).
		/// </summary>
		RegisterOrMemoryOperand = RegisterOperand | MemoryOperand,
		/// <summary>
		/// An offset (reloff) relative to the instruction pointer.
		/// </summary>
		RelativeOffset = 0x0040,
		// TODO: Add the rest.

	}
}
