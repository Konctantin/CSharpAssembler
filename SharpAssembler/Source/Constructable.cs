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
using System.Collections;
using System.Diagnostics.Contracts;
using SharpAssembler.Instructions;
using System.Collections.Generic;

namespace SharpAssembler
{
	/// <summary>
	/// An interface for entities which can be contained in a <see cref="Section"/> and can create a
	/// representation of themselves.
	/// </summary>
	[ContractClass(typeof(Contracts.ConstructableContract))]
	public abstract class Constructable : IAnnotatable, IObjectFileVisitable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Constructable"/> class.
		/// </summary>
		protected Constructable()
		{
		}
		#endregion

		#region Properties
		private IDictionary annotations = new Hashtable();
		/// <summary>
		/// Gets a dictionary which may be used to store data specific to this object.
		/// </summary>
		/// <value>An implementation of the <see cref="IDictionary"/> interface.</value>
		/// <remarks>
		/// This property is not serialized or deserialized.
		/// </remarks>
		public IDictionary Annotations
		{
			get { return annotations; }
		}

		private Comment comment;
		/// <summary>
		/// Gets or sets a <see cref="Comment"/> which is associated with this <see cref="Constructable"/>.
		/// </summary>
		/// <value>A <see cref="Comment"/>; or <see langword="null"/> when no comment is associated.</value>
		public Comment Comment
		{
			get { return comment; }
			set
			{
				#region Contract
				Contract.Requires<NotSupportedException>(value == null || !(this is Comment),
					"You cannot assign a comment to a comment.");
				#endregion
				comment = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The mutable <see cref="Context"/> in which the emittable will be constructed.</param>
		/// <returns>A list of constructed emittables; or an empty list.</returns>
		public abstract IEnumerable<IEmittable> Construct(Context context);

		/// <inheritdoc />
		public virtual void Accept(IObjectFileVisitor visitor)
		{
			visitor.VisitConstructable(this);
		}
		#endregion

		#region Hierarchy
#if false
		private Section parent;
		/// <summary>
		/// Gets the <see cref="Section"/> in which this constructable is declared.
		/// </summary>
		/// <value>A <see cref="Section"/>; or <see langword="null"/> when the constructable is not part of any
		/// section.</value>
		public Section Parent
		{
			get { return parent; }
			internal set { parent = value; }
		}
#endif
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.annotations != null);
		}
		#endregion
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="Constructable"/> interface.
		/// </summary>
		[ContractClassFor(typeof(Constructable))]
		abstract class ConstructableContract : Constructable
		{
			public override IEnumerable<IEmittable> Construct(Context context)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Ensures(Contract.Result<IEnumerable<IEmittable>>() != null);

				return default(IEnumerable<IEmittable>);
			}
		}
	}
	#endregion
}
