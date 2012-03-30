using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace SharpAssembler
{
	/// <summary>
	/// Extensions to the <see cref="Collection{T}"/> class.
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Adds a range of object to the collection.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="collection">The collection to add to.</param>
		/// <param name="values">The values to add.</param>
		public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> values)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(collection != null);
			Contract.Requires<ArgumentNullException>(values != null);
			#endregion
			foreach (var item in values)
			{
				collection.Add(item);
			}
		}

		/// <summary>
		/// Finds the first item in the collection that fulfulls the specified predicate.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="collection">The collection to search in.</param>
		/// <param name="predicate">The predicate to use.</param>
		public static T Find<T>(this Collection<T> collection, Predicate<T> predicate)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(collection != null);
			Contract.Requires<ArgumentNullException>(predicate != null);
			#endregion
			foreach (var item in collection)
			{
				if (predicate(item))
					return item;
			}
			return default(T);
		}
	}
}
