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

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// An interface for instructions which are executed depending on a condition.
	/// </summary>
	[ContractClass(typeof(Contracts.IConditionalInstructionContract))]
	public interface IConditionalInstruction
	{
		/// <summary>
		/// Gets or sets the condition on which this instruction executes.
		/// </summary>
		/// <value>A member of the <see cref="InstructionCondition"/> enumeration.</value>
		InstructionCondition Condition
		{
			get;
#if OPERAND_SET
			set;
#endif
		}
	}

	#region Contract
	namespace Contracts
	{
		/// <summary>
		/// Contract class for the <see cref="IConditionalInstruction"/> interface.
		/// </summary>
		[ContractClassFor(typeof(IConditionalInstruction))]
		abstract class IConditionalInstructionContract : IConditionalInstruction
		{
			public InstructionCondition Condition
			{
				get
				{
					Contract.Ensures(Enum.IsDefined(typeof(InstructionCondition),
						Contract.Result<InstructionCondition>()));
					Contract.Ensures(Contract.Result<InstructionCondition>() != InstructionCondition.None);

					return default(InstructionCondition);
				}
				set
				{
					Contract.Requires<InvalidEnumArgumentException>(
						Enum.IsDefined(typeof(InstructionCondition), value));
					Contract.Requires<ArgumentException>(value != InstructionCondition.None);
				}
			}
		}
	}
	#endregion
}
