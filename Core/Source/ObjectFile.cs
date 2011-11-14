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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using SharpAssembler.Core.Symbols;

namespace SharpAssembler.Core
{
	/// <summary>
	/// An object file, containing sections of code and data.
	/// </summary>
	[Serializable]
	[ContractClass(typeof(Contracts.ObjectFileContract))]
	public abstract class ObjectFile : Collection<Section>,
		IFile, IIdentifiable, IAnnotatable, IObjectFileVisitable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectFile"/> class with the specified name.
		/// </summary>
		/// <param name="name">The name of the object file.</param>
		/// <param name="architecture">The architecture.</param>
		protected ObjectFile(string name, IArchitecture architecture)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentNullException>(architecture != null);
			if (!IsSupportedArchitecture(architecture))
				throw new NotSupportedException("The specified architecture must be supported " +
					"by this object file format.");
			#endregion

			this.name = name;
			this.architecture = architecture;
			this.associatedSymbol = new Symbol(this, SymbolType.None);
		}
		#endregion

		#region Properties
		private string name;
		/// <summary>
		/// Gets or sets the name of the object file.
		/// </summary>
		/// <value>The name of the object file.</value>
		/// <remarks>
		/// There are no restrictions on the names which are allowed. Implementations using this property should
		/// ensure that the name conforms to any restrictions which may apply, such as file naming restrictions.
		/// </remarks>
		public virtual string Name
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return name;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				name = value;
			}
		}

		/// <summary>
		/// Gets the identifier of this <see cref="IIdentifiable"/>.
		/// </summary>
		/// <value>An identifier.</value>
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		string IIdentifiable.Identifier
		{
			get { return name; }
		}

		private Symbol associatedSymbol;
		/// <summary>
		/// Gets the <see cref="Symbol"/> associated with this block.
		/// </summary>
		/// <value>A <see cref="Symbol"/>.</value>
		public Symbol AssociatedSymbol
		{
			get { return associatedSymbol; }
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
		/// Assembles the object file and writes the resulting binary to the specified <see cref="BinaryWriter"/>.
		/// </summary>
		/// <param name="writer">A <see cref="BinaryWriter"/> object.</param>
		public abstract void Assemble(BinaryWriter writer);

		/// <summary>
		/// Returns whether this <see cref="ObjectFile"/> can contain binary data for the specified architecture.
		/// </summary>
		/// <param name="architecture">The <see cref="IArchitecture"/> to test.</param>
		/// <returns><see langword="true"/> when this <see cref="ObjectFile"/> can contain binary data for
		/// <paramref name="architecture"/>; otherwise, <see langword="false"/>.</returns>
		[Pure]
		public abstract bool IsSupportedArchitecture(IArchitecture architecture);

		/// <summary>
		/// Checks whether the object file format supports the specified feature.
		/// </summary>
		/// <param name="feature">The <see cref="ObjectFileFeature"/> to test.</param>
		/// <returns><see langword="true"/> when it is supported; otherwise, <see langword="false"/>.</returns>
		[Pure]
		public virtual bool SupportsFeature(ObjectFileFeature feature)
		{
			return false;
		}

		/// <summary>
		/// Returns whether the specified identifier is valid for this object file.
		/// </summary>
		/// <param name="identifier">The identifier to test.</param>
		/// <returns><see langword="true"/> when the identifier is valid for this object file;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public virtual bool IsValidIdentifier(string identifier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			#endregion

			return true;
		}

		/// <summary>
		/// Creates a new <see cref="Section"/> and adds it to the <see cref="ObjectFile"/>.
		/// </summary>
		/// <param name="identifier">The identifier of the section.</param>
		/// <param name="type">The type of section to create.</param>
		/// <returns>The created <see cref="Section"/>.</returns>
		public virtual Section AddNewSection(string identifier, SectionType type)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<ArgumentException>(IsValidIdentifier(identifier),
				"The identifier is not valid or reserved.");
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SectionType), type));
			#endregion

			Section s = new Section(identifier);

			switch (type)
			{
				case SectionType.None:
					break;
				case SectionType.Program:
					s.Allocate = true;
					s.Writable = false;
					s.Executable = true;
					s.NoBits = false;
					break;
				case SectionType.Data:
					s.Allocate = true;
					s.Writable = true;
					s.Executable = false;
					s.NoBits = false;
					break;
				case SectionType.Bss:
					s.Allocate = true;
					s.Writable = true;
					s.Executable = false;
					s.NoBits = true;
					break;
				default:
					throw new InvalidEnumArgumentException("type", (int)type, typeof(SectionType));
			}

			this.Add(s);
			return s;
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The <see cref="IObjectFileVisitor"/> visiting.</param>
		public void Accept(IObjectFileVisitor visitor)
		{
			visitor.VisitObjectFile(this);
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="Object"/>.
		/// </summary>
		/// <returns>A <see cref="String"/> that represents the current <see cref="Object"/>.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "<ObjectFile Name=\"{0}\">", name);
		}
		#endregion

		#region Indexers
		/// <summary>
		/// Gets the <see cref="Section"/> with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier of the section.</param>
		/// <value>The <see cref="Section"/>; or <see langword="null"/> when not found.</value>
		public Section this[string identifier]
		{
			get
			{
				return (from section in this
						 where String.Compare(section.Identifier, identifier, CultureInfo.InvariantCulture, CompareOptions.None) == 0
						 select section).FirstOrDefault();
			}
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

		/// <summary>
		/// Removes all elements from the
		/// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
		/// </summary>
		protected sealed override void ClearItems()
		{
			foreach (Section s in this)
			{
				if (s.Parent == this)
					s.Parent = null;
			}

			base.ClearItems();
		}

		/// <summary>
		/// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified
		/// index.
		/// </summary>
		/// <param name="index">The zero-based index at which
		/// <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		protected sealed override void InsertItem(int index, Section item)
		{
			#region Contract
			// CONTRACT: Collection<T>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion

			if (item.Parent != null)
				item.Parent.Remove(item);
			base.InsertItem(index, item);
			item.Parent = this;
		}

		/// <summary>
		/// Removes the element at the specified index of the
		/// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		protected sealed override void RemoveItem(int index)
		{
			// CONTRACT: Collection<T>

			if (this[index].Parent == this)
				this[index].Parent = null;

			base.RemoveItem(index);
		}

		/// <summary>
		/// Replaces the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to replace.</param>
		/// <param name="item">The new value for the element at the specified index.</param>
		protected sealed override void SetItem(int index, Section item)
		{
			#region Contract
			// CONTRACT: Collection<T>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion

			if (this[index].Parent == this)
				this[index].Parent = null;
			if (item.Parent != null)
				item.Parent.Remove(item);
			base.SetItem(index, item);
			item.Parent = this;
		}
		#endregion
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="ObjectFile"/> classs.
		/// </summary>
		[ContractClassFor(typeof(ObjectFile))]
		abstract class ObjectFileContract : ObjectFile
		{
			private ObjectFileContract() : base(null, null) { }

			public override void Assemble(BinaryWriter writer)
			{
				Contract.Requires<ArgumentNullException>(writer != null);
			}

			public override bool IsSupportedArchitecture(IArchitecture architecture)
			{
				Contract.Requires<ArgumentNullException>(architecture != null);

				return default(bool);
			}
		}
	}
	#endregion
}
