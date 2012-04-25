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
		/// <summary>
		/// Gets the platform for which this <see cref="OpcodeSpec"/> is written.
		/// </summary>
		/// <value>A platform identifier; or <see langword="null"/> when not specified.</value>
		public virtual string Platform
		{
			get { return null; }
		}

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

		private string name;
		/// <summary>
		/// Gets or sets the name of the opcode as used in classes and identifiers in the code.
		/// </summary>
		/// <value>The name to use; or <see langword="null"/> to use the mnemonic.
		/// The default is <see langword="null"/>.</value>
		public string Name
		{
			get
			{
				if (this.name != null)
					return this.name;
				else
					return Char.ToUpperInvariant(this.mnemonic[0]).ToString() + this.mnemonic.Substring(1).ToLowerInvariant();
			}
			set { this.name = value; }
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
