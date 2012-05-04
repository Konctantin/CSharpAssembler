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
	/// The INC (Increment by 1) instruction opcode.
	/// </summary>
	public class IncOpcode : X86Opcode
	{
		/// <inheritdoc />
		public override bool CanLock
		{
			get { return true; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="IncOpcode"/> class.
		/// </summary>
		public IncOpcode()
			: base("inc", GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// INC reg16
				new X86OpcodeVariant(
					new byte[] { 0x40 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit, OperandEncoding.OpcodeAdd))
					{ SupportedModes = ProcessorModes.ProtectedReal },
				// INC reg32
				new X86OpcodeVariant(
					new byte[] { 0x40 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit, OperandEncoding.OpcodeAdd))
					{ SupportedModes = ProcessorModes.ProtectedReal },
				// INC reg/mem8
				new X86OpcodeVariant(
					new byte[] { 0xFE }, 0,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
				// INC reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0xFF }, 0,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// INC reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0xFF }, 0,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// INC reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0xFF }, 0,
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
		/// Creates a new INC (Increment by 1) instruction.
		/// </summary>
		/// <param name="destination">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Inc(Register destination)
		{ return X86Opcode.Inc.CreateInstruction(new RegisterOperand(destination)); }

		/// <summary>
		/// Creates a new INC (Increment by 1) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Inc(EffectiveAddress destination)
		{ return X86Opcode.Inc.CreateInstruction(destination); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The INC (Increment by 1) instruction opcode.
		/// </summary>
		public static readonly X86Opcode Inc = new IncOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
