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
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SharpAssembler.Core.Symbols;

namespace SharpAssembler.Core.Tests.Symbols
{
	/// <summary>
	/// Tests the <see cref="SymbolTable"/> class.
	/// </summary>
	[TestFixture]
	public class SymbolTableTests
	{
		/// <summary>
		/// Tests that null objects cannot be added to the <see cref="SymbolTable"/>.
		/// </summary>
		[Theory]
		public void NullObjectsCannotBeAdded()
		{
			SymbolTable table = new SymbolTable();
			Assert.Throws<NullReferenceException>(() => table.Add(null));
		}

		/// <summary>
		/// Tests that the <see cref="SymbolTable.SymbolTable(IEnumerable{T})"/> constructor
		/// correctly adds the collection of symbols to the table.
		/// </summary>
		[Test]
		public void Constructor_IEnumerable()
		{
			var associatableMock = new Mock<IAssociatable>();
			IAssociatable associatable = associatableMock.Object;

			var symbols = new Symbol[]{
				new Symbol(associatable, SymbolType.Private, "id1"),
				new Symbol(associatable, SymbolType.Public, "id2")
			};

			SymbolTable table = new SymbolTable(symbols);
			Assert.AreEqual(symbols.Length, table.Count);
			Assert.AreEqual(symbols, table.ToArray());
		}

		/// <summary>
		/// Tests that <see cref="SymbolTable.AddRange"/> correctly adds the specified items.
		/// </summary>
		[Test]
		public void AddRange()
		{
			var associatableMock = new Mock<IAssociatable>();
			IAssociatable associatable = associatableMock.Object;

			var symbols = new Symbol[]{
				new Symbol(associatable, SymbolType.Private, "id1"),
				new Symbol(associatable, SymbolType.Public, "id2")
			};

			SymbolTable table = new SymbolTable();
			Assert.AreEqual(0, table.Count);
			table.AddRange(symbols);

			Assert.AreEqual(symbols.Length, table.Count);
			Assert.AreEqual(symbols, table.ToArray());
		}
	}
}
