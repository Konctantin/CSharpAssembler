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
	/// Tests all variants of the ADD opcode.
	/// </summary>
	[TestFixture]
	public class AddTests : OpcodeTestBase
	{
		[Test]
		public void ADD_AL_imm8()
		{
			var instruction = Instr.Add(Register.AL, (byte)0x59);

			// ADD al, BYTE 0x59
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x04, 0x59 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x04, 0x59 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x04, 0x59 });
		}

		[Test]
		public void ADD_AX_imm16()
		{
			var instruction = Instr.Add(Register.AX, (ushort)0x19CA);

			// ADD ax, WORD 0x19CA
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x05, 0xCA, 0x19 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x05, 0xCA, 0x19 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x05, 0xCA, 0x19 });
		}

		[Test]
		public void ADD_EAX_imm32()
		{
			var instruction = Instr.Add(Register.EAX, (uint)0x1104);

			// ADD eax, DWORD 0x1104
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x05, 0x04, 0x11, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x05, 0x04, 0x11, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x05, 0x04, 0x11, 0x00, 0x00 });
		}

		[Test]
		public void ADD_RAX_imm32()
		{
			var instruction = Instr.Add(Register.RAX, (uint)0xBE38);

			// ADD rax, DWORD 0xBE38
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x05, 0x38, 0xBE, 0x00, 0x00 });
		}

		[Test]
		public void ADD_regmem8_imm8()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x47F4)), (byte)0x47);

			// ADD BYTE [0x47F4], BYTE 0x47
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x80, 0x06, 0xF4, 0x47, 0x47 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x80, 0x05, 0xF4, 0x47, 0x00, 0x00, 0x47 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x80, 0x04, 0x25, 0xF4, 0x47, 0x00, 0x00, 0x47 });
		}

		[Test]
		public void ADD_regmem16_imm8()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x7FCD)), (byte)0x7F);

			// ADD WORD [0x7FCD], BYTE 0x7F
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x83, 0x06, 0xCD, 0x7F, 0x7F });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x83, 0x05, 0xCD, 0x7F, 0x00, 0x00, 0x7F });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x83, 0x04, 0x25, 0xCD, 0x7F, 0x00, 0x00, 0x7F });
		}

		[Test]
		public void ADD_regmem32_imm8()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x2B86)), (byte)0x2A);

			// ADD DWORD [0x2B86], BYTE 0x2A
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x83, 0x06, 0x86, 0x2B, 0x2A });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x83, 0x05, 0x86, 0x2B, 0x00, 0x00, 0x2A });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x83, 0x04, 0x25, 0x86, 0x2B, 0x00, 0x00, 0x2A });
		}

		[Test]
		public void ADD_regmem64_imm8()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0x6B4C)), (byte)0x6A);

			// ADD QWORD [0x6B4C], BYTE 0x6A
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x83, 0x04, 0x25, 0x4C, 0x6B, 0x00, 0x00, 0x6A });
		}

		[Test]
		public void ADD_regmem16_imm16()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xFBC5)), (ushort)0xFBC5);

			// ADD WORD [0xFBC5], WORD 0xFBC5
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x81, 0x06, 0xC5, 0xFB, 0xC5, 0xFB });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x81, 0x05, 0xC5, 0xFB, 0x00, 0x00, 0xC5, 0xFB });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x81, 0x04, 0x25, 0xC5, 0xFB, 0x00, 0x00, 0xC5, 0xFB });
		}

		[Test]
		public void ADD_regmem32_imm32()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x69AF)), (uint)0x6918);

			// ADD DWORD [0x69AF], DWORD 0x6918
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x81, 0x06, 0xAF, 0x69, 0x18, 0x69, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x81, 0x05, 0xAF, 0x69, 0x00, 0x00, 0x18, 0x69, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x81, 0x04, 0x25, 0xAF, 0x69, 0x00, 0x00, 0x18, 0x69, 0x00, 0x00 });
		}

		[Test]
		public void ADD_regmem64_imm32()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0x83A3)), (uint)0x8327);

			// ADD QWORD [0x83A3], DWORD 0x8327
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x81, 0x04, 0x25, 0xA3, 0x83, 0x00, 0x00, 0x27, 0x83, 0x00, 0x00 });
		}

		[Test]
		public void ADD_regmem8_reg8()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x165E)), Register.CL);

			// ADD BYTE [0x165E], cl
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x00, 0x0E, 0x5E, 0x16 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x00, 0x0D, 0x5E, 0x16, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x00, 0x0C, 0x25, 0x5E, 0x16, 0x00, 0x00 });
		}

		[Test]
		public void ADD_regmem16_reg16()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xCEEA)), Register.DI);

			// ADD WORD [0xCEEA], di
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x01, 0x3E, 0xEA, 0xCE });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x01, 0x3D, 0xEA, 0xCE, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x01, 0x3C, 0x25, 0xEA, 0xCE, 0x00, 0x00 });
		}

		[Test]
		public void ADD_regmem32_reg32()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0xBFD4)), Register.ESI);

			// ADD DWORD [0xBFD4], esi
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x01, 0x36, 0xD4, 0xBF });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x01, 0x35, 0xD4, 0xBF, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x01, 0x34, 0x25, 0xD4, 0xBF, 0x00, 0x00 });
		}

		[Test]
		public void ADD_regmem64_reg64()
		{
			var instruction = Instr.Add(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xB992)), Register.R12);

			// ADD QWORD [0xB992], r12
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x4C, 0x01, 0x24, 0x25, 0x92, 0xB9, 0x00, 0x00 });
		}

		[Test]
		public void ADD_reg8_regmem8()
		{
			var instruction = Instr.Add(Register.CL, new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x302C)));

			// ADD cl, BYTE [0x302C]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x02, 0x0E, 0x2C, 0x30 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x02, 0x0D, 0x2C, 0x30, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x02, 0x0C, 0x25, 0x2C, 0x30, 0x00, 0x00 });
		}

		[Test]
		public void ADD_reg16_regmem16()
		{
			var instruction = Instr.Add(Register.DX, new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x36C5)));

			// ADD dx, WORD [0x36C5]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x03, 0x16, 0xC5, 0x36 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x03, 0x15, 0xC5, 0x36, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x03, 0x14, 0x25, 0xC5, 0x36, 0x00, 0x00 });
		}

		[Test]
		public void ADD_reg32_regmem32()
		{
			var instruction = Instr.Add(Register.ECX, new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x129F)));

			// ADD ecx, DWORD [0x129F]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x03, 0x0E, 0x9F, 0x12 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x03, 0x0D, 0x9F, 0x12, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x03, 0x0C, 0x25, 0x9F, 0x12, 0x00, 0x00 });
		}

		[Test]
		public void ADD_reg64_regmem64()
		{
			var instruction = Instr.Add(Register.RCX, new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xDCD)));

			// ADD rcx, QWORD [0xDCD]
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x03, 0x0C, 0x25, 0xCD, 0x0D, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
