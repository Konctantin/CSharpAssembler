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
using System.Collections.Generic;
using System.Linq;

namespace SharpAssembler.Architectures.X86.Opcodes
{
	/// <summary>
	/// The AND (Logical AND) instruction opcode.
	/// </summary>
	/// <remarks>
	/// Instructions with this opcode expect two operands that have the following semantics:
	/// <list type="table">
	/// <listheader><term>Index</term><description>Semantics</description></listheader>
	/// <item><term>0</term><description>Destination</description></item>
	/// <item><term>1</term><description>Source</description></item>
	/// </list>
	/// </remarks>
	public class AndOpcode : X86Opcode
	{
		/// <inheritdoc />
		public override bool CanLock
		{
			get { return true; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AndOpcode"/> class.
		/// </summary>
		public AndOpcode()
			: base("and", 2, GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion
		
		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				#region Variants
				// AND AL, imm8
				new X86OpcodeVariant(
					new byte[] { 0x24 },
					new OperandDescriptor(Register.AL),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// AND AX, imm16
				new X86OpcodeVariant(
					new byte[] { 0x25 },
					new OperandDescriptor(Register.AX),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
				// AND EAX, imm32
				new X86OpcodeVariant(
					new byte[] { 0x25 },
					new OperandDescriptor(Register.EAX),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// AND RAX, imm32
				new X86OpcodeVariant(
					new byte[] { 0x25 },
					new OperandDescriptor(Register.RAX),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

				// AND reg/mem8, imm8
				new X86OpcodeVariant(
					new byte[] { 0x80 }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// AND reg/mem16, imm16
				new X86OpcodeVariant(
					new byte[] { 0x81 }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
				// AND reg/mem32, imm32
				new X86OpcodeVariant(
					new byte[] { 0x81 }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
				// AND reg/mem64, imm32
				new X86OpcodeVariant(
					new byte[] { 0x81 }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

				// AND reg/mem16, imm8
				new X86OpcodeVariant(
					new byte[] { 0x83 }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// AND reg/mem32, imm8
				new X86OpcodeVariant(
					new byte[] { 0x83 }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
				// AND reg/mem64, imm8
				new X86OpcodeVariant(
					new byte[] { 0x83 }, 4,
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),


				// AND reg/mem8, reg8
				new X86OpcodeVariant(
					new byte[] { 0x20 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
				// AND reg/mem16, reg16
				new X86OpcodeVariant(
					new byte[] { 0x21 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// AND reg/mem32, reg32
				new X86OpcodeVariant(
					new byte[] { 0x21 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// AND reg/mem64, reg64
				new X86OpcodeVariant(
					new byte[] { 0x21 },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),


				// AND reg8, reg/mem8
				new X86OpcodeVariant(
					new byte[] { 0x22 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
				// AND reg16, reg/mem16
				new X86OpcodeVariant(
					new byte[] { 0x23 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
				// AND reg32, reg/mem32
				new X86OpcodeVariant(
					new byte[] { 0x23 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
				// AND reg64, reg/mem64
				new X86OpcodeVariant(
					new byte[] { 0x23 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
				#endregion
			};
		}
	}
}
