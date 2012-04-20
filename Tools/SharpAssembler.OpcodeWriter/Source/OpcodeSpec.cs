using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Describes a single opcode.
	/// </summary>
	public class OpcodeSpec
	{
		private string mnemonic;
		/// <summary>
		/// Gets or sets the mnemonic of the opcode.
		/// </summary>
		/// <value>The opcode's mnemonic; or <see langword="null"/> to specify none.
		/// The default is <see langword="null"/>.</value>
		public string Mnemonic
		{
			get { return this.mnemonic; }
			set { this.mnemonic = value; }
		}

		private string shortDescription;
		/// <summary>
		/// Gets or sets a short description of the opcode.
		/// </summary>
		/// <value>A one-sentence description of the opcode; or <see langword="null"/> to specify none.
		/// The default is <see langword="null"/>.</value>
		public string ShortDescription
		{
			get { return this.shortDescription; }
			set { this.shortDescription = value; }
		}

		///// <summary>
		///// Gets the number of operands that the opcode can accept.
		///// </summary>
		///// <value>The number of operands.</value>
		//public int OperandCount
		//{
		//    get
		//    {
		//        #region Contract
		//        Contract.Ensures(Contract.Result<int>() >= 0);
		//        #endregion
		//        return this.operands.Count;
		//    }
		//}

		//private readonly StringCollection operands = new StringCollection();
		///// <summary>
		///// Gets a collection of operands for this opcode.
		///// </summary>
		///// <value>A collection with a name for each of the operands.</value>
		///// <remarks>
		///// The number of entries in the collection determines the number of operands that the opcode can accept.
		///// </remarks>
		//public StringCollection Operands
		//{
		//    get
		//    {
		//        #region Contract
		//        Contract.Ensures(Contract.Result<StringCollection>() != null);
		//        #endregion
		//        return this.operands;
		//    }
		//}

		private readonly Collection<OpcodeVariantSpec> variants = new Collection<OpcodeVariantSpec>();
		/// <summary>
		/// Gets a collection of opcode variants.
		/// </summary>
		/// <value>A collection of opcode variants.</value>
		public Collection<OpcodeVariantSpec> Variants
		{
			get
			{
				#region Contract
				Contract.Ensures(Contract.Result<Collection<OpcodeVariantSpec>>() != null);
				#endregion
				return this.variants;
			}
		}

		#region Invariant
		/// <summary>
		/// Asserts the invariants for this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			//Contract.Invariant(this.operands != null);
			Contract.Invariant(this.variants != null);
		}
		#endregion
	}
}
