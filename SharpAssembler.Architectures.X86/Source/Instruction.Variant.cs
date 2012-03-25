#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011 Daniël Pelsmaeker
 * 
 * This file is part of SharpAssembler.
 * 
 * SharpAssembler is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpAssembler is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SharpAssembler.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using SharpAssembler;
using SharpAssembler.Architectures.X86.Operands;

namespace SharpAssembler.Architectures.X86
{
	partial class X86Instruction
	{
		// TODO: Rename to Variant
		/// <summary>
		/// A single variant of an instruction. Most instructions have multiple possible variants.
		/// </summary>
		internal sealed class InstructionVariant
		{
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="X86Instruction.InstructionVariant"/> class.
			/// </summary>
			/// <param name="opcode">An array of bytes representing the opcode bytes for this instruction
			/// variant.</param>
			/// <param name="descriptors">The operand descriptors describing this instruction variant's
			/// operands.</param>
			public InstructionVariant(byte[] opcode, params OperandDescriptor[] descriptors)
				: this(opcode, 0, DataSize.None, descriptors)
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(opcode != null);
				Contract.Requires<ArgumentNullException>(descriptors != null);
				#endregion
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="X86Instruction.InstructionVariant"/> class.
			/// </summary>
			/// <param name="opcode">An array of bytes representing the opcode bytes for this instruction
			/// variant.</param>
			/// <param name="operandSize">The explicit operand size.</param>
			public InstructionVariant(byte[] opcode, DataSize operandSize)
				: this(opcode, 0, operandSize, new OperandDescriptor[0])
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(opcode != null);
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
				#endregion
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="X86Instruction.InstructionVariant"/> class.
			/// </summary>
			/// <param name="opcode">An array of bytes representing the opcode bytes for this instruction
			/// variant.</param>
			/// <param name="fixedReg">The 3-bit fixed value of the REG part of the ModR/M byte;
			/// or 0 when not used.</param>
			/// <param name="descriptors">The operand descriptors describing this instruction variant's
			/// operands.</param>
			public InstructionVariant(byte[] opcode, byte fixedReg, params OperandDescriptor[] descriptors)
				: this(opcode, fixedReg, DataSize.None, descriptors)
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(opcode != null);
				Contract.Requires<ArgumentOutOfRangeException>(fixedReg <= 0x07,
					"Only the least significant 3 bits may be set.");
				Contract.Requires<ArgumentNullException>(descriptors != null);
				#endregion
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="X86Instruction.InstructionVariant"/> class.
			/// </summary>
			/// <param name="opcode">An array of bytes representing the opcode bytes for this instruction
			/// variant.</param>
			/// <param name="fixedReg">The 3-bit fixed value of the REG part of the ModR/M byte;
			/// or 0 when not used.</param>
			/// <param name="operandSize">The explicit operand size.</param>
			/// <param name="descriptors">The operand descriptors describing this instruction variant's
			/// operands.</param>
			public InstructionVariant(byte[] opcode, byte fixedReg, DataSize operandSize,
				params OperandDescriptor[] descriptors)
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(opcode != null);
				Contract.Requires<ArgumentOutOfRangeException>(fixedReg <= 0x07,
					"Only the least significant 3 bits may be set.");
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
				Contract.Requires<ArgumentNullException>(descriptors != null);
				#endregion

				this.opcode = opcode;
				this.fixedReg = fixedReg;
				this.descriptors = descriptors;
				this.operandSize = operandSize;
			}
			#endregion

