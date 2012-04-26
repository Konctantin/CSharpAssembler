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
	/// The CMOVBE (Move if below or equal, or not above) instruction opcode.
	/// </summary>
	public class CMovBEOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CMovBEOpcode"/> class.
		/// </summary>
		public CMovBEOpcode()
			: base("cmovbe", 2, GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// CMOVBE reg16, reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0x46 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVBE reg32, reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0x46 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVBE reg64, reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0x46 },
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
		/// Creates a new CMOVBE (Move if below or equal, or not above) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction CMovBE(Register destination, Register source)
		{ return X86Opcode.CMovBE.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new CMOVBE (Move if below or equal, or not above) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction CMovBE(Register destination, EffectiveAddress source)
		{ return X86Opcode.CMovBE.CreateInstruction(new RegisterOperand(destination), source); }



		/// <summary>
		/// Creates a new CMOVBE (Move if below or equal, or not above) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction CMovNA(Register destination, Register source)
		{ return X86Opcode.CMovBE.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new CMOVBE (Move if below or equal, or not above) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <param name="source">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction CMovNA(Register destination, EffectiveAddress source)
		{ return X86Opcode.CMovBE.CreateInstruction(new RegisterOperand(destination), source); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The CMOVBE (Move if below or equal, or not above) instruction opcode.
		/// </summary>
		public static readonly X86Opcode CMovBE = new CMovBEOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
