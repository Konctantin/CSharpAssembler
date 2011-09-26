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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Core.Symbols
{
	/// <summary>
	/// The symbol table. Symbols can be retrieved by name, and are enumerated by virtual address.
	/// </summary>
	public class SymbolTable : KeyedCollection<String, Symbol>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SymbolTable"/> class.
		/// </summary>
		public SymbolTable()
		{
			// Nothing to do.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SymbolTable"/> class.
		/// </summary>
		/// <param name="symbols">The items to add to this collection.</param>
		public SymbolTable(IEnumerable<Symbol> symbols)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(symbols != null);
			#endregion

			AddRange(symbols);
		}
		#endregion

		/// <summary>
		/// Adds a range of symbols to the symbol table.
		/// </summary>
		/// <param name="symbols">The symbols to add.</param>
		public void AddRange(IEnumerable<Symbol> symbols)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(symbols != null);
			#endregion

			foreach (Symbol symbol in symbols)
			{
				this.Add(symbol);
			}
		}

		/// <summary>
		/// Extracts the key from the specified element.
		/// </summary>
		/// <param name="item">The element from which to extract the key.</param>
		/// <returns>The key for the specified element.</returns>
		protected override String GetKeyForItem(Symbol item)
		{
			return item.Identifier;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the
		/// key is found; otherwise, the default value for the type of the value parameter.</param>
		/// <returns><see langword="true"/> if the <see cref="SymbolTable"/> contains an element with the specified
		/// key; otherwise, <see langword="false"/>.</returns>
		public bool TryGetValue(string key, out Symbol value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(key != null);
			#endregion

			if (this.Contains(key))
			{
				value = this[key];
				return true;
			}
			else
			{
				value = default(Symbol);
				return false;
			}
		}
	}
}
