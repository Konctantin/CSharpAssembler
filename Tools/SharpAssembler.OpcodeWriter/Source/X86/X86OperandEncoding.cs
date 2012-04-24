using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.OpcodeWriter.X86
{
	/// <summary>
	/// Specifies how an operand is encoded.
	/// </summary>
	public enum X86OperandEncoding
	{
		/// <summary>
		/// The default encoding.
		/// </summary>
		/// <remarks>This has the following influence on the operand's encoding:
		/// <list type="table">
		/// <listheader><term>Operand type</term><description>Encoding</description></listheader>
		/// <item>
		/// <term><see cref="SharpAssembler.Architectures.X86.OperandType.RegisterOperand"/></term>
		/// <description>ModR/M.Reg = value</description>
		/// </item>
		/// </list>
		/// </remarks>
		Default,
		/// <summary>
		/// Adds the value to the last opcode byte. This is only valid for the
		/// <see cref="SharpAssembler.Architectures.X86.OperandType.RegisterOperand"/> operand type.
		/// </summary>
		/// <remarks>This has the following influence on the operand's encoding:
		/// <list type="table">
		/// <listheader><term>Operand type</term><description>Encoding</description></listheader>
		/// <item>
		/// <term><see cref="SharpAssembler.Architectures.X86.OperandType.RegisterOperand"/></term>
		/// <description>Opcode + value</description>
		/// </item>
		/// </list>
		/// </remarks>
		OpcodeAdd,
		/// <summary>
		/// Sets the immediate value as the 'extra' immediate value, which is encoded after the normal immediate
		/// value. This is only valid for the
		/// <see cref="SharpAssembler.Architectures.X86.OperandType.Immediate"/> operand type.
		/// </summary>
		ExtraImmediate,
	}
}
