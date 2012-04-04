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

namespace SharpAssembler.Languages.Nasm
{
	/// <summary>
	/// Interface for the NASM language internal workings.
	/// </summary>
	[ContractClassFor(typeof(Contracts.INasmLanguageControlContract))]
	public interface INasmLanguageControl
	{
		/// <summary>
		/// Writes the comment associated with the specified constructable.
		/// </summary>
		/// <param name="constructable">The <see cref="Constructable"/> to write the comment of.</param>
		/// <param name="linelength">The number of written characters on this line.</param>
		void WriteCommentOf(Constructable constructable, int linelength);

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
		int WriteCommentString(string comment, int column, int startindex, int lineCount);

		
		/// <summary>
		/// Removes an indentation level.
		/// </summary>
		void PopIndent();

		/// <summary>
		/// Adds an indentation level.
		/// </summary>
		void PushIndent();

		/// <summary>
		/// Writes the indent.
		/// </summary>
		/// <param name="level">The indent level.</param>
		/// <returns>The number of written characters.</returns>
		int WriteIndent(int level);
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(INasmLanguageControl))]
		abstract class INasmLanguageControlContract : INasmLanguageControl
		{
			public void WriteCommentOf(Constructable constructable, int linelength)
			{
				Contract.Requires<ArgumentNullException>(constructable != null);
				Contract.Requires<ArgumentOutOfRangeException>(linelength > 0);
			}

			public int WriteCommentString(string comment, int column, int startindex, int lineCount)
			{
				Contract.Requires<ArgumentOutOfRangeException>(column >= 0);
				Contract.Requires<ArgumentOutOfRangeException>(lineCount >= 0);
				Contract.Ensures(Contract.Result<int>() >= -1);
				return default(int);
			}

			public void PopIndent()
			{
				
			}

			public void PushIndent()
			{
			}

			public int WriteIndent(int level)
			{
				Contract.Requires<ArgumentOutOfRangeException>(level >= 0);
				Contract.Ensures(Contract.Result<int>() >= 0);
				return default(int);
			}
		}
	}
	#endregion
}
