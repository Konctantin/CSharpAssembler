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
using NUnit.Framework;
using System;
using SharpAssembler.Formats.Bin;

namespace SharpAssembler.Architectures.X86.Tests.Operands
{
	/// <summary>
	/// Tests the <see cref="Operand"/> class.
	/// </summary>
	[TestFixture]
	public class OperandTests
	{
		[Test]
		public void OperandSize_CorrectlyDetermined()
		{

		}

#if false
		/// <summary>
		/// Assembles the given instruction.
		/// </summary>
		/// <param name="instruction">The <see cref="X86Instruction"/> instance to test.</param>
		/// <returns>A tuple with the assembled bytes and any (error) messages.</returns>
		private DataSize GetOperandSize(X86Instruction instruction)
		{
			#region Contract
			if (instruction == null)
				throw new ArgumentNullException("instruction");
			#endregion

			// Assemble the SharpAssembler instruction.
			byte[] actual = null;
			List<string> messages = new List<string>();

			BinObjectFileFormat format = new BinObjectFileFormat();
			var arch = new X86Architecture(CpuType.AmdBulldozer, DataSize.Bit32);
			BinObjectFile objectFile = (BinObjectFile)format.CreateObjectFile(arch);
			Section textSection = objectFile.Sections.AddNew(SectionType.Program);
			var text = textSection.Contents;

			text.Add(instruction);

			var assembler = objectFile.Format.CreateAssembler(objectFile);
			assembler.Assemble(writer);

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

			return new Tuple<byte[], IEnumerable<string>>(actual, messages);
		}
#endif
	}
}
