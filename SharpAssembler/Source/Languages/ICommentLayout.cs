using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages
{
	/// <summary>
	/// Represents a layouter for comments.
	/// </summary>
	[ContractClass(typeof(Contracts.ICommentLayoutContract))]
	public interface ICommentLayout
	{
		/// <summary>
		/// Returns the string representation of a single comment line.
		/// </summary>
		/// <param name="comment">The whole comment being written.</param>
		/// <param name="index">The zero-based index of the next character to be consumed.</param>
		/// <param name="count">The number of consumed characters.</param>
		/// <param name="maxLength">The preferred maximum length of the comment line, in characters;
		/// or 0 to specify no limit.</param>
		/// <returns>The formatted comment line.</returns>
		/// <remarks>
		/// For a source code file layout with a maximum line length of X and starting the comment at
		/// character Y on the current line, specify X - Y as <paramref name="maxLength"/>.
		/// </remarks>
		string GetLine(string comment, int index, out int count, int maxLength);
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(ICommentLayout))]
		abstract class ICommentLayoutContract : ICommentLayout
		{
			public string GetLine(string comment, int index, out int count, int maxLength)
			{
				Contract.Requires<ArgumentNullException>(comment != null);
				Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index <= comment.Length);
				Contract.Requires<ArgumentOutOfRangeException>(maxLength >= 0);
				Contract.Ensures(Contract.ValueAtReturn(out count) >= 0);
				return default(string);
			}
		}
	}
	#endregion
}
