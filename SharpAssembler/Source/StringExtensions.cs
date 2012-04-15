using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// Extensions to the <see cref="String"/> class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Splits the specified string before or after each of the specified characters,
		/// and keeps the characters in the resulting substrings.
		/// </summary>
		/// <param name="str">The string to split.</param>
		/// <param name="beforeChars">The characters before which the substring is split.</param>
		/// <param name="afterChars">The characters after which the substring is split.</param>
		/// <returns>The resulting substrings.</returns>
		internal static IList<String> SplitAndKeep(this string str, char[] beforeChars, char[] afterChars)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(str != null);
			Contract.Requires<ArgumentNullException>(beforeChars != null);
			Contract.Requires<ArgumentNullException>(afterChars != null);
			#endregion

			List<string> substrings = new List<string>();

			IList<int> beforeIndices = str.IndexOfEach(beforeChars);
			IList<int> afterIndices = str.IndexOfEach(afterChars).Select(ind => ind + 1).ToList();

			IEnumerable<int> splitIndices = Enumerable.Union(beforeIndices, afterIndices).OrderBy(ind => ind);

			int lastIndex = 0;
			foreach (int index in splitIndices)
			{
				substrings.Add(str.Substring(lastIndex, index - lastIndex));
				lastIndex = index;
			}
			if (lastIndex < str.Length)
				substrings.Add(str.Substring(lastIndex));

			return substrings;
		}

		/// <summary>
		/// Returns the indices at which any of the specified characters are found in the string.
		/// </summary>
		/// <param name="str">The string to search.</param>
		/// <param name="eachOf">The characters to look for.</param>
		/// <returns>The indices of each of the occurences of the specified characters
		/// in the specified string.</returns>
		internal static IList<int> IndexOfEach(this string str, char[] eachOf)
		{
			return IndexOfEach(str, eachOf, 0, str.Length);
		}

		/// <summary>
		/// Returns the indices at which any of the specified characters are found in the string.
		/// </summary>
		/// <param name="str">The string to search.</param>
		/// <param name="eachOf">The characters to look for.</param>
		/// <param name="startIndex">The zero-base index of the first character of the string
		/// to include in the search.</param>
		/// <returns>The indices of each of the occurences of the specified characters
		/// in the specified string.</returns>
		internal static IList<int> IndexOfEach(this string str, char[] eachOf, int startIndex)
		{
			return IndexOfEach(str, eachOf, startIndex, str.Length - startIndex);
		}

		/// <summary>
		/// Returns the indices at which any of the specified characters are found in the string.
		/// </summary>
		/// <param name="str">The string to search.</param>
		/// <param name="eachOf">The characters to look for.</param>
		/// <param name="startIndex">The zero-base index of the first character of the string
		/// to include in the search.</param>
		/// <param name="count">The maximum number of characters to search.</param>
		/// <returns>The indices of each of the occurences of the specified characters
		/// in the specified string.</returns>
		internal static IList<int> IndexOfEach(this string str, char[] eachOf, int startIndex, int count)
		{
			int maxIndex = startIndex + count;
			List<int> indices = new List<int>();

			int index = startIndex;
			while (index >= 0 && index < maxIndex)
			{
				index = str.IndexOfAny(eachOf, startIndex, count);
				indices.Add(index);
			}
			// Note that the last index to be added is invalid, so it must be removed.
			indices.RemoveAt(indices.Count - 1);

			return indices;
		}

		/// <summary>
		/// Returns the string from the specified index to the first occurence of any
		/// of the specified delimiters.
		/// </summary>
		/// <param name="str">The string to get the substring of.</param>
		/// <param name="startIndex">The start index in the string.</param>
		/// <param name="delimiters">The deminiters to look for.</param>
		/// <returns>The substring from <paramref name="startIndex"/> up to but excluding the delimiter, if found;
		/// otherwise, the substring from <paramref name="startIndex"/> to the end of the string.</returns>
		internal static string Substring(this string str, int startIndex, char[] delimiters)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(str != null);
			Contract.Requires<ArgumentNullException>(delimiters != null);
			Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0 && startIndex < str.Length);
			Contract.Ensures(Contract.Result<string>() != null);
			#endregion

			int endIndex = str.IndexOfAny(delimiters, startIndex);

			string substring;
			if (endIndex > 0)
				substring = str.Substring(startIndex, endIndex - startIndex);
			else
				substring = str.Substring(startIndex);

			return substring;
		}
	}
}
