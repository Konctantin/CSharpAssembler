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

namespace SharpAssembler
{
	/// <summary>
	/// An ordered collection of <see cref="Constructable"/> objects.
	/// </summary>
	/// <remarks>
	/// This collection may contain values that are <see langword="null"/>. They should be ignored.
	/// </remarks>
	public class ConstructableCollection : Collection<Constructable>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstructableCollection"/> class.
		/// </summary>
		public ConstructableCollection()
		{

		}
		#endregion

		/// <summary>
		/// Adds a range of <see cref="Constructable"/> objects to the collection.
		/// </summary>
		/// <param name="constructables">An enumerable collection of <see cref="Constructable"/> objects.</param>
		public void AddRange(IEnumerable<Constructable> constructables)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(constructables != null);
			#endregion

			foreach (var item in constructables)
			{
				this.Add(item);
			}
		}
	}
}
