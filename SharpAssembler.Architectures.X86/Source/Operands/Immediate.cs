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
using SharpAssembler;
using SharpAssembler.Symbols;
using System.Linq.Expressions;

namespace SharpAssembler.Architectures.X86.Operands
{
	/// <summary>
	/// An immediate value.
	/// </summary>
	public class Immediate : Operand,
		ISourceOperand
	{
		#region Fields
		/// <summary>
		/// Whether this <see cref="Immediate"/> is encoded as the 'extra' immediate value.
		/// </summary>
		private bool asExtraImmediate = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Immediate"/> class.
		/// </summary>
		/// <param name="constant">A constant.</param>
		public Immediate(Int128 constant)
			: this(constant, DataSize.None)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Immediate"/> class.
		/// </summary>
		/// <param name="constant">A constant.</param>
		/// <param name="size">The size of the resulting value.</param>
		public Immediate(Int128 constant, DataSize size)
			: this(c => new ReferenceOffset(constant), size)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Immediate"/> class.
		/// </summary>
		/// <param name="reference">A reference.</param>
		public Immediate(Reference reference)
			: this(reference, DataSize.None)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(reference != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Immediate"/> class.
		/// </summary>
		/// <param name="reference">A reference.</param>
		/// <param name="size">The size of the resulting value.</param>
		public Immediate(Reference reference, DataSize size)
			: this(c => new ReferenceOffset(reference), size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(reference != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Immediate"/> class.
		/// </summary>
		/// <param name="expression">The expression.</param>
		public Immediate(Expression<Func<Context, ReferenceOffset>> expression)
			: this(expression, DataSize.None)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Immediate"/> class.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="size">The size of the resulting value.</param>
		public Immediate(Expression<Func<Context, ReferenceOffset>> expression, DataSize size)
			: base(size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			#endregion

			this.expression = expression;
		}
		#endregion

		#region Properties
		private Expression<Func<Context, ReferenceOffset>> expression;
		/// <summary>
		/// Gets or sets the expression evaluating to the immediate value.
		/// </summary>
		/// <value>A function taking a <see cref="Context"/> and returning a <see cref="ReferenceOffset"/>.</value>
		public Expression<Func<Context, ReferenceOffset>> Expression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Expression<Func<Context, ReferenceOffset>>>() != null);
				#endregion
				return expression;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.expression = value;
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

			// Let's evaluate the expression.
			ReferenceOffset result = expression.Compile()(context);

			// Determine the size of the immediate operand.
			DataSize size = PreferredSize;
			if (size == DataSize.None)
			{
				// Does the result have a (resolved or not resolved) reference?
				if (result.Reference != null)
					// When the result has a reference, use the architecture's operand size.
					size = context.Representation.Architecture.OperandSize;
				else
					// Otherwise, use the most efficient word size.
					size = MathExt.GetSizeOfValue(result.Constant);
			}
			if (size > DataSize.Bit64)
			    throw new AssemblerException("Operands with more than 64 bits cannot be encoded.");
			else if (size == DataSize.None)
				throw new AssemblerException("The operand size is not specified.");

			// Set the parameters.
			if (!asExtraImmediate)
			{
				instr.Immediate = result;
				instr.ImmediateSize = size;
			}
			else
			{
				instr.ExtraImmediate = result;
				instr.ExtraImmediateSize = size;
			}
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
				case OperandType.Immediate:
					return this.Size == DataSize.None || this.Size <= descriptor.Size;
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
			this.asExtraImmediate = (descriptor.OperandEncoding == OperandEncoding.ExtraImmediate);

			Contract.Assume(this.PreferredSize == DataSize.None || this.PreferredSize <= descriptor.Size);
			this.PreferredSize = descriptor.Size;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			// TODO: Implement.
			return base.ToString();
		}
		#endregion

		#region Conversions
		/// <summary>
		/// Converts a 128-bit signed integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		public static implicit operator Immediate(Int128 value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts a 64-bit signed integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		public static implicit operator Immediate(long value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts a 64-bit unsigned integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator Immediate(ulong value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts a 32-bit signed integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		public static implicit operator Immediate(int value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts a 32-bit unsigned integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator Immediate(uint value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts a 16-bit signed integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		public static implicit operator Immediate(short value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts a 16-bit unsigned integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator Immediate(ushort value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts an 8-bit signed integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator Immediate(sbyte value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts an 8-bit unsigned integer to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		public static implicit operator Immediate(byte value)
		{
			return new Immediate(value);
		}

		/// <summary>
		/// Converts a reference to an <see cref="Immediate"/> value.
		/// </summary>
		/// <param name="reference">The reference to convert.</param>
		/// <returns>The resulting <see cref="Immediate"/>.</returns>
		public static implicit operator Immediate(Reference reference)
		{
			return new Immediate(reference);
		}
		#endregion
	}
}
