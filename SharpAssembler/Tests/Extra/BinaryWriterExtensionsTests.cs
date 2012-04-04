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
using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace SharpAssembler.Core.Tests.Extra
{
	/// <summary>
	/// Tests the <see cref="BinaryWriterExtensions"/> class.
	/// </summary>
	[TestFixture]
	public class BinaryWriterExtensionsTests
	{
		/// <summary>
		/// Tests the <see cref="BinaryWriterExtensions.Align"/> method.
		/// </summary>
		[Test]
		public void Align()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					writer.Write((byte)0xFF);
					writer.Write((byte)0xFF);
					writer.Write((byte)0xFF);

					var padding = BinaryWriterExtensions.Align(writer, 8);
					Assert.AreEqual(5, padding);

					writer.Write((byte)0xCC);
					writer.Write((byte)0xCC);
					writer.Write((byte)0xCC);
					writer.Write((byte)0xCC);

					padding = BinaryWriterExtensions.Align(writer, 4);
					Assert.AreEqual(0, padding);

					writer.Write((byte)0xAA);
					
					Assert.AreEqual(new byte[]{
						0xFF, 0xFF, 0xFF, 0x00,
						0x00, 0x00, 0x00, 0x00,
						0xCC, 0xCC, 0xCC, 0xCC,
						0xAA
					}, stream.ToArray());
				}
			}
		}

		/// <summary>
		/// Tests the <see cref="BinaryWriterExtensions.WriteEncodedString"/> method.
		/// </summary>
		[Test]
		public void WriteEncodedString()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					var value = "T\u00E9st string!";	//C3 A9

					var written = BinaryWriterExtensions.WriteEncodedString(writer, value, Encoding.UTF8);

					Assert.AreEqual(13, written);
					Assert.AreEqual(new byte[]{
						0x54, 0xC3, 0xA9, 0x73,
						0x74, 0x20, 0x73, 0x74,
						0x72, 0x69, 0x6E, 0x67,
						0x21
					}, stream.ToArray());
					stream.SetLength(0);

					written = BinaryWriterExtensions.WriteEncodedString(writer, value, Encoding.UTF8, 0);

					Assert.AreEqual(14, written);
					Assert.AreEqual(new byte[]{
						0x54, 0xC3, 0xA9, 0x73,
						0x74, 0x20, 0x73, 0x74,
						0x72, 0x69, 0x6E, 0x67,
						0x21, 0x00
					}, stream.ToArray());
				}
			}
		}

		/// <summary>
		/// Tests the <see cref="BinaryWriterExtensions.Write(BinaryWriter, Int128)"/> method.
		/// </summary>
		[Test]
		public void WriteInt128Test()
		{
			// These tests are for little endian.
			Assert.IsTrue(BitConverter.IsLittleEndian);

			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);

			var value = new Int128((ulong)0xCAFEBABEDECEA5ED, unchecked((long)0xF0CA112E10CA112E));

			BinaryWriterExtensions.Write(writer, value);

			Assert.AreEqual(new byte[]{
				0xED, 0xA5, 0xCE, 0xDE,
				0xBE, 0xBA, 0xFE, 0xCA,
				0x2E, 0x11, 0xCA, 0x10,
				0x2E, 0x11, 0xCA, 0xF0
			}, stream.ToArray());
		}

		/// <summary>
		/// Tests the <see cref="BinaryWriterExtensions.Write"/> method and overloads.
		/// </summary>
		[Test]
		public void WriteDataSizeTest()
		{
			// These tests are for little endian.
			Assert.IsTrue(BitConverter.IsLittleEndian);

			MemoryStream stream = new MemoryStream(32);
			BinaryWriter writer = new BinaryWriter(stream);
			int written;

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (byte)0xCA, DataSize.Bit256);
			Assert.AreEqual(32, written);
			Assert.AreEqual(new byte[]{
				0xCA, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (ushort)0xCAFE, DataSize.Bit128);
			Assert.AreEqual(16, written);
			Assert.AreEqual(new byte[]{
				0xFE, 0xCA, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (uint)0xCAFEBABE, DataSize.Bit64);
			Assert.AreEqual(8, written);
			Assert.AreEqual(new byte[]{
				0xBE, 0xBA, 0xFE, 0xCA,
				0x00, 0x00, 0x00, 0x00
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (ulong)0xCAFEBABEDECEA5ED, DataSize.Bit64);
			Assert.AreEqual(8, written);
			Assert.AreEqual(new byte[]{
				0xED, 0xA5, 0xCE, 0xDE,
				0xBE, 0xBA, 0xFE, 0xCA
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (ulong)0xCAFEBABEDECEA5ED, DataSize.Bit32);
			Assert.AreEqual(4, written);
			Assert.AreEqual(new byte[]{
				0xED, 0xA5, 0xCE, 0xDE
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (ulong)0xCAFEBABEDECEA5ED, DataSize.Bit16);
			Assert.AreEqual(2, written);
			Assert.AreEqual(new byte[]{
				0xED, 0xA5
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (ulong)0xCAFEBABEDECEA5ED, DataSize.Bit8);
			Assert.AreEqual(1, written);
			Assert.AreEqual(new byte[]{
				0xED
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer, (ulong)0xCAFEBABEDECEA5ED, DataSize.Bit8);
			Assert.AreEqual(1, written);
			Assert.AreEqual(new byte[]{
				0xED
			}, stream.ToArray());

			stream.SetLength(0);
			written = BinaryWriterExtensions.Write(writer,
				new Int128((ulong)0xCAFEBABEDECEA5ED, unchecked((long)0xF0CA112E10CA112E)), DataSize.Bit256);
			Assert.AreEqual(32, written);
			Assert.AreEqual(new byte[]{
				0xED, 0xA5, 0xCE, 0xDE,
				0xBE, 0xBA, 0xFE, 0xCA,
				0x2E, 0x11, 0xCA, 0x10,
				0x2E, 0x11, 0xCA, 0xF0,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00
			}, stream.ToArray());
		}
	}
}
