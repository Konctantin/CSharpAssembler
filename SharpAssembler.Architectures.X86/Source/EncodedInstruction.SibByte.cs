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

namespace SharpAssembler.Architectures.X86
{
	partial class EncodedInstruction
	{
		/// <summary>
		/// Represents the SIB byte.
		/// </summary>
		public class SibByte
		{
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the
			/// <see cref="SibByte"/> class.
			/// </summary>
			public SibByte()
			{ }

			/// <summary>
			/// Initializes a new instance of the
			/// <see cref="SibByte"/> class.
			/// </summary>
			/// <param name="baseReg">The 4-bit BASE part.</param>
			/// <param name="index">The 4-bit INDEX part.</param>
			/// <param name="scale">The 2-bit SCALE part.</param>
			public SibByte(byte baseReg, byte index, byte scale)
			{
				this.@base = (byte)(baseReg & 0x0F);
				this.index = (byte)(index & 0x0F);
				this.scale = (byte)(scale & 0x03);
			}
			#endregion

			#region Properties
			private byte @base;
			/// <summary>
			/// Gets or sets the value of the BASE part of the SIB byte.
			/// </summary>
			/// <value>The 4-bit BASE value.</value>
			/// <remarks>
			/// The least significant three bits are encoded in the SIB byte, while the fourth bit is encoded as the
			/// REX.B bit when there is a SIB byte.
			/// </remarks>
			public byte Base
			{
				get
				{
					Contract.Ensures(Contract.Result<byte>() <= 0x0F,
						"Only the first 4 bits may be set.");
					return @base;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentOutOfRangeException>(value <= 0x0F,
						"Only the first 4 bits may be set.");
					#endregion
					@base = value;
				}
			}

			private byte index;
			/// <summary>
			/// Gets or sets the value of the INDEX part of the SIB byte.
			/// </summary>
			/// <value>The 4-bit INDEX value.</value>
			/// <remarks>
			/// The least significant three bits are encoded in the SIB byte, while the fourth bit is encoded in the
			/// REX.X bit.
			/// </remarks>
			public byte Index
			{
				get
				{
					Contract.Ensures(Contract.Result<byte>() <= 0x0F,
						"Only the first 4 bits may be set.");
					return index;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentOutOfRangeException>(value <= 0x0F,
						"Only the first 4 bits may be set.");
					#endregion
					index = value;
				}
			}

			private byte scale;
			/// <summary>
			/// Gets or sets the value of the SCALE part of the SIB byte.
			/// </summary>
			/// <value>The 2-bit SCALE value.</value>
			/// <remarks>
			/// The actual scale is two to the power of this value, so that 0 equals a scale of 1, 1 equals a scale of
			/// 2, 2 equals a scale of 4 and 3 equals a scale of 8.
			/// </remarks>
			public byte Scale
			{
				get
				{
					Contract.Ensures(Contract.Result<byte>() <= 0x03,
						"Only the first 2 bits may be set.");
					return scale;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentOutOfRangeException>(value <= 0x03,
						"Only the first 2 bits may be set.");
					#endregion
					scale = value;
				}
			}
			#endregion

			#region Methods
			/// <summary>
			/// Returns a byte array representation of this sub structure.
			/// </summary>
			/// <returns>A byte array.</returns>
			public byte[] ToBytes()
			{
				byte result = 0x00;
				result |= (byte)((@base & 0x07));
				result |= (byte)((index & 0x07) << 3);
				result |= (byte)((scale & 0x03) << 6);
				return new byte[] { result };
			}
			#endregion

			#region Invariant
			/// <summary>
			/// Asserts the invariants of this type.
			/// </summary>
			[ContractInvariantMethod]
			private void ObjectInvariant()
			{
				Contract.Invariant(this.@base <= 0x0F);
				Contract.Invariant(this.index <= 0x0F);
				Contract.Invariant(this.scale <= 0x03);
			}
			#endregion
		}
	}
}
