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
using NUnit.Framework;
using SharpAssembler;
using SharpAssembler.Formats.Bin;

namespace SharpAssembler.Architectures.X86.Tests
{
	/// <summary>
	/// Tests the <see cref="Architecture"/> class.
	/// </summary>
	[TestFixture]
	public class ArchitectureTests
	{
		/// <summary>
		/// Tests the <see cref="Architecture.Architecture()"/> constructor.
		/// </summary>
		[Test]
		public void ConstructorTest()
		{
			var arch = new X86Architecture();

			Assert.AreEqual(CpuType.IntelSandyBridge, arch.CpuType);
			Assert.AreEqual(DataSize.Bit32, arch.AddressSize);
			Assert.AreEqual(DataSize.Bit32, arch.OperandSize);
			Assert.AreEqual(CpuType.IntelSandyBridge.Features, arch.Features);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.Architecture(CpuType)"/> constructor.
		/// </summary>
		[Test]
		public void ConstructorTest_CpuType()
		{
			var type = CpuType.AmdBulldozer;

			var arch = new X86Architecture(type);

			Assert.AreEqual(type, arch.CpuType);
			Assert.AreEqual(DataSize.Bit32, arch.AddressSize);
			Assert.AreEqual(DataSize.Bit32, arch.OperandSize);
			Assert.AreEqual(type.Features, arch.Features);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.Architecture(CpuFeatures)"/> constructor.
		/// </summary>
		[Test]
		public void ConstructorTest_CpuFeatures()
		{
			var features = CpuFeatures.PclMulQdq | CpuFeatures.Privileged;

			var arch = new X86Architecture(features);

			Assert.AreEqual(null, arch.CpuType);
			Assert.AreEqual(DataSize.Bit32, arch.AddressSize);
			Assert.AreEqual(DataSize.Bit32, arch.OperandSize);
			Assert.AreEqual(features, arch.Features);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.Architecture(CpuType, DataSize)"/> constructor.
		/// </summary>
		[Test]
		public void ConstructorTest_CpuType_DataSize()
		{
			var type = CpuType.AmdBulldozer;
			var size = DataSize.Bit64;

			var arch = new X86Architecture(type, size);

			Assert.AreEqual(type, arch.CpuType);
			Assert.AreEqual(size, arch.AddressSize);
			Assert.AreEqual(size, arch.OperandSize);
			Assert.AreEqual(type.Features, arch.Features);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.Architecture(CpuFeatures, DataSize)"/> constructor.
		/// </summary>
		[Test]
		public void ConstructorTest_CpuFeatures_DataSize()
		{
			var size = DataSize.Bit64;
			var features = CpuFeatures.PclMulQdq | CpuFeatures.Privileged;

			var arch = new X86Architecture(features, size);

			Assert.AreEqual(null, arch.CpuType);
			Assert.AreEqual(size, arch.AddressSize);
			Assert.AreEqual(size, arch.OperandSize);
			Assert.AreEqual(features, arch.Features);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.Architecture(CpuType, CpuFeatures, DataSize)"/> constructor.
		/// </summary>
		[Test]
		public void ConstructorTest_CpuType_CpuFeatures_DataSize()
		{
			var type = CpuType.IntelPenryn;
			var size = DataSize.Bit64;
			var features = CpuFeatures.PclMulQdq | CpuFeatures.Privileged;

			var arch = new X86Architecture(type, features, size);

			Assert.AreEqual(type, arch.CpuType);
			Assert.AreEqual(size, arch.AddressSize);
			Assert.AreEqual(size, arch.OperandSize);
			Assert.AreEqual(type.Features | features, arch.Features);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.Name"/> property.
		/// </summary>
		[Test]
		public void NameTest()
		{
			X86Architecture arch;

			arch = new X86Architecture(CpuType.IntelNehalem);
			Assert.AreEqual("Intel codename Nehalem (x86-64)", arch.Name);

			arch = new X86Architecture(CpuFeatures.Privileged);
			Assert.AreEqual("x86-64 architecture", arch.Name);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.CreateContext"/> method.
		/// </summary>
		[Test]
		public void CreateContextTest()
		{
			BinObjectFileFormat format = new BinObjectFileFormat();
			var arch = new X86Architecture();
			BinObjectFile objectFile = (BinObjectFile)format.CreateObjectFile(arch, "test");
			var context = arch.CreateContext(objectFile);
			Assert.IsNotNull(context);
			Assert.IsInstanceOf<Context>(context);
			Assert.AreEqual(objectFile, context.Representation);
		}

		/// <summary>
		/// Tests the <see cref="Architecture.IsValidAddressSize"/> method.
		/// </summary>
		[Test]
		public void IsValidAddressSizeTest()
		{
			Assert.IsTrue(X86Architecture.IsValidAddressSize(CpuType.AmdBulldozer, DataSize.Bit64));
			Assert.IsFalse(X86Architecture.IsValidAddressSize(CpuType.IntelPentium4, DataSize.Bit64));

			Assert.IsTrue(X86Architecture.IsValidAddressSize(null, DataSize.Bit64));
			Assert.IsFalse(X86Architecture.IsValidAddressSize(null, DataSize.Bit256));
		}
	}
}
