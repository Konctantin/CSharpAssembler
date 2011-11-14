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
	/// The JMP FAR (Jump Far) instruction.
	/// </summary>
	public class JmpFar : Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="JmpFar"/> class.
		/// </summary>
		/// <param name="target">A far pointer with the new far jump target address and segment.</param>
		public JmpFar(FarPointer target)
			: this((Operand)target)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(target != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JmpFar"/> class.
		/// </summary>
		/// <param name="target">The memory location containing the new far jump target address and segment.</param>
		public JmpFar(EffectiveAddress target)
			: this((Operand)target)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(target != null);
			#endregion
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="JmpFar"/> class.
		/// </summary>
		/// <param name="target">The operand.</param>
		public JmpFar(Operand target)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(target != null);
			Contract.Requires<InvalidCastException>(
					target is EffectiveAddress ||
					target is FarPointer);
			#endregion

			this.target = target;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "jmp"; }
		}

		private Operand target;
		/// <summary>
		/// Gets the target of the instruction.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Target
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is FarPointer ||
					Contract.Result<Operand>() is EffectiveAddress);
				#endregion
				return target;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidCastException>(
					value is FarPointer ||
					value is EffectiveAddress);
				#endregion
				target = value;
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
			yield return this.target;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// JMP pntr16:16
			new InstructionVariant(
				new byte[] { 0xEA },
				new OperandDescriptor(OperandType.FarPointer, DataSize.Bit16)),
			// JMP pntr16:32
			new InstructionVariant(
				new byte[] { 0xEA },
				new OperandDescriptor(OperandType.FarPointer, DataSize.Bit32)),

			// JMP mem16:16
			new InstructionVariant(
				new byte[] { 0xFF }, 5,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit16)),
			// JMP mem16:32
			new InstructionVariant(
				new byte[] { 0xFF }, 5,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit32)),
			// JMP mem16:64
			new InstructionVariant(
				new byte[] { 0xFF }, 5,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit64)),
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
			Contract.Invariant(this.target != null);
			Contract.Invariant(
					this.target is FarPointer ||
					this.target is EffectiveAddress);
		}
		#endregion
	}
}
