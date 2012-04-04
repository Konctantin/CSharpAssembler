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
using SharpAssembler;

namespace SharpAssembler.Architectures.X86.Operands
{
	/// <summary>
	/// A relative offset.
	/// </summary>
	/// <remarks>
	/// In the Intel manuals, a memory offset is denoted as <c>moffs8</c>, <c>moffs16</c>, <c>moffs32</c> and
	/// <c>moffs64</c>. In the AMD manuals, a memory offset is denoted as <c>moffset8</c>, <c>moffset16</c>,
	/// <c>moffset32</c> and <c>moffset64</c>.
	/// </remarks>
	public class MemoryOffset : Operand
	{
		private MemoryOffset()
			: base(DataSize.None)
		{
			
		}

		/// <summary>
		/// Constructs the operand's representation.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instruction">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		internal override void Construct(Context context, EncodedInstruction instruction)
		{
			throw new NotImplementedException();
		}
#if false
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryOffset"/>
		/// class.
		/// </summary>
		/// <param name="value">The expression describing the jump
		/// target.</param>
		/// <param name="size">The size of the offset;
		/// or <see cref="DataSize.None"/> to specify no size.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		public MemoryOffset(Expression value, DataSize size)
			: base(size)
		{
		#region Contract
			if (value == null) throw new ArgumentNullException("value");
			#endregion

			this.expression = value;
		}
		#endregion

		#region Properties
		private Expression expression;
		/// <summary>
		/// Gets or sets the expression evaluating to the immediate value.
		/// </summary>
		/// <value>An <see cref="Expression"/>.</value>
		/// <exception cref="ArgumentNullException">
		/// The value is <see langword="null"/>.
		/// </exception>
		public Expression Expression
		{
			get { return expression; }
			set
			{
		#region Contract
				if (value == null) throw new ArgumentNullException("value");
				#endregion
				this.expression = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructs the operand's representation.
		/// </summary>
		/// <param name="context">The <see cref="X86Context"/> in which the
		/// operand is used.</param>
		/// <param name="instr">The <see cref="EncodedInstruction"/> encoding the
		/// operand.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="context"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="instr"/> is <see langword="null"/>.</para>
		/// </exception>
		internal override void Construct(X86Context context, EncodedInstruction instr)
		{
		#region Contract
			if (context == null) throw new ArgumentNullException("context");
			if (instr == null) throw new ArgumentNullException("instr");
			#endregion

			// Let's evaluate the expression.
			ExpressionResult result = new BinaryExpression(
					expression,
					BinaryOperation.Subtract,
					new BinaryExpression(
						new CurrentPosition(),
						BinaryOperation.Add,
						new Constant((long)instr.GetLength()))).Evaluate(context);

			// Determine the size of the immediate operand.
			DataSize size = RequestedSize;
			if (size == DataSize.None)
			{
				// Does the result have a (resolved or not resolved) reference?
				if (result.HasReference)
					// When the result has a reference, use the architecture's operand size.
					size = context.Representation.Architecture.OperandSize;
				else
					// Otherwise, use the most efficient word size.
					size = result.Constant.GetSize();
			}
			if (size >= DataSize.Bit64) throw new AssemblerException(ExceptionStrings.OperandSizeNotEncodable);
			else if (size == DataSize.None) throw new AssemblerException(ExceptionStrings.OperandSizeNotSpecified);

			// Set the parameters.
			instr.Immediate = result;
			instr.ImmediateSize = size;
			instr.SetOperandSize(context.Representation.Architecture.OperandSize, size);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return expression.ToString();
		}
		#endregion
#endif

		internal override bool IsMatch(OperandDescriptor descriptor)
		{
			throw new NotImplementedException();
		}

		internal override void Adjust(OperandDescriptor descriptor)
		{
			throw new NotImplementedException();
		}
	}
}
