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
using Moq;
using NUnit.Framework;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Tests.Opcodes
{
	/// <summary>
	/// Tests all variants of the MOV opcode.
	/// </summary>
	[TestFixture]
	public class MovTests : OpcodeTestBase
	{
		[Test]
		public void MOV_regmem8_reg8()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x165E)), Register.CL);

			// MOV BYTE [0x165E], cl
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x88, 0x0E, 0x5E, 0x16 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x88, 0x0D, 0x5E, 0x16, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x88, 0x0C, 0x25, 0x5E, 0x16, 0x00, 0x00 });
		}

		[Test]
		public void MOV_regmem16_reg16()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xCEEA)), Register.DI);

			// MOV WORD [0xCEEA], di
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x89, 0x3E, 0xEA, 0xCE });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x89, 0x3D, 0xEA, 0xCE, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x89, 0x3C, 0x25, 0xEA, 0xCE, 0x00, 0x00 });
		}

		[Test]
		public void MOV_regmem32_reg32()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0xBFD4)), Register.ESI);

			// MOV DWORD [0xBFD4], esi
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x89, 0x36, 0xD4, 0xBF });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x89, 0x35, 0xD4, 0xBF, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x89, 0x34, 0x25, 0xD4, 0xBF, 0x00, 0x00 });
		}

		[Test]
		public void MOV_regmem64_reg64()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xB992)), Register.R12);

			// MOV QWORD [0xB992], r12
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x4C, 0x89, 0x24, 0x25, 0x92, 0xB9, 0x00, 0x00 });
		}

		[Test]
		public void MOV_reg8_regmem8()
		{
			var instruction = Instr.Mov(Register.CL, new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x302C)));

			// MOV cl, BYTE [0x302C]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x8A, 0x0E, 0x2C, 0x30 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x8A, 0x0D, 0x2C, 0x30, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x8A, 0x0C, 0x25, 0x2C, 0x30, 0x00, 0x00 });
		}

		[Test]
		public void MOV_reg16_regmem16()
		{
			var instruction = Instr.Mov(Register.DX, new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x36C5)));

			// MOV dx, WORD [0x36C5]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x8B, 0x16, 0xC5, 0x36 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x8B, 0x15, 0xC5, 0x36, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x8B, 0x14, 0x25, 0xC5, 0x36, 0x00, 0x00 });
		}

		[Test]
		public void MOV_reg32_regmem32()
		{
			var instruction = Instr.Mov(Register.ECX, new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x129F)));

			// MOV ecx, DWORD [0x129F]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x8B, 0x0E, 0x9F, 0x12 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x8B, 0x0D, 0x9F, 0x12, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x8B, 0x0C, 0x25, 0x9F, 0x12, 0x00, 0x00 });
		}

		[Test]
		public void MOV_reg64_regmem64()
		{
			var instruction = Instr.Mov(Register.RCX, new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xDCD)));

			// MOV rcx, QWORD [0xDCD]
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x8B, 0x0C, 0x25, 0xCD, 0x0D, 0x00, 0x00 });
		}

		[Test]
		public void MOV_reg8_imm8()
		{
			var instruction = Instr.Mov(Register.BL, (byte)0xB8);

			// MOV bl, BYTE 0xB8
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xB3, 0xB8 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xB3, 0xB8 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xB3, 0xB8 });
		}

		[Test]
		public void MOV_reg16_imm16()
		{
			var instruction = Instr.Mov(Register.BP, (ushort)0x9FDF);

			// MOV bp, WORD 0x9FDF
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xBD, 0xDF, 0x9F });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0xBD, 0xDF, 0x9F });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0xBD, 0xDF, 0x9F });
		}

		[Test]
		public void MOV_reg32_imm32()
		{
			var instruction = Instr.Mov(Register.ESP, (uint)0x6352);

			// MOV esp, DWORD 0x6352
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0xBC, 0x52, 0x63, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xBC, 0x52, 0x63, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xBC, 0x52, 0x63, 0x00, 0x00 });
		}

		[Test]
		public void MOV_reg64_imm64()
		{
			var instruction = Instr.Mov(Register.RSP, (ulong)0xDBA8000038A2);

			// MOV rsp, QWORD 0xDBA8000038A2
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0xBC, 0xA2, 0x38, 0x00, 0x00, 0xA8, 0xDB, 0x00, 0x00 });
		}

		[Test]
		public void MOV_regmem8_imm8()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x47F4)), (byte)0x47);

			// MOV BYTE [0x47F4], BYTE 0x47
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xC6, 0x06, 0xF4, 0x47, 0x47 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xC6, 0x05, 0xF4, 0x47, 0x00, 0x00, 0x47 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xC6, 0x04, 0x25, 0xF4, 0x47, 0x00, 0x00, 0x47 });
		}

		[Test]
		public void MOV_regmem16_imm16()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xFBC5)), (ushort)0xFBC5);

			// MOV WORD [0xFBC5], WORD 0xFBC5
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xC7, 0x06, 0xC5, 0xFB, 0xC5, 0xFB });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0xC7, 0x05, 0xC5, 0xFB, 0x00, 0x00, 0xC5, 0xFB });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0xC7, 0x04, 0x25, 0xC5, 0xFB, 0x00, 0x00, 0xC5, 0xFB });
		}

		[Test]
		public void MOV_regmem32_imm32()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x69AF)), (uint)0x6918);

			// MOV DWORD [0x69AF], DWORD 0x6918
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0xC7, 0x06, 0xAF, 0x69, 0x18, 0x69, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xC7, 0x05, 0xAF, 0x69, 0x00, 0x00, 0x18, 0x69, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xC7, 0x04, 0x25, 0xAF, 0x69, 0x00, 0x00, 0x18, 0x69, 0x00, 0x00 });
		}

		[Test]
		public void MOV_regmem64_imm32()
		{
			var instruction = Instr.Mov(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0x83A3)), (uint)0x8327);

			// MOV QWORD [0x83A3], DWORD 0x8327
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0xC7, 0x04, 0x25, 0xA3, 0x83, 0x00, 0x00, 0x27, 0x83, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
