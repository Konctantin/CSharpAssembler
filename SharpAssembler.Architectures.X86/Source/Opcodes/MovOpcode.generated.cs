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
	/// The MOV (Move) instruction opcode.
	/// </summary>
	public class MovOpcode : X86Opcode
	{
		/// <inheritdoc />
		public override bool CanLock
		{
			get { return true; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MovOpcode"/> class.
		/// </summary>
		public MovOpcode()
			: base("mov", 2, GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// MOV reg/mem8, reg8
				new X86OpcodeVariant(
					new byte[] { 0x88 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
				// MOV reg/mem16, reg16
				new X86OpcodeVariant(
					new byte[] { 0x89 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// MOV reg/mem32, reg32
				new X86OpcodeVariant(
					new byte[] { 0x89 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// MOV reg/mem64, reg64
				new X86OpcodeVariant(
					new byte[] { 0x89 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
				// MOV reg8, reg/mem8
				new X86OpcodeVariant(
					new byte[] { 0x8A },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
				// MOV reg16, reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0x8B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// MOV reg32, reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0x8B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// MOV reg64, reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0x8B },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
				// MOV reg8, imm8
				new X86OpcodeVariant(
					new byte[] { 0xB0 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit, OperandEncoding.OpcodeAdd),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// MOV reg16, imm16
				new X86OpcodeVariant(
					new byte[] { 0xB8 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit, OperandEncoding.OpcodeAdd),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
				// MOV reg32, imm32
				new X86OpcodeVariant(
					new byte[] { 0xB8 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit, OperandEncoding.OpcodeAdd),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// MOV reg64, imm64
				new X86OpcodeVariant(
					new byte[] { 0xB8 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit, OperandEncoding.OpcodeAdd),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit64)),
				// MOV reg/mem8, imm8
				new X86OpcodeVariant(
					new byte[] { 0xC6 }, 0,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// MOV reg/mem16, imm16
				new X86OpcodeVariant(
					new byte[] { 0xC7 }, 0,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
				// MOV reg/mem32, imm32
				new X86OpcodeVariant(
					new byte[] { 0xC7 }, 0,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// MOV reg/mem64, imm32
				new X86OpcodeVariant(
					new byte[] { 0xC7 }, 0,
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
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, Register source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, Register source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, EffectiveAddress source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), source); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, byte source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Mov(Register destination, sbyte source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, short source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Mov(Register destination, ushort source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, int source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Mov(Register destination, uint source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, long source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit64)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Mov(Register destination, ulong source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit64)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, byte source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Mov(EffectiveAddress destination, sbyte source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, short source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Mov(EffectiveAddress destination, ushort source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, int source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Mov(EffectiveAddress destination, uint source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit32)); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The MOV (Move) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Mov = new MovOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
