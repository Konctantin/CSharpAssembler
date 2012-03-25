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
using SharpAssembler.Architectures.X86.Instructions;

namespace SharpAssembler.Architectures.X86.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Aas"/> instruction.
	/// </summary>
	[TestFixture]
	public class AasTest : InstructionTestBase
	{
		/// <summary>
		/// Tests the <c>aas</c> instruction variant.
		/// </summary>
		[Test]
		public void Aas()
		{
			var instruction = new Aas();

			Assert16BitInstruction(instruction,
				new byte[] { 0x3F });
			Assert32BitInstruction(instruction,
				new byte[] { 0x3F });
			Assert64BitInstructionFails(instruction);
		}
	}
}
