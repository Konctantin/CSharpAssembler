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
	/// The NEG (Two's Complement Negation) instruction.
	/// </summary>
	public class Neg : Instruction, ILockInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Neg"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Neg(RegisterOperand value)
			: this((Operand)value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Neg"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Neg(EffectiveAddress value)
			: this((Operand)value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			#endregion
		}



		/// <summary>
		/// Initializes a new instance of the <see cref="Neg"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		private Neg(Operand value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<InvalidCastException>(
					value is EffectiveAddress ||
					value is RegisterOperand);
			#endregion

			this.value = value;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "neg"; }
		}

		private Operand value;
		/// <summary>
		/// Gets the value used by the instruction.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Value
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return value;
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
				this.value = value;
			}
#endif
		}

		private bool lockInstruction = false;
		/// <summary>
		/// Gets or sets whether the lock prefix is used.
		/// </summary>
		/// <value><see langword="true"/> to enable the lock prefix; otherwise, <see langword="false"/>.
		/// The default is <see langword="false"/>.</value>
		/// <remarks>
		/// When this property is set to <see langword="true"/>, the lock signal is asserted before accessing the
		/// specified memory location. When the lock signal has already been asserted, the instruction must wait for it
		/// to be released. Instructions without the lock prefix do not check the lock signal, and will be executed
		/// even when the lock signal is asserted by some other instruction.
		/// </remarks>
		public bool Lock
		{
			get { return lockInstruction; }
			set { lockInstruction = value; }
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
			yield return this.value;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// NEG reg/mem8
			new InstructionVariant(
				new byte[] { 0xF6 }, 3,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// NEG reg/mem16
			new InstructionVariant(
				new byte[] { 0xF7 }, 3,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// NEG reg/mem32
			new InstructionVariant(
				new byte[] { 0xF7 }, 3,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// NEG reg/mem64
			new InstructionVariant(
				new byte[] { 0xF7 }, 3,
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
			Contract.Invariant(this.value != null);
			Contract.Invariant(
					this.value is EffectiveAddress ||
					this.value is RegisterOperand);
		}
		#endregion
	}
}
