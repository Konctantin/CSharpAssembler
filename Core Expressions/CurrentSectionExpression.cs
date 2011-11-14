using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// This expression evaluates to the virtual address of the start of the
	/// section currently being assembled.
	/// </summary>
	public class CurrentSectionExpression : Expression
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CurrentSectionExpression"/> class.
		/// </summary>
		public CurrentSectionExpression()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Accepts a visitor.
		/// </summary>
		/// <param name="visitor">The <see cref="ExpressionVisitor"/> to accept.</param>
		public override void Accept(ExpressionVisitor visitor)
		{
			visitor.VisitCurrentSectionExpression(this);
		}
		#endregion

		#region Evaluation
		/// <summary>
		/// Evaluates the expression in the given context.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this expression is found.</param>
		/// <returns>The expression to which the expression evaluated.</returns>
		public ExpressionResult Evaluate(IContext context)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentNullException>(context.Section != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			return new ExpressionResult(context.Section.AssociatedSymbol, 0);
		}
		#endregion
	}
}
