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

namespace SharpAssembler.x86.Operands
{
	partial class RegisterOperand
	{
		/// <summary>
		/// Specifies how the <see cref="RegisterOperand"/> gets encoded.
		/// </summary>
		internal enum OperandEncoding
		{
			/// <summary>
			/// The default encoding of the register.
			/// </summary>
			/// <remarks>
			/// This is used when the operand is part of a 'reg' operand, but encoded
			/// in the ModR/M byte.
			/// </remarks>
			Default,
			/// <summary>
			/// Add the register value to the opcode.
			/// </summary>
			/// <remarks>
			/// This is used when the operand is part of a 'reg' operand, but encoded
			/// in the last opcode byte.
			/// </remarks>
			AddToOpcode,
			/// <summary>
			/// Reg/mem encoding.
			/// </summary>
			/// <remarks>
			/// This is used when the operand is part of a 'reg/mem' operand, encoded
			/// in the ModR/M byte.
			/// </remarks>
			ModRm,
			/// <summary>
			/// The operand is not encoded.
			/// </summary>
			/// <remarks>
			/// This is used for operands which are implicitly part of the instruction.
			/// </remarks>
			Ignore,
		}
	}
}
