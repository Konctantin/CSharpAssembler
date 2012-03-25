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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpAssembler.Formats.Bin;
using SharpAssembler;

namespace SharpAssembler.Architectures.X86.Tests.Instructions
{
	/// <summary>
	/// Base class for instruction testing.
	/// </summary>
	public class InstructionTestBase
	{
		/// <summary>
		/// Tests the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="expected">The expected result.</param>
		public void Assert16BitInstruction(X86Instruction instruction, byte[] expected)
		{
			AssertXBitInstruction(instruction, expected, DataSize.Bit16);
		}

		/// <summary>
		/// Tests that the given instruction fails.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		public void Assert16BitInstructionFails(X86Instruction instruction)
		{
			AssertXBitInstructionFails(instruction, DataSize.Bit16);
		}

		/// <summary>
		/// Tests the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="expected">The expected result.</param>
		public void Assert32BitInstruction(X86Instruction instruction, byte[] expected)
		{
			AssertXBitInstruction(instruction, expected, DataSize.Bit32);
		}

		/// <summary>
		/// Tests that the given instruction fails.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		public void Assert32BitInstructionFails(X86Instruction instruction)
		{
			AssertXBitInstructionFails(instruction, DataSize.Bit32);
		}

		/// <summary>
		/// Tests the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="expected">The expected result.</param>
		public void Assert64BitInstruction(X86Instruction instruction, byte[] expected)
		{
			AssertXBitInstruction(instruction, expected, DataSize.Bit64);
		}

		/// <summary>
		/// Tests that the given instruction fails.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		public void Assert64BitInstructionFails(X86Instruction instruction)
		{
			AssertXBitInstructionFails(instruction, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="expected">The expected result.</param>
		private void AssertXBitInstruction(X86Instruction instruction, byte[] expected, DataSize mode)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			if (expected == null)
				throw new ArgumentNullException("expected");
			#endregion

			byte[] actual = Assemble(instruction, mode);

			string expectedBytes = String.Join(" ", from b in expected select String.Format("{0:X2}", b));
			string actualBytes = String.Join(" ", from b in actual select String.Format("{0:X2}", b));
			Assert.AreEqual(expected, actual, String.Format("Expected {0}, got {1}.", expectedBytes, actualBytes));
		}

		/// <summary>
		/// Tests that the given instruction fails.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		private void AssertXBitInstructionFails(X86Instruction instruction, DataSize mode)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			#endregion

			Assert.Throws<AssemblerException>(() => Assemble(instruction, mode));
		}

		/// <summary>
		/// Assembles the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> to assemble.</param>
		/// <param name="mode">The mode in which to assemble.</param>
		/// <returns>The bytes representing the assembled instruction.</returns>
		/// <exception cref="AssemblerException">
		/// An assembler exception occurred.
		/// </exception>
		private byte[] Assemble(X86Instruction instruction, DataSize mode)
		{
			byte[] actual = null;
			BinObjectFileFormat format = new BinObjectFileFormat();
			var arch = new X86Architecture(CpuType.AmdBulldozer, mode);
			BinObjectFile objectFile = (BinObjectFile)format.CreateObjectFile(arch, "test");
			Section textSection = objectFile.Sections.AddNew(SectionType.Program);
			var text = textSection.Contents;

			text.Add(instruction);

			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(ms))
				{
					objectFile.Format.CreateAssembler(objectFile).Assemble(writer);
					actual = ms.ToArray();
				}
			}

