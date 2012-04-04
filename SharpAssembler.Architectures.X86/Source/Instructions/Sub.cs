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
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The SUB (Subtract) instruction.
	/// </summary>
	public class Sub : X86Instruction, ILockInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Sub"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate value.</param>
		public Sub(RegisterOperand destination, Immediate source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sub"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source immediate value.</param>
		public Sub(EffectiveAddress destination, Immediate source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sub"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate register operand.</param>
		public Sub(RegisterOperand destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sub"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source immediate register operand.</param>
		public Sub(EffectiveAddress destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sub"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate memory operand.</param>
		public Sub(RegisterOperand destination, EffectiveAddress source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Sub"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		private Sub(Operand destination, Operand source)
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
			get { return "sub"; }
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
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// SUB AL, imm8
			new X86OpcodeVariant(
				new byte[] { 0x2C },
				new OperandDescriptor(Register.AL),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// SUB AX, imm16
			new X86OpcodeVariant(
				new byte[] { 0x2D },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// SUB EAX, imm32
			new X86OpcodeVariant(
				new byte[] { 0x2D },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// SUB RAX, imm32
			new X86OpcodeVariant(
				new byte[] { 0x2D },
				new OperandDescriptor(Register.RAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

			// SUB reg/mem8, imm8
			new X86OpcodeVariant(
				new byte[] { 0x80 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// SUB reg/mem16, imm16
			new X86OpcodeVariant(
				new byte[] { 0x81 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// SUB reg/mem32, imm32
			new X86OpcodeVariant(
				new byte[] { 0x81 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// SUB reg/mem64, imm32
			new X86OpcodeVariant(
				new byte[] { 0x81 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

			// SUB reg/mem16, imm8
			new X86OpcodeVariant(
				new byte[] { 0x83 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// SUB reg/mem32, imm8
			new X86OpcodeVariant(
				new byte[] { 0x83 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// SUB reg/mem64, imm8
			new X86OpcodeVariant(
				new byte[] { 0x83 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),


			// SUB reg/mem8, reg8
			new X86OpcodeVariant(
				new byte[] { 0x28 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
			// SUB reg/mem16, reg16
			new X86OpcodeVariant(
				new byte[] { 0x29 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
			// SUB reg/mem32, reg32
			new X86OpcodeVariant(
				new byte[] { 0x29 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
			// SUB reg/mem64, reg64
			new X86OpcodeVariant(
				new byte[] { 0x29 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),


			// SUB reg8, reg/mem8
			new X86OpcodeVariant(
				new byte[] { 0x2A },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// SUB reg16, reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0x2B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// SUB reg32, reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0x2B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// SUB reg64, reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0x2B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86OpcodeVariant"/>
		/// objects.</returns>
		internal override X86OpcodeVariant[] GetVariantList()
		{ return variants; }

		/// <summary>
		/// Initializes a static instance of the <see cref="Sub"/> class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static Sub()
		{
			variants = new X86OpcodeVariant[19];
			int index = 0;

			// SUB AL, imm8
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2C },
				new OperandDescriptor(Register.AL),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8));
			// SUB AX, imm16
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2D },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16));
			// SUB EAX, imm32
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2D },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32));
			// SUB RAX, imm32
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2D },
				new OperandDescriptor(Register.RAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32));

			// SUB reg/mem8, imm8
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x80 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8));
			// SUB reg/mem16, imm16
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x81 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16));
			// SUB reg/mem32, imm32
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x81 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32));
			// SUB reg/mem64, imm32
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x81 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32));

			// SUB reg/mem16, imm8
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x83 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8));
			// SUB reg/mem32, imm8
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x83 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8));
			// SUB reg/mem64, imm8
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x83 }, 5,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8));


			// SUB reg/mem8, reg8
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x28 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit));
			// SUB reg/mem16, reg16
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x29 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit));
			// SUB reg/mem32, reg32
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x29 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit));
			// SUB reg/mem64, reg64
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x29 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit));


			// SUB reg8, reg/mem8
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2A },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit));
			// SUB reg16, reg/mem16
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit));
			// SUB reg32, reg/mem32
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit));
			// SUB reg64, reg/mem64
			variants[index++] = new X86OpcodeVariant(
				new byte[] { 0x2B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit));
		}
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
