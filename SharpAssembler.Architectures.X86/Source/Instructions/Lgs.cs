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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The LGS (Load Far Pointer) instruction.
	/// </summary>
	public class Lgs : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Lgs"/> class.
		/// </summary>
		/// <param name="source">The source operand.</param>
		/// <param name="destination">The destination operand.</param>
		public Lgs(EffectiveAddress source, RegisterOperand destination)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
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
			get { return "lgs"; }
		}

		private RegisterOperand destination;
		/// <summary>
		/// Gets the destination operand of the instruction.
		/// </summary>
		/// <value>An <see cref="RegisterOperand"/>.</value>
		public RegisterOperand Destination
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<RegisterOperand>() != null);
				#endregion
				return destination;
			}
#if OPERAND_SET
			set
			{
			#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
			#endregion
				destination = value;
			}
#endif
		}

		private EffectiveAddress source;
		/// <summary>
		/// Gets the source operand of the instruction.
		/// </summary>
		/// <value>An <see cref="EffectiveAddress"/>.</value>
		public EffectiveAddress Source
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<EffectiveAddress>() != null);
				#endregion
				return source;
			}
#if OPERAND_SET
			set
			{
			#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				source = value;
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
			// LGS reg16, mem16:16
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0xB5 },
				new OperandDescriptor(OperandType.RegisterOperand, DataSize.Bit16),
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit16)),
			// LGS reg32, mem16:32
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0xB5 },
				new OperandDescriptor(OperandType.RegisterOperand, DataSize.Bit32),
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit32)),
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
			Contract.Invariant(this.source != null);
		}
		#endregion
	}
}
