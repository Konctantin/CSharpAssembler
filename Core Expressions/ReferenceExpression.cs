using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// A reference to a label.
	/// </summary>
	public class ReferenceExpression : Expression
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceExpression"/> class, targeting a symbol.
		/// </summary>
		/// <param name="target">The target identifier.</param>
		public ReferenceExpression(string target)
			: base()
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(target != null);
			Contract.Requires<ArgumentNullException>(target.Length > 0);
			Contract.Ensures(this.targetIdentifier == target);
			#endregion

			this.targetIdentifier = target;
		}
		#endregion

		#region Properties
		private string targetIdentifier;
		/// <summary>
		/// Gets the identifier of the target <see cref="Symbol"/>.
		/// </summary>
		/// <value>The identifier of the <see cref="Symbol"/>.</value>
		/// <exception cref="ArgumentNullException">
		/// The value is <see langword="null"/>.
		/// </exception>
		public string TargetIdentifier
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				Contract.Ensures(Contract.Result<string>().Length > 0);
				#endregion

				return targetIdentifier;
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
			visitor.VisitReferenceExpression(this);
		}
		#endregion

		#region Evaluation
		/// <summary>
		/// Evaluates the expression in the given context.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this expression is found.</param>
		/// <returns>The result of the expression.</returns>
		public ExpressionResult Evaluate(IContext context)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Ensures(Contract.Result<ExpressionResult>() != null);
			#endregion

			ExpressionResult result;
			Symbol target = context.SymbolTable[this.targetIdentifier];
			if (target != null)
				result = new ExpressionResult(target);
			else
				result = new ExpressionResult(this.targetIdentifier);

			return result;
		}
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
			Contract.Invariant(this.targetIdentifier != null);
			Contract.Invariant(this.targetIdentifier.Length > 0);
		}
		#endregion
	}
}
