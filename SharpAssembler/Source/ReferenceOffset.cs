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
using System.Diagnostics.Contracts;
using SharpAssembler.Symbols;

namespace SharpAssembler
{
	/// <summary>
	/// A simple expression of (reference + offset).
	/// </summary>
	public class ReferenceOffset
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceOffset"/> class
		/// with the specified (reference + offset) expression.
		/// </summary>
		/// <param name="reference">The reference whose address is used.</param>
		/// <param name="constant">A constant value added to the reference address.</param>
		public ReferenceOffset(Reference reference, Int128 constant)
		{
			this.reference = reference;
			this.constant = constant;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceOffset"/> class
		/// with the specified value.
		/// </summary>
		/// <param name="constant">A constant value.</param>
		public ReferenceOffset(Int128 constant)
			: this(null, constant)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceOffset"/> class
		/// with the specified reference address.
		/// </summary>
		/// <param name="reference">The reference whose address is used.</param>
		public ReferenceOffset(Reference reference)
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

		#region Conversions
		/// <summary>
		/// Converts the specified <see cref="String"/> to a symbol reference.
		/// </summary>
		/// <param name="reference">The reference.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(string reference)
		{
			return new ReferenceOffset(new Reference(reference));
		}

		/// <summary>
		/// Converts the specified <see cref="Reference"/> to a simple expression.
		/// </summary>
		/// <param name="reference">The reference.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(Reference reference)
		{
			return new ReferenceOffset(reference);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator ReferenceOffset(sbyte value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(byte value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(short value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator ReferenceOffset(ushort value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(int value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator ReferenceOffset(uint value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(long value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		[CLSCompliant(false)]
		public static implicit operator ReferenceOffset(ulong value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(Int128 value)
		{
			return new ReferenceOffset(value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(Int128? value)
		{
			if (value.HasValue)
				throw new ArgumentException("The specified variable is not assigned a value.");

			return new ReferenceOffset(value.Value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(UInt128 value)
		{
			return new ReferenceOffset((Int128)value);
		}

		/// <summary>
		/// Converts the specified constant to a simple expression.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The <see cref="ReferenceOffset"/>.</returns>
		public static implicit operator ReferenceOffset(UInt128? value)
		{
			if (value.HasValue)
				throw new ArgumentException("The specified variable is not assigned a value.");

			return new ReferenceOffset((Int128)value.Value);
		}

		public static ReferenceOffset operator +(ReferenceOffset left, ReferenceOffset right)
		{
			if (left.reference != null && right.reference != null)
				throw new Exception("Cannot add two symbol-relative expressions.");
			else if (left.reference != null)
				return new ReferenceOffset(left.reference, left.constant + right.constant);
			else
				return new ReferenceOffset(right.reference, left.constant + right.constant);
		}
		#endregion
	}
}
