using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SharpAssembler.OpcodeWriter.Tests
{
	[TestFixture]
	public class ScriptReaderTests
	{
		#region Basic Operations
		[Test]
		public void ReadReturnsCurrentTokenAndAdvancesPosition()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"a", "b", "c"
			});

			// When
			var token1 = reader.Read();
			var token2 = reader.Read();
			var token3 = reader.Read();

			// Then
			Assert.That(token1, Is.EqualTo("a"));
			Assert.That(token2, Is.EqualTo("b"));
			Assert.That(token3, Is.EqualTo("c"));
		}

		[Test]
		public void ReadAtEndOfStreamThrowsException()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"a"
			});

			// When
			reader.Read();

			// Then
			Assert.Throws<ScriptException>(() => reader.Read());
		}

		[Test]
		public void PeekReturnsCurrentToken()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"a", "b", "c"
			});

			// When
			var token1 = reader.Peek();
			reader.Read();
			var token2 = reader.Peek();
			reader.Read();
			var token3 = reader.Peek();

			// Then
			Assert.That(token1, Is.EqualTo("a"));
			Assert.That(token2, Is.EqualTo("b"));
			Assert.That(token3, Is.EqualTo("c"));
		}

		[Test]
		public void PeekAtEndOfStreamReturnsNull()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"a"
			});

			// When
			reader.Read();
			var token = reader.Peek();

			// Then
			Assert.That(token, Is.Null);
		}

		[Test]
		public void ReadToEndOfFileSetsEndOfFilePropertyToTrue()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"a"
			});

			// When
			Assert.That(reader.EndOfFile, Is.False);
			reader.Read();

			// Then
			Assert.That(reader.EndOfFile, Is.True);
		}

		[Test]
		public void EmptyStreamHasEndOfFilePropertySetToTrue()
		{
			// Given
			ScriptReader reader = new ScriptReader(Enumerable.Empty<string>());

			// Then
			Assert.That(reader.EndOfFile, Is.True);
		}
		#endregion

		#region Identifiers
		[Test]
		public void ReadIdentifierAcceptsValidUnquotedIdentifiers()
		{
			// Given
			string[] identifiers = new string[]{
				"abc", "ABC", "_abC", "%abC", "z91.f34.2/_3+3*2#\\25%"
			};
			ScriptReader reader = new ScriptReader(identifiers);

			// When
			string[] results = new string[identifiers.Length];
			for (int i = 0; i < identifiers.Length; i++)
				results[i] = reader.ReadIdentifier();

			// Then
			Assert.That(results, Is.EquivalentTo(identifiers));
		}

		[Test]
		public void ReadIdentifierAcceptsQuotedIdentifiers()
		{
			// Given
			string[] identifiers = new string[]{
				"`abc`", "`abc def ghi`", "`go (test) {something} \"subquotes\"`"
			};
			ScriptReader reader = new ScriptReader(identifiers);

			// When
			string[] results = new string[identifiers.Length];
			for (int i = 0; i < identifiers.Length; i++)
				results[i] = reader.ReadIdentifier();

			// Then
			Assert.That(results, Is.EquivalentTo(from i in identifiers select i.Trim('`')));
		}

		[Test]
		public void ReadIdentifierThrowsOnInvalidIdentifier()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"9@2"
			});

			// Then
			Assert.Throws<ScriptException>(() => reader.ReadIdentifier());
		}

		[Test]
		public void PeekIdentifierWorksLikeReadIdentifier()
		{
			// Given
			string[] identifiers = new string[]{
				"abc", "ABC", "_abC", "%abC", "z91.f34.2/_3+3*2#\\25%",
				"`abc`", "`abc def ghi`", "`go (test) {something} \"subquotes\"`"
			};
			ScriptReader reader = new ScriptReader(identifiers);

			// When
			string[] results = new string[identifiers.Length];
			for (int i = 0; i < identifiers.Length; i++)
			{
				results[i] = reader.PeekIdentifier();
				reader.Read();
			}

			// Then
			Assert.That(results, Is.EquivalentTo(from i in identifiers select i.Trim('`')));
		}

		[Test]
		public void PeekIdentifierReturnsNullOnInvalidIdentifier()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"9@2"
			});

			// When
			string result = reader.PeekIdentifier();

			// Then
			Assert.That(result, Is.Null);
		}
		#endregion

		#region Strings
		[Test]
		public void ReadStringAcceptsQuotedStrings()
		{
			// Given
			string[] strings = new string[]{
				"\"abc\"", "\"abc def {ghi jkl}\""
			};
			ScriptReader reader = new ScriptReader(strings);

			// When
			string[] results = new string[strings.Length];
			for (int i = 0; i < strings.Length; i++)
				results[i] = reader.ReadString();

			// Then
			Assert.That(results, Is.EquivalentTo(from s in strings select s.Trim('"')));
		}

		[Test]
		public void ReadStringThrowsOnInvalidString()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"9@2"
			});

			// Then
			Assert.Throws<ScriptException>(() => reader.ReadString());
		}

		[Test]
		public void PeekStringWorksLikeReadString()
		{
			// Given
			string[] strings = new string[]{
				"\"abc\"", "\"abc def {ghi jkl}\""
			};
			ScriptReader reader = new ScriptReader(strings);

			// When
			string[] results = new string[strings.Length];
			for (int i = 0; i < strings.Length; i++)
			{
				results[i] = reader.PeekString();
				reader.Read();
			}

			// Then
			Assert.That(results, Is.EquivalentTo(from i in strings select i.Trim('"')));
		}

		[Test]
		public void PeekStringReturnsNullOnInvalidString()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"9@2"
			});

			// When
			string result = reader.PeekString();

			// Then
			Assert.That(result, Is.Null);
		}
		#endregion

		#region Integers
		[Test]
		public void ReadIntegerAcceptsDecimalIntegers()
		{
			// Given
			string[] values = new string[]{
				"123", "0123", "-123"
			};
			ScriptReader reader = new ScriptReader(values);

			// When
			int[] results = new int[values.Length];
			for (int i = 0; i < values.Length; i++)
				results[i] = reader.ReadInteger();

			// Then
			Assert.That(results, Is.EquivalentTo(new int[]{
				123, 123, -123
			}));
		}

		[Test]
		public void ReadIntegerAcceptsHexadecimalIntegers()
		{
			// Given
			string[] values = new string[]{
				"0xDE12", "0x0DE12"
			};
			ScriptReader reader = new ScriptReader(values);

			// When
			int[] results = new int[values.Length];
			for (int i = 0; i < values.Length; i++)
				results[i] = reader.ReadInteger();

			// Then
			Assert.That(results, Is.EquivalentTo(new int[]{
				0xDE12, 0xDE12
			}));
		}

		[Test]
		public void ReadIntegerThrowsOnInvalidInteger()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"9@2"
			});

			// Then
			Assert.Throws<ScriptException>(() => reader.ReadInteger());
		}

		[Test]
		public void PeekIntegerWorksLikeReadInteger()
		{
			// Given
			string[] values = new string[]{
				"123", "0123", "-123",
				"0xDE12", "0x0DE12"
			};
			ScriptReader reader = new ScriptReader(values);

			// When
			int?[] results = new int?[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				results[i] = reader.PeekInteger();
				reader.Read();
			}

			// Then
			Assert.That(results, Is.EquivalentTo(new int[]{
				123, 123, -123,
				0xDE12, 0xDE12
			}));
		}

		[Test]
		public void PeekIntegerReturnsNullOnInvalidInteger()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"9@2"
			});

			// When
			int? result = reader.PeekInteger();

			// Then
			Assert.That(result, Is.Null);
		}
		#endregion

		#region Regions and Lists
		[Test]
		public void ReadRegionStart_TryReadRegionEnd()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"keyword", "{", "sub", "(", "subsub", ")", "}"
			});

			// When
			reader.Read();
			int level1 = reader.ReadRegionStart(ScriptReader.RegionType.CurlyBrackets);
			reader.Read();
			bool tryExitLevel1 = reader.TryReadRegionEnd(level1);
			int level2 = reader.ReadRegionStart(ScriptReader.RegionType.Parentheses);
			bool tryExitLevel2 = reader.TryReadRegionEnd(level2);
			reader.Read();
			bool exitLevel2 = reader.TryReadRegionEnd(level2);
			bool exitLevel1 = reader.TryReadRegionEnd(level1);

			// Then
			Assert.That(level1, Is.EqualTo(1));
			Assert.That(tryExitLevel1, Is.False);
			Assert.That(level2, Is.EqualTo(2));
			Assert.That(tryExitLevel2, Is.False);
			Assert.That(exitLevel2, Is.True);
			Assert.That(exitLevel1, Is.True);
		}

		[Test]
		public void ReadRegion()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"keyword", "{", "sub", "(", "subsub", ")", "}"
			});

			// When
			reader.Read();
			reader.ReadRegion(ScriptReader.RegionType.CurlyBrackets, () =>
			{
				reader.Read();
				reader.ReadRegion(ScriptReader.RegionType.Parentheses, () =>
				{
					reader.Read();
				});
			});
			bool eof = reader.EndOfFile;

			// Then
			Assert.That(eof, Is.True);
		}

		[Test]
		public void ReadRegionRepeat()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"keyword", "{", "sub", "sub", "sub", "}"
			});

			// When
			int count = 0;
			reader.Read();
			reader.ReadRegionAndRepeat(ScriptReader.RegionType.CurlyBrackets, () =>
			{
				reader.Read();
				count++;
			});
			bool eof = reader.EndOfFile;

			// Then
			Assert.That(count, Is.EqualTo(3));
			Assert.That(eof, Is.True);
		}

		[Test]
		public void ReadList()
		{
			// Given
			ScriptReader reader = new ScriptReader(new string[]{
				"keyword", "(", "item1", ",", "item2", ",", "item3", ")", "keyword"
			});

			// When
			reader.Read();
			List<string> items = new List<string>();
			reader.ReadListInRegion(ScriptReader.RegionType.Parentheses, ",",
				() => items.Add(reader.Read()));
			reader.Read();

			// Then
			Assert.That(items, Is.EquivalentTo(new [] { "item1", "item2", "item3" }));
		}
		#endregion
	}
}
