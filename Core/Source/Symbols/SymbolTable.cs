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
	/// <remarks>
	/// It is possible for a symbol to have no name, and for two symbols to have the same name.
	/// </remarks>
	public class SymbolTable : Collection<Symbol>
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
		/// Inserts an element into the <see cref="SymbolTable"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which
		/// <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		protected sealed override void InsertItem(int index, Symbol item)
		{
			#region Contract
			// CONTRACT: Collection<T>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion

			base.InsertItem(index, item);
		}

		/// <summary>
		/// Replaces the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to replace.</param>
		/// <param name="item">The new value for the element at the specified index.</param>
		protected sealed override void SetItem(int index, Symbol item)
		{
			#region Contract
			// CONTRACT: Collection<T>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion

			base.SetItem(index, item);
		}

		/// <summary>
		/// Checks whether the collection contains a <see cref="Symbol"/> with a particular identifier.
		/// </summary>
		/// <param name="identifier">The identifier to look for.</param>
		/// <returns><see langword="true"/> when the collection has a symbol with the specified identifier;
		/// otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// Symbols that have no identifier (their <see cref="Symbol.Identifier"/> is <see langword="null"/>)
		/// cannot be located using this method.
		/// </remarks>
		public bool Contains(string identifier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			#endregion
			
			return IndexOf(identifier) != -1;
		}

		/// <summary>
		/// Returns the index of the first symbol with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier to look for.</param>
		/// <returns>The zero-based index of the symbol in this table;
		/// or -1 when no symbol with the specified identifier was found.</returns>
		/// <remarks>
		/// Symbols that have no identifier (their <see cref="Symbol.Identifier"/> is <see langword="null"/>)
		/// cannot be located using this method.
		/// </remarks>
		public int IndexOf(string identifier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			#endregion

			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Identifier == identifier)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Gets the first symbol with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier to look for.</param>
		/// <returns>The <see cref="Symbol"/> with the specified identifier;
		/// or <see langword="null"/> when not found.</returns>
		/// <remarks>
		/// Symbols that have no identifier (their <see cref="Symbol.Identifier"/> is <see langword="null"/>)
		/// cannot be located using this method.
		/// </remarks>
		public Symbol this[string identifier]
		{
			get
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(identifier != null);
				#endregion
				
				int index = IndexOf(identifier);
				if (index < 0)
					return null;
				return this[index];
			}
		}
	}
}
