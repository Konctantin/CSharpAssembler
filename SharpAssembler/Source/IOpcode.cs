using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler
{
	/// <summary>
	/// An opcode, which describes the semantics of an instruction.
	/// </summary>
	[ContractClass(typeof(Contracts.IOpcodeContract))]
	public interface IOpcode
	{
		/// <summary>
		/// Gets the mnemonic of the opcode.
		/// </summary>
		/// <value>The mnemonic of the opcode.</value>
		string Mnemonic
		{ get; }
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IOpcode))]
		abstract class IOpcodeContract : IOpcode
		{
			public string Mnemonic
			{
				get
				{
					Contract.Ensures(Contract.Result<string>() != null);
					return default(string);
				}
			}
		}
	}
	#endregion
}
