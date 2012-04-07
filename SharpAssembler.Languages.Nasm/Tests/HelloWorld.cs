using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpAssembler.Formats.Bin;
using SharpAssembler.Architectures.X86;
using SharpAssembler.Instructions;
using SharpAssembler.Symbols;
using System.IO;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Languages.Nasm.Tests
{
	[TestFixture]
	public class HelloWorld
	{
		[Test]
		public void Test()
		{
			// Given
			var format = new BinObjectFileFormat();
			var architecture = new X86Architecture();
			var objectFile = new ObjectFile(format, architecture, "test");

			var text = objectFile.Sections.AddNew(SectionType.Program);
			var data = objectFile.Sections.AddNew(SectionType.Data);
			var bss = objectFile.Sections.AddNew(SectionType.Bss);

			text.Contents.AddRange(new Constructable[]{
				new Label("main"),
				Instr.Mov(Register.EDX, new EffectiveAddress(DataSize.Bit32, DataSize.Bit32, c => new Reference("len"))),
				Instr.Mov(Register.ECX, new EffectiveAddress(DataSize.Bit32, DataSize.Bit32, c => new Reference("len"))),
				Instr.Mov(Register.EBX, 1),
				Instr.Mov(Register.EAX, 4),
				Instr.Int(0x80),

				Instr.Mov(Register.EBX, 0),
				Instr.Mov(Register.EAX, 1),
				Instr.Int(0x80),
			});

			data.Contents.AddRange(new Constructable[]{
				new Label("str"),
				new DeclareString("Hello World\n"),
				new Define("len", c => new SimpleExpression(c.Address - c.SymbolTable["str"].Value)),
			});

			// When
			string code;
			using (var writer = new StringWriter())
			{
				var language = new BinNasmLanguage(writer);
				language.Write(objectFile);
				code = writer.ToString();
			}
			
			// Then
			Assert.That(code, Is.EqualTo(
@"

"));
		}
	}
}
