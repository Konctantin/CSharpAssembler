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
using SharpAssembler.Instructions;
using NUnit.Framework;
using System.Linq;

namespace SharpAssembler.Core.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Comment"/> class.
	/// </summary>
	[TestFixture]
	public class CommentTests : InstructionTestsBase
	{
		/// <summary>
		/// Tests the general use of the <see cref="Comment"/> 'instruction'.
		/// </summary>
		[Test]
		public void CommentTest()
		{
			// Test the Text property's getter and setter and the constructor.
			var instr = new Comment("Test 1");
			Assert.AreEqual("Test 1", instr.Text);
#if OPERAND_SET
			instr.Text = "Test 2";
			Assert.AreEqual("Test 2", instr.Text);
#endif

			// The 'instruction' has no representation.
			Assert.IsEmpty(instr.Construct(this.Context).ToList());
		}
	}
}
