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
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpAssembler.Core;
using SharpAssembler.x86.Operands;

namespace SharpAssembler.x86.Instructions
{
	/// <summary>
	/// The OR (Logical OR) instruction.
	/// </summary>
	public class Or : Instruction, ILockInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Or"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate value.</param>
		public Or(RegisterOperand destination, Immediate source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Or"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source immediate value.</param>
		public Or(EffectiveAddress destination, Immediate source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Or"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate register operand.</param>
		public Or(RegisterOperand destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Or"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source immediate register operand.</param>
		public Or(EffectiveAddress destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Or"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate memory operand.</param>
		public Or(RegisterOperand destination, EffectiveAddress source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Or"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		private Or(Operand destination, Operand source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidCastException>(
					destination is EffectiveAddress ||
					destination is RegisterOperand);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidCastException>(
					source is Immediate ||
					source is EffectiveAddress ||
					source is RegisterOperand);
			#endregion

			this.destination = destination;
			this.source = source;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "or"; }
		}

		private Operand source;
		/// <summary>
		/// Gets the source operand of the instruction.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Source
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
				return source;
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
				source = value;
			}
#endif
		}

		private Operand destination;
		/// <summary>
		/// Gets the destination operand of the instruction.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Destination
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return destination;
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
				destination = value;
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
			yield return this.destination;
			yield return this.source;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// OR AL, imm8
			new InstructionVariant(
				new byte[] { 0x0C },
				new OperandDescriptor(Register.AL),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// OR AX, imm16
			new InstructionVariant(
				new byte[] { 0x0D },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// OR EAX, imm32
			new InstructionVariant(
				new byte[] { 0x0D },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// OR RAX, imm32
			new InstructionVariant(
				new byte[] { 0x0D },
				new OperandDescriptor(Register.RAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

			// OR reg/mem8, imm8
			new InstructionVariant(
				new byte[] { 0x80 }, 1,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// OR reg/mem16, imm16
			new InstructionVariant(
				new byte[] { 0x81 }, 1,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// OR reg/mem32, imm32
			new InstructionVariant(
				new byte[] { 0x81 }, 1,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// OR reg/mem64, imm32
			new InstructionVariant(
				new byte[] { 0x81 }, 1,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

			// OR reg/mem16, imm8
			new InstructionVariant(
				new byte[] { 0x83 }, 1,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// OR reg/mem32, imm8
			new InstructionVariant(
				new byte[] { 0x83 }, 1,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// OR reg/mem64, imm8
			new InstructionVariant(
				new byte[] { 0x83 }, 1,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),


			// OR reg/mem8, reg8
			new InstructionVariant(
				new byte[] { 0x08 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
			// OR reg/mem16, reg16
			new InstructionVariant(
				new byte[] { 0x09 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
			// OR reg/mem32, reg32
			new InstructionVariant(
				new byte[] { 0x09 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
			// OR reg/mem64, reg64
			new InstructionVariant(
				new byte[] { 0x09 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),


			// OR reg8, reg/mem8
			new InstructionVariant(
				new byte[] { 0x0A },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// OR reg16, reg/mem16
			new InstructionVariant(
				new byte[] { 0x0B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// OR reg32, reg/mem32
			new InstructionVariant(
				new byte[] { 0x0B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// OR reg64, reg/mem64
			new InstructionVariant(
				new byte[] { 0x0B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
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
			Contract.Invariant(this.destination != null);
			Contract.Invariant(
					this.destination is EffectiveAddress ||
					this.destination is RegisterOperand);
			Contract.Invariant(this.source != null);
			Contract.Invariant(
					this.source is Immediate ||
					this.source is EffectiveAddress ||
					this.source is RegisterOperand);
		}
		#endregion
	}
}
