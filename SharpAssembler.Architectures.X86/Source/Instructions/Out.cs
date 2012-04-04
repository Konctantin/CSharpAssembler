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
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86.Instructions
{
	/// <summary>
	/// The OUT (Output to Port) instruction.
	/// </summary>
	public class Out : X86Instruction
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Out"/> class.
		/// </summary>
		/// <param name="port">The port.</param>
		public Out(RegisterOperand port)
			: this((Operand)port)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(port != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Out"/> class.
		/// </summary>
		/// <param name="port">The port.</param>
		public Out(Immediate port)
			: this((Operand)port)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(port != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Out"/> class.
		/// </summary>
		/// <param name="port">The port.</param>
		private Out(Operand port)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(port != null);
			Contract.Requires<InvalidCastException>(
					port is EffectiveAddress ||
					port is RegisterOperand);
			#endregion

			this.port = port;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the mnemonic of the instruction.
		/// </summary>
		/// <value>The mnemonic of the instruction.</value>
		public override string Mnemonic
		{
			get { return "out"; }
		}

		private Operand port;
		/// <summary>
		/// Gets the port.
		/// </summary>
		/// <value>An <see cref="Immediate"/> or <see cref="RegisterOperand"/>.</value>
		public Operand Port
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Operand>() != null);
				Contract.Ensures(
					Contract.Result<Operand>() is Immediate ||
					Contract.Result<Operand>() is RegisterOperand);
				#endregion
				return port;
			}
#if OPERAND_SET
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidCastException>(
					value is Immediate ||
					value is RegisterOperand);
				#endregion
				port = value;
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
			yield return this.port;
		}
		#endregion

		#region Instruction Variants
		/// <summary>
		/// An array of <see cref="X86OpcodeVariant"/> objects
		/// describing the possible variants of this instruction.
		/// </summary>
		private static X86OpcodeVariant[] variants = new[]{
			// OUT imm8, AL
			new X86OpcodeVariant(
				new byte[] { 0xE6 },
				new OperandDescriptor(Register.AL),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// OUT imm8, AX
			new X86OpcodeVariant(
				new byte[] { 0xE7 },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
			// OUT imm8, EAX
			new X86OpcodeVariant(
				new byte[] { 0xE7 },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),

			// OUT DX, AL
			new X86OpcodeVariant(
				new byte[] { 0xEE },
				new OperandDescriptor(Register.AL),
				new OperandDescriptor(Register.DX)),
			// OUT DX, AX
			new X86OpcodeVariant(
				new byte[] { 0xEF },
				new OperandDescriptor(Register.AX),
				new OperandDescriptor(Register.DX)),
			// OUT DX, EAX
			new X86OpcodeVariant(
				new byte[] { 0xEF },
				new OperandDescriptor(Register.EAX),
				new OperandDescriptor(Register.DX)),
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
			Contract.Invariant(this.port != null);
			Contract.Invariant(
					this.port is Immediate ||
					this.port is RegisterOperand);
		}
		#endregion
	}
}



