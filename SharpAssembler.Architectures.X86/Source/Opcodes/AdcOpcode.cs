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
using System.Linq;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Opcodes
{
	/// <summary>
	/// The ADC (Add with Carry) instruction opcode.
	/// </summary>
	/// <remarks>
	/// Instructions with this opcode expect two operands that have the following semantics:
	/// <list type="table">
	/// <listheader><term>Index</term><description>Semantics</description></listheader>
	/// <item><term>0</term><description>Destination</description></item>
	/// <item><term>1</term><description>Source</description></item>
	/// </list>
	/// </remarks>
	public class AdcOpcode : X86Opcode
	{
		/// <inheritdoc />
		public override bool CanLock
		{
			get { return true; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AdcOpcode"/> class.
		/// </summary>
		public AdcOpcode()
			: base("adc", 2, GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion
		
		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				#region Variants
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
				#endregion
			};
		}
	}
}

namespace SharpAssembler.Architectures.X86
{
	partial class Instr
	{
		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(Register destination, byte source)
		{ return X86Opcode.Adc.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(Register destination, short source)
		{ return X86Opcode.Adc.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(Register destination, int source)
		{ return X86Opcode.Adc.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit32)); }



		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(EffectiveAddress destination, byte source)
		{ return X86Opcode.Adc.CreateInstruction(destination, new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(EffectiveAddress destination, short source)
		{ return X86Opcode.Adc.CreateInstruction(destination, new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(EffectiveAddress destination, int source)
		{ return X86Opcode.Adc.CreateInstruction(destination, new Immediate(source, DataSize.Bit32)); }


		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(EffectiveAddress destination, Register source)
		{ return X86Opcode.Adc.CreateInstruction(destination, new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new ADC (Add with Carry) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The effective address of the source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Adc(Register destination, EffectiveAddress source)
		{ return X86Opcode.Adc.CreateInstruction(new RegisterOperand(destination), source); }
	}
}
