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
	/// The PUSH (Push onto Stack) instruction.
	/// </summary>
	public class Push : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Push"/> class.
		/// </summary>
		/// <param name="source">The source memory operand.</param>
		public Push(RegisterOperand source)
			: this((Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Push"/> class.
		/// </summary>
		/// <param name="source">The source memory operand.</param>
		public Push(EffectiveAddress source)
			: this((Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Push"/> class.
		/// </summary>
		/// <param name="source">The source immediate value.</param>
		public Push(Immediate source)
			: this((Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		

		/// <summary>
		/// Initializes a new instance of the <see cref="Push"/> class.
		/// </summary>
		/// <param name="source">The destination operand.</param>
		private Push(Operand source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidCastException>(
					source is Immediate ||
					source is EffectiveAddress ||
					source is RegisterOperand);
			#endregion

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
			get { return "push"; }
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
				this.source = value;
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
			yield return this.source;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// PUSH reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0xFF }, 6,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// PUSH reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0xFF }, 6,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// PUSH reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0xFF }, 6,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),

			// PUSH reg16
			new X86OpcodeVariant(
				new byte[] { 0x50 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit, OperandEncoding.OpcodeAdd)),
			// PUSH reg32
			new X86OpcodeVariant(
				new byte[] { 0x50 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit, OperandEncoding.OpcodeAdd)),
			// PUSH reg64
			new X86OpcodeVariant(
				new byte[] { 0x50 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit, OperandEncoding.OpcodeAdd)),

			// PUSH imm8
			new X86OpcodeVariant(
				new byte[] { 0x6A },
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// PUSH imm16
			new X86OpcodeVariant(
				new byte[] { 0x68 },
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// PUSH imm32
			new X86OpcodeVariant(
				new byte[] { 0x68 },
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// PUSH imm64
			new X86OpcodeVariant(
				new byte[] { 0x68 },
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit64)),

			// TODO: These three are not valid in 64-bit mode:
			// PUSH CS
			new X86OpcodeVariant(
				new byte[] { 0x0E },
				new OperandDescriptor(Register.CS)),
			// PUSH SS
			new X86OpcodeVariant(
				new byte[] { 0x16 },
				new OperandDescriptor(Register.SS)),
			// PUSH DS
			new X86OpcodeVariant(
				new byte[] { 0x1E },
				new OperandDescriptor(Register.DS)),
			// PUSH ES
			new X86OpcodeVariant(
				new byte[] { 0x06 },
				new OperandDescriptor(Register.ES)),

			// PUSH FS
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0xA0 },
				new OperandDescriptor(Register.FS)),
			// PUSH GS
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0xA8 },
				new OperandDescriptor(Register.GS)),
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
			Contract.Invariant(this.source != null);
			Contract.Invariant(
					this.source is Immediate ||
					this.source is EffectiveAddress ||
					this.source is RegisterOperand);
		}
		#endregion
	}
}
