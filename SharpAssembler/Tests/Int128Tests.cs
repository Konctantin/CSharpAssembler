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
using NUnit.Framework;

namespace SharpAssembler.Core.Tests
{
	/// <summary>
	/// Tests the <see cref="Int128"/> structure.
	/// </summary>
	[TestFixture]
	public class Int128Tests
	{
		Int128 valuem5 = -5;
		Int128 valuem4 = -4;
		Int128 valuem3 = -3;
		Int128 valuem2 = -2;
		Int128 valuem1 = -1;
		Int128 value0 = 0;
		Int128 value1 = 1;
		Int128 value2 = 2;
		Int128 value3 = 3;
		Int128 value4 = 4;
		Int128 value5 = 5;

		Int128 valueBig = new Int128(0xDEADBEEFCAFEBABE, 0x6789ABCDEF012345);

		/// <summary>
		/// Tests the <see cref="Int128.IsEven"/> property.
		/// </summary>
		[Test]
		public void IsEvenTest()
		{
			Assert.IsFalse(valuem5.IsEven);
			Assert.IsTrue(valuem4.IsEven);
			Assert.IsFalse(valuem3.IsEven);
			Assert.IsTrue(valuem2.IsEven);
			Assert.IsFalse(valuem1.IsEven);
			Assert.IsTrue(value0.IsEven);
			Assert.IsFalse(value1.IsEven);
			Assert.IsTrue(value2.IsEven);
			Assert.IsFalse(value3.IsEven);
			Assert.IsTrue(value4.IsEven);
			Assert.IsFalse(value5.IsEven);

			Assert.IsTrue(valueBig.IsEven);
		}

		/// <summary>
		/// Tests the <see cref="Int128.IsZero"/> property.
		/// </summary>
		[Test]
		public void IsZeroTest()
		{
			Assert.IsFalse(valuem5.IsZero);
			Assert.IsFalse(valuem4.IsZero);
			Assert.IsFalse(valuem3.IsZero);
			Assert.IsFalse(valuem2.IsZero);
			Assert.IsFalse(valuem1.IsZero);
			Assert.IsTrue(value0.IsZero);
			Assert.IsFalse(value1.IsZero);
			Assert.IsFalse(value2.IsZero);
			Assert.IsFalse(value3.IsZero);
			Assert.IsFalse(value4.IsZero);
			Assert.IsFalse(value5.IsZero);

			Assert.IsFalse(valueBig.IsZero);
		}

		/// <summary>
		/// Tests the <see cref="Int128.IsOne"/> property.
		/// </summary>
		[Test]
		public void IsOneTest()
		{
			Assert.IsFalse(valuem5.IsOne);
			Assert.IsFalse(valuem4.IsOne);
			Assert.IsFalse(valuem3.IsOne);
			Assert.IsFalse(valuem2.IsOne);
			Assert.IsFalse(valuem1.IsOne);
			Assert.IsFalse(value0.IsOne);
			Assert.IsTrue(value1.IsOne);
			Assert.IsFalse(value2.IsOne);
			Assert.IsFalse(value3.IsOne);
			Assert.IsFalse(value4.IsOne);
			Assert.IsFalse(value5.IsOne);

			Assert.IsFalse(valueBig.IsOne);
		}

		/// <summary>
		/// Tests the <see cref="Int128.IsPowerOfTwo"/> property.
		/// </summary>
		[Test]
		public void IsPowerOfTwoTest()
		{
			Assert.IsFalse(valuem5.IsPowerOfTwo);
			Assert.IsFalse(valuem4.IsPowerOfTwo);
			Assert.IsFalse(valuem3.IsPowerOfTwo);
			Assert.IsFalse(valuem2.IsPowerOfTwo);
			Assert.IsFalse(valuem1.IsPowerOfTwo);
			Assert.IsFalse(value0.IsPowerOfTwo);
			Assert.IsTrue(value1.IsPowerOfTwo);
			Assert.IsTrue(value2.IsPowerOfTwo);
			Assert.IsFalse(value3.IsPowerOfTwo);
			Assert.IsTrue(value4.IsPowerOfTwo);
			Assert.IsFalse(value5.IsPowerOfTwo);

			Assert.IsFalse(valueBig.IsPowerOfTwo);
		}

		/// <summary>
		/// Tests the <see cref="Int128.Sign"/> property.
		/// </summary>
		[Test]
		public void SignTest()
		{
			Assert.AreEqual(-1, valuem5.Sign);
			Assert.AreEqual(-1, valuem4.Sign);
			Assert.AreEqual(-1, valuem3.Sign);
			Assert.AreEqual(-1, valuem2.Sign);
			Assert.AreEqual(-1, valuem1.Sign);
			Assert.AreEqual(0, value0.Sign);
			Assert.AreEqual(1, value1.Sign);
			Assert.AreEqual(1, value2.Sign);
			Assert.AreEqual(1, value3.Sign);
			Assert.AreEqual(1, value4.Sign);
			Assert.AreEqual(1, value5.Sign);

			Assert.AreEqual(1, valueBig.Sign);
		}

