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
using System.Collections;
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// An interface for classes and structures which can store data specific to the object.
	/// </summary>
	[ContractClass(typeof(Contracts.IAnnotatableContract))]
	public interface IAnnotatable
	{
		/// <summary>
		/// Gets a dictionary which may be used to store data specific to this object.
		/// </summary>
		/// <value>An implementation of the <see cref="IDictionary"/> interface.</value>
		/// <remarks>
		/// This property is not serialized or deserialized.
		/// </remarks>
		IDictionary Annotations
		{ get; }
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IAnnotatable"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IAnnotatable))]
		abstract class IAnnotatableContract : IAnnotatable
		{
			public IDictionary Annotations
			{
				get
				{
					Contract.Ensures(Contract.Result<IDictionary>() != null);

					return default(IDictionary);
				}
			}
		}
	}
	#endregion
}
