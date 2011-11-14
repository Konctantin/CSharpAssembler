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
using SharpAssembler.Core;
using SharpAssembler.Core.Symbols;
using SharpAssembler.x86;

namespace SharpAssembler.BinFormat
{
	/// <summary>
	/// A BIN object file.
	/// </summary>
	public class BinObjectFile : ObjectFile
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BinObjectFile"/> class.
		/// </summary>
		/// <param name="name">The name of the object file.</param>
		/// <param name="architecture">The architecture.</param>
		public BinObjectFile(string name, IArchitecture architecture)
			: base(name, architecture)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentNullException>(architecture != null);
			if (!IsSupportedArchitecture(architecture))
				throw new NotSupportedException("The specified architecture must be supported " +
					"by this object file format.");
			#endregion
		}
		#endregion

		#region Methods
		/// <summary>
		/// Assembles the object file and writes the resulting binary to the specified <see cref="BinaryWriter"/>.
		/// </summary>
		/// <param name="writer">A <see cref="BinaryWriter"/> object.</param>
		public override void Assemble(BinaryWriter writer)
		{
			// CONTRACT: ObjectFile

			// TODO: Emit .bss after the last progbits section.

			// Create a new context.
			Context context = this.Architecture.CreateContext(this);

			// Construct each section.
			context.Reset();
			context.Address = 0;		// Addresses relative to file.
			foreach (Section section in this)
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
			foreach (Section section in this)
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

		/// <summary>
		/// Returns whether this <see cref="ObjectFile"/> can contain binary data for the specified architecture.
		/// </summary>
		/// <param name="architecture">The <see cref="IArchitecture"/> to test.</param>
		/// <returns><see langword="true"/> when this <see cref="ObjectFile"/> can contain binary data for
		/// <paramref name="architecture"/>; otherwise, <see langword="false"/>.</returns>
		[Pure]
		public override bool IsSupportedArchitecture(IArchitecture architecture)
		{
			Architecture x86arch = architecture as Architecture;
			if (x86arch == null)
				return false;
			if (x86arch.AddressSize != DataSize.Bit64 &&
				x86arch.AddressSize != DataSize.Bit32 &&
				x86arch.AddressSize != DataSize.Bit16)
				return false;
			return true;
		}

		/// <summary>
		/// Checks whether the object file format supports the specified feature.
		/// </summary>
		/// <param name="feature">The <see cref="ObjectFileFeature"/> to test.</param>
		/// <returns><see langword="true"/> when it is supported; otherwise, <see langword="false"/>.</returns>
		public override bool SupportsFeature(ObjectFileFeature feature)
		{
			switch (feature)
			{
				case ObjectFileFeature.ArbitraryPhysicalStart:
					return true;
				case ObjectFileFeature.None:
					return true;
				default:
					return false;
			}
		}
		#endregion
	}
}
