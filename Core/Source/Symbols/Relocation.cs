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
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace SharpAssembler.Core.Symbols
{
	/// <summary>
	/// Specifies a single relocation.
	/// </summary>
	public sealed class Relocation : IAnnotatable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Relocation"/> class.
		/// </summary>
		/// <param name="symbol">The target symbol.</param>
		/// <param name="section">The section in which the storage unit to be relocated resides.</param>
		/// <param name="offset">The offset relative to the start of <paramref name="section"/> at which the storage
		/// unit to be relocated resides.</param>
		/// <param name="addend">The constant used to compute the value of the relocatable field.</param>
		/// <param name="type">The type of relocation compution to perform.</param>
		public Relocation(Symbol symbol, Section section, Int128 offset, Int128 addend, RelocationType type)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(symbol != null);
			Contract.Requires<ArgumentNullException>(section != null);
			#endregion

			this.targetSymbol = symbol;
			this.section = section;
			this.offset = offset;
			this.addend = addend;
			this.type = type;
		}
		#endregion

		#region Properties
		private Symbol targetSymbol;
		/// <summary>
		/// Gets or sets the target symbol of this relocation.
		/// </summary>
		/// <value>A <see cref="Symbol"/>.</value>
		/// <remarks>
		/// When the target symbol is a defined symbol (as opposed to a symbol which is not defined in this object
		/// file), the resulting relocation (as encoded by the file format) may have a value which equals the offset
		/// of the symbol relative to it's section, and a symbol specifier which targets that section.
		/// </remarks>
		public Symbol TargetSymbol
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Symbol>() != null);
				#endregion
				return targetSymbol;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				targetSymbol = value;
			}
		}

		private Section section;
		/// <summary>
		/// Gets or sets the <see cref="Section"/> in which the storage unit to be relocated resides.
		/// </summary>
		/// <value>The <see cref="Section"/> containing the storage unit.</value>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public Section Section
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Section>() != null);
				#endregion
				return section;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				section = value;
			}
		}

		private Int128 offset;
		/// <summary>
		/// Gets or sets the offset of the storage unit to be relocated, relative to the start of the section
		/// containing the storage unit.
		/// </summary>
		/// <value>The offset of the storage unit relative to the start of <see cref="Section"/>.</value>
		public Int128 Offset
		{
			get { return offset; }
			set { offset = value; }
		}

#if false
		private int length;
		/// <summary>
		/// Gets or sets the length of the storage unit to be relocated.
		/// </summary>
		/// <value>The length of the storage unit, in bytes.</value>
		/// <remarks>Not all lengths may be usable with all file
		/// formats.</remarks>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The value is negative or zero.
		/// </exception>
		public int Length
		{
			get { return length; }
			set
			{
		#region Contract
				if (value <= 0) throw new ArgumentOutOfRangeException("value");
				#endregion
				length = value;
			}
		}
#endif

		private Int128 addend;
		/// <summary>
		/// Gets or sets a constant addend used to compute the value to be stored into the relocatable field.
		/// </summary>
		/// <value>The addend.</value>
		public Int128 Addend
		{
			get { return addend; }
			set { addend = value; }
		}

		private RelocationType type;
		/// <summary>
		/// Gets or sets the relocation type.
		/// </summary>
		/// <value>A member of the <see cref="RelocationType"/> enumeration.</value>
		/// <remarks>
		/// There are no (contract) restrictions on the relocation type.
		/// </remarks>
		public RelocationType Type
		{
			get { return type; }
			set { type = value; }
		}

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
		#endregion

		#region Methods
		/// <summary>
		/// Returns a <see cref="String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format(
				CultureInfo.InvariantCulture,
				"<Relocation [{0} + 0x{1:X} -> {3}>",
				section.Identifier,
				offset,
				//length, "({2} bytes)]"
				targetSymbol.Identifier);
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.targetSymbol != null);
			Contract.Invariant(this.section != null);
		}
		#endregion
	}
}
