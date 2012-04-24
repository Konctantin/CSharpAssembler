using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Writes code based on an opcode specification.
	/// </summary>
	[ContractClass(typeof(Contracts.SpecWriterContract))]
	public abstract partial class SpecWriter
	{
		/// <summary>
		/// Represents a tab.
		/// </summary>
		protected const string T = "\t";

		/// <summary>
		/// Gets the base namespace.
		/// </summary>
		protected virtual string BaseNamespace
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return String.Format("SharpAssembler.Architectures.{0}", this.SubNamespace);
			}
		}

		/// <summary>
		/// Gets the opcode namespace.
		/// </summary>
		protected virtual string OpcodeNamespace
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return this.BaseNamespace + ".Opcodes";
			}
		}

		/// <summary>
		/// Gets the opcode test namespace.
		/// </summary>
		protected virtual string OpcodeTestNamespace
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return this.BaseNamespace + ".Tests.Opcodes";
			}
		}

		/// <summary>
		/// Gets or sets the sub namespace to use.
		/// </summary>
		/// <value>The sub namespace to use.</value>
		/// <remarks>
		/// The sub namespace is the part in brackets in the following example namespace identifier:
		/// <example>
		/// SharpAssembler.Architectures.&lt;subnamespace&gt;.Opcodes
		/// </example>
		/// </remarks>
		protected abstract string SubNamespace
		{ get; }

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SpecWriter"/> class.
		/// </summary>
		public SpecWriter()
		{
		}
		#endregion

		/// <summary>
		/// Writes the specification in code to the specified code file,
		/// and tests for it to the specified test file.
		/// </summary>
		/// <param name="spec">The specification to write.</param>
		/// <param name="codeFile">The full path to the code file to write to;
		/// or <see langword="null"/> to write no code.</param>
		/// <param name="testFile">The full path to the code file to write to;
		/// or <see langword="null"/> to write no tests.</param>
		/// <remarks>
		/// Any data that is already in the files is overwritten and discarded.
		/// </remarks>
		public void Write(OpcodeSpec spec, string codeFile, string testFile)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			#endregion

			FileStream codeStream = null;
			FileStream testStream = null;
			try
			{
				if (codeFile != null)
					codeStream = File.Create(codeFile);
				if (testFile != null)
					testStream = File.Create(testFile);

				Write(spec, codeStream, testStream);
			}
			finally
			{
				if (codeStream != null)
					codeStream.Close();
				if (testStream != null)
					testStream.Close();
			}
		}

		/// <summary>
		/// Writes the specification in code to the specified code stream,
		/// and tests for it to the specified test stream.
		/// </summary>
		/// <param name="spec">The specification to write.</param>
		/// <param name="codeStream">The code <see cref="Stream"/> to write to;
		/// or <see langword="null"/> to write no code.</param>
		/// <param name="testStream">The test <see cref="Stream"/> to write to;
		/// or <see langword="null"/> to write no tests.</param>
		public void Write(OpcodeSpec spec, Stream codeStream, Stream testStream)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			#endregion

			StreamWriter codeWriter = null;
			StreamWriter testWriter = null;
			try
			{
				if (codeStream != null)
					codeWriter = new StreamWriter(codeStream, Encoding.UTF8);
				if (testStream != null)
					testWriter = new StreamWriter(testStream, Encoding.UTF8);

				Write(spec, codeWriter, testWriter);
			}
			finally
			{
				// FIXME: Don't close the streams when the writer is closed. Use a wrapper.
				if (codeWriter != null)
					codeWriter.Close();
				if (testWriter != null)
					testWriter.Close();
			}
		}

		/// <summary>
		/// Writes the specification in code to the specified code writer,
		/// and tests for it to the specified test writer.
		/// </summary>
		/// <param name="spec">The specification to write.</param>
		/// <param name="codeWriter">The code writer to use;
		/// or <see langword="null"/> to write no code.</param>
		/// <param name="testWriter">The test writer to use;
		/// or <see langword="null"/> to write no tests.</param>
		public void Write(OpcodeSpec spec, TextWriter codeWriter, TextWriter testWriter)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(spec != null);
			#endregion

			if (codeWriter != null)
				WriteCode(spec, codeWriter);

			if (testWriter != null)
				WriteTest(spec, testWriter);
		}

		/// <summary>
		/// Writes a warning about the file being generated.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteWarning(TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.WriteLine("//////////////////////////////////////////////////////");
			writer.WriteLine("//                     WARNING                      //");
			writer.WriteLine("//     The contents of this file is generated.      //");
			writer.WriteLine("//    DO NOT MODIFY, your changes will be lost!     //");
			writer.WriteLine("//////////////////////////////////////////////////////");
		}

		/// <summary>
		/// Writes the copyright notice and license information.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
		protected void WriteLicense(TextWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.WriteLine("#region Copyright and License");
			writer.WriteLine("/*");
			writer.WriteLine(" * SharpAssembler");
			writer.WriteLine(" * Library for .NET that assembles a predetermined list of");
			writer.WriteLine(" * instructions into machine code.");
			writer.WriteLine(" * ");
			writer.WriteLine(" * Copyright (C) 2011-2012 Daniël Pelsmaeker");
			writer.WriteLine(" * ");
			writer.WriteLine(" * This file is part of SharpAssembler.");
			writer.WriteLine(" * ");
			writer.WriteLine(" * SharpAssembler is free software: you can redistribute it and/or modify");
			writer.WriteLine(" * it under the terms of the GNU General Public License as published by");
			writer.WriteLine(" * the Free Software Foundation, either version 3 of the License, or");
			writer.WriteLine(" * (at your option) any later version.");
			writer.WriteLine(" * ");
			writer.WriteLine(" * SharpAssembler is distributed in the hope that it will be useful,");
			writer.WriteLine(" * but WITHOUT ANY WARRANTY; without even the implied warranty of");
			writer.WriteLine(" * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the");
			writer.WriteLine(" * GNU General Public License for more details.");
			writer.WriteLine(" * ");
			writer.WriteLine(" * You should have received a copy of the GNU General Public License");
			writer.WriteLine(" * along with SharpAssembler.  If not, see <http://www.gnu.org/licenses/>.");
			writer.WriteLine(" */");
			writer.WriteLine("#endregion");
		}

		/// <summary>
		/// A list of reserved C# keywords.
		/// </summary>
		private static readonly string[] Keywords = new[]{
			"abstract", "event", "new", "struct", "as", "explicit", "null", "switch", "base", "extern", "object",
			"this", "bool", "false", "operator", "throw", "break", "finally", "out", "true", "byte", "fixed",
			"override", "try", "case", "float", "params", "typeof", "catch", "for", "private", "uint", "char",
			"foreach", "protected", "ulong", "checked", "goto", "public", "unchecked", "class", "if", "readonly",
			"unsafe", "const", "implicit", "ref", "ushort", "continue", "in", "return", "using", "decimal", "int",
			"sbyte", "virtual", "default", "interface", "sealed", "volatile", "delegate", "internal", "short", "void",
			"do", "is", "sizeof", "while", "double", "lock", "stackalloc", "else", "long", "static", "enum",
			"namespace", "string"
		};

		/// <summary>
		/// Returns whether the specified variable name is a keyword.
		/// </summary>
		/// <param name="variableName">The name to check.</param>
		/// <returns><see langword="true"/> when it is a keyword; otherwise, <see langword="false"/>.</returns>
		protected bool IsKeyword(string variableName)
		{
			return Keywords.Contains(variableName);
		}

		/// <summary>
		/// Returns the given variable name as a valid C# variable name.
		/// </summary>
		/// <param name="variableName">The variable name to return.</param>
		/// <returns>The variable name made safe for use in C#.</returns>
		protected string AsVariableName(string variableName)
		{
			if (IsKeyword(variableName))
				return "@" + variableName;
			else
				return variableName;
		}

		#region Invariant
		/// <summary>
		/// Asserts the invariants for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			
		}
		#endregion
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(SpecWriter))]
		abstract partial class SpecWriterContract : SpecWriter
		{
			protected override string SubNamespace
			{
				get
				{
					Contract.Ensures(!String.IsNullOrWhiteSpace(Contract.Result<string>()));
					return default(string);
				}
			}
		}
	}
	#endregion
}
