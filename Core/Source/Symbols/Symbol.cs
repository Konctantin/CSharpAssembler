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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace SharpAssembler.Core.Symbols
{
	/// <summary>
	/// A symbol reference or definition.
	/// </summary>
	[Serializable]
	public sealed class Symbol : IAnnotatable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Symbol"/> class.
		/// </summary>
		/// <param name="association">The object to which this symbol is associated.</param>
		/// <param name="symbolType">The type of symbol.</param>
		/// <remarks>
		/// The value of the <see cref="Identifier"/> property reflects the identifier of the associated object.
		/// </remarks>
		public Symbol(IIdentifiable association, SymbolType symbolType)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(association != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SymbolType), symbolType));
			#endregion

			this.identifier = ((IIdentifiable)association).Identifier;
			this.association = association;
			this.symbolType = symbolType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Symbol"/> class.
		/// </summary>
		/// <param name="identifier">The identifier of the symbol.</param>
		/// <param name="association">The object to which this symbol is associated.</param>
		/// <param name="symbolType">The type of symbol.</param>
		public Symbol(string identifier, IAssociatable association, SymbolType symbolType)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<ArgumentNullException>(association != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SymbolType), symbolType));
			#endregion

			this.identifier = identifier;
			this.association = association;
			this.symbolType = symbolType;
		}
		#endregion

		#region Properties
		private string identifier;
		/// <summary>
		/// Gets the identifier of the symbol.
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
		}

		private IAssociatable association;
		/// <summary>
		/// Gets the label, block, section or file this <see cref="Symbol"/> is associated with.
		/// </summary>
		/// <value>An object implementing the <see cref="IAssociatable"/> interface.</value>
		public IAssociatable Association
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<IAssociatable>() != null);
				#endregion
				return association;
			}
		}

		private SymbolType symbolType;
		/// <summary>
		/// Gets or sets the type of symbol.
		/// </summary>
		/// <value>A member of the <see cref="SymbolType"/> enumeration.</value>
		public SymbolType SymbolType
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(SymbolType), Contract.Result<SymbolType>()));
				#endregion
				return symbolType;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SymbolType), value));
				#endregion
				symbolType = value;
			}
		}

		private IFile definingFile;
		/// <summary>
		/// Gets or sets the <see cref="IFile"/> in which this symbol is defined.
		/// </summary>
		/// <value>The <see cref="IFile"/> in which this symbol is defined; or <see langword="null"/>.</value>
		/// <remarks>
		/// When <see cref="DefiningSection"/> is not <see langword="null"/>, this member should be the
		/// <see cref="ObjectFile"/> in which the section is defined.
		/// </remarks>
		public IFile DefiningFile
		{
			get { return definingFile; }
			set { definingFile = value; }
		}

		private Section definingSection;
		/// <summary>
		/// Gets or sets the <see cref="Section"/> in which this symbol is
		/// defined.
		/// </summary>
		/// <value>The <see cref="Section"/> in which this symbol is defined; or <see langword="null"/> when
		/// <see cref="IsAbsolute"/> is <see langword="true"/> or when the symbol is not defined in this object file
		/// (i.e. it is extern, <see cref="IsExtern"/> is <see langword="true"/>).</value>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public Section DefiningSection
		{
			get { return definingSection; }
			set { definingSection = value; }
		}

		private bool isAbsolute;
		/// <summary>
		/// Gets or sets whether the address of this symbol is an absolute value.
		/// </summary>
		/// <value><see langword="true"/> when the symbol is defined and its <see cref="Address"/> is an absolute
		/// address; otherwise, <see langword="false"/>.</value>
		public bool IsAbsolute
		{
			get { return isAbsolute; }
			set { isAbsolute = value; }
		}

		/// <summary>
		/// Gets whether this symbol is defined in another file.
		/// </summary>
		/// <value><see langword="true"/> when the symbol is not defined in this file;
		/// otherwise, <see langword="false"/>.</value>
		public bool IsExtern
		{
			get { return definingSection == null && isAbsolute == false; }
		}

		private Int128 address;
		/// <summary>
		/// Gets or sets the address at which this symbol is defined.
		/// </summary>
		/// <value>An address, relative to the start of the section when <see cref="IsAbsolute"/> is
		/// <see langword="false"/>; or when <see cref="IsAbsolute"/> is <see langword="true"/>, relative to the start
		/// of memory. Otherwise, 0 when <see cref="IsExtern"/> is <see langword="true"/>.</value>
		public Int128 Address
		{
			get { return address; }
			set { address = value; }
		}

		private IDictionary annotations = new Hashtable();
		/// <summary>
		/// Gets a dictionary which may be used to store data specific to
		/// this object.
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
		/// Returns a <see cref="String"/> that represents the current <see cref="Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="String"/> that represents the current <see cref="Object"/>.
		/// </returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "<Symbol id=\"{0}\">", identifier);
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
			Contract.Invariant(this.association != null);
			Contract.Invariant(Enum.IsDefined(typeof(SymbolType), this.symbolType));
		}
		#endregion
	}
}
