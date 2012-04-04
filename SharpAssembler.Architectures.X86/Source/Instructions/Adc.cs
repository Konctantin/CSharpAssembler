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
using System.Diagnostics.Contracts;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The ADC (Add with Carry) instruction.
	/// </summary>
	public class Adc : ArithmeticInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Adc"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source immediate operand.</param>
		public Adc(RegisterOperand destination, Immediate source)
			: base(destination, source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentException>(destination.Register.IsGeneralPurposeRegister());
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adc"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source immediate operand.</param>
		public Adc(EffectiveAddress destination, Immediate source)
			: base(destination, source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adc"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source register operand.</param>
		public Adc(RegisterOperand destination, RegisterOperand source)
			: base(destination, source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentException>(destination.Register.IsGeneralPurposeRegister());
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentException>(source.Register.IsGeneralPurposeRegister());
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adc"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="source">The source register operand.</param>
		public Adc(EffectiveAddress destination, RegisterOperand source)
			: base(destination, source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentException>(source.Register.IsGeneralPurposeRegister());
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adc"/> class.
		/// </summary>
		/// <param name="destination">The destination register operand.</param>
		/// <param name="source">The source register or memory operand.</param>
		public Adc(RegisterOperand destination, EffectiveAddress source)
			: base(destination, source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentException>(destination.Register.IsGeneralPurposeRegister());
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}
		#endregion

		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "adc"; }
		}

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86Instruction.X86OpcodeVariant"/> objects describing the possible variants of this
		/// instruction.
		/// </summary>
		/// <remarks>
		/// The order of the instruction variants matters. When more than one instruction variant would be a match,
		/// the first one is used. Therefor, the instructions encoding in shorter representations are put before
		/// instructions encoding in a longer representation.
		/// </remarks>
		private static X86OpcodeVariant[] variants = new[]{
			// ADC AL, imm8
			new X86OpcodeVariant(
				new byte[] { 0x14 },
				new OperandDescriptor(Register.AL),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// ADC AX, imm16
			new X86OpcodeVariant(
				new byte[] { 0x15 },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// ADC EAX, imm32
			new X86OpcodeVariant(
				new byte[] { 0x15 },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// ADC RAX, imm32
			new X86OpcodeVariant(
				new byte[] { 0x15 },
				new OperandDescriptor(Register.RAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),


			// ADC reg/mem16, imm8
			new X86OpcodeVariant(
				new byte[] { 0x83 }, 2,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// ADC reg/mem32, imm8
			new X86OpcodeVariant(
				new byte[] { 0x83 }, 2,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// ADC reg/mem64, imm8
			new X86OpcodeVariant(
				new byte[] { 0x83 }, 2,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),


			// ADC reg/mem8, imm8
			new X86OpcodeVariant(
				new byte[] { 0x80 }, 2,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// ADC reg/mem16, imm16
			new X86OpcodeVariant(
				new byte[] { 0x81 }, 2,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
			// ADC reg/mem32, imm32
			new X86OpcodeVariant(
				new byte[] { 0x81 }, 2,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			// ADC reg/mem64, imm32
			new X86OpcodeVariant(
				new byte[] { 0x81 }, 2,
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),


			// ADC reg/mem8, reg8
			new X86OpcodeVariant(
				new byte[] { 0x10 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
			// ADC reg/mem16, reg16
			new X86OpcodeVariant(
				new byte[] { 0x11 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
			// ADC reg/mem32, reg32
			new X86OpcodeVariant(
				new byte[] { 0x11 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
			// ADC reg/mem64, reg64
			new X86OpcodeVariant(
				new byte[] { 0x11 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),


			// ADC reg8, reg/mem8
			new X86OpcodeVariant(
				new byte[] { 0x12 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			// ADC reg16, reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0x13 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			// ADC reg32, reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0x13 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
			// ADC reg64, reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0x13 },
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
	}
}
