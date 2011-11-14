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
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace SharpAssembler.Core.Instructions
{
	/// <summary>
	/// Emits padding bytes up to a specified boundary.
	/// </summary>
	public class Align : Constructable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Align"/> class.
		/// </summary>
		/// <param name="boundary">The boundary to align to. Must be a power of two.</param>
		public Align(int boundary)
			: this(boundary, 0)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(MathExt.IsPowerOfTwo(boundary));
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Align"/> class.
		/// </summary>
		/// <param name="boundary">The boundary to align to. Must be a power of two.</param>
		/// <param name="paddingbyte">The padding byte value used.</param>
		public Align(int boundary, byte paddingbyte)
		{
			#region Contract
			Contract.Requires<ArgumentOutOfRangeException>(boundary >= 1);
			Contract.Requires<ArgumentException>(MathExt.IsPowerOfTwo(boundary));
			#endregion

			this.boundary = boundary;
			this.paddingByte = paddingbyte;
		}
		#endregion

		#region Properties
		private int boundary;
		/// <summary>
		/// Gets or sets the boundary to which is aligned.
		/// </summary>
		/// <value>The boundary to align to, which must be a power of two.</value>
		public int Boundary
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<int>() >= 1);
				Contract.Ensures(MathExt.IsPowerOfTwo(Contract.Result<int>()));
				#endregion
				return boundary;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(value >= 1);
				Contract.Requires<ArgumentException>(MathExt.IsPowerOfTwo(value));
				#endregion
				this.boundary = value;
			}
#endif
		}

		private byte paddingByte;
		/// <summary>
		/// Gets or sets the byte value used to pad.
		/// </summary>
		/// <value>A byte value. The default is 0x00.</value>
		public byte PaddingByte
		{
			get { return paddingByte; }
#if OPERAND_SET
			set { paddingByte = value; }
#endif
		}
		#endregion

		#region Methods
		/// <summary>
		/// Generates an array of bytes which represent the padding bytes used for the align instruction.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the padding will be generated.</param>
		/// <param name="length">The length of the padding, in bytes.</param>
		/// <returns>A byte array which has a length of <paramref name="length"/>.</returns>
		/// <remarks>
		/// The default operation is to generate a sequence of bytes with the value <see cref="PaddingByte"/>.
		/// Architectures may provide their own implementation of this method, which may generate more appropriate
		/// padding sequences (depending on the processor used and other factors).
		/// </remarks>
		protected virtual byte[] GeneratePadding(Context context, int length)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
			#endregion

			byte[] paddingbytes = new byte[length];
			// Because an empty array is automatically initialized with 0x00 bytes,
			// we only need to extend the array with padding bytes for other values.
			if (paddingByte != 0x00)
			{
				for (int i = 0; i < paddingbytes.Length; i++)
					paddingbytes[i] = paddingByte;
			}
			return paddingbytes;
		}

		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The mutable <see cref="Context"/> in which the emittable will be constructed.</param>
		/// <returns>A list of constructed emittables; or an empty list.</returns>
		public override IList<IEmittable> Construct(Context context)
		{
			int padding = (int)context.Address.GetPadding(this.boundary);

			return new IEmittable[]{ new RawEmittable(GeneratePadding(context, padding)) };
		}
		#endregion

		#region Invariant
		/// <summary>
		/// The invariant method for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(boundary >= 1);
			Contract.Invariant(MathExt.IsPowerOfTwo(boundary));
		}
		#endregion
	}
}
