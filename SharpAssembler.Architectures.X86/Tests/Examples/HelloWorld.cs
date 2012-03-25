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
using System.IO;
using NUnit.Framework;
using SharpAssembler.Formats.Bin;
using SharpAssembler;
using SharpAssembler.Instructions;
using SharpAssembler.Symbols;
using SharpAssembler.Architectures.X86.Instructions;

namespace SharpAssembler.Architectures.X86.Tests.Instructions
{
	/// <summary>
	/// Assembles a 'Hello World' program.
	/// </summary>
	[TestFixture]
	public class HelloWorld : ExampleBase
	{
		[Test]
		public void Do()
		{
			BinObjectFileFormat format = new BinObjectFileFormat();
			var arch = new X86Architecture(CpuType.AmdBulldozer, DataSize.Bit32);
			BinObjectFile objectFile = (BinObjectFile)format.CreateObjectFile(arch, "helloworld");

			Section textSection = objectFile.Sections.AddNew(SectionType.Program);

			var text = textSection.Contents;
			text.Add(new Label("main"));
			text.Add(new Mov(Register.EDX, new Reference("len")));
			text.Add(new Mov(Register.ECX, new Reference("str")));
			text.Add(new Mov(Register.EBX, 1));
			text.Add(new Mov(Register.EAX, 4));
			text.Add(new Int(0x80));

			text.Add(new Mov(Register.EBX, 0));
			text.Add(new Mov(Register.EAX, 1));
			text.Add(new Int(0x80));

			Section dataSection = objectFile.Sections.AddNew(SectionType.Data);
			var data = dataSection.Contents;
			data.Add(new Label("str"));
			data.Add(new DeclareString("Hello World\n"));

			data.Add(new Define("len", (context) =>
				{
					Symbol strSymbol = context.SymbolTable["str"];
					return new SimpleExpression(context.Address - strSymbol.Address);
				}));

			byte[] result = Assemble(objectFile);
			byte[] expected = new byte[]{
				0xBA, 0x0C, 0x00, 0x00, 0x00,		// mov EDX, len
				0xB9, 0x30, 0x00, 0x00, 0x00,		// mov ECX, str
				0xBB, 0x01, 0x00, 0x00, 0x00,		// mov EBX, 1
				0xB8, 0x04, 0x00, 0x00, 0x00,		// mov EAX, 4
				0xCD, 0x80,							// int 0x80

				0xBB, 0x00, 0x00, 0x00, 0x00,		// mov EBX, 0
				0xB8, 0x01, 0x00, 0x00, 0x00,		// mov EAX, 1
				0xCD, 0x80,							// int 0x80

				// Padding
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				// "Hello World\n"
				0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x0A
			};
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Do2()
		{
			BinObjectFileFormat format = new BinObjectFileFormat();
			var arch = new X86Architecture(CpuType.AmdBulldozer, DataSize.Bit32);
			BinObjectFile objectFile = (BinObjectFile)format.CreateObjectFile(arch, "helloworld");

			Section textSection = objectFile.Sections.AddNew(SectionType.Program);
			var text = textSection.Contents;
			text.Add(new Label("main"));
			text.Add(new Mov(Register.EDX, new Reference("len")));
			text.Add(new Mov(Register.ECX, new Reference("str")));
			text.Add(new Mov(Register.EBX, 1));
			text.Add(new Mov(Register.EAX, 4));
			text.Add(new Int(0x80));

			text.Add(new Mov(Register.EBX, 0));
			text.Add(new Mov(Register.EAX, 1));
			text.Add(new Int(0x80));

			Section dataSection = objectFile.Sections.AddNew(SectionType.Data);
			var data = dataSection.Contents;
			data.Add(new Label("str"));
			data.Add(new DeclareString("Hello World\n"));

			data.Add(new Define("len", (context) =>
			{
				Symbol strSymbol = context.SymbolTable["str"];
				return new SimpleExpression(context.Address - strSymbol.Address);
			}));

			using (FileStream fs = File.Create("helloworld.bin"))
				using (BinaryWriter writer = new BinaryWriter(fs))
					objectFile.Format.CreateAssembler(objectFile).Assemble(writer);
		}
	}
}
