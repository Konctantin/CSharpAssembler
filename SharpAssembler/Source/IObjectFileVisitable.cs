#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011-2012 Daniël Pelsmaeker
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
using System;
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// An interface implemented by objects which accept an <see cref="IObjectFileVisitor"/>.
	/// </summary>
	[ContractClass(typeof(Contracts.IObjectFileVisitableContract))]
	public interface IObjectFileVisitable
	{
		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The <see cref="IObjectFileVisitor"/> visiting.</param>
		void Accept(IObjectFileVisitor visitor);
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IObjectFileVisitable"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IObjectFileVisitable))]
		abstract class IObjectFileVisitableContract : IObjectFileVisitable
		{
			public void Accept(IObjectFileVisitor visitor)
			{
				Contract.Requires<ArgumentNullException>(visitor != null);
			}
		}
	}
	#endregion
}
