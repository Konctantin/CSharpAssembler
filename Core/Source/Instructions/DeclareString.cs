﻿#region Copyright and License
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
using System.Text;

namespace SharpAssembler.Core.Instructions
{
	/// <summary>
	/// Declares a string.
	/// </summary>
	public class DeclareString : Constructable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareString"/> class declaring the specified string
		/// with UTF-8 encoding.
		/// </summary>
		/// <param name="data">The data.</param>
		public DeclareString(string data)
			: this(data, Encoding.UTF8)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(data != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareString"/> class declaring the specified string
		/// with the specified encoding.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="encoding">The encoding of the string.</param>
		public DeclareString(string data, Encoding encoding)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(data != null);
			Contract.Requires<ArgumentNullException>(encoding != null);
			#endregion

			this.data = data;
			this.encoding = encoding;
		}
		#endregion

		#region Properties
		private string data;
		/// <summary>
		/// Gets or sets the string that will be declared.
		/// </summary>
		/// <value>A string.</value>
		public string Data
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return data;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.data = value;
			}
#endif
		}

		private Encoding encoding;
		/// <summary>
		/// Gets or sets the encoding of the declared string.
		/// </summary>
		/// <value>An <see cref="Encoding"/>.</value>
		public Encoding Encoding
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Encoding>() != null);
				#endregion
				return encoding;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.encoding = value;
			}
#endif
		}
		#endregion

		#region Methods
		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The mutable <see cref="Context"/> in which the emittable will be constructed.</param>
		/// <returns>The constructed emittable; or <see langword="null"/> when no emittable results from this
		/// constructable.</returns>
		public override IEmittable Construct(Context context)
		{
			// CONTRACT: Constructable
			return new RawEmittable(this.encoding.GetBytes(this.data));
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.data != null);
			Contract.Invariant(this.encoding != null);
		}
		#endregion
	}
}
