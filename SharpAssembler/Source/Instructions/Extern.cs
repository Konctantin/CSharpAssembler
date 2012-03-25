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
using SharpAssembler.Symbols;
using System.Collections.Generic;

namespace SharpAssembler.Instructions
{
	/// <summary>
	/// Declares that a symbol is defined elsewhere.
	/// </summary>
	public class Extern : Constructable, IIdentifiable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Extern"/> class.
		/// </summary>
		/// <param name="identifier">The identifier of the external symbol.</param>
		public Extern(string identifier)
			: this(identifier, 0)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Extern"/> class.
		/// </summary>
		/// <param name="identifier">The identifier of the external symbol.</param>
		/// <param name="length">The length of the symbol.</param>
		public Extern(string identifier, Int128 length)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
			#endregion

			this.identifier = identifier;
			this.length = length;
			this.associatedSymbol = new Symbol(this, SymbolType.Extern);
		}
		#endregion

		#region Properties
		private string identifier;
		/// <summary>
		/// Gets the identifier of the extern symbol.
		/// </summary>
		/// <value>The identifier of the extern symbol.</value>
		public string Identifier
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return identifier;
			}
		}

		private Symbol associatedSymbol;
		/// <summary>
		/// Gets the <see cref="Symbol"/> associated with this block.
		/// </summary>
		/// <value>A <see cref="Symbol"/>.</value>
		public Symbol AssociatedSymbol
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Symbol>() != null);
				#endregion
				return associatedSymbol;
			}
		}

		private Int128 length = 0;
		/// <summary>
		/// Gets the length of this <see cref="Extern"/>.
		/// </summary>
		/// <value>The length, in bytes. The default is 0.</value>
		public Int128 Length
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Int128>() >= 0);
				#endregion
				return length;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(value >= 0);
				#endregion
				length = value;
			}
#endif
		}
		#endregion

		#region Methods
		/// <inheritdoc />
		public override IEnumerable<IEmittable> Construct(Context context)
		{
			// CONTRACT: Constructable
			context.SymbolTable.Add(associatedSymbol);

			yield break;
		}

		/// <inheritdoc />
		public override void Accept(IObjectFileVisitor visitor)
		{
			visitor.VisitExtern(this);
		}
		#endregion

		#region Hierarchy
		/// <summary>
		/// Gets the <see cref="IFile"/> in which this <see cref="Section"/> is defined.
		/// </summary>
		/// <value>A <see cref="IFile"/>.</value>
		IFile IAssociatable.ParentFile
		{
			// TODO: Implement.
			get { throw new NotImplementedException(); }
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.identifier != null);
			Contract.Invariant(this.length >= 0);
			Contract.Invariant(this.associatedSymbol != null);
		}
		#endregion
	}
}
