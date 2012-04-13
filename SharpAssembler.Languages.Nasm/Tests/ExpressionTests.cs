using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using SharpAssembler.Symbols;
using System.IO;

namespace SharpAssembler.Languages.Nasm.Tests
{
	/// <summary>
	/// Tests whether expressions are properly translated to NASM.
	/// </summary>
	[TestFixture]
	public class ExpressionTests
	{
		[Test]
		public void BinaryBitwiseOperators()
		{
			AssertExpression("$ | 0xFFFF",
				c => c.Address | 0xFFFF);

			AssertExpression("$ ^ 0xFFFF",
				c => c.Address ^ 0xFFFF);

			AssertExpression("$ & 0xFFFF",
				c => c.Address & 0xFFFF);

			AssertExpression("$ << 10",
				c => c.Address << 10);

			AssertExpression("$ >> 10",
				c => c.Address >> 10);
		}

		[Test]
		public void BinaryArithmeticOperators()
		{
			AssertExpression("$ + 0xFFFF",
				c => c.Address + 0xFFFF);

			AssertExpression("$ - 0xFFFF",
				c => c.Address - 0xFFFF);

			AssertExpression("$ * 0xFFFF",
				c => c.Address * 0xFFFF);
		}

		[Test]
		public void UnsignedDivision()
		{
			AssertExpression("$ / 0xFFFF",
				c => c.Address / (ulong)0xFFFF);
		}

		[Test]
		public void SignedDivision()
		{
			AssertExpression("$ // 0xFFFF",
				c => c.Address / (long)0xFFFF);
		}

		[Test]
		public void UnsignedModulo()
		{
			AssertExpression("$ % 0x100",
				c => c.Address % (ulong)0x100);
		}

		[Test]
		public void SignedModulo()
		{
			AssertExpression("$ %% 0x100",
				c => c.Address % (long)0x100);
		}

		[Test]
		public void References()
		{
			AssertExpression("10 + test",
				c => 10 + new Reference("test"));

			AssertExpression("test + 0xABCD",
				c => new Reference("test") + 0xABCD);

			AssertExpression("test",
				c => "test");
		}

		[Test]
		public void Constants()
		{
			AssertExpression("0xABCD",
				c => 0xABCD);

			AssertExpression("10",
				c => 10);
		}

		[Test]
		public void SectionOffset()
		{
			AssertExpression("0xF00000000 - $$",
				c => 0xF00000000 - c.Section.Address);

			AssertExpression("$ - $$",
				c => c.Address - c.Section.Address);
		}

		[Test]
		public void Offset()
		{
			AssertExpression("0xF00000000 - $",
				c => 0xF00000000 - c.Address);

			AssertExpression("$ - $$",
				c => c.Address - c.Section.Address);
		}

		/// <summary>
		/// Asserts that the specified expression written using the <see cref="NasmExpressionWriter"/>
		/// is equal to the specified string.
		/// </summary>
		/// <param name="expected">The expected string.</param>
		/// <param name="actual">The actual expression.</param>
		private void AssertExpression(string expected, Expression<Func<Context, ReferenceOffset>> actual)
		{
			string actualString;
			using (StringWriter sw = new StringWriter())
			{
				NasmExpressionWriter expressionWriter = new NasmExpressionWriter(sw);
				expressionWriter.Write(actual);

				actualString = sw.ToString();
			}

			Assert.That(actualString, Is.EqualTo(expected));
		}
	}
}
