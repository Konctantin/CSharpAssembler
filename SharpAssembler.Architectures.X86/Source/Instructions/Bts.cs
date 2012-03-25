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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The BTS (Bit Test and Set) instruction.
	/// </summary>
	public class Bts : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Bts"/> class.
		/// </summary>
		/// <param name="subject">The register operand whose bit is copied and set.</param>
		/// <param name="bitIndex">The index of the bit to copy.</param>
		public Bts(RegisterOperand subject, RegisterOperand bitIndex)
			: this((Operand)subject, (Operand)bitIndex)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<ArgumentNullException>(bitIndex != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bts"/> class.
		/// </summary>
		/// <param name="subject">The memory operand whose bit is copied and set.</param>
		/// <param name="bitIndex">The index of the bit to copy.</param>
		public Bts(EffectiveAddress subject, RegisterOperand bitIndex)
			: this((Operand)subject, (Operand)bitIndex)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<ArgumentNullException>(bitIndex != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bts"/> class.
		/// </summary>
		/// <param name="subject">The register operand whose bit is copied and set.</param>
		/// <param name="bitIndex">The index of the bit to copy.</param>
		public Bts(RegisterOperand subject, Immediate bitIndex)
			: this((Operand)subject, (Operand)bitIndex)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<ArgumentNullException>(bitIndex != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bts"/> class.
		/// </summary>
		/// <param name="subject">The memory operand whose bit is copied and set.</param>
		/// <param name="bitIndex">The index of the bit to copy.</param>
		public Bts(EffectiveAddress subject, Immediate bitIndex)
			: this((Operand)subject, (Operand)bitIndex)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<ArgumentNullException>(bitIndex != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bts"/> class.
		/// </summary>
		/// <param name="subject">The register or memory operand whose bit is copied and set.</param>
		/// <param name="bitIndex">The index of the bit to copy.</param>
		public Bts(Operand subject, Operand bitIndex)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<InvalidCastException>(
				subject is RegisterOperand ||
				subject is EffectiveAddress);
			Contract.Requires<ArgumentNullException>(bitIndex != null);
			Contract.Requires<InvalidCastException>(
				bitIndex is RegisterOperand ||
				bitIndex is Immediate);
			#endregion

			this.subject = subject;
			this.bitIndex = bitIndex;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "bts"; }
		}

		private Operand bitIndex;
		/// <summary>
		/// Gets the index of the bit to copy.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand BitIndex
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is RegisterOperand ||
					Contract.Result<Operand>() is Immediate);
				#endregion
				return bitIndex;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidCastException>(
					value is RegisterOperand ||
					value is Immediate);
				#endregion
				bitIndex = value;
			}
#endif
		}

		private Operand subject;
		/// <summary>
		/// Gets the register or memory operand from which the bit is copied and set.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Subject
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is RegisterOperand ||
					Contract.Result<Operand>() is EffectiveAddress);
				#endregion
				return subject;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidCastException>(
					value is RegisterOperand ||
					value is EffectiveAddress);
				#endregion
				subject = value;
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
			yield return this.subject;
			yield return this.bitIndex;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// BTS reg/mem16, reg16
			new InstructionVariant(
				new byte[] { 0x0F, 0xAB },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
			// BTS reg/mem32, reg32
			new InstructionVariant(
				new byte[] { 0x0F, 0xAB },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
			// BTS reg/mem64, reg64
			new InstructionVariant(
				new byte[] { 0x0F, 0xAB },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),

			// BTS reg/mem16, imm8
			new InstructionVariant(
				new byte[] { 0x0F, 0xBA }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// BTS reg/mem32, imm8
			new InstructionVariant(
				new byte[] { 0x0F, 0xBA }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// BTS reg/mem64, imm8
			new InstructionVariant(
				new byte[] { 0x0F, 0xBA }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
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
			Contract.Invariant(this.subject != null);
			Contract.Invariant(
					this.subject is EffectiveAddress ||
					this.subject is RegisterOperand);
			Contract.Invariant(this.bitIndex != null);
			Contract.Invariant(
					this.bitIndex is Immediate ||
					this.bitIndex is RegisterOperand);
		}
		#endregion
	}
}