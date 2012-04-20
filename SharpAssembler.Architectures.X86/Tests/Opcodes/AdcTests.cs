using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Tests.Opcodes
{
	/// <summary>
	/// Tests all variants of the ADC opcode.
	/// </summary>
	[TestFixture]
	public class AdcTests : OpcodeTestBase
	{
		[Test]
		public void AL_imm8()
		{
			var instruction = Instr.Adc(Register.AL, (byte)0x7B);

			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x14, 0x7B });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x14, 0x7B });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x14, 0x7B });
		}

		[Test]
		public void AX_imm16()
		{
			var instrString = "adc AX, 12345";
			var instruction = Instr.Adc(Register.AX, (short)12345);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void EAX_imm32()
		{
			var instrString = "adc EAX, 1234567890";
			var instruction = Instr.Adc(Register.EAX, (int)1234567890);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void RAX_imm32()
		{
			var instruction = Instr.Adc(Register.RAX, (int)1234567890);

			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x15, 0xD2, 0x02, 0x96, 0x49 });
		}

		[Test]
		public void regmem8_imm8()
		{
			var instrString = "adc BYTE [1234], 123";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(1234)),
				(byte)123);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem16_imm16()
		{
			var instrString = "adc WORD [1234], WORD 12345";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(1234)),
				(short)12345);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem32_imm32()
		{
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(1234)),
				(int)12345);

			AssertInstruction(instruction, DataSize.Bit16, new byte[] { 0x66, 0x81, 0x16, 0xd2, 0x04, 0x39, 0x30, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit32, new byte[] { 0x81, 0x15, 0xd2, 0x04, 0x00, 0x00, 0x39, 0x30, 0x00, 0x00 });
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x81, 0x14, 0x25, 0xd2, 0x04, 0x00, 0x00, 0x39, 0x30, 0x00, 0x00 });
		}

		[Test]
		public void regmem64_imm32()
		{
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(1234)),
				(int)12345);

			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x81, 0x14, 0x25, 0xD2, 0x04, 0x00, 0x00, 0x39, 0x30, 0x00, 0x00 });
		}

		[Test]
		public void regmem16_imm8()
		{
			var instrString = "adc WORD [1234], 123";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(1234)),
				(byte)123);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem32_imm8()
		{
			var instrString = "adc DWORD [1234], 123";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(1234)),
				(byte)123);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem64_imm8()
		{
			var instrString = "adc QWORD [1234], 123";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(1234)),
				(byte)123);

			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem8_reg8()
		{
			var instrString = "adc BYTE [1234], CL";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(1234)),
				Register.CL);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem16_reg16()
		{
			var instrString = "adc WORD [1234], CX";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(1234)),
				Register.CX);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem32_reg32()
		{
			var instrString = "adc DWORD [1234], ECX";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(1234)),
				Register.ECX);

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void regmem64_reg64()
		{
			var instrString = "adc QWORD [1234], RCX";
			var instruction = Instr.Adc(
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(1234)),
				Register.RCX);

			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void reg8_regmem8()
		{
			var instrString = "adc CL, BYTE [1234]";
			var instruction = Instr.Adc(
				Register.CL,
				new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new ReferenceOffset(1234)));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void reg16_regmem16()
		{
			var instrString = "adc CX, WORD [1234]";
			var instruction = Instr.Adc(
				Register.CX,
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new ReferenceOffset(1234)));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg32, reg/mem32</c> instruction variant.
		/// </summary>
		[Test]
		public void reg32_regmem32()
		{
			var instrString = "adc ECX, DWORD [1234]";
			var instruction = Instr.Adc(
				Register.ECX,
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new ReferenceOffset(1234)));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		[Test]
		public void reg64_regmem64()
		{
			var instruction = Instr.Adc(
				Register.RCX,
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(1234)));

			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x13, 0x0C, 0x25, 0xD2, 0x04, 0x00, 0x00 });
		}



		[Test]
		public void test()
		{
			var x = new RelativeOffset(c => 0x1234, DataSize.Bit16);

			var instruction = Instr.Adc(
				Register.RCX,
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new ReferenceOffset(1234)));

			AssertInstructionFail(instruction, DataSize.Bit16);
			AssertInstructionFail(instruction, DataSize.Bit32);
			AssertInstruction(instruction, DataSize.Bit64, new byte[] { 0x48, 0x13, 0x0C, 0x25, 0xD2, 0x04, 0x00, 0x00 });
		}
	}
}
