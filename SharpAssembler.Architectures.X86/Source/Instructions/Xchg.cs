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
using System.Diagnostics.Contracts;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The XCHG (Exchange) instruction.
	/// </summary>
	public class Xchg : X86Instruction, ILockInstruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Xchg"/> class.
		/// </summary>
		/// <param name="first">The first operand.</param>
		/// <param name="second">The second operand.</param>
		public Xchg(RegisterOperand first, RegisterOperand second)
			: this((Operand)first, second)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(first != null);
			Contract.Requires<ArgumentNullException>(second != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Xchg"/> class.
		/// </summary>
		/// <param name="first">The first operand.</param>
		/// <param name="second">The second operand.</param>
		public Xchg(EffectiveAddress first, RegisterOperand second)
			: this((Operand)first, second)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(first != null);
			Contract.Requires<ArgumentNullException>(second != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Xchg"/> class.
		/// </summary>
		/// <param name="first">The first operand.</param>
		/// <param name="second">The second operand.</param>
		private Xchg(Operand first, RegisterOperand second)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(first != null);
			Contract.Requires<InvalidCastException>(
					first is EffectiveAddress ||
					first is RegisterOperand);
			Contract.Requires<ArgumentNullException>(second != null);
			#endregion

			this.first = first;
			this.second = second;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "xchg"; }
		}

		private Operand first;
		/// <summary>
		/// Gets the first operand of the instruction.
		/// </summary>
		/// <value>An <see cref="Operand"/>.</value>
		public Operand First
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is EffectiveAddress ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return first;
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
				first = value;
			}
#endif
		}

		private RegisterOperand second;
		/// <summary>
		/// Gets the second operand of the instruction.
		/// </summary>
		/// <value>A <see cref="RegisterOperand"/>.</value>
		public RegisterOperand Second
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				#endregion
				return second;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				second = value;
			}
#endif
		}

		private bool lockInstruction = false;
		/// <summary>
		/// Gets or sets whether the lock prefix is used.
		/// </summary>
		/// <value><see langword="true"/> to enable the lock prefix; otherwise, <see langword="false"/>.
		/// The default is <see langword="false"/>.</value>
		/// <remarks>
		/// When this property is set to <see langword="true"/>, the lock signal is asserted before accessing the
		/// specified memory location. When the lock signal has already been asserted, the instruction must wait for it
		/// to be released. Instructions without the lock prefix do not check the lock signal, and will be executed
		/// even when the lock signal is asserted by some other instruction.
		/// </remarks>
		public bool Lock
		{
			get { return lockInstruction; }
			set { lockInstruction = value; }
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
			yield return this.first;
			yield return this.second;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// XCHG AX, reg16
			new X86OpcodeVariant(
				new byte[] { 0x90 },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit, OperandEncoding.OpcodeAdd)),
			// XCHG reg16, AX
			new X86OpcodeVariant(
				new byte[] { 0x90 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit, OperandEncoding.OpcodeAdd),
				new OperandDescriptor(Register.AX)),
			
			// XCHG EAX, reg32
			new X86OpcodeVariant(
				new byte[] { 0x90 },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit, OperandEncoding.OpcodeAdd)),
			// XCHG reg32, EAX
			new X86OpcodeVariant(
				new byte[] { 0x90 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit, OperandEncoding.OpcodeAdd),
				new OperandDescriptor(Register.EAX)),
			
			// XCHG RAX, reg64
			new X86OpcodeVariant(
				new byte[] { 0x90 },
				new OperandDescriptor(Register.RAX),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit, OperandEncoding.OpcodeAdd)),
			// XCHG reg64, RAX
			new X86OpcodeVariant(
				new byte[] { 0x90 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit, OperandEncoding.OpcodeAdd),
				new OperandDescriptor(Register.RAX)),


			// XCHG reg/mem8, reg8
			new X86OpcodeVariant(
				new byte[] { 0x86 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
			// XCHG reg8, reg/mem8
			new X86OpcodeVariant(
				new byte[] { 0x86 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
			
			// XCHG reg/mem16, reg16
			new X86OpcodeVariant(
				new byte[] { 0x87 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
			// XCHG reg16, reg/mem16
			new X86OpcodeVariant(
				new byte[] { 0x87 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
			
			// XCHG reg/mem32, reg32
			new X86OpcodeVariant(
				new byte[] { 0x87 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
			// XCHG reg32, reg/mem32
			new X86OpcodeVariant(
				new byte[] { 0x87 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),

			// XCHG reg/mem64, reg64
			new X86OpcodeVariant(
				new byte[] { 0x87 },
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),
			// XCHG reg64, reg/mem64
			new X86OpcodeVariant(
				new byte[] { 0x87 },
				new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
				new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
		};

		/// <summary>
		/// Returns an array containing the <see cref="X86OpcodeVariant"/>
		/// objects representing all the possible variants of this instruction.
		/// </summary>
		/// <returns>An array of <see cref="X86OpcodeVariant"/>
		/// objects.</returns>
		internal override X86OpcodeVariant[] GetVariantList()
		{ return variants; }
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.first != null);
			Contract.Invariant(
					this.first is EffectiveAddress ||
					this.first is RegisterOperand);
			Contract.Invariant(this.second != null);
		}
		#endregion
	}
}
