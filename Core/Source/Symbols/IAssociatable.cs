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

namespace SharpAssembler.Core.Symbols
{
	/// <summary>
	/// An interface for classes and structures that have a location in memory.
	/// </summary>
	[ContractClass(typeof(Contracts.IAssociatableContract))]
	public interface IAssociatable
	{
		/// <summary>
		/// Gets the <see cref="Symbol"/> associated with this <see cref="IAssociatable"/>.
		/// </summary>
		/// <value>A <see cref="Symbol"/>.</value>
		Symbol AssociatedSymbol
		{ get; }

		/// <summary>
		/// Gets the <see cref="IFile"/> in which this object is defined.
		/// </summary>
		/// <value>A <see cref="IFile"/>.</value>
		IFile ParentFile
		{ get; }
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
					Contract.Ensures(Contract.Result<Symbol>() != null);
					Contract.Ensures(Contract.Result<Symbol>().Association == this);

					return default(Symbol);
				}
			}

			public IFile ParentFile
			{
				get
				{
					Contract.Ensures(Contract.Result<IFile>() != null);

					return default(IFile);
				}
			}
		}
	}
	#endregion
}
