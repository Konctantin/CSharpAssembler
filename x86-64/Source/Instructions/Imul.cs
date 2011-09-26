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
	/// The IMUL (Signed Multiply) instruction.
	/// </summary>
	public class Imul : Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Imul"/> class.
		/// </summary>
		/// <param name="multiplier">The multiplier.</param>
		public Imul(EffectiveAddress multiplier)
			: this(null, null, (Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Imul"/> class.
		/// </summary>
		/// <param name="multiplier">The multiplier.</param>
		public Imul(RegisterOperand multiplier)
			: this(null, null, (Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Imul"/> class.
		/// </summary>
		/// <param name="value">The destination and source operand.</param>
		/// <param name="multiplier">The multiplier.</param>
		public Imul(RegisterOperand value, EffectiveAddress multiplier)
			: this(value, null, (Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Imul"/> class.
		/// </summary>
		/// <param name="value">The destination and source operand.</param>
		/// <param name="multiplier">The multiplier.</param>
		public Imul(RegisterOperand value, RegisterOperand multiplier)
			: this(value, null, (Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Imul"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="multiplier">The multiplier.</param>
		public Imul(RegisterOperand destination, EffectiveAddress source, Immediate multiplier)
			: this(destination, (Operand)source, (Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Imul"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="multiplier">The multiplier.</param>
		public Imul(RegisterOperand destination, RegisterOperand source, Immediate multiplier)
			: this(destination, (Operand)source, (Operand)multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Imul"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="multiplier">The multiplier.</param>
		private Imul(RegisterOperand destination, Operand source, Operand multiplier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(multiplier != null);
			Contract.Requires<InvalidCastException>(
					multiplier is Immediate ||
					multiplier is EffectiveAddress ||
					multiplier is RegisterOperand);
			Contract.Requires<InvalidCastException>(source == null || (
					source is EffectiveAddress ||
					source is RegisterOperand));
			#endregion

			this.destination = destination;
			this.source = source;
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
			get { return "imul"; }
		}

		private RegisterOperand destination;
		/// <summary>
		/// Gets the destination/value of the operation.
		/// </summary>
		/// <value>An <see cref="Operand"/>; or <see langword="null"/>.</value>
		public RegisterOperand Destination
		{
			get { return destination; }
#if OPERAND_SET
			set { destination = value; }
#endif
		}

		private Operand source;
		/// <summary>
		/// Gets the source of the operation.
		/// </summary>
		/// <value>An <see cref="Operand"/>; or <see langword="null"/>.</value>
		public Operand Source
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() == null || (
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand));
				#endregion
				return source;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<InvalidCastException>(value == null || (
					value is EffectiveAddress ||
					value is RegisterOperand));
				#endregion
				source = value;
			}
#endif
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
					Contract.Result<Operand>() is Immediate ||
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
					value is Immediate ||
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
			yield return this.destination;
			yield return this.source;
			yield return this.multiplier;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// 0: -
			// 1: -
			// 2: Multiplier (reg/mem)

			// IMUL reg/mem8
			new InstructionVariant(
				new byte[] { 0xF6 }, 5,
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// IMUL reg/mem16
			new InstructionVariant(
				new byte[] { 0xF7 }, 5,
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// IMUL reg/mem32
			new InstructionVariant(
				new byte[] { 0xF7 }, 5,
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// IMUL reg/mem64
			new InstructionVariant(
				new byte[] { 0xF7 }, 5,
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),

			// 0: Destination/Value (reg)
			// 1: -
			// 2: Multiplier (reg/mem)

			// IMUL reg16,reg/mem16
			new InstructionVariant(
				new byte[] { 0x0F, 0xAF },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// IMUL reg32,reg/mem32
			new InstructionVariant(
				new byte[] { 0x0F, 0xAF },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// IMUL reg64,reg/mem64
			new InstructionVariant(
				new byte[] { 0x0F, 0xAF },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.None, DataSize.None),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),

			// 0: Destination (reg)
			// 1: Value (reg/mem)
			// 2: Multiplier (imm)

			// IMUL reg16,reg/mem16,imm8
			new InstructionVariant(
				new byte[] { 0x6B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// IMUL reg32,reg/mem32,imm8
			new InstructionVariant(
				new byte[] { 0x6B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// IMUL reg64,reg/mem64,imm8
			new InstructionVariant(
				new byte[] { 0x6B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),

			// IMUL reg16,reg/mem16,imm16
			new InstructionVariant(
				new byte[] { 0x69 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// IMUL reg32,reg/mem32,imm32
			new InstructionVariant(
				new byte[] { 0x69 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// IMUL reg64,reg/mem64,imm32
			new InstructionVariant(
				new byte[] { 0x69 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
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
			Contract.Invariant(this.source == null || (
					this.source is EffectiveAddress ||
					this.source is RegisterOperand));
			Contract.Invariant(this.multiplier != null);
			Contract.Invariant(
					this.multiplier is Immediate ||
					this.multiplier is EffectiveAddress ||
					this.multiplier is RegisterOperand);
		}
		#endregion
	}
}
