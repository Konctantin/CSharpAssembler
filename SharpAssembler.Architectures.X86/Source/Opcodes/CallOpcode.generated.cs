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
	/// The CALL (Near Procedure Call) instruction opcode.
	/// </summary>
	public class CallOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CallOpcode"/> class.
		/// </summary>
		public CallOpcode()
			: base("call", 1, GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// CALL rel16off
				new X86OpcodeVariant(
					new byte[] { 0xE8 },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// CALL rel32off
				new X86OpcodeVariant(
					new byte[] { 0xE8 },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
				// CALL reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0xFF }, 2,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// CALL reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0xFF }, 2,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// CALL reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0xFF }, 2,
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
		/// Creates a new CALL (Near Procedure Call) instruction.
		/// </summary>
		/// <param name="displacement">A relative offset.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Call(RelativeOffset displacement)
		{ return X86Opcode.Call.CreateInstruction(displacement); }

		/// <summary>
		/// Creates a new CALL (Near Procedure Call) instruction.
		/// </summary>
		/// <param name="target">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Call(Register target)
		{ return X86Opcode.Call.CreateInstruction(new RegisterOperand(target)); }

		/// <summary>
		/// Creates a new CALL (Near Procedure Call) instruction.
		/// </summary>
		/// <param name="target">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Call(EffectiveAddress target)
		{ return X86Opcode.Call.CreateInstruction(target); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The CALL (Near Procedure Call) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Call = new CallOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
