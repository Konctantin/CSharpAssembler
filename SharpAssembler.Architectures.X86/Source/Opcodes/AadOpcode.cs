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
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Opcodes
{
	/// <summary>
	/// The AAD (ASCII Adjust Before Division) instruction opcode.
	/// </summary>
	/// <remarks>
	/// Instructions with this opcode expect one operand that has the following semantics:
	/// <list type="table">
	/// <listheader><term>Index</term><description>Semantics</description></listheader>
	/// <item><term>0</term><description>Base</description></item>
	/// </list>
	/// </remarks>
	public class AadOpcode : X86Opcode
	{
		/// <inheritdoc />
		public override bool IsValidIn64BitMode
		{
			get { return false; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AadOpcode"/> class.
		/// </summary>
		public AadOpcode()
			: base("aad", 1, GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion
		
		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// AAD
				new X86OpcodeVariant(
					new byte[] { 0xD5, 0x0A }),
				// (None) imm8
				new X86OpcodeVariant(
					new byte[] { 0xD5 },
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			};
		}
	}
}

namespace SharpAssembler.Architectures.X86
{
	partial class Instr
	{
		/// <summary>
		/// Creates a new AAD (ASCII Adjust Before Division) instruction
		/// with base 10.
		/// </summary>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Aad()
		{ return X86Opcode.Aad.CreateInstruction(); }

		/// <summary>
		/// Creates a new AAD (ASCII Adjust Before Division) instruction
		/// with the specified base.
		/// </summary>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Aad(byte @base)
		{ return X86Opcode.Aad.CreateInstruction(new Immediate(@base)); }
	}
}
