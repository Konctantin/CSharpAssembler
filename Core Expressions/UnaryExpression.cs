using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// This unary expression takes one operand and performs some
	/// operation on it.
	/// </summary>
	public class UnaryExpression : Expression
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryExpression"/>
		/// class.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="operation">The unary operation to perform.</param>
		public UnaryExpression(Expression expression, UnaryOperation operation)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(BinaryOperation), operation));
			Contract.Requires<ArgumentException>(operation != UnaryOperation.None);
			Contract.Ensures(this.expression == expression);
			Contract.Ensures(this.operation == operation);
			#endregion

			this.expression = expression;
			this.operation = operation;
		}
		#endregion

		#region Properties
		private UnaryOperation operation;
		/// <summary>
		/// Gets or sets the operation to perform on the operands.
		/// </summary>
		/// <value>The unary operation to perform.</value>
		public UnaryOperation Operation
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<UnaryOperation>() != UnaryOperation.None);
				Contract.Ensures(Enum.IsDefined(typeof(UnaryOperation), Contract.Result<UnaryOperation>()));
				#endregion

				return operation;
			}
		}

		private Expression expression;
		/// <summary>
		/// Gets or sets the sub expression of this unary expression.
		/// </summary>
		/// <value>An <see cref="Expression"/>.</value>
		public Expression Expression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Expression>() != null);
				#endregion

				return expression;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Accepts a visitor.
		/// </summary>
		/// <param name="visitor">The <see cref="ExpressionVisitor"/> to accept.</param>
		public override void Accept(ExpressionVisitor visitor)
		{
			visitor.VisitUnaryExpression(this);
		}
		#endregion

		#region Evaluation
		/// <summary>
		/// Evaluates the expression in the given context.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this expression is found;
		/// or <see langword="null"/> when there is no context for this expression.</param>
		/// <param name="value">The sub expression result.</param>
		/// <returns>The result of the expression.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed on a value which is relative to some symbol.
		/// </exception>
		public ExpressionResult Evaluate(IContext context, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			ExpressionResult result = UnaryOperations[(int)operation](this, value);
			Contract.Assume(result != null);
			return result;
		}

		/// <summary>
		/// A delegate for unary operations.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> under consideration.</param>
		/// <param name="value">The value.</param>
		/// <returns>The resulting value.</returns>
		private delegate ExpressionResult UnaryOperator(UnaryExpression expression, ExpressionResult value);

		/// <summary>
		/// An ordered list of methods to invoke for each possible unary operation.
		/// </summary>
		private static readonly UnaryOperator[] UnaryOperations =
		{
			null,											// UnaryOperation.None,
			new	UnaryOperator(NegateOperation),				// UnaryOperation.Negate
			new	UnaryOperator(PositivateOperation),			// UnaryOperation.Positivate
			new	UnaryOperator(IncrementOperation),			// UnaryOperation.Increment
			new	UnaryOperator(DecrementOperation),			// UnaryOperation.Decrement
			new	UnaryOperator(ComplementOperation),			// UnaryOperation.Complement
			new	UnaryOperator(NotOperation),				// UnaryOperation.Not
		};

		/// <summary>
		/// Checks whether the expression result is not relative to a symbol.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> in which context the result is checked.</param>
		/// <param name="value">The result to check.</param>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed on a value which is relative to some symbol.
		/// </exception>
		private static void CheckNotRelativeToSymbol(UnaryExpression expression, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(value != null);
			#endregion

			if (value.HasReference)
			{
				throw new IllegalOperationEvaluationException(
					EvaluationExceptionStrings.IllegalOperationOnRelativeUnaryValue,
					expression);
			}
		}

		#region Unary Operations
		/// <summary>
		/// Negates the operand.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> under consideration.</param>
		/// <param name="value">The value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed on a value which is relative to some symbol.
		/// </exception>
		private static ExpressionResult NegateOperation(UnaryExpression expression, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			CheckNotRelativeToSymbol(expression, value);
			return new ExpressionResult(-value.Constant);
		}

		/// <summary>
		/// Positivates the operand.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> under consideration.</param>
		/// <param name="value">The value.</param>
		/// <returns>The resulting value.</returns>
		private static ExpressionResult PositivateOperation(UnaryExpression expression, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			return value;
		}

		/// <summary>
		/// Increments the operand.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> under consideration.</param>
		/// <param name="value">The value.</param>
		/// <returns>The resulting value.</returns>
		private static ExpressionResult IncrementOperation(UnaryExpression expression, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			return new ExpressionResult(value, value.Constant + 1);
		}

		/// <summary>
		/// Decrements the operand.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> under consideration.</param>
		/// <param name="value">The value.</param>
		/// <returns>The resulting value.</returns>
		private static ExpressionResult DecrementOperation(UnaryExpression expression, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			return new ExpressionResult(value, value.Constant - 1);
		}

		/// <summary>
		/// Calculates the bitwise complement of the operand.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> under consideration.</param>
		/// <param name="value">The value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed on a value which is relative to some symbol.
		/// </exception>
		private static ExpressionResult ComplementOperation(UnaryExpression expression, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			CheckNotRelativeToSymbol(expression, value);
			return new ExpressionResult(~value.Constant);
		}

		/// <summary>
		/// Calculates the NOT of the operand.
		/// </summary>
		/// <param name="expression">The <see cref="UnaryExpression"/> under consideration.</param>
		/// <param name="value">The value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed on a value which is relative to some symbol.
		/// </exception>
		private static ExpressionResult NotOperation(UnaryExpression expression, ExpressionResult value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			CheckNotRelativeToSymbol(expression, value);
			return new ExpressionResult(value.Constant != 0 ? 0 : 1);
		}
		#endregion
		#endregion

		#region Invariant
		/// <summary>
		/// The invariant for this type.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code contracts invariant method.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Code contracts invariant method.")]
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(UnaryOperation), this.operation));
			Contract.Invariant(this.operation != UnaryOperation.None);

			Contract.Invariant(this.expression != null);
		}
		#endregion
	}
}
