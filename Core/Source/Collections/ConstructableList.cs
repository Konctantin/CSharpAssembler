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
using System.Collections.ObjectModel;

namespace SharpAssembler.Core.Collections
{
	/// <summary>
	/// An ordered list of <see cref="Constructable"/> objects.
	/// </summary>
	public sealed class ConstructableList : Collection<Constructable>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstructableList"/> class.
		/// </summary>
		public ConstructableList()
		{

		}
		#endregion

		/// <summary>
		/// Inserts an element into the <see cref="ConstructableList"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which
		/// <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		protected sealed override void InsertItem(int index, Constructable item)
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
		protected sealed override void SetItem(int index, Constructable item)
		{
			#region Contract
			// CONTRACT: Collection<T>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion

			base.SetItem(index, item);
		}
	}
}
