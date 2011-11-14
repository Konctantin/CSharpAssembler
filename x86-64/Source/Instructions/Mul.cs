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
using SharpAssembler.x86.Operands;

namespace SharpAssembler.x86.Instructions
{
	/// <summary>
	/// The MUL (Unsigned Multiply) instruction.
	/// </summary>
	public class Mul : Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Mul"/> class.
		/// </summary>
		/// <param name="multiplier">The multiplier.</param>
		public Mul(EffectiveAddress multiplier)
			: this((Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mul"/> class.
		/// </summary>
		/// <param name="multiplier">The multiplier.</param>
		public Mul(RegisterOperand multiplier)
			: this((Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mul"/> class.
		/// </summary>
		/// <param name="multiplier">The multiplier.</param>
		private Mul(Operand multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			Contract.Requires<InvalidCastException>(
					multiplier is EffectiveAddress ||
					multiplier is RegisterOperand);
			#endregion

			this.multiplier = multiplier;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "mul"; }
		}

		private Operand multiplier;
		/// <summary>
		/// Gets the multiplier.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Multiplier
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return multiplier;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidCastException>(
					value is EffectiveAddress ||
					value is RegisterOperand);
				#endregion
				multiplier = value;
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
			yield return this.multiplier;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// MUL reg/mem8
			new InstructionVariant(
				new byte[] { 0xF6 }, 4,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// MUL reg/mem16
			new InstructionVariant(
				new byte[] { 0xF7 }, 4,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// MUL reg/mem32
			new InstructionVariant(
				new byte[] { 0xF7 }, 4,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// MUL reg/mem64
			new InstructionVariant(
				new byte[] { 0xF7 }, 4,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
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
			Contract.Invariant(this.multiplier != null);
			Contract.Invariant(
					this.multiplier is EffectiveAddress ||
					this.multiplier is RegisterOperand);
		}
		#endregion
	}
}
