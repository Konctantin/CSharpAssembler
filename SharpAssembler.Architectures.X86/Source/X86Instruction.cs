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
using System.Globalization;
using System.Linq;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;
using System.Collections.ObjectModel;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// An x86-64 instruction.
	/// </summary>
	public partial class X86Instruction : Constructable, IInstruction
	{
		private readonly X86Opcode opcode;
		/// <summary>
		/// Gets the opcode of the instruction.
		/// </summary>
		/// <value>The <see cref="IOpcode"/> of the instruction,
		/// which describes the semantics of the instruction.</value>
		public X86Opcode Opcode
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<X86Opcode>() != null);
				#endregion
				return this.opcode;
			}
		}

		/// <inheritdoc />
		IOpcode IInstruction.Opcode
		{
			get { return this.opcode; }
		}

		private DataSize explicitOperandSize = DataSize.None;
		/// <summary>
		/// Gets or sets an explicit operand size for this instruction.
		/// </summary>
		/// <value>The explicit operand size for this instruction; or <see cref="DataSize.None"/> to determine it from
		/// the operands. The default is <see cref="DataSize.None"/>.</value>
		/// <remarks>
		/// This property is intended to be used with instructions which do not have any operand from which the operand
		/// size can be determined.
		/// </remarks>
		public DataSize ExplicitOperandSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return explicitOperandSize;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				explicitOperandSize = value;
			}
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode of this instruction.</param>
		/// <param name="operands">The operands to the instruction.</param>
		public X86Instruction(X86Opcode opcode, params Operand[] operands)
			: this(opcode, (IList<Operand>)operands)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(opcode != null);
			Contract.Requires<ArgumentNullException>(operands != null);
			//Contract.Requires<ArgumentException>(operands.Length == opcode.OperandCount);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode of this instruction.</param>
		/// <param name="operands">The operands to the instruction.</param>
		public X86Instruction(X86Opcode opcode, IList<Operand> operands)
			: base()
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(opcode != null);
			Contract.Requires<ArgumentNullException>(operands != null);
			//Contract.Requires<ArgumentException>(operands.Count == opcode.OperandCount);
			#endregion
			this.opcode = opcode;
			this.operands = operands.ToArray();
		}
		#endregion

		#region Operands
		private Operand[] operands;
		/// <inheritdoc />
		ReadOnlyCollection<IOperand> IInstruction.GetOperands()
		{
			return new ReadOnlyCollection<IOperand>((IOperand[])operands);
		}

		/// <summary>
		/// Returns the operands to the instruction.
		/// </summary>
		/// <returns>An ordered list of operands.</returns>
		public ReadOnlyCollection<Operand> GetOperands()
		{
			#region Contract
			Contract.Ensures(Contract.Result<ReadOnlyCollection<Operand>>() != null);
			#endregion
			return new ReadOnlyCollection<Operand>(operands);
		}
		#endregion

		#region Methods
		/// <inheritdoc />
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}({1})",
				this.opcode.Mnemonic,
				String.Join(", ", GetOperands()));
		}
		#endregion

		#region Construction
		/// <inheritdoc />
		public override IEnumerable<IEmittable> Construct(Context context)
		{
			// CONTRACT: Constructable

			// Get the most efficient instruction variant.
			X86OpcodeVariant variant = GetVariant(context);
			if (variant == null)
				throw new AssemblerException("No matching instruction variant was found.");

			// Construct the chosen variant.
			EncodedInstruction instr = variant.Construct(context, GetOperands(), GetLockPrefix());

			return new IEmittable[] { instr };
		}

		/// <summary>
		/// Gets whether a lock prefix is used for this instruction.
		/// </summary>
		/// <returns><see langword="true"/> to use a lock prefix;
		/// otherwise, <see langword="false"/>.</returns>
		protected virtual bool GetLockPrefix()
		{
			return false;
		}

		/// <summary>
		/// Finds the most efficient variant for the instruction with the current operands.
		/// </summary>
		/// <param name="context">The <see cref="Context"/>.</param>
		/// <returns>The most efficient <see cref="X86OpcodeVariant"/>;
		/// or <see langword="null"/> when none was found.</returns>
		private X86OpcodeVariant GetVariant(Context context)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			#endregion

			var variants =
				from v in this.opcode.Variants
				where v.Match(explicitOperandSize, context, GetOperands().ToList())
				select v;
			var variant = variants.FirstOrDefault();
			
			return variant;
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.opcode != null);
			Contract.Invariant(this.operands != null);
			//Contract.Invariant(this.operands.Length == this.opcode.OperandCount);
		}
		#endregion
	}
}
