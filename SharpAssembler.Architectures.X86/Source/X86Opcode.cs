using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// A description for an instruction.
	/// </summary>
	[ContractClass(typeof(Contracts.X86OpcodeContract))]
	public abstract class X86Opcode
	{
		/// <inheritdoc />
		public abstract string Mnemonic
		{ get; }

		/// <summary>
		/// Gets a list of variants of the opcode.
		/// </summary>
		/// <returns>A list of <see cref="X86OpcodeVariant"/> objects.</returns>
		public abstract IList<X86OpcodeVariant> GetVariants();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="X86Opcode"/> class.
		/// </summary>
		protected X86Opcode()
		{ /* Nothing to do. */ }
		#endregion
	}

	#region Contract
	namespace Contracts
	{
		[ContractClassFor(typeof(X86Opcode))]
		abstract class X86OpcodeContract : X86Opcode
		{
			public override IList<X86OpcodeVariant> GetVariants()
			{
				Contract.Ensures(Contract.Result<IList<X86OpcodeVariant>>() != null);
				return default(IList<X86OpcodeVariant>);
			}

			public override abstract string Mnemonic { get; }
		}
	}
	#endregion

	/// <summary>
	/// The AND (Logical AND) instruction opcode.
	/// </summary>
	public class AndOpcode : X86Opcode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AndOpcode"/> class.
		/// </summary>
		public AndOpcode()
			: base()
		{ /* Nothing to do. */ }
		#endregion

		/// <inheritdoc />
		public override string Mnemonic
		{
			get { return "and"; }
		}

		private ReadOnlyCollection<X86OpcodeVariant> variants;
		/// <inheritdoc />
		public override IList<X86OpcodeVariant> GetVariants()
		{
			if (variants == null)
			{
				X86OpcodeVariant[] list = new X86OpcodeVariant[]
				{
					// AND AL, imm8
					new X86OpcodeVariant(
						new byte[] { 0x24 },
						new OperandDescriptor(Register.AL),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
					// AND AX, imm16
					new X86OpcodeVariant(
						new byte[] { 0x25 },
						new OperandDescriptor(Register.AX),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
					// AND EAX, imm32
					new X86OpcodeVariant(
						new byte[] { 0x25 },
						new OperandDescriptor(Register.EAX),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
					// AND RAX, imm32
					new X86OpcodeVariant(
						new byte[] { 0x25 },
						new OperandDescriptor(Register.RAX),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

					// AND reg/mem8, imm8
					new X86OpcodeVariant(
						new byte[] { 0x80 }, 4,
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
					// AND reg/mem16, imm16
					new X86OpcodeVariant(
						new byte[] { 0x81 }, 4,
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit16)),
					// AND reg/mem32, imm32
					new X86OpcodeVariant(
						new byte[] { 0x81 }, 4,
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),
					// AND reg/mem64, imm32
					new X86OpcodeVariant(
						new byte[] { 0x81 }, 4,
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit32)),

					// AND reg/mem16, imm8
					new X86OpcodeVariant(
						new byte[] { 0x83 }, 4,
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
					// AND reg/mem32, imm8
					new X86OpcodeVariant(
						new byte[] { 0x83 }, 4,
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),
					// AND reg/mem64, imm8
					new X86OpcodeVariant(
						new byte[] { 0x83 }, 4,
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
						new OperandDescriptor(OperandType.Immediate, DataSize.Bit8)),


					// AND reg/mem8, reg8
					new X86OpcodeVariant(
						new byte[] { 0x20 },
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit),
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit)),
					// AND reg/mem16, reg16
					new X86OpcodeVariant(
						new byte[] { 0x21 },
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit),
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit)),
					// AND reg/mem32, reg32
					new X86OpcodeVariant(
						new byte[] { 0x21 },
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit),
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit)),
					// AND reg/mem64, reg64
					new X86OpcodeVariant(
						new byte[] { 0x21 },
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit),
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit)),


					// AND reg8, reg/mem8
					new X86OpcodeVariant(
						new byte[] { 0x22 },
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose8Bit),
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose8Bit)),
					// AND reg16, reg/mem16
					new X86OpcodeVariant(
						new byte[] { 0x23 },
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose16Bit),
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose16Bit)),
					// AND reg32, reg/mem32
					new X86OpcodeVariant(
						new byte[] { 0x23 },
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose32Bit),
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose32Bit)),
					// AND reg64, reg/mem64
					new X86OpcodeVariant(
						new byte[] { 0x23 },
						new OperandDescriptor(OperandType.RegisterOperand, RegisterType.GeneralPurpose64Bit),
						new OperandDescriptor(OperandType.RegisterOrMemoryOperand, RegisterType.GeneralPurpose64Bit)),
				};
				this.variants = new ReadOnlyCollection<X86OpcodeVariant>(list);
			}
			return this.variants;
		}
	}
}
