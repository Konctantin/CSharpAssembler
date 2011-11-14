using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// A constant value.
	/// </summary>
	public class ConstantExpression : Expression
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantExpression"/> class with a specific constant.
		/// </summary>
		/// <param name="value">The value.</param>
		public ConstantExpression(long value)
		{
			#region Contract
			Contract.Ensures(this.value == value);
			#endregion

			this.value = value;
		}
		#endregion

		#region Properties
		private long value;
		/// <summary>
		/// Gets the value of this constant.
		/// </summary>
		/// <value>The constant value.</value>
		public long Value
		{
			get { return value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Accepts a visitor.
		/// </summary>
		/// <param name="visitor">The <see cref="ExpressionVisitor"/> to accept.</param>
		public override void Accept(ExpressionVisitor visitor)
		{
			visitor.VisitConstantExpression(this);
		}
		#endregion

		#region Evaluation
		/// <summary>
		/// Evaluates the expression in the given context.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this expression is found;
		/// or <see langword="null"/> when there is no context for this expression.</param>
		/// <returns>The expression to which the expression evaluated.</returns>
		public ExpressionResult Evaluate(IContext context)
		{
			#region Contract
			// NOTE: There is no requirement that 'context' must be non-null.
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			return new ExpressionResult(value);
		}
		#endregion
	}
}
