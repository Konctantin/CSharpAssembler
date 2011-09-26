#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011 Daniël Pelsmaeker
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
using NUnit.Framework;
using SharpAssembler.Core;
using SharpAssembler.x86.Instructions;

namespace SharpAssembler.x86.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Cmps"/> instruction.
	/// </summary>
	[TestFixture]
	public class CmpsTest : InstructionTestBase
	{
		/// <summary>
		/// Tests the <c>cmps mem8, mem8</c> instruction variant.
		/// </summary>
		[Test]
		public void Cmps_mem8_mem8()
		{
			var instruction = new Cmps(DataSize.Bit8);

			Assert16BitInstruction(instruction,
				new byte[] { 0xA6 });
			Assert32BitInstruction(instruction,
				new byte[] { 0xA6 });
			Assert64BitInstruction(instruction,
				new byte[] { 0xA6 });
		}

		/// <summary>
		/// Tests the <c>cmps mem16, mem16</c> instruction variant.
		/// </summary>
		[Test]
		public void Cmps_mem16_mem16()
		{
			var instruction = new Cmps(DataSize.Bit16);

			Assert16BitInstruction(instruction,
				new byte[] { 0xA7 });
			Assert32BitInstruction(instruction,
				new byte[] { 0x66, 0xA7 });
			Assert64BitInstruction(instruction,
				new byte[] { 0x66, 0xA7 });
		}

		/// <summary>
		/// Tests the <c>cmps mem32, mem32</c> instruction variant.
		/// </summary>
		[Test]
		public void Cmps_mem32_mem32()
		{
			var instruction = new Cmps(DataSize.Bit32);

			Assert16BitInstruction(instruction,
				new byte[] { 0x66, 0xA7 });
			Assert32BitInstruction(instruction,
				new byte[] { 0xA7 });
			Assert64BitInstruction(instruction,
				new byte[] { 0xA7 });
		}

		/// <summary>
		/// Tests the <c>cmps mem64, mem64</c> instruction variant.
		/// </summary>
		[Test]
		public void Cmps_mem64_mem64()
		{
			var instruction = new Cmps(DataSize.Bit64);

			Assert16BitInstructionFails(instruction);
			Assert32BitInstructionFails(instruction);
			Assert64BitInstruction(instruction,
				new byte[] { 0x48, 0xA7 });
		}
	}
}
