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
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using SharpAssembler.Symbols;

namespace SharpAssembler
{
	/// <summary>
	/// A class for architecture-specific assembler context information.
	/// </summary>
	/// <remarks>
	/// An instance of the <see cref="Context"/> class (or a derived class) is created at the start of the assembling,
	/// and updated as the instructions are processed.
	/// </remarks>
	public class Context
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="representation">The representation being assembled.</param>
		public Context(ObjectFile representation)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(representation != null);
			#endregion

			this.representation = representation;
			this.symbolTable = new SymbolTable();
			this.relocationTable = new Collection<Relocation>();
		}
		#endregion

		#region Properties
		private ObjectFile representation;
		/// <summary>
		/// Gets the <see cref="ObjectFile"/> representation being assembled.
		/// </summary>
		/// <value>An <see cref="ObjectFile"/>.</value>
		public ObjectFile Representation
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<ObjectFile>() != null);
				#endregion
				return representation;
			}
		}

		private Section section;
		/// <summary>
		/// Gets or sets the <see cref="Section"/> currently being processed.
		/// </summary>
		/// <value>A <see cref="Section"/>; or <see langword="null"/> when no section is currently being
		/// processed.</value>
		public Section Section
		{
			get { return section; }
			set { section = value; }
		}

		private Int128 address;
		/// <summary>
		/// Gets or sets the current virtual address.
		/// </summary>
		/// <value>The current virtual address.</value>
		public Int128 Address
		{
			get { return address; }
			set { address = value; }
		}

		private Collection<Relocation> relocationTable;
		/// <summary>
		/// Gets a collection of relocations in the current context.
		/// </summary>
		/// <value>A <see cref="Collection{Relocation}"/> object.</value>
		public Collection<Relocation> RelocationTable
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Collection<Relocation>>() != null);
				#endregion
				return relocationTable;
			}
		}

		private SymbolTable symbolTable;
		/// <summary>
		/// Gets a dictionary of defined symbols in the current context.
		/// </summary>
		/// <value>A <see cref="SymbolTable"/> object.</value>
		public SymbolTable SymbolTable
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<SymbolTable>() != null);
				#endregion
				return symbolTable;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Resets this context without clearing the symbol or relocation tables.
		/// </summary>
		/// <remarks>
		/// The context's address is reset to zero, the curent section is set to <see langword="null"/>. Derived
		/// classes may override this method and reset other fields and properties. The symbol and relocation tables
		/// are never cleared.
		/// </remarks>
		public virtual void Reset()
		{
			this.address = 0;
			this.section = null;
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.representation != null);
			Contract.Invariant(this.relocationTable != null);
			Contract.Invariant(this.symbolTable != null);
		}
		#endregion
	}
}
