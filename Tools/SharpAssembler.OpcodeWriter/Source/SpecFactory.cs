using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Factory for specifications.
	/// </summary>
	public abstract class SpecFactory
	{
		/// <summary>
		/// Creates a new <see cref="OpcodeSpec"/> object.
		/// </summary>
		/// <returns>The created object.</returns>
		public virtual OpcodeSpec CreateOpcodeSpec()
		{
			#region Contract
			Contract.Ensures(Contract.Result<OpcodeSpec>() != null);
			#endregion
			return new OpcodeSpec();
		}

		/// <summary>
		/// Creates a new <see cref="OpcodeVariantSpec"/> object.
		/// </summary>
		/// <returns>The created object.</returns>
		public virtual OpcodeVariantSpec CreateOpcodeVariantSpec()
		{
			#region Contract
			Contract.Ensures(Contract.Result<OpcodeVariantSpec>() != null);
			#endregion
			return new OpcodeVariantSpec();
		}

		/// <summary>
		/// Creates a new <see cref="OperandSpec"/> object.
		/// </summary><param name="type">The operand type.</param>
		/// <param name="defaultValue">The default value; or <see langword="null"/>.</param>
		/// <returns>The created object.</returns>
		public abstract OperandSpec CreateOperandSpec(string type, object defaultValue);
	}
}
