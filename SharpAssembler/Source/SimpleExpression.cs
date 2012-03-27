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
using SharpAssembler.Symbols;

namespace SharpAssembler
{
	/// <summary>
	/// A simple expression of (reference + offset).
	/// </summary>
	public class SimpleExpression
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleExpression"/> class
		/// with the specified (reference + offset) expression.
		/// </summary>
		/// <param name="reference">The reference whose address is used.</param>
		/// <param name="constant">A constant value added to the reference address.</param>
		public SimpleExpression(Reference reference, Int128 constant)
		{
			this.reference = reference;
			this.constant = constant;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleExpression"/> class
		/// with the specified value.
		/// </summary>
		/// <param name="constant">A constant value.</param>
		public SimpleExpression(Int128 constant)
			: this(null, constant)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleExpression"/> class
		/// with the specified reference address.
		/// </summary>
		/// <param name="reference">The reference whose address is used.</param>
		public SimpleExpression(Reference reference)
			: this(reference, 0)
		{
		}
		#endregion

		#region Properties
		private Reference reference;
		/// <summary>
		/// Gets the <see cref="Reference"/> to a symbol whose address is used as the value for the expression.
		/// </summary>
		/// <value>A <see cref="Reference"/>; or <see langword="null"/> when no reference is specified.</value>
		public Reference Reference
		{
			get { return reference; }
		}

		private Int128 constant;
		/// <summary>
		/// Gets the constant added to reference address value (or 0 when no reference is provided).
		/// </summary>
		/// <value>An <see cref="Int128"/>.</value>
		public Int128 Constant
		{
			get { return constant; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Resolves any unresolved symbol references and returns the actual value of the expression.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the unresolved symbol references are
		/// resolved.</param>
		/// <returns>The result of the expression.</returns>
		/// <exception cref="InvalidOperationException">
		/// The symbol referenced could not be resolved.
		/// </exception>
		public Int128 Evaluate(Context context)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			#endregion

			// Attempts to resolve any unresolved symbol references.
			if (this.reference != null && !this.reference.Resolve(context))
				throw new InvalidOperationException("The expression still contains an unresolved symbol reference.");

			Int128 value = this.constant;
			if (this.reference != null)
				value += this.reference.Symbol.Value;

			return value;
		}
		#endregion
	}
}
