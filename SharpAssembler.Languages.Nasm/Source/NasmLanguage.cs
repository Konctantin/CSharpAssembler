#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011-2012 Daniël Pelsmaeker
 * 
 * This file is part of SharpAssembler.
 * 
 * SharpAssembler is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpAssembler is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SharpAssembler.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion
using System;
using System.Diagnostics.Contracts;
using System.IO;
using SharpAssembler.Instructions;
using SharpAssembler.Symbols;
using System.Linq.Expressions;

namespace SharpAssembler.Languages.Nasm
{
	/// <summary>
	/// Writes NASM assembler.
	/// </summary>
	public class NasmLanguage : Language, IIndented
	{
		private CodeWriter writer;
		/// <summary>
		/// The underlying <see cref="CodeWriter"/> that is used.
		/// </summary>
		protected internal CodeWriter Writer
		{
			get { return this.writer; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="NasmLanguage"/> class.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> used to write the assembly code to.</param>
		public NasmLanguage(TextWriter writer)
			: base(writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			this.writer = new CodeWriter(writer);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name of the language.
		/// </summary>
		/// <value>The name of the language.</value>
		public override string Name
		{
			get { return "NASM"; }
		}

		private int commentAlignment = 40;
		/// <summary>
		/// Gets or sets the column to which all comments about <see cref="Constructable"/> objects
		/// are aligned.
		/// </summary>
		/// <value>A column number; or 0 to put the comments directly after the corresponding instruction.
		/// The default is 40.</value>
		public int CommentAlignment
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<int>() >= 0);
				#endregion
				return commentAlignment;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(value >= 0);
				#endregion
				commentAlignment = value;
			}
		}
		#endregion

		#region Methods
		#region ObjectFile
		/// <inheritdoc />
		protected override void VisitObjectFile(ObjectFile objectFile)
		{
			switch (objectFile.Architecture.AddressSize)
			{
				case DataSize.Bit16: Writer.WriteLine("[BITS 16]"); break;
				case DataSize.Bit32: Writer.WriteLine("[BITS 32]"); break;
				case DataSize.Bit64: Writer.WriteLine("[BITS 64]"); break;
				default:
					throw new LanguageException("The object file's architecture address size is not supported " +
						"by this language.");
			}

			// Visit the sections.
			base.VisitObjectFile(objectFile);
		}
		#endregion

		#region Section
		/// <inheritdoc />
		protected override void VisitSection(Section section)
		{
			if (InsertNewlines)
				Writer.WriteLine();

			Writer.PopIndent();
			WriteSectionStart(section);
			Writer.PushIndent();

			// Visit the constructables.
			base.VisitSection(section);

			if (InsertNewlines)
				Writer.WriteLine();
		}

		/// <summary>
		/// Writes the object file format specific section start.
		/// </summary>
		/// <param name="section">The section.</param>
		protected virtual void WriteSectionStart(Section section)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(section != null);
			#endregion
			Writer.WriteLine("SECTION {0}", section.Identifier);
		}
		#endregion

		#region Constructable
		/// <inheritdoc />
		protected override void VisitConstructable(Constructable constructable)
		{
			throw new LanguageException("The constructable is not known or supported.");
		}

		/// <inheritdoc />
		protected override void VisitInstruction(Instruction instruction)
		{
			throw new NotImplementedException();
			// TODO: Implement a platform-specific translation
#if false
			int linelength = 0;
			linelength = WriteIndent(indentationlevel);

			// Write prefixes.
			ILockInstruction lockInstr = instruction as ILockInstruction;
			if (lockInstr != null && lockInstr.Lock)
			{
				Writer.Write("lock ");
				linelength += 5;
			}
			// TODO: REP, REPE/REPZ, REPNE/REPNZ prefixes
			// TODO: Segment override

			// Write the mnemonic.
			Writer.Write(instruction.Mnemonic);
			linelength += instruction.Mnemonic.Length;

			// Special instruction handling
			switch (instruction.Mnemonic)
			{
				case "enter":
					// Write the operands.
					if (instruction.Operands.Count >= 1)
					{
						Writer.Write(" ");
						linelength++;
						linelength += WriteOperand(instruction.Operands[0], false);
						for (int i = 1; i < instruction.Operands.Count; i++)
						{
							Writer.Write(", ");
							linelength += 2;
							linelength += WriteOperand(instruction.Operands[i], false);
						}
					}
					break;
				default:
					// Write the operands.
					if (instruction.Operands.Count >= 1)
					{
						Writer.Write(" ");
						linelength++;
						linelength += WriteOperand(instruction.Operands[0]);
						for (int i = 1; i < instruction.Operands.Count; i++)
						{
							Writer.Write(", ");
							linelength += 2;
							linelength += WriteOperand(instruction.Operands[i]);
						}
					}
					break;
			}

			WriteCommentOf(instruction, linelength);
#endif
		}

