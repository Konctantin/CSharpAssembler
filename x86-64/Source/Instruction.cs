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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using SharpAssembler.Core;
using SharpAssembler.x86.Operands;

namespace SharpAssembler.x86
{
	/// <summary>
	/// An x86-64 instruction.
	/// </summary>
	public abstract partial class Instruction : Constructable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Instruction"/> class.
		/// </summary>
		protected Instruction()
			: base()
		{ /* Nothing to do. */ }

#if false
		/// <summary>
		/// Initializes a new instance of the <see cref="Instruction"/> class.
		/// </summary>
		/// <param name="operandCount">The number of operands for this instruction.</param>
		protected Instruction(int operandCount)
			: base()
		{
			#region Contract
			if (operandCount < 0) throw new ArgumentOutOfRangeException("operandCount");
			#endregion

			operands = new FixedSizeList<Operand>(operandCount);
		}
#endif
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public abstract string Mnemonic
		{ get; }

		private DataSize operandSize = DataSize.None;
		/// <summary>
		/// Gets an explicit operand size for this instruction.
		/// </summary>
		/// <value>The explicit operand size for this instruction; or <see cref="DataSize.None"/> to determine it from
		/// the operands.</value>
		/// <remarks>
		/// This property is intended to be used with instructions which do not have any operand from which the operand
		/// size can be determined.
		/// </remarks>
		public DataSize OperandSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return operandSize;
			}
#if OPERAND_SET
			set
#else
			protected set
#endif
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				operandSize = value;
			}
		}

		/// <summary>
		/// Gets whether this instruction is valid in 64-bit mode.
		/// </summary>
		/// <value><see langword="true"/> when the instruction is valid in 64-bit mode;
		/// otherwise, <see langword="false"/>.</value>
		public virtual bool ValidIn64BitMode
		{
			get { return true; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enumerates an ordered list of operands used by this instruction.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Operand"/> objects.</returns>
		public abstract IEnumerable<Operand> GetOperands();

		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the emittable will be constructed, and which may
		/// be modified.</param>
		/// <returns>The constructed emittable.</returns>
		public override IEmittable Construct(Context context)
		{
			// CONTRACT: Constructable

			var arch = context.Representation.Architecture;
			if (!ValidIn64BitMode && (arch.AddressSize == DataSize.Bit64 || arch.OperandSize == DataSize.Bit64 || OperandSize == DataSize.Bit64))
				throw new AssemblerException("The instruction is not valid in 64-bit mode.");
			if (ValidIn64BitMode && OperandSize == DataSize.Bit64 && arch.OperandSize != DataSize.Bit64)
				throw new AssemblerException("The instruction variant is only valid in 64-bit mode.");

			// Get the most efficient instruction variant.
			InstructionVariant variant = GetVariant(context);
			if (variant == null)
				throw new AssemblerException("No matching instruction variant was found.");

			// Lock prefix?
			bool lockprefix = false;
			ILockInstruction lockinstr = this as ILockInstruction;
			if (lockinstr != null)
				lockprefix = lockinstr.Lock;

			// Construct the chosen variant.
			EncodedInstruction instr = variant.Construct(context, GetOperands(), lockprefix);

			return instr;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}({1})",
				this.Mnemonic,
				String.Join(", ", GetOperands()));
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// Returns an array containing the <see cref="Instruction.InstructionVariant"/> objects representing all the possible
		/// variants of the instruction.
		/// </summary>
		/// <returns>An array of <see cref="Instruction.InstructionVariant"/> objects.</returns>
		internal abstract InstructionVariant[] GetVariantList();

		/// <summary>
		/// Finds the most efficient variant for the instruction with the current operands.
		/// </summary>
		/// <param name="context">The <see cref="Context"/>.</param>
		/// <returns>The most efficient <see cref="Instruction.InstructionVariant"/>; or <see langword="null"/> when none was
		/// found.</returns>
		private InstructionVariant GetVariant(Context context)
		{
			var variants =
				from variant in GetVariantList()
				where variant.Match(operandSize, GetOperands().ToList())
				select variant;
			return variants.FirstOrDefault();
		}
		#endregion
	}
}
