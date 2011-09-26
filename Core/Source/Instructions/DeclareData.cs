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

namespace SharpAssembler.Core.Instructions
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
		public DeclareData(Func<Context, SimpleExpression> expression, DataSize size)
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
		private Func<Context, SimpleExpression> expression;
		/// <summary>
		/// Gets or sets the expression that will be declared.
		/// </summary>
		/// <value>A function accepting a <see cref="Context"/> and returning a <see cref="SimpleExpression"/>.</value>
		public Func<Context, SimpleExpression> Expression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Func<Context, SimpleExpression>>() != null);
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
		/// <summary>
		/// Modifies the context and constructs an emittable representing this constructable.
		/// </summary>
		/// <param name="context">The mutable <see cref="Context"/> in which the emittable will be constructed.</param>
		/// <returns>The constructed emittable; or <see langword="null"/> when no emittable results from this
		/// constructable.</returns>
		public override IEmittable Construct(Context context)
		{
			// CONTRACT: Constructable
			return new ExpressionEmittable(expression(context), size);
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
