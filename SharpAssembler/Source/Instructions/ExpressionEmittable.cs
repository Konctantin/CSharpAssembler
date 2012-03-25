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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;

namespace SharpAssembler.Instructions
{
	/// <summary>
	/// An emittable which emits an expression result.
	/// </summary>
	public class ExpressionEmittable : IEmittable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionEmittable"/> class.
		/// </summary>
		/// <param name="expression">The expression of the value.</param>
		/// <param name="size">The size of the result.</param>
		public ExpressionEmittable(SimpleExpression expression, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Requires<ArgumentException>(size != DataSize.None);
			#endregion

			this.expression = expression;
			this.size = size;
		}
		#endregion

		#region Properties
		private SimpleExpression expression;
		/// <summary>
		/// Gets or sets the expression result to emit.
		/// </summary>
		/// <value>A <see cref="SimpleExpression"/>.</value>
		public SimpleExpression Expression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<SimpleExpression>() != null);
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

		private DataSize size;
		/// <summary>
		/// Gets or sets the size of the emitted data.
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
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				Contract.Requires<ArgumentException>(value != DataSize.None);
				#endregion
				this.size = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Modifies the context and emits the binary representation of this emittable.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		/// <param name="context">The <see cref="Context"/> in which the emittable will be emitted.</param>
		/// <returns>The number of emitted bytes.</returns>
		public int Emit(BinaryWriter writer, Context context)
		{
			// CONTRACT: IEmittable

			Int128 value = this.expression.Evaluate(context);
			return writer.Write(value, this.size);
		}

		/// <summary>
		/// Gets the length of the emittable.
		/// </summary>
		/// <returns>The length of the emittable, in bytes.</returns>
		public int GetLength()
		{
			// CONTRACT: IEmittable
			return (int)this.size;
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
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
