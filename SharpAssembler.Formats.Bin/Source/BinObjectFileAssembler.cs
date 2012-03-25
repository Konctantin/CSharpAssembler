#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011 Daniël Pelsmaeker
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
using SharpAssembler.Symbols;

namespace SharpAssembler.Formats.Bin
{
	/// <summary>
	/// An assembler for BIN object files.
	/// </summary>
	public class BinObjectFileAssembler : ObjectFileAssembler
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BinObjectFileAssembler"/> class.
		/// </summary>
		/// <param name="objectFile">The object file that will be assembled.</param>
		public BinObjectFileAssembler(ObjectFile objectFile)
			: base(objectFile)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(objectFile != null);
			Contract.Requires<ArgumentException>(typeof(BinObjectFileFormat).IsAssignableFrom(objectFile.Format.GetType()));
			#endregion
		}
		#endregion

		/// <inheritdoc />
		public override void Assemble(BinaryWriter writer)
		{
			// CONTRACT: ObjectFile

			// TODO: Emit .bss after the last progbits section.

			// Create a new context.
			Context context = this.ObjectFile.Architecture.CreateContext(this.ObjectFile);

			// Construct each section.
			context.Reset();
			context.Address = 0;		// Addresses relative to file.
			foreach (Section section in this.ObjectFile.Sections)
			{
				string sectionName = String.Format("section.{0}.start", section.Identifier);
				var symbol = new Symbol(section, SymbolType.Private, sectionName);
				symbol.Address = context.Address;
				symbol.DefiningSection = section;
				symbol.DefiningFile = section.Parent;
				context.SymbolTable.Add(symbol);

				section.Construct(context);
			}

			// Emit each section and write it directly to the writer.
			context.Reset();
			context.Address = 0;		// Addresses relative to file.
			foreach (Section section in this.ObjectFile.Sections)
			{
				MathExt.CalculatePadding(writer.BaseStream.Position, section.Alignment);
				writer.Align(section.Alignment);
				section.Emit(writer, context);
			}

			// Test for illegal symbols.
			CheckSymbolSupport(context);

			writer.Flush();
		}

		/// <summary>
		/// Checks whether the symbols in the context are supported by this object file format.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> to check.</param>
		/// <exception cref="AssemblerException">
		/// <para>Public symbols are not supported.</para>
		/// -or-
		/// <para>Extern symbols are not supported.</para>
		/// </exception>
		private void CheckSymbolSupport(Context context)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			#endregion

			foreach (Symbol s in context.SymbolTable)
			{
				if (s.SymbolType == SymbolType.Public)
					throw new AssemblerException("Public symbols are not supported by the BIN object file format.");
				if (s.IsExtern)
					throw new AssemblerException("Extern symbols are not supported by the BIN object file format.");
			}
		}
	}
}
