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
	/// Tests all variants of the CMPXCHG opcode.
	/// </summary>
	[TestFixture]
	public class CmpXchgTests : OpcodeTestBase
	{
		[Test]
		public void CMPXCHG_regmem8_reg8()
		{
			var instruction = Instr.CmpXchg(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x165E)), Register.CL);

			// CMPXCHG BYTE [0x165E], cl
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x0F, 0xB0, 0x0E, 0x5E, 0x16 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x0F, 0xB0, 0x0D, 0x5E, 0x16, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x0F, 0xB0, 0x0C, 0x25, 0x5E, 0x16, 0x00, 0x00 });
		}

		[Test]
		public void CMPXCHG_regmem16_reg16()
		{
			var instruction = Instr.CmpXchg(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0xCEEA)), Register.DI);

			// CMPXCHG WORD [0xCEEA], di
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x0F, 0xB1, 0x3E, 0xEA, 0xCE });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x0F, 0xB1, 0x3D, 0xEA, 0xCE, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0x0F, 0xB1, 0x3C, 0x25, 0xEA, 0xCE, 0x00, 0x00 });
		}

		[Test]
		public void CMPXCHG_regmem32_reg32()
		{
			var instruction = Instr.CmpXchg(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0xBFD4)), Register.ESI);

			// CMPXCHG DWORD [0xBFD4], esi
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x0F, 0xB1, 0x36, 0xD4, 0xBF });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x0F, 0xB1, 0x35, 0xD4, 0xBF, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x0F, 0xB1, 0x34, 0x25, 0xD4, 0xBF, 0x00, 0x00 });
		}

		[Test]
		public void CMPXCHG_regmem64_reg64()
		{
			var instruction = Instr.CmpXchg(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0xB992)), Register.R12);

			// CMPXCHG QWORD [0xB992], r12
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x4C, 0x0F, 0xB1, 0x24, 0x25, 0x92, 0xB9, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
