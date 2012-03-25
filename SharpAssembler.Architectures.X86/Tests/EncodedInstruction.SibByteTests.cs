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

namespace SharpAssembler.Architectures.X86.Tests
{
	/// <summary>
	/// Tests the <see cref="EncodedInstruction.SibByte"/> class.
	/// </summary>
	[TestFixture]
	public class EncodedInstruction_SubByteTests
	{
		/// <summary>
		/// Tests the <see cref="EncodedInstruction.SibByte.ToBytes"/> method.
		/// </summary>
		[Test]
		public void ToBytesTest()
		{
			byte[] bytes;
			EncodedInstruction.SibByte instance;

			// The default Mod R/M byte is zero.
			instance = new EncodedInstruction.SibByte();
			Assert.AreEqual(0x0, instance.Base);
			Assert.AreEqual(0x0, instance.Index);
			Assert.AreEqual(0x0, instance.Scale);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0x00 }, bytes);

			// The BASE part. Note that bit 4 is put in the REX byte.
			instance = new EncodedInstruction.SibByte(0xF, 0x0, 0x0);
			Assert.AreEqual(0xF, instance.Base);
			Assert.AreEqual(0x0, instance.Index);
			Assert.AreEqual(0x0, instance.Scale);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0x07 }, bytes);

			// The INDEX part. Note that bit 4 is put in the REX byte.
			instance = new EncodedInstruction.SibByte(0x0, 0xF, 0x0);
			Assert.AreEqual(0x0, instance.Base);
			Assert.AreEqual(0xF, instance.Index);
			Assert.AreEqual(0x0, instance.Scale);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0x38 }, bytes);

			// The SCALE part.
			instance = new EncodedInstruction.SibByte(0x0, 0x0, 0x3);
			Assert.AreEqual(0x0, instance.Base);
			Assert.AreEqual(0x0, instance.Index);
			Assert.AreEqual(0x3, instance.Scale);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0xC0 }, bytes);

			// All parts together.
			instance = new EncodedInstruction.SibByte(0x5, 0x5, 0x2);
			Assert.AreEqual(0x5, instance.Base);
			Assert.AreEqual(0x5, instance.Index);
			Assert.AreEqual(0x2, instance.Scale);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0xAD }, bytes);
		}
	}
}
