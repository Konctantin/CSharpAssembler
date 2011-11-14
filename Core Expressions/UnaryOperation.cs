using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// Specifies the kind of unary operation performed on the operands.
	/// </summary>
	public enum UnaryOperation
	{
		/// <summary>
		/// No operation.
		/// </summary>
		/// <remarks>Evaluates to 0.</remarks>
		None,
		/// <summary>
		/// A negation.
		/// </summary>
		/// <remarks>Evaluates to -<see cref="UnaryExpression.Expression"/>.</remarks>
		Negate = 1,	//0x0700,
		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <remarks>Evaluates to <see cref="UnaryExpression.Expression"/>.</remarks>
		Positivate = 2,	//0x0701,
		/// <summary>
		/// Increment.
		/// </summary>
		/// <remarks>Evaluates to <see cref="UnaryExpression.Expression"/>+1.</remarks>
		Increment = 3,	//0x0800,
		/// <summary>
		/// Decrement.
		/// </summary>
		/// <remarks>Evaluates to <see cref="UnaryExpression.Expression"/>-1.</remarks>
		Decrement = 4,	//0x0801,
		/// <summary>
		/// Bitwise complement.
		/// </summary>
		/// <remarks>Evaluates to ~<see cref="UnaryExpression.Expression"/>.</remarks>
		Complement = 5,	//0x0703,
		/// <summary>
		/// Logical negation.
		/// </summary>
		/// <remarks>Evaluates to !<see cref="UnaryExpression.Expression"/>.</remarks>
		Not = 6,		//0x0702,
	}
}
