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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using SharpAssembler;

namespace SharpAssembler.Architectures.X86.Operands
{
	/// <summary>
	/// An operand for an instruction.
	/// </summary>
	[ContractClass(typeof(Contracts.IConstructableOperandContract))]
	public interface IConstructableOperand : IOperand
	{
		/// <summary>
		/// Constructs the operand's representation.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instruction">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		void Construct(Context context, EncodedInstruction instruction);

		/// <summary>
		/// Determines whether the specified <see cref="OperandDescriptor"/> matches this <see cref="Operand"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="OperandDescriptor"/> to match.</param>
		/// <returns><see langword="true"/> when the specified descriptor matches this operand;
		/// otherwise, <see langword="false"/>.</returns>
		[Pure]
		bool IsMatch(OperandDescriptor descriptor);

		/// <summary>
		/// Adjusts this <see cref="Operand"/> based on the specified <see cref="OperandDescriptor"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="OperandDescriptor"/> used to adjust.</param>
		/// <remarks>
		/// Only <see cref="OperandDescriptor"/> instances for which <see cref="IsMatch"/> returns
		/// <see langword="true"/> may be used as a parameter to this method.
		/// </remarks>
		void Adjust(OperandDescriptor descriptor);
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IConstructableOperand"/> type.
		/// </summary>
		[ContractClassFor(typeof(IConstructableOperand))]
		abstract class IConstructableOperandContract : IConstructableOperand
		{
			public void Construct(Context context, EncodedInstruction instruction)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(instruction != null);
			}

			public bool IsMatch(OperandDescriptor descriptor)
			{
				Contract.Requires<ArgumentNullException>(descriptor != null);

				return default(bool);
			}

			public void Adjust(OperandDescriptor descriptor)
			{
				Contract.Requires<ArgumentNullException>(descriptor != null);
				Contract.Requires<ArgumentException>(IsMatch(descriptor));
			}

			public abstract DataSize PreferredSize
			{ get; set; }

			public abstract DataSize Size
			{ get; }
		}
	}
	#endregion
}
