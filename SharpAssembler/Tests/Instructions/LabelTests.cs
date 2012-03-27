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
using SharpAssembler.Instructions;
using NUnit.Framework;
using System;
using System.Text;
using System.Linq;
using SharpAssembler.Symbols;

namespace SharpAssembler.Core.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Label"/> class.
	/// </summary>
	[TestFixture]
	public class LabelTests : InstructionTestsBase
	{
		/// <summary>
		/// Tests whether the <see cref="Label"/> instruction correctly adds a symbol to the context.
		/// </summary>
		[Test]
		public void AddsSymbol()
		{
			var instr = new Label("test", SymbolType.Public);
			Assert.AreEqual("test", instr.DefinedSymbol.Identifier);
			Assert.AreEqual(LabelType.Public, instr.DefinedSymbol.SymbolType);

			Context.Address = 5;

			Assert.IsEmpty(instr.Construct(Context).ToList());
			Assert.AreEqual(SymbolType.Public, Context.SymbolTable["test"].SymbolType);
			Assert.AreEqual((Int128)5, Context.SymbolTable["test"].Value);
		}
	}
}
