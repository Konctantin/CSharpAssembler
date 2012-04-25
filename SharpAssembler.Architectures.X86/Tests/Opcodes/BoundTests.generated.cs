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
	/// Tests all variants of the BOUND opcode.
	/// </summary>
	[TestFixture]
	public class BoundTests : OpcodeTestBase
	{
		[Test]
		public void BOUND_reg16_mem16()
		{
			var instruction = Instr.Bound(Register.CX, new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(0x786D)));

			// BOUND cx, WORD [0x786D]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x62, 0x0E, 0x6D, 0x78 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x66, 0x62, 0x0D, 0x6D, 0x78, 0x00, 0x00 });
			AssertInstructionFail(instruction, DataSize.Bit64);
		}

		[Test]
		public void BOUND_reg32_mem32()
		{
			var instruction = Instr.Bound(Register.ECX, new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(0x7F6D)));

			// BOUND ecx, DWORD [0x7F6D]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x62, 0x0E, 0x6D, 0x7F });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x62, 0x0D, 0x6D, 0x7F, 0x00, 0x00 });
			AssertInstructionFail(instruction, DataSize.Bit64);
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