			return actual;
		}

		// ------------------------------------------------------------- //

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

			var result = AssembleInstruction(instruction, null, mode);
			byte[] actual = result.Item2;

			string expectedBytes = String.Join(" ", from b in expected select String.Format("{0:X2}", b));
			string actualBytes = String.Join(" ", from b in actual select String.Format("{0:X2}", b));
			Assert.AreEqual(expected, actual, String.Format("Expected {0}, got {1}.", expectedBytes, actualBytes));
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

			var result = AssembleInstruction(instruction, null, mode);
			byte[] actual = result.Item2;

			Assert.IsNull(actual);
		}

		/// <summary>
		/// Tests the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="nasmInstruction">The NASM string representation of the same instruction.</param>
		/// <param name="mode">The mode (16-bit, 32-bit or 64-bit) to use.</param>
		public void AssertInstruction(X86Instruction instruction, string nasmInstruction, DataSize mode)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			if (nasmInstruction == null)
				throw new ArgumentNullException("nasmInstruction");
			if (!Enum.IsDefined(typeof(DataSize), mode))
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(DataSize));
			if (mode != DataSize.Bit16 && mode != DataSize.Bit32 && mode != DataSize.Bit64)
				throw new ArgumentException(null, "mode");
			#endregion

			var result = AssembleInstruction(instruction, nasmInstruction, mode);
			byte[] expected = result.Item1;
			byte[] actual = result.Item2;
			string expectedBytes = String.Join(", ", from b in expected select String.Format("0x{0:X2}", b));
			string actualBytes = String.Join(", ", from b in actual select String.Format("0x{0:X2}", b));
			Assert.AreEqual(expected, actual, String.Format("Expected {0}, actual {1}.", expectedBytes, actualBytes));
		}

		/// <summary>
		/// Tests that the given instruction does not assemble.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="nasmInstruction">The NASM string representation of the same instruction.</param>
		/// <param name="mode">The mode (16-bit, 32-bit or 64-bit) to use.</param>
		public void AssertInstructionFail(X86Instruction instruction, string nasmInstruction, DataSize mode)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			if (nasmInstruction == null)
				throw new ArgumentNullException("nasmInstruction");
			if (!Enum.IsDefined(typeof(DataSize), mode))
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(DataSize));
			if (mode != DataSize.Bit16 && mode != DataSize.Bit32 && mode != DataSize.Bit64)
				throw new ArgumentException(null, "mode");
			#endregion

			var result = AssembleInstruction(instruction, nasmInstruction, mode);
			byte[] expected = result.Item1;
			byte[] actual = result.Item2;

			Assert.IsNull(expected);
			Assert.IsNull(actual);
		}

		/// <summary>
		/// Assembles the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <param name="nasmInstruction">The NASM string representation of the same instruction.</param>
		/// <param name="mode">The mode (16-bit, 32-bit or 64-bit) to use.</param>
		/// <returns>A (expected, actual) tuple.</returns>
		private Tuple<byte[], byte[]> AssembleInstruction(X86Instruction instruction, string nasmInstruction, DataSize mode)
		{
			#region Contract
			if (!Enum.IsDefined(typeof(DataSize), mode))
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(DataSize));
			if (mode != DataSize.Bit16 && mode != DataSize.Bit32 && mode != DataSize.Bit64)
				throw new ArgumentException(null, "mode");
			#endregion

			// Assemble the NASM instruction.
			byte[] expected = null;
			if (nasmInstruction != null)
			{
				StringBuilder sb = new StringBuilder();
				switch (mode)
				{
					case DataSize.Bit16:
						sb.AppendLine("[BITS 16]");
						break;
					case DataSize.Bit32:
						sb.AppendLine("[BITS 32]");
						break;
					case DataSize.Bit64:
						sb.AppendLine("[BITS 64]");
						break;
					default:
						throw new NotSupportedException();
				}
				sb.AppendLine(nasmInstruction);
				string feedback;
				expected = RunAssembler(sb.ToString(), out feedback);
				if (feedback != null && feedback.Length > 0)
				{
					Console.WriteLine("Assembler feedback:");
					Console.WriteLine(feedback);
				}
			}

			// Assemble the SharpAssembler instruction.
			byte[] actual = null;
			if (instruction != null)
			{
				BinObjectFileFormat format = new BinObjectFileFormat();
				var arch = new X86Architecture(CpuType.AmdBulldozer, mode);
				BinObjectFile objectFile = (BinObjectFile)format.CreateObjectFile(arch, "test");
				Section textSection = objectFile.Sections.AddNew(SectionType.Program);
				var text = textSection.Contents;

				text.Add(instruction);

				try
				{
					using (MemoryStream ms = new MemoryStream())
					{
						using (BinaryWriter writer = new BinaryWriter(ms))
						{
							objectFile.Format.CreateAssembler(objectFile).Assemble(writer);
							actual = ms.ToArray();
						}
					}
				}
				catch (AssemblerException ex)
				{
					Console.WriteLine(ex);
					actual = null;
				}
			}

			return new Tuple<byte[], byte[]>(expected, actual);
		}

		/// <summary>
		/// Runs an assembler and returns the results.
		/// </summary>
		/// <param name="data">The string data to assemble.</param>
		/// <param name="feedback">The feedback from the assembler.</param>
		/// <returns>The binary data resulting from the assembling; or <see langword="null"/> when an error
		/// occurred.</returns>
		private byte[] RunAssembler(string data, out string feedback)
		{
			byte[] encodedData = Encoding.UTF8.GetBytes(data);
			using (FileStream fs = File.Create("test.asm"))
			{
				fs.Write(encodedData, 0, encodedData.Length);
			}
			File.Delete("test.bin");
			ProcessStartInfo psi = new ProcessStartInfo(@"..\..\..\..\Yasm\yasm-1.1.0-win64",
				"-a x86 -f bin -o test.bin test.asm");
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			psi.UseShellExecute = false;

			Process process = System.Diagnostics.Process.Start(psi);
			StreamReader std = process.StandardOutput;
			StreamReader err = process.StandardError;
			process.WaitForExit();
			feedback = std.ReadToEnd() + err.ReadToEnd();

			byte[] result;
			try
			{
				using (FileStream fs = File.OpenRead("test.bin"))
				{
					result = new byte[fs.Length];
					fs.Read(result, 0, (int)fs.Length);
				}
			}
			catch (FileNotFoundException)
			{
				result = null;
			}
			return result;
		}
	}
}
