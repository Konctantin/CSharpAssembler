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
using System.Diagnostics.Contracts;
using SharpAssembler;

namespace SharpAssembler.OpcodeWriter.X86
{
	/// <summary>
	/// An x86-64 register.
	/// </summary>
	/// <remarks>
	/// <list type="table">
	/// With a simple operations, it is possible to derive some special features from the values of the enumeration
	/// members.
	/// <listheader><term>Operation</term><description>Result</description></listheader>
	/// <item><term>value &amp; 0x7</term><description>The value of the register.</description></item>
	/// <item><term>value &amp; 0x8</term><description>A value of 1 when REX.b, REX.r or REX.x is set;
	/// otherwise, 0.</description></item>
	/// <item><term>value &amp; 0x10</term><description>A value of 1 when a REX prefix is required;
	/// otherwise, 0.</description></item>
	/// <item><term>value &gt;&gt; 5</term><description>The <see cref="RegisterType"/> of the
	/// register.</description></item>
	/// <item><term>(value &gt;&gt; 5) &amp; 0xFF</term><description>The number of bytes required for this type of
	/// register.</description></item>
	/// <item><term>(value &gt;&gt; 5) &amp; 0xFF</term><description>The <see cref="DataSize"/> of this
	/// register.</description></item>
	/// </list>
	/// </remarks>
	public enum Register
	{
		/// <summary>
		/// No register.
		/// </summary>
		None = 0,




