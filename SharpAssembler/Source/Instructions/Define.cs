#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011-2012 Daniël Pelsmaeker
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
	/// Defines a symbol with a value.
	/// </summary>
	/// <remarks>
	/// Whereas a <see cref="Label"/> is a reference to a particular location in the file, a <see cref="Define"/> is
	/// more like the <c>EQU</c> in NASM and defines a symbol to have a particular value.
	/// </remarks>
	public class Define : Constructable, IAssociatable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Define"/> class
		/// that defines a private anonymous symbol.
		/// </summary>
		/// <param name="expression">The expression which returns the value for the symbol.</param>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Define(Func<Context, SimpleExpression> expression)
			: this(null, SymbolType.Private, expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Define"/> class
		/// that defines a private symbol with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol; or <see langword="null"/>.</param>
		/// <param name="expression">The expression which returns the value for the symbol.</param>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Define(string identifier, Func<Context, SimpleExpression> expression)
			: this(identifier, SymbolType.Private, expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Define"/> class
		/// that defines a symbol with the specified type and identifier.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol.</param>
		/// <param name="symbolType">The type of symbol defined.</param>
		/// <param name="expression">The expression which returns the value for the symbol.</param>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Define(string identifier, SymbolType symbolType, Func<Context, SimpleExpression> expression)
			: this(new Symbol(symbolType, identifier), expression)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SymbolType), symbolType));
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Define"/> class
		/// that defines the specified symbol.
		/// </summary>
		/// <param name="symbol">The symbol that is defined.</param>
		/// <param name="expression">The expression which returns the value for the symbol.</param>
		/// <remarks>
		/// The <see cref="DefinedSymbol"/> property holds the symbol that is defined.
		/// </remarks>
		public Define(Symbol symbol, Func<Context, SimpleExpression> expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			this.expression = expression;
			this.DefinedSymbol = symbol;
		}
		#endregion

		#region Properties
		private Func<Context, SimpleExpression> expression;
		/// <summary>
		/// Gets or sets the expression evaluated to result in the symbol's value.
		/// </summary>
		/// <value>A function accepting a <see cref="Context"/> and returning a <see cref="SimpleExpression"/>.</value>
		public Func<Context, SimpleExpression> Expression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Func<Context, SimpleExpression>>() != null);
				#endregion
				return expression;
			}
		}

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
			var result = expression(context).Evaluate(context);
			if (this.definedSymbol != null)
				this.definedSymbol.Define(context, result);

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
			visitor.VisitDefine(this);
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
			Contract.Invariant(this.expression != null);
		}
		#endregion
	}
}
