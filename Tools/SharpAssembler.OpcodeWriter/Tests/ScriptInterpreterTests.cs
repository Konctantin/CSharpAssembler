using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpAssembler.OpcodeWriter.X86;

namespace SharpAssembler.OpcodeWriter.Tests
{
	[TestFixture]
	public class ScriptInterpreterTests
	{
		[Test]
		public void ReadASimpleScript()
		{
			// Given
			SpecFactoryDispenser dispenser = new SpecFactoryDispenser();
			dispenser.Register("x86", new X86SpecFactory());
			ScriptInterpreter reader = new ScriptInterpreter(new ScriptTokenizer(), dispenser);
			string script =
@"opcode x86 mov
{
	set ShortDescription = ""Move"";
	set IsValidIn64BitMode = true;
	set CanLock = true;

	/* A comment
	on multiple lines */
	var `88` (reg/mem8  destination, reg8  source);
	var `A3` (moffset32 destination, void source = %EAX);
	var `B8 D7` (reg32 destination, imm32 source);
	var `C7` (reg/mem64 destination, imm32 source) { set FixedReg = 1; }
}";

			// When
			reader.Read(script);

			// Then
			Assert.Pass();
		}

		public void ReadASimpleScript2()
		{
			// Given
			SpecFactoryDispenser dispenser = new SpecFactoryDispenser();
			dispenser.Register("x86", new X86SpecFactory());
			ScriptInterpreter reader = new ScriptInterpreter(new ScriptTokenizer(), dispenser);
			string script =
@"opcode x86 mov
{
	set ShortDescription = ""Move"";
	set IsValidIn64BitMode = true;
	set CanLock = true;

	/* A comment
	on multiple lines */
	var `88` (reg/mem8  destination, reg8  source);
	var `A3` (moffset32 destination, void source = %EAX);
	var `B8 D7` (reg32 destination, imm32 source);
	var `C7` (reg/mem64 destination, imm32 source) { set FixedReg = 1; }
}";

			// When
			var opcodeSpec = reader.Read(script).First() as X86OpcodeSpec;

			// Then
			Assert.That(opcodeSpec.Mnemonic, Is.EqualTo("mov"));
			Assert.That(opcodeSpec.ShortDescription, Is.EqualTo("Move"));
			Assert.That(opcodeSpec.IsValidIn64BitMode, Is.True);
			Assert.That(opcodeSpec.CanLock, Is.True);

			Assert.That(opcodeSpec.Variants.Count, Is.EqualTo(4));

			var var1 = (X86OpcodeVariantSpec)opcodeSpec.Variants[0];
			var var2 = (X86OpcodeVariantSpec)opcodeSpec.Variants[1];
			var var3 = (X86OpcodeVariantSpec)opcodeSpec.Variants[2];
			var var4 = (X86OpcodeVariantSpec)opcodeSpec.Variants[3];
			X86OperandSpec dest;
			X86OperandSpec src;

			Assert.That(var1.OpcodeBytes, Is.EquivalentTo(new byte[] { 0x88 }));
			Assert.That(var1.Operands.Count, Is.EqualTo(2));
			dest = (X86OperandSpec)var1.Operands[0];
			Assert.That(dest.Name, Is.EqualTo("destination"));
			Assert.That(dest.Type, Is.EqualTo(X86OperandType.RegisterOrMemoryOperand));
			Assert.That(dest.Size, Is.EqualTo(DataSize.Bit8));
			src = (X86OperandSpec)var1.Operands[1];
			Assert.That(src.Name, Is.EqualTo("source"));
			Assert.That(src.Type, Is.EqualTo(X86OperandType.RegisterOperand));
			Assert.That(src.Size, Is.EqualTo(DataSize.Bit8));

			Assert.That(var2.OpcodeBytes, Is.EquivalentTo(new byte[] { 0xA3 }));
			Assert.That(var2.Operands.Count, Is.EqualTo(2));
			dest = (X86OperandSpec)var2.Operands[0];
			Assert.That(dest.Name, Is.EqualTo("destination"));
			Assert.That(dest.Type, Is.EqualTo(X86OperandType.MemoryOffset));
			Assert.That(dest.Size, Is.EqualTo(DataSize.Bit32));
			src = (X86OperandSpec)var2.Operands[1];
			Assert.That(src.Name, Is.EqualTo("source"));
			Assert.That(src.Type, Is.EqualTo(X86OperandType.FixedRegister));
			Assert.That(src.FixedRegister, Is.EqualTo(Register.EAX));

			Assert.That(var3.OpcodeBytes, Is.EquivalentTo(new byte[] { 0xB8, 0xD7 }));
			Assert.That(var3.Operands.Count, Is.EqualTo(2));
			dest = (X86OperandSpec)var3.Operands[0];
			Assert.That(dest.Name, Is.EqualTo("destination"));
			Assert.That(dest.Type, Is.EqualTo(X86OperandType.RegisterOperand));
			Assert.That(dest.Size, Is.EqualTo(DataSize.Bit32));
			src = (X86OperandSpec)var3.Operands[1];
			Assert.That(src.Name, Is.EqualTo("source"));
			Assert.That(src.Type, Is.EqualTo(X86OperandType.Immediate));
			Assert.That(src.Size, Is.EqualTo(DataSize.Bit32));

			Assert.That(var4.OpcodeBytes, Is.EquivalentTo(new byte[] { 0xC7 }));
			Assert.That(var4.FixedReg, Is.EqualTo(1));
			Assert.That(var4.Operands.Count, Is.EqualTo(2));
			dest = (X86OperandSpec)var4.Operands[0];
			Assert.That(dest.Name, Is.EqualTo("destination"));
			Assert.That(dest.Type, Is.EqualTo(X86OperandType.RegisterOrMemoryOperand));
			Assert.That(dest.Size, Is.EqualTo(DataSize.Bit64));
			src = (X86OperandSpec)var4.Operands[1];
			Assert.That(src.Name, Is.EqualTo("source"));
			Assert.That(src.Type, Is.EqualTo(X86OperandType.Immediate));
			Assert.That(src.Size, Is.EqualTo(DataSize.Bit32));
		}
	}
}
