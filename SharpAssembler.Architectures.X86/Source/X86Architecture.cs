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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using SharpAssembler;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Describes the x86 processor architecture.
	/// </summary>
	public class X86Architecture : IArchitecture
	{
		#region Fields
		/// <summary>
		/// The default CPU type.
		/// </summary>
		private static readonly CpuType DefaultCpuType = CpuType.IntelSandyBridge;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="X86Architecture"/> class.
		/// </summary>
		/// <remarks>
		/// The <see cref="CpuType"/> is set to <see cref="SharpAssembler.Architectures.X86.CpuType.IntelSandyBridge"/>.
		/// </remarks>
		public X86Architecture()
			: this(null, CpuFeatures.None, DataSize.None)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Architecture"/> class.
		/// </summary>
		/// <param name="type">The type of CPU.</param>
		/// <remarks>
		/// The <see cref="Features"/> are set according to the selected <paramref name="type"/>.
		/// </remarks>
		public X86Architecture(CpuType type)
			: this(type, CpuFeatures.None, DataSize.None)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Architecture"/> class.
		/// </summary>
		/// <param name="features">The features of the CPU.</param>
		public X86Architecture(CpuFeatures features)
			: this(null, features, DataSize.None)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Architecture"/> class.
		/// </summary>
		/// <param name="type">The type of CPU.</param>
		/// <param name="addressingMode">The addressing mode to use.</param>
		/// <remarks>
		/// The <see cref="Features"/> are set according to the selected <paramref name="type"/>.
		/// </remarks>
		public X86Architecture(CpuType type, DataSize addressingMode)
			: this(type, CpuFeatures.None, addressingMode)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), addressingMode));
			Contract.Requires<ArgumentException>(addressingMode == DataSize.None || IsValidAddressSize(type, addressingMode),
				"Specify a valid address size for this architecture and CPU type.");
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Architecture"/> class.
		/// </summary>
		/// <param name="features">The features of the CPU.</param>
		/// <param name="addressingMode">The addressing mode to use.</param>
		public X86Architecture(CpuFeatures features, DataSize addressingMode)
			: this(null, features, addressingMode)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), addressingMode));
			Contract.Requires<ArgumentException>(addressingMode == DataSize.None || IsValidAddressSize(null, addressingMode),
				"Specify a valid address size for this architecture.");
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Architecture"/> class.
		/// </summary>
		/// <param name="type">The type of CPU.</param>
		/// <param name="features">The features of the CPU.</param>
		/// <param name="addressingMode">The addressing mode to use.</param>
		/// <param name="ripRelative">Whether to use RIP-relative addressing by default.
		/// The default is <see langword="false"/>.</param>
		/// <remarks>
		/// The <see cref="Features"/> are set according to the selected <paramref name="type"/>, bitwise OR-ed with
		/// <paramref name="features"/>.
		/// </remarks>
		public X86Architecture(CpuType type, CpuFeatures features, DataSize addressingMode, bool ripRelative = false)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), addressingMode));
			Contract.Requires<ArgumentException>(addressingMode == DataSize.None || IsValidAddressSize(type, addressingMode),
				"Specify a valid address size for this architecture and CPU type.");
			#endregion

			// When none of the parameters (type, features) are specified, use the default
			// CPU type.
			if (type == null && features == CpuFeatures.None)
				type = DefaultCpuType;

			if (addressingMode == DataSize.None)
				addressingMode = GetDefaultAddressingMode(type);

			this.cpuType = type;
			this.features = features;
			this.addressSize = addressingMode;
			this.operandSize = addressingMode;
			this.useRIPRelativeAddressing = ripRelative;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name of the architecture.
		/// </summary>
		/// <value>The short, human readable name of the architecture.</value>
		public string Name
		{
			get
			{
				if (cpuType != null)
					return String.Format(CultureInfo.InvariantCulture, "{0} (x86-64)", cpuType.Name);
				else
					return "x86-64 architecture";
			}
		}

		private DataSize addressSize;
		/// <summary>
		/// Gets the default address size used by this architecture.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.</value>
		/// <remarks>
		/// The address size may be overridden by individual instructions.
		/// </remarks>
		public DataSize AddressSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				Contract.Ensures(IsValidAddressSize(this.CpuType, Contract.Result<DataSize>()));
				#endregion
				return addressSize;
			}
		}

		private DataSize operandSize;
		/// <summary>
		/// Gets the default operand size used by this architecture.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.</value>
		/// <remarks>
		/// The operand size may be overridden by individual instructions.
		/// </remarks>
		public DataSize OperandSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				Contract.Ensures(IsValidOperandSize(this.CpuType, Contract.Result<DataSize>()));
				#endregion
				return operandSize;
			}
		}

		private CpuType cpuType;
		/// <summary>
		/// Gets or sets the type of CPU represented by this architecture.
		/// </summary>
		/// <value>A <see cref="CpuType"/>; or <see langword="null"/> when no particular CPU type is used.</value>
		public CpuType CpuType
		{
			get { return cpuType; }
		}

		private CpuFeatures features;
		/// <summary>
		/// Gets the features which are supported by the CPU.
		/// </summary>
		/// <value>A bitwise combination of members of the <see cref="CpuFeatures"/> enumeration.</value>
		public CpuFeatures Features
		{
			get { return features | (cpuType != null ? cpuType.Features : CpuFeatures.None); }
		}

		private bool useRIPRelativeAddressing;
		/// <summary>
		/// Gets or sets whether to use RIP-relative addressing by default.
		/// </summary>
		/// <value><see langword="true"/> to use RIP-relative addressing by default;
		/// otherwise, <see langword="false"/>.</value>
		/// <remarks>
		/// This property's value may only be <see langword="true"/> in 64-bit addressing mode.
		/// </remarks>
		public bool UseRIPRelativeAddressing
		{
			get { return useRIPRelativeAddressing; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Creates a new <see cref="Context"/> object which can be used to construct and encode an object file.
		/// </summary>
		/// <param name="objectfile">The <see cref="ObjectFile"/> for which the context is created.</param>
		/// <returns>An architecture specific <see cref="Context"/>.</returns>
		public Context CreateContext(ObjectFile objectfile)
		{
			// CONTRACT: IArchitecture
			return new Context(objectfile);
		}

		/// <summary>
		/// Gets the default size of the address.
		/// </summary>
		/// <param name="type">The type of the CPU.</param>
		/// <returns>A <see cref="DataSize"/> other than <see cref="SharpAssembler.DataSize.None"/>.</returns>
		[Pure]
		private static DataSize GetDefaultAddressingMode(CpuType type)
		{
			#region Contract
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			Contract.Ensures(Contract.Result<DataSize>() != DataSize.None);
			Contract.Ensures(IsValidAddressSize(type, Contract.Result<DataSize>()));
			#endregion

			// By default: 32-bit.
			DataSize addressSize = DataSize.Bit32;

			if (!IsValidAddressSize(type, addressSize))
				// 32-bit not possible? Try 16-bit.
				addressSize = DataSize.Bit16;

			return addressSize;
		}

		/// <summary>
		/// Checks whether the specified address size is valid for the specified CPU type.
		/// </summary>
		/// <param name="type">The CPU type to test against.</param>
		/// <param name="addressSize">The address size to test.</param>
		/// <returns><see langword="true"/> when <paramref name="addressSize"/> is valid for <paramref name="type"/>;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsValidAddressSize(CpuType type, DataSize addressSize)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), addressSize));
			#endregion

			if (type != null)
			{
				// Test whether the specified address size is part of the list
				// of allowed operating modes for the CPU type.
				return (type.OperatingModes & addressSize) != 0;
			}
			else
			{
				return addressSize == DataSize.Bit16
					|| addressSize == DataSize.Bit32
					|| addressSize == DataSize.Bit64;
			}
		}

		/// <summary>
		/// Checks whether the specified operand size is valid for the specified CPU type.
		/// </summary>
		/// <param name="type">The CPU type to test against.</param>
		/// <param name="operandSize">The operand size to test.</param>
		/// <returns><see langword="true"/> when <paramref name="operandSize"/> is valid for <paramref name="type"/>;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsValidOperandSize(CpuType type, DataSize operandSize)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
			#endregion

			if (type != null)
			{
				// Test whether the specified operand size is part of the list
				// of allowed operating modes for the CPU type.
				return (type.OperatingModes & operandSize) != 0;
			}
			else
			{
				return operandSize == DataSize.Bit16
					|| operandSize == DataSize.Bit32
					|| operandSize == DataSize.Bit64;
			}
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(DataSize), addressSize));
			Contract.Invariant(IsValidAddressSize(this.CpuType, addressSize));
			
			Contract.Invariant(Enum.IsDefined(typeof(DataSize), operandSize));
			Contract.Invariant(IsValidOperandSize(this.CpuType, operandSize));
		}
		#endregion
	}
}
