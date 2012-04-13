#region Copyright and License
/*
 * SharpAssembler
 * Library for .NET that assembles a predetermined list of
 * instructions into machine code.
 * 
 * Copyright (C) 2011-2012 Daniël Pelsmaeker
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
using SharpAssembler;
using System.Linq.Expressions;

namespace SharpAssembler.Architectures.X86.Operands
{
	/// <summary>
	/// An effective address.
	/// </summary>
	public partial class EffectiveAddress : Operand,
		IRegisterOrMemoryOperand, ISourceOperand
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="EffectiveAddress"/> class.
		/// </summary>
		public EffectiveAddress()
			: this(DataSize.Bit32, Register.None, Register.None, 0, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="EffectiveAddress"/> class.
		/// </summary>
		/// <param name="operandSize">The <see cref="DataSize"/> of the operand.</param>
		/// <param name="addressSize">The address size.</param>
		/// <param name="displacement">The displacement expression.</param>
		public EffectiveAddress(
			DataSize operandSize,
			DataSize addressSize,
			Expression<Func<Context, ReferenceOffset>> displacement)
			: this(operandSize, Register.None, Register.None, 0, displacement)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), addressSize));
			#endregion

			this.addressSize = addressSize;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EffectiveAddress"/> class.
		/// </summary>
		/// <param name="operandSize">The <see cref="DataSize"/> of the operand.</param>
		/// <param name="baseRegister">The base <see cref="Register"/>; or <see langword="Register.None"/>.</param>
		/// <param name="indexRegister">The index <see cref="Register"/>; or <see langword="Register.None"/>.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="displacement">The displacement expression.</param>
		public EffectiveAddress(
			DataSize operandSize,
			Register baseRegister,
			Register indexRegister,
			int scale,
			Expression<Func<Context, ReferenceOffset>> displacement)
			: base(operandSize)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), operandSize));
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), baseRegister));
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), indexRegister));
			Contract.Requires<ArgumentException>(MathExt.IsPowerOfTwo(scale));
			Contract.Requires<ArgumentOutOfRangeException>(scale <= 8);
			#endregion

			this.baseRegister = baseRegister;
			this.indexRegister = indexRegister;
			this.scale = scale;
			this.displacement = displacement;
		}
		#endregion

		#region Properties
		private Register baseRegister;
		/// <summary>
		/// Gets the base register.
		/// </summary>
		/// <value>A <see cref="Register"/>.</value>
		public Register BaseRegister
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(Register), Contract.Result<Register>()));
				#endregion
				return baseRegister;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), value));
				#endregion
				baseRegister = value;
			}
		}

		private Register indexRegister;
		/// <summary>
		/// Gets the index register.
		/// </summary>
		/// <value>A <see cref="Register"/>.</value>
		public Register IndexRegister
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(Register), Contract.Result<Register>()));
				#endregion
				return indexRegister;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), value));
				#endregion
				indexRegister = value;
			}
		}

		private int scale;
		/// <summary>
		/// Gets or sets the scaling factor.
		/// </summary>
		/// <value>The scaling factor, which must be zero, or a positive power of two less than or equal to 8.</value>
		public int Scale
		{
			get
			{
				#region Contract
				Contract.Ensures(MathExt.IsPowerOfTwo(Contract.Result<int>()));
				Contract.Ensures(Contract.Result<int>() <= 8);
				#endregion
				return scale;
			}
			set
			{
				#region Contract
				Contract.Requires<ArgumentException>(MathExt.IsPowerOfTwo(value));
				Contract.Requires<ArgumentOutOfRangeException>(value <= 8);
				#endregion
				scale = value;
			}
		}

		private Expression<Func<Context, ReferenceOffset>> displacement;
		/// <summary>
		/// Gets or sets the expression specifying the displacement of the effective address.
		/// </summary>
		/// <value>The displacement <see cref="Func{Context, ReferenceOffset}"/>; or <see langword="null"/> to specify
		/// no displacement.</value>
		public Expression<Func<Context, ReferenceOffset>> Displacement
		{
			get { return displacement; }
			set { displacement = value; }
		}

		private DataSize addressSize;
		/// <summary>
		/// Gets or sets the address size of the effective address.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration;
		/// or <see cref="SharpAssembler.DataSize.None"/> to specify no particular address size.</value>
		/// <remarks>
		/// When set to a value other than <see cref="SharpAssembler.DataSize.None"/>, the specified address
		/// size is used. The registers are checked to have the same size. When set to
		/// <see cref="SharpAssembler.DataSize.None"/>, the address size is determined from the registers. When
		/// no registers are set, the address size is determined from the <see cref="Context"/>.
		/// </remarks>
		public DataSize AddressSize
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return addressSize;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				addressSize = value;
			}
		}

		private bool? relativeAddress = null;
		/// <summary>
		/// Gets or sets whether the effective address is RIP-relative.
		/// </summary>
		/// <value><see langword="true"/> to encode a RIP-relative address; <see langword="false"/> to encode an
		/// absolute address; otherwise, <see langword="null"/> to use the <see cref="X86Architecture"/>'s default.
		/// The default is <see langword="null"/>.</value>
		/// <remarks>
		/// <para>The value of this property may only be <see langword="true"/> in 64-bit addressing mode.</para>
		/// <para>When this property's value is <see langword="null"/> and the default is to use RIP-relative
		/// addressing, it is not guaranteed that the effective address is encoded using RIP-relative addressing.
		/// There are some effective addresses for which RIP-relative addressing is not possible.</para>
		/// </remarks>
		public bool? RelativeAddress
		{
			get { return relativeAddress; }
			set { value = relativeAddress; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructs the operand's representation.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instr">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		internal override void Construct(Context context, EncodedInstruction instr)
		{
			// CONTRACT: Operand

			DataSize addressSize = GetAddressSize(context);

			instr.SetOperandSize(context.Representation.Architecture.OperandSize, Size);

			if (context.Representation.Architecture.OperandSize != DataSize.Bit64 &&
				this.Size == DataSize.Bit64)
				throw new AssemblerException("A 64-bit operand cannot be used with non-64-bit operand sizes.");
			if (context.Representation.Architecture.AddressSize != DataSize.Bit64 &&
				addressSize == DataSize.Bit64)
				throw new AssemblerException("A 64-bit effective address cannot be used with non-64-bit address sizes.");

			EncodeDisplacement(context, instr, addressSize);

			switch(addressSize)
			{
				case DataSize.Bit16:
					Encode16BitEffectiveAddress(instr);
					break;
				case DataSize.Bit32:
					Encode32BitEffectiveAddress(instr);
					break;
				case DataSize.Bit64:
					Encode64BitEffectiveAddress(context, instr);
					break;
				default:
					throw new NotSupportedException();
			}

			// Address size prefix.
			// When the registers have a width different from the current
			// operating mode width, then we have to add an address size prefix.
			// At this point, we know that the widths are valid.
			instr.SetAddressSize(context.Representation.Architecture.AddressSize, addressSize);
		}

		/// <summary>
		/// Determines the address size used by this instruction.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the register width is determined.</param>
		/// <returns>The width of the registers as a <see cref="DataSize"/>.</returns>
		private DataSize GetAddressSize(Context context)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
			Contract.Ensures(Contract.Result<DataSize>() != DataSize.None);
			#endregion

			DataSize addressSize = this.addressSize;
			DataSize baseWidth = baseRegister.GetSize();
			DataSize indexWidth = indexRegister.GetSize();
			DataSize contextAddressSize = context.Representation.Architecture.AddressSize;

			if (addressSize == DataSize.None)
				addressSize = baseWidth;
			if (addressSize == DataSize.None)
				addressSize = indexWidth;
			if (addressSize == DataSize.None)
				addressSize = contextAddressSize;

			
			if (baseWidth != DataSize.None && baseWidth != addressSize)
				throw new AssemblerException(String.Format(CultureInfo.InvariantCulture,
					"BASE register {0} has a different width than the determined address size.",
					Enum.GetName(typeof(Register), baseRegister)));
			
			if (baseWidth != DataSize.None && indexWidth != DataSize.None && baseWidth != indexWidth)
				throw new AssemblerException(String.Format(CultureInfo.InvariantCulture,
					"BASE register {0} and INDEX register {1} have different widths.",
					Enum.GetName(typeof(Register), baseRegister),
					Enum.GetName(typeof(Register), indexRegister)));

			if (!IsValidRegisterWidthForMode(baseWidth, contextAddressSize))
				throw new AssemblerException(String.Format(CultureInfo.InvariantCulture,
					"The {0} BASE register is not valid for the {1}-bit address size.",
					Enum.GetName(typeof(Register), baseRegister),
					((int)contextAddressSize) << 3));

			if (!IsValidRegisterWidthForMode(indexWidth, contextAddressSize))
				throw new AssemblerException(String.Format(CultureInfo.InvariantCulture,
					"The {0} INDEX register is not valid for the {1}-bit address size.",
					Enum.GetName(typeof(Register), indexRegister),
					((int)contextAddressSize) << 3));

			if (indexWidth != DataSize.None && indexWidth != addressSize)
				throw new AssemblerException(String.Format(CultureInfo.InvariantCulture,
					"INDEX register {0} has a different width than the determined address size.",
					Enum.GetName(typeof(Register), indexRegister)));

			return addressSize;
		}

		/// <summary>
		/// Encodes the displacement in the <see cref="EncodedInstruction"/>.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instr">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		/// <param name="addressSize">The address size used by the effective address.</param>
		private void EncodeDisplacement(Context context, EncodedInstruction instr, DataSize addressSize)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentNullException>(instr != null);
			#endregion

			DataSize displacementSize = DataSize.None;
			ReferenceOffset displacementExpression = null;

			if (displacement != null)
			{
				// Let's evaluate the displacement expression.
				displacementExpression = displacement.Compile()(context);

				// Determine the size of the displacement.
				displacementSize = addressSize;
				if (displacementSize == DataSize.None)
				{
					// Does the result have a (resolved or not resolved) reference?
					if (displacementExpression.Reference != null && !displacementExpression.Reference.Resolved)
						// When the result has a reference, use the architecture's address size.
						displacementSize = context.Representation.Architecture.AddressSize;
					else
						// Otherwise, use the most efficient word size.
						displacementSize = MathExt.GetSizeOfValue(displacementExpression.Evaluate(context));	//.Constant);
				}
			}

			instr.DisplacementSize = displacementSize;
			instr.Displacement = displacementExpression;
		}

		/// <summary>
		/// Returns whether the effective address register's width is valid for the given mode.
		/// </summary>
		/// <param name="registerWidth">The register's <see cref="DataSize"/>.</param>
		/// <param name="mode">The <see cref="DataSize"/> of the mode.</param>
		/// <returns><see langword="true"/> when the register may be used in an effective address in the specified
		/// mode; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// When <paramref name="registerWidth"/> equals <see cref="DataSize.None"/>, this method returns
		/// <see langword="true"/>. A register with no size is valid for any mode.
		/// </remarks>
		private static bool IsValidRegisterWidthForMode(DataSize registerWidth, DataSize mode)
		{
			#region Contract
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), registerWidth));
			Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), mode));
			#endregion

			switch (mode)
			{
				case DataSize.Bit16:
				case DataSize.Bit32:
					return registerWidth == DataSize.Bit16
						|| registerWidth == DataSize.Bit32
						|| registerWidth == DataSize.None;
				case DataSize.Bit64:
					return registerWidth == DataSize.Bit32
						|| registerWidth == DataSize.Bit64
						|| registerWidth == DataSize.None;
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="OperandDescriptor"/> matches this
		/// <see cref="Operand"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="OperandDescriptor"/> to match.</param>
		/// <returns><see langword="true"/> when the specified descriptor matches this operand;
		/// otherwise, <see langword="false"/>.</returns>
		internal override bool IsMatch(OperandDescriptor descriptor)
		{
			switch (descriptor.OperandType)
			{
				case OperandType.RegisterOrMemoryOperand:
					return this.Size == descriptor.RegisterType.GetSize();
				case OperandType.MemoryOperand:
					return this.Size == descriptor.Size;
				default:
					return false;
			}
		}

		/// <summary>
		/// Adjusts this <see cref="Operand"/> based on the specified
		/// <see cref="OperandDescriptor"/>.
		/// </summary>
		/// <param name="descriptor">The <see cref="OperandDescriptor"/> used to
		/// adjust.</param>
		/// <remarks>
		/// Only <see cref="OperandDescriptor"/> instances for which <see cref="IsMatch"/>
		/// returns <see langword="true"/> may be used as a parameter to this method.
		/// </remarks>
		internal override void Adjust(OperandDescriptor descriptor)
		{
			// Nothing to do.
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return base.ToString();
#if false
			StringBuilder sb = new StringBuilder(30);
			// The size of the operand.
			switch (RequestedSize)
			{
				case DataSize.Bit8:
					sb.Append("byte ");
					break;
				case DataSize.Bit16:
					sb.Append("word ");
					break;
				case DataSize.Bit32:
					sb.Append("dword ");
					break;
				case DataSize.Bit64:
					sb.Append("qword ");
					break;
				case DataSize.Bit128:
					sb.Append("oword ");
					break;
				case DataSize.None:
				default:
					break;
			}

			sb.Append("[");
			// The width of the displacement value.
			switch (displacementSize)
			{
				case DataSize.Bit8:
					sb.Append("byte ");
					break;
				case DataSize.Bit16:
					sb.Append("word ");
					break;
				case DataSize.Bit32:
					sb.Append("dword ");
					break;
				case DataSize.Bit64:
					sb.Append("qword ");
					break;
				case DataSize.Bit128:
					sb.Append("oword ");
					break;
				case DataSize.None:
				default:
					break;
			}
			if (baseRegister != X86Register.None)
				sb.Append(baseRegister.GetName());
			if (indexRegister != X86Register.None && scale > 0)
			{
				if (baseRegister != X86Register.None)
					sb.Append("+");
				sb.Append(indexRegister.GetName());
				if (scale > 1)
				{
					sb.Append("*");
					sb.Append(scale);
				}
			}
			if (displacement != null)
			{
				if (baseRegister != X86Register.None ||
					(indexRegister != X86Register.None && scale > 0))
					sb.Append("+");

				sb.Append(displacement.ToString());
			}
			sb.Append("]");
			return sb.ToString();
#endif
		}
		#endregion

		#region Invariant
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(Register), this.baseRegister));
			Contract.Invariant(Enum.IsDefined(typeof(Register), this.indexRegister));

			Contract.Invariant(MathExt.IsPowerOfTwo(this.scale));
			Contract.Invariant(this.scale <= 8);
		}
		#endregion
	}
}
