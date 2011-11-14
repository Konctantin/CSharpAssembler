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
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace SharpAssembler.Core.Instructions
{
	/// <summary>
	/// A comment.
	/// </summary>
	public class Comment : Constructable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Comment"/> class with the specified comment text.
		/// </summary>
		/// <param name="text">The text in the comment.</param>
		public Comment(string text)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(text != null);
			#endregion

			this.text = text;
		}
		#endregion

		#region Properties
		private string text;
		/// <summary>
		/// Gets or sets the text of the comment.
		/// </summary>
		/// <value>The text of the comment.</value>
		public string Text
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return text;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				text = value;
			}
#endif
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
			// Comments are not represented in the resulting binary file.
			return new IEmittable[0];
		}
		#endregion

		#region Invariant
		/// <summary>
		/// The invariant method for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.text != null);
		}
		#endregion
	}
}
