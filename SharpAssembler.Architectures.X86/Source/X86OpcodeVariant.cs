using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// A single variant of an opcode. Most opcodes have multiple possible variants.
	/// </summary>
	public sealed class X86OpcodeVariant
	{
		private byte[] opcodeBytes;
		/// <summary>
		/// Gets or sets the opcode bytes emitted for this opcode variant.
		/// </summary>
		/// <value>An array of bytes. Use an empty array to specify no opcode bytes.</value>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public byte[] OpcodeBytes
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<byte[]>() != null);
				#endregion
				return opcodeBytes;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				opcodeBytes = value;
			}
		}

		private byte fixedReg;
		/// <summary>
		/// Gets or sets the fixed value of the REG part of the ModR/M byte.
		/// </summary>
		/// <value>The 3-bit fixed REG value.</value>
		/// <remarks>
		/// When no operands require a ModR/M byte, or when the encoding of an operand puts a value into the Mod
		/// R/M's REG field, this property is ignored.
		/// </remarks>
		public byte FixedReg
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<byte>() <= 0x07,
					"Only the least significant 3 bits may be set.");
				#endregion
				return fixedReg;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(value <= 0x07,
					"Only the least significant 3 bits may be set.");
				#endregion
				fixedReg = value;
			}
		}

		private DataSize operandSize = DataSize.None;
		/// <summary>
		/// Gets or sets the operand size for this instruction variant.
		/// </summary>
		/// <value>The explicit operand size for this instruction; or <see cref="DataSize.None"/> to determine it
		/// from the operands. The default is <see cref="DataSize.None"/>.</value>
		/// <remarks>
		/// This property is intended to be used with instructions which do not have any operand from which the
		/// operand size can be determined.
		/// </remarks>
		public DataSize OperandSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return operandSize;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				operandSize = value;
			}
		}

		private bool validIn64BitMode = true;
		/// <summary>
		/// Gets or sets whether this opcode variant is valid in 64-bit mode.
		/// </summary>
		/// <value><see langword="true"/> when the opcode variant is valid in 64-bit mode;
		/// otherwise, <see langword="false"/>.</value>
		public bool ValidIn64BitMode
		{
			get { return validIn64BitMode; }
			set
			{
				validIn64BitMode = value;
				if (value == false)
					requires64BitMode = false;
			}
		}

		private bool requires64BitMode = false;
		/// <summary>
		/// Gets or sets whether this opcode variant requires 64-bit mode. If so, no REX prefix is encoded.
		/// </summary>
		/// <value><see langword="true"/> when the opcode variant requires 64-bit mode;
		/// otherwise, <see langword="false"/>.</value>
		public bool Requires64BitMode
		{
			get { return requires64BitMode; }
			set
			{
				requires64BitMode = value;
				if (value == true)
					validIn64BitMode = true;
			}
		}

		private CpuFeatures requiredFeatures = CpuFeatures.None;
		/// <summary>
		/// Gets or sets the CPU features which are required for this opcode variant to be valid.
		/// </summary>
		/// <value>A bitwise combination of members of the <see cref="CpuFeatures"/> enumeration.</value>
		public CpuFeatures RequiredFeatures
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(CpuFeatures), Contract.Result<CpuFeatures>()));
				#endregion
				return requiredFeatures;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(CpuFeatures), value));
				#endregion
				requiredFeatures = value;
			}
		}

		private Collection<OperandDescriptor> descriptors = new Collection<OperandDescriptor>();
		/// <summary>
		/// Gets a collection of operand descriptors.
		/// </summary>
		/// <value>A collection of <see cref="OperandDescriptor"/> objects.</value>
		public Collection<OperandDescriptor> Descriptors
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Collection<OperandDescriptor>>() != null);
				#endregion
				return descriptors;
			}
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="X86OpcodeVariant"/> class.
		/// </summary>
		/// <param name="opcodeBytes">An array of bytes representing the opcode bytes for this instruction
		/// variant.</param>
		/// <param name="descriptors">The operand descriptors describing this instruction variant's
		/// operands.</param>
		public X86OpcodeVariant(
			byte[] opcodeBytes,
			params OperandDescriptor[] descriptors)
			: this(opcodeBytes, 0, DataSize.None, descriptors)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(opcodeBytes != null);
			Contract.Requires<ArgumentNullException>(descriptors != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86OpcodeVariant"/> class.
		/// </summary>
		/// <param name="opcodeBytes">An array of bytes representing the opcode bytes for this instruction
		/// variant.</param>
		/// <param name="operandSize">The explicit operand size.</param>
		public X86OpcodeVariant(
			byte[] opcodeBytes,
			DataSize operandSize)
			: this(opcodeBytes, 0, operandSize, new OperandDescriptor[0])
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(opcodeBytes != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86OpcodeVariant"/> class.
		/// </summary>
		/// <param name="opcodeBytes">An array of bytes representing the opcode bytes for this instruction
		/// variant.</param>
		/// <param name="fixedReg">The 3-bit fixed value of the REG part of the ModR/M byte;
		/// or 0 when not used.</param>
		/// <param name="descriptors">The operand descriptors describing this instruction variant's
		/// operands.</param>
		public X86OpcodeVariant(
			byte[] opcodeBytes,
			byte fixedReg,
			params OperandDescriptor[] descriptors)
			: this(opcodeBytes, fixedReg, DataSize.None, descriptors)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(opcodeBytes != null);
			Contract.Requires<ArgumentOutOfRangeException>(fixedReg <= 0x07,
				"Only the least significant 3 bits may be set.");
			Contract.Requires<ArgumentNullException>(descriptors != null);
			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86OpcodeVariant"/> class.
		/// </summary>
		/// <param name="opcodeBytes">An array of bytes representing the opcode bytes for this instruction
		/// variant.</param>
		/// <param name="fixedReg">The 3-bit fixed value of the REG part of the ModR/M byte;
		/// or 0 when not used.</param>
		/// <param name="operandSize">The explicit operand size.</param>
		/// <param name="descriptors">The operand descriptors describing this instruction variant's
		/// operands.</param>
		public X86OpcodeVariant(
			byte[] opcodeBytes,
			byte fixedReg,
			DataSize operandSize,
			params OperandDescriptor[] descriptors)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(opcodeBytes != null);
			Contract.Requires<ArgumentOutOfRangeException>(fixedReg <= 0x07,
				"Only the least significant 3 bits may be set.");
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
			Contract.Requires<ArgumentNullException>(descriptors != null);
			#endregion

			this.opcodeBytes = opcodeBytes;
			this.fixedReg = fixedReg;
			this.descriptors.AddRange(descriptors);
			this.operandSize = operandSize;
		}
		#endregion

		#region Construction
		/// <summary>
		/// Constructs the representation of this instruction variant using the specified operands.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> used.</param>
		/// <param name="operands">The <see cref="Operand"/> objects to encode.</param>
		/// <returns>The encoded instruction.</returns>
		public EncodedInstruction Construct(Context context, IEnumerable<Operand> operands)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentNullException>(operands != null);
			Contract.Ensures(Contract.Result<EncodedInstruction>() != null);
			#endregion

			return Construct(context, operands, false);
		}

		/// <summary>
		/// Constructs the representation of this instruction variant using the specified operands.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> used.</param>
		/// <param name="operands">The <see cref="Operand"/> objects to encode.</param>
		/// <param name="lockPrefix">Whether to use a lock prefix.</param>
		/// <returns>The encoded instruction.</returns>
		public EncodedInstruction Construct(Context context, IEnumerable<IConstructableOperand> operands, bool lockPrefix)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentNullException>(operands != null);
			Contract.Ensures(Contract.Result<EncodedInstruction>() != null);
			#endregion

			EncodedInstruction instr = new EncodedInstruction();

			// Set the lock prefix.
			instr.SetLock(lockPrefix);

			// Set the opcode.
			instr.Opcode = opcodeBytes;

			// Set the fixed REG value, if any.
			instr.FixedReg = fixedReg;

			int i = 0;
			foreach (var operand in operands)
			{
				if (operand == null)
					// No operand. Nothing to do.
					continue;
				if (i >= descriptors.Count)
					// No descriptors left. Nothing to be done.
					break;

				operand.Adjust(descriptors[i]);
				operand.Construct(context, instr);
				i++;
			}

			// When the operand size has been explicitly set, set it on the encoded instruction.
			if (operandSize != DataSize.None)
				instr.SetOperandSize(context.Representation.Architecture.OperandSize, operandSize);
			if (requires64BitMode)
				// SetOperandSize() will cause the instruction to encode a REX prefix, which is not required in
				// this particular case. So reset it back to null to encode no REX prefix.
				instr.Use64BitOperands = null;

			// We are done.
			return instr;
		}

		/// <summary>
		/// Checks whether the specified array of operands would provide a match to this
		/// <see cref="X86Instruction.X86OpcodeVariant"/>.
		/// </summary>
		/// <param name="operandSize">The explicitly provided operand size of the instruction to be matched,
		/// or <see cref="DataSize.None"/>.</param>
		/// <param name="context">The <see cref="Context"/>/</param>
		/// <param name="operands">The array of <see cref="Operand"/> objects to test.</param>
		/// <returns><see langword="true"/> when the operands match this
		/// <see cref="X86Instruction.X86OpcodeVariant"/>; otherwise, <see langword="false"/>.</returns>
		public bool Match(DataSize operandSize, Context context, IList<Operand> operands)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentNullException>(operands != null);
			#endregion

			// Check whether the variant is valid in the current mode.
			if (!this.validIn64BitMode && context.Representation.Architecture.OperandSize == DataSize.Bit64)
				return false;
			else if (this.requires64BitMode && context.Representation.Architecture.OperandSize != DataSize.Bit64)
				return false;

			DataSize variantOperandSize = DataSize.None;
			int j = 0;
			for (int i = 0; i < descriptors.Count; i++)
			{
				if (descriptors[i].OperandType == OperandType.None)
				{
					// A None operand descriptor, which MAY correspond to a null-operand.
					if (j < operands.Count && operands[j] == null)
						j++;
				}
				else
				{
					// A not-None operand descriptor, which MUST correspond to a non-null operand.
					if (j >= operands.Count || operands[j] == null || !operands[j].IsMatch(descriptors[i]))
						return false;
					j++;
				}

				switch (descriptors[i].OperandType)
				{
					case OperandType.RegisterOperand:
					case OperandType.RegisterOrMemoryOperand:
						variantOperandSize = descriptors[i].RegisterType.GetSize();
						break;
					case OperandType.FixedRegister:
						variantOperandSize = descriptors[i].FixedRegister.GetSize();
						break;
					default:
						variantOperandSize = descriptors[i].Size;
						break;
				}
			}
			// We have more non-null operands than descriptors.
			for (; j < operands.Count; j++)
			{
				if (operands[j] != null)
					return false;
			}

			if (this.operandSize != DataSize.None)
			{
				Contract.Assume(variantOperandSize == DataSize.None || this.operandSize == variantOperandSize);
				variantOperandSize = this.operandSize;
			}
			// Has the operand size been specified explicitly,
			// then test wheter it matches the operand size of this variant.
			if (operandSize != DataSize.None && operandSize != variantOperandSize)
				return false;

			// All tests passed. It's a match.
			return true;
		}
		#endregion

		/// <summary>
		/// Checks whether the instruction variant is supported in the specified architecture (depending 64-bit
		/// mode or CPU features).
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <returns><see langword="true"/> when the instruction variant is valid for the
		/// specified architecture; otherwise, <see langword="false"/>.</returns>
		public bool IsValid(X86Architecture architecture)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(architecture != null);
			#endregion

			if ((architecture.Features & requiredFeatures) != requiredFeatures)
				return false;
			// TODO: Implement.
			//if (architecture.AddressSize
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns>A string.</returns>
		public override string ToString()
		{
			string operandSizeStr = "";
			if (operandSize != DataSize.None)
				operandSizeStr = String.Format("O{0}", (int)operandSize << 3);
			return String.Format(CultureInfo.InvariantCulture, "{0}{1} /{2} {3}",
				operandSizeStr,
				String.Join(" ", from b in opcodeBytes select String.Format("{0:X2}", b)),
				fixedReg,
				String.Join(", ", descriptors));
		}
	}
}
