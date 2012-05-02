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
	/// Tests all variants of the IMUL opcode.
	/// </summary>
	[TestFixture]
	public class ImulTests : OpcodeTestBase
	{
		[Test]
		public void IMUL_regmem8()
		{
			var instruction = Instr.Imul(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0xDA92)));

			// IMUL BYTE [0xDA92]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xF6, 0x2E, 0x92, 0xDA });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xF6, 0x2D, 0x92, 0xDA, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xF6, 0x2C, 0x25, 0x92, 0xDA, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_regmem16()
		{
			var instruction = Instr.Imul(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x2FD2)));

			// IMUL WORD [0x2FD2]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xF7, 0x2E, 0xD2, 0x2F });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0xF7, 0x2D, 0xD2, 0x2F, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0xF7, 0x2C, 0x25, 0xD2, 0x2F, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_regmem32()
		{
			var instruction = Instr.Imul(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x2A79)));

			// IMUL DWORD [0x2A79]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0xF7, 0x2E, 0x79, 0x2A });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xF7, 0x2D, 0x79, 0x2A, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xF7, 0x2C, 0x25, 0x79, 0x2A, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_regmem64()
		{
			var instruction = Instr.Imul(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0x1DC3)));

			// IMUL QWORD [0x1DC3]
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0xF7, 0x2C, 0x25, 0xC3, 0x1D, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_reg16_regmem16()
		{
			var instruction = Instr.Imul(Register.DX, new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x36C5)));

			// IMUL dx, WORD [0x36C5]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x0F, 0xAF, 0x16, 0xC5, 0x36 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x0F, 0xAF, 0x15, 0xC5, 0x36, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x0F, 0xAF, 0x14, 0x25, 0xC5, 0x36, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_reg32_regmem32()
		{
			var instruction = Instr.Imul(Register.ECX, new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x129F)));

			// IMUL ecx, DWORD [0x129F]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x0F, 0xAF, 0x0E, 0x9F, 0x12 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x0F, 0xAF, 0x0D, 0x9F, 0x12, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x0F, 0xAF, 0x0C, 0x25, 0x9F, 0x12, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_reg64_regmem64()
		{
			var instruction = Instr.Imul(Register.RCX, new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xDCD)));

			// IMUL rcx, QWORD [0xDCD]
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x0F, 0xAF, 0x0C, 0x25, 0xCD, 0x0D, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_reg16_regmem16_imm8()
		{
			var instruction = Instr.Imul(Register.SI, new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xB3D1)), (byte)0xB3);

			// IMUL si, WORD [0xB3D1], BYTE 0xB3
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x6B, 0x36, 0xD1, 0xB3, 0xB3 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x6B, 0x35, 0xD1, 0xB3, 0x00, 0x00, 0xB3 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x6B, 0x34, 0x25, 0xD1, 0xB3, 0x00, 0x00, 0xB3 });
		}

		[Test]
		public void IMUL_reg32_regmem32_imm8()
		{
			var instruction = Instr.Imul(Register.EDX, new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x35F8)), (byte)0x35);

			// IMUL edx, DWORD [0x35F8], BYTE 0x35
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x6B, 0x16, 0xF8, 0x35, 0x35 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x6B, 0x15, 0xF8, 0x35, 0x00, 0x00, 0x35 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x6B, 0x14, 0x25, 0xF8, 0x35, 0x00, 0x00, 0x35 });
		}

		[Test]
		public void IMUL_reg64_regmem64_imm8()
		{
			var instruction = Instr.Imul(Register.RDX, new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0x1C80)), (byte)0x1B);

			// IMUL rdx, QWORD [0x1C80], BYTE 0x1B
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x6B, 0x14, 0x25, 0x80, 0x1C, 0x00, 0x00, 0x1B });
		}

		[Test]
		public void IMUL_reg16_regmem16_imm16()
		{
			var instruction = Instr.Imul(Register.SI, new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xA15C)), (ushort)0xA15C);

			// IMUL si, WORD [0xA15C], WORD 0xA15C
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x69, 0x36, 0x5C, 0xA1, 0x5C, 0xA1 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x69, 0x35, 0x5C, 0xA1, 0x00, 0x00, 0x5C, 0xA1 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x69, 0x34, 0x25, 0x5C, 0xA1, 0x00, 0x00, 0x5C, 0xA1 });
		}

		[Test]
		public void IMUL_reg32_regmem32_imm32()
		{
			var instruction = Instr.Imul(Register.EDI, new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0xC7FC)), (uint)0xC7C4);

			// IMUL edi, DWORD [0xC7FC], DWORD 0xC7C4
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x69, 0x3E, 0xFC, 0xC7, 0xC4, 0xC7, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x69, 0x3D, 0xFC, 0xC7, 0x00, 0x00, 0xC4, 0xC7, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x69, 0x3C, 0x25, 0xFC, 0xC7, 0x00, 0x00, 0xC4, 0xC7, 0x00, 0x00 });
		}

		[Test]
		public void IMUL_reg64_regmem64_imm32()
		{
			var instruction = Instr.Imul(Register.R11, new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xA629)), (uint)0xA5CF);

			// IMUL r11, QWORD [0xA629], DWORD 0xA5CF
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x4C, 0x69, 0x1C, 0x25, 0x29, 0xA6, 0x00, 0x00, 0xCF, 0xA5, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
