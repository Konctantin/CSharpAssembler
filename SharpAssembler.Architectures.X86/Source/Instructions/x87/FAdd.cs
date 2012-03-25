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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions.x87
{
	/// <summary>
	/// The FADD (Floating-Point Add) instruction.
	/// </summary>
	public class FAdd : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="FAdd"/> class.
		/// </summary>
		/// <param name="destination">The x87 register being modified.</param>
		/// <param name="source">The x87 register with the value to add.</param>
		public FAdd(RegisterOperand destination, RegisterOperand source)
			: this(destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FAdd"/> class.
		/// </summary>
		/// <param name="destination">The x87 register being modified.</param>
		/// <param name="source">The memory location of the value to add.</param>
		public FAdd(RegisterOperand destination, EffectiveAddress source)
			: this(destination, (Operand)source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FAdd"/> class.
		/// </summary>
		/// <param name="destination">The x87 register being modified.</param>
		/// <param name="source">The value to add.</param>
		private FAdd(RegisterOperand destination, Operand source)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(destination != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<InvalidCastException>(
				source is RegisterOperand ||
				source is EffectiveAddress);
			#endregion

			this.destination = destination;
			this.source = source;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "fadd"; }
		}

		private RegisterOperand destination;
		/// <summary>
		/// Gets the destination of the result of the instruction.
		/// </summary>
		/// <value>A <see cref="RegisterOperand"/>.</value>
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

		private Operand source;
		/// <summary>
		/// Gets the source of the value used by the instruction.
		/// </summary>
		/// <value>A <see cref="Operand"/>.</value>
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
		#endregion

		#region Methods
		/// <summary>
		/// Enumerates an ordered list of operands used by this instruction.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Operand"/> objects.</returns>
		public override IEnumerable<Operand> GetOperands()
		{
			// The order is important here!
			yield return destination;
			yield return source;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="SharpAssembler.Architectures.X86.X86Instruction.InstructionVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static InstructionVariant[] variants = new[]{
			// FADD ST(0), ST(i)
			new InstructionVariant(
				new byte[] { 0xD8, 0xC0 },
				new OperandDescriptor(Register.ST0),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.FloatingPoint,
					OperandEncoding.OpcodeAdd)),
			// FADD ST(i), ST(0)
			new InstructionVariant(
				new byte[] { 0xDC, 0xC0 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.FloatingPoint,
					OperandEncoding.OpcodeAdd),
				new OperandDescriptor(Register.ST0)),

			// FADD mem32real
			new InstructionVariant(
				new byte[] { 0xD8 }, 0,
				new OperandDescriptor(Register.ST0),
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit32)),
			// FADD mem64real
			new InstructionVariant(
				new byte[] { 0xDC }, 0,
				new OperandDescriptor(Register.ST0),
				new OperandDescriptor(OperandType.MemoryOperand, DataSize.Bit64)),
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
			Contract.Invariant(destination != null);
			Contract.Invariant(source != null);
			Contract.Invariant(
				source is EffectiveAddress ||
				source is RegisterOperand);
		}
		#endregion
	}
}
