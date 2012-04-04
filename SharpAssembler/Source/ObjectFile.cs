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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using SharpAssembler.Symbols;

namespace SharpAssembler
{
	/// <summary>
	/// An object file, containing sections of code and data.
	/// </summary>
	public class ObjectFile
		: IFile, IAssociatable, IAnnotatable, IObjectFileVisitable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectFile"/> class with the specified name.
		/// </summary>
		/// <param name="format">The format of the object file.</param>
		/// <param name="architecture">The architecture.</param>
		/// <param name="name">The name of the object file; or <see langword="null"/> to specify no name.</param>
		public ObjectFile(IObjectFileFormat format, IArchitecture architecture, string name)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(format != null);
			Contract.Requires<ArgumentNullException>(architecture != null);
			Contract.Requires<ArgumentException>(format.IsSupportedArchitecture(architecture));
			#endregion

			this.format = format;
			this.architecture = architecture;
			this.sections = new SectionCollection(this);
			this.AssociatedSymbol = new Symbol(SymbolType.None, name);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the object file.
		/// </summary>
		/// <value>The name of the object file; or <see langword="null"/> when no name is specified.</value>
		/// <remarks>
		/// There are no restrictions on the names which are allowed. Implementations using this property should
		/// ensure that the name conforms to any restrictions which may apply, such as file naming restrictions.
		/// </remarks>
		public string Name
		{
			get { return this.associatedSymbol.Identifier; }
			set { this.associatedSymbol.Identifier = value; }
		}

		private Symbol associatedSymbol;
		/// <inheritdoc />
		public Symbol AssociatedSymbol
		{
			get { return this.associatedSymbol; }
			set { Symbol.SetAssociation(this, value); }
		}

		private IArchitecture architecture;
		/// <summary>
		/// Gets the architecture for which this object file was created.
		/// </summary>
		/// <value>An <see cref="IArchitecture"/>.</value>
		public IArchitecture Architecture
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<IArchitecture>() != null);
				#endregion
				return architecture;
			}
		}

		private readonly IObjectFileFormat format;
		/// <summary>
		/// Gets the object file format of this object file.
		/// </summary>
		/// <value>An <see cref="IObjectFileFormat"/>.</value>
		public IObjectFileFormat Format
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<IObjectFileFormat>() != null);
				#endregion
				return this.format;
			}
		}

		private readonly SectionCollection sections;
		/// <summary>
		/// Gets a collection of sections in this object file.
		/// </summary>
		/// <value>A <see cref="SectionCollection"/>.</value>
		public SectionCollection Sections
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<SectionCollection>() != null);
				#endregion
				return this.sections;
			}
		}

		[NonSerialized]
		private IDictionary annotations = new Hashtable();
		/// <inheritdoc />
		public IDictionary Annotations
		{
			get { return annotations; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The <see cref="IObjectFileVisitor"/> visiting.</param>
		public void Accept(IObjectFileVisitor visitor)
		{
			visitor.VisitObjectFile(this);
		}

		/// <inheritdoc />
		void IAssociatable.SetAssociatedSymbol(Symbol symbol)
		{
			this.associatedSymbol = symbol;
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="Object"/>.
		/// </summary>
		/// <returns>A <see cref="String"/> that represents the current <see cref="Object"/>.</returns>
		public override string ToString()
		{
			return this.associatedSymbol.Identifier ?? base.ToString();
		}
		#endregion

		#region Hierarchy
		/// <summary>
		/// Gets the <see cref="IFile"/> in which this
		/// <see cref="Section"/> is defined.
		/// </summary>
		/// <value>A <see cref="IFile"/>.</value>
		IFile IAssociatable.ParentFile
		{
			get { return this; }
		}
		#endregion
	}
}
