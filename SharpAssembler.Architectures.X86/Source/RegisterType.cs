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
using SharpAssembler;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Specifies a type of register.
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// With some simple operations, it is possible to derive some special features from the values of the members of
	/// this enumeration.
	/// <listheader><term>Operation</term><description>Result</description></listheader>
	/// <item><term>value &amp; 0xFF</term><description>The number of bytes required for this type of
	/// register.</description></item>
	/// <item><term>(value &amp; 0xFF) &lt;&lt; 3</term><description>The <see cref="DataSize"/> of this type of
	/// register.</description></item>
	/// </list>
	/// </remarks>
	public enum RegisterType
	{
		/// <summary>
		/// No register type.
		/// </summary>
		None = 0,
#if false
		/// <summary>
		/// A general purpose register.
		/// </summary>
		GeneralPurpose = GeneralPurpose8Bit | GeneralPurpose16Bit | GeneralPurpose32Bit | GeneralPurpose64Bit,
#endif
		/// <summary>
		/// An 8-bit general purpose register.
		/// </summary>
		GeneralPurpose8Bit = 0x100 | DataSize.Bit8,
		/// <summary>
		/// A 16-bit general purpose register.
		/// </summary>
		GeneralPurpose16Bit = 0x100 | DataSize.Bit16,
		/// <summary>
		/// A 32-bit general purpose register.
		/// </summary>
		GeneralPurpose32Bit = 0x100 | DataSize.Bit32,
		/// <summary>
		/// A 64-bit general purpose register.
		/// </summary>
		GeneralPurpose64Bit = 0x100 | DataSize.Bit64,
		/// <summary>
		/// A 80-bit floating point register.
		/// </summary>
		FloatingPoint = 0x200 | DataSize.Bit80,
		/// <summary>
		/// A 64-bit MMX register.
		/// </summary>
		Simd64Bit = 0x400 | DataSize.Bit64,
		/// <summary>
		/// A 128-bit XMM register.
		/// </summary>
		Simd128Bit = 0x800 | DataSize.Bit128,
		/// <summary>
		/// A 16-bit segment register.
		/// </summary>
		Segment = 0x1000 | DataSize.Bit16,
		/// <summary>
		/// A 32-bit control register.
		/// </summary>
		Control = 0x2000 | DataSize.Bit32,
		/// <summary>
		/// A 32-bit debug register.
		/// </summary>
		Debug = 0x4000 | DataSize.Bit32,
	}




	/// <summary>
	/// Extensions for the <see cref="RegisterType"/> type.
	/// </summary>
	public static class RegisterTypeExtensions
	{
		/// <summary>
		/// Returns the register size of the specified register type.
		/// </summary>
		/// <param name="register">The <see cref="RegisterType"/> to get the size for.</param>
		/// <returns>The <see cref="DataSize"/> of the register.</returns>
		public static DataSize GetSize(this RegisterType register)
		{
			#region Contract
			//Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(RegisterType), register));
#if false
			Contract.Requires<ArgumentException>(register != RegisterType.GeneralPurpose);
#endif
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			if (!Enum.IsDefined(typeof(RegisterType), register))
				return DataSize.None;

			int size = ((int)register & 0x3F);
			return (DataSize)size;
		}
	}
}
