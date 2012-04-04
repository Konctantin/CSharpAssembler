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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The PREFETCHL (Prefetch Data to Cache Level) instruction.
	/// </summary>
	public partial class Prefetchl : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Prefetchl"/> class.
		/// </summary>
		/// <param name="address">The address to prefetch.</param>
		/// <param name="level">The prefetch level.</param>
		public Prefetchl(EffectiveAddress address, PrefetchLevel level)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(address != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(PrefetchLevel), level));
			Contract.Requires<ArgumentException>(level != PrefetchLevel.None);
			#endregion

			this.address = address;
			this.level = level;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get
			{
				switch (level)
				{
					case PrefetchLevel.NonTemporalAccess:
						return "prefetchnta";
					case PrefetchLevel.T0:
						return "prefetch0";
					case PrefetchLevel.T1:
						return "prefetch1";
					case PrefetchLevel.T2:
						return "prefetch2";
					case PrefetchLevel.None:
					default:
						throw new Exception();
				}
			}
		}

		private PrefetchLevel level;
		/// <summary>
		/// Gets the prefetch level.
		/// </summary>
		/// <value>A member of the <see cref="PrefetchLevel"/> enumeration.</value>
		public PrefetchLevel Level
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(PrefetchLevel), Contract.Result<PrefetchLevel>()));
				Contract.Ensures(Contract.Result<PrefetchLevel>() != PrefetchLevel.None);
				#endregion
				return level;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(PrefetchLevel), value));
				Contract.Requires<ArgumentException>(value != PrefetchLevel.None);
				#endregion
				level = value;
			}
#endif
		}

		private EffectiveAddress address;
		/// <summary>
		/// Gets the address to prefetch.
		/// </summary>
		/// <value>A <see cref="EffectiveAddress"/>.</value>
		public EffectiveAddress Address
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<EffectiveAddress>() != null);
				#endregion
				return address;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				address = value;
			}
#endif
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enumerates an ordered list of operands used by this instruction.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Operand"/> objects.</returns>
		public override IEnumerable<Operand> GetOperands()
		{
			// The order is important here!
			yield return this.address;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// PREFETCHNTA mem8
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x1B }, 0,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit8)),
			// PREFETCH0 mem8
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x1B }, 1,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit8)),
			// PREFETCH1 mem8
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x1B }, 2,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit8)),
			// PREFETCH2 mem8
			new X86OpcodeVariant(
				new byte[] { 0x0F, 0x1B }, 3,
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit8)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86OpcodeVariant"/>
		/// objects.</returns>
		internal override X86OpcodeVariant[] GetVariantList()
		{
			return new X86OpcodeVariant[] { variants[(int)level & 0xFF] };
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.address != null);
		}
		#endregion
	}
}



