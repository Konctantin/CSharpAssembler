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
using Moq;
using NUnit.Framework;

namespace SharpAssembler.Architectures.X86.Tests
{
	/// <summary>
	/// Tests the <see cref="EncodedInstruction.SubStructure"/> class.
	/// </summary>
	[TestFixture]
	public class EncodedInstruction_SubStructureTests
	{
		/// <summary>
		/// Tests the <see cref="EncodedInstruction.SubStructure.CopyTo"/> method.
		/// </summary>
		[Test]
		public void CopyToTest()
		{
			var classMock = new Mock<EncodedInstruction.SubStructure>();
			classMock.Setup(obj => obj.ToBytes()).Returns(new byte[] { 0xAB, 0xCD, 0xEF });
			var instance = classMock.Object;
			byte[] target;
			int written;

			// Copying into an array.
			target = new byte[]{0x12, 0x34, 0x56, 0x78, 0x90};
			written = instance.CopyTo(target, 1);
			Assert.AreEqual(3, written);
			Assert.AreEqual(new byte[] { 0x12, 0xAB, 0xCD, 0xEF, 0x90 }, target);

			// Copying past the length of the array.
			target = new byte[] { 0x12, 0x34};
			written = instance.CopyTo(target, 2);
			Assert.AreEqual(3, written);
			Assert.AreEqual(new byte[] { 0x12, 0x34, 0xAB, 0xCD, 0xEF }, target);

			// Limited copying into an array.
			target = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 };
			written = instance.CopyTo(target, 2, 2);
			Assert.AreEqual(2, written);
			Assert.AreEqual(new byte[] { 0x12, 0x34, 0xAB, 0xCD, 0x90}, target);

			// Limited copying past an array.
			target = new byte[] { 0x12, 0x34, 0x56 };
			written = instance.CopyTo(target, 2, 2);
			Assert.AreEqual(2, written);
			Assert.AreEqual(new byte[] { 0x12, 0x34, 0x56, 0xAB, 0xCD }, target);

			// Unlimited limited copying.
			target = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 };
			written = instance.CopyTo(target, 1, 4);
			Assert.AreEqual(3, written);
			Assert.AreEqual(new byte[] { 0x12, 0xAB, 0xCD, 0xEF, 0x90 }, target);
		}

		/// <summary>
		/// Tests the <see cref="EncodedInstruction.SubStructure.ToString"/> method.
		/// </summary>
		[Test]
		public void ToStringTest()
		{
			var classMock = new Mock<EncodedInstruction.SubStructure>();
			classMock.Setup(obj => obj.ToBytes()).Returns(new byte[] { 0xAB, 0xCD, 0xEF });
			var instance = classMock.Object;

			var result = instance.ToString();
			Assert.AreEqual("{0xAB, 0xCD, 0xEF}", result);
		}
	}
}
