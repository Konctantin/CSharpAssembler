﻿#region Copyright and License
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
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The MOVD (Move Doubleword or Quadword) instruction.
	/// </summary>
	public class Movd : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Movd"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source register.</param>
		public Movd(RegisterOperand destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Movd"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source memory operand.</param>
		public Movd(RegisterOperand destination, EffectiveAddress source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Movd"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source register.</param>
		public Movd(EffectiveAddress destination, RegisterOperand source)
			: this((Operand)destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Movd"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		private Movd(Operand destination, Operand source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidCastException>(
					destination is EffectiveAddress ||
					destination is RegisterOperand);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidCastException>(
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
			get { return "movd"; }
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
			// MOVD xmm, reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0x66, 0x0F, 0x6E },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd128Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// MOVD xmm, reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0x66, 0x0F, 0x6E },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd128Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),

			// MOVD reg/mem32, xmm
			new X86OpcodeVariant(
				new byte[] { 0x66, 0x0F, 0x7E },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd128Bit)),
			// MOVD reg/mem64, xmm
			new X86OpcodeVariant(
				new byte[] { 0x66, 0x0F, 0x7E },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd128Bit)),



			// MOVD mmx, reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x6E },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// MOVD mmx, reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x6E },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),

			// MOVD reg/mem32, mmx
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x7E },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd64Bit)),
			// MOVD reg/mem64, mmx
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x7E },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Simd64Bit)),
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
					this.destination is RegisterOperand);
			Contract.Invariant(this.source != null);
			Contract.Invariant(
					this.source is EffectiveAddress ||
					this.source is RegisterOperand);
		}
		#endregion
	}
}
