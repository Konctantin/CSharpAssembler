//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
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
using SharpAssembler.Architectures.X86.Opcodes;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Opcodes
{
	/// <summary>
	/// The IMUL (Signed Multiply) instruction opcode.
	/// </summary>
	public class ImulOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ImulOpcode"/> class.
		/// </summary>
		public ImulOpcode()
			: base("imul", GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// IMUL reg/mem8
				new X86OpcodeVariant(
					new byte[] { 0xF6 }, 5,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
				// IMUL reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0xF7 }, 5,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// IMUL reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0xF7 }, 5,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// IMUL reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0xF7 }, 5,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
				// IMUL reg16, reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xAF },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// IMUL reg32, reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xAF },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// IMUL reg64, reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xAF },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
				// IMUL reg16, reg/mem16, imm8
				new X86OpcodeVariant(
					new byte[] { 0x6B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// IMUL reg32, reg/mem32, imm8
				new X86OpcodeVariant(
					new byte[] { 0x6B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// IMUL reg64, reg/mem64, imm8
				new X86OpcodeVariant(
					new byte[] { 0x6B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// IMUL reg16, reg/mem16, imm16
				new X86OpcodeVariant(
					new byte[] { 0x69 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
				// IMUL reg32, reg/mem32, imm32
				new X86OpcodeVariant(
					new byte[] { 0x69 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// IMUL reg64, reg/mem64, imm32
				new X86OpcodeVariant(
					new byte[] { 0x69 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
			};
		}
	}
}

namespace SharpAssembler.Architectures.X86
{
	partial class Instr
	{
		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register source)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(EffectiveAddress source)
		{ return X86Opcode.Imul.CreateInstruction(source); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, Register source)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, EffectiveAddress source)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), source); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, Register left, byte right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(left), new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Imul(Register destination, Register left, sbyte right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(left), new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, EffectiveAddress left, byte right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), left, new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Imul(Register destination, EffectiveAddress left, sbyte right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), left, new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, Register left, short right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(left), new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Imul(Register destination, Register left, ushort right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(left), new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, EffectiveAddress left, short right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), left, new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Imul(Register destination, EffectiveAddress left, ushort right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), left, new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, Register left, int right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(left), new Immediate(right, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Imul(Register destination, Register left, uint right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(left), new Immediate(right, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Imul(Register destination, EffectiveAddress left, int right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), left, new Immediate(right, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new IMUL (Signed Multiply) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Imul(Register destination, EffectiveAddress left, uint right)
		{ return X86Opcode.Imul.CreateInstruction(new RegisterOperand(destination), left, new Immediate(right, DataSize.Bit32)); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The IMUL (Signed Multiply) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Imul = new ImulOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
