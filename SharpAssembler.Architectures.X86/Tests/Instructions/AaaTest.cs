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
using NUnit.Framework;
using SharpAssembler.Architectures.X86.Instructions;

namespace SharpAssembler.Architectures.X86.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Aaa"/> instruction.
	/// </summary>
	[TestFixture]
	public class AaaTest : InstructionTestBase
	{
		/// <summary>
		/// Tests the <c>aaa</c> instruction variant.
		/// </summary>
		[Test]
		public void Aaa()
		{
			var instruction = new Aaa();

			Assert16BitInstruction(instruction,
				new byte[] { 0x37 });
			Assert32BitInstruction(instruction,
				new byte[] { 0x37 });
			Assert64BitInstructionFails(instruction);
		}
	}
}