		/// <summary>
		/// The lower 8-bits of the accumulator register.
		/// </summary>
		AL = 0x00 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the accumulator register.
		/// </summary>
		AX = 0x00 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the accumulator register.
		/// </summary>
		EAX = 0x00 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the accumulator register.
		/// </summary>
		RAX = 0x00 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The first 64-bit SIMD register.
		/// </summary>
		MM0 = 0x00 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The first 128-bit SIMD register.
		/// </summary>
		XMM0 = 0x00 | (RegisterType.Simd128Bit << 5),
		/// <summary>
		/// The extra segment register.
		/// </summary>
		ES = 0x00 | (RegisterType.Segment << 5),
		/// <summary>
		/// The first control register.
		/// </summary>
		CR0 = 0x00 | (RegisterType.Control << 5),
		/// <summary>
		/// The first debug register.
		/// </summary>
		DR0 = 0x00 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 0.
		/// </summary>
		ST0 = 0x00 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The lower 8-bits of the counter register.
		/// </summary>
		CL = 0x01 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the counter register.
		/// </summary>
		CX = 0x01 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the counter register.
		/// </summary>
		ECX = 0x01 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the counter register.
		/// </summary>
		RCX = 0x01 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The second 64-bit SIMD register.
		/// </summary>
		MM1 = 0x01 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The second 128-bit SIMD register.
		/// </summary>
		XMM1 = 0x01 | (RegisterType.Simd128Bit << 5),
		/// <summary>
		/// The code segment register.
		/// </summary>
		CS = 0x01 | (RegisterType.Segment << 5),
		/// <summary>
		/// The second control register.
		/// </summary>
		CR1 = 0x01 | (RegisterType.Control << 5),
		/// <summary>
		/// The second debug register.
		/// </summary>
		DR1 = 0x01 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 1.
		/// </summary>
		ST1 = 0x01 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The lower 8-bits of the data register.
		/// </summary>
		DL = 0x02 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the data register.
		/// </summary>
		DX = 0x02 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the data register.
		/// </summary>
		EDX = 0x02 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the data register.
		/// </summary>
		RDX = 0x02 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The third 64-bit SIMD register.
		/// </summary>
		MM2 = 0x02 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The third 128-bit SIMD register.
		/// </summary>
		XMM2 = 0x02 | (RegisterType.Simd128Bit << 5),
		/// <summary>
		/// The stack segment register.
		/// </summary>
		SS = 0x02 | (RegisterType.Segment << 5),
		/// <summary>
		/// The third control register.
		/// </summary>
		CR2 = 0x02 | (RegisterType.Control << 5),
		/// <summary>
		/// The third debug register.
		/// </summary>
		DR2 = 0x02 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 2.
		/// </summary>
		ST2 = 0x02 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The lower 8-bits of the base register.
		/// </summary>
		BL = 0x03 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the base register.
		/// </summary>
		BX = 0x03 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the base register.
		/// </summary>
		EBX = 0x03 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the base register.
		/// </summary>
		RBX = 0x03 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The fourth 64-bit SIMD register.
		/// </summary>
		MM3 = 0x03 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The fourth 128-bit SIMD register.
		/// </summary>
		XMM3 = 0x03 | (RegisterType.Simd128Bit << 5),
		/// <summary>
		/// The data segment register.
		/// </summary>
		DS = 0x03 | (RegisterType.Segment << 5),
		/// <summary>
		/// The fourth control register.
		/// </summary>
		CR3 = 0x03 | (RegisterType.Control << 5),
		/// <summary>
		/// The fourth debug register.
		/// </summary>
		DR3 = 0x03 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 3.
		/// </summary>
		ST3 = 0x03 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The higher 8-bits of the lower 16-bits of the accumulator register.
		/// </summary>
		AH = 0x04 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 8-bits of the stack pointer register. 
		/// </summary>
		SPL = AH | 0x10,	// Required REX prefix.
		/// <summary>
		/// The lower 16-bits of the stack pointer register.
		/// </summary>
		SP = 0x04 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the stack pointer register.
		/// </summary>
		ESP = 0x04 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the stack pointer register.
		/// </summary>
		RSP = 0x04 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The fifth 64-bit SIMD register.
		/// </summary>
		MM4 = 0x04 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The fifth 128-bit SIMD register.
		/// </summary>
		XMM4 = 0x04 | (RegisterType.Simd128Bit << 5),
		/// <summary>
		/// The second extra segment register.
		/// </summary>
		FS = 0x04 | (RegisterType.Segment << 5),
		/// <summary>
		/// The fifth control register.
		/// </summary>
		CR4 = 0x04 | (RegisterType.Control << 5),
		/// <summary>
		/// The fifth debug register.
		/// </summary>
		DR4 = 0x04 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 4.
		/// </summary>
		ST4 = 0x04 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The higher 8-bits of the lower 16-bits of the counter register.
		/// </summary>
		CH = 0x05 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 8-bits of the base pointer register. 
		/// </summary>
		BPL = CH | 0x10,	// Required REX prefix.
		/// <summary>
		/// The lower 16-bits of the base pointer register.
		/// </summary>
		BP = 0x05 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the base pointer register.
		/// </summary>
		EBP = 0x05 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the base pointer register.
		/// </summary>
		RBP = 0x05 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The sixth 64-bit SIMD register.
		/// </summary>
		MM5 = 0x05 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The sixth 128-bit SIMD register.
		/// </summary>
		XMM5 = 0x05 | (RegisterType.Simd128Bit << 5),
		/// <summary>
		/// The third extra segment register.
		/// </summary>
		GS = 0x05 | (RegisterType.Segment << 5),
		/// <summary>
		/// The sixth control register.
		/// </summary>
		CR5 = 0x05 | (RegisterType.Control << 5),
		/// <summary>
		/// The sixth debug register.
		/// </summary>
		DR5 = 0x05 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 5.
		/// </summary>
		ST5 = 0x05 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The higher 8-bits of the lower 16-bits of the data register.
		/// </summary>
		DH = 0x06 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 8-bits of the source index register. 
		/// </summary>
		SIL = DH | 0x10,	// Required REX prefix.
		/// <summary>
		/// The lower 16-bits of the source index register.
		/// </summary>
		SI = 0x06 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the source index register.
		/// </summary>
		ESI = 0x06 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the source index register.
		/// </summary>
		RSI = 0x06 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The seventh 64-bit SIMD register.
		/// </summary>
		MM6 = 0x06 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The seventh 128-bit SIMD register.
		/// </summary>
		XMM6 = 0x06 | (RegisterType.Simd128Bit << 5),
		// Stack register
		/// <summary>
		/// The seventh control register.
		/// </summary>
		CR6 = 0x06 | (RegisterType.Control << 5),
		/// <summary>
		/// The seventh debug register.
		/// </summary>
		DR6 = 0x06 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 6.
		/// </summary>
		ST6 = 0x06 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The higher 8-bits of the lower 16-bits of the base register.
		/// </summary>
		BH = 0x07 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 8-bits of the destination index register. 
		/// </summary>
		DIL = BH | 0x10,	// Required REX prefix.
		/// <summary>
		/// The lower 16-bits of the destination index register.
		/// </summary>
		DI = 0x07 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the destination index register.
		/// </summary>
		EDI = 0x07 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the destination index register.
		/// </summary>
		RDI = 0x07 | (RegisterType.GeneralPurpose64Bit << 5),
		/// <summary>
		/// The eighth 64-bit SIMD register.
		/// </summary>
		MM7 = 0x07 | (RegisterType.Simd64Bit << 5),
		/// <summary>
		/// The eighth 128-bit SIMD register.
		/// </summary>
		XMM7 = 0x07 | (RegisterType.Simd128Bit << 5),
		// Stack register
		/// <summary>
		/// The eighth control register.
		/// </summary>
		CR7 = 0x07 | (RegisterType.Control << 5),
		/// <summary>
		/// The eighth debug register.
		/// </summary>
		DR7 = 0x07 | (RegisterType.Debug << 5),
		/// <summary>
		/// The floating point stack register index 7.
		/// </summary>
		ST7 = 0x07 | (RegisterType.FloatingPoint << 5),



