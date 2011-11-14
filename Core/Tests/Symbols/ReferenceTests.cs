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
using Moq;
using NUnit.Framework;
using SharpAssembler.Core.Symbols;

namespace SharpAssembler.Core.Tests.Symbols
{
	/// <summary>
	/// Tests the <see cref="Reference"/> class.
	/// </summary>
	[TestFixture]
	public class ReferenceTests
	{
		/// <summary>
		/// Tests the <see cref="Reference.Resolve"/> method.
		/// </summary>
		[Test]
		public void ResolveTest()
		{
			var associatableMock = new Mock<IAssociatable>();
			IAssociatable associatable = associatableMock.Object;

			var symbol1 = new Symbol(associatable, SymbolType.Private, "id1");
			var symbol2 = new Symbol(associatable, SymbolType.Private, "id2");

			Context context = new Context(new ObjectFileMock());

			var reference1 = new Reference("id1");
			var reference2 = new Reference(symbol2);

			Assert.IsFalse(reference1.Resolved);
			Assert.IsFalse(reference1.Resolve(context));
			context.SymbolTable.Add(symbol1);
			Assert.IsTrue(reference1.Resolve(context));
			Assert.IsTrue(reference1.Resolved);

			Assert.IsFalse(reference2.Resolved);
			Assert.IsFalse(reference2.Resolve(context));
			context.SymbolTable.Add(symbol2);
			Assert.IsTrue(reference2.Resolve(context));
			Assert.IsTrue(reference2.Resolved);
		}
	}
}
