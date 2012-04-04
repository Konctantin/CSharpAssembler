using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAssembler.Instructions;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages.Nasm
{
	/// <summary>
	/// Writes instructions.
	/// </summary>
	[ContractClass(typeof(Contracts.IInstructionWriterContract))]
	public interface IInstructionWriter
	{
		/// <summary>
		/// Writes the specified instruction to the text writer.
		/// </summary>
		/// <param name="instruction">The instruction to write.</param>
		void WriteInstruction(IInstruction instruction);
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IInstructionWriter))]
		abstract class IInstructionWriterContract : IInstructionWriter
		{
			public void WriteInstruction(IInstruction instruction)
			{
				Contract.Requires<ArgumentNullException>(instruction != null);
			}
		}
	}
	#endregion
}
