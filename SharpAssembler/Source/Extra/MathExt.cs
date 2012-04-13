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
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// Extra math functions.
	/// </summary>
	public static class MathExt
	{
		/// <summary>
		/// Tests that the specified value is zero or a positive power of two.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns><see langword="true"/> when the value is positive and a power of two, or zero.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsPowerOfTwo(ulong value)
		{
			return (value & (value - 1)) == 0;
		}

		#region IsPowerOfTwo()
		/// <summary>
		/// Tests that the specified value is zero or a positive power of two.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns><see langword="true"/> when the value is positive and a power of two, or zero.</returns>
		[Pure]
		public static bool IsPowerOfTwo(long value)
		{
			return IsPowerOfTwo((ulong)value);
		}

		/// <summary>
		/// Tests that the specified value is zero or a positive power of two.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns><see langword="true"/> when the value is positive and a power of two, or zero.</returns>
		[Pure]
		public static bool IsPowerOfTwo(int value)
		{
			return IsPowerOfTwo((long)value);
		}

		/// <summary>
		/// Tests that the specified value is zero or a positive power of two.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns><see langword="true"/> when the value is positive and a power of two, or zero.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsPowerOfTwo(uint value)
		{
			return IsPowerOfTwo((ulong)value);
		}
		#endregion




		/// <summary>
		/// Calculates the padding from the specified value to the next boundary.
		/// </summary>
		/// <param name="value">The value from which to calculate the padding.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The padding from the value to the next boundary.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static ulong CalculatePadding(ulong value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<ulong>() >= 0);
			#endregion
			return Align(value, boundary) - value;
		}

		/// <summary>
		/// Calculates the padding from the specified value to the next boundary.
		/// </summary>
		/// <param name="value">The value from which to calculate the padding.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The padding from the value to the next boundary.</returns>
		[Pure]
		public static long CalculatePadding(long value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<long>() >= 0);
			#endregion
			return Align(value, boundary) - value;
		}

		/// <summary>
		/// Calculates the padding from the specified value to the next boundary.
		/// </summary>
		/// <param name="value">The value from which to calculate the padding.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The padding from the value to the next boundary.</returns>
		[Pure]
		public static Int128 CalculatePadding(Int128 value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<Int128>() >= 0);
			#endregion
			return Align(value, boundary) - value;
		}

		/// <summary>
		/// Calculates the padding from the specified value to the next boundary.
		/// </summary>
		/// <param name="value">The value from which to calculate the padding.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The padding from the value to the next boundary.</returns>
		[Pure]
		public static UInt128 CalculatePadding(UInt128 value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<UInt128>() >= 0);
			#endregion
			return Align(value, boundary) - value;
		}

		#region CalculatePadding()
		/// <summary>
		/// Calculates the padding from the specified value to the next boundary.
		/// </summary>
		/// <param name="value">The value from which to calculate the padding.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The padding from the value to the next boundary.</returns>
		[Pure]
		public static int CalculatePadding(int value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion
			return (int)CalculatePadding((long)value, boundary);
		}

		/// <summary>
		/// Calculates the padding from the specified value to the next boundary.
		/// </summary>
		/// <param name="value">The value from which to calculate the padding.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The padding from the value to the next boundary.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static uint CalculatePadding(uint value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<uint>() >= 0);
			#endregion
			return (uint)CalculatePadding((ulong)value, boundary);
		}
		#endregion

		/// <summary>
		/// Aligns the value to the next specified boundary.
		/// </summary>
		/// <param name="value">The value to align.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The aligned value.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static ulong Align(ulong value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<ulong>() >= value);
			#endregion
			return ((ulong)boundary + ((value - 1) & ~((ulong)boundary - 1)));
		}

		/// <summary>
		/// Aligns the value to the next specified boundary.
		/// </summary>
		/// <param name="value">The value to align.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The aligned value.</returns>
		[Pure]
		public static long Align(long value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<long>() >= value);
			#endregion
			return (boundary + ((value - 1) & ~(boundary - 1)));
		}

		/// <summary>
		/// Aligns the value to the next specified boundary.
		/// </summary>
		/// <param name="value">The value to align.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The aligned value.</returns>
		[Pure]
		public static Int128 Align(Int128 value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<Int128>() >= value);
			#endregion
			return (boundary + ((value - 1) & ~(boundary - 1)));
		}

		/// <summary>
		/// Aligns the value to the next specified boundary.
		/// </summary>
		/// <param name="value">The value to align.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The aligned value.</returns>
		[Pure]
		public static UInt128 Align(UInt128 value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<UInt128>() >= value);
			#endregion
			return (boundary + ((value - 1) & ~(boundary - 1)));
		}

		#region Align()
		/// <summary>
		/// Aligns the value to the next specified boundary.
		/// </summary>
		/// <param name="value">The value to align.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The aligned value.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static uint Align(uint value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<uint>() >= value);
			#endregion
			return (uint)Align((ulong)value, boundary);
		}

		/// <summary>
		/// Aligns the value to the next specified boundary.
		/// </summary>
		/// <param name="value">The value to align.</param>
		/// <param name="boundary">The boundary, which is a power of two.</param>
		/// <returns>The aligned value.</returns>
		[Pure]
		public static int Align(int value, int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<int>() >= value);
			#endregion
			return (int)Align((long)value, boundary);
		}
		#endregion

		/// <summary>
		/// Determines the minimum width that can fit the specified (signed or unsigned) value.
		/// </summary>
		/// <param name="value">The value to fit.</param>
		/// <param name="signed">Whether to fit the value signed or unsigned.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int128 value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			
			if (signed && value < 0)
				value = -value;

			if (value.High != 0) return DataSize.Bit128;
			if ((value.Low & 0xFFFFFFFF00000000) != 0) return DataSize.Bit64;
			if ((value.Low & 0x00000000FFFF0000) != 0) return DataSize.Bit32;
			if ((value.Low & 0x000000000000FF00) != 0) return DataSize.Bit16;
			return DataSize.Bit8;
		}

		#region GetSizeOfValue()
		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Byte value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, false);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Byte value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(SByte value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, true);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(SByte value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int16 value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, true);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int16 value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(UInt16 value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, false);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(UInt16 value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int32 value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, true);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int32 value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(UInt32 value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, false);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(UInt32 value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int64 value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, true);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int64 value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(UInt64 value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, false);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="signed">Whether to do a signed check.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static DataSize GetSizeOfValue(UInt64 value, bool signed)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue((Int128)value, signed);
		}

		/// <summary>
		/// Returns the minimum size required to fit the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A member of the <see cref="DataSize"/> enumeration.</returns>
		[Pure]
		public static DataSize GetSizeOfValue(Int128 value)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetSizeOfValue(value, true);
		}
		#endregion
	}
}