		/// <summary>
		/// The lower 8-bits of the nineth general purpose register. 
		/// </summary>
		R8L = 0x08 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the nineth general purpose register.
		/// </summary>
		R8W = 0x08 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the nineth general purpose register.
		/// </summary>
		R8D = 0x08 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the nineth general purpose register.
		/// </summary>
		R8 = 0x08 | (RegisterType.GeneralPurpose64Bit << 5),
		// MM0
		/// <summary>
		/// The nineth 128-bit SIMD register.
		/// </summary>
		XMM8 = 0x08 | (RegisterType.Simd128Bit << 5),
		// ES
		/// <summary>
		/// The nineth control register.
		/// </summary>
		CR8 = 0x08 | (RegisterType.Control << 5),
		/// <summary>
		/// The nineth debug register.
		/// </summary>
		DR8 = 0x08 | (RegisterType.Debug << 5),
		// Floating point stack register.


		/// <summary>
		/// The lower 8-bits of the tenth general purpose register. 
		/// </summary>
		R9L = 0x09 | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the tenth general purpose register.
		/// </summary>
		R9W = 0x09 | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the tenth general purpose register.
		/// </summary>
		R9D = 0x09 | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the tenth general purpose register.
		/// </summary>
		R9 = 0x09 | (RegisterType.GeneralPurpose64Bit << 5),
		// MM1
		/// <summary>
		/// The tenth 128-bit SIMD register.
		/// </summary>
		XMM9 = 0x09 | (RegisterType.Simd128Bit << 5),
		// CS
		/// <summary>
		/// The tenth control register.
		/// </summary>
		CR9 = 0x09 | (RegisterType.Control << 5),
		/// <summary>
		/// The tenth debug register.
		/// </summary>
		DR9 = 0x09 | (RegisterType.Debug << 5),
		// Floating point stack register.



		/// <summary>
		/// The lower 8-bits of the eleventh general purpose register. 
		/// </summary>
		R10L = 0x0A | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the eleventh general purpose register.
		/// </summary>
		R10W = 0x0A | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the eleventh general purpose register.
		/// </summary>
		R10D = 0x0A | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the eleventh general purpose register.
		/// </summary>
		R10 = 0x0A | (RegisterType.GeneralPurpose64Bit << 5),
		// MM2
		/// <summary>
		/// The eleventh 128-bit SIMD register.
		/// </summary>
		XMM10 = 0x0A | (RegisterType.Simd128Bit << 5),
		// SS
		/// <summary>
		/// The eleventh control register.
		/// </summary>
		CR10 = 0x0A | (RegisterType.Control << 5),
		/// <summary>
		/// The eleventh debug register.
		/// </summary>
		DR10 = 0x0A | (RegisterType.Debug << 5),
		// Floating point stack register.


