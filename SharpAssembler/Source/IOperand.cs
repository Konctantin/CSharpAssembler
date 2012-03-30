using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// An operand to an instruction.
	/// </summary>
	[ContractClass(typeof(Contracts.IOperandContract))]
	public interface IOperand
	{
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IOperand))]
		abstract class IOperandContract : IOperand
		{
			
		}
	}
	#endregion
}
