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
	/// The CMP (Compare) instruction opcode.
	/// </summary>
	public class CmpOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CmpOpcode"/> class.
		/// </summary>
		public CmpOpcode()
			: base("cmp", GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// CMP AL, imm8
				new X86OpcodeVariant(
					new byte[] { 0x3C },
					new OperandDescriptor(Register.AL),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// CMP AX, imm16
				new X86OpcodeVariant(
					new byte[] { 0x3D },
					new OperandDescriptor(Register.AX),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
				// CMP EAX, imm32
				new X86OpcodeVariant(
					new byte[] { 0x3D },
					new OperandDescriptor(Register.EAX),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// CMP RAX, imm32
				new X86OpcodeVariant(
					new byte[] { 0x3D },
					new OperandDescriptor(Register.RAX),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// CMP reg/mem8, imm8
				new X86OpcodeVariant(
					new byte[] { 0x80 }, 7,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// CMP reg/mem16, imm8
				new X86OpcodeVariant(
					new byte[] { 0x83 }, 7,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// CMP reg/mem32, imm8
				new X86OpcodeVariant(
					new byte[] { 0x83 }, 7,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// CMP reg/mem64, imm8
				new X86OpcodeVariant(
					new byte[] { 0x83 }, 7,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// CMP reg/mem16, imm16
				new X86OpcodeVariant(
					new byte[] { 0x81 }, 7,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
				// CMP reg/mem32, imm32
				new X86OpcodeVariant(
					new byte[] { 0x81 }, 7,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// CMP reg/mem64, imm32
				new X86OpcodeVariant(
					new byte[] { 0x81 }, 7,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// CMP reg/mem8, reg8
				new X86OpcodeVariant(
					new byte[] { 0x38 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
				// CMP reg/mem16, reg16
				new X86OpcodeVariant(
					new byte[] { 0x39 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMP reg/mem32, reg32
				new X86OpcodeVariant(
					new byte[] { 0x39 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMP reg/mem64, reg64
				new X86OpcodeVariant(
					new byte[] { 0x39 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
				// CMP reg8, reg/mem8
				new X86OpcodeVariant(
					new byte[] { 0x3A },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
				// CMP reg16, reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0x3B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// CMP reg32, reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0x3B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// CMP reg64, reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0x3B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
			};
		}
	}
}

namespace SharpAssembler.Architectures.X86
{
	partial class Instr
	{
		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(Register left, byte right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Cmp(Register left, sbyte right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(Register left, short right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Cmp(Register left, ushort right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(Register left, int right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), new Immediate(right, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Cmp(Register left, uint right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), new Immediate(right, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(EffectiveAddress left, byte right)
		{ return X86Opcode.Cmp.CreateInstruction(left, new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Cmp(EffectiveAddress left, sbyte right)
		{ return X86Opcode.Cmp.CreateInstruction(left, new Immediate(right, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(EffectiveAddress left, short right)
		{ return X86Opcode.Cmp.CreateInstruction(left, new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Cmp(EffectiveAddress left, ushort right)
		{ return X86Opcode.Cmp.CreateInstruction(left, new Immediate(right, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(EffectiveAddress left, int right)
		{ return X86Opcode.Cmp.CreateInstruction(left, new Immediate(right, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Cmp(EffectiveAddress left, uint right)
		{ return X86Opcode.Cmp.CreateInstruction(left, new Immediate(right, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(Register left, Register right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), new RegisterOperand(right)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register or memory operand.</param>
		/// <param name="right">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(EffectiveAddress left, Register right)
		{ return X86Opcode.Cmp.CreateInstruction(left, new RegisterOperand(right)); }

		/// <summary>
		/// Creates a new CMP (Compare) instruction.
		/// </summary>
		/// <param name="left">A register.</param>
		/// <param name="right">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Cmp(Register left, EffectiveAddress right)
		{ return X86Opcode.Cmp.CreateInstruction(new RegisterOperand(left), right); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The CMP (Compare) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Cmp = new CmpOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
