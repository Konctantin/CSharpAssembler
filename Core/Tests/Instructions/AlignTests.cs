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
using SharpAssembler.Core.Instructions;
using System.Linq;
using NUnit.Framework;

namespace SharpAssembler.Core.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Align"/> class.
	/// </summary>
	[TestFixture]
	public class AlignTests : InstructionTestsBase
	{
		/// <summary>
		/// Tests whether the <see cref="Align"/> instruction emits 0x00 bytes.
		/// </summary>
		[Test]
		public void EmitsNullBytes()
		{
			var instr = new Align(4);
			Assert.AreEqual(0x00, instr.PaddingByte);
			Assert.AreEqual(4, instr.Boundary);

			Context.Address = 1;

			var emittable = instr.Construct(Context).First() as RawEmittable;
			Assert.AreEqual(new byte[]{0x00, 0x00, 0x00}, emittable.Content);

#if OPERAND_SET
			instr.Boundary = 8;
			emittable = instr.Construct(Context) as RawEmittable;
			Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, emittable.Content);
#endif
		}

		/// <summary>
		/// Tests whether the <see cref="Align"/> instruction emits the specified bytes.
		/// </summary>
		[Test]
		public void EmitsSpecifiedBytes()
		{
			var instr = new Align(8, 0xAB);
			Assert.AreEqual(0xAB, instr.PaddingByte);
			Assert.AreEqual(8, instr.Boundary);

			Context.Address = 5;
			var emittable = instr.Construct(Context).First() as RawEmittable;
			Assert.AreEqual(new byte[] { 0xAB, 0xAB, 0xAB }, emittable.Content);

#if OPERAND_SET
			instr.PaddingByte = 0xCD;
			emittable = instr.Construct(Context) as RawEmittable;
			Assert.AreEqual(new byte[] { 0xCD, 0xCD, 0xCD }, emittable.Content);
#endif
		}
	}
}
