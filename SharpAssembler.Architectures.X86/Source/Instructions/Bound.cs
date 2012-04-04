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
using System.Diagnostics.Contracts;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The BOUND (Check Array Bound) instruction.
	/// </summary>
	public class Bound : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Bound"/> class.
		/// </summary>
		/// <param name="index">The array index to check.</param>
		/// <param name="bounds">Memory address of two (double)words specifying the lower and upper limits of the
		/// array.</param>
		public Bound(RegisterOperand index, EffectiveAddress bounds)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(index != null);
			Contract.Requires<ArgumentNullException>(bounds != null);
			#endregion

			this.index = index;
			this.bounds = bounds;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "bound"; }
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

		private RegisterOperand index;
		/// <summary>
		/// Gets the array index to check.
		/// </summary>
		/// <value>A <see cref="RegisterOperand"/> operand.</value>
		public RegisterOperand Index
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<RegisterOperand>() != null);
				#endregion
				return index;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				index = value;
			}
#endif
		}

		private EffectiveAddress bounds;
		/// <summary>
		/// Gets the array's limits.
		/// </summary>
		/// <value>An <see cref="EffectiveAddress"/> operand.</value>
		public EffectiveAddress Bounds
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<EffectiveAddress>() != null);
				#endregion
				return bounds;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				bounds = value;
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
			yield return this.index;
			yield return this.bounds;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86Instruction.X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// BOUND reg16, mem16&mem16
			new X86OpcodeVariant(
				new byte[] { 0x62 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit16)),
			// BOUND reg32, mem32&mem32
			new X86OpcodeVariant(
				new byte[] { 0x62 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit32)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86Instruction.X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86Instruction.X86OpcodeVariant"/>
		/// objects.</returns>
		internal override X86OpcodeVariant[] GetVariantList()
		{ return variants; }
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.index != null);
			Contract.Invariant(this.bounds != null);
		}
		#endregion
	}
}
