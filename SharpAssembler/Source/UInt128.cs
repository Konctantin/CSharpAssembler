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
using System.Text;
using System.Globalization;

namespace SharpAssembler
{
	/// <summary>
	/// A 128-bit unsigned integer.
	/// </summary>
	/// <remarks>
	/// This implementation is based on the code described in
	/// <see href="http://www.informit.com/guides/content.aspx?g=dotnet&amp;seqNum=636"/>.
	/// </remarks>
	public struct UInt128 : IFormattable, IConvertible,
		IComparable, IComparable<UInt128>, IEquatable<UInt128>
	{
		#region Constants
		/// <summary>
		/// The maximum value a <see cref="UInt128"/> can represent.
		/// </summary>
		public static readonly UInt128 MaxValue = new UInt128(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);
		/// <summary>
		/// The minimum value a <see cref="UInt128"/> can represent.
		/// </summary>
		public static readonly UInt128 MinValue = new UInt128(0, 0);

		/// <summary>
		/// A <see cref="UInt128"/> value of 0.
		/// </summary>
		public static readonly UInt128 Zero = new UInt128(0, 0);
		/// <summary>
		/// A <see cref="UInt128"/> value of 1.
		/// </summary>
		public static readonly UInt128 One = new UInt128(1, 0);
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="UInt128"/> struct.
		/// </summary>
		/// <param name="low">The least significant 64-bits of the integer.</param>
		/// <param name="high">The most siginificant 64-bits of the integer.</param>
		[CLSCompliant(false)]
		public UInt128(ulong low, ulong high)
		{
			this.low = low;
			this.high = high;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets whether the value of this <see cref="UInt128"/> is an even number.
		/// </summary>
		/// <value><see langword="true"/> when this value is an even number;
		/// otherwise, <see langword="false"/>.</value>
		/// <remarks>
		/// Zero is also an even number.
		/// </remarks>
		public bool IsEven
		{
			get
			{
				return (low & 1) == 0;
			}
		}

		/// <summary>
		/// Gets whether the value of this <see cref="UInt128"/> is zero.
		/// </summary>
		/// <value><see langword="true"/> when this value is zero;
		/// otherwise, <see langword="false"/>.</value>
		public bool IsZero
		{
			get { return high == 0 && low == 0; }
		}

		/// <summary>
		/// Gets whether the value of this <see cref="UInt128"/> is one.
		/// </summary>
		/// <value><see langword="true"/> when this value is one;
		/// otherwise, <see langword="false"/>.</value>
		public bool IsOne
		{
			get { return high == 0 && low == 1; }
		}

		/// <summary>
		/// Gets whether the value of this <see cref="UInt128"/> is a power of two.
		/// </summary>
		/// <value><see langword="true"/> when this value is a power of two;
		/// otherwise, <see langword="false"/>.</value>
		public bool IsPowerOfTwo
		{
			get { return !IsZero && (this & (this - 1)) == 0; }
		}

		/// <summary>
		/// Gets the 64 least significant bits of the value.
		/// </summary>
		/// <value>The 64 least significant bits.</value>
		[CLSCompliant(false)]
		public ulong Low
		{
			get { return this.low; }
		}

		/// <summary>
		/// Gets the 64 most significant bits of the value.
		/// </summary>
		/// <value>The 64 most significant bits.</value>
		[CLSCompliant(false)]
		public ulong High
		{
			get { return this.high; }
		}
		#endregion

		#region Equality
		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns><see langword="true"/> if <paramref name="obj"/> and this instance are the same type and represent
		/// the same value; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			if (Object.ReferenceEquals(obj, null) ||
				!(obj is UInt128))
				return false;
			return Equals((UInt128)obj);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><see langword="true"/> if the current object is equal to the other parameter;
		/// otherwise, <see langword="false"/>.</returns>
		public bool Equals(UInt128 other)
		{
			return this.high == other.high && this.low == other.low;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + low.GetHashCode();
				hash = hash * 23 + high.GetHashCode();
				return hash;
			}
		}

		/// <summary>
		/// Returns a value that indicates whether two <see cref="UInt128"/> objects have the same value.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the <paramref name="left"/> and <paramref name="right"/> parameters have
		/// the same value; otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(UInt128 left, UInt128 right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Returns a value that indicates whether two <see cref="UInt128"/> objects have different values.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the <paramref name="left"/> and <paramref name="right"/> parameters have
		/// different values; otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(UInt128 left, UInt128 right)
		{
			return !left.Equals(right);
		}
		#endregion

