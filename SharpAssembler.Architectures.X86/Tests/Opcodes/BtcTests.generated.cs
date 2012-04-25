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
	/// Tests all variants of the BTC opcode.
	/// </summary>
	[TestFixture]
	public class BtcTests : OpcodeTestBase
	{
		[Test]
		public void BTC_reg_mem16_imm8()
		{
			var instruction = Instr.Btc(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x7FCD)), (byte)0x7F);

			// BTC WORD [0x7FCD], BYTE 0x7F
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x0F, 0xBA, 0x3E, 0xCD, 0x7F, 0x7F });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x0F, 0xBA, 0x3D, 0xCD, 0x7F, 0x00, 0x00, 0x7F });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x0F, 0xBA, 0x3C, 0x25, 0xCD, 0x7F, 0x00, 0x00, 0x7F });
		}

		[Test]
		public void BTC_reg_mem32_imm8()
		{
			var instruction = Instr.Btc(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x2B86)), (byte)0x2A);

			// BTC DWORD [0x2B86], BYTE 0x2A
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x0F, 0xBA, 0x3E, 0x86, 0x2B, 0x2A });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x0F, 0xBA, 0x3D, 0x86, 0x2B, 0x00, 0x00, 0x2A });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x0F, 0xBA, 0x3C, 0x25, 0x86, 0x2B, 0x00, 0x00, 0x2A });
		}

		[Test]
		public void BTC_reg_mem64_imm8()
		{
			var instruction = Instr.Btc(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0x6B4C)), (byte)0x6A);

			// BTC QWORD [0x6B4C], BYTE 0x6A
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x0F, 0xBA, 0x3C, 0x25, 0x4C, 0x6B, 0x00, 0x00, 0x6A });
		}

		[Test]
		public void BTC_reg_mem16_reg16()
		{
			var instruction = Instr.Btc(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xCEEA)), Register.CX);

			// BTC WORD [0xCEEA], cx
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x0F, 0xBB, 0x0E, 0xEA, 0xCE });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x0F, 0xBB, 0x0D, 0xEA, 0xCE, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x0F, 0xBB, 0x0C, 0x25, 0xEA, 0xCE, 0x00, 0x00 });
		}

		[Test]
		public void BTC_reg_mem32_reg32()
		{
			var instruction = Instr.Btc(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0xBFD4)), Register.ECX);

			// BTC DWORD [0xBFD4], ecx
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x0F, 0xBB, 0x0E, 0xD4, 0xBF });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x0F, 0xBB, 0x0D, 0xD4, 0xBF, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x0F, 0xBB, 0x0C, 0x25, 0xD4, 0xBF, 0x00, 0x00 });
		}

		[Test]
		public void BTC_reg_mem64_reg64()
		{
			var instruction = Instr.Btc(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xB992)), Register.RCX);

			// BTC QWORD [0xB992], rcx
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x0F, 0xBB, 0x0C, 0x25, 0x92, 0xB9, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