		/// <summary>
		/// The lower 8-bits of the twelfth general purpose register. 
		/// </summary>
		R11L = 0x0B | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the twelfth general purpose register.
		/// </summary>
		R11W = 0x0B | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the twelfth general purpose register.
		/// </summary>
		R11D = 0x0B | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the twelfth general purpose register.
		/// </summary>
		R11 = 0x0B | (RegisterType.GeneralPurpose64Bit << 5),
		// MM3
		/// <summary>
		/// The twelfth 128-bit SIMD register.
		/// </summary>
		XMM11 = 0x0B | (RegisterType.Simd128Bit << 5),
		// DS
		/// <summary>
		/// The twelfth control register.
		/// </summary>
		CR11 = 0x0B | (RegisterType.Control << 5),
		/// <summary>
		/// The twelfth debug register.
		/// </summary>
		DR11 = 0x0B | (RegisterType.Debug << 5),
		// Floating point stack register.




		/// <summary>
		/// The lower 8-bits of the thirteenth general purpose register. 
		/// </summary>
		R12L = 0x0C | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the thirteenth general purpose register.
		/// </summary>
		R12W = 0x0C | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the thirteenth general purpose register.
		/// </summary>
		R12D = 0x0C | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the thirteenth general purpose register.
		/// </summary>
		R12 = 0x0C | (RegisterType.GeneralPurpose64Bit << 5),
		// MM4
		/// <summary>
		/// The thirteenth 128-bit SIMD register.
		/// </summary>
		XMM12 = 0x0C | (RegisterType.Simd128Bit << 5),
		// FS
		/// <summary>
		/// The thirteenth control register.
		/// </summary>
		CR12 = 0x0C | (RegisterType.Control << 5),
		/// <summary>
		/// The thirteenth debug register.
		/// </summary>
		DR12 = 0x0C | (RegisterType.Debug << 5),
		// Floating point stack register.



		/// <summary>
		/// The lower 8-bits of the fourteenth general purpose register. 
		/// </summary>
		R13L = 0x0D | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the fourteenth general purpose register.
		/// </summary>
		R13W = 0x0D | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the fourteenth general purpose register.
		/// </summary>
		R13D = 0x0D | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the fourteenth general purpose register.
		/// </summary>
		R13 = 0x0D | (RegisterType.GeneralPurpose64Bit << 5),
		// MM5
		/// <summary>
		/// The fourteenth 128-bit SIMD register.
		/// </summary>
		XMM13 = 0x0D | (RegisterType.Simd128Bit << 5),
		// GS
		/// <summary>
		/// The fourteenth control register.
		/// </summary>
		CR13 = 0x0D | (RegisterType.Control << 5),
		/// <summary>
		/// The fourteenth debug register.
		/// </summary>
		DR13 = 0x0D | (RegisterType.Debug << 5),
		// Floating point stack register.



		/// <summary>
		/// The lower 8-bits of the fifteenth general purpose register. 
		/// </summary>
		R14L = 0x0E | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the fifteenth general purpose register.
		/// </summary>
		R14W = 0x0E | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the fifteenth general purpose register.
		/// </summary>
		R14D = 0x0E | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the fifteenth general purpose register.
		/// </summary>
		R14 = 0x0E | (RegisterType.GeneralPurpose64Bit << 5),
		// MM6
		/// <summary>
		/// The fifteenth 128-bit SIMD register.
		/// </summary>
		XMM14 = 0x0E | (RegisterType.Simd128Bit << 5),
		// Stack register
		/// <summary>
		/// The fifteenth control register.
		/// </summary>
		CR14 = 0x0E | (RegisterType.Control << 5),
		/// <summary>
		/// The fifteenth debug register.
		/// </summary>
		DR14 = 0x0E | (RegisterType.Debug << 5),
		// Floating point stack register.



