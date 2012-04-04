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
using System.Diagnostics.Contracts;

namespace SharpAssembler.Architectures.X86
{
	partial class EncodedInstruction
	{
		/// <summary>
		/// Represents the ModR/M byte.
		/// </summary>
		public class ModRMByte : SubStructure
		{
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="ModRMByte"/> class.
			/// </summary>
			public ModRMByte()
			{ }

			/// <summary>
			/// Initializes a new instance of the <see cref="ModRMByte"/> class.
			/// </summary>
			/// <param name="rm">The 4-bit R/M part.</param>
			/// <param name="reg">The 4-bit REG part.</param>
			/// <param name="mod">The 2-bit MOD part.</param>
			public ModRMByte(byte rm, byte reg, byte mod)
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(rm <= 0x0F,
					"Only the first 4 bits may be set.");
				Contract.Requires<ArgumentOutOfRangeException>(reg <= 0x0F,
					"Only the first 4 bits may be set.");
				Contract.Requires<ArgumentOutOfRangeException>(mod <= 0x03,
					"Only the first 2 bits may be set.");
				#endregion
				this.rm = rm;
				this.reg = reg;
				this.mod = mod;
			}
			#endregion

			#region Properties
			private byte rm;
			/// <summary>
			/// Gets or sets the value of the R/M part of the ModR/M byte.
			/// </summary>
			/// <value>The 4-bit R/M value.</value>
			/// <remarks>
			/// The least significant three bits are encoded in the ModR/M byte, while the fourth bit is encoded as the
			/// REX.B bit when there is no SIB byte.
			/// </remarks>
			public byte RM
			{
				get
				{
					#region Contract
					Contract.Ensures(Contract.Result<byte>() <= 0x0F,
						"Only the first 4 bits may be set.");
					#endregion
					return rm;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentOutOfRangeException>(value <= 0x0F,
						"Only the first 4 bits may be set.");
					#endregion
					rm = value;
				}
			}

			private byte reg;
			/// <summary>
			/// Gets or sets the value of the REG part of the ModR/M byte.
			/// </summary>
			/// <value>The 4-bit REG value.</value>
			/// <remarks>
			/// The least significant three bits are encoded in the ModR/M byte, while the fourth bit is encoded in the
			/// REX.R bit.
			/// </remarks>
			public byte Reg
			{
				get
				{
					#region Contract
					Contract.Ensures(Contract.Result<byte>() <= 0x0F,
						"Only the first 4 bits may be set.");
					#endregion
					return reg;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentOutOfRangeException>(value <= 0x0F,
						"Only the first 4 bits may be set.");
					#endregion
					reg = value;
				}
			}

			private byte mod;
			/// <summary>
			/// Gets or sets the value of the MOD part of the ModR/M byte.
			/// </summary>
			/// <value>The 2-bit MOD value.</value>
			public byte Mod
			{
				get
				{
					#region Contract
					Contract.Ensures(Contract.Result<byte>() <= 0x03,
						"Only the first 2 bits may be set.");
					#endregion
					return mod;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentOutOfRangeException>(value <= 0x03,
						"Only the first 2 bits may be set.");
					#endregion
					mod = value;
				}
			}
			#endregion

			#region Methods
			/// <summary>
			/// Returns a byte array representation of this sub structure.
			/// </summary>
			/// <returns>A byte array.</returns>
			public override byte[] ToBytes()
			{
				// CONTRACT: EncodedInstruction.SubStructure
				byte result = 0x00;
				result |= (byte)((rm & 0x07));
				result |= (byte)((reg & 0x07) << 3);
				result |= (byte)((mod & 0x03) << 6);
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
				Contract.Invariant(this.rm <= 0x0F);
				Contract.Invariant(this.reg <= 0x0F);
				Contract.Invariant(this.mod <= 0x03);
			}
			#endregion
		}
	}
}
