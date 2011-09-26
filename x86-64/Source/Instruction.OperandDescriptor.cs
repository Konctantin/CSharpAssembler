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
using System.Diagnostics.Contracts;
using System.Globalization;
using SharpAssembler.Core;

namespace SharpAssembler.x86
{
	partial class Instruction
	{
		/// <summary>
		/// Descibes a single operand.
		/// </summary>
		internal struct OperandDescriptor : IEquatable<OperandDescriptor>
		{
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="OperandDescriptor"/> structure.
			/// </summary>
			/// <param name="operandType">The type of operand.</param>
			/// <param name="size">The size of the operand.</param>
			public OperandDescriptor(OperandType operandType, DataSize size)
				: this(operandType, RegisterType.None, size, OperandEncoding.Default)
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandType), operandType));
				Contract.Requires<ArgumentException>(operandType != OperandType.FixedRegister,
					"To specify a fixed register, use the appropriate constructor.");
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
				Contract.Requires<ArgumentException>(
					operandType != OperandType.RegisterOperand && operandType != OperandType.RegisterOrMemoryOperand,
					"A register type must be specified when the type of operand may take a register." +
					"Use the appropriate constructor.");
				#endregion
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="OperandDescriptor"/> structure.
			/// </summary>
			/// <param name="operandType">The type of operand.</param>
			/// <param name="registerType">The type of register.</param>
			public OperandDescriptor(OperandType operandType, RegisterType registerType)
				: this(operandType, registerType, OperandEncoding.Default)
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandType), operandType));
				Contract.Requires<ArgumentException>(operandType != OperandType.FixedRegister,
					"To specify a fixed register, use the appropriate constructor.");
				Contract.Requires<ArgumentException>(
					(operandType != OperandType.RegisterOperand && operandType != OperandType.RegisterOrMemoryOperand)
					|| registerType != RegisterType.None,
					"A register type must be specified when the type of operand may take a register.");
				#endregion
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="OperandDescriptor"/> structure.
			/// </summary>
			/// <param name="operandType">The type of operand.</param>
			/// <param name="size">The size of the operand.</param>
			/// <param name="encoding">How the operand is encoded.</param>
			public OperandDescriptor(OperandType operandType, DataSize size, OperandEncoding encoding)
				: this(operandType, RegisterType.None, size, encoding)
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandType), operandType));
				Contract.Requires<ArgumentException>(operandType != OperandType.FixedRegister,
					"To specify a fixed register, use the appropriate constructor.");
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandEncoding), encoding));
				Contract.Requires<ArgumentException>(
					operandType != OperandType.RegisterOperand && operandType != OperandType.RegisterOrMemoryOperand,
					"A register type must be specified when the type of operand may take a register." +
					"Use the appropriate constructor.");
				#endregion
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="OperandDescriptor"/> structure.
			/// </summary>
			/// <param name="operandType">The type of operand.</param>
			/// <param name="registerType">The type of register.</param>
			/// <param name="encoding">How the operand is encoded.</param>
			public OperandDescriptor(OperandType operandType, RegisterType registerType, OperandEncoding encoding)
				: this(operandType, registerType, DataSize.None, encoding)
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandType), operandType));
				Contract.Requires<ArgumentException>(operandType != OperandType.FixedRegister,
					"To specify a fixed register, use the appropriate constructor.");
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandEncoding), encoding));
				Contract.Requires<ArgumentException>(
					(operandType != OperandType.RegisterOperand && operandType != OperandType.RegisterOrMemoryOperand)
					|| registerType != RegisterType.None,
					"A register type must be specified when the type of operand may take a register.");
				#endregion
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="OperandDescriptor"/> structure.
			/// </summary>
			/// <param name="operandType">The type of operand.</param>
			/// <param name="registerType">A bitwise combination of valid types of the register.</param>
			/// <param name="size">The size of the operand.</param>
			/// <param name="encoding">Specifies how the operand is encoded.</param>
			private OperandDescriptor(OperandType operandType, RegisterType registerType, DataSize size, OperandEncoding encoding)
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandType), operandType));
				Contract.Requires<ArgumentException>(operandType != OperandType.FixedRegister,
					"To specify a fixed register, use the appropriate constructor.");
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), size));
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(OperandEncoding), encoding));
				Contract.Requires<ArgumentException>(
					(operandType != OperandType.RegisterOperand && operandType != OperandType.RegisterOrMemoryOperand)
					|| registerType != RegisterType.None,
					"A register type must be specified when the type of operand may take a register.");
				#endregion

				this.operandType = operandType;
				this.registerType = registerType;
				this.size = (size != DataSize.None ? size : registerType.GetSize());
				this.fixedRegister = Register.None;
				this.operandEncoding = encoding;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="OperandDescriptor"/> structure.
			/// </summary>
			/// <param name="register">The fixed <see cref="Register"/> value of the operand.</param>
			public OperandDescriptor(Register register)
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), register));
				#endregion

				this.operandType = OperandType.FixedRegister;
				this.registerType = register.GetRegisterType();
				this.size = register.GetSize();
				this.fixedRegister = register;
				this.operandEncoding = OperandEncoding.Default;
			}
			#endregion

			#region Properties
			private OperandType operandType;
			/// <summary>
			/// Gets the type of operand.
			/// </summary>
			/// <value>A member of the <see cref="OperandType"/> enumeration.</value>
			public OperandType OperandType
			{
				get
				{
					#region Contract
					Contract.Ensures(Enum.IsDefined(typeof(OperandType), Contract.Result<OperandType>()));
					#endregion
					return operandType;
				}
			}

			private RegisterType registerType;
			/// <summary>
			/// Gets the type of registers allowed for this operand.
			/// </summary>
			/// <value>A bitwise combination of members of the <see cref="RegisterType"/> enumeration;
			/// or <see cref="SharpAssembler.x86.RegisterType.None"/> when this does not apply.</value>
			/// <remarks>
			/// This property is only valid for operands which take a register.
			/// </remarks>
			public RegisterType RegisterType
			{
				get { return registerType; }
			}

			private OperandEncoding operandEncoding;
			/// <summary>
			/// Gets how the operand is encoded.
			/// </summary>
			/// <value>A member of the <see cref="OperandEncoding"/> enumeration.</value>
			public OperandEncoding OperandEncoding
			{
				get
				{
					#region Contract
					Contract.Ensures(Enum.IsDefined(typeof(OperandEncoding), Contract.Result<OperandEncoding>()));
					#endregion
					return operandEncoding;
				}
			}

			private DataSize size;
			/// <summary>
			/// Gets the size of the operand.
			/// </summary>
			/// <value>A member of the <see cref="DataSize"/> enumeration; or <see cref="DataSize.None"/> when the size
			/// of the operand does not matter.</value>
			/// <remarks>
			/// This property returns the size of <see cref="FixedRegister"/> when <see cref="OperandType"/> equals
			/// <see cref="SharpAssembler.x86.Instruction.OperandType.FixedRegister"/>.
			/// </remarks>
			public DataSize Size
			{
				get
				{
					#region Contract
					Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
					#endregion
					return size;
				}
			}

			private Register fixedRegister;
			/// <summary>
			/// Gets the <see cref="Register"/> which must be set when <see cref="OperandType"/> equals
			/// <see cref="SharpAssembler.x86.Instruction.OperandType.FixedRegister"/>.
			/// </summary>
			/// <value>A <see cref="Register"/>; or <see cref="Register.None"/> when it does not apply.</value>
			public Register FixedRegister
			{
				get
				{
					#region Contract
					Contract.Ensures(Enum.IsDefined(typeof(Register), Contract.Result<Register>()));
					Contract.Ensures(
						(Contract.Result<Register>() == Register.None)
						==
						(OperandType != Instruction.OperandType.FixedRegister));
					#endregion
					return fixedRegister;
				}
			}
			#endregion

			#region Methods
			/// <summary>
			/// Indicates whether the current object is equal to another object of the same type.
			/// </summary>
			/// <param name="other">An object to compare with this object.</param>
			/// <returns><see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter;
			/// otherwise, <see langword="false"/>.</returns>
			public bool Equals(OperandDescriptor other)
			{
				if (other == null)
					return false;

				return other.operandType == this.operandType
					&& other.registerType == this.registerType
					&& other.size == this.size
					&& other.fixedRegister == this.fixedRegister;
			}

			/// <summary>
			/// Indicates whether this instance and a specified object are equal.
			/// </summary>
			/// <param name="obj">Another object to compare to.</param>
			/// <returns><see langword="true"/> if <paramref name="obj"/> and this instance are the same type and
			/// represent the same value; otherwise, <see langword="false"/>.</returns>
			public override bool Equals(object obj)
			{
				if (obj == null || !(obj is OperandDescriptor))
					return false;
				return Equals((OperandDescriptor)obj);
			}

			/// <summary>
			/// Returns the hash code for this instance.
			/// </summary>
			/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
			public override int GetHashCode()
			{
				// TODO: Better GetHashCode() method implementation.
				return operandType.GetHashCode() ^ registerType.GetHashCode() ^ size.GetHashCode() ^ fixedRegister.GetHashCode();
			}

			// TODO: Implement these in the operand types.
