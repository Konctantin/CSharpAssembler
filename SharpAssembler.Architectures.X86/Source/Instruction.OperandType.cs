#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011 Daniël Pelsmaeker
 * 
 * This file is part of SharpAssembler.
 * 
 * SharpAssembler is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpAssembler is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SharpAssembler.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion
using System;

namespace SharpAssembler.Architectures.X86
{
	partial class X86Instruction
	{
		/// <summary>
		/// Specifies the type of operand.
		/// </summary>
		[Flags]
		internal enum OperandType
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
}
