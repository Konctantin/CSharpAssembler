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

namespace SharpAssembler.Formats.Bin
{
	/// <summary>
	/// A plain binary object file format.
	/// </summary>
	public class BinObjectFileFormat : IObjectFileFormat
	{
		/// <inheritdoc />
		public string Name
		{
			get { return "BIN"; }
		}

		private readonly SectionFactory sectionFactory = new SectionFactory();
		/// <inheritdoc />
		public SectionFactory SectionFactory
		{
			get { return this.sectionFactory; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="BinObjectFileFormat"/> class.
		/// </summary>
		public BinObjectFileFormat()
		{

		}
		#endregion

		/// <inheritdoc />
		public bool IsSupportedArchitecture(IArchitecture architecture)
		{
			var x86arch = architecture as SharpAssembler.Architectures.X86.X86Architecture;
			if (x86arch == null)
				return false;
			if (x86arch.AddressSize != DataSize.Bit64 &&
				x86arch.AddressSize != DataSize.Bit32 &&
				x86arch.AddressSize != DataSize.Bit16)
				return false;
			return true;
		}

		/// <inheritdoc />
		public bool SupportsFeature(ObjectFileFeature feature)
		{
			switch (feature)
			{
				case ObjectFileFeature.ArbitraryPhysicalStart:
					return true;
				default:
					return false;
			}
		}

		/// <inheritdoc />
		public ObjectFile CreateObjectFile(IArchitecture architecture)
		{
			return CreateObjectFile(architecture, null);
		}

		/// <inheritdoc />
		public ObjectFile CreateObjectFile(IArchitecture architecture, string name)
		{
			return new BinObjectFile(this, architecture, name);
		}

		/// <inheritdoc />
		public ObjectFileAssembler CreateAssembler(ObjectFile objectFile)
		{
			return new BinObjectFileAssembler(objectFile);
		}
	}
}
