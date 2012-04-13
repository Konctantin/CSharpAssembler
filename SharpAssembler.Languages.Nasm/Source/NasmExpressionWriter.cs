using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.IO;
using System.Diagnostics.Contracts;
using System.Globalization;
using SharpAssembler.Symbols;

namespace SharpAssembler.Languages.Nasm
{
	/// <summary>
	/// Writes NASM expressions.
	/// </summary>
	public class NasmExpressionWriter : ExpressionVisitor
	{
		private readonly TextWriter writer;
		/// <summary>
		/// Gets the text writer to which the expression is written.
		/// </summary>
		/// <value>The <see cref="TextWriter"/> to use.</value>
		protected TextWriter Writer
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<TextWriter>() != null);
				#endregion
				return this.writer;
			}
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="NasmExpressionWriter"/> class.
		/// </summary>
		/// <param name="writer">The writer to which the expression is written.</param>
		public NasmExpressionWriter(TextWriter writer)
			: base()
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion
			this.writer = writer;
		}
		#endregion

		/// <summary>
		/// Writes the textual representation of the specified expression to the <see cref="Writer"/>.
		/// </summary>
		/// <param name="expression">The expression to write.</param>
		/// <remarks>
		/// This is a convenience method fort he functionality provided by the <see cref="Visit"/> method.
		/// </remarks>
		public void Write(Expression<Func<Context, ReferenceOffset>> expression)
		{
			this.Visit((Expression)expression);
		}

		/// <inheritdoc />
		protected override Expression VisitBinary(BinaryExpression node)
		{
			Visit(node.Left);
			switch (node.NodeType)
			{
				case ExpressionType.Or: this.writer.Write(" | "); break;
				case ExpressionType.ExclusiveOr: this.writer.Write(" ^ "); break;
				case ExpressionType.And: this.writer.Write(" & "); break;

				case ExpressionType.LeftShift: this.writer.Write(" << "); break;
				case ExpressionType.RightShift: this.writer.Write(" >> "); break;

				case ExpressionType.Add: this.writer.Write(" + "); break;
				case ExpressionType.Subtract: this.writer.Write(" - "); break;

				case ExpressionType.Multiply: this.writer.Write(" * "); break;

				case ExpressionType.Divide:
					{
						bool? unsigned = IsUnsigned(node.Left, node.Right);
						if (!unsigned.HasValue)
							throw new InvalidOperationException(String.Format("Weird expressions: ({0}, {1})", node.Left, node.Right));
						if (unsigned.Value)
							this.writer.Write(" / ");
						else
							this.writer.Write(" // ");
						break;
					}
				case ExpressionType.Modulo:
					{
						bool? unsigned = IsUnsigned(node.Left, node.Right);
						if (!unsigned.HasValue)
							throw new InvalidOperationException(String.Format("Weird expressions: ({0}, {1})", node.Left, node.Right));
						if (unsigned.Value)
							this.writer.Write(" % ");
						else
							this.writer.Write(" %% ");
						break;
					}

				default:
					throw new NotSupportedException(String.Format("Unknown or unsupported binary operator {0}.", Enum.GetName(typeof(ExpressionType), node.NodeType)));
			}
			Visit(node.Right);
			return node;
		}

		/// <inheritdoc />
		protected override Expression VisitUnary(UnaryExpression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.Negate: this.writer.Write("+"); break;
				case ExpressionType.Not: this.writer.Write("~"); break;
				case ExpressionType.Convert: break;
				default:
					throw new NotSupportedException(String.Format("Unknown or unsupported unary operator {0}.", Enum.GetName(typeof(ExpressionType), node.NodeType)));
			}
			Visit(node.Operand);
			return node;
		}

		private bool? IsUnsigned(Expression left, Expression right)
		{
			bool? leftunsign = IsUnsigned(left);
			bool? rightunsign = IsUnsigned(right);
			if (!leftunsign.HasValue || !rightunsign.HasValue)
				return null;
			bool leftunsign2 = leftunsign.Value;
			bool rightunsign2 = rightunsign.Value;

			if (leftunsign.Value == rightunsign.Value)
				// When both expressions are unsigned, the binary expression is unsigned.
				// When both expressions are signed, the binary expression is signed.
				return rightunsign.Value;
			else
				// When one of the expressions is unsigned and the other signed,
				// they could be cast to two bigger signed values, resulting in a signed expression.
				// However, this is a dubious case.
				return false;
		}

		/// <summary>
		/// Determines whether the expression is unsigned.
		/// </summary>
		/// <param name="expr">The expression to test.</param>
		/// <returns><see langword="true"/> when it is unsigned;
		/// <see langword="false"/> when it is signed;
		/// <see langword="null"/> when it is unknown.</returns>
		private bool? IsUnsigned(Expression expr)
		{
			if (expr.NodeType == ExpressionType.Convert)
				return IsUnsigned(((UnaryExpression)expr).Operand.Type);
			else
				return IsUnsigned(expr.Type);
		}

		/// <summary>
		/// Determines whether the type is unsigned.
		/// </summary>
		/// <param name="type">The type to test.</param>
		/// <returns><see langword="true"/> when it is unsigned;
		/// <see langword="false"/> when it is signed;
		/// <see langword="null"/> when it is unknown.</returns>
		private bool? IsUnsigned(Type type)
		{
			if (typeof(Int128).IsAssignableFrom(type) ||
				typeof(Int64).IsAssignableFrom(type) ||
				typeof(Int32).IsAssignableFrom(type) ||
				typeof(Int16).IsAssignableFrom(type) ||
				typeof(SByte).IsAssignableFrom(type))
				return false;
			if (typeof(UInt128).IsAssignableFrom(type) ||
				typeof(UInt64).IsAssignableFrom(type) ||
				typeof(UInt32).IsAssignableFrom(type) ||
				typeof(UInt16).IsAssignableFrom(type) ||
				typeof(Byte).IsAssignableFrom(type))
				return true;
			return null;
		}

		/// <inheritdoc />
		protected override Expression VisitConstant(ConstantExpression node)
		{
			if (typeof(string).IsAssignableFrom(node.Type))
			{
				// A string in an expression, probably a symbol reference.
				this.writer.Write(node.Value as String);
			}
			else
			{
				Int128? value = AsIntegerValue(node.Value);
				if (!value.HasValue)
					throw new NotSupportedException(String.Format("Constants of type {0} are not supported.", node.Value.GetType()));

				if (value > 15)
					this.writer.Write("0x" + value.Value.ToString("X", CultureInfo.InvariantCulture));
				else
					this.writer.Write(value.Value);
			}
			
			return base.VisitConstant(node);
		}

		/// <inheritdoc />
		protected override Expression VisitBlock(BlockExpression node)
		{
			throw new NotSupportedException("Block expressions are not supported.");
		}

		/// <inheritdoc />
		protected override Expression VisitTry(TryExpression node)
		{
			throw new NotSupportedException("Exception handling is not supported.");
		}

		/// <inheritdoc />
		protected override CatchBlock VisitCatchBlock(CatchBlock node)
		{
			throw new NotSupportedException("Exception handling is not supported.");
		}

		/// <inheritdoc />
		protected override Expression VisitMember(MemberExpression node)
		{
			if (typeof(Context).IsAssignableFrom(node.Member.DeclaringType) &&
				node.Member.Name.Equals("Address"))
			{
				Visit(node.Expression);		// c
				this.writer.Write("$");		// c.Address
			}
			else if (typeof(Context).IsAssignableFrom(node.Member.DeclaringType) &&
				node.Member.Name.Equals("Section"))
			{
				Visit(node.Expression);		// c
			}
			else if (typeof(Section).IsAssignableFrom(node.Member.DeclaringType) &&
				node.Member.Name.Equals("Address"))
			{
				Visit(node.Expression);		// c.Section
				this.writer.Write("$$");	// c.Section.Address
			}
			else
				throw new NotSupportedException(String.Format("References to class members are not supported: {0}", node.Member));

			return node;
		}

		/// <inheritdoc />
		protected override Expression VisitSwitch(SwitchExpression node)
		{
			throw new NotSupportedException("Switch statements are not supported.");
		}

		/// <inheritdoc />
		protected override SwitchCase VisitSwitchCase(SwitchCase node)
		{
			throw new NotSupportedException("Switch statements are not supported.");
		}

		/// <inheritdoc />
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			throw new NotSupportedException("Method calls are not supported.");
		}

		/// <inheritdoc />
		protected override Expression VisitDebugInfo(DebugInfoExpression node)
		{
			// Debug info is not written.
			return base.VisitDebugInfo(node);
		}

		/// <inheritdoc />
		protected override Expression VisitConditional(ConditionalExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitDefault(DefaultExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitDynamic(DynamicExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override ElementInit VisitElementInit(ElementInit node)
		{
			throw new NotSupportedException();
		}
		/// <inheritdoc />
		protected override Expression VisitExtension(Expression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitGoto(GotoExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitIndex(IndexExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitInvocation(InvocationExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitLabel(LabelExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override LabelTarget VisitLabelTarget(LabelTarget node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			return base.VisitLambda(node);
		}

		/// <inheritdoc />
		protected override Expression VisitListInit(ListInitExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitLoop(LoopExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override MemberBinding VisitMemberBinding(MemberBinding node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitMemberInit(MemberInitExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitNew(NewExpression node)
		{
			if (typeof(Reference).IsAssignableFrom(node.Type))
			{
				if (typeof(string).IsAssignableFrom(node.Constructor.GetParameters()[0].ParameterType))
				{
					Visit(node.Arguments[0]);
					return node;
				}
				else
					throw new NotSupportedException("The Reference-constructor accepting a Symbol is not supported.");
			}
			else
				throw new NotSupportedException("Cannot create new objects in the expression.");
		}

		/// <inheritdoc />
		protected override Expression VisitNewArray(NewArrayExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitParameter(ParameterExpression node)
		{
			// We know the parameters to the lambda function,
			// so nothing to do here.
			return base.VisitParameter(node);
		}

		/// <inheritdoc />
		protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override Expression VisitTypeBinary(TypeBinaryExpression node)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns the specified object as a 128-bit signed value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The corresponding 128-bit signed value; or <see langword="null"/>
		/// when the value could not be converted.</returns>
		[Pure]
		private static Int128? AsIntegerValue(object value)
		{
			Int128? valueu128 = value as Int128?;
			if (valueu128 != null)
				return valueu128;
			ulong? valueu64 = value as ulong?;
			if (valueu64 != null)
				return (Int128)valueu64;
			long value64;
			try
			{
				value64 = Convert.ToInt64(value);
			}
			catch (FormatException)
			{
				return null;
			}
			catch (InvalidCastException)
			{
				return null;
			}
			catch (OverflowException)
			{
				return null;
			}
			return (Int128)value64;
		}
	}
}
