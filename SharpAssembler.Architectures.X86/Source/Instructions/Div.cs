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
	/// The DIV (Unsigned Divide) instruction.
	/// </summary>
	public class Div : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// </summary>
		/// <param name="subject">The subject memory operand.</param>
		public Div(EffectiveAddress subject)
			: this((Operand)subject)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// </summary>
		/// <param name="subject">The subject register operand.</param>
		public Div(RegisterOperand subject)
			: this((Operand)subject)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Div"/> class.
		/// </summary>
		/// <param name="subject">The subject operand.</param>
		private Div(Operand subject)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(subject != null);
			Contract.Requires<InvalidCastException>(
					subject is EffectiveAddress ||
					subject is RegisterOperand);
			#endregion

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
			get { return "div"; }
		}

		private Operand subject;
		/// <summary>
		/// Gets the subject operand of the instruction.
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
			yield return this.subject;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// DIV reg/mem8
			new X86OpcodeVariant(
				new byte[] { 0xF6 }, 6,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// DIV reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0xF7 }, 6,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// DIV reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0xF7 }, 6,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// DIV reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0xF7 }, 6,
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
		}
		#endregion
	}
}
