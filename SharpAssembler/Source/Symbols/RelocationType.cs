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

namespace SharpAssembler.Symbols
{
	/// <summary>
	/// Specifies the type of relocation.
	/// </summary>
	public enum RelocationType
	{
		/// <summary>
		/// No relocation.
		/// </summary>
		None,
		/// <summary>
		/// The resulting value is calculated by adding the specified symbol's value to the addend.
		/// The resulting value is 32 bit.
		/// </summary>
		Default32,
		/// <summary>
		/// The resulting value is calculated by adding the specified symbol's value to the addend, and subtracting the
		/// place (section offset or address) of the storage unit being relocated (computed using r_offset).
		/// The resulting value is 32 bit.
		/// </summary>
		Pc32,
		/// <summary>
		/// This relocation type computes the distance from the base of the global offset table to the symbol's global
		/// offset table entry. The resulting value is calculated by adding the offset into the global offset table at
		/// which the address of the relocation entry's symbol will reside during execution to the addend, and
		/// subtracting the place (section offset or address) of the storage unit being relocated (computed using
		/// r_offset).
		/// The resulting value is 32 bit.
		/// </summary>
		Got32,
		/// <summary>
		/// This relocation type computes the address of the symbol's procedure linkage table entry. The resulting
		/// value is calculated by adding the place (section offset or address) of the procedure linkage table entry
		/// for a symbol to the addend, and subtracting the place (section offset or address) of the storage unit being
		/// relocated (computed using r_offset).
		/// The resulting value is 32 bit.
		/// </summary>
		Plt32,
		/// <summary>
		/// The link editor creates this relocation type for dynamic linking. Its offset member refers to a location in
		/// a writable segment. The symbol table index specifies a symbol that should exist both in the current object
		/// file and in a shared object. During execution, the dynamic linker copies data associated with the shared
		/// object's symbol to the location specified by the offset.
		/// </summary>
		Copy,
		/// <summary>
		/// This relocation type is used to set a global offset table entry to the address of the specified symbol.
		/// The resulting value is the specified symbol's value.
		/// The resulting value is 32 bit.
		/// </summary>
		GlobalData,
		/// <summary>
		/// The link editor creates this relocation type for dynamic linking. Its offset member gives the location of
		/// a procedure linkage table entry. The resulting value is the specified symbol's value.
		/// The resulting value is 32 bit.
		/// </summary>
		JumpSlot,
		/// <summary>
		/// The link editor creates this relocation type for dynamic linking. Its offset member gives a location within
		/// a shared object that contains a value representing a relative address. The resulting value is calculated by
		/// adding the base address at which a shared object has been loaded into memory during execution to the
		/// addend.
		/// The resulting value is 32 bit.
		/// </summary>
		Relative,
		/// <summary>
		/// This relocation type computes the difference between a symbol's value and the address of the global offset
		/// table. The resulting value is calculated by adding the specified symbol's value to the addend, and
		/// subtracting the address of the global offset table.
		/// The resulting value is 32 bit.
		/// </summary>
		GotOffset,
		/// <summary>
		/// The resulting value is calculated by adding the address of the global offset table to the addend, and
		/// subtracting the place (section offset or address) of the storage unit being relocated (computed using
		/// r_offset).
		/// The resulting value is 32 bit.
		/// </summary>
		GotPc,
	}
}
