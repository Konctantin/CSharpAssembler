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
	/// Tests all variants of the INC opcode.
	/// </summary>
	[TestFixture]
	public class IncTests : OpcodeTestBase
	{
		[Test]
		public void INC_reg16()
		{
			var instruction = Instr.Inc(Register.DX);

			// INC dx
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x42 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x42 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0xFF, 0xC2 });
		}

		[Test]
		public void INC_reg32()
		{
			var instruction = Instr.Inc(Register.EDX);

			// INC edx
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x42 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x42 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xFF, 0xC2 });
		}

		[Test]
		public void INC_regmem8()
		{
			var instruction = Instr.Inc(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0xDA92)));

			// INC BYTE [0xDA92]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xFE, 0x06, 0x92, 0xDA });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xFE, 0x05, 0x92, 0xDA, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xFE, 0x04, 0x25, 0x92, 0xDA, 0x00, 0x00 });
		}

		[Test]
		public void INC_regmem16()
		{
			var instruction = Instr.Inc(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x2FD2)));

			// INC WORD [0x2FD2]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xFF, 0x06, 0xD2, 0x2F });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0xFF, 0x05, 0xD2, 0x2F, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0xFF, 0x04, 0x25, 0xD2, 0x2F, 0x00, 0x00 });
		}

		[Test]
		public void INC_regmem32()
		{
			var instruction = Instr.Inc(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x2A79)));

			// INC DWORD [0x2A79]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0xFF, 0x06, 0x79, 0x2A });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xFF, 0x05, 0x79, 0x2A, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xFF, 0x04, 0x25, 0x79, 0x2A, 0x00, 0x00 });
		}

		[Test]
		public void INC_regmem64()
		{
			var instruction = Instr.Inc(new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(0x1DC3)));

			// INC QWORD [0x1DC3]
			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0xFF, 0x04, 0x25, 0xC3, 0x1D, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
