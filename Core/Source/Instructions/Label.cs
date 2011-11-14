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
using SharpAssembler.Core.Symbols;
using System.Collections.Generic;

namespace SharpAssembler.Core.Instructions
{
	/// <summary>
	/// A label, which defines a symbol.
	/// </summary>
	public class Label : Constructable, IIdentifiable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class that creates a private label.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol.</param>
		public Label(string identifier)
			: this(identifier, LabelType.Private, 0)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol.</param>
		/// <param name="labelType">The type of symbol defined.</param>
		public Label(string identifier, LabelType labelType)
			: this(identifier, labelType, 0)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(LabelType), labelType));
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Label"/> class.
		/// </summary>
		/// <param name="identifier">The identifier of the defined symbol.</param>
		/// <param name="labelType">The type of symbol defined.</param>
		/// <param name="length">The length of the symbol.</param>
		public Label(string identifier, LabelType labelType, Int128 length)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(LabelType), labelType));
			Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
			#endregion

			this.identifier = identifier;
			this.labelType = labelType;
			this.length = length;
			this.associatedSymbol = new Symbol(this, labelType.ToSymbolType());
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

		private LabelType labelType;
		/// <summary>
		/// Gets or sets the type of symbol which this label defines.
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
		/// Gets the length of this <see cref="Label"/>.
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
		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The mutable <see cref="Context"/> in which the emittable will be constructed.</param>
		/// <returns>A list of constructed emittables; or an empty list.</returns>
		public override IList<IEmittable> Construct(Context context)
		{
			associatedSymbol.Address = context.Address;
			associatedSymbol.DefiningSection = context.Section;
			associatedSymbol.DefiningFile = context.Section.Parent;
			context.SymbolTable.Add(associatedSymbol);

			return new IEmittable[0];
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
			Contract.Invariant(Enum.IsDefined(typeof(LabelType), this.labelType));
			Contract.Invariant(this.associatedSymbol != null);
			Contract.Invariant(this.length >= 0);
		}
		#endregion
	}
}
