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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Instructions;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="And"/> instruction.
	/// </summary>
	[TestFixture]
	public class AndTest : InstructionTestBase
	{
		/// <summary>
		/// Tests the <c>and AL, imm8</c> instruction variant.
		/// </summary>
		[Test]
		public void And_AL_imm8()
		{
			var instrString = "and AL, 123";
			var instruction = new And(new RegisterOperand(Register.AL), new Immediate(123, DataSize.Bit8));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
		}
	}
}
