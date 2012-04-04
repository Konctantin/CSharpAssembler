using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAssembler.Architectures.X86;
using System.Diagnostics.Contracts;

namespace SharpAssembler.Languages.Nasm
{
	/// <summary>
	/// Writes X86 instructions.
	/// </summary>
	public class X86NasmInstructionWriter : IInstructionWriter, IInstructionWriter<X86Instruction>
	{
		private readonly NasmLanguage owner;
		/// <summary>
		/// Gets the owner of this writer.
		/// </summary>
		/// <value>A <see cref="NasmLanguage"/> object.</value>
		public NasmLanguage Owner
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<NasmLanguage>() != null);
				#endregion
				return this.owner;
			}
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="X86NasmInstructionWriter"/> class.
		/// </summary>
		/// <param name="owner">The <see cref="NasmLanguage"/> that owns this writer.</param>
		public X86NasmInstructionWriter(NasmLanguage owner)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(owner != null);
			#endregion

			this.owner = owner;
		}
		#endregion

		/// <inheritdoc />
		public void WriteInstruction(X86Instruction instruction)
		{
			int linelength = 0;
			linelength = Owner.WriteIndent(indentationlevel);

			// Write prefixes.
			ILockInstruction lockInstr = instruction as ILockInstruction;
			if (lockInstr != null && lockInstr.Lock)
			{
				Writer.Write("lock ");
				linelength += 5;
			}
			// TODO: REP, REPE/REPZ, REPNE/REPNZ prefixes
			// TODO: Segment override

			// Write the mnemonic.
			Writer.Write(instruction.Mnemonic);
			linelength += instruction.Mnemonic.Length;

			// Special instruction handling
			switch (instruction.Mnemonic)
			{
				case "enter":
					// Write the operands.
					if (instruction.Operands.Count >= 1)
					{
						Writer.Write(" ");
						linelength++;
						linelength += WriteOperand(instruction.Operands[0], false);
						for (int i = 1; i < instruction.Operands.Count; i++)
						{
							Writer.Write(", ");
							linelength += 2;
							linelength += WriteOperand(instruction.Operands[i], false);
						}
					}
					break;
				default:
					// Write the operands.
					if (instruction.Operands.Count >= 1)
					{
						Writer.Write(" ");
						linelength++;
						linelength += WriteOperand(instruction.Operands[0]);
						for (int i = 1; i < instruction.Operands.Count; i++)
						{
							Writer.Write(", ");
							linelength += 2;
							linelength += WriteOperand(instruction.Operands[i]);
						}
					}
					break;
			}

			WriteCommentOf(instruction, linelength);
		}

		/// <inheritdoc />
		void IInstructionWriter.WriteInstruction(IInstruction instruction)
		{
			WriteInstruction((X86Instruction)instruction);
		}

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		public void ObjectInvariant()
		{
			Contract.Invariant(this.owner != null);
		}
		#endregion
	}
}
