using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// Base class for <see cref="Expression"/> visitor implementations.
	/// </summary>
	public abstract class ExpressionVisitor : IExpressionVisitor
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionVisitor"/> class.
		/// </summary>
		protected ExpressionVisitor()
		{

		}
		#endregion

		#region Visitor Methods
		/// <summary>
		/// Visits an <see cref="Expression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="Expression"/> to visit.</param>
		protected internal virtual void VisitCustomExpression(Expression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			// Nothing to do.
		}

		/// <summary>
		/// Visits a <see cref="UnaryExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> to visit.</param>
		/// <remarks>
		/// Overriding methods may call this base method to ensure that the subexpressions are also visited.
		/// </remarks>
		protected internal virtual void VisitUnaryExpression(UnaryExpression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			expression.Expression.Accept(this);
		}

		/// <summary>
		/// Visits a <see cref="BinaryExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> to visit.</param>
		/// <remarks>
		/// Overriding methods may call this base method to ensure that the subexpressions are also visited.
		/// </remarks>
		protected internal virtual void VisitBinaryExpression(BinaryExpression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			expression.LeftHandExpression.Accept(this);
			expression.RightHandExpression.Accept(this);
		}

		/// <summary>
		/// Visits a <see cref="FunctionExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="FunctionExpression"/> to visit.</param>
		/// <remarks>
		/// Overriding methods may call this base method to ensure that the subexpressions are also visited.
		/// </remarks>
		protected internal virtual void VisitFunctionExpression(FunctionExpression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			foreach (Expression exp in expression.Arguments)
			{
				exp.Accept(this);
			}
		}

		/// <summary>
		/// Visits a <see cref="ConstantExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="ConstantExpression"/> to visit.</param>
		protected internal virtual void VisitConstantExpression(ConstantExpression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			// Nothing to do.
		}

		/// <summary>
		/// Visits a <see cref="CurrentPositionExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="CurrentPositionExpression"/> to visit.</param>
		protected internal virtual void VisitCurrentPositionExpression(CurrentPositionExpression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			// Nothing to do.
		}

		/// <summary>
		/// Visits a <see cref="CurrentSectionExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="CurrentSectionExpression"/> to visit.</param>
		protected internal virtual void VisitCurrentSectionExpression(CurrentSectionExpression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			// Nothing to do.
		}

		/// <summary>
		/// Visits a <see cref="ReferenceExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="ReferenceExpression"/> to visit.</param>
		protected internal virtual void VisitReferenceExpression(ReferenceExpression expression)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			#endregion

			// Nothing to do.
		}
		#endregion
	}
}
