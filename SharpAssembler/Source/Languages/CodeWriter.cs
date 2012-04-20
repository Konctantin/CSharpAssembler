using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages
{
	/// <summary>
	/// Writes code.
	/// </summary>
	/// <remarks>
	/// Use the <see cref="CodeWriter.WriteLine()"/> methods to indicate the end of a line,
	/// instead of writing the end-of-line character sequence manually.
	/// </remarks>
	public class CodeWriter : TextWriter
	{
		/// <summary>
		/// The underlying text writer.
		/// </summary>
		private readonly TextWriter underlyingWriter;
		/// <summary>
		/// Whether this writer owns the underlying <see cref="TextWriter"/>.
		/// </summary>
		private readonly bool ownsWriter;
		/// <summary>
		/// The length of the current line, in characters.
		/// </summary>
		private int currentLineLength;

		/// <inheritdoc />
		public override Encoding Encoding
		{
			get { return this.underlyingWriter.Encoding; }
		}

		private int preferredLineLength = 80;
		/// <summary>
		/// Gets or sets the preferred length of a line.
		/// </summary>
		/// <value>The preferred length of each line, in characters;
		/// or 0 to specify no limit.</value>
		public int PreferredLineLength
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<int>() >= 0);
				#endregion
				return this.preferredLineLength;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(value >= 0);
				#endregion
				this.preferredLineLength = value;
			}
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeWriter"/> class
		/// with the specified underlying <see cref="TextWriter"/>.
		/// </summary>
		/// <param name="writer">The underlying <see cref="TextWriter"/>.</param>
		public CodeWriter(TextWriter writer)
			: this(writer, false)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeWriter"/> class
		/// writing to the specified <see cref="Stream"/> using the specified encoding.
		/// </summary>
		/// <param name="stream">The stream to which this code writer writes.</param>
		/// <param name="encoding">The character encoding to use.</param>
		public CodeWriter(Stream stream, Encoding encoding)
			: this(new StreamWriter(stream, encoding), true)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(stream != null);
			Contract.Requires<ArgumentException>(stream.CanWrite);
			Contract.Requires<ArgumentNullException>(encoding != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeWriter"/> class
		/// writing to the specified <see cref="Stream"/> using UTF-8 encoding.
		/// </summary>
		/// <param name="stream">The stream to which this code writer writes.</param>
		/// <remarks>
		/// This constructor overload uses UTF-8 encoding.
		/// </remarks>
		public CodeWriter(Stream stream)
			: this(stream, Encoding.UTF8)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(stream != null);
			Contract.Requires<ArgumentException>(stream.CanWrite);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeWriter"/> class
		/// with the specified underlying <see cref="TextWriter"/>.
		/// </summary>
		/// <param name="writer">The underlying <see cref="TextWriter"/>.</param>
		/// <param name="ownsWriter">Whether this <see cref="CodeWriter3"/> owns <paramref name="writer"/>.</param>
		/// <remarks>
		/// When <paramref name="writer"/> is owned by this object, it is disposed when this object is disposed.
		/// </remarks>
		private CodeWriter(TextWriter writer, bool ownsWriter)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion
			this.underlyingWriter = writer;
			this.ownsWriter = ownsWriter;
		}
		#endregion

		#region Miscelaneous operations
		/// <inheritdoc />
		public override void Flush()
		{
			this.underlyingWriter.Flush();
		}
		#endregion

		#region General Writing
		/// <inheritdoc />
		public override void Write(char value)
		{
			currentLineLength++;
			this.underlyingWriter.Write(value);
		}

		/// <inheritdoc />
		public override void Write(char[] buffer)
		{
			currentLineLength += buffer.Length;
			this.underlyingWriter.Write(buffer);
		}

		/// <inheritdoc />
		public override void Write(char[] buffer, int index, int count)
		{
			currentLineLength += count;
			this.underlyingWriter.Write(buffer, index, count);
		}

		/// <inheritdoc />
		public override void Write(string value)
		{
			currentLineLength += value.Length;
			this.underlyingWriter.Write(value);
		}

		/// <inheritdoc />
		public override void WriteLine()
		{
			this.underlyingWriter.WriteLine();
			currentLineLength = 0;
		}

		/// <inheritdoc />
		public override void WriteLine(string value)
		{
			this.underlyingWriter.WriteLine(value);
			currentLineLength = 0;
		}
		#endregion

		#region Indentations
		private string indentString = "    ";
		/// <summary>
		/// Gets or sets the string to use for indents.
		/// </summary>
		/// <value>An indentation string.</value>
		public virtual string IndentString
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return this.indentString;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.indentString = value;
			}
		}

		private bool indent = true;
		/// <summary>
		/// Gets or sets whether to use indentation.
		/// </summary>
		/// <value><see langword="true"/> to use indentation;
		/// otherwise, <see langword="false"/>.</value>
		public bool Indent
		{
			get { return this.indent; }
			set { this.indent = value; }
		}

		private int indentationLevel = 0;

		/// <summary>
		/// Removes an indentation level.
		/// </summary>
		public void PopIndent()
		{
			PopIndent(1);
		}

		/// <summary>
		/// Removes indentation levels.
		/// </summary>
		/// <param name="levels">The number of indentation levels to pop.</param>
		public void PopIndent(int levels)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(levels >= 0);
			#endregion
			indentationLevel -= levels;
			if (indentationLevel < 0)
				indentationLevel = 0;
		}

		/// <summary>
		/// Adds an indentation level.
		/// </summary>
		public void PushIndent()
		{
			PushIndent(1);
		}

		/// <summary>
		/// Adds indentation levels.
		/// </summary>
		/// <param name="levels">The number of indentation levels to push.</param>
		public void PushIndent(int levels)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(levels >= 0);
			#endregion
			indentationLevel += levels;
		}

		/// <summary>
		/// Writes the indent.
		/// </summary>
		/// <returns>The number of written characters.</returns>
		/// <remarks>
		/// When <see cref="Indent"/> is <see langword="false"/>,
		/// this method does nothing.
		/// </remarks>
		public void WriteIndent()
		{
			WriteIndent(0);
		}

		/// <summary>
		/// Writes the indent.
		/// </summary>
		/// <param name="adjustment">The indent level adjustment, which is added to the current
		/// indent level, with a minimum of zero.</param>
		/// <remarks>
		/// When <see cref="Indent"/> is <see langword="false"/>,
		/// this method does nothing.
		/// </remarks>
		public void WriteIndent(int adjustment)
		{
			// Only write an indentation when it is enabled.
			if (!this.indent)
				return;

			int level = this.indentationLevel + adjustment;
			while (level > 0)
			{
				this.Write(indentString);
				level--;
			}
		}
		#endregion

		#region Comments
		/// <summary>
		/// Writes the specified comment on the end of the current line and any
		/// subsequent lines.
		/// </summary>
		/// <param name="comment">The comment to write.</param>
		/// <remarks>
		/// Call <see cref="WriteLine()"/> after calling this method to write the first
		/// line of the multi-line comment on that line. On each subsequent call to
		/// <see cref="WriteLine()"/> the next line of the multi-line comment is written
		/// at the end of the line.
		/// </remarks>
		public void StartWritingMultiLineComment(string comment)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes any remaining comment lines on the end of the current line
		/// and any added subsequent lines.
		/// </summary>
		/// <remarks>
		/// Call <see cref="WriteLine()"/> after calling this method to write the next line
		/// of the multi-line comment and any other lines required to end the comment.
		/// </remarks>
		public void EndWritingMultiLineComment()
		{
			throw new NotImplementedException();
		}


