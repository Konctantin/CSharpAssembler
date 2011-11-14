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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace SharpAssembler.Core.Instructions
{
	/// <summary>
	/// Declares data.
	/// </summary>
	/// <typeparam name="T">The type of the declared data.</typeparam>
	public class DeclareData<T> : Constructable
		where T : struct
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData{T}"/> class.
		/// </summary>
		public DeclareData()
		{
			this.data = new List<T>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData{T}"/> class.
		/// </summary>
		/// <param name="data">The data to declare.</param>
		public DeclareData(T data)
		{
			this.data = new List<T>();
			this.data.Add(data);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData{T}"/> class.
		/// </summary>
		/// <param name="data">The data to declare.</param>
		public DeclareData(params T[] data)
		{
			this.data = new List<T>(data);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData{T}"/> class.
		/// </summary>
		/// <param name="data">An array of structures representing the data to declare.</param>
		public DeclareData(IEnumerable<T> data)
		{
			this.data = new List<T>(data);
		}
		#endregion

		#region Properties
		private IList<T> data;
		/// <summary>
		/// Gets the data that will be declared.
		/// </summary>
		/// <value>A <see cref="IList{T}"/> of structures.</value>
		public IList<T> Data
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<IList<T>>() != null);
				#endregion
				return data;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The mutable <see cref="Context"/> in which the emittable will be constructed.</param>
		/// <returns>A list of constructed emittables; or an empty list.</returns>
		public override IList<IEmittable> Construct(Context context)
		{
			// CONTRACT: Constructable

			int totallength = 0;
			foreach (T value in data)
			{
				totallength += GetSize(value);
			}


			byte[] databytes = new byte[totallength];
			int offset = 0;
			foreach (T value in data)
			{
				Contract.Assume(offset >= 0);
				offset += CopyBytes(value, databytes, offset);
			}

			return new IEmittable[] { new RawEmittable(databytes) };
		}

		/// <summary>
		/// Gets the size of the specified object.
		/// </summary>
		/// <param name="value">The object whose size to determine.</param>
		/// <returns>The size, in bytes.</returns>
		[Pure]
		private int GetSize(T value)
		{
			return Marshal.SizeOf(value);
		}

		/// <summary>
		/// Copies the byte representation of the value to the specified array at the specified location.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="array">The array to copy the representation to.</param>
		/// <param name="offset">The offset in <paramref name="array"/> where to start copying.</param>
		/// <returns>The number of bytes copied.</returns>
		private int CopyBytes(T value, byte[] array, int offset)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(array != null);
			Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
			Contract.Requires<ArgumentException>(array.Length - offset >= GetSize(value),
				"The target array must be big enough.");
			#endregion

			int length = GetSize(value);
			IntPtr ptr = Marshal.AllocHGlobal(length);
			Marshal.StructureToPtr(value, ptr, true);
			Marshal.Copy(ptr, array, offset, length);
			Marshal.FreeHGlobal(ptr);
			return length;
		}
		#endregion

		#region Invariant
		/// <summary>
		/// The invariant method for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.data != null);
		}
		#endregion
	}
}
