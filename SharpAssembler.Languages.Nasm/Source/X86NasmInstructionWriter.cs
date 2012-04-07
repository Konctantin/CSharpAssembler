using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAssembler.Architectures.X86;
using System.Diagnostics.Contracts;
using SharpAssembler.Architectures.X86.Operands;
using System.ComponentModel;
using System.Linq.Expressions;

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
			linelength = Owner.WriteIndent();

			// Write prefixes.
			ILockInstruction lockInstr = instruction as ILockInstruction;
			if (lockInstr != null && lockInstr.Lock)
			{
				Owner.Writer.Write("lock ");
				linelength += 5;
			}
			// TODO: REP, REPE/REPZ, REPNE/REPNZ prefixes
			// TODO: Segment override

			// Write the mnemonic.
			string mnemonic = instruction.Opcode.Mnemonic;
			Owner.Writer.Write(mnemonic);
			linelength += mnemonic.Length;

			// Special instruction handling
			bool writeSize;
			switch (mnemonic)
			{
				case "enter": writeSize = false; break;
				default: writeSize = true; break;
			}

			// Write the operands.
			var operands = instruction.GetOperands();
			if (operands.Count >= 1)
			{
				Owner.Writer.Write(" ");
				linelength++;
				linelength += WriteOperand(operands[0], writeSize);
				for (int i = 1; i < operands.Count; i++)
				{
					Owner.Writer.Write(", ");
					linelength += 2;
					linelength += WriteOperand(operands[i], writeSize);
				}
			}
			
			Owner.WriteCommentOf(instruction, linelength);
		}

		/// <inheritdoc />
		void IInstructionWriter.WriteInstruction(IInstruction instruction)
		{
			WriteInstruction((X86Instruction)instruction);
		}

		#region Operands
		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(Operand operand)
		{ return WriteOperand(operand, true); }

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteOperand(Operand operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion

			int length = 0;
			TypeSwitch.On(operand)
				.Case((EffectiveAddress x) => length = WriteEffectiveAddressOperand(x, writeSize))
				.Case((Immediate x) => length = WriteImmediateOperand(x, writeSize))
				.Case((RegisterOperand x) => length = WriteRegisterOperand(x, writeSize))
				.Case((RelativeOffset x) => length = WriteRelativeOffsetOperand(x, writeSize))
				.Case((FarPointer x) => length = WriteFarPointerOperand(x, writeSize))
				.Default(() => { throw new InvalidOperationException("Unknown operand type."); });
			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteRelativeOffsetOperand(RelativeOffset operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
				length += WriteSize(operand.PreferredSize);

			length += Owner.WriteExpression(operand.Expression);
			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteRegisterOperand(RegisterOperand operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
				length += WriteSize(operand.PreferredSize);

			string str = Enum.GetName(typeof(Register), operand.Register);
			length += str.Length;
			Owner.Writer.Write(str);
			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteImmediateOperand(Immediate operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
				length += WriteSize(operand.PreferredSize);

			length += Owner.WriteExpression(operand.Expression);
			return length;
		}

		/// <summary>
		/// Writes an effective address operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteEffectiveAddressOperand(EffectiveAddress operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			Owner.Writer.Write("[");
			length++;
			// TODO: Possible segment override here.

			// Base register.
			string str;
			if (operand.BaseRegister != Register.None)
			{
				str = Enum.GetName(typeof(Register), operand.BaseRegister);
				length += str.Length;
				Owner.Writer.Write(str);
			}
			// Index register.
			if (operand.IndexRegister != Register.None)
			{
				if (operand.BaseRegister != Register.None)
				{
					Owner.Writer.Write("+");
					length++;
				}
				str = Enum.GetName(typeof(Register), operand.IndexRegister);
				length += str.Length;
				Owner.Writer.Write(str);
				// Scale
				if (operand.Scale > 1)
				{
					str = String.Format("*{0}", operand.Scale);
					length += str.Length;
					Owner.Writer.Write(str);
				}
			}
			// Displacement
			if (operand.Displacement != null)
			{
				if (operand.BaseRegister != Register.None || operand.IndexRegister != Register.None)
				{
					Owner.Writer.Write("+");
					length++;
				}
				length += Owner.WriteExpression(operand.Displacement);
			}

			Owner.Writer.Write("]");
			length++;

			return length;
		}

		/// <summary>
		/// Writes a far pointer operand.
		/// </summary>
		/// <returns>The number of characters written.</returns>
		private int WriteFarPointerOperand(FarPointer operand, bool writeSize)
		{
			#region Contract
			if (operand == null) return 0;
			#endregion
			int length = 0;

			if (writeSize)
				length += WriteSize(operand.PreferredSize);

			length += Owner.WriteExpression(operand.Selector);
			Owner.Writer.Write(":");
			length += 1;
			length += Owner.WriteExpression(operand.Offset);
			return length;
		}

		/// <summary>
		/// Writes the specified size to the text writer.
		/// </summary>
		/// <param name="size">The <see cref="DataSize"/> to write.</param>
		/// <returns>The number of written characters.</returns>
		private int WriteSize(DataSize size)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion
			switch (size)
			{
				case DataSize.Bit8:
					Owner.Writer.Write("byte ");
					return 5;
				case DataSize.Bit16:
					Owner.Writer.Write("word ");
					return 5;
				case DataSize.Bit32:
					Owner.Writer.Write("dword ");
					return 6;
				case DataSize.Bit64:
					Owner.Writer.Write("qword ");
					return 6;
				case DataSize.None:
					return 0;
				default:
					throw new LanguageException(String.Format("Unknown or unsupported DataSize: {0}",
						Enum.GetName(typeof(DataSize), size)));
			}
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.owner != null);
		}
		#endregion
	}
}
