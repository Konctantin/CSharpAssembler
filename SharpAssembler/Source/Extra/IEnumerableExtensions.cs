using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// Extensions for the <see cref="IEnumerable{T}"/> type.
	/// </summary>
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Returns only those elements from the sequence that are not <see langword="null"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements.</typeparam>
		/// <param name="enumerable">The enumerable sequence.</param>
		/// <returns>The enumerable sequence without any <see langword="null"/> elements.</returns>
		public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> enumerable)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(enumerable != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<T>>(), v => v != null));
			#endregion
			return enumerable.Where(v => v != null);
		}
	}
}
