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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler.Core;

namespace SharpAssembler.x86.Operands
{
	/// <summary>
	/// An operand for an instruction.
	/// </summary>
	public abstract class Operand : IOperand, IConstructableOperand
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Operand"/> class.
		/// </summary>
		/// <param name="preferredSize">The preferred size of the operand.</param>
		protected Operand(DataSize preferredSize)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), preferredSize));
			#endregion

			this.preferredSize = preferredSize;
		}
		#endregion

		#region Properties
		private DataSize preferredSize;
		/// <summary>
		/// Gets or sets the preferred size of the operand.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration;
		/// or <see cref="SharpAssembler.Core.DataSize.None"/> to specify no size.</value>
		public virtual DataSize PreferredSize
		{
			get
			{
				// CONTRACT: IOperand
				return preferredSize;
			}
			set
			{
				// CONTRACT: IOperand
				preferredSize = value;
			}
		}

		/// <summary>
		/// Gets the actual size of operand.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration;
		/// or <see cref="SharpAssembler.Core.DataSize.None"/>.</value>
		public virtual DataSize Size
		{
			get
			{
				// CONTRACT: IOperand
				return PreferredSize;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructs the operand's representation.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instruction">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		void IConstructableOperand.Construct(Context context, EncodedInstruction instruction)
		{
			this.Construct(context, instruction);
		}

		/// <summary>
		/// Constructs the operand's representation.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instruction">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		internal abstract void Construct(Context context, EncodedInstruction instruction);

		/// <summary>
		/// Determines whether the specified <see cref="Instruction.OperandDescriptor"/> matches this
		/// <see cref="Operand"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="Instruction.OperandDescriptor"/> to match.</param>
		/// <returns><see langword="true"/> when the specified descriptor matches this operand;
		/// otherwise, <see langword="false"/>.</returns>
		bool IConstructableOperand.IsMatch(Instruction.OperandDescriptor descriptor)
		{
			return this.IsMatch(descriptor);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Instruction.OperandDescriptor"/> matches this
		/// <see cref="Operand"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="Instruction.OperandDescriptor"/> to match.</param>
		/// <returns><see langword="true"/> when the specified descriptor matches this operand;
		/// otherwise, <see langword="false"/>.</returns>
		internal abstract bool IsMatch(Instruction.OperandDescriptor descriptor);

		/// <summary>
		/// Adjusts this <see cref="Operand"/> based on the specified <see cref="Instruction.OperandDescriptor"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="Instruction.OperandDescriptor"/> used to adjust.</param>
		/// <remarks>
		/// Only <see cref="Instruction.OperandDescriptor"/> instances for which <see cref="IsMatch"/> returns
		/// <see langword="true"/> may be used as a parameter to this method.
		/// </remarks>
		void IConstructableOperand.Adjust(Instruction.OperandDescriptor descriptor)
		{
			this.Adjust(descriptor);
		}

		/// <summary>
		/// Adjusts this <see cref="Operand"/> based on the specified <see cref="Instruction.OperandDescriptor"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="Instruction.OperandDescriptor"/> used to adjust.</param>
		/// <remarks>
		/// Only <see cref="Instruction.OperandDescriptor"/> instances for which <see cref="IsMatch"/> returns
		/// <see langword="true"/> may be used as a parameter to this method.
		/// </remarks>
		internal abstract void Adjust(Instruction.OperandDescriptor descriptor);
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(DataSize), preferredSize));
		}
		#endregion
	}
}
