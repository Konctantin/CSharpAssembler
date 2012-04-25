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
	/// Tests all variants of the CALL FAR opcode.
	/// </summary>
	[TestFixture]
	public class CallFarTests : OpcodeTestBase
	{
		[Test]
		public void CALL_FAR_pntr16_16()
		{
			var instruction = Instr.CallFar(new FarPointer(c => 0xF46A, c => 0x99CE, DataSize.Bit16));

			// CALL FAR WORD 0xF46A:0x99CE
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x9A, 0xCE, 0x99, 0x6A, 0xF4 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x9A, 0xCE, 0x99, 0x6A, 0xF4 });
			AssertInstructionFail(instruction, DataSize.Bit64);
		}

		[Test]
		public void CALL_FAR_pntr16_32()
		{
			var instruction = Instr.CallFar(new FarPointer(c => 0xC113, c => 0x760D, DataSize.Bit32));

			// CALL FAR DWORD 0xC113:0x760D
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x9A, 0x0D, 0x76, 0x00, 0x00, 0x13, 0xC1 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x9A, 0x0D, 0x76, 0x00, 0x00, 0x13, 0xC1 });
			AssertInstructionFail(instruction, DataSize.Bit64);
		}

		[Test]
		public void CALL_FAR_mem16()
		{
			var instruction = Instr.CallFar(new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x55B6)));

			// CALL FAR WORD [0x55B6]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0xFF, 0x1E, 0xB6, 0x55 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0xFF, 0x1D, 0xB6, 0x55, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x66, 0xFF, 0x1C, 0x25, 0xB6, 0x55, 0x00, 0x00 });
		}

		[Test]
		public void CALL_FAR_mem32()
		{
			var instruction = Instr.CallFar(new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x505D)));

			// CALL FAR DWORD [0x505D]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0xFF, 0x1E, 0x5D, 0x50 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0xFF, 0x1D, 0x5D, 0x50, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0xFF, 0x1C, 0x25, 0x5D, 0x50, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
