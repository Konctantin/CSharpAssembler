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
	public class Extern : Constructable, IAssociatable
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
		public Extern(string identifier, long length)
			: this(new Symbol(SymbolType.Extern, identifier){Size = length})
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Extern"/> class.
		/// </summary>
		/// <param name="symbol">The external symbol.</param>
		/// <remarks>
		/// Ensure that the symbol has a valid identifiers. Most object file formats do not support anonymous
		/// external symbols.
		/// </remarks>
		public Extern(Symbol symbol)
		{
			#region Contract
			Contract.Requires<ArgumentException>(symbol == null || symbol.IsExtern);
			#endregion

			this.ExternSymbol = symbol;
		}
		#endregion

		#region Properties
		private Symbol externSymbol;
		/// <summary>
		/// Gets or sets the symbol that is referenced by this instruction.
		/// </summary>
		/// <value>The <see cref="Symbol"/> that is referenced by this instruction;
		/// or <see langword="null"/>.</value>
		/// <remarks>
		/// When <see cref="ExternSymbol"/> is <see langword="null"/>, the instruction
		/// does not have any effect on the generated assembly.
		/// </remarks>
		public Symbol ExternSymbol
		{
			get { return this.externSymbol; }
			// NOTE: This property's field is set by the SetAssociatedSymbol() method.
			set { Symbol.SetAssociation(this, value); }
		}

		/// <inheritdoc />
		Symbol IAssociatable.AssociatedSymbol
		{
			get { return this.ExternSymbol; }
		}
		#endregion

		#region Methods
		/// <inheritdoc />
		public override IEnumerable<IEmittable> Construct(Context context)
		{
			// CONTRACT: Constructable
			if (this.externSymbol != null)
				this.externSymbol.ReferenceExtern(context);

			yield break;
		}

		/// <inheritdoc />
		void IAssociatable.SetAssociatedSymbol(Symbol symbol)
		{
			this.externSymbol = symbol;
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
			
		}
		#endregion
	}
}
