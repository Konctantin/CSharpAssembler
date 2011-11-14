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

namespace SharpAssembler.Core
{
	/// <summary>
	/// An interface for classes which represent an input or main source file.
	/// </summary>
	[ContractClass(typeof(Contracts.IFileContract))]
	public interface IFile
	{
		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		string Name
		{ get; }
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IFile"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IFile))]
		abstract class IFileContract : IFile
		{
			public string Name
			{
				get
				{
					Contract.Ensures(Contract.Result<string>() != null);
					Contract.Ensures(Contract.Result<string>().Length > 0);

					return default(string);
				}
			}
		}
	}
	#endregion
}
