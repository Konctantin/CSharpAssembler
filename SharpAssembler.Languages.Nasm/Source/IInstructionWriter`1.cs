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
	[ContractClass(typeof(Contracts.IInstructionWriterContract<>))]
	public interface IInstructionWriter<T>
		where T : class, IInstruction
	{
		/// <summary>
		/// Writes the specified instruction to the text writer.
		/// </summary>
		/// <param name="instruction">The instruction to write.</param>
		void WriteInstruction(T instruction);
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(IInstructionWriter))]
		abstract class IInstructionWriterContract<T> : IInstructionWriter<T>
		{
			public void WriteInstruction(T instruction)
			{
				Contract.Requires<ArgumentNullException>(instruction != null);
			}
		}
	}
	#endregion
}
