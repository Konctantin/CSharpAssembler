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
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// A collection of <see cref="Section"/> objects.
	/// </summary>
	public class SectionCollection : KeyedCollection<String, Section>
	{
		/// <summary>
		/// The owner of the collection.
		/// </summary>
		private readonly ObjectFile owner;

		/// <summary>
		/// Gets the program code section.
		/// </summary>
		/// <value>The <see cref="Section"/> with the identifier <c>.text</c>;
		/// or <see langword="null"/> when there is no such section.</value>
		public Section Text
		{
			get
			{
				Section section;
				TryGetValue(".text", out section);
				return section;
			}
		}

		/// <summary>
		/// Gets the initialized data section.
		/// </summary>
		/// <value>The <see cref="Section"/> with the identifier <c>.data</c>;
		/// or <see langword="null"/> when there is no such section.</value>
		public Section Data
		{
			get
			{
				Section section;
				TryGetValue(".data", out section);
				return section;
			}
		}

		/// <summary>
		/// Gets the uninitialized data section.
		/// </summary>
		/// <value>The <see cref="Section"/> with the identifier <c>.bss</c>;
		/// or <see langword="null"/> when there is no such section.</value>
		public Section Bss
		{
			get
			{
				Section section;
				TryGetValue(".bss", out section);
				return section;
			}
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SectionCollection"/> class.
		/// </summary>
		/// <param name="owner">The owner of the collection.</param>
		internal SectionCollection(ObjectFile owner)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(owner != null);
			#endregion

			this.owner = owner;
		}
		#endregion

		/// <summary>
		/// Adds a new section with the specified type and the default identifier to the section collection.
		/// </summary>
		/// <param name="type">The type of the section.</param>
		/// <returns>The section that was created and added.</returns>
		/// <exception cref="NotSupportedException">
		/// <paramref name="type"/> is not a section type that is supported.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// A section with the default identifier is already present in this collection.
		/// </exception>
		public Section AddNew(SectionType type)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SectionType), type));
			Contract.Ensures(Contract.Result<Section>() != null);
			#endregion
			// THROWS: NotSupportedException
			var section = this.owner.Format.SectionFactory.CreateSection(type);
			// THROWS: ArgumentException
			this.Add(section);

			return section;
		}

		/// <summary>
		/// Adds a new section with the specified type and identifier to the section collection.
		/// </summary>
		/// <param name="type">The type of the section.</param>
		/// <param name="identifier">The identifier of the section.</param>
		/// <returns>The section that was created and added.</returns>
		/// <exception cref="NotSupportedException">
		/// <paramref name="type"/> is not a section type that is supported.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// A section with the specified identifier is already present in this collection.
		/// </exception>
		public Section AddNew(SectionType type, string identifier)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SectionType), type));
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Ensures(Contract.Result<Section>() != null);
			#endregion
			// THROWS: NotSupportedException
			var section = this.owner.Format.SectionFactory.CreateSection(type, identifier);
			// THROWS: ArgumentException
			this.Add(section);

			return section;
		}

		/// <summary>
		/// Attempts to return the <see cref="Section"/> with the specified identifier.
		/// </summary>
		/// <param name="identifier">The identifier of the section.</param>
		/// <param name="value">The <see cref="Section"/>, if found; otherwise, <see langword="null"/>.</param>
		/// <returns><see langword="true"/> when the section with the specified identifier was found;
		/// otherwise, <see langword="false"/>.</returns>
		public bool TryGetValue(string identifier, out Section value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			#endregion
			value = null;
			if (!this.Contains(identifier))
				return false;
			value = this[identifier];
			return true;
		}

		/// <inhertidoc />
		protected override string GetKeyForItem(Section item)
		{
			#region Contract
			// CONTRACT: KeyedCollection<TKey, TValue>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion
			return item.Identifier;
		}

		/// <inheritdoc />
		protected sealed override void ClearItems()
		{
			foreach (var section in this)
			{
				if (section.Parent == this.owner)
					section.Parent = null;
			}

			base.ClearItems();
		}

		/// <inheritdoc />
		protected sealed override void InsertItem(int index, Section item)
		{
			#region Contract
			// CONTRACT: Collection<T>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion

			if (item.Parent != null)
				item.Parent.Sections.Remove(item);
			base.InsertItem(index, item);
			item.Parent = this.owner;
		}

		/// <inheritdoc />
		protected sealed override void RemoveItem(int index)
		{
			// CONTRACT: Collection<T>

			if (this[index].Parent == this.owner)
				this[index].Parent = null;

			base.RemoveItem(index);
		}

		/// <inheritdoc />
		protected sealed override void SetItem(int index, Section item)
		{
			#region Contract
			// CONTRACT: Collection<T>
			if (item == null)
				throw new ArgumentNullException("item");
			#endregion

			if (this[index].Parent == this.owner)
				this[index].Parent = null;
			if (item.Parent != null)
				item.Parent.Sections.Remove(item);
			base.SetItem(index, item);
			item.Parent = this.owner;
		}

		#region Invariants
		/// <summary>
		/// Asserts the invariants on this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.owner != null);
		}
		#endregion
	}
}
