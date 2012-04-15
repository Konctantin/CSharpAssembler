using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpAssembler.Instructions;
using SharpAssembler.Formats.Bin;
using SharpAssembler.Architectures.X86;
using SharpAssembler.Symbols;
using System.IO;

namespace SharpAssembler.Tests
{
	[TestFixture]
	public class Expressions : ObjectFileTestBase
	{
		[Test]
		public void SectionRelativePosition()
		{
			var objectFile = CreateObjectFile();

			var text = objectFile.Sections.AddNew(SectionType.Program);
			text.Address = 0x1000;
			text.Contents.AddRange(new Constructable[] { 
				new DeclareData((byte)0xFF),		// Offset 0
				new DeclareData((byte)0xEF),
				new DeclareData((byte)0xDF),
				new DeclareData((byte)0xCF),
				new DeclareData((byte)0xBF),		// Offset 4
				new Define("currentoffset1", c => c.Section.Address - c.Address),
				new DeclareData(new Reference("currentoffset1"), DataSize.Bit32),
			});

			var data = objectFile.Sections.AddNew(SectionType.Data);
			data.Address = 0x2000;
			data.Contents.AddRange(new Constructable[] { 
				new DeclareData((byte)0xFF),		// Offset 0
				new DeclareData((byte)0xFE),
				new DeclareData((byte)0xFD),
				new DeclareData((byte)0xFC),
				new DeclareData((byte)0xFB),
				new DeclareData((byte)0xFA),
				new DeclareData((byte)0xF0),		// Offset 6
				new Define("currentoffset2", c => c.Section.Address - c.Address),
				new DeclareData(new Reference("currentoffset2"), DataSize.Bit32),
			});

			byte[] assembled = Assemble(objectFile);
			Assert.That(BitConverter.ToUInt32(assembled, 5),
				Is.EqualTo(0x1005));

		}
	}
}
