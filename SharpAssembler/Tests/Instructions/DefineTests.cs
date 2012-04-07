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
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace SharpAssembler.Core.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Define"/> class.
	/// </summary>
	[TestFixture]
	public class DefineTests : InstructionTestsBase
	{
		/// <summary>
		/// Tests whether the <see cref="Define"/> instruction correctly adds a symbol to the context.
		/// </summary>
		[Test]
		public void AddsSymbol()
		{
			Expression<Func<Context, SimpleExpression>> expression = (context) => new SimpleExpression(context.Address + 3);

			var instr = new Define("test", expression);
			Assert.AreEqual("test", instr.DefinedSymbol.Identifier);
			Assert.AreEqual(LabelType.Private, instr.DefinedSymbol.SymbolType);
			Assert.AreEqual(expression, instr.Expression);

			Context.Address = 5;

			Assert.IsEmpty(instr.Construct(Context).ToList());
			Assert.AreEqual((Int128)8, Context.SymbolTable["test"].Value);
		}
	}
}
