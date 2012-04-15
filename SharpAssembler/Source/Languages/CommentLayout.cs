using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages
{
	/// <summary>
	/// Describes the layout of a comment.
	/// </summary>
	public class CommentLayout : ICommentLayout
	{
		private string prefix = "# ";
		/// <summary>
		/// Gets or sets the prefix to use on all general comment lines.
		/// </summary>
		/// <value>The prefix used on normal comment lines.
		/// The default is "<c># </c>", including the trailing space.</value>
		public string Prefix
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return this.prefix;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.prefix = value;
			}
		}

		/// <inheritdoc />
		public string GetLine(string comment, int index, out int count, int maxLength)
		{
			// CONTRACT: ICommentLayout

			StringBuilder sb = new StringBuilder();
			
			// While there are lines to write...
			while (index < comment.Length)
			{
				sb.Append(this.prefix);

				// Get the rest of the current line.
				string line = comment.Substring(index, new char[] { '\r', '\n' });

				// Split the line into (partial) words and substring containing only whitespace.
				var words = line.SplitAndKeep(new char[] { ' ', '\t' }, new char[] { ' ', '-', '.' });

				// Append the first word.
				sb.Append(words[0]);

				// Append words until the output may not be any longer, or when there are no more words.
				int i = 1;
				while (i < words.Count &&
					(sb.Length + words[i].Length < maxLength || String.IsNullOrWhiteSpace(words[i])))
				{
					sb.Append(words[i]);
					i++;
				}
			}

			count = sb.Length;

			return sb.ToString();
		}

		
	}
}
