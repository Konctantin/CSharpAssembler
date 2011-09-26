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
using SharpAssembler.Core;
using NUnit.Framework;
using SharpAssembler.Core.Symbols;
using SharpAssembler.Core.Instructions;

namespace SharpAssembler.Core.Tests
{
	/// <summary>
	/// Tests for the <see cref="LabelTypeExtensions"/> class.
	/// </summary>
	[TestFixture]
	public class LabelTypeExtensionsTests
	{
		/// <summary>
		/// Tests that <see cref="LabelTypeExtensions.ToSymbolType"/> returns the expected values.
		/// </summary>
		public void ToSymbolTypeTest()
		{
			Assert.AreEqual(SymbolType.Private, LabelTypeExtensions.ToSymbolType(LabelType.Private));
			Assert.AreEqual(SymbolType.Public, LabelTypeExtensions.ToSymbolType(LabelType.Public));
			Assert.AreEqual(SymbolType.Weak, LabelTypeExtensions.ToSymbolType(LabelType.Weak));			
		}
	}
}
