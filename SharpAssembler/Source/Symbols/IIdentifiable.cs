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

namespace SharpAssembler.Symbols
{
	/// <summary>
	/// An interface for classes which provide an identifier.
	/// </summary>
	[ContractClass(typeof(Contracts.IIdentifiableContract))]
	public interface IIdentifiable : IAssociatable
	{
		/// <summary>
		/// Gets the identifier of this <see cref="IIdentifiable"/>.
		/// </summary>
		/// <value>An identifier, never <see langword="null"/>.</value>
		string Identifier
		{ get; }
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IAssociatable"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IIdentifiable))]
		abstract class IIdentifiableContract : IIdentifiable
		{
			public string Identifier
			{
				get
				{
					Contract.Ensures(Contract.Result<string>() != null);

					return default(string);
				}
			}

			public abstract Symbol AssociatedSymbol { get; }
			public abstract IFile ParentFile { get; }
			
			public abstract void SetAssociatedSymbol(Symbol symbol);
		}
	}
	#endregion
}
