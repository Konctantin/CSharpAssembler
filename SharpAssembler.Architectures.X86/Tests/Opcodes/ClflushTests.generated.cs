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
	/// Tests all variants of the CLFLUSH opcode.
	/// </summary>
	[TestFixture]
	public class ClflushTests : OpcodeTestBase
	{
		[Test]
		public void CLFLUSH_mem8()
		{
			var instruction = Instr.Clflush(new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(0x94CF)));

			// CLFLUSH BYTE [0x94CF]
			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x0F, 0xAE, 0x3E, 0xCF, 0x94 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x0F, 0xAE, 0x3D, 0xCF, 0x94, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x0F, 0xAE, 0x3C, 0x25, 0xCF, 0x94, 0x00, 0x00 });
		}
	}
}

//////////////////////////////////////////////////////
//                     WARNING                      //
//     The contents of this file is generated.      //
//    DO NOT MODIFY, your changes will be lost!     //
//////////////////////////////////////////////////////
