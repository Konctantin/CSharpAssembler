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
	/// The MOV (Move) instruction opcode.
	/// </summary>
	/// <remarks>
	/// Instructions with this opcode expect two operands that have the following semantics:
	/// <list type="table">
	/// <listheader><term>Index</term><description>Semantics</description></listheader>
	/// <item><term>0</term><description>Destination</description></item>
	/// <item><term>1</term><description>Source</description></item>
	/// </list>
	/// </remarks>
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
				#region Variants
				// MOV AL, moffset8
				new X86OpcodeVariant(
					new byte[] { 0xA0 },
					new OperandDescriptor(Register.AL),
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit8)),
				// MOV AX, moffset16
				new X86OpcodeVariant(
					new byte[] { 0xA1 },
					new OperandDescriptor(Register.AX),
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit16)),
				// MOV EAX, moffset32
				new X86OpcodeVariant(
					new byte[] { 0xA1 },
					new OperandDescriptor(Register.EAX),
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit32)),
				// MOV RAX, moffset64
				new X86OpcodeVariant(
					new byte[] { 0xA1 },
					new OperandDescriptor(Register.RAX),
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit64)),


				// MOV moffset8, AL
				new X86OpcodeVariant(
					new byte[] { 0xA2 },
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit8),
					new OperandDescriptor(Register.AL)),
				// MOV moffset16, AX
				new X86OpcodeVariant(
					new byte[] { 0xA3 },
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit16),
					new OperandDescriptor(Register.AX)),
				// MOV moffset32, EAX
				new X86OpcodeVariant(
					new byte[] { 0xA3 },
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit32),
					new OperandDescriptor(Register.EAX)),
				// MOV moffset64, RAX
				new X86OpcodeVariant(
					new byte[] { 0xA3 },
					new OperandDescriptor(OperandType.MemoryOffset, DataSize.Bit64),
					new OperandDescriptor(Register.RAX)),


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


				// MOV reg16/32/64/mem16, segReg
				new X86OpcodeVariant(
					new byte[] { 0xC6 }, 0,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit | RegisterType.GeneralPurpose32Bit | RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Segment)),

				// MOV segReg, reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0xC6 }, 0,
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.Segment),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),


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
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, Register source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The effective address of the source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, EffectiveAddress source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), source); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source memory offset.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, MemoryOffset source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), source); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The destination memory offset.</param>
		/// <param name="source">The source register.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(MemoryOffset destination, Register source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new RegisterOperand(source)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, byte source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, short source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, int source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit32)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(Register destination, long source)
		{ return X86Opcode.Mov.CreateInstruction(new RegisterOperand(destination), new Immediate(source, DataSize.Bit64)); }



		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, byte source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit8)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, short source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit16)); }

		/// <summary>
		/// Creates a new MOV (Move) instruction.
		/// </summary>
		/// <param name="destination">The effective address of the destination value.</param>
		/// <param name="source">The source value.</param>
		/// <returns>The created instruction.</returns>
		public static X86Instruction Mov(EffectiveAddress destination, int source)
		{ return X86Opcode.Mov.CreateInstruction(destination, new Immediate(source, DataSize.Bit32)); }


		
	}
}