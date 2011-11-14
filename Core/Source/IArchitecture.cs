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

namespace SharpAssembler.Core
{
	/// <summary>
	/// A general interface for classes which describe a processor architecture.
	/// </summary>
	/// <remarks>
	/// Implementations of this interface may provide extra properties used to enable or disable certain features
	/// available in that processor's architecture.
	/// </remarks>
	[ContractClass(typeof(Contracts.IArchitectureContract))]
	public interface IArchitecture
	{
		/// <summary>
		/// Gets the name of the architecture.
		/// </summary>
		/// <value>The short, human readable name of the architecture.</value>
		string Name
		{ get; }

		/// <summary>
		/// Gets the default address size used by this architecture.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.</value>
		/// <remarks>
		/// The address size may be overrided for individual instructions, depending on the architecture.
		/// </remarks>
		DataSize AddressSize
		{ get; }

		/// <summary>
		/// Gets the default operand size used by this architecture.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.</value>
		/// <remarks>
		/// The operand size may be overrided for individual instructions, depending on the architecture.
		/// </remarks>
		DataSize OperandSize
		{ get; }

		/// <summary>
		/// Creates a new <see cref="Context"/> object which can be used to construct and encode an object file.
		/// </summary>
		/// <param name="objectfile">The <see cref="ObjectFile"/> for which the context is created.</param>
		/// <returns>An architecture specific <see cref="Context"/>.</returns>
		Context CreateContext(ObjectFile objectfile);
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IArchitecture"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IArchitecture))]
		abstract class IArchitectureContract : IArchitecture
		{
			public string Name
			{
				get
				{
					Contract.Ensures(Contract.Result<string>() != null);

					return default(string);
				}
			}

			public DataSize AddressSize
			{
				get
				{
					Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));

					return default(DataSize);
				}
			}

			public DataSize OperandSize
			{
				get
				{
					Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));

					return default(DataSize);
				}
			}

			public Context CreateContext(ObjectFile objectfile)
			{
				Contract.Requires<ArgumentNullException>(objectfile != null);
				Contract.Ensures(Contract.Result<Context>() != null);

				return default(Context);
			}
		}
	}
	#endregion
}