#if false
		/// <summary>
		/// Writes the specified comment on the current line, and if necessary,
		/// on any subsequent lines. Ends the current line.
		/// </summary>
		/// <param name="comment">The comment to write; or <see langword="null"/> to write no comment.</param>
		public void WriteCommentLines(string comment)
		{

		}

		/// <summary>
		/// Writes the specified comment on the current line, and if necessary,
		/// on any subsequent lines. Ends the current line.
		/// </summary>
		/// <param name="comment">The comment to write; or <see langword="null"/> to write no comment.</param>
		/// <param name="lines">The number of comment lines to write; or -1 to write them all.</param>
		/// <remarks>
		/// The number of lines used by a comment depends on the length of the comment,
		/// the text already present on each line and the maximum line length.
		/// </remarks>
		public void WriteCommentLines(string comment, int lines)
		{

		}

		/// <summary>
		/// Writes the comment associated with the specified constructable.
		/// </summary>
		/// <param name="constructable">The <see cref="Constructable"/> to write the comment of.</param>
		/// <param name="linelength">The number of written characters on this line.</param>
		protected internal void WriteCommentOf(Constructable constructable, int linelength)
		{
		#region Contract
			Contract.Requires<ArgumentNullException>(constructable != null);
			Contract.Requires<ArgumentOutOfRangeException>(linelength > 0);
		#endregion
			// We don't write comments associated with comments.
			if (constructable is Comment)
				return;

			if (constructable.Comment != null)
				WriteCommentString(constructable.Comment.Text, linelength, 0, -1);
			else
				Writer.WriteLine();
		}

		/// <summary>
		/// Writes a comment.
		/// </summary>
		/// <param name="comment">The comment to write.</param>
		/// <param name="column">The column right after the written constructable;
		/// or -1 to specify that the comment is not written right after a constructable.</param>
		/// <param name="startindex">The index of the first character to write, starting from 0.</param>
		/// <param name="lineCount">The number of lines to write; or -1 to write them all.</param>
		/// <returns>
		/// The index of the last character written; or -1 when all have been written. Use this
		/// as the <paramref name="startindex"/> for the next call.
		/// </returns>
		protected internal int WriteCommentString(string comment, int column, int startindex, int lineCount)
		{
		#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(column >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(lineCount >= 0);
			Contract.Ensures(Contract.Result<int>() >= -1);
			if (comment == null || startindex < 0) return -1;
		#endregion

			int linelength = (column < 0 ? 0 : column);		// The length of the current output line.
			int written = 0;								// Number of comment characters written.
			int commentstart = startindex;					// Start position in the comment.
			int commentbreak;								// End of a line in the comment.
			string[] words;									// Array of split words.

			// While there are lines to write...
			while (commentstart < comment.Length && lineCount != 0)
			{
				// 1. Write the comment start.
				if (linelength == 0)
				{
					// This starts on a new, indented line.
					linelength += WriteIndent(indentationlevel);
				}
				if (column >= 0)
				{
					// Pad to 'commentAlignment'.
					int count = commentAlignment - linelength;
					for (int i = 0; i < count; i++)
					{
						Writer.Write(" ");
						linelength++;
					}
				}
				Writer.Write("; ");
				linelength += 2;

				// 2. Split the comment (from the current position to the start of the next line) into words.
				commentbreak = comment.IndexOfAny(new char[] { '\r', '\n' }, startindex);
				if (commentbreak > 0)
					words = comment.Substring(commentstart, commentbreak - commentstart).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				else
					words = comment.Substring(commentstart).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				// 3. Write the first word.
				Writer.Write(words[0]);
				written += words[0].Length;

				// 4. Write words until the output line may be no longer; or when there are no more words in the current comment line.
				int j = 1;
				while (j < words.Length && linelength + written + 1 + words[j].Length <= MaximumLineLength)
				{
					Writer.Write(" ");
					Writer.Write(words[j]);
					written += 1 + words[j].Length;
					j++;
				}

				// 5. Determine where to start the next line.
				Writer.WriteLine();
				commentstart += written + 1;		// Written characters + space or newline.
				written = 0;
				linelength = 0;
				lineCount--;
			}

			// All lines have been written.
			if (commentstart >= comment.Length)
				return -1;
			return commentstart;
		}
#endif
		#endregion

		#region Disposing
		/// <summary>
		/// Closes this writer.
		/// </summary>
		/// <remarks>
		/// The underlying <see cref="TextWriter"/> is not closed when it was not created by this class.
		/// </remarks>
		public override void Close()
		{
			this.Flush();
			base.Close();
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.indentString != null);
			Contract.Invariant(this.preferredLineLength >= 0);
		}
		#endregion
	}
}
