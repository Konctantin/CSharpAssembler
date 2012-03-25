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
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The ENTER (Create Procesure Stack Frame) instruction.
	/// </summary>
	public class Enter : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Enter"/> class.
		/// </summary>
		/// <param name="stackframeSize">The stack frame size.</param>
		/// <param name="nestingLevel">The nesting level.</param>
		public Enter(Immediate stackframeSize, Immediate nestingLevel)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(stackframeSize != null);
			Contract.Requires<ArgumentNullException>(nestingLevel != null);
			#endregion

			this.stackframeSize = stackframeSize;
			this.nestingLevel = nestingLevel;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "enter"; }
		}

		private Immediate stackframeSize;
		/// <summary>
		/// Gets the size of the stack frame.
		/// </summary>
		/// <value>An <see cref="Immediate"/> value.</value>
		public Immediate StackFrameSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Immediate>() != null);
				#endregion
				return stackframeSize;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				stackframeSize = value;
			}
#endif
		}

		private Immediate nestingLevel;
		/// <summary>
		/// Gets the nesting level.
		/// </summary>
		/// <value>An <see cref="Immediate"/> value.</value>
		public Immediate NestingLevel
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Immediate>() != null);
				#endregion
				return nestingLevel;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				nestingLevel = value;
			}
#endif
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enumerates an ordered list of operands used by this instruction.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Operand"/> objects.</returns>
		public override IEnumerable<Operand> GetOperands()
		{
			// The order is important here!
			yield return this.stackframeSize;
			yield return this.nestingLevel;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// ENTER imm16, imm8
			new InstructionVariant(
				new byte[] { 0xC8 },
				new OperandDescriptor(OperandType.Immediate, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.Immediate, RegisterType.GeneralPurpose8Bit, OperandEncoding.ExtraImmediate)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects.</returns>
		internal override InstructionVariant[] GetVariantList()
		{ return variants; }
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.stackframeSize != null);
			Contract.Invariant(this.nestingLevel != null);
		}
		#endregion
	}
}