		/// <inheritdoc />
		protected override void VisitLabel(Label label)
		{
			int commentstart = 0;
			string commentstring = null;
			if (label.Comment != null)
				commentstring = label.Comment.Text;

			Writer.PopIndent();

			string identifier;
			SymbolType type;
			if (label.DefinedSymbol != null)
			{
				identifier = label.DefinedSymbol.Identifier;
				if (identifier == null)
					throw new NotSupportedException("Symbols without identifiers are not supported.");
				type = label.DefinedSymbol.SymbolType;

				if (label.DefinedSymbol.SymbolType == SymbolType.Private)
				{
					// Single line, with comments.

					Writer.Write("{0}:", identifier);

					WriteCommentOf(label, linelength);
				}
				else
				{
					// Two lines, with comments.

					switch (label.DefinedSymbol.SymbolType)
					{
						case SymbolType.Public:
							Writer.Write("global {0}", identifier);
							break;
						case SymbolType.Weak:
							Writer.Write("weak {0}", identifier);
							break;
						case SymbolType.Private:
						default:
							break;
					}

					// Write the first part of the comment.
					if (commentstring != null)
						commentstart = WriteCommentString(commentstring, linelength, 0, 1);
					else
						Writer.WriteLine();

					Writer.Write("{0}:", identifier);

					// Write the rest of the comment.
					if (commentstring != null)
						WriteCommentString(commentstring, linelength, commentstart, -1);
					else
						Writer.WriteLine();
				}
			}
			else
			{
				WriteCommentString("label <unkown>", -1, 0, -1);

				WriteCommentOf(label, linelength);
			}

			Writer.PushIndent();
		}

		/// <inheritdoc />
		protected override void VisitExtern(Extern reference)
		{
			Writer.PopIndent();

			if (reference.ExternSymbol != null)
			{
				string identifier = reference.ExternSymbol.Identifier;
				if (identifier == null)
					throw new NotSupportedException("Symbols without identifiers are not supported.");
				Writer.Write("extern {0}", reference.ExternSymbol.Identifier);
				linelength = reference.ExternSymbol.Identifier.Length + 7;
			}
			else
			{
				linelength = WriteCommentString("extern <unkown>", -1, 0, -1);
			}

			WriteCommentOf(reference, linelength);

			Writer.PushIndent();
		}

		/// <inheritdoc />
		protected override void VisitAlign(Align align)
		{
			Writer.WriteIndent();

			string s;
			if (align.PaddingByte != 0x90)
				s = String.Format("align {0}, db {1}", align.Boundary, align.PaddingByte);
			else
				s = String.Format("align {0}", align.Boundary);
			Writer.Write(s);

			WriteCommentOf(align, linelength);
		}

		/// <inheritdoc />
		protected override void VisitDeclareData(DeclareData declaration)
		{
			Writer.WriteIndent();

			switch (declaration.Size)
			{
				case DataSize.Bit8: Writer.Write("db "); break;
				case DataSize.Bit16: Writer.Write("dw "); break;
				case DataSize.Bit32: Writer.Write("dd "); break;
				case DataSize.Bit64: Writer.Write("dq "); break;
				case DataSize.Bit128:
				case DataSize.Bit256:
				default:
					throw new LanguageException("Declared data's size is not supported.");
			}

			WriteExpression(declaration.Expression);

			WriteCommentOf(declaration, linelength);
		}

		/// <inheritdoc />
		protected override void VisitComment(Comment comment)
		{
			WriteCommentString(comment.Text, -1, 0, -1);
		}
		#endregion

