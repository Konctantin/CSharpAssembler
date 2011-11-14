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
using System.Diagnostics.CodeAnalysis;

namespace SharpAssembler.Core
{
	/// <summary>
	/// Specifies flags which apply to a <see cref="Section"/>.
	/// </summary>
	[Flags]
	[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
	public enum SectionFlags
	{
		/// <summary>
		/// No flags are specified.
		/// </summary>
		None = 0x00,
		/// <summary>
		/// The contents of this section is copied from the object file to
		/// memory on execution.
		/// </summary>
		Allocated = 0x01,
		/// <summary>
		/// The contents of this section requires writing permission.
		/// </summary>
		Writable = 0x02,
		/// <summary>
		/// The contents of this section requires execute permission.
		/// </summary>
		Executable = 0x04,
		/// <summary>
		/// The contents of this section is zeroed. It is assumed to be
		/// present in memory on execution, but is not written in the file.
		/// </summary>
		Virtual = 0x08,
	}

	/// <summary>
	/// Extensions to the <see cref="SectionFlags"/> enumeration.
	/// </summary>
	public static class SectionFlagsExtensions
	{
		/// <summary>
		/// Sets or clears the specified flags in an enum, and returns the result.
		/// </summary>
		/// <param name="value">The value to change.</param>
		/// <param name="flag">The flags to set or clear.</param>
		/// <param name="set"><see langword="true"/> to set the flags; <see langword="false"/> to clear them.</param>
		public static SectionFlags SetFlag(this SectionFlags value, SectionFlags flag, bool set)
		{
			if (set)
				return (SectionFlags)(((uint)value) | ((uint)flag));
			else
				return (SectionFlags)(((uint)value) & ~((uint)flag));
		}
	}
}
