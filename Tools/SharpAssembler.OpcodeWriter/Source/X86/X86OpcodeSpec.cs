using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.OpcodeWriter.X86
{
	/// <summary>
	/// Describes a single x86-64 opcode.
	/// </summary>
	public class X86OpcodeSpec : OpcodeSpec
	{
		/// <inheritdoc />
		public override string Platform
		{
			get { return "x86"; }
		}

		private bool canLock = false;
		/// <summary>
		/// Gets or sets whether the instruction can have a lock prefix.
		/// </summary>
		/// <value><see langword="true"/> when the instruction can have a lock prefix;
		/// otherwise, <see langword="false"/>.</value>
		public bool CanLock
		{
			get { return this.canLock; }
			set { this.canLock = value; }
		}
	}
}
