using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Core.Expressions
{
	/// <summary>
	/// A base class for classes which represent a part of an expression.
	/// </summary>
	[ContractClass(typeof(Contracts.ExpressionContract))]
	public abstract class Expression
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Expression"/> class.
		/// </summary>
		protected Expression()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Accepts a visitor.
		/// </summary>
		/// <param name="visitor">The <see cref="ExpressionVisitor"/> to accept.</param>
		/// <remarks>
		/// All implementations should call the appropriate <c>Visit*()</c> method on <paramref name="visitor"/>.
		/// </remarks>
		public abstract void Accept(ExpressionVisitor visitor);

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current expression.
		/// </summary>
		/// <returns>
		/// A <see cref="String"/> that represents the current expression.
		/// </returns>
		public override string ToString()
		{
			#region Contract
			Contract.Ensures(Contract.Result<string>() != null);
			#endregion

			SimpleExpressionPrettyPrinter printer = new SimpleExpressionPrettyPrinter();
			this.Accept(printer);
			return printer.ToString();
		}
		#endregion
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="Expression"/> type.
		/// </summary>
		[ContractClassFor(typeof(Expression))]
		abstract class ExpressionContract : Expression
		{
			public override void Accept(ExpressionVisitor visitor)
			{
				Contract.Requires<ArgumentNullException>(visitor != null);
			}
		}
	}
	#endregion
}
