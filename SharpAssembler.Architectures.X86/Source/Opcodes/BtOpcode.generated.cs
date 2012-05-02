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
	/// The BT (Bit Test) instruction opcode.
	/// </summary>
	public class BtOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BtOpcode"/> class.
		/// </summary>
		public BtOpcode()
			: base("bt", GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// BT reg/mem16, imm8
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xBA }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// BT reg/mem32, imm8
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xBA }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// BT reg/mem64, imm8
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xBA }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// BT reg/mem16, reg16
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xA3 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// BT reg/mem32, reg32
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xA3 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// BT reg/mem64, reg64
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xA3 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			};
		}
	}
}

namespace SharpAssembler.Architectures.X86
{
	partial class Instr
	{
		/// <summary>
		/// Creates a new BT (Bit Test) instruction.
		/// </summary>
		/// <param name="value">A register or memory operand.</param>
		/// <param name="bitindex">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Bt(Register value, byte bitindex)
		{ return X86Opcode.Bt.CreateInstruction(new RegisterOperand(value), new Immediate(bitindex, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new BT (Bit Test) instruction.
		/// </summary>
		/// <param name="value">A register or memory operand.</param>
		/// <param name="bitindex">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Bt(Register value, sbyte bitindex)
		{ return X86Opcode.Bt.CreateInstruction(new RegisterOperand(value), new Immediate(bitindex, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new BT (Bit Test) instruction.
		/// </summary>
		/// <param name="value">A register or memory operand.</param>
		/// <param name="bitindex">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Bt(EffectiveAddress value, byte bitindex)
		{ return X86Opcode.Bt.CreateInstruction(value, new Immediate(bitindex, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new BT (Bit Test) instruction.
		/// </summary>
		/// <param name="value">A register or memory operand.</param>
		/// <param name="bitindex">An immediate value.</param>
		/// <returns>The created instruction.</returns>
		[CLSCompliant(false)]
		public static X86Instruction Bt(EffectiveAddress value, sbyte bitindex)
		{ return X86Opcode.Bt.CreateInstruction(value, new Immediate(bitindex, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new BT (Bit Test) instruction.
		/// </summary>
		/// <param name="value">A register or memory operand.</param>
		/// <param name="bitindex">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Bt(Register value, Register bitindex)
		{ return X86Opcode.Bt.CreateInstruction(new RegisterOperand(value), new RegisterOperand(bitindex)); }

		/// <summary>
		/// Creates a new BT (Bit Test) instruction.
		/// </summary>
		/// <param name="value">A register or memory operand.</param>
		/// <param name="bitindex">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Bt(EffectiveAddress value, Register bitindex)
		{ return X86Opcode.Bt.CreateInstruction(value, new RegisterOperand(bitindex)); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The BT (Bit Test) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Bt = new BtOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
