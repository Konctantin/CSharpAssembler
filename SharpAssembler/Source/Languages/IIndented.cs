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
using System;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages
{
	/// <summary>
	/// Represents a language that can use indentation.
	/// </summary>
	[ContractClass(typeof(Contracts.IIndentedContract))]
	public interface IIndented
	{
		/// <summary>
		/// Gets or sets the indentation string to use for each level.
		/// </summary>
		/// <value>A string.</value>
		string IndentString
		{ get; set; }

		/// <summary>
		/// Gets or sets whether to use indentation.
		/// </summary>
		/// <value><see langword="true"/> to use indentation;
		/// otherwise, <see langword="false"/>.</value>
		bool Indent
		{ get; set; }
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IIndented))]
		abstract class IIndentedContract : IIndented
		{
			public string IndentString
			{
				get
				{
					Contract.Ensures(Contract.Result<string>() != null);
					return default(string);
				}
				set
				{
					Contract.Requires<ArgumentNullException>(value != null);
				}
			}

			public bool Indent
			{
				get
				{
					return default(bool);
				}
				set
				{
				}
			}
		}
	}
	#endregion
}
