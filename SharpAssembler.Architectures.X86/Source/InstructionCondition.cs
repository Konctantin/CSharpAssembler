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

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Specifies the type of condition on which the instruction executes.
	/// </summary>
	/// <remarks>
	/// The lower 8 bits specify the condition code as used by the x86 instruction set. Bits 8-11 are 1 when it is a
	/// flag condition, 2 when it is a comparison operator or 3 when it deals with even and odd parity. Bits 12-15 are
	/// 1 when it is the complement of the same value with those bits set to 0.
	/// </remarks>
	public enum InstructionCondition
	{
		/// <summary>
		/// No condition or unconditional.
		/// </summary>
		None = 0,
		/// <summary>
		/// The overflow flag (<c>OF</c>) equals 1.
		/// </summary>
		Overflow = 0x00 | InstructionConditionType.Flag,
		/// <summary>
		/// The overflow flag (<c>OF</c>) equals 0.
		/// </summary>
		NotOverflow = 0x01 | InstructionConditionType.Flag | InstructionConditionType.Complement,
		/// <summary>
		/// The carry flag (<c>CF</c>) equals 1.
		/// </summary>
		Below = 0x02 | InstructionConditionType.Comparison,
		/// <summary>
		/// The carry flag (<c>CF</c>) equals 1.
		/// </summary>
		Carry = 0x02 | InstructionConditionType.Flag,
		/// <summary>
		/// The carry flag (<c>CF</c>) equals 1.
		/// </summary>
		NotAboveOrEqual = 0x02 | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The carry flag (<c>CF</c>) equals 0.
		/// </summary>
		NotBelow = 0x03 | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The carry flag (<c>CF</c>) equals 0.
		/// </summary>
		NotCarry = 0x03 | InstructionConditionType.Flag | InstructionConditionType.Complement,
		/// <summary>
		/// The carry flag (<c>CF</c>) equals 0.
		/// </summary>
		AboveOrEqual = 0x03 | InstructionConditionType.Comparison,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 1.
		/// </summary>
		Zero = 0x04 | InstructionConditionType.Flag,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 1.
		/// </summary>
		Equal = 0x04 | InstructionConditionType.Comparison,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 0.
		/// </summary>
		NotZero = 0x05 | InstructionConditionType.Flag | InstructionConditionType.Complement,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 0.
		/// </summary>
		NotEqual = 0x05 | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The carry flag (<c>CF</c>) or the zero flag (<c>ZF</c>) equals 1.
		/// </summary>
		BelowOrEqual = 0x06 | InstructionConditionType.Comparison,
		/// <summary>
		/// The carry flag (<c>CF</c>) or the zero flag (<c>ZF</c>) equals 1.
		/// </summary>
		NotAbove = 0x06 | InstructionConditionType.Flag | InstructionConditionType.Complement,
		/// <summary>
		/// The carry flag (<c>CF</c>) and the zero flag (<c>ZF</c>) equal 0.
		/// </summary>
		NotBelowOrEqual = 0x07 | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The carry flag (<c>CF</c>) and the zero flag (<c>ZF</c>) equal 0.
		/// </summary>
		Above = 0x07 | InstructionConditionType.Comparison,
		/// <summary>
		/// The sign flag (<c>SF</c>) equals 1.
		/// </summary>
		Sign = 0x08 | InstructionConditionType.Flag,
		/// <summary>
		/// The sign flag (<c>SF</c>) equals 0.
		/// </summary>
		NotSign = 0x09 | InstructionConditionType.Flag | InstructionConditionType.Complement,
		/// <summary>
		/// The parity flag (<c>PF</c>) equals 1.
		/// </summary>
		Parity = 0x0A | InstructionConditionType.Flag,
		/// <summary>
		/// The parity flag (<c>PF</c>) equals 1.
		/// </summary>
		ParityEven = 0x0A | InstructionConditionType.Parity,
		/// <summary>
		/// The parity flag (<c>PF</c>) equals 0.
		/// </summary>
		NotParity = 0x0B | InstructionConditionType.Flag | InstructionConditionType.Complement,
		/// <summary>
		/// The parity flag (<c>PF</c>) equals 0.
		/// </summary>
		ParityOdd = 0x0B | InstructionConditionType.Parity,
		/// <summary>
		/// The sign flag (<c>SF</c>) does not equal the overflow flag (<c>OF</c>).
		/// </summary>
		Less = 0x0C | InstructionConditionType.Comparison,
		/// <summary>
		/// The sign flag (<c>SF</c>) does not equal the overflow flag (<c>OF</c>).
		/// </summary>
		NotGreaterOrEqual = 0x0C | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The sign flag (<c>SF</c>) equals the overflow flag (<c>OF</c>).
		/// </summary>
		NotLess = 0x0D | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The sign flag (<c>SF</c>) equals the overflow flag (<c>OF</c>).
		/// </summary>
		GreaterOrEqual = 0x0D | InstructionConditionType.Comparison,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 1 or the sign flag (<c>SF</c>) does not equal the overflow flag (<c>OF</c>).
		/// </summary>
		LessOrEqual = 0x0E | InstructionConditionType.Comparison,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 1 or the sign flag (<c>SF</c>) does not equal the overflow flag (<c>OF</c>).
		/// </summary>
		NotGreater = 0x0E | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 0 and the sign flag (<c>SF</c>) equals the overflow flag (<c>OF</c>).
		/// </summary>
		NotLessOrEqual = 0x0F | InstructionConditionType.Comparison | InstructionConditionType.Complement,
		/// <summary>
		/// The zero flag (<c>ZF</c>) equals 0 and the sign flag (<c>SF</c>) equals the overflow flag (<c>OF</c>).
		/// </summary>
		Greater = 0x0F | InstructionConditionType.Comparison,
	}

	/// <summary>
	/// Specifies the type of an instruction condition.
	/// </summary>
	internal enum InstructionConditionType
	{
		/// <summary>
		/// The instruction condition does not have a special type.
		/// </summary>
		None = 0x000,
		/// <summary>
		/// The instruction condition is a flag condition.
		/// </summary>
		Flag = 0x100,
		/// <summary>
		/// The instruction condition is a comparison operator.
		/// </summary>
		Comparison = 0x200,
		/// <summary>
		/// The instruction condition deals with even and odd parity.
		/// </summary>
		Parity = 0x300,

		/// <summary>
		/// The instruction condition is the complement of the instruction condition which does not have this
		/// enumeration member's value set.
		/// </summary>
		Complement = 0x1000,
	}

	/// <summary>
	/// Extension methods to the <see cref="InstructionCondition"/> enumeration type.
	/// </summary>
	public static class InstructionConditionExtensions
	{
		/// <summary>
		/// Gets the 8-bit condition code as used by the instruction set.
		/// </summary>
		/// <param name="condition">The <see cref="InstructionCondition"/>.</param>
		/// <returns>The condition code.</returns>
		public static int GetConditionCode(this InstructionCondition condition)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Ensures(Contract.Result<int>() >= 0x00);
			Contract.Ensures(Contract.Result<int>() <= 0xFF);
			#endregion

			return ((int)condition) & 0xFF;
		}
	}
}
