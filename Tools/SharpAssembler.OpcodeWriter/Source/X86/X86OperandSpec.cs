using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;

namespace SharpAssembler.OpcodeWriter.X86
{
	/// <summary>
	/// Describes an x86-64 opcode variant.
	/// </summary>
	public class X86OperandSpec : OperandSpec
	{
		private DataSize size = DataSize.None;
		/// <summary>
		/// Gets or sets the size of the operand.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration.
		/// The default is <see cref="SharpAssembler.OpcodeWriter.DataSize.None"/>.</value>
		public DataSize Size
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(DataSize), Contract.Result<DataSize>()));
				#endregion
				return this.size;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(DataSize), value));
				#endregion
				this.size = value;
			}
		}

		private Register fixedRegister = Register.None;
		/// <summary>
		/// Gets or sets the fixed register that is used for this operand.
		/// </summary>
		/// <value>A member of the <see cref="Register"/> enumeration;
		/// or <see cref="SharpAssembler.OpcodeWriter.X86.Register.None"/> to specify none.
		/// The default is <see cref="SharpAssembler.OpcodeWriter.X86.Register.None"/>.</value>
		public Register FixedRegister
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(Register), Contract.Result<Register>()));
				#endregion
				return this.fixedRegister;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(Register), value));
				#endregion
				this.fixedRegister = value;
			}
		}

		private X86OperandType type = X86OperandType.None;
		/// <summary>
		/// Gets or sets the type of operand.
		/// </summary>
		/// <value>A member of the <see cref="X86OperandType"/> enumeration;
		/// or <see cref="SharpAssembler.OpcodeWriter.X86.X86OperandType.None"/> to specify none.
		/// The default is <see cref="SharpAssembler.OpcodeWriter.X86.X86OperandType.None"/>.</value>
		public X86OperandType Type
		{
			get
			{
				#region Contract
				Contract.Ensures(Enum.IsDefined(typeof(X86OperandType), Contract.Result<X86OperandType>()));
				#endregion
				return this.type;
			}
			set
			{
				#region Contract
				Contract.Requires<InvalidEnumArgumentException>(Enum.IsDefined(typeof(X86OperandType), value));
				#endregion
				this.type = value;
			}
		}

		#region Invariant
		/// <summary>
		/// Asserts the invariants for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(Enum.IsDefined(typeof(DataSize), this.size));
			Contract.Invariant(Enum.IsDefined(typeof(Register), this.fixedRegister));
		}
		#endregion
	}
}
