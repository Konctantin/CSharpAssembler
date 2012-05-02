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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpAssembler.Formats.Bin;
using SharpAssembler;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace SharpAssembler.Architectures.X86.Tests.Opcodes
{
	/// <summary>
	/// Base class for operand testing.
	/// </summary>
	public abstract class OpcodeTestBase
	{
		/// <summary>
		/// Tests the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="mode">The mode (16-bit, 32-bit or 64-bit) to use.</param>
		/// <param name="expected">The expected result.</param>
		public void AssertInstruction(X86Instruction instruction, DataSize mode, byte[] expected)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			if (!Enum.IsDefined(typeof(DataSize), mode))
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(DataSize));
			if (mode != DataSize.Bit16 && mode != DataSize.Bit32 && mode != DataSize.Bit64)
				throw new ArgumentException(null, "mode");
			if (expected == null)
				throw new ArgumentNullException("expected");
			#endregion

			Tuple<byte[], IEnumerable<string>> tuple = AssembleInstruction(instruction, mode);
			byte[] actual = tuple.Item1;
			string messages = tuple.Item2.Any() ? "(" + String.Join(", ", tuple.Item2) + ")" : String.Empty;

			Assert.AreEqual(expected, actual, String.Format("Expected {0}, got {1}{2}.",
				ByteArrayToString(expected),
				actual != null && actual.Length > 0 ? ByteArrayToString(actual) : "nothing",
				messages));
		}

		private string ByteArrayToString(byte[] array)
		{
			if (array != null)
				return String.Join(" ", from b in array select String.Format("{0:X2}", b));
			else
				return String.Empty;
		}

		/// <summary>
		/// Tests that the given instruction does not assemble.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="mode">The mode (16-bit, 32-bit or 64-bit) to use.</param>
		public void AssertInstructionFail(X86Instruction instruction, DataSize mode)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			if (!Enum.IsDefined(typeof(DataSize), mode))
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(DataSize));
			if (mode != DataSize.Bit16 && mode != DataSize.Bit32 && mode != DataSize.Bit64)
				throw new ArgumentException(null, "mode");
			#endregion

			Tuple<byte[], IEnumerable<string>> tuple = AssembleInstruction(instruction, mode);
			byte[] actual = tuple.Item1;
			string messages = tuple.Item2.Any() ? "(" + String.Join(", ", tuple.Item2) + ")" : String.Empty;

			Assert.IsNull(actual, String.Format("Expected failure, got {0}{1}.",
				ByteArrayToString(actual), messages));
		}

		/// <summary>
		/// Assembles the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="mode">The mode (16-bit, 32-bit or 64-bit) to use.</param>
		/// <returns>A tuple with the assembled bytes and any (error) messages.</returns>
		private Tuple<byte[], IEnumerable<string>> AssembleInstruction(X86Instruction instruction, DataSize mode)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			if (!Enum.IsDefined(typeof(DataSize), mode))
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(DataSize));
			if (mode != DataSize.Bit16 && mode != DataSize.Bit32 && mode != DataSize.Bit64)
				throw new ArgumentException(null, "mode");
			#endregion

			// Assemble the SharpAssembler instruction.
			byte[] actual = null;
			List<string> messages = new List<string>();

			BinObjectFileFormat format = new BinObjectFileFormat();
			var arch = new X86Architecture(CpuType.AmdBulldozer, mode);
			BinObjectFile objectFile = (BinObjectFile)format.CreateObjectFile(arch, "test");
			Section textSection = objectFile.Sections.AddNew(SectionType.Program);
			var text = textSection.Contents;

			text.Add(instruction);

			try
			{
				using (MemoryStream ms = new MemoryStream())
				using (BinaryWriter writer = new BinaryWriter(ms))
				{
					var assembler = objectFile.Format.CreateAssembler(objectFile);
					assembler.Assemble(writer);
					actual = ms.ToArray();
				}
			}
			catch (AssemblerException ex)
			{
				messages.Add(String.Format("Error: {0}", ex.Message));
				actual = null;
			}

			return new Tuple<byte[],IEnumerable<string>>(actual, messages);
		}
	}
}
