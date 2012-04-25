﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace SharpAssembler.OpcodeWriter.X86
{
	/// <summary>
	/// 
	/// </summary>
	public class X86OpcodeVariantSpec : OpcodeVariantSpec
	{
		private byte fixedReg = 0;
		/// <summary>
		/// Gets or sets the fixed value of the REG part of the ModR/M byte.
		/// </summary>
		/// <value>The 3-bit fixed REG value. The default value is 0.</value>
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
		/// Gets or sets the operand size of the opcode variant.
		/// </summary>
		/// <value>A member of the <see cref="DataSize"/> enumeration;
		/// or <see cref="SharpAssembler.OpcodeWriter.DataSize.None"/> to specify no operand size.
		/// The default is <see cref="SharpAssembler.OpcodeWriter.DataSize.None"/>.</value>
		public DataSize OperandSize
		{
			get { return this.operandSize; }
			set { this.operandSize = value; }
		}
	}
}
