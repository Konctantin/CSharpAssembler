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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The CMOV (Conditional Move) instruction.
	/// </summary>
	public class Cmov : X86Instruction, IConditionalInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Cmov"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source register.</param>
		/// <param name="condition">The condition on which this instruction executes.</param>
		public Cmov(RegisterOperand destination, RegisterOperand source, InstructionCondition condition)
			: this(destination, (Operand)source, condition)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Requires<ArgumentException>(condition != InstructionCondition.None);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cmov"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="source">The source memory operand.</param>
		/// <param name="condition">The condition on which this instruction executes.</param>
		public Cmov(RegisterOperand destination, EffectiveAddress source, InstructionCondition condition)
			: this(destination, (Operand)source, condition)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Requires<ArgumentException>(condition != InstructionCondition.None);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cmov"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="condition">The condition on which this instruction executes.</param>
		private Cmov(RegisterOperand destination, Operand source, InstructionCondition condition)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidCastException>(
					source is EffectiveAddress ||
					source is RegisterOperand);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Requires<ArgumentException>(condition != InstructionCondition.None);
			#endregion

			this.destination = destination;
			this.source = source;

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
						return "cmovo";
					case InstructionCondition.NotOverflow:
						return "cmovno";
					case InstructionCondition.Carry:
						return "cmovc";
					case InstructionCondition.Below:
						return "cmovb";
					case InstructionCondition.NotAboveOrEqual:
						return "cmovnae";
					case InstructionCondition.NotBelow:
						return "cmovnb";
					case InstructionCondition.NotCarry:
						return "cmovnc";
					case InstructionCondition.AboveOrEqual:
						return "cmovae";
					case InstructionCondition.Zero:
						return "cmovz";
					case InstructionCondition.Equal:
						return "cmove";
					case InstructionCondition.NotZero:
						return "cmovnz";
					case InstructionCondition.NotEqual:
						return "cmovne";
					case InstructionCondition.BelowOrEqual:
						return "cmovbe";
					case InstructionCondition.NotAbove:
						return "cmovna";
					case InstructionCondition.NotBelowOrEqual:
						return "cmovnbe";
					case InstructionCondition.Above:
						return "cmova";
					case InstructionCondition.Sign:
						return "cmovs";
					case InstructionCondition.NotSign:
						return "cmovns";
					case InstructionCondition.Parity:
						return "cmovp";
					case InstructionCondition.ParityEven:
						return "cmovpe";
					case InstructionCondition.NotParity:
						return "cmovnp";
					case InstructionCondition.ParityOdd:
						return "cmovpo";
					case InstructionCondition.Less:
						return "cmovl";
					case InstructionCondition.NotGreaterOrEqual:
						return "cmovnge";
					case InstructionCondition.NotLess:
						return "cmovnl";
					case InstructionCondition.GreaterOrEqual:
						return "cmovge";
					case InstructionCondition.LessOrEqual:
						return "cmovle";
					case InstructionCondition.NotGreater:
						return "cmovng";
					case InstructionCondition.NotLessOrEqual:
						return "cmovnle";
					case InstructionCondition.Greater:
						return "cmovg";
					default:
						return "cmov";
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

		private Operand source;
		/// <summary>
		/// Gets the source operand of the instruction.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Source
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return source;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidCastException>(
					value is EffectiveAddress ||
					value is RegisterOperand);
				#endregion
				source = value;
			}
#endif
		}

		private RegisterOperand destination;
		/// <summary>
		/// Gets the destination operand of the instruction.
		/// </summary>
		/// <value>An <see cref="RegisterOperand"/>.</value>
		public RegisterOperand Destination
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<RegisterOperand>() != null);
				#endregion
				return destination;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				destination = value;
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
			yield return this.destination;
			yield return this.source;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of each possible
		/// condition of this instruction.
		/// </summary>
		private static InstructionVariant[][] variants = new InstructionVariant[][] {
		   new[] {
				// CMOVO reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x0) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVO reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x0) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVO reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x0) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVNO reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x1) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVNO reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x1) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVNO reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x1) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVB reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x2) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVB reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x2) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVB reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x2) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVAE reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x3) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVAE reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x3) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVAE reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x3) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVE reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x4) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVE reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x4) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVE reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x4) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVNE reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x5) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVNE reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x5) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVNE reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x5) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVBE reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x6) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVBE reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x6) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVBE reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x6) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVA reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x7) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVA reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x7) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVA reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x7) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVS reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x8) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVS reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x8) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVS reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x8) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVNS reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x9) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVNS reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x9) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVNS reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0x9) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVPE reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xA) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVPE reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xA) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVPE reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xA) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVPO reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xB) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVPO reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xB) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVPO reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xB) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVL reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xC) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVL reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xC) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVL reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xC) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVGE reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xD) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVGE reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xD) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVGE reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xD) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVLE reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xE) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVLE reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xE) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVLE reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xE) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
			new[] {
				// CMOVG reg/mem16, reg16
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xF) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
				// CMOVG reg/mem32, reg32
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xF) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
				// CMOVG reg/mem64, reg64
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x40 | 0xF) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
					new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			},
		};

		/// <summary>
		/// Returns an array containing the <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/>
		/// objects.</returns>
		internal override InstructionVariant[] GetVariantList()
		{
			return variants[((int)condition) & 0xFF];
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(InstructionCondition), this.condition));
			Contract.Invariant(this.condition != InstructionCondition.None);

			Contract.Invariant(this.destination != null);
			Contract.Invariant(this.source != null);
			Contract.Invariant(
					this.source is EffectiveAddress ||
					this.source is RegisterOperand);
		}
		#endregion
	}
}
