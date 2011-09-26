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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpAssembler.Core;
using SharpAssembler.x86.Operands;

namespace SharpAssembler.x86.Instructions
{
	/// <summary>
	/// The AAM (ASCII Adjust After Multiply) instruction.
	/// </summary>
	public class Aam : Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Aam"/> class.
		/// </summary>
		public Aam()
			: this(null)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Aam"/> class.
		/// </summary>
		/// <param name="base">The base of the operation.</param>
		public Aam(Immediate @base)
		{
			this.@base = @base;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "aam"; }
		}

		/// <summary>
		/// Gets whether this instruction is valid in 64-bit mode.
		/// </summary>
		/// <value><see langword="true"/> when the instruction is valid in 64-bit mode;
		/// otherwise, <see langword="false"/>.</value>
		public override bool ValidIn64BitMode
		{
			get { return false; }
		}

		private Immediate @base;
		/// <summary>
		/// Gets the base of the operation.
		/// </summary>
		/// <value>An <see cref="Immediate"/> operand;
		/// or <see langword="null"/> to use base 10.</value>
		public Immediate Base
		{
			get { return @base; }
#if OPERAND_SET
			set { @base = value; }
#endif
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enumerates an ordered list of operands used by this instruction.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Operand"/> objects.</returns>
		public override IEnumerable<Operand> GetOperands()
		{
			// The order is important here!
			yield return this.@base;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// AAD
			new InstructionVariant(
				new byte[] { 0xD4, 0x0A }),
			// (None) imm8
			new InstructionVariant(
				new byte[] { 0xD4 },
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="Instruction.InstructionVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="Instruction.InstructionVariant"/>
		/// objects.</returns>
		internal override InstructionVariant[] GetVariantList()
		{ return variants; }
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			
		}
		#endregion
	}
}
