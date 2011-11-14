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

namespace SharpAssembler.x86
{
	partial class Instruction
	{
		/// <summary>
		/// Specifies how an operand is encoded.
		/// </summary>
		internal enum OperandEncoding
		{
			/// <summary>
			/// The default encoding.
			/// </summary>
			/// <remarks>This has the following influence on the operand's encoding:
			/// <list type="table">
			/// <listheader><term>Operand type</term><description>Encoding</description></listheader>
			/// <item>
			/// <term><see cref="SharpAssembler.x86.Instruction.OperandType.RegisterOperand"/></term>
			/// <description>ModR/M.Reg = value</description>
			/// </item>
			/// </list>
			/// </remarks>
			Default,
			/// <summary>
			/// Adds the value to the last opcode byte. This is only valid for the
			/// <see cref="SharpAssembler.x86.Instruction.OperandType.RegisterOperand"/> operand type.
			/// </summary>
			/// <remarks>This has the following influence on the operand's encoding:
			/// <list type="table">
			/// <listheader><term>Operand type</term><description>Encoding</description></listheader>
			/// <item>
			/// <term><see cref="SharpAssembler.x86.Instruction.OperandType.RegisterOperand"/></term>
			/// <description>Opcode + value</description>
			/// </item>
			/// </list>
			/// </remarks>
			OpcodeAdd,
			/// <summary>
			/// Sets the immediate value as the 'extra' immediate value, which is encoded after the normal immediate
			/// value. This is only valid for the <see cref="SharpAssembler.x86.Instruction.OperandType.Immediate"/> operand
			/// type.
			/// </summary>
			ExtraImmediate,
		}
	}
}
