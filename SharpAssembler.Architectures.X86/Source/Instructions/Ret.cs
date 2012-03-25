﻿#region Copyright and License
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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The RET (Near Return from Called Procedure) instruction.
	/// </summary>
	public class Ret : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Ret"/> class.
		/// </summary>
		public Ret()
			: this(null)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Ret"/> class.
		/// </summary>
		/// <param name="popcount">The number of bytes to pop from the stack after returning.</param>
		private Ret(Immediate popcount)
		{
			this.popcount = popcount;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "ret"; }
		}

		private Immediate popcount;
		/// <summary>
		/// Gets the number of bytes to pop from the stack after returning.
		/// </summary>
		/// <value>An <see cref="Immediate"/> value; or <see langword="null"/>.</value>
		public Immediate PopCount
		{
			get { return popcount; }
			set { popcount = value; }
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
			yield return this.popcount;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// RET
			new InstructionVariant(
				new byte[] { 0xC3 },
				new OperandDescriptor(OperandType.None, DataSize.None)),
			// RET imm16
			new InstructionVariant(
				new byte[] { 0xC2 },
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
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