using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SharpAssembler.OpcodeWriter.Tests
{
	[TestFixture]
	public class ScriptTokenizerTests
	{
		[Test]
		public void RemovesMultiLineComments()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = 
@"Hello /* A multi-line
comment */ world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[]{"Hello", "world"}));
		}

		[Test]
		public void RemovesMultiLineCommentsThatEndTheScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input =
@"Hello world /* A multi-line
comment */";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesMultiLineCommentsThatStartTheScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input =
@"/* A multi-line
comment */ Hello world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesMultiLineCommentsWithCRLF()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "/* A multi-line \r\n comment */ Hello world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesMultiLineCommentsWithCR()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "/* A multi-line \r comment */ Hello world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesMultiLineCommentsWithLF()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "/* A multi-line \n comment */ Hello world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesSingleLineComments()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input =
@"Hello // A single-line comment
world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesSingleLineCommentsThatEndsTheScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello world // A single-line comment";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesSingleLineCommentsThatStartsTheScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input =
@"// A single-line comment
Hello world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesSingleLineCommentsWithCRLF()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello // A single-line comment\r\nworld";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesSingleLineCommentsWithCR()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello // A single-line comment\rworld";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void RemovesSingleLineCommentsWithLF()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello // A single-line comment\nworld";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void NonCommentSlashAtEndOfScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello world /";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world", "/" }));
		}

		[Test]
		public void EmptySingleLineCommentAtEndOfScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello world //";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void EmptyMultiLineCommentAtEndOfScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello world /**/";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void NestedMultiLineComments()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello /* Nested /* Comment! */ world */";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world", "*/" }));
		}

		[Test]
		public void String()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello \"My string\" world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "\"My string\"", "world"}));
		}

		[Test]
		public void Literal2()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello 'My string' world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "'My string'", "world" }));
		}

		[Test]
		public void Literal3()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello `My string` world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "`My string`", "world" }));
		}

		[Test]
		public void NestedLiterals()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello `My \"nested\" string` world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "`My \"nested\" string`", "world" }));
		}

		[Test]
		public void StringAtEndOfScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello \"My string\"";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "\"My string\"" }));
		}

		[Test]
		public void StringAtStartOfScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "\"My string\" Hello";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "\"My string\"", "Hello" }));
		}

		[Test]
		public void SpecialTokens()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello{}()world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "{", "}", "(", ")", "world" }));
		}

		[Test]
		public void SpecialTokensAtStartOfScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "{} Hello()world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "{", "}", "Hello", "(", ")", "world" }));
		}

		[Test]
		public void SpecialTokensAtEndOfScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello()world {}";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "(", ")", "world", "{", "}" }));
		}

		[Test]
		public void SpecialTokensInStringsAreNotTokenized()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello \"My {}() string\" world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "\"My {}() string\"", "world" }));
		}

		[Test]
		public void SpecialTokensInCommentsAreNotTokenized()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "Hello /*My {}() comment*/ world";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[] { "Hello", "world" }));
		}

		[Test]
		public void EmptyScript()
		{
			// Given
			ScriptTokenizer tokenizer = new ScriptTokenizer();
			string input = "";

			// When
			var tokens = tokenizer.Tokenize(input);

			// Then
			Assert.That(tokens, Is.EquivalentTo(new string[0]));
		}
	}
}
