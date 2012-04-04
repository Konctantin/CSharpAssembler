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
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The BSF (Bit Scan Forward) instruction.
	/// </summary>
	public class Bsf : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Bsf"/> class.
		/// </summary>
		/// <param name="destination">The register in which the bit's index will be stored.</param>
		/// <param name="subject">The register operand which is checked.</param>
		public Bsf(RegisterOperand destination, RegisterOperand subject)
			: this(destination, (Operand)subject)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(subject != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bsf"/> class.
		/// </summary>
		/// <param name="destination">The register in which the bit's index will be stored.</param>
		/// <param name="subject">The memory operand which is checked.</param>
		public Bsf(RegisterOperand destination, EffectiveAddress subject)
			: this(destination, (Operand)subject)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(subject != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bsf"/> class.
		/// </summary>
		/// <param name="destination">The register in which the bit's index will be stored.</param>
		/// <param name="subject">The register or memory operand which is checked.</param>
		private Bsf(RegisterOperand destination, Operand subject)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidCastException>(
					subject is EffectiveAddress ||
					subject is RegisterOperand);
			Contract.Requires<ArgumentNullException>(subject != null);
			#endregion

			this.destination = destination;
			this.subject = subject;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "bsf"; }
		}

		private RegisterOperand destination;
		/// <summary>
		/// Gets the register where the bit index is stored.
		/// </summary>
		/// <value>A <see cref="RegisterOperand"/> operand.</value>
		/// <exception cref="ArgumentNullException">
		/// The value is <see langword="null"/>.
		/// </exception>
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

		private Operand subject;
		/// <summary>
		/// Gets the subject register or memory operand.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Subject
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return subject;
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
			yield return this.destination;
			yield return this.subject;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86Instruction.X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// BSF reg16, reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0xBC },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// BSF reg32, reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0xBC },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// BSF reg64, reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0xBC },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86Instruction.X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86Instruction.X86OpcodeVariant"/>
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
			Contract.Invariant(this.subject != null);
			Contract.Invariant(
				this.subject is EffectiveAddress ||
				this.subject is RegisterOperand);
		}
		#endregion
	}
}
