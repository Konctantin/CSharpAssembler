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
using SharpAssembler;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Describes a type of CPU.
	/// </summary>
	public class CpuType
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CpuType"/> class.
		/// </summary>
		/// <param name="name">The name of the CPU type.</param>
		/// <param name="operatingModes">A bitwise combination of members of the <see cref="DataSize"/> enumeration,
		/// specifying the available operating modes.</param>
		/// <param name="features">A bitwise combination of members of the <see cref="CpuFeatures"/> enumeration,
		/// specifying the available features.</param>
		public CpuType(string name, DataSize operatingModes, CpuFeatures features)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(name != null);
			#endregion

			this.name = name;
			this.operatingModes = operatingModes;
			this.features = features;
		}
		#endregion

		#region Properties
		private string name;
		/// <summary>
		/// Gets the name of the CPU type.
		/// </summary>
		/// <value>A name.</value>
		public virtual string Name
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<string>() != null);
				#endregion
				return name;
			}
		}

		private DataSize operatingModes;
		/// <summary>
		/// Gets the operating modes supported by this CPU type.
		/// </summary>
		/// <value>A bitwise combination of members of the <see cref="DataSize"/> enumeration.</value>
		public virtual DataSize OperatingModes
		{
			get { return operatingModes; }
		}

		private CpuFeatures features;
		/// <summary>
		/// Gets the features supported by this CPU type.
		/// </summary>
		/// <value>A bitwise combination of members of the <see cref="CpuFeatures"/> enumeration.</value>
		public virtual CpuFeatures Features
		{
			get { return features; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Converts the value of this instance to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		public override string ToString()
		{
			return name;
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.name != null);
		}
		#endregion

		#region Predefined
		/// <summary>
		/// Intel 8086.
		/// </summary>
		public static readonly CpuType Intel8086 = new CpuType(
			"Intel 8086",
			DataSize.Bit16,
			CpuFeatures.Privileged);

		/// <summary>
		/// Intel 80186 (i186).
		/// </summary>
		public static readonly CpuType Intel80186 = new CpuType(
			"Intel 80186",
			DataSize.Bit16,
			CpuFeatures.Privileged);

		/// <summary>
		/// Intel 80286 (i286).
		/// </summary>
		public static readonly CpuType Intel80286 = new CpuType(
			"Intel 80286",
			DataSize.Bit16,
			CpuFeatures.Privileged);

		/// <summary>
		/// Intel 80386 (i386).
		/// </summary>
		public static readonly CpuType Intel80386 = new CpuType(
			"Intel 80386",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode);

		/// <summary>
		/// Intel 80486 (i486).
		/// </summary>
		public static readonly CpuType Intel80486 = new CpuType(
			"Intel 80486",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu);

		/// <summary>
		/// Intel Pentium (i586) or Intel codename P5.
		/// </summary>
		public static readonly CpuType IntelPentium = new CpuType(
			"Intel Pentium",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu);

		/// <summary>
		/// Intel Pentium Pro (i686) or Intel codename P6.
		/// </summary>
		public static readonly CpuType IntelPentiumPro = new CpuType(
			"Intel Pentium Pro",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu);

		/// <summary>
		/// Intel Pentium II.
		/// </summary>
		public static readonly CpuType IntelPentiumII = new CpuType(
			"Intel Pentium II",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx);

		/// <summary>
		/// Intel Pentium III or Intel codename Katmai.
		/// </summary>
		public static readonly CpuType IntelPentium3 = new CpuType(
			"Intel Pentium III",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx | CpuFeatures.Sse);

		/// <summary>
		/// Intel Pentium IV (P4) or Intel codename Williamette.
		/// </summary>
		public static readonly CpuType IntelPentium4 = new CpuType(
			"Intel Pentium 4",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2);

		/// <summary>
		/// Intel Itanium, IA64 (x86).
		/// </summary>
		public static readonly CpuType IntelItanium = new CpuType(
			"Intel Itanium",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2);

		/// <summary>
		/// Intel codename Prescott.
		/// </summary>
		public static readonly CpuType IntelPrescott = new CpuType(
			"Intel codename Prescott",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx | 
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3);

		/// <summary>
		/// Intel codename Conroe or Intel Core2.
		/// </summary>
		public static readonly CpuType IntelConroe = new CpuType(
			"Intel codename Conroe",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3 | CpuFeatures.Ssse3);

		/// <summary>
		/// Intel codename Penryn.
		/// </summary>
		public static readonly CpuType IntelPenryn = new CpuType(
			"Intel codename Penryn",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3 | CpuFeatures.Ssse3 |
			CpuFeatures.Sse4Penryn);

		/// <summary>
		/// Intel codename Nehalem or Intel Core i7.
		/// </summary>
		public static readonly CpuType IntelNehalem = new CpuType(
			"Intel codename Nehalem",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3 | CpuFeatures.Ssse3 |
			CpuFeatures.Sse4Penryn | CpuFeatures.Sse4Nehalemn |
			CpuFeatures.XSave);

		/// <summary>
		/// Intel codename Westmere.
		/// </summary>
		public static readonly CpuType IntelWestmere = new CpuType(
			"Intel codename Westmere",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx | 
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3 | CpuFeatures.Ssse3 |
			CpuFeatures.Sse4Penryn | CpuFeatures.Sse4Nehalemn |
			CpuFeatures.XSave | CpuFeatures.PclMulQdq |	CpuFeatures.Aes);

		/// <summary>
		/// Intel codename Sandy Bridge.
		/// </summary>
		public static readonly CpuType IntelSandyBridge = new CpuType(
			"Intel codename Sandy Bridge",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3 | CpuFeatures.Ssse3 |
			CpuFeatures.Sse4Penryn | CpuFeatures.Sse4Nehalemn |
			CpuFeatures.XSave | CpuFeatures.PclMulQdq | CpuFeatures.Aes | CpuFeatures.Avx);

		/// <summary>
		/// AMD K6 architecture.
		/// </summary>
		public static readonly CpuType AmdK6 = new CpuType(
			"AMD K6 architecture",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Amd3DNow | CpuFeatures.Mmx);

		/// <summary>
		/// AMD Athlon K7 architecture.
		/// </summary>
		public static readonly CpuType AmdAthlon = new CpuType(
			"AMD Athlon",
			DataSize.Bit16 | DataSize.Bit32,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Amd3DNow | CpuFeatures.Mmx |
			CpuFeatures.Sse);

		/// <summary>
		/// AMD Athlon64, AMD codename Opteron, Hammer or Clawhammer K8 architecture.
		/// </summary>
		public static readonly CpuType AmdAthlon64 = new CpuType(
			"AMD Athlon64",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Amd3DNow | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2);

		/// <summary>
		/// AMD codename Venice.
		/// </summary>
		public static readonly CpuType AmdVenice = new CpuType(
			"AMD codename Venice",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Amd3DNow | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3);

		/// <summary>
		/// AMD codename K10, AMD Phenom or AMD Family10h.
		/// </summary>
		public static readonly CpuType AmdK10 = new CpuType(
			"AMD K10 architecture",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Amd3DNow | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3 | CpuFeatures.Sse4A);

		/// <summary>
		/// AMD codename Bulldozer.
		/// </summary>
		public static readonly CpuType AmdBulldozer = new CpuType(
			"AMD Bulldozer",
			DataSize.Bit16 | DataSize.Bit32 | DataSize.Bit64,
			CpuFeatures.Privileged | CpuFeatures.Smm | CpuFeatures.ProtectedMode |
			CpuFeatures.Fpu | CpuFeatures.Amd3DNow | CpuFeatures.Mmx |
			CpuFeatures.Sse | CpuFeatures.Sse2 | CpuFeatures.Sse3 | CpuFeatures.Sse4A | CpuFeatures.Sse5);
		#endregion
	}
}
