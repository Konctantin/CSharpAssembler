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
using NUnit.Framework;

namespace SharpAssembler.Core.Tests
{
	/// <summary>
	/// Tests for the <see cref="ObjectFile"/> class.
	/// </summary>
	[TestFixture]
	public class ObjectFileTests
	{
		/// <summary>
		/// Tests the <see cref="ObjectFile.AddNewSection"/> method.
		/// </summary>
		[Test]
		public void AddNewSectionTest()
		{
			ObjectFile objectfile = new ObjectFileMock();

			objectfile.AddNewSection(".text", SectionType.Program);
			Assert.IsTrue(objectfile[0].Allocate);
			Assert.IsTrue(objectfile[0].Executable);
			Assert.IsFalse(objectfile[0].Writable);
			Assert.IsFalse(objectfile[0].NoBits);

			objectfile.AddNewSection(".data", SectionType.Data);
			Assert.IsTrue(objectfile[1].Allocate);
			Assert.IsFalse(objectfile[1].Executable);
			Assert.IsTrue(objectfile[1].Writable);
			Assert.IsFalse(objectfile[1].NoBits);

			objectfile.AddNewSection(".bss", SectionType.Bss);
			Assert.IsTrue(objectfile[2].Allocate);
			Assert.IsFalse(objectfile[2].Executable);
			Assert.IsTrue(objectfile[2].Writable);
			Assert.IsTrue(objectfile[2].NoBits);
		}
	}
}
