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
using SharpAssembler.x86.Operands;

namespace SharpAssembler.x86.Instructions
{
	/// <summary>
	/// The POP (Pop Stack) instruction.
	/// </summary>
	public class Pop : Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Pop"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		public Pop(RegisterOperand destination)
			: this((Operand)destination)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pop"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		public Pop(EffectiveAddress destination)
			: this((Operand)destination)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			#endregion
		}

		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Pop"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		private Pop(Operand destination)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidCastException>(
					destination is EffectiveAddress ||
					destination is RegisterOperand);
			#endregion

			this.destination = destination;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "pop"; }
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
				this.destination = value;
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
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.x86.Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// POP reg/mem16
			new InstructionVariant(
				new byte[] { 0x8F }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// POP reg/mem32
			new InstructionVariant(
				new byte[] { 0x8F }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// POP reg/mem64
			new InstructionVariant(
				new byte[] { 0x8F }, 0,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),

			// POP reg16
			new InstructionVariant(
				new byte[] { 0x58 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit, OperandEncoding.OpcodeAdd)),
			// POP reg32
			new InstructionVariant(
				new byte[] { 0x58 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit, OperandEncoding.OpcodeAdd)),
			// POP reg64
			new InstructionVariant(
				new byte[] { 0x58 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit, OperandEncoding.OpcodeAdd)),

			// TODO: These three are not valid in 64-bit mode:
			// POP DS
			new InstructionVariant(
				new byte[] { 0x1F },
				new OperandDescriptor(Register.DS)),
			// POP ES
			new InstructionVariant(
				new byte[] { 0x07 },
				new OperandDescriptor(Register.ES)),
			// POP SS
			new InstructionVariant(
				new byte[] { 0x17 },
				new OperandDescriptor(Register.SS)),

			// POP FS
			new InstructionVariant(
				new byte[] { 0x0F, 0xA1 },
				new OperandDescriptor(Register.FS)),
			// POP GS
			new InstructionVariant(
				new byte[] { 0x0F, 0xA9 },
				new OperandDescriptor(Register.GS)),
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
		}
		#endregion
	}
}
