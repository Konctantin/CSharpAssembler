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
	/// The FADD (Floating-Point Add) instruction opcode.
	/// </summary>
	/// <remarks>
	/// Instructions with this opcode expect two operands that have the following semantics:
	/// <list type="table">
	/// <listheader><term>Index</term><description>Semantics</description></listheader>
	/// <item><term>0</term><description>Destination</description></item>
	/// <item><term>1</term><description>Source</description></item>
	/// </list>
	/// </remarks>
	public class FAddOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="FAddOpcode"/> class.
		/// </summary>
		public FAddOpcode()
			: base("fadd", GetOpcodeVariants())
		{ /* Nothing to do. */ }
		#endregion
		
		/// <summary>
		/// Returns the opcode variants of this opcode.
		/// </summary>
		/// <returns>An enumerable collection of <see cref="X86OpcodeVariant"/> objects.</returns>
		private static IEnumerable<X86OpcodeVariant> GetOpcodeVariants()
		{
			return new X86OpcodeVariant[]{
				// FADD ST(0), ST(i)
				new X86OpcodeVariant(
					new byte[] { 0xD8, 0xC0 },
					new OperandDescriptor(Register.ST0),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.FloatingPoint,
						OperandEncoding.OpcodeAdd)),
				// FADD ST(i), ST(0)
				new X86OpcodeVariant(
					new byte[] { 0xDC, 0xC0 },
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.FloatingPoint,
						OperandEncoding.OpcodeAdd),
					new OperandDescriptor(Register.ST0)),

				// FADD mem32real
				new X86OpcodeVariant(
					new byte[] { 0xD8 }, 0,
					new OperandDescriptor(Register.ST0),
					new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit32)),
				// FADD mem64real
				new X86OpcodeVariant(
					new byte[] { 0xDC }, 0,
					new OperandDescriptor(Register.ST0),
					new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit64)),
			};
		}
	}
}
