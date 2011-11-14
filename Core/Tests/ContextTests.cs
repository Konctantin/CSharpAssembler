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
using SharpAssembler.Core.Symbols;
using NUnit.Framework;

namespace SharpAssembler.Core.Tests
{
	/// <summary>
	/// Tests the <see cref="Context"/> class.
	/// </summary>
	[TestFixture]
	public class ContextTests
	{
		/// <summary>
		/// Tests the <see cref="Context.Context"/> constructor.
		/// </summary>
		[Test]
		public void ConstructorTest()
		{
			var objectFile = new ObjectFileMock();
			Context context = new Context(objectFile);

			Assert.AreEqual(objectFile, context.Representation);
			Assert.IsNotNull(context.RelocationTable);
			Assert.IsNotNull(context.SymbolTable);
		}

		/// <summary>
		/// Tests the <see cref="Context.Section"/> property.
		/// </summary>
		[Test]
		public void SectionTest()
		{
			var objectFile = new ObjectFileMock();
			Context context = new Context(objectFile);

			Section section = new Section("Test");

			context.Section = section;
			Assert.AreEqual(section, context.Section);
		}

		/// <summary>
		/// Tests the <see cref="Context.Address"/> property.
		/// </summary>
		[Test]
		public void AddressTest()
		{
			var objectFile = new ObjectFileMock();
			Context context = new Context(objectFile);

			context.Address = 123456789;
			Assert.AreEqual((Int128)123456789, context.Address);
		}

		/// <summary>
		/// Tests the <see cref="Context.Reset"/> method.
		/// </summary>
		[Test]
		public void ResetTest()
		{
			var objectFile = new ObjectFileMock();
			Context context = new Context(objectFile);

			Section section = new Section("Test");
			var symbol = new Symbol(objectFile, SymbolType.Public, "test");
			var relocation = new Relocation(symbol, section, 0, 0, RelocationType.Default32);

			context.SymbolTable.Add(symbol);
			context.RelocationTable.Add(relocation);
			context.Section = section;
			context.Address = 123456789;
			Assert.AreEqual(symbol, context.SymbolTable[0]);
			Assert.AreEqual(relocation, context.RelocationTable[0]);
			Assert.AreEqual(section, context.Section);
			Assert.AreEqual((Int128)123456789, context.Address);

			context.Reset();

			// These have not changed
			Assert.AreEqual(symbol, context.SymbolTable[0]);
			Assert.AreEqual(relocation, context.RelocationTable[0]);

			// These are reset
			Assert.AreEqual(null, context.Section);
			Assert.AreEqual((Int128)0, context.Address);
		}
	}
}
