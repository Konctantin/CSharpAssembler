﻿#region Copyright and License
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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The INS (Input String) instruction.
	/// </summary>
	public class Ins : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Ins"/> class.
		/// </summary>
		/// <param name="size">The size of the data to compare.</param>
		public Ins(DataSize size)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size == DataSize.Bit8 || size == DataSize.Bit16 || size == DataSize.Bit32,
				"The size must be either 8, 16 or 32-bits.");
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
						return "insb";
					case DataSize.Bit16:
						return "insw";
					case DataSize.Bit32:
						return "insd";
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
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// INSB
			new X86OpcodeVariant(
				new byte[] { 0x6C }, DataSize.Bit8),
			// INSW
			new X86OpcodeVariant(
				new byte[] { 0x6D }, DataSize.Bit16),
			// INSD
			new X86OpcodeVariant(
				new byte[] { 0x6D }, DataSize.Bit32),
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86OpcodeVariant"/>
		/// objects.</returns>
		internal override X86OpcodeVariant[] GetVariantList()
		{
			return variants;
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
