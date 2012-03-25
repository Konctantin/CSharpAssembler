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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace SharpAssembler
{
	/// <summary>
	/// Extension methods to the <see cref="BinaryWriter"/> class.
	/// </summary>
	public static class BinaryWriterExtensions
	{
		/// <summary>
		/// Writes a 128-bit value to the <see cref="BinaryWriter"/>.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		public static void Write(this BinaryWriter writer, Int128 value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			// We maintain the same byte ordering as the BinaryWriter.
			if (BitConverter.IsLittleEndian)
			{
				writer.Write((ulong)value.Low);
				writer.Write((ulong)value.High);
			}
			else
			{
				writer.Write((ulong)value.High);
				writer.Write((ulong)value.Low);
			}
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		public static int Write(this BinaryWriter writer, Int128 value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			switch (size)
			{
				case DataSize.Bit8:
				case DataSize.Bit16:
				case DataSize.Bit32:
				case DataSize.Bit64:
				case DataSize.Bit80:
					writer.Write((ulong)value, size);
					break;
				case DataSize.Bit128:
					writer.Write(value);
					break;
				case DataSize.Bit256:
					// We maintain the same byte ordering as the BinaryWriter.
					if (BitConverter.IsLittleEndian)
					{
						writer.Write(value);
						writer.Write((Int128)0);
					}
					else
					{
						writer.Write((Int128)0);
						writer.Write(value);
					}
					break;
				default:
					throw new NotSupportedException();
			}

			return (int)size;
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		[CLSCompliant(false)]
		public static int Write(this BinaryWriter writer, ulong value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			switch (size)
			{
				case DataSize.Bit8:
					writer.Write((byte)value);
					break;
				case DataSize.Bit16:
					writer.Write((ushort)value);
					break;
				case DataSize.Bit32:
					writer.Write((uint)value);
					break;
				case DataSize.Bit64:
					writer.Write((ulong)value);
					break;
				case DataSize.Bit80:
					// We maintain the same byte ordering as the BinaryWriter.
					if (BitConverter.IsLittleEndian)
					{
						writer.Write((ulong)value);
						writer.Write((ushort)0);
					}
					else
					{
						writer.Write((ushort)0);
						writer.Write((ulong)value);
					}
					break;
				case DataSize.Bit128:
					writer.Write((Int128)value);
					break;
				case DataSize.Bit256:
					writer.Write((Int128)value, DataSize.Bit256);
					break;
				default:
					throw new NotSupportedException();
			}

			return (int)size;
		}

		#region Write()
		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		public static int Write(this BinaryWriter writer, long value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			return Write(writer, (ulong)value, size);
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		[CLSCompliant(false)]
		public static int Write(this BinaryWriter writer, uint value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			return Write(writer, (ulong)value, size);
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		public static int Write(this BinaryWriter writer, int value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			return Write(writer, (ulong)value, size);
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		[CLSCompliant(false)]
		public static int Write(this BinaryWriter writer, ushort value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			return Write(writer, (ulong)value, size);
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		public static int Write(this BinaryWriter writer, short value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			return Write(writer, (ulong)value, size);
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		public static int Write(this BinaryWriter writer, byte value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			return Write(writer, (ulong)value, size);
		}

		/// <summary>
		/// Writes a value to the <see cref="BinaryWriter"/> as a value with the specified size.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="size">The size of the value to write.</param>
		/// <returns>The number of written bytes.</returns>
		[CLSCompliant(false)]
		public static int Write(this BinaryWriter writer, sbyte value, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			return Write(writer, (ulong)value, size);
		}
		#endregion


		/// <summary>
		/// Writes the specified string encoded as bytes with no terminating byte.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> being used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="encoding">The <see cref="Encoding"/> used to encode the string.</param>
		/// <returns>The number of bytes written.</returns>
		public static int WriteEncodedString(this BinaryWriter writer, string value, Encoding encoding)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			byte[] encodedString = encoding.GetBytes(value);
			writer.Write(encodedString);
			return encodedString.Length;
		}

		/// <summary>
		/// Writes a string and the specified terminating byte.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> being used.</param>
		/// <param name="value">The value to write.</param>
		/// <param name="encoding">The <see cref="Encoding"/> used to decode the string.</param>
		/// <param name="terminator">The terminating byte.</param>
		public static int WriteEncodedString(this BinaryWriter writer, string value, Encoding encoding, byte terminator)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			byte[] encodedString = encoding.GetBytes(value);
			writer.Write(encodedString);
			writer.Write(terminator);
			return encodedString.Length + 1;
		}

		/// <summary>
		/// Outputs as many bytes as necessary to align the output to the specified boundary.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> being used.</param>
		/// <param name="boundary">The boundary to align the output to. Must be a power of 2.</param>
		/// <returns>The number of padding bytes used.</returns>
		public static long Align(this BinaryWriter writer, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<ArgumentNullException>(MathExt.IsPowerOfTwo(boundary));
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Ensures(Contract.Result<long>() >= 0);
			#endregion

			long padding = MathExt.CalculatePadding(writer.BaseStream.Position, boundary);
			writer.Write(new byte[padding]);
			return padding;
		}
	}
}
