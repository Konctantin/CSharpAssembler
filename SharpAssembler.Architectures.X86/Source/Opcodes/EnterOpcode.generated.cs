//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011-2012 Daniël Pelsmaeker
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
using System.Collections.Generic;
using System.Linq;
using SharpAssembler.Architectures.X86.Opcodes;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Opcodes
{
	/// <summary>
	/// The ENTER (Create Procedure Stack Frame) instruction opcode.
	/// </summary>
	public class EnterOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="EnterOpcode"/> class.
		/// </summary>
		public EnterOpcode()
			: base("enter", GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// ENTER imm16, imm8
				new X86OpcodeVariant(
					new byte[] { 0xC8 }, 6,
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8, OperandEncoding.ExtraImmediate)),
			};
		}
	}
}

namespace SharpAssembler.Architectures.X86
{
	partial class Instr
	{
		/// <summary>
		/// Creates a new ENTER (Create Procedure Stack Frame) instruction.
		/// </summary>
		/// <param name="size">An immediate value.</param>
		/// <param name="nestingLevel">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Enter(short size, byte nestingLevel)
		{ return X86Opcode.Enter.CreateInstruction(new Immediate(size, DataSize.Bit16), new Immediate(nestingLevel, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new ENTER (Create Procedure Stack Frame) instruction.
		/// </summary>
		/// <param name="size">An immediate value.</param>
		/// <param name="nestingLevel">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Enter(short size, sbyte nestingLevel)
		{ return X86Opcode.Enter.CreateInstruction(new Immediate(size, DataSize.Bit16), new Immediate(nestingLevel, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new ENTER (Create Procedure Stack Frame) instruction.
		/// </summary>
		/// <param name="size">An immediate value.</param>
		/// <param name="nestingLevel">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Enter(ushort size, byte nestingLevel)
		{ return X86Opcode.Enter.CreateInstruction(new Immediate(size, DataSize.Bit16), new Immediate(nestingLevel, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new ENTER (Create Procedure Stack Frame) instruction.
		/// </summary>
		/// <param name="size">An immediate value.</param>
		/// <param name="nestingLevel">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Enter(ushort size, sbyte nestingLevel)
		{ return X86Opcode.Enter.CreateInstruction(new Immediate(size, DataSize.Bit16), new Immediate(nestingLevel, DataSize.Bit8)); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The ENTER (Create Procedure Stack Frame) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Enter = new EnterOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
