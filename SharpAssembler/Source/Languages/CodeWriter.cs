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
	/// Use the <see cref="CodeWriter.WriteLine"/> methods to indicate the end of a line,
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
		/// Initializes a new instance of the <see cref="CodeWriter3"/> class
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
		/// Initializes a new instance of the <see cref="CodeWriter3"/> class
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
		/// Initializes a new instance of the <see cref="CodeWriter3"/> class
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
		/// <summary>
		/// Writes any data to be written to the underlying stream and clears all buffers.
		/// </summary>
		public void Flush()
		{
			this.underlyingWriter.Flush();
		}
		#endregion

		/// <inhertidoc />
		public override void Write(char value)
		{
			currentLineLength++;
			this.underlyingWriter.Write(value);
		}

		/// <inhertidoc />
		public override void Write(char[] buffer, int index, int count)
		{
			currentLineLength += count;
			this.underlyingWriter.Write(buffer, index, count);
		}

		/// <inhertidoc />
		public override void WriteLine()
		{
			this.underlyingWriter.WriteLine();
			currentLineLength = 0;
		}

		#region Comments
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
			//((IDisposable)this).Dispose();
		}
		#endregion
	}
}
