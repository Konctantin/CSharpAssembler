﻿#region Copyright and License
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

namespace SharpAssembler.x86.Tests
{
	/// <summary>
	/// Tests the <see cref="EncodedInstruction.ModRMByte"/> class.
	/// </summary>
	[TestFixture]
	public class EncodedInstruction_ModRMByteTests
	{
		/// <summary>
		/// Tests the <see cref="EncodedInstruction.ModRMByte.ToBytes"/> method.
		/// </summary>
		[Test]
		public void ToBytesTest()
		{
			byte[] bytes;
			EncodedInstruction.ModRMByte instance;

			// The default Mod R/M byte is zero.
			instance = new EncodedInstruction.ModRMByte();
			Assert.AreEqual(0x0, instance.RM);
			Assert.AreEqual(0x0, instance.Reg);
			Assert.AreEqual(0x0, instance.Mod);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0x00 }, bytes);

			// The RM part. Note that bit 4 is put in the REX byte.
			instance = new EncodedInstruction.ModRMByte(0xF, 0x0, 0x0);
			Assert.AreEqual(0xF, instance.RM);
			Assert.AreEqual(0x0, instance.Reg);
			Assert.AreEqual(0x0, instance.Mod);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0x07 }, bytes);

			// The REG part. Note that bit 4 is put in the REX byte.
			instance = new EncodedInstruction.ModRMByte(0x0, 0xF, 0x0);
			Assert.AreEqual(0x0, instance.RM);
			Assert.AreEqual(0xF, instance.Reg);
			Assert.AreEqual(0x0, instance.Mod);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0x38 }, bytes);

			// The MOD part.
			instance = new EncodedInstruction.ModRMByte(0x0, 0x0, 0x3);
			Assert.AreEqual(0x0, instance.RM);
			Assert.AreEqual(0x0, instance.Reg);
			Assert.AreEqual(0x3, instance.Mod);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0xC0 }, bytes);

			// All parts together.
			instance = new EncodedInstruction.ModRMByte(0x5, 0x5, 0x2);
			Assert.AreEqual(0x5, instance.RM);
			Assert.AreEqual(0x5, instance.Reg);
			Assert.AreEqual(0x2, instance.Mod);
			bytes = instance.ToBytes();
			Assert.AreEqual(new byte[] { 0xAD }, bytes);
		}
	}
}