		/// <summary>
		/// Tests the comparison operators.
		/// </summary>
		[Test]
		public void ComparisonTest()
		{
			var valuem3_2 = valuem3;

			Assert.IsFalse(valuem3 <= valuem5);
			Assert.IsTrue(valuem3 <= valuem3_2);
			Assert.IsTrue(valuem3 <= value0);
			Assert.IsTrue(valuem3 <= value3);
			Assert.IsTrue(valuem3 <= value5);

			Assert.IsFalse(valuem3 < valuem5);
			Assert.IsFalse(valuem3 < valuem3_2);
			Assert.IsTrue(valuem3 < value0);
			Assert.IsTrue(valuem3 < value3);
			Assert.IsTrue(valuem3 < value5);

			Assert.IsFalse(valuem3 == valuem5);
			Assert.IsTrue(valuem3 == valuem3_2);
			Assert.IsFalse(valuem3 == value0);
			Assert.IsFalse(valuem3 == value3);
			Assert.IsFalse(valuem3 == value5);

			Assert.IsTrue(valuem3 > valuem5);
			Assert.IsFalse(valuem3 > valuem3_2);
			Assert.IsFalse(valuem3 > value0);
			Assert.IsFalse(valuem3 > value3);
			Assert.IsFalse(valuem3 > value5);

			Assert.IsTrue(valuem3 >= valuem5);
			Assert.IsTrue(valuem3 >= valuem3_2);
			Assert.IsFalse(valuem3 >= value0);
			Assert.IsFalse(valuem3 >= value3);
			Assert.IsFalse(valuem3 >= value5);
		}

		/// <summary>
		/// Tests the unary arithmetic operators.
		/// </summary>
		[Test]
		public void UnaryArithmeticTest()
		{
			Assert.AreEqual((Int128)5, +value5);
			Assert.AreEqual((Int128)3, +value3);
			Assert.AreEqual((Int128)0, +value0);
			Assert.AreEqual((Int128)(-2), +valuem2);
			Assert.AreEqual((Int128)(-3), +valuem3);

			Assert.AreEqual((Int128)(-5), -value5);
			Assert.AreEqual((Int128)(-3), -value3);
			Assert.AreEqual((Int128)0, -value0);
			Assert.AreEqual((Int128)2, -valuem2);
			Assert.AreEqual((Int128)3, -valuem3);

			Assert.AreEqual((Int128)(-6), ~value5);
			Assert.AreEqual((Int128)(-4), ~value3);
			Assert.AreEqual((Int128)(-1), ~value0);
			Assert.AreEqual((Int128)1, ~valuem2);
			Assert.AreEqual((Int128)2, ~valuem3);
		}

