﻿//////////////////////////////////////////////////////
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
	/// The BSF (Bit Scan Forward) instruction opcode.
	/// </summary>
	public class BsfOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BsfOpcode"/> class.
		/// </summary>
		public BsfOpcode()
			: base("bsf", 2, GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
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
		}
	}
}

namespace SharpAssembler.Architectures.X86
{
	partial class Instr
	{
		/// <summary>
		/// Creates a new BSF (Bit Scan Forward) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="subject">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Bsf(Register destination, Register subject)
		{ return X86Opcode.Bsf.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(subject)); }

		/// <summary>
		/// Creates a new BSF (Bit Scan Forward) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="subject">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Bsf(Register destination, EffectiveAddress subject)
		{ return X86Opcode.Bsf.CreateInstruction(new RegisterOperand(destination), subject); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The BSF (Bit Scan Forward) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Bsf = new BsfOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
