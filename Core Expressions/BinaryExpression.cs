using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// This binary expression takes two operands and performs some operation on them.
	/// </summary>
	public class BinaryExpression : Expression
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryExpression"/>
		/// class.
		/// </summary>
		/// <param name="left">The left-hand expression.</param>
		/// <param name="operation">The binary operation to perform.</param>
		/// <param name="right">The right-hand expression.</param>
		public BinaryExpression(Expression left, BinaryOperation operation, Expression right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(BinaryOperation), operation));
			Contract.Requires<ArgumentException>(operation != BinaryOperation.None);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(this.leftHandExpression == left);
			Contract.Ensures(this.rightHandExpression == right);
			Contract.Ensures(this.operation == operation);
			#endregion

			this.leftHandExpression = left;
			this.operation = operation;
			this.rightHandExpression = right;
		}
		#endregion

		#region Properties
		private BinaryOperation operation;
		/// <summary>
		/// Gets or sets the operation to perform on the operands.
		/// </summary>
		/// <value>The binary operation to perform.</value>
		public BinaryOperation Operation
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<BinaryOperation>() != BinaryOperation.None);
				Contract.Ensures(Enum.IsDefined(typeof(BinaryOperation), Contract.Result<BinaryOperation>()));
				#endregion

				return operation;
			}
		}

		private Expression leftHandExpression;
		/// <summary>
		/// Gets the left hand expression of this binary expression.
		/// </summary>
		/// <value>An <see cref="Expression"/>.</value>
		public Expression LeftHandExpression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Expression>() != null);
				#endregion

				return leftHandExpression;
			}
		}

		private Expression rightHandExpression;
		/// <summary>
		/// Gets the right hand expression of this binary expression.
		/// </summary>
		/// <value>An <see cref="Expression"/>.</value>
		public Expression RightHandExpression
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Expression>() != null);
				#endregion

				return rightHandExpression;
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
			visitor.VisitBinaryExpression(this);
		}
		#endregion

		#region Evaluation
		/// <summary>
		/// Evaluates the expression in the given context.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this expression is found;
		/// or <see langword="null"/> when there is no context for this expression.</param>
		/// <param name="left">The left-hand expression result.</param>
		/// <param name="right">The right-hand expression result.</param>
		/// <returns>The result of the expression.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when both values are relative to some symbol.
		/// </exception>
		/// <exception cref="SymbolEvaluationException">
		/// The two symbols on which this operation is performed are from two different sections of the file.
		/// </exception>
		public ExpressionResult Evaluate(IContext context, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			// NOTE: There is no requirement that 'context' must be non-null.
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			ExpressionResult result = BinaryOperations[(int)operation](this, left, right);
			Contract.Assume(result != null);
			return result;
		}

		/// <summary>
		/// A delegate for binary operations.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		private delegate ExpressionResult BinaryOperator(BinaryExpression expression, ExpressionResult left, ExpressionResult right);

		/// <summary>
		/// An ordered list of methods to invoke for each possible binary operation.
		/// </summary>
		private static readonly BinaryOperator[] BinaryOperations =
		{
			null,											// BinaryOperation.None,
			new	BinaryOperator(BitwiseOrOperation),			// BinaryOperation.BitwiseOr
			new	BinaryOperator(BitwiseXOrOperation),		// BinaryOperation.BitwiseXOr
			new	BinaryOperator(BitwiseAndOperation),		// BinaryOperation.BitwiseAnd
			new	BinaryOperator(LeftShiftOperation),			// BinaryOperation.LeftShift
			new	BinaryOperator(RightShiftOperation),		// BinaryOperation.RightShift
			new	BinaryOperator(AddOperation),				// BinaryOperation.Add
			new	BinaryOperator(SubtractOperation),			// BinaryOperation.Subtract
			new	BinaryOperator(MultiplyOperation),			// BinaryOperation.Multiply
			new	BinaryOperator(DivideUnsignedOperation),	// BinaryOperation.DivideUnsigned
			new	BinaryOperator(DivideSignedOperation),		// BinaryOperation.DivideSigned
			new	BinaryOperator(ModuloUnsignedOperation),	// BinaryOperation.ModuloUnsigned
			new	BinaryOperator(ModuloSignedOperation),		// BinaryOperation.ModuloSigned
		};

		/// <summary>
		/// Checks whether both the left and right expression results are not relative to a symbol.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> in which context the left and right
		/// results are checked.</param>
		/// <param name="left">The left result to check.</param>
		/// <param name="right">The right result to check.</param>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static void CheckNotRelativeToSymbol(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			#endregion

			if (left.HasReference || right.HasReference)
			{
				throw new IllegalOperationEvaluationException(
					EvaluationExceptionStrings.IllegalOperationOnRelativeBinaryValue,
					expression);
			}
		}

		#region Binary Operations
		/// <summary>
		/// Performs a bitwise OR on the two operands.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult BitwiseOrOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult(left.Constant | right.Constant);
		}

		/// <summary>
		/// Performs a bitwise XOR on the two operands.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult BitwiseXOrOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult(left.Constant ^ right.Constant);
		}

		/// <summary>
		/// Performs a bitwise AND on the two operands.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult BitwiseAndOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult(left.Constant & right.Constant);
		}

		/// <summary>
		/// Left shifts the left operand by the number of bits specified in the right operand.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult LeftShiftOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			// Shifts are always unsigned.
			return new ExpressionResult((long)((ulong)left.Constant << (int)right.Constant));
		}

		/// <summary>
		/// Right shifts the left operand by the number of bits specified in the right operand.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult RightShiftOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			// Shifts are always unsigned.
			return new ExpressionResult((long)((ulong)left.Constant >> (int)right.Constant));
		}

		/// <summary>
		/// Multiplies the left operand by the right operand.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult MultiplyOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult(left.Constant * right.Constant);
		}

		/// <summary>
		/// Divides the left operand by the right operand, unsigned.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult DivideUnsignedOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult((long)((ulong)left.Constant / (ulong)right.Constant));
		}

		/// <summary>
		/// Divides the left operand by the right operand, signed.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult DivideSignedOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult(left.Constant / right.Constant);
		}

		/// <summary>
		/// Calculates the remainder of an unsigned division of the left operand by the right operand.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult ModuloUnsignedOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult((long)((ulong)left.Constant % (ulong)right.Constant));
		}

		/// <summary>
		/// Calculates the remainder of a signed division of the left operand by the right operand.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when any of the values is relative to some symbol.
		/// </exception>
		private static ExpressionResult ModuloSignedOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			CheckNotRelativeToSymbol(expression, left, right);
			#endregion

			return new ExpressionResult(left.Constant % right.Constant);
		}

		/// <summary>
		/// Adds the right hand operand to the left hand operand.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="IllegalOperationEvaluationException">
		/// This operation cannot be performed when both values are relative to some symbol.
		/// </exception>
		private static ExpressionResult AddOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			// It is not allowed to add two values which are both relative to a section.
			if (left.HasReference && right.HasReference)
			{
				throw new IllegalOperationEvaluationException(
					EvaluationExceptionStrings.IllegalOperationOnTwoRelativeBinaryValues, expression);
			}

			// Adding one result to another, where none or only one of the results
			// is relative to a section, results in a result which is relative to that same section.
			ExpressionResult referenced = (left.HasReference ? left : right);

			return new ExpressionResult(referenced, left.Constant + right.Constant);
		}

		/// <summary>
		/// Subtracts the right hand operand from the left hand operand.
		/// </summary>
		/// <param name="expression">The <see cref="BinaryExpression"/> under consideration.</param>
		/// <param name="left">The left hand value.</param>
		/// <param name="right">The right hand value.</param>
		/// <returns>The resulting value.</returns>
		/// <exception cref="SymbolEvaluationException">
		/// The two symbols on which this operation is performed are from two different sections of the file.
		/// </exception>
		private static ExpressionResult SubtractOperation(BinaryExpression expression, ExpressionResult left, ExpressionResult right)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(expression != null);
			Contract.Requires<ArgumentNullException>(left != null);
			Contract.Requires<ArgumentNullException>(right != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			ExpressionResult result;
			if (left.HasResolvedReference && right.HasResolvedReference)
			{
				// Subtracting two results which are relative to the same section
				// results in only the difference between the two constants.
				if (left.Reference.DefiningSection != right.Reference.DefiningSection)
				{
					throw new SymbolEvaluationException(
						EvaluationExceptionStrings.SymbolsInDifferentSections, expression);
				}

				result = new ExpressionResult(left.Reference.Address - right.Reference.Address);
			}
			else if (left.HasReference && right.HasReference)
			{
				// Subtracting two results where both have a reference but one or both of them are not resolved
				// is not allowed.
				throw new IllegalOperationEvaluationException(
					EvaluationExceptionStrings.UnresolvedSymbol, expression);
			}
			else
			{
				// Subtracting one result from another, where none or only one of the results
				// is relative to a section, results in a result which is relative to that same section.
				ExpressionResult referenced = (left.HasReference ? left : right);

				result = new ExpressionResult(referenced, left.Constant - right.Constant);
			}

			return result;
		}
		#endregion
		#endregion

		#region Invariant
		/// <summary>
		/// The invariant for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(BinaryOperation), this.operation));
			Contract.Invariant(this.operation != BinaryOperation.None);

			Contract.Invariant(this.leftHandExpression != null);
			Contract.Invariant(this.rightHandExpression != null);
		}
		#endregion
	}
}
