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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The MOVS (Move String) instruction.
	/// </summary>
	public class Movs : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Movs"/> class.
		/// </summary>
		/// <param name="size">The size of the data to move.</param>
		public Movs(DataSize size)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size == DataSize.Bit8 || size == DataSize.Bit16 ||
				size == DataSize.Bit32 || size == DataSize.Bit64,
				"The size must be either 8, 16, 32 or 64-bits.");
			#endregion

			this.OperandSize = size;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get
			{
				switch (OperandSize)
				{
					case DataSize.Bit8:
						return "movsb";
					case DataSize.Bit16:
						return "movsw";
					case DataSize.Bit32:
						return "movsd";
					case DataSize.Bit64:
						return "movsq";
					default:
						throw new Exception();
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enumerates an ordered list of operands used by this instruction.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Operand"/> objects.</returns>
		public override IEnumerable<Operand> GetOperands()
		{
			yield break;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// LODSB
			new InstructionVariant(
				new byte[] { 0xA4 }, DataSize.Bit8),
			// LODSW
			new InstructionVariant(
				new byte[] { 0xA5 }, DataSize.Bit16),
			// LODSD
			new InstructionVariant(
				new byte[] { 0xA5 }, DataSize.Bit32),
			// LODSQ
			new InstructionVariant(
				new byte[] { 0xA5 }, DataSize.Bit64),
		};

		/// <summary>
		/// Returns an array containing the <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects.</returns>
		internal override InstructionVariant[] GetVariantList()
		{
			return variants;
		}

		/// <summary>
		/// Initializes a static instance of the <see cref="Movs"/> class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static Movs()
		{
			variants = new InstructionVariant[2];
			int index = 0;

			// LODSB
			variants[index++] = new InstructionVariant(
				new byte[] { 0xA4 }, DataSize.Bit8);
			// LODSW
			variants[index++] = new InstructionVariant(
				new byte[] { 0xA5 }, DataSize.Bit16);
			// LODSD
			variants[index++] = new InstructionVariant(
				new byte[] { 0xA5 }, DataSize.Bit32);
			// LODSQ
			variants[index++] = new InstructionVariant(
				new byte[] { 0xA5 }, DataSize.Bit64);
		}
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
