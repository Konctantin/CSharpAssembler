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
	/// The CMPXCHG (Compare and Exchange) instruction opcode.
	/// </summary>
	public class CmpXchgOpcode : X86Opcode
	{
		/// <inheritdoc />
		public override bool CanLock
		{
			get { return true; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CmpXchgOpcode"/> class.
		/// </summary>
		public CmpXchgOpcode()
			: base("cmpxchg", GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// CMPXCHG reg/mem8, reg8
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xB0 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
				// CMPXCHG reg/mem16, reg16
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xB1 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMPXCHG reg/mem32, reg32
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xB1 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMPXCHG reg/mem64, reg64
				new X86OpcodeVariant(
					new byte[] { 0x0F, 0xB1 },
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
		/// Creates a new CMPXCHG (Compare and Exchange) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction CmpXchg(Register destination, Register source)
		{ return X86Opcode.CmpXchg.CreateInstruction(new RegisterOperand(destination), new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new CMPXCHG (Compare and Exchange) instruction.
		/// </summary>
		/// <param name="destination">A register or memory operand.</param>
		/// <param name="source">A register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction CmpXchg(EffectiveAddress destination, Register source)
		{ return X86Opcode.CmpXchg.CreateInstruction(destination, new RegisterOperand(source)); }
	}

	partial class X86Opcode
	{
		/// <summary>
		/// The CMPXCHG (Compare and Exchange) instruction opcode.
		/// </summary>
		public static readonly X86Opcode CmpXchg = new CmpXchgOpcode();
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
