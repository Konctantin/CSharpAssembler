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
	public class ElfNasmLanguage : NasmLanguage
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

			#region Defaults
			bool defaultProgbits;
			bool defaultAlloc;
			bool defaultExec;
			bool defaultWrite;
			int defaultAlign;
			bool defaultThreadLocalVariables;
			switch (section.Identifier)
			{
				case ".text":
					defaultProgbits = true;
					defaultAlloc = true;
					defaultExec = true;
					defaultWrite = false;
					defaultAlign = 16;
					defaultThreadLocalVariables = false;
					break;
				case ".rodata":
				case ".lrodata":
					defaultProgbits = true;
					defaultAlloc = true;
					defaultExec = false;
					defaultWrite = false;
					defaultAlign = 4;
					defaultThreadLocalVariables = false;
					break;
				case ".data":
				case ".ldata":
					defaultProgbits = true;
					defaultAlloc = true;
					defaultExec = false;
					defaultWrite = true;
					defaultAlign = 4;
					defaultThreadLocalVariables = false;
					break;
				case ".bss":
				case ".lbss":
					defaultProgbits = false;
					defaultAlloc = true;
					defaultExec = false;
					defaultWrite = true;
					defaultAlign = 4;
					defaultThreadLocalVariables = false;
					break;
				case ".tdata":
					defaultProgbits = true;
					defaultAlloc = true;
					defaultExec = false;
					defaultWrite = true;
					defaultAlign = 4;
					defaultThreadLocalVariables = true;
					break;
				case ".tbss":
					defaultProgbits = false;
					defaultAlloc = true;
					defaultExec = false;
					defaultWrite = true;
					defaultAlign = 4;
					defaultThreadLocalVariables = true;
					break;
				case ".comment":
					defaultProgbits = true;
					defaultAlloc = false;
					defaultExec = false;
					defaultWrite = false;
					defaultAlign = 1;
					defaultThreadLocalVariables = false;
					break;
				default:
					defaultProgbits = true;
					defaultAlloc = true;
					defaultExec = false;
					defaultWrite = false;
					defaultAlign = 1;
					defaultThreadLocalVariables = false;
					break;
			}
			#endregion

			// Only write 'progbits' or 'nobits' when it is not the default.
			if (section.NoBits != !defaultProgbits)
			{
				if (section.NoBits)
					Writer.Write(" nobits");
				else
					Writer.Write(" progbits");
			}

			// Only write 'alloc' or 'noalloc' when it is not the default.
			if (section.Allocate != defaultAlloc)
			{
				if (section.Allocate)
					Writer.Write(" alloc");
				else
					Writer.Write(" noalloc");
			}

			// Only write 'exec' or 'noexec' when it is not the default.
			if (section.Executable != defaultExec)
			{
				if (section.Executable)
					Writer.Write(" exec");
				else
					Writer.Write(" noexec");
			}

			// Only write 'write' or 'nowrite' when it is not the default.
			if (section.Writable != defaultWrite)
			{
				if (section.Writable)
					Writer.Write(" write");
				else
					Writer.Write(" nowrite");
			}

			// Only write the alignment when it is not the default
			if (section.Alignment != defaultAlign)
				Writer.Write(" align={0}", section.Alignment);

			// TODO: Support TLS.
#if false
			// Only write 'tls' when it is not the default.
			if (section.TLS && section.TLS != defaultThreadLocalVariables)
				Writer.Write(" tls");
#endif

			Writer.WriteLine();
		}
	}
}
