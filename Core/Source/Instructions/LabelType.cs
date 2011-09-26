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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler.Core.Symbols;

namespace SharpAssembler.Core.Instructions
{
	/// <summary>
	/// Specifies the type of symbol a label creates.
	/// </summary>
	public enum LabelType
	{
		/// <summary>
		/// A private symbol. This is the default.
		/// </summary>
		Private = 0,
		/// <summary>
		/// A weak public symbol.
		/// </summary>
		Weak,
		/// <summary>
		/// A strong public symbol.
		/// </summary>
		Public,
	}

	/// <summary>
	/// Extensions for the <see cref="LabelType"/> enumeration.
	/// </summary>
	public static class LabelTypeExtensions
	{
		/// <summary>
		/// Converts a <see cref="LabelType"/> to the corresponding <see cref="SymbolType"/>.
		/// </summary>
		/// <param name="type">The <see cref="LabelType"/> to convert.</param>
		/// <returns>The resulting <see cref="SymbolType"/>.</returns>
		public static SymbolType ToSymbolType(this LabelType type)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(LabelType), type));
			#endregion
			switch (type)
			{
				case LabelType.Private:
					return SymbolType.Private;
				case LabelType.Weak:
					return SymbolType.Weak;
				case LabelType.Public:
					return SymbolType.Public;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
