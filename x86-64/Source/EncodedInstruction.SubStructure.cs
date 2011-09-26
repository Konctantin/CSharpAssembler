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
using System.Text;

namespace SharpAssembler.x86
{
	partial class EncodedInstruction
	{
		/// <summary>
		/// A base class for sub structures of the <see cref="EncodedInstruction"/>.
		/// </summary>
		[ContractClass(typeof(Contracts.EncodedInstruction_SubStructureContract))]
		public abstract class SubStructure
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="SubStructure"/> class.
			/// </summary>
			protected SubStructure()
			{
			}

			/// <summary>
			/// Returns a byte array representation of this sub structure.
			/// </summary>
			/// <returns>A byte array.</returns>
			public abstract byte[] ToBytes();

			/// <summary>
			/// Copies the representation of this sub structure to the specified array.
			/// </summary>
			/// <param name="target">The array to copy to.</param>
			/// <param name="index">The index at which copying starts.</param>
			/// <returns>The number of bytes copied.</returns>
			public int CopyTo(byte[] target, int index)
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(target != null);
				Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < target.Length);
				#endregion

				return CopyTo(target, index, -1);
			}

			/// <summary>
			/// Copies the representation of this sub structure to the specified array.
			/// </summary>
			/// <param name="target">The array to copy to.</param>
			/// <param name="index">The index at which copying starts.</param>
			/// <param name="count">The maximum number of bytes to copy; or -1 to specify no limit.</param>
			/// <returns>The number of bytes copied.</returns>
			public int CopyTo(byte[] target, int index, int count)
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(target != null);
				Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < target.Length);
				Contract.Requires<ArgumentOutOfRangeException>(count == -1 || (count >= 0 && index + count - 1 < target.Length));
				#endregion

				byte[] source = ToBytes();
				int length = source.Length;
				if (count >= 0 && count < length)
					length = count;

				Array.Copy(source, 0, target, index, length);

				return length;
			}

			/// <summary>
			/// Returns a <see cref="String"/> that represents the current <see cref="Object"/>.
			/// </summary>
			/// <returns>A <see cref="String"/> that represents the current <see cref="Object"/>.</returns>
			public override string ToString()
			{
				byte[] bytes = ToBytes();

				StringBuilder sb = new StringBuilder(bytes.Length * 4 + 2);
				sb.Append("{");

				if (bytes.Length > 0)
				{
					sb.AppendFormat("0x{0:X2}", bytes[0]);
					for (int i = 1; i < bytes.Length; i++)
						sb.AppendFormat(", 0x{0:X2}", bytes[i]);
				}

				sb.Append("}");
				return sb.ToString();
			}
		}
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="EncodedInstruction.SubStructure"/> type.
		/// </summary>
		[ContractClassFor(typeof(EncodedInstruction.SubStructure))]
		abstract class EncodedInstruction_SubStructureContract : EncodedInstruction.SubStructure
		{
			public override byte[] ToBytes()
			{
				Contract.Ensures(Contract.Result<byte[]>() != null);

				return default(byte[]);
			}
		}
	}
	#endregion
}
