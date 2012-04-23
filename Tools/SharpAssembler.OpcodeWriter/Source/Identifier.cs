using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// An identifier.
	/// </summary>
	public class Identifier : IEquatable<Identifier>
	{
		/// <summary>
		/// The actual identifier.
		/// </summary>
		private readonly string value;

		/// <summary>
		/// Initializes a new instance of the <see cref="Identifier"/> class.
		/// </summary>
		/// <param name="value">The value of the identifier.</param>
		public Identifier(string value)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(value != null);
			#endregion
			this.value = value;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return value;
		}

		#region Equality
		/// <inheritdoc />
		public bool Equals(Identifier other)
		{
			if (Object.ReferenceEquals(other, null))
				return false;
			return this.value.Equals(other.value);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return Equals(obj as Identifier);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}
		#endregion

		#region Conversions
		/// <summary>
		/// Converts an <see cref="Identifier"/> to a string.
		/// </summary>
		/// <param name="identifier">The identifier to convert.</param>
		/// <returns>The corresponding string.</returns>
		public static implicit operator string(Identifier identifier)
		{
			return identifier.value;
		}

		/// <summary>
		/// Converts a string to an <see cref="Identifier"/>.
		/// </summary>
		/// <param name="identifier">The string to convert.</param>
		/// <returns>The corresponding <see cref="Identifier"/>.</returns>
		public static implicit operator Identifier(string identifier)
		{
			return new Identifier(identifier);
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.value != null);
		}
		#endregion
	}
}
