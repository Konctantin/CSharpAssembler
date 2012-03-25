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
using SharpAssembler;
using NUnit.Framework;

namespace SharpAssembler.Core.Tests
{
	/// <summary>
	/// Tests for the <see cref="DataSizeExtensions"/> class.
	/// </summary>
	[TestFixture]
	public class DataSizeExtensionsTests
	{
		/// <summary>
		/// Tests that <see cref="DataSizeExtensions.GetBitCount"/> returns the expected values.
		/// </summary>
		public void GetBitCount_ReturnsExpectedValues()
		{
			Assert.That(DataSize.Bit8.GetBitCount(), Is.EqualTo(8));
			Assert.That(DataSize.Bit16.GetBitCount(), Is.EqualTo(16));
			Assert.That(DataSize.Bit32.GetBitCount(), Is.EqualTo(32));
			Assert.That(DataSize.Bit64.GetBitCount(), Is.EqualTo(64));
			Assert.That(DataSize.Bit80.GetBitCount(), Is.EqualTo(80));
			Assert.That(DataSize.Bit128.GetBitCount(), Is.EqualTo(128));
			Assert.That(DataSize.Bit256.GetBitCount(), Is.EqualTo(256));

			Assert.That(DataSize.None.GetBitCount(), Is.EqualTo(0));
		}
	}
}
