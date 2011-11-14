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
using NUnit.Framework;
using System;
using System.Linq;

namespace SharpAssembler.Core.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="DeclareData{T}"/> class.
	/// </summary>
	[TestFixture]
	public class DeclareData_1Tests : InstructionTestsBase
	{
		/// <summary>
		/// Tests whether the <see cref="DeclareData{T}"/> instruction emits the result of the expression.
		/// </summary>
		[Test]
		public void EmitsData()
		{
			var instr = new DeclareData<ushort>(0xABCD, 0xEF01);
			Assert.AreEqual(new ushort[]{0xABCD, 0xEF01}, instr.Data);


			var emittable = instr.Construct(Context).First() as RawEmittable;
			Assert.AreEqual(new byte[]{0xCD, 0xAB, 0x01, 0xEF}, emittable.Content);
		}
	}
}
