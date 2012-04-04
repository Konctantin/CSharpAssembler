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

namespace SharpAssembler
{
	/// <summary>
	/// Creates new <see cref="Section"/> objects.
	/// </summary>
	public class SectionFactory
	{
		/// <summary>
		/// Returns the default section identifier for a section of the specified type.
		/// </summary>
		/// <param name="type">The type of section.</param>
		/// <returns>The default section identifier for a section of the specified type.</returns>
		/// <remarks>
		/// The default implementation returns the following identifiers:
		/// <list type="table">
		/// <listheader><term>Section type</term><description>Identifier</description></listheader>
		/// <item><term><see cref="SharpAssembler.SectionType.Program"/></term>
		/// <description><c>.code</c></description></item>
		/// <item><term><see cref="SharpAssembler.SectionType.Data"/></term>
		/// <description><c>.data</c></description></item>
		/// <item><term><see cref="SharpAssembler.SectionType.Bss"/></term>
		/// <description><c>.bss</c></description></item>
		/// </list>
		/// </remarks>
		/// <exception cref="NotSupportedException">
		/// <paramref name="type"/> is not a section type that is supported.
		/// </exception>
		[Pure]
		public virtual string GetDefaultSectionIdentifier(SectionType type)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SectionType), type));
			Contract.Requires<ArgumentException>(type != SectionType.None);
			#endregion

			switch (type)
			{
				case SectionType.Program:
					return ".code";
				case SectionType.Data:
					return ".data";
				case SectionType.Bss:
					return ".bss";
				default:
					throw new NotSupportedException("The specified SectionType is not supported.");
			}
		}

		/// <summary>
		/// Creates a new <see cref="Section"/> of the specified section type,
		/// with the default identifier that is associated with that section.
		/// </summary>
		/// <param name="type">The type of section to create.</param>
		/// <returns>The created <see cref="Section"/>.</returns>
		/// <exception cref="NotSupportedException">
		/// <paramref name="type"/> is not a section type that is supported.
		/// </exception>
		[Pure]
		public Section CreateSection(SectionType type)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SectionType), type));
			Contract.Ensures(Contract.Result<Section>() != null);
			#endregion

			// THROWS: NotSupportedException
			var identifier = GetDefaultSectionIdentifier(type);
			// THROWS: NotSupportedException
			return CreateSection(type, identifier);
		}

		/// <summary>
		/// Creates a new <see cref="Section"/> of the specified section type.
		/// </summary>
		/// <param name="type">The type of section to create.</param>
		/// <param name="identifier">The identifier of the section.</param>
		/// <returns>The created <see cref="Section"/>.</returns>
		/// <exception cref="NotSupportedException">
		/// <paramref name="type"/> is not a section type that is supported.
		/// </exception>
		[Pure]
		public virtual Section CreateSection(SectionType type, string identifier)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(SectionType), type));
			Contract.Requires<ArgumentNullException>(identifier != null);
			Contract.Requires<ArgumentException>(IsValidIdentifier(identifier));
			Contract.Ensures(Contract.Result<Section>() != null);
			#endregion

			var section = new Section(identifier);

			switch (type)
			{
				case SectionType.None:
					break;
				case SectionType.Program:
					section.Allocate = true;
					section.Writable = false;
					section.Executable = true;
					section.NoBits = false;
					break;
				case SectionType.Data:
					section.Allocate = true;
					section.Writable = true;
					section.Executable = false;
					section.NoBits = false;
					break;
				case SectionType.Bss:
					section.Allocate = true;
					section.Writable = true;
					section.Executable = false;
					section.NoBits = true;
					break;
				default:
					throw new NotSupportedException("The specified SectionType is not supported.");
			}

			return section;
		}

		/// <summary>
		/// Returns whether the specified identifier is a valid section identifier.
		/// </summary>
		/// <param name="identifier">The identifier to test.</param>
		/// <returns><see langword="true"/> when the identifier is valid for the object file format;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public virtual bool IsValidIdentifier(string identifier)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(identifier != null);
			#endregion

			return true;
		}
	}
}
