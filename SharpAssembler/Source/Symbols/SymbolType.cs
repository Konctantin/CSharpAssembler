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

namespace SharpAssembler.Symbols
{
	/// <summary>
	/// Specifies the type of a symbol.
	/// </summary>
	public enum SymbolType
	{
		/// <summary>
		/// The symbol's type is not specified.
		/// </summary>
		None = 0,
		/// <summary>
		/// The symbol is local to the object file.
		/// </summary>
		Private,
		/// <summary>
		/// The symbol is defined and available for other object files to refer to.
		/// </summary>
		Public,
		/// <summary>
		/// Similar to <see cref="SymbolType.Public"/> but with less precedence.
		/// </summary>
		Weak,
		/// <summary>
		/// The symbol is not defined but available in another object file.
		/// </summary>
		Extern,
	}
}