		/// <summary>
		/// Tests the binary arithmetic operators.
		/// </summary>
		[Test]
		public void BinaryArithmeticTest()
		{
			Assert.AreEqual((Int128)8, value5 + value3);
			Assert.AreEqual((Int128)8, value3 + value5);
			Assert.AreEqual((Int128)3, value3 + value0);
			Assert.AreEqual((Int128)3, value0 + value3);
			Assert.AreEqual((Int128)1, valuem2 + value3);
			Assert.AreEqual((Int128)1, value3 + valuem2);

			Assert.AreEqual((Int128)2, value5 - value3);
			Assert.AreEqual((Int128)(-2), value3 - value5);
			Assert.AreEqual((Int128)3, value3 - value0);
			Assert.AreEqual((Int128)(-3), value0 - value3);
			Assert.AreEqual((Int128)(-5), valuem2 - value3);
			Assert.AreEqual((Int128)5, value3 - valuem2);

			Assert.AreEqual((Int128)1, value5 & value3);
			Assert.AreEqual((Int128)1, value3 & value5);
			Assert.AreEqual((Int128)0, value3 & value0);
			Assert.AreEqual((Int128)0, value0 & value3);
			Assert.AreEqual((Int128)2, valuem2 & value3);
			Assert.AreEqual((Int128)2, value3 & valuem2);

			Assert.AreEqual((Int128)7, value5 | value3);
			Assert.AreEqual((Int128)7, value3 | value5);
			Assert.AreEqual((Int128)3, value3 | value0);
			Assert.AreEqual((Int128)3, value0 | value3);
			Assert.AreEqual((Int128)(-1), valuem2 | value3);
			Assert.AreEqual((Int128)(-1), value3 | valuem2);

			Assert.AreEqual((Int128)6, value5 ^ value3);
			Assert.AreEqual((Int128)6, value3 ^ value5);
			Assert.AreEqual((Int128)3, value3 ^ value0);
			Assert.AreEqual((Int128)3, value0 ^ value3);
			Assert.AreEqual((Int128)(-3), valuem2 ^ value3);
			Assert.AreEqual((Int128)(-3), value3 ^ valuem2);

			Assert.AreEqual((Int128)10, value5 << 1);
			Assert.AreEqual((Int128)12, value3 << 2);
			Assert.AreEqual((Int128)3, value3 << 0);
			Assert.AreEqual((Int128)0, value0 << 12345);
			Assert.AreEqual((Int128)(-32), valuem2 << 4);
			Assert.AreEqual((Int128)(-12), valuem3 << 2);

			Assert.AreEqual((Int128)2, value5 >> 1);
			Assert.AreEqual((Int128)0, value3 >> 2);
			Assert.AreEqual((Int128)3, value3 >> 0);
			Assert.AreEqual((Int128)0, value0 >> 12345);
			Assert.AreEqual((Int128)(-1), valuem2 >> 4);
			Assert.AreEqual((Int128)(-1), valuem3 >> 2);

			Assert.AreEqual((Int128)15, value5 * value3);
			Assert.AreEqual((Int128)15, value3 * value5);
			Assert.AreEqual((Int128)0, value3 * value0);
			Assert.AreEqual((Int128)0, value0 * value3);
			Assert.AreEqual((Int128)(-6), valuem2 * value3);
			Assert.AreEqual((Int128)(-6), value3 * valuem2);

			Assert.AreEqual((Int128)1, value5 / value3);
			Assert.AreEqual((Int128)0, value3 / value5);
			Assert.Catch<DivideByZeroException>(() => { Int128 r = value3 / value0; });
			Assert.AreEqual((Int128)0, value0 / value3);
			Assert.AreEqual((Int128)0, valuem2 / value3);
			Assert.AreEqual((Int128)(-1), value3 / valuem2);

			Assert.AreEqual((Int128)2, value5 % value3);
			Assert.AreEqual((Int128)3, value3 % value5);
			Assert.Catch<DivideByZeroException>(() => { Int128 r = value3 % value0; });
			Assert.AreEqual((Int128)0, value0 % value3);
			Assert.AreEqual((Int128)(-2), valuem2 % value3);
			Assert.AreEqual((Int128)1, value3 % valuem2);
		}

		/// <summary>
		/// Tests the <see cref="Int128.Abs"/> method.
		/// </summary>
		[Test]
		public void AbsTest()
		{
			Assert.AreEqual((Int128)5, Int128.Abs(value5));
			Assert.AreEqual((Int128)3, Int128.Abs(value3));
			Assert.AreEqual((Int128)0, Int128.Abs(value0));
			Assert.AreEqual((Int128)2, Int128.Abs(valuem2));
			Assert.AreEqual((Int128)3, Int128.Abs(valuem3));
		}

		/// <summary>
		/// Tests the <see cref="Int128.Max"/> method.
		/// </summary>
		[Test]
		public void MaxTest()
		{
			Assert.AreEqual((Int128)5, Int128.Max(value5, value3));
			Assert.AreEqual((Int128)5, Int128.Max(value3, value5));
			Assert.AreEqual((Int128)3, Int128.Max(value3, value3));
			Assert.AreEqual((Int128)3, Int128.Max(value3, value0));
			Assert.AreEqual((Int128)3, Int128.Max(value0, value3));
			Assert.AreEqual((Int128)3, Int128.Max(valuem2, value3));
			Assert.AreEqual((Int128)(-2), Int128.Max(valuem3, valuem2));
		}

		/// <summary>
		/// Tests the <see cref="Int128.Min"/> method.
		/// </summary>
		[Test]
		public void MinTest()
		{
			Assert.AreEqual((Int128)3, Int128.Min(value5, value3));
			Assert.AreEqual((Int128)3, Int128.Min(value3, value5));
			Assert.AreEqual((Int128)3, Int128.Min(value3, value3));
			Assert.AreEqual((Int128)0, Int128.Min(value3, value0));
			Assert.AreEqual((Int128)0, Int128.Min(value0, value3));
			Assert.AreEqual((Int128)(-2), Int128.Min(valuem2, value3));
			Assert.AreEqual((Int128)(-3), Int128.Min(valuem3, valuem2));
		}

		/// <summary>
		/// Tests the <see cref="Int128.GetPadding"/> method.
		/// </summary>
		[Test]
		public void GetPaddingTest()
		{
			Assert.AreEqual((Int128)3, value5.GetPadding(4));
			Assert.AreEqual((Int128)1, value3.GetPadding(4));
			Assert.AreEqual((Int128)0, value0.GetPadding(4));
		}

		/// <summary>
		/// Tests the <see cref="Int128.Align"/> method.
		/// </summary>
		[Test]
		public void AlignTest()
		{
			Assert.AreEqual((Int128)8, value5.Align(4));
			Assert.AreEqual((Int128)4, value3.Align(4));
			Assert.AreEqual((Int128)0, value0.Align(4));
		}
	}
}
