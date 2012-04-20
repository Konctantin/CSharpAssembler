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

		///// <summary>
		///// Gets a collection of operands.
		///// </summary>
		///// <value>A collection of operands.</value>
		//public new IList<X86OperandSpec> Operands
		//{
		//    get
		//    {
		//        #region Contract
		//        Contract.Ensures(Contract.Result<IList<X86OperandSpec>>() != null);
		//        #endregion
		//        return (IList<X86OperandSpec>)base.Operands;
		//    }
		//}
	}
}
