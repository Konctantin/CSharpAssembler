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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Instructions;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Tests.Instructions
{
	/// <summary>
	/// Tests the <see cref="Adc"/> instruction.
	/// </summary>
	[TestFixture]
	public class AdcTest : InstructionTestBase
	{
		/// <summary>
		/// Tests the <c>adc AL, imm8</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_AL_imm8()
		{
			var instruction = new Adc(
				new RegisterOperand(Register.AL),
				new Immediate(123));

			Assert16BitInstruction(instruction,
				new byte[] { 0x14, 0x7B });
			Assert32BitInstruction(instruction,
				new byte[] { 0x14, 0x7B });
			Assert64BitInstruction(instruction,
				new byte[] { 0x14, 0x7B });
		}

		/// <summary>
		/// Tests the <c>adc AX, imm16</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_AX_imm16()
		{
			var instrString = "adc AX, 12345";
			var instruction = new Adc(
				new RegisterOperand(Register.AX),
				new Immediate(12345));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc EAX, imm32</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_EAX_imm32()
		{
			var instrString = "adc EAX, 1234567890";
			var instruction = new Adc(
				new RegisterOperand(Register.EAX),
				new Immediate(1234567890));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc RAX, imm32</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_RAX_imm32()
		{
			var instruction = new Adc(
				new RegisterOperand(Register.RAX),
				new Immediate(1234567890));

			Assert16BitInstructionFails(instruction);
			Assert32BitInstructionFails(instruction);
			Assert64BitInstruction(instruction,
				new byte[] { 0x48, 0x15, 0xD2, 0x02, 0x96, 0x49 });
		}

		/// <summary>
		/// Tests the <c>adc reg/mem8, imm8</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem8_imm8()
		{
			var instrString = "adc BYTE [1234], 123";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new SimpleExpression(1234)),
				new Immediate(123));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem16, imm16</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem16_imm16()
		{
			var instrString = "adc WORD [1234], WORD 12345";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new SimpleExpression(1234)),
				new Immediate(12345, DataSize.Bit16));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem32, imm32</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem32_imm32()
		{
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new SimpleExpression(1234)),
				new Immediate(12345, DataSize.Bit32));

			Assert16BitInstruction(instruction,
				new byte[] { 0x66, 0x81, 0x16, 0xd2, 0x04, 0x39, 0x30, 0x00, 0x00 });
			Assert32BitInstruction(instruction,
				new byte[] { 0x81, 0x15, 0xd2, 0x04, 0x00, 0x00, 0x39, 0x30, 0x00, 0x00 });
			Assert64BitInstruction(instruction,
				new byte[] { 0x81, 0x14, 0x25, 0xd2, 0x04, 0x00, 0x00, 0x39, 0x30, 0x00, 0x00 });
		}

		/// <summary>
		/// Tests the <c>adc reg/mem64, imm32</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem64_imm32()
		{
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new SimpleExpression(1234)),
				new Immediate(12345, DataSize.Bit32));

			Assert16BitInstructionFails(instruction);
			Assert32BitInstructionFails(instruction);
			Assert64BitInstruction(instruction,
				new byte[] { 0x48, 0x81, 0x14, 0x25, 0xD2, 0x04, 0x00, 0x00, 0x39, 0x30, 0x00, 0x00 });
		}

		/// <summary>
		/// Tests the <c>adc reg/mem16, imm8</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem16_imm8()
		{
			var instrString = "adc WORD [1234], 123";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new SimpleExpression(1234)),
				new Immediate(123));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem32, imm8</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem32_imm8()
		{
			var instrString = "adc DWORD [1234], 123";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new SimpleExpression(1234)),
				new Immediate(123));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem64, imm8</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem64_imm8()
		{
			var instrString = "adc QWORD [1234], 123";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new SimpleExpression(1234)),
				new Immediate(123));

			Assert16BitInstructionFails(instruction);
			Assert32BitInstructionFails(instruction);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem8, reg8</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem8_reg8()
		{
			var instrString = "adc BYTE [1234], CL";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new SimpleExpression(1234)),
				new RegisterOperand(Register.CL));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem16, reg16</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem16_reg16()
		{
			var instrString = "adc WORD [1234], CX";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new SimpleExpression(1234)),
				new RegisterOperand(Register.CX));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem32, reg32</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem32_reg32()
		{
			var instrString = "adc DWORD [1234], ECX";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new SimpleExpression(1234)),
				new RegisterOperand(Register.ECX));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg/mem64, reg64</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_regmem64_reg64()
		{
			var instrString = "adc QWORD [1234], RCX";
			var instruction = new Adc(
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new SimpleExpression(1234)),
				new RegisterOperand(Register.RCX));

			Assert16BitInstructionFails(instruction);
			Assert32BitInstructionFails(instruction);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg8, reg/mem8</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_reg8_regmem8()
		{
			var instrString = "adc CL, BYTE [1234]";
			var instruction = new Adc(
				new RegisterOperand(Register.CL),
				new EffectiveAddress(DataSize.Bit8, DataSize.None, c => new SimpleExpression(1234)));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg16, reg/mem16</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_reg16_regmem16()
		{
			var instrString = "adc CX, WORD [1234]";
			var instruction = new Adc(
				new RegisterOperand(Register.CX),
				new EffectiveAddress(DataSize.Bit16, DataSize.None, c => new SimpleExpression(1234)));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg32, reg/mem32</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_reg32_regmem32()
		{
			var instrString = "adc ECX, DWORD [1234]";
			var instruction = new Adc(
				new RegisterOperand(Register.ECX),
				new EffectiveAddress(DataSize.Bit32, DataSize.None, c => new SimpleExpression(1234)));

			AssertInstruction(instruction, instrString, DataSize.Bit16);
			AssertInstruction(instruction, instrString, DataSize.Bit32);
			AssertInstruction(instruction, instrString, DataSize.Bit64);
		}

		/// <summary>
		/// Tests the <c>adc reg64, reg/mem64</c> instruction variant.
		/// </summary>
		[Test]
		public void Adc_reg64_regmem64()
		{
			var instruction = new Adc(
				new RegisterOperand(Register.RCX),
				new EffectiveAddress(DataSize.Bit64, DataSize.None, c => new SimpleExpression(1234)));
			
			Assert16BitInstructionFails(instruction);
			Assert32BitInstructionFails(instruction);
			Assert64BitInstruction(instruction,
				new byte[] { 0x48, 0x13, 0x0C, 0x25, 0xD2, 0x04, 0x00, 0x00 });
		}
	}
}
