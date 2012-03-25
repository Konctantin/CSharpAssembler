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
	/// Defines a symbol with a value.
	/// </summary>
	/// <remarks>
	/// Whereas a <see cref="Label"/> is a reference to a particular location in the file, a <see cref="Define"/> is
	/// more like the <c>EQU</c> in NASM and defines a symbol to have a particular value.
	/// </remarks>
	public class Define : Constructable, IIdentifiable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Define"/> class that defines a private symbol.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol.</param>
		/// <param name="expression">The expression which returns the value for the symbol.</param>
		public Define(string identifier, Func<Context, SimpleExpression> expression)
			: this(identifier, LabelType.Private, expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Define"/> class.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol.</param>
		/// <param name="labelType">The type of symbol defined.</param>
		/// <param name="expression">The expression which returns the value for the symbol.</param>
		public Define(string identifier, LabelType labelType, Func<Context, SimpleExpression> expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(LabelType), labelType));
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			this.identifier = identifier;
			this.labelType = labelType;
			this.associatedSymbol = new Symbol(this, labelType.ToSymbolType());
			this.expression = expression;
		}
		#endregion

		#region Properties
		private string identifier;
		/// <summary>
		/// Gets or sets the identifier of the symbol.
		/// </summary>
		/// <value>The identifier of the symbol.</value>
		public string Identifier
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return identifier;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				identifier = value;
			}
#endif
		}

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
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				expression = value;
			}
#endif
		}

		private LabelType labelType;
		/// <summary>
		/// Gets or sets the type of symbol which this definition defines.
		/// </summary>
		/// <value>A member of the <see cref="LabelType"/> enumeration.</value>
		public LabelType LabelType
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(LabelType), Contract.Result<LabelType>()));
				#endregion
				return labelType;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(LabelType), value));
				#endregion
				labelType = value;
				associatedSymbol.SymbolType = value.ToSymbolType();
			}
#endif
		}

		private Symbol associatedSymbol;
		/// <summary>
		/// Gets the <see cref="Symbol"/> associated with this value.
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
		#endregion

		#region Methods
		/// <inheritdoc />
		public override IEnumerable<IEmittable> Construct(Context context)
		{
			var result = expression(context).Evaluate(context);
			associatedSymbol.Address = result;
			associatedSymbol.DefiningSection = context.Section;
			associatedSymbol.DefiningFile = context.Section.Parent;
			context.SymbolTable.Add(associatedSymbol);

			yield break;
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
			Contract.Invariant(this.identifier != null);
			Contract.Invariant(this.expression != null);
			Contract.Invariant(Enum.IsDefined(typeof(LabelType), this.labelType));
			Contract.Invariant(this.associatedSymbol != null);
		}
		#endregion
	}
}
