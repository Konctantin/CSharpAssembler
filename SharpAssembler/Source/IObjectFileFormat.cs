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

namespace SharpAssembler
{
	/// <summary>
	/// Represents an object file format.
	/// </summary>
	[ContractClass(typeof(Contracts.IObjectFileFormatContract))]
	public interface IObjectFileFormat
	{
		/// <summary>
		/// Gets the name of the object file format.
		/// </summary>
		/// <value>The name of the object file format.</value>
		string Name
		{ get; }

		/// <summary>
		/// Gets a factory that can be used to create new <see cref="Section"/> objects for the object file format.
		/// </summary>
		/// <value>A <see cref="SectionFactory"/>.</value>
		SectionFactory SectionFactory
		{ get; }

		#region Support
		/// <summary>
		/// Returns whether this <see cref="ObjectFile"/> can contain binary data for the specified architecture.
		/// </summary>
		/// <param name="architecture">The <see cref="IArchitecture"/> to test.</param>
		/// <returns><see langword="true"/> when this <see cref="ObjectFile"/> can contain binary data for
		/// <paramref name="architecture"/>; otherwise, <see langword="false"/>.</returns>
		[Pure]
		bool IsSupportedArchitecture(IArchitecture architecture);

		/// <summary>
		/// Checks whether the object file format supports the specified feature.
		/// </summary>
		/// <param name="feature">The <see cref="ObjectFileFeature"/> to test.</param>
		/// <returns><see langword="true"/> when it is supported; otherwise, <see langword="false"/>.</returns>
		[Pure]
		bool SupportsFeature(ObjectFileFeature feature);
		#endregion

		/// <summary>
		/// Creates a new object file of the specified object file format.
		/// </summary>
		/// <param name="architecture">The architecture of the machine code that will be stored
		/// in the object file.</param>
		/// <returns>The created <see cref="ObjectFile"/>.</returns>
		ObjectFile CreateObjectFile(IArchitecture architecture);

		/// <summary>
		/// Creates a new object file of the specified object file format.
		/// </summary>
		/// <param name="architecture">The architecture of the machine code that will be stored
		/// in the object file.</param>
		/// <param name="name">The name of the object file; or <see langword="null"/> to specify no name.</param>
		/// <returns>The created <see cref="ObjectFile"/>.</returns>
		ObjectFile CreateObjectFile(IArchitecture architecture, string name);

		/// <summary>
		/// Creates a new assembler for the specified object file
		/// with the default configuration.
		/// </summary>
		/// <param name="objectFile">The <see cref="ObjectFile"/> to be assembled.</param>
		/// <returns>The created assembler.</returns>
		ObjectFileAssembler CreateAssembler(ObjectFile objectFile);
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IObjectFileFormat))]
		abstract class IObjectFileFormatContract : IObjectFileFormat
		{
			public string Name
			{
				get
				{
					Contract.Ensures(Contract.Result<string>() != null);
					return default(string);
				}
			}

			public SectionFactory SectionFactory
			{
				get
				{
					Contract.Ensures(Contract.Result<SectionFactory>() != null);
					return default(SectionFactory);
				}
			}

			public bool IsSupportedArchitecture(IArchitecture architecture)
			{
				Contract.Requires<ArgumentNullException>(architecture != null);
				return default(bool);
			}


			public bool SupportsFeature(ObjectFileFeature feature)
			{
				Contract.Requires<ArgumentException>(feature != ObjectFileFeature.None);
				return default(bool);
			}

			public ObjectFile CreateObjectFile(IArchitecture architecture)
			{
				Contract.Requires<ArgumentNullException>(architecture != null);
				return default(ObjectFile);
			}

			public ObjectFile CreateObjectFile(IArchitecture architecture, string name)
			{
				Contract.Requires<ArgumentNullException>(architecture != null);
				return default(ObjectFile);
			}

			public ObjectFileAssembler CreateAssembler(ObjectFile objectFile)
			{
				Contract.Requires<ArgumentNullException>(objectFile != null);
				Contract.Ensures(Contract.Result<ObjectFileAssembler>() != null);
				return default(ObjectFileAssembler);
			}
		}
	}
	#endregion
}