		/// <summary>
		/// The lower 8-bits of the sixteenth general purpose register. 
		/// </summary>
		R15L = 0x0F | (RegisterType.GeneralPurpose8Bit << 5),
		/// <summary>
		/// The lower 16-bits of the sixteenth general purpose register.
		/// </summary>
		R15W = 0x0F | (RegisterType.GeneralPurpose16Bit << 5),
		/// <summary>
		/// The lower 32-bits of the sixteenth general purpose register.
		/// </summary>
		R15D = 0x0F | (RegisterType.GeneralPurpose32Bit << 5),
		/// <summary>
		/// The lower 64-bits of the sixteenth general purpose register.
		/// </summary>
		R15 = 0x0F | (RegisterType.GeneralPurpose64Bit << 5),
		// MM7
		/// <summary>
		/// The sixteenth 128-bit SIMD register.
		/// </summary>
		XMM15 = 0x0F | (RegisterType.Simd128Bit << 5),
		// Stack register
		/// <summary>
		/// The sixteenth control register.
		/// </summary>
		CR15 = 0x0F | (RegisterType.Control << 5),
		/// <summary>
		/// The sixteenth debug register.
		/// </summary>
		DR15 = 0x0F | (RegisterType.Debug << 5),
		// Floating point stack register.
	}



	/// <summary>
	/// Extensions for the <see cref="Register"/> type.
	/// </summary>
	public static class RegisterExtensions
	{
		/// <summary>
		/// Returns the register type of the specified register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to get the type for.</param>
		/// <returns>The <see cref="RegisterType"/> of the register.</returns>
		[Pure]
		public static RegisterType GetRegisterType(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			Contract.Ensures(Enum.IsDefined(typeof(RegisterType), Contract.Result<RegisterType>()));
			#endregion
			return (RegisterType)((int)register >> 5);
		}

		/// <summary>
		/// Returns the value of the specified register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to get the value for.</param>
		/// <returns>The value (between 0 and 15) of the register.</returns>
		[Pure]
		public static byte GetValue(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			Contract.Ensures(Contract.Result<byte>() >= 0 && Contract.Result<byte>() <= 15);
			#endregion
			return (byte)((int)register & 0xF);
		}

		/// <summary>
		/// Returns the register size of the specified register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to get the size for.</param>
		/// <returns>The <see cref="DataSize"/> of the register.</returns>
		[Pure]
		public static DataSize GetSize(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			#endregion
			return GetRegisterType(register).GetSize();
		}

		#region Register Types
		/// <summary>
		/// Returns whether the register is a general purpose register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to test.</param>
		/// <returns><see langword="true"/> when the register is a general purpose register;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsGeneralPurposeRegister(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			#endregion
			var registerType = register.GetRegisterType();
			return registerType == RegisterType.GeneralPurpose8Bit
				|| registerType == RegisterType.GeneralPurpose16Bit
				|| registerType == RegisterType.GeneralPurpose32Bit
				|| registerType == RegisterType.GeneralPurpose64Bit;
		}

		/// <summary>
		/// Returns whether the register is a control register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to test.</param>
		/// <returns><see langword="true"/> when the register is a control register;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsControlRegister(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			#endregion
			return register.GetRegisterType() == RegisterType.Control;
		}

		/// <summary>
		/// Returns whether the register is a debug register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to test.</param>
		/// <returns><see langword="true"/> when the register is a debug register;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsDebugRegister(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			#endregion
			return register.GetRegisterType() == RegisterType.Debug;
		}

		/// <summary>
		/// Returns whether the register is an x87 floating-point register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to test.</param>
		/// <returns><see langword="true"/> when the register is an x87 floating-point register;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsFloatingPointRegister(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			#endregion
			return register.GetRegisterType() == RegisterType.FloatingPoint;
		}

		/// <summary>
		/// Returns whether the register is a segment register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to test.</param>
		/// <returns><see langword="true"/> when the register is a segment register;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsSegmentRegister(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			#endregion
			return register.GetRegisterType() == RegisterType.Segment;
		}

		/// <summary>
		/// Returns whether the register is a SIMD register.
		/// </summary>
		/// <param name="register">The <see cref="Register"/> to test.</param>
		/// <returns><see langword="true"/> when the register is a SIMD register;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		public static bool IsSimdRegister(this Register register)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
			#endregion
			var registerType = register.GetRegisterType();
			return registerType == RegisterType.Simd64Bit
				|| registerType == RegisterType.Simd128Bit;
		}
		#endregion
	}
}
