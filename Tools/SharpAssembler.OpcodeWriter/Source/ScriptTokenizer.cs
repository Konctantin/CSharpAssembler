using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// A script tokenizer.
	/// </summary>
	public sealed class ScriptTokenizer
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptTokenizer"/> class.
		/// </summary>
		public ScriptTokenizer()
		{

		}
		#endregion

		/// <summary>
		/// The tokens that should be used by themselves, when not used in strings.
		/// </summary>
		private static readonly char[] SpecialTokens = new char[] { '{', '}', '[', ']', '(', ')', ';', ',', '=' };

		/// <summary>
		/// Divides the input into separate tokens, and may perform preprocessing steps before doing so
		/// (such as removing comments).
		/// </summary>
		/// <param name="input">The script input.</param>
		/// <returns>An enumerable ordered collection of strings, each representing one token.</returns>
		public IEnumerable<string> Tokenize(string input)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
			#endregion

			var subparts = SplitInSubparts(input);
			var noComments = from s in subparts where !s.StartsWith("//") && !s.StartsWith("/*") select s;
			var tokenized = SplitNonStringsIntoTokens(noComments);
			return tokenized;
		}

		/// <summary>
		/// Splits the parts that do not represent strings into tokens.
		/// </summary>
		/// <param name="parts">The parts.</param>
		/// <returns>The split strings.</returns>
		private IEnumerable<string> SplitNonStringsIntoTokens(IEnumerable<string> parts)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(parts != null);
			Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
			#endregion
			foreach (string s in parts)
			{
				if (!s.StartsWith("\""))
				{
					var tokens = from t in SplitOnTokens(s, SpecialTokens)
								 from u in t.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
								 select u;
					foreach (var token in tokens)
						yield return token;
				}
				else
					yield return s;
			}
		}

		/// <summary>
		/// Splits a string on the characters specified in the array, but keeps the characters in the result.
		/// </summary>
		/// <param name="str">The string to split.</param>
		/// <param name="tokens">The characters to split on.</param>
		/// <returns>The split string.</returns>
		private IEnumerable<string> SplitOnTokens(string str, char[] tokens)
		{
			int position = 0;
			int index = 0;

			while ((index = str.IndexOfAny(tokens, position)) != -1)
			{
				if (index - position > 0)
					yield return str.Substring(position, index - position);
				yield return str.Substring(index, 1);
				position = index + 1;
			}

			if (position < str.Length)
				yield return str.Substring(position);
		}

		/// <summary>
		/// Splits the input string into strings (starting and ending with a double quote)
		/// and single- and multi-line comments (starting with <c>//</c> or <c>/*</c> respectively).
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>The splitted strings.</returns>
		private IEnumerable<string> SplitInSubparts(string input)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
			#endregion

			SortedSet<int> splits = new SortedSet<int>();
			State state = State.Normal;
			int position = 0;

			while (state != State.Done)
			{
				switch (state)
				{
					case State.Normal:
						state = ToNextSubpartStart(input, ref position, splits);
						break;
					case State.InString:
						state = ToEndOfString(input, ref position, splits);
						break;
					case State.InSingleLineComment:
						state = ToEndOfSingleLineComment(input, ref position, splits);
						break;
					case State.InMultiLineComment:
						state = ToEndOfMultiLineComment(input, ref position, splits);
						break;
					case State.Done:
						break;
					default:
						throw new NotImplementedException();
				}
			}

			// Split the string at the split points.
			List<string> split = new List<string>();
			int start = 0;
			foreach (var pos in splits)
			{
				if (pos - start > 0)
					split.Add(input.Substring(start, pos - start));
				start = pos;
			}
			split.Add(input.Substring(start));
			return split;
		}

		/// <summary>
		/// Goes to the next start of a sub part (string or comment).
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="position">The current zero-based character position within <paramref name="input"/>.</param>
		/// <param name="splits">A set of indices of characters before which the input string will be split.</param>
		/// <returns>The new state.</returns>
		private State ToNextSubpartStart(string input, ref int position, SortedSet<int> splits)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position <= input.Length);
			Contract.Requires<ArgumentNullException>(splits != null);
			#endregion
			int next = input.IndexOfAny(new char[] { '"', '/' }, position);

			bool foundNothing = next < 0;
			bool atEndOfLine = position >= input.Length - 1;

			if (atEndOfLine || foundNothing)
			{
				// We have reached the end of the input string,
				// or we've found a slash just as the last character of the string.
				position = input.Length - 1;
				return State.Done;
			}

			bool notASubpart =
				input[next] == '/' &&									// We did find the start of a comment
				(next >= input.Length - 1 ||								// but it is the last character of the string
				!(input[next + 1] == '*' || input[next + 1] == '/'));	// or it is not part of the "//" or "/*" sequence.
			
			if (notASubpart)
			{
				// We found something that looked like the start of a subpart, but it wasn't.
				position = next + 1;
				return State.Normal;
			}

			splits.Add(next);

			if (input[next] == '"')
			{
				position = next + 1;
				return State.InString;
			}
			else if (input[next + 1] == '/')
			{
				position = next + 2;
				return State.InSingleLineComment;
			}
			else if (input[next + 1] == '*')
			{
				position = next + 2;
				return State.InMultiLineComment;
			}

			throw new NotImplementedException();
		}

		/// <summary>
		/// Goes to the end of the string sub part.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="position">The current zero-based character position within <paramref name="input"/>.</param>
		/// <param name="splits">A set of indices of characters before which the input string will be split.</param>
		/// <returns>The new state.</returns>
		private State ToEndOfString(string input, ref int position, SortedSet<int> splits)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < input.Length);
			Contract.Requires<ArgumentNullException>(splits != null);
			#endregion

			int next = input.IndexOf('"', position);
			if (next < 0)
				throw new ScriptException(String.Format("End of string started at index {0} not found.", splits.Last()));
			position = next + 1;
			splits.Add(position);

			return State.Normal;
		}

		/// <summary>
		/// Goes to the end of the single-line comment sub part.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="position">The current zero-based character position within <paramref name="input"/>.</param>
		/// <param name="splits">A set of indices of characters before which the input string will be split.</param>
		/// <returns>The new state.</returns>
		private State ToEndOfSingleLineComment(string input, ref int position, SortedSet<int> splits)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position <= input.Length);
			Contract.Requires<ArgumentNullException>(splits != null);
			#endregion

			int next = input.IndexOfAny(new char[] { '\r', '\n' }, position);
			if (next >= 0)
			{
				if (next < input.Length - 1 && input[next + 1] == '\n')
					position = next + 2;
				else
					position = next + 1;
				splits.Add(position);
			}
			return State.Normal;
		}

		/// <summary>
		/// Goes to the end of the multi-line comment sub part.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="position">The current zero-based character position within <paramref name="input"/>.</param>
		/// <param name="splits">A set of indices of characters before which the input string will be split.</param>
		/// <returns>The new state.</returns>
		private State ToEndOfMultiLineComment(string input, ref int position, SortedSet<int> splits)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(input != null);
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < input.Length);
			Contract.Requires<ArgumentNullException>(splits != null);
			#endregion

			int next = input.IndexOf("*/", position);
			if (next < 0)
				throw new ScriptException(String.Format("End of comment started at index {0} not found.", splits.Last()));
			position = next + 2;
			splits.Add(position);

			return State.Normal;
		}

		#region State Enum
		/// <summary>
		/// Specifies the state of the tokenizer.
		/// </summary>
		private enum State
		{
			/// <summary>
			/// Normal and initial state.
			/// </summary>
			Normal,
			/// <summary>
			/// The tokenizer entered a single-line comment.
			/// </summary>
			InSingleLineComment,
			/// <summary>
			/// The tokenizer entered a multi-line comment.
			/// </summary>
			InMultiLineComment,
			/// <summary>
			/// The tokenizer entered a string.
			/// </summary>
			InString,
			/// <summary>
			/// The tokenizer is done.
			/// </summary>
			Done,
		}
		#endregion
	}
}