		#region Comparisons
		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>A value that indicates the relative order of the objects being compared.</returns>
		public int CompareTo(object obj)
		{
			if (obj is UInt128)
				return CompareTo((UInt128)obj);
			else
				throw new ArgumentException("The specified object is not the same type as this instance.", "obj");
		}

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>A value that indicates the relative order of the objects being compared.</returns>
		public int CompareTo(UInt128 other)
		{
			int result = this.high.CompareTo(other.high);
			if (result == 0)
				result = this.low.CompareTo(other.low);
			return result;
		}

		/// <summary>
		/// Returns a value that indicates whether a <see cref="UInt128"/> value is greater than another
		/// <see cref="UInt128"/> value.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is greater than the value of
		/// <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator >(UInt128 left, UInt128 right)
		{
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Returns a value that indicates whether a <see cref="UInt128"/> value is less than another
		/// <see cref="UInt128"/> value.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is less than the value of
		/// <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator <(UInt128 left, UInt128 right)
		{
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Returns a value that indicates whether a <see cref="UInt128"/> value is greater than or equal to another
		/// <see cref="UInt128"/> value.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to the
		/// value of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator >=(UInt128 left, UInt128 right)
		{
			return left.CompareTo(right) >= 0;
		}

		/// <summary>
		/// Returns a value that indicates whether a <see cref="UInt128"/> value is less than or equal to another
		/// <see cref="UInt128"/> value.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value
		/// of <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator <=(UInt128 left, UInt128 right)
		{
			return left.CompareTo(right) <= 0;
		}
		#endregion

		#region Conversions
		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		[CLSCompliant(false)]
		public static explicit operator UInt128(Int128 value)
		{
			return new UInt128(value.Low, unchecked((ulong)value.High));
		}

		/// <summary>
		/// Converts the specfied unsigned 64-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		[CLSCompliant(false)]
		public static implicit operator UInt128(ulong value)
		{
			return new UInt128(value, 0);
		}

		/// <summary>
		/// Converts the specfied signed 64-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		public static implicit operator UInt128(long value)
		{
			return (UInt128)(Int128)value;
		}

		/// <summary>
		/// Converts the specfied unsigned 32-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		[CLSCompliant(false)]
		public static implicit operator UInt128(uint value)
		{
			return (UInt128)(ulong)value;
		}

		/// <summary>
		/// Converts the specfied signed 32-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		public static implicit operator UInt128(int value)
		{
			return (UInt128)(long)value;
		}

		/// <summary>
		/// Converts the specfied unsigned 16-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		[CLSCompliant(false)]
		public static implicit operator UInt128(ushort value)
		{
			return (UInt128)(ulong)value;
		}

		/// <summary>
		/// Converts the specfied signed 16-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		public static implicit operator UInt128(short value)
		{
			return (UInt128)(long)value;
		}

		/// <summary>
		/// Converts the specfied unsigned 8-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		public static implicit operator UInt128(byte value)
		{
			return (UInt128)(ulong)value;
		}

		/// <summary>
		/// Converts the specfied signed 8-bit value to an unsigned 128-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 128-bit value.</returns>
		[CLSCompliant(false)]
		public static implicit operator UInt128(sbyte value)
		{
			return (UInt128)(long)value;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 64-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 64-bit value.</returns>
		[CLSCompliant(false)]
		public static explicit operator ulong(UInt128 value)
		{
			return (ulong)value.low;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 64-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 64-bit value.</returns>
		public static explicit operator long(UInt128 value)
		{
			return (long)value.low;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 32-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 32-bit value.</returns>
		[CLSCompliant(false)]
		public static explicit operator uint(UInt128 value)
		{
			return (uint)value.low;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 32-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 32-bit value.</returns>
		public static explicit operator int(UInt128 value)
		{
			return (int)value.low;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 16-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 16-bit value.</returns>
		[CLSCompliant(false)]
		public static explicit operator ushort(UInt128 value)
		{
			return (ushort)value.low;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 16-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 16-bit value.</returns>
		public static explicit operator short(UInt128 value)
		{
			return (short)value.low;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 8-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 8-bit value.</returns>
		public static explicit operator byte(UInt128 value)
		{
			return (byte)value.low;
		}

		/// <summary>
		/// Converts the specfied signed 128-bit value to an unsigned 8-bit value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting 8-bit value.</returns>
		[CLSCompliant(false)]
		public static explicit operator sbyte(UInt128 value)
		{
			return (sbyte)value.low;
		}

		/// <summary>
		/// Returns the <see cref="TypeCode"/> for this instance.
		/// </summary>
		/// <returns>The enumerated constant that is the <see cref="TypeCode"/> of the class or value type that
		/// implements this interface.</returns>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		/// <summary>
		/// Converts the value of this instance to an <see cref="Object"/> of the specified <see cref="Type"/> that has
		/// an equivalent value, using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="conversionType">The <see cref="Type"/> to which the value of this instance is
		/// converted.</param>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>An <see cref="Object"/> instance of type <paramref name="conversionType"/> whose value is
		/// equivalent to the value of this instance.</returns>
		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			IConvertible conv = (IConvertible)this;
			
			if (conversionType == typeof(bool))
				return conv.ToBoolean(provider);
			if (conversionType == typeof(byte))
				return conv.ToByte(provider);
			if (conversionType == typeof(char))
				return conv.ToChar(provider);
			if (conversionType == typeof(DateTime))
				return conv.ToDateTime(provider);
			if (conversionType == typeof(Decimal))
				return conv.ToDecimal(provider);
			if (conversionType == typeof(Double))
				return conv.ToDouble(provider);
			if (conversionType == typeof(Int16))
				return conv.ToInt16(provider);
			if (conversionType == typeof(Int32))
				return conv.ToInt32(provider);
			if (conversionType == typeof(Int64))
				return conv.ToInt64(provider);
			if (conversionType == typeof(SByte))
				return conv.ToSByte(provider);
			if (conversionType == typeof(Single))
				return conv.ToSingle(provider);
			if (conversionType == typeof(String))
				return conv.ToString(provider);
			if (conversionType == typeof(UInt16))
				return conv.ToUInt16(provider);
			if (conversionType == typeof(UInt32))
				return conv.ToUInt32(provider);
			if (conversionType == typeof(UInt64))
				return conv.ToUInt64(provider);
			if (conversionType == typeof(UInt128))
				return this;

			throw new InvalidCastException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent boolean value using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A boolean value equivalent to the value of this instance.</returns>
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return !(this.low == 0 && this.high == 0);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent Unicode character using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A Unicode character equivalent to the value of this instance.</returns>
		Char IConvertible.ToChar(IFormatProvider provider)
		{
			if (this < Char.MinValue || this > Char.MaxValue)
				throw new OverflowException();
			return (Char)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 8-bit unsigned integer equivalent to the value of this instance.</returns>
		Byte IConvertible.ToByte(IFormatProvider provider)
		{
			if (this < Byte.MinValue || this > Byte.MaxValue)
				throw new OverflowException();
			return (Byte)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 8-bit signed integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 8-bit signed integer equivalent to the value of this instance.</returns>
		SByte IConvertible.ToSByte(IFormatProvider provider)
		{
			if (this < SByte.MinValue || this > SByte.MaxValue)
				throw new OverflowException();
			return (SByte)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 16-bit signed integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 16-bit signed integer equivalent to the value of this instance.</returns>
		Int16 IConvertible.ToInt16(IFormatProvider provider)
		{
			if (this < Int16.MinValue || this > Int16.MaxValue)
				throw new OverflowException();
			return (Int16)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 32-bit signed integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 32-bit signed integer equivalent to the value of this instance.</returns>
		Int32 IConvertible.ToInt32(IFormatProvider provider)
		{
			if (this < Int32.MinValue || this > Int32.MaxValue)
				throw new OverflowException();
			return (Int32)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 64-bit signed integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 64-bit signed integer equivalent to the value of this instance.</returns>
		Int64 IConvertible.ToInt64(IFormatProvider provider)
		{
			if (this < Int64.MinValue || this > Int64.MaxValue)
				throw new OverflowException();
			return (Int64)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 16-bit unsigned integer equivalent to the value of this instance.</returns>
		UInt16 IConvertible.ToUInt16(IFormatProvider provider)
		{
			if (this < UInt16.MinValue || this > UInt16.MaxValue)
				throw new OverflowException();
			return (UInt16)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 32-bit unsigned integer equivalent to the value of this instance.</returns>
		UInt32 IConvertible.ToUInt32(IFormatProvider provider)
		{
			if (this < UInt32.MinValue || this > UInt32.MaxValue)
				throw new OverflowException();
			return (UInt32)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A 64-bit unsigned integer equivalent to the value of this instance.</returns>
		UInt64 IConvertible.ToUInt64(IFormatProvider provider)
		{
			if (this < UInt64.MinValue || this > UInt64.MaxValue)
				throw new OverflowException();
			return (UInt64)this;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent <see cref="DateTime"/> value using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A <see cref="DateTime"/> value equivalent to the value of this instance.</returns>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent decimal value using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A decimal value equivalent to the value of this instance.</returns>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent floating-point value using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A floating-point value equivalent to the value of this instance.</returns>
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent floating-point value using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A floating-point value equivalent to the value of this instance.</returns>
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent string value using the specified
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies
		/// culture-specific formatting information.</param>
		/// <returns>A string value equivalent to the value of this instance.</returns>
		string IConvertible.ToString(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}
		#endregion

		#region Arithmetic
		#region Unary
		/// <summary>
		/// Returns the value of the <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">An integer value.</param>
		/// <returns>The value of the <paramref name="value"/> parameter.</returns>
		public static UInt128 operator +(UInt128 value)
		{
			return value;
		}

		/// <summary>
		/// Returns the bitwise one's complement of an <see cref="UInt128"/> value.
		/// </summary>
		/// <param name="value">An integer value.</param>
		/// <returns>The bitwise one's complement of <paramref name="value"/>.</returns>
		public static UInt128 operator ~(UInt128 value)
		{
			return new UInt128(~value.low, ~value.high);
		}

		/// <summary>
		/// Increments a <see cref="UInt128"/> value by 1.
		/// </summary>
		/// <param name="value">The value to increment.</param>
		/// <returns>The value of the <paramref name="value"/> parameter incremented by 1.</returns>
		public static UInt128 operator ++(UInt128 value)
		{
			value.low++;
			if (value.low == 0)
				value.high++;
			return value;
		}

		/// <summary>
		/// Decrements a <see cref="UInt128"/> value by 1.
		/// </summary>
		/// <param name="value">The value to decrement.</param>
		/// <returns>The value of the <paramref name="value"/> parameter decremented by 1.</returns>
		public static UInt128 operator --(UInt128 value)
		{
			if (value.low == 0)
				value.high--;
			value.low--;
			return value;
		}
		#endregion

		#region Binary
		/// <summary>
		/// Adds the values of two specified <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
		/// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
		public static UInt128 operator +(UInt128 left, UInt128 right)
		{
			return Add(left, right);
		}

		/// <summary>
		/// Adds two <see cref="UInt128"/> values and returns the result.
		/// </summary>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
		/// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
		public static UInt128 Add(UInt128 left, UInt128 right)
		{
			var oldLow = left.low;

			left.low += right.low;
			left.high += right.high;
			if (left.low < oldLow)
				left.high++;

			return left;
		}

		/// <summary>
		/// Subtracts an <see cref="UInt128"/> from another <see cref="UInt128"/> value.
		/// </summary>
		/// <param name="left">The value to subtract from.</param>
		/// <param name="right">The value to subtract.</param>
		/// <returns>The result of subtracting <paramref name="right"/> from <paramref name="left"/>.</returns>
		public static UInt128 operator -(UInt128 left, UInt128 right)
		{
			return Subtract(left, right);
		}

		/// <summary>
		/// Subtracts one <see cref="UInt128"/> from another and returns the result.
		/// </summary>
		/// <param name="left">The value to subtract from.</param>
		/// <param name="right">The value to subtract.</param>
		/// <returns>The result of subtracting <paramref name="right"/> from <paramref name="left"/>.</returns>
		public static UInt128 Subtract(UInt128 left, UInt128 right)
		{
			return (UInt128)((Int128)left - (Int128)right);
		}

		/// <summary>
		/// Performs a bitwise AND operation on two <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first value.</param>
		/// <param name="right">The second value.</param>
		/// <returns>The result of the bitwise AND operation.</returns>
		public static UInt128 operator &(UInt128 left, UInt128 right)
		{
			return new UInt128(left.low & right.low, left.high & right.high);
		}

		/// <summary>
		/// Performs a bitwise OR operation on two <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first value.</param>
		/// <param name="right">The second value.</param>
		/// <returns>The result of the bitwise OR operation.</returns>
		public static UInt128 operator |(UInt128 left, UInt128 right)
		{
			return new UInt128(left.low | right.low, left.high | right.high);
		}

		/// <summary>
		/// Performs a bitwise exclusive OR (XOR) operation on two <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first value.</param>
		/// <param name="right">The second value.</param>
		/// <returns>The result of the bitwise XOR operation.</returns>
		public static UInt128 operator ^(UInt128 left, UInt128 right)
		{
			return new UInt128(left.low ^ right.low, left.high ^ right.high);
		}

		/// <summary>
		/// Shifts an <see cref="UInt128"/> value a specified number of bits to the left.
		/// </summary>
		/// <param name="value">The value whose bits are to be shifted.</param>
		/// <param name="shift">The number of bits to shift <paramref name="value"/> to the left.</param>
		/// <returns>A value that has been shifted to the left by the specified number of bits.</returns>
		public static UInt128 operator <<(UInt128 value, int shift)
		{
			if (shift == 0)
				return value;
			if (shift < 0)
				return value >> -shift;

			// Shifting more than 127 bits would shift out any bits.
			if (shift > 127)
				return 0;

			// Shifting more than 64 bits would shift all the low bits at least
			// to the high bits.
			if (shift > 63)
			{
				shift -= 64;
				value.high = value.low;
				value.low = 0;
			}

			if (shift > 0)
			{
				ulong highbits = value.low >> (64 - shift);
				value.low <<= shift;
				value.high <<= shift;
				value.high |= highbits;
			}

			return value;
		}

		/// <summary>
		/// Shifts an <see cref="UInt128"/> value a specified number of bits to the right.
		/// </summary>
		/// <param name="value">The value whose bits are to be shifted.</param>
		/// <param name="shift">The number of bits to shift <paramref name="value"/> to the right.</param>
		/// <returns>A value that has been shifted to the right by the specified number of bits.</returns>
		public static UInt128 operator >>(UInt128 value, int shift)
		{
			if (shift == 0)
				return value;
			if (shift < 0)
				return value << -shift;

			// Shifting more than 127 bits would shift out any bits.
			if (shift > 127)
				return 0;

			// Shifting more than 64 bits would shift all the high bits at least
			// to the low bits.
			if (shift > 63)
			{
				shift -= 64;
				value.low = value.high;
				value.high = 0;
			}

			if (shift > 0)
			{
				ulong lowbits = value.high << (64 - shift);
				value.low >>= shift;
				value.high >>= shift;
				value.low |= lowbits;
			}

			return value;
		}

		/// <summary>
		/// Multiplies two specified <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first value to multiply.</param>
		/// <param name="right">The second value to multiply.</param>
		/// <returns>The product of <paramref name="left"/> and <paramref name="right"/>.</returns>
		public static UInt128 operator *(UInt128 left, UInt128 right)
		{
			return Multiply(left, right);
		}

		/// <summary>
		/// Returns the product of two <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first number to multiply.</param>
		/// <param name="right">The second number to multiply.</param>
		/// <returns>The product of <paramref name="left"/> and <paramref name="right"/>.</returns>
		public static UInt128 Multiply(UInt128 left, UInt128 right)
		{
			uint left3 = (uint)(left.high >> 32);
			uint left2 = (uint)left.high;
			uint left1 = (uint)(left.low >> 32);
			uint left0 = (uint)left.low;

			ulong right3 = (uint)(right.high >> 32);
			ulong right2 = (uint)right.high;
			ulong right1 = (uint)(right.low >> 32);
			ulong right0 = (uint)right.low;

			UInt128 value00 = (UInt128)(left0 * right0);
			UInt128 value10 = (UInt128)(left1 * right0) << 32;
			UInt128 value20 = new UInt128(0, left2 * right0);
			UInt128 value30 = new UInt128(0, (left3 * right0) << 32);

			UInt128 value01 = (UInt128)(left0 * right1) << 32;
			UInt128 value11 = new UInt128(0, left1 * right1);
			UInt128 value21 = new UInt128(0, (left2 * right1) << 32);

			UInt128 value02 = new UInt128(0, left0 * right2);
			UInt128 value12 = new UInt128(0, (left1 * right2) << 32);

			UInt128 value03 = new UInt128(0, (left0 * right3) << 32);

			return value00 + value10 + value20 + value30
				+ value01 + value11 + value21
				+ value02 + value21
				+ value03;
		}

		/// <summary>
		/// Divides a specified <see cref="UInt128"/> value by another specified <see cref="UInt128"/> value by using integer division.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <returns>The integral result of the division.</returns>
		public static UInt128 operator /(UInt128 dividend, UInt128 divisor)
		{
			#region Contract
			Contract.Requires<DivideByZeroException>(divisor != 0);
			#endregion

			return Divide(dividend, divisor);
		}

		/// <summary>
		/// Divides one <see cref="UInt128"/> by another and returns the result.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <returns>The quotient of the division.</returns>
		public static UInt128 Divide(UInt128 dividend, UInt128 divisor)
		{
			#region Contract
			Contract.Requires<DivideByZeroException>(divisor != 0);
			#endregion

			UInt128 remainder;
			return DivRem(dividend, divisor, out remainder);
		}

		/// <summary>
		/// Returns the remainder that results from division with two specified <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <returns>The remainder that results from the division.</returns>
		public static UInt128 operator %(UInt128 dividend, UInt128 divisor)
		{
			#region Contract
			Contract.Requires<DivideByZeroException>(divisor != 0);
			#endregion

			return Remainder(dividend, divisor);
		}

		/// <summary>
		/// Performs integer division on two <see cref="UInt128"/> values and returns the remainder.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <returns>The remainder after dividing <paramref name="dividend"/> by <paramref name="divisor"/>.</returns>
		public static UInt128 Remainder(UInt128 dividend, UInt128 divisor)
		{
			#region Contract
			Contract.Requires<DivideByZeroException>(divisor != 0);
			#endregion

			UInt128 remainder;
			DivRem(dividend, divisor, out remainder);
			return remainder;
		}
		#endregion

		/// <summary>
		/// Divides one <see cref="UInt128"/> value by another, using signed integer division, and returns the result
		/// and the remainder.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <param name="remainder">The remainder from the division.</param>
		/// <returns>The quotient of the division.</returns>
		public static UInt128 DivRem(UInt128 dividend, UInt128 divisor, out UInt128 remainder)
		{
			#region Contract
			Contract.Requires<DivideByZeroException>(divisor != 0);
			#endregion

			UInt128 quotient = UnsignedDivRem(dividend, divisor, out remainder);

			return quotient;
		}

		/// <summary>
		/// Divides one <see cref="UInt128"/> value by another, using unsigned integer division, and returns the result
		/// and the remainder.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <param name="remainder">The remainder from the division.</param>
		/// <returns>The quotient of the division.</returns>
		public static UInt128 UnsignedDivRem(UInt128 dividend, UInt128 divisor, out UInt128 remainder)
		{
			UInt128 quotient = dividend;
			remainder = 0;
			for (int i = 0; i < 128; i++)
			{
				remainder <<= 1;
				if (quotient < 0)
					remainder.low |= 1;
				quotient <<= 1;

				if (remainder >= divisor)
				{
					remainder -= divisor;
					quotient++;
				}
			}

			return quotient;
		}

		/// <summary>
		/// Returns the larger of two <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>The <paramref name="left"/> or <paramref name="right"/> parameter, whichever is larger.</returns>
		public static UInt128 Max(UInt128 left, UInt128 right)
		{
			#region Contract
			Contract.Ensures(Contract.Result<UInt128>() >= left);
			Contract.Ensures(Contract.Result<UInt128>() >= right);
			#endregion

			UInt128 result = left;
			if (right > left)
				result = right;
			Contract.Assert(result >= left);
			Contract.Assert(result >= right);
			return result;
		}

		/// <summary>
		/// Returns the smaller of two <see cref="UInt128"/> values.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>The <paramref name="left"/> or <paramref name="right"/> parameter, whichever is smaller.</returns>
		public static UInt128 Min(UInt128 left, UInt128 right)
		{
			#region Contract
			Contract.Ensures(Contract.Result<UInt128>() <= left);
			Contract.Ensures(Contract.Result<UInt128>() <= right);
			#endregion

			UInt128 result = left;
			if (right < left)
				result = right;
			Contract.Assume(result <= left);
			Contract.Assume(result <= right);
			return result;
		}

		/// <summary>
		/// Calculates the padding required to align the value to the next specified boundary.
		/// </summary>
		/// <param name="boundary">The boundary to align to, which must be a power of two.</param>
		/// <returns>The number of padding bytes required to align the address to the specified boundary.</returns>
		public UInt128 GetPadding(int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(MathExt.IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<UInt128>() >= 0);
			#endregion

			return MathExt.CalculatePadding(this, boundary);
		}

		/// <summary>
		/// Aligns the value to the next specified boundary.
		/// </summary>
		/// <param name="boundary">The boundary to align to, which must be a power of two.</param>
		/// <returns>The address aligned to the specified boundary.</returns>
		public UInt128 Align(int boundary)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(MathExt.IsPowerOfTwo(boundary));
			Contract.Ensures(Contract.Result<UInt128>() >= this);
			#endregion

			return MathExt.Align(this, boundary);
		}
		#endregion

		#region Strings
		/// <inheritdoc />
		public override string ToString()
		{
			return ToString(null, null);
		}

		/// <inheritdoc />
		public string ToString(string format)
		{
			return ToString(format, null);
		}

		/// <inheritdoc />
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (formatProvider == null)
				formatProvider = CultureInfo.CurrentCulture;

			char formatID = 'd';
			if (!String.IsNullOrEmpty(format))
				formatID = format[0];

			// We only support hexadecimal and decimal.
			if (formatID == 'x' || formatID == 'X')
			{
				int minimumDigitCount;
				bool uppercase = (formatID == 'X');
				Int32.TryParse(format.Substring(1).Trim(), out minimumDigitCount);

				return ToHexadecimalString(minimumDigitCount, uppercase);
			}
			else if (formatID == 'd' || formatID == 'D'
				|| formatID == 'g' || formatID == 'G')
			{
				return ToDecimalString((NumberFormatInfo)formatProvider.GetFormat(typeof(NumberFormatInfo)));
			}
			else
				throw new NotSupportedException(String.Format("Format specifier '{0}' is not supported.", format));
		}

		/// <summary>
		/// Returns the value of this <see cref="UInt128"/> as a string with a hexadecimal number.
		/// </summary>
		/// <param name="minimumDigitCount">The minimum number of digits to write.</param>
		/// <param name="uppercase">Whether to use uppercase hexadecimal characters.</param>
		/// <returns>The hexadecimal string.</returns>
		private string ToHexadecimalString(int minimumDigitCount, bool uppercase)
		{
			StringBuilder sb = new StringBuilder();
			
			string formatID = uppercase ? "X" : "x";

			int highDigitCount = Math.Max(minimumDigitCount - 16, 0);
			int lowDigitCount = Math.Min(minimumDigitCount, 16);

			if (this.high != 0 || highDigitCount > 0)
			{
				sb.Append(this.high.ToString(formatID + highDigitCount.ToString()));
			}

			sb.Append(this.low.ToString(formatID + lowDigitCount.ToString()));

			return sb.ToString();
		}

		/// <summary>
		/// Returns the value of this <see cref="UInt128"/> as a string with a decimal number.
		/// </summary>
		/// <param name="format">The number format info to use.</param>
		/// <returns>The decimal string.</returns>
		private string ToDecimalString(NumberFormatInfo format)
		{
			if (this.IsZero)
				return "0";

			// We don't do number format special features, such as digit grouping and zero placeholders.

			StringBuilder sb = new StringBuilder();

			UInt128 @base = 10;
			UInt128 current = this;
			UInt128 remainder;
			do
			{
				current = DivRem(current, @base, out remainder);
				if (!current.IsZero || remainder.low > 0)
					sb.Insert(0, (char)('0' + remainder.low));
			} while (!current.IsZero);

			return sb.ToString();
		}
		#endregion

		#region Fields
		/// <summary>
		/// The low part of the integer.
		/// </summary>
		private ulong low;
		/// <summary>
		/// The high part of the integer.
		/// </summary>
		private ulong high;
		#endregion
	}
}
