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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The J (Jump on Condition) instruction.
	/// </summary>
	public class J : X86Instruction, IConditionalInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="J"/> class.
		/// </summary>
		/// <param name="offset">The jump offset.</param>
		/// <param name="condition">The condition on which this instruction executes.</param>
		public J(RelativeOffset offset, InstructionCondition condition)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(offset != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Requires<ArgumentException>(condition != InstructionCondition.None);
			#endregion

			this.offset = offset;

			this.condition = condition;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get
			{
				switch (this.condition)
				{
					case InstructionCondition.Overflow:
						return "jo";
					case InstructionCondition.NotOverflow:
						return "jno";
					case InstructionCondition.Below:
						return "jb";
					case InstructionCondition.Carry:
						return "jc";
					case InstructionCondition.NotAboveOrEqual:
						return "jnae";
					case InstructionCondition.NotBelow:
						return "jnb";
					case InstructionCondition.NotCarry:
						return "jnc";
					case InstructionCondition.AboveOrEqual:
						return "jae";
					case InstructionCondition.Zero:
						return "jz";
					case InstructionCondition.Equal:
						return "je";
					case InstructionCondition.NotZero:
						return "jnz";
					case InstructionCondition.NotEqual:
						return "jne";
					case InstructionCondition.BelowOrEqual:
						return "jbe";
					case InstructionCondition.NotAbove:
						return "jna";
					case InstructionCondition.NotBelowOrEqual:
						return "jnbe";
					case InstructionCondition.Above:
						return "ja";
					case InstructionCondition.Sign:
						return "js";
					case InstructionCondition.NotSign:
						return "jns";
					case InstructionCondition.Parity:
						return "jp";
					case InstructionCondition.ParityEven:
						return "jpe";
					case InstructionCondition.NotParity:
						return "jnp";
					case InstructionCondition.ParityOdd:
						return "jpo";
					case InstructionCondition.Less:
						return "jl";
					case InstructionCondition.NotGreaterOrEqual:
						return "jnge";
					case InstructionCondition.NotLess:
						return "jnl";
					case InstructionCondition.GreaterOrEqual:
						return "jge";
					case InstructionCondition.LessOrEqual:
						return "jle";
					case InstructionCondition.NotGreater:
						return "jng";
					case InstructionCondition.NotLessOrEqual:
						return "jnle";
					case InstructionCondition.Greater:
						return "jg";
					default:
						return "j";
				}
			}
		}

		private InstructionCondition condition;
		/// <summary>
		/// Gets the condition on which this instruction executes.
		/// </summary>
		/// <value>A member of the <see cref="InstructionCondition"/> enumeration.</value>
		public InstructionCondition Condition
		{
			get
			{
				// CONTRACT: IConditionalInstruction
				return condition;
			}
#if OPERAND_SET
			set
			{
				// CONTRACT: IConditionalInstruction
				condition = value;
			}
#endif
		}

		private RelativeOffset offset;
		/// <summary>
		/// Gets the jump offset.
		/// </summary>
		/// <value>An <see cref="RelativeOffset"/>.</value>
		public RelativeOffset Offset
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<RelativeOffset>() != null);
				#endregion
				return offset;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				offset = value;
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
			yield return this.offset;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of each possible
		/// condition of this instruction.
		/// </summary>
		private static X86OpcodeVariant[][] variants = new X86OpcodeVariant[][]{
			new [] {
				// JO rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x0) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JO rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x0) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JO rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x0) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JNO rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x1) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JNO rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x1) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JNO rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x1) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JB rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x2) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JB rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x2) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JB rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x2) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JAE rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x3) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JAE rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x3) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JAE rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x3) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JE rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x4) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JE rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x4) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JE rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x4) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JNE rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x5) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JNE rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x5) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JNE rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x5) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JBE rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x6) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JBE rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x6) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JBE rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x6) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JA rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x7) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JA rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x7) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JA rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x7) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JS rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x8) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JS rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x8) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JS rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x8) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JNS rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0x9) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JNS rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x9) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JNS rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0x9) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JPE rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0xA) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JPE rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xA) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JPE rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xA) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JPO rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0xB) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JPO rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xB) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JPO rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xB) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JL rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0xC) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JL rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xC) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JL rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xC) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JGE rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0xD) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JGE rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xD) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JGE rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xD) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JLE rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0xE) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JLE rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xE) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JLE rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xE) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
			new [] {
				// JG rel8off
				new X86OpcodeVariant(
					new byte[] { (byte)(0x70 | 0xF) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit8)),
				// JG rel16off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xF) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit16)),
				// JG rel32off
				new X86OpcodeVariant(
					new byte[] { 0x0F, (byte)(0x80 | 0xF) },
					new OperandDescriptor(OperandType.RelativeOffset, DataSize.Bit32)),
			},
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86OpcodeVariant"/>
		/// objects.</returns>
		internal override X86OpcodeVariant[] GetVariantList()
		{
			return variants[((int)condition) & 0xF];
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.offset != null);
			Contract.Invariant(Enum.IsDefined(typeof(InstructionCondition), this.condition));
			Contract.Invariant(this.condition != InstructionCondition.None);
		}
		#endregion
	}
}
