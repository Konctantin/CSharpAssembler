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

namespace SharpAssembler
{
	/// <summary>
	/// Specifies the size of the a data unit.
	/// </summary>
	public enum DataSize
	{
		/// <summary>
		/// No data size.
		/// </summary>
		None = 0,


		/// <summary>
		/// An 8-bit data unit.
		/// </summary>
		Bit8 = 8 >> 3,
		/// <summary>
		/// A 16-bit data unit.
		/// </summary>
		Bit16 = 16 >> 3,
		/// <summary>
		/// A 32-bit data unit.
		/// </summary>
		Bit32 = 32 >> 3,
		/// <summary>
		/// A 64-bit data unit.
		/// </summary>
		Bit64 = 64 >> 3,
		/// <summary>
		/// A 80-bit data unit.
		/// </summary>
		Bit80 = 80 >> 3,
		/// <summary>
		/// A 128-bit data unit.
		/// </summary>
		Bit128 = 128 >> 3,
		/// <summary>
		/// A 256-bit data unit.
		/// </summary>
		Bit256 = 256 >> 3,
	}


	/// <summary>
	/// Extensions for the <see cref="DataSize"/> type.
	/// </summary>
	public static class DataSizeExtensions
	{
		/// <summary>
		/// Returns the number of bits representing the specified data size.
		/// </summary>
		/// <param name="datasize">The <see cref="DataSize"/> to get the number of bits for.</param>
		/// <returns>The number of bits for the data size.</returns>
		public static int GetBitCount(this DataSize datasize)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), datasize));
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion
			return ((int)datasize << 3);
		}
	}
}
