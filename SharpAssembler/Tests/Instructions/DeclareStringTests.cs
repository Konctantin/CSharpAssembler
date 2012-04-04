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
using System;
using System.Linq;
using System.Text;

namespace SharpAssembler.Core.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="DeclareString"/> class.
	/// </summary>
	[TestFixture]
	public class DeclareStringTests : InstructionTestsBase
	{
		/// <summary>
		/// Tests whether the <see cref="DeclareString"/> instruction emits the result of the expression.
		/// </summary>
		[Test]
		public void EmitsData()
		{
			var str = "Hèlló wòrld!";
			var encoding = Encoding.UTF32;

			var instr = new DeclareString(str, encoding);
			Assert.AreEqual(str, instr.Data);
			Assert.AreEqual(encoding, instr.Encoding);

			var emittable = instr.Construct(Context).First() as RawEmittable;
			Assert.AreEqual(encoding.GetBytes(str), emittable.Content);
		}
	}
}
