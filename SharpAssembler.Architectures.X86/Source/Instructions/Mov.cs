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
	/// The MOV (Move) instruction.
	/// </summary>
	public class Mov : X86Instruction, ILockInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source register.</param>
		public Mov(RegisterOperand destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source memory operand.</param>
		public Mov(RegisterOperand destination, EffectiveAddress source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source register.</param>
		public Mov(EffectiveAddress destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination memory offset.</param>
		/// <param name="source">The source register.</param>
		public Mov(MemoryOffset destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source memory offset.</param>
		public Mov(RegisterOperand destination, MemoryOffset source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source immediate value.</param>
		public Mov(RegisterOperand destination, Immediate source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source immediate value.</param>
		public Mov(EffectiveAddress destination, Immediate source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Mov"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		private Mov(Operand destination, Operand source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidCastException>(
					destination is EffectiveAddress ||
					destination is MemoryOffset ||
					destination is RegisterOperand);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidCastException>(
					source is Immediate ||
					source is MemoryOffset ||
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
			get { return "mov"; }
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
					Contract.Result<Operand>() is MemoryOffset ||
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
						value is MemoryOffset ||
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
					Contract.Result<Operand>() is Immediate ||
					Contract.Result<Operand>() is MemoryOffset ||
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
						value is MemoryOffset ||
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
			// MOV AL, moffset8
			new X86OpcodeVariant(
				new byte[] { 0xA0 },
				new OperandDescriptor(Register.AL),
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit8)),
			// MOV AX, moffset16
			new X86OpcodeVariant(
				new byte[] { 0xA1 },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit16)),
			// MOV EAX, moffset32
			new X86OpcodeVariant(
				new byte[] { 0xA1 },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit32)),
			// MOV RAX, moffset64
			new X86OpcodeVariant(
				new byte[] { 0xA1 },
				new OperandDescriptor(Register.RAX),
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit64)),


			// MOV moffset8, AL
			new X86OpcodeVariant(
				new byte[] { 0xA2 },
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit8),
				new OperandDescriptor(Register.AL)),
			// MOV moffset16, AX
			new X86OpcodeVariant(
				new byte[] { 0xA3 },
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit16),
				new OperandDescriptor(Register.AX)),
			// MOV moffset32, EAX
			new X86OpcodeVariant(
				new byte[] { 0xA3 },
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit32),
				new OperandDescriptor(Register.EAX)),
			// MOV moffset64, RAX
			new X86OpcodeVariant(
				new byte[] { 0xA3 },
				new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit64),
				new OperandDescriptor(Register.RAX)),


			// MOV reg8, imm8
			new X86OpcodeVariant(
				new byte[] { 0xB0 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit, OperandEncoding.OpcodeAdd),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// MOV reg16, imm16
			new X86OpcodeVariant(
				new byte[] { 0xB8 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit, OperandEncoding.OpcodeAdd),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// MOV reg32, imm32
			new X86OpcodeVariant(
				new byte[] { 0xB8 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit, OperandEncoding.OpcodeAdd),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// MOV reg64, imm64
			new X86OpcodeVariant(
				new byte[] { 0xB8 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit, OperandEncoding.OpcodeAdd),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit64)),


			// MOV reg/mem8, reg8
			new X86OpcodeVariant(
				new byte[] { 0x88 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
			// MOV reg/mem16, reg16
			new X86OpcodeVariant(
				new byte[] { 0x89 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
			// MOV reg/mem32, reg32
			new X86OpcodeVariant(
				new byte[] { 0x89 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
			// MOV reg/mem64, reg64
			new X86OpcodeVariant(
				new byte[] { 0x89 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),


			// MOV reg8, reg/mem8
			new X86OpcodeVariant(
				new byte[] { 0x8A },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// MOV reg16, reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0x8B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// MOV reg32, reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0x8B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// MOV reg64, reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0x8B },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),


			// MOV reg16/32/64/mem16, segReg
			new X86OpcodeVariant(
				new byte[] { 0xC6 }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit | RegisterType.GeneralPurpose32Bit | RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Segment)),

			// MOV segReg, reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0xC6 }, 0,
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Segment),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),


			// MOV reg/mem8, imm8
			new X86OpcodeVariant(
				new byte[] { 0xC6 }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// MOV reg/mem16, imm16
			new X86OpcodeVariant(
				new byte[] { 0xC7 }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// MOV reg/mem32, imm32
			new X86OpcodeVariant(
				new byte[] { 0xC7 }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// MOV reg/mem64, imm32
			new X86OpcodeVariant(
				new byte[] { 0xC7 }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86OpcodeVariant"/>
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
			Contract.Invariant(this.destination != null);
			Contract.Invariant(
					this.destination is EffectiveAddress ||
					this.destination is MemoryOffset ||
					this.destination is RegisterOperand);
			Contract.Invariant(this.source != null);
			Contract.Invariant(
					this.source is Immediate ||
					this.source is EffectiveAddress ||
					this.source is MemoryOffset ||
					this.source is RegisterOperand);
		}
		#endregion
	}
}
