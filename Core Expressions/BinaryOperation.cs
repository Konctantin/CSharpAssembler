using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// Specifies the kind of binary operation performed on the operands.
	/// </summary>
	public enum BinaryOperation
	{
		/// <summary>
		/// No operation.
		/// </summary>
		None,
		/// <summary>
		/// A bitwise OR operation.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> OR
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		BitwiseOr = 1,
		/// <summary>
		/// A bitwise Exclusive OR operation.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> XOR
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		BitwiseXOr = 2,
		/// <summary>
		/// A bitwise AND operation.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> AND
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		BitwiseAnd = 3,
		/// <summary>
		/// An unsigned bitwise left shift.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> &lt;&lt;
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		LeftShift = 4,
		/// <summary>
		/// An unsigned bitwise right shift.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> &gt;&gt;
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		RightShift = 5,
		/// <summary>
		/// Addition.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> +
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		Add = 6,
		/// <summary>
		/// Subtraction.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> -
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		Subtract = 7,
		/// <summary>
		/// Multiplication.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> *
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		Multiply = 8,
		/// <summary>
		/// Unsigned division.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> /
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		DivideUnsigned = 9,
		/// <summary>
		/// Signed division.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> /
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		DivideSigned = 10,
		/// <summary>
		/// Unsigned modulo.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> %
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		ModuloUnsigned = 11,
		/// <summary>
		/// Signed modulo.
		/// </summary>
		/// <remarks>Evaluates to
		/// <see cref="BinaryExpression.LeftHandExpression"/> %
		/// <see cref="BinaryExpression.RightHandExpression"/>.</remarks>
		ModuloSigned = 12,
	}
}
