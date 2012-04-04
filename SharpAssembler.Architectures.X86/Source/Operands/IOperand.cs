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
	[ContractClass(typeof(Contracts.IOperandContract))]
	public interface IOperand
	{
		/// <summary>
		/// Gets or sets the preferred size of the operand.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration;
		/// or <see cref="SharpAssembler.DataSize.None"/> to specify no size.</value>
		DataSize PreferredSize
		{ get; set; }

		/// <summary>
		/// Gets the actual size of operand.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration;
		/// or <see cref="SharpAssembler.DataSize.None"/>.</value>
		DataSize Size
		{ get; }
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IOperand"/> type.
		/// </summary>
		[ContractClassFor(typeof(IOperand))]
		abstract class IOperandContract : IOperand
		{
			public DataSize PreferredSize
			{
				get
				{
					Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
					
					return default(DataSize);
				}
				set
				{
					Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				}
			}

			public DataSize Size
			{
				get
				{
					Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
					
					return default(DataSize);
				}
			}
		}
	}
	#endregion
}
