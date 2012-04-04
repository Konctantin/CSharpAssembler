using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages.Nasm
{
	/// <summary>
	/// The NASM language for x86-64 assembly in the BIN object file format.
	/// </summary>
	public class BinX86NasmLanguage : BinNasmLanguage
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BinX86NasmLanguage"/> class.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> used to write the assembly code to.</param>
		public BinX86NasmLanguage(TextWriter writer)
			: base(writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion
		}
		#endregion

	}
}
