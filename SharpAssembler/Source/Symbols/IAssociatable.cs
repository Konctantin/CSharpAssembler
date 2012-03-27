#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011 Daniël Pelsmaeker
 * 
 * This file is part of SharpAssembler.
 * 
 * SharpAssembler is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpAssembler is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SharpAssembler.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion
using System.Diagnostics.Contracts;
using System;

namespace SharpAssembler.Symbols
{
	/// <summary>
	/// Represents a structure that has an associated symbol.
	/// </summary>
	[ContractClass(typeof(Contracts.IAssociatableContract))]
	public interface IAssociatable
	{
		/// <summary>
		/// Gets the <see cref="Symbol"/> associated with this <see cref="IAssociatable"/>.
		/// </summary>
		/// <value>A <see cref="Symbol"/>.</value>
		/// <remarks>
		/// If an implementation provides a setter for this property,
		/// call the static <see cref="Symbol.SetAssociation(IAssociatable, Symbol)"/> method.
		/// </remarks>
		Symbol AssociatedSymbol
		{ get; }

		/// <summary>
		/// Gets the <see cref="IFile"/> in which this object is defined.
		/// </summary>
		/// <value>A <see cref="IFile"/>.</value>
		IFile ParentFile
		{ get; }

		/// <summary>
		/// Sets the symbol that is associated with this object.
		/// </summary>
		/// <param name="symbol">The associated symbol; or <see langword="null"/> when no symbol is associated
		/// with this object.</param>
		/// <remarks>
		/// Implementations should only set the associated symbol in this class.
		/// This method should be implemented explicitly.
		/// </remarks>
		/// <exception cref="System.NotSupportedException">
		/// The associated symbol cannot be set.
		/// </exception>
		void SetAssociatedSymbol(Symbol symbol);
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IAssociatable"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IAssociatable))]
		abstract class IAssociatableContract : IAssociatable
		{
			public Symbol AssociatedSymbol
			{
				get
				{
					return default(Symbol);
				}
			}

			public IFile ParentFile
			{
				get
				{
					return default(IFile);
				}
			}

			public void SetAssociatedSymbol(Symbol symbol)
			{

			}
		}
	}
	#endregion
}
