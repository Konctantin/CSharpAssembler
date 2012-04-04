using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace SharpAssembler
{
	/// <summary>
	/// An instruction.
	/// </summary>
	[ContractClass(typeof(Contracts.IInstructionContract))]
	public interface IInstruction
	{
		/// <summary>
		/// Gets the opcode of the instruction.
		/// </summary>
		/// <value>The <see cref="IOpcode"/> of the instruction,
		/// which describes the semantics of the instruction.</value>
		IOpcode Opcode
		{ get; }

		/// <summary>
		/// Returns the operands to the instruction.
		/// </summary>
		/// <returns>An ordered list of operands.</returns>
		ReadOnlyCollection<IOperand> GetOperands();
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IInstruction))]
		abstract class IInstructionContract : IInstruction
		{
			public IOpcode Opcode
			{
				get
				{
					Contract.Ensures(Contract.Result<IOpcode>() != null);
					return default(IOpcode);
				}
			}

			public ReadOnlyCollection<IOperand> GetOperands()
			{
				Contract.Ensures(Contract.Result<ReadOnlyCollection<IOperand>>() != null);
				return default(ReadOnlyCollection<IOperand>);
			}
		}
	}
	#endregion
}
