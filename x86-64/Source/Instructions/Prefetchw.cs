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
using System.Diagnostics.Contracts;
using SharpAssembler.Core;
using SharpAssembler.x86.Operands;

namespace SharpAssembler.x86.Instructions
{
	/// <summary>
	/// The PREFETCHW (Prefetch L1 Data-Cache Line) instruction.
	/// </summary>
	public class Prefetchw : Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Prefetchw"/> class.
		/// </summary>
		/// <param name="address">The address to prefetch.</param>
		public Prefetchw(EffectiveAddress address)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(address != null);
			#endregion

			this.address = address;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "prefetchw"; }
		}

		private EffectiveAddress address;
		/// <summary>
		/// Gets the address to prefetch.
		/// </summary>
		/// <value>A <see cref="EffectiveAddress"/>.</value>
		public EffectiveAddress Address
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<EffectiveAddress>() != null);
				#endregion
				return address;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				address = value;
			}
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
			yield return this.address;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// PREFETCHW mem8
			new InstructionVariant(
				new byte[] { 0x0F, 0x0D }, 1,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit8)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/>
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
			Contract.Invariant(this.address != null);
		}
		#endregion
	}
}



