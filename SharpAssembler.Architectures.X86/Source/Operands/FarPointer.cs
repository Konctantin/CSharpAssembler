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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using SharpAssembler;
using System.Linq.Expressions;

namespace SharpAssembler.Architectures.X86.Operands
{
	/// <summary>
	/// A relative offset.
	/// </summary>
	/// <remarks>
	/// In the Intel manuals, a far pointer is denoted as <c>ptr16:16</c>, <c>ptr16:32</c> or <c>ptr16:64</c>.
	/// In the AMD manuals, a far pointer is denoted as <c>pntr16:16</c> and <c>pntr16:32</c>.
	/// </remarks>
	public class FarPointer : Operand
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="FarPointer"/> class.
		/// </summary>
		/// <param name="selector">The 16-bit selector expression.</param>
		/// <param name="offset">The offset expression.</param>
		/// <param name="size">The size of the offset; or <see cref="DataSize.None"/> to specify no size.</param>
		public FarPointer(
			Expression<Func<Context, ReferenceOffset>> selector,
			Expression<Func<Context, ReferenceOffset>> offset,
			DataSize size)
			: base(size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(selector != null);
			Contract.Requires<ArgumentNullException>(offset != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			#endregion

			this.selector = selector;
			this.offset = offset;
			this.PreferredSize = size;
		}
		#endregion

		#region Properties
		private Expression<Func<Context, ReferenceOffset>> selector;
		/// <summary>
		/// Gets or sets the expression evaluating to the 16-bit selector.
		/// </summary>
		/// <value>A function taking a <see cref="Context"/> and returning a <see cref="ReferenceOffset"/>.</value>
		public Expression<Func<Context, ReferenceOffset>> Selector
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Expression<Func<Context, ReferenceOffset>>>() != null);
				#endregion
				return selector;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.selector = value;
			}
		}

		private Expression<Func<Context, ReferenceOffset>> offset;
		/// <summary>
		/// Gets or sets the expression evaluating to the offset.
		/// </summary>
		/// <value>A function taking a <see cref="Context"/> and returning a <see cref="ReferenceOffset"/>.</value>
		public Expression<Func<Context, ReferenceOffset>> Offset
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Expression<Func<Context, ReferenceOffset>>>() != null);
				#endregion
				return offset;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.offset = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructs the operand's representation.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instr">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		internal override void Construct(Context context, EncodedInstruction instr)
		{
			// CONTRACT: Operand

			ReferenceOffset offsetResult = offset.Compile()(context);
			ReferenceOffset selectorResult = selector.Compile()(context);

			// Determine the size of the immediate operand.
			DataSize size = PreferredSize;
			if (size == DataSize.None)
			{
				// Does the result have a (resolved or not resolved) reference?
				if (offsetResult.Reference != null)
					// When the result has a reference, use the architecture's operand size.
					size = context.Representation.Architecture.OperandSize;
				else
					// Otherwise, use the most efficient word size.
					size = MathExt.GetSizeOfValue(offsetResult.Constant);
			}
			if (size <= DataSize.Bit8)
				size = DataSize.Bit16;

			if (size > DataSize.Bit64)
				throw new AssemblerException("The operand cannot be encoded.");
			else if (size == DataSize.None)
				throw new AssemblerException("The operand size is not specified.");

			

			// Set the parameters.
			instr.Immediate = offsetResult;
			instr.ImmediateSize = size;
			instr.ExtraImmediate = selectorResult;
			instr.ExtraImmediateSize = (DataSize)2;
			instr.SetOperandSize(context.Representation.Architecture.OperandSize, size);
		}

		/// <summary>
		/// Determines whether the specified <see cref="OperandDescriptor"/> matches this
		/// <see cref="Operand"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="OperandDescriptor"/> to match.</param>
		/// <returns><see langword="true"/> when the specified descriptor matches this operand;
		/// otherwise, <see langword="false"/>.</returns>
		internal override bool IsMatch(OperandDescriptor descriptor)
		{
			switch (descriptor.OperandType)
			{
				case OperandType.FarPointer:
					return this.Size == descriptor.Size;
				default:
					return false;
			}
		}

		/// <summary>
		/// Adjusts this <see cref="Operand"/> based on the specified <see cref="OperandDescriptor"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="OperandDescriptor"/> used to adjust.</param>
		/// <remarks>
		/// Only <see cref="OperandDescriptor"/> instances for which <see cref="IsMatch"/> returns
		/// <see langword="true"/> may be used as a parameter to this method.
		/// </remarks>
		internal override void Adjust(OperandDescriptor descriptor)
		{
			// Nothing to do.
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}:{1}",
				this.selector,
				this.offset);
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.selector != null);
			Contract.Invariant(this.offset != null);
		}
		#endregion
	}
}