		#region Comment
#if false
		/// <inheritdoc />
		void INasmLanguageControl.WriteCommentOf(Constructable constructable, int linelength)
		{
			WriteCommentOf(constructable, linelength);
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

		/// <inheritdoc />
		int INasmLanguageControl.WriteCommentString(string comment, int column, int startindex, int lineCount)
		{
			return WriteCommentString(comment, column, startindex, lineCount);
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

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns>The number of written characters.</returns>
		protected internal int WriteExpression(Expression<Func<Context, ReferenceOffset>> expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion
			throw new NotImplementedException();
		}

		// TODO: Implement operands
#if false
		#region Operands
		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(Operand operand)
		{ return WriteOperand(operand, true); }

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(Operand operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion

			int length = 0;
			length += WriteOperand(operand as EffectiveAddress, writeSize);
			length += WriteOperand(operand as Immediate, writeSize);
			length += WriteOperand(operand as RegisterOperand, writeSize);
			length += WriteOperand(operand as RelativeOffset, writeSize);
			length += WriteOperand(operand as FarPointer, writeSize);

			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(RelativeOffset operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
			{
				switch (operand.RequestedSize)
				{
					case DataSize.Bit8:
						Writer.Write("byte ");
						length += 5;
						break;
					case DataSize.Bit16:
						Writer.Write("word ");
						length += 5;
						break;
					case DataSize.Bit32:
						Writer.Write("dword ");
						length += 6;
						break;
					case DataSize.Bit64:
						Writer.Write("qword ");
						length += 6;
						break;
					case DataSize.None:
						break;
					default:
						throw new LanguageException(ExceptionStrings.UnknownRequestedSize);
				}
			}

			length += WriteExpression(operand.Expression);
			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(RegisterOperand operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
			{
				switch (operand.RequestedSize)
				{
					case DataSize.Bit8:
						Writer.Write("byte ");
						length += 5;
						break;
					case DataSize.Bit16:
						Writer.Write("word ");
						length += 5;
						break;
					case DataSize.Bit32:
						Writer.Write("dword ");
						length += 6;
						break;
					case DataSize.Bit64:
						Writer.Write("qword ");
						length += 6;
						break;
					case DataSize.None:
						break;
					default:
						throw new LanguageException(ExceptionStrings.UnknownRequestedSize);
				}
			}

			string str = Enum.GetName(typeof(Register), operand.Register);
			length += str.Length;
			Writer.Write(str);
			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(Immediate operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
			{
				switch (operand.RequestedSize)
				{
					case DataSize.Bit8:
						Writer.Write("byte ");
						length += 5;
						break;
					case DataSize.Bit16:
						Writer.Write("word ");
						length += 5;
						break;
					case DataSize.Bit32:
						Writer.Write("dword ");
						length += 6;
						break;
					case DataSize.Bit64:
						Writer.Write("qword ");
						length += 6;
						break;
					case DataSize.None:
						break;
					default:
						throw new LanguageException(ExceptionStrings.UnknownRequestedSize);
				}
			}

			length += WriteExpression(operand.Expression);
			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(EffectiveAddress operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			Writer.Write("[");
			length++;
			// TODO: Possible segment override here.

			// Base register.
			string str;
			if (operand.BaseRegister != Register.None)
			{
				str = Enum.GetName(typeof(Register), operand.BaseRegister);
				length += str.Length;
				Writer.Write(str);
			}
			// Index register.
			if (operand.IndexRegister != Register.None)
			{
				if (operand.BaseRegister != Register.None)
				{
					Writer.Write("+");
					length++;
				}
				str = Enum.GetName(typeof(Register), operand.IndexRegister);
				length += str.Length;
				Writer.Write(str);
				// Scale
				if (operand.Scale > 1)
				{
					str = String.Format("*{0}", operand.Scale);
					length += str.Length;
					Writer.Write(str);
				}
			}
			// Displacement
			if (operand.Displacement != null)
			{
				if (operand.BaseRegister != Register.None || operand.IndexRegister != Register.None)
				{
					Writer.Write("+");
					length++;
				}
				length += WriteExpression(operand.Displacement);
			}

			Writer.Write("]");
			length++;

			return length;
		}

		/// <summary>
		/// Writes a far pointer operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(FarPointer operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
			{
				switch (operand.RequestedSize)
				{
					case DataSize.Bit8:
						Writer.Write("byte ");
						length += 5;
						break;
					case DataSize.Bit16:
						Writer.Write("word ");
						length += 5;
						break;
					case DataSize.Bit32:
						Writer.Write("dword ");
						length += 6;
						break;
					case DataSize.Bit64:
						Writer.Write("qword ");
						length += 6;
						break;
					case DataSize.None:
						break;
					default:
						throw new LanguageException(ExceptionStrings.UnknownRequestedSize);
				}
			}

			length += WriteExpression(operand.Selector);
			Writer.Write(":");
			length += 1;
			length += WriteExpression(operand.Offset);

			return length;
		}
		#endregion
#endif
#if false
		#region Expressions
		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(Expression expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion

			int length = 0;
			length += WriteExpression(expression as BinaryExpression);
			length += WriteExpression(expression as Constant);
			length += WriteExpression(expression as CurrentPosition);
			length += WriteExpression(expression as CurrentSection);
			length += WriteExpression(expression as FunctionBase);
			length += WriteExpression(expression as Reference);
			length += WriteExpression(expression as UnaryExpression);

			return length;
		}

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(Reference expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion

			// TODO: Test identifier validity.
			Writer.Write(expression.TargetIdentifier);

			return expression.TargetIdentifier.Length;
		}

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(FunctionBase expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion

			throw new LanguageException(ExceptionStrings.FunctionsNotSupported);
		}

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(CurrentSection expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion

			Writer.Write("$$");

			return 2;
		}

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(CurrentPosition expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion

			Writer.Write("$");

			return 1;
		}

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(Constant expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion

			string str = String.Format("0x{0:X}", expression.Value);
			Writer.Write(str);

			return str.Length;
		}

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(UnaryExpression expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion
			int length = 0;

			switch (expression.Operation)
			{
				case UnaryOperation.Negate:
					Writer.Write("-");
					length++;
					length += WriteExpression(expression.Expression);
					break;
				case UnaryOperation.Positivate:
					length += WriteExpression(expression.Expression);
					break;
				case UnaryOperation.Increment:
					length += WriteExpression(expression.Expression);
					Writer.Write("+1");
					length += 2;
					break;
				case UnaryOperation.Decrement:
					length += WriteExpression(expression.Expression);
					Writer.Write("-1");
					length += 2;
					break;
				case UnaryOperation.Complement:
					Writer.Write("~");
					length++;
					length += WriteExpression(expression.Expression);
					break;
				case UnaryOperation.Not:
					Writer.Write("!");
					length++;
					length += WriteExpression(expression.Expression);
					break;
				case UnaryOperation.None:
				default:
					throw new LanguageException(ExceptionStrings.UnknownUnaryOperation);
			}

			return length;
		}

		/// <summary>
		/// Writes the specified expression.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteExpression(BinaryExpression expression)
		{
			#region Contract
			if (expression == null) return 0;
			#endregion
			int length = 0;

			string op;
			switch (expression.Operation)
			{
				case BinaryOperation.BitwiseOr:
					op = "|";
					break;
				case BinaryOperation.BitwiseXOr:
					op = "^";
					break;
				case BinaryOperation.BitwiseAnd:
					op = "&";
					break;
				case BinaryOperation.LeftShift:
					op = "<<";
					break;
				case BinaryOperation.RightShift:
					op = ">>";
					break;
				case BinaryOperation.Add:
					op = "+";
					break;
				case BinaryOperation.Subtract:
					op = "-";
					break;
				case BinaryOperation.Multiply:
					op = "*";
					break;
				case BinaryOperation.DivideUnsigned:
					op = "/";
					break;
				case BinaryOperation.DivideSigned:
					op = "//";
					break;
				case BinaryOperation.ModuloUnsigned:
					op = "%";
					break;
				case BinaryOperation.ModuloSigned:
					op = "%%";
					break;
				case BinaryOperation.None:
				default:
					throw new LanguageException(ExceptionStrings.UnknownBinaryOperation);
			}

			// A child expression gets parentheses when:
			// - it is less strong than this expression (precedence); or
			// - it is equally strong, but this expression is left associative.

			BinaryExpression leftbinexpr = expression.LeftHandExpression as BinaryExpression;
			bool leftparentheses =
				(leftbinexpr != null &&
				(((int)leftbinexpr.Operation & 0x0F00) < ((int)expression.Operation & 0x0F00) ||
				(((int)leftbinexpr.Operation & 0x0F00) == ((int)expression.Operation & 0x0F00) &&
				((int)expression.Operation & 0x00F0) == 0x0010)));
			BinaryExpression rightbinexpr = expression.RightHandExpression as BinaryExpression;
			bool rightparentheses =
				(rightbinexpr != null &&
				(((int)rightbinexpr.Operation & 0x0F00) < ((int)expression.Operation & 0x0F00) ||
				(((int)rightbinexpr.Operation & 0x0F00) == ((int)expression.Operation & 0x0F00) &&
				((int)expression.Operation & 0x00F0) == 0x0010)));

			if (leftparentheses)
			{
				Writer.Write("(");
				length++;
			}
			length += WriteExpression(expression.LeftHandExpression);
			if (leftparentheses)
			{
				Writer.Write(")");
				length++;
			}
			Writer.Write(op);
			length += op.Length;
			if (rightparentheses)
			{
				Writer.Write("(");
				length++;
			}
			length += WriteExpression(expression.RightHandExpression);
			if (rightparentheses)
			{
				Writer.Write(")");
				length++;
			}

			return length;
		}
		#endregion
#endif
		
		#endregion

		#region Indentation
		/// <inheritdoc />
		public string IndentString
		{
			get { return Writer.IndentString; }
			set { Writer.IndentString = value; }
		}

		/// <inheritdoc />
		public bool Indent
		{
			get { return Writer.Indent; }
			set { Writer.Indent = value; }
		}
		#endregion
	}
}
