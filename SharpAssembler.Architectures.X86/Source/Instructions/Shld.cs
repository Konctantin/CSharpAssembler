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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The SHLD (Shift Left Double) instruction.
	/// </summary>
	public class Shld : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Shld"/> class.
		/// </summary>
		/// <param name="value">The value to shift.</param>
		/// <param name="source">The bits to shift in.</param>
		/// <param name="positions">The number of positions to adjust.</param>
		public Shld(RegisterOperand value, RegisterOperand source, Immediate positions)
			: this((Operand)positions, source, (Operand)positions)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentNullException>(positions != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Shld"/> class.
		/// </summary>
		/// <param name="value">The value to shift.</param>
		/// <param name="source">The bits to shift in.</param>
		/// <param name="positions">The number of positions to adjust.</param>
		public Shld(EffectiveAddress value, RegisterOperand source, Immediate positions)
			: this((Operand)value, source, (Operand)positions)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentNullException>(positions != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Shld"/> class.
		/// </summary>
		/// <param name="value">The value to shift.</param>
		/// <param name="source">The bits to shift in.</param>
		/// <param name="positions">The register with number of positions to adjust.
		/// May only be the CL register; or <see langword="null"/> to use the CL register.</param>
		public Shld(RegisterOperand value, RegisterOperand source, RegisterOperand positions = null)
			: this((Operand)positions, source, (Operand)positions)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentNullException>(positions != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Shld"/> class.
		/// </summary>
		/// <param name="value">The value to shift.</param>
		/// <param name="source">The bits to shift in.</param>
		/// <param name="positions">The register with number of positions to adjust.
		/// May only be the CL register; or <see langword="null"/> to use the CL register.</param>
		public Shld(EffectiveAddress value, RegisterOperand source, RegisterOperand positions = null)
			: this((Operand)value, source, (Operand)positions)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentNullException>(positions != null);
			#endregion
		}





		/// <summary>
		/// Initializes a new instance of the <see cref="Shld"/> class.
		/// </summary>
		/// <param name="value">The value to shift.</param>
		/// <param name="source">The bits to shift in.</param>
		/// <param name="positions">The number of positions to adjust;
		/// or <see langword="null"/> to adjust one position.</param>
		private Shld(Operand value, RegisterOperand source, Operand positions)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<InvalidCastException>(
					value is EffectiveAddress ||
					value is RegisterOperand);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidCastException>(positions == null || (
					positions is Immediate ||
					positions is RegisterOperand));
			#endregion

			this.value = value;
			this.source = source;
			this.positions = positions;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "shld"; }
		}

		private Operand value;
		/// <summary>
		/// Gets the value being modified.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Value
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return value;
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
				this.value = value;
			}
#endif
		}

		private RegisterOperand source;
		/// <summary>
		/// Gets the source from which the bits are shifted into the new value.
		/// </summary>
		/// <value>A <see cref="RegisterOperand"/>.</value>
		public RegisterOperand Source
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<RegisterOperand>() != null);
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

		private Operand positions;
		/// <summary>
		/// Gets the number of positions to shift.
		/// </summary>
		/// <value>An <see cref="Operand"/>; or <see langword="null"/>.</value>
		public Operand Positions
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() == null || (
					Contract.Result<Operand>() is Immediate ||
					Contract.Result<Operand>() is RegisterOperand));
				#endregion
				return positions;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<InvalidCastException>(value == null || (
					value is Immediate ||
					value is RegisterOperand));
				#endregion
				positions = value;
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
			yield return this.value;
			yield return this.source;
			yield return this.positions;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// SHLD reg/mem16, reg16, imm8
			new InstructionVariant(
				new byte[] { 0x0F, 0xA4 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// SHLD reg/mem16, reg16, CL
			new InstructionVariant(
				new byte[] { 0x0F, 0xA5 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(Register.CL)),

			// SHLD reg/mem32, reg32, imm8
			new InstructionVariant(
				new byte[] { 0x0F, 0xA4 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// SHLD reg/mem32, reg32, CL
			new InstructionVariant(
				new byte[] { 0x0F, 0xA5 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(Register.CL)),

			// SHLD reg/mem64, reg64, imm8
			new InstructionVariant(
				new byte[] { 0x0F, 0xA4 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// SHLD reg/mem64, reg64, CL
			new InstructionVariant(
				new byte[] { 0x0F, 0xA5 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(Register.CL)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
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
			Contract.Invariant(this.value != null);
			Contract.Invariant(
					this.value is EffectiveAddress ||
					this.value is RegisterOperand);
			Contract.Invariant(this.source != null);
			Contract.Invariant(this.positions == null || (
					this.positions is Immediate ||
					this.positions is RegisterOperand));
		}
		#endregion
	}
}
