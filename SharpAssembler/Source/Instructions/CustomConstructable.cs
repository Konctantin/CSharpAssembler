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
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Instructions
{
	/// <summary>
	/// Base class for custom constructables.
	/// </summary>
	public abstract class CustomConstructable : Constructable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomConstructable"/> class.
		/// </summary>
		protected CustomConstructable()
		{
		}
		#endregion

		/// <summary>
		/// Gets all the constructables that represent this custom constructable.
		/// </summary>
		/// <param name="context">The context in which the constructables are retrieved.</param>
		/// <returns>An enumerable collection of <see cref="Constructable"/> objects.</returns>
		/// <remarks>
		/// Elements of the returned enumerable may be <see langword="null"/>.
		/// </remarks>
		protected abstract IEnumerable<Constructable> GetContent(Context context);

		/// <inheritdoc />
		public sealed override IEnumerable<IEmittable> Construct(Context context)
		{
			var constructables = GetContent(context);
			var emittables = constructables.SelectMany(c => c.Construct(context));
			return emittables;
		}

		/// <inheritdoc />
		public override void Accept(IObjectFileVisitor visitor)
		{
			visitor.VisitCustomConstructable(this);
		}

		#region Invariant
		/// <summary>
		/// The invariant method for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
		}
		#endregion
	}
}
