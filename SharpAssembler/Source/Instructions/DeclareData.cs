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
using System.Collections.Generic;
using SharpAssembler.Symbols;
using System.Linq.Expressions;

namespace SharpAssembler.Instructions
{
	/// <summary>
	/// Declares data.
	/// </summary>
	public class DeclareData : Constructable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="expression">The expression of the value.</param>
		/// <param name="size">The size of the result.</param>
		public DeclareData(Expression<Func<Context, SimpleExpression>> expression, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			#endregion

			this.expression = expression;
			this.size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="reference">A symbol reference.</param>
		/// <param name="size">The size of the result.</param>
		public DeclareData(Reference reference, DataSize size)
			: this(c => new SimpleExpression(reference), size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(reference != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			#endregion
		}

		#region Primitive Overloads
		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 8-bit signed integer value.</param>
		[CLSCompliant(false)]
		public DeclareData(sbyte value)
			: this(c => new SimpleExpression(value), DataSize.Bit8)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 8-bit unsigned integer value.</param>
		public DeclareData(byte value)
			: this(c => new SimpleExpression(value), DataSize.Bit8)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 16-bit signed integer value.</param>
		public DeclareData(short value)
			: this(c => new SimpleExpression(value), DataSize.Bit16)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 16-bit unsigned integer value.</param>
		[CLSCompliant(false)]
		public DeclareData(ushort value)
			: this(c => new SimpleExpression(value), DataSize.Bit16)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 32-bit signed integer value.</param>
		public DeclareData(int value)
			: this(c => new SimpleExpression(value), DataSize.Bit32)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 32-bit unsigned integer value.</param>
		[CLSCompliant(false)]
		public DeclareData(uint value)
			: this(c => new SimpleExpression(value), DataSize.Bit32)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 64-bit signed integer value.</param>
		public DeclareData(long value)
			: this(c => new SimpleExpression(value), DataSize.Bit64)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 64-bit unsigned integer value.</param>
		[CLSCompliant(false)]
		public DeclareData(ulong value)
			: this(c => new SimpleExpression(value), DataSize.Bit64)
		{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclareData"/> class.
		/// </summary>
		/// <param name="value">A 128-bit signed integer value.</param>
		[CLSCompliant(false)]
		public DeclareData(Int128 value)
			: this(c => new SimpleExpression(value), DataSize.Bit128)
		{ /* Nothing to do. */ }
		#endregion
		#endregion

		#region Properties
		private Expression<Func<Context, SimpleExpression>> expression;
		/// <summary>
		/// Gets or sets the expression that will be declared.
		/// </summary>
		/// <value>A function accepting a <see cref="Context"/> and returning a <see cref="SimpleExpression"/>.</value>
		public Expression<Func<Context, SimpleExpression>> Expression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Expression<Func<Context, SimpleExpression>>>() != null);
				#endregion
				return expression;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				this.expression = value;
			}
#endif
		}

		private DataSize size;
		/// <summary>
		/// Gets or sets the size of the declared data.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.</value>
		public DataSize Size
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				Contract.Ensures(Contract.Result<DataSize>() != DataSize.None);
				#endregion
				return size;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				Contract.Requires<ArgumentException>(value != DataSize.None);
				#endregion
				this.size = value;
			}
#endif
		}
		#endregion

		#region Methods
		/// <inheritdoc />
		public override IEnumerable<IEmittable> Construct(Context context)
		{
			// CONTRACT: Constructable
			yield return new ExpressionEmittable(expression.Compile()(context), size);
		}

		/// <inheritdoc />
		public override void Accept(IObjectFileVisitor visitor)
		{
			visitor.VisitDeclareData(this);
		}
		#endregion

		#region Invariant
		/// <summary>
		/// The invariant method for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.expression != null);
			Contract.Invariant(Enum.IsDefined(typeof(DataSize), this.size));
			Contract.Invariant(this.size != DataSize.None);
		}
		#endregion
	}
}
