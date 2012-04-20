using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Reads a test script.
	/// </summary>
	public class ScriptReader : IScriptReader
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptReader"/> class.
		/// </summary>
		/// <param name="tokenizer">The tokenizer to use.</param>
		public ScriptReader(ScriptTokenizer tokenizer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(tokenizer != null);
			#endregion

			this.tokenizer = tokenizer;
		}
		#endregion

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> Read(string path)
		{
			// CONTRACT: IScriptReader

			using (var stream = File.OpenRead(path))
			{
				return Read(stream);
			}
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> Read(Stream stream)
		{
			// CONTRACT: IScriptReader

			// TODO: Ensure that 'stream' is not closed afterwards.
			using (var reader = new StreamReader(stream))
			{
				return Read(reader);
			}
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> Read(TextReader reader)
		{
			// CONTRACT: IScriptReader

			this.state = ReaderState.Initial;
			this.opcodespecs = new List<OpcodeSpec>();
			
			string script = reader.ReadToEnd();
			this.tokens = tokenizer.Tokenize(script).ToArray();
			this.tokenIndex = 0;
			Execute();
			return opcodespecs;
		}

		/// <summary>
		/// The tokenizer to use.
		/// </summary>
		private ScriptTokenizer tokenizer;

		/// <summary>
		/// The current reader state.
		/// </summary>
		private ReaderState state;

		/// <summary>
		/// A list to which all read <see cref="OpcodeSpec"/> objects are added.
		/// </summary>
		private List<OpcodeSpec> opcodespecs;

		/// <summary>
		/// An array of tokens.
		/// </summary>
		private string[] tokens;

		/// <summary>
		/// The index of the current token.
		/// </summary>
		private int tokenIndex;

		/// <summary>
		/// Executes the state machine.
		/// </summary>
		private void Execute()
		{
			while (this.state != ReaderState.Finished)
			{
				switch (this.state)
				{
					case ReaderState.Initial:
						this.state = ReaderState.ObjectDefinition;
						break;
					case ReaderState.ObjectDefinition:
						ReadObjectDefinition();
						break;
					default:
						throw new NotImplementedException("In an unexpected state.");
				}
			}
		}

		/// <summary>
		/// Gets a token and advances the token list.
		/// </summary>
		/// <returns>A token; or <see langword="null"/> when there are no more tokens.</returns>
		private string GetToken()
		{
			if (this.tokenIndex >= this.tokens.Length)
				return null;

			string token = this.tokens[this.tokenIndex];
			this.tokenIndex++;
			return token;
		}

		/// <summary>
		/// Reads an object definition from the file.
		/// </summary>
		private void ReadObjectDefinition()
		{
			
		}
	}
}
