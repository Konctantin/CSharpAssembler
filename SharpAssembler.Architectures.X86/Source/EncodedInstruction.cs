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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using SharpAssembler;
using SharpAssembler.Symbols;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Represents an encoded instruction.
	/// </summary>
	public sealed partial class EncodedInstruction : IEmittable
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="EncodedInstruction"/> class.
		/// </summary>
		public EncodedInstruction()
		{
		}
		#endregion

		#region Properties
		private PrefixLockRepeat prefix1 = PrefixLockRepeat.None;
		/// <summary>
		/// Gets or sets the group 1 prefix used.
		/// </summary>
		/// <value>A member of the <see cref="PrefixLockRepeat"/> enumeration; or
		/// <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixLockRepeat.None"/> to specify no prefix.
		/// The default is <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixLockRepeat.None"/>.</value>
		public PrefixLockRepeat Prefix1
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(PrefixLockRepeat), Contract.Result<PrefixLockRepeat>()));
				#endregion
				return prefix1;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(PrefixLockRepeat), value));
				#endregion
				prefix1 = value;
			}
		}

		private PrefixSegmentBranch prefix2 = PrefixSegmentBranch.None;
		/// <summary>
		/// Gets or sets the group 2 prefix used.
		/// </summary>
		/// <value>A member of the <see cref="PrefixSegmentBranch"/> enumeration; or
		/// <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixSegmentBranch.None"/> to specify no prefix.
		/// The default is <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixSegmentBranch.None"/>.</value>
		public PrefixSegmentBranch Prefix2
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(PrefixSegmentBranch), Contract.Result<PrefixSegmentBranch>()));
				#endregion
				return prefix2;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(PrefixSegmentBranch), value));
				#endregion
				prefix2 = value;
			}
		}

		private PrefixAddressSizeOverride prefix3 = PrefixAddressSizeOverride.None;
		/// <summary>
		/// Gets or sets the group 3 prefix used.
		/// </summary>
		/// <value>A member of the <see cref="PrefixAddressSizeOverride"/> enumeration; or
		/// <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixAddressSizeOverride.None"/> to specify no prefix.
		/// The default is <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixAddressSizeOverride.None"/>.</value>
		public PrefixAddressSizeOverride Prefix3
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(PrefixAddressSizeOverride), Contract.Result<PrefixAddressSizeOverride>()));
				#endregion
				return prefix3;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(PrefixAddressSizeOverride), value));
				#endregion
				prefix3 = value;
			}
		}

		private PrefixOperandSizeOverride prefix4 = PrefixOperandSizeOverride.None;
		/// <summary>
		/// Gets or sets the group 4 prefix used.
		/// </summary>
		/// <value>A member of the <see cref="PrefixOperandSizeOverride"/> enumeration; or
		/// <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixOperandSizeOverride.None"/> to specify no prefix.
		/// The default is <see cref="SharpAssembler.Architectures.X86.EncodedInstruction.PrefixOperandSizeOverride.None"/>.</value>
		public PrefixOperandSizeOverride Prefix4
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(PrefixOperandSizeOverride), Contract.Result<PrefixOperandSizeOverride>()));
				#endregion
				return prefix4;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(PrefixOperandSizeOverride), value));
				#endregion
				prefix4 = value;
			}
		}

		private byte[] mandatoryPrefix = new byte[0];
		/// <summary>
		/// Gets or sets the mandatory prefix.
		/// </summary>
		/// <value>The mandatory prefix as an array of bytes. The default is an empty array.</value>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public byte[] MandatoryPrefix
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<byte[]>() != null);
				#endregion
				return mandatoryPrefix;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentNullException>(value != null);
				#endregion
				mandatoryPrefix = value;
			}
		}

		private byte[] opcode = new byte[0];
		/// <summary>
		/// Gets or sets the opcode of the encoded instruction.
		/// </summary>
		/// <value>The opcode bytes of the instruction. The default is an empty array.</value>
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

		private byte opcodeReg = 0;
		/// <summary>
		/// Gets or sets a value which is added to the opcode byte.
		/// </summary>
		/// <value>The 4-bit opcode REG value. The default is 0.</value>
		/// <remarks>
		/// The least significant three bits are encoded by OR-ing them with the last opcode byte, while the fourth bit
		/// is encoded in the REX.B bit.
		/// </remarks>
		public byte OpcodeReg
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<byte>() <= 0x0F,
					"Only the first 4 bits may be set.");
				#endregion
				return opcodeReg;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(value <= 0x0F,
					"Only the first 4 bits may be set.");
				#endregion
				opcodeReg = value;
			}
		}

		private ModRMByte modRM = null;
		/// <summary>
		/// Gets the ModR/M byte to encode.
		/// </summary>
		/// <value>A <see cref="ModRMByte"/>; or <see langword="null"/> to not encode a ModR/M byte. The default is
		/// <see langword="null"/>.</value>
		/// <remarks>
		/// To ensure that a ModR/M byte will be created (i.e. that this property is not <see langword="null"/>), call
		/// the <see cref="SetModRMByte"/> method.
		/// </remarks>
		public ModRMByte ModRM
		{
			get { return modRM; }
		}

		private SibByte sib = null;
		/// <summary>
		/// Gets the SIB byte to encode.
		/// </summary>
		/// <value>A <see cref="SibByte"/>; or <see langword="null"/> to not encode a SIB byte. The default is
		/// <see langword="null"/>.</value>
		/// <remarks>
		/// To ensure that a ModR/M byte will be created (i.e. that this property is not <see langword="null"/>), call
		/// the <see cref="SetSIBByte"/> method.
		/// </remarks>
		public SibByte Sib
		{
			get { return sib; }
		}

		private byte fixedReg;
		/// <summary>
		/// Gets or sets the fixed value of the REG part of the ModR/M byte, when a ModR/M is used.
		/// </summary>
		/// <value>The 3-bit fixed REG value.</value>
		/// <remarks>
		/// <para>When no operands require a ModR/M byte, or when the encoding of an operand puts a value into the
		/// ModR/M's REG field, this property is ignored.</para>
		/// <para>Specify this value before the first call to <see cref="SetModRMByte"/>; otherwise the REG field is
		/// not set.</para>
		/// </remarks>
		public byte FixedReg
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<byte>() <= 0x07,
					"Only the first 3 bits may be set.");
				#endregion
				return fixedReg;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentOutOfRangeException>(value <= 0x07,
					"Only the first 3 bits may be set.");
				#endregion
				fixedReg = value;
			}
		}

		private bool? use64BitOperands = null;
		/// <summary>
		/// Gets or sets whether 64-bit operands are used, and whether a REX prefix is used.
		/// </summary>
		/// <value><see langword="true"/> to use 64-bit operands; otherwise, <see langword="false"/>;
		/// or <see langword="null"/> when this does not apply. The default is <see langword="null"/>.</value>
		/// <remarks>
		/// When this property is not <see langword="null"/>, a REX byte is encoded. When this property is
		/// <see langword="null"/>, no REX byte is encoded, regardless of the fourth bit of the SIB byte's BASE o
		/// INDEX fields, or the fourth bit of the ModR/M byte's RM or REG fields.
		/// </remarks>
		public bool? Use64BitOperands
		{
			get { return use64BitOperands; }
			set { use64BitOperands = value; }
		}

		private SimpleExpression displacement = null;
		/// <summary>
		/// Gets or sets the displacement value.
		/// </summary>
		/// <value>An <see cref="SimpleExpression"/> specifying the displacement value or symbol;
		/// or <see langword="null"/> to use no displacement. The default is <see langword="null"/>.</value>
		public SimpleExpression Displacement
		{
			get { return displacement; }
			set { displacement = value; }
		}

		private DataSize displacementSize = DataSize.None;
		/// <summary>
		/// Gets or sets the actual size of the displacement value.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration. The default is
		/// <see cref="SharpAssembler.DataSize.None"/>.</value>
		public DataSize DisplacementSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return displacementSize;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				displacementSize = value;
			}
		}

		// FIXME: Where do AMD 3DNow! bytes go in here?
		private SimpleExpression immediate = null;
		/// <summary>
		/// Gets or sets the immediate value.
		/// </summary>
		/// <value>An <see cref="SimpleExpression"/> specifying the immediate value or symbol;
		/// or <see langword="null"/> to use no immediate. The default is <see langword="null"/>.</value>
		/// <remarks>
		/// The immediate value may be used as third opcode byte for AMD 3DNow! instructions.
		/// </remarks>
		public SimpleExpression Immediate
		{
			get { return immediate; }
			set { immediate = value; }
		}

		private DataSize immediateSize = DataSize.None;
		/// <summary>
		/// Gets or sets the actual size of the immediate value.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.
		/// The default is <see cref="SharpAssembler.DataSize.None"/>.</value>
		public DataSize ImmediateSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return immediateSize;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				immediateSize = value;
			}
		}

		private SimpleExpression extraImmediate = null;
		/// <summary>
		/// Gets or sets the immediate extra value.
		/// </summary>
		/// <value>An <see cref="SimpleExpression"/> specifying the extra immediate value or symbol;
		/// or <see langword="null"/> to use no extra immediate. The default is <see langword="null"/>.</value>
		public SimpleExpression ExtraImmediate
		{
			get { return extraImmediate; }
			set { extraImmediate = value; }
		}

		private DataSize extraImmediateSize = DataSize.None;
		/// <summary>
		/// Gets or sets the actual size of the extra immediate value.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.
		/// The default is <see cref="SharpAssembler.DataSize.None"/>.</value>
		public DataSize ExtraImmediateSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return extraImmediateSize;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				extraImmediateSize = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets the length of the encoded instruction.
		/// </summary>
		/// <returns>The length of the encoded instruction, in bytes.</returns>
		public int GetLength()
		{
			#region Contract
			Contract.Ensures(Contract.Result<int>() >= 0);
			#endregion

			int length = 0;

			// Legacy prefixes
			if (prefix1 != PrefixLockRepeat.None)
				length++;
			if (prefix2 != PrefixSegmentBranch.None)
				length++;
			if (prefix3 != PrefixAddressSizeOverride.None)
				length++;
			if (prefix4 != PrefixOperandSizeOverride.None)
				length++;

			// Mandatory prefix
			if (mandatoryPrefix != null)
				length += (int)mandatoryPrefix.Length;

			// REX prefix
			if (use64BitOperands.HasValue)
				length++;

			// Opcode
			if (opcode != null)
				length += (int)opcode.Length;

			// ModR/M byte
			if (modRM != null)
				length++;

			// SIB byte
			if (sib != null)
				length++;

			// Displacement
			length += (int)displacementSize;

			// Immediate
			length += (int)immediateSize;

			// Extra Immediate
			length += (int)extraImmediateSize;

			return length;
		}

		#region Emitting
		/// <summary>
		/// Modifies the context and emits the binary representation of this emittable.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the emittable will be emitted.</param>
		/// <param name="context">The <see cref="Context"/> in which the emittable will be emitted.</param>
		/// <returns>The number of emitted bytes.</returns>
		public int Emit(BinaryWriter writer, Context context)
		{
			// CONTRACT: IEmittable

			long instructionOffset = writer.BaseStream.Position;

			EmitLegacyPrefixes(writer);
			EmitMandatoryPrefix(writer);
			EmitREXPrefix(writer);

			EmitOpcode(writer);

			EmitModRMByte(writer);
			EmitSIBByte(writer);

			EmitSimpleExpression(writer, instructionOffset, context, this.displacement, this.displacementSize);
			EmitSimpleExpression(writer, instructionOffset, context, this.immediate, this.immediateSize);
			EmitSimpleExpression(writer, instructionOffset, context, this.extraImmediate, this.extraImmediateSize);

			return checked((int)(writer.BaseStream.Position - instructionOffset));
		}

		/// <summary>
		/// Emits the legacy prefixes.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		private void EmitLegacyPrefixes(BinaryWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			if (prefix1 != PrefixLockRepeat.None)
				writer.Write((byte)prefix1);
			if (prefix2 != PrefixSegmentBranch.None)
				writer.Write((byte)prefix2);
			if (prefix3 != PrefixAddressSizeOverride.None)
				writer.Write((byte)prefix3);
			if (prefix4 != PrefixOperandSizeOverride.None)
				writer.Write((byte)prefix4);
		}

		/// <summary>
		/// Emits the mandatory prefix.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		private void EmitMandatoryPrefix(BinaryWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			writer.Write(mandatoryPrefix);
		}

		/// <summary>
		/// Emits the REX-prefix, if used.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		private void EmitREXPrefix(BinaryWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			if (!use64BitOperands.HasValue)
				return;
			
			byte rex = 0x40;
			if (use64BitOperands.Value)
				rex |= (byte)0x08;
			if (modRM != null && sib != null)
			{
				rex |= (byte)((sib.Base & 0x08) >> 3);		// REX.B
				rex |= (byte)((sib.Index & 0x08) >> 2);		// REX.X
				rex |= (byte)((modRM.Reg & 0x08) >> 1);		// REX.R
			}
			else if (modRM != null)
			{
				rex |= (byte)((modRM.RM & 0x08) >> 3);		// REX.B
				rex |= (byte)((modRM.Reg & 0x08) >> 1);		// REX.R
			}
			else
			{
				// No ModR/M or SIB bytes, but a reg-value anyway.
				rex |= (byte)((opcodeReg & 0x08) >> 3);		// REX.B
			}

			writer.Write(rex);
		}

		/// <summary>
		/// Emits the opcode of the instruction.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		private void EmitOpcode(BinaryWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			// We OR the least siginificant 3 bits of the opcode REG with the last byte of the opcode, if necessary.
			byte[] actualOpcode;
			if (opcodeReg > 0)
			{
				actualOpcode = (byte[])opcode.Clone();
				actualOpcode[actualOpcode.Length - 1] |= (byte)(opcodeReg & 0x7);
			}
			else
				actualOpcode = opcode;

			writer.Write(actualOpcode);
		}

		/// <summary>
		/// Emits the MOD-R/M byte of the instruction, if any.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		private void EmitModRMByte(BinaryWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			if (modRM == null)
				return;
			
			byte modrmbyte = 0;
			modrmbyte |= (byte)((modRM.RM & 0x07));
			modrmbyte |= (byte)((modRM.Reg & 0x07) << 3);
			modrmbyte |= (byte)(modRM.Mod << 6);

			writer.Write(modrmbyte);
		}

		/// <summary>
		/// Emits the SIB byte of the instruction, if any.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		private void EmitSIBByte(BinaryWriter writer)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			#endregion

			if (sib == null)
				return;

			byte sibbyte = 0;
			sibbyte |= (byte)((sib.Base & 0x07));
			sibbyte |= (byte)((sib.Index & 0x07) << 3);
			sibbyte |= (byte)(sib.Scale << 6);

			writer.Write(sibbyte);
		}

		/// <summary>
		/// Emits the immediate value as part of the instruction, if any.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		/// <param name="instructionOffset">The offset of the instruction in the stream underlying
		/// <paramref name="writer"/>.</param>
		/// <param name="context">The <see cref="Context"/> of the instruction.</param>
		/// <param name="expression">The <see cref="SimpleExpression"/> to emit.</param>
		/// <param name="size">The size of the value to emit.</param>
		private void EmitSimpleExpression(BinaryWriter writer, long instructionOffset, Context context, SimpleExpression expression, DataSize size)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			#endregion

			if (expression == null)
				return;

			// Number of bytes before the expression.
			ulong relocationDiff = (ulong)(writer.BaseStream.Position - instructionOffset);
			Relocation relocation = null;

			Int128 actualValue = expression.Evaluate(context);
			if (expression.Reference != null)
			{
				relocation = new Relocation(
					expression.Reference.Symbol,
					context.Section,
					context.Address,
					actualValue,
					RelocationType.Default32);
			}

			// Emit the expression's value.
			EmitConstant(writer, size, actualValue);

			// Add the relocation to the context.
			if (relocation != null)
			{
				relocation.Offset += relocationDiff;
				context.RelocationTable.Add(relocation);
			}
		}

		/// <summary>
		/// Emits a constant value.
		/// </summary>
		/// <param name="writer">The <see cref="BinaryWriter"/> to which the encoded instruction is written.</param>
		/// <param name="size">The size of the constant to emit.</param>
		/// <param name="constant">The constant value to emit.</param>
		private void EmitConstant(BinaryWriter writer, DataSize size, Int128 constant)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(writer != null);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
			#endregion
			
			try
			{
				switch (size)
				{
					case DataSize.Bit8:
						writer.Write(checked((byte)constant));
						break;
					case DataSize.Bit16:
						writer.Write(checked((ushort)constant));
						break;
#if false
				case DataSize.Bit24:
					// Special case, which is used for a 16-bit + 8-bit data.
					value = (UInt64)ConvertChecked.ToUInt64(actualImmediateValue, out overflow);
					overflow = overflow || value > 0xFFFFFF;
					pos += buffer.Copy(pos, (UInt16)value);
					pos += buffer.Copy(pos, (Byte)(value >> 16));
					break;
#endif
					case DataSize.Bit32:
						writer.Write(checked((uint)constant));
						break;
#if false
				case DataSize.Bit48:
					// Special case for 'cp', which is used for a 32-bit + 16-bit far pointer.
					value = (UInt64)ConvertChecked.ToUInt64(actualImmediateValue, out overflow);
					overflow = overflow || value > 0xFFFFFFFFFFFF;
					pos += buffer.Copy(pos, (UInt32)value);
					pos += buffer.Copy(pos, (UInt16)(value >> 32));
					break;
#endif
					case DataSize.Bit64:
						writer.Write(checked((ulong)constant));
						break;
					case DataSize.Bit128:
						writer.Write(checked((Int128)constant));
						break;
					case DataSize.None:
					default:
						throw new InvalidOperationException();
				}
			}
			catch (OverflowException ex)
			{
				throw new AssemblerException("The value to emit does not fit in the specified width.", ex);
			}
		}
		#endregion

		/// <summary>
		/// Sets that the ModR/M byte gets encoded.
		/// </summary>
		public void SetModRMByte()
		{
			if (modRM == null)
			{
				modRM = new ModRMByte();
				modRM.Reg = fixedReg;
			}
		}

		/// <summary>
		/// Sets that the SIB byte gets encoded.
		/// </summary>
		public void SetSIBByte()
		{
			if (sib == null)
				sib = new SibByte();
		}

		/// <summary>
		/// Sets the correct lock prefix.
		/// </summary>
		/// <param name="doLock"><see langword="true"/> to add a lock prefix;
		/// otherwise, <see langword="false"/>.</param>
		public void SetLock(bool doLock)
		{
			if (doLock)
				Prefix1 = PrefixLockRepeat.Lock;
			else
				Prefix1 = PrefixLockRepeat.None;
		}

		/// <summary>
		/// Gets the address size based on the address size prefix and the current assembler mode.
		/// </summary>
		/// <param name="assemblerMode">The assembler mode.</param>
		/// <returns>The address size.</returns>
		public DataSize GetAddressSize(DataSize assemblerMode)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), assemblerMode));
			Contract.Requires<ArgumentException>(assemblerMode != DataSize.None);
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			Contract.Ensures(Contract.Result<DataSize>() != DataSize.None);
			#endregion

			if (this.Prefix3 == PrefixAddressSizeOverride.None)
				return assemblerMode;

			switch (assemblerMode)
			{
				case DataSize.Bit16:
					return DataSize.Bit32;
				case DataSize.Bit32:
					return DataSize.Bit16;
				case DataSize.Bit64:
					return DataSize.Bit32;
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Sets the address size prefix when needed.
		/// </summary>
		/// <param name="assemblerMode">The assembler mode.</param>
		/// <param name="addressSize">The address size.</param>
		/// <remarks>
		/// This method assumes that only valid combinations of address size and mode are given.
		/// </remarks>
		public void SetAddressSize(DataSize assemblerMode, DataSize addressSize)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), assemblerMode));
			Contract.Requires<ArgumentException>(assemblerMode != DataSize.None);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), addressSize));
			Contract.Requires<ArgumentException>(addressSize != DataSize.None);
			#endregion

			if (addressSize != DataSize.None && assemblerMode != addressSize)
				this.Prefix3 = PrefixAddressSizeOverride.AddressSizeOverride;
			else
				this.Prefix3 = PrefixAddressSizeOverride.None;
		}

		/// <summary>
		/// Gets the operand size based on the operand size prefix and the current assembler mode.
		/// </summary>
		/// <param name="assemblerMode">The assembler mode.</param>
		/// <returns>The operand size.</returns>
		public DataSize GetOperandSize(DataSize assemblerMode)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), assemblerMode));
			Contract.Requires<ArgumentException>(assemblerMode != DataSize.None);
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			Contract.Ensures(Contract.Result<DataSize>() != DataSize.None);
			#endregion

			switch (assemblerMode)
			{
				case DataSize.Bit16:
					if (this.Prefix4 == PrefixOperandSizeOverride.OperandSizeOverride)
						return DataSize.Bit32;
					else
						return DataSize.Bit16;
				case DataSize.Bit32:
					if (this.Prefix4 == PrefixOperandSizeOverride.OperandSizeOverride)
						return DataSize.Bit16;
					else
						return DataSize.Bit32;
				case DataSize.Bit64:
					if (this.use64BitOperands.HasValue && this.use64BitOperands.Value)
						return DataSize.Bit64;
					else if (this.Prefix4 == PrefixOperandSizeOverride.OperandSizeOverride)
						return DataSize.Bit16;
					else
						return DataSize.Bit32;
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Sets the operand size prefix when needed.
		/// </summary>
		/// <param name="assemblerMode">The assembler mode.</param>
		/// <param name="operandSize">The operand size.</param>
		public void SetOperandSize(DataSize assemblerMode, DataSize operandSize)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), assemblerMode));
			Contract.Requires<ArgumentException>(assemblerMode != DataSize.None);
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
			Contract.Requires<ArgumentException>(operandSize != DataSize.None);
			#endregion

			if (assemblerMode == DataSize.Bit16 &&
				operandSize == DataSize.Bit32)
				this.Prefix4 = PrefixOperandSizeOverride.OperandSizeOverride;
			else if ((assemblerMode == DataSize.Bit32 ||
				assemblerMode == DataSize.Bit64) &&
				operandSize == DataSize.Bit16)
				this.Prefix4 = PrefixOperandSizeOverride.OperandSizeOverride;

			if (assemblerMode == DataSize.Bit64 &&
				operandSize == DataSize.Bit64)
			{
				// Setting this to anything other than null causes a REX prefix to be encoded.
				this.use64BitOperands = true;
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
			Contract.Invariant(Enum.IsDefined(typeof(PrefixLockRepeat), this.prefix1));
			Contract.Invariant(Enum.IsDefined(typeof(PrefixSegmentBranch), this.prefix2));
			Contract.Invariant(Enum.IsDefined(typeof(PrefixAddressSizeOverride), this.prefix3));
			Contract.Invariant(Enum.IsDefined(typeof(PrefixOperandSizeOverride), this.prefix4));

			Contract.Invariant(this.mandatoryPrefix != null);
			Contract.Invariant(this.opcode != null);
			Contract.Invariant(this.opcodeReg <= 0x0F,
					"Only the first 4 bits may be set.");
			Contract.Invariant(this.fixedReg <= 0x07,
					"Only the first 3 bits may be set.");

			Contract.Invariant(Enum.IsDefined(typeof(DataSize), this.displacementSize));
			Contract.Invariant(Enum.IsDefined(typeof(DataSize), this.immediateSize));
			Contract.Invariant(Enum.IsDefined(typeof(DataSize), this.extraImmediateSize));
		}
		#endregion
	}
}
