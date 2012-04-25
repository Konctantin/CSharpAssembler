using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Reads scripts.
	/// </summary>
	public class ScriptReader
	{
		/// <summary>
		/// A stack of tokens.
		/// </summary>
		private readonly Stack<string> tokens;

		//private readonly IEnumerator<string> tokenEnumerator;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptReader"/> class.
		/// </summary>
		public ScriptReader()
			: this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptReader"/> class.
		/// </summary>
		/// <param name="tokens">The tokens.</param>
		public ScriptReader(IEnumerable<string> tokens)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(tokens != null);
			#endregion

			this.tokens = new Stack<string>(tokens.Reverse());
		}
		#endregion

		#region Basic Methods
		/// <summary>
		/// Gets whether the reader is positioned at the end of file.
		/// </summary>
		/// <value><see langword="true"/> when the reader is positioned at the end of file;
		/// otherwise, <see langword="false"/>.</value>
		public bool EndOfFile
		{
			get { return this.tokens.Count == 0; }
		}

		/// <summary>
		/// Reads a single token.
		/// </summary>
		/// <returns>The token that was read.</returns>
		/// <exception cref="ScriptException">
		/// Unexpected end of file.
		/// </exception>
		public string Read()
		{
			if (this.EndOfFile)
				throw new ScriptException("Unexpected end of file.");

			return this.tokens.Pop();
		}

		/// <summary>
		/// Peeks at the next token.
		/// </summary>
		/// <returns>The token that will be returned by the next call to <see cref="Read"/>;
		/// or <see langword="null"/> when there are no more tokens.</returns>
		public string Peek()
		{
			if (this.EndOfFile)
				return null;

			return this.tokens.Peek();
		}

		/// <summary>
		/// Prepends the specified list of tokens to the current reader.
		/// </summary>
		/// <param name="tokens">The tokens to prepend.</param>
		public void Prepend(IEnumerable<string> tokens)
		{
			foreach (var token in tokens.Reverse())
				this.tokens.Push(token);
		}
		#endregion

		#region Identifiers
		/// <summary>
		/// The regular expression to which identifiers must adhere.
		/// </summary>
		private static readonly Regex IdentifierRegex = new Regex("^[a-zA-Z_%][a-zA-Z0-9_%:&/\\\\+*#\\-.]*$", RegexOptions.Compiled);

		/// <summary>
		/// Reads an identifier.
		/// </summary>
		/// <returns>The read identifier, without escape sequences or surrounding quotes.</returns>
		/// <remarks>
		/// An identifier is any combination of characters enclosed by backticks,
		/// or a literal that adheres to a particular regular expression pattern.
		/// </remarks>
		/// <exception cref="ScriptException">
		/// <para>The token is not a valid identifier.</para>
		/// -or-
		/// <para>Unexpected end of file.</para>
		/// </exception>
		public string ReadIdentifier()
		{
			// THROWS: ScriptException
			string token = Read();
			string identifier = ToIdentifier(token);
			if (identifier == null)
				throw new ScriptException(String.Format("Not a valid identifier: {0}", token));
			return identifier;
		}

		/// <summary>
		/// Peeks an identifier.
		/// </summary>
		/// <returns>The read identifier, without escape sequences or surrounding quotes;
		/// or <see langword="null"/> when the next token is not a valid identifier.</returns>
		/// <remarks>
		/// An identifier is any combination of characters enclosed by backticks,
		/// or a literal that adheres to a particular regular expression pattern.
		/// </remarks>
		public string PeekIdentifier()
		{
			return ToIdentifier(Peek());
		}

		/// <summary>
		/// Returns the given token converted to an identifier, if possible.
		/// </summary>
		/// <param name="token">The token to convert.</param>
		/// <returns>The actual identifier; or <see langword="null"/> when it fails.</returns>
		private string ToIdentifier(string token)
		{
			if (token == null)
				return null;

			if (token.StartsWith("`"))
				return token.Trim('`');
			else if (IdentifierRegex.IsMatch(token))
				return token;
			else
				return null;
		}
		#endregion

		#region Strings
		/// <summary>
		/// Reads a string.
		/// </summary>
		/// <returns>The read string, without escape sequences or surrounding quotes.</returns>
		/// <remarks>
		/// A string is any combination of characters enclosed by double quotes.
		/// </remarks>
		/// <exception cref="ScriptException">
		/// <para>The token is not a valid string.</para>
		/// -or-
		/// <para>Unexpected end of file.</para>
		/// </exception>
		public string ReadString()
		{
			// THROWS: ScriptException
			string token = Read();
			string str = ToString(token);
			if (str == null)
				throw new ScriptException(String.Format("Not a valid string: {0}", token));
			return str;
		}

		/// <summary>
		/// Peeks a string.
		/// </summary>
		/// <returns>The read string, without escape sequences or surrounding quotes;
		/// or <see langword="null"/> when the next token is not a valid string.</returns>
		/// <remarks>
		/// A string is any combination of characters enclosed by double quotes.
		/// </remarks>
		public string PeekString()
		{
			return ToString(Peek());
		}

		/// <summary>
		/// Returns the given token converted to a string, if possible.
		/// </summary>
		/// <param name="token">The token to convert.</param>
		/// <returns>The actual string; or <see langword="null"/> when it fails.</returns>
		private string ToString(string token)
		{
			if (token == null)
				return null;

			if (token.StartsWith("\""))
				return token.Trim('"');
			else
				return null;
		}
		#endregion

		#region Integers
		/// <summary>
		/// Reads an integer.
		/// </summary>
		/// <returns>The read integer.</returns>
		/// <remarks>
		/// An integer may be in decimal form or hexadecimal form (starting with <c>0x</c>).
		/// </remarks>
		/// <exception cref="ScriptException">
		/// <para>The token is not a valid integer, or out of range.</para>
		/// -or-
		/// <para>Unexpected end of file.</para>
		/// </exception>
		public int ReadInteger()
		{
			// THROWS: ScriptException
			string token = Read();
			int? value = ToInteger(token);
			if (!value.HasValue)
				throw new ScriptException(String.Format("Not a valid integer, or out of range: {0}", token));
			return value.Value;
		}

		/// <summary>
		/// Peeks an integer.
		/// </summary>
		/// <returns>The read integer;
		/// or <see langword="null"/> when the next token is not a valid integer.</returns>
		/// <remarks>
		/// An integer may be in decimal form or hexadecimal form (starting with <c>0x</c>).
		/// </remarks>
		public int? PeekInteger()
		{
			return ToInteger(Peek());
		}

		/// <summary>
		/// Returns the given token converted to an integer, if possible.
		/// </summary>
		/// <param name="token">The token to convert.</param>
		/// <returns>The actual integer; or <see langword="null"/> when it fails.</returns>
		private int? ToInteger(string token)
		{
			if (token == null)
				return null;

			int result;
			if (token.StartsWith("0x"))
			{
				if (!Int32.TryParse(token.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
					return null;
			}
			else
			{
				if (!Int32.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
					return null;
			}

			return result;
		}
		#endregion

		#region Regions
		/// <summary>
		/// Specifies a type of region
		/// </summary>
		public enum RegionType
		{
			/// <summary>
			/// A region surrounded by parentheses.
			/// </summary>
			Parentheses,
			/// <summary>
			/// A region surrounded by curly brackets.
			/// </summary>
			CurlyBrackets,
			/// <summary>
			/// A region surrounded by square brackets.
			/// </summary>
			SquareBrackets,
			/// <summary>
			/// A region surrounded by angled brackets.
			/// </summary>
			AngledBrackets,
			/// <summary>
			/// A special region that uses curly brackets or, when empty, may be
			/// written as a semi-colon.
			/// </summary>
			Body,
		}

		/// <summary>
		/// Specifies the character that starts the region.
		/// </summary>
		private static readonly char[] RegionStart = new char[] { '(', '{', '[', '<' };

		/// <summary>
		/// Specifies the character that ends the region.
		/// </summary>
		private static readonly char[] RegionEnd = new char[] { ')', '}', ']', '>' };

		/// <summary>
		/// For each region that is entered, this stack tracks the type of region.
		/// </summary>
		private Stack<RegionType> regions = new Stack<RegionType>();

		/// <summary>
		/// Reads the start of the specified type of region.
		/// </summary>
		/// <param name="type">The type of region.</param>
		/// <returns>The depth of the region that was started.</returns>
		/// <exception cref="ScriptException">
		/// <para>Expected start of region.</para>
		/// -or-
		/// <para>Unexpected end of file.</para>
		/// </exception>
		public int ReadRegionStart(RegionType type)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(RegionType), type));
			#endregion
			// THROWS: ScriptException
			string token = Read();
			if (type == RegionType.Body)
			{
				if (token.Equals(";"))
				{
					regions.Push(type);
					return regions.Count;
				}
				else
					type = RegionType.CurlyBrackets;
			}

			if (token.Length != 1 || token[0] != RegionStart[(int)type])
				throw new ScriptException(String.Format("Expected start of the '{0}' region.", RegionStart[(int)type]));

			regions.Push(type);
			return regions.Count;
		}

		/// <summary>
		/// Attempts to read the end of the region.
		/// </summary>
		/// <param name="level">The level of region to end.</param>
		/// <returns><see langword="true"/> when the end of the region was read;
		/// otherwise, <see langword="false"/>.</returns>
		public bool TryReadRegionEnd(int level)
		{
			if (regions.Count > level)
				throw new ScriptException("One or more subsections were not closed properly.");
			if (regions.Count < level)
				throw new ScriptException("More subsections are closed than expected.");

			RegionType type = regions.Peek();

			if (type == RegionType.Body)
			{
				// We actually never entered the subsection, so we leave it immediately.
				regions.Pop();
				return true;
			}

			string token = Peek();
			if (token == null)
				throw new ScriptException("Unexpected end of file.");

			if (token.Equals(RegionEnd[(int)type].ToString()))
			{
				// Exit the entered subsection.
				Read();
				regions.Pop();
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Reads a region.
		/// </summary>
		/// <param name="region">The type of region.</param>
		/// <param name="action">The action to perform in the region.</param>
		/// <exception cref="ScriptException">
		/// <para>Expected end of region.</para>
		/// </exception>
		public void ReadRegion(RegionType region, Action action)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(RegionType), region));
			#endregion

			int level = ReadRegionStart(region);

			if (action != null)
				action();

			if (!TryReadRegionEnd(level))
				throw new ScriptException("Expected end of region.");
		}

		/// <summary>
		/// Reads a region and repeats a given action as often as possible.
		/// </summary>
		/// <param name="region">The type of region.</param>
		/// <param name="action">The action to repeat in the region.</param>
		/// <exception cref="ScriptException">
		/// <para>Expected end of region.</para>
		/// </exception>
		public void ReadRegionAndRepeat(RegionType region, Action action)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(RegionType), region));
			#endregion

			int level = ReadRegionStart(region);
			while (!TryReadRegionEnd(level))
			{
				if (action != null)
					action();	
			}
		}
		#endregion

		#region Lists
		/// <summary>
		/// Reads a list in a region.
		/// </summary>
		/// <param name="region">The type of region.</param>
		/// <param name="separator">The list item separator.</param>
		/// <param name="action">The action to perform at the start of each item.</param>
		public void ReadListInRegion(RegionType region, string separator, Action action)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(RegionType), region));
			Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(separator));
			#endregion

			int level = ReadRegionStart(region);
			if (!TryReadRegionEnd(level))
			{
				string next;
				do
				{
					if (action != null)
						action();

					next = Peek();
					if (next.Equals(separator))
						Read();
				} while (next.Equals(separator));
				if (!TryReadRegionEnd(level))
					throw new ScriptException("Expected end of list.");
			}
		}
		#endregion
	}
}
