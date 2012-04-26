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
using Moq;
using NUnit.Framework;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Tests.Opcodes
{
	/// <summary>
	/// Tests all variants of the CMOVG opcode.
	/// </summary>
	[TestFixture]
	public class CMovGTests : OpcodeTestBase
	{
		[Test]
		public void CMOVG_reg16_regmem16()
		{
			var instruction = Instr.CMovG(Register.CX, new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x36C5)));

			// CMOVG cx, WORD [0x36C5]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x0F, 0x4F, 0x0E, 0xC5, 0x36 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x0F, 0x4F, 0x0D, 0xC5, 0x36, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x0F, 0x4F, 0x0C, 0x25, 0xC5, 0x36, 0x00, 0x00 });
		}

		[Test]
		public void CMOVG_reg32_regmem32()
		{
			var instruction = Instr.CMovG(Register.ECX, new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x129F)));

			// CMOVG ecx, DWORD [0x129F]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x0F, 0x4F, 0x0E, 0x9F, 0x12 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x0F, 0x4F, 0x0D, 0x9F, 0x12, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x0F, 0x4F, 0x0C, 0x25, 0x9F, 0x12, 0x00, 0x00 });
		}

		[Test]
		public void CMOVG_reg64_regmem64()
		{
			var instruction = Instr.CMovG(Register.RCX, new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xDCD)));

			// CMOVG rcx, QWORD [0xDCD]
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x0F, 0x4F, 0x0C, 0x25, 0xCD, 0x0D, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
