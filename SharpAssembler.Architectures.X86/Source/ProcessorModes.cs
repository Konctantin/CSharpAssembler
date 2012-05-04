using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.Architectures.X86
{
	/// <summary>
	/// Specifies processor modes of operation.
	/// </summary>
	[Flags]
	public enum ProcessorModes
	{
		/// <summary>
		/// No modes specified.
		/// </summary>
		None = 0,
		/// <summary>
		/// Real mode.
		/// </summary>
		Real = 0x01,
		/// <summary>
		/// Protected mode.
		/// </summary>
		Protected = 0x02,
		/// <summary>
		/// Virtual 8086/real mode.
		/// </summary>
		Virtual8086 = 0x04,
		/// <summary>
		/// Long mode.
		/// </summary>
		Long = 0x08,
		/// <summary>
		/// System management mode.
		/// </summary>
		SystemManagement = 0x10,

		/// <summary>
		/// A combination of the real, protected and long processor modes.
		/// </summary>
		LongProtectedReal = Real | Protected | Long,
		/// <summary>
		/// A combination of the protected and long processor modes.
		/// </summary>
		LongProtected = Protected | Long,
		/// <summary>
		/// A combination of the real and protected processor modes.
		/// </summary>
		ProtectedReal = Real | Protected,
	}
}
