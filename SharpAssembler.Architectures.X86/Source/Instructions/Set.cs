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
	/// The SET (Set Byte on Condition) instruction.
	/// </summary>
	public class Set : X86Instruction, IConditionalInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Set"/> class.
		/// </summary>
		/// <param name="destination">The destination register.</param>
		/// <param name="condition">The condition on which this instruction executes.</param>
		public Set(RegisterOperand destination, InstructionCondition condition)
			: this((Operand)destination, condition)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Requires<ArgumentException>(condition != InstructionCondition.None);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Set"/> class.
		/// </summary>
		/// <param name="destination">The destination memory operand.</param>
		/// <param name="condition">The condition on which this instruction executes.</param>
		public Set(EffectiveAddress destination, InstructionCondition condition)
			: this((Operand)destination, condition)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Requires<ArgumentException>(condition != InstructionCondition.None);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Set"/> class.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="condition">The condition on which this instruction executes.</param>
		private Set(Operand destination, InstructionCondition condition)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<InvalidCastException>(
					destination is EffectiveAddress ||
					destination is RegisterOperand);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(InstructionCondition), condition));
			Contract.Requires<ArgumentException>(condition != InstructionCondition.None);
			#endregion

			this.destination = destination;
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
						return "seto";
					case InstructionCondition.NotOverflow:
						return "setno";
					case InstructionCondition.Carry:
						return "setc";
					case InstructionCondition.Below:
						return "setb";
					case InstructionCondition.NotAboveOrEqual:
						return "setnae";
					case InstructionCondition.NotBelow:
						return "setnb";
					case InstructionCondition.NotCarry:
						return "setnc";
					case InstructionCondition.AboveOrEqual:
						return "setae";
					case InstructionCondition.Zero:
						return "setz";
					case InstructionCondition.Equal:
						return "sete";
					case InstructionCondition.NotZero:
						return "setnz";
					case InstructionCondition.NotEqual:
						return "setne";
					case InstructionCondition.BelowOrEqual:
						return "setbe";
					case InstructionCondition.NotAbove:
						return "setna";
					case InstructionCondition.NotBelowOrEqual:
						return "setnbe";
					case InstructionCondition.Above:
						return "seta";
					case InstructionCondition.Sign:
						return "sets";
					case InstructionCondition.NotSign:
						return "setns";
					case InstructionCondition.Parity:
						return "setp";
					case InstructionCondition.ParityEven:
						return "setpe";
					case InstructionCondition.NotParity:
						return "setnp";
					case InstructionCondition.ParityOdd:
						return "setpo";
					case InstructionCondition.Less:
						return "setl";
					case InstructionCondition.NotGreaterOrEqual:
						return "setnge";
					case InstructionCondition.NotLess:
						return "setnl";
					case InstructionCondition.GreaterOrEqual:
						return "setge";
					case InstructionCondition.LessOrEqual:
						return "setle";
					case InstructionCondition.NotGreater:
						return "setng";
					case InstructionCondition.NotLessOrEqual:
						return "setnle";
					case InstructionCondition.Greater:
						return "setg";
					default:
						return "set";
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

		private Operand destination;
		/// <summary>
		/// Gets the destination operand of the instruction.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand Destination
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return destination;
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
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of each possible
		/// condition of this instruction.
		/// </summary>
		private static InstructionVariant[][] variants = new InstructionVariant[][]{
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x0) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x1) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x2) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x3) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x4) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x5) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x6) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x7) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x8) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x9) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0x0) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0xA) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0xB) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0xC) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0xD) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0xE) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			},
			new []{
				// SETxx reg/mem8
				new InstructionVariant(
					new byte[] { 0x0F, (byte)(0x90 | 0xF) },
					new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
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
			Contract.Invariant(this.destination != null);
			Contract.Invariant(
					this.destination is EffectiveAddress ||
					this.destination is RegisterOperand);
			Contract.Invariant(Enum.IsDefined(typeof(InstructionCondition), this.condition));
			Contract.Invariant(this.condition != InstructionCondition.None);
		}
		#endregion
	}
}