			#region Properties
			private byte[] opcode;
			/// <summary>
			/// Gets or sets the opcode emitted for this instruction variant.
			/// </summary>
			/// <value>An array of bytes. Use an empty array to specify no opcode.</value>
			[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
			public byte[] Opcode
			{
				get
				{
					#region Contract
					Contract.Ensures(Contract.Result<byte[]>() != null);
					#endregion
					return opcode;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentNullException>(value != null);
					#endregion
					opcode = value;
				}
			}

			private byte fixedReg;
			/// <summary>
			/// Gets or sets the fixed value of the REG part of the ModR/M byte.
			/// </summary>
			/// <value>The 3-bit fixed REG value.</value>
			/// <remarks>
			/// When no operands require a ModR/M byte, or when the encoding of an operand puts a value into the Mod
			/// R/M's REG field, this property is ignored.</remarks>
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
			/// Gets or sets whether this instruction is valid in 64-bit mode.
			/// </summary>
			/// <value><see langword="true"/> when the instruction is valid in 64-bit mode;
			/// otherwise, <see langword="false"/>.</value>
			public bool ValidIn64BitMode
			{
				get { return validIn64BitMode; }
				set { validIn64BitMode = value; }
			}

			private CpuFeatures requiredFeatures = CpuFeatures.None;
			/// <summary>
			/// Gets or sets the CPU features which are required for this instruction to be valid.
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

			private OperandDescriptor[] descriptors;
			/// <summary>
			/// Gets or sets an array of operand descriptors.
			/// </summary>
			/// <value>An array of <see cref="OperandDescriptor"/> objects.</value>
			[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
			public OperandDescriptor[] Descriptors
			{
				get
				{
					#region Contract
					Contract.Ensures(Contract.Result<OperandDescriptor[]>() != null);
					#endregion
					return descriptors;
				}
				set
				{
					#region Contract
					Contract.Requires<ArgumentNullException>(value != null);
					#endregion
					descriptors = value;
				}
			}
			#endregion

			#region Methods
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
				instr.Opcode = opcode;

				// Set the fixed REG value, if any.
				instr.FixedReg = fixedReg;

				int i = 0;
				foreach(var operand in operands)
				{
					if (operand == null)
						// No operand. Nothing to do.
						continue;
					if (i >= descriptors.Length)
						// No descriptors left. Nothing to be done.
						break;

					operand.Adjust(descriptors[i]);
					operand.Construct(context, instr);
					i++;
				}

				// When the operand size has been explicitly set, set it on the encoded instruction.
				if (operandSize != DataSize.None)
					instr.SetOperandSize(context.Representation.Architecture.OperandSize, operandSize);

				// We are done.
				return instr;
			}

#if false
		/// <summary>
		/// Encodes this instruction variant into its binary representation
		/// using the specified operands.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> used.</param>
		/// <param name="operands">The <see cref="Operand"/> objects to
		/// encode.</param>
		/// <remarks>This method assumes that a call to <see
		/// cref="Match(IList{Operand})"/>
		/// with <paramref name="operands"/> would return
		/// <see langword="true"/>. Otherwise, exceptions may be thrown or
		/// the results may yield unexpected values.</remarks>
		internal void Encode(Context context, IList<Operand> operands)
		{
			InstructionEncoder encoder = context.Encoder;

			encoder.Opcode = opcode;
			encoder.Reg = fixedReg;

			for (int i = 0; i < operands.Count; i++)
			{
				if (descriptors[i].OperandType != OperandType.None &&
					descriptors[i].OperandType != OperandType.FixedRegister)
				{
					descriptors[i].Adjust(operands[i]);
					operands[i].Encode(context);
				}
			}
		}
#endif

			/// <summary>
			/// Checks whether the specified array of operands would provide a match to this
			/// <see cref="X86Instruction.InstructionVariant"/>.
			/// </summary>
			/// <param name="operandSize">The explicitly provided operand size of the instruction to be matched,
			/// or <see cref="DataSize.None"/>.</param>
			/// <param name="operands">The array of <see cref="Operand"/> objects to test.</param>
			/// <returns><see langword="true"/> when the operands match this
			/// <see cref="X86Instruction.InstructionVariant"/>; otherwise, <see langword="false"/>.</returns>
			public bool Match(DataSize operandSize, IList<Operand> operands)
			{
				DataSize variantOperandSize = DataSize.None;
				int j = 0;
				for (int i = 0; i < descriptors.Length; i++)
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

					switch(descriptors[i].OperandType)
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
					String.Join(" ", from b in opcode select String.Format("{0:X2}", b)),
					fixedReg,
					String.Join(", ", descriptors));
			}
			#endregion
		}
	}
}
