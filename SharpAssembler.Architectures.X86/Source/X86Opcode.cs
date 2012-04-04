using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// A description for an instruction.
	/// </summary>
	[ContractClass(typeof(Contracts.X86OpcodeContract))]
	public abstract partial class X86Opcode : IOpcode
	{
		private readonly string mnemonic;
		/// <inheritdoc />
		public virtual string Mnemonic
		{
			get { return this.mnemonic; }
		}

		private readonly int operandCount;
		/// <summary>
		/// Gets the number of operands that instructions with this opcode use.
		/// </summary>
		/// <value>The number of operands; or 0 when the instruction does not take any operands.</value>
		public int OperandCount
		{
			get { return this.operandCount; }
		}

		private readonly ReadOnlyCollection<X86OpcodeVariant> variants;
		/// <summary>
		/// Gets a read-only ordered collection of opcode variants.
		/// </summary>
		/// <value>A collection of <see cref="X86OpcodeVariant"/> objects.</value>
		public ReadOnlyCollection<X86OpcodeVariant> Variants
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<ReadOnlyCollection<X86OpcodeVariant>>() != null);
				#endregion
				return this.variants;
			}
		}

		/// <summary>
		/// Gets whether the instruction can lock so that the read-modify-write
		/// operation is executed atomically.
		/// </summary>
		/// <value><see langword="true"/> when the instruction can lock;
		/// otherwise, <see langword="false"/>.</value>
		/// <remarks>
		/// The default implementation returns <see langword="false"/>.
		/// </remarks>
		public virtual bool CanLock
		{
			get { return false; }
		}

		/// <summary>
		/// Gets whether this opcode is valid in 64-bit mode.
		/// </summary>
		/// <value><see langword="true"/> when the opcode is valid in 64-bit mode;
		/// otherwise, <see langword="false"/>.</value>
		/// <remarks>
		/// <para>Even when an opcode is valid in 64-bit mode,
		/// it may be that certain opcode variants are not valid in 64-bit mode.</para>
		/// <para>The default implementation returns <see langword="true"/>.</para>
		/// </remarks>
		public virtual bool IsValidIn64BitMode
		{
			get { return true; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="X86Opcode"/> class.
		/// </summary>
		/// <param name="mnemonic">The mnemonic of the opcode.</param>
		/// <param name="operandCount">The number of operands used by the opcode.</param>
		/// <param name="variants">The opcode variants.</param>
		protected X86Opcode(string mnemonic, int operandCount, IEnumerable<X86OpcodeVariant> variants)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(mnemonic != null);
			Contract.Requires<ArgumentNullException>(variants != null);
			Contract.Requires<ArgumentOutOfRangeException>(operandCount >= 0);
			#endregion
			this.mnemonic = mnemonic;
			this.operandCount = operandCount;
			this.variants = variants.ToList().AsReadOnly();
		}
		#endregion

		/// <summary>
		/// Creates a new instruction for this opcode.
		/// </summary>
		/// <param name="operands">The operands of the instruction.</param>
		/// <returns>The created instruction.</returns>
		public X86Instruction CreateInstruction(params Operand[] operands)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(operands != null);
			Contract.Ensures(Contract.Result<X86Instruction>() != null);
			#endregion
			return CreateInstruction((IList<Operand>)operands);
		}

		/// <summary>
		/// Creates a new instruction for this opcode.
		/// </summary>
		/// <param name="operands">The operands of the instruction.</param>
		/// <returns>The created instruction.</returns>
		public virtual X86Instruction CreateInstruction(IList<Operand> operands)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(operands != null);
			Contract.Ensures(Contract.Result<X86Instruction>() != null);
			#endregion
			X86Instruction instr;
			if (this.CanLock)
				instr = new LockInstruction(this, operands);
			else
				instr = new X86Instruction(this, operands);
			return instr;
		}

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(this.mnemonic != null);
			Contract.Invariant(this.operandCount >= 0);
			Contract.Invariant(this.variants != null);
		}
		#endregion
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(X86Opcode))]
		abstract class X86OpcodeContract : X86Opcode
		{
			public X86OpcodeContract() : base(null, 0, null) { }
		}
	}
	#endregion
}