#if false
			/// <summary>
			/// Checks whether the specified operand is a match to this <see cref="OperandDescriptor"/>.
			/// </summary>
			/// <param name="operand">The <see cref="Operand"/> to test.</param>
			/// <returns><see langword="true"/> when <paramref name="operand"/> is a match to this
			/// <see cref="OperandDescriptor"/>; otherwise, <see langword="false"/>.</returns>
			[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
			public bool Match(Operand operand)
			{
				switch (operandType)
				{
					case OperandType.FixedRegister:
						{
							RegisterOperand castoperand = operand as RegisterOperand;
							if (castoperand != null)
								return (castoperand.Register == fixedRegister);
							return false;
						}
					case OperandType.Immediate:
						{
							Immediate castoperand = operand as Immediate;
							if (castoperand != null)
								return (castoperand.Size == DataSize.None || castoperand.Size == size);
							return false;
						}
					case OperandType.RegisterOperand:
						{
							RegisterOperand castoperand = operand as RegisterOperand;
							if (castoperand != null)
								return (/*castoperand.Size == size &&*/ registerType.HasFlag(castoperand.Register.GetRegisterType()));
							return false;
						}
					case OperandType.RegisterOrMemoryOperand:
						{
							RegisterOperand castoperand1 = operand as RegisterOperand;
							EffectiveAddress castoperand2 = operand as EffectiveAddress;
							if (castoperand1 != null)
								return (/*castoperand1.Size == size &&*/ registerType.HasFlag(castoperand1.Register.GetRegisterType()));
							if (castoperand2 != null)
								return (/*castoperand2.Size == size*/ registerType.HasFlag(castoperand2.Size));
							return false;
						}
					case OperandType.MemoryOperand:
						{
							EffectiveAddress castoperand = operand as EffectiveAddress;
							if (castoperand != null)
								return (castoperand.Size == size);
							return false;
						}
					case OperandType.MemoryOffset:
						// TODO: Implement!
						return false;
					case OperandType.FarPointer:
						{
							FarPointer castoperand = operand as FarPointer;
							if (castoperand != null)
								return (castoperand.Size == size);
							return false;
						}

					case OperandType.RelativeOffset:
						{
							// A relative offset matches when the Size of the operand
							// equals DataSize.None. The Size will be set when the
							// operand has been preprocessed, after which it is tested
							// again.
							RelativeOffset castoperand = operand as RelativeOffset;
							if (castoperand != null)
								return (castoperand.Size == DataSize.None || castoperand.Size == size);
							return false;
						}
					case OperandType.None:
						return (operand == null);
					default:
						throw new NotSupportedException();
				}
			}

			/// <summary>
			/// Adjusts the operand to reflect the encoding differences
			/// between variants.
			/// </summary>
			/// <param name="operand">The operand to adjust to this descriptor.</param>
			public void Adjust(Operand operand)
			{
				switch (operandType)
				{
					case OperandType.RegisterOrMemoryOperand:
						{
							RegisterOperand castoperand = operand as RegisterOperand;
							if (castoperand != null)
								// When the operand is a register (and not a memory reference), it needs to be encoded as part of the reg/mem.
								castoperand.Encoding = RegisterOperandEncoding.ModRm;
							break;
						}
					case OperandType.RegisterOperand:
						{
							RegisterOperand castoperand = operand as RegisterOperand;
							if (castoperand != null)
								// When the operand needs to be added to the opcode, set it as such.
								castoperand.Encoding = (operandEncoding == OperandEncoding.OpcodeAdd ? RegisterOperandEncoding.AddToOpcode : RegisterOperandEncoding.Default);
							break;
						}
					case OperandType.Immediate:
						{
							Immediate castoperand = operand as Immediate;
							if (castoperand != null)
								castoperand.AsExtraImmediate = (operandEncoding == OperandEncoding.ExtraImmediate);
							break;
						}
					case OperandType.FixedRegister:
					case OperandType.MemoryOperand:
					case OperandType.MemoryOffset:
					case OperandType.FarPointer:
					case OperandType.RelativeOffset:
					case OperandType.None:
					default:
						break;
				}
			}
#endif

#if false
		/// <summary>
		/// Checks whether the specified object is valid to be assigned to
		/// the <see cref="OperandType"/>.
		/// </summary>
		/// <param name="operandType">A bitwise combination of <see cref="OperandType"/> members
		/// for which the operand must be valid.</param>
		/// <param name="registerType">A bitwise combination of <see cref="RegisterType"/> members
		/// for which the operand must be valid. The lower 8 bits are ignored.</param>
		/// <param name="operand">The <see cref="Operand"/> to check.</param>
		/// <returns><see langword="true"/> when an <paramref name="operand"/> may be assigned
		/// to an operand with the specified operand type and register type; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// When <paramref name="operand"/> is <see langword="null"/>, this method always returns <see langword="true"/>
		/// because there is no flag in <see cref="OperandType"/> to indicate that <see langword="null"/> operands are
		/// (dis)allowed.
		/// </remarks>
		public static bool IsValidArgument(OperandType operandType, RegisterType registerType, Operand operand)
		{
			// Null operands are implicitly allowed.
			if (operand == null)
				return true;

			// Ignore the lower 8 bits.
			registerType = (RegisterType)((int)registerType & ~0xFF);

			// TODO: Implement.
			if (operandType.HasFlag(OperandType.MemoryOffset))
				throw new NotImplementedException();

			// Check the type of operand.
			if (operand is Immediate)
				return operandType.HasFlag(OperandType.Immediate);
			if (operand is EffectiveAddress)
				return operandType.HasFlag(OperandType.MemoryOperand)
					|| operandType.HasFlag(OperandType.RegisterOrMemoryOperand);
			if (operand is RelativeOffset)
				return operandType.HasFlag(OperandType.RelativeOffset);
			if (operand is FarPointer)
				return operandType.HasFlag(OperandType.FarPointer);
			if (operand is RegisterOperand)
			{
				if (!operandType.HasFlag(OperandType.RegisterOperand) &&
					!operandType.HasFlag(OperandType.FixedRegister) &&
					!operandType.HasFlag(OperandType.RegisterOrMemoryOperand))
					return false;
				if (operandType.HasFlag(OperandType.FixedRegister))
					return true;

				// Check the type of register.
				return (registerType.HasFlag((RegisterType)((int)(operand as RegisterOperand).Register.GetRegisterType() & ~0xFF)));
			}

			return false;
		}
#endif

			/// <summary>
			/// Returns a string representation of this object.
			/// </summary>
			/// <returns>A string.</returns>
			public override string ToString()
			{
				switch (operandType)
				{
					case OperandType.FixedRegister:
						return Enum.GetName(typeof(Register), fixedRegister);
					case OperandType.Immediate:
						return "imm" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
					case OperandType.MemoryOperand:
						return "mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "");
					case OperandType.MemoryOffset:
						return "moffset" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
					case OperandType.FarPointer:
						return "pntr16:" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
					case OperandType.RegisterOperand:
						switch (registerType)
						{
							case RegisterType.FloatingPoint:
								return "ST(i)";
							case RegisterType.Simd64Bit:
								return "mmx";
							case RegisterType.Simd128Bit:
								return "xmm";
							case RegisterType.Segment:
								return "segReg";
							case RegisterType.Control:
								return "cReg";
							case RegisterType.Debug:
								return "dReg";
							case RegisterType.GeneralPurpose8Bit:
							case RegisterType.GeneralPurpose16Bit:
							case RegisterType.GeneralPurpose32Bit:
							case RegisterType.GeneralPurpose64Bit:
							case RegisterType.None:
							default:
								return "reg" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
						}
					case OperandType.RegisterOrMemoryOperand:
						switch (registerType)
						{
							case RegisterType.FloatingPoint:
								return "ST(i)/mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
							case RegisterType.Simd64Bit:
								return "mmx/mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
							case RegisterType.Simd128Bit:
								return "xmm/mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
							case RegisterType.Segment:
								return "segReg/mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
							case RegisterType.Control:
								return "cReg/mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
							case RegisterType.Debug:
								return "dReg/mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
							case RegisterType.GeneralPurpose8Bit:
							case RegisterType.GeneralPurpose16Bit:
							case RegisterType.GeneralPurpose32Bit:
							case RegisterType.GeneralPurpose64Bit:
							case RegisterType.None:
							default:
								return "reg/mem" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?");
						}
					case OperandType.RelativeOffset:
						return "rel" + (size != DataSize.None ? ((int)size * 8).ToString(CultureInfo.InvariantCulture) : "?") + "off";
					case OperandType.None:
						// Nothing to do.
						return string.Empty;
					default:
						return string.Empty;
				}
			}
			#endregion

			#region Operators
			/// <summary>
			/// Determines whether two specified <see cref="OperandDescriptor"/> objects have the same value.
			/// </summary>
			/// <param name="first">An <see cref="OperandDescriptor"/>.</param>
			/// <param name="second">An <see cref="OperandDescriptor"/>.</param>
			/// <returns><see langword="true"/> if the value of <paramref name="first"/> is the same as the value of
			/// <paramref name="second"/>; otherwise, <see langword="false"/>.</returns>
			public static bool operator ==(OperandDescriptor first, OperandDescriptor second)
			{
				return first.Equals(second);
			}

			/// <summary>
			/// Determines whether two specified <see cref="OperandDescriptor"/> objects have different values.
			/// </summary>
			/// <param name="first">An <see cref="OperandDescriptor"/>.</param>
			/// <param name="second">An <see cref="OperandDescriptor"/>.</param>
			/// <returns><see langword="true"/> if the value of <paramref name="first"/> is different from the value of
			/// <paramref name="second"/>; otherwise, <see langword="false"/>.</returns>
			public static bool operator !=(OperandDescriptor first, OperandDescriptor second)
			{
				return !(first == second);
			}
			#endregion
		}
	}
}
