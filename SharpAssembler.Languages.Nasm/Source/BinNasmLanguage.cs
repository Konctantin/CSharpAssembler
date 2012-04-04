using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages.Nasm
{
	/// <summary>
	/// The NASM language for the BIN object file format.
	/// </summary>
	public class BinNasmLanguage : NasmLanguage
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BinNasmLanguage"/> class.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> used to write the assembly code to.</param>
		public BinNasmLanguage(TextWriter writer)
			: base(writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion
		}
		#endregion

		/// <inheritdoc />
		protected override void WriteSectionStart(Section section)
		{
			Writer.Write("SECTION {0}", section.Identifier);

			// Only write 'progbits' or 'nobits' when it is not the default.
			if (section.NoBits && section.Identifier != ".bss")
				Writer.Write(" nobits");
			else if (!section.NoBits && section.Identifier == ".bss")
				Writer.Write(" progbits");

			// Alignment, when it is not the default
			if (section.Alignment != 4)
				Writer.Write(" align={0}", section.Alignment);
			// TODO: Support Start, Follows, VFollows, ORG
			if (section.Address != null)
				Writer.Write(" vstart={0}", section.Address.Value);

			Writer.WriteLine();
		}
	}
}
