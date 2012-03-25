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
using SharpAssembler;
using SharpAssembler.Symbols;
using SharpAssembler.Architectures.X86;

namespace SharpAssembler.Formats.Bin
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
		/// <param name="format">The object file format.</param>
		/// <param name="architecture">The architecture.</param>
		/// <param name="name">The name of the object file; or <see langword="null"/>.</param>
		internal BinObjectFile(BinObjectFileFormat format, IArchitecture architecture, string name)
			: base(format, architecture, name)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(format != null);
			Contract.Requires<ArgumentNullException>(architecture != null);
			#endregion
		}
		#endregion
	}
}
