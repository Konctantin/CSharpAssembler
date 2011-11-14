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
using SharpAssembler.Core.Collections;

namespace SharpAssembler.Core.Instructions
{
	/// <summary>
	/// A group of constructables.
	/// </summary>
	public class Group : Constructable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Group"/> class.
		/// </summary>
		public Group()
			: this(new ConstructableList())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Group"/> class with the specified list
		/// of <see cref="Constructable"/> objects.
		/// </summary>
		/// <param name="constructables">The list to use.</param>
		protected Group(IList<Constructable> constructables)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(constructables != null);
			#endregion

			this.constructables = constructables;
		}
		#endregion

		#region Properties
		private IList<Constructable> constructables;
		/// <summary>
		/// Gets an ordered list of <see cref="Constructable"/> objects in this group.
		/// </summary>
		/// <value>A <see cref="Collection{T}"/> of <see cref="Constructable"/> objects.</value>
		public IList<Constructable> Constructables
		{
			get { return constructables; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The mutable <see cref="Context"/> in which the emittable will be constructed.</param>
		/// <returns>A list of constructed emittables; or an empty list.</returns>
		public override IList<IEmittable> Construct(Context context)
		{
			List<IEmittable> emittables = new List<IEmittable>();
			foreach (Constructable constructable in this.constructables)
			{
				emittables.AddRange(constructable.Construct(context));
			}
			return emittables;
		}
		#endregion

		#region Invariant
		/// <summary>
		/// The invariant method for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.constructables != null);
		}
		#endregion
	}
}
