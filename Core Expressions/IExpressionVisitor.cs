using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// Interface for <see cref="Expression"/> visitors.
	/// </summary>
	[ContractClass(typeof(Contracts.IExpressionVisitorContract))]
	public interface IExpressionVisitor
	{
		/// <summary>
		/// Visits an <see cref="Expression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="Expression"/> to visit.</param>
		void VisitCustomExpression(Expression expression);

		/// <summary>
		/// Visits a <see cref="BinaryExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> to visit.</param>
		void VisitBinaryExpression(BinaryExpression expression);

		/// <summary>
		/// Visits a <see cref="ConstantExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="ConstantExpression"/> to visit.</param>
		void VisitConstantExpression(ConstantExpression expression);

		/// <summary>
		/// Visits a <see cref="CurrentPositionExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="CurrentPositionExpression"/> to visit.</param>
		void VisitCurrentPositionExpression(CurrentPositionExpression expression);

		/// <summary>
		/// Visits a <see cref="CurrentSectionExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="CurrentSectionExpression"/> to visit.</param>
		void VisitCurrentSectionExpression(CurrentSectionExpression expression);

		/// <summary>
		/// Visits a <see cref="FunctionExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="FunctionExpression"/> to visit.</param>
		void VisitFunctionExpression(FunctionExpression expression);

		/// <summary>
		/// Visits a <see cref="ReferenceExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="ReferenceExpression"/> to visit.</param>
		void VisitReferenceExpression(ReferenceExpression expression);

		/// <summary>
		/// Visits a <see cref="UnaryExpression"/>.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> to visit.</param>
		void VisitUnaryExpression(UnaryExpression expression);
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IExpressionVisitor"/> type.
		/// </summary>
		[ContractClassFor(typeof(IExpressionVisitor))]
		abstract class IExpressionVisitorContract : IExpressionVisitor
		{
			public void VisitCustomExpression(Expression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}

			public void VisitBinaryExpression(BinaryExpression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}

			public void VisitConstantExpression(ConstantExpression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}

			public void VisitCurrentPositionExpression(CurrentPositionExpression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}

			public void VisitCurrentSectionExpression(CurrentSectionExpression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}

			public void VisitFunctionExpression(FunctionExpression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}

			public void VisitReferenceExpression(ReferenceExpression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}

			public void VisitUnaryExpression(UnaryExpression expression)
			{
				Contract.Requires<ArgumentNullException>(expression != null);
			}
		}
	}
	#endregion
}
