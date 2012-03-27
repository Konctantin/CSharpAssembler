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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler.Symbols;
using System.Collections.Generic;

namespace SharpAssembler.Instructions
{
	/// <summary>
	/// A label, which defines a symbol.
	/// </summary>
	public class Label : Constructable, IAssociatable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class
		/// that defines a private anonymous symbol.
		/// </summary>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Label()
			: this(null, SymbolType.Private)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class
		/// that defines a private symbol with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol; or <see langword="null"/>.</param>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Label(string identifier)
			: this(identifier, SymbolType.Private)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class
		/// that defines a symbol with the specified type and identifier.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol.</param>
		/// <param name="symbolType">The type of symbol defined.</param>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Label(string identifier, SymbolType symbolType)
			: this(new Symbol(symbolType, identifier))
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SymbolType), symbolType));
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class
		/// that defines the specified symbol.
		/// </summary>
		/// <param name="symbol">The symbol that is defined.</param>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Label(Symbol symbol)
		{
			this.DefinedSymbol = symbol;
		}
		#endregion

		#region Properties
		private Symbol definedSymbol;
		/// <summary>
		/// Gets or sets the symbol that is defined by this instruction.
		/// </summary>
		/// <value>The <see cref="Symbol"/> that is defined by this instruction;
		/// or <see langword="null"/>.</value>
		/// <remarks>
		/// When <see cref="DefinedSymbol"/> is <see langword="null"/>, the instruction
		/// does not have any effect on the generated assembly.
		/// </remarks>
		public Symbol DefinedSymbol
		{
			get { return this.definedSymbol; }
			// NOTE: This property's field is set by the SetAssociatedSymbol() method.
			set { Symbol.SetAssociation(this, value); }
		}

		/// <inheritdoc />
		Symbol IAssociatable.AssociatedSymbol
		{
			get { return this.DefinedSymbol; }
		}
		#endregion

		#region Methods
		/// <inheritdoc />
		public override IEnumerable<IEmittable> Construct(Context context)
		{
			if (this.definedSymbol != null)
				this.definedSymbol.Define(context, context.Address);

			yield break;
		}

		/// <inheritdoc />
		void IAssociatable.SetAssociatedSymbol(Symbol symbol)
		{
			this.definedSymbol = symbol;
		}

		/// <inheritdoc />
		public override void Accept(IObjectFileVisitor visitor)
		{
			visitor.VisitLabel(this);
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
